using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using Logger = Rocket.Core.Logging.Logger;

namespace ExPresidents.Loadout
{
    public class DBManager
    {
        private bool DebugMode;

        #region Schemas

        public DBManager()
        {
            CheckSchema();
        }

        private void CheckSchema()
        {
            using (MySqlConnection Connection = CreateConnection())
            {
                DebugMode = Loadout.Instance.Configuration.Instance.DebugMode;
                if (DebugMode)
                    Logger.Log("MySql connection created.");

                Connection.Open();

                using (MySqlCommand Command = Connection.CreateCommand())
                {
                    Command.CommandText = "show tables like 'loadout'";

                    if (Command.ExecuteScalar() == null)
                    {
                        Command.CommandText = "CREATE TABLE `loadout`" +
                            " (`servername` varchar(64) NOT NULL," +
                            "`dictionary` BLOB NOT NULL);";
                        Command.ExecuteNonQuery();
                        if (DebugMode)
                            Logger.Log("No loadout table found, created one.");
                    }
                    else
                        if (DebugMode)
                        Logger.Log("Loadout table found.");
                }
                Connection.Close();
            }
        }

        #endregion Schemas

        #region Commands

        private MySqlCommand SaveDictionaryCommand(MySqlConnection Connection, String ServerName)
        {
            MySqlCommand Cmd = Connection.CreateCommand();
            Cmd.Parameters.AddWithValue("@servername", ServerName);
            Cmd.Parameters.AddWithValue("@dictionary", BArrayManager.ToArray(Loadout.Instance.playerInvs));
            Cmd.CommandText = "Insert into loadout " +
                "(servername, dictionary) " +
                "values " +
                "(@servername, @dictionary);";
            return Cmd;
        }

        private MySqlCommand UpdateDictionaryCommand(MySqlConnection Connection, String ServerName)
        {
            MySqlCommand Cmd = Connection.CreateCommand();
            Cmd.Parameters.AddWithValue("@servername", ServerName);
            Cmd.Parameters.AddWithValue("@dictionary", BArrayManager.ToArray(Loadout.Instance.playerInvs));
            Cmd.CommandText = "Update loadout set dictionary = @dictionary where servername = @servername";
            return Cmd;
        }

        #endregion Commands

        private MySqlConnection CreateConnection()
        {
            Configuration config = Loadout.Instance.Configuration.Instance;

            return new MySqlConnection(String.Format("SERVER={0};DATABASE={1};UID={2};PASSWORD={3};PORT={4};", config.DatabaseAddress, config.DatabaseName, config.DatabaseUsername, config.DatabasePassword, config.DatabasePort));
        }

        public void SaveDictionary(String ServerName)
        {
            using (MySqlConnection Connection = CreateConnection())
            {
                Connection.Open();
                using (MySqlCommand Cmd = UpdateDictionaryCommand(Connection, ServerName))
                {
                    if (Cmd.ExecuteNonQuery() == 0)
                    {
                        using (MySqlCommand Command = SaveDictionaryCommand(Connection, ServerName))
                            Command.ExecuteNonQuery();
                        if (DebugMode)
                            Logger.Log("No dictionary for " + ServerName + " found, creating one.");
                    }
                    Logger.Log("Dictionary saved for " + ServerName + ".");
                }
                Connection.Close();
            }
        }

        public void LoadDictionary(String ServerName)
        {
            using (MySqlConnection Connection = CreateConnection())
            {
                Connection.Open();
                using (MySqlCommand Cmd = Connection.CreateCommand())
                {
                    Cmd.CommandText = "Select * from loadout where servername = " + ServerName + ";";
                    object Result = Cmd.ExecuteNonQuery();
                    using (MySqlDataReader Reader = Cmd.ExecuteReader())
                    {
                        if (Reader.HasRows)
                        {
                            if (Reader.Read())
                                Loadout.Instance.playerInvs = BArrayManager.ToObject((byte[])Reader.GetValue(1)); 
                        }
                        Reader.Close();
                    }
                }
                Connection.Close();
            }
        }

        public bool CheckDictionary(String ServerName)
        {
            using (MySqlConnection Connection = CreateConnection())
            {
                bool retval;
                Connection.Open();
                using (MySqlCommand Cmd = Connection.CreateCommand())
                {
                    Cmd.CommandText = "Select * from loadout where servername = " + ServerName + ";";
                    object Result = Cmd.ExecuteNonQuery();
                    using (MySqlDataReader Reader = Cmd.ExecuteReader())
                    {
                        if (!Reader.HasRows)
                            retval = false;
                        else
                            retval = true;
                        Reader.Close();
                    }
                }
                Connection.Close();
                return retval;
            }
        }
    }
}