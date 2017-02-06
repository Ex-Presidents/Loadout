using System;
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
                connection = new MySqlConnection(String.Format("SERVER={0};DATABASE={1};UID={2};PASSWORD={3};PORT={4};", Loadout.Instance.Configuration.Instance.DatabaseAddress, Loadout.Instance.Configuration.Instance.DatabaseName, Loadout.Instance.Configuration.Instance.DatabaseUsername, Loadout.Instance.Configuration.Instance.DatabasePassword, Loadout.Instance.Configuration.Instance.DatabasePort));
            }
            catch (Exception ex) { Logger.LogException(ex); }

            return connection;
        }

        public void CheckSchema()
        {
            try
            {
                using (MySqlConnection connection = CreateConnection())
                {
                    using(MySqlCommand command = connection.CreateCommand())
                    {
                        command.CommandText = "show tables like 'loadouts'";
                        connection.Open();

                        if (command.ExecuteScalar() == null)
                        {
                            command.CommandText = "CREATE TABLE `loadouts`" +
                                " (`id` INT NOT NULL AUTO INCREMENT," +
                                "`name` varchar(15) NOT NULL," +
                                "`steamId` varchar(32) NOT NULL);";
                            command.ExecuteNonQuery();
                        }
                        command.CommandText = "show tables like 'loadoutclothing'";

                        if(command.ExecuteScalar() == null)
                        {
                            command.CommandText = "CREATE TABLE `loadoutclothing`" +
                                " (`id` INT NOT NULL," +
                                "`type` varchar(8) NOT NULL," +
                                "`kitid` INT NOT NULL," +
                                "`quality` INT NOT NULL," +
                                "`state` INT NOT NULL);";
                            command.ExecuteNonQuery();
                        }
                        command.CommandText = "show tables like 'loadoutitems'";

                        if(command.ExecuteScalar() == null)
                        {
                            command.CommandText = "CREATE TABLE `loadoutitems`" +
                                " (`id` INT NOT NULL," +
                                "`kitid` INT NOT NULL," +
                                "`meta` INT NOT NULL);";
                            command.ExecuteNonQuery();
                        }
                        connection.Close();
                    }
                }
            }
            catch (Exception ex) { Logger.LogException(ex);  }
        }

        public void SaveLoadouts(ulong steamID, string name) // unfinished
        {
            try
            {
                using (MySqlConnection Connection = CreateConnection())
                {
                    using (MySqlCommand Cmd = Connection.CreateCommand())
                    {
                        Cmd.CommandText = "Insert into loadouts " +
                            "(id, name, steamid) " +
                            "values " +
                            "(1, 'cuckoo', 2);";
                        Connection.Open();
                        Cmd.ExecuteNonQuery();
                        Connection.Close();
                    }
                }
            }
            catch(Exception ex){ Logger.LogException(ex); }
        }

        public void LoadLoadouts(ulong steamID) // unfinished
        {
            try
            {
                using (MySqlConnection Connection = CreateConnection())
                {
                    using (MySqlCommand Cmd = Connection.CreateCommand())
                    {
                        Connection.Open();
                        Cmd.CommandText = "Select ";
                    }
                }
            }
            catch (Exception ex){ Logger.LogException(ex); }
        }
    }
}
