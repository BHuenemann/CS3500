using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TankWars;

namespace Server
{
    public class DatabaseController
    {
        public Dictionary<int, GameModel> AllGames = new Dictionary<int, GameModel>();
        public Dictionary<int, PlayerModel> PlayerGames = new Dictionary<int, PlayerModel>();


        public const string connectionString = "server=atr.eng.utah.edu;" +
            "database=TankWars;" +
            "uid=u1190338;" +
            "password=AlanTuring";

        public static void SaveGameToDatabase(World TheWorld)
        {
            using (MySqlConnection conn = new MySqlConnection(connectionString))
            {
                try
                {
                    conn.Open();

                    MySqlCommand command = conn.CreateCommand();
                    command.CommandText = "";

                    using(MySqlDataReader reader = command.ExecuteReader())
                    {
                        while(reader.Read())
                        {
                            //Console write line
                        }
                    }
                }
                catch
                {

                }
            }
                throw new NotImplementedException();
        }

        public static void GetAllGames()
        {
            //Get info from the server for info needed and iterate thru the info in Player games dictionary

            using (MySqlConnection conn = new MySqlConnection(connectionString))
            {
                try
                {
                    conn.Open();

                    MySqlCommand command = conn.CreateCommand();
                    command.CommandText = "";

                    using (MySqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            //Console write line
                        }
                    }
                }
                catch
                {

                }
            }
            throw new NotImplementedException();

        }

        public static void GetAllPlayerGames()
        {
            //select all this stuff from table for player info

            using (MySqlConnection conn = new MySqlConnection(connectionString))
            {
                try
                {
                    conn.Open();

                    MySqlCommand command = conn.CreateCommand();
                    command.CommandText = "";

                    using (MySqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            //Console write line
                        }
                    }
                }
                catch
                {

                }
            }
            throw new NotImplementedException();
        }
    }
}
