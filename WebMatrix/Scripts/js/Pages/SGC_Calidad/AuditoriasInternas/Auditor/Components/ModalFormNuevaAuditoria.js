import { LitElement, html, css } from '/Scripts/js/libs/lit/lit.js';
import '/Scripts/js/Components/Lit/index.js';
import { resetStyles } from '/Scripts/js/Components/Lit/Styles/font-awesome.js';
import { SGC_AI_FormNuevaAuditoria } from './FormNuevaAuditoria.js'

export class SGC_AI_ModalFormNuevaAuditoria extends LitElement {
    static properties = {
    };
    constructor() {
        super();
        this.Auditoria = null;
    }
    firstUpdated() {
        document.body.appendChild(this);
    }
    updated(changedProperties) {
        
    }
    render() {
        return html`
            <mx-modal-container
            @mxClose=${this.handleCloseModalAuditoria}
            >
                <div class="mx-modal-content">
                    <mx-card class="mx-card">
                        <mx-sgc-ai-form-nueva-auditoria slot="body"
                            @mxSave=${this.handleCloseModalAuditoria}
                        >
                        </mx-sgc-ai-form-nueva-auditoria>
                    </mx-card>
                </div>
            </mx-modal-container>
        `;
    }
    get _control() {
        return this.renderRoot.querySelector("#sgc-ai-auditor-form-auditoria");
    }
    handleCloseModalAuditoria(event) {
        this.dispatchEvent(new CustomEvent('mxClose'), event);
    }
    static styles = [resetStyles, css`
        .mx-modal-content{
            background-color:white;
            height:400px;
            overflow-y:auto;
            width:500px;
            padding:10px;
        }
        .mx-card{
            width:500px;
            padding:10px;
        }
    `]
}
customElements.define('mx-sgc-ai-modal-form-nueva-auditoria', SGC_AI_ModalFormNuevaAuditoria)