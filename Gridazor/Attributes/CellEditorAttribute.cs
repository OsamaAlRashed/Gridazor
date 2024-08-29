using System;

namespace Gridazor.Attributes;

[AttributeUsage(AttributeTargets.Property)]
public class CellEditorAttribute(string name) : Attribute
{
    public string Name { get; } = name;
}
