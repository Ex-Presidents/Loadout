using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

namespace ExPresidents.Loadout
{
    public class BArrayManager
    {
        public static Byte[] ToArray(object Object)
        {
            using (MemoryStream MStream = new MemoryStream())
            {
                BinaryFormatter BFormatter = new BinaryFormatter();
                BFormatter.Serialize(MStream, Object);

                return MStream.ToArray();
            }
        }

        public static Dictionary<ulong, LoadoutList> ToObject(Byte[] BArray)
        {
            using (MemoryStream MStream = new MemoryStream())
            {
                BinaryFormatter BFormatter = new BinaryFormatter();
                MStream.Write(BArray, 0, BArray.Length);
                MStream.Position = 0;

                return BFormatter.Deserialize(MStream) as Dictionary<ulong, LoadoutList>;
            }
        }

        

    }
}