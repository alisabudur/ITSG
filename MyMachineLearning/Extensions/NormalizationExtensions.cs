using System.Collections.Generic;
using System.Linq;

namespace MyMachineLearning.Extensions
{
    public static class NormalizationExtensions
    {
        public static IEnumerable<double> Normalize(this IEnumerable<int> items)
        {
            var itemsArray = items.ToArray();
            var normalizedList = new List<double>();
            var min = itemsArray.Min();
            var max = itemsArray.Max();

            foreach (var item in items)
            {
                normalizedList.Add((item - min) / (max - min));
            }

            return normalizedList;
        }
    }
}
