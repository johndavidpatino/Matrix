import { LitElement, html, css } from '/Scripts/js/libs/lit/lit.js';
import '/Scripts/js/Components/Lit/index.js';
import { resetStyles } from '/Scripts/js/Components/Lit/Styles/font-awesome.js';
import { SGC_AI_FormNuevaAuditoria } from '/Scripts/js/Pages/SGC_Calidad/AuditoriasInternas/NuevaAuditoria/Components/FormNuevaAuditoria.js';

export class SGC_AI_NuevaAuditoria extends LitElement {
    static properties = {
    };
    constructor() {
        super();
    }
    async firstUpdated() {
    }
    render() {
        return html`
            <mx-card>
                <div slot="body">
                    <mx-sgc-ai-form-nueva-auditoria>
                    </mx-sgc-ai-form-nueva-auditoria>
                </div>
            </mx-card>
        `;
    }
    static styles = [resetStyles, css`
        
    `]
}
customElements.define('mx-sgc-ai-nueva-auditoria', SGC_AI_NuevaAuditoria)