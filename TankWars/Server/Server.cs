﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Xml;
using NetworkUtil;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using TankWars;

namespace Server
{
    public class Server
    {
        private static World TheWorld = new World();
        private static HashSet<Socket> SocketConnections = new HashSet<Socket>();

        static private int UniverseSize;
        static private int MSPerFrame;
        static private int FramesPerShot;
        static private int RespawnRate;


        public static void Main(string[] args)
        {
            ReadSettingFile(@"..\\..\\..\\Resources\settings.xml");

            Networking.StartServer(ReceivePlayerName, 11000);

            Stopwatch watch = new Stopwatch();
            watch.Start();

            while (true)
            {
                while(watch.ElapsedMilliseconds < MSPerFrame)
                {
                    //Do Nothing
                }
                watch.Restart();

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

            ss.RemoveData(0, tankName.Length);

            Tank t = new Tank(tankName.Substring(0, tankName.Length - 2), tankID);
            TheWorld.Tanks[tankID] = t;

            ss.OnNetworkAction = ReceiveCommandData;

            string message = tankID + "\n" + UniverseSize.ToString() + "\n";
            Networking.Send(ss.TheSocket, message);

            StringBuilder wallMessage = new StringBuilder();
            foreach (Wall w in TheWorld.Walls.Values)
                wallMessage.Append(JsonConvert.SerializeObject(w) + "\n");
            Networking.Send(ss.TheSocket, wallMessage.ToString());

            TheWorld.PlayerCommands.Add(tankID, new ControlCommands());
            SocketConnections.Add(ss.TheSocket);

            Networking.GetData(ss);
        }


        private static void ReceiveCommandData(SocketState ss)
        {
            if (ss.ErrorOccured == true)
            {

            }

            ProcessData(ss);

            Networking.GetData(ss);
        }


        private static void ProcessData(SocketState ss)
        {
            //Splits the string but keeps the '\n' characters
            string totalData = ss.GetData();
            string[] parts = Regex.Split(totalData, @"(?<=[\n])");

            lock (TheWorld)
            {
                string lastCommand = null;

                foreach (string p in parts)
                {
                    //This is to ignore empty strings
                    if (p.Length == 0)
                        continue;
                    //This is so it ignores the last string if it doesn't end in '\n'
                    if (p[p.Length - 1] != '\n')
                        break;

                    lastCommand = p;
                    ss.RemoveData(0, p.Length);
                }

                if (lastCommand != null)
                    //Calls a method to deserialize the data and then removes the data from the buffer
                    UpdateObject(lastCommand, (int)ss.ID);
            }
        }


        private static void UpdateObject(string serializedObject, int ID)
        {
            JObject obj = JObject.Parse(serializedObject);

            //Command
            JToken token = obj["moving"];
            if (token != null)
            {
                ControlCommands com = JsonConvert.DeserializeObject<ControlCommands>(serializedObject);
                TheWorld.PlayerCommands[ID] = com;
                return;
            }
        }


        private static void ReadSettingFile(string fileName)
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

                                    Vector2D p1V = new Vector2D();
                                    Vector2D p2V = new Vector2D();

                                    reader.Read();
                                    switch (reader.Name)
                                    {
                                        case "p1":
                                            reader.Read(); //gets x
                                            double p1X = Double.Parse(reader.Value);
                                            reader.Read(); //gets y
                                            double p1Y = Double.Parse(reader.Value);
                                            p1V = new Vector2D(p1X, p1Y);
                                            break;

                                        case "p2":
                                            reader.Read(); //gets x
                                            double p2X = Double.Parse(reader.Value);
                                            reader.Read(); //gets y
                                            double p2Y = Double.Parse(reader.Value);
                                            p2V = new Vector2D(p2X, p2Y);
                                            break;


                                    }
                                    Wall w = new Wall(p1V, p2V);
                                    TheWorld.Walls.Add(w.ID, w);
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
