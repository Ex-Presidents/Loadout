using Rocket.API;
using Rocket.Unturned.Chat;
using Rocket.Unturned.Player;
using SDG.Unturned;
using System;
using System.Collections.Generic;

namespace Loadout
{
    public class commandLoad : IRocketCommand
    {
        public AllowedCaller AllowedCaller { get { return AllowedCaller.Player; } }

        public string Name { get { return "loadkit"; } }

        public string Help { get { return "This command will load your saved gear, you can save your gear by typing /savekit"; } }

        public string Syntax { get { return "[Kit Name]"; } }

        public List<string> Aliases { get { return new List<string>(); } }

        public List<string> Permissions { get { return new List<string> { "loadout.loadkit" }; } }

        public void Execute(IRocketPlayer caller, string[] command)
        {
            UnturnedPlayer player = (UnturnedPlayer)caller;
            if (!Loadout.instance.inventories.ContainsKey(player.CSteamID))
            {
                UnturnedChat.Say(player, Loadout.instance.Translate("no_kits"));
                return;
            }

            String kitName = "default";

            if (caller.HasPermission("loadout.multiplekits"))
                if (command.Length == 1)
                    kitName = command[0];
                else
                    UnturnedChat.Say(player, Loadout.instance.Translate("only_default_load"));

            if (!Loadout.instance.inventories[player.CSteamID].ContainsKey(kitName))
            {
                UnturnedChat.Say(player, Loadout.instance.Translate("no_kit"));
                return;
            }

            #region clothing

            PlayerClothing clo = player.Player.clothing;
            LoadoutClothes clothes = Loadout.instance.inventories[player.CSteamID][kitName].clothes;

            LoadoutHat hat = clothes.hat;
            LoadoutMask mask = clothes.mask;
            LoadoutShirt shirt = clothes.shirt;
            LoadoutVest vest = clothes.vest;
            LoadoutBackpack backpack = clothes.backpack;
            LoadoutPants pants = clothes.pants;

            if (hat != null) clo.askWearHat(hat.id, hat.quality, hat.state, true);
            if (mask != null) clo.askWearMask(mask.id, mask.quality, mask.state, true);
            if (shirt != null) clo.askWearShirt(shirt.id, shirt.quality, shirt.state, true);
            if (vest != null) clo.askWearVest(vest.id, vest.quality, vest.state, true);
            if (backpack != null) clo.askWearBackpack(backpack.id, backpack.quality, backpack.state, true);
            if (pants != null) clo.askWearPants(pants.id, pants.quality, pants.state, true);

            #endregion clothing

            #region items

            for (int i = 0; i < Loadout.instance.inventories[player.CSteamID][kitName].items.Count; i++)
            {
                LoadoutItem item = Loadout.instance.inventories[player.CSteamID][kitName].items[i];
                Item item2 = new Item(item.id, true);
                item2.metadata = item.meta;
                player.Inventory.tryAddItem(item2, true);
            }

            #endregion items

            UnturnedChat.Say(player, kitName + ", " + Loadout.instance.Translate("loaded"));
        }
    }
}