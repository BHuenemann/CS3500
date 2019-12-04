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
            }
                throw new NotImplementedException();
        }
    }
}
