//Authors: Ben Huenemann and Jonathan Wigderson

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SpreadsheetGUI
{
    /// <summary>
    /// This is a form that can't be resized and has a number of different labels with advice on how to use those
    /// features. Everything in this form is private/readonly.
    /// </summary>
    public partial class HelpMenu : Form
    {
        /// <summary>
        /// Each string is written in a format where it is one line per sentence. This makes it easy to edit the
        /// sentences without having to change what's on each line. These strings are written here rather than
        /// properties for convenience in writing and editing.
        /// </summary>
        private readonly string cellEditingString =
            "To edit a cell, click on it and type the value that you want it to be set to." +
            " Then you can set it to this value by either pressing the Enter key or an arrow key." +
            " The Enter key will update the cell while keeping the same selection and the arrow keys will update it and move the selection over one cell." +
            " When a cell is selected, its value, name, and contents are displayed above the spreadsheet.";

        private readonly string formulaString =
            "A formula can be inserted by typing an \"=\" and then the formula." +
            " Any cells in the formula can be represented by \"[ColumnLetter][RowNumber]\"." +
            " If anything is an incorrect format, it will show a \"FormulaError\" but if an error occurs when trying to evaluate an error dialog box will come up.";

        private readonly string undoString =
            "To undo a change, press Ctrl + Z." +
            " This only goes back one change but it reverts the previous cell you edited to how it previously was.";

        private readonly string newOpenString =
            "Pressing the New button under the File Menu creates a new spreadsheet in another window." +
            " The Open button under the File Menu opens a dialog box to chose which file to open." +
            " You can either open a \".sprd\" file or another type of file (depending on the filter you choose)." +
            " When you click OK, it tries to load the spreadsheet file.";

        private readonly string saveString =
            "Pressing the Save As button under the File Menu opens a dialog box to choose where to save the file." +
            " You can either open a \".sprd\" file or another type of file (depending on the filter you choose)." +
            " When you click OK, it tries to save the spreadsheet file." +
            " If saving it would overwrite another file, it gives a warning." +
            " The Save button first checks if the file has already been saved somewhere." +
            " If it has, it saves to that file again without any dialog boxes." +
            " Otherwise if it's an untitled spreadsheet, it does the save as the Save As button.";

        private readonly string changedString =
            "When the spreadsheet has been changed from a saved or empty version, it adds \"*\" to the title.";

        private readonly string closedString =
            "When a file is closed, it will give a warning if unsaved but otherwise just close it." +
            " The warning allows you to save your work, close the file, or cancel." +
            " The spreadsheet program won't close completely until every window is closed.";



        /// <summary>
        /// A helper method that sets up the text in each of the labels to equal the strings specified earlier.
        /// </summary>
        private void InitializeLabels()
        {
            CellEditingText.Text = cellEditingString;
            FormulaText.Text = formulaString;
            UndoText.Text = undoString;
            NewOpenText.Text = newOpenString;
            SaveText.Text = saveString;
            ChangedText.Text = changedString;
            ClosedText.Text = closedString;
        }



        public HelpMenu()
        {
            InitializeComponent();
            InitializeLabels();
        }
    }
}
