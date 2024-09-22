class GridazorFileInput {
    init(params) {

        this.params = params;
        this.eInput = document.createElement('input');
        this.eInput.setAttribute('type', 'file');
        this.eInput.addEventListener('change', this.handleFileChange.bind(this, params));

        this.eFilePreview = document.createElement('div');
        this.eFilePreview.classList.add('file-preview');

        if (params.value) {
            this.createDownloadLink(params.value); // Show download button for existing files
        }
    }

    getGui() {
        const container = document.createElement('div');
        container.appendChild(this.eInput);
        container.appendChild(this.eFilePreview);
        return container;
    }

    getValue() {
        return this.eInput.files[0] ? this.eInput.files[0] : this.params.value; // Return the file object if selected, otherwise keep the original value
    }

    afterGuiAttached() {
        this.eInput.focus();
    }

    handleFileChange(params) {
        const file = this.eInput.files[0];
        if (file) {
            const reader = new FileReader();
            reader.onload = (e) => {
                this.previewFile(file, e.target.result);
            };
            reader.readAsDataURL(file); // Use this for preview (image) or icon for non-images
        }
    }

    previewFile(file, result) {
        this.eFilePreview.innerHTML = ''; // Clear previous preview
        if (file.type.startsWith('image/')) {
            // If it's an image, display a small preview
            const img = document.createElement('img');
            img.src = result;
            img.alt = file.name;
            img.style.maxWidth = '100px';
            this.eFilePreview.appendChild(img);
        } else {
            // For other file types, show a file icon or generic preview
            const fileIcon = document.createElement('span');
            fileIcon.textContent = `File: ${file.name}`;
            this.eFilePreview.appendChild(fileIcon);
        }
    }

    createDownloadLink(fileName) {
        const downloadLink = document.createElement('a');
        downloadLink.textContent = `Download ${fileName}`;
        downloadLink.href = `${fileName}`; // Replace this with the actual path to the file
        downloadLink.download = fileName;
        downloadLink.classList.add('download-link');
        this.eFilePreview.appendChild(downloadLink);
    }
}
