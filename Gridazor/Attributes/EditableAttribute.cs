using System;

namespace Gridazor.Attributes;

[AttributeUsage(AttributeTargets.Property)]
public class EditableAttribute(bool editable) : Attribute
{
    public bool Editable { get; } = editable;
}
