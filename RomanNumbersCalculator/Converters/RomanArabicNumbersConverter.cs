using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace RomanArabianNumbersConverter
{
    public class RomanArabicNumbersConverter : IRomanArabicNumbersConverter
    {
        private const string RomanNumberRegularExpressionPattern = "^(M{0,3})(D?C{0,3}|C[DM])(L?X{0,3}|X[LC])(V?I{0,3}|I[VX])$";
        private const int ArabicNumberConverterLimit = 3999;
        private readonly static Regex _romanNumberRegularExpression = new Regex(RomanNumberRegularExpressionPattern);
        private readonly static Dictionary<char, int> _romanCharsValuesDict = new Dictionary<char, int>()
        {
            { 'I', 1 },
            { 'V', 5 },
            { 'X', 10 },
            { 'L', 50 },
            { 'C', 100 },
            { 'D', 500 },
            { 'M', 1000 }
        };

        private readonly static (int Number, string Character)[] _romanNumbersValuesTuplesForToArabicConvert = new (int, string)[]
        {
            (1000,"M"), (900,"CM"), (500,"D"), (400,"CD"), (100,"C"), (90,"XC"), (50,"L"), (40,"XL"),  (10,"X"), (9,"IX"), (5,"V"), (4,"IV"), (1,"I")
        };

        public bool IsRomanNumberCharacter(char character)
        {
            return _romanCharsValuesDict.ContainsKey(character);
        }

        public bool TryConvertToArabicNumber(string romanNumber, out int arabicNumber)
        {
            arabicNumber = 0;
            var match = _romanNumberRegularExpression.Match(romanNumber);

            if (!match.Success)
                return false;

            for (var i = 1; i < match.Groups.Count; ++i)
            {
                arabicNumber += RomanPartToArabic(match.Groups[i].Value);
            }

            return true;
        }

        public string ConvertToRomanNumber(int arabicNumber)
        {
            if (arabicNumber <= 0)
                throw new ArgumentException("There is no roman representation of zero or negative numbers");

            if (arabicNumber > ArabicNumberConverterLimit)
                throw new ArgumentException($"Current converter can't convert to roman arabic number greater than {ArabicNumberConverterLimit}. Input number is {arabicNumber}");

            var i = 0;
            var result = new StringBuilder();

            while (arabicNumber > 0)
            {
                while (_romanNumbersValuesTuplesForToArabicConvert[i].Number > arabicNumber)
                    ++i;

                result.Append(_romanNumbersValuesTuplesForToArabicConvert[i].Character);
                arabicNumber -= _romanNumbersValuesTuplesForToArabicConvert[i].Number;
            }

            return result.ToString();
        }

        private static int RomanPartToArabic(string romanPart)
        {
            if (romanPart.Length == 0)
                return 0;

            if (romanPart.Length == 1)
                return _romanCharsValuesDict[romanPart[0]];

            if (_romanCharsValuesDict[romanPart[0]] < _romanCharsValuesDict[romanPart[1]])
            {
                return _romanCharsValuesDict[romanPart[1]] - _romanCharsValuesDict[romanPart[0]];
            }
            else
            {
                var sum = _romanCharsValuesDict[romanPart[0]] + _romanCharsValuesDict[romanPart[1]];
                
                if (romanPart.Length > 2)
                {
                    for (var i = 2; i < romanPart.Length; ++i)
                        sum += _romanCharsValuesDict[romanPart[i]];
                }

                return sum;
            }
        }
    }
}
