//Author: Ben Huenemann

using SpreadsheetUtilities;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

/// <summary>
/// Namespace that contains different kinds of spreadsheets. It contains an AbstractSpreadsheet
/// class (for the skeleton of a spreadsheet) and a Spreadsheet class (which extends the
/// spreadsheet class and specifies the implementation)
/// </summary>
namespace SS
{
    /// <summary>
    /// An Spreadsheet object represents the state of a simple spreadsheet.  It extends
    /// the AbstractSpreadsheet class so it can define the abstract methods and inherit
    /// the recalculating methods. 
    /// A spreadsheet consists of an infinite number of named cells.
    /// 
    /// A string is a valid cell name if and only if:
    ///   (1) its first character is an underscore or a letter
    ///   (2) its remaining characters (if any) are underscores and/or letters and/or digits
    /// Note that this is the same as the definition of valid variable from the PS3 Formula class.
    /// 
    /// For example, "x", "_", "x2", "y_15", and "___" are all valid cell  names, but
    /// "25", "2x", and "&" are not.  Cell names are case sensitive, so "x" and "X" are
    /// different cell names.
    /// 
    /// A spreadsheet contains a cell corresponding to every possible cell name.  (This
    /// means that a spreadsheet contains an infinite number of cells.)  In addition to 
    /// a name, each cell has a contents and a value.  The distinction is important.
    /// 
    /// The contents of a cell can be (1) a string, (2) a double, or (3) a Formula.  If the
    /// contents is an empty string, we say that the cell is empty.  (By analogy, the contents
    /// of a cell in Excel is what is displayed on the editing line when the cell is selected.)
    /// 
    /// In a new spreadsheet, the contents of every cell is the empty string.
    ///  
    /// The value of a cell can be (1) a string, (2) a double, or (3) a FormulaError.  
    /// (By analogy, the value of an Excel cell is what is displayed in that cell's position
    /// in the grid.)
    /// 
    /// If a cell's contents is a string, its value is that string.
    /// 
    /// If a cell's contents is a double, its value is that double.
    /// 
    /// If a cell's contents is a Formula, its value is either a double or a FormulaError,
    /// as reported by the Evaluate method of the Formula class.  The value of a Formula,
    /// of course, can depend on the values of variables.  The value of a variable is the 
    /// value of the spreadsheet cell it names (if that cell's value is a double) or 
    /// is undefined (otherwise).
    /// 
    /// Spreadsheets are never allowed to contain a combination of Formulas that establish
    /// a circular dependency.  A circular dependency exists when a cell depends on itself.
    /// For example, suppose that A1 contains B1*2, B1 contains C1*2, and C1 contains A1*2.
    /// A1 depends on B1, which depends on C1, which depends on A1.  That's a circular
    /// dependency.
    /// </summary>
    public class Spreadsheet : AbstractSpreadsheet
    {
        //Private data fields to store the dependencies of the spreadsheet and the cells
        private DependencyGraph Dependencies;
        private IDictionary<string, Cell> Cells;

        /// <summary>
        /// Empty constructor that sets up the DependencyGraph and Dictionary
        /// </summary>
        public Spreadsheet()
        {
            Dependencies = new DependencyGraph();
            Cells = new Dictionary<string, Cell>();
        }

        /// <summary>
        /// Enumerates the names of all the non-empty cells in the spreadsheet.
        /// </summary>
        public override IEnumerable<String> GetNamesOfAllNonemptyCells()
        {
            foreach (KeyValuePair<string, Cell> CellPair in Cells)
                yield return CellPair.Key.ToString();
        }

        /// <summary>
        /// If name is null or invalid, throws an InvalidNameException.
        /// 
        /// Otherwise, returns the contents (as opposed to the value) of the named cell.  The return
        /// value should be either a string, a double, or a Formula.
        /// 
        /// If the name is valid but not in the spreadsheet, it returns an empty string.
        /// </summary>
        /// <param name="name">Name of the cell being accessed</param>
        /// <returns>Contents of the cell being accessed</returns>
        public override object GetCellContents(String name)
        {
            if (name is null || !IsVariable(name))
                throw new InvalidNameException();

            if (!Cells.ContainsKey(name))
                return "";
            return Cells[name].CellContent;
        }

        /// <summary>
        /// If name is null or invalid, throws an InvalidNameException.
        /// 
        /// Otherwise, the contents of the named cell becomes number.  The method returns a
        /// list consisting of name plus the names of all other cells whose value depends, 
        /// directly or indirectly, on the named cell.
        /// 
        /// For example, if name is A1, B1 contains A1*2, and C1 contains B1+A1, the
        /// list {A1, B1, C1} is returned.
        /// </summary>
        /// <param name="name">Name of the cell being set</param>
        /// <param name="number">Value the cell should be set to</param>
        /// <returns>List of cells that depend on the named cell</returns>
        public override IList<String> SetCellContents(String name, double number)
        {
            if (name is null || !IsVariable(name))
                throw new InvalidNameException();

            if (!Cells.ContainsKey(name))
                Cells.Add(name, new Cell(name, number));
            //If the name already has a cell, replace the cell
            else
                Cells[name].CellContent = number;

            //Recalculate at the end and return the dependents
            return GetCellsToRecalculate(name).ToList();
        }

        /// <summary>
        /// If text is null, throws an ArgumentNullException.
        /// 
        /// Otherwise, if name is null or invalid, throws an InvalidNameException.
        /// 
        /// Otherwise, if the text is empty it removes the dependencies and then removes
        /// the cell.
        /// 
        /// Otherwise, the contents of the named cell becomes text.  The method returns a
        /// list consisting of name plus the names of all other cells whose value depends, 
        /// directly or indirectly, on the named cell.
        /// 
        /// For example, if name is A1, B1 contains A1*2, and C1 contains B1+A1, the
        /// list {A1, B1, C1} is returned.
        /// </summary>
        /// <param name="name">Name of the cell being set</param>
        /// <param name="text">Text the cell should be set to</param>
        /// <returns>List of cells that depend on the named cell</returns>
        public override IList<String> SetCellContents(String name, String text)
        {
            if (name is null || !IsVariable(name))
                throw new InvalidNameException();
            else if (text is null)
                throw new ArgumentNullException();

            else if (text == "")
            {
                if (Cells.ContainsKey(name))
                {
                    //It only removes the dependencies if it is a formula
                    if (Cells[name].CellContent is Formula)
                    {
                        foreach (string variable in ((Formula)Cells[name].CellContent).GetVariables())
                            Dependencies.RemoveDependency(variable, name);
                    }
                    Cells.Remove(name);
                }
                //Recalculate at the end and return the dependents
                return GetCellsToRecalculate(name).ToList();
            }

            if (!Cells.ContainsKey(name))
                Cells.Add(name, new Cell(name, text));
            else
                Cells[name].CellContent = text;

            return GetCellsToRecalculate(name).ToList();
        }

        /// <summary>
        /// If the formula parameter is null, throws an ArgumentNullException.
        /// 
        /// Otherwise, if name is null or invalid, throws an InvalidNameException.
        /// 
        /// Otherwise, if changing the contents of the named cell to be the formula would cause a 
        /// circular dependency, throws a CircularException, and no change is made to the spreadsheet.
        /// 
        /// Otherwise, the contents of the named cell becomes formula.  The method returns a
        /// list consisting of name plus the names of all other cells whose value depends,
        /// directly or indirectly, on the named cell.
        /// 
        /// For example, if name is A1, B1 contains A1*2, and C1 contains B1+A1, the
        /// list {A1, B1, C1} is returned.
        /// </summary>
        /// <param name="name">Name of the cell being set</param>
        /// <param name="formula">Formula the cell should be set to</param>
        /// <returns>List of cells that depend on the named cell</returns>
        public override IList<String> SetCellContents(String name, Formula formula)
        {
            if (name is null || !IsVariable(name))
                throw new InvalidNameException();
            if (formula is null)
                throw new ArgumentNullException();

            List<string> AllDependencies;

            //Adds all of the dependencies from the formula
            foreach (string variable in formula.GetVariables())
                Dependencies.AddDependency(variable, name);

            //This is for the case of a circular exception
            try
            {
                AllDependencies = GetCellsToRecalculate(name).ToList();
            }
            /* If it catches a circular exception, it undos the changes by removing
             * dependencies and then throws the exception at the end.*/
            catch (CircularException)
            {
                foreach (string variable in formula.GetVariables())
                    Dependencies.RemoveDependency(variable, name);

                throw new CircularException();
            }

            if (!Cells.ContainsKey(name))
                Cells.Add(name, new Cell(name, formula));
            else
                Cells[name].CellContent = formula;

            return AllDependencies;
        }

        /// <summary>
        /// If name is null, throws an ArgumentNullException.
        /// 
        /// Otherwise, if name isn't a valid cell name, throws an InvalidNameException.
        /// 
        /// Otherwise, returns an enumeration, without duplicates, of the names of all cells whose
        /// values depend directly on the value of the named cell.  In other words, returns
        /// an enumeration, without duplicates, of the names of all cells that contain
        /// formulas containing name.
        /// 
        /// For example, suppose that
        /// A1 contains 3
        /// B1 contains the formula A1 * A1
        /// C1 contains the formula B1 + A1
        /// D1 contains the formula B1 - C1
        /// The direct dependents of A1 are B1 and C1
        /// </summary>
        /// <param name="name">Name of the cell being accessed</param>
        /// <returns>An IEnumerable consisting of cells that directly depend on the named cell</returns>
        protected override IEnumerable<String> GetDirectDependents(String name)
        {
            if (name is null)
                throw new ArgumentException();
            foreach (string dependent in Dependencies.GetDependents(name))
                yield return dependent;
        }

        /// <summary>
        /// Tests if a string fits the format of a variable. This format is that it is a letter or underscore followed by a combination of 
        /// letters, underscores, or digits.
        /// </summary>
        /// <param name="s">String to be tested</param>
        /// <returns>Whether or not the string is a variable</returns>
        private bool IsVariable(string s)
        {
            return Regex.IsMatch(s, @"^[a-zA-Z_](?:[a-zA-Z_]|\d)*$");
        }

        /// <summary>
        /// Private cell class for the elements of the dictionary. Each cell
        /// stores it's name and it's value. The cell's value can only be a
        /// double, string, or formula.
        /// 
        /// The name of a cell is immutable but it's value can be changed.
        /// </summary>
        private class Cell
        {
            /// <summary>
            /// Constructor for if the cell holds a double
            /// </summary>
            /// <param name="name">Name of the cell</param>
            /// <param name="value">Double that should be stored in the cell</param>
            public Cell(string name, double value)
            {
                Name = name;
                CellContent = value;
            }

            /// <summary>
            /// 
            /// </summary>
            /// <param name="name">Name of the cell</param>
            /// <param name="text">String that should be stored in the cell</param>
            public Cell(string name, string text)
            {
                Name = name;
                CellContent = text;
            }

            /// <summary>
            /// 
            /// </summary>
            /// <param name="name">Name of the cell</param>
            /// <param name="formula">Formula that should be stored in the cell</param>
            public Cell(string name, Formula formula)
            {
                Name = name;
                CellContent = formula;
            }

            /// <summary>
            /// Immutable name property. Currently it isn't used but it's good to store
            /// it just in case.
            /// </summary>
            public object Name
            {
                get;
                private set;
            }

            /// <summary>
            /// Content the cell is storing. It's written as a property since future versions
            /// may require different get and set methods.
            /// </summary>
            public object CellContent
            {
                get;
                set;
            }
        }
    }
}
