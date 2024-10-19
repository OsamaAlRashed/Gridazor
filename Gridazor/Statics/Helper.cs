using System;
using System.Linq;
using static Gridazor.Statics.Constants;

namespace Gridazor.Statics;

internal static class Helper
{
    private static readonly Type[] NumberTypes =
    {
        typeof(int), typeof(short), typeof(long),
        typeof(double), typeof(decimal), typeof(float),
        typeof(int?), typeof(short?), typeof(long?),
        typeof(double?), typeof(decimal?), typeof(float?)
    };

    private static readonly Type[] DateTypes =
    {
        typeof(DateTime), typeof(DateOnly), typeof(DateTimeOffset),
        typeof(DateTime?), typeof(DateOnly?), typeof(DateTimeOffset?)
    };

    private static readonly Type[] BooleanTypes =
    {
        typeof(bool), typeof(bool?)
    };

    internal static string GetDefaultCellDataType(Type type)
    {
        if (NumberTypes.Contains(type))
            return CellDataType.Number;

        if (type == typeof(string))
            return CellDataType.Text;

        if (DateTypes.Contains(type))
            return CellDataType.DateString;

        if (BooleanTypes.Contains(type))
            return CellDataType.Boolean;

        return string.Empty;
    }

    internal static string GetDefaultCellEditor(Type type)
    {
        if (NumberTypes.Contains(type))
            return CellEditor.AgNumberCellEditor;

        if (BooleanTypes.Contains(type))
            return CellEditor.AgCheckboxCellEditor;

        if (DateTypes.Contains(type))
            return CellEditor.AgDateStringCellEditor;

        return string.Empty;
    }

    internal static bool IsNullableType(Type type)
        => Nullable.GetUnderlyingType(type) != null;

    internal static string FirstToLower(this string text)
    {
        if (string.IsNullOrEmpty(text))
            return text;

        return char.ToLower(text[0]) + text[1..];
    }
}

