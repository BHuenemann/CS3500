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

                    command.CommandText = "insert into Games(Duration) values (\"" + TheWorld.Duration.Elapsed + "\");";
                    command.ExecuteNonQuery();

                    int gID = 0;
                    command.CommandText = "select gID from Games;";
                    using (MySqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                            gID = (int)reader["gID"];
                    }

                    foreach (Tank t in TheWorld.Players.Values)
                    {
                        command.CommandText += "insert into Players values (\"" + t.ID + "\", \"" + t.Name + "\");";

                        int Accuracy = 100 * (t.ShotsHit / t.ShotsFired);
                        command.CommandText += "insert into GamesPlayed values (\"" + gID + "\", \"" + t.ID + "\", \"" + t.Score + "\", \"" + Accuracy  + "\");";
                    }

                    command.ExecuteNonQuery();
                }
                catch(Exception e)
                {
                    Console.WriteLine(e.Message);
                    Console.ReadLine();
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

                    command.CommandText = "Select * from GamesPlayed join Players";

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

        public static Dictionary<uint, PlayerModel> GetAllPlayerGames()
        {
            using (MySqlConnection conn = new MySqlConnection(connectionString))
            {
                try
                {
                    conn.Open();

                    Dictionary<uint, PlayerModel> Players = new Dictionary<uint, PlayerModel>();

                    MySqlCommand command = conn.CreateCommand();
                    command.CommandText = "Select * from GamesPlayed join Players";

                    using (MySqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            PlayerModel player = new PlayerModel((string)reader["Name"], (uint)reader["Score"], (uint)reader["Accuracy"]);
                            Players.Add((uint)reader["pID"], player);
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
    }
}
