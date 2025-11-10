import { LitElement, html, css } from '/Scripts/js/libs/lit/lit.js';
import { resetStyles } from '/Scripts/js/Components/Lit/Styles/font-awesome.js';
import { MxFormField } from './mx-form-field.js'; 

export class MxFieldDate extends MxFormField {
    constructor(){
        super()
    }
    render(){
        return html`
        <label class="mx-field-date${this.isValidationExecuted && !this.isValid?' mx-invalid':''}">
            <span class="mx-field-date__label">
                <slot></slot>
            </span>
            <input class="mx-field-date__input${this.value != ''?' mx-filled':''}" type="date" id="${this.id}" .value="${this.value}" @change="${this.handleChange}" />
            <span class="mx-error">
                ${this.errorMessage}
            </span>
        </label>`
    }
    handleChange(e){
        this.value = e.target.value
        this.dispatchEvent(new CustomEvent('mxChange', {
            detail: e.target.value
        }))
    }
    static styles = [this.styles,resetStyles, css`
        .mx-field-date{
            display: flex;
            flex-direction: column;
            gap: .25rem;
            position: relative;
        }
        .mx-field-date__input{
            border: none;
            border: 1px solid #d0d0d0;
            outline: none;
            padding: .5rem;
            border-radius: 3px;
            accent-color: var(--primary);
        }
        .mx-field-date__input:focus{
            border-color: var(--primary);
        }
        .mx-invalid .mx-field-date__input{
            border-color: var(--red);
        }

    `]
}
customElements.define('mx-field-date', MxFieldDate)