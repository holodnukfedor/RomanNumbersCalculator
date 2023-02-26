using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RomanArabianNumbersConverter
{
    public class RomanNumberCalculator : ICalculator
    {
        private const string IncorrectBracketsSequence = "{0} expression has incorrect number of brackets";
        private readonly Dictionary<char, IBinaryOperator> _operatorsDictionary;
        private readonly IRomanArabicNumbersConverter _romanArabicNumbersConverter;

        public RomanNumberCalculator(IBinaryOperator[] operators, IRomanArabicNumbersConverter romanArabicNumbersConverter)
        {
            if (operators == null || operators.Length == 0)
                throw new ArgumentException($"{nameof(operators)} array shouldn't be empty!");

            if (romanArabicNumbersConverter == null)
                throw new ArgumentException($"{nameof(_romanArabicNumbersConverter)} shouldn't be null!");

            _operatorsDictionary = operators.ToDictionary(x => x.OperatorChar);
            _romanArabicNumbersConverter = romanArabicNumbersConverter;
        }

        public string Evaluate(string expression)
        {
            var operatorCharStack = new Stack<char>();
            var operandsStack = new Stack<int>();

            for (var i = 0; i < expression.Length; ++i)
            {
                var currentCharacter = expression[i];

                if (char.IsWhiteSpace(currentCharacter))
                {
                    continue;
                }

                if (IsLeftBracket(currentCharacter))
                {
                    operatorCharStack.Push(currentCharacter);
                }
                else if (IsRightBracket(currentCharacter))
                {
                    ProcessRightBracket(expression, operatorCharStack, operandsStack);
                }
                else if (IsOperator(currentCharacter))
                {
                    ProcessOperator(expression, operatorCharStack, operandsStack, currentCharacter);
                }
                else if (_romanArabicNumbersConverter.IsRomanNumberCharacter(currentCharacter))
                {
                    ReadNumber(expression, operandsStack, ref i, currentCharacter);
                }
                else
                {
                    throw new ArgumentException($"Unknown character {currentCharacter} in {nameof(expression)}");
                }
            }

            while(operatorCharStack.Count > 0)
            {
                var currentOperator = operatorCharStack.Pop();

                if (!IsOperator(currentOperator))
                {
                    throw new ArgumentException(string.Format(IncorrectBracketsSequence, nameof(expression)));
                }

                ApplyOperator(expression, currentOperator, operandsStack);
            }

            if (operandsStack.Count > 1)
                throw new ArgumentException($"{nameof(expression)} has incorrect number of spaces or incorrect sequence of operators");

            return _romanArabicNumbersConverter.ConvertToRomanNumber(operandsStack.Pop());
        }

        private void ProcessRightBracket(string expression, Stack<char> operatorCharStack, Stack<int> operandsStack)
        {
            while (operatorCharStack.Count > 0 && !IsLeftBracket(operatorCharStack.Peek()))
            {
                var currentOperatorCharacter = operatorCharStack.Pop();
                ApplyOperator(expression, currentOperatorCharacter, operandsStack);
            }

            if (operatorCharStack.Count == 0)
            {
                throw new ArgumentException(GetIncorrectNumberOfBracketsMessage(expression));
            }
            else
            {
                operatorCharStack.Pop();
            }
        }

        private static string GetIncorrectNumberOfBracketsMessage(string expression)
        {
            return string.Format(IncorrectBracketsSequence, nameof(expression));
        }

        private void ProcessOperator(string expression, Stack<char> operatorCharStack, Stack<int> operandsStack, char currentCharacter)
        {
            while (operatorCharStack.Count > 0 
                && IsOperator(operatorCharStack.Peek()) 
                && _operatorsDictionary[operatorCharStack.Peek()].Priority >= _operatorsDictionary[currentCharacter].Priority)
            {
                var previousOperatorCharacter = operatorCharStack.Pop();
                ApplyOperator(expression, previousOperatorCharacter, operandsStack);
            }

            operatorCharStack.Push(currentCharacter);
        }

        private void ApplyOperator(string expression, char currentOperatorCharacter, Stack<int> operandsStack)
        {
            if (operandsStack.Count < 2)
            {
                throw new ArgumentException($"{nameof(expression)} has incorrect sequence of operands!");
            }

            var second = operandsStack.Pop();
            var first = operandsStack.Pop();
            var operationResult = _operatorsDictionary[currentOperatorCharacter].ApplyOperator(first, second);
            operandsStack.Push(operationResult);
        }

        private void ReadNumber(string expression, Stack<int> opearandStack, ref int i, char character)
        {
            var digitCharacterBuilder = new StringBuilder(character.ToString());
            ++i;

            while (i < expression.Length && _romanArabicNumbersConverter.IsRomanNumberCharacter(expression[i]))
            {
                digitCharacterBuilder.Append(expression[i]);
                ++i;
            }

            --i;
                
            var digitStr = digitCharacterBuilder.ToString();

            if (_romanArabicNumbersConverter.TryConvertToArabicNumber(digitStr, out var arabicNumber))
            {
                opearandStack.Push(arabicNumber);
            }
            else
            {
                throw new ArgumentException($"{digitStr} token is not a roman number, invalid token");
            }
        }
        private bool IsOperator(char character) => _operatorsDictionary.ContainsKey(character);
        static private bool IsLeftBracket(char character) => character == '(';
        static private bool IsRightBracket(char character) => character == ')';
    }
}
