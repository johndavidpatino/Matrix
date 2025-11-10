import { LitElement, html, css } from 'https://cdn.jsdelivr.net/gh/lit/dist@2/all/lit-all.min.js';

export class MxContainer extends LitElement {
    static styles = [
        css`
            :host {
                display: block;
            }
        `
    ];



    static get properties() {
        return {
            isVisible: { type: Boolean},
        };
    }

    render() {
        if(!this.isVisible) return html``
        return html`
            <div>
                <slot></slot>
            </div>
        `;
    }

    setVisible(value) {
        this.isVisible = value
        this.requestUpdate()
        this.render()
    }
}
customElements.define('mx-container', MxContainer);
