import { LitElement, html, css, repeat } from '/Scripts/js/libs/lit/lit.js';
import { resetStyles } from '../Styles/font-awesome.js';

class MxSelect extends LitElement {
    static properties = {
        name: { type: String },
        value: { type: String },
        options: { type: Array },
        required: { type: Boolean },
        ValidateAction: { type: Function },
        HasEmptyOption: { type: Boolean },
        EmptyOptionLabel: { type: String },
        EmptyOptionValue: { type: String },
        IsExecutedFirstUpdate: { type: Boolean }
    }

    constructor(){
        super()
        this.name = ''
        this.value = ''
        this.options = []
        this.required = false
        this.ValidateAction = () => {
            return true
        }
        this.HasEmptyOption = false
        this.EmptyOptionLabel = '---Seleccione---'
        this.EmptyOptionValue = ''
    }
    
   
    render(){
        return html`
            <label class="mx-dropdown">
                <span class="mx-dropdown-label">
                    <slot></slot>
                </span>
                <span class="mx-dropdown-control-wrapper">
                    <select class="mx-dropdown-control" name=${this.name} ?required=${this.required} @change=${this.handleChange} .value="${this.value}">
                        ${this.HasEmptyOption ? html`<option value=${this.EmptyOptionValue}>${this.EmptyOptionLabel}</option>` : ''}
                        ${repeat(this.options, option => html`<option class="mx-select-option" value=${option.value} .selected=${this.value == option.value}>${option.label}</option>`)}
                    </select>
                </span>
            </label>
        `
    }

    firstUpdated(){
        
        this.IsExecutedFirstUpdate = true
        
    }

    clear(){
        this.value = ""
        this._control.value = ""
        let option = this._control.querySelector(`option[value="${this.EmptyOptionValue}"]`)
        if(option) option.selected = true
    }

    handleChange(event){
        this.value = event.target.value
        this.IsValid();
        let isValid = this.ValidateAction()
        this.dispatchEvent(new CustomEvent('mxChange', { detail: { value: this.value, isValid: isValid } }))
    }

    setOptions(options){
        this.options = options
    }

    setValue(value){
        this.value = value
    }

    get _control(){
        return this.renderRoot.querySelector('.mx-dropdown-control')
    }

    IsValid() {
        const state = this._control.checkValidity();
        this._control.classList.toggle('invalid', !state);
        return state;
    }

    static styles = [resetStyles,css`
    .mx-dropdown {
        display: flex;
        flex-direction: column;
        gap: .25rem;
        position: relative;
        box-sizing: border-box;
        line-height: 1.5;
    }
    .mx-dropdown-control{
        appearance: none;
        height: 2.5rem;
        padding-inline: .5rem;
        border-radius: 3px;
        width: 100%;
        border: 1px solid #d0d0d0;
        outline: none;

    }
    .mx-dropdown-control-wrapper{
        position: relative;
    }
    .mx-dropdown-control-wrapper::after{
        content: url("/Images/icons-svg/arrow-down.svg");
        pointer-events: none;
        width: 1rem;
        height: 1rem;
        position: absolute;
        z-index: 2;
        right: .5rem;
        top: 50%;
        transform: translateY(-50%);
    }
    .mx-dropdown-control.invalid{
        border: 1px dotted red;
        background-color: #FFF8F8;
     }
    `]

}

customElements.define('mx-select', MxSelect)

export {MxSelect}