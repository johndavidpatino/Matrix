import { LitElement, html, css, repeat } from '/Scripts/js/libs/lit/lit.js';
import { resetStyles } from '../Styles/font-awesome.js';

class MxCheckboxList extends LitElement {
    static properties = {
        name: { type: String },
        value: { type: Array },
        options: { type: Array },
        required: { type: Boolean },
        ValidateAction: { type: Function },
        IsExecutedFirstUpdate: { type: Boolean },
        orientation: { type: String }
    }

    constructor() {
        super()
        this.name = ''
        this.value = []
        this.options = []
        this.required = false
        this.ValidateAction = () => {
            return true
        }
        this.orientation = "vertical"
    }


    render() {
        return html`
            <label class="mx-checkbox-list">
                <span class="mx-checkbox-list-label">
                    <slot></slot>
                </span>
                <span class="mx-checkbox-list-wrapper">
                    ${repeat(this.options, option => html`<label class="mx-checkbox-wrapper"><input type="checkbox" class="mx-checkbox-option" name=${this.name} value=${option.value} @click=${this.handleClick}>${option.label}</label>`)}
                </span>
            </label>
        `
    }

    firstUpdated() {
        this.IsExecutedFirstUpdate = true
        this._control.classList.toggle("orientation-vertical", this.orientation == "vertical");
    }

    clear() {
        this.value.forEach(check => {
            this._control.querySelector(`input[value="${check}"]`).checked = false;
        });
        this.value = [];
    }

    handleClick(event) {
        const value = event.target.value;

        if (this.value.some(x => x == value)) {
            this.value = this.value.filter(x => x != value);
        }
        else {
            this.value.push(value);
        }
        this.IsValid();
    }

    setOptions(options) {
        this.options = options
    }

    get _control() {
        return this.renderRoot.querySelector(`.mx-checkbox-list-wrapper`)
    }

    IsValid() {
        const state = this.required ? this.value.length > 0 : true;
        this._control.classList.toggle('invalid', !state);
        return state;
    }

    static styles = [resetStyles, css`
        .mx-checkbox-list-wrapper
        {
            padding:10px;
            display:flex;
            gap:5px;
        }
        .mx-checkbox-list-wrapper.invalid
        {
            border: 1px dotted red;
            background-color: #FFF8F8;
        }
        .mx-checkbox-list-wrapper.orientation-vertical
        {
            flex-direction:column;
        }
        .mx-checkbox-wrapper{
            display:flex;
            gap:5px;
        }
        `]

}

customElements.define('mx-checkbox-list', MxCheckboxList)