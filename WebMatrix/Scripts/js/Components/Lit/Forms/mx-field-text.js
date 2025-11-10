import { LitElement, html, css } from '/Scripts/js/libs/lit/lit.js'
import { MxFormField } from './mx-form-field.js';

export class MxFieldText extends MxFormField {
    static properties = {
        value: { type: String },
        type: { type: String },
        required: { type: Boolean }
    }
    constructor() {
        super()
        this.value = '';
    }
    render(){
           return html`
            <label class="mx-field-text${this.isValidationExecuted && !this.isValid?' mx-invalid':''}" >
                <span class="mx-field-text__label">
                    <slot>
                        
                    </slot>
                </span>
                <input type="${this.type}" class="mx-field-text__input${this.value !== '' ? ' mx-filled' : ''}" @input="${this.handleInput}" @change="${this.handleChange}" .value="${this.value}" ?required=${this.required}>
            </label>
        `
    }
    get _control() {
        return this.renderRoot.querySelector('.mx-field-text__input');
    }
    handleInput(event) {
        this.value = event.target.value
        this.dispatchEvent(new CustomEvent('mxInput', { detail: event.target.value }))
        this.IsValid();
    }
    handleChange(event) {
        this.value = event.target.value
        this.dispatchEvent(new CustomEvent('mxChange', { detail: event.target.value }))
        this.IsValid();
    }
    IsValid() {
        const state = this._control.checkValidity();
        this._control.classList.toggle('invalid', !state);
        return state;
    }
    clear() {
        this.value = '';
        this.isFirstChange = false
    }

    static styles = [this.styles,
        css`
        .mx-field-text{
            position: relative;
            display: inline-flex;
            flex-direction: column;
            width: 100%;
        }
        .mx-field-text__label{
            display: block;
            font-size: 14px;
            margin-bottom: 5px;
        }
        .mx-field-text__input{
            box-sizing: border-box;
            display: block;
            width: 100%;
            height: 40px;
            border: 1px solid #ccc;
            border-radius: 5px;
            padding: 10px;
            outline: none;
        }
        .mx-field-text__input:focus{
            border-color: var(--primary);
        }
        .mx-invalid .mx-field-text__input{
            border-color: var(--red);
        }

        .mx-field-text__input.invalid
        {
            border: 1px dotted red;
            background-color: #FFF8F8;
        }
    `]
}

customElements.define('mx-field-text', MxFieldText)