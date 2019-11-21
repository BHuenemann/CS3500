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

            this.drawingPanel.MouseDown += new MouseEventHandler(this.TankWars_MouseDown);
            this.drawingPanel.MouseUp += new MouseEventHandler(this.TankWars_MouseUp);

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

        private void TankWars_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button.Equals(MouseButtons.Left))
                TheController.commands.fire = "main";
            else if (e.Button.Equals(MouseButtons.Right))
                TheController.commands.fire = "alt";
        }

        private void TankWars_MouseUp(object sender, MouseEventArgs e)
        {
            TheController.commands.fire = "none";
        }
    }
}