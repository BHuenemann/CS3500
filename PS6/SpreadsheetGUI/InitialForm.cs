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
        private Spreadsheet mainSpreadsheet;
        private string FileName = "Untitled Form";
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



        private void CreateNewSpreadsheet()
        {
            if (!UnsavedWarning())
            {
                mainSpreadsheet = new Spreadsheet(SpreadsheetCellIsValid, SpreadsheetCellNormalizer, "ps6");
                SpreadsheetGrid.Clear();
                FileName = "Untitled Form";
                Text = Path.GetFileName(FileName);

                SpreadsheetGrid.SetSelection(0, 0);
                VisualUpdate("A1");
            }
        }



        private void StartSaveDialog()
        {
            try
            {
                SaveDialogBox.OverwritePrompt = !(File.Exists(SaveDialogBox.FileName) && SaveDialogBox.FileName == FileName);

                if (SaveDialogBox.ShowDialog() == DialogResult.OK)
                    SaveFile(SaveDialogBox.FileName);
            }
            catch (Exception)
            {
                DialogResult = MessageBox.Show("An error occured while trying to save the file", "Save Spreadsheet Error", MessageBoxButtons.OK);
            }
        }



        private void SaveFile(string FileName)
        {
            if (File.Exists(FileName) && FileName == this.FileName)
                File.Delete(FileName);

            if (SaveDialogBox.FilterIndex == 2 || FileName.Substring(FileName.Length - 5) == ".sprd")
            {
                mainSpreadsheet.Save(FileName);
                this.FileName = FileName;
            }
            else
            {
                mainSpreadsheet.Save(FileName);
                this.FileName = FileName + ".sprd";
            }
            Text = Path.GetFileName(FileName);
        }



        private void StartOpenDialog()
        {
            try
            {
                if (!UnsavedWarning() && OpenDialogBox.ShowDialog() == DialogResult.OK)
                    OpenFile(OpenDialogBox.FileName);
            }
            catch (SpreadsheetReadWriteException)
            {
                DialogResult = MessageBox.Show("File contents are incompatible", "Open Spreadsheet Error", MessageBoxButtons.OK);
            }
            catch (Exception e)
            {
                DialogResult = MessageBox.Show("Error while trying to open the file of type" + e.ToString(), "Open Spreadsheet Error", MessageBoxButtons.OK);
            }
        }



        private void OpenFile(string FileName)
        {
            SpreadsheetGrid.Clear();
            previousCol = previousRow = 0;

            mainSpreadsheet = new Spreadsheet(FileName, SpreadsheetCellIsValid, SpreadsheetCellNormalizer, "ps6");
            this.FileName = FileName;
            Text = Path.GetFileName(FileName);

            foreach(string cell in mainSpreadsheet.GetNamesOfAllNonemptyCells().ToList())
            {
                int rowToChange = Int32.Parse(cell.Substring(1)) - 1;
                int letterToNumberCol = char.ToUpper(cell[0]) - 65;

                if (mainSpreadsheet.GetCellValue(cell) is FormulaError)
                    SpreadsheetGrid.SetValue(letterToNumberCol, rowToChange, "Formula Error");
                else
                    SpreadsheetGrid.SetValue(letterToNumberCol, rowToChange, mainSpreadsheet.GetCellValue(cell).ToString());
            }

            SpreadsheetGrid.SetSelection(0, 0);
            VisualUpdate("A1");
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



        private void OnSelectionChanged(SpreadsheetPanel spreadSheet)
        {
            spreadSheet.GetSelection(out int col, out int row);

            UpdateCells(previousCol, previousRow);

            spreadSheet.SetSelection(col, row);
            previousCol = col;
            previousRow = row;

            VisualUpdate(SelectedCellName(col, row));

            CellContentsBox.Focus();

            CellContentsBox.SelectionStart = CellContentsBox.Text.Length + 1;

        }



        private String ColNumberToLetter(int col)
        {
            Char c = (Char)((true ? 65 : 97) + (col));

            return c.ToString();
        }



        private String SelectedCellName(int col, int row)
        {
            row++;
            return ColNumberToLetter(col) + row.ToString();
        }



        private void UpdateCells(int col, int row)
        {
            if (CellContentsBox.Text.Trim() == "") return;

            string cellName = SelectedCellName(col, row);
            string previousContents = CellContentsBox.Text;

            try
            {
                IList<string> dependencies = mainSpreadsheet.SetContentsOfCell(cellName, CellContentsBox.Text);

                foreach (string cell in dependencies)
                {
                    int rowToChange = Int32.Parse(cell.Substring(1)) - 1;
                    int letterToNumberCol = char.ToUpper(cell[0]) - 65;

                    if (mainSpreadsheet.GetCellValue(cell) is FormulaError)
                        SpreadsheetGrid.SetValue(letterToNumberCol, rowToChange, "Formula Error");
                    else
                        SpreadsheetGrid.SetValue(letterToNumberCol, rowToChange, mainSpreadsheet.GetCellValue(cell).ToString());
                }

                SpreadsheetGrid.SetSelection(col, row);

                // Visual Change in cell
                string cellValue = mainSpreadsheet.GetCellValue(cellName).ToString();

                VisualUpdate(cellName);

                if (mainSpreadsheet.Changed)
                    Text = Path.GetFileName(FileName) + "*";
            }

            catch
            {
                MessageBox.Show("An error occured while trying to change the contents of a cell", "Contents of Cell Error", MessageBoxButtons.OK);
                VisualUpdate(cellName);
            }
        }



        private void VisualUpdate(string cellName)
        {
            CellNameBox.Text = cellName;

            if (mainSpreadsheet.GetCellValue(cellName) is FormulaError)
                CellValueBox.Text = "Formula Error";
            else
                CellValueBox.Text = mainSpreadsheet.GetCellValue(cellName).ToString();

            if (mainSpreadsheet.GetCellContents(cellName) is Formula)
                CellContentsBox.Text = "=" + mainSpreadsheet.GetCellContents(cellName).ToString();
            else
                CellContentsBox.Text = mainSpreadsheet.GetCellContents(cellName).ToString();

            CellContentsBox.Focus();
        }



        private void HandleKeys(Keys keyCode, Keys keyModifier)
        {
            SpreadsheetGrid.GetSelection(out int col, out int row);

            if (keyCode == Keys.Enter)
            {
                UpdateCells(col, row);
            }

            if (keyCode == Keys.Up)
            {
                UpdateCells(col, row);
                string cellName = SelectedCellName(col, row);
                if (row != 0)
                {
                    previousCol = col;
                    previousRow = row - 1;
                    SpreadsheetGrid.SetSelection(col, row - 1);
                    cellName = SelectedCellName(col, row - 1);
                }

                VisualUpdate(cellName);
            }

            if (keyCode == Keys.Down)
            {
                UpdateCells(col, row);
                string cellName = SelectedCellName(col, row);
                if (row != 98)
                {
                    previousCol = col;
                    previousRow = row + 1;
                    SpreadsheetGrid.SetSelection(col, row + 1);
                    cellName = SelectedCellName(col, row + 1);
                }

                VisualUpdate(cellName);
            }

            if (keyCode == Keys.Left)
            {
                UpdateCells(col, row);
                string cellName = SelectedCellName(col, row);
                if (col != 0)
                {
                    previousCol = col - 1;
                    previousRow = row;
                    SpreadsheetGrid.SetSelection(col - 1, row);
                    cellName = SelectedCellName(col - 1, row);
                }

                VisualUpdate(cellName);
            }

            if (keyCode == Keys.Right)
            {
                UpdateCells(col, row);
                string cellName = SelectedCellName(col, row);
                if (col != 25)
                {
                    previousCol = col + 1;
                    previousRow = row;
                    SpreadsheetGrid.SetSelection(col + 1, row);
                    cellName = SelectedCellName(col + 1, row);
                }

                VisualUpdate(cellName);
            }

            if (keyCode == Keys.Control && keyModifier == Keys.Z)
            {
                MessageBox.Show("You pressed ctrl + z");
            }

            CellContentsBox.SelectionStart = CellContentsBox.Text.Length + 1;
        }



        public InitialForm()
        {
            InitializeComponent();
            mainSpreadsheet = new Spreadsheet(SpreadsheetCellIsValid, SpreadsheetCellNormalizer, "ps6");
            Text = FileName;

            SpreadsheetGrid.SelectionChanged += OnSelectionChanged;

            SpreadsheetGrid.GetSelection(out int col, out int row);
            int selectedRow = row + 1;
            CellNameBox.Text = SelectedCellName(col, row);
        }



        private void NewToolStripMenuItem_Click(object sender, EventArgs e)
        {
            CreateNewSpreadsheet();
        }



        private void SaveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (File.Exists(FileName))
                SaveFile(FileName);
            else
                StartSaveDialog();
        }



        private void SaveAsToolStripMenuItem_Click(object sender, EventArgs e)
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



        private void CellContentsBox_KeyDown(object sender, KeyEventArgs e)
        {
            HandleKeys(e.KeyCode, e.Modifiers);
        }
    }
}
