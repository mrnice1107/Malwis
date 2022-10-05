using System.Text.RegularExpressions;

namespace Malwis.General.Texts.Validators;
public static class EmailValidator
{
    private static readonly string emailRegex = @"^[\w-\.]+@([\w-]+\.)+[\w-]{2,4}$";

    public static bool IsEmail(string str) => !string.IsNullOrEmpty(str) && Regex.IsMatch(str, emailRegex);

}
