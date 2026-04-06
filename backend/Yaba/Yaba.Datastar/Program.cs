using Microsoft.AspNetCore.Builder;
using StarFederation.Datastar.DependencyInjection;
using System;
using System.Text.Json;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);

// Add Datastar as an ASP Service
//  allows injection of IDatastarService, to respond to a request with a Datastar friendly ServerSentEvent
//  and to read the signals sent by the client
builder.Services
    .AddDatastar()
    .AddJsonOptions(options =>
    {
        options.Converters.Add(new JsonStringEnumConverter());
    });

var app = builder.Build();

app.UseStaticFiles();

// displayDate - patching an element
app.MapGet("/displayDate", async (IDatastarService datastarService) =>
{
    string today = DateTime.Now.ToString("yy-MM-dd HH:mm:ss");
    await datastarService.PatchElementsAsync($"""<div id='target'><span id='date'><b>{today}</b><button data-on-click="@get('/removeDate')">Remove</button></span></div>""");
});

// removeDate - removing an element
app.MapGet("/removeDate", async (IDatastarService datastarService) =>
{
    await datastarService.RemoveElementAsync("#date");
});

// changeOutput - reads the signals, update the Output, and merge back
app.MapPost("/changeOutput", async (IDatastarService datastarService) =>
{
    MySignals signals = await datastarService.ReadSignalsAsync<MySignals>();
    MySignals newSignals = new() { Output = $"Your Input: {signals.Input}" };
    await datastarService.PatchSignalsAsync(newSignals.Serialize());
});

app.Run();

// Type declarations must come after all top-level statements
public record MySignals
{
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string? Input { get; init; } = null;

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string? Output { get; init; } = null;

    public string Serialize() => JsonSerializer.Serialize(this);
}
