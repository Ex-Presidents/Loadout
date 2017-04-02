using System;

namespace ExPresidents.Loadout.items
{
    [Serializable]
    public class LItem
    {
        public Byte[] Meta;

        public ushort ID;

        private LItem()
        {
        }

        public LItem(ushort id, Byte[] meta)
        {
            Meta = meta;
            ID = id;
        }
    }
}