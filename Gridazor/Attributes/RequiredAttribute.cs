using System;

namespace Gridazor.Attributes;

/// <summary>
/// Attribute to specify whether the property is required.
/// </summary>
[AttributeUsage(AttributeTargets.Property)]
public class RequiredAttribute(bool required) : Attribute
{
    /// <summary>
    /// Gets a value indicating whether the property is required.
    /// </summary>
    public bool Required { get; } = required;
}
