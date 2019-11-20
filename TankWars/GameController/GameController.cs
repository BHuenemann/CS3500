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
        private string tankName;

        public delegate void ConnectEventHandler(bool errorOccurred, string errorMessage = "");
        public event ConnectEventHandler OnConnectEvent;

        public delegate void OnFrameHandler();
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
                OnConnectEvent(true, "Unable to connect to server");

            if (!Networking.Send(ss.TheSocket, tankName + @"\n"))
                OnConnectEvent(true, "Couldn't send player name since socket was closed");

            ss.OnNetworkAction = ReceiveStartingData;
            Networking.GetData(ss);
        }

        private void ReceiveStartingData(SocketState ss)
        {
            if (ss.ErrorOccured == true)
                OnConnectEvent(true, "Unable to receive tank ID and world size");

            string[] startingInfo = Regex.Split(ss.GetData(), @"\n");

            clientTank = new Tank(tankName, Int32.Parse(startingInfo[0]));
            TheWorld.worldSize = Int32.Parse(startingInfo[1]);

            OnConnectEvent(false);

            ss.OnNetworkAction = ReceiveFrameData;
            Networking.GetData(ss);
        }

        private void ReceiveFrameData(SocketState ss)
        {
            ProcessData(ss);

            OnFrameEvent();

            if (wallsDone)
                Networking.Send(ss.TheSocket, SerializeObjects());

            Networking.GetData(ss);
        }

        private string SerializeObjects()
        {
            StringBuilder message = new StringBuilder();
            foreach (Tank tank in TheWorld.Tanks.Values)
            {
                
            }
            throw new NotImplementedException();
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

                UpdateObject(p.Substring(0, p.Length - 2));

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
                TheWorld.Tanks[tank.ID] = tank;
                wallsDone = true;
                return;
            }

            token = obj["proj"];
            if (token != null)
            {
                Projectile proj = JsonConvert.DeserializeObject<Projectile>(serializedObject);
                TheWorld.Projectiles[proj.ID] = proj;
                wallsDone = true;
                return;
            }

            token = obj["power"];
            if (token != null)
            {
                PowerUp power = JsonConvert.DeserializeObject<PowerUp>(serializedObject);
                TheWorld.PowerUps[power.ID] = power;
                wallsDone = true;
                return;
            }

            token = obj["beam"];
            if (token != null)
            {
                Beam beam = JsonConvert.DeserializeObject<Beam>(serializedObject);
                TheWorld.Beams[beam.ID] = beam;
                wallsDone = true;
                return;
            }

            token = obj["wall"];
            if (token != null)
            {
                Wall wall = JsonConvert.DeserializeObject<Wall>(serializedObject);
                TheWorld.Walls[wall.ID] = wall;
                return;
            }
        }
    }
}
