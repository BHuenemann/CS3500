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
