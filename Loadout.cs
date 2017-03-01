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
			Logger.Log(Rocket.Core.Environment.PluginsDirectory);
			playerInvs = JsonManager.Deseralize();
        }
		protected override void Unload()
		{
			JsonManager.Seralize(playerInvs);
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

		void Start()
		{
			Logger.LogWarning("\tPlugin Loadout Loaded Successfully");//Load is before the gameobject is made here is A TRUE comment

		}
    }
}