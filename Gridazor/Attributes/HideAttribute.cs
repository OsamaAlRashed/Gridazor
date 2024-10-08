﻿using System;

namespace Gridazor.Attributes;

/// <summary>
/// Attribute to specify that the column should be hidden.
/// </summary>
[AttributeUsage(AttributeTargets.Property)]
public class HideAttribute : Attribute { }
