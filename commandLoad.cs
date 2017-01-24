using Rocket.API;
using Rocket.Unturned.Chat;
using Rocket.Unturned.Player;
using SDG.Unturned;
using System;
using System.Collections.Generic;

namespace Loadout
{
    public class CommandLoad : IRocketCommand
    {
        #region Properties
        public AllowedCaller AllowedCaller { get { return AllowedCaller.Player; } }

        public string Name { get { return "loadkit"; } }

        public string Help { get { return "This command will load your saved gear, you can save your gear by typing /savekit"; } }

        public string Syntax { get { return "[Kit Name]"; } }

        public List<string> Aliases { get { return new List<string>(); } }

        public List<string> Permissions { get { return new List<string> { "loadout.loadkit" }; } }

        #endregion Properties

        public void Execute(IRocketPlayer caller, string[] command)
        {
            UnturnedPlayer player = (UnturnedPlayer)caller;
            if (command.Length != 1)
            {
                UnturnedChat.Say(caller, Syntax);
                return;
            }

            if (!Loadout.instance.playerInvs[player.CSteamID]._invs.ContainsKey(command[0])) 
            {
                UnturnedChat.Say(player, Loadout.instance.Translate("no_kit"));
                return;
            }

            #region clothing

            PlayerClothing clo = player.Player.clothing;
            LoadoutClothes clothes = Loadout.instance.playerInvs[player.CSteamID]._invs[command[0]].clothes;

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

            for (int i = 0; i < Loadout.instance.playerInvs[player.CSteamID]._invs[command[0]].items.Count; i++)
            {
                LoadoutItem item = Loadout.instance.playerInvs[player.CSteamID]._invs[command[0]].items[i];
                Item item2 = new Item(item.id, true)
                {
                    metadata = item.meta
                };
                player.Inventory.tryAddItem(item2, true);
            }

            #endregion items

            UnturnedChat.Say(caller, Loadout.instance.Translate("loaded"));
        }
    }
}