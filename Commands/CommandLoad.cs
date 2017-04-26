using ExPresidents.Loadout.items;
using Rocket.API;
using Rocket.Unturned.Chat;
using Rocket.Unturned.Player;
using SDG.Unturned;
using System.Collections.Generic;

namespace ExPresidents.Loadout
{
    public class CommandLoad : IRocketCommand
    {
        #region Properties

        public AllowedCaller AllowedCaller => AllowedCaller.Player;

        public string Name => "loadkit";

        public string Help => "This command will load your saved gear, you can save your gear by typing /savekit";

        public string Syntax => "[Kit Name]";

        public List<string> Aliases => new List<string>();

        public List<string> Permissions => new List<string> { "loadout.loadkit" };

        #endregion Properties

        public void Execute(IRocketPlayer caller, string[] command)
        {
            UnturnedPlayer player = (UnturnedPlayer)caller;
            if (command.Length != 1)
            {
                UnturnedChat.Say(caller, Syntax);
                return;
            }

            if (!Loadout.Instance.playerInvs[player.CSteamID.m_SteamID].inventories.ContainsKey(command[0]))
            {
                UnturnedChat.Say(player, Loadout.Instance.Translate("no_kit"));
                return;
            }

            #region clothing

            PlayerClothing clo = player.Player.clothing;
            LoadoutClothes clothes = Loadout.Instance.playerInvs[player.CSteamID.m_SteamID].inventories[command[0]].clothes;

            LoadoutClothing hat = clothes.hat;
            LoadoutClothing glasses = clothes.glasses;
            LoadoutClothing mask = clothes.mask;
            LoadoutClothing shirt = clothes.shirt;
            LoadoutClothing vest = clothes.vest;
            LoadoutClothing backpack = clothes.backpack;
            LoadoutClothing pants = clothes.pants;

            if (hat != null) clo.askWearHat(hat.id, hat.quality, hat.state, true);
            if (glasses != null) clo.askWearGlasses(glasses.id, glasses.quality, glasses.state, true);
            if (mask != null) clo.askWearMask(mask.id, mask.quality, mask.state, true);
            if (shirt != null) clo.askWearShirt(shirt.id, shirt.quality, shirt.state, true);
            if (vest != null) clo.askWearVest(vest.id, vest.quality, vest.state, true);
            if (backpack != null) clo.askWearBackpack(backpack.id, backpack.quality, backpack.state, true);
            if (pants != null) clo.askWearPants(pants.id, pants.quality, pants.state, true);

            #endregion clothing

            #region items

            for (int i = 0; i < Loadout.Instance.playerInvs[player.CSteamID.m_SteamID].inventories[command[0]].items.Count; i++)
            {
                LItem item = Loadout.Instance.playerInvs[player.CSteamID.m_SteamID].inventories[command[0]].items[i];
                Item item2 = new Item(item.ID, true)
                {
                    metadata = item.Meta
                };
                player.Inventory.tryAddItem(item2, true);
            }

            #endregion items

            UnturnedChat.Say(caller, Loadout.Instance.Translate("loaded"));
        }
    }
}