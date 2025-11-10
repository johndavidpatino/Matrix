import { LitElement, html, css } from '/Scripts/js/libs/lit/lit.js';
import '/Scripts/js/Components/Lit/index.js';
import { resetStyles } from '/Scripts/js/Components/Lit/Styles/font-awesome.js';
import { SGC_AI_FormHallazgos } from './FormHallazgos.js'
import { EmpleadosService } from '/Scripts/js/Services/EmpleadosService.js'
import { SGC_AuditoriasInternasService } from '/Scripts/js/Services/SGC_AuditoriasInternasService.js'
import { HelperService } from '/Scripts/js/Services/HelperService.js'

export class SGC_AI_Auditor_FormAuditoriaView extends LitElement {
    static properties = {
        Auditoria: { type: Object },
        EmpleadosActivos: { type: Array, state: true },
    };
    constructor() {
        super();
        this.Auditoria = null;
        this.EmpleadosActivos = [];
    }
    async firstUpdated() {
        const empleadosActivos = await EmpleadosService.EmpleadosActivos();
        this.EmpleadosActivos = empleadosActivos.map(x => ({ label: x.NombreCompleto, value: x.Id }));
    }
    render() {
        return html`
            <mx-form id="sgc-ai-auditor-form-auditoria">
                        <mx-card>
                            <div slot="body" class="sgc-ai-auditor-form-auditoria">
                                <h2>Informe preliminar de auditoria:</h2>
                                <mx-multi-select
                                    HasEmptyOption=${true}
                                    .required=${true}
                                    .options=${this.EmpleadosActivos}
                                >
                                    Auditados:
                                </mx-multi-select>
                                <mx-loader-file
                                    .formatsAllowed=${[{ fileExtension: ".xlsx", mimeType: "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet" }]}
                                >
                                    Archivo de evidencia:
                                </mx-loader-file>
                                <mx-field-text
                                    required=${true}
                                    type="date"
                                >
                                    Fecha auditoria:
                                </mx-field-text>
                                <mx-text-area
                                    required=${true}
                                >
                                    <div>
                                        <h3>Fortalezas</h3>
                                        <p><strong>Cualidades Destacables:</strong> Todas aquellas características que pueden servir de ejemplo para otras áreas de la empresa.</p>
                                    </div>
                                </mx-text-area>
                                <mx-sgc-ai-form-hallazgos>
                                </mx-sgc-ai-form-hallazgos>
                                <mx-button
                                    type="submit"
                                    @mxClick=${this.HandleBtnClick}
                                    class="mx-button"
                                >
                                    <span>
                                        Guardar
                                    </span>
                                </mx-button>
                            </div>
                         </mx-card>
            </mx-form>
        `;
    }
    async HandleBtnClick(event) {
        if (this.isValid()) {
            const auditados = this.renderRoot.querySelector("mx-multi-select");
            const archivoEvidencia = this.renderRoot.querySelector("mx-loader-file");
            const fortalezas = this.renderRoot.querySelector("mx-text-area");
            const fechaAuditoria = this.renderRoot.querySelector("mx-field-text");
            const hallazgos = this.renderRoot.querySelector("mx-sgc-ai-form-hallazgos");
            const btnSave = this.renderRoot.querySelector("mx-button")

            btnSave.isLoading = true;

            await SGC_AuditoriasInternasService.AuditoriaInformeAuditor(
                {
                    auditoriaId: this.Auditoria.Id,
                    fechaAuditoria: fechaAuditoria.value,
                    fortalezas: fortalezas.value,
                    auditados: auditados.value.map(x => ({ AuditadoId: x.value })),
                    hallazgos: hallazgos.value.map(x => ({ Hallazgo: x.Hallazgo, TipoId: x.TipoId })),
                    fileEvidenciaBase64: await HelperService.FormatExcelFileToBase64Async(archivoEvidencia.file()),
                    fileEvidenciaNameConExtension: archivoEvidencia.file().name
                }
            )
            btnSave.isLoading = false;
            this.dispatchEvent(new CustomEvent('mxClose'), event);
        }
    }
    isValid() {
        const auditados = this.renderRoot.querySelector("mx-multi-select");
        const archivoEvidencia = this.renderRoot.querySelector("mx-loader-file");
        const fortalezas = this.renderRoot.querySelector("mx-text-area");
        const fechaAuditoria = this.renderRoot.querySelector("mx-field-text");
        const hallazgos = this.renderRoot.querySelector("mx-sgc-ai-form-hallazgos");

        return auditados.IsValid() && archivoEvidencia.IsValid() && fechaAuditoria.IsValid() && fortalezas.IsValid() && hallazgos.IsValid();
    }
    get _control() {
        return this.renderRoot.querySelector("#sgc-ai-auditor-form-auditoria");
    }
    static styles = [css`
        .sgc-ai-auditor-form-auditoria{
            display:flex;
            column-gap:10px;
            flex-direction:column;
            justify-content: center;
            row-gap: 10px;
        }
        .mx-button{
            display:flex;
            justify-content: end;
        }
    `]
}
customElements.define('mx-sgc-ai-auditor-form-auditoria-view', SGC_AI_Auditor_FormAuditoriaView)