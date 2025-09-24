using System.Text.RegularExpressions;

namespace SafeVault
{
    public static class InputValidator
    {
        public static bool IsValidUsername(string username)
        {
            return Regex.IsMatch(username, @"^[A-Za-z0-9_]{3,20}$");
        }

        public static string SanitizeInput(string input)
        {
            return System.Net.WebUtility.HtmlEncode(input);
        }
    }
}