﻿(function ($) {
    let data = [];
    let inputRow = {};

    $.fn.gridazor = function (options) {
        const settings = $.extend({
            propertyName: "",
            enableRtl: false,
            overrideColumnDefs: [],
            enableDelete: true,
            deleteButtonId: ""
        }, options);

        // check
        if (!settings.propertyName) {
            throw new Error("'propertyName' is required.");
        }

        if (settings.enableDelete && !settings.deleteButtonId) {
            throw new Error("'deleteButtonId' is required since the delete button is enabled.");
        }

        // update columnDefs
        var columnDefs = JSON.parse($(`#columnDefs-${settings.propertyName}`).html());
        columnDefs = updateSelectionProperties();
        var requiredColumns = columnDefs.filter(x => x.required).map(x => x.field);
        columnDefs = overrideColumns(columnDefs, settings.overrideColumnDefs);

        // grid options
        const gridOptions = {
            data: null,
            pagination: true,
            enableRtl: settings.enableRtl,
            columnDefs: columnDefs,
            domLayout: 'autoHeight',
            rowSelection: 'multiple',
            onCellValueChanged: updateCell,
            pinnedTopRowData: [inputRow],
            getRowStyle: ({ node }) =>
                node.rowPinned ? { 'font-weight': 'bold', 'font-style': 'italic' } : 0,
            defaultColDef: {
                flex: 1,
                editable: true,
                valueFormatter: (params) =>
                    isEmptyPinnedCell(params)
                        ? createPinnedCellPlaceholder(params)
                        : undefined,
            },
            onCellEditingStopped: (params) => {
                if (isPinnedRowDataCompleted(params, requiredColumns)) {
                    var newRow = { ...inputRow };
                    gridApi.applyTransaction({ add: [newRow] });

                    inputRow = {};

                    const pinnedTopRow = gridApi.getPinnedTopRow(0);
                    pinnedTopRow.setData(inputRow);

                    const event = new CustomEvent('rowsChanged', { detail: getAllRows() });
                    document.dispatchEvent(event);
                }
            },
            onGridReady: (params) => {
                gridApi.sizeColumnsToFit();
            },
            onFirstDataRendered: (params) => {
                gridApi.sizeColumnsToFit();
            }
        };

        // init the grid
        const gridDiv = this[0];
        let gridApi = agGrid.createGrid(gridDiv, gridOptions);

        var jsonData = JSON.parse($(`#jsonData-${settings.propertyName}`).html());
        setRowData(jsonData)


        // functions
        function isEmptyPinnedCell({ node, value }) {
            return (
                (node.rowPinned === 'top' && value == null) ||
                (node.rowPinned === 'top' && value == '')
            );
        }

        function setRowData(newData) {
            data = newData;
            gridApi.applyTransaction({ add: data });
        }

        function updateSelectionProperties() {
            var newProperties = {
                headerCheckboxSelection: true,
                checkboxSelection: true,
                showDisabledCheckboxes: true
            }

            return columnDefs.map(col => {
                if (col.selectable) {
                    return { ...col, ...newProperties };
                }

                return col;
            });
        }

        function createPinnedCellPlaceholder({ colDef }) {
            return colDef.headerName + '...';
        }

        function isPinnedRowDataCompleted(params, requiredColumns) {
            if (params.rowPinned !== 'top')
                return;

            var isCompleted = columnDefs
                .filter(
                    (def) => requiredColumns.includes(def.field))
                .every((def) => inputRow[def.field]);

            return isCompleted;
        }

        function updateCell(e) {
            if (e.oldValue !== e.newValue) {
                const event = new CustomEvent('rowsChanged', { detail: getAllRows() });
                document.dispatchEvent(event);
            }
        }

        function getAllRows() {
            let rowData = [];
            gridApi.forEachNode(node => rowData.push(node.data));
            return rowData;
        }

        function overrideColumns(columnDefs, overrideColumnDefs) {
            const columnMap = new Map();
            columnDefs.forEach(col => columnMap.set(col.field, col));

            overrideColumnDefs.forEach(overrideCol => {
                if (columnMap.has(overrideCol.field)) {
                    columnMap.set(overrideCol.field, overrideCol);
                }
            });

            return Array.from(columnMap.values());
        }


        // Event listeners
        document.addEventListener('rowsChanged', (e) => {
            const newData = e.detail;
            const container = $(`#data-${settings.propertyName}`);

            container.html('');

            newData.forEach((row, index) => {
                if (typeof row === 'object' && row !== null) {
                    let rowHtml = '<div class="row">';

                    Object.keys(row).forEach(key => {
                        const value = row[key];
                        rowHtml += `
                        <input type="hidden" id="${settings.propertyName}_${index}__${key}" name="${settings.propertyName}[${index}].${key}" value="${value}" />
                        `;
                    });

                    rowHtml += '</div>';
                    container.append(rowHtml);
                } else {
                    console.warn(`Expected object but got ${typeof row}`);
                }
            });
        });

        if (settings.enableDelete) {
            document.getElementById(settings.deleteButtonId).addEventListener('click', function () {
                var selectItems = gridApi.getSelectedRows();

                if (selectItems.length === 0) {
                    return;
                }

                gridApi.applyTransaction({ remove: selectItems });

                const event = new CustomEvent('rowsChanged', { detail: getAllRows() });
                document.dispatchEvent(event);
            })
        }

        return this;
    };
}(jQuery));