namespace View
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
            this.panel1 = new System.Windows.Forms.Panel();
            this.SuspendLayout();
            // 
            // ServerText
            // 
            this.ServerText.AutoSize = true;
            this.ServerText.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ServerText.Location = new System.Drawing.Point(13, 13);
            this.ServerText.Name = "ServerText";
            this.ServerText.Size = new System.Drawing.Size(63, 20);
            this.ServerText.TabIndex = 0;
            this.ServerText.Text = "Server:";
            // 
            // ServerInput
            // 
            this.ServerInput.Location = new System.Drawing.Point(82, 15);
            this.ServerInput.Name = "ServerInput";
            this.ServerInput.Size = new System.Drawing.Size(152, 20);
            this.ServerInput.TabIndex = 1;
            // 
            // NameText
            // 
            this.NameText.AutoSize = true;
            this.NameText.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.NameText.Location = new System.Drawing.Point(249, 13);
            this.NameText.Name = "NameText";
            this.NameText.Size = new System.Drawing.Size(58, 20);
            this.NameText.TabIndex = 2;
            this.NameText.Text = "Name:";
            // 
            // NameInput
            // 
            this.NameInput.Location = new System.Drawing.Point(313, 15);
            this.NameInput.Name = "NameInput";
            this.NameInput.Size = new System.Drawing.Size(152, 20);
            this.NameInput.TabIndex = 3;
            // 
            // ConnectButton
            // 
            this.ConnectButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ConnectButton.Location = new System.Drawing.Point(504, 10);
            this.ConnectButton.Name = "ConnectButton";
            this.ConnectButton.Size = new System.Drawing.Size(132, 26);
            this.ConnectButton.TabIndex = 4;
            this.ConnectButton.Text = "Connect";
            this.ConnectButton.UseVisualStyleBackColor = true;
            // 
            // panel1
            // 
            this.panel1.Location = new System.Drawing.Point(10, 45);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(800, 800);
            this.panel1.TabIndex = 5;
            // 
            // TankWars
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(822, 853);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.ConnectButton);
            this.Controls.Add(this.NameInput);
            this.Controls.Add(this.NameText);
            this.Controls.Add(this.ServerInput);
            this.Controls.Add(this.ServerText);
            this.Name = "TankWars";
            this.Text = "TankWars";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label ServerText;
        private System.Windows.Forms.TextBox ServerInput;
        private System.Windows.Forms.Label NameText;
        private System.Windows.Forms.TextBox NameInput;
        private System.Windows.Forms.Button ConnectButton;
        private System.Windows.Forms.Panel panel1;
    }
}

