using Gridazor.Settings;
using System;
using System.Collections.Generic;

namespace Gridazor.Abstractions;


/// <summary>
/// Provides the metadata of columns from the property type.
/// </summary>
public interface IColumnsProvider
{
    /// <summary>
    /// Gets a list of metadata for the columns based on the provided type.
    /// </summary>
    /// <param name="type">The enumerable type.</param>
    /// <returns>A list of columns containing metadata.</returns>
    public IEnumerable<Column> Get(Type type);
}
