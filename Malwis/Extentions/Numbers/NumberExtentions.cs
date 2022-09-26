using Malwis.General.Numbers;

namespace Malwis.Extentions.Numbers;
public static class NumberExtentions
{
    public static bool IsNumeric(this object value) => General.Numbers.NumberHelper.IsNumeric(value);
    public static bool IsIntegerNumeric(this object value) => General.Numbers.NumberHelper.IsIntegerNumeric(value);
    public static bool IsFloatingPointNumeric(this object value) => General.Numbers.NumberHelper.IsFloatingPointNumeric(value);
    public static bool IsBoolean(this object value) => General.Numbers.NumberHelper.IsBoolean(value);
}
