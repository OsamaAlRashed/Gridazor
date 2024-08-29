using System;

namespace Gridazor.Attributes;

[AttributeUsage(AttributeTargets.Property)]
public class CellDataTypeAttribute(string type) : Attribute
{
    public string Type { get; } = type;
}
