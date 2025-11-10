import { LitElement, html, css, repeat } from '/Scripts/js/libs/lit/lit.js';
import { MxFormField } from './mx-form-field.js';
import { MxButton } from '../Buttons/mx-button.js';

class MxFormManager extends LitElement {
  static properties = {
    formControls: { type: Array },
    eventListenter: { type: Object },
    submitButton: { type: Object }
  }
  constructor() {
    super()
    this.formControls = []
    this.submitAction = (data) => {}
  }
  render() {
    return html`
      <form class="mx-form-manager" @submit="${this.handleSubmit}">
        <slot></slot>
      </form>
    `;
  }

  findControls(){
    let formElements = this.querySelectorAll('*')

    for (let i = 0; i < formElements.length; i++) {
      let formElement = formElements[i]
      if(formElement instanceof MxFormField && formElement.isFormElement){
        this.formControls.push(formElement)
      }
    }
  }



  executeValidations(){
    this.formControls.forEach(control => {
      if(control.executeValidation){
        control.executeValidation()
      }
    })
  }
  handleSubmit(e){
    e.preventDefault()
    this.executeValidations()
    let isValid = this.formControls.every(control => control.isValid)
    if(isValid){
      let data = {}
      this.formControls.forEach(control => {
        //has input date
        if(control.nodeName === 'MX-FIELD-DATE'){
          data[control.name] = new Date(control.value)
        }else
        {
          data[control.name] = control.value
        }
      })
      this.dispatchEvent(new CustomEvent('mxSubmit', {detail: {data:data}}))
    }
  }

  updated(changedProperties){
    this.findControls()
    this.eventListenter = this.addEventListener('mxClick', this.handleSubmit)
  }
  reset(){
    this.formControls.forEach(control => {
      control.clear()
    })
  }
}

customElements.define('mx-form-manager', MxFormManager);