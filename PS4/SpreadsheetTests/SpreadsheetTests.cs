//Author: Ben Huenemann

using Microsoft.VisualStudio.TestTools.UnitTesting;
using SpreadsheetUtilities;
using SS;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Xml;

namespace SpreadsheetTests
{

    [TestClass]
    public class SpreadsheetTests
    {
        public void CreateValidXML(string name)
        {
            XmlWriterSettings settings = new XmlWriterSettings();
            settings.Indent = true;
            settings.IndentChars = "\t";

            using (XmlWriter writer = XmlWriter.Create(name))
            {
                writer.WriteStartDocument();


                writer.WriteStartElement("spreadsheet");
                writer.WriteAttributeString("version", "1.0");


                writer.WriteStartElement("cell");
                writer.WriteElementString("name", "A1");
                writer.WriteElementString("content", "15");
                writer.WriteEndElement();

                writer.WriteStartElement("cell");
                writer.WriteElementString("name", "A2");
                writer.WriteElementString("content", "5");
                writer.WriteEndElement();

                writer.WriteStartElement("cell");
                writer.WriteElementString("name", "B1");
                writer.WriteElementString("content", "=A1+A2");
                writer.WriteEndElement();

                writer.WriteStartElement("cell");
                writer.WriteElementString("name", "B3");
                writer.WriteElementString("content", "text");
                writer.WriteEndElement();


                writer.WriteEndElement();


                writer.WriteEndDocument();
            }
        }



        public void CreateInvalidOrderXML(string name)
        {
            XmlWriterSettings settings = new XmlWriterSettings();
            settings.Indent = true;
            settings.IndentChars = "\t";

            using (XmlWriter writer = XmlWriter.Create(name))
            {
                writer.WriteStartDocument();


                writer.WriteStartElement("spreadsheet");
                writer.WriteAttributeString("version", "1.0");


                writer.WriteStartElement("cell");
                writer.WriteElementString("name", "A1");
                writer.WriteElementString("content", "15");

                writer.WriteStartElement("cell");
                writer.WriteElementString("name", "A2");
                writer.WriteElementString("content", "5");
                writer.WriteEndElement();


                writer.WriteEndElement();


                writer.WriteEndDocument();
            }
        }



        public void CreateInvalidCellNameXML(string name)
        {
            XmlWriterSettings settings = new XmlWriterSettings();
            settings.Indent = true;
            settings.IndentChars = "\t";

            using (XmlWriter writer = XmlWriter.Create(name))
            {
                writer.WriteStartDocument();


                writer.WriteStartElement("spreadsheet");
                writer.WriteAttributeString("version", "1.0");


                writer.WriteStartElement("cell");
                writer.WriteElementString("name", "11A");
                writer.WriteElementString("content", "15");
                writer.WriteEndElement();


                writer.WriteEndElement();


                writer.WriteEndDocument();
            }
        }



        public void CreateInvalidCircularXML(string name)
        {
            XmlWriterSettings settings = new XmlWriterSettings();
            settings.Indent = true;
            settings.IndentChars = "\t";

            using (XmlWriter writer = XmlWriter.Create(name))
            {
                writer.WriteStartDocument();


                writer.WriteStartElement("spreadsheet");
                writer.WriteAttributeString("version", "1.0");


                writer.WriteStartElement("cell");
                writer.WriteElementString("name", "A1");
                writer.WriteElementString("content", "=A2");
                writer.WriteEndElement();

                writer.WriteStartElement("cell");
                writer.WriteElementString("name", "A2");
                writer.WriteElementString("content", "=A1");
                writer.WriteEndElement();


                writer.WriteEndElement();


                writer.WriteEndDocument();
            }
        }



        public void CreateNoVersionXML(string name)
        {
            XmlWriterSettings settings = new XmlWriterSettings();
            settings.Indent = true;
            settings.IndentChars = "\t";

            using (XmlWriter writer = XmlWriter.Create(name))
            {
                writer.WriteStartDocument();


                writer.WriteStartElement("spreadsheets");


                writer.WriteStartElement("cell");
                writer.WriteElementString("name", "A1");
                writer.WriteElementString("content", "=A2");
                writer.WriteEndElement();

                writer.WriteStartElement("cell");
                writer.WriteElementString("name", "A2");
                writer.WriteElementString("content", "=A1");
                writer.WriteEndElement();


                writer.WriteEndElement();


                writer.WriteEndDocument();
            }
        }



        public void CreateInvalidFormulaXML(string name)
        {
            XmlWriterSettings settings = new XmlWriterSettings();
            settings.Indent = true;
            settings.IndentChars = "\t";

            using (XmlWriter writer = XmlWriter.Create(name))
            {
                writer.WriteStartDocument();


                writer.WriteStartElement("spreadsheet");
                writer.WriteAttributeString("version", "1.0");


                writer.WriteStartElement("cell");
                writer.WriteElementString("name", "A1");
                writer.WriteElementString("content", "=1++3");
                writer.WriteEndElement();


                writer.WriteEndElement();


                writer.WriteEndDocument();
            }
        }



        public void CreateInvalidEndXML(string name)
        {
            XmlWriterSettings settings = new XmlWriterSettings();
            settings.Indent = true;
            settings.IndentChars = "\t";

            using (XmlWriter writer = XmlWriter.Create(name))
            {
                writer.WriteStartDocument();


                writer.WriteStartElement("spreadsheet");
                writer.WriteAttributeString("version", "1.0");


                writer.WriteStartElement("cell");
                writer.WriteElementString("name", "A1");
                writer.WriteElementString("content", "=1++3");


                writer.WriteEndDocument();
            }
        }



        public void CreateInvalidElementXML(string name)
        {
            XmlWriterSettings settings = new XmlWriterSettings();
            settings.Indent = true;
            settings.IndentChars = "\t";

            using (XmlWriter writer = XmlWriter.Create(name))
            {
                writer.WriteStartDocument();


                writer.WriteStartElement("spreadsheet");
                writer.WriteAttributeString("version", "1.0");


                writer.WriteStartElement("cellular");


                writer.WriteEndElement();


                writer.WriteEndDocument();
            }
        }



        [TestMethod]
        public void TestReadXML()
        {
            CreateValidXML("Test1.xml");
            Spreadsheet s = new Spreadsheet("Test1.xml", n => true, n => n, "1.0");

            Assert.AreEqual(15.0, s.GetCellContents("A1"));
            Assert.AreEqual(15.0, s.GetCellValue("A1"));
            Assert.AreEqual(5.0, s.GetCellContents("A2"));
            Assert.AreEqual(5.0, s.GetCellValue("A2"));
            Assert.AreEqual(new Formula("A1 + A2"), s.GetCellContents("B1"));
            Assert.AreEqual(20.0, s.GetCellValue("B1"));
            Assert.AreEqual("text", s.GetCellContents("B3"));
            Assert.AreEqual("text", s.GetCellValue("B3"));
        }



        [ExpectedException(typeof(SpreadsheetReadWriteException))]
        [TestMethod]
        public void TestReadXMLWrongVersion()
        {
            CreateValidXML("Test2.xml");
            Spreadsheet s = new Spreadsheet("Test2.xml", n => true, n => n, "BestVersion");
        }



        [ExpectedException(typeof(SpreadsheetReadWriteException))]
        [TestMethod]
        public void TestReadXMLInvalidFileName()
        {
            Spreadsheet s = new Spreadsheet("Invalid.xml", n => true, n => n, "1.0");
        }



        [ExpectedException(typeof(SpreadsheetReadWriteException))]
        [TestMethod]
        public void TestReadXMLInvalidElementOrder()
        {
            CreateInvalidOrderXML("Test3.xml");
            Spreadsheet s = new Spreadsheet("Test3.xml", n => true, n => n, "1.0");
        }



        [ExpectedException(typeof(SpreadsheetReadWriteException))]
        [TestMethod]
        public void TestReadXMLInvalidElementName()
        {
            CreateInvalidCellNameXML("Test4.xml");
            Spreadsheet s = new Spreadsheet("Test4.xml", n => true, n => n, "1.0");
        }



        [ExpectedException(typeof(SpreadsheetReadWriteException))]
        [TestMethod]
        public void TestReadXMLInvalidCircularError()
        {
            CreateInvalidCircularXML("Test5.xml");
            Spreadsheet s = new Spreadsheet("Test5.xml", n => true, n => n, "1.0");
        }



        [ExpectedException(typeof(SpreadsheetReadWriteException))]
        [TestMethod]
        public void TestReadXMLInvalidFormula()
        {
            CreateInvalidFormulaXML("Test6.xml");
            Spreadsheet s = new Spreadsheet("Test6.xml", n => true, n => n, "1.0");
        }



        [ExpectedException(typeof(SpreadsheetReadWriteException))]
        [TestMethod]
        public void TestReadXMLInvalidElement()
        {
            CreateInvalidElementXML("Test7.xml");
            Spreadsheet s = new Spreadsheet("Test7.xml", n => true, n => n, "1.0");
        }



        [ExpectedException(typeof(SpreadsheetReadWriteException))]
        [TestMethod]
        public void TestReadXMLInvalidEnding()
        {
            CreateInvalidEndXML("Test8.xml");
            Spreadsheet s = new Spreadsheet("Test8.xml", n => true, n => n, "1.0");
        }



        [ExpectedException(typeof(SpreadsheetReadWriteException))]
        [TestMethod]
        public void TestReadXMLNoVersion()
        {
            CreateNoVersionXML("Test9.xml");
            Spreadsheet s = new Spreadsheet("Test9.xml", n => true, n => n, "1.0");
        }



        [ExpectedException(typeof(SpreadsheetReadWriteException))]
        [TestMethod]
        public void TestReadXMLFileNull()
        {
            Spreadsheet s = new Spreadsheet((string)null, n => true, n => n, "1.0");
        }



        [TestMethod]
        public void TestWriteXML()
        {
            Spreadsheet s1 = new Spreadsheet();

            s1.SetContentsOfCell("A1", "15");
            s1.SetContentsOfCell("A2", "5");
            s1.SetContentsOfCell("B1", "=A1 + A2");
            s1.SetContentsOfCell("B3", "text");

            s1.Save("Test10.xml");

            Spreadsheet s2 = new Spreadsheet("Test10.xml", n => true, n => n, "default");

            Assert.AreEqual(15.0, s2.GetCellContents("A1"));
            Assert.AreEqual(15.0, s2.GetCellValue("A1"));
            Assert.AreEqual(5.0, s2.GetCellContents("A2"));
            Assert.AreEqual(5.0, s2.GetCellValue("A2"));
            Assert.AreEqual(new Formula("A1 + A2"), s2.GetCellContents("B1"));
            Assert.AreEqual(20.0, s2.GetCellValue("B1"));
            Assert.AreEqual("text", s2.GetCellContents("B3"));
            Assert.AreEqual("text", s2.GetCellValue("B3"));

        }
        


        [ExpectedException(typeof(SpreadsheetReadWriteException))]
        [TestMethod]
        public void TestWriteXMLNull()
        {
            Spreadsheet s = new Spreadsheet();

            s.SetContentsOfCell("A1", "15");
            s.SetContentsOfCell("A2", "5");
            s.SetContentsOfCell("B1", "=A1 + A2");
            s.SetContentsOfCell("B3", "text");

            s.Save((string) null);
        }



        [TestMethod]
        public void TestChanged()
        {
            Spreadsheet s = new Spreadsheet();
            Assert.IsFalse(s.Changed);
            s.SetContentsOfCell("A1", "4.0");
            Assert.IsTrue(s.Changed);
        }



        [ExpectedException(typeof(InvalidNameException))]
        [TestMethod]
        public void TestDelegates()
        {
            Spreadsheet s = new Spreadsheet(n => n.Length == 2, n => n.ToUpper(), "1.0");
            s.SetContentsOfCell("a1", "4.0");
            Assert.AreEqual(4.0, s.GetCellContents("A1"));
            s.SetContentsOfCell("AA1", "4.0");
        }



        [TestMethod]
        public void TestValueDouble()
        {
            Spreadsheet s = new Spreadsheet();
            s.SetContentsOfCell("A1", "1.5");
            Assert.AreEqual(1.5, s.GetCellValue("A1"));
        }



        [TestMethod]
        public void TestValueFormulaDouble()
        {
            Spreadsheet s = new Spreadsheet();
            s.SetContentsOfCell("A1", "=1.5");
            Assert.AreEqual(1.5, s.GetCellValue("A1"));
        }



        [TestMethod]
        public void TestValueComplexFormulaDouble()
        {
            Spreadsheet s = new Spreadsheet();
            s.SetContentsOfCell("B1", "=1.5");
            s.SetContentsOfCell("C1", "=5.5");
            s.SetContentsOfCell("A1", "=C1 - B1");
            Assert.AreEqual(4.0, s.GetCellValue("A1"));
        }



        [TestMethod]
        public void TestValueString()
        {
            Spreadsheet s = new Spreadsheet();
            s.SetContentsOfCell("A1", "why not");
            Assert.AreEqual("why not", s.GetCellValue("A1"));
        }



        [TestMethod]
        public void TestValueFormulaError()
        {
            Spreadsheet s = new Spreadsheet();
            s.SetContentsOfCell("A1", "=1/0");
            Assert.IsTrue(s.GetCellValue("A1") is FormulaError);
        }



        [TestMethod]
        public void SetCellToEmpty()
        {
            Spreadsheet s = new Spreadsheet();
            s.SetContentsOfCell("A1", "10");
            s.SetContentsOfCell("A1", "");
            Assert.AreEqual("", s.GetCellValue("A1"));
        }
        


        [TestMethod]
        public void SetCellFromFormulaToString()
        {
            Spreadsheet s = new Spreadsheet();
            s.SetContentsOfCell("A1", "=A2 + A3");
            s.SetContentsOfCell("A2", "4");
            s.SetContentsOfCell("A3", "10");
            s.SetContentsOfCell("A1", "text");
            Assert.AreEqual("text", s.GetCellValue("A1"));
        }



        [TestMethod]
        public void StressTest1()
        {
            Spreadsheet s = new Spreadsheet();

            s.SetContentsOfCell("A200", "1");

            for (int i = 0; i < 200; i++)
            {
                s.SetContentsOfCell("A" + i, "=A" + (i + 1));
            }

            for (int i = 0; i < 201; i++)
            {
                Assert.AreEqual(1.0, s.GetCellValue("A" + i));
            }

            s.SetContentsOfCell("A200", "10");

            for (int i = 0; i < 201; i++)
            {
                Assert.AreEqual(10.0, s.GetCellValue("A" + i));
            }
        }



        [TestMethod]
        public void StressTest1a()
        {
            StressTest1();
        }



        [TestMethod]
        public void StressTest1b()
        {
            StressTest1();
        }



        [TestMethod]
        public void StressTest1c()
        {
            StressTest1();
        }



        [ExpectedException(typeof(CircularException))]
        [TestMethod]
        public void StressTest2()
        {
            Spreadsheet s = new Spreadsheet();

            s.SetContentsOfCell("A200", "=A1");

            for (int i = 0; i < 200; i++)
            {
                s.SetContentsOfCell("A" + i, "=A" + (i + 1));
            }
        }



        [ExpectedException(typeof(CircularException))]
        [TestMethod]
        public void StressTest2a()
        {
            StressTest2();
        }



        [ExpectedException(typeof(CircularException))]
        [TestMethod]
        public void StressTest2b()
        {
            StressTest2();
        }



        [ExpectedException(typeof(CircularException))]
        [TestMethod]
        public void StressTest2c()
        {
            StressTest2();
        }



        [TestMethod]
        public void StressTest3()
        {
            Spreadsheet s = new Spreadsheet();

            s.SetContentsOfCell("A200", "1");

            for (int i = 0; i < 200; i++)
            {
                s.SetContentsOfCell("A" + i, "=A" + (i + 1));
            }

            for (int i = 0; i < 201; i++)
            {
                Assert.AreEqual(1.0, s.GetCellValue("A" + i));
            }

            s.Save("Stress3.xml");

            Spreadsheet s2 = new Spreadsheet("Stress3.xml", i => true, i => i, "default");

            for (int i = 0; i < 201; i++)
            {
                Assert.AreEqual(1.0, s2.GetCellValue("A" + i));
            }
        }



        [TestMethod]
        public void StressTest3a()
        {
            StressTest3();
        }



        [TestMethod]
        public void StressTest3b()
        {
            StressTest3();
        }



        [TestMethod]
        public void StressTest3c()
        {
            StressTest3();
        }



        // PS4 GRADING TESTS
        // EMPTY SPREADSHEETS
        [TestMethod(), Timeout(5000)]
        [ExpectedException(typeof(InvalidNameException))]
        public void TestEmptyGetNull()
        {
            Spreadsheet s = new Spreadsheet();
            s.GetCellContents(null);
        }



        [TestMethod(), Timeout(5000)]
        [ExpectedException(typeof(InvalidNameException))]
        public void TestEmptyGetContents()
        {
            Spreadsheet s = new Spreadsheet();
            s.GetCellContents("1AA");
        }



        [TestMethod(), Timeout(5000)]
        public void TestGetEmptyContents()
        {
            Spreadsheet s = new Spreadsheet();
            Assert.AreEqual("", s.GetCellContents("A2"));
        }



        [TestMethod(), Timeout(5000)]
        [ExpectedException(typeof(InvalidNameException))]
        public void TestSetInvalidName()
        {
            Spreadsheet s = new Spreadsheet();
            s.SetContentsOfCell("1A1A", "1.5");
        }



        [TestMethod(), Timeout(5000)]
        public void TestSimpleSetDouble()
        {
            Spreadsheet s = new Spreadsheet();
            s.SetContentsOfCell("Z7", "1.5");
            Assert.AreEqual(1.5, (double)s.GetCellContents("Z7"), 1e-9);
        }



        // SETTING CELL TO A STRING
        [TestMethod(), Timeout(5000)]
        [ExpectedException(typeof(ArgumentNullException))]
        public void TestSetNullVal()
        {
            Spreadsheet s = new Spreadsheet();
            s.SetContentsOfCell("A8", null);
        }



        [TestMethod(), Timeout(5000)]
        [ExpectedException(typeof(InvalidNameException))]
        public void TestSetNullName()
        {
            Spreadsheet s = new Spreadsheet();
            s.SetContentsOfCell(null, "hello");
        }



        [TestMethod(), Timeout(5000)]
        [ExpectedException(typeof(InvalidNameException))]
        public void TestSetSimpleString()
        {
            Spreadsheet s = new Spreadsheet();
            s.SetContentsOfCell("1AZ", "hello");
        }



        [TestMethod(), Timeout(5000)]
        public void TestSetGetSimpleString()
        {
            Spreadsheet s = new Spreadsheet();
            s.SetContentsOfCell("Z7", "hello");
            Assert.AreEqual("hello", s.GetCellContents("Z7"));
        }



        [TestMethod(), Timeout(5000)]
        [ExpectedException(typeof(InvalidNameException))]
        public void TestSetSimpleForm()
        {
            Spreadsheet s = new Spreadsheet();
            s.SetContentsOfCell("1AZ", "=2");
        }



        [TestMethod(), Timeout(5000)]
        public void TestSetGetForm()
        {
            Spreadsheet s = new Spreadsheet();
            s.SetContentsOfCell("Z7", "=3");
            Formula f = (Formula)s.GetCellContents("Z7");
            Assert.AreEqual(new Formula("3"), f);
            Assert.AreNotEqual(new Formula("2"), f);
        }



        // CIRCULAR FORMULA DETECTION
        [TestMethod(), Timeout(5000)]
        [ExpectedException(typeof(CircularException))]
        public void TestSimpleCircular()
        {
            Spreadsheet s = new Spreadsheet();
            s.SetContentsOfCell("A1", "=A2");
            s.SetContentsOfCell("A2", "=A1");
        }



        [TestMethod(), Timeout(5000)]
        [ExpectedException(typeof(CircularException))]
        public void TestComplexCircular()
        {
            Spreadsheet s = new Spreadsheet();
            s.SetContentsOfCell("A1", "=A2+A3");
            s.SetContentsOfCell("A3", "=A4+A5");
            s.SetContentsOfCell("A5", "=A6+A7");
            s.SetContentsOfCell("A7", "=A1+A1");
        }



        [TestMethod(), Timeout(5000)]
        [ExpectedException(typeof(CircularException))]
        public void TestUndoCircular()
        {
            Spreadsheet s = new Spreadsheet();
            try
            {
                s.SetContentsOfCell("A1", "=A2+A3");
                s.SetContentsOfCell("A2", "15");
                s.SetContentsOfCell("A3", "30");
                s.SetContentsOfCell("A2", "=A3*A1");
            }
            catch (CircularException e)
            {
                Assert.AreEqual(15, (double)s.GetCellContents("A2"), 1e-9);
                throw e;
            }
        }



        // NONEMPTY CELLS
        [TestMethod(), Timeout(5000)]
        public void TestEmptyNames()
        {
            Spreadsheet s = new Spreadsheet();
            Assert.IsFalse(s.GetNamesOfAllNonemptyCells().GetEnumerator().MoveNext());
        }



        [TestMethod(), Timeout(5000)]
        public void TestExplicitEmptySet()
        {
            Spreadsheet s = new Spreadsheet();
            s.SetContentsOfCell("B1", "");
            Assert.IsFalse(s.GetNamesOfAllNonemptyCells().GetEnumerator().MoveNext());
        }



        [TestMethod(), Timeout(5000)]
        public void TestSimpleNamesString()
        {
            Spreadsheet s = new Spreadsheet();
            s.SetContentsOfCell("B1", "hello");
            Assert.IsTrue(new HashSet<string>(s.GetNamesOfAllNonemptyCells()).SetEquals(new HashSet<string>() { "B1" }));
        }



        [TestMethod(), Timeout(5000)]
        public void TestSimpleNamesDouble()
        {
            Spreadsheet s = new Spreadsheet();
            s.SetContentsOfCell("B1", "52.25");
            Assert.IsTrue(new HashSet<string>(s.GetNamesOfAllNonemptyCells()).SetEquals(new HashSet<string>() { "B1" }));
        }



        [TestMethod(), Timeout(5000)]
        public void TestSimpleNamesFormula()
        {
            Spreadsheet s = new Spreadsheet();
            s.SetContentsOfCell("B1", "=3.5");
            Assert.IsTrue(new HashSet<string>(s.GetNamesOfAllNonemptyCells()).SetEquals(new HashSet<string>() { "B1" }));
        }



        [TestMethod(), Timeout(5000)]
        public void TestMixedNames()
        {
            Spreadsheet s = new Spreadsheet();
            s.SetContentsOfCell("A1", "17.2");
            s.SetContentsOfCell("C1", "hello");
            s.SetContentsOfCell("B1", "=3.5");
            Assert.IsTrue(new HashSet<string>(s.GetNamesOfAllNonemptyCells()).SetEquals(new HashSet<string>() { "A1", "B1", "C1" }));
        }



        // RETURN VALUE OF SET CELL CONTENTS
        [TestMethod(), Timeout(5000)]
        public void TestSetSingletonDouble()
        {
            Spreadsheet s = new Spreadsheet();
            s.SetContentsOfCell("B1", "hello");
            s.SetContentsOfCell("C1", "=5");
            Assert.IsTrue(s.SetContentsOfCell("A1", "=17.2").SequenceEqual(new List<string>() { "A1" }));
        }



        [TestMethod(), Timeout(5000)]
        public void TestSetSingletonString()
        {
            Spreadsheet s = new Spreadsheet();
            s.SetContentsOfCell("A1", "17.2");
            s.SetContentsOfCell("C1", "=5");
            Assert.IsTrue(s.SetContentsOfCell("B1", "hello").SequenceEqual(new List<string>() { "B1" }));
        }



        [TestMethod(), Timeout(5000)]
        public void TestSetSingletonFormula()
        {
            Spreadsheet s = new Spreadsheet();
            s.SetContentsOfCell("A1", "17.2");
            s.SetContentsOfCell("B1", "hello");
            Assert.IsTrue(s.SetContentsOfCell("C1", "=5").SequenceEqual(new List<string>() { "C1" }));
        }



        [TestMethod(), Timeout(5000)]
        public void TestSetChain()
        {
            Spreadsheet s = new Spreadsheet();
            s.SetContentsOfCell("A1", "=A2+A3");
            s.SetContentsOfCell("A2", "6");
            s.SetContentsOfCell("A3", "=A2+A4");
            s.SetContentsOfCell("A4", "=A2+A5");
            Assert.IsTrue(s.SetContentsOfCell("A5", "82.5").SequenceEqual(new List<string>() { "A5", "A4", "A3", "A1" }));
        }



        // CHANGING CELLS
        [TestMethod(), Timeout(5000)]
        public void TestChangeFtoD()
        {
            Spreadsheet s = new Spreadsheet();
            s.SetContentsOfCell("A1", "=A2+A3");
            s.SetContentsOfCell("A1", "2.5");
            Assert.AreEqual(2.5, (double)s.GetCellContents("A1"), 1e-9);
        }



        [TestMethod(), Timeout(5000)]
        public void TestChangeFtoS()
        {
            Spreadsheet s = new Spreadsheet();
            s.SetContentsOfCell("A1", "A2+A3");
            s.SetContentsOfCell("A1", "Hello");
            Assert.AreEqual("Hello", (string)s.GetCellContents("A1"));
        }



        [TestMethod(), Timeout(5000)]
        public void TestChangeStoF()
        {
            Spreadsheet s = new Spreadsheet();
            s.SetContentsOfCell("A1", "Hello");
            s.SetContentsOfCell("A1", "=23");
            Assert.AreEqual(new Formula("23"), (Formula)s.GetCellContents("A1"));
            Assert.AreNotEqual(new Formula("24"), (Formula)s.GetCellContents("A1"));
        }
    }
}
