using ExPresidents.Loadout.items;
using Rocket.API;
using Rocket.Unturned.Chat;
using Rocket.Unturned.Player;
using SDG.Unturned;
using System.Collections.Generic;

using Logger = Rocket.Core.Logging.Logger;

namespace ExPresidents.Loadout
{
    public class CommandSave : IRocketCommand
    {
        #region Properties

        public AllowedCaller AllowedCaller => AllowedCaller.Player;

        public string Name => "savekit";

        public string Help => "This command will save your current gear, you can load it by typing /loadkit";

        public string Syntax => "[Kit Name]";

        public List<string> Aliases => new List<string>();

        public List<string> Permissions => new List<string> { "loadout.savekit" };

        #endregion Properties

        public void Execute(IRocketPlayer caller, string[] command)
        {
            if (command.Length != 1)
            {
                UnturnedChat.Say(caller, Syntax);
                return;
            }

            #region vars

            UnturnedPlayer player = (UnturnedPlayer)caller;

            ulong id = player.CSteamID.m_SteamID;

            List<LItem> itemList = new List<LItem>();

            PlayerClothing clo = player.Player.clothing;

            bool DebugMode = Loadout.Instance.Configuration.Instance.DebugMode;

            #endregion vars

            #region items

            if (DebugMode)
                Logger.Log("Beginning item saves");

            for (byte p = 0; p < PlayerInventory.PAGES - 1; p++)
            {
                for (byte i = 0; i < player.Inventory.getItemCount(p); i++)
                {
                    Item item = player.Inventory.getItem(p, i).item;
                    if (Loadout.Instance.Configuration.Instance.ItemBlacklist.Contains(item.id))
                    {
                        if (Loadout.Instance.Configuration.Instance.DenyOnBlacklist)
                        {
                            UnturnedChat.Say(caller, Loadout.Instance.Translate("denied", item.id));
                            return;
                        }
                        else
                        {
                            UnturnedChat.Say(caller, Loadout.Instance.Translate("blacklisted", item.id));
                            continue;
                        }
                    }
                    itemList.Add(new LItem(item.id, item.metadata));
                }
            }

            if (DebugMode)
                Logger.Log("Beginning blacklist check");

            if (itemList.Count > Loadout.Instance.Configuration.Instance.ItemLimit)
            {
                UnturnedChat.Say(caller, Loadout.Instance.Translate("too_much", Loadout.Instance.Configuration.Instance.ItemLimit, itemList.Count));
                return;
            }

            if (DebugMode)
            {
                Logger.Log("Item saves complete");
                if (itemList == null)
                    Logger.Log("Null");
            }

            #endregion items

            #region clothing

            if (DebugMode)
                Logger.Log("Beginning clothing save");

            LoadoutClothing hat = new LoadoutClothing(clo.hat, clo.hatQuality, clo.hatState);
            LoadoutClothing glasses = new LoadoutClothing(clo.glasses, clo.glassesQuality, clo.glassesState);
            LoadoutClothing mask = new LoadoutClothing(clo.mask, clo.maskQuality, clo.maskState);
            LoadoutClothing shirt = new LoadoutClothing(clo.shirt, clo.shirtQuality, clo.shirtState);
            LoadoutClothing vest = new LoadoutClothing(clo.vest, clo.vestQuality, clo.vestState);
            LoadoutClothing backpack = new LoadoutClothing(clo.backpack, clo.backpackQuality, clo.backpackState);
            LoadoutClothing pants = new LoadoutClothing(clo.pants, clo.pantsQuality, clo.pantsState);

            LoadoutClothes clothes = new LoadoutClothes(hat, glasses, mask, shirt, vest, backpack, pants);

            if (DebugMode)
            {
                Logger.Log("Clothing save complete");
                if (clothes == null)
                    Logger.Log("Null");
            }

            #endregion clothing

            #region dictionary

            if (DebugMode)
            {
                Logger.Log("Beginning final dictionary save");
                if (Loadout.Instance.playerInvs == null)
                    Logger.Log("Null");
            }

            if (!Loadout.Instance.playerInvs.ContainsKey(id))
            {
                Loadout.Instance.playerInvs.Add(id, new LoadoutList(new Dictionary<string, LoadoutInventory>()));
                Loadout.Instance.playerInvs[id].inventories.Add(command[0], new LoadoutInventory(itemList, clothes));
                UnturnedChat.Say(caller, Loadout.Instance.Translate("saved"));

                if (DebugMode)
                    Logger.Log("Player has no saves, adding to dictionary");
            }
            else
            {
                if (DebugMode)
                    Logger.Log("Player has saves");

                if (!Loadout.Instance.playerInvs[id].inventories.ContainsKey(command[0]))
                {
                    Loadout.Instance.playerInvs[id].inventories.Add(command[0], new LoadoutInventory(itemList, clothes));
                    UnturnedChat.Say(caller, Loadout.Instance.Translate("saved"));

                    if (DebugMode)
                        Logger.Log("Player saved a unique kit");
                }
                else
                {
                    Loadout.Instance.playerInvs[id].inventories.Remove(command[0]);
                    Loadout.Instance.playerInvs[id].inventories.Add(command[0], new LoadoutInventory(itemList, clothes));
                    UnturnedChat.Say(caller, Loadout.Instance.Translate("replaced"));

                    if (DebugMode)
                        Logger.Log("Player has replaced a kit");
                }
            }

            #endregion dictionary
        }
    }
}