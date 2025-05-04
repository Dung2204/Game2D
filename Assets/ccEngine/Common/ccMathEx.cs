/// <summary>
/// 数学扩展  扩展Excel数学的函数
/// </summary>
public static class MathEx
{
    /// <summary>
    /// 类似Excel的RoundUp
    /// </summary>
    /// <param name="value">原始数据</param>
    /// <param name="digits">取整小数位置，负数向前,负数向后</param>
    /// <returns></returns>
    public static decimal RoundUp(this decimal value, sbyte digits)
    {
        if (digits == 0)
        {
            return (value >= 0 ? decimal.Ceiling(value) : decimal.Floor(value));
        }
        decimal multiple = System.Convert.ToDecimal(System.Math.Pow(10, digits));
        return (value >= 0 ? decimal.Ceiling(value * multiple) : decimal.Floor(value * multiple)) / multiple;
    }

    /// <summary>
    /// 类似Excel的RoundDown
    /// </summary>
    /// <param name="value">原始数据</param>
    /// <param name="digits">取整小数位置，负数向前,负数向后</param>
    /// <returns></returns>
    public static decimal RoundDown(this decimal value, sbyte digits)
    {
        if (digits == 0)
        {
            return (value >= 0 ? decimal.Floor(value) : decimal.Ceiling(value));
        }
        decimal multiple = System.Convert.ToDecimal(System.Math.Pow(10, digits));
        return (value >= 0 ? decimal.Floor(value * multiple) : decimal.Ceiling(value * multiple)) / multiple;
    }
}
