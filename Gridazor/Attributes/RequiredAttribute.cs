using System;

namespace Gridazor.Attributes;

[AttributeUsage(AttributeTargets.Property)]
public class RequiredAttribute(bool required) : Attribute
{
    public bool Required { get; } = required;
}
