using Gridazor.Staticts;
using System;
using Gridazor.Abstractions;

namespace Gridazor.Settings;

/// <summary>
/// Represents the metadeta of the column that got from <see cref="IColumnsProvider"/>
/// </summary>
public class Column
{
    /// <summary>
    /// Gets the header name of the column
    /// the default value is property name
    /// see <see cref="https://www.ag-grid.com/javascript-data-grid/column-headers/"/>
    /// </summary>
    public string HeaderName { get; private set; }

    /// <summary>
    /// Gets the field name of the column
    /// field is such the id of the column and it should be unique
    /// the default value is property name (first letter is in lower case)
    /// </summary>
    public string Field { get; private set; }

    /// <summary>
    /// Gets whether if the column is editable
    /// the default value is true
    /// </summary>
    public bool Editable { get; private set; }

    /// <summary>
    /// Gets the data type of the column
    /// </summary>
    public string CellDataType { get; private set; }

    /// <summary>
    /// Gets the editor of the column
    /// </summary>
    public string CellEditor { get; private set; }

    public bool Required { get; private set; }
    public bool Hide { get; private set; }
    public bool Selectable { get; private set; }

    internal Column(Type type, string name)
    {
        CellDataType = Helper.GetDefaultCellDataType(type);
        CellEditor = Helper.GetDefaultCellEditor(type);
        Editable = true;
        Field = name.FirstToLower();
        HeaderName = name;
        Required = type.IsValueType && !Helper.IsNullableType(type);
        Hide = false;
        Selectable = false;
    }

    internal Column SetHeaderName(string headerName)
    {
        HeaderName = headerName;

        return this;
    }

    internal Column SetField(string field)
    {
        Field = field;

        return this;
    }

    internal Column SetEditable(bool editable)
    {
        Editable = editable;

        return this;
    }

    internal Column SetCellDataType(string cellDataType)
    {
        CellDataType = cellDataType;

        return this;
    }

    internal Column SetCellEditor(string cellEditor)
    {
        CellEditor = cellEditor;

        return this;
    }

    internal Column SetRequired(bool required)
    {
        Required = required;

        return this;
    }

    internal Column SetHide(bool hide)
    {
        Hide = hide;

        return this;
    }

    internal Column SetSelectable(bool selectable)
    {
        Selectable = selectable;

        return this;
    }
}
