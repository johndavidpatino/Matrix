import { LitElement, html, css } from '/Scripts/js/libs/lit/lit.js';
import '/Scripts/js/Components/Lit/index.js';
import { resetStyles } from '/Scripts/js/Components/Lit/Styles/font-awesome.js';

export class SGC_AI_FormHallazgos extends LitElement {
    static properties = {
        TiposHallazgos: { type: Array },
        Hallazgos: { type: Array },
        ColumnsDefinitionTableHallazgos: { type: Array },
        Editable: { type: Boolean },
        PageSizeTableHallazgos: { type: Number, state: true },
        PageIndexTableHallazgos: { type: Number, state: true },
        ActionsDefinitionTableHallazgos: { type: Array, state: true },
        PaginationDefinitionTableHallazgos: { type: Array, state: true },
        HallazgosActual: { type: Array, state: true },
        HallazgosPaginaActual: { type: Array, state: true },
    };
    constructor() {
        super();
        this.TiposHallazgos = [
            { label: "No Conformidad", value: 1 },
            { label: "Observación", value: 2 },
            { label: "Recomendación", value: 3 },
            { label: "No Aplica para el proceso auditado", value: 4 },
            { label: "Norma ISO 9001", value: 5 },
            { label: "Norma ISO 20252", value: 6 }
        ];
        this.Hallazgos = [];
        this.HallazgosActual = [];
        this.HallazgosPaginaActual = [];
        this.ColumnsDefinitionTableHallazgos = this.#buildColumnsDefinitionTableHallazgos();
        this.PageSizeTableHallazgos = 3;
        this.PageIndexTableHallazgos = 1;
        this.ActionsDefinitionTableHallazgos = [];
        this.Editable = false;
    }
    async firstUpdated() {

    }
    updated(changedProperties) {
        if (changedProperties.has("Hallazgos")) {
            this.PaginationDefinitionTableHallazgos = this.#buildPaginationObjectTableHallazgos(this.PageIndexTableHallazgos, this.Hallazgos);
            this.HallazgosActual = this.Hallazgos.map((x, index) => { x["IdConsecutivo"] = index; return x; });
            this.#actualizarHallazgosPaginaActual();
        }
        if (changedProperties.has("Editable")) {
            this.ActionsDefinitionTableHallazgos = this.#buildActionObjectTableHallazgos();
        }
    }
    render() {
        return html`
                    <div class="sgc-ai-info-hallazgos">
                        <h3>Hallazgos:</h3>
                        <p><strong>Observación:</strong> Son fallas puntuales sin mayor trascendencia pero que conviene controlar para que no se repita</p>
                        <p><strong>No Conformidad:</strong> Son incumplimientos que demuestran la falla del sistema. Puede tratarse de una observación que se repite</p>
                    </div>
                    <div>
                        ${this.Editable ? html`
                            <div class="sgc-ai-form-hallazgos">
                                <mx-text-area
                                    .resizable=${true}
                                    .required=${true}
                                >
                                    Hallazgo
                                </mx-text-area>
                                <mx-select
                                    name="TipoHallazgo"
                                    .options=${this.TiposHallazgos}
                                    .required=${true}
                                    HasEmptyOption=true
                                >
                                    <span>
                                        Tipo
                                    </span>
                                </mx-select>
                                <mx-button
                                    type="submit"
                                    @mxClick=${this.handleBtnClick}
                                >
                                    <span>
                                        Agregar
                                    </span>
                                </mx-button>
                            </div>
                            `
                : ''}
                            <mx-table
                                .data=${this.HallazgosPaginaActual}
                                .columns=${this.ColumnsDefinitionTableHallazgos}
                                .pagination=${this.PaginationDefinitionTableHallazgos}
                                .actions=${this.ActionsDefinitionTableHallazgos}
                                @page-change=${this.handlerPageChangeTableHallazgos}
                            >
                            </mx-table>
                    </div>
        `;
    }
    get _control() {
        return this.renderRoot.querySelector("#sgc-ai-auditor-form-auditoria");
    }
    handleBtnClick() {
        if (!this.formIsValid()) {
            return;
        }
        const hallazgo = this.renderRoot.querySelector("mx-text-area");
        const tipo = this.renderRoot.querySelector("mx-select");
        const newHallazgo = {
            Hallazgo: hallazgo.value,
            TipoId: tipo.value,
            Tipo: this.TiposHallazgos.find(x => x.value == tipo.value).label,
            Id: null,
            IdConsecutivo: this.HallazgosActual.length + 1
        };
        this.HallazgosActual = [...this.HallazgosActual, newHallazgo];
        hallazgo.clear();
        tipo.clear();
        this.#actualizarHallazgosPaginaActual();
        this.PaginationDefinitionTableHallazgos = this.#buildPaginationObjectTableHallazgos(this.PageIndexTableHallazgos, this.HallazgosActual);
    }
    #buildColumnsDefinitionTableHallazgos() {
        return [
            { name: "Hallazgo", displayName: "Hallazgo", maxLines: 1 },
            { name: "Tipo", displayName: "Tipo" },
        ]
    }
    #buildPaginationObjectTableHallazgos(pageNumber, hallazgos) {
        const totalRows = hallazgos.length == 0 ? 0 : hallazgos.length;
        const totalPages = Math.ceil(totalRows / this.PageSizeTableHallazgos);
        return {
            pageNumber: pageNumber,
            pageSize: this.PageSizeTableHallazgos,
            totalCount: totalRows,
            totalPages: totalPages,
            nextPage: totalPages > pageNumber ? pageNumber + 1 : null,
            previousPage: pageNumber > 1 ? pageNumber - 1 : null
        }
    }
    #buildActionObjectTableHallazgos() {
        if (this.Editable) {
            return [
                { name: 'Eliminar', icon: 'trash', iconColor: '#6c757d', fun: (e) => this.handlerActionTableHallazgosEliminar(e) }
            ];
        }
        return [];
    }
    #actualizarHallazgosPaginaActual() {
        const startIndex = (this.PageIndexTableHallazgos - 1) * this.PageSizeTableHallazgos;
        const endIndex = (this.PageIndexTableHallazgos * this.PageSizeTableHallazgos);
        this.HallazgosPaginaActual = this.HallazgosActual.slice(startIndex, endIndex);
    }
    handlerActionTableHallazgosEliminar(row) {
        this.HallazgosActual = this.HallazgosActual.filter(x => x.IdConsecutivo != row.IdConsecutivo);
        this.#actualizarHallazgosPaginaActual();
    }
    IsValid() {
        return this.HallazgosActual.length > 0;
    }
    formIsValid() {
        const hallazgo = this.renderRoot.querySelector("mx-text-area");
        const tipo = this.renderRoot.querySelector("mx-select");
        return hallazgo.IsValid() && tipo.IsValid();
    }
    handlerPageChangeTableHallazgos(page) {
        this.PageIndexTableHallazgos = page.detail.page;
        this.#actualizarHallazgosPaginaActual()
        this.PaginationDefinitionTableHallazgos = this.#buildPaginationObjectTableHallazgos(this.PageIndexTableHallazgos, this.HallazgosActual);
    }
    get value() {
        return this.HallazgosActual;
    }
    static styles = [resetStyles, css`
        .sgc-ai-form-info-hallazgos{
            display:flex;
            column-gap:10px;
            flex-direction:column;
            justify-content: center;
            align-items: end;
        }
        .sgc-ai-form-hallazgos{
            display:grid;
            grid-template-columns:1fr;
            row-gap:10px;
            justify-content: center;
            align-items: end;
        }
    `]
}
customElements.define('mx-sgc-ai-form-hallazgos', SGC_AI_FormHallazgos)