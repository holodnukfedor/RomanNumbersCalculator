using System;

namespace RomanArabianNumbersConverter
{
    class Program
    {
        static void Main(string[] args)
        {
            var calculator = new PlumsailRomanNumberCalculator();
            Console.WriteLine(calculator.Evaluate("(MMMDCCXXIV  - MMCCXXIX) * II"));
        }
    }
}
