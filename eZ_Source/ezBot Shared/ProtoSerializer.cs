using System;
using System.IO;

namespace ezBot_Shared
{
    public class ProtoSerializer {
        public static byte[] Serialize<T>(T obj) {
            byte[] result;
            using (MemoryStream stream = new MemoryStream())
            {
                ProtoBuf.Serializer.Serialize(stream, obj);
                result = stream.ToArray();
            }
            return result;
        }

        public static T Deserialize<T>(byte[] bytes) {
            T result = default(T);
            try {
                using (MemoryStream stream = new MemoryStream(bytes, 0, bytes.Length))
                {
                    result = ProtoBuf.Serializer.Deserialize<T>(stream);
                }
            }
            catch(Exception e) {
                Console.WriteLine(e.StackTrace);
            }
            return result;
        }

    }
}
