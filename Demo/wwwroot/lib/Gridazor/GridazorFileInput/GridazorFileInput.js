class GridazorFileInput {
    init(params) {
        this.params = params;
        this.eInput = document.createElement('input');
        this.eInput.setAttribute('type', 'file');
    }

    getGui() {
        const container = document.createElement('div');
        container.appendChild(this.eInput);
        return container;
    }

    getValue() {
        return this.eInput.files[0] ? this.eInput.files[0] : this.params.value;
    }

    afterGuiAttached() {
        this.eInput.focus();
    }
}

var gridazorFileInputHelper = {
    cellRender: function (params){
        if (params.value instanceof File) {
            return `<span>${params.value.name}</span>`;
        }

        return params.value ? `<a href="${params.value.path}" download>${params.value.name}</a>` : '<span>No file selected</span>';
    }
};
