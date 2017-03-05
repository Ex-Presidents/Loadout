﻿using System;
using System.Collections.Generic;
using MySql.Data.MySqlClient;

using Logger = Rocket.Core.Logging.Logger;

namespace ExPresidents.Loadout
{
    public class DBManager
    {
        bool DebugMode;

        #region Schemas

        public DBManager() { CheckSchema(); }

        private void CheckSchema()
        {
            MySqlConnection Connection = CreateConnection();
            if (DebugMode)
                Logger.Log("MySql connection created.");
            using (MySqlCommand Command = Connection.CreateCommand())
            {
                Connection.Open();

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

                Connection.Close();
            }
            Loadout.Instance.Connection = Connection;
            DebugMode = Loadout.Instance.Configuration.Instance.DebugMode;
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
            MySqlConnection connection = null;

            connection = new MySqlConnection(String.Format("SERVER={0};DATABASE={1};UID={2};PASSWORD={3};PORT={4};", Loadout.Instance.Configuration.Instance.DatabaseAddress, Loadout.Instance.Configuration.Instance.DatabaseName, Loadout.Instance.Configuration.Instance.DatabaseUsername, Loadout.Instance.Configuration.Instance.DatabasePassword, Loadout.Instance.Configuration.Instance.DatabasePort));

            return connection;
        }

        public void SaveDictionary(MySqlConnection Connection, String ServerName)
        {
            Dictionary<ulong, LoadoutList> Dictionary = Loadout.Instance.playerInvs;

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

        public void LoadDictionary(MySqlConnection Connection, String ServerName)
        {
            using(MySqlCommand Cmd = Connection.CreateCommand())
            {
                Connection.Open();
                Cmd.CommandText = "Select * from loadout where servername = " + ServerName + ";";
                object Result = Cmd.ExecuteNonQuery();
                MySqlDataReader Reader = Cmd.ExecuteReader();
                if (Reader.HasRows)
                {
                    if (Reader.Read())
                        Loadout.Instance.playerInvs = Reader.GetValue(1) as Dictionary<ulong, LoadoutList>;
                }
            }
        }
    }
}