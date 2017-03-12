using Rocket.API;
using Rocket.Unturned.Chat;
using Rocket.Unturned.Player;
using SDG.Unturned;
using System.Collections.Generic;

namespace ExPresidents.Loadout
{
    public class CommandSave : IRocketCommand
    {
        #region Properties
        public AllowedCaller AllowedCaller { get { return AllowedCaller.Player; } }

        public string Name { get { return "savekit"; } }

        public string Help { get { return "This command will save your current gear, you can load it by typing /loadkit"; } }

        public string Syntax { get { return "[Kit Name]"; } }

        public List<string> Aliases { get { return new List<string>(); } }

        public List<string> Permissions { get { return new List<string> { "loadout.savekit" }; } }

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

            List<LoadoutItem> itemList = new List<LoadoutItem>();

            PlayerClothing clo = player.Player.clothing;

            #endregion vars

            #region items

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
                    itemList.Add(new LoadoutItem(item.id, item.metadata));
                }
            }

            if(itemList.Count > Loadout.Instance.Configuration.Instance.ItemLimit)
            {
                UnturnedChat.Say(caller, Loadout.Instance.Translate("too_much", Loadout.Instance.Configuration.Instance.ItemLimit, itemList.Count));
                return;
            }

            #endregion items

            #region clothing

            LoadoutHat hat = new LoadoutHat(clo.hat, clo.hatQuality, clo.hatState);
            LoadoutGlasses glasses = new LoadoutGlasses(clo.glasses, clo.glassesQuality, clo.glassesState);
            LoadoutMask mask = new LoadoutMask(clo.mask, clo.maskQuality, clo.maskState);
            LoadoutShirt shirt = new LoadoutShirt(clo.shirt, clo.shirtQuality, clo.shirtState);
            LoadoutVest vest = new LoadoutVest(clo.vest, clo.vestQuality, clo.vestState);
            LoadoutBackpack backpack = new LoadoutBackpack(clo.backpack, clo.backpackQuality, clo.backpackState);
            LoadoutPants pants = new LoadoutPants(clo.pants, clo.pantsQuality, clo.pantsState);

            LoadoutClothes clothes = new LoadoutClothes(hat, glasses, mask, shirt, vest, backpack, pants);

            #endregion clothing

            #region dictionary

            if (!Loadout.Instance.playerInvs.ContainsKey(id))
            {
                Loadout.Instance.playerInvs.Add(id, new LoadoutList(new Dictionary<string, LoadoutInventory>()));
                Loadout.Instance.playerInvs[player.CSteamID.m_SteamID]._invs.Add(command[0], new LoadoutInventory(itemList, clothes));
                UnturnedChat.Say(caller, Loadout.Instance.Translate("saved"));
            }
            else
            {
                if (!Loadout.Instance.playerInvs[id]._invs.ContainsKey(command[0]))
                {

                    Loadout.Instance.playerInvs[player.CSteamID.m_SteamID]._invs.Add(command[0], new LoadoutInventory(itemList, clothes));
                    UnturnedChat.Say(caller, Loadout.Instance.Translate("saved"));
                }
                else
                {
                    Loadout.Instance.playerInvs[id]._invs.Remove(command[0]);
                    Loadout.Instance.playerInvs[player.CSteamID.m_SteamID]._invs.Add(command[0], new LoadoutInventory(itemList, clothes));
                    UnturnedChat.Say(caller, Loadout.Instance.Translate("replaced"));
                }
            }

            #endregion dictionary
        }
    }
}
