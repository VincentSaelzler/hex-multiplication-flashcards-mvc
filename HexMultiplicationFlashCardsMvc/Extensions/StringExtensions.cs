using System.Globalization;

namespace HexMultiplicationFlashCardsMvc.Extensions
{
    public static class StringExtensions
    {
        public static int ParseHex(this string value)
        {
            return int.Parse(value, NumberStyles.HexNumber);
        }
    }
}