using Gridazor.Abstractions;
using Gridazor.Attributes;
using Gridazor.Settings;
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

    public IEnumerable<Column> Get(Type type)
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
                    case SelectionAttribute _:
                        column.SetSelectable(true);
                        break;
                }
            }

            yield return column;
        }
    }
}
