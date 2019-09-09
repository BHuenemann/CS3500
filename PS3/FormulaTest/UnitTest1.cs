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
            Assert.AreEqual(f.Evaluate(TestLookup), 2.0);
        }

        [TestMethod]
        public void SingleInputFloat()
        {
            Formula f = new Formula("5.0");
            Assert.AreEqual(f.Evaluate(TestLookup), 5.0);
        }

        [TestMethod]
        public void SingleVariable()
        {
            Formula f = new Formula("X");
            Assert.AreEqual(f.Evaluate(TestLookup), 5.0);
        }
    }
}
