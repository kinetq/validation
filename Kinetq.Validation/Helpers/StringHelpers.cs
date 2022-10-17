using System.Text;
using System.Text.RegularExpressions;

namespace Kinetq.Validation.Helpers
{
    public static class StringHelpers
    {
        public static string Clean(this string s, params string[] replacements)
        {
            var stringBuilder = new StringBuilder(s);

            foreach (var replacement in replacements)
            {
                stringBuilder.Replace(replacement, string.Empty);
            }

            return stringBuilder.ToString();
        }

        public static string MakeUrlFriendly(this string value)
        {
            return Regex.Replace(value, @"[^A-Za-z0-9_\.~]+", "-").ToLower();
        }
        
        public static string FirstCharToLowerCase(this string str)
        {
            if (string.IsNullOrEmpty(str) || char.IsLower(str[0]))
                return str;

            return char.ToLower(str[0]) + str.Substring(1);
        }
    }
}