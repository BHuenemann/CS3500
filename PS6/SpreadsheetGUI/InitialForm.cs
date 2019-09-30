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
    public partial class Form1 : Form
    {
        private Spreadsheet mainSpreadsheet;



        private bool SpreadsheetCellIsValid(string cellName)
        {
            return Regex.IsMatch(cellName, "^[a-zA-Z][1-9][0-9]?$");
        }



        private string SpreadsheetCellNormalizer(string s)
        {
            return s.ToUpper();
        }



        public Form1()
        {
            InitializeComponent();
            mainSpreadsheet = new Spreadsheet(SpreadsheetCellIsValid, SpreadsheetCellNormalizer, "ps6");
        }



        public void OpenSaveDialogue()
        {
            Stream stream;
            SaveFileDialog saveWindow = new SaveFileDialog();

            saveWindow.Filter = "Spreadsheet|*.sprd|All Files|*.*";
            saveWindow.Title = "Save your spreadsheet";

            if(saveWindow.ShowDialog() == DialogResult.OK)
            {
                if((stream = saveWindow.OpenFile()) != null)
                {
                    stream.Close();
                }
            }

            saveWindow.Dispose();
        }



        private void SaveFile(string FileName)
        {
            if (SaveDialogBox.FilterIndex == 1 || FileName.Substring(FileName.Length - 5) == ".sprd")
            {
                mainSpreadsheet.Save(SaveDialogBox.FileName);
            }
            else
            {
                mainSpreadsheet.Save(SaveDialogBox.FileName + ".sprd");
            }
        }



        private void Form1_Load(object sender, EventArgs e)
        {

        }



        private void Filebar_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {

        }



        private void CloseToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Form WarningBox = new Form();

            this.Close();
        }



        private void SaveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OpenSaveDialogue();
        }



        private void SaveDialogBox_FileOk(object sender, CancelEventArgs e)
        {
            SaveFile(SaveDialogBox.FileName);
        }
    }
}
