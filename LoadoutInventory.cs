using System;
using SDG.Unturned;
using System.Collections.Generic;

namespace ExPresidents.Loadout
{
    [Serializable]
    public class LoadoutInventory
    {
        public List<Item> items;
        public LoadoutClothes clothes;

        

        public LoadoutInventory(List<Item> items, LoadoutClothes clothes)
        {
            this.items = items;
            this.clothes = clothes;
        }
    }
    
    [Serializable]
    public class LoadoutList
    {
        public Dictionary<string, LoadoutInventory> inventories;
        
        public LoadoutList(Dictionary<string, LoadoutInventory> inventories)
        {
            this.inventories = inventories;
        }
    }
}
