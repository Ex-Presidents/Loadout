using Rocket.API.Collections;
using Rocket.Unturned.Chat;
using Rocket.Core.Plugins;
using Rocket.API;
using Rocket.Unturned.Events;
using Rocket.Unturned.Player;
using UnityEngine;
using System;
using System.Collections.Generic;

using Logger = Rocket.Core.Logging.Logger;
using SDG.Unturned;
using ExPresidents.Loadout.items;

namespace ExPresidents.Loadout
{
    public class Loadout : RocketPlugin<Configuration>
    {
        #region Fields

        public Dictionary<ulong, LoadoutList> playerInvs;
        public List<ulong> Autos;
        public static Loadout Instance;
        public DBManager DB;
        private bool DebugMode;

        #endregion Fields

        protected override void Load()
        {
            Instance = this;
            try
            {
                if(KUtils.Main.Load(typeof(Loadout).Assembly) == KUtils.Error.OUTDATED)
                {
                    Logger.Log("Please update KUtils or plugin to use plugin.");
                    UnloadPlugin();
                }
            }
            catch(Exception ex)
            {
                Logger.Log("Please update KUtils or plugin to use plugin.");
                    UnloadPlugin();
            }
            DebugMode = Instance.Configuration.Instance.DebugMode;
            if (DebugMode)
                Logger.Log("Initializing database.");
            try { DB = new DBManager(); }
            catch (Exception ex) { Logger.LogException(ex); }
            if (!DB.CheckDictionary(SDG.Unturned.Provider.ip.ToString()))
            {
                playerInvs = new Dictionary<ulong, LoadoutList>();
                if (DebugMode)
                    Logger.Log("No dictionary found, creating one.");
            }
            else
            {
                if (DebugMode)
                    Logger.Log("Dictionary found, attempting to load it.");
                try { DB.LoadDictionary(SDG.Unturned.Provider.ip.ToString()); }
                catch (Exception ex) { Logger.LogException(ex); }
            }
            UnturnedPlayerEvents.OnPlayerRevive += OnRevive;
            Logger.LogWarning("\tLoadout loaded successfully.");
        }

        protected override void Unload()
        {
            try
            {
                if (playerInvs != null)
                    DB.SaveDictionary(SDG.Unturned.Provider.ip.ToString());
            }
            catch (Exception ex) { Logger.LogException(ex); }
            UnturnedPlayerEvents.OnPlayerRevive -= OnRevive;
            Logger.Log("\tPlugin Loadout unloaded successfully.");
        }

        public void OnRevive(UnturnedPlayer Player, Vector3 Place , byte idk)
        {
            if(Autos.Contains(Player.CSteamID.m_SteamID) && ((IRocketPlayer)Player).HasPermission("loadout.autoload"))
            {
                LoadoutList List = playerInvs[Player.CSteamID.m_SteamID];
                if (List.inventories.ContainsKey("default"))
                {
                    LoadoutInventory Inventory = List.inventories["default"];

                    #region clothing

                    PlayerClothing clo = Player.Player.clothing;
                    LoadoutClothes clothes = Inventory.clothes;

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

                    for (int i = 0; i < Inventory.items.Count; i++)
                    {
                        LItem item = Inventory.items[i];
                        Item item2 = new Item(item.ID, true)
                        {
                            metadata = item.Meta
                        };
                        Player.Inventory.tryAddItem(item2, true);
                    }

                    #endregion items
                    UnturnedChat.Say(Player, Instance.Translate("auto_loaded"));
                }
                else
                    UnturnedChat.Say(Player, Instance.Translate("no_default"));
            }
        }

        public override TranslationList DefaultTranslations
        {
            get
            {
                return new TranslationList()
                {
                    {"no_kit", "You have no kits saved!"},
                    {"no_default", "You have no default kit saved!"},
                    {"loaded", "Loaded kit successfully!"},
                    {"auto_loaded", "Automatically loaded kit successfully!"},
                    {"saved", "Saved kit successfully!"},
                    {"replaced", "Replaced kit successfully!" },
                    {"syntax", "You used this command with invalid syntax." },
                    {"null", "Dictionary is null, nothing to save." },
                    {"denied", "Item {0} is blacklisted. Your kit has not been saved." },
                    {"blacklisted", "Item {0} is blacklisted. This particular item was not saved." },
                    {"too_much", "Item limit is {0}, you tried to save {1} items. Your kit has not been saved." }
                };
            }
        }
    }
}