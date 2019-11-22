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

        private Image sourceImageWall = Image.FromFile(@"..\\..\\..\\Resources\Images\WallSprite.png");



        public DrawingPanel(GameController controller)
        {
            DoubleBuffered = true;
            TheController = controller;
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

            e.Graphics.DrawImage(background, 0, 0, TheController.TheWorld.worldSize, TheController.TheWorld.worldSize);


            lock (TheController.TheWorld)
            {

                // Draw the walls
                foreach (Wall wall in TheController.TheWorld.Walls.Values)
                {
                    DrawObjectWithTransform(e, wall, TheController.TheWorld.worldSize, (wall.endPoint1.GetX() + wall.endPoint2.GetX()) / 2,
                        (wall.endPoint1.GetY() + wall.endPoint2.GetY()) / 2, 0, WallDrawer);
                }

                // Draw the players
                foreach (Tank tank in TheController.TheWorld.Tanks.Values)
                {
                    tank.orientation.Normalize();
                    tank.aiming.Normalize();
                    DrawObjectWithTransform(e, tank, TheController.TheWorld.worldSize, tank.location.GetX(), tank.location.GetY(), tank.orientation.ToAngle(),
                        TankDrawer);
                    DrawObjectWithTransform(e, tank, TheController.TheWorld.worldSize, tank.location.GetX(), tank.location.GetY(), tank.aiming.ToAngle(),
                        TurretDrawer);
                    DrawObjectWithTransform(e, tank, TheController.TheWorld.worldSize, tank.location.GetX(), tank.location.GetY(), 0,
                        NameDrawer);

                }

                // Draw the powerups
                foreach (PowerUp pow in TheController.TheWorld.PowerUps.Values)
                {
                    DrawObjectWithTransform(e, pow, TheController.TheWorld.worldSize, pow.location.GetX(), pow.location.GetY(), 0, PowerUpDrawer);
                }

                // Draw the beams
                foreach (Beam beam in TheController.TheWorld.Beams.Values)
                {
                    DrawObjectWithTransform(e, beam, TheController.TheWorld.worldSize, beam.origin.GetX(), beam.origin.GetY(), beam.orientation.ToAngle(), BeamDrawer);
                }

                // Draw the projectiles
                foreach (Projectile proj in TheController.TheWorld.Projectiles.Values)
                {
                    proj.orientation.Normalize();
                    DrawObjectWithTransform(e, proj, TheController.TheWorld.worldSize, proj.location.GetX(), proj.location.GetY(), proj.orientation.ToAngle(), ProjectileDrawer);
                }

                if(TheController.TheWorld.Beams.Count != 0)
                {
                    foreach(Beam b in TheController.TheWorld.Beams.Values.ToList())
                    {
                        if (b.beamFrames == Constants.BeamFrameLength)
                        {
                            TheController.TheWorld.Beams.Remove(b.ID);
                        }
                    }
                }
            }

            // Do anything that Panel (from which we inherit) needs to do
            base.OnPaint(e);
        }


        private void TankDrawer(object o, PaintEventArgs e)
        {
            Tank t = o as Tank;

            if (t.hitPoints == 0)
                return;

            int tankWidth = 60;
            int tankHeight = 60;

            int colorID = TheController.GetColor(t.ID);

            switch(colorID)
            {
                case 0:
                    e.Graphics.DrawImage(sourceImageBlueTank, -(tankWidth / 2), -(tankHeight / 2), tankWidth, tankHeight);
                    break;
                case 1:
                    e.Graphics.DrawImage(sourceImageDarkTank, -(tankWidth / 2), -(tankHeight / 2), tankWidth, tankHeight);
                    break;
                case 2:
                    e.Graphics.DrawImage(sourceImageGreenTank, -(tankWidth / 2), -(tankHeight / 2), tankWidth, tankHeight);
                    break;
                case 3:
                    e.Graphics.DrawImage(sourceImageLightGreenTank, -(tankWidth / 2), -(tankHeight / 2), tankWidth, tankHeight);
                    break;
                case 4:
                    e.Graphics.DrawImage(sourceImageOrangeTank, -(tankWidth / 2), -(tankHeight / 2), tankWidth, tankHeight);
                    break;
                case 5:
                    e.Graphics.DrawImage(sourceImagePurpleTank, -(tankWidth / 2), -(tankHeight / 2), tankWidth, tankHeight);
                    break;
                case 6:
                    e.Graphics.DrawImage(sourceImageRedTank, -(tankWidth / 2), -(tankHeight / 2), tankWidth, tankHeight);
                    break;
                case 7:
                    e.Graphics.DrawImage(sourceImageYellowTank, -(tankWidth / 2), -(tankHeight / 2), tankWidth, tankHeight);
                    break;
            }

        }

        private void TurretDrawer(object o, PaintEventArgs e)
        {
            Tank t = o as Tank;

            if (t.hitPoints == 0)
                return;

            int turretWidth = 50;
            int turretHeight = 50;

            int colorID = TheController.GetColor(t.ID);

            switch (colorID)
            {
                case 0:
                    e.Graphics.DrawImage(sourceImageBlueTurret, -(turretWidth / 2), -(turretHeight / 2), turretWidth, turretHeight);
                    break;
                case 1:
                    e.Graphics.DrawImage(sourceImageDarkTurret, -(turretWidth / 2), -(turretHeight / 2), turretWidth, turretHeight);
                    break;
                case 2:
                    e.Graphics.DrawImage(sourceImageGreenTurret, -(turretWidth / 2), -(turretHeight / 2), turretWidth, turretHeight);
                    break;
                case 3:
                    e.Graphics.DrawImage(sourceImageLightGreenTurret, -(turretWidth / 2), -(turretHeight / 2), turretWidth, turretHeight);
                    break;
                case 4:
                    e.Graphics.DrawImage(sourceImageOrangeTurret, -(turretWidth / 2), -(turretHeight / 2), turretWidth, turretHeight);
                    break;
                case 5:
                    e.Graphics.DrawImage(sourceImagePurpleTurret, -(turretWidth / 2), -(turretHeight / 2), turretWidth, turretHeight);
                    break;
                case 6:
                    e.Graphics.DrawImage(sourceImageRedTurret, -(turretWidth / 2), -(turretHeight / 2), turretWidth, turretHeight);
                    break;
                case 7:
                    e.Graphics.DrawImage(sourceImageYellowTurret, -(turretWidth / 2), -(turretHeight / 2), turretWidth, turretHeight);
                    break;
            }
        }

        private void NameDrawer(object o, PaintEventArgs e)
        {
            Tank t = o as Tank;

            using (Font font1 = new Font("Times New Roman", 24, FontStyle.Bold, GraphicsUnit.Pixel))
            {
                PointF pointF1 = new PointF(-20 - t.name.Length * 5, 26);
                e.Graphics.DrawString(t.name + ": " + t.score, font1, Brushes.Blue, pointF1);
            }
        }

        private void HealthDrawer(object o, PaintEventArgs e)
        {
            Tank t = o as Tank;

            using (System.Drawing.SolidBrush greenBrush = new System.Drawing.SolidBrush(System.Drawing.Color.Green))
            using (System.Drawing.SolidBrush yellowBrush = new System.Drawing.SolidBrush(System.Drawing.Color.Yellow))
            using (System.Drawing.SolidBrush redBrush = new System.Drawing.SolidBrush(System.Drawing.Color.Red))
            {

            }
        }

        private void PowerUpDrawer(object o, PaintEventArgs e)
        {
            PowerUp p = o as PowerUp;

            int width = 8;
            int height = 8;
            e.Graphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
            using (System.Drawing.SolidBrush blackBrush = new System.Drawing.SolidBrush(System.Drawing.Color.Black))
            using (System.Drawing.SolidBrush redBrush = new System.Drawing.SolidBrush(System.Drawing.Color.Red))
            {
                Rectangle r = new Rectangle(-(width / 2), -(height / 2), width, height);
                e.Graphics.FillEllipse(blackBrush, r);
                r = new Rectangle(-(width / 4), -(height / 4), width / 2, height / 2);
                e.Graphics.FillEllipse(redBrush, r);
            }

        }

        private void BeamDrawer(object o, PaintEventArgs e)
        {
            Beam b = o as Beam;

            int width = 10;

            // Draw portion of source image
            Pen pen = new Pen(new SolidBrush(Color.White), width);
            e.Graphics.DrawLine(pen, 0, 0, 0, -Constants.ViewSize);


            b.beamFrames++;
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

            int SingleWidth = 50;
            int SingleHeight = 50;

            int Width = (int)Math.Abs(w.endPoint1.GetX() - w.endPoint2.GetX());
            int Height = (int)Math.Abs(w.endPoint1.GetY() - w.endPoint2.GetY());

            // Draw portion of source image
            for (int i = 0; i <= Width; i += SingleWidth)
            {
                for (int j = 0; j <= Height; j += SingleHeight)
                {
                    e.Graphics.DrawImage(sourceImageWall, -(Width/2) - (SingleWidth/2) + i, -(Height/2) - (SingleHeight/2) + j, SingleWidth, SingleHeight);
                }
            }
        }
    }
}

