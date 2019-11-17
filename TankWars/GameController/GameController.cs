using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using NetworkUtil;

namespace TankWars
{

    public class GameController
    {
        private World theWorld;

        private int worldSize;

        private Tank clientTank;

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
            if (name.Length <= 16)
            {
                clientTank = new Tank();
            }
            else
            {
                //Throw Error
            }

            Networking.ConnectToServer(SendName, server, port);

        }

        /// <summary>
        /// This is trash. Only here as backup.
        /// </summary>
        /// <param name="obj"></param>
        private void SendName(SocketState ss)
        {
            if(ss.ErrorOccured == true)
            {
                //Throw Error
            }

            if(!Networking.Send(ss.TheSocket, clientTank.Name + "\n"))
            {
                //Throw Error (Socket was closed)
            }

            ss.OnNetworkAction = ReceiveStartingData;
            Networking.GetData(ss);
        }

        private void ReceiveStartingData(SocketState ss)
        {
            if (ss.ErrorOccured == true)
            {
                //Throw Error
            }

            string[] startingInfo = Regex.Split(ss.GetData(), @"\n");

            clientTank.ID = Int32.Parse(startingInfo[0]);
            worldSize = Int32.Parse(startingInfo[1]);

            ss.OnNetworkAction = ReceiveFrameData;

            Networking.GetData(ss);
        }

        private void ReceiveFrameData(SocketState ss)
        {

        }
    }
}
