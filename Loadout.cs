using System.Collections.Generic;
using Rocket.Core.Plugins;
using Rocket.Core.Logging;
using Rocket.API.Collections;

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
                    {"no_kit", "Please save a kit first!"},
                    {"loaded", "Loaded kit successfully!"},
                    {"saved", "Saved kit successfully!"}
                };
            }
        }
    }
}
