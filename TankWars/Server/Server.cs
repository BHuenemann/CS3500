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
    class Server
    {
        private World theWorld;

        private int UniverseSize;
        private int MSPerFrame;
        private int FramesPerShot;
        private int RespawnRate;


        static void Main(string[] args)
        {
            Networking.StartServer(SendStartupInfo, 11000);
        }


        private static void SendStartupInfo(SocketState obj)
        {
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
                                case "test":
                                    break;

                            }
                        }
                    }
                }
            }

            catch
            {

            }

        }
    }
}
