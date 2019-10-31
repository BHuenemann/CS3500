using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace FormulaEvaluator
{
    //Class for new stack commands
    public static class StackExtensions
    {
        /// <summary>
        /// Takes in a value and checks to see if it's on top of the stack. It returns false if the stack is empty.
        /// </summary>
        /// <typeparam name="E">Generic for the stack type</typeparam>
        /// <param name="s">Stack being used</param>
        /// <param name="n">Variable to look for on top of stack</param>
        /// <returns></returns>
        public static Boolean isOnTop<E>(this Stack<E> s, E n)
        {
            if (s.Count == 0)
                return false;
            else return s.Peek().Equals(n);
        }
    }


    public static class Evaluator
    {
        /// <summary>
        /// Delegate guideline for a method to evaluate variables
        /// </summary>
        /// <param name="v">Variable name</param>
        /// <returns>Variable value</returns>
        public delegate int Lookup(String v);

        /// <summary>
        /// Method used to do simple calculations with +-* and /.
        /// </summary>
        /// <param name="num1">First number input</param>
        /// <param name="num2">Second number input</param>
        /// <param name="op">Operation to be performed between the two numbers (+-* or /)</param>
        /// <returns>Calculated value of the expression</returns>
        public static int CalculateExpression(int num1, int num2, string op)
        {
            if (op == "+")
                return num1 + num2;
            else if (op == "-")
                return num1 - num2;
            else if (op == "*")
                return num1 * num2;
            else if (op == "/")
            {
                //Uses this if statement to avoid dividing by 0
                if (num2 == 0)
                    throw new ArgumentException("Error dividing by 0");
                return num1 / num2;
            }

            //Invalid operator was inputted (This error shouldn't ever happen but just in case)
            throw new ArgumentException("Invalid operator");
        }

        /// <summary>
        /// Method that calculates an expression using two numbers from the first stack and one operation from the second stack.
        /// </summary>
        /// <param name="stack1">Stack of values</param>
        /// <param name="stack2">Stack of operators</param>
        /// <returns>Calculated value of the expression between the stack values</returns>
        public static int CalcFromStacks(Stack<int> stack1, Stack<string> stack2)
        {
            int num1 = stack1.Pop();
            int num2 = stack1.Pop();
            string op = stack2.Pop();

            return CalculateExpression(num2, num1, op);
        }

        /// <summary>
        /// Method for evaluating an arithmatic expression
        /// </summary>
        /// <param name="exp">Expression to be evaluated</param>
        /// <param name="variableEvaluator">Method for interpretting any variables</param>
        /// <returns>Value of the simplified expression</returns>
        public static int Evaluate(String exp, Lookup variableEvaluator)
        {
            //Splits the expression into parts and removes the whitespace leading and tailing each component
            string[] substrings = Regex.Split(exp, "(\\()|(\\))|(-)|(\\+)|(\\*)|(/)");
            for (int i = 0; i < substrings.Length; i++)
            {
                substrings[i] = substrings[i].Trim();
            }

            //Creates stacks to hold the values and operators
            Stack<int> valueStack = new Stack<int>();
            Stack<string> operatorStack = new Stack<string>();

            //Goes through each token and manipulates the stacks
            for (int i = 0; i < substrings.Length; i++)
            {
                //This is the case in that the token is either a number or a variable
                if (Regex.IsMatch(substrings[i], @"^\d+$") || Regex.IsMatch(substrings[i], @"^[a-zA-Z]+\d+$"))
                {
                    int num1;

                    //If it's a number, convert the string into an integer. Otherwise convert the variable into an integer using the inputted function.
                    if (Regex.IsMatch(substrings[i], @"^\d+$"))
                        num1 = Convert.ToInt32(substrings[i]);
                    else
                        num1 = variableEvaluator(substrings[i]);

                    /* If it's empty or the stack doesn't have * or / on the top it just pushes the number to the value stack. Otherwise it uses the
                     * number and the top values of each stack to form an expression, evaluate it, and push it to the value stack. */
                    if (!operatorStack.isOnTop("*") && !operatorStack.isOnTop("/"))
                        valueStack.Push(num1);
                    else
                    {
                        int num2 = valueStack.Pop();
                        string op = operatorStack.Pop();

                        valueStack.Push(CalculateExpression(num2, num1, op));
                    }
                }

                //It just adds the operator to the stack if it's a *, /, or (
                else if (substrings[i] == "*" || substrings[i] == "/" || substrings[i] == "(")
                    operatorStack.Push(substrings[i]);

                //This is the case that the token is a + or -
                else if (substrings[i] == "+" || substrings[i] == "-")
                {
                    /* In either case it pushes the operator to the operator stack but if + or - is next on the stack it also uses the top values
                     * from each stack to form an expression, evaluate it, and push it to the value stack. */
                    if (!operatorStack.isOnTop("+") && !operatorStack.isOnTop("-"))
                        operatorStack.Push(substrings[i]);
                    else
                    {
                        valueStack.Push(CalcFromStacks(valueStack, operatorStack));
                        operatorStack.Push(substrings[i]);
                    }
                }

                //This is the case that the token is a )
                else if (substrings[i] == ")")
                {
                    //If the top value is a + or - it uses the top values from both stacks to form an expression and push it
                    if (operatorStack.isOnTop("+") || operatorStack.isOnTop("-"))
                        valueStack.Push(CalcFromStacks(valueStack, operatorStack));

                    /* It throws an exception if the operator stack is empty or if ( isn't next on the stack. Assuming there's no error it pops (
                     * off the stack. */
                    if (!operatorStack.isOnTop("("))
                        throw new ArgumentException("Unmatched parentheses");
                    operatorStack.Pop();

                    /* If it's not empty and the operator stack has * or / on the top, it uses the values of each stack to form an expression, evaluate it,
                     * and push it to the value stack. */
                    if (operatorStack.isOnTop("*") || operatorStack.isOnTop("/"))
                        valueStack.Push(CalcFromStacks(valueStack, operatorStack));

                }
                //It throws an exeption for any other cases as long as they aren't an empty string
                else if (substrings[i] != "")
                    throw new ArgumentException("Invalid token");
            }

            //If there's just a number remaining it returns that
            if (operatorStack.Count == 0)
            {
                //It throws an exception if there's an incorrect amount of remaining values on the stack
                if (valueStack.Count != 1)
                    throw new ArgumentException("Invalid expression");
                return valueStack.Pop();
            }
            //Otherwise it does one final calculation and returns that number
            else
            {
                //It throws an exception if there's an incorrect amount of remaining values on the stack or an incorrect amount of operators on the other stack
                if (operatorStack.Count != 1 || valueStack.Count != 2)
                    throw new ArgumentException("Invalid expression");
                return CalcFromStacks(valueStack, operatorStack);
                //
            }
        }
    }
}
