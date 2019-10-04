//Author: Ben Huenemann

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

/// <summary>
/// Namespace for classes relating to the spreadsheet that contains an evaluator class for some inputted string expression and a stack
/// extensions class for more convenient stack operations.
/// </summary>
namespace SpreadsheetUtilities
{
    /// <summary>
    /// Class for added stack operations
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
        public static Boolean IsOnTop<E>(this Stack<E> s, E n)
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
        //Private string array for containing the tokens that are inputted into a formula.
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
            //Converts all of the tokens into an array
            tokens = GetTokens(formula).ToArray();

            //Error for empty formulas
            if (tokens.Length == 0)
                throw new FormulaFormatException("No tokens found");

            int leftParenCount = 0;
            int rightParenCount = 0;

            for (int i = 0; i < tokens.Length; i++)
            {
                //It only checks for precedence errors if it isn't the last element of the array in order to avoid going out of bounds
                if (i != tokens.Length - 1)
                {
                    /*This is the case for voilating the parenthesis/operator following rule (any token that immediately follows an opening
                     * parenthesis or an operator must be either a number, a variable, or an opening parenthesis). */
                    if ((tokens[i] == "(" || IsOperator(tokens[i])) &&
                       !(tokens[i + 1] == "(" || IsVariable(normalize(tokens[i + 1])) || IsDoubleOrScientific(tokens[i + 1])))
                        throw new FormulaFormatException("Operator/closing_parenthesis follows an operator/opening_parenthesis");
                    /*This is the case for voilating the extra following rule (any token that immediately follows a number, a variable, or
                     * a closing parenthesis must be either an operator or a closing parenthesis.). */
                    if ((tokens[i] == ")" || IsVariable(normalize(tokens[i])) || IsDoubleOrScientific(tokens[i])) &&
                        !(tokens[i + 1] == ")" || IsOperator(tokens[i + 1])))
                        throw new FormulaFormatException("Number/variable/closing_parenthesis follows a number/variable");
                }

                //Increments the parentheses count variables
                if (tokens[i] == "(")
                    leftParenCount++;
                else if (tokens[i] == ")")
                    rightParenCount++;
                /*If the variable is in the right form, it replaces it with the normalized version and then tests to see if it
                 * fits the specifications of the IsValid delegate*/
                else if (IsVariable(tokens[i]))
                {
                    tokens[i] = normalize(tokens[i]);

                    if (!isValid(tokens[i]))
                        throw new FormulaFormatException("Variable name doesn't fit validity specifications");
                }
                //If it's a double, it parses the double and replaces the token with the string version of that parsed double
                else if (IsDoubleOrScientific(tokens[i]))
                    tokens[i] = Double.Parse(tokens[i]).ToString();
                //If it isn't an operator, it must be an invalid token
                else if (!IsOperator(tokens[i]))
                    throw new FormulaFormatException("Invalid token");

                //Throws an exception if the right parentheses exceeds the left parentheses.
                if (rightParenCount > leftParenCount)
                    throw new FormulaFormatException("Unmatched right parenthesis");
            }

            //Stores start and end for simplicity
            string startToken = tokens[0];
            string endToken = tokens[tokens.Length - 1];

            //Checks to see if it starts and ends with valid tokens
            if (startToken != "(" && !IsVariable(startToken) && !IsDoubleOrScientific(startToken))
                throw new FormulaFormatException("Starting token isn't a number/variable/opening_parenthesis");
            else if (endToken != ")" && !IsVariable(endToken) && !IsDoubleOrScientific(endToken))
                throw new FormulaFormatException("Ending token isn't a number/variable/closing_parenthesis");

            //Throws an exception if it doesn't follow the balanced parentheses rule
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

            //Goes through each token and does different actions depending on the type
            for (int i = 0; i < tokens.Length; i++)
            {
                //If it's a double or a variable
                if (IsDoubleOrScientific(tokens[i]) || IsVariable(tokens[i]))
                {
                    double num1;

                    /*If it fits the format of a double, convert the string into an double. Otherwise convert the variable into an double using
                     * the inputted lookup delegate. */
                    if (IsDoubleOrScientific(tokens[i]))
                        Double.TryParse(tokens[i], out num1);
                    else
                    {
                        try
                        {
                            num1 = lookup(tokens[i]);
                        }
                        catch (Exception)
                        {
                            return new FormulaError("Unknown Variable");
                        }
                    }

                    /* If the stack doesn't have * or / on the top it just pushes the number to the value stack. Otherwise it uses
                     * the number and the top values of each stack to form an expression, evaluate it, and push it to the value stack. */
                    if (!operatorStack.IsOnTop("*") && !operatorStack.IsOnTop("/"))
                        valueStack.Push(num1);
                    else
                    {
                        double num2 = valueStack.Pop();
                        string op = operatorStack.Pop();

                        //If this expression fails, it means that it tried to divide by zero
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
                    if (!operatorStack.IsOnTop("+") && !operatorStack.IsOnTop("-"))
                        operatorStack.Push(tokens[i]);
                    else
                    {
                        TryCalcFromStacks(valueStack, operatorStack, ref returnVal);

                        valueStack.Push(returnVal);
                        operatorStack.Push(tokens[i]);
                    }
                }

                //This is the case that the token is a )
                else if (tokens[i] == ")")
                {
                    //If the top value is a + or - it uses the top values from both stacks to form an expression and push it
                    if (operatorStack.IsOnTop("+") || operatorStack.IsOnTop("-"))
                    {
                        TryCalcFromStacks(valueStack, operatorStack, ref returnVal);

                        valueStack.Push(returnVal);
                    }

                    operatorStack.Pop();

                    /* If the operator stack has * or / on the top, it uses the values of each stack to form an
                     * expression, evaluate it, and push it to the value stack. */
                    if (operatorStack.IsOnTop("*") || operatorStack.IsOnTop("/"))
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
                TryCalcFromStacks(valueStack, operatorStack, ref returnVal);
                return returnVal;
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
            //Creates a hash set to return since then duplicates will be combined
            HashSet<string> variables = new HashSet<string>();

            foreach (string token in tokens)
            {
                //Adds the tokens that are variables
                if (IsVariable(token))
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
            //Joins the token array and returns that
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
            //If the other object is null or it isn't a formula it returns false.
            if (obj is null || obj.GetType() != typeof(Formula))
                return false;
            else
            {
                //Otherwise it compares the string versions
                string otherTokens = obj.ToString();
                return otherTokens == this.ToString();
            }
        }

        /// <summary>
        /// Reports whether f1 == f2, using the notion of equality from the Equals method.
        /// Note that if both f1 and f2 are null, this method should return true.  If one is
        /// null and one is not, this method should return false.
        /// </summary>
        public static bool operator ==(Formula f1, Formula f2)
        {
            //If they're both null it returns true but if one is null it returns false
            if (f1 is null && f2 is null)
                return true;
            else if (f1 is null || f2 is null)
                return false;

            //Otherwise it just depends on the equals method
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
            return this.ToString().GetHashCode(); ;
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
            else if (op == "/")
            {
                //Dividing by zero returns false
                if (num2 == 0)
                    return false;
                output = num1 / num2;
            }
            //Incorrect operator returns false
            else return false;

            return true;
        }

        /// <summary>
        /// Tests if a string is an operator (+-*/)
        /// </summary>
        /// <param name="s">String to be tested</param>
        /// <returns>Whether or not the string is an operator</returns>
        bool IsOperator(string s)
        {
            return (s == "+" || s == "-" || s == "*" || s == "/");
        }

        /// <summary>
        /// Tests if a string fits the format of a variable. This format is that it is a letter or underscore followed by a combination of 
        /// letters, underscores, or digits.
        /// </summary>
        /// <param name="s">String to be tested</param>
        /// <returns>Whether or not the string is a variable</returns>
        bool IsVariable(string s)
        {
            return Regex.IsMatch(s, @"^[a-zA-Z_](?:[a-zA-Z_]|\d)*$");
        }

        /// <summary>
        /// Tests if a string fits the format of a double or a number in scientific notation
        /// </summary>
        /// <param name="s">String to be tested</param>
        /// <returns>Whether or not it is a double/scientific notation number</returns>
        bool IsDoubleOrScientific(string s)
        {
            if (Regex.IsMatch(s, @"^(?:\d+\.\d*|\d*\.\d+|\d+)(?:[eE][\+-]?\d+)?$"))
                return true;
            return false;
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

