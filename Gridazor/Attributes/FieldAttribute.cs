using System;

namespace Gridazor.Attributes;

/// <summary>
/// Attribute to specify the field name for the grid column.
/// </summary>
[AttributeUsage(AttributeTargets.Property)]
public class FieldAttribute(string name) : Attribute
{
    /// <summary>
    /// Gets the field name for the column.
    /// </summary>
    public string Name { get; } = name;
}
