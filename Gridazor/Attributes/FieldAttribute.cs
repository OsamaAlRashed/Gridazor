using System;

namespace Gridazor.Attributes;

[AttributeUsage(AttributeTargets.Property)]
public class FieldAttribute(string name) : Attribute
{
    public string Name { get; } = name;
}
