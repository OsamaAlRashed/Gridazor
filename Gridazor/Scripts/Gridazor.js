(function ($) {
    let data = [];
    let inputRow = {};

    $.fn.gridazor = function (options) {
        const settings = $.extend({
            propertName: "",
            enableRtl: false,
            overrideColumnDefs: [],
            enableDelete: true,
            deleteButtonId: ""
        }, options);

        const gridOptions = {
            data: null,
            enableRtl: settings.enableRtl,
            columnDefs: settings.columnDefs,
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
                if (isPinnedRowDataCompleted(params, settings.notRequiredColumns)) {
                    var newRow = inputRow;
                    gridApi.applyTransaction({ add: [newRow] });
                    inputRow = {};

                    const pinnedTopRow = gridApi.getPinnedTopRow(0);
                    pinnedTopRow.setData({});

                    const event = new CustomEvent('rowAdded', { detail: newRow });
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

        function createPinnedCellPlaceholder({ colDef }) {
            return colDef.headerName + '...';
        }

        function isPinnedRowDataCompleted(params, notRequiredColumns) {
            if (params.rowPinned !== 'top') return;

            return columnDefs.filter((def) => def.field !== "id" && !notRequiredColumns.includes(def.field)).every((def) => inputRow[def.field]);
        }

        function guidGenerator() {
            var S4 = function () {
                return (((1 + Math.random()) * 0x10000) | 0).toString(16).substring(1);
            };
            return (S4() + S4() + "-" + S4() + "-" + S4() + "-" + S4() + "-" + S4() + S4() + S4());
        }

        function updateCell(e) {
            if (e.data.id && e.oldValue !== e.newValue) {
                // Trigger custom "rowUpdated" event
                const event = new CustomEvent('rowUpdated', { detail: e.data });
                document.dispatchEvent(event);
            }
        }

        const gridDiv = this[0];
        let gridApi = agGrid.createGrid(gridDiv, gridOptions);

        var data = JSON.parse($(`#jsonData-${propertName}`).val());

        $.ajax({
            url: settings.getUrl,
        }).done(function (data) {
            setRowData(data)
        });

        if (enableDelete) {
            document.getElementById(settings.deleteButtonId).addEventListener('click', function () {
                var selectItems = gridApi.getSelectedRows();

                if (selectItems.length === 0) {
                    return;
                }

                gridApi.applyTransaction({ remove: selectItems });
                
                const event = new CustomEvent('rowsDeleted', { detail: selectItems });
                document.dispatchEvent(event);
            })
        }

        return this;
    };
}(jQuery));
