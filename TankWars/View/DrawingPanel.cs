﻿using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace TankWars
{
    public class DrawingPanel : Panel
    {
        private GameController TheController;

        public DrawingPanel(GameController controller)
        {
            DoubleBuffered = true;
            TheController = controller;

            TheController.OnConnectEvent += DrawBackground;
        }

        /// <summary>
        /// Helper method for DrawObjectWithTransform
        /// </summary>
        /// <param name="size">The world (and image) size</param>
        /// <param name="w">The worldspace coordinate</param>
        /// <returns></returns>
        private static int WorldSpaceToImageSpace(int size, double w)
        {
            return (int)w + size / 2;
        }

        // A delegate for DrawObjectWithTransform
        // Methods matching this delegate can draw whatever they want using e  
        public delegate void ObjectDrawer(object o, PaintEventArgs e);

        private void DrawBackground(bool errorOccured, string errorMessage)
        {
            if (errorOccured)
            {
                //dialog box
            }
            else
            {
                //ViewPanel.BackgroundImage = Image.FromFile(@"C:\Users\Jonathan Wigderson\source\repos\u11903382\TankWars\Resources\Images\Background.png");
                Image background = Image.FromFile(@"..\\..\\..\\Resources\Images\Background.png");
                Bitmap resized = new Bitmap(background, new Size(TheController.TheWorld.worldSize, TheController.TheWorld.worldSize));

                BackgroundImage = resized;
                BackgroundImageLayout = ImageLayout.None;
            }
        }

        /// <summary>
        /// This method performs a translation and rotation to drawn an object in the world.
        /// </summary>
        /// <param name="e">PaintEventArgs to access the graphics (for drawing)</param>
        /// <param name="o">The object to draw</param>
        /// <param name="worldSize">The size of one edge of the world (assuming the world is square)</param>
        /// <param name="worldX">The X coordinate of the object in world space</param>
        /// <param name="worldY">The Y coordinate of the object in world space</param>
        /// <param name="angle">The orientation of the objec, measured in degrees clockwise from "up"</param>
        /// <param name="drawer">The drawer delegate. After the transformation is applied, the delegate is invoked to draw whatever it wants</param>
        private void DrawObjectWithTransform(PaintEventArgs e, object o, int worldSize, double worldX, double worldY, double angle, ObjectDrawer drawer)
        {
            // "push" the current transform
            System.Drawing.Drawing2D.Matrix oldMatrix = e.Graphics.Transform.Clone();

            int x = WorldSpaceToImageSpace(worldSize, worldX);
            int y = WorldSpaceToImageSpace(worldSize, worldY);
            e.Graphics.TranslateTransform(x, y);
            e.Graphics.RotateTransform((float)angle);
            drawer(o, e);

            // "pop" the transform
            e.Graphics.Transform = oldMatrix;
        }

        // This method is invoked when the DrawingPanel needs to be re-drawn
        protected override void OnPaint(PaintEventArgs e)
        {
            if (!TheController.wallsDone)
                return;

            //do the player location stuff here TODO
            double playerX = TheController.GetPlayerTank().location.GetX();
            double playerY = TheController.GetPlayerTank().location.GetY();

            // calculate view/world size ratio
            double ratio = (double)Constants.ViewSize / (double)TheController.TheWorld.worldSize;
            int halfSizeScaled = (int)(TheController.TheWorld.worldSize / 2.0 * ratio);

            double inverseTranslateX = -WorldSpaceToImageSpace(TheController.TheWorld.worldSize, playerX) + halfSizeScaled;
            double inverseTranslateY = -WorldSpaceToImageSpace(TheController.TheWorld.worldSize, playerY) + halfSizeScaled;

            e.Graphics.TranslateTransform((float)inverseTranslateX, (float)inverseTranslateY);

            lock (TheController.TheWorld.Tanks)
            {
                // Draw the players
                foreach (Tank tank in TheController.TheWorld.Tanks.Values)
                {
                    DrawObjectWithTransform(e, tank, this.Size.Width, tank.location.GetX(), tank.location.GetY(), tank.orientation.ToAngle(),
                        TankDrawer);
                }
            }

            lock (TheController.TheWorld.PowerUps)
            {
                // Draw the powerups
                foreach (PowerUp pow in TheController.TheWorld.PowerUps.Values)
                {
                    DrawObjectWithTransform(e, pow, this.Size.Width, pow.location.GetX(), pow.location.GetY(), 0, PowerUpDrawer);
                }
            }

            lock (TheController.TheWorld.Beams)
            {
                // Draw the beams
                foreach (Beam beam in TheController.TheWorld.Beams.Values)
                {
                    DrawObjectWithTransform(e, beam, this.Size.Width, beam.origin.GetX(), beam.origin.GetY(), beam.origin.ToAngle(), BeamDrawer);
                }
            }

            lock (TheController.TheWorld.Projectiles)
            {
                // Draw the projectiles
                foreach (Projectile proj in TheController.TheWorld.Projectiles.Values)
                {
                    DrawObjectWithTransform(e, proj, this.Size.Width, proj.location.GetX(), proj.location.GetY(), proj.orientation.ToAngle(), ProjectileDrawer);
                }
            }

            lock (TheController.TheWorld.Walls)
            {
                // Draw the walls
                foreach (Wall wall in TheController.TheWorld.Walls.Values)
                {
                    //if x is same for p1 and p2 is same then vertically long
                    //if y is same for p1 and p2 is same then horizontally long
                    DrawObjectWithTransform(e, wall, this.Size.Width, (wall.endPoint1.GetX() + wall.endPoint2.GetX()) / 2,
                        (wall.endPoint1.GetY() + wall.endPoint2.GetY()) / 2, 0, WallDrawer);
                }
            }

            // Do anything that Panel (from which we inherit) needs to do
            base.OnPaint(e);
        }

        private void TankDrawer(object o, PaintEventArgs e)
        {
            Tank t = o as Tank;

            int tankWidth = 60;
            int tankHeight = 60;
            int turretWidth = 50;
            int turretHeight = 50;

            using (System.Drawing.SolidBrush blueBrush = new System.Drawing.SolidBrush(System.Drawing.Color.Blue))
            {
                Rectangle r = new Rectangle(-(tankWidth / 2), -(tankWidth / 2), tankWidth, tankWidth);
                e.Graphics.FillRectangle(blueBrush, r);
            }
        }

        private void PowerUpDrawer(object o, PaintEventArgs e)
        {
            PowerUp p = o as PowerUp;

            int width = 8;
            int height = 8;

            // Creat Bitmap object of image
            Image sourceImage = Image.FromFile(@"..\\..\\..\\Resources\Images\RedTurret.png");
            // Draw portion of source image
            Rectangle sourceRect = new Rectangle(0, 0, width, height);
            e.Graphics.DrawImage(sourceImage, 0, 0, sourceRect, GraphicsUnit.Pixel);
        }

        private void BeamDrawer(object o, PaintEventArgs e)
        {
            Beam b = o as Beam;

            int width = 30;
            int height = 30;

            // Creat Bitmap object of image
            Image sourceImage = Image.FromFile(@"..\\..\\..\\Resources\Images\shot-white.png");
            // Draw portion of source image
            Rectangle sourceRect = new Rectangle(0, 0, width, height);
            e.Graphics.DrawImage(sourceImage, 0, 0, sourceRect, GraphicsUnit.Pixel);
        }

        private void ProjectileDrawer(object o, PaintEventArgs e)
        {
            Projectile p = o as Projectile;

            int width = 30;
            int height = 30;

        }

        private void WallDrawer(object o, PaintEventArgs e)
        {
            Wall w = o as Wall;

            int width = 50;
            int height = 50;

        }
    }
}
