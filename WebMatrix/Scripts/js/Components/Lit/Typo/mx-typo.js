import { LitElement, html, css } from '/Scripts/js/libs/lit/lit.js';

class MxTypo extends LitElement {
    static properties = {
        type: { type: String }, // small, detail, body, subtitle, title, main, display
        tag: { type: String },
        weight: { type: Number },
        color: { type: String }
    }
    constructor(){
        super()
        this.tag = 'p'
        this.type = 'body'
        this.weight = 400
        this.color = '#000'
    }
    render(){
        switch(this.tag){
            case 'span':
                return html`<span class="mx-typo mx-typo--${this.type}" style="--weight: ${this.weight}; --color: ${this.color};">
                    <slot></slot>
                </span>`
            case 'p':
                return html`<p class="mx-typo mx-typo--${this.type}" style="--weight: ${this.weight}; --color: ${this.color};">
                    <slot></slot>
                </p>`
            case 'h1':
                return html`<h1 class="mx-typo mx-typo--${this.type}" style="--weight: ${this.weight}; --color: ${this.color};">
                    <slot></slot>
                </h1>`
            case 'h2':
                return html`<h2 class="mx-typo mx-typo--${this.type}" style="--weight: ${this.weight}; --color: ${this.color};">
                    <slot></slot>
                </h2>`
            case 'h3':
                return html`<h3 class="mx-typo mx-typo--${this.type}" style="--weight: ${this.weight}; --color: ${this.color};">
                    <slot></slot>
                </h3>`
            case 'h4':
                return html`<h4 class="mx-typo mx-typo--${this.type}" style="--weight: ${this.weight}; --color: ${this.color};">
                    <slot></slot>
                </h4>`
            case 'h5':
                return html`<h5 class="mx-typo mx-typo--${this.type}" style="--weight: ${this.weight}; --color: ${this.color};">
                    <slot></slot>
                </h5>`
            case 'h6':
                return html`<h6 class="mx-typo mx-typo--${this.type}" style="--weight: ${this.weight}; --color: ${this.color};" >
                    <slot></slot>
                </h6>`
            default:
                return html`<p class="mx-typo mx-typo--${this.type}" style="--weight: ${this.weight}; --color: ${this.color};">
                    <slot></slot>
                </p>`
        }
    }



    static styles = css`
        :host{
            --weight: 300;
            --color: #495057;
            box-sizing: border-box;
        }
        .mx-typo{
            font-weight: var(--weight);
            color: var(--color);
        }
        .mx-typo--small{
            font-size: 12px;
        }
        .mx-typo--detail{
            font-size: 14px;
        }
        .mx-typo--body{
            font-size: 16px;
        }
        .mx-typo--subtitle{
            font-size: 18px;
        }
        .mx-typo--title{
            font-size: 24px;
        }
        .mx-typo--main{
            font-size: 32px;
        }
        .mx-typo--display{
            font-size: 60px;
        }
    `;
}

customElements.define('mx-typo', MxTypo);