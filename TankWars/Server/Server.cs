using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using NetworkUtil;
using TankWars;

namespace Server
{
    public class Server
    {
        private World theWorld = new World();

        private int UniverseSize;
        private int MSPerFrame;
        private int FramesPerShot;
        private int RespawnRate;


        public static void Main(string[] args)
        {
            Networking.StartServer(ReceivePlayerName, 11000);
        }


        private void ReceivePlayerName(SocketState ss)
        {
            ss.OnNetworkAction = SendStartupInfo;
            Networking.GetData(ss);
        }


        private void SendStartupInfo(SocketState ss)
        {
            string tankName = ss.GetData();
            int tankID = (int)ss.ID;

            Tank t = new Tank(tankName.Substring(0, tankName.Length - 2), tankID);
            theWorld.Tanks[tankID] = t;

            throw new NotImplementedException();
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
