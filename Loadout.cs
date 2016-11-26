using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Rocket.Core.Plugins;
using Rocket.API;
using Rocket.Core;
using Rocket.Core.Logging;
using Rocket.Unturned.Player;

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
    }
}
