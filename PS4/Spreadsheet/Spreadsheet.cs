using SpreadsheetUtilities;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace SS
{
    public class Spreadsheet : AbstractSpreadsheet
    {
        private DependencyGraph Dependencies;
        private IDictionary<string, Cell> Cells;

        public Spreadsheet()
        {
            Dependencies = new DependencyGraph();
            Cells = new Dictionary<string, Cell>();
        }

        public override IEnumerable<String> GetNamesOfAllNonemptyCells()
        {
            foreach (KeyValuePair<string, Cell> CellPair in Cells)
                yield return CellPair.Key.ToString();
        }

        public override object GetCellContents(String name)
        {
            if (name is null || !IsVariable(name))
                throw new InvalidNameException();

            if (!Cells.ContainsKey(name))
                return "";
            return Cells[name].CellContent;
        }

        public override IList<String> SetCellContents(String name, double number)
        {
            if (!IsVariable(name))
                throw new InvalidNameException();

            if (!Cells.ContainsKey(name))
                Cells.Add(name, new Cell(number));
            else
                Cells[name] = new Cell(number);

            return GetCellsToRecalculate(name).ToList();
        }

        public override IList<String> SetCellContents(String name, String text)
        {
            if (text is null || text == "")
                throw new ArgumentNullException();
            if (!IsVariable(name))
                throw new InvalidNameException();

            if (!Cells.ContainsKey(name))
                Cells.Add(name, new Cell(text));
            else
                Cells[name] = new Cell(text);

            return GetCellsToRecalculate(name).ToList();
        }

        public override IList<String> SetCellContents(String name, Formula formula)
        {
            if (!IsVariable(name))
                throw new InvalidNameException();
            if (formula is null)
                throw new ArgumentNullException();

            if (!Cells.ContainsKey(name))
                Cells.Add(name, new Cell(formula));
            else
                Cells[name] = new Cell(formula);

            foreach (string variable in formula.GetVariables())
                Dependencies.AddDependency(variable, name);

            return GetCellsToRecalculate(name).ToList();
        }

        protected override IEnumerable<String> GetDirectDependents(String name)
        {
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

        private class Cell
        {
            public Cell(double n)
            {
                CellContent = n;
            }
            public Cell(string s)
            {
                CellContent = s;
            }
            public Cell(Formula f)
            {
                CellContent = f;
            }

            public object CellContent
            {
                get;
                private set;
            }
        }
    }
}
