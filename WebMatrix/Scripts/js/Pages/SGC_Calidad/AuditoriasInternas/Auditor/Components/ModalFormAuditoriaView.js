import { LitElement, html, css } from '/Scripts/js/libs/lit/lit.js';
import '/Scripts/js/Components/Lit/index.js';
import { resetStyles } from '/Scripts/js/Components/Lit/Styles/font-awesome.js';
import { SGC_AI_Auditor_FormAuditoriaView } from './FormAuditoriaView.js'

export class SGC_AI_Auditor_ModalFormAuditoriaView extends LitElement {
    static properties = {
        Auditoria: { type: Object }
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
                <div class="mx-sgc-ai-auditor-form-auditoria">
                    <mx-sgc-ai-auditor-form-auditoria
                        .Auditoria=${this.Auditoria}
                        @mxClose=${this.handleCloseModalAuditoria}
                    >
                    </mx-sgc-ai-auditor-form-auditoria>
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
        .mx-sgc-ai-auditor-form-auditoria{
            height:400px;
            overflow-y:auto;
        }
    `]
}
customElements.define('mx-sgc-ai-auditor-modal-form-auditoria-view', SGC_AI_Auditor_ModalFormAuditoriaView)