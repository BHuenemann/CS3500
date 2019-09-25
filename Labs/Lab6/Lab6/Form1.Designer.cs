namespace Lab6
{
    partial class Form1
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
            this.EnterBillLabel = new System.Windows.Forms.Label();
            this.BillTextBox = new System.Windows.Forms.TextBox();
            this.TipTextBox = new System.Windows.Forms.TextBox();
            this.ComputeTip = new System.Windows.Forms.Button();
            this.TipBox = new System.Windows.Forms.TextBox();
            this.TipText = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.TotalBox = new System.Windows.Forms.TextBox();
            this.TotalText = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // EnterBillLabel
            // 
            this.EnterBillLabel.AutoSize = true;
            this.EnterBillLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 15F);
            this.EnterBillLabel.Location = new System.Drawing.Point(148, 143);
            this.EnterBillLabel.Name = "EnterBillLabel";
            this.EnterBillLabel.Size = new System.Drawing.Size(137, 25);
            this.EnterBillLabel.TabIndex = 0;
            this.EnterBillLabel.Text = "Enter Total Bill";
            // 
            // BillTextBox
            // 
            this.BillTextBox.Location = new System.Drawing.Point(341, 148);
            this.BillTextBox.Name = "BillTextBox";
            this.BillTextBox.Size = new System.Drawing.Size(332, 20);
            this.BillTextBox.TabIndex = 1;
            this.BillTextBox.TextChanged += new System.EventHandler(this.BillTextBox_TextChanged);
            // 
            // TipTextBox
            // 
            this.TipTextBox.Location = new System.Drawing.Point(341, 258);
            this.TipTextBox.Name = "TipTextBox";
            this.TipTextBox.Size = new System.Drawing.Size(332, 20);
            this.TipTextBox.TabIndex = 2;
            // 
            // ComputeTip
            // 
            this.ComputeTip.Enabled = false;
            this.ComputeTip.Font = new System.Drawing.Font("Microsoft Sans Serif", 15F);
            this.ComputeTip.ForeColor = System.Drawing.Color.Green;
            this.ComputeTip.Location = new System.Drawing.Point(143, 252);
            this.ComputeTip.Name = "ComputeTip";
            this.ComputeTip.Size = new System.Drawing.Size(146, 34);
            this.ComputeTip.TabIndex = 3;
            this.ComputeTip.Text = "Compute Tip";
            this.ComputeTip.UseVisualStyleBackColor = true;
            this.ComputeTip.Click += new System.EventHandler(this.ComputeTip_Click);
            // 
            // TipBox
            // 
            this.TipBox.Location = new System.Drawing.Point(341, 205);
            this.TipBox.Name = "TipBox";
            this.TipBox.Size = new System.Drawing.Size(271, 20);
            this.TipBox.TabIndex = 4;
            this.TipBox.TextChanged += new System.EventHandler(this.TipBox_TextChanged);
            // 
            // TipText
            // 
            this.TipText.AutoSize = true;
            this.TipText.Font = new System.Drawing.Font("Microsoft Sans Serif", 15F);
            this.TipText.ForeColor = System.Drawing.Color.Red;
            this.TipText.Location = new System.Drawing.Point(148, 200);
            this.TipText.Name = "TipText";
            this.TipText.Size = new System.Drawing.Size(145, 25);
            this.TipText.TabIndex = 5;
            this.TipText.Text = "Tip Percentage";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 15F);
            this.label1.Location = new System.Drawing.Point(628, 205);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(30, 25);
            this.label1.TabIndex = 6;
            this.label1.Text = "%";
            // 
            // TotalBox
            // 
            this.TotalBox.Location = new System.Drawing.Point(341, 312);
            this.TotalBox.Name = "TotalBox";
            this.TotalBox.Size = new System.Drawing.Size(332, 20);
            this.TotalBox.TabIndex = 7;
            // 
            // TotalText
            // 
            this.TotalText.AutoSize = true;
            this.TotalText.Font = new System.Drawing.Font("Microsoft Sans Serif", 15F);
            this.TotalText.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(64)))), ((int)(((byte)(0)))));
            this.TotalText.Location = new System.Drawing.Point(184, 307);
            this.TotalText.Name = "TotalText";
            this.TotalText.Size = new System.Drawing.Size(56, 25);
            this.TotalText.TabIndex = 8;
            this.TotalText.Text = "Total";
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.TotalText);
            this.Controls.Add(this.TotalBox);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.TipText);
            this.Controls.Add(this.TipBox);
            this.Controls.Add(this.ComputeTip);
            this.Controls.Add(this.TipTextBox);
            this.Controls.Add(this.BillTextBox);
            this.Controls.Add(this.EnterBillLabel);
            this.Name = "Form1";
            this.Text = "Form1";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label EnterBillLabel;
        private System.Windows.Forms.TextBox BillTextBox;
        private System.Windows.Forms.TextBox TipTextBox;
        private System.Windows.Forms.Button ComputeTip;
        private System.Windows.Forms.TextBox TipBox;
        private System.Windows.Forms.Label TipText;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox TotalBox;
        private System.Windows.Forms.Label TotalText;
    }
}

