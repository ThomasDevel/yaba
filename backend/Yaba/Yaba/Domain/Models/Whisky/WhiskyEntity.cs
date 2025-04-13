using System;

namespace Yaba.Domain.Models
{
    /// <summary>
    /// 
    /// </summary>
    public class WhiskyEntity
    {
        public string Id { get; set; }

        public BeverageType Type => Whisky.Type;

        public string Name { get; set; }

        public WhiskyCategory Category { get; set; }

        public string Distillery { get; set; }

        public int Bottled { get; set; }

        public int Age { get; set; }

        public float Strength { get; set; }

        public int Size { get; set; }

        public bool? NaturalColor { get; set; }

        public bool? ChillFiltered { get; set; }

        public DateTimeOffset Created { get; set; }

    }
}
