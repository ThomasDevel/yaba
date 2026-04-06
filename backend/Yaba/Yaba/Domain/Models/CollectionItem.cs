using System;

namespace Yaba.Domain.Models
{
    public class CollectionItem
    {
        public string UserId { get; set; }

        public BeverageType SpiritType { get; set; }

        public string SpiritId { get; set; }

        public DateTimeOffset Added { get; set; }
    }
}
