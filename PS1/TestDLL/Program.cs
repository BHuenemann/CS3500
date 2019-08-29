using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestDLL
{
    class Program
    {
        static void Main(string[] args)
        {
            //Try catch loop in case of error
            try
            {
                Console.WriteLine(FormulaEvaluator.Evaluator.Evaluate("1 + 3 + 3", DelMethod));
                Console.WriteLine(FormulaEvaluator.Evaluator.Evaluate("5 * 2", DelMethod));
                Console.WriteLine(FormulaEvaluator.Evaluator.Evaluate("1 - 7", DelMethod));
                Console.WriteLine(FormulaEvaluator.Evaluator.Evaluate("7 / 2", DelMethod));
                Console.WriteLine(FormulaEvaluator.Evaluator.Evaluate("1+      2 + 3", DelMethod));
                Console.WriteLine(FormulaEvaluator.Evaluator.Evaluate("(5)", DelMethod));
                Console.WriteLine(FormulaEvaluator.Evaluator.Evaluate("12", DelMethod));
                Console.WriteLine(FormulaEvaluator.Evaluator.Evaluate("5 * (6 - 4)", DelMethod));
                Console.WriteLine(FormulaEvaluator.Evaluator.Evaluate("A1 * 7", DelMethod));
                Console.WriteLine(FormulaEvaluator.Evaluator.Evaluate("A1 + (BuB45 / 15)", DelMethod));


                //ERROR STATEMENTS
                //               Console.WriteLine(FormulaEvaluator.Evaluator.Evaluate("", DelMethod));
                //               Console.WriteLine(FormulaEvaluator.Evaluator.Evaluate("A1A", DelMethod));
                //               Console.WriteLine(FormulaEvaluator.Evaluator.Evaluate("C7", DelMethod));
                //               Console.WriteLine(FormulaEvaluator.Evaluator.Evaluate("1 / 0", DelMethod));
                //               Console.WriteLine(FormulaEvaluator.Evaluator.Evaluate("7 + 8) * 2", DelMethod));
                //               Console.WriteLine(FormulaEvaluator.Evaluator.Evaluate("(4 + 8", DelMethod));
            }
            catch (ArgumentException)
            {
                Console.WriteLine("Invalid Expression");
            }
            finally
            {
                Console.WriteLine("Type any key to close this window");
                Console.ReadKey();
            }
        }

        //Function for variable definitions
        public static int DelMethod(string s)
        {
            if (s == "A1")
                return -5;
            else if (s == "BuB45")
                return 7;
            if (s == "A1A")
                return 5;
            return 0;
        }
    }
}
