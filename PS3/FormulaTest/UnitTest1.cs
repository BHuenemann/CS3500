using Microsoft.VisualStudio.TestTools.UnitTesting;
using SpreadsheetUtilities;

namespace FormulaTest
{
    [TestClass]
    public class FormulaTest
    {
        double TestLookup(string n)
        {
            if (n == "X")
                return 5;
            else if (n == "X1")
                return 2;
            throw new System.ArgumentException("InvalidVariable");
        }



        [TestMethod]
        public void SingleInputInteger()
        {
            Formula f = new Formula("2");
            Assert.AreEqual(2.0, f.Evaluate(TestLookup));
        }



        [TestMethod]
        public void SingleInputFloat()
        {
            Formula f = new Formula("5.0");
            Assert.AreEqual(5.0, f.Evaluate(TestLookup));
        }



        [TestMethod]
        public void SingleInputScientificNotationLarge()
        {
            Formula f = new Formula("5E+9");
            Assert.AreEqual(5000000000.0, f.Evaluate(TestLookup));
        }



        [TestMethod]
        public void SingleInputScientificNotationSmall()
        {
            Formula f = new Formula("2E-5");
            Assert.AreEqual(0.00002, f.Evaluate(TestLookup));
        }



        [TestMethod]
        public void ScientificNotationUpperCase()
        {
            Formula f = new Formula("7E+6");
            Assert.AreEqual(7000000.0, f.Evaluate(TestLookup));
        }



        [TestMethod]
        public void ScientificNotationLowerCase()
        {
            Formula f = new Formula("3e+5");
            Assert.AreEqual(300000.0, f.Evaluate(TestLookup));
        }



        [TestMethod]
        public void IntegerAddition()
        {
            Formula f = new Formula("3 + 4");
            Assert.AreEqual(7.0, f.Evaluate(TestLookup));
        }



        [TestMethod]
        public void FloatAddition()
        {
            Formula f = new Formula("3.0 + 4.0");
            Assert.AreEqual(7.0, f.Evaluate(TestLookup));
        }



        [TestMethod]
        public void FloatAndIntegerAddition()
        {
            Formula f = new Formula("3 + 4.0");
            Assert.AreEqual(7.0, f.Evaluate(TestLookup));
        }



        [TestMethod]
        public void AdditionWithScientificNotation()
        {
            Formula f = new Formula("3.0E+7 + 4.0E+4 + 10");
            Assert.AreEqual(30040010.0, f.Evaluate(TestLookup));
        }



        [TestMethod]
        public void IntegerSubtraction()
        {
            Formula f = new Formula("4 - 3");
            Assert.AreEqual(1.0, f.Evaluate(TestLookup));
        }



        [TestMethod]
        public void FloatSubtraction()
        {
            Formula f = new Formula("4.0 - 3.0");
            Assert.AreEqual(1.0, f.Evaluate(TestLookup));
        }



        [TestMethod]
        public void FloatAndIntegerSubtraction()
        {
            Formula f = new Formula("4 - 3.0");
            Assert.AreEqual(1.0, f.Evaluate(TestLookup));
        }



        [TestMethod]
        public void SubtractionWithScientificNotation()
        {
            Formula f = new Formula("3.0E+7 - 4.0E+4 - 10");
            Assert.AreEqual(29959990.0, f.Evaluate(TestLookup));
        }



        public void SubtractionNegativeResult()
        {
            Formula f = new Formula("5 - 7");
            Assert.AreEqual(-2.0, f.Evaluate(TestLookup));
        }



        [TestMethod]
        public void IntegerMultiplication()
        {
            Formula f = new Formula("4 * 3");
            Assert.AreEqual(12.0, f.Evaluate(TestLookup));
        }



        [TestMethod]
        public void FloatMultiplication()
        {
            Formula f = new Formula("4.0 * 3.0");
            Assert.AreEqual(12.0, f.Evaluate(TestLookup));
        }



        [TestMethod]
        public void FloatAndIntegerMultiplication()
        {
            Formula f = new Formula("3 * 4.0");
            Assert.AreEqual(12.0, f.Evaluate(TestLookup));
        }



        [TestMethod]
        public void MultiplicationWithScientificNotation()
        {
            Formula f = new Formula("3E+2 * 3E+2 * 10");
            Assert.AreEqual(900000.0, f.Evaluate(TestLookup));
        }



        [TestMethod]
        public void IntegerDivision()
        {
            Formula f = new Formula("4 / 3");
            Assert.AreEqual(1.33333333333333, f.Evaluate(TestLookup), "1e-9");
        }



        [TestMethod]
        public void FloatDivision()
        {
            Formula f = new Formula("4.0 / 3.0");
            Assert.AreEqual(1.33333333333333, f.Evaluate(TestLookup), "1e-9");
        }



        [TestMethod]
        public void FloatAndIntegerDivision()
        {
            Formula f = new Formula("4 / 3.0");
            Assert.AreEqual(1.33333333333333, f.Evaluate(TestLookup), "1e-9");
        }



        [TestMethod]
        public void DivisionWithScientificNotation()
        {
            Formula f = new Formula("9E+10 / 1E+2");
            Assert.AreEqual(900000000.0, f.Evaluate(TestLookup));
        }



        [TestMethod]
        [ExpectedException(typeof(FormulaError))]
        public void DividingByZero()
        {
            Formula f = new Formula("9E+10 / 1E+2");
            f.Evaluate(TestLookup);
        }
    }
}
