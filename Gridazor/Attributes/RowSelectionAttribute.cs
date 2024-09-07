using System;

namespace Gridazor.Attributes;

/// <summary>
/// Attribute to specify that the column is selectable.
/// </summary>
[AttributeUsage(AttributeTargets.Property)]
public class RowSelectionAttribute : Attribute { }
