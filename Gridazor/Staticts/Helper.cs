using System;

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

    internal static bool IsNullableType(Type type) 
        => type.IsGenericType && type.GetGenericTypeDefinition() == typeof(Nullable<>);

}
