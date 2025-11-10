import { html,css,repeat, LitElement } from "../libs/lit/lit.js"

export class MxDropDown extends LitElement {

    static properties = {
        items: { type: Array },
    };
    constructor() {
        super()
        this.items = [{value: 1, label: "A�o"}]
    }

    render() {
        console.log(this.items)
        return html`
        <label class="mx-dropdown">
            <span class="mx-dropdown-label"><slot></slot></span>
            <span class="mx-dropdown-control-wrapper">
                <select @change="${this.ChangeValue}" class="mx-dropdown-control">
                    ${repeat(this.items, (item, index) => html`<option value="${item.value}">${item.label}</option>`)}
                </select>
            </span>
        </label>
                `
    }

    ChangeValue(event) {
        console.log("change")
    }

    static styles = css`
        .mx-dropdown {
            display: flex;
            flex-direction: column;
            gap: .25rem;
            position: relative;
        }
        .mx-dropdown-control{
            appearance: none;
            padding: .5rem;
            border-radius: 3px;
            width: 100%;
            border: 1px solid #d0d0d0;
            outline: none;

        }


        .mx-dropdown-control-wrapper{
            position: relative;
        }
        .mx-dropdown-control-wrapper::after{
            content: url("../../Images/icons-svg/arrow-down.svg");
            pointer-events: none;
            width: 1rem;
            height: 1rem;
            position: absolute;
            z-index: 2;
            right: .5rem;
            top: 50%;
            transform: translateY(-50%);
        }`
}

customElements.define('mx-dropdown', MxDropDown)