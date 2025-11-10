import { LitElement, html, css } from '/Scripts/js/libs/lit/lit.js';
import '/Scripts/js/Components/Lit/index.js';
import { resetStyles } from '/Scripts/js/Components/Lit/Styles/font-awesome.js';
import { SGC_AI_Auditor_FormFiltroAuditorias } from './Components/FormFiltroAuditorias.js'
import { SGC_AI_Auditor_ModalFormAuditoria } from './Components/ModalFormAuditoria.js'
import { SGC_AI_Auditor_ModalFormAuditoriaApproval } from './Components/ModalFormAuditoriaApproval.js'
import { SGC_AI_Auditor_ModalFormAuditoriaView } from './Components/ModalFormAuditoriaView.js'
import { SGC_AI_ModalFormNuevaAuditoria } from './Components/ModalFormNuevaAuditoria.js'

import { UsuariosService } from '/Scripts/js/Services/UsuariosService.js'

import { SGC_AuditoriasInternasService } from '/Scripts/js/Services/SGC_AuditoriasInternasService.js'

export class SGC_AI_Auditor extends LitElement {

    static properties = {
        Auditorias: { type: Array, state: true },
        Pagination: { type: Object, state: true },
        ColumnsDefinitionTableAuditorias: { type: Array, state: true },
        PaginationDefinitionTableAuditorias: { type: Array, state: true },
        ActionsDefinitionTableAuditorias: { type: Array, state: true },
        PageSizeTableAuditorias: { type: Number, state: true },
        PageIndexTableAuditorias: { type: Number, state: true },
        FilterAnoAuditoriaTableAuditorias: { type: Number, state: true },
        FilterEstadoTableAuditorias: { type: Number, state: true },
        ModalNuevaAuditoriaComponent: { type: Object, state: true },
        ModalInformeAuditoriaComponent: { type: Object, state: true },
        UsuarioTieneRolCalidad: { type: Boolean, state: true },
        AuditoriaStateCreada: { type: Number, state: true },
        AuditoriaStateRequiredApproval: { type: Number, state: true }
    };
    constructor() {
        super();
        this.Auditorias = [];
        this.ColumnsDefinitionTableAuditorias = [];
        this.PaginationDefinitionTableAuditorias = null;
        this.ActionsDefinitionTableAuditorias = [];
        this.FilterAnoAuditoriaTableAuditorias = null;
        this.FilterEstadoTableAuditorias = null;
        this.MostrarModalAuditoria = false;
        this.PageSizeTableAuditorias = 5;
        this.PageIndexTableAuditorias = 1;
        this.UsuarioTieneRolCalidad = false;
        this.AuditoriaStateCreada = 20/*Creada*/
        this.AuditoriaStateRequiredApproval = 30/*Auditoria diligenciada por auditor */
    }
    async firstUpdated() {
        this.ColumnsDefinitionTableAuditorias = this.#columnsDefinitionTableAuditoriasAsignadas();
        this.Auditorias = await SGC_AuditoriasInternasService.AuditoriasBy({
            estadoId: this.FilterEstadoTableAuditorias,
            anoAuditoria: this.FilterAnoAuditoriaTableAuditorias,
            pageIndex: this.PageIndexTableAuditorias,
            pageSize: this.PageSizeTableAuditorias
        });
        this.PaginationDefinitionTableAuditorias = this.#buildPaginationObjectTableAuditorias(this.PageIndexTableAuditorias, this.Auditorias);
        this.ActionsDefinitionTableAuditorias = this.#buildActionObjectTableAuditorias();
        this.UsuarioTieneRolCalidad = await UsuariosService.UsuarioTieneRolCalidad();
    }
    updated(changedProperties) {

    }
    render() {
        return html`
            <mx-card>
                <div slot="header" class="mx-card-header">
                    <mx-sgc-ai-auditor-form-filtro-auditorias
                        @filter-change=${this.handlerChangeFiltroAuditorias}
                    >
                    </mx-sgc-ai-auditor-form-filtro-auditorias>
                    ${this.UsuarioTieneRolCalidad ? html`<mx-button @mxClick=${this.handleClickNuevaAuditoria}>Nueva auditoria</mx-button>` : ''}
                </div>
                <div slot="body">
                    <mx-table
                        .data=${this.Auditorias}
                        .columns=${this.ColumnsDefinitionTableAuditorias}
                        idData="Id"
                        .pagination=${this.PaginationDefinitionTableAuditorias}
                        .actions=${this.ActionsDefinitionTableAuditorias}
                        @page-change=${this.handleChangePageTableAuditorias}
                    >
                    </mx-table>
                </div>
            </mx-card>
        `;
    }
    #columnsDefinitionTableAuditoriasAsignadas() {
        return [
            { name: "AreaAuditada", displayName: "AreaAuditada" },
            { name: "ProcesoAuditado", displayName: "ProcesoAuditado" },
            { name: "FechaLimiteAuditoria", displayName: "FechaLimiteAuditoria" },
            { name: "SGC_AI_EstadoAuditoria", displayName: "EstadoAuditoria" }
        ]
    }
    async handleChangePageTableAuditorias(page) {
        this.PageIndexTableAuditorias = page.detail.page;
        this.#LoadTableAuditorias(this.FilterAnoAuditoriaTableAuditorias, this.FilterEstadoTableAuditorias, this.PageIndexTableAuditorias, this.PageSizeTableAuditorias);

    }
    #buildPaginationObjectTableAuditorias(pageNumber, auditorias) {
        const totalRows = auditorias.length == 0 ? 0 : auditorias[0].TotalRows;
        const totalPages = Math.ceil(totalRows / this.PageSizeTableAuditorias);
        return {
            pageNumber: pageNumber,
            pageSize: this.PageSizeTableAuditorias,
            totalCount: totalRows,
            totalPages: totalPages,
            nextPage: totalPages > pageNumber ? pageNumber + 1 : null,
            previousPage: pageNumber > 1 ? pageNumber - 1 : null
        }
    }
    #buildActionObjectTableAuditorias() {
        return [
            { name: 'Ver', icon: 'folder-open', iconColor: '#6c757d', fun: (e) => this.#DisplayModalInformeAuditoria(e), isEditAction: true }
        ]
    }
    async handlerChangeFiltroAuditorias(filtros) {
        this.FilterAnoAuditoriaTableAuditorias = filtros.detail.anoAuditoria == "" ? null : filtros.detail.anoAuditoria;
        this.FilterEstadoTableAuditorias = filtros.detail.estadoAuditoria == "" ? null : filtros.detail.estadoAuditoria;
        this.PageIndexTableAuditorias = 1;
        this.#LoadTableAuditorias(this.FilterAnoAuditoriaTableAuditorias, this.FilterEstadoTableAuditorias, this.PageIndexTableAuditorias, this.PageSizeTableAuditorias);
    }
    async #LoadTableAuditorias(anoAuditoria, estadoId, pageIndex, pageSize) {
        this.Auditorias = await SGC_AuditoriasInternasService.AuditoriasBy({
            estadoId: estadoId,
            anoAuditoria: anoAuditoria,
            pageIndex: pageIndex,
            pageSize: pageSize
        });
        this.PaginationDefinitionTableAuditorias = this.#buildPaginationObjectTableAuditorias(pageIndex, this.Auditorias);
    }
    handleCloseModalInformeAuditoria = () => {
        this.ModalInformeAuditoriaComponent.remove();
        this.#LoadTableAuditorias(this.FilterAnoAuditoriaTableAuditorias, this.FilterEstadoTableAuditorias, this.PageIndexTableAuditorias, this.PageSizeTableAuditorias);
    }
    handleClickNuevaAuditoria() {
        this.#DisplayModalNuevaAuditoria();
    }
    handleCloseModalNuevaAuditoria = () => {
        this.ModalNuevaAuditoriaComponent.remove();
        this.#LoadTableAuditorias(this.FilterAnoAuditoriaTableAuditorias, this.FilterEstadoTableAuditorias, this.PageIndexTableAuditorias, this.PageSizeTableAuditorias);
    }
    #DisplayModalNuevaAuditoria() {
        this.ModalNuevaAuditoriaComponent = document.createElement("mx-sgc-ai-modal-form-nueva-auditoria");
        this.ModalNuevaAuditoriaComponent.addEventListener("mxClose", this.handleCloseModalNuevaAuditoria);
        document.body.appendChild(this.ModalNuevaAuditoriaComponent)
    }
    async #DisplayModalInformeAuditoria(row) {

        const informeAuditoria = await SGC_AuditoriasInternasService.AuditoriaInformeAuditorBy({ auditoriaId: row.Id });

        switch (row.SGC_AI_EstadoId) {
            case 20:/*Creada*/
                this.ModalInformeAuditoriaComponent = document.createElement("mx-sgc-ai-auditor-modal-form-auditoria");
                this.ModalInformeAuditoriaComponent.Auditoria = row;
                this.ModalInformeAuditoriaComponent.addEventListener("mxClose", this.handleCloseModalInformeAuditoria);
                document.body.appendChild(this.ModalInformeAuditoriaComponent)
                break;
            case 30:/*Approval*/
                this.ModalInformeAuditoriaComponent = document.createElement("mx-sgc-ai-auditor-modal-form-auditoria-approval");
                this.ModalInformeAuditoriaComponent.Auditoria = informeAuditoria;
                this.ModalInformeAuditoriaComponent.addEventListener("mxClose", this.handleCloseModalInformeAuditoria);
                document.body.appendChild(this.ModalInformeAuditoriaComponent)
                break;
            default:
                this.ModalInformeAuditoriaComponent = document.createElement("mx-sgc-ai-auditor-modal-form-auditoria-view");
                this.ModalInformeAuditoriaComponent.Auditoria = row;
                this.ModalInformeAuditoriaComponent.addEventListener("mxClose", this.handleCloseModalInformeAuditoria);
                document.body.appendChild(this.ModalInformeAuditoriaComponent);
        }


    }
    static styles = [resetStyles, css`
        .mx-card-header {
            display:flex;
            align-items: end;
            justify-content: space-between;
            padding: 10px;
        }
    `]
}
customElements.define('mx-sgc-ai-auditor', SGC_AI_Auditor)