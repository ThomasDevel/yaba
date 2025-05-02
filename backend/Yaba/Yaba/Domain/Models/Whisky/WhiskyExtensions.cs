using System;
using Yaba.Domain.Models;

namespace Yaba.Models
{
    public static class WhiskyExtensions
    {
        public static WhiskyEntity ToEntity(this Whisky whisky)
        {
            return new WhiskyEntity()
            {
                Age = whisky.Age,
                Bottled = whisky.Bottled,
                BottlingSeries = whisky.BottlingSeries,
                CaskType = whisky.CaskType,
                Category = whisky.Category.ToString(),
                Created = whisky.Created,
                Distillery = whisky.Distillery,
                Id = whisky.Id,
                Name = whisky.Name,
                NaturalColor = whisky.NaturalColor,
                NonChillFiltered = whisky.NonChillFiltered,
                Size = whisky.Size,
                Strength = whisky.Strength
            };
        }

        public static Whisky ToDomain(this WhiskyEntity entity)
        {
            return new Whisky()
            {
                Age = entity.Age,
                Bottled = entity.Bottled,
                BottlingSeries = entity.BottlingSeries,
                CaskType = entity.CaskType,
                Category = Enum.Parse<WhiskyCategory>(entity.Category, true),
                Created = entity.Created,
                Distillery = entity.Distillery,
                Id = entity.Id,
                Name = entity.Name,
                NaturalColor = entity.NaturalColor,
                NonChillFiltered = entity.NonChillFiltered,
                Size = entity.Size,
                Strength = entity.Strength
            };
        }
    }
}
