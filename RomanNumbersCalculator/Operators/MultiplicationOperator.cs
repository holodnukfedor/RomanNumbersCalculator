namespace RomanArabianNumbersConverter
{
    public class MultiplicationOperator : IBinaryOperator
    {
        public char OperatorChar => '*';
        public int Priority => 2;
        public int ApplyOperator(int first, int second)
        {
            return first * second;
        }
    }
}
