//Authors: Ben Huenemann and Jonathan Wigderson

using System;
using System.Collections.Generic;

namespace TankWars
{
    public class World
    {
        public Dictionary<int, ControlCommands> PlayerCommands = new Dictionary<int, ControlCommands>();

        //Dictionaries that contain the objects that are in the world. Their IDs are used as the keys
        public Dictionary<int, Tank> Tanks { get; private set; } = new Dictionary<int, Tank>();
        public Dictionary<int, PowerUp> PowerUps { get; private set; } = new Dictionary<int, PowerUp>();
        public Dictionary<int, Beam> Beams { get; private set; } = new Dictionary<int, Beam>();
        public Dictionary<int, Projectile> Projectiles { get; private set; } = new Dictionary<int, Projectile>();
        public Dictionary<int, Wall> Walls { get; private set; } = new Dictionary<int, Wall>();

        public Dictionary<int, TankExplosion> TankExplosions = new Dictionary<int, TankExplosion>();

        //Keeps track of the world size sent by the server
        public int worldSize;



        public void UpdateCommand(int ID, ControlCommands c)
        {
            PlayerCommands[ID] = c;
        }


        public void UpdateTank(Tank t)
        {
            Tanks[t.ID] = t;
        }


        public void UpdatePowerUp(PowerUp p)
        {
            PowerUps[p.ID] = p;
        }


        public void UpdateBeam(Beam b)
        {
            Beams[b.ID] = b;
        }


        public void UpdateProjectile(Projectile p)
        {
            Projectiles[p.ID] = p;
        }


        public void UpdateWall(Wall w)
        {
            Walls[w.ID] = w;
        }



        public void TankSetOrientation(int ID, Vector2D orientation)
        {
            Tanks[ID].orientation = orientation;
            Tanks[ID].orientation.Normalize();
        }


        public void TankSetVelocity(int ID, Vector2D velocity)
        {
            Tanks[ID].velocity = velocity;
        }


        public void TankSetLocation(int ID, Vector2D location)
        {
            Tanks[ID].location = location;
        }


        public void TankSetAiming(int ID, Vector2D aiming)
        {
            Tanks[ID].aiming = aiming;
            Tanks[ID].aiming.Normalize();
        }


        public void TankIncrementCooldownFrames(int ID)
        {
            Tanks[ID].cooldownFrames++;
        }


        public void TankSetCooldownFrames(int ID, int value)
        {
            Tanks[ID].cooldownFrames = value;
        }



        public void ProjectileSetLocation(int ID, Vector2D location)
        {
            Projectiles[ID].location = location;
        }


        public void ProjectileSetDied(int ID)
        {
            Projectiles[ID].died = true;
        }


        public void ProjectileRemove(int ID)
        {
            Projectiles.Remove(ID);
        }



        public void PowerUpSetLocation(int ID, Vector2D location)
        {
            PowerUps[ID].location = location;
        }


        public void PowerUpSetDied(int ID)
        {
            PowerUps[ID].died = true;
        }


        public void PowerUpRemove(int ID)
        {
            PowerUps.Remove(ID);
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
