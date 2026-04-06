using System;

namespace Yaba.Domain.Models
{
    public class SpiritImage
    {
        public string Id { get; set; }

        public BeverageType SpiritType { get; set; }

        public string SpiritId { get; set; }

        public string UserId { get; set; }

        /// <summary>
        /// File name on disk (e.g. "{id}.jpg"). Full path is derived at runtime
        /// from spiritType/spiritId/fileName so the database stays portable.
        /// </summary>
        public string FileName { get; set; }

        /// <summary>
        /// Controls display ordering. Lower values appear first.
        /// </summary>
        public int SortOrder { get; set; }

        public DateTimeOffset Created { get; set; }
    }
}
