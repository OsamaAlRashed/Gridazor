class GridazorDropdown {
    constructor() {
        this.eInput = null;
        this.eDropdown = null;
        this.eGui = null;
        this.selectedValue = null;
        this.options = [];
    }

    init(params) {
        this.options = params.values || [];
        this.selectedValue = params.value;

        // Create the GUI container
        this.eGui = document.createElement('div');
        this.eGui.classList.add('custom-select-wrapper');

        // Create input for search and selection
        this.eInput = document.createElement('input');
        this.eInput.classList.add('custom-select');
        this.eInput.value = this.getTextByValue(this.selectedValue);
        this.eGui.appendChild(this.eInput);

        // Create dropdown list
        this.eDropdown = document.createElement('div');
        this.eDropdown.classList.add('custom-dropdown');
        this.eDropdown.style.display = 'none';
        this.eGui.appendChild(this.eDropdown);

        // Render options in dropdown
        this.renderDropdown(this.options);

        // Input event listener for searching
        this.eInput.addEventListener('input', () => this.filterOptions(this.eInput.value));
        this.eInput.addEventListener('focus', () => this.showDropdown());

        // Click outside to close the dropdown
        document.addEventListener('click', (event) => {
            if (!this.eGui.contains(event.target)) {
                this.hideDropdown();
            }
        });
    }

    getGui() {
        return this.eGui;
    }

    afterGuiAttached() {
        this.eInput.focus();
    }

    getValue() {
        return this.selectedValue;
    }

    isPopup() {
        return true;
    }

    showDropdown() {
        this.eDropdown.style.display = 'block';
    }

    hideDropdown() {
        this.eDropdown.style.display = 'none';
    }

    renderDropdown(options) {
        this.eDropdown.innerHTML = ''; // Clear dropdown content

        options.forEach(option => {
            const optionElement = document.createElement('div');
            optionElement.classList.add('custom-dropdown-item');
            optionElement.innerText = option.text;
            if (option.value === this.selectedValue) {
                optionElement.classList.add('selected'); // Highlight selected option
            }
            optionElement.addEventListener('click', () => {
                this.selectedValue = option.value;
                this.eInput.value = option.text;
                this.hideDropdown();
            });
            this.eDropdown.appendChild(optionElement);
        });
    }

    filterOptions(query) {
        const filteredOptions = this.options.filter(option => option.text.toLowerCase().includes(query.toLowerCase()));
        this.renderDropdown(filteredOptions);
    }

    getTextByValue(value) {
        const selectedOption = this.options.find(option => option.value == value);
        return selectedOption ? selectedOption.text : '';
    }
}

var gridazorDropdownHelper = {
    valueFormatter: function (params) {
        const option = params.colDef.cellEditorParams.values
            .find(opt => opt.value === params.value);

        return option ? option.text : params.value;
    }
};