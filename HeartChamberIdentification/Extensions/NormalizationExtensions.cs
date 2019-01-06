using System.Collections.Generic;
using System.Linq;
using HeartChamberIdentification.DAL;

namespace HeartChamberIdentification.Extensions
{
    public static class NormalizationExtensions
    {
        public static IEnumerable<Pixel> Normalize(this IEnumerable<Pixel> items)
        {
            var itemsArray = items.ToArray();
            var minX = itemsArray.Select(x => x.PositionX).Min();
            var minY = itemsArray.Select(x => x.PositionY).Min();
            var minR = itemsArray.Select(x => x.Red).Min();

            var maxX = itemsArray.Select(x => x.PositionX).Max();
            var maxY = itemsArray.Select(x => x.PositionY).Max();
            var maxR = itemsArray.Select(x => x.Red).Max();

            var normalizedList = new List<Pixel>();
            foreach (var item in itemsArray)
            {
                var pixel = new Pixel
                {
                    Id = item.Id,
                    ImageName = item.ImageName,
                    IsContour = item.IsContour,
                    PositionX = (item.PositionX - minX) / (maxX - minX),
                    PositionY = (item.PositionY - minY) / (maxY - minY),
                    Red = (item.Red - minR) / (maxR - minR),
                };

                normalizedList.Add(pixel);
            }

            return normalizedList;
        }
    }
}
