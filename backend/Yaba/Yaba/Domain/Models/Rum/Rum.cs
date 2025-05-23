﻿namespace Yaba.Domain.Models.Rum
{
    public class Rum
    {
        public RumCategory Category { get; set; }

        public string Distillery { get; set; }

        public int Bottled { get; set; }

        public int Age { get; set; }

        public string CaskType { get; set; }

        public string BottlingSeries { get; set; }

        public bool? NaturalColor { get; set; }

        public bool? NonChillFiltered { get; set; }

        public bool? AddedSuger { get; set; }
    }
}
