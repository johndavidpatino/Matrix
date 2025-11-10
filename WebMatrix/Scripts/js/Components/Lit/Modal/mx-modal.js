import { LitElement, html, css } from '/Scripts/js/libs/lit/lit.js'


export class MxModal extends LitElement {

    static properties = {
        title: {type: String},
        content: {type: String},
        confirmAction: {type: Function},
        cancelAction: {type: Function},
        confirmButtonText: {type: String},
        cancelButtonText: {type: String},
        type: {type: String}
    }

    constructor(){
        super()
        this.title = 'Ejemplo titulo'
        this.content = 'lorem ipsum dolor sit amet'
        this.confirmAction = null
        this.cancelAction = null
        this.confirmButtonText = 'Confirmar'
        this.cancelButtonText = 'Cancelar'
        this.type = 'confirm' // confirm, alert, danger, outline
    }

    render() {
        return html`
        <div class="mx-modal__overlay" @mousedown="${this.handleClickOutside}">
            <div class="mx-modal mx-modal--${this.type}">
                <div class="mx-modal__content">
                    <h2 class="mx-modal__title">${this.title}</h2>
                    <p class="mx-modal__text">${this.content}</p>
                </div>
                <div class="mx-modal__actions">
                    <mx-button variant="outline" @click="${this.handleCancelAction}">${this.cancelButtonText}</mx-button>
                    <mx-button @click="${this.handleConfirmAction}">${this.confirmButtonText}</mx-button>
                </div>
            </div>
        </div>
        `
    }

    firstUpdated(){
        document.body.appendChild(this)
    }

    //handle click outside
    async handleClickOutside(event){
        let target = event.target
        if(target.classList.contains('mx-modal__overlay')){
            await this.cancelAction()
            this.remove()
        }
    }

    async handleCancelAction(){
        await this.cancelAction()
        this.remove()
    }

    async handleConfirmAction(event){
        let target = event.target
        target.setLoading(true)
        try{
            await this.confirmAction()
            target.setLoading(false)
        }catch(error){
            target.setLoading(false)
        }
        this.remove()
    }

    static styles = css`
        .mx-modal__overlay{
            position: fixed;
            top: 0;
            left: 0;
            width: 100%;
            height: 100%;
            background-color: rgba(0, 0, 0, 0.5);
            z-index: 100;
        }
        .mx-modal {
            position: fixed;
            top: 50%;
            left: 50%;
            transform: translate(-50%, -50%);
            background-color: white;
            padding: 2rem;
            min-width: 300px;
            box-shadow: 0 0 10px rgba(0, 0, 0, 0.1);
            border-radius: 0.5rem;
            z-index: 100;
        }
        .mx-modal__actions {
            display: flex;
            justify-content: space-between;
            gap: 1rem;
            margin-top: 1rem;
            border-top: 1px solid #e0e0e0;
            padding-top: 1rem;
        }

        `   
}

customElements.define('mx-modal', MxModal)

