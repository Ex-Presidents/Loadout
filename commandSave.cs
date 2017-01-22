using Rocket.API;
using Rocket.Unturned.Chat;
using Rocket.Unturned.Player;
using SDG.Unturned;
using Steamworks;
using System.Collections.Generic;

namespace Loadout
{
    public class commandSave : IRocketCommand
    {
        public AllowedCaller AllowedCaller { get { return AllowedCaller.Player; } }

        public string Name { get { return "savekit"; } }

        public string Help { get { return "This command will save your current gear, you can load it by typing /loadkit"; } }

        public string Syntax { get { return "[Kit Name]"; } }

        public List<string> Aliases { get { return new List<string>(); } }

        public List<string> Permissions { get { return new List<string> { "loadout.savekit" }; } }

        public void Execute(IRocketPlayer caller, string[] command)
        {
            UnturnedPlayer player = (UnturnedPlayer)caller;

            string kitName = "default";

            CSteamID id = player.CSteamID;

            if (!Loadout.instance.inventories.ContainsKey(id)) Loadout.instance.inventories.Add(id, new Dictionary<string, LoadoutInventory>());

            if (caller.HasPermission("loadout.multiplekits"))
                if (command.Length == 1)
                    kitName = command[0].ToLower();
                else
                    UnturnedChat.Say(player, Loadout.instance.Translate("only_default_save"));

            int maxKits = 0;

            if (caller.HasPermission("loadout.infite"))
                maxKits = int.MaxValue;
            else if (caller.HasPermission("loadout.four"))
                maxKits = 4;
            else if (caller.HasPermission("loadout.two"))
                maxKits = 2;
            else
                maxKits = 1;

            if (Loadout.instance.inventories[id].Count == maxKits)
            {
                UnturnedChat.Say(player, Loadout.instance.Translate("max_kits"));
                return;
            }

            if (Loadout.instance.inventories[id].ContainsKey(kitName)) Loadout.instance.inventories[id].Remove(kitName);

            List<LoadoutItem> itemList = new List<LoadoutItem>();

            #region items

            for (byte p = 0; p < PlayerInventory.PAGES - 1; p++)
            {
                for (byte i = 0; i < player.Inventory.getItemCount(p); i++)
                {
                    Item item = player.Inventory.getItem(p, i).item;
                    itemList.Add(new LoadoutItem(item.id, item.metadata));
                }
            }

            #endregion items

            #region clothing

            PlayerClothing clo = player.Player.clothing;

            LoadoutHat hat = new LoadoutHat(clo.hat, clo.hatQuality, clo.hatState);
            LoadoutMask mask = new LoadoutMask(clo.mask, clo.maskQuality, clo.maskState);
            LoadoutShirt shirt = new LoadoutShirt(clo.shirt, clo.shirtQuality, clo.shirtState);
            LoadoutVest vest = new LoadoutVest(clo.vest, clo.vestQuality, clo.vestState);
            LoadoutBackpack backpack = new LoadoutBackpack(clo.backpack, clo.backpackQuality, clo.backpackState);
            LoadoutPants pants = new LoadoutPants(clo.pants, clo.pantsQuality, clo.pantsState);

            LoadoutClothes clothes = new LoadoutClothes(hat, mask, shirt, vest, backpack, pants);

            #endregion clothing

            Loadout.instance.inventories[id].Add(kitName, new LoadoutInventory(itemList, clothes));
            UnturnedChat.Say(player, kitName + ", " + Loadout.instance.Translate("saved"));
        }
    }
}