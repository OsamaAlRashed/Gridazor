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
        console.log(this.params.value)
        return this.eInput.files[0] ? this.eInput.files[0] : this.params.value;
    }

    afterGuiAttached() {
        this.eInput.focus();
    }
}
