namespace Pizza.Utils
{
    public static class StringExtensions
    {
        public static string CapitalizeFirstLetter(this string s)
        {
            char[] chars = s.ToCharArray();
            chars[0] = char.ToUpper(chars[0]);
            return new string(chars);
        }
    }
}