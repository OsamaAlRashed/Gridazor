using System;
using static Gridazor.Statics.Constants;

namespace Gridazor.Statics;

internal static class Helper
{
    internal static string GetDefaultCellDataType(Type type)
    {
        if (type == typeof(int) || 
            type == typeof(short) || 
            type == typeof(long) ||
            type == typeof(double) ||
            type == typeof(decimal) ||
            type == typeof(float))
        {
            return CellDataType.Number;
        }

        if (type == typeof(string))
        {
            return CellDataType.Text;
        }

        if (type == typeof(DateTime) ||
            type == typeof(DateOnly) ||
            type == typeof(DateTimeOffset))
        {
            return CellDataType.DateString;
        }

        if (type == typeof(bool))
        {
            return CellDataType.Boolean;
        }

        return string.Empty;
    }

    internal static string GetDefaultCellEditor(Type type)
    {
        if (type == typeof(int) ||
            type == typeof(short) ||
            type == typeof(long) ||
            type == typeof(double) ||
            type == typeof(decimal) ||
            type == typeof(float))
        {
            return CellEditor.AgNumberCellEditor;
        }

        if(type == typeof(bool))
        {
            return CellEditor.AgCheckboxCellEditor;
        }

        if (type == typeof(DateTime) ||
            type == typeof(DateOnly) ||
            type == typeof(DateTimeOffset))
        {
            return CellEditor.AgDateStringCellEditor;
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
