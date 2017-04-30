using Rocket.API;
using Rocket.Unturned.Chat;
using SDG.Unturned;
using System.Collections.Generic;

using Logger = Rocket.Core.Logging.Logger;

namespace ExPresidents.Loadout
{
    public class CommandStore : IRocketCommand
    {
        #region Properties

        public AllowedCaller AllowedCaller => AllowedCaller.Both;

        public string Name => "storekits";

        public string Help => "This command will store all player kits to a configurable mysql database";

        public string Syntax => "";

        public List<string> Aliases => new List<string>();

        public List<string> Permissions => new List<string> { "loadout.storekits" };

        #endregion Properties

        public void Execute(IRocketPlayer caller, string[] command)
        {
            if (command.Length != 0)
            {
                if (caller.Id == "Console")
                    Logger.Log(Loadout.Instance.Translate("syntax"));
                else
                    UnturnedChat.Say(caller, Loadout.Instance.Translate("syntax"));
            }
            if (Loadout.Instance.playerInvs == null)
            {
                if (caller.Id == "Console")
                    Logger.Log(Loadout.Instance.Translate("null"));
                else
                    UnturnedChat.Say(caller, Loadout.Instance.Translate("null"));
                return;
            }
            Loadout.Instance.DB.SaveDictionary(Loadout.Instance.Configuration.Instance.TableIndex);
        }
    }
}