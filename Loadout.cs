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
        public Dictionary <ulong, LoadoutList> playerInvs;
        public static Loadout Instance;
        public MySqlConnection Connection;
        public DBManager DB;

        protected override void Load()
        {
            Instance = this;
            playerInvs = new Dictionary<ulong, LoadoutList>();
            try
            {
                DB = new DBManager();
                DB.LoadDictionary(Connection, SDG.Unturned.Provider.ip.ToString());
            }
            catch(Exception ex) { Logger.LogException(ex); }
            Logger.LogWarning("\tPlugin Loadout loaded successfully.");
        }

        protected override void Unload()
        {
            try
            {
                if (playerInvs != null)
                    DB.SaveDictionary(Connection, SDG.Unturned.Provider.ip.ToString());
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
                    {"null", "Dictionary is null, nothing to save." }
                };
            }
        }
    }
}