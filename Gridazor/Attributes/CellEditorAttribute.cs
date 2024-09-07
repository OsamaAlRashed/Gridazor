using System;

namespace Gridazor.Attributes;

/// <summary>
/// Attribute to specify the editor of a column's cell.
/// </summary>
[AttributeUsage(AttributeTargets.Property)]
public class CellEditorAttribute : Attribute
{
    /// <summary>
    /// Gets the editor name for the cell.
    /// </summary>
    public string Name { get; }

    /// <summary>
    /// Constructs CellEditorAttribute
    /// </summary>
    /// <param name="name">The editor name for the cell.</param>
    public CellEditorAttribute(string name)
    {
        Name = name;
    }
}
