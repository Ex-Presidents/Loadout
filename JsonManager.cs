using System;
using Steamworks;
using SDG.Unturned;
using System.Collections.Generic;
using System.IO;

namespace Loadout
{
	public class JsonManager
	{
		public static Dictionary<CSteamID, LoadoutList> Deseralize()
		{
			try
			{
				if (!ReadWrite.fileExists(Rocket.Core.Environment.PluginDirectory + "/loadout.json", false))
					return null;



				return ReadWrite.deserializeJSON<Dictionary<CSteamID, LoadoutList>>(Rocket.Core.Environment.PluginDirectory + "loadout.json", false);


			}
			catch { return null; }
		}

		public static void Seralize(Dictionary<CSteamID, LoadoutList> loadouts)
		{
			try
			{
				if (!ReadWrite.fileExists(Rocket.Core.Environment.PluginDirectory + "/loadout.json", false))
					File.AppendText(Rocket.Core.Environment.PluginDirectory + "/loadout.json");


				ReadWrite.serializeJSON(Rocket.Core.Environment.PluginDirectory + "/loadout.json", false, loadouts);
			}
			catch { }
		}
	}
}

