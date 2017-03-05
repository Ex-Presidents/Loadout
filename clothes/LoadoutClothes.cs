using System;
namespace ExPresidents.Loadout
{
	public class LoadoutClothes
	{
		public LoadoutHat hat;
        public LoadoutGlasses glasses;
		public LoadoutMask mask;
		public LoadoutShirt shirt;
		public LoadoutVest vest;
		public LoadoutBackpack backpack;
		public LoadoutPants pants;

		public LoadoutClothes(LoadoutHat hat, LoadoutGlasses glasses , LoadoutMask mask, LoadoutShirt shirt, LoadoutVest vest, LoadoutBackpack backpack, LoadoutPants pants)
		{
			this.hat = hat;
            this.glasses = glasses;
			this.mask = mask;
			this.shirt = shirt;
			this.vest = vest;
			this.backpack = backpack;
			this.pants = pants;
		}
	}
}
