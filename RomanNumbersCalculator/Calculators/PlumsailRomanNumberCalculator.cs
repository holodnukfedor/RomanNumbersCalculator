namespace RomanArabianNumbersConverter
{
    public class PlumsailRomanNumberCalculator : ICalculator
    {
        ICalculator _calculator;

        public PlumsailRomanNumberCalculator()
        {
            _calculator = new RomanNumberCalculator(
              new IBinaryOperator[]
              {
                    new AdditionOperator(),
                    new SubstractionOperator(),
                    new MultiplicationOperator()
              },
              new RomanArabicNumbersConverter());
        }

        public string Evaluate(string expression)
        {
            return _calculator.Evaluate(expression);
        }
    }
}
