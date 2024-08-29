using Gridazor.Settings;
using System;
using System.Collections.Generic;

namespace Gridazor.Abstractions;

public interface IColumnsProvider
{
    public IEnumerable<Column> Get(Type type);
}
