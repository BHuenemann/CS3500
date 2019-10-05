//Authors: Ben Huenemann and Jonathan Wigderson

namespace SpreadsheetGUI
{
    partial class HelpMenu
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
            this.CellEditingTitle = new System.Windows.Forms.Label();
            this.CellEditingText = new System.Windows.Forms.Label();
            this.FormulaTitle = new System.Windows.Forms.Label();
            this.FormulaText = new System.Windows.Forms.Label();
            this.UndoTitle = new System.Windows.Forms.Label();
            this.UndoText = new System.Windows.Forms.Label();
            this.NewOpenTitle = new System.Windows.Forms.Label();
            this.NewOpenText = new System.Windows.Forms.Label();
            this.SaveTitle = new System.Windows.Forms.Label();
            this.SaveText = new System.Windows.Forms.Label();
            this.ChangedTitle = new System.Windows.Forms.Label();
            this.ChangedText = new System.Windows.Forms.Label();
            this.CloseTitle = new System.Windows.Forms.Label();
            this.ClosedText = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // CellEditingTitle
            // 
            this.CellEditingTitle.AutoSize = true;
            this.CellEditingTitle.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.CellEditingTitle.Location = new System.Drawing.Point(13, 13);
            this.CellEditingTitle.Name = "CellEditingTitle";
            this.CellEditingTitle.Size = new System.Drawing.Size(117, 24);
            this.CellEditingTitle.TabIndex = 0;
            this.CellEditingTitle.Text = "Cell Editing";
            // 
            // CellEditingText
            // 
            this.CellEditingText.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.CellEditingText.Location = new System.Drawing.Point(42, 37);
            this.CellEditingText.Name = "CellEditingText";
            this.CellEditingText.Size = new System.Drawing.Size(486, 54);
            this.CellEditingText.TabIndex = 1;
            this.CellEditingText.Text = "Cell Editing Text";
            // 
            // FormulaTitle
            // 
            this.FormulaTitle.AutoSize = true;
            this.FormulaTitle.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.FormulaTitle.Location = new System.Drawing.Point(13, 91);
            this.FormulaTitle.Name = "FormulaTitle";
            this.FormulaTitle.Size = new System.Drawing.Size(87, 24);
            this.FormulaTitle.TabIndex = 2;
            this.FormulaTitle.Text = "Formula";
            // 
            // FormulaText
            // 
            this.FormulaText.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.FormulaText.Location = new System.Drawing.Point(42, 115);
            this.FormulaText.Name = "FormulaText";
            this.FormulaText.Size = new System.Drawing.Size(486, 44);
            this.FormulaText.TabIndex = 3;
            this.FormulaText.Text = "Formula Text";
            // 
            // UndoTitle
            // 
            this.UndoTitle.AutoSize = true;
            this.UndoTitle.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.UndoTitle.Location = new System.Drawing.Point(13, 159);
            this.UndoTitle.Name = "UndoTitle";
            this.UndoTitle.Size = new System.Drawing.Size(60, 24);
            this.UndoTitle.TabIndex = 4;
            this.UndoTitle.Text = "Undo";
            // 
            // UndoText
            // 
            this.UndoText.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.UndoText.Location = new System.Drawing.Point(42, 183);
            this.UndoText.Name = "UndoText";
            this.UndoText.Size = new System.Drawing.Size(486, 32);
            this.UndoText.TabIndex = 5;
            this.UndoText.Text = "Undo Text";
            // 
            // NewOpenTitle
            // 
            this.NewOpenTitle.AutoSize = true;
            this.NewOpenTitle.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.NewOpenTitle.Location = new System.Drawing.Point(13, 215);
            this.NewOpenTitle.Name = "NewOpenTitle";
            this.NewOpenTitle.Size = new System.Drawing.Size(151, 24);
            this.NewOpenTitle.TabIndex = 6;
            this.NewOpenTitle.Text = "New/Open File";
            // 
            // NewOpenText
            // 
            this.NewOpenText.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.NewOpenText.Location = new System.Drawing.Point(42, 239);
            this.NewOpenText.Name = "NewOpenText";
            this.NewOpenText.Size = new System.Drawing.Size(486, 55);
            this.NewOpenText.TabIndex = 7;
            this.NewOpenText.Text = "New Open Text";
            // 
            // SaveTitle
            // 
            this.SaveTitle.AutoSize = true;
            this.SaveTitle.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.SaveTitle.Location = new System.Drawing.Point(13, 294);
            this.SaveTitle.Name = "SaveTitle";
            this.SaveTitle.Size = new System.Drawing.Size(132, 24);
            this.SaveTitle.TabIndex = 8;
            this.SaveTitle.Text = "Save/SaveAs";
            // 
            // SaveText
            // 
            this.SaveText.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.SaveText.Location = new System.Drawing.Point(42, 318);
            this.SaveText.Name = "SaveText";
            this.SaveText.Size = new System.Drawing.Size(494, 70);
            this.SaveText.TabIndex = 9;
            this.SaveText.Text = "Save Text";
            // 
            // ChangedTitle
            // 
            this.ChangedTitle.AutoSize = true;
            this.ChangedTitle.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ChangedTitle.Location = new System.Drawing.Point(13, 456);
            this.ChangedTitle.Name = "ChangedTitle";
            this.ChangedTitle.Size = new System.Drawing.Size(95, 24);
            this.ChangedTitle.TabIndex = 10;
            this.ChangedTitle.Text = "Changed";
            // 
            // ChangedText
            // 
            this.ChangedText.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.ChangedText.Location = new System.Drawing.Point(42, 480);
            this.ChangedText.Name = "ChangedText";
            this.ChangedText.Size = new System.Drawing.Size(486, 17);
            this.ChangedText.TabIndex = 11;
            this.ChangedText.Text = "Changed Text";
            // 
            // CloseTitle
            // 
            this.CloseTitle.AutoSize = true;
            this.CloseTitle.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.CloseTitle.Location = new System.Drawing.Point(13, 388);
            this.CloseTitle.Name = "CloseTitle";
            this.CloseTitle.Size = new System.Drawing.Size(104, 24);
            this.CloseTitle.TabIndex = 12;
            this.CloseTitle.Text = "Close File";
            // 
            // ClosedText
            // 
            this.ClosedText.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.ClosedText.Location = new System.Drawing.Point(42, 412);
            this.ClosedText.Name = "ClosedText";
            this.ClosedText.Size = new System.Drawing.Size(486, 44);
            this.ClosedText.TabIndex = 13;
            this.ClosedText.Text = "ClosedText";
            // 
            // HelpMenu
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(540, 514);
            this.Controls.Add(this.ClosedText);
            this.Controls.Add(this.CloseTitle);
            this.Controls.Add(this.ChangedText);
            this.Controls.Add(this.ChangedTitle);
            this.Controls.Add(this.SaveText);
            this.Controls.Add(this.SaveTitle);
            this.Controls.Add(this.NewOpenText);
            this.Controls.Add(this.NewOpenTitle);
            this.Controls.Add(this.UndoText);
            this.Controls.Add(this.UndoTitle);
            this.Controls.Add(this.FormulaText);
            this.Controls.Add(this.FormulaTitle);
            this.Controls.Add(this.CellEditingText);
            this.Controls.Add(this.CellEditingTitle);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Name = "HelpMenu";
            this.Text = "Help";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label CellEditingTitle;
        private System.Windows.Forms.Label CellEditingText;
        private System.Windows.Forms.Label FormulaTitle;
        private System.Windows.Forms.Label FormulaText;
        private System.Windows.Forms.Label UndoTitle;
        private System.Windows.Forms.Label UndoText;
        private System.Windows.Forms.Label NewOpenTitle;
        private System.Windows.Forms.Label NewOpenText;
        private System.Windows.Forms.Label SaveTitle;
        private System.Windows.Forms.Label SaveText;
        private System.Windows.Forms.Label ChangedTitle;
        private System.Windows.Forms.Label ChangedText;
        private System.Windows.Forms.Label CloseTitle;
        private System.Windows.Forms.Label ClosedText;
    }
}