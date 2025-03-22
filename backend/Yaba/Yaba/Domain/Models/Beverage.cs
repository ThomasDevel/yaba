using System;

namespace Yaba.Domain.Models
{
    public class Beverage
    {
        public string Id { get; set; }

        public BeverageClass Class { get; set; }

        public string Name { get; set; }

        public float Strength { get; set; }

        public int Size { get; set; }

        // Should not be on entity
        public string Thumbnail { get; set; }

        // Should not be on entity
        public string[] Images { get; set; }

        public DateTimeOffset Created { get; set; }
    }
}
