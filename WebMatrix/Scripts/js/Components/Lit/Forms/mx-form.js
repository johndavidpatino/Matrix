import { LitElement, html, css } from '/Scripts/js/libs/lit/lit.js'

class MxForm extends LitElement {
    static properties = {
        data: { type: Object },
        elements: { type: Array, state: true }
    }

    constructor() {
        super()
        this.onSubmit = () => { }
        this.data = {}
        this.elements = []
    }

    render() {
        return html`
            <form class="mx-form" @submit=${this.handleSubmit}>
                <slot></slot>
            </form>
        `
    }
    handleSubmit(event) {
        event.preventDefault()
        this.dispatchEvent(new CustomEvent('mxSubmit', { detail: this.data }))
    }
    firstUpdated() {
        const slot = this.shadowRoot.querySelector('slot');
        const elements = slot.assignedElements({ flatten: true });
        elements.forEach(element => {
            this.#LoadElements(element);
        });
    }
    isValid() {
        let isValid = true;
        this.elements.forEach((element) => {
            if (typeof element.IsValid === 'function') {
                if (!element.IsValid()) {
                    isValid = false;
                }
            }
        });
        return isValid;
    }
    clear() {
        this.elements.forEach((element) => {
            if (typeof element.clear === 'function') {
                element.clear()
            }
        });
    }
    #LoadElements(element) {
        if ("IsValid" in element && typeof element.IsValid === 'function') {
            return this.elements.push(element);
        }

        if (element.children.length > 0) {
            Array.from(element.children).forEach(elementI => {
                this.#LoadElements(elementI);
            });
        }
    }
    get _control() {
        return this.renderRoot.querySelector(".mx-form");
    }
}

customElements.define('mx-form', MxForm)