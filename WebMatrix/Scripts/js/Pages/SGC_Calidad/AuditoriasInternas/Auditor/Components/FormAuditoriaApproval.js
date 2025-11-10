import { LitElement, html, css } from '/Scripts/js/libs/lit/lit.js';
import '/Scripts/js/Components/Lit/index.js';
import { resetStyles } from '/Scripts/js/Components/Lit/Styles/font-awesome.js';
import { SGC_AI_FormHallazgos } from './FormHallazgos.js'
import { EmpleadosService } from '/Scripts/js/Services/EmpleadosService.js'
import { SGC_AuditoriasInternasService } from '/Scripts/js/Services/SGC_AuditoriasInternasService.js'
import { HelperService } from '/Scripts/js/Services/HelperService.js'

export class SGC_AI_Auditor_FormAuditoriaApproval extends LitElement {
    static properties = {
        Auditoria: { type: Object },
        EmpleadosActivos: { type: Array, state: true },
        Hallazgos: { type: Array }
    };
    constructor() {
        super();
        this.Auditoria = null;
        this.EmpleadosActivos = [];
        this.Hallazgos = [];
    }
    async firstUpdated() {
        this.Hallazgos = this.Auditoria.Hallazgos.map(x => ({ Id: x.Id, TipoId: x.TipoHallazgoId, Tipo: x.TipoHallazgo, Hallazgo: x.Hallazgo }));
    }
    updated(changedProperties) {
        
    }
    render() {
        return html`
            <mx-form id="sgc-ai-auditor-form-auditoria">
                        <mx-card>
                            <div slot="body" class="sgc-ai-auditor-form-auditoria">
                                <h2>Informe preliminar de auditoria:</h2>
                                <h4>Auditados:</h4>
                                <p>${this.Auditoria.Auditados.map(x => x.Nombres + x.Apellidos).join(",")}</p>
                                <h4>Archivo de evidencia:<h4>
                                <a href="${this.Auditoria.PathFileEvidencia}">Descargar</a>
                                <h4>Fecha auditoria:</h4>
                                <p>${HelperService.FromDateASPNetJsonFormatToDate(this.Auditoria.FechaAuditoria).toLocaleDateString()}</p>
                                <h4>Fortalezas:</h4>
                                <p>${this.Auditoria.Fortalezas}</p>
                                <mx-sgc-ai-form-hallazgos
                                    .Hallazgos=${this.Hallazgos}
                                >
                                </mx-sgc-ai-form-hallazgos>
                                <mx-button
                                    type="submit"
                                    @mxClick=${this.HandleBtnClick}
                                    class="mx-button"
                                >
                                    <span>
                                        Aprobar
                                    </span>
                                </mx-button>
                            </div>
                         </mx-card>
            </mx-form>
        `;
    }
    async HandleBtnClick(event) {
        if (this.isValid()) {
            btnSave.isLoading = true;



            btnSave.isLoading = false;
            this.dispatchEvent(new CustomEvent('mxClose'), event);
        }
    }
    isValid() {
        return true;
    }
    get _control() {
        return this.renderRoot.querySelector("#sgc-ai-auditor-form-auditoria");
    }
    static styles = [resetStyles, css`
        .sgc-ai-auditor-form-auditoria{
            display:flex;
            column-gap:10px;
            flex-direction:column;
            justify-content: center;
            row-gap: 10px;
            padding:10px;
        }
        .mx-button{
            display:flex;
            justify-content: end;
        }
        .mx-card{
            width:500px;
            padding:10px;
        }
    `]
}
customElements.define('mx-sgc-ai-auditor-form-auditoria-approval', SGC_AI_Auditor_FormAuditoriaApproval)