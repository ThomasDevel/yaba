using System;
using System.IO;

namespace Yaba.Data
{
    public static class YabaPaths
    {
        public static string ResolveDataDirectory(string? configuredPath = null)
        {
            if (!string.IsNullOrWhiteSpace(configuredPath))
            {
                return Path.GetFullPath(configuredPath);
            }

            var fromEnvironment = Environment.GetEnvironmentVariable("YABA_DATA_DIR");
            if (!string.IsNullOrWhiteSpace(fromEnvironment))
            {
                return Path.GetFullPath(fromEnvironment);
            }

            return Path.GetFullPath(Path.Combine(AppContext.BaseDirectory, "..", "..", "..", "..", "data"));
        }

        public static string GetDatabasePath(string? configuredPath = null) =>
            Path.Combine(ResolveDataDirectory(configuredPath), "yaba.db");

        public static string GetContentRoot(string? configuredPath = null) =>
            Path.Combine(ResolveDataDirectory(configuredPath), "content");
    }
}
