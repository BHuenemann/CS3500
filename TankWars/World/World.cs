//Authors: Ben Huenemann and Jonathan Wigderson

using System;
using System.Collections.Generic;

namespace TankWars
{
    public class World
    {
        // In reality, these should not be public,
        // but for the purposes of this lab, the "World" 
        // class is just a wrapper around these two fields.
        public Dictionary<int, Tank> Tanks;
        public Dictionary<int, PowerUp> PowerUps;
        public Dictionary<int, Beam> Beams;
        public Dictionary<int, Projectile> Projectiles;
        public Dictionary<int, Wall> Walls;
        public Dictionary<int, TankExplosion> TankExplosions;

        public int worldSize;


        public World()
        {
            Tanks = new Dictionary<int, Tank>();
            PowerUps = new Dictionary<int, PowerUp>();
            Beams = new Dictionary<int, Beam>();
            Projectiles = new Dictionary<int, Projectile>();
            Walls = new Dictionary<int, Wall>();
            TankExplosions = new Dictionary<int, TankExplosion>();
        }
    }
}
