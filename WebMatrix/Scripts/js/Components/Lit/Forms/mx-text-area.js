import { LitElement, html, css } from '/Scripts/js/libs/lit/lit.js'
import { resetStyles } from '../Styles/font-awesome.js';
import { MxFormField } from './mx-form-field.js'; 

export class mxTextArea extends MxFormField {
    static properties = {
        value: { type: String },
        resizable: { type: Boolean },
        required: { type: Boolean }
    }
    constructor() {
        super()
        this.value = '';
        this.resizable = false;
    }
    async firstUpdated() {

    }
    render() {
        return html`
            <label class="mx-input-text">
                <span class="mx-input-label" >
                    <slot></slot>
                </span>
                <textarea class="mx-input-control ${this.resizable ? "resizable" : "no-resize"}" type="text"  @change=${this.handleChange} .value=${this.value} @input=${this.handleInput} ?required=${this.required}></textarea>
            </label>
        `
    }

    handleChange(event) {
        this.value = event.target.value
        this.dispatchEvent(new CustomEvent('mxChange', { detail: this.value }))
    }

    handleInput(event) {
        this.value = event.target.value
        this.dispatchEvent(new CustomEvent('mxInput', { detail: this.value }))
    }

    clear() {
        this.value = ""
        this._control.value = ""
    }

    IsValid() {
        const state = this._control.checkValidity();
        this._control.classList.toggle('invalid', !state);
        return state;
    }

    get _control() {
        return this.renderRoot.querySelector('.mx-input-control')
    }

    static styles = [resetStyles, css`
    .mx-input-text{
        display: flex;
        flex-direction: column;
    }

    .mx-input-control {
        padding: .5rem;
        border: 1px solid #d0d0d0;
        outline: none;
        border-radius: 3px;
        height: 100px;
        width:100%;
        box-sizing:border-box;
    }
    .mx-input-control.resizable{
        resize: both;
    }
    .mx-input-control.no-resize {
        resize: none;
    }
    .mx-input-control.invalid{
        border: 1px dotted red;
        background-color: #FFF8F8;
     }
    `]

}

customElements.define('mx-text-area', mxTextArea)