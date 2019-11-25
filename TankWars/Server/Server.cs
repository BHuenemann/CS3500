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


        static void Main(string[] args)
        {
            Networking.StartServer(ReceivePlayerName, 11000);
        }


        private static void ReceivePlayerName(SocketState ss)
        {
            ss.OnNetworkAction = SendStartupInfo;
            Networking.GetData(ss);
        }


        private static void SendStartupInfo(SocketState ss)
        {
            ss.GetData()
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
                                    break;

                                case "MSPerFrame":
                                    break;

                                case "FramesPerShot":
                                    break;

                                case "RespawnRate":
                                    break;

                                case "Wall":
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
