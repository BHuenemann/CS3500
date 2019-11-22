using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using System.Timers;
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

        public ControlCommands commands;

        public delegate void ErrorHandler(string errorMessage = "");
        public event ErrorHandler ErrorEvent;
        public event ErrorHandler NameErrorEvent;

        public delegate void OnActionHandler();
        public event OnActionHandler OnFrameEvent;

        public bool wallsDone = false;

        private Dictionary<int, int> TankColorRecord = new Dictionary<int, int>();
        private int SeenPlayers = 0;
        private string tankName;
        private int tankID;

        private bool upKey = false;
        private bool downKey = false;
        private bool leftKey = false;
        private bool rightKey = false;



        public GameController()
        {
            TheWorld = new World();
            commands = new ControlCommands();
        }

        public Tank GetPlayerTank()
        {
            return TheWorld.Tanks[tankID];
        }


        public void ProcessKeyDown(Keys key)
        {
            CalculateMovement();
            switch (key)
            {
                case Keys.W:
                    upKey = true;
                    commands.direction = "up";
                    break;
                case Keys.S:
                    downKey = true;
                    commands.direction = "down";
                    break;
                case Keys.A:
                    leftKey = true;
                    commands.direction = "left";
                    break;
                case Keys.D:
                    rightKey = true;
                    commands.direction = "right";
                    break;
            }
        }


        public void ProcessKeyUp(Keys key)
        {
            switch (key)
            {
                case Keys.W:
                    upKey = false;
                    commands.direction = "none";
                    break;
                case Keys.S:
                    downKey = false;
                    commands.direction = "none";
                    break;
                case Keys.A:
                    leftKey = false;
                    commands.direction = "none";
                    break;
                case Keys.D:
                    rightKey = false;
                    commands.direction = "none";
                    break;
            }
            CalculateMovement();
        }



        private void CalculateMovement()
        {
            if (upKey != downKey)
                commands.direction = (upKey) ? "up" : "down";
            if (leftKey != rightKey)
                commands.direction = (leftKey) ? "left" : "right";
        }


        public void ProcessMouseDown(MouseButtons button)
        {
            if (button.Equals(MouseButtons.Left))
            {
                commands.fire = "main";

            }
            else if (button.Equals(MouseButtons.Right))
                commands.fire = "alt";
        }


        public void ProcessMouseUp()
        {
            commands.fire = "none";
        }

        public void ProcessMouseMove(double x, double y)
        {
            Vector2D loc = new Vector2D(x, y);
            loc.Normalize();

            commands.aiming = new Vector2D(loc.GetX(), loc.GetY());
            GetPlayerTank().aiming = new Vector2D(loc.GetX(), loc.GetY());

        }


        public void TryConnect(string name, string server, int port)
        {
            if (name.Length <= 16)
                tankName = name;
            else
            {
                NameErrorEvent("Name is longer than 16 characters");
                return;
            }

            Networking.ConnectToServer(SendName, server, port);
        }

        private void SendName(SocketState ss)
        {
            if (ss.ErrorOccured == true)
            {
                ErrorEvent("Unable to connect to server");
                if (ss.TheSocket.Connected)
                    ss.TheSocket.Close();
                return;
            }

            if (!Networking.Send(ss.TheSocket, tankName + "\n"))
            {
                ErrorEvent("Couldn't send player name since socket was closed");
                if (ss.TheSocket.Connected)
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
                ErrorEvent("Unable to receive tank ID and world size");
                if (ss.TheSocket.Connected)
                    ss.TheSocket.Close();
                return;
            }

            string[] startingInfo = Regex.Split(ss.GetData(), @"\n");

            lock (TheWorld.Tanks)
            {
                tankID = Int32.Parse(startingInfo[0]);
                TheWorld.Tanks[Int32.Parse(startingInfo[0])] = new Tank(tankName, tankID);
            }
            TheWorld.worldSize = Int32.Parse(startingInfo[1]);

            ss.RemoveData(0, tankID.ToString().Length + TheWorld.worldSize.ToString().Length + 2);

            ProcessData(ss);

            ss.OnNetworkAction = ReceiveFrameData;
            Networking.GetData(ss);
        }

        private void ReceiveFrameData(SocketState ss)
        {
            if (ss.ErrorOccured == true)
            {
                ErrorEvent("Error occured while receiving data from the server");
                if (ss.TheSocket.Connected)
                    ss.TheSocket.Close();
                return;
            }

            ProcessData(ss);

            if (wallsDone)
                Networking.Send(ss.TheSocket, JsonConvert.SerializeObject(commands) + "\n");

            OnFrameEvent();

            Networking.GetData(ss);

        }

        private void ProcessData(SocketState ss)
        {
            string totalData = ss.GetData();
            string[] parts = Regex.Split(totalData, @"(?<=[\n])");

            lock (TheWorld)
            {
                foreach (string p in parts)
                {
                    //This is to ignore empty strings
                    if (p.Length == 0)
                        continue;
                    //This is so it ignores the last string if it doesn't end in \n
                    if (p[p.Length - 1] != '\n')
                        break;

                    UpdateObject(p);

                    // Then remove it from the SocketState's growable buffer
                    ss.RemoveData(0, p.Length);
                }
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
                if (!TankColorRecord.ContainsKey(tank.ID))
                {
                    TankColorRecord.Add(tank.ID, SeenPlayers % 8);
                    SeenPlayers++;
                }
                if (tank.died)
                    TheWorld.Projectiles.Remove(tank.ID);
                wallsDone = true;
                return;
            }

            token = obj["proj"];
            if (token != null)
            {
                Projectile proj = JsonConvert.DeserializeObject<Projectile>(serializedObject);
                TheWorld.Projectiles[proj.ID] = proj;
                if (proj.died)
                    TheWorld.Projectiles.Remove(proj.ID);
                return;
            }

            token = obj["power"];
            if (token != null)
            {
                PowerUp power = JsonConvert.DeserializeObject<PowerUp>(serializedObject);
                TheWorld.PowerUps[power.ID] = power;
                if (power.died)
                    TheWorld.Projectiles.Remove(power.ID);
                return;
            }

            token = obj["beam"];
            if (token != null)
            {
                Beam beam = JsonConvert.DeserializeObject<Beam>(serializedObject);
                TheWorld.Beams[beam.ID] = beam;
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

        public int GetColor(int ID)
        {
            return TankColorRecord[ID];
        }
    }
}
