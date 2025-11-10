import { LitElement, html, css, repeat } from 'https://cdn.jsdelivr.net/gh/lit/dist@2/all/lit-all.min.js'
import { MxModal } from './MxModal.js';


export class MxTable extends LitElement {
	static styles = [
		css`
    .ip-table{
      border-collapse: collapse;
      background-color: white;
      color: #666;
      border-radius: 3px;
      width: 100%;
      border: none;
      font-size: 12px;
    }
    
    .ip-table th{
        padding: .25rem;
        color: #5c5c5c;
        background-color: rgba(0, 0, 0, 0.041);
        text-shadow: none;

    }
    
    .ip-table td{
        padding: .25rem;
        text-align: center;
    }
    
    .ip-table tr:not(:last-child) {
        border-bottom: 1px solid #e6e6e6;
    }
    
    .ip-table td:not(:last-child), .ip-table th:not(:last-child){
        border-right: 1px solid #cecece;
    }




    .ip-btn {
      border: none;
      min-height: 24px;
      background-color: #1AAFA4;
      color: white;
      border-radius: 3px;
      box-sizing: border-box;
      box-shadow: 0 1px .5px 1px rgba(0,0,0,.2);
  	}
  
	.ip-btn:hover {
		background-color: #22ebdd;
		box-shadow: none;
	}
	
	.ip-btnContainer{
		display: flex;
		justify-content: center;
		align-items: center;
	}
  
	.ip-btnIcon{
		background-color: transparent;
		display: flex;
		justify-content: center;
		align-items: center;
		padding: 0;
		width: 1.5rem;
		height: 1.5rem;
		border: none;
		margin: 0;
	}
	.ip-tdEmpty{
		background-color: white;
	}
	.ip-btnIcon:not(:disabled){
		cursor: pointer;
	}
	.ip-btnIcon img{
		display: block;
		width: 100%;
	}
	
	.ip-btnIcon:hover{
		filter: brightness(150%);
		
	}
	.ip-btnIcon:disabled{
		opacity: .3;
		filter: saturate(0%);
		pointer-events: none;
	}
	.ip-table-row.success .ip-btnIcon{
		opacity: 1;
		filter: saturate(100%);
	}
	.bold{
		font-weight: bold;
	}
	input[type="number"] {
		border: 1px solid gray;
		width: 40px;
		padding: 0.5rem;
		text-align: right;
		border-radius: 4px;
	}
    `
	];
	static get properties() {
		return {
			columns: { type: Array },
			entries: {
				type: Array,
			},
			options: { type: Object },
			currentEntitieTarget: { type: Object },
			handlers: { type: Array },
			modal: {type: MxModal},
			propNameInModal: {type: String},
			removeInfo: {type: String}
		};
	}
	constructor() {
		super()

		this.columns = []
		
		this.entries = []
		this.options = {
			actions: {
				add: false,
				remove: false,
				edit: false,
			}
		}
		this.currentEntitieTarget = null
		this.handlers = []
		this.propNameInModal = null
	}
	firstUpdated() {
		this.modal = this.renderRoot.querySelector('mx-modal');
	}
	render() {
		let { add, remove, edit } = this.options.actions
		return html`
      <table class="ip-table">
        <thead>
          <tr>
            ${this.renderHeaders()}
            ${add ? html`<th>Agregar</th>` : ''}
            ${edit ? html`<th>Guardar</th>` : ''}
            ${remove ? html`<th>Quitar</th>` : ''}
          </tr>
        </thead>
        <tbody>
          ${this.entries.length === 0
				? html`<td class="ip-td ip-tdEmpty" colspan="100%">No hay datos para enseñar</td>`
				: repeat(this.entries, (entry, index) => this.renderEntry(entry, index))}
        </tbody>
		</table>
		<mx-modal modalType="alert">
			¿Quiere eliminar el registro identificado como: <span class="bold">${this.removeInfo}</span></span>?
		</mx-modal>
    `
	}

	renderHeaders() {
		return html`
      ${this.columns.map((header, index) => {
			return html`
          <th>
            ${header.name}
          </th>
        `
		})}
    `
	}

	renderEntry(entry, index) {
		const { add, edit, remove } = this.options.actions
		return html`
        <tr class="ip-table-row ${entry.flag}">
          ${this.columns.map(header => {
			return this.renderCell(entry, header, index)
		})}
          ${add ? html`
          <td>
            <div class="ip-btnContainer">
                <button type="button" class="ip-btnIcon" data-rowid='${index}' @click="${this.addItem}">
                    <img src='../Scripts/img/iconset/icons-add.png' alt="Agregar" />
                </button>
            </div>
          </td>
          `: ''}
          ${edit ? html`
          <td>
            <div class="ip-btnContainer">
                <button type="button" class="ip-btnIcon" data-rowid="${index}" @click="${this.updateItem}" .disabled=${!entry.enableEdit}>
                    <img src='${this.getIconByFralg(entry.flag)}' alt="Guardar" />
                </button>
            </div>
          </td>
          `: ''}
          ${remove ? html`
          <td>
            <div class="ip-btnContainer">
                <button type="button" class="ip-btnIcon ip-btnRemove" data-rowid="${index}" @click="${this.removeItem}">
                    <img src="../Scripts/img/iconset/icons-remove.png" alt="Quitar" />
                </button>
            </div>
          </td>
          `: ''}
        </tr>
      `
	}

	renderCell(entry, header, index) {
		let { type } = header

		switch (type) {
			case "input:number":
				return html`<td><input type="number" data-rowid=${index} data-property=${header.property} .value="${entry[header.property]}" @change=${this._inputNumberChange}></input></td>`
			case "input:text":
				return html`<td><input type="text" @change=${this._inputDefaultChange} data-rowid=${index} data-property=${header.property} .value="${entry[header.property]}"></input></td>`
			case "input:checkbox":
				return html`<td><input type="checkbox" @change=${this._inputCheckboxChange} data-rowid=${index} data-property=${header.property} .value="_${index}" name="aprobacion" ?checked=${entry[header.property]}></input></td>`
			default: return html`<td>${entry[header.property]}</td>`
		}
	}
	removeItem(event) {
		let context = this;
		this.modal.setActive(true)
		let target = event.currentTarget
		let rowId = parseInt(target.dataset.rowid)
		let entry = context.entries[rowId]

		if(context.propNameInModal) {
			console.log(context.removeInfo)
			context.removeInfo = entry[context.propNameInModal]
			context.requestUpdate()
		}
		context.modal.setConfirmAction(function() {
			context.dispatchEvent(new CustomEvent('removeEntry', {
				detail: { ...entry, rowId: rowId }
			}))
			context.entries.splice(rowId, 1)
			context.modal.setActive(false)
			context.requestUpdate()
			context.removeInfo = ''
		})
	}

	addItem(event) {
		let target = event.currentTarget
		let rowId = parseInt(target.dataset.rowid)
		this.dispatchEvent(new CustomEvent('addEntry', {
			detail: { ...this.entries[rowId], rowId: rowId }
		}))
	}

	updateItem(event) {
		let target = event.currentTarget
		let rowId = parseInt(target.dataset.rowid)
		let context = this

		if(!context.entries[rowId].enableEdit) return;

		context.entries[rowId].flag = 'updating'
		this.dispatchEvent(new CustomEvent('updateEntry', {
			detail: {...this.entries[rowId], CallbackUpdated: function(isUpdated = false) {
				if(isUpdated === false) return;
				setTimeout(() => {
					context.entries[rowId].flag = ''
					context.requestUpdate()
				}, 2000);
				context.entries[rowId].flag = 'success'
				context.entries[rowId].enableEdit = false
				context.requestUpdate()
			}}
		}))
	}

	removeEntry(entryIndex) {
		this.entries.splice(entryIndex, 1)

		this.requestUpdate()
	}
	addEntry(entry) {
		this.entries.push(entry)
		this.requestUpdate()
	}

	setEntries(newEntries) {
		this.entries = newEntries.map(item => {
			return {...item, enableEdit: false, flag: ''}
		})
		this.requestUpdate()
	}

	getEntries(){
		return this.entries
	}
	_getInputData(target) {
		return {
			value: target.value,
			rowId: target.dataset.rowid,
			property: target.dataset.property,
			checked: target.checked
		}
	}
	_inputNumberChange(event) {
		const { value, rowId, property } = this._getInputData(event.currentTarget)

		let convertValue = parseFloat(value)
		let currentEntry = this.entries[rowId]
		currentEntry[property] = convertValue
		currentEntry.flag = 'edited'
		currentEntry.enableEdit = true

		this.requestUpdate()
	}
	_inputCheckboxChange(event) {
		const { checked, rowId, property } = this._getInputData(event.currentTarget)


		this.entries[rowId][property] = checked
		this.entries[rowId].enableEdit = true
		this.requestUpdate()
	}
	_inputDefaultChange(event) {
		const { value, rowId, property } = this._getInputData(event.currentTarget)

		this.entries[rowId][property] = value
		this.entries[rowId].enableEdit = true
		this.requestUpdate()
	}

	getIconByFralg(flag){
		switch (flag) {
			case 'success':
				return '../Scripts/img/iconset/icons-check.png'
			default: 
				return '../Scripts/img/iconset/icons-save.png'
		}
	}
}

customElements.define('mx-table', MxTable)



