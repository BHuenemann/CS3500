using Microsoft.VisualStudio.TestTools.UnitTesting;
using SpreadsheetUtilities;
using SS;
using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace SpreadsheetTests
{
    [TestClass]
    public class SpreadsheetTests
    {
        [TestMethod]
        public void CreateNewSpreadsheet()
        {
            AbstractSpreadsheet s = new Spreadsheet();
        }


        [TestMethod]
        public void IsNotStatic()
        {
            AbstractSpreadsheet s1 = new Spreadsheet();
            AbstractSpreadsheet s2 = new Spreadsheet();

            s1.SetCellContents("A1", 2.0);

            Assert.AreEqual("", s2.GetCellContents("A1"));
        }


        [TestMethod]
        public void GetContentsFromCell()
        {
            AbstractSpreadsheet s = new Spreadsheet();
            Formula f = new Formula("1 + A1");

            s.SetCellContents("A1", 5.0);
            s.SetCellContents("B5", "Why Not");
            s.SetCellContents("A20", f);

            Assert.AreEqual(5.0, s.GetCellContents("A1"));
            Assert.AreEqual("Why Not", s.GetCellContents("B5"));
            Assert.AreEqual(f, s.GetCellContents("A20"));
        }


        [ExpectedException(typeof(InvalidNameException))]
        [TestMethod]
        public void GetContentsEmptyName()
        {
            AbstractSpreadsheet s = new Spreadsheet();

            s.GetCellContents("");
        }


        [ExpectedException(typeof(InvalidNameException))]
        [TestMethod]
        public void GetContentsNullName()
        {
            AbstractSpreadsheet s = new Spreadsheet();
            String st = null;

            s.GetCellContents(st);
        }


        [TestMethod]
        public void GetNonemptyCellNames()
        {
            AbstractSpreadsheet s = new Spreadsheet();
            Formula f = new Formula("1 + A1");

            String[] names = { "A1", "z1", "bSj", "Bbn44a3", "A2", "z22", "bS4j", "Bbn54a3", "Aa1", "zy1", "b2Sj", "Bbn445a3" };

            for(int i = 0; i < 4; i++)
            {
                s.SetCellContents(names[i], 5.0);
                s.SetCellContents(names[4+i], "Why Not");
                s.SetCellContents(names[8+i], f);
            }

            HashSet<string> namesSet = new HashSet<string>(names);
            HashSet<string> cellNames = new HashSet<string>(s.GetNamesOfAllNonemptyCells());

            Assert.IsTrue(namesSet.SetEquals(cellNames));
        }


        [TestMethod]
        public void ReplaceContents()
        {
            AbstractSpreadsheet s = new Spreadsheet();
            Formula f1 = new Formula("1 + A1");
            Formula f2 = new Formula("2 + A1");

            s.SetCellContents("a1", 2.0);
            s.SetCellContents("b5", "Why");
            s.SetCellContents("a20", f1);

            s.SetCellContents("a1", 5.0);
            s.SetCellContents("b5", "Why not");
            s.SetCellContents("a20", f2);

            Assert.AreEqual(5.0, s.GetCellContents("a1"));
            Assert.AreEqual("Why not", s.GetCellContents("b5"));
            Assert.AreEqual(f2, s.GetCellContents("a20"));
        }


        [TestMethod]
        public void SetCellToDouble()
        {
            AbstractSpreadsheet s = new Spreadsheet();

            IList<String> list = s.SetCellContents("a1", 2.0);
            Assert.AreEqual("a1", list[0]);
        }


        [TestMethod]
        public void SetCellToInteger()
        {
            AbstractSpreadsheet s = new Spreadsheet();

            IList<String> list = s.SetCellContents("a1", 2);
            Assert.AreEqual("a1", list[0]);
        }


        [TestMethod]
        public void SetCellToString()
        {
            AbstractSpreadsheet s = new Spreadsheet();

            IList<String> list = s.SetCellContents("a1", "Mantis Shrimp");
            Assert.AreEqual("a1", list[0]);
        }


        [TestMethod]
        public void SetCellToFormula()
        {
            AbstractSpreadsheet s = new Spreadsheet();
            Formula f = new Formula("b1 + 2 / 5");

            IList<String> list = s.SetCellContents("a1", f);
            Assert.AreEqual("a1", list[0]);
        }


        [TestMethod]
        [ExpectedException(typeof(InvalidNameException))]
        public void SetCellDoubleNameError()
        {
            AbstractSpreadsheet s = new Spreadsheet();

            s.SetCellContents("1Ab2", 2.0);
        }


        [TestMethod]
        [ExpectedException(typeof(InvalidNameException))]
        public void SetCellStringNameError()
        {
            AbstractSpreadsheet s = new Spreadsheet();

            s.SetCellContents("1Ab2", "Mantis Shrimp");
        }


        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void SetCellStringEmptyTextError()
        {
            AbstractSpreadsheet s = new Spreadsheet();

            s.SetCellContents("A2", "");
        }


        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void SetCellStringNullTextError()
        {
            AbstractSpreadsheet s = new Spreadsheet();
            string st = null;

            s.SetCellContents("A2", st);
        }


        [TestMethod]
        [ExpectedException(typeof(InvalidNameException))]
        public void SetCellFormulaNameError()
        {
            AbstractSpreadsheet s = new Spreadsheet();
            Formula f = new Formula("a1 + 2 / 5");

            s.SetCellContents("1Ab2", f);
        }


        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void SetCellFormulaNullError()
        {
            AbstractSpreadsheet s = new Spreadsheet();
            Formula f = null;

            s.SetCellContents("A2", f);
        }


        [TestMethod]
        [ExpectedException(typeof(CircularException))]
        public void TwoStepCircular()
        {
            AbstractSpreadsheet s = new Spreadsheet();
            Formula f1 = new Formula("B1");
            Formula f2 = new Formula("A1");

            s.SetCellContents("A1", f1);
            s.SetCellContents("B1", f2);
        }


        [TestMethod]
        [ExpectedException(typeof(CircularException))]
        public void MultiStepCircular()
        {
            AbstractSpreadsheet s = new Spreadsheet();
            Formula f1 = new Formula("B1");
            Formula f2 = new Formula("C1");
            Formula f3 = new Formula("D1");
            Formula f4 = new Formula("A1");

            s.SetCellContents("A1", f1);
            s.SetCellContents("B1", f2);
            s.SetCellContents("C1", f3);
            s.SetCellContents("D1", f4);
        }


        /// <summary>
        /// Tests if the list of dependents is returned when setting a cell to a double
        /// </summary>
        [TestMethod]
        public void DependentsReturnedWithNumber()
        {
            AbstractSpreadsheet s = new Spreadsheet();
            Formula f1 = new Formula("a1 + 2");
            Formula f2 = new Formula("a1 - b1");
            Formula f3 = new Formula("b1");

            s.SetCellContents("b1", f1);
            s.SetCellContents("c1", f2);
            s.SetCellContents("d1", f3);
            IEnumerator<String> list = s.SetCellContents("a1", 2.0).GetEnumerator();

            Assert.IsTrue(list.MoveNext());
            Assert.AreEqual("a1", list.Current);
            Assert.IsTrue(list.MoveNext());
            Assert.AreEqual("b1", list.Current);
            Assert.IsTrue(list.MoveNext());
            Assert.AreEqual("d1", list.Current);
            Assert.IsTrue(list.MoveNext());
            Assert.AreEqual("c1", list.Current);
            Assert.IsFalse(list.MoveNext());
        }


        /// <summary>
        /// Tests if the list of dependents is returned when setting a cell to a string
        /// </summary>
        [TestMethod]
        public void DependentsReturnedWithString()
        {
            AbstractSpreadsheet s = new Spreadsheet();
            Formula f1 = new Formula("a1");
            Formula f2 = new Formula("b1");

            s.SetCellContents("b1", f1);
            s.SetCellContents("c1", f2);
            IEnumerator<String> list = s.SetCellContents("a1", "Cat").GetEnumerator();

            Assert.IsTrue(list.MoveNext());
            Assert.AreEqual("a1", list.Current);
            Assert.IsTrue(list.MoveNext());
            Assert.AreEqual("b1", list.Current);
            Assert.IsTrue(list.MoveNext());
            Assert.AreEqual("c1", list.Current);
            Assert.IsFalse(list.MoveNext());
        }


        /// <summary>
        /// Tests if the list of dependents is returned when setting a cell to a formula
        /// </summary>
        [TestMethod]
        public void DependentsReturnedWithFormula()
        {
            AbstractSpreadsheet s = new Spreadsheet();
            Formula f1 = new Formula("a1 + 2");
            Formula f2 = new Formula("a1 - b1");
            Formula f3 = new Formula("b1");

            s.SetCellContents("a1", 2.0);
            s.SetCellContents("c1", f2);
            s.SetCellContents("d1", f3);
            IEnumerator<String> list = s.SetCellContents("b1", f1).GetEnumerator();

            Assert.IsTrue(list.MoveNext());
            Assert.AreEqual("b1", list.Current);
            Assert.IsTrue(list.MoveNext());
            Assert.AreEqual("d1", list.Current);
            Assert.IsTrue(list.MoveNext());
            Assert.AreEqual("c1", list.Current);
            Assert.IsFalse(list.MoveNext());
        }


        /// <summary>
        /// A more complicated dependency test
        /// </summary>
        [TestMethod]
        public void ComplexDependency()
        {
            AbstractSpreadsheet s = new Spreadsheet();
            Formula f1 = new Formula("a1 + 2");
            Formula f2 = new Formula("a1 - b1");
            Formula f3 = new Formula("c1 - e1");
            Formula f4 = new Formula("b1");

            s.SetCellContents("b1", f1);
            s.SetCellContents("c1", f2);
            s.SetCellContents("d1", f3);
            s.SetCellContents("e1", f4);

            IEnumerator<String> list = s.SetCellContents("a1", 2.0).GetEnumerator();

            Assert.IsTrue(list.MoveNext());
            Assert.AreEqual("a1", list.Current);
            Assert.IsTrue(list.MoveNext());
            Assert.AreEqual("b1", list.Current);
            Assert.IsTrue(list.MoveNext());
            Assert.AreEqual("e1", list.Current);
            Assert.IsTrue(list.MoveNext());
            Assert.AreEqual("c1", list.Current);
            Assert.IsTrue(list.MoveNext());
            Assert.AreEqual("d1", list.Current);
            Assert.IsFalse(list.MoveNext());
        }


        /// <summary>
        /// This tests if a A1 will be different than a1 (it tests that case matters).
        /// </summary>
        [TestMethod]
        public void CaseSensitiveCells()
        {
            AbstractSpreadsheet s = new Spreadsheet();

            s.SetCellContents("a1", 2.0);
            s.SetCellContents("A1", 5.0);

            Assert.AreEqual(2.0, s.GetCellContents("a1"));
            Assert.AreEqual(5.0, s.GetCellContents("A1"));
        }
    }
}
