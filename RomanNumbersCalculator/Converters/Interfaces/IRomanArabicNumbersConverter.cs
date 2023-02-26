namespace RomanArabianNumbersConverter
{
    public interface IRomanArabicNumbersConverter
    {
        bool IsRomanNumberCharacter(char character);
        bool TryConvertToArabicNumber(string romanNumber, out int arabicNumber);
        string ConvertToRomanNumber(int arabicNumber);
    }
}
