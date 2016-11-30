using System;
using Rocket.API;
using Rocket.Unturned.Player;
using System.Collections.Generic;
using Rocket.Unturned.Chat;

namespace Loadout
{
    public class commandDel : IRocketCommand
    {
        public AllowedCaller AllowedCaller
        {
            get { return AllowedCaller.Player; }
        }

        public string Name
        {
            get { return "delkit"; }
        }

        public string Help
        {
            get { return "This command will delete your kit."; }
        }

        public string Syntax
        {
            get { return "<Kit Name>"; }
        }

        public List<string> Aliases
        {
            get { return new List<string>(); }
        }

        public void Execute(IRocketPlayer caller, string[] command)
        {
            UnturnedPlayer player = (UnturnedPlayer)caller;
            if (!Loadout.instance.inventories.ContainsKey(player.CSteamID))
            {
                UnturnedChat.Say(player, Loadout.instance.Translate("no_kits"));
                return;
            }

			if (command.Length != 1)
			{
				UnturnedChat.Say(player, Loadout.instance.Translate("syntax_del"));
				return;
			}

			String kitName = command[0].ToLower();

			if (!Loadout.instance.inventories[player.CSteamID].ContainsKey(kitName))
			{
				UnturnedChat.Say(player, Loadout.instance.Translate("no_kit"));
				return;
			}

			Loadout.instance.inventories[player.CSteamID].Remove(kitName);

			UnturnedChat.Say(player, Loadout.instance.Translate("deleted"));
        }

        public List<string> Permissions
        {
            get
            {
                return new List<string>
                {
                    "loadout.delkit"
                };

            }
        }
    }
}