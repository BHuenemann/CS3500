//Authors: Ben Huenemann and Jonathan Wigderson

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
    /// <summary>
    /// This class creates a spreadsheet window. The spreadsheet window allows the user to:
    ///    -Edit cell contents
    ///    -Create formulas
    ///    -View cell values
    ///    -Save and open files
    ///    -Create new windows
    ///    -Close windows
    ///    -Open help Menu
    ///    
    /// Additionally, it also allows the user to select different cells with the arrow keys
    /// and press enter to evaluate cells. It also has an extra save function that saves to
    /// the file that was opened and if the spreadsheet has been changed, an asterisk is
    /// added to the title.
    /// </summary>
    public partial class SpreadsheetWindow : Form
    {
        /// <summary>
        /// Spreadsheet variable to store the values
        /// </summary>
        private Spreadsheet mainSpreadsheet;

        /// <summary>
        /// Stores the file name. It's "Untitled Form" by default.
        /// </summary>
        private string FileName = "Untitled Form";

        /// <summary>
        /// Keeps track of the previous columns and rows selected.
        /// </summary>
        private int previousCol = 0;
        private int previousRow = 0;



        /// <summary>
        /// Method to be passed into the spreadsheet as the IsValid delegate. This only
        /// accepts cells that have one letter followed by a number from 1-99.
        /// </summary>
        /// <param name="cellName">Cell name that should be tested</param>
        /// <returns>Whether or not the cell is valid</returns>
        private bool SpreadsheetCellIsValid(string cellName)
        {
            return Regex.IsMatch(cellName, "^[a-zA-Z][1-9][0-9]?$");
        }



        /// <summary>
        /// Method to be passed into the spreadsheet as the Normalizer delegate. This
        /// converts a string to upper case.
        /// </summary>
        /// <param name="s">String to be converted.</param>
        /// <returns>Upper case version of the string.</returns>
        private string SpreadsheetCellNormalizer(string s)
        {
            return s.ToUpper();
        }



        /// <summary>
        /// This method sends an overwrite prompt if applicable, opens a save dialog,
        /// saves the file, and catches any exceptions that occur in this process.
        /// </summary>
        private void StartSaveDialog()
        {
            try
            {
                //Gives an overwrite prompt if a file exists that matches the file name stored
                SaveDialogBox.OverwritePrompt = !(File.Exists(SaveDialogBox.FileName) && SaveDialogBox.FileName == FileName);

                if (SaveDialogBox.ShowDialog() == DialogResult.OK)
                    SaveFile(SaveDialogBox.FileName);
            }
            catch
            {
                //Creates a dialog box to show the error with an OK button.
                DialogResult = MessageBox.Show("An error occured while trying to save the file", "Save Spreadsheet Error", MessageBoxButtons.OK);
            }
        }



        /// <summary>
        /// This method saves the current spreadsheet to a file path.
        /// </summary>
        /// <param name="FileName">File path that it should be saved to</param>
        private void SaveFile(string FileName)
        {
            //If it already exists, delete that existing file.
            if (File.Exists(FileName) && FileName == this.FileName)
                File.Delete(FileName);

            /*Depending on the filter, handle the file name differently. If the first index is selected and it
            * doesn't end in ".sprd", append that to the string.*/
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



        /// <summary>
        /// This method starts a dialog for opening a file, opens the file, and catches
        /// any exceptions that occur during the process.
        /// </summary>
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
            catch
            {
                DialogResult = MessageBox.Show("Error while trying to open the file of type", "Open Spreadsheet Error", MessageBoxButtons.OK);
            }
        }



        /// <summary>
        /// This method opens a file from a specified file path. It does this by creating a
        /// new spreadsheet with the specified contents and updating the visuals.
        /// </summary>
        /// <param name="FileName">Name of file being opened</param>
        private void OpenFile(string FileName)
        {
            //Clears the spreadsheet and updates the previous row/column
            SpreadsheetGrid.Clear();
            previousCol = previousRow = 0;

            //Imports the spreadsheet and records the text. The title of the form is set equal to the file name.
            mainSpreadsheet = new Spreadsheet(FileName, SpreadsheetCellIsValid, SpreadsheetCellNormalizer, "ps6");
            this.FileName = FileName;
            Text = Path.GetFileName(FileName);

            //Updates the visuals of each nonempty cell
            foreach(string cell in mainSpreadsheet.GetNamesOfAllNonemptyCells().ToList())
            {
                int rowToChange = Int32.Parse(cell.Substring(1)) - 1;
                int letterToNumberCol = char.ToUpper(cell[0]) - 65;

                if (mainSpreadsheet.GetCellValue(cell) is FormulaError)
                    SpreadsheetGrid.SetValue(letterToNumberCol, rowToChange, "Formula Error");
                else
                    SpreadsheetGrid.SetValue(letterToNumberCol, rowToChange, mainSpreadsheet.GetCellValue(cell).ToString());
            }

            //Resets the selection and text boxes.
            SpreadsheetGrid.SetSelection(0, 0);
            VisualUpdate("A1");
        }



        /// <summary>
        /// Creates a dialog warning for if you try to close an unsaved document. It returns
        /// whether or not the form should close so whatever method calls it knows what to do
        /// next.
        /// </summary>
        /// <returns>Whether or not the form should close</returns>
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



        /// <summary>
        /// This converts the column number to a letter
        /// </summary>
        /// <param name="col">Column number to be converted</param>
        /// <returns></returns>
        private String ColNumberToLetter(int col)
        {
            Char c = (Char)((true ? 65 : 97) + (col));

            return c.ToString();
        }



        /// <summary>
        /// This returns the name of a selected cell at certain coordinates.
        /// </summary>
        /// <param name="col">Column that the cell is in</param>
        /// <param name="row">Row that the cell is in</param>
        /// <returns></returns>
        private String SelectedCellName(int col, int row)
        {
            row++;
            return ColNumberToLetter(col) + row.ToString();
        }



        /// <summary>
        /// Updates a specified cell and it's dependencies.
        /// 
        /// If it's empty, it just returns out of this method.
        /// 
        /// This also catches any exceptions that occur
        /// </summary>
        /// <param name="col">Column of the cell that's being updated</param>
        /// <param name="row">Row of the cell that's being updated</param>
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

                    //Sets the visuals to the string "Formula Error" if the value is a FormulaError
                    if (mainSpreadsheet.GetCellValue(cell) is FormulaError)
                        SpreadsheetGrid.SetValue(letterToNumberCol, rowToChange, "Formula Error");
                    else
                        SpreadsheetGrid.SetValue(letterToNumberCol, rowToChange, mainSpreadsheet.GetCellValue(cell).ToString());
                }

                SpreadsheetGrid.SetSelection(col, row);

                // Visual change in cell
                string cellValue = mainSpreadsheet.GetCellValue(cellName).ToString();

                VisualUpdate(cellName);

                //Adds an asterisk to the title if the file has changed
                if (mainSpreadsheet.Changed)
                    Text = Path.GetFileName(FileName) + "*";
            }
            //Catches any exceptions while updating the value and creates a dialog to show it.
            catch
            {
                MessageBox.Show("An error occured while trying to change the contents of a cell", "Contents of Cell Error", MessageBoxButtons.OK);
                VisualUpdate(cellName);
            }
        }



        /// <summary>
        /// Visually updates the cell name, cell value, and cell contents boxes and then focusses on the cell contents box.
        /// </summary>
        /// <param name="cellName">Name of the cell that's being updated</param>
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



        /// <summary>
        /// This method handles any key inputs.
        ///    -The Enter key updates the value of a cell.
        ///    -The arrow keys update the value and then move the selection.
        ///    -Ctrl + Z undos the last change that has been made.
        /// </summary>
        /// <param name="keyCode">Code of the key that's been pressed</param>
        /// <param name="keyModifier">Modifier of the key that's been pressed</param>
        private void HandleKeys(Keys keyCode, Keys keyModifier)
        {
            SpreadsheetGrid.GetSelection(out int col, out int row);

            //Enter Key
            if (keyCode == Keys.Enter)
                UpdateCells(col, row);

            //Up Arrow Key
            if (keyCode == Keys.Up)
            {
                UpdateCells(col, row);
                string cellName = SelectedCellName(col, row);

                //Only moves up if the row isn't at 0
                if (row != 0)
                {
                    previousCol = col;
                    previousRow = row - 1;
                    SpreadsheetGrid.SetSelection(col, row - 1);
                    cellName = SelectedCellName(col, row - 1);
                }

                VisualUpdate(cellName);
            }

            //Down Arrow Key
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

            //Left Arrow Key
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

            //Right Arrow Key
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

            //Control + Z
            if (keyCode == Keys.Control && keyModifier == Keys.Z)
            {
                MessageBox.Show("You pressed ctrl + z");
            }

            CellContentsBox.SelectionStart = CellContentsBox.Text.Length + 1;
        }



        /// <summary>
        /// Spreadsheet window constructor. It updates the title of the spreadsheet to the default,
        /// Adds the selection changed method to the event, and updates the name text box.
        /// </summary>
        public SpreadsheetWindow()
        {
            InitializeComponent();
            mainSpreadsheet = new Spreadsheet(SpreadsheetCellIsValid, SpreadsheetCellNormalizer, "ps6");
            Text = FileName;

            SpreadsheetGrid.SelectionChanged += OnSelectionChanged;

            CellNameBox.Text = SelectedCellName(0, 0);
        }



        /// <summary>
        /// When the selection is changed, it calls this method. This method updates the cells in
        /// the spreadsheet and updates them visually.
        /// 
        /// At the end it moves the selection over because of a weird glitch that sets the selection
        /// to the second to last digit.
        /// </summary>
        /// <param name="spreadSheet">The spreadsheet panel that is being used</param>
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



        /// <summary>
        /// Adds a new SpreadsheetWindow to the Application Context. Doing this opens the window
        /// and makes sure that the form doesn't close if the new window closes.
        /// </summary>
        private void NewToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SpreadsheetApplicationContext.GetAppContext().RunForm(new SpreadsheetWindow());
        }



        /// <summary>
        /// The Save button saves the file if the file stored exists but otherwise starts a save
        /// dialog just like SaveAs.
        /// </summary>
        private void SaveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (File.Exists(FileName))
                SaveFile(FileName);
            else
                StartSaveDialog();
        }



        /// <summary>
        /// The SaveAs button starts a save dialog. 
        /// </summary>
        private void SaveAsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            StartSaveDialog();
        }



        /// <summary>
        /// The Open button starts an open dialog
        /// </summary>
        private void OpenToolStripMenuItem_Click(object sender, EventArgs e)
        {
            StartOpenDialog();
        }



        /// <summary>
        /// The close button closes the form. This calls the form closing method which makes asks
        /// if you want to save your progress if the file is unsaved.
        /// </summary>
        private void CloseToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Close();
        }



        /// <summary>
        /// If the help button is pressed, it creates a new HelpMenu form and shows that.
        /// </summary>
        private void HelpToolStripMenuItem_Click(object sender, EventArgs e)
        {
            HelpMenu help = new HelpMenu();
            help.Show();
        }



        /// <summary>
        /// Depending on if the file has changed and the results of the unsaved warning if it has
        /// changed, it cancels the closing process.
        /// </summary>
        private void SpreadsheetWindow_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (UnsavedWarning())
                e.Cancel = true;
        }



        /// <summary>
        /// If a button is pressed, it handles the key press.
        /// </summary>
        private void CellContentsBox_KeyDown(object sender, KeyEventArgs e)
        {
            HandleKeys(e.KeyCode, e.Modifiers);
        }
    }
}
