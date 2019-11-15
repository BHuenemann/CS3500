using System;
using System.Collections.Generic;
using NetworkUtil;

namespace TankWars
{

    public class GameController
    {
        private World theWorld;

        public const int worldSize = 800;

        List<Tank> TankList;

        public GameController()
        {
            theWorld = new World();
        }

        public World GetWorld()
        {
            return theWorld;
        }

        public void ConnectPlayer(string name, string server, int port)
        {
            Networking.ConnectToServer(SendName, server, port);
            //create tank with this name
        }

        /// <summary>
        /// This is trash. Only here as backup.
        /// </summary>
        /// <param name="obj"></param>
        private void SendName(SocketState ss)
        {
            Networking.Send(ss.TheSocket, "\n");
            throw new NotImplementedException();
        }
    }
}
