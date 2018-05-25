namespace HexMultiplicationFlashCardsMvc.Extensions
{
    public static class IntExtensions
    {
        public static string ToStringHex(this int value)
        {
            //use lowercase "x" if lowercase hex strings are desired
            return value.ToString("X");
        }
        public static string ToStringHex(this int? value)
        {
            //use lowercase "x" if lowercase hex strings are desired
            return value?.ToString("X");
        }
    }
}