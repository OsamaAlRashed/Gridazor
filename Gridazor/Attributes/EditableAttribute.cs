using System;

namespace Gridazor.Attributes;

/// <summary>
/// Attribute to specify whether the property is editable in the grid.
/// </summary>
[AttributeUsage(AttributeTargets.Property)]
public class EditableAttribute : Attribute
{
    /// <summary>
    /// Gets a value indicating whether the property is editable.
    /// </summary>
    public bool Editable { get; }

    /// <summary>
    /// Constructs EditableAttribute
    /// </summary>
    /// <param name="editable">editable</param>
    public EditableAttribute(bool editable)
    {
        Editable = editable;
    }
}
