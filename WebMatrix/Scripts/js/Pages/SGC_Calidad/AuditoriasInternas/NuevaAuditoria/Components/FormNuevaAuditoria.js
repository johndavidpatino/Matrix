import { LitElement, html, css } from '/Scripts/js/libs/lit/lit.js';
import '/Scripts/js/Components/Lit/index.js';
import { resetStyles } from '/Scripts/js/Components/Lit/Styles/font-awesome.js';
import { UsuariosService } from '/Scripts/js/Services/UsuariosService.js'
import { SGC_AuditoriasInternasService } from '/Scripts/js/Services/SGC_AuditoriasInternasService.js'

export class SGC_AI_FormNuevaAuditoria extends LitElement {
    static properties = {
        Auditores: { type: Array, state: true },
        Normativas: { type: Array, state: true },
        TiposAuditoria: { type: Array, state: true }
    };
    constructor() {
        super();
        this.Auditores = [];
        this.Normativas = [];
        this.TiposAuditoria = [];
    }
    async firstUpdated() {
        const Rol_SGC_AI_AuditorInterno = 57;
        const auditores = await UsuariosService.UsuariosXRol(Rol_SGC_AI_AuditorInterno);
        this.Auditores = auditores.map(x => ({ label: x.Nombres + " " + x.Apellidos, value: x.id }));
        this.Normativas = [{ label: "ISO 9001", value: 1 }, { label: "ISO 20252", value: 2 }];
        this.TiposAuditoria = [{ label: "Campo", value: 1 }, { label: "Documental", value: 2 }];
    }
    render() {
        return html`
            <mx-form id="mx-form-nueva-auditoria">
                <div class="mx-form-nueva-auditoria">
                    <mx-select
                        name="Auditores"
                        .options=${this.Auditores} 
                        required=true
                        HasEmptyOption=true
                    >
                        <span>
                            Auditor
                        </span>
                    </mx-select>
                    <mx-field-text
                        type="text"
                        name="txtAreaAuditada"
                        required=true
                    >
                        <span>
                            Area auditada
                        </span>
                    </mx-field-text>
                    <mx-field-text
                        type="text"
                        name="txtProcesoAuditado"
                        required=true
                    >
                        <span>
                            Proceso auditado
                        </span>
                    </mx-field-text>
                    <mx-field-text
                        type="date"
                        name="txtFechaLimiteAuditoria"
                        required=true
                    >
                        <span>
                            Fecha limite auditoria
                        </span>
                    </mx-field-text>
                    <mx-checkbox-list
                        name="cblEstandaresAuditar"
                        required=true
                        .options=${this.Normativas}
                    >
                    <span>
                        Estándar a auditar
                    </span>
                    </mx-checkbox-list>
                    <mx-checkbox-list
                        name="cblTipoAuditoria"
                        required=true
                        .options=${this.TiposAuditoria}
                    >
                    <span>
                        Tipo de auditoria
                    </span>
                    </mx-checkbox-list>
                    <mx-button
                        type="submit"
                        @mxClick=${this.HandleBtnClick}
                    >
                        <span>
                            Guardar
                        </span>
                    </mx-button>
                </div>
            </mx-form>
        `;
    }
    async HandleBtnClick() {
        if (this._control.isValid()) {
            const auditor = this.renderRoot.querySelector("mx-select[name='Auditores']");
            const areaAuditada = this.renderRoot.querySelector("mx-field-text[name='txtAreaAuditada']");
            const procesoAuditado = this.renderRoot.querySelector("mx-field-text[name='txtProcesoAuditado']");
            const fechaLimiteAuditoria = this.renderRoot.querySelector("mx-field-text[name='txtFechaLimiteAuditoria']");
            const tiposAuditoria = this.renderRoot.querySelector("mx-checkbox-list[name='cblTipoAuditoria']");
            const normativasAAuditar = this.renderRoot.querySelector("mx-checkbox-list[name='cblEstandaresAuditar']");
            const btnSave = this.renderRoot.querySelector("mx-button")

            btnSave.isLoading = true;

            await SGC_AuditoriasInternasService.Nueva(
                {
                    auditoriId: auditor.value,
                    areaAuditada: areaAuditada.value,
                    procesoAuditado: procesoAuditado.value,
                    fechaLimiteAuditoria: fechaLimiteAuditoria.value,
                    tiposAuditoria: tiposAuditoria.value,
                    normativasAAuditar: normativasAAuditar.value
                }
            );
            this._control.clear();
            btnSave.isLoading = false;
        }
    }
    get _control() {
        return this.renderRoot.querySelector("#mx-form-nueva-auditoria");
    }
    static styles = [resetStyles, css`
        .mx-form-nueva-auditoria{
            display:flex;
            row-gap:10px;
            flex-direction:column;
        }
    `]
}
customElements.define('mx-sgc-ai-form-nueva-auditoria', SGC_AI_FormNuevaAuditoria)