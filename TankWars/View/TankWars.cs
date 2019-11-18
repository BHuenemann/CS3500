using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using View.Properties;

namespace TankWars
{
    public partial class TankWars : Form
    {
        private GameController theController;


        public TankWars(GameController ctl)
        {
            InitializeComponent();
            theController = ctl;

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

        public void drawBackground(bool connected, string message)
        {
            if(!connected)
            {
                //dialog box
            }

            else
            {
                ViewPanel.BackgroundImage = Image.FromFile(@"C:\Users\Jonathan Wigderson\source\repos\u11903382\TankWars\Resources\Images\Background.jpg");
                //ViewPanel.BackgroundImage = Image.FromFile(@"C:\Users\Jonathan Wigderson\source\repos\u11903382\TankWars\Resources\Images\Background.jpg");
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

        private void ConnectButton_Click(object sender, EventArgs e)
        {
            theController.TryConnect(NameInput.Text, ServerInput.Text, 11000);

        }

        // This method is invoked when the DrawingPanel needs to be re-drawn
        protected override void OnPaint(PaintEventArgs e)
        {
            //do the player location stuff here TODO

            lock (theController.TheWorld)
            {
                // Draw the players
                foreach (Tank tank in theController.TheWorld.Tanks.Values)
                {
                    DrawObjectWithTransform(e, tank, this.Size.Width, tank.location.GetX(), tank.location.GetY(), tank.orientation.ToAngle(), 
                        TankDrawer);
                }

                // Draw the powerups
                foreach (PowerUp pow in theController.TheWorld.PowerUps.Values)
                {
                    DrawObjectWithTransform(e, pow, this.Size.Width, pow.location.GetX(), pow.location.GetY(), 0, PowerUpDrawer);
                }

                // Draw the beams
                foreach (Beam beam in theController.TheWorld.Beams.Values)
                {
                    DrawObjectWithTransform(e, beam, this.Size.Width, beam.origin.GetX(), beam.origin.GetY(), beam.origin.ToAngle(), BeamDrawer);
                }

                // Draw the projectiles
                foreach (Projectile proj in theController.TheWorld.Projectiles.Values)
                {
                    DrawObjectWithTransform(e, proj, this.Size.Width, proj.location.GetX(), proj.location.GetY(), proj.orientation.ToAngle(), ProjectileDrawer);
                }

                // Draw the walls
                foreach (Wall wall in theController.TheWorld.Walls.Values)
                {
                    //if x is same for p1 and p2 is same then vertically long
                    //if y is same for p1 and p2 is same then horizontally long
                    DrawObjectWithTransform(e, wall, this.Size.Width, (wall.endPoint1.GetX() + wall.endPoint2.GetX())/2, 
                        (wall.endPoint1.GetY() + wall.endPoint2.GetY()) / 2, 0, WallDrawer);
                }
            }

            // Do anything that Panel (from which we inherit) needs to do
            base.OnPaint(e);
        }

        private void TankDrawer(object o, PaintEventArgs e)
        {
            Tank t = o as Tank;
        }

        private void PowerUpDrawer(object o, PaintEventArgs e)
        {
            PowerUp p = o as PowerUp;
        }

        private void BeamDrawer(object o, PaintEventArgs e)
        {
            Beam b = o as Beam;
        }

        private void ProjectileDrawer(object o, PaintEventArgs e)
        {
            Projectile p = o as Projectile;
        }

        private void WallDrawer(object o, PaintEventArgs e)
        {
            Wall w = o as Wall;
        }
    }
}

