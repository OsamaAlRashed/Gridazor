class Gridazor {
    constructor(element, options = {}) {
        this.element = element;
        this.data = [];
        this.inputRow = {};

        this.settings = {
            propertyName: "",
            overrideColumnDefs: [],
            enableDelete: true,
            deleteButtonId: "",
            addByButton: false,
            addButtonId: "",
            ...options
        };

        this.validateSettings();
        this.columnDefs = this.updateSelectionProperties(
            JSON.parse(document.getElementById(`columnDefs-${this.settings.propertyName}`).innerHTML)
        );
        
        this.columnDefs = this.updateCellStyleIfRequired(this.columnDefs);

        this.requiredColumns = this.columnDefs
            .filter(col => col.required)
            .map(col => col.field);

        this.columnDefs = this.overrideColumns(this.columnDefs, this.settings.overrideColumnDefs);

        // Grid options
        this.gridOptions = this.initializeGridOptions();
        this.gridApi = agGrid.createGrid(this.element, this.gridOptions);

        const jsonData = JSON.parse(document.getElementById(`jsonData-${this.settings.propertyName}`).innerHTML);
        this.setRowData(jsonData);

        // Add event listeners
        this.addRowChangedListener();
        
        if (this.settings.enableDelete) {
            this.addDeleteButtonListener();
        }
        if (this.settings.addByButton) {
            this.addAddButtonListener();
        }
    }

    validateSettings() {
        if (!this.settings.propertyName) {
            throw new Error("'propertyName' is required.");
        }

        if (this.settings.enableDelete && !this.settings.deleteButtonId) {
            throw new Error("'deleteButtonId' is required since the delete button is enabled.");
        }

        if (this.settings.addByButton && !this.settings.addButtonId) {
            throw new Error("'addButtonId' is required since the add button is enabled.");
        }
    }

    updateCellStyleIfRequired(columnDefs) {
        var cellClassProp = {
            cellClass: params => {
                if (params.value === undefined || params.value === null || params.value === '') {
                    return 'red-border-cell';
                }

                return '';
            }
        }

        columnDefs = columnDefs.map(overrideCol => {
            if (overrideCol.required) {
                return { ...overrideCol, ...cellClassProp }
            }

            return overrideCol;
        })

        return columnDefs;
    }

    initializeGridOptions() {
        const defaultGridOptions = {
            data: null,
            pagination: true,
            columnDefs: this.columnDefs,
            domLayout: 'autoHeight',
            rowSelection: 'multiple', // Default value
            onCellValueChanged: (e) => {
                this.updateCell(e);
            },
            pinnedTopRowData: [this.inputRow],
            getRowStyle: ({ node }) =>
                node.rowPinned ? { 'font-weight': 'bold', 'font-style': 'italic', backgroundColor: '#EFEEEE' } : null,
            defaultColDef: {
                flex: 1,
                editable: true,
                valueFormatter: (params) => this.isEmptyPinnedCell(params)
                    ? this.createPinnedCellPlaceholder(params)
                    : undefined,
            },
            onCellEditingStopped: (params) => {
                if (!this.settings.addByButton) {
                    this.addRow(params);
                }
            },
            onGridReady: () => {
                this.gridApi.sizeColumnsToFit();
            },
            onFirstDataRendered: () => {
                this.gridApi.sizeColumnsToFit();
            }
        };

        var options = {
            ...defaultGridOptions,
            ...this.settings
        };
        console.log(options)

        return options;
    }

    addRow() {
        const pinnedTopRow = this.gridApi.getPinnedTopRow(0);
        if (this.isPinnedRowDataCompleted(pinnedTopRow.rowPinned)) {
            const newRow = { ...this.inputRow };
            this.gridApi.applyTransaction({ add: [newRow] });

            this.inputRow = {};
            pinnedTopRow.setData(this.inputRow);

            this.dispatchRowsAfterChangingEvent();
            this.dispatchRowAddedEvent(newRow);
        }
    }

    setRowData(newData) {
        this.data = newData;
        this.gridApi.applyTransaction({ add: this.data });
    }

    updateSelectionProperties(columnDefs) {
        const newProperties = {
            headerCheckboxSelection: true,
            checkboxSelection: true,
            showDisabledCheckboxes: true
        };

        return columnDefs.map(col => {
            if (col.isRowSelectable) {
                return { ...col, ...newProperties };
            }
            return col;
        });
    }

    isEmptyPinnedCell({ node, value }) {
        return (node.rowPinned === 'top' && (value === null || value === ''));
    }

    createPinnedCellPlaceholder({ colDef }) {
        return `${colDef.headerName}...`;
    }

    isPinnedRowDataCompleted(rowPinned) {
        if (rowPinned !== 'top') return false;

        return this.columnDefs
            .filter(col => this.requiredColumns.includes(col.field))
            .every(col => {
                const value = this.inputRow[col.field];

                return value !== undefined && value !== null && value !== '';
            });
    }

    updateCell(e) {
        if (e.oldValue !== e.newValue) {
            this.dispatchRowUpdatedEvent(e.data);
            this.dispatchRowsAfterChangingEvent();
            this.dispatchGridazorCellValueChanged(e)
        }
    }

    getAllRows() {
        const rowData = [];
        this.gridApi.forEachNode(node => rowData.push(node.data));
        return rowData;
    }

    overrideColumns(columnDefs, overrideColumnDefs) {
        const columnMap = new Map();
        columnDefs.forEach(col => columnMap.set(col.field, col));

        overrideColumnDefs.forEach(overrideCol => {
            if (columnMap.has(overrideCol.field)) {
                const originalCol = columnMap.get(overrideCol.field);
                Object.keys(overrideCol).forEach(key => {
                    originalCol[key] = overrideCol[key];
                });
            }
        });

        return Array.from(columnMap.values());
    }


    dispatchRowsAfterChangingEvent() {
        const event = new CustomEvent('rowsAfterChanging', { detail: this.getAllRows() });
        document.dispatchEvent(event);
    }

    dispatchRowUpdatedEvent(row) {
        const event = new CustomEvent('rowUpdated', { detail: row });
        document.dispatchEvent(event);
    }

    dispatchRowAddedEvent(row) {
        const event = new CustomEvent('rowAdded', { detail: row });
        document.dispatchEvent(event);
    }

    dispatchRowsDeletedEvent(rows) {
        const event = new CustomEvent('rowDeleted', { detail: rows });
        document.dispatchEvent(event);
    }

    dispatchGridazorCellValueChanged(e) {
        const event = new CustomEvent('gridazorCellValueChanged', { detail: e });
        document.dispatchEvent(event);
    }

    addRowChangedListener() {
        document.addEventListener('rowsAfterChanging', (e) => {
            const newData = e.detail;
            const container = document.getElementById(`data-${this.settings.propertyName}`);
            container.innerHTML = '';

            newData.forEach((row, index) => {
                if (typeof row === 'object' && row !== null) {
                    let rowHtml = '<div class="row">';
                    let fileInstance = null;
                    let filePropName = null;

                    Object.keys(row).forEach(col => {
                        const colValue = row[col];
                        if (colValue && typeof colValue === 'object' && colValue.hasOwnProperty('file')) {
                            Object.keys(colValue).forEach(fileProperty => {
                                if (fileProperty !== 'file') {
                                    rowHtml += `
                                        <input type="hidden" id="${this.settings.propertyName}_${index}__${col}__${fileProperty}" 
                                               name="${this.settings.propertyName}[${index}].${col}.${fileProperty}" 
                                               value="${colValue[fileProperty]}" />
                                    `;
                                }
                            });
                        } else if (colValue instanceof File) {
                            fileInstance = colValue;
                            filePropName = col;
                            rowHtml += `
                                <input type="file" id="${this.settings.propertyName}_${index}__${col}__File" 
                                       name="${this.settings.propertyName}[${index}].${col}.File" />
                            `;
                        } else {
                            rowHtml += `
                                <input type="hidden" id="${this.settings.propertyName}_${index}__${col}" 
                                       name="${this.settings.propertyName}[${index}].${col}" 
                                       value="${colValue}" />
                            `;
                        }
                    });

                    rowHtml += '</div>';
                    container.insertAdjacentHTML('beforeend', rowHtml);

                    if (fileInstance) {
                        const fileInput = document.getElementById(`${this.settings.propertyName}_${index}__${filePropName}__File`);
                        if (fileInput) {
                            const dt = new DataTransfer();
                            dt.items.add(new File([fileInstance.slice(0, fileInstance.size, fileInstance.type)], fileInstance.name));
                            fileInput.files = dt.files;
                        }
                    }
                } else {
                    console.warn(`Expected object but got ${typeof row}`);
                }
            });
        });
    }

    addDeleteButtonListener() {
        if (!document.getElementById(this.settings.deleteButtonId)) {
            console.warn("there is no element with id" + this.settings.deleteButtonId)
            return;
        }

        document.getElementById(this.settings.deleteButtonId).addEventListener('click', () => {
            const selectedItems = this.gridApi.getSelectedRows();

            if (selectedItems.length > 0) {
                this.gridApi.applyTransaction({ remove: selectedItems });
                this.dispatchRowsAfterChangingEvent();
                this.dispatchRowsDeletedEvent(selectedItems);
            }
        });
    }

    addAddButtonListener() {
        if (!document.getElementById(this.settings.addButtonId)) {
            console.warn("there is no element with id" + this.settings.addButtonId)
            return;
        }

        document.getElementById(this.settings.addButtonId).addEventListener('click', () => {
            this.addRow(this.params);
        });
    }
}

var gridazorDateInputHelper = {
    valueGetter: function (p) {
        var colName = p.colDef.field;

        if (!p.data[colName]) {
            return '';
        }

        return p.data[colName].substring(0, 10);
    }
};