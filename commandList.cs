using System;
using Rocket.API;
using Rocket.Unturned.Player;
using System.Collections.Generic;
using Rocket.Unturned.Chat;

namespace Loadout
{
    public class commandList : IRocketCommand
    {
        public AllowedCaller AllowedCaller
        {
            get { return AllowedCaller.Player; }
        }

        public string Name
        {
            get { return "ĺistkit"; }
        }

        public string Help
        {
            get { return "This command will list your kits."; }
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
            if (!Loadout.instance.inventories.ContainsKey(player.CSteamID))
            {
                UnturnedChat.Say(player, Loadout.instance.Translate("no_kits"));
                return;
            }

			UnturnedChat.Say(player, Loadout.instance.Translate("listkit") + Loadout.instance.inventories[player.CSteamID].Keys);
        }

        public List<string> Permissions
        {
            get
            {
                return new List<string>
                {
                    "loadout.listkit"
                };
            }
        }
    }
}