using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Yaba.Data.Repositories;
using Yaba.Data.Repositories.Sqlite;
using Yaba.Domain.Models;

namespace Yaba.Seeding
{
    public static class Kilkerran12Seed
    {
        public const string WhiskyBaseId = "192360";

        public static Whisky CreateWhisky() => new()
        {
            Id = WhiskyBaseId,
            Name = "Kilkerran 12-year-old",
            Category = WhiskyCategory.SingleMalt,
            Distillery = "Mitchell's Glengyle",
            Bottled = 2016,
            Age = 12,
            CaskType = "70% Bourbon / 30% Sherry",
            BottlingSeries = "Core Range",
            Strength = 46.0f,
            Size = 70,
            NaturalColor = true,
            NonChillFiltered = true,
            Created = new DateTimeOffset(2016, 8, 1, 0, 0, 0, TimeSpan.Zero),
        };

        /// <summary>
        /// Public bottle and distillery imagery aligned with Whiskybase WID 192360.
        /// Whiskybase blocks automated downloads, so official Mitchell's Glengyle media is used.
        /// </summary>
        public static IReadOnlyList<(string Url, string Caption)> Images { get; } =
            new List<(string, string)>
            {
                ("https://kilkerran.scot/wp-content/uploads/2024/04/12yo.png", "Bottle"),
                ("https://kilkerran.scot/wp-content/uploads/2024/04/@frombarreltobottle-9-600x1000.jpg", "Bottle and glass"),
                ("https://kilkerran.scot/wp-content/uploads/2024/04/GLENGYLE-SIGN-3-600x1000.jpg", "Glengyle distillery sign"),
                ("https://kilkerran.scot/wp-content/uploads/2024/04/DSC02290-600x1000.jpg", "Distillery"),
                ("https://kilkerran.scot/wp-content/uploads/2024/04/Kilkerran-Warehouse-Landscape-600x1000.jpeg", "Warehouse")
            };

        public static async Task SeedAsync(
            IWhiskyRepository whiskyRepository,
            IContentRepository contentRepository,
            HttpClient httpClient,
            string seedUserId = "system")
        {
            var existing = whiskyRepository.FindEntryById(WhiskyBaseId);
            if (existing == null)
            {
                whiskyRepository.CreateEntry(CreateWhisky());
            }
            else
            {
                whiskyRepository.UpdateEntryById(WhiskyBaseId, CreateWhisky());
            }

            contentRepository.DeleteAllImages(BeverageType.Whisky, WhiskyBaseId);

            foreach (var (url, _) in Images)
            {
                var bytes = await httpClient.GetByteArrayAsync(url);
                var extension = GetExtension(url);
                contentRepository.SaveImage(BeverageType.Whisky, WhiskyBaseId, seedUserId, bytes, extension);
            }
        }

        private static string GetExtension(string url)
        {
            var path = new Uri(url).AbsolutePath;
            var extension = System.IO.Path.GetExtension(path);
            return string.IsNullOrWhiteSpace(extension) ? ".jpg" : extension;
        }
    }
}
