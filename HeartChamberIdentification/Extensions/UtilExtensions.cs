using System;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;

namespace HeartChamberIdentification.Extensions
{
    public static class UtilExtensions
    {
        public static T DeepClone<T>(this T obj)
        {
            using (var ms = new MemoryStream())
            {
                var formatter = new BinaryFormatter();
                formatter.Serialize(ms, obj);
                ms.Position = 0;

                return (T)formatter.Deserialize(ms);
            }
        }

        public static int[][] ConvertToIntArray(this double[][] array)
        {
            var n = array.Length;

            var newArray = new int[n][];
            for (var i = 0; i < n; i++)
            {
                var m = array[i].Length;
                newArray[i] = new int[m];
                for (var j = 0; j < m; j++)
                {
                    newArray[i][j] = Convert.ToInt32(array[i][j]);
                }
            }

            return newArray;
        }
    }
}
