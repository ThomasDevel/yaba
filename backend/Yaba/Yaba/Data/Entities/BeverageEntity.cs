using System;
using System.ComponentModel.DataAnnotations;
using Yaba.Domain.Models;

namespace Yaba.Data.Entities
{
    public class BeverageEntity
    {
        [Required]
        public string Id { get; set; }

        public BeverageClass Class { get; }

        public string Name { get; set; }

        public float Strength { get; set; }

        public int Size { get; set; }

        public DateTimeOffset Created { get; set; }

        // Should not be on entity
        public string Thumbnail { get; set; }

        // Should not be on entity
        public string[] Images { get; set; }

        public static BeverageEntity ToEntity(Beverage beverage)
        {
            return new BeverageEntity()
            {
                Id = beverage.Id,
                Class = beverage.Class,
                Name = beverage.Name,
                Created = beverage.Created,
                Strength = beverage.Strength,
                Size = beverage.Size
            };
        }

        public static Beverage ToDomain(BeverageEntity entity)
        {
            return new Beverage()
            {
                Id = entity.Id,
                Class = entity.Class,
                Name = entity.Name,
                Created = entity.Created,
                Strength = entity.Strength,
                Size = entity.Size
            };
        }
    }
}
