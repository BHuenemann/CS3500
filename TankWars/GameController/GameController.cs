using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using NetworkUtil;

namespace TankWars
{

    public class GameController
    {
        public World TheWorld
        {
            get;
            private set;
        }
        private int worldSize;
        private Tank clientTank;

        public GameController()
        {
            TheWorld = new World();
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

            if(!Networking.Send(ss.TheSocket, clientTank.Name + @"\n"))
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

            string[] startingInfo = Regex.Split(ss.GetData(), @"(?<=[\n])");

            clientTank.ID = Int32.Parse(startingInfo[0]);
            worldSize = Int32.Parse(startingInfo[1]);

            ss.OnNetworkAction = ReceiveFrameData;

            Networking.GetData(ss);
        }

        private void ReceiveFrameData(SocketState ss)
        {
            string totalData = ss.GetData();
            string[] parts = Regex.Split(totalData, @"(?<=[\n])");

            foreach (string p in parts)
            {
                //This is to ignore empty strings
                if (p.Length == 0)
                    continue;
                //This is so it ignores the last string if it doesn't end in \n
                if (p[p.Length - 1] != '\n')
                    break;

                UpdateObject(p.Substring(0, p.Length - 2));

                // Then remove it from the SocketState's growable buffer
                ss.RemoveData(0, p.Length);
            }

            Networking.GetData(ss);
        }

        private void UpdateObject(string serializedObject)
        {
            throw new NotImplementedException();
        }
    }
}
