using System;

namespace Gridazor.Attributes;

/// <summary>
/// Attribute to specify the data type of a column's cell.
/// </summary>
[AttributeUsage(AttributeTargets.Property)]
public class CellDataTypeAttribute(string type) : Attribute
{
    /// <summary>
    /// Gets the data type for the cell.
    /// </summary>
    public string Type { get; } = type;
}
