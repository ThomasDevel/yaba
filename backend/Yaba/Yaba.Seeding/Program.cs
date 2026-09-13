using System;
using System.Data.SQLite;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;
using Yaba.Data;
using Yaba.Data.Repositories;
using Yaba.Data.Repositories.Sqlite;
using Yaba.Domain.Models;
using Yaba.Seeding;

namespace Yaba.SeedingTool
{
    class Program
    {
        static async Task Main(string[] args)
        {
            var dataDirectory = YabaPaths.ResolveDataDirectory(args.Length > 0 ? args[0] : null);
            Directory.CreateDirectory(dataDirectory);
            Directory.CreateDirectory(YabaPaths.GetContentRoot(dataDirectory));

            var databasePath = YabaPaths.GetDatabasePath(dataDirectory);
            Console.WriteLine($"Database: {databasePath}");
            Console.WriteLine($"Content:  {YabaPaths.GetContentRoot(dataDirectory)}");

            using var connection = new SQLiteConnection($"Data Source={databasePath};");
            connection.Open();

            DatabaseSchema.EnsureCreated(connection);

            var whiskyRepository = new WhiskyRepository(connection);
            var contentRepository = new ImageBlobsRepository(connection, YabaPaths.GetContentRoot(dataDirectory));

            using var httpClient = new HttpClient
            {
                Timeout = TimeSpan.FromMinutes(2)
            };
            httpClient.DefaultRequestHeaders.UserAgent.ParseAdd("YABA-Seeder/1.0");

            await Kilkerran12Seed.SeedAsync(whiskyRepository, contentRepository, httpClient);

            var whisky = whiskyRepository.FindEntryById(Kilkerran12Seed.WhiskyBaseId);
            var images = contentRepository.GetImages(BeverageType.Whisky, Kilkerran12Seed.WhiskyBaseId);

            Console.WriteLine($"Seeded {whisky.Name} ({whisky.Strength}% ABV, {whisky.Age}yo)");
            Console.WriteLine($"Stored {images.Length} images for Whiskybase WID {Kilkerran12Seed.WhiskyBaseId}");
        }
    }
}
