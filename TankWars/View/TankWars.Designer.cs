namespace TankWars
{
    partial class TankWars
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.ServerText = new System.Windows.Forms.Label();
            this.ServerInput = new System.Windows.Forms.TextBox();
            this.NameText = new System.Windows.Forms.Label();
            this.NameInput = new System.Windows.Forms.TextBox();
            this.ConnectButton = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // ServerText
            // 
            this.ServerText.AutoSize = true;
            this.ServerText.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ServerText.Location = new System.Drawing.Point(13, 13);
            this.ServerText.Name = "ServerText";
            this.ServerText.Size = new System.Drawing.Size(54, 17);
            this.ServerText.TabIndex = 0;
            this.ServerText.Text = "Server:";
            // 
            // ServerInput
            // 
            this.ServerInput.Location = new System.Drawing.Point(82, 15);
            this.ServerInput.Name = "ServerInput";
            this.ServerInput.Size = new System.Drawing.Size(152, 20);
            this.ServerInput.TabIndex = 1;
            this.ServerInput.Text = "localhost";
            // 
            // NameText
            // 
            this.NameText.AutoSize = true;
            this.NameText.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.NameText.Location = new System.Drawing.Point(249, 13);
            this.NameText.Name = "NameText";
            this.NameText.Size = new System.Drawing.Size(49, 17);
            this.NameText.TabIndex = 2;
            this.NameText.Text = "Name:";
            // 
            // NameInput
            // 
            this.NameInput.Location = new System.Drawing.Point(313, 15);
            this.NameInput.Name = "NameInput";
            this.NameInput.Size = new System.Drawing.Size(152, 20);
            this.NameInput.TabIndex = 3;
            this.NameInput.Text = "player";
            // 
            // ConnectButton
            // 
            this.ConnectButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ConnectButton.Location = new System.Drawing.Point(504, 10);
            this.ConnectButton.Name = "ConnectButton";
            this.ConnectButton.Size = new System.Drawing.Size(132, 26);
            this.ConnectButton.TabIndex = 4;
            this.ConnectButton.TabStop = false;
            this.ConnectButton.Text = "Connect";
            this.ConnectButton.UseVisualStyleBackColor = true;
            this.ConnectButton.Click += new System.EventHandler(this.ConnectButton_Click);
            // 
            // TankWars
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSize = true;
            this.ClientSize = new System.Drawing.Size(884, 857);
            this.Controls.Add(this.ConnectButton);
            this.Controls.Add(this.NameInput);
            this.Controls.Add(this.NameText);
            this.Controls.Add(this.ServerInput);
            this.Controls.Add(this.ServerText);
            this.KeyPreview = true;
            this.Name = "TankWars";
            this.Text = "TankWars";
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.TankWars_KeyDown);
            this.KeyUp += new System.Windows.Forms.KeyEventHandler(this.TankWars_KeyUp);
            this.MouseDown += new System.Windows.Forms.MouseEventHandler(this.TankWars_MouseDown);
            this.MouseUp += new System.Windows.Forms.MouseEventHandler(this.TankWars_MouseUp);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label ServerText;
        private System.Windows.Forms.TextBox ServerInput;
        private System.Windows.Forms.Label NameText;
        private System.Windows.Forms.TextBox NameInput;
        private System.Windows.Forms.Button ConnectButton;
    }
}

