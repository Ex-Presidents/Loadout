using Rocket.API;

namespace ExPresidents.Loadout
{
    public class Configuration : IRocketPluginConfiguration
    {
        public int DatabasePort;
        public string DatabaseName;
        public string DatabaseAddress;
        public string DatabaseUsername;
        public string DatabasePassword;
        public bool DebugMode;

        public void LoadDefaults()
        {
            DatabasePort = 3306;
            DatabaseAddress = "localhost";
            DatabaseName = "Unturned";
            DatabaseUsername = "admin";
            DatabasePassword = "admin";
            DebugMode = true;
        }
    }
}
