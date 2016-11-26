using System;
using System.Collections.Generic;

namespace Loadout
{
	public class LoadoutInventory
	{
		public List<ushort> items;
		public LoadoutClothes clothes;

		public LoadoutInventory(List<ushort> items, LoadoutClothes clothes)
		{
			this.items = items;
			this.clothes = clothes;
		}
	}
}
