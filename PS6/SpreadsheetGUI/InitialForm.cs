using SS;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SpreadsheetGUI
{
    public partial class InitialForm : Form
    {
        public Spreadsheet mainSpreadsheet;
        private string FileName = "";



        private bool SpreadsheetCellIsValid(string cellName)
        {
            return Regex.IsMatch(cellName, "^[a-zA-Z][1-9][0-9]?$");
        }



        private string SpreadsheetCellNormalizer(string s)
        {
            return s.ToUpper();
        }



        public void StartSaveDialog()
        {
            try
            {
                SaveDialogBox.OverwritePrompt = !(File.Exists(SaveDialogBox.FileName) && SaveDialogBox.FileName == FileName);

                if (SaveDialogBox.ShowDialog() == DialogResult.OK)
                {
                    if (File.Exists(SaveDialogBox.FileName) && SaveDialogBox.FileName == FileName)
                        File.Delete(SaveDialogBox.FileName);

                    SaveFile(SaveDialogBox.FileName);
                }
            }
            catch(Exception)
            {
                DialogResult = MessageBox.Show("An error occured while trying to save the file", "Save Spreadsheet Error", MessageBoxButtons.OK);
            }
        }



        private void SaveFile(string FileName)
        {
            if (SaveDialogBox.FilterIndex == 2 || FileName.Substring(FileName.Length - 5) == ".sprd")
            {
                mainSpreadsheet.Save(FileName);
                this.FileName = SaveDialogBox.FileName;
            }
            else
            {
                mainSpreadsheet.Save(FileName);
                this.FileName = SaveDialogBox.FileName + ".sprd";
            }
        }



        private void StartOpenDialog()
        {
            try
            {
                if (OpenDialogBox.ShowDialog() == DialogResult.OK)
                    OpenFile(OpenDialogBox.FileName);
            }
            catch (SpreadsheetReadWriteException)
            {
                DialogResult = MessageBox.Show("File is in an incorrect format", "Open Spreadsheet Error", MessageBoxButtons.OK);
            }
            catch (Exception)
            {
                DialogResult = MessageBox.Show("Error while trying to open the file", "Open Spreadsheet Error", MessageBoxButtons.OK);
            }
        }



        private void OpenFile(string FileName)
        {
            mainSpreadsheet = new Spreadsheet(FileName, SpreadsheetCellIsValid, SpreadsheetCellNormalizer, "ps6");
            this.FileName = FileName;
        }



        private bool UnsavedWarning()
        {
            if (mainSpreadsheet.Changed)
            {
                DialogResult warningBox = MessageBox.Show("File contains unsaved changes. Would you like to save your work?", "Unsaved Changes", MessageBoxButtons.YesNoCancel);
                switch (warningBox)
                {
                    case DialogResult.Yes:
                        StartSaveDialog();
                        return false;

                    case DialogResult.No:
                        return false;

                    case DialogResult.Cancel:
                        return true;
                }
            }

            return false;
        }



        public InitialForm()
        {
            InitializeComponent();
            mainSpreadsheet = new Spreadsheet(SpreadsheetCellIsValid, SpreadsheetCellNormalizer, "ps6");
        }



        private void Form1_Load(object sender, EventArgs e)
        {

        }



        private void NewToolStripMenuItem_Click(object sender, EventArgs e)
        {
            mainSpreadsheet = new Spreadsheet(SpreadsheetCellIsValid, SpreadsheetCellNormalizer, "ps6");
        }



        private void SaveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            StartSaveDialog();
        }



        private void OpenToolStripMenuItem_Click(object sender, EventArgs e)
        {
            StartOpenDialog();
        }



        private void CloseToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Close();
        }



        private void InitialForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (UnsavedWarning())
                e.Cancel = true;
        }
    }
}
