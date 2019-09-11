//Author: Ben Huenemann

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace SpreadsheetUtilities
{
    /// <summary>
    /// Class for added stack commands
    /// </summary>
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

    /// <summary>
    /// Represents formulas written in standard infix notation using standard precedence
    /// rules.  The allowed symbols are non-negative numbers written using double-precision 
    /// floating-point syntax (without unary preceeding '-' or '+'); 
    /// variables that consist of a letter or underscore followed by 
    /// zero or more letters, underscores, or digits; parentheses; and the four operator 
    /// symbols +, -, *, and /.  
    /// 
    /// Spaces are significant only insofar that they delimit tokens.  For example, "xy" is
    /// a single variable, "x y" consists of two variables "x" and y; "x23" is a single variable; 
    /// and "x 23" consists of a variable "x" and a number "23".
    /// 
    /// Associated with every formula are two delegates:  a normalizer and a validator.  The
    /// normalizer is used to convert variables into a canonical form, and the validator is used
    /// to add extra restrictions on the validity of a variable (beyond the standard requirement 
    /// that it consist of a letter or underscore followed by zero or more letters, underscores,
    /// or digits.)  Their use is described in detail in the constructor and method comments.
    /// </summary>
    public class Formula
    {
        private string[] tokens;

        /// <summary>
        /// Creates a Formula from a string that consists of an infix expression written as
        /// described in the class comment.  If the expression is syntactically invalid,
        /// throws a FormulaFormatException with an explanatory Message.
        /// 
        /// The associated normalizer is the identity function, and the associated validator
        /// maps every string to true.  
        /// </summary>
        public Formula(String formula) :
            this(formula, s => s, s => true)
        {
        }

        /// <summary>
        /// Creates a Formula from a string that consists of an infix expression written as
        /// described in the class comment.  If the expression is syntactically incorrect,
        /// throws a FormulaFormatException with an explanatory Message.
        /// 
        /// The associated normalizer and validator are the second and third parameters,
        /// respectively.  
        /// 
        /// If the formula contains a variable v such that normalize(v) is not a legal variable, 
        /// throws a FormulaFormatException with an explanatory message. 
        /// 
        /// If the formula contains a variable v such that isValid(normalize(v)) is false,
        /// throws a FormulaFormatException with an explanatory message.
        /// 
        /// Suppose that N is a method that converts all the letters in a string to upper case, and
        /// that V is a method that returns true only if a string consists of one letter followed
        /// by one digit.  Then:
        /// 
        /// new Formula("x2+y3", N, V) should succeed
        /// new Formula("x+y3", N, V) should throw an exception, since V(N("x")) is false
        /// new Formula("2x+y3", N, V) should throw an exception, since "2x+y3" is syntactically incorrect.
        /// </summary>
        public Formula(String formula, Func<string, string> normalize, Func<string, bool> isValid)
        {
            tokens = GetTokens(formula).ToArray();

            if (tokens.Length == 0)
                throw new FormulaFormatException("No tokens found");

            int leftParenCount = 0;
            int rightParenCount = 0;

            for (int i = 0; i < tokens.Length; i++)
            {
                if (i != 0)
                {
                    if ((tokens[i] == ")" || isOperator(tokens[i])) &&
                       (tokens[i - 1] == "(" || isOperator(tokens[i - 1])))
                        throw new FormulaFormatException("Operator/closing_parenthesis follows an operator/opening_parenthesis");
                    if ((tokens[i] == ")" || isVariable(normalize(tokens[i])) || isDoubleOrScientific(tokens[i])) &&
                        (isVariable(normalize(tokens[i - 1])) || isDoubleOrScientific(tokens[i - 1])))
                        throw new FormulaFormatException("Number/variable/closing_parenthesis follows a number/variable");
                }

                if (tokens[i] == "(")
                    leftParenCount++;
                else if (tokens[i] == ")")
                    rightParenCount++;
                else if (isVariable(normalize(tokens[i])))
                {
                    if (!isValid(normalize(tokens[i])))
                        throw new FormulaFormatException("Variable name doesn't fit validity specifications");

                    tokens[i] = normalize(tokens[i]);
                }
                else if (isVariable(tokens[i]))
                    throw new FormulaFormatException("Variable name doesn't fit specifications after normalization");
                else if (!isOperator(tokens[i]) && !isDoubleOrScientific(tokens[i]))
                    throw new FormulaFormatException("Invalid token");

                if (rightParenCount > leftParenCount)
                    throw new FormulaFormatException("Unmatched right parenthesis");
            }

            string startToken = tokens[0];
            string endToken = tokens[tokens.Length - 1];

            if (startToken != "(" && !isVariable(startToken) && !isDoubleOrScientific(startToken))
                throw new FormulaFormatException("Starting token isn't a number/variable/opening_parenthesis");
            else if (endToken != ")" && !isVariable(endToken) && !isDoubleOrScientific(endToken))
                throw new FormulaFormatException("Ending token isn't a number/variable/closing_parenthesis");

            if (leftParenCount != rightParenCount)
                throw new FormulaFormatException("Unbalanced parentheses");
        }

        /// <summary>
        /// Evaluates this Formula, using the lookup delegate to determine the values of
        /// variables.  When a variable symbol v needs to be determined, it should be looked up
        /// via lookup(normalize(v)). (Here, normalize is the normalizer that was passed to 
        /// the constructor.)
        /// 
        /// For example, if L("x") is 2, L("X") is 4, and N is a method that converts all the letters 
        /// in a string to upper case:
        /// 
        /// new Formula("x+7", N, s => true).Evaluate(L) is 11
        /// new Formula("x+7").Evaluate(L) is 9
        /// 
        /// Given a variable symbol as its parameter, lookup returns the variable's value 
        /// (if it has one) or throws an ArgumentException (otherwise).
        /// 
        /// If no undefined variables or divisions by zero are encountered when evaluating 
        /// this Formula, the value is returned.  Otherwise, a FormulaError is returned.  
        /// The Reason property of the FormulaError should have a meaningful explanation.
        ///
        /// This method should never throw an exception.
        /// </summary>
        public object Evaluate(Func<string, double> lookup)
        {
            //Temporary double used to pop numbers to the value stack
            double returnVal = 0;

            Stack<double> valueStack = new Stack<double>();
            Stack<string> operatorStack = new Stack<string>();

            for (int i = 0; i < tokens.Length; i++)
            {
                if (isDoubleOrScientific(tokens[i]) || isVariable(tokens[i]))
                {
                    double num1;

                    /*If it fits the format of a double, convert the string into an double. Otherwise convert the variable into an double using
                     * the inputted function. */
                    if (isDoubleOrScientific(tokens[i]))
                        Double.TryParse(tokens[i], out num1);
                    else
                        num1 = lookup(tokens[i]);

                    /* If it's empty or the stack doesn't have * or / on the top it just pushes the number to the value stack. Otherwise it uses
                     * the number and the top values of each stack to form an expression, evaluate it, and push it to the value stack. */
                    if (!operatorStack.isOnTop("*") && !operatorStack.isOnTop("/"))
                        valueStack.Push(num1);
                    else
                    {
                        double num2 = valueStack.Pop();
                        string op = operatorStack.Pop();

                        if (!TryCalculateExpression(num2, num1, op, ref returnVal))
                            return new FormulaError("Error dividing by 0");

                        valueStack.Push(returnVal);
                    }
                }

                //It just adds the operator to the stack if it's a *, /, or (
                else if (tokens[i] == "*" || tokens[i] == "/" || tokens[i] == "(")
                    operatorStack.Push(tokens[i]);

                //This is the case that the token is a + or -
                else if (tokens[i] == "+" || tokens[i] == "-")
                {
                    /* In either case it pushes the operator to the operator stack but if + or - is next on the stack it also uses the top values
                     * from each stack to form an expression, evaluate it, and push it to the value stack. */
                    if (!operatorStack.isOnTop("+") && !operatorStack.isOnTop("-"))
                        operatorStack.Push(tokens[i]);
                    else
                    {
                        if (!TryCalcFromStacks(valueStack, operatorStack, ref returnVal))
                            return new FormulaError("Error dividing by 0");

                        valueStack.Push(returnVal);
                        operatorStack.Push(tokens[i]);
                    }
                }

                //This is the case that the token is a )
                else if (tokens[i] == ")")
                {
                    //If the top value is a + or - it uses the top values from both stacks to form an expression and push it
                    if (operatorStack.isOnTop("+") || operatorStack.isOnTop("-"))
                    {
                        if (!TryCalcFromStacks(valueStack, operatorStack, ref returnVal))
                            return new FormulaError("Error dividing by 0");

                        valueStack.Push(returnVal);
                    }

                    operatorStack.Pop();

                    /* If it's not empty and the operator stack has * or / on the top, it uses the values of each stack to form an expression, evaluate it,
                     * and push it to the value stack. */
                    if (operatorStack.isOnTop("*") || operatorStack.isOnTop("/"))
                    {
                        if (!TryCalcFromStacks(valueStack, operatorStack, ref returnVal))
                            return new FormulaError("Error dividing by 0");

                        valueStack.Push(returnVal);
                    }

                }
            }

            //If there's just a number remaining it returns that
            if (operatorStack.Count == 0)
                return valueStack.Pop();

            //Otherwise it does one final calculation and returns that number
            else
            {
                if (!TryCalcFromStacks(valueStack, operatorStack, ref returnVal))
                    return new FormulaError("Error dividing by 0");
                return returnVal;
            }
        }

        /// <summary>
        /// Enumerates the normalized versions of all of the variables that occur in this 
        /// formula.  No normalization may appear more than once in the enumeration, even 
        /// if it appears more than once in this Formula.
        /// 
        /// For example, if N is a method that converts all the letters in a string to upper case:
        /// 
        /// new Formula("x+y*z", N, s => true).GetVariables() should enumerate "X", "Y", and "Z"
        /// new Formula("x+X*z", N, s => true).GetVariables() should enumerate "X" and "Z".
        /// new Formula("x+X*z").GetVariables() should enumerate "x", "X", and "z".
        /// </summary>
        public IEnumerable<String> GetVariables()
        {
            HashSet<string> variables = new HashSet<string>();

            foreach (string token in tokens)
            {
                if (isVariable(token))
                    variables.Add(token);
            }
            return variables;
        }

        /// <summary>
        /// Returns a string containing no spaces which, if passed to the Formula
        /// constructor, will produce a Formula f such that this.Equals(f).  All of the
        /// variables in the string should be normalized.
        /// 
        /// For example, if N is a method that converts all the letters in a string to upper case:
        /// 
        /// new Formula("x + y", N, s => true).ToString() should return "X+Y"
        /// new Formula("x + Y").ToString() should return "x+Y"
        /// </summary>
        public override string ToString()
        {
            return string.Join("", tokens);
        }

        /// <summary>
        /// If obj is null or obj is not a Formula, returns false.  Otherwise, reports
        /// whether or not this Formula and obj are equal.
        /// 
        /// Two Formulae are considered equal if they consist of the same tokens in the
        /// same order.  To determine token equality, all tokens are compared as strings 
        /// except for numeric tokens and variable tokens.
        /// Numeric tokens are considered equal if they are equal after being "normalized" 
        /// by C#'s standard conversion from string to double, then back to string. This 
        /// eliminates any inconsistencies due to limited floating point precision.
        /// Variable tokens are considered equal if their normalized forms are equal, as 
        /// defined by the provided normalizer.
        /// 
        /// For example, if N is a method that converts all the letters in a string to upper case:
        ///  
        /// new Formula("x1+y2", N, s => true).Equals(new Formula("X1  +  Y2")) is true
        /// new Formula("x1+y2").Equals(new Formula("X1+Y2")) is false
        /// new Formula("x1+y2").Equals(new Formula("y2+x1")) is false
        /// new Formula("2.0 + x7").Equals(new Formula("2.000 + x7")) is true
        /// </summary>
        public override bool Equals(object obj)
        {
            if (obj == null || obj.GetType() != typeof(Formula))
                return false;
            else
            {
                string[] otherTokens = GetTokens(obj.ToString()).ToArray();

                if (otherTokens.Length != tokens.Length || tokens.Length == 0)
                    return false;
                else
                {
                    bool returnBool = true;

                    for (int i = 0; i < tokens.Length; i++)
                    {
                        if (isDoubleOrScientific(tokens[i]) && isDoubleOrScientific(otherTokens[i]))
                            returnBool = returnBool && (Double.Parse(tokens[i]).ToString() == Double.Parse(otherTokens[i]).ToString());
                        else if (!isDoubleOrScientific(tokens[i]) && !isDoubleOrScientific(otherTokens[i]))
                            returnBool = returnBool && (tokens[i] == otherTokens[i]);
                        else
                            return false;

                        if (!returnBool)
                            return false;
                    }
                    return true;
                }
            }
        }

        /// <summary>
        /// Reports whether f1 == f2, using the notion of equality from the Equals method.
        /// Note that if both f1 and f2 are null, this method should return true.  If one is
        /// null and one is not, this method should return false.
        /// </summary>
        public static bool operator ==(Formula f1, Formula f2)
        {
            if (f1 is null && f2 is null)
                return true;
            else if (f1 is null || f2 is null)
                return false;

            return f1.Equals(f2);
        }

        /// <summary>
        /// Reports whether f1 != f2, using the notion of equality from the Equals method.
        /// Note that if both f1 and f2 are null, this method should return false.  If one is
        /// null and one is not, this method should return true.
        /// </summary>
        public static bool operator !=(Formula f1, Formula f2)
        {
            return !(f1 == f2);
        }

        /// <summary>
        /// Returns a hash code for this Formula.  If f1.Equals(f2), then it must be the
        /// case that f1.GetHashCode() == f2.GetHashCode().  Ideally, the probability that two 
        /// randomly-generated unequal Formulae have the same hash code should be extremely small.
        /// </summary>
        public override int GetHashCode()
        {
            return 0;
        }

        /// <summary>
        /// Method that calculates an expression using two numbers from the first stack and one operation from the second stack. If the
        /// calculation fails, it returns false.
        /// </summary>
        /// <param name="stack1">Stack of values</param>
        /// <param name="stack2">Stack of operators</param>
        /// <param name="output">Variable where the output of the operation will be saved</param>
        /// <returns>Whether the calculation succeeds or fails</returns>
        public static bool TryCalcFromStacks(Stack<double> stack1, Stack<string> stack2, ref double output)
        {
            double num1 = stack1.Pop();
            double num2 = stack1.Pop();
            string op = stack2.Pop();

            return TryCalculateExpression(num2, num1, op, ref output);
        }

        /// <summary>
        /// Method used to do simple calculations with +-* and /. It returns false if passed an invalid operator or when dividing by zero.
        /// </summary>
        /// <param name="num1">First number input</param>
        /// <param name="num2">Second number input</param>
        /// <param name="op">Operation to be performed between the two numbers (+-* or /)</param>
        /// <param name="output">Variable where the output of the operation will be saved</param>
        /// <returns>Whether or not it performs the operation</returns>
        public static bool TryCalculateExpression(double num1, double num2, string op, ref double output)
        {
            if (op == "+")
                output = num1 + num2;
            else if (op == "-")
                output = num1 - num2;
            else if (op == "*")
                output = num1 * num2;
            else if (op == "/") {
                if (num2 == 0)
                    return false;
                output = num1 / num2;
            }
            else return false;

            return true;
        }

        bool isOperator(string s)
        {
            return (s == "(" || s == ")" || s == "+" || s == "-" || s == "*" || s == "/");
        }

        bool isVariable(string s)
        {
            return Regex.IsMatch(s, @"[a-zA-Z_](?: [a-zA-Z_]|\d)*");
        }

        bool isDoubleOrScientific(string s)
        {
            bool returnVal = false;

            returnVal = returnVal || Regex.IsMatch(s, @"\d+\.\d*");
            returnVal = returnVal || Regex.IsMatch(s, @"\d*\.\d+");
            returnVal = returnVal || Regex.IsMatch(s, @"\d+");
            returnVal = returnVal || Regex.IsMatch(s, @"[eE][\+-]?\d+");
            return returnVal;
        }

        /// <summary>
        /// Given an expression, enumerates the tokens that compose it.  Tokens are left paren;
        /// right paren; one of the four operator symbols; a string consisting of a letter or underscore
        /// followed by zero or more letters, digits, or underscores; a double literal; and anything that doesn't
        /// match one of those patterns.  There are no empty tokens, and no token contains white space.
        /// </summary>
        private static IEnumerable<string> GetTokens(String formula)
        {
            // Patterns for individual tokens
            String lpPattern = @"\(";
            String rpPattern = @"\)";
            String opPattern = @"[\+\-*/]";
            String varPattern = @"[a-zA-Z_](?: [a-zA-Z_]|\d)*";
            String doublePattern = @"(?: \d+\.\d* | \d*\.\d+ | \d+ ) (?: [eE][\+-]?\d+)?";
            String spacePattern = @"\s+";

            // Overall pattern
            String pattern = String.Format("({0}) | ({1}) | ({2}) | ({3}) | ({4}) | ({5})",
                                            lpPattern, rpPattern, opPattern, varPattern, doublePattern, spacePattern);

            // Enumerate matching tokens that don't consist solely of white space.
            foreach (String s in Regex.Split(formula, pattern, RegexOptions.IgnorePatternWhitespace))
            {
                if (!Regex.IsMatch(s, @"^\s*$", RegexOptions.Singleline))
                {
                    yield return s;
                }
            }

        }
    }

    /// <summary>
    /// Used to report syntactic errors in the argument to the Formula constructor.
    /// </summary>
    public class FormulaFormatException : Exception
    {
        /// <summary>
        /// Constructs a FormulaFormatException containing the explanatory message.
        /// </summary>
        public FormulaFormatException(String message)
            : base(message)
        {
        }
    }

    /// <summary>
    /// Used as a possible return value of the Formula.Evaluate method.
    /// </summary>
    public struct FormulaError
    {
        /// <summary>
        /// Constructs a FormulaError containing the explanatory reason.
        /// </summary>
        /// <param name="reason"></param>
        public FormulaError(String reason)
            : this()
        {
            Reason = reason;
        }

        /// <summary>
        ///  The reason why this FormulaError was created.
        /// </summary>
        public string Reason { get; private set; }
    }
}

