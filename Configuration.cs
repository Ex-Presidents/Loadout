using Rocket.API;
using System.Collections.Generic;
using System.Xml.Serialization;

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
        public int ItemLimit;
        [XmlArrayItem(ElementName = "Item")]
        public List<ushort> ItemBlacklist;
        public bool DenyOnBlacklist;

        public void LoadDefaults()
        {
            DatabasePort = 3306;
            DatabaseAddress = "localhost";
            DatabaseName = "Unturned";
            DatabaseUsername = "unturned";
            DatabasePassword = "";
            DebugMode = true;
            ItemLimit = 100;
            ItemBlacklist = new List<ushort>()
            {
                520,
                1394
            };
            DenyOnBlacklist = false;
        }
    }
}
