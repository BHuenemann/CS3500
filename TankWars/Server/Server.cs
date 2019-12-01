using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using NetworkUtil;
using Newtonsoft.Json;
using TankWars;

namespace Server
{
    public class Server
    {
        private static World TheWorld = new World();
        private static HashSet<Socket> SocketConnections = new HashSet<Socket>();

        static private int UniverseSize = 1200;
        static private int MSPerFrame = 30;
        static private int FramesPerShot;
        static private int RespawnRate;


        public static void Main(string[] args)
        {
            Networking.StartServer(ReceivePlayerName, 11000);

            Stopwatch watch = new Stopwatch();

            while(true)
            {
                while(watch.ElapsedMilliseconds < MSPerFrame)
                {
                    //Do Nothing
                }
                watch.Reset();

                UpdateData();

                SendDataToSockets();
            }
        }


        private static void UpdateData()
        {

        }


        private static void SendDataToSockets()
        {
            foreach (Socket s in SocketConnections)
            {
                StringBuilder frameMessage = new StringBuilder();


                foreach (Tank t in TheWorld.Tanks.Values)
                    frameMessage.Append(JsonConvert.SerializeObject(t) + "\n");

                foreach (PowerUp p in TheWorld.PowerUps.Values)
                    frameMessage.Append(JsonConvert.SerializeObject(p) + "\n");

                foreach (Projectile p in TheWorld.Projectiles.Values)
                    frameMessage.Append(JsonConvert.SerializeObject(p) + "\n");

                foreach (Beam b in TheWorld.Beams.Values)
                    frameMessage.Append(JsonConvert.SerializeObject(b) + "\n");


                Networking.Send(s, frameMessage.ToString());
            }
        }


        private static void ReceivePlayerName(SocketState ss)
        {
            if (ss.ErrorOccured == true)
            {

            }
            ss.OnNetworkAction = SendStartupInfo;
            Networking.GetData(ss);
        }


        private static void SendStartupInfo(SocketState ss)
        {
            if (ss.ErrorOccured == true)
            {

            }
            string tankName = ss.GetData();
            int tankID = (int)ss.ID;

            Tank t = new Tank(tankName.Substring(0, tankName.Length - 2), tankID);
            TheWorld.Tanks[tankID] = t;

            string message = tankID + "\n" + UniverseSize.ToString() + "\n";
            Networking.Send(ss.TheSocket, message);

            StringBuilder wallMessage = new StringBuilder();
            foreach (Wall w in TheWorld.Walls.Values)
                wallMessage.Append(JsonConvert.SerializeObject(t) + "\n");
            Networking.Send(ss.TheSocket, wallMessage.ToString());

            SocketConnections.Add(ss.TheSocket);

            ss.OnNetworkAction = ReceiveCommandData;
            Networking.GetData(ss);
        }


        private static void ReceiveCommandData(SocketState ss)
        {
            if (ss.ErrorOccured == true)
            {

            }

            Networking.GetData(ss);
        }

        private void readSettingFile(string fileName)
        {
            try
            {
                // Create an XmlReader inside this block, and automatically Dispose() it at the end.
                using (XmlReader reader = XmlReader.Create(fileName))
                {
                    while (reader.Read())
                    {
                        if (reader.IsStartElement())
                        {
                            switch (reader.Name)
                            {
                                case "UniverseSize":
                                    reader.Read();
                                    UniverseSize = Int32.Parse(reader.Value);
                                    break;

                                case "MSPerFrame":
                                    reader.Read();
                                    MSPerFrame = Int32.Parse(reader.Value);
                                    break;

                                case "FramesPerShot":
                                    reader.Read();
                                    FramesPerShot = Int32.Parse(reader.Value);
                                    break;

                                case "RespawnRate":
                                    reader.Read();
                                    RespawnRate = Int32.Parse(reader.Value);
                                    break;

                                case "Wall":
                                    reader.Read();

                                    switch (reader.Name)
                                    {
                                        case "p1":
                                            reader.Read(); //gets x
                                            reader.Read(); //gets y
                                            break;

                                        case "p2":
                                            reader.Read(); //gets x
                                            reader.Read(); //gets y
                                            break;
                                    }
                                    break;
                            }
                        }
                    }
                }
            }

            catch
            {
                throw new Exception("There was a problem opening the saved file...");
            }

        }
    }
}
