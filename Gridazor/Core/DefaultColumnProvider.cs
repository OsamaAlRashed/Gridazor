using Gridazor.Abstractions;
using Gridazor.Attributes;
using Gridazor.Settings;
using System;
using System.Collections.Generic;

namespace Gridazor.Core;

internal sealed class DefaultColumnProvider : IColumnsProvider
{
    public IEnumerable<Column> Get(Type type)
    {
        var properties = type.GetProperties();

        foreach (var property in properties) 
        {
            var column = new Column(type);
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
                    case SelectableAttribute _:
                        column.SetSelectable(true);
                        break;
                }
            }

            yield return column;
        }
    }
}
