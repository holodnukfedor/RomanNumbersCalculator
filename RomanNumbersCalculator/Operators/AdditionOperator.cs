namespace RomanArabianNumbersConverter
{
    public class AdditionOperator : IBinaryOperator
    {
        public char OperatorChar => '+';
        public int Priority => 1;
        public int ApplyOperator(int first, int second)
        {
            return first + second;
        }
    }
}
