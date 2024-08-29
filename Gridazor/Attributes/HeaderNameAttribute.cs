using System;

namespace Gridazor.Attributes;

[AttributeUsage(AttributeTargets.Property)]
public class HeaderNameAttribute(string name) : Attribute
{
    public string Name { get; } = name;
}
