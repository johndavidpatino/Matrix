import { LitElement, html, css } from '/Scripts/js/libs/lit/lit.js'
import { resetStyles, fontAwesomeStyles } from '../Styles/font-awesome.js';

export class mxLoaderFile extends LitElement {
    static properties = {
        required: { type: Boolean },
        formatsAllowed: { type: Array },
        iconFile: { type: String, state: true },
        isFileLoaded: { type: Boolean, state: true }
    }
    constructor() {
        super()
        this.resizable = false;
        this.iconFile = "fa-cloud-upload-alt";
        this.isFileLoaded = false;
    }
    async firstUpdated() {

    }
    render() {
        return html`
            <label class="mx-loader-file">
                <span class="mx-input-label" >
                    <slot></slot>
                </span>
                <span class="mx-loader-file-control-wrapper">
                    <input type="file" accept="${this.formatsAllowed.map(x => x.mimeType).join(",")}" @change=${this.handlerChange} />
                    <i class="fa fa-4x ${this.iconFile}"></i>
                    ${this.isFileLoaded
                ? html`<p>${this._control.files[0].name}</p>`
                : html`<p>Debes seleccionar un archivo para cargar <br/>Solo archivos <strong>${this.formatsAllowed.map(x => x.fileExtension).join(",")}</strong> son aceptados </p>`
            }
                </span>
            </label>
        `
    }

    handlerChange(event) {
        this.iconFile = "fa-check";
        this.isFileLoaded = true;
        this.IsValid();
    }

    clear() {
        this.value = ""
        this._control.value = ""
    }

    IsValid() {
        const state = this._control.files.length > 0;
        this.renderRoot.querySelector(".mx-loader-file-control-wrapper").classList.toggle('invalid', !state);
        return state;
    }

    get _control() {
        return this.renderRoot.querySelector('.mx-loader-file-control-wrapper input')
    }

    file() {
        return this._control.files[0];
    }

    static styles = [resetStyles, fontAwesomeStyles, css`
    .mx-loader-file{
        display: flex;
        flex-direction: column;
    }
    .mx-loader-file input[type="file"] {
        display: none;
    }

    .mx-loader-file-control-wrapper{
        display: flex;
        flex-direction: column;
        align-items: center;
        justify-content: center;
        border: 1px dashed #d0d0d0;
        border-radius: 3px;
        padding: 1rem;
        cursor: pointer;
    }
    .mx-loader-file-control-wrapper.invalid{
        border: 1px dotted red;
        background-color: #FFF8F8;
     }
    `]

}

customElements.define('mx-loader-file', mxLoaderFile)