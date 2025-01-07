using Gridazor.Statics;
using System;
using Gridazor.Abstractions;

namespace Gridazor.Models;

/// <summary>
/// Represents the metadata of a column obtained from <see cref="IColumnsProvider"/>.
/// </summary>
public sealed class Column
{
    /// <summary>
    /// Gets the header name of the column. Defaults to the property name.
    /// </summary>
    /// <remarks>For more details, see <see href="https://www.ag-grid.com/javascript-data-grid/column-headers/">AG Grid Column Headers</see>.</remarks>
    public string HeaderName { get; private set; }

    /// <summary>
    /// Gets the field name of the column. It serves as the unique identifier for the column, defaulting to the property name with a lowercase first letter.
    /// </summary>
    public string Field { get; private set; }

    /// <summary>
    /// Gets a value indicating whether the column is editable. Defaults to true.
    /// </summary>
    public bool Editable { get; private set; }

    /// <summary>
    /// Gets the data type for the cell in the column.
    /// </summary>
    public string CellDataType { get; private set; }

    /// <summary>
    /// Gets the editor for the cell in the column.
    /// </summary>
    public string CellEditor { get; private set; }

    /// <summary>
    /// Gets a value indicating whether the column is required.
    /// </summary>
    public bool Required { get; private set; }

    /// <summary>
    /// Gets a value indicating whether the column is hidden.
    /// </summary>
    public bool Hide { get; private set; }

    /// <summary>
    /// Gets a value indicating whether the column is used for row selection.
    /// </summary>
    public bool IsRowSelectable { get; private set; }

    internal Column(Type type, string name)
    {
        CellDataType = Helper.GetDefaultCellDataType(type);
        CellEditor = Helper.GetDefaultCellEditor(type);
        Editable = true;
        Field = name.FirstToLower();
        HeaderName = name;
        Required = type.IsValueType && !Helper.IsNullableType(type);
        Hide = false;
        IsRowSelectable = false;
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

    internal Column SetIsRowSelectable(bool isRowSelectable)
    {
        IsRowSelectable = isRowSelectable;

        return this;
    }
}
