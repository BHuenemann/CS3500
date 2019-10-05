//Author: Ben Huenemann

using SpreadsheetUtilities;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Xml;

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
        /// <summary>
        /// Private class to store the dependencies between the cells. It maps cell names
        /// to the cell's dependents and dependees.
        /// </summary>
        private DependencyGraph Dependencies = new DependencyGraph();

        /// <summary>
        /// Private dictionary to map between the cell names (keys) and the actual cells. This
        /// way the cells can be accessed easily and there can't be any repeats.
        /// </summary>
        private IDictionary<string, Cell> Cells = new Dictionary<string, Cell>();

        /// <summary>
        /// True if this spreadsheet has been modified since it was created or saved                  
        /// (whichever happened most recently); false otherwise.
        /// </summary>
        public override bool Changed { get; protected set; } = false;



        /// <summary>
        /// Constructs an abstract spreadsheet by setting it's variable validity test
        /// to always return true, it's normalization to the identity method, and the
        /// version to "default".
        /// </summary>
        public Spreadsheet() : base(i => true, i => i, "default") { }

        /// <summary>
        /// Constructs an abstract spreadsheet by recording its variable validity test,
        /// its normalization method, and its version information.  The variable validity
        /// test is used throughout to determine whether a string that consists of one or
        /// more letters followed by one or more digits is a valid cell name.
        /// </summary>
        /// <param name="isValid">Method to determine if a cell name is valid</param>
        /// <param name="normalize">Method to define a standard form for all cell names</param>
        /// <param name="version">Version of the spreadsheet being created</param>
        public Spreadsheet(Func<string, bool> isValid, Func<string, string> normalize, string version) :
            base(isValid, normalize, version)
        { }

        /// <summary>
        /// Constructs an abstract spreadsheet by recording the file path that contains the
        /// contents for the spreadsheet, its variable validity test, its normalization method,
        /// and its version information. It reads a file and thows an accurate
        /// SpreadsheetReadWriteException if any errors occur. It also throws an exception if
        /// the version of file and version specified don't match. The values from the file are
        /// inserted into the spreadsheet as cells. The variable validity test is used throughout
        /// to determine whether a string that consists of one or more letters followed by one
        /// or more digits is a valid cell name.
        /// </summary>
        /// <param name="filePath">String path of the file being read and converted to a spreadsheet</param>
        /// <param name="isValid">Method to determine if a cell name is valid</param>
        /// <param name="normalize">Method to define a standard form for all cell names</param>
        /// <param name="version">Version of the spreadsheet being created</param>
        public Spreadsheet(string filePath, Func<string, bool> isValid, Func<string, string> normalize, string version) :
            base(isValid, normalize, version)
        {
            ReadFileAndCheckVersion(filePath, version);
        }



        /// <summary>
        /// Returns the version information of the spreadsheet saved in the named file.
        /// If there are any problems opening, reading, or closing the file, the method
        /// should throw a SpreadsheetReadWriteException with an explanatory message.
        /// </summary>
        /// <param name="filename">String name of the file being read</param>
        /// <returns>Returns a string containing the version of the spreadsheet XML file</returns>
        public override string GetSavedVersion(String filename)
        {
            XmlReader reader;

            //Creating the reader is put in a try catch block so it can handle any errors that come up
            try
            {
                reader = XmlReader.Create(filename);
            }



            catch (System.IO.FileNotFoundException)
            {
                throw new SpreadsheetReadWriteException("Couldn't find specified file");
            }
            catch (ArgumentNullException)
            {
                throw new SpreadsheetReadWriteException("Couldn't read since filename was set to null");
            }
            catch (Exception)
            {
                throw new SpreadsheetReadWriteException("Encountered an error while reading the file");
            }



            try
            {
                while (reader.Read())
                {
                    if (reader.IsStartElement())
                    {
                        switch (reader.Name)
                        {
                            case "spreadsheet":
                                return reader["version"];

                            //Throws an error if any other element is encountered
                            default:
                                throw new SpreadsheetReadWriteException("Invalid starting element (couldn't find version)");
                        }
                    }

                }
            }
            catch (SpreadsheetReadWriteException)
            {
                throw;
            }
            catch (Exception)
            {
                throw new SpreadsheetReadWriteException("Error occured while reading file");
            }
            finally
            {
                reader.Dispose();
            }

            throw new SpreadsheetReadWriteException("Error occured while reading file");
        }

        /// <summary>
        /// Writes the contents of this spreadsheet to the named file using an XML format.
        /// The XML elements should be structured as follows:
        /// 
        /// <spreadsheet version="version information goes here">
        /// 
        /// <cell>
        /// <name>
        /// cell name goes here
        /// </name>
        /// <contents>
        /// cell contents goes here
        /// </contents>    
        /// </cell>
        /// 
        /// </spreadsheet>
        /// 
        /// There should be one cell element for each non-empty cell in the spreadsheet.  
        /// If the cell contains a string, it should be written as the contents.  
        /// If the cell contains a double d, d.ToString() should be written as the contents.  
        /// If the cell contains a Formula f, f.ToString() with "=" prepended should be written as the contents.
        /// 
        /// If there are any problems opening, writing, or closing the file, the method should throw a
        /// SpreadsheetReadWriteException with an explanatory message.
        /// </summary>
        /// <param name="filename">String name of the file being written to</param>
        public override void Save(String filename)
        {
            XmlWriterSettings settings = new XmlWriterSettings();
            settings.Indent = true;
            settings.IndentChars = "\t";

            XmlWriter writer;

            try
            {
                writer = XmlWriter.Create(filename, settings);
            }
            catch (ArgumentNullException)
            {
                throw new SpreadsheetReadWriteException("Couldn't save since filename was set to null");
            }
            catch (Exception)
            {
                throw new SpreadsheetReadWriteException("Error occured while writing the file");
            }

            try
            {
                writer.WriteStartDocument();


                writer.WriteStartElement("spreadsheet");
                writer.WriteAttributeString("version", Version);

                foreach (Cell c in Cells.Values)
                {
                    writer.WriteStartElement("cell");


                    writer.WriteElementString("name", c.Name);
                    if (c.CellContent is string)
                        writer.WriteElementString("contents", (string)c.CellContent);
                    else if (c.CellContent is double)
                        writer.WriteElementString("contents", c.CellContent.ToString());
                    else if (c.CellContent is Formula)
                        writer.WriteElementString("contents", "=" + c.CellContent.ToString());

                    writer.WriteEndElement();
                }

                writer.WriteEndElement();


                writer.WriteEndDocument();
            }



            catch (InvalidOperationException)
            {
                throw new SpreadsheetReadWriteException("Invalid operation while writing file");
            }



            finally
            {
                Changed = false;
                writer.Dispose();
            }
        }

        /// <summary>
        /// This method inputs a file name and a version. First it checks if the file has the same
        /// version as the one specified and throws an error if it does. After this, it reads in the
        /// 
        /// </summary>
        /// <param name="filePath"></param>
        /// <param name="version"></param>
        protected void ReadFileAndCheckVersion(string filePath, string version)
        {
            if (version != GetSavedVersion(filePath))
                throw new SpreadsheetReadWriteException("File version doesn't match inputted version while reading file");

            XmlReader reader;

            /*This isn't in a try catch block since readfile is only used after GetSavedVersion. Any
             *file errors will be thrown in GetSavedVersion.*/
            reader = XmlReader.Create(filePath);

            /*This stack is used to contain the names for each cell so it can create the cell when
             *it gets to the cell content */
            Stack<string> nameStack = new Stack<string>();

            int previousElement = 0;

            try
            {
                while (reader.Read())
                {
                    if (reader.IsStartElement())
                    {
                        switch (reader.Name)
                        {
                            //Each case checks the previous element and throws an error if the order is incorrect
                            case "spreadsheet":
                                CheckPreviousElement(previousElement, 0);
                                previousElement = 1;
                                break;

                            case "cell":
                                if (previousElement != 1 && previousElement != 6)
                                    throw new SpreadsheetReadWriteException("Invalid element order in XML file");
                                previousElement = 2;
                                break;

                            case "name":
                                CheckPreviousElement(previousElement, 2);
                                previousElement = 3;
                                reader.Read();

                                //Pushes the name to the stack
                                nameStack.Push(reader.Value);
                                break;

                            case "contents":
                                CheckPreviousElement(previousElement, 7);
                                previousElement = 4;
                                reader.Read();

                                //Creates a cell with the specified name and value
                                SetContentsOfCell(nameStack.Pop(), reader.Value);
                                break;

                            //Thows an exception if the element is unknown
                            default:
                                throw new SpreadsheetReadWriteException("Unknown element while reading file");

                        }
                    }
                    else
                    {
                        //It also checks the end elements to make sure those are in the right order.
                        switch (reader.Name)
                        {
                            case "spreadsheet":
                                CheckPreviousElement(previousElement, 6);
                                previousElement = 5;
                                break;

                            case "cell":
                                CheckPreviousElement(previousElement, 8);
                                previousElement = 6;
                                break;

                            case "name":
                                CheckPreviousElement(previousElement, 3);
                                previousElement = 7;
                                break;

                            case "contents":
                                CheckPreviousElement(previousElement, 4);
                                previousElement = 8;
                                break;

                            default:
                                throw new SpreadsheetReadWriteException("Unknown element while reading file");
                        }
                    }
                }
            }



            //Exception handling for reading in the file.
            catch (SpreadsheetReadWriteException)
            {
                throw;
            }
            catch (InvalidNameException)
            {
                throw new SpreadsheetReadWriteException("Invalid cell name while reading file");
            }
            catch (CircularException)
            {
                throw new SpreadsheetReadWriteException("Encountered a circular dependency while reading the file");
            }
            catch (FormulaFormatException)
            {
                throw new SpreadsheetReadWriteException("Tried to insert an invalid formula while reading the file");
            }
            catch (Exception)
            {
                throw new SpreadsheetReadWriteException("Error occured while reading the file");
            }



            finally
            {
                reader.Dispose();
            }
        }

        /// <summary>
        /// Helper method for checking to see if the current value matches the correct condition.
        /// If they don't match, it throws a spreadsheetReadWriteException.
        /// </summary>
        /// <param name="currentValue">Value that should be compared</param>
        /// <param name="correctCondition">Condition that the value should match</param>
        protected void CheckPreviousElement(int currentValue, int correctCondition)
        {
            if (currentValue != correctCondition)
                throw new SpreadsheetReadWriteException("Invalid element order in XML file");
        }

        /// <summary>
        /// If name is null or invalid, throws an InvalidNameException.
        /// 
        /// Otherwise, returns the value (as opposed to the contents) of the named cell.  The return
        /// value should be either a string, a double, or a SpreadsheetUtilities.FormulaError.
        /// </summary>
        /// <param name="name">Name of the cell being accessed</param>
        /// <returns>Value of the cell being accessed</returns>
        public override object GetCellValue(String name)
        {
            //Normalizes the name in this scope and handles name exceptions
            name = Normalize(name);
            CellNameExceptionHandling(name);

            if (!Cells.ContainsKey(name))
                return "";

            return Cells[name].CellValue;
        }

        /// <summary>
        /// Enumerates the names of all the non-empty cells in the spreadsheet.
        /// </summary>
        /// <returns>Enumerable of all nonempty cells</returns>
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
            name = Normalize(name);
            CellNameExceptionHandling(name);

            //If the cell is empty it returns an empty string
            if (!Cells.ContainsKey(name))
                return "";
            return Cells[name].CellContent;
        }

        /// <summary>
        /// If content is null, throws an ArgumentNullException.
        /// 
        /// Otherwise, if name is null or invalid, throws an InvalidNameException.
        /// 
        /// Otherwise, if content parses as a double, the contents of the named
        /// cell becomes that double.
        /// 
        /// Otherwise, if content begins with the character '=', an attempt is made
        /// to parse the remainder of content into a Formula f using the Formula
        /// constructor.  There are then three possibilities:
        /// 
        ///   (1) If the remainder of content cannot be parsed into a Formula, a 
        ///       SpreadsheetUtilities.FormulaFormatException is thrown.
        ///       
        ///   (2) Otherwise, if changing the contents of the named cell to be f
        ///       would cause a circular dependency, a CircularException is thrown,
        ///       and no change is made to the spreadsheet.
        ///       
        ///   (3) Otherwise, the contents of the named cell becomes f.
        /// 
        /// Otherwise, the contents of the named cell becomes content.
        /// 
        /// If an exception is not thrown, the method returns a list consisting of
        /// name plus the names of all other cells whose value depends, directly
        /// or indirectly, on the named cell. The order of the list should be any
        /// order such that if cells are re-evaluated in that order, their dependencies 
        /// are satisfied by the time they are evaluated.
        /// 
        /// For example, if name is A1, B1 contains A1*2, and C1 contains B1+A1, the
        /// </summary>
        /// <param name="name">Name of the cell that should be set</param>
        /// <param name="content">Content the cell should be set to</param>
        /// <returns></returns>
        public override IList<String> SetContentsOfCell(String name, String content)
        {
            name = Normalize(name);
            IList<string> cellDependents;

            CellNameExceptionHandling(name);

            if (content is null)
                throw new ArgumentNullException();

            //DOUBLE
            if (Regex.IsMatch(content, @"^(\d+\.\d*|\d*\.\d+|\d+)+$"))
                cellDependents = SetCellContents(name, Double.Parse(content));

            //FORMULA
            else if (content != "" && content[0] == '=')
            {
                Formula cellFormula = new Formula(content.Substring(1), Normalize, IsValid);

                cellDependents = SetCellContents(name, cellFormula);
            }

            //STRING
            else
                cellDependents = SetCellContents(name, content);

            if(content.Trim() != "")
                Changed = true;

            //Updates the values of all of the dependents
            foreach (string s in cellDependents)
            {
                if (Cells.ContainsKey(s) && Cells[s].CellContent is Formula)
                    Cells[s].CellValue = ((Formula)Cells[s].CellContent).Evaluate(CellValueLookup);
            }

            return cellDependents;
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
        protected override IList<String> SetCellContents(String name, double number)
        {
            name = Normalize(name);

            if (Cells.ContainsKey(name))
            {
                //It only removes the dependencies if it is a formula
                if (Cells[name].CellContent is Formula)
                {
                    foreach (string variable in ((Formula)Cells[name].CellContent).GetVariables())
                        Dependencies.RemoveDependency(variable, name);
                }
                Cells[name].CellContent = number;
                Cells[name].CellValue = number;
            }

            //If the name already has a cell, replace the cell
            else
                Cells.Add(name, new Cell(name, number));

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
        protected override IList<String> SetCellContents(String name, String text)
        {
            name = Normalize(name);

            if (Cells.ContainsKey(name))
            {
                if (Cells[name].CellContent is Formula)
                {
                    foreach (string variable in ((Formula)Cells[name].CellContent).GetVariables())
                        Dependencies.RemoveDependency(variable, name);
                }

                if (text == "")
                    Cells.Remove(name);
                else
                {
                    Cells[name].CellContent = text;
                    Cells[name].CellValue = text;
                }

            }
            else if (text != "")
                Cells.Add(name, new Cell(name, text));


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
        protected override IList<String> SetCellContents(String name, Formula formula)
        {
            name = Normalize(name);

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
        /// Returns an enumeration, without duplicates, of the names of all cells whose
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
            name = Normalize(name);
            CellNameExceptionHandling(name);

            foreach (string dependent in Dependencies.GetDependents(name))
                yield return dependent;
        }

        /// <summary>
        /// Lookup formula to be passed into a formula evaluate method. It returns the value of the inputted
        /// cell as a double. If the cell's value is a string or FormulaError, it throws an ArgumentException.
        /// </summary>
        /// <param name="cell">Name of cell that's being looked up</param>
        /// <returns>Value of the cell as a double</returns>
        protected double CellValueLookup(string cell)
        {
            object value = GetCellValue(cell);
            if (value is string || value is FormulaError)
                throw new ArgumentException();

            return (double)value;
        }

        /// <summary>
        /// Helper method that takes in a string and makes sure that it's valid and that it isn't null. If
        /// it's either invalid or null, it throws an InvalidNameException. This is used to test cell name
        /// inputs.
        /// </summary>
        /// <param name="s">Name that shoudl be tested (string)</param>
        private void CellNameExceptionHandling(string s)
        {
            if (s is null || !IsNameValid(s))
                throw new InvalidNameException();
        }

        /// <summary>
        /// Tests if a string fits the format of a cell name. This format is that it is one or more letters
        /// followed by one or more digits.
        /// </summary>
        /// <param name="s">String to be tested</param>
        /// <returns>Whether or not the string is a valid cell name</returns>
        private bool IsNameValid(string s)
        {
            return Regex.IsMatch(s, @"^[a-zA-Z]+\d+$") && IsValid(s);
        }

        /// <summary>
        /// Private cell class for the elements of the dictionary. Each cell
        /// stores it's name, value, and content.
        /// 
        /// A cell's content can only be a double, string, or formula.
        /// 
        /// A cell's value can only be a double, string, or FormulaError.
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
                CellValue = value;
            }

            /// <summary>
            /// Constructor for if the cell holds a string
            /// </summary>
            /// <param name="name">Name of the cell</param>
            /// <param name="text">String that should be stored in the cell</param>
            public Cell(string name, string text)
            {
                Name = name;
                CellContent = text;
                CellValue = text;
            }

            /// <summary>
            /// Constructor for if the cell holds a formula. It calculates the value in the SetContentsOfCell
            /// method.
            /// </summary>
            /// <param name="name">Name of the cell</param>
            /// <param name="formula">Formula that should be stored in the cell</param>
            public Cell(string name, Formula formula)
            {
                Name = name;
                CellContent = formula;
            }

            /// <summary>
            /// Immutable name property of the cell. This way the only way to change the name is by creating
            /// a new cell.
            /// </summary>
            public string Name
            {
                get;
                private set;
            }

            /// <summary>
            /// Content the cell is storing.
            /// </summary>
            public object CellContent;

            /// <summary>
            /// Value that the cell is storing.
            /// </summary>
            public object CellValue;
        }
    }
}
