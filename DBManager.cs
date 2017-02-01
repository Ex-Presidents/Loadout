using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MySql.Data.MySqlClient;

using Logger = Rocket.Core.Logging.Logger;

namespace Loadout
{
    public class DBManager
    {
        public DBManager() { CheckSchema(); }

        private MySqlConnection CreateConnection()
        {
            MySqlConnection connection = null;
            try
            {
                if (Loadout.Instance.Configuration.Instance.DatabasePort == 0) Loadout.Instance.Configuration.Instance.DatabasePort = 3306;
                connection = new MySqlConnection(String.Format("SERVER={0};DATABASE={1};UID={2};PASSWORD={3};PORT={4};", Loadout.Instance.Configuration.Instance.DatabaseAddress, Loadout.Instance.Configuration.Instance.DatabaseName, Loadout.Instance.Configuration.Instance.DatabaseUsername, Loadout.Instance.Configuration.Instance.DatabasePassword, Loadout.Instance.Configuration.Instance.DatabasePort));
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
            }
            return connection;
        }

        public void CheckSchema()
        {
            try
            {
                using (MySqlConnection connection = CreateConnection())
                {
                  using (MySqlCommand command = connection.CreateCommand())
                  {
                      command.CommandText = "show tables like '" + Loadout.Instance.Configuration.Instance.DatabaseTableName + "'";
                      connection.Open();

                      if (command.ExecuteScalar() == null)
                      {
                          command.CommandText = "CREATE TABLE `"
                              + Loadout.Instance.Configuration.Instance.DatabaseTableName +
                              "` (`id` int(11) NOT NULL AUTO_INCREMENT," +
                              "`steamId` varchar(32) NOT NULL," +
                              "PRIMARY KEY (`id`));";
                              command.ExecuteNonQuery();
                      }
                      connection.Close();
                      }
                    }
                  }
            catch (Exception ex)
            {
                Logger.LogException(ex);
            }
        }
    }
}
