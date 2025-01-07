using Gridazor.Abstractions;
using Gridazor.Attributes;
using Gridazor.Models;
using System;
using System.Collections.Generic;

namespace Gridazor.Core;

internal sealed class DefaultColumnProvider : IColumnsProvider
{
    private DefaultColumnProvider() { }

    private static readonly Lazy<DefaultColumnProvider> _lazy =
        new(() => new DefaultColumnProvider());
    internal static DefaultColumnProvider Instance 
    { 
        get 
        { 
            return _lazy.Value; 
        } 
    }

    public IEnumerable<Column> Get(Type type, Dictionary<string, Func<Column, object>> overrideColumns)
    {
        var properties = type.GetProperties();

        foreach (var property in properties)
        {
            var column = new Column(property.PropertyType, property.Name);
            var customAttributes = property.GetCustomAttributes(true);

            foreach (var attribute in customAttributes)
            {
                switch (attribute)
                {
                    case FieldAttribute fieldAttribute:
                        column.SetField(fieldAttribute.Name);
                        break;
                    case HeaderNameAttribute headerNameAttribute:
                        column.SetHeaderName(headerNameAttribute.Name);
                        break;
                    case EditableAttribute editableAttribute:
                        column.SetEditable(editableAttribute.Editable);
                        break;
                    case CellDataTypeAttribute cellDataTypeAttribute:
                        column.SetCellDataType(cellDataTypeAttribute.Type);
                        break;
                    case CellEditorAttribute cellEditorAttribute:
                        column.SetCellEditor(cellEditorAttribute.Name);
                        break;
                    case RequiredAttribute requiredAttribute:
                        column.SetRequired(requiredAttribute.Required);
                        break;
                    case HideAttribute _:
                        column.SetHide(true);
                        break;
                    case RowSelectionAttribute _:
                        column.SetIsRowSelectable(true);
                        break;
                }
            }

            OverrideColumnMetadataValues(overrideColumns, ref column);

            yield return column;
        }
    }

    private static void OverrideColumnMetadataValues(Dictionary<string, Func<Column, object>> overrideColumns, ref Column column)
    {
        if (overrideColumns.TryGetValue(nameof(FieldAttribute), out Func<Column, object>? fieldAttributeFunc))
        {
            column.SetField((string)fieldAttributeFunc(column));
        }

        if (overrideColumns.TryGetValue(nameof(HeaderNameAttribute), out Func<Column, object>? headerNameAttributeFunc))
        {
            column.SetHeaderName((string)headerNameAttributeFunc(column));
        }

        if (overrideColumns.TryGetValue(nameof(EditableAttribute), out Func<Column, object>? editableAttributeFunc))
        {
            column.SetEditable((bool)editableAttributeFunc(column));
        }

        if (overrideColumns.TryGetValue(nameof(CellDataTypeAttribute), out Func<Column, object>? cellDataTypeAttributeFunc))
        {
            column.SetCellDataType((string)cellDataTypeAttributeFunc(column));
        }

        if (overrideColumns.TryGetValue(nameof(CellEditorAttribute), out Func<Column, object>? cellEditorAttributeFunc))
        {
            column.SetCellEditor((string)cellEditorAttributeFunc(column));
        }

        if (overrideColumns.TryGetValue(nameof(RequiredAttribute), out Func<Column, object>? requiredAttributeFunc))
        {
            column.SetRequired((bool)requiredAttributeFunc(column));
        }

        if (overrideColumns.TryGetValue(nameof(HideAttribute), out Func<Column, object>? hideAttributeFunc))
        {
            column.SetHide((bool)hideAttributeFunc(column));
        }

        if (overrideColumns.TryGetValue(nameof(RowSelectionAttribute), out Func<Column, object>? rowSelectionAttributeFunc))
        {
            column.SetIsRowSelectable((bool)rowSelectionAttributeFunc(column));
        }
    }
}
