using System;
using static Gridazor.Statics.Constants;

namespace Gridazor.Statics;

internal static class Helper
{
    internal static string GetDefaultCellDataType(Type type)
    {
        if (type == typeof(int))
        {
            return CellDataType.Number;
        }

        if (type == typeof(string))
        {
            return CellDataType.Text;
        }

        if (type == typeof(DateTime))
        {
            return CellDataType.DateString;
        }

        return string.Empty;
    }

    internal static string GetDefaultCellEditor(Type type)
    {
        if (type == typeof(int))
        {
            return CellEditor.AgNumberCellEditor;
        }

        return string.Empty;
    }

    internal static bool IsNullableType(Type type) 
        => type.IsGenericType && type.GetGenericTypeDefinition() == typeof(Nullable<>);

    internal static string FirstToLower(this string text)
    {
        if (string.IsNullOrEmpty(text))
            return text;

        return char.ToLower(text[0]) + text[1..];
    }

}
