using System;
namespace ExPresidents.Loadout
{
    [Serializable]
	public class LoadoutClothing
	{
		public ushort id;
		public byte quality;
		public byte[] state;

		public LoadoutClothing(ushort id, byte quality, byte[] state)
		{
			this.id = id;
			this.quality = quality;
			this.state = state;
		}
	}
}