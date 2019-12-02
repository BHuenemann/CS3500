﻿//Authors: Ben Huenemann and Jonathan Wigderson

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




        public void TankSetOrientation(Tank t, Vector2D orientation)
        {
            t.orientation = orientation;
        }


        public void TankSetVelocity(Tank t, Vector2D velocity)
        {
            t.velocity = velocity;
        }


        public void TankSetLocation(Tank t, Vector2D location)
        {
            t.location = location;
        }


        public void TankSetAiming(Tank t, Vector2D aiming)
        {
            t.aiming = aiming;
            t.aiming.Normalize();
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
