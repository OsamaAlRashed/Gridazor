const Gridazor = require('./Gridazor'); // Adjust path if needed

describe('Gridazor', () => {
    let gridazor;

    beforeEach(() => {
        document.body.innerHTML = `
            <div id="grid"></div>
            <script id="columnDefs-test" type="application/json">
                [{ "field": "name", "required": true, "isRowSelectable": true }]
            </script>
            <script id="jsonData-test" type="application/json">
                [{ "name": "Test Row" }]
            </script>
        `;
        gridazor = new Gridazor(document.getElementById('grid'), {
            propertyName: 'test'
        });
    });

    test('should initialize with correct settings', () => {
        expect(gridazor.settings.propertyName).toBe('test');
        expect(gridazor.columnDefs.length).toBe(1);
        expect(gridazor.data.length).toBe(1);
    });

    test('should throw error if propertyName is missing', () => {
        expect(() => new Gridazor(document.getElementById('grid'), {})).toThrowError(/'propertyName' is required/);
    });

    // More tests can be added here...
});
