using System.Collections.Generic;

namespace HeartChamberIdentification.Extensions
{
    public static class NormalizationExtensions
    {
        public static double[] Normalize(this IEnumerable<double> array, int min, int max)
        {
            var newArray = new List<double>();
            foreach (var item in array)
            {
                newArray.Add((item - min) /(max - min));
            }

            return newArray.ToArray();
        }
    }
}
