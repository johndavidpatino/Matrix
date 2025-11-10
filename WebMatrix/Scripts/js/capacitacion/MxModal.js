import { LitElement, html, css } from 'https://cdn.jsdelivr.net/gh/lit/dist@2/all/lit-all.min.js';


export class MxModal extends LitElement {
    static styles = [
        css`
            .ip-modal-container {
                display: none;
                justify-content: center;
                align-items: center;
                position: fixed;
                left: 0;
                top: 0;
                width: 100%;
                height: 100%;              
            }
            .ip-modal-container.active{
                display: flex;
            }
            .ip-modal-card{
                min-width: 300px;
                background-color: white;
                display: grid;
                gap: 2rem;
                padding: 1rem;
                border-radius: 4px;
                box-shadow: 0 1.5px 3px 1px rgba(0,0,0,.2);
            }
            
            .alert .ip-modal-card{
                border: 1px solid #EB5757;
            }
            header{
                display: flex;
                justify-content: flex-end;
            }

            .ip-modal-accent-text{
                font-weight: bold;
            }

            .ip-modal-btn {
                border: none;
                border-radius: 4px;
                box-sizing: border-box;
            }
            .ip-modal-btn-confirm, .ip-modal-btn-cancel {
                padding: .5rem;
                min-width: 80px;
            }
            .ip-modal-btn-confirm{
                background-color: #13b0a8;
            }
            .ip-modal-btn-cancel{
                border: 1px solid rgba(0,0,0,.2);
            }
            .ip-modal-container.alert .ip-modal-btn-confirm{
                background-color: #EB5757;
                color: white;
            }
            .alert .ip-modal-btn-confirm:hover{
                opacity: .8;
             }
            .ip-modal-btn-close{
                width: 1.5rem;
                height: 1.5rem;
                display: flex;
                justify-content: center;
                align-items:center;
                border-radius: 1.5rem;
                position: relative;
                font-size: 18px;
                transition: all .3s;
            }
            .ip-modal-btn-close:hover{
                background-color: gray;
                color: white;
            }
            footer{
                display: flex;
                justify-content: flex-end;
            }
            .ip-modal-buttons{
                display: flex;
                gap: .5rem;
            }
        `
    ];

    static get properties() {
        return {
            confirmAction: { type: Function },
            active: {type: Boolean},
            modalType: {type: String}
        };
    }

    constructor(){
        super()
        this.active = false
        this.modalType = 'info'
    }

    render() {
        return html`
            <div class="ip-modal-container${this.active?' active':''} ${this.modalType}">            
                <article class="ip-modal-card">
                    <header>
                        <button class="ip-modal-btn ip-modal-btn-close" @click="${this._onCloseButton}">&times;</button>
                    </header>
                    <div class="ip-modal-content">
                        <slot>
                        </slot>
                    </div>
                    <footer>
                        <div class="ip-modal-buttons">
                            <button class="ip-modal-btn ip-modal-btn-cancel" @click="${this._onCloseButton}">Cancelar</button>
                            <button class="ip-modal-btn ip-modal-btn-confirm" @click="${this._onConfirmButton}">Aceptar</button>
                        </div>
                    </footer>
                </article>
            </div>
        `;
    }

    setActive(active){
        this.active = active
    }

    setConfirmAction(fun) {
        this.confirmAction = fun
    }

    _onCloseButton(){
        this.active = false
    }
    _onConfirmButton(){
        console.log("confirm action")
        this.confirmAction()
    }
}
customElements.define('mx-modal', MxModal);
