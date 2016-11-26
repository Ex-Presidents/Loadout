using System;
using Rocket.API;
using Rocket.Unturned.Player;
using System.Collections.Generic;
using SDG.Unturned;
using Rocket.Core.Logging;
using Steamworks;
using Rocket.Unturned.Chat;

namespace Loadout
{
	public class commandSave : IRocketCommand
	{
		public AllowedCaller AllowedCaller
		{
			get { return AllowedCaller.Player; }
		}

		public string Name
		{
			get { return "savekit"; }
		}

		public string Help
		{
			get { return "This command will save your current gear, you can load it by typing /loadkit"; }
		}

		public string Syntax
		{
			get { return ""; }
		}

		public List<string> Aliases
		{
			get { return new List<string>(); }
		}

		public void Execute(IRocketPlayer caller, string[] command)
		{
			UnturnedPlayer player = (UnturnedPlayer)caller;
			//SAVING INVENTORY :3
				//SAVING ITEMS :3
				List<LoadoutItem> itemList = new List<LoadoutItem>();
				for (byte p = 0; p < PlayerInventory.PAGES - 1; p++)
				{
					for (byte i = 0; i < player.Inventory.getItemCount(p); i++)
					{
						Item item = player.Inventory.getItem(p, i).item;
						itemList.Add(new LoadoutItem(item.id, item.metadata));
					}
				}

				if (Loadout.instance.inventories.ContainsKey(player.CSteamID))
				{
					Loadout.instance.inventories.Remove(player.CSteamID);
				}
				//END
				
				//Saving clothing :3
				PlayerClothing clo = player.Player.clothing;

				LoadoutHat hat = new LoadoutHat(clo.hat, clo.hatQuality, clo.hatState);
				LoadoutMask mask = new LoadoutMask(clo.mask, clo.maskQuality, clo.maskState);
				LoadoutShirt shirt = new LoadoutShirt(clo.shirt, clo.shirtQuality, clo.shirtState);
				LoadoutVest vest = new LoadoutVest(clo.vest, clo.vestQuality, clo.vestState);
				LoadoutBackpack backpack = new LoadoutBackpack(clo.backpack, clo.backpackQuality, clo.backpackState);
				LoadoutPants pants = new LoadoutPants(clo.pants, clo.pantsQuality, clo.pantsState);

				Loadout.instance.inventories.Add(player.CSteamID, new LoadoutInventory(itemList, new LoadoutClothes(hat, mask, shirt, vest, backpack, pants)));
				//END
			//END

			UnturnedChat.Say(player, Loadout.instance.Translate("saved"));
		}

		public List<string> Permissions
		{
			get
			{
				return new List<string>
				{
					"loadout.use"
				};

			}
		}
	}
}
