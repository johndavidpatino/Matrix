import { LitElement, html, css } from '/Scripts/js/libs/lit/lit.js'

export class MxButton extends LitElement {
    static properties = {
        disabled: { type: Boolean },
        type: { type: String },
        variant: { type: String },
        isLoading: { type: Boolean },
        icon: { type: String }
    }

    constructor(){
        super()
        this.disabled = false
        this.type = 'button'
        this.variant = 'primary' // primary, secondary, danger, warning, success, info, outline
        this.isLoading = false
        this.icon = null
    }
    
    render(){
        return html`
            <button class="mx-button ${this.variant}" ?disabled=${this.disabled || this.isLoading} type=${this.type} @click=${this.handleClick}>
                <span class="mx-button-label">
                    <slot></slot>
                </span>
                ${this.icon ? html`<i class="mx-button-icon fa fa-${this.icon}"></i>` : ''}
                ${this.isLoading ? html`<span class="mx-button-loader"></span>` : ''}
            </button>
        `
    }
    
    handleClick(event) {
        event.preventDefault()
        if(this.isLoading || this.disabled) return
        this.dispatchEvent(new CustomEvent('mxClick', event))
    }

    setLoading(active) {
        this.isLoading = active
        this.disabled = active
        this.requestUpdate()
    }

    static styles = css`
        .mx-button{
            --bg-color: var(--primary);
            --text-color: var(--white);
            background-color: var(--bg-color);
            color: var(--text-color);
            border: none;
            padding-inline: 1rem;
            height: 40px;
            border-radius: 5px;
            cursor: pointer;
            display: flex;
            align-items: center;
            justify-content: center;
        }
        .mx-button:hover{
            opacity: 0.9;
        }
        .mx-button:active{
            opacity: 0.8;
        }
        .mx-button:disabled{
            opacity: 0.5;
            cursor: not-allowed;
            pointer-events: none;
        }
        .mx-button-loader{
            display: inline-block;
            width: 10px;
            height: 10px;
            border: 2px solid rgba(255, 255, 255, 0.6);
            border-radius: 50%;
            border-top-color: var(--white);
            animation: spin 1s linear infinite;
            margin-left: 10px;
        }
        @keyframes spin {
            to {
                transform: rotate(360deg);
            }
        }
        .mx-button.primary{
            --bg-color: var(--primary);
            --text-color: var(--white);
        }
        .mx-button.secondary{
            --bg-color: var(--secondary);
            --text-color: var(--white);
        }
        .mx-button.danger{
            --bg-color: var(--danger);
            --text-color: var(--white);
        }
        .mx-button.warning{
            --bg-color: var(--warning);
            --text-color: var(--white);
        }
        .mx-button.success{
            --bg-color: var(--success);
            --text-color: var(--white);
        }
        .mx-button.info{
            --bg-color: var(--info);
            --text-color: var(--white);
        }
        .mx-button.outline{
            --bg-color: transparent;
            --text-color: var(--primary);
            border: 1px solid var(--primary);
        }
        `
}

customElements.define('mx-button', MxButton)