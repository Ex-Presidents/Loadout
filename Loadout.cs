using Rocket.API.Collections;
using Rocket.Core.Logging;
using Rocket.Core.Plugins;
using System.Collections.Generic;

namespace Loadout
{
    public class Loadout : RocketPlugin
    {
        public Dictionary<Steamworks.CSteamID, Dictionary<string, LoadoutInventory>> inventories;

        public static Loadout instance;

        protected override void Load()
        {
            instance = this;
            Logger.LogWarning("\tPlugin Loadout Loaded Successfully");
            inventories = new Dictionary<Steamworks.CSteamID, Dictionary<string, LoadoutInventory>>();
        }

        public override TranslationList DefaultTranslations
        {
            get
            {
                return new TranslationList()
                {
                    {"no_kits", "You haven't saved any kit yet, do /savekit [name] to save kit!"},
                    {"no_kit", "Kit with that name doesn't exist."},
                    {"loaded", "Loaded kit successfully!"},
                    {"saved", "Saved kit successfully!"},
                    {"deleted", "Deleted kit suffescully!"},
                    {"max_kits", "You don't have free kit slots, delete old kits with /delkit <Kit Name> or condinser buying VIP for more kit slots and more."},
                    {"only_default_save", "You can't save multiple kits, saving this kit as 'default' load it with /loadkit"},
                    {"only_default_load", "You can't save multiple kits, loading kit 'default'."},
                    {"syntax_del", "Invalid arguments, Usage: '/delkit <Kit Name', to get list of your kits type /listkit"},
                    {"listkit", "You have saved following kits: "},
                };
            }
        }
    }
}