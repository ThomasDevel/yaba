using System;

namespace Yaba.Domain.Models
{
    public class Whisky : IBeverage
    {
        public string Id { get; set; }

        public static BeverageType Type => BeverageType.Whisky;

        public string Name { get; set; }

        public float Strength { get; set; }

        public int Size { get; set; }

        public DateTimeOffset Created { get; set; }

        public WhiskyCategory Category { get; set; }

        public string Distillery { get; set; }

        public int Bottled { get; set; }

        public int Age { get; set; }

        public string CaskType { get; set; }

        public string BottlingSeries { get; set; }

        public bool? NaturalColor { get; set; }

        public bool? NonChillFiltered { get; set; }

    }
}
