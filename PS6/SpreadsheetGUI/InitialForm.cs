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



        private bool SpreadsheetCellIsValid(string cellName)
        {
            return Regex.IsMatch(cellName, "^[a-zA-Z][1-9][0-9]?$");
        }



        private string SpreadsheetCellNormalizer(string s)
        {
            return s.ToUpper();
        }



        public InitialForm()
        {
            InitializeComponent();
            mainSpreadsheet = new Spreadsheet(SpreadsheetCellIsValid, SpreadsheetCellNormalizer, "ps6");
        }



        public void StartSaveDialogue()
        {
            if (SaveDialogBox.ShowDialog() == DialogResult.OK)
                SaveFile(SaveDialogBox.FileName);
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



        private void StartOpenDialog()
        {
            if (OpenDialogBox.ShowDialog() == DialogResult.OK)
                OpenFile(OpenDialogBox.FileName);
        }



        private void OpenFile(string FileName)
        {
            mainSpreadsheet = new Spreadsheet(FileName, SpreadsheetCellIsValid, SpreadsheetCellNormalizer, "ps6");
        }



        private void Form1_Load(object sender, EventArgs e)
        {

        }



        private void Filebar_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {

        }



        private void NewToolStripMenuItem_Click(object sender, EventArgs e)
        {
            mainSpreadsheet = new Spreadsheet(SpreadsheetCellIsValid, SpreadsheetCellNormalizer, "ps6");
        }



        private void SaveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            StartSaveDialogue();
        }



        private void OpenToolStripMenuItem_Click(object sender, EventArgs e)
        {
            StartOpenDialog();
        }


        private void OpenDialogBox_FileOk(object sender, CancelEventArgs e)
        {
            OpenFile(OpenDialogBox.FileName);
        }



        private void CloseToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Close();
        }



        private void SpreadsheetGrid_KeyDown(object sender, KeyEventArgs e)
        {
            Console.WriteLine(mainSpreadsheet.GetNamesOfAllNonemptyCells());
        }
    }
}
