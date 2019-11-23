//Authors: Ben Huenemann and Jonathan Wigderson

using System;
using System.Collections.Generic;
using System.Text;

namespace TankWars
{
    public static class Constants
    {
        //Tanks HP
        public const int MaxHP = 3;
        public const int ViewSize = 800;
        public const int BeamFrameLength = 30;

        public const int ViewLocationX = 10;
        public const int ViewLocationY = 45;

        //Information for the beam: width, particles count, speed, and radius
        public const int BeamWidth = 6;
        public const int BeamParticleCount = 30;
        public const int BeamParticleSpeed = 5;
        public const int BeamParticleRadius = 3;

        //Size of Tanks
        public const int TankSize = 60;
        //Size of Turrets
        public const int TurretSize = 50;
        //Size of Power Ups
        public const int PowerUpSize = 8;
        //Size of Projectiles
        public const int ProjectileSize = 30;
        //Size of Walls
        public const int WallSize = 50;
    }
}
