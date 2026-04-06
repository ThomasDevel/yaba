using Yaba.Domain.Models;

namespace Yaba.Data.Repositories
{
    /// <summary>
    /// Handles file storage (images/thumbnails) on the file system.
    /// Paths are derived from spiritType/spiritId/fileName so when the
    /// database is moved, it can still work with a different content root.
    /// </summary>
    public interface IContentRepository
    {
        /// <summary>
        /// Saves an image and its thumbnail to disk and records metadata in SQLite.
        /// Returns the created <see cref="SpiritImage"/> with the generated Id and FileName.
        /// </summary>
        SpiritImage SaveImage(BeverageType spiritType, string spiritId, string userId, byte[] imageData, string extension);

        /// <summary>
        /// Gets all image metadata for a spirit, ordered by SortOrder.
        /// </summary>
        SpiritImage[] GetImages(BeverageType spiritType, string spiritId);

        /// <summary>
        /// Deletes a single image (original + thumbnail) from disk and SQLite.
        /// </summary>
        bool DeleteImage(string imageId);

        /// <summary>
        /// Deletes all images for a spirit (used when removing a spirit from the catalog).
        /// </summary>
        bool DeleteAllImages(BeverageType spiritType, string spiritId);

        /// <summary>
        /// Returns the full file system path for an original image.
        /// </summary>
        string GetImagePath(BeverageType spiritType, string spiritId, string fileName);

        /// <summary>
        /// Returns the full file system path for a thumbnail.
        /// </summary>
        string GetThumbnailPath(BeverageType spiritType, string spiritId, string fileName);
    }
}
