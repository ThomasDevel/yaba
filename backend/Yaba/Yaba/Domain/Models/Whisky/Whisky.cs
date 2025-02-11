using System;
using Yaba.Models;

namespace Yaba.Domain.Models
{
    public class Whisky : Beverage
    {
        public WhiskyCategory Category { get; set; }

        public string Distillery { get; set; }

        public int Bottled { get; set; }

        public int Age { get; set; }

        public string CaskType { get; set; }

        public bool NaturalColor { get; set; }

        public bool NonChillFiltered { get; set; }

        public DateTimeOffset Created { get; set; }
    }
}
