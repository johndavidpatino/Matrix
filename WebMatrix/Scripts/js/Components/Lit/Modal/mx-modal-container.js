import { LitElement, html, css } from '/Scripts/js/libs/lit/lit.js'


export class MxModalContainer extends LitElement {
    constructor() {
        super()

    }
    render() {
        return html`
            <div class="mx-modal-container" @mousedown="${this.handleClickOutside}">
                <slot></slot>
            </div>
        `;
    }

    async handleClickOutside(event) {
        let target = event.target;
        if (target.classList.contains('mx-modal-container')) {
            this.cancelAction(event);
        }
    }

    cancelAction(event) {
        this.dispatchEvent(new CustomEvent('mxClose'), event);
        this.remove();
    }

    static styles = css`
        .mx-modal-container {            
            position: fixed;
            display: flex;
            justify-content: center;
            align-items: center;
            top: 0;
            left: 0;
            width: 100%;
            height: 100vh;
            background-color: rgba(0, 0, 0, 0.5);
            z-index: 100;
            overflow-y: auto;
            padding-inline: 4rem;
            box-sizing: border-box;
        }
        ::slotted([slot="content"]) {
            height:400px;
            display:flex;
            overflow-y:auto;
        }
    `;
}

customElements.define('mx-modal-container', MxModalContainer)
