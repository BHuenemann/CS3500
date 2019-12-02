//Authors: Ben Huenemann and Jonathan Wigderson

using System;
using System.Collections.Generic;

namespace TankWars
{
    public class World
    {
        public Dictionary<int, ControlCommands> PlayerCommands = new Dictionary<int, ControlCommands>();

        //Dictionaries that contain the objects that are in the world. Their IDs are used as the keys
        public Dictionary<int, Tank> Tanks = new Dictionary<int, Tank>();
        public Dictionary<int, PowerUp> PowerUps = new Dictionary<int, PowerUp>();
        public Dictionary<int, Beam> Beams = new Dictionary<int, Beam>();
        public Dictionary<int, Projectile> Projectiles = new Dictionary<int, Projectile>();
        public Dictionary<int, Wall> Walls = new Dictionary<int, Wall>();
        public Dictionary<int, TankExplosion> TankExplosions = new Dictionary<int, TankExplosion>();

        //Keeps track of the world size sent by the server
        public int worldSize;



        public void UpdateTanks()
        {
            foreach(Tank t in Tanks.Values)
            {
                switch(PlayerCommands[t.ID].direction)
                {
                    case "left":
                        t.orientation = new Vector2D(-1, 0);
                        t.velocity = t.orientation * Constants.TankSpeed;
                        break;
                    case "right":
                        t.orientation = new Vector2D(1, 0);
                        t.velocity = t.orientation * Constants.TankSpeed;
                        break;
                    case "up":
                        t.orientation = new Vector2D(0, -1);
                        t.velocity = t.orientation * Constants.TankSpeed;
                        break;
                    case "down":
                        t.orientation = new Vector2D(0, 1);
                        t.velocity = t.orientation * Constants.TankSpeed;
                        break;
                    case "none":
                        t.velocity = new Vector2D(0, 0);
                        break;
                }
            }
        }


        public static bool CollisionTankWall(Tank t, Wall w)
        {
            return false;
        }


        public static bool CollisionTankPowerUp(Tank t, PowerUp p)
        {
            return false;
        }


        public static bool CollisionProjectileTank(Projectile p, Tank t)
        {
            return false;
        }


        public static bool CollisionProjectileWall(Projectile p, Wall w)
        {
            return false;
        }


        public static bool CollisionBeamTank(Beam b, Tank t)
        {
            return false;
        }


        public void ExplosionIncrementFrames(TankExplosion e)
        {
            e.tankFrames++;
        }

        public void ExplosionClearFrames(TankExplosion e)
        {
            e.tankFrames = 0;
        }
    }
}
