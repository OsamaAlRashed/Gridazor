using System;

namespace Gridazor.Attributes;

/// <summary>
/// Attribute to specify the editor of a column's cell.
/// </summary>
[AttributeUsage(AttributeTargets.Property)]
public class CellEditorAttribute(string name) : Attribute
{
    /// <summary>
    /// Gets the editor name for the cell.
    /// </summary>
    public string Name { get; } = name;
}
