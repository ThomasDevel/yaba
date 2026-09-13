using System;
using System.Data.SQLite;
using System.IO;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using StarFederation.Datastar.DependencyInjection;
using Yaba.Data;
using Yaba.Data.Repositories;
using Yaba.Data.Repositories.Sqlite;
using Yaba.Domain.Models;
using Yaba.Seeding;
using Yaba.Web;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDatastar();

var configuredDataDirectory = builder.Configuration["Yaba:DataDirectory"];
var dataDirectory = string.IsNullOrWhiteSpace(configuredDataDirectory)
    ? YabaPaths.ResolveDataDirectory()
    : Path.GetFullPath(Path.Combine(builder.Environment.ContentRootPath, configuredDataDirectory));
var databasePath = YabaPaths.GetDatabasePath(dataDirectory);
var contentRoot = YabaPaths.GetContentRoot(dataDirectory);

Directory.CreateDirectory(dataDirectory);
Directory.CreateDirectory(contentRoot);

var sqliteConnection = new SQLiteConnection($"Data Source={databasePath};");
sqliteConnection.Open();
DatabaseSchema.EnsureCreated(sqliteConnection);

builder.Services.AddSingleton(sqliteConnection);
builder.Services.AddSingleton<IWhiskyRepository>(_ => new WhiskyRepository(sqliteConnection));
builder.Services.AddSingleton<IContentRepository>(_ => new ImageBlobsRepository(sqliteConnection, contentRoot));

var app = builder.Build();

await EnsureSeedDataAsync(app.Services);

app.UseDefaultFiles();
app.UseStaticFiles();

app.MapGet("/content/{**path}", (string path) =>
{
    var fullPath = Path.GetFullPath(Path.Combine(contentRoot, path.Replace('/', Path.DirectorySeparatorChar)));
    if (!fullPath.StartsWith(contentRoot, StringComparison.OrdinalIgnoreCase) || !File.Exists(fullPath))
    {
        return Results.NotFound();
    }

    return Results.File(fullPath);
});

app.MapGet("/bottles", async (IDatastarService datastar, IWhiskyRepository whiskies) =>
{
    await datastar.PatchElementsAsync(RenderCollection(whiskies.ListEntries()));
});

app.MapGet("/whisky/192360/detail", async (
    IDatastarService datastar,
    IWhiskyRepository whiskies,
    IContentRepository content) =>
{
    var whisky = whiskies.FindEntryById(Kilkerran12Seed.WhiskyBaseId);
    if (whisky == null)
    {
        await datastar.PatchElementsAsync("""<div id="whisky-detail"><p class="muted">Whisky not found. Run the seeder first.</p></div>""");
        return;
    }

    var images = content.GetImages(BeverageType.Whisky, whisky.Id);
    var html = WhiskyDetailRenderer.RenderDetail(whisky, images, BuildImageUrl);
    await datastar.PatchElementsAsync(html);
});

app.Run();

static async Task EnsureSeedDataAsync(IServiceProvider services)
{
    using var scope = services.CreateScope();
    var whiskies = scope.ServiceProvider.GetRequiredService<IWhiskyRepository>();
    if (whiskies.FindEntryById(Kilkerran12Seed.WhiskyBaseId) != null)
    {
        return;
    }

    var content = scope.ServiceProvider.GetRequiredService<IContentRepository>();
    using var httpClient = new HttpClient { Timeout = TimeSpan.FromMinutes(2) };
    httpClient.DefaultRequestHeaders.UserAgent.ParseAdd("YABA/1.0");

    try
    {
        await Kilkerran12Seed.SeedAsync(whiskies, content, httpClient);
    }
    catch (Exception ex)
    {
        Console.WriteLine($"Auto-seed failed: {ex.Message}. Run Yaba.Seeding manually.");
    }
}

static string RenderCollection(Whisky[] whiskies)
{
    if (whiskies.Length == 0)
    {
        return """<div id="bottle-list"><p class="muted">Your collection is empty.</p></div>""";
    }

    var html = new StringBuilder();
    html.Append("""<div id="bottle-list"><table class="bottle-table"><thead><tr><th>Name</th><th>Distillery</th><th>Age</th><th>ABV</th></tr></thead><tbody>""");

    foreach (var whisky in whiskies)
    {
        html.Append(WhiskyDetailRenderer.RenderCollectionRow(whisky, $"/whisky/{whisky.Id}.html"));
    }

    html.Append("</tbody></table></div>");
    return html.ToString();
}

static string BuildImageUrl(SpiritImage image, bool thumbnail)
{
    var fileName = image.FileName;
    if (thumbnail)
    {
        var extension = Path.GetExtension(fileName);
        var nameWithoutExtension = Path.GetFileNameWithoutExtension(fileName);
        fileName = $"{nameWithoutExtension}_thumb{extension}";
    }

    return $"/content/{image.SpiritType.ToString().ToLowerInvariant()}/{image.SpiritId}/{fileName}";
}
