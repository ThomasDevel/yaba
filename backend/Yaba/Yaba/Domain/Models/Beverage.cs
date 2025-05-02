using System;

namespace Yaba.Domain.Models
{
    public class Beverage : IBeverage
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public float Strength { get; set; }
        public int Size { get; set; }
        public DateTimeOffset Created { get; set; }
    }
}
