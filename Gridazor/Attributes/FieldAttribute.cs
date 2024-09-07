using System;

namespace Gridazor.Attributes;

/// <summary>
/// Attribute to specify the field name for the grid column.
/// </summary>
[AttributeUsage(AttributeTargets.Property)]
public class FieldAttribute : Attribute
{
    /// <summary>
    /// Gets the field name for the column.
    /// </summary>
    public string Name { get; }

    /// <summary>
    /// Constructs FieldAttribute
    /// </summary>
    /// <param name="name">the field name for the column.</param>
    public FieldAttribute(string name)
    {
        Name = name;
    }
}
