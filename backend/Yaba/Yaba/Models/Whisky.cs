using System;

namespace Yaba
{
    public class Whisky
    {
        public string Id { get; set; }

        public string Name { get; set; }

        public WhiskyCategory Category { get; set; }

        public string Distillery { get; set; }

        public int Bottled { get; set; }

        public int Age { get; set; }

        public string CaskType { get; set; }

        public float Strength { get; set; }

        public int SizeInCl { get; set; }

        public bool NaturalColor { get; set; }

        public bool NonChillFiltered { get; set; }

        public DateTimeOffset Created { get; set; }
    }
}
