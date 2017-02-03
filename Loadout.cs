using Rocket.API.Collections;
using Rocket.Core.Logging;
using Rocket.Core.Plugins;
using Steamworks;
using System.Collections.Generic;

namespace Loadout
{
    public class Loadout : RocketPlugin<Configuration>
    {
       // public Dictionary<Steamworks.CSteamID, LoadoutInventory> inventories;
        public Dictionary <CSteamID, LoadoutList> playerInvs;
        public static Loadout Instance;

        protected override void Load()
        {
            Instance = this;
            Logger.LogWarning("\tPlugin Loadout Loaded Successfully");
            playerInvs = new Dictionary<Steamworks.CSteamID, LoadoutList>();
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
                    {"replaced", "Replaced kit successfully!" }
                };
            }
        }
    }
}