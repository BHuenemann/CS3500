using SpreadsheetUtilities;
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
        private int previousCol = 0;
        private int previousRow = 0;



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
            SpreadsheetGrid.SelectionChanged += OnSelectionChanged;

            SpreadsheetGrid.GetSelection(out int col, out int row);
            int selectedRow = row + 1;
            CellNameBox.Text = selectedCellName(col, row);
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

        //Jon's Code Methods

        private void OnSelectionChanged(SpreadsheetPanel spreadSheet)
        {
            spreadSheet.GetSelection(out int col, out int row);

            updateCells(previousCol, previousRow);

            spreadSheet.SetSelection(col, row);
            previousCol = col;
            previousRow = row;
            string cellName = selectedCellName(col, row);

            CellNameBox.Text = cellName;
            CellValueBox.Text = mainSpreadsheet.GetCellValue(cellName).ToString();

            if (mainSpreadsheet.GetCellContents(cellName) is Formula)
            {
                CellContentsBox.Text = "=" + mainSpreadsheet.GetCellContents(cellName).ToString();
            }
            else
            {
                CellContentsBox.Text = mainSpreadsheet.GetCellContents(cellName).ToString();
            }
            CellContentsBox.Focus();

            CellContentsBox.SelectionStart = CellContentsBox.Text.Length + 1;

        }


        private void CellContentsBox_KeyDown(object sender, KeyEventArgs e)
        {
            SpreadsheetGrid.GetSelection(out int col, out int row);

            if (e.KeyCode == Keys.Enter)
            {
                updateCells(col, row);
            }

            if (e.KeyCode == Keys.Up)
            {
                updateCells(col, row);
                string cellName = null;
                if (row != 0)
                {
                    previousCol = col;
                    previousRow = row - 1;
                    SpreadsheetGrid.SetSelection(col, row - 1);
                    cellName = selectedCellName(col, row - 1);
                }
                else
                {
                    cellName = selectedCellName(col, row);

                }

                visualUpdate(cellName);
            }

            if (e.KeyCode == Keys.Down)
            {
                updateCells(col, row);
                string cellName = null;
                if (row != 98)
                {
                    previousCol = col;
                    previousRow = row + 1;
                    SpreadsheetGrid.SetSelection(col, row + 1);
                    cellName = selectedCellName(col, row + 1);
                }
                else
                {
                    cellName = selectedCellName(col, row);
                }

                visualUpdate(cellName);
            }

            if (e.KeyCode == Keys.Left)
            {
                updateCells(col, row);
                string cellName = null;
                if (col != 0)
                {
                    previousCol = col - 1;
                    previousRow = row;
                    SpreadsheetGrid.SetSelection(col - 1, row);
                    cellName = selectedCellName(col - 1, row);
                }
                else
                {
                    cellName = selectedCellName(col, row);

                }

                visualUpdate(cellName);
            }

            if (e.KeyCode == Keys.Right)
            {
                updateCells(col, row);
                string cellName = null;
                if (col != 25)
                {
                    previousCol = col + 1;
                    previousRow = row;
                    SpreadsheetGrid.SetSelection(col + 1, row);
                    cellName = selectedCellName(col + 1, row);
                }
                else
                {

                    cellName = selectedCellName(col, row);

                }

                visualUpdate(cellName);
            }

            CellContentsBox.SelectionStart = CellContentsBox.Text.Length + 1;
        }

        private String colNumberToLetter(int col)
        {
            Char c = (Char)((true ? 65 : 97) + (col));

            return c.ToString();
        }

        private String selectedCellName(int col, int row)
        {
            row = row + 1;
            return colNumberToLetter(col) + row.ToString();
        }

        private void updateCells(int col, int row)
        {
            string cellName = selectedCellName(col, row);

            foreach (string cell in mainSpreadsheet.SetContentsOfCell(cellName, CellContentsBox.Text))
            {
                char colToChange = cell[0];
                int rowToChange = Int32.Parse(cell.Substring(1)) - 1;
                int letterToNumberCol = char.ToUpper(colToChange) - 65;


                SpreadsheetGrid.SetValue(letterToNumberCol, rowToChange, mainSpreadsheet.GetCellValue(cell).ToString());

            }

            SpreadsheetGrid.SetSelection(col, row);
            // Visual Change in cell
            SpreadsheetGrid.SetValue(col, row, mainSpreadsheet.GetCellValue(cellName).ToString());

            CellValueBox.Text = mainSpreadsheet.GetCellValue(cellName).ToString();

            if(mainSpreadsheet.GetCellContents(cellName) is Formula)
            {
                CellContentsBox.Text = "=" + mainSpreadsheet.GetCellContents(cellName).ToString();
            }
            else
            {
                CellContentsBox.Text = mainSpreadsheet.GetCellContents(cellName).ToString();
            }
        }

        private void visualUpdate(string cellName)
        {
            CellNameBox.Text = cellName;
            CellValueBox.Text = mainSpreadsheet.GetCellValue(cellName).ToString();

            if (mainSpreadsheet.GetCellContents(cellName) is Formula)
            {
                CellContentsBox.Text = "=" + mainSpreadsheet.GetCellContents(cellName).ToString();
            }
            else
            {
                CellContentsBox.Text = mainSpreadsheet.GetCellContents(cellName).ToString();
            }
            CellContentsBox.Focus();
        }

        private void CellContentsBox_TextChanged(object sender, EventArgs e)
        {

        }

        private void SpreadsheetGrid_Load(object sender, EventArgs e)
        {

        }
    }
}
