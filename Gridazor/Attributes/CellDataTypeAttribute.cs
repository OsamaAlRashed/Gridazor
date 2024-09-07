using System;

namespace Gridazor.Attributes;

/// <summary>
/// Attribute to specify the data type of a column's cell.
/// </summary>
[AttributeUsage(AttributeTargets.Property)]
public class CellDataTypeAttribute : Attribute
{
    /// <summary>
    /// Constructs CellDataTypeAttribute
    /// </summary>
    /// <param name="type">the data type fro the cell</param>
    public CellDataTypeAttribute(string type)
    {
        Type = type;
    }

    /// <summary>
    /// Gets the data type for the cell.
    /// </summary>
    public string Type { get; }
}
