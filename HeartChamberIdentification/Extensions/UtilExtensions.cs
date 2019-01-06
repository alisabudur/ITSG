﻿using System.IO;
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
    }
}
