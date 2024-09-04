class FileInput {
    init(params) {
        this.params = params;

        // Create a wrapper div to hold the file input and a label
        this.eContainer = document.createElement('div');
        this.eContainer.className = 'file-input-editor-container'; // Use CSS class

        // Create the file input element (hidden)
        this.eInput = document.createElement('input');
        this.eInput.type = 'file';
        this.eInput.className = 'file-input'; // Use CSS class
        // Create a custom button to trigger the file input
        this.eButton = document.createElement('button');
        this.eButton.innerText = 'Choose File';
        this.eButton.className = 'file-input-button'; // Use CSS class

        // Create a label to display the selected file name
        this.eLabel = document.createElement('span');
        this.eLabel.className = 'file-input-label'; // Use CSS class

        // Initialize the label with the current value or a placeholder
        if (this.params.value && this.params.value !== '') {
            this.eLabel.innerText = this.params.value;
        } else {
            this.eLabel.innerText = 'No file chosen';
        }

        // Event listener to open the file dialog when the button is clicked
        this.eButton.addEventListener('click', (event) => {
            event.preventDefault(); // Prevent default button action
            this.eInput.click();
        });

        // Event listener for file change
        this.eInput.addEventListener('change', (event) => {
            const file = event.target.files[0];
            if (file) {
                // Update the label with the selected file name
                this.eLabel.innerText = file.name;
                this.selectedFile = file.name;

                let dt = new DataTransfer();
                dt.items.add(
                    new File(
                        [file.slice(0, file.size, file.type)],
                        file.name
                    ));

                console.log(`Tests_${params.rowIndex}__file`)
                var back = document.getElementById(`Tests_${params.rowIndex}__file`);
                console.log(back)
                back.files = dt.files;
            }
        });

        // Append the button and label to the container
        this.eContainer.appendChild(this.eButton);
        this.eContainer.appendChild(this.eLabel);
    }

    getGui() {
        return this.eContainer;
    }

    afterGuiAttached() {
        this.eButton.focus();
    }

    getValue() {
        return this.selectedFile || this.params.value || '';
    }

    destroy() {
        this.eButton.removeEventListener('click', this.onFileChange);
        this.eInput.removeEventListener('change', this.onFileChange);
    }

    isPopup() {
        return true;
    }

    isCancelBeforeStart() {
        return false;
    }
}