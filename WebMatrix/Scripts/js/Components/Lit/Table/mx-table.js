import { LitElement, html, css, repeat } from '/Scripts/js/libs/lit/lit.js'
import {fontAwesomeStyles } from '../Styles/font-awesome.js'
export class MxTable extends LitElement {
    static styles = [
        fontAwesomeStyles,
        css`
        :host {
            display: block;
            overflow-x: auto;
        }
        .mx-table {
            width: 100%;
            border-collapse: collapse;
            margin-top: 1rem;
            box-shadow: 0 1px 3px rgba(0,0,0,0.1);
        }
        .mx-table__th,
        .mx-table__td {
            padding: 0.75rem;
            text-align: left;
        }
        .mx-table__th {
            background-color: #f5f5f5;
            font-weight: 600;
            color: #333;
            border-bottom: 1px solid #e0e0e0;
        }
        .mx-table__tr {
            border-bottom: 1px solid #e0e0e0;
        }
        .mx-table__tr:hover {
            background-color: #f9f9f9;
        }
        .mx-table__td-content {
            --max-lines: 2; 
            -webkit-line-clamp: var(--max-lines);
            max-width: 200px;
            line-clamp: var(--max-lines);
            max-height: calc(var(--max-lines) * 1.5em); 
            overflow: hidden;
            text-overflow: ellipsis;
            display: -webkit-box;
            -webkit-box-orient: vertical;
            line-height: 1.5em;
        }
        .mx-table__button {
            --color: #6c757d;
            --hoverColor: #007bff;
            background: none;
            border: none;
            width: 2rem;
            height: 2rem;
            display: flex;
            align-items: center;
            justify-content: center;
            cursor: pointer;
            font-size: 1rem;
            color: var(--color);
            transition: color 0.3s ease, background-color 0.3s ease;
            border-radius: 3px;
        }
        .mx-table__button:hover {
            --color: var(--hover-color);
            background-color: #ececec;
        }
        .mx-table__icon {
            pointer-events: none;
            color: currentColor;
        }
        .mx-table__td-actions{
            display: flex;
            gap: .25rem;
            height: 100%;
            align-items: center;
            justify-content: center;
        }
        .mx-table__td--sm {
            width: 2%;
        }
        .mx-table__no-records{
            text-align: center;
            font-style: italic;
            color: #888;
            padding: 2rem;
        }
        .mx-table__pagination {
            display: flex;
            justify-content: center;
            align-items: center;
            margin-top: 1rem;
            font-size: 0.9rem;
        }
        .mx-table__pagination-button {
            background: none;
            border: 1px solid #e0e0e0;
            padding: 0.5rem 0.75rem;
            margin: 0 0.25rem;
            cursor: pointer;
            border-radius: 4px;
            transition: background-color 0.3s ease;
        }
        .mx-table__pagination-button:hover:not(:disabled) {
            background-color: #f5f5f5;
        }
        .mx-table__pagination-button:disabled {
            opacity: 0.5;
            cursor: not-allowed;
        }
        .mx-table__pagination-current {
            font-weight: bold;
            padding: 0.5rem 0.75rem;
        }
        .mx-table__pagination-ellipsis {
            padding: 0.5rem 0.25rem;
        }
        .mx-table__pagination-current {
            font-weight: bold;
            padding: 0.5rem 0.75rem;
            background-color: #007bff;
            color: white;
            
        }
        .mx-table__pagination-current:disabled {
            opacity: 1;
        }
        `
    ]
    static properties = {
        data: { type: Array },
        transformedData: { type: Array },
        columns: { type: Array },
        actions: { type: Array },
        idData: { type: String },
        lastElementDeleted: { type: Object },
        pagination: { type: Object },
        transformations: {type: Array}
    }

    constructor() {
        super();
        this.data = []
        this.columns = []
        this.actions = []
        this.idData = 'id'
        this.lastElementDeleted = {
            element: null,
            index: null
        }
        this.pagination = null;
        this.transformations = []
        this.transformedData = []
    }
    
    render() {
        return html`
            <table class="mx-table">
                <thead class="mx-table__thead">
                    <tr class="mx-table__tr">
                        ${repeat(this.columns, column => html`<th class="mx-table__th">${column.displayName}</th>`)}
                        ${this.actions.length > 0 ? html`<th class="mx-table__th">Acciones</th>` : ''}
                    </tr>
                </thead>
                <tbody class="mx-table__tbody">
                    ${this.data.length > 0 ? repeat(this.addTransformations(this.data), (row,index) => html`
                        <tr class="mx-table__tr">
                            ${repeat(this.columns, column => html`<td class="mx-table__td">
                                <div class="${column.maxLines ? 'mx-table__td-content' : ''}" style="--max-lines: ${column.maxLines};" title="${row[column.name]}">
                                    ${this.#formatValue(row[column.name])}
                                </div>
                            </td>`)}
                            ${this.actions.length > 0 ? html`
                            <td class="mx-table__td mx-table__td--sm">
                                <div class="mx-table__td-actions">
                                    ${repeat(this.actions, action => html`
                                        <button class="mx-table__button" @click="${() => this.handleAction(action,this.data[index],index)}" style="--color: ${action.iconColor}; --hover-color: ${action.iconHoverColor};">
                                            <i class="mx-table__icon fa fa-${action.icon}"></i>
                                        </button>
                                    `)}
                                </div>
                            </td>
                            `:''}
  
                        </tr>
                    `):html`<tr><td colspan="${this.columns.length + (this.actions.length > 0 ? 1 : 0)}" class="mx-table__no-records">No se encontraron registros</td></tr>`}
                </tbody>
            </table>
            ${this.renderPagination()}
        `;
    }

    renderPagination() {
        if (!this.pagination) return '';

        const { pageNumber, totalPages, pageSize, nextPage, previousPage } = this.pagination;

        return html`
            <div class="mx-table__pagination">
                <button class="mx-table__pagination-button" ?disabled=${pageNumber <= 1} @click=${() => this.changePage(1)}>
                    <i class="fas fa-angle-double-left"></i>
                </button>
                <button class="mx-table__pagination-button" ?disabled=${!previousPage} @click=${() => this.changePage(previousPage)}>
                    <i class="fas fa-angle-left"></i>
                </button>
                ${this.renderPageNumbers()}
                <button class="mx-table__pagination-button" ?disabled=${!nextPage} @click=${() => this.changePage(nextPage)}>
                    <i class="fas fa-angle-right"></i>
                </button>
                <button class="mx-table__pagination-button" ?disabled=${pageNumber >= totalPages} @click=${() => this.changePage(totalPages)}>
                    <i class="fas fa-angle-double-right"></i>
                </button>
            </div>
        `;
    }

    renderPageNumbers() {
        const { pageNumber, totalPages } = this.pagination;
        const pageNumbers = [];

        for (let i = 1; i <= totalPages; i++) {
            if (i === 1 || i === totalPages || (i >= pageNumber - 1 && i <= pageNumber + 1)) {
                pageNumbers.push(html`
                    <button class="mx-table__pagination-button ${i === pageNumber ? 'mx-table__pagination-current' : ''}" ?disabled=${i === pageNumber} @click=${() => this.changePage(i)}>
                        ${i}
                    </button>
                `);
            } else if (i === pageNumber - 2 || i === pageNumber + 2) {
                pageNumbers.push(html`<span class="mx-table__pagination-ellipsis">...</span>`);
            }
        }
        return pageNumbers;
    }

    addTransformations(data = []){
        this.data = data
        this.transformedData = structuredClone(data)
        if(this.transformations.length === 0) return this.transformedData
        this.transformedData = this.transformedData.map(item => {
            let transformedItem = {}
            this.transformations.forEach(transform => {
                transformedItem = transform(item)
            })
            return transformedItem
        })
        return this.transformedData
    }

    setPagination(pagination){
        this.pagination = pagination
        this.requestUpdate()
    }
    changePage(page) {
        const event = new CustomEvent('page-change', {
            detail: { page },
            bubbles: true,
            composed: true
        });
        this.dispatchEvent(event);
    }

    async handleAction(action, row, index) {
        if(action.isDeleteAction){
            this.deleteItem(index)
            await action.fun(row,(success)=>{
                if(success){
                    this.deleteItem(index)
                    return
                }
                this.rolbackDelete()
            })
        }
        if(action.isEditAction){
            await action.fun(row,(data)=>{
                this.setRowData(data,index)
            })
        }
    }
    deleteItem(refIndex){
        this.data = this.data.filter((item, index) => {
            if(refIndex === index){
                this.lastElementDeleted.element = structuredClone(item)
                this.lastElementDeleted.index = index 
            }
            return refIndex !== index
        });
        this.requestUpdate()
    }
    rolbackDelete(){
        this.data.splice(this.lastElementDeleted.index, 0, this.lastElementDeleted.element);
        this.requestUpdate()
    }
    #formatValue(value) {
        if(typeof value !== 'string') return value;
        if (value.substring(0, 5) == "/Date") {
            return this.#formatMillisecondsToyyyyMMdd(value);
        }
        return value;
    }
    #dateFromMilliseconds(milliseconds) {
        return new Date(parseInt(milliseconds.substr(6)));
    }
    #formatMillisecondsToyyyyMMdd(milliseconds) {
        let date = this.#dateFromMilliseconds(milliseconds);
        return this.#formatDateToyyyyMMdd(date);
    }
    #formatDateToyyyyMMdd(dateToFormat) {
        return dateToFormat.format('yyyy-MM-dd');
    }
    setRowData(data,index){
        this.data = this.data.map((item, i) => {
            if(i === index){
                return data
            }
            return item
        });
    }
}   
customElements.define('mx-table', MxTable);