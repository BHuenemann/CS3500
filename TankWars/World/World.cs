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
            foreach (Tank t in Tanks.Values)
            {
                switch (PlayerCommands[t.ID].direction)
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
            double minX = Math.Min(w.endPoint1.GetX(), w.endPoint2.GetX());
            double minY = Math.Min(w.endPoint1.GetY(), w.endPoint2.GetY());
            double maxX = Math.Max(w.endPoint1.GetX(), w.endPoint2.GetX());
            double maxY = Math.Min(w.endPoint1.GetY(), w.endPoint2.GetY());
            if (t.location.GetX() >= minX - Constants.WallSize / 2 - Constants.TankSize / 2 && t.location.GetX() <= maxX + Constants.WallSize / 2 + Constants.TankSize / 2)
            {
                if (t.location.GetY() >= minY - Constants.WallSize / 2 - Constants.TankSize / 2 && t.location.GetY() <= maxY + Constants.WallSize / 2 + Constants.TankSize / 2)
                {
                    return true;
                }
            }
            return false;
        }


        public static bool CollisionTankPowerUp(Tank t, PowerUp p)
        {
            if ((p.location - t.location).Length() <= Constants.TankSize / 2)
            {
                return true;
            }
            return false;
        }


        public static bool CollisionProjectileTank(Projectile p, Tank t)
        {
            if ((p.location - t.location).Length() <= Constants.TankSize / 2)
            {
                return true;
            }
            return false;
        }


        public static bool CollisionProjectileWall(Projectile p, Wall w)
        {
            double minX = Math.Min(w.endPoint1.GetX(), w.endPoint2.GetX());
            double minY = Math.Min(w.endPoint1.GetY(), w.endPoint2.GetY());
            double maxX = Math.Max(w.endPoint1.GetX(), w.endPoint2.GetX());
            double maxY = Math.Min(w.endPoint1.GetY(), w.endPoint2.GetY());
            if (p.location.GetX() >= minX - Constants.WallSize / 2 && p.location.GetX() <= maxX + Constants.WallSize / 2)
            {
                if (p.location.GetY() >= minY - Constants.WallSize / 2 && p.location.GetY() <= maxY + Constants.WallSize / 2)
                {
                    return true;
                }
            }
            return false;
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
