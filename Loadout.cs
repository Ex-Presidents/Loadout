using Rocket.API.Collections;
using Rocket.Core.Logging;
using Rocket.Core.Plugins;
using System.Collections.Generic;
using MySql.Data.MySqlClient;
using System;

namespace ExPresidents.Loadout
{
    public class Loadout : RocketPlugin<Configuration>
    {
        #region Fields

        public Dictionary <ulong, LoadoutList> playerInvs;
        public static Loadout Instance;
        public MySqlConnection Connection;
        public DBManager DB;

        #endregion Fields

        protected override void Load()
        {
            Instance = this;
            if (!DB.CheckDictionary(Connection, SDG.Unturned.Provider.ip.ToString()))
                playerInvs = new Dictionary<ulong, LoadoutList>();
            else
            {
                try
                {
                    DB = new DBManager();
                    DB.LoadDictionary(Connection, SDG.Unturned.Provider.ip.ToString());
                }
                catch (Exception ex) { Logger.LogException(ex); }
            }
            Logger.LogWarning("\tPlugin Loadout loaded successfully.");
        }

        protected override void Unload()
        {
            try
            {
                if (playerInvs != null)
                    DB.SaveDictionary(Connection, SDG.Unturned.Provider.ip.ToString());
                Connection.Dispose();
            }
            catch(Exception ex) { Logger.LogException(ex); }

            Logger.Log("\tPlugin Loadout unloaded successfully.");
        }

        public override TranslationList DefaultTranslations
        {
            get
            {
                return new TranslationList()
                {
                    {"no_kit", "You have no kits saved!"},
                    {"loaded", "Loaded kit successfully!"},
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