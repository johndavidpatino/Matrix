import { LitElement, html, css, repeat } from '/Scripts/js/libs/lit/lit.js';
import { resetStyles, fontAwesomeStyles } from '/Scripts/js/Components/Lit/Styles/font-awesome.js';
import { MxFormField } from './mx-form-field.js';
import { NotificationManager } from '../../../reportes/notifications/notifications.js';

const formItemTypes = {
    text: (content, name) => html`<mx-field-text class="mx-field-list_form-item" name="${name}" type="text">${content}</mx-field-text>`,
    textarea: (content, name) => html`<mx-text-area class="mx-field-list_form-item" name="${name}" type="textarea">${content}</mx-text-area>`,
    date: (content, name) => html`<mx-field-date class="mx-field-list_form-item" name="${name}" type="date">${content}</mx-field-date>`,
}

const formItemsPlane = {
  text: (content, id, value) => `<mx-field-text class="mx-field-list_form-item" id="${id}" type="text" value="${value}">${content}</mx-field-text>`,
  textarea: (content, id, value) => `<mx-text-area class="mx-field-list_form-item" id="${id}" type="textarea" value="${value}">${content}</mx-text-area>`,
  date: (content, id, value) => `<mx-field-date class="mx-field-list_form-item" id="${id}" type="date" value="${value}">${content}</mx-field-date>`,
}

const notificationManager = new NotificationManager()
//TODO: Terminar implementación, aun falta agregar mas funcionalidades
export class MxFieldList extends MxFormField {
    static properties = {
        formItems: { type: Array },
        items: { type: Array },
        validations: { type: Array }
    }
    constructor(){
        super()
        this.name = ''
        this.items = []
        this.formItems = []
        this.validations = []
    }
    
    render(){
        return html`
        <div class="mx-field-list">
            <span class="mx-field-list_label">
                <slot></slot>
            </span>
            <div class="mx-field-list_form">
                ${repeat(this.filterFormItems(this.formItems), item => this.renderFormItem(item))}
                <mx-button type="button" class="mx-field-list_add-item" @click="${this.addItem}">Agregar</mx-button>
            </div>
            <div class="mx-field-list_items">
                ${this.items.map((item,index) => {
                  let keys = Object.keys(item).filter(key => {
                    if(item[key] === undefined) return false
                    return item[key].hidden ? !item[key].hidden : true
                  })
                  return html`
                  <div class="mx-field-list_item-container">
                  ${[...keys.map(key => html`<div class="mx-field-list_item mx-field-list_item--${item[key].type}">${item[key].value}</div>`),
                    html`<button type="button" class="mx-field-list_remove-item" @click="${() => this.removeItem(item)}"><i class="fa fa-trash"></i></button>`,
                    html`<button type="button" class="mx-field-list_edit-item" @click="${() => this.editItem(item,index)}"><i class="fa fa-edit"></i></button>`
                  ]}
                  </div>`
                })}
            </div>
        </div>
        `
    }
    filterFormItems(formItems){
      return formItems.filter(formItem => formItem.hidden ? !formItem.hidden : true)
    }
    addItem = () => {
      const formElements = this.renderRoot.querySelector('.mx-field-list_form').querySelectorAll('.mx-field-list_form-item')
      let formData = {}
      let anyInvalid = false
      for(const formElement of formElements){
        let value = formElement.getValue()
        formElement.executeValidation()
        if(!formElement.isValid) {
          anyInvalid = true
        }
        if(value === '' || value === null || value === undefined) {
          formData[formElement.name] = {value: '', type: formElement.type}
          continue
        }

        formData[formElement.name] = {value: value, type: formElement.type}
      }

      if(anyInvalid) return

      if (Object.keys(formData).length > 0) {
        this.items = [...this.items, formData];
      }
      this.clearForm()
      this.dispatchEvent(new CustomEvent('mxChange', {detail: this.items}))
    }

    removeItem = (item) => {
      this.items = this.items.filter(i => i !== item)
      this.dispatchEvent(new CustomEvent('mxChange', {detail: this.items}))
    }

    clearForm = () => {
      const formElements = this.renderRoot.querySelectorAll('.mx-field-list_form-item')
      for(const formElement of formElements){
        formElement.clear()
      }
    }

    getItemHTML(item){
      return html`<div class="mx-field-list_item">${item}</div>`
    }

    renderFormItem = (formItem)=> {
      let action = formItemTypes[formItem.type]
      return action(formItem.label, formItem.name)
    }
    editItem =(item,index)=>{
      let modal = document.createElement('mx-modal-container')
      
      let itemKeys = this.formItems.map(item => item.name)
      let filterItemKeys = itemKeys.filter(key => {
        let formItem = this.formItems.find(formItem => formItem.name === key)
        return formItem.hidden ? !formItem.hidden : true 
      })
      
      modal.innerHTML = `
        <div class="mx-section-modal">
            <div class="mx-form-container">
                ${filterItemKeys.map(key => {
                  let formItem = this.formItems.find(formItem => formItem.name === key)
                  return formItemsPlane[formItem.type](formItem.label,formItem.name + index, item[key].value)
                }).join('')}
            </div>
            <div class="mx-modal-footer">
                <mx-button type="button" id="btnModalCancel" variant="outline">Cancelar</mx-button>
                <mx-button type="button" id="btnModalEdit">Guardar</mx-button>
            </div>
        </div> `
      
      this.renderRoot.appendChild(modal)

      modal.querySelectorAll('.mx-field-list_form-item').forEach(element => {
        let formItem = this.formItems.find(formItem => formItem.name === element.name)
        if(!formItem) return
        if(formItem.validation !== undefined){
          element.customValidation = formItem.validation
        }
      })
      let btnModalEdit$ = modal.querySelector('#btnModalEdit')
      let btnModalCancel$ = modal.querySelector('#btnModalCancel')

      btnModalEdit$.addEventListener('click', () => {
        let formData = {}
        let anyInvalid = false
        for (let i = 0; i < itemKeys.length; i++) {
          const key = itemKeys[i];
          let formItem = this.formItems.find(formItem => formItem.name == key);
          let formElement = modal.querySelector(`#${formItem.name + index}`);
          if (!formElement) {
            formData[key] = item[key];
            continue;
          }
          let value = formElement.getValue();
          formElement.executeValidation();
          if (!formElement.isValid) {
            anyInvalid = true;
          }
          formData[key] = { value: value, type: formItem.type };
        }

        if (anyInvalid) return;
        this.items[index] = formData
        modal.remove()
        this.requestUpdate()
      })
      btnModalCancel$.addEventListener('click', () => {
        modal.remove()
      })
    }

    getValue(){
      return this.items
    }

    updated(){
      this.renderRoot.querySelectorAll('.mx-field-list_form-item').forEach(element => {
        let formItem = this.formItems.find(formItem => formItem.name === element.name)
        if(!formItem) return
        if(formItem.validation !== undefined){
          element.customValidation = formItem.validation
        }
      })
    }
    static styles = [
        resetStyles,
      fontAwesomeStyles,
      css`
        .mx-field-list{
          border: 1px solid #e0e0e0;
          padding: 1rem;
          border-radius: 4px;
        }
        .mx-field-list_label{
          display: block;
          padding-bottom: .5rem;
        }
        .mx-field-list_form{
          display: flex;
          flex-wrap: wrap;
          gap: 10px;
          align-items: flex-end;
          justify-content: space-between;
        }
        .mx-field-list_remove-item, .mx-field-list_edit-item{
          border: none;
          background: none;
          width: 2rem;
          height: 2rem;
          display: flex;
          align-items: center;
          justify-content: center;
          border-radius: 4px;
          cursor: pointer;
          transition: background-color 0.3s ease;
          &:hover{
            background-color: rgba(0, 0, 0, 0.05);
          }
          padding: 0;
        }
        .mx-field-list_remove-item{
          color: var(--red);
        }
        .mx-field-list_items{
          padding-top: 2rem;
          display: flex;
          flex-direction: column;
        }
        .mx-field-list_item-container{
          display: flex;
          gap: 10px;
          justify-content: space-between;
          width: 100%;
          padding: 0.25rem;

          border-bottom: 1px solid #e0e0e0;
          &:first-child{
            border-top: 1px solid #e0e0e0;
          }          
        }
        .mx-field-list_form-item{
          flex: 1;
        }
        .mx-field-list_item{
          display: flex;
          align-items: center;
          flex: 1;
        }
        .mx-section-modal{
          background-color: white;
          padding: 1rem;
          min-width: 400px;
          border-radius: 4px;
        }
        .mx-form-container{
          display: grid;
          gap: 1rem;
        }
        .mx-modal-footer{
          display: flex;
          justify-content: space-between;
          padding-top: 2rem;
        }
      `
    ]
}
customElements.define('mx-field-list', MxFieldList) 