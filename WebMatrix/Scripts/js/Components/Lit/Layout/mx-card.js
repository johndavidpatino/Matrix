import { LitElement, html, css, repeat } from '/Scripts/js/libs/lit/lit.js';
import { resetStyles } from '../Styles/font-awesome.js';

class MxCard extends LitElement {
    static properties = {
    }

    constructor() {
        super()
    }


    render() {
        return html`
            <div class="mx-card">
                <slot name="header">
                </slot>
                <slot name="body"></slot>
            </div>
        `
    }

    static styles = [resetStyles, css`
        .mx-card
        {
            position: relative;
            display: flex;
            flex-direction: column;
            min-width: 0;
            word-wrap: break-word;
            background-color: #fff;
            background-clip: border-box;
            border: 1px solid rgba(26,54,126,0.125);
            border-radius: .25rem
        }
        ::slotted([slot="header"]) {
            padding: .75rem 1.25rem;
            margin-bottom: 0;
            color: inherit;
            background-color: #fff;
            border-bottom: 1px solid rgba(26,54,126,0.125)
        }
        ::slotted([slot="body"]) {
            flex: 1 1 auto;
            padding: 1.25rem
        }
        `]

}

customElements.define('mx-card', MxCard)