namespace Malwis.General.Numbers;

public static class NumberHelper
{
    public static bool IsNumeric(object value) => IsIntegerNumeric(value) || IsFloatingPointNumeric(value);

    public static bool IsIntegerNumeric(object value) => value is sbyte
            || value is byte
            || value is short
            || value is ushort
            || value is int
            || value is uint
            || value is long
            || value is ulong;

    public static bool IsFloatingPointNumeric(object value) => value is float
            || value is double
            || value is decimal;

    public static bool IsBoolean(object value) => value is bool;
}
