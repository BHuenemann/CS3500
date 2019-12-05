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
            "database=cs3500_u1190338;" +
            "uid=cs3500_u1190338;" +
            "password=AlanTuring";

        public static void SaveGameToDatabase(World TheWorld)
        {
            using (MySqlConnection conn = new MySqlConnection(connectionString))
            {
                try
                {
                    conn.Open();

                    MySqlCommand command = conn.CreateCommand();

                    command.CommandText = "insert into Games(Duration) values (\"" + TheWorld.Duration.Elapsed.Seconds + "\");";
                    command.ExecuteNonQuery();

                    UInt64 gID = 0;
                    command.CommandText = "select LAST_INSERT_ID();";
                    using (MySqlDataReader reader = command.ExecuteReader())
                    {
                        reader.Read();
                        gID = (UInt64)reader["LAST_INSERT_ID()"];
                    }

                    foreach (Tank t in TheWorld.Players.Values)
                    {
                        command.CommandText = "insert into Players(Name) values (\"" + t.Name + "\");";
                        command.ExecuteNonQuery();


                        int Accuracy;
                        if (t.ShotsFired != 0)
                            Accuracy = (int)(100 * (((float)t.ShotsHit) / ((float)t.ShotsFired)));
                        else
                            Accuracy = 100;

                        command.CommandText = "insert into GamesPlayed(gID,Score,Accuracy) values (\"" + gID + "\", \"" + t.Score + "\", \"" + Accuracy  + "\");";
                        command.ExecuteNonQuery();
                    }

                }
                catch(Exception e)
                {
                    Console.WriteLine(e.Message);
                }
            }
        }

        public static Dictionary<uint, GameModel> GetAllGames()
        {
            using (MySqlConnection conn = new MySqlConnection(connectionString))
            {
                try
                {
                    conn.Open();

                    Dictionary<uint, GameModel> Games = new Dictionary<uint, GameModel>();

                    MySqlCommand command = conn.CreateCommand();
                    command.CommandText = "Select * from Games;";

                    using (MySqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            GameModel game = new GameModel((uint)reader["gID"], (uint)reader["Duration"]);
                            Games.Add((uint)reader["gID"], game);
                        }
                    }

                    command.CommandText = "Select * from GamesPlayed natural join Players";

                    using (MySqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            Games[(uint)reader["gID"]].AddPlayer((string)reader["Name"], (uint)reader["Score"], (uint)reader["Accuracy"]);
                        }
                    }
                    return Games;
                }
                catch(Exception e)
                {
                    Console.WriteLine(e.Message);
                    return new Dictionary<uint, GameModel>();
                }
            }
        }

        public static Dictionary<uint, PlayerModel> GetAllPlayerGames(string PlayerName)
        {
            using (MySqlConnection conn = new MySqlConnection(connectionString))
            {
                try
                {
                    conn.Open();

                    Dictionary<uint, PlayerModel> Players = new Dictionary<uint, PlayerModel>();

                    MySqlCommand command = conn.CreateCommand();
                    command.CommandText = "Select * from GamesPlayed natural join Players where Name = \'" + PlayerName + "\';";

                    using (MySqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            PlayerModel player = new PlayerModel((string)reader["Name"], (uint)reader["Score"], (uint)reader["Accuracy"]);
                            if(!Players.ContainsKey((uint)reader["gID"]))
                                Players.Add((uint)reader["gID"], player);
                        }
                    }
                    return Players;
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                    return new Dictionary<uint, PlayerModel>();
                }
            }
        }


        public static uint GetGameDuration(uint gID)
        {
            using (MySqlConnection conn = new MySqlConnection(connectionString))
            {
                try
                {
                    conn.Open();

                    MySqlCommand command = conn.CreateCommand();
                    command.CommandText = "Select Duration from Games where gID = \'" + gID + "\';";

                    using (MySqlDataReader reader = command.ExecuteReader())
                    {
                        reader.Read();
                        return (uint)reader["Duration"];
                    }
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                    return 0;
                }
            }
        }
    }
}
