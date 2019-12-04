//Authors: Ben Huenemann and Jonathan Wigderson

using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace TankWars
{
    public class World
    {
        Stopwatch Duration = new Stopwatch();

        public Dictionary<int, ControlCommands> PlayerCommands = new Dictionary<int, ControlCommands>();

        public Dictionary<int, Tank> Players = new Dictionary<int, Tank>();

        //Dictionaries that contain the objects that are in the world. Their IDs are used as the keys
        public Dictionary<int, Tank> Tanks { get; private set; } = new Dictionary<int, Tank>();
        public Dictionary<int, PowerUp> PowerUps { get; private set; } = new Dictionary<int, PowerUp>();
        public Dictionary<int, Beam> Beams { get; private set; } = new Dictionary<int, Beam>();
        public Dictionary<int, Projectile> Projectiles { get; private set; } = new Dictionary<int, Projectile>();
        public Dictionary<int, Wall> Walls { get; private set; } = new Dictionary<int, Wall>();

        public Dictionary<int, Tank> DeadTanks { get; private set; } = new Dictionary<int, Tank>();

        public Dictionary<int, TankExplosion> TankExplosions = new Dictionary<int, TankExplosion>();

        //Keeps track of the world size sent by the server
        public int worldSize;

        public int powerUpFrames = 0;



        public World()
        {
            Duration.Start();
        }

        
        
        public void UpdateCommand(int ID, ControlCommands c)
        {
            PlayerCommands[ID] = c;
        }


        public void UpdateTank(Tank t)
        {
            Players[t.ID] = t;
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
            Tanks[ID].Orientation = orientation;
            Tanks[ID].Orientation.Normalize();
        }


        public void TankSetVelocity(int ID, Vector2D velocity)
        {
            Tanks[ID].Velocity = velocity;
        }


        public void TankSetLocation(int ID, Vector2D location)
        {
            Tanks[ID].Location = location;
        }


        public void TankSetAiming(int ID, Vector2D aiming)
        {
            Tanks[ID].Aiming = aiming;
            Tanks[ID].Aiming.Normalize();
        }


        public void TankIncrementCooldownFrames(int ID)
        {
            Tanks[ID].CooldownFrames++;
        }


        public void TankSetCooldownFrames(int ID, int value)
        {
            Tanks[ID].CooldownFrames = value;
        }


        public void TankIncrementRespawnFrames(int ID)
        {
            DeadTanks[ID].RespawnFrames++;
        }


        public void TankSetRespawnFrames(int ID, int value)
        {
            Tanks[ID].RespawnFrames = value;
        }


        public void TankProjectileDamage(int tankID, int ProjID)
        {
            if(Tanks[tankID].HitPoints > 1)
                Tanks[tankID].HitPoints--;
            else
            {
                Tanks[Projectiles[ProjID].ownerID].Score++;
                Tanks[tankID].HitPoints = 0;
                Tanks[tankID].Died = true;
                DeadTanks[tankID] = Tanks[tankID];
            }
        }


        public void TankBeamDamage(int tankID, int BeamID)
        {
            Tanks[Beams[BeamID].ownerID].Score++;
            Tanks[tankID].HitPoints = 0;
            Tanks[tankID].Died = true;
            DeadTanks[tankID] = Tanks[tankID];
        }


        public void TankRestoreHealth(int ID)
        {
            Tanks[ID] = DeadTanks[ID];
            DeadTanks.Remove(ID);

            Tanks[ID].HitPoints = Constants.MaxHP;
            Tanks[ID].Died = false;
        }


        public void TankRemove(int ID)
        {
            Tanks.Remove(ID);
        }


        public void TankIncrementPowerUps(int ID)
        {
            Tanks[ID].PowerUps++;
        }


        public void TankDecrementPowerUps(int ID)
        {
            if(Tanks[ID].PowerUps > 0)
                Tanks[ID].PowerUps--;
        }


        public void TankDisconnect(int ID)
        {
            Tanks[ID].Disconnected = true;
        }


        public void TankKill(int ID)
        {
            Tanks[ID].Died = true;
            Tanks[ID].HitPoints = 0;
        }


        public void TankIncrementShotsFired(int ID)
        {
            Tanks[ID].ShotsFired++;
        }


        public void TankIncrementShotsHit(int ID)
        {
            Tanks[ID].ShotsHit++;
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



        public void BeamSetSpawnedTrue(int ID)
        {
            Beams[ID].Spawned = true;
        }


        public void BeamRemove(int ID)
        {
            Beams.Remove(ID);
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
