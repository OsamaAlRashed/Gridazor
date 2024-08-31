using Gridazor.Staticts;
using System;

namespace Gridazor.Settings;

public class Column
{
    public string HeaderName { get; private set; }
    public string Field { get; private set; }
    public bool Editable { get; private set; }
    public string CellDataType { get; private set; }
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
        Required = !Helper.IsNullableType(type);
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
