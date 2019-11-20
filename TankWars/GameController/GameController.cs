using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using NetworkUtil;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace TankWars
{

    public class GameController
    {
        public World TheWorld
        {
            get;
            private set;
        }

        public Tank clientTank;
        public ControlCommands commands;
        private string tankName;

        public delegate void ConnectEventHandler(bool errorOccurred = false, string errorMessage = "");
        public event ConnectEventHandler OnConnectEvent;

        public delegate void OnFrameHandler(bool errorOccurred = false, string errorMessage = "");
        public event OnFrameHandler OnFrameEvent;

        private bool wallsDone = false;


        public GameController()
        {
            TheWorld = new World();
        }

        public void TryConnect(string name, string server, int port)
        {
            if (name.Length <= 16)
                tankName = name;
            else
                OnConnectEvent(true, "Name is longer than 16 characters");

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
                OnConnectEvent(true, "Unable to connect to server");
                ss.TheSocket.Close();
                return;
            }

            if (!Networking.Send(ss.TheSocket, tankName + "\n")) {
                OnConnectEvent(true, "Couldn't send player name since socket was closed");
                ss.TheSocket.Close();
                return;
            }

        ss.OnNetworkAction = ReceiveStartingData;
            Networking.GetData(ss);
        }

        private void ReceiveStartingData(SocketState ss)
        {
            if (ss.ErrorOccured == true)
            {
                OnConnectEvent(true, "Unable to receive tank ID and world size");
                ss.TheSocket.Close();
                return;
            }

            string[] startingInfo = Regex.Split(ss.GetData(), @"\n");

            clientTank = new Tank(tankName, Int32.Parse(startingInfo[0]));
            TheWorld.worldSize = Int32.Parse(startingInfo[1]);

            ss.ClearData();

            OnConnectEvent(false);

            ss.OnNetworkAction = ReceiveFrameData;
            Networking.GetData(ss);
        }

        private void ReceiveFrameData(SocketState ss)
        {
            if (ss.ErrorOccured == true)
            {
 //               OnFrameEvent(true, "Unable to receive tank ID and world size");
                ss.TheSocket.Close();
                return;
            }

            ProcessData(ss);

//            OnFrameEvent();

            if (wallsDone)
                Networking.Send(ss.TheSocket, JsonConvert.SerializeObject(commands));

            Networking.GetData(ss);
        }

        private void ProcessData(SocketState ss)
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

                UpdateObject(p.Substring(0, p.Length - 1));

                // Then remove it from the SocketState's growable buffer
                ss.RemoveData(0, p.Length);
            }
        }

        private void UpdateObject(string serializedObject)
        {
            JObject obj = JObject.Parse(serializedObject);

            JToken token = obj["tank"];
            if (token != null)
            {
                Tank tank = JsonConvert.DeserializeObject<Tank>(serializedObject);
                lock(TheWorld.Tanks)
                {
                    TheWorld.Tanks[tank.ID] = tank;
                }
                wallsDone = true;
                return;
            }

            token = obj["proj"];
            if (token != null)
            {
                Projectile proj = JsonConvert.DeserializeObject<Projectile>(serializedObject);
                lock (TheWorld.Projectiles)
                {
                    TheWorld.Projectiles[proj.ID] = proj;
                }
                wallsDone = true;
                return;
            }

            token = obj["power"];
            if (token != null)
            {
                PowerUp power = JsonConvert.DeserializeObject<PowerUp>(serializedObject);
                lock (TheWorld.PowerUps)
                {
                    TheWorld.PowerUps[power.ID] = power;
                }
                wallsDone = true;
                return;
            }

            token = obj["beam"];
            if (token != null)
            {
                Beam beam = JsonConvert.DeserializeObject<Beam>(serializedObject);
                lock (TheWorld.Beams)
                {
                    TheWorld.Beams[beam.ID] = beam;
                }
                wallsDone = true;
                return;
            }

            token = obj["wall"];
            if (token != null)
            {
                Wall wall = JsonConvert.DeserializeObject<Wall>(serializedObject);
                lock (TheWorld.Walls)
                {
                    TheWorld.Walls[wall.ID] = wall;
                }
                return;
            }
        }
    }
}
