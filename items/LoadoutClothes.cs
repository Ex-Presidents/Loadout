using System;

namespace ExPresidents.Loadout
{
    [Serializable]
    public class LoadoutClothes
    {
        public LoadoutClothing hat;
        public LoadoutClothing glasses;
        public LoadoutClothing mask;
        public LoadoutClothing shirt;
        public LoadoutClothing vest;
        public LoadoutClothing backpack;
        public LoadoutClothing pants;

        public LoadoutClothes(LoadoutClothing hat, LoadoutClothing glasses, LoadoutClothing mask, LoadoutClothing shirt, LoadoutClothing vest, LoadoutClothing backpack, LoadoutClothing pants)
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