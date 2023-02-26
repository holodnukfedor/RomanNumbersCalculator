namespace RomanArabianNumbersConverter
{
    public interface IBinaryOperator
    {
        char OperatorChar { get; }
        int Priority { get; }
        int ApplyOperator(int first, int second);
    }
}
