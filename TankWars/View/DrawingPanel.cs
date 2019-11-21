using System;
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
        private Image sourceImageWall = Image.FromFile(@"..\\..\\..\\Resources\Images\WallSprite.png");

        private GameController TheController;

        private Image background = Image.FromFile(@"..\\..\\..\\Resources\Images\Background.png");

        private Image sourceImageBlueTank = Image.FromFile(@"..\\..\\..\\Resources\Images\BlueTank.png");
        private Image sourceImageDarkTank = Image.FromFile(@"..\\..\\..\\Resources\Images\DarkTank.png");
        private Image sourceImageGreenTank = Image.FromFile(@"..\\..\\..\\Resources\Images\GreenTank.png");
        private Image sourceImageLightGreenTank = Image.FromFile(@"..\\..\\..\\Resources\Images\LightGreenTank.png");
        private Image sourceImageOrangeTank = Image.FromFile(@"..\\..\\..\\Resources\Images\OrangeTank.png");
        private Image sourceImagePurpleTank = Image.FromFile(@"..\\..\\..\\Resources\Images\PurpleTank.png");
        private Image sourceImageRedTank = Image.FromFile(@"..\\..\\..\\Resources\Images\RedTank.png");
        private Image sourceImageYellowTank = Image.FromFile(@"..\\..\\..\\Resources\Images\YellowTank.png");

        private Image sourceImageBlueTurret = Image.FromFile(@"..\\..\\..\\Resources\Images\BlueTurret.png");
        private Image sourceImageDarkTurret = Image.FromFile(@"..\\..\\..\\Resources\Images\DarkTurret.png");
        private Image sourceImageGreenTurret = Image.FromFile(@"..\\..\\..\\Resources\Images\GreenTurret.png");
        private Image sourceImageLightGreenTurret = Image.FromFile(@"..\\..\\..\\Resources\Images\LightGreenTurret.png");
        private Image sourceImageOrangeTurret = Image.FromFile(@"..\\..\\..\\Resources\Images\OrangeTurret.png");
        private Image sourceImagePurpleTurret = Image.FromFile(@"..\\..\\..\\Resources\Images\PurpleTurret.png");
        private Image sourceImageRedTurret = Image.FromFile(@"..\\..\\..\\Resources\Images\RedTurret.png");
        private Image sourceImageYellowTurret = Image.FromFile(@"..\\..\\..\\Resources\Images\YellowTurret.png");

        private Image sourceImageBlueShot = Image.FromFile(@"..\\..\\..\\Resources\Images\shot-blue.png");
        private Image sourceImageDarkShot = Image.FromFile(@"..\\..\\..\\Resources\Images\shot-grey.png");
        private Image sourceImageGreenShot = Image.FromFile(@"..\\..\\..\\Resources\Images\shot-green.png");
        private Image sourceImageLightGreenShot = Image.FromFile(@"..\\..\\..\\Resources\Images\shot-white.png");
        private Image sourceImageOrangeShot = Image.FromFile(@"..\\..\\..\\Resources\Images\shot-brown.png");
        private Image sourceImagePurpleShot = Image.FromFile(@"..\\..\\..\\Resources\Images\shot-violet.png");
        private Image sourceImageRedShot = Image.FromFile(@"..\\..\\..\\Resources\Images\shot-red.png");
        private Image sourceImageYellowShot = Image.FromFile(@"..\\..\\..\\Resources\Images\shot-yellow.png");





        public DrawingPanel(GameController controller)
        {
            DoubleBuffered = true;
            TheController = controller;

            TheController.ErrorEvent += DrawBackground;
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

        private void DrawBackground(string errorMessage)
        {
            //dialog box
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

            Bitmap resized = new Bitmap(background, new Size(TheController.TheWorld.worldSize, TheController.TheWorld.worldSize));

            e.Graphics.DrawImage(resized, new Point(0, 0));

            lock (TheController.TheWorld.Tanks)
            {
                // Draw the players
                foreach (Tank tank in TheController.TheWorld.Tanks.Values)
                {
                    tank.orientation.Normalize();
                    DrawObjectWithTransform(e, tank, TheController.TheWorld.worldSize, tank.location.GetX(), tank.location.GetY(), tank.orientation.ToAngle(),
                        TankDrawer);
                }
            }

            lock (TheController.TheWorld.PowerUps)
            {
                // Draw the powerups
                foreach (PowerUp pow in TheController.TheWorld.PowerUps.Values)
                {
                    DrawObjectWithTransform(e, pow, TheController.TheWorld.worldSize, pow.location.GetX(), pow.location.GetY(), 0, PowerUpDrawer);
                }
            }

            lock (TheController.TheWorld.Beams)
            {
                // Draw the beams
                foreach (Beam beam in TheController.TheWorld.Beams.Values)
                {
                    DrawObjectWithTransform(e, beam, TheController.TheWorld.worldSize, beam.origin.GetX(), beam.origin.GetY(), beam.origin.ToAngle(), BeamDrawer);
                }
            }

            lock (TheController.TheWorld.Projectiles)
            {
                // Draw the projectiles
                foreach (Projectile proj in TheController.TheWorld.Projectiles.Values)
                {
                    DrawObjectWithTransform(e, proj, TheController.TheWorld.worldSize, proj.location.GetX(), proj.location.GetY(), proj.orientation.ToAngle(), ProjectileDrawer);
                }
            }

            lock (TheController.TheWorld.Walls)
            {
                // Draw the walls
                foreach (Wall wall in TheController.TheWorld.Walls.Values)
                {
                    //if x is same for p1 and p2 is same then vertically long
                    //if y is same for p1 and p2 is same then horizontally long
                    DrawObjectWithTransform(e, wall, TheController.TheWorld.worldSize, (wall.endPoint1.GetX() + wall.endPoint2.GetX()) / 2,
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

            int colorID = TheController.GetColor(t.ID);

            switch(colorID)
            {
                case 0:
                    e.Graphics.DrawImage(sourceImageBlueTank, -(tankWidth / 2), -(tankHeight / 2), tankWidth, tankHeight);
                    e.Graphics.DrawImage(sourceImageBlueTurret, -(turretWidth / 2), -(turretHeight / 2), turretWidth, turretHeight);
                    break;
                case 1:
                    e.Graphics.DrawImage(sourceImageDarkTank, -(tankWidth / 2), -(tankHeight / 2), tankWidth, tankHeight);
                    e.Graphics.DrawImage(sourceImageDarkTurret, -(turretWidth / 2), -(turretHeight / 2), turretWidth, turretHeight);
                    break;
                case 2:
                    e.Graphics.DrawImage(sourceImageGreenTank, -(tankWidth / 2), -(tankHeight / 2), tankWidth, tankHeight);
                    e.Graphics.DrawImage(sourceImageGreenTurret, -(turretWidth / 2), -(turretHeight / 2), turretWidth, turretHeight);

                    break;
                case 3:
                    e.Graphics.DrawImage(sourceImageLightGreenTank, -(tankWidth / 2), -(tankHeight / 2), tankWidth, tankHeight);
                    e.Graphics.DrawImage(sourceImageLightGreenTurret, -(turretWidth / 2), -(turretHeight / 2), turretWidth, turretHeight);

                    break;
                case 4:
                    e.Graphics.DrawImage(sourceImageOrangeTank, -(tankWidth / 2), -(tankHeight / 2), tankWidth, tankHeight);
                    e.Graphics.DrawImage(sourceImageOrangeTurret, -(turretWidth / 2), -(turretHeight / 2), turretWidth, turretHeight);
                    break;
                case 5:
                    e.Graphics.DrawImage(sourceImagePurpleTank, -(tankWidth / 2), -(tankHeight / 2), tankWidth, tankHeight);
                    e.Graphics.DrawImage(sourceImagePurpleTurret, -(turretWidth / 2), -(turretHeight / 2), turretWidth, turretHeight);
                    break;
                case 6:
                    e.Graphics.DrawImage(sourceImageRedTank, -(tankWidth / 2), -(tankHeight / 2), tankWidth, tankHeight);
                    e.Graphics.DrawImage(sourceImageRedTurret, -(turretWidth / 2), -(turretHeight / 2), turretWidth, turretHeight);
                    break;
                case 7:
                    e.Graphics.DrawImage(sourceImageYellowTank, -(tankWidth / 2), -(tankHeight / 2), tankWidth, tankHeight);
                    e.Graphics.DrawImage(sourceImageYellowTurret, -(turretWidth / 2), -(turretHeight / 2), turretWidth, turretHeight);
                    break;

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

            int colorID = TheController.GetColor(p.ownerID);

            switch (colorID)
            {
                case 0:
                    e.Graphics.DrawImage(sourceImageBlueShot, -(width / 2), -(height / 2), width, height);
                    break;
                case 1:
                    e.Graphics.DrawImage(sourceImageDarkShot, -(width / 2), -(height / 2), width, height);
                    break;
                case 2:
                    e.Graphics.DrawImage(sourceImageGreenShot, -(width / 2), -(height / 2), width, height);
                    break;
                case 3:
                    e.Graphics.DrawImage(sourceImageLightGreenShot, -(width / 2), -(height / 2), width, height);
                    break;
                case 4:
                    e.Graphics.DrawImage(sourceImageOrangeShot, -(width / 2), -(height / 2), width, height);
                    break;
                case 5:
                    e.Graphics.DrawImage(sourceImagePurpleShot, -(width / 2), -(height / 2), width, height);
                    break;
                case 6:
                    e.Graphics.DrawImage(sourceImageRedShot, -(width / 2), -(height / 2), width, height);
                    break;
                case 7:
                    e.Graphics.DrawImage(sourceImageYellowShot, -(width / 2), -(height / 2), width, height);
                    break;
            }
        }

        private void WallDrawer(object o, PaintEventArgs e)
        {
            Wall w = o as Wall;

            int width = 50;
            int height = 50;

            Vector2D center = new Vector2D(Math.Abs(w.endPoint1.GetX() - w.endPoint2.GetX()),
                Math.Abs(w.endPoint1.GetY() - w.endPoint2.GetY()));

            // Draw portion of source image
            //e.Graphics.DrawImage(sourceImageWall, 0, 0, i, j, x, y);
        }
    }
}

