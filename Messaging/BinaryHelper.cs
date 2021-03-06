﻿using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

namespace Busey.Messaging
{
    public static class BinaryHelper
    {
        public static byte[] ToBytes<T>(this T obj)
        {
            if (obj == null)
            {
                return null;
            }

            var bf = new BinaryFormatter();
            using (var ms = new MemoryStream())
            {
                bf.Serialize(ms, obj);
                return ms.ToArray();
            }
        }

        public static T ToMessage<T>(this byte[] obj)
        {
            var bf = new BinaryFormatter();
            using (var ms = new MemoryStream(obj))
            {
                var message = bf.Deserialize(ms);

                return (T) message;
            }
        }
    }
}