using System;
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

        Stopwatch FireRateStopWatch = new Stopwatch();


        public static void Main(string[] args)
        {
            ReadSettingFile(@"..\\..\\..\\Resources\settings.xml");

            Networking.StartServer(ReceivePlayerName, 11000);

            Console.WriteLine("Server is running. Accepting clients.");

            Stopwatch watch = new Stopwatch();
            watch.Start();

            while (true)
            {
                while (watch.ElapsedMilliseconds < MSPerFrame)
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
            lock(TheWorld)
            {
                UpdateTanks();
                UpdateProjectiles();
            }
        }


        public static void UpdateTanks()
        {
            foreach (Tank t in TheWorld.Tanks.Values)
            {
                //MOVEMENT
                switch (TheWorld.PlayerCommands[t.ID].direction)
                {
                    case "left":
                        TheWorld.TankSetOrientation(t.ID, new Vector2D(-1, 0));
                        TheWorld.TankSetVelocity(t.ID, t.orientation * Constants.TankSpeed);
                        break;
                    case "right":
                        TheWorld.TankSetOrientation(t.ID, new Vector2D(1, 0));
                        TheWorld.TankSetVelocity(t.ID, t.orientation * Constants.TankSpeed);
                        break;
                    case "up":
                        TheWorld.TankSetOrientation(t.ID, new Vector2D(0, -1));
                        TheWorld.TankSetVelocity(t.ID, t.orientation * Constants.TankSpeed);
                        break;
                    case "down":
                        TheWorld.TankSetOrientation(t.ID, new Vector2D(0, 1));
                        TheWorld.TankSetVelocity(t.ID, t.orientation * Constants.TankSpeed);
                        break;
                    case "none":
                        TheWorld.TankSetVelocity(t.ID, new Vector2D(0, 0));
                        break;
                }

                TheWorld.TankSetLocation(t.ID, t.location + t.velocity);

                foreach (Wall w in TheWorld.Walls.Values)
                {
                    if (CollisionTankWall(t, w))
                        TheWorld.TankSetLocation(t.ID, t.location - t.velocity);
                }

                //AIMING
                TheWorld.TankSetAiming(t.ID, TheWorld.PlayerCommands[t.ID].aiming);

                //FIRING
                if (t.cooldownFrames < FramesPerShot)
                    TheWorld.TankIncrementCooldownFrames(t.ID);

                switch (TheWorld.PlayerCommands[t.ID].fire)
                {
                    case "main":
                        if(t.cooldownFrames == FramesPerShot)
                        {
                            Projectile p = new Projectile(t.location, t.aiming, t.ID);
                            TheWorld.UpdateProjectile(p);

                            TheWorld.TankSetCooldownFrames(t.ID, 0);
                        }
                        break;
                    case "alt":
                        Beam b = new Beam(t.location, t.orientation, t.ID);
                        TheWorld.UpdateBeam(b);
                        break;
                    case "none":
                        break;
                }
            }
        }


        public static void UpdateProjectiles()
        {
            foreach (Projectile p in TheWorld.Projectiles.Values.ToList())
            {
                if (p.died)
                {
                    TheWorld.ProjectileRemove(p.ID);
                    continue;
                }

                TheWorld.ProjectileSetLocation(p.ID, p.location + p.velocity);

                if (Math.Abs(p.location.GetX()) > UniverseSize / 2 || Math.Abs(p.location.GetY()) > UniverseSize / 2)
                    TheWorld.ProjectileSetDied(p.ID);

                foreach (Tank t in TheWorld.Tanks.Values)
                {
                    if(CollisionProjectileTank(p, t) && p.ownerID != t.ID)
                    {
                        TheWorld.ProjectileSetDied(p.ID);
                    }
                }
                foreach (Wall w in TheWorld.Walls.Values)
                {
                    if (CollisionProjectileWall(p, w))
                        TheWorld.ProjectileSetDied(p.ID);
                }
            }
        }


        private static void SendDataToSockets()
        {
            foreach (Socket s in SocketConnections)
            {
                StringBuilder frameMessage = new StringBuilder();


                lock (TheWorld)
                {
                    foreach (Tank t in TheWorld.Tanks.Values)
                        frameMessage.Append(JsonConvert.SerializeObject(t) + "\n");

                    foreach (PowerUp p in TheWorld.PowerUps.Values)
                        frameMessage.Append(JsonConvert.SerializeObject(p) + "\n");

                    foreach (Projectile p in TheWorld.Projectiles.Values)
                        frameMessage.Append(JsonConvert.SerializeObject(p) + "\n");

                    foreach (Beam b in TheWorld.Beams.Values)
                        frameMessage.Append(JsonConvert.SerializeObject(b) + "\n");
                }


                Networking.Send(s, frameMessage.ToString());
            }
        }


        private static void ReceivePlayerName(SocketState ss)
        {
            if (ss.ErrorOccured == true)
            {

            }
            Console.WriteLine("Accepted new client");
            ss.OnNetworkAction = SendStartupInfo;
            Networking.GetData(ss);
        }


        private static void SendStartupInfo(SocketState ss)
        {
            if (ss.ErrorOccured == true)
            {
                Console.WriteLine("Error occured while accepting- " + ss.ErrorMessage);
                return;
            }

            string tankName = ss.GetData();
            int tankID = (int)ss.ID;


            ss.RemoveData(0, tankName.Length);


            lock(TheWorld)
            {
                Tank t = new Tank(tankName.Substring(0, tankName.Length - 1), tankID);
                TheWorld.UpdateTank(t);
                TheWorld.TankSetCooldownFrames(t.ID, FramesPerShot);
                TheWorld.UpdateCommand(tankID, new ControlCommands());
                SpawnTank(t);

                Console.WriteLine("Player(" + tankID + ") " + "\"" + t.name + "\" joined");

            }


            ss.OnNetworkAction = ReceiveCommandData;


            string message = tankID + "\n" + UniverseSize.ToString() + "\n";
            Networking.Send(ss.TheSocket, message);

            lock(TheWorld)
            {
                StringBuilder wallMessage = new StringBuilder();
                foreach (Wall w in TheWorld.Walls.Values)
                    wallMessage.Append(JsonConvert.SerializeObject(w) + "\n");
                Networking.Send(ss.TheSocket, wallMessage.ToString());
            }


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
                TheWorld.UpdateCommand(ID, com);
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
                                    reader.ReadToFollowing("x");
                                    reader.Read(); //gets x
                                    double p1X = Double.Parse(reader.Value);
                                    reader.ReadToFollowing("y");
                                    reader.Read(); //gets y
                                    double p1Y = Double.Parse(reader.Value);
                                    Vector2D p1V = new Vector2D(p1X, p1Y);

                                    reader.ReadToFollowing("x");
                                    reader.Read(); //gets x
                                    double p2X = Double.Parse(reader.Value);
                                    reader.ReadToFollowing("y");
                                    reader.Read(); //gets y
                                    double p2Y = Double.Parse(reader.Value);
                                    Vector2D p2V = new Vector2D(p2X, p2Y);



                                    Wall w = new Wall(p1V, p2V);
                                    TheWorld.UpdateWall(w);
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


        public static void SpawnTank(Tank t)
        {
            Random random = new Random();

            do
            {
                int xLocation = random.Next(-UniverseSize / 2, UniverseSize / 2);
                int yLocation = random.Next(-UniverseSize / 2, UniverseSize / 2);

                TheWorld.TankSetLocation(t.ID, new Vector2D(xLocation, yLocation));
            }
            while (TankSpawnCollisions(t));
        }


        public static bool TankSpawnCollisions(Tank t)
        {
            foreach(Wall w in TheWorld.Walls.Values)
            {
                if (CollisionTankWall(t, w))
                    return true;
            }
            foreach(Projectile p in TheWorld.Projectiles.Values)
            {
                if (CollisionProjectileTank(p, t))
                    return true;
            }
            foreach(Beam b in TheWorld.Beams.Values)
            {
                if (CollisionBeamTank(b, t))
                    return true;
            }

            return false;
        }


        public static bool CollisionTankWall(Tank t, Wall w)
        {
            double minX = Math.Min(w.endPoint1.GetX(), w.endPoint2.GetX());
            double minY = Math.Min(w.endPoint1.GetY(), w.endPoint2.GetY());
            double maxX = Math.Max(w.endPoint1.GetX(), w.endPoint2.GetX());
            double maxY = Math.Max(w.endPoint1.GetY(), w.endPoint2.GetY());

            bool xCollide = (t.location.GetX() >= minX - Constants.WallSize / 2 - Constants.TankSize / 2 &&
                t.location.GetX() <= maxX + Constants.WallSize / 2 + Constants.TankSize / 2);
            bool yCollide = (t.location.GetY() >= minY - Constants.WallSize / 2 - Constants.TankSize / 2 &&
                t.location.GetY() <= maxY + Constants.WallSize / 2 + Constants.TankSize / 2);

            if (xCollide && yCollide)
                return true;

            return false;
        }


        public static bool CollisionTankPowerUp(Tank t, PowerUp p)
        {
            if ((p.location - t.location).Length() <= Constants.TankSize / 2)
                return true;
            return false;
        }


        public static bool CollisionProjectileTank(Projectile p, Tank t)
        {
            return (p.location - t.location).Length() < Constants.TankSize / 2;
        }


        public static bool CollisionProjectileWall(Projectile p, Wall w)
        {
            double minX = Math.Min(w.endPoint1.GetX(), w.endPoint2.GetX());
            double minY = Math.Min(w.endPoint1.GetY(), w.endPoint2.GetY());
            double maxX = Math.Max(w.endPoint1.GetX(), w.endPoint2.GetX());
            double maxY = Math.Max(w.endPoint1.GetY(), w.endPoint2.GetY());
            if (p.location.GetX() >= minX - Constants.WallSize / 2 && p.location.GetX() <= maxX + Constants.WallSize / 2)
            {
                if (p.location.GetY() >= minY - Constants.WallSize / 2 && p.location.GetY() <= maxY + Constants.WallSize / 2)
                {
                    return true;
                }
            }
            return false;
        }


        public static bool CollisionBeamTank(Beam b, Tank t)
        {
            return Intersects(b.origin, b.orientation, t.location, Constants.TankSize / 2);
        }


        /// <summary>
        /// Determines if a ray interescts a circle
        /// </summary>
        /// <param name="rayOrig">The origin of the ray</param>
        /// <param name="rayDir">The direction of the ray</param>
        /// <param name="center">The center of the circle</param>
        /// <param name="r">The radius of the circle</param>
        /// <returns></returns>
        public static bool Intersects(Vector2D rayOrig, Vector2D rayDir, Vector2D center, double r)
        {
            // ray-circle intersection test
            // P: hit point
            // ray: P = O + tV
            // circle: (P-C)dot(P-C)-r^2 = 0
            // substitute to solve for t gives a quadratic equation:
            // a = VdotV
            // b = 2(O-C)dotV
            // c = (O-C)dot(O-C)-r^2
            // if the discriminant is negative, miss (no solution for P)
            // otherwise, if both roots are positive, hit

            double a = rayDir.Dot(rayDir);
            double b = ((rayOrig - center) * 2.0).Dot(rayDir);
            double c = (rayOrig - center).Dot(rayOrig - center) - r * r;

            // discriminant
            double disc = b * b - 4.0 * a * c;

            if (disc < 0.0)
                return false;

            // find the signs of the roots
            // technically we should also divide by 2a
            // but all we care about is the sign, not the magnitude
            double root1 = -b + Math.Sqrt(disc);
            double root2 = -b - Math.Sqrt(disc);

            return (root1 > 0.0 && root2 > 0.0);
        }
    }
}
