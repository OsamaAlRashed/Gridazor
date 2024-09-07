using System;

namespace Gridazor.Attributes;

/// <summary>
/// Attribute to specify whether the property is required.
/// </summary>
[AttributeUsage(AttributeTargets.Property)]
public class RequiredAttribute : Attribute
{
    /// <summary>
    /// Gets a value indicating whether the property is required.
    /// </summary>
    public bool Required { get; }

    /// <summary>
    /// Constructs RequiredAttribute
    /// </summary>
    /// <param name="required">Required</param>
    public RequiredAttribute(bool required)
    {
        Required = required;
    }
}
