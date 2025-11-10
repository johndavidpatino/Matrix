import { LitElement, html, css } from '/Scripts/js/libs/lit/lit.js';
import '/Scripts/js/Components/Lit/index.js';
import { resetStyles } from '/Scripts/js/Components/Lit/Styles/font-awesome.js';

export class SGC_AI_Auditor_FormFiltroAuditorias extends LitElement {
    static properties = {
        AnosAuditorias: { type: Array, state: true },
        EstadosAuditorias: { type: Array, state: true },
    };
    constructor() {
        super();
        this.AnosAuditorias = [];
        this.EstadosAuditorias = [];
    }
    async firstUpdated() {
        this.AnosAuditorias = [{ label: "2024", value: 2024 }];
        this.EstadosAuditorias = [{ label: "Creada", value: 20 }, { label: "Asignada auditor", value: 30 }, { label: "Auditor Diligenciada", value: 40 }];
    }
    render() {
        return html`
            <mx-form id="sgc-ai-auditor-form-filtro-auditorias">
                <div class="sgc-ai-auditor-form-filtro-auditorias">
                    <mx-select
                        name="AnoAuditoria"
                        .options=${this.AnosAuditorias} 
                        HasEmptyOption=true
                    >
                        <span>
                            Año
                        </span>
                    </mx-select>
                    <mx-select
                        name="EstadoAuditoria"
                        .options=${this.EstadosAuditorias}
                        HasEmptyOption=true
                    >
                        <span>
                            Estado
                        </span>
                    </mx-select>
                    <mx-button
                        type="submit"
                        @mxClick=${this.HandleBtnClick}
                    >
                        <span>
                            Buscar
                        </span>
                    </mx-button>
                </div>
            </mx-form>
        `;
    }
    async HandleBtnClick() {
        if (this._control.isValid()) {
            const anoAuditoria = this.renderRoot.querySelector("mx-select[name='AnoAuditoria']").value;
            const estadoAuditoria = this.renderRoot.querySelector("mx-select[name='EstadoAuditoria']").value;
            const event = new CustomEvent('filter-change', {
                detail: { anoAuditoria, estadoAuditoria },
                bubbles: true,
                composed: true
            });
            this.dispatchEvent(event);
        }
    }
    get _control() {
        return this.renderRoot.querySelector("#sgc-ai-auditor-form-filtro-auditorias");
    }
    static styles = [resetStyles, css`
        .sgc-ai-auditor-form-filtro-auditorias{
            display:flex;
            column-gap:10px;
            flex-direction:row;
            justify-content: center;
            align-items: end;
}
        }
    `]
}
customElements.define('mx-sgc-ai-auditor-form-filtro-auditorias', SGC_AI_Auditor_FormFiltroAuditorias)