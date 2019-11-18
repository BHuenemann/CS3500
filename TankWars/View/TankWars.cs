﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

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
            theController.ConnectPlayer(NameInput.Text, ServerInput.Text, 11000);
        }

        // This method is invoked when the DrawingPanel needs to be re-drawn
        protected override void OnPaint(PaintEventArgs e)
        {
            lock (theController.TheWorld)
            {
                // Draw the players
                foreach (Tank tank in theController.TheWorld.Tanks.Values)
                {
                    DrawObjectWithTransform(e, tank, this.Size.Width, tank.GetLocation().GetX(), tank.GetLocation().GetY(), tank.GetOrientation().ToAngle(), 
                        TankDrawer(tank, e);
                }

                // Draw the powerups
                foreach (PowerUp pow in theController.TheWorld.PowerUps.Values)
                {
                    DrawObjectWithTransform(e, pow, this.Size.Width, pow.GetLocation().GetX(), pow.GetLocation().GetY(), 0, PowerupDrawer);
                }

                // Draw the powerups
                foreach (Beam beam in theController.TheWorld.Beams.Values)
                {
                    DrawObjectWithTransform(e, beam, this.Size.Width, beam.GetLocation().GetX(), beam.GetLocation().GetY(), 0, BeamDrawer);
                }

                // Draw the powerups
                foreach (Projectile proj in theController.TheWorld.Projectiles.Values)
                {
                    DrawObjectWithTransform(e, proj, this.Size.Width, proj.GetLocation().GetX(), proj.GetLocation().GetY(), 0, ProjectileDrawer);
                }

                // Draw the powerups
                foreach (Wall wall in theController.TheWorld.Walls.Values)
                {
                    DrawObjectWithTransform(e, wall, this.Size.Width, wall.GetLocation().GetX(), wall.GetLocation().GetY(), 0, WallDrawer);
                }
            }

            // Do anything that Panel (from which we inherit) needs to do
            base.OnPaint(e);
        }
    }

    /// <summary>
    /// Scrap example for referance
    /// </summary>
    /// <param name="o"></param>
    /// <param name="e"></param>
    private void TankDrawer(object o, PaintEventArgs e)
        {
            Tank t = o as Tank;
            if(t.ID == 9)
            {

            }
            if(t.ID == ...)
                color = ...;
            else if(...)
                color = ...;
            Rectangle r = new Rectangle(-(tankWidth / 2), -(tankWidth / 2), tankWidth, tankWidth);
            e.Graphics.FillRectangle(someBrush, r);
        }
    }
}
