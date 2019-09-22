using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Calculator.Model
{
    public enum Operation
    {
        Addition,
        Subtraction,
        Multiplication,
        Division,
        Percent
    }

    public enum Associativity
    {
        Left,
        Right
    }

    public class Model
    {
        public struct Token
        {
            public double? Operand { get; set; }

            public Operation? Operation { get; set; }
        }

        private List<Token> _tokens = new List<Token>();

        public List<Token> GetTokens()
        {
            return _tokens;
        }

        public Model() { }

        public void Clear()
        {
            _tokens.Clear();
        }

        public bool CanPushOperand()
        {
            return _tokens.IsEmpty() || _tokens.Last().Operation.HasValue;
        }

        public bool CanPushOperation()
        {
            return !CanPushOperand();
        }

        public void Push(Operation operation)
        {
            if (!CanPushOperation())
            {
                throw new SyntaxErrorException();
            }
            _tokens.Add(new Token { Operation = operation });
        }

        public void Push(double number)
        {
            if (!CanPushOperand())
            {
                throw new SyntaxErrorException();
            }
            _tokens.Add(new Token { Operand = number });
        }

        public double GetResult()
        {
            if (_tokens.IsEmpty())
            {
                return 0.0;
            }

            return EvauluateRpn(ShuntingYard(_tokens));
        }

        private static List<Token> ShuntingYard(List<Token> tokens)
        {
            List<Token> outputStack = new List<Token>();
            Stack<Operation> operatorStack = new Stack<Operation>();

            foreach (var token in tokens)
            {
                if (token.Operand.HasValue)
                {
                    outputStack.Add(token);
                    continue;
                }

                while (!operatorStack.IsEmpty()
                    && (operatorStack.Last().Precedence() > token.Operation.Value.Precedence()
                      || (operatorStack.Last().GetAssociativity() == Associativity.Left
                        && operatorStack.Last().Precedence() == token.Operation.Value.Precedence())))
                {
                    outputStack.Add(new Token { Operation = operatorStack.Pop() });
                }

                operatorStack.Push(token.Operation.Value);
            }

            while (!operatorStack.IsEmpty())
            {
                outputStack.Add(new Token { Operation = operatorStack.Pop() });
            }

            return outputStack;
        }
        //RPN = Reverse Polish notation
        private static double EvauluateRpn(List<Token> outputStack)
        {
            Stack<double> rpnStack = new Stack<double>();
            foreach (var token in outputStack)
            {
                if (token.Operand.HasValue)
                {
                    rpnStack.Push(token.Operand.Value);
                    continue;
                }

                if (rpnStack.Count < 2)
                {
                    throw new EvaluateException();
                }

                var rhs = rpnStack.Pop();
                var lhs = rpnStack.Pop();
                rpnStack.Push(token.Operation.Value.Calculate(lhs, rhs));
            }

            if (rpnStack.Count > 1)
            {
                throw new EvaluateException();
            }

            return rpnStack.Pop();
        }
    }

    public static class RandomExtensionMethods
    {
        public static bool IsEmpty(this ICollection container)
        {
            return container.Count == 0;
        }

        public static Associativity GetAssociativity(this Operation op)
        {
            switch (op)
            {
                case Operation.Addition:
                    return Associativity.Left;
                case Operation.Subtraction:
                    return Associativity.Left;
                case Operation.Multiplication:
                    return Associativity.Left;
                case Operation.Division:
                    return Associativity.Left;
                case Operation.Percent:
                    return Associativity.Left;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        public static int Precedence(this Operation op)
        {
            switch (op)
            {
                case Operation.Addition:
                    return 1;
                case Operation.Subtraction:
                    return 1;
                case Operation.Multiplication:
                    return 2;
                case Operation.Division:
                    return 2;
                case Operation.Percent:
                    return 2;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        public static double Calculate(this Operation op, double lhs, double rhs)
        {
            switch (op)
            {
                case Operation.Addition:
                    return lhs + rhs;
                case Operation.Subtraction:
                    return lhs - rhs;
                case Operation.Multiplication:
                    return lhs * rhs;
                case Operation.Division:
                    return lhs / rhs;
                case Operation.Percent:
                    return (rhs / lhs) * 100;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
    }
}
