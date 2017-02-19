using System;
using MySql.Data.MySqlClient;

namespace MySQLExperimentation
{
    public class DBManager
    {
        #region Schemas

        public DBManager() { CheckSchema(); }

        private void CheckSchema()
        {
            try
            {
                MySqlConnection Connection = CreateConnection();
                using (MySqlCommand command = Connection.CreateCommand())
                {
                    Connection.Open();
                    CheckLoadoutClothing(Connection, command);
                    CheckLoadoutItems(Connection, command);
                    Connection.Close();
                }
            }
            catch (Exception ex) { Console.WriteLine(ex); }
        }

        private void CheckLoadoutClothing(MySqlConnection Connection, MySqlCommand Command)
        {
            Command.CommandText = "show tables like 'loadoutclothing'";

            if (Command.ExecuteScalar() == null)
            {
                Command.CommandText = "CREATE TABLE `loadoutclothing`" +
                    " (`id` INT NOT NULL," +
                    "`type` varchar(11) NOT NULL," +
                    "`kitname` varchar(255) NOT NULL," +
                    "`ownerid` varchar(32) NOT NULL," +
                    "`quality` INT NOT NULL," +
                    "`state` INT NOT NULL);";
                Command.ExecuteNonQuery();
            }
        }

        private void CheckLoadoutItems(MySqlConnection Connection, MySqlCommand Command)
        {
            Command.CommandText = "show tables like 'loadoutitems'";

            if (Command.ExecuteScalar() == null)
            {
                Command.CommandText = "CREATE TABLE `loadoutitems`" +
                    " (`id` INT NOT NULL," +
                    "`kitname` varchar(255) NOT NULL," +
                    "`ownerid` varchar(32) NOT NULL," +
                    "`meta` INT NOT NULL);";
                Command.ExecuteNonQuery();
            }
        }

        #endregion Schemas

        #region Commands

        private MySqlCommand SaveItemsCommand(MySqlConnection Connection, string steamID, byte id, string name, byte[] meta)
        {
            MySqlCommand Cmd = Connection.CreateCommand();
            Cmd.Parameters.AddWithValue("@id", id);
            Cmd.Parameters.AddWithValue("@kitname", name);
            Cmd.Parameters.AddWithValue("@ownerid", steamID);
            Cmd.Parameters.AddWithValue("@meta", 3);
            Cmd.CommandText = "Insert into loadoutitems " +
                "(id, kitname, ownerid, meta) " +
                "values " +
                "(@id, @kitname, @ownerid, @meta);";
            return Cmd;
        }

        private MySqlCommand SaveClothingCommand(MySqlConnection Connection, string steamid, string name, byte id, string type, int quality, int state)
        {
            MySqlCommand Cmd = Connection.CreateCommand();
            Cmd.Parameters.AddWithValue("@id", id);
            Cmd.Parameters.AddWithValue("@type", type);
            Cmd.Parameters.AddWithValue("@kitname", name);
            Cmd.Parameters.AddWithValue("@ownerid", steamid);
            Cmd.Parameters.AddWithValue("@quality", quality);
            Cmd.Parameters.AddWithValue("@state", state);
            Cmd.CommandText = "Insert into loadoutclothing " +
                "(id, type, kitname, ownerid, quality, state) " +
                "values " +
                "(@id, @type, @kitname, @ownerid, @quality, @state);";
            return Cmd;
        }

        #endregion Commands

        private MySqlConnection CreateConnection()
        {
            MySqlConnection connection = null;
            try
            {
                connection = new MySqlConnection(String.Format("SERVER={0};DATABASE={1};UID={2};PASSWORD={3};PORT={4};", "localhost", "unturned", "root", "helloworld", 3306));
            }
            catch (Exception ex) { Console.WriteLine(ex); }

            return connection;
        }

        public void SaveLoadouts(MySqlConnection Connection, string steamID, byte id, string type, string name, int quality, int state, byte[] meta)
        {
            try
            {
                Connection.Open();
                using (MySqlCommand Cmd = SaveItemsCommand(Connection, steamID, id, name, meta))
                    Cmd.ExecuteNonQuery();
                using (MySqlCommand Cmd = SaveClothingCommand(Connection, steamID, name, id, type, quality, state))
                    Cmd.ExecuteNonQuery();
                Connection.Close();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
        }

        public void LoadLoadouts(MySqlConnection Connection, string steamID)
        {
            try
            {
                using (MySqlCommand Cmd = Connection.CreateCommand())
                {
                    Connection.Open();
                    Cmd.CommandText = "Select * from loadoutitems where `ownerid` = '" + steamID + "';";
                    object Result = Cmd.ExecuteNonQuery();
                    MySqlDataReader Reader = Cmd.ExecuteReader();
                    if (Reader.HasRows)
                    {
                        if (Reader.Read())
                        {
                            // do something 
                        }
                    }
                }
            }
            catch (Exception ex) { Console.WriteLine(ex); }
        }
    }
}
