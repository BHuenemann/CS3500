namespace SpreadsheetGUI
{
    partial class InitialForm
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
            this.SpreadsheetGrid = new SS.SpreadsheetPanel();
            this.CellNameBox = new System.Windows.Forms.TextBox();
            this.CellValueBox = new System.Windows.Forms.TextBox();
            this.CellContentsBox = new System.Windows.Forms.TextBox();
            this.Menu = new System.Windows.Forms.MenuStrip();
            this.fileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.newToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.saveToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.saveAsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.openToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem1 = new System.Windows.Forms.ToolStripSeparator();
            this.closeToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.helpToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.SaveDialogBox = new System.Windows.Forms.SaveFileDialog();
            this.OpenDialogBox = new System.Windows.Forms.OpenFileDialog();
            this.Menu.SuspendLayout();
            this.SuspendLayout();
            // 
            // SpreadsheetGrid
            // 
            this.SpreadsheetGrid.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.SpreadsheetGrid.Location = new System.Drawing.Point(4, 51);
            this.SpreadsheetGrid.Margin = new System.Windows.Forms.Padding(2);
            this.SpreadsheetGrid.Name = "SpreadsheetGrid";
            this.SpreadsheetGrid.Size = new System.Drawing.Size(572, 306);
            this.SpreadsheetGrid.TabIndex = 0;
            // 
            // CellNameBox
            // 
            this.CellNameBox.Location = new System.Drawing.Point(4, 25);
            this.CellNameBox.Margin = new System.Windows.Forms.Padding(2);
            this.CellNameBox.Name = "CellNameBox";
            this.CellNameBox.ReadOnly = true;
            this.CellNameBox.Size = new System.Drawing.Size(76, 20);
            this.CellNameBox.TabIndex = 3;
            // 
            // CellValueBox
            // 
            this.CellValueBox.Location = new System.Drawing.Point(84, 25);
            this.CellValueBox.Margin = new System.Windows.Forms.Padding(2);
            this.CellValueBox.Name = "CellValueBox";
            this.CellValueBox.ReadOnly = true;
            this.CellValueBox.Size = new System.Drawing.Size(69, 20);
            this.CellValueBox.TabIndex = 4;
            // 
            // CellContentsBox
            // 
            this.CellContentsBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.CellContentsBox.Location = new System.Drawing.Point(157, 25);
            this.CellContentsBox.Margin = new System.Windows.Forms.Padding(2);
            this.CellContentsBox.Name = "CellContentsBox";
            this.CellContentsBox.Size = new System.Drawing.Size(420, 20);
            this.CellContentsBox.TabIndex = 0;
            this.CellContentsBox.KeyDown += new System.Windows.Forms.KeyEventHandler(this.CellContentsBox_KeyDown);
            // 
            // Menu
            // 
            this.Menu.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.Menu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.fileToolStripMenuItem,
            this.helpToolStripMenuItem});
            this.Menu.Location = new System.Drawing.Point(0, 0);
            this.Menu.Name = "Menu";
            this.Menu.Padding = new System.Windows.Forms.Padding(4, 2, 0, 2);
            this.Menu.Size = new System.Drawing.Size(585, 24);
            this.Menu.TabIndex = 6;
            this.Menu.Text = "menuStrip1";
            // 
            // fileToolStripMenuItem
            // 
            this.fileToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.newToolStripMenuItem,
            this.saveToolStripMenuItem,
            this.saveAsToolStripMenuItem,
            this.openToolStripMenuItem,
            this.toolStripMenuItem1,
            this.closeToolStripMenuItem});
            this.fileToolStripMenuItem.Name = "fileToolStripMenuItem";
            this.fileToolStripMenuItem.Size = new System.Drawing.Size(37, 20);
            this.fileToolStripMenuItem.Text = "File";
            // 
            // newToolStripMenuItem
            // 
            this.newToolStripMenuItem.Name = "newToolStripMenuItem";
            this.newToolStripMenuItem.Size = new System.Drawing.Size(114, 22);
            this.newToolStripMenuItem.Text = "New";
            this.newToolStripMenuItem.Click += new System.EventHandler(this.NewToolStripMenuItem_Click);
            // 
            // saveToolStripMenuItem
            // 
            this.saveToolStripMenuItem.Name = "saveToolStripMenuItem";
            this.saveToolStripMenuItem.Size = new System.Drawing.Size(114, 22);
            this.saveToolStripMenuItem.Text = "Save";
            this.saveToolStripMenuItem.Click += new System.EventHandler(this.SaveToolStripMenuItem_Click);
            // 
            // saveAsToolStripMenuItem
            // 
            this.saveAsToolStripMenuItem.Name = "saveAsToolStripMenuItem";
            this.saveAsToolStripMenuItem.Size = new System.Drawing.Size(114, 22);
            this.saveAsToolStripMenuItem.Text = "Save As";
            this.saveAsToolStripMenuItem.Click += new System.EventHandler(this.SaveAsToolStripMenuItem_Click);
            // 
            // openToolStripMenuItem
            // 
            this.openToolStripMenuItem.Name = "openToolStripMenuItem";
            this.openToolStripMenuItem.Size = new System.Drawing.Size(114, 22);
            this.openToolStripMenuItem.Text = "Open";
            this.openToolStripMenuItem.Click += new System.EventHandler(this.OpenToolStripMenuItem_Click);
            // 
            // toolStripMenuItem1
            // 
            this.toolStripMenuItem1.Name = "toolStripMenuItem1";
            this.toolStripMenuItem1.Size = new System.Drawing.Size(111, 6);
            // 
            // closeToolStripMenuItem
            // 
            this.closeToolStripMenuItem.Name = "closeToolStripMenuItem";
            this.closeToolStripMenuItem.Size = new System.Drawing.Size(114, 22);
            this.closeToolStripMenuItem.Text = "Close";
            this.closeToolStripMenuItem.Click += new System.EventHandler(this.CloseToolStripMenuItem_Click);
            // 
            // helpToolStripMenuItem
            // 
            this.helpToolStripMenuItem.Name = "helpToolStripMenuItem";
            this.helpToolStripMenuItem.Size = new System.Drawing.Size(44, 20);
            this.helpToolStripMenuItem.Text = "Help";
            // 
            // SaveDialogBox
            // 
            this.SaveDialogBox.Filter = "Spreadsheet|*.sprd|All Files|*.*";
            this.SaveDialogBox.Title = "Save your spreadsheet";
            // 
            // OpenDialogBox
            // 
            this.OpenDialogBox.FileName = "OpenDialogBox";
            this.OpenDialogBox.Filter = "Spreadsheet|*.sprd|All Files|*.*";
            this.OpenDialogBox.Title = "Open a spreadsheet file";
            // 
            // InitialForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(585, 363);
            this.Controls.Add(this.CellContentsBox);
            this.Controls.Add(this.CellValueBox);
            this.Controls.Add(this.CellNameBox);
            this.Controls.Add(this.SpreadsheetGrid);
            this.Controls.Add(this.Menu);
            this.KeyPreview = true;
            this.Margin = new System.Windows.Forms.Padding(2);
            this.Name = "InitialForm";
            this.Text = "Form1";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.InitialForm_FormClosing);
            this.Menu.ResumeLayout(false);
            this.Menu.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private SS.SpreadsheetPanel SpreadsheetGrid;
        private System.Windows.Forms.TextBox CellNameBox;
        private System.Windows.Forms.TextBox CellValueBox;
        private System.Windows.Forms.TextBox CellContentsBox;
        private System.Windows.Forms.MenuStrip Menu;
        private System.Windows.Forms.ToolStripMenuItem fileToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem newToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem saveToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem openToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripMenuItem1;
        private System.Windows.Forms.ToolStripMenuItem closeToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem helpToolStripMenuItem;
        private System.Windows.Forms.SaveFileDialog SaveDialogBox;
        private System.Windows.Forms.OpenFileDialog OpenDialogBox;
        private System.Windows.Forms.ToolStripMenuItem saveAsToolStripMenuItem;
    }
}

