using System;
namespace ExPresidents.Loadout
{
	public class LoadoutItem
	{
		public ushort id;
		public byte[] meta;

		public LoadoutItem(ushort id, byte[] meta)
		{
			this.id = id;
			this.meta = meta;
		}
	}
}
