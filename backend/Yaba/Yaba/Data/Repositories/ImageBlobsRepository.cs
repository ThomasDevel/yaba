using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.IO;
using Yaba.Domain.Models;

namespace Yaba.Data.Repositories
{
    public class ImageBlobsRepository : IContentRepository
    {
        private readonly SQLiteConnection _connection;
        private readonly string _contentRoot;
        private const string ThumbnailSuffix = "_thumb";

        /// <param name="connection">Open SQLite connection.</param>
        /// <param name="contentRoot">
        /// Root directory for image storage (e.g. "C:\yaba\content").
        /// Sub-folders are created per spiritType/spiritId.
        /// </param>
        public ImageBlobsRepository(SQLiteConnection connection, string contentRoot)
        {
            _connection = connection;
            _contentRoot = contentRoot;
        }

        public SpiritImage SaveImage(BeverageType spiritType, string spiritId, string userId, byte[] imageData, string extension)
        {
            var id = Guid.NewGuid().ToString("N");
            var fileName = $"{id}{extension}";
            var directory = GetDirectory(spiritType, spiritId);

            Directory.CreateDirectory(directory);

            // Write original
            var originalPath = Path.Combine(directory, fileName);
            File.WriteAllBytes(originalPath, imageData);

            // Write thumbnail placeholder (same bytes for now — swap in a
            // resizing library like SkiaSharp or ImageSharp when ready)
            var thumbFileName = $"{id}{ThumbnailSuffix}{extension}";
            var thumbPath = Path.Combine(directory, thumbFileName);
            File.WriteAllBytes(thumbPath, imageData);

            // Determine next sort order
            var sortOrder = GetNextSortOrder(spiritType, spiritId);

            var image = new SpiritImage
            {
                Id = id,
                SpiritType = spiritType,
                SpiritId = spiritId,
                UserId = userId,
                FileName = fileName,
                SortOrder = sortOrder,
                Created = DateTimeOffset.UtcNow
            };

            InsertMetadata(image);

            return image;
        }

        public SpiritImage[] GetImages(BeverageType spiritType, string spiritId)
        {
            var command = _connection.CreateCommand();
            command.CommandText = "SELECT id, spirit_type, spirit_id, user_id, file_name, sort_order, created FROM images " +
                                  "WHERE spirit_type = @spiritType AND spirit_id = @spiritId ORDER BY sort_order";
            command.Parameters.AddWithValue("@spiritType", spiritType.ToString());
            command.Parameters.AddWithValue("@spiritId", spiritId);

            return ReadImages(command);
        }

        public bool DeleteImage(string imageId)
        {
            // Look up metadata first so we can delete files
            var command = _connection.CreateCommand();
            command.CommandText = "SELECT id, spirit_type, spirit_id, user_id, file_name, sort_order, created FROM images WHERE id = @id";
            command.Parameters.AddWithValue("@id", imageId);

            var images = ReadImages(command);
            if (images.Length == 0) return false;

            var image = images[0];
            DeleteFiles(image);

            var deleteCommand = _connection.CreateCommand();
            deleteCommand.CommandText = "DELETE FROM images WHERE id = @id";
            deleteCommand.Parameters.AddWithValue("@id", imageId);

            return deleteCommand.ExecuteNonQuery() > 0;
        }

        public bool DeleteAllImages(BeverageType spiritType, string spiritId)
        {
            // Delete the entire directory for this spirit
            var directory = GetDirectory(spiritType, spiritId);
            if (Directory.Exists(directory))
            {
                Directory.Delete(directory, recursive: true);
            }

            var command = _connection.CreateCommand();
            command.CommandText = "DELETE FROM images WHERE spirit_type = @spiritType AND spirit_id = @spiritId";
            command.Parameters.AddWithValue("@spiritType", spiritType.ToString());
            command.Parameters.AddWithValue("@spiritId", spiritId);

            return command.ExecuteNonQuery() > 0;
        }

        public string GetImagePath(BeverageType spiritType, string spiritId, string fileName)
        {
            return Path.Combine(GetDirectory(spiritType, spiritId), fileName);
        }

        public string GetThumbnailPath(BeverageType spiritType, string spiritId, string fileName)
        {
            var ext = Path.GetExtension(fileName);
            var nameWithoutExt = Path.GetFileNameWithoutExtension(fileName);
            var thumbFileName = $"{nameWithoutExt}{ThumbnailSuffix}{ext}";

            return Path.Combine(GetDirectory(spiritType, spiritId), thumbFileName);
        }

        private string GetDirectory(BeverageType spiritType, string spiritId)
        {
            return Path.Combine(_contentRoot, spiritType.ToString().ToLowerInvariant(), spiritId);
        }

        private int GetNextSortOrder(BeverageType spiritType, string spiritId)
        {
            var command = _connection.CreateCommand();
            command.CommandText = "SELECT COALESCE(MAX(sort_order), -1) + 1 FROM images WHERE spirit_type = @spiritType AND spirit_id = @spiritId";
            command.Parameters.AddWithValue("@spiritType", spiritType.ToString());
            command.Parameters.AddWithValue("@spiritId", spiritId);

            return Convert.ToInt32(command.ExecuteScalar());
        }

        private void InsertMetadata(SpiritImage image)
        {
            var command = new SQLiteCommand(
                "INSERT INTO images(id, spirit_type, spirit_id, user_id, file_name, sort_order, created) " +
                "VALUES(@id, @spiritType, @spiritId, @userId, @fileName, @sortOrder, @created)", _connection);

            command.Parameters.AddWithValue("@id", image.Id);
            command.Parameters.AddWithValue("@spiritType", image.SpiritType.ToString());
            command.Parameters.AddWithValue("@spiritId", image.SpiritId);
            command.Parameters.AddWithValue("@userId", image.UserId);
            command.Parameters.AddWithValue("@fileName", image.FileName);
            command.Parameters.AddWithValue("@sortOrder", image.SortOrder);
            command.Parameters.AddWithValue("@created", image.Created.ToString("o"));

            command.Prepare();
            command.ExecuteNonQuery();
        }

        private void DeleteFiles(SpiritImage image)
        {
            var originalPath = GetImagePath(image.SpiritType, image.SpiritId, image.FileName);
            if (File.Exists(originalPath)) File.Delete(originalPath);

            var thumbPath = GetThumbnailPath(image.SpiritType, image.SpiritId, image.FileName);
            if (File.Exists(thumbPath)) File.Delete(thumbPath);
        }

        private SpiritImage[] ReadImages(SQLiteCommand command)
        {
            var items = new List<SpiritImage>();
            var reader = command.ExecuteReader();
            while (reader.Read())
            {
                items.Add(new SpiritImage
                {
                    Id = reader.GetString(0),
                    SpiritType = Enum.Parse<BeverageType>(reader.GetString(1), true),
                    SpiritId = reader.GetString(2),
                    UserId = reader.IsDBNull(3) ? null : reader.GetString(3),
                    FileName = reader.GetString(4),
                    SortOrder = reader.GetInt32(5),
                    Created = DateTimeOffset.Parse(reader.GetString(6))
                });
            }

            return items.ToArray();
        }
    }
}
