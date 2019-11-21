using System;
using System.Drawing;
using System.Windows.Forms;
namespace TankWars
{
    public partial class TankWars : Form
    {
        private GameController TheController;

        DrawingPanel drawingPanel;

        public TankWars(GameController ctl)
        {
            InitializeComponent();
            TheController = ctl;

            TheController.OnFrameEvent += OnFrame;

            ClientSize = new Size(Constants.ViewSize, Constants.ViewSize);
            drawingPanel = new DrawingPanel(TheController);
            drawingPanel.BackColor = Color.Black;
            drawingPanel.Location = new Point(Constants.ViewLocationX, Constants.ViewLocationY);
            drawingPanel.Size = new Size(ClientSize.Width, ClientSize.Height);
            Controls.Add(drawingPanel);
        }

        private void OnFrame()
        {
            // Don't try to redraw if the window doesn't exist yet.
            // This might happen if the controller sends an update
            // before the Form has started.
            if (!IsHandleCreated)
                return;

            // Invalidate this form and all its children
            // This will cause the form to redraw as soon as it can
            MethodInvoker m = new MethodInvoker(() => Invalidate(true));
            this.Invoke(m);
        }

        private void ConnectButton_Click(object sender, EventArgs e)
        {
            TheController.TryConnect(NameInput.Text, ServerInput.Text, 11000);
        }

        // This method is invoked when the DrawingPanel needs to be re-drawn
        protected override void OnPaint(PaintEventArgs e)
        {
            ////do the player location stuff here TODO
            //double playerx = ...;
            //double playery = ...;

            //// calculate view/world size ratio
            //double ratio = (double)viewSize / (double)theController.TheWorld.worldSize;
            //int halfSizeScaled = (int)(theController.TheWorld.worldSize / 2.0 * ratio);

            //double inverseTranslateX = -WorldSpaceToImageSpace(theController.TheWorld.worldSize, playerX) + halfSizeScaled;
            //double inverseTranslateY = -WorldSpaceToImageSpace(theController.TheWorld.worldSize, playerY) + halfSizeScaled;

            //e.Graphics.TranslateTransform((float)inverseTranslateX, (float)inverseTranslateY);

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

            int colorID = TheController.GetColor(t.ID);

            switch (colorID)
            {
                case 0:
                    // Creat Bitmap object of image
                    Image sourceImageBlueTank = Image.FromFile(@"..\\..\\..\\Resources\Images\shot-white.png");
                    // Draw portion of source image
                    Rectangle sourceRectBlueTank = new Rectangle(0, 0, tankWidth, tankHeight);
                    e.Graphics.DrawImage(sourceImageBlueTank, 0, 0, sourceRectBlueTank, GraphicsUnit.Pixel);
                    break;
                case 1:
                    // Creat Bitmap object of image
                    Image sourceImageDarkTank = Image.FromFile(@"..\\..\\..\\Resources\Images\shot-white.png");
                    // Draw portion of source image
                    Rectangle sourceRectDarkTank = new Rectangle(0, 0, tankWidth, tankHeight);
                    e.Graphics.DrawImage(sourceImageDarkTank, 0, 0, sourceRectDarkTank, GraphicsUnit.Pixel);
                    break;
                case 2:
                    // Creat Bitmap object of image
                    Image sourceImageGreenTank = Image.FromFile(@"..\\..\\..\\Resources\Images\shot-white.png");
                    // Draw portion of source image
                    Rectangle sourceRectGreenTank = new Rectangle(0, 0, tankWidth, tankHeight);
                    e.Graphics.DrawImage(sourceImageGreenTank, 0, 0, sourceRectGreenTank, GraphicsUnit.Pixel);
                    break;
                case 3:
                    // Creat Bitmap object of image
                    Image sourceImageLightGreenTank = Image.FromFile(@"..\\..\\..\\Resources\Images\shot-white.png");
                    // Draw portion of source image
                    Rectangle sourceRectLightGreenTank = new Rectangle(0, 0, tankWidth, tankHeight);
                    e.Graphics.DrawImage(sourceImageLightGreenTank, 0, 0, sourceRectLightGreenTank, GraphicsUnit.Pixel);
                    break;
                case 4:
                    // Creat Bitmap object of image
                    Image sourceImageOrangeTank = Image.FromFile(@"..\\..\\..\\Resources\Images\shot-white.png");
                    // Draw portion of source image
                    Rectangle sourceRectOrangeTank = new Rectangle(0, 0, tankWidth, tankHeight);
                    e.Graphics.DrawImage(sourceImageOrangeTank, 0, 0, sourceRectOrangeTank, GraphicsUnit.Pixel);
                    break;
                case 5:
                    // Creat Bitmap object of image
                    Image sourceImagePurpleTank = Image.FromFile(@"..\\..\\..\\Resources\Images\shot-white.png");
                    // Draw portion of source image
                    Rectangle sourceRectPurpleTank = new Rectangle(0, 0, tankWidth, tankHeight);
                    e.Graphics.DrawImage(sourceImagePurpleTank, 0, 0, sourceRectPurpleTank, GraphicsUnit.Pixel);
                    break;
                case 6:
                    // Creat Bitmap object of image
                    Image sourceImageRedTank = Image.FromFile(@"..\\..\\..\\Resources\Images\shot-white.png");
                    // Draw portion of source image
                    Rectangle sourceRectRedTank = new Rectangle(0, 0, tankWidth, tankHeight);
                    e.Graphics.DrawImage(sourceImageRedTank, 0, 0, sourceRectRedTank, GraphicsUnit.Pixel);
                    break;
                case 7:
                    // Creat Bitmap object of image
                    Image sourceImageYellowTank = Image.FromFile(@"..\\..\\..\\Resources\Images\shot-white.png");
                    // Draw portion of source image
                    Rectangle sourceRectYellowTank = new Rectangle(0, 0, tankWidth, tankHeight);
                    e.Graphics.DrawImage(sourceImageYellowTank, 0, 0, sourceRectYellowTank, GraphicsUnit.Pixel);
                    break;

            }
            
        }

        private void PowerUpDrawer(object o, PaintEventArgs e)
        {
            PowerUp p = o as PowerUp;

            int width = 8;
            int height = 8;

            // Creat Bitmap object of image
            //Image sourceImage = Image.FromFile(@"..\\..\\..\\Resources\Images\RedTurret.png");
            // Draw portion of source image
            //Rectangle sourceRect = new Rectangle(0, 0, width, height);
            //e.Graphics.DrawImage(sourceImage, 0, 0, sourceRect, GraphicsUnit.Pixel);
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

        private void TankWars_KeyDown(object sender, KeyEventArgs e)
        {
            if(e.KeyCode.Equals(Keys.W))
                TheController.commands.direction = "up";
            else if (e.KeyCode.Equals(Keys.S))
                TheController.commands.direction = "down";
            else if (e.KeyCode.Equals(Keys.A))
                TheController.commands.direction = "left";
            else if (e.KeyCode.Equals(Keys.D))
                TheController.commands.direction = "right";
        }

        private void TankWars_KeyUp(object sender, KeyEventArgs e)
        {
            TheController.commands.direction = "none";
        }
    }
}