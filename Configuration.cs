using Rocket.API;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Loadout
{
    public class Configuration : IRocketPluginConfiguration
    {
        public int DatabasePort;
        public string DatabaseName;
        public string DatabaseTableName;
        public string DatabaseAddress;
        public string DatabaseUsername;
        public string DatabasePassword;

        public void LoadDefaults()
        {
            DatabasePort = 3306;
            DatabaseAddress = "localhost";
            DatabaseName = "Unturned";
            DatabaseTableName = "Loadout";
            DatabaseUsername = "admin";
            DatabasePassword = "admin";
        }
    }
}
