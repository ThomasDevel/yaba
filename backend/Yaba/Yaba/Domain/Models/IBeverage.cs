using System;

namespace Yaba.Domain.Models
{
    public interface IBeverage
    {
        public string Id { get; set; }

        public static BeverageType Type { get; }

        public string Name { get; set; }

        public float Strength { get; set; }

        public int Size { get; set; }

        public DateTimeOffset Created { get; set; }
    }
}
