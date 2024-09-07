using System;

namespace Gridazor.Attributes;

/// <summary>
/// Attribute to specify whether the property is editable in the grid.
/// </summary>
[AttributeUsage(AttributeTargets.Property)]
public class EditableAttribute(bool editable) : Attribute
{
    /// <summary>
    /// Gets a value indicating whether the property is editable.
    /// </summary>
    public bool Editable { get; } = editable;
}
