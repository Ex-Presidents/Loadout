using System.Collections.Generic;

namespace ExPresidents.Loadout
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
    
    [Serializable]
    public class LoadoutList
    {
        public Dictionary<string, LoadoutInventory> _invs;
        
        public LoadoutList(Dictionary<string, LoadoutInventory> z)
        {
            this._invs = z;
        }
    }
}
