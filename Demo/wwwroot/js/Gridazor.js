(function ($) {
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


        var columnDefs = JSON.parse($(`#columnDefs-${settings.propertyName}`).html());
        columnDefs = updateSelectProperties();
        var notRequiredColumns = columnDefs.filter(x => !x.required)
            .map(x => x.field);

        const gridOptions = {
            data: null,
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
                if (isPinnedRowDataCompleted(params, notRequiredColumns)) {
                    var newRow = inputRow;
                    gridApi.applyTransaction({ add: [newRow] });
                    inputRow = {};

                    const pinnedTopRow = gridApi.getPinnedTopRow(0);
                    pinnedTopRow.setData({});

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

        function updateSelectProperties() {
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

        function isPinnedRowDataCompleted(params, notRequiredColumns) {
            if (params.rowPinned !== 'top') return;
            console.log(inputRow)
            return columnDefs.filter((def) => !notRequiredColumns.includes(def.field)).every((def) => inputRow[def.field]);
        }

        function guidGenerator() {
            var S4 = function () {
                return (((1 + Math.random()) * 0x10000) | 0).toString(16).substring(1);
            };
            return (S4() + S4() + "-" + S4() + "-" + S4() + "-" + S4() + "-" + S4() + S4() + S4());
        }

        function updateCell(e) {
            if (e.oldValue !== e.newValue) {
                const event = new CustomEvent('rowsChanged', { detail: getAllRows() });
                document.dispatchEvent(event);
            }
        }

        const gridDiv = this[0];
        let gridApi = agGrid.createGrid(gridDiv, gridOptions);

        var jsonData = JSON.parse($(`#jsonData-${settings.propertyName}`).html());
        setRowData(jsonData)

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

        function getAllRows() {
            let rowData = [];
            gridApi.forEachNode(node => rowData.push(node.data));
            return rowData;
        }

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
                            <input type="hidden" name="${settings.propertyName}[${index}].${key}" value="${value}" />
                        `;
                    });

                    rowHtml += '</div>';
                    container.append(rowHtml);
                } else {
                    console.warn(`Expected object but got ${typeof row}`);
                }
            });
        });

        return this;
    };
}(jQuery));
