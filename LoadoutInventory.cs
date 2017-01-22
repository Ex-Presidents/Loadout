using System.Collections.Generic;

namespace Loadout
{
    public class LoadoutInventory
    {
        public List<LoadoutItem> items;
        public LoadoutClothes clothes;

        public LoadoutInventory(List<LoadoutItem> items, LoadoutClothes clothes)
        {
            this.items = items;
            this.clothes = clothes;
        }
    }
}