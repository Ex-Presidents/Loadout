using Rocket.API.Collections;
using Rocket.Core.Logging;
using Rocket.Core.Plugins;
using System.Collections.Generic;

namespace Loadout
{
    public class Loadout : RocketPlugin
    {
        public Dictionary<Steamworks.CSteamID, LoadoutInventory> inventories;

        public static Loadout instance;

        protected override void Load()
        {
            instance = this;
            Logger.LogWarning("\tPlugin Loadout Loaded Successfully");
            inventories = new Dictionary<Steamworks.CSteamID, LoadoutInventory>();
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