using System;
using System.Reflection;

namespace Gridazor.Staticts;

internal static class Helper
{
    internal static string GetDefaultCellDataType(Type t)
    {
        if (t == typeof(int))
        {
            return "number";
        }

        if (t == typeof(DateTime))
        {
            return "dateString";
        }

        return "text";
    }

    internal static string GetDefaultCellEditor(Type t)
    {
        if(t == typeof(int))
        {
            return "agNumberCellEditor";
        }

        return string.Empty;
    }

    internal static bool IsNullable(PropertyInfo property)
    {
        return 
    }

    internal static string FirstToLower(this string text)
    {
        if (string.IsNullOrEmpty(text))
            return text;

        return char.ToLower(text[0]) + text.Substring(1);
    }

}
