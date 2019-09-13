using Microsoft.VisualStudio.TestTools.UnitTesting;
using SpreadsheetUtilities;
using System;
using System.Collections.Generic;

namespace FormulaTest
{
    [TestClass]
    public class FormulaTest
    {
        double TestLookup(string n)
        {
            if (n == "a1")
                return 1;
            else if (n == "A1")
                return 2;
            throw new System.ArgumentException("InvalidVariable");
        }



        [TestMethod]
        public void SingleInputInteger()
        {
            Formula f = new Formula("2");
            Assert.AreEqual(2.0, f.Evaluate(s => 0));
        }



        [TestMethod]
        public void SingleInputFloat()
        {
            Formula f = new Formula("5.0");
            Assert.AreEqual(5.0, f.Evaluate(s => 0));
        }



        [TestMethod]
        public void SingleInputScientificNotationLarge()
        {
            Formula f = new Formula("5E+9");
            Assert.AreEqual(5000000000.0, f.Evaluate(s => 0));
        }



        [TestMethod]
        public void SingleInputScientificNotationSmall()
        {
            Formula f = new Formula("2E-5");
            Assert.AreEqual(0.00002, f.Evaluate(s => 0));
        }



        [TestMethod]
        public void ScientificNotationUpperCase()
        {
            Formula f = new Formula("7E+6");
            Assert.AreEqual(7000000.0, f.Evaluate(s => 0));
        }



        [TestMethod]
        public void ScientificNotationLowerCase()
        {
            Formula f = new Formula("3e+5");
            Assert.AreEqual(300000.0, f.Evaluate(s => 0));
        }



        [TestMethod]
        public void IntegerAddition()
        {
            Formula f = new Formula("3 + 4");
            Assert.AreEqual(7.0, f.Evaluate(s => 0));
        }



        [TestMethod]
        public void FloatAddition()
        {
            Formula f = new Formula("3.0 + 4.0");
            Assert.AreEqual(7.0, f.Evaluate(s => 0));
        }



        [TestMethod]
        public void FloatAndIntegerAddition()
        {
            Formula f = new Formula("3 + 4.0");
            Assert.AreEqual(7.0, f.Evaluate(s => 0));
        }



        [TestMethod]
        public void AdditionWithScientificNotation()
        {
            Formula f = new Formula("3.0E+7 + 4.0E+4 + 10");
            Assert.AreEqual(30040010.0, f.Evaluate(s => 0));
        }



        [TestMethod]
        public void IntegerSubtraction()
        {
            Formula f = new Formula("4 - 3");
            Assert.AreEqual(1.0, f.Evaluate(s => 0));
        }



        [TestMethod]
        public void FloatSubtraction()
        {
            Formula f = new Formula("4.0 - 3.0");
            Assert.AreEqual(1.0, f.Evaluate(s => 0));
        }



        [TestMethod]
        public void FloatAndIntegerSubtraction()
        {
            Formula f = new Formula("4 - 3.0");
            Assert.AreEqual(1.0, f.Evaluate(s => 0));
        }



        [TestMethod]
        public void SubtractionWithScientificNotation()
        {
            Formula f = new Formula("3.0E+7 - 4.0E+4 - 10");
            Assert.AreEqual(29959990.0, f.Evaluate(s => 0));
        }



        public void SubtractionNegativeResult()
        {
            Formula f = new Formula("5 - 7");
            Assert.AreEqual(-2.0, f.Evaluate(s => 0));
        }



        [TestMethod]
        public void IntegerMultiplication()
        {
            Formula f = new Formula("4 * 3");
            Assert.AreEqual(12.0, f.Evaluate(s => 0));
        }



        [TestMethod]
        public void FloatMultiplication()
        {
            Formula f = new Formula("4.0 * 3.0");
            Assert.AreEqual(12.0, f.Evaluate(s => 0));
        }



        [TestMethod]
        public void FloatAndIntegerMultiplication()
        {
            Formula f = new Formula("3 * 4.0");
            Assert.AreEqual(12.0, f.Evaluate(s => 0));
        }



        [TestMethod]
        public void MultiplicationWithScientificNotation()
        {
            Formula f = new Formula("3E+2 * 3E+2 * 10");
            Assert.AreEqual(900000.0, f.Evaluate(s => 0));
        }



        [TestMethod]
        public void IntegerDivision()
        {
            Formula f = new Formula("4 / 3");
            Assert.AreEqual(4.0 / 3.0, (double)f.Evaluate(s => 0), 1e-9);
        }



        [TestMethod]
        public void FloatDivision()
        {
            Formula f = new Formula("4.0 / 3.0");
            Assert.AreEqual(4.0 / 3.0, (double)f.Evaluate(s => 0), 1e-9);
        }



        [TestMethod]
        public void FloatAndIntegerDivision()
        {
            Formula f = new Formula("4 / 3.0");
            Assert.AreEqual(4.0 / 3.0, (double)f.Evaluate(s => 0), 1e-9);
        }



        [TestMethod]
        public void DivisionWithScientificNotation()
        {
            Formula f = new Formula("9E+10 / 1E+2");
            Assert.AreEqual(900000000.0, f.Evaluate(s => 0));
        }



        [TestMethod]
        public void DividingByZero()
        {
            Formula f = new Formula("1 / 0");
            Assert.IsTrue(f.Evaluate(s => 0) is FormulaError);
        }



        [TestMethod]
        public void DividingByZeroWithParen()
        {
            Formula f = new Formula("(1 + 2) / 0");
            Assert.IsTrue(f.Evaluate(s => 0) is FormulaError);
        }



        [TestMethod]
        public void DividingByZeroWithVariable()
        {
            Formula f = new Formula("a1 / 0");
            Assert.IsTrue(f.Evaluate(s => 1) is FormulaError);
        }



        [TestMethod]
        public void UnknownVariable()
        {
            Formula f = new Formula("1 + X1");
            Assert.IsTrue(f.Evaluate(s => { throw new Exception(); }) is FormulaError);
        }



        [TestMethod]
        public void KnownVariable()
        {
            Formula f = new Formula("1 + X1");
            Assert.AreEqual(3.0, f.Evaluate(s => (s == "X1") ? 2 : 1));
        }



        [TestMethod]
        public void VariableWithUnderscore()
        {
            Formula f = new Formula("1 + _X1");
            Assert.AreEqual(3.0, f.Evaluate(s => (s == "_X1") ? 2 : 1));
        }



        [TestMethod]
        public void LeftToRight()
        {
            Formula f = new Formula("5 * 2 + 7");
            Assert.AreEqual(17.0, f.Evaluate(s => 0));
        }



        [TestMethod]
        public void OrderOfOperations()
        {
            Formula f = new Formula("5 * 2 + 7");
            Assert.AreEqual(17.0, f.Evaluate(s => 0));
        }



        [TestMethod]
        public void ParenthesesTimes()
        {
            Formula f = new Formula("(2+6)*3");
            Assert.AreEqual(24.0, f.Evaluate(s => 0));
        }



        [TestMethod]
        public void TimesParentheses()
        {
            Formula f = new Formula("2*(3+5)");
            Assert.AreEqual(16.0, f.Evaluate(s => 0));
        }



        [TestMethod]
        public void PlusParentheses()
        {
            Formula f = new Formula("2+(3+5)");
            Assert.AreEqual(10.0, f.Evaluate(s => 0));
        }



        [TestMethod]
        public void ComplicatedPlus()
        {
            Formula f = new Formula("2+(3+5*9)");
            Assert.AreEqual(50.0, f.Evaluate(s => 0));
        }



        [TestMethod]
        public void OperatorAfterParens()
        {
            Formula f = new Formula("(1*1)-2/2");
            Assert.AreEqual(0.0, f.Evaluate(s => 0));
        }



        [TestMethod]
        public void ComplicatedTimesParentheses()
        {
            Formula f = new Formula("2+3*(3+5)");
            Assert.AreEqual(26.0, f.Evaluate(s => 0));
        }



        [TestMethod]
        public void ComplicatedAndParentheses()
        {
            Formula f = new Formula("2+3*5+(3+4*8)*5+2");
            Assert.AreEqual(194.0, f.Evaluate(s => 0));
        }



        [TestMethod]
        [ExpectedException(typeof(FormulaFormatException))]
        public void SingleOperator()
        {
            Formula f = new Formula("+");
            f.Evaluate(s => 0);
        }



        [TestMethod]
        [ExpectedException(typeof(FormulaFormatException))]
        public void ExtraOperator()
        {
            Formula f = new Formula("2+5+");
            f.Evaluate(s => 0);
        }



        [TestMethod]
        [ExpectedException(typeof(FormulaFormatException))]
        public void ExtraRightParentheses()
        {
            Formula f = new Formula("2+5*7)");
            f.Evaluate(s => 0);
        }



        [TestMethod]
        [ExpectedException(typeof(FormulaFormatException))]
        public void ExtraLeftParentheses()
        {
            Formula f = new Formula("(2+5*7");
            f.Evaluate(s => 0);
        }



        [TestMethod]
        [ExpectedException(typeof(FormulaFormatException))]
        public void ParensNoOperator()
        {
            Formula f = new Formula("5+7+(5)8");
            f.Evaluate(s => 0);
        }



        [TestMethod]
        [ExpectedException(typeof(FormulaFormatException))]
        public void Empty()
        {
            Formula f = new Formula("");
            f.Evaluate(s => 0);
        }



        [TestMethod]
        public void ComplicatedMultiVar()
        {
            Formula f = new Formula("y1*3-8/2+4*(8-9*2)/14*x7");
            Assert.AreEqual(5.14285714285714, (double)f.Evaluate(s => (s == "x7") ? 1 : 4), 1e-9);
        }



        [TestMethod]
        public void ComplicatedNestedParensRight()
        {
            Formula f = new Formula("x1+(x2+(x3+(x4+(x5+x6))))");
            Assert.AreEqual(6.0, f.Evaluate(s => 1));
        }



        [TestMethod]
        public void ComplicatedNestedParensLeft()
        {
            Formula f = new Formula("((((x1+x2)+x3)+x4)+x5)+x6");
            Assert.AreEqual(12.0, f.Evaluate(s => 2));
        }



        [TestMethod]
        public void RepeatedVar()
        {
            Formula f = new Formula("a4-a4*a4/a4");
            Assert.AreEqual(0.0, f.Evaluate(s => 3));
        }



        [TestMethod]
        public void NoStatic()
        {
            Formula f1 = new Formula("10 - 5");
            Formula f2 = new Formula("1 + 5");

            Assert.AreEqual(6.0, f2.Evaluate(s => 0));
            Assert.AreEqual(5.0, f1.Evaluate(s => 0));
        }



        [TestMethod]
        [ExpectedException(typeof(FormulaFormatException))]
        public void OperatorThenClosingParen()
        {
            Formula f = new Formula("(1+)");
            f.Evaluate(s => 0);
        }



        [TestMethod]
        [ExpectedException(typeof(FormulaFormatException))]
        public void OpeningAndClosingParen()
        {
            Formula f = new Formula("1 + ()");
            f.Evaluate(s => 0);
        }



        [TestMethod]
        [ExpectedException(typeof(FormulaFormatException))]
        public void OpeningParenThenOperator()
        {
            Formula f = new Formula("5 (+6)");
            f.Evaluate(s => 0);
        }



        /// <summary>
        /// These variables should all be normalized and set equal to 2 instead of 1
        /// </summary>
        [TestMethod]
        public void NormalizerTest()
        {
            Formula f = new Formula("a1 + a1 + A1", s => s.ToUpper(), s => true);
            Assert.AreEqual(6.0, f.Evaluate(TestLookup));
        }



        [TestMethod]
        public void IsValidAndNormalizerTest()
        {
            Formula f = new Formula("a1 + 6", s => s.ToUpper(), s => (s == "A1"));
            Assert.AreEqual(8.0, f.Evaluate(TestLookup));
        }



        [TestMethod]
        [ExpectedException(typeof(FormulaFormatException))]
        public void IsValidTest()
        {
            Formula f = new Formula("a1 + 6", s => s, s => (s == "A3"));
        }



        [TestMethod]
        [ExpectedException(typeof(FormulaFormatException))]
        public void NormalizedNotVariable()
        {
            Formula f = new Formula("a1 + 6", s => "1AB2", s => true);
        }



        [TestMethod]
        [ExpectedException(typeof(FormulaFormatException))]
        public void InvalidToken()
        {
            Formula f = new Formula("/? + 6");
        }



        [TestMethod]
        public void GetVariables()
        {
            Formula f = new Formula("(((x1+x2)+x3))");
            IEnumerator<string> t = f.GetVariables().GetEnumerator();

            Assert.IsTrue(t.MoveNext());
            Assert.AreEqual("x1", t.Current);
            Assert.IsTrue(t.MoveNext());
            Assert.AreEqual("x2", t.Current);
            Assert.IsTrue(t.MoveNext());
            Assert.AreEqual("x3", t.Current);
            Assert.IsFalse(t.MoveNext());
        }



        [TestMethod]
        public void ToStringWithFloats()
        {
            Formula f = new Formula("2 + 2.0");
            Assert.AreEqual("2+2", f.ToString());
        }



        [TestMethod]
        public void ToStringWithScientific()
        {
            Formula f = new Formula("2 + 2e6");
            Assert.AreEqual("2+2e6", f.ToString());
        }



        [TestMethod]
        public void ToStringWithVariables()
        {
            Formula f = new Formula("((((x1+x2)+x3)+x4)+x5)+x6");
            Assert.AreEqual("((((x1+x2)+x3)+x4)+x5)+x6", f.ToString());
        }



        [TestMethod]
        public void EqualsOperatorTest()
        {
            Formula f1 = new Formula("1 + 5 + a2+ 6   + 7.0");
            Formula f2 = new Formula("1+5+a2+6+7");
            Formula f3 = new Formula("1+a2+6+7");
            Assert.IsTrue(f1 == f2);
            Assert.IsFalse(f1 != f2);
            Assert.IsFalse(f1 == f3);
            Assert.IsTrue(f1 != f3);
        }



        [TestMethod]
        public void EqualsFunctionTest()
        {
            Formula f1 = new Formula("1 + 5 + a2+ 6   + 7.0");
            Formula f2 = new Formula("1+5+a2+6+7");
            Formula f3 = new Formula("1+a2+6+7");
            Assert.IsTrue(f1.Equals(f2));
            Assert.IsFalse(f1.Equals(f3));
        }



        [TestMethod]
        public void EqualsOperatorOneNullTest()
        {
            Formula f1 = null;
            Formula f2 = new Formula("1+5+6+7");
            Assert.IsFalse(f1 == f2);
            Assert.IsTrue(f1 != f2);
        }



        [TestMethod]
        public void EqualsOperatorBothNullTest()
        {
            Formula f1 = null;
            Formula f2 = null;
            Assert.IsTrue(f1 == f2);
            Assert.IsFalse(f1 != f2);
        }



        [TestMethod]
        public void EqualsFunctionOneNullTest()
        {
            Formula f1 = new Formula("1+5+6+7");
            Formula f2 = null;
            Assert.IsFalse(f1.Equals(f2));
        }



        [TestMethod]
        public void HashCodeTest()
        {
            Formula f1 = new Formula("1 + 5 + a2+ 6   + 7.0");
            Formula f2 = new Formula("1+5+a2+6+7");
            Formula f3 = new Formula("1+a2+6+7");
            Assert.IsTrue(f1.GetHashCode() == f2.GetHashCode());
            Assert.IsFalse(f1.GetHashCode() == f3.GetHashCode());
        }
    }
}
