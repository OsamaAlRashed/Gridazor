using System;

namespace Gridazor.Attributes;

/// <summary>
/// Attribute to specify a custom header name for the grid column.
/// </summary>
[AttributeUsage(AttributeTargets.Property)]
public class HeaderNameAttribute : Attribute
{
    /// <summary>
    /// Gets the custom header name for the column.
    /// </summary>
    public string Name { get; }

    /// <summary>
    /// Constructs HeaderNameAttribute
    /// </summary>
    /// <param name="name">header name for the column.</param>
    public HeaderNameAttribute(string name)
    {
        Name = name;
    }
}
