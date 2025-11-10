import { LitElement,css } from  '/Scripts/js/libs/lit/lit.js'

export class MxFormField extends LitElement {
    static properties = {
        value: { type: String },
        name: { type: String },
        type: { type: String },
        required: { type: Boolean },
        customValidation: { type: Function },
        isValidationExecuted: { type: Boolean },
        errorMessage: { type: String },
        validate: { type: Function},
        isValid: { type: Boolean },
        validate: { type: Function },
        isFormElement: { type: Boolean },
    }
    constructor(){
        super()
        this.name = ''
        this.value = ''
        this.required = true
        this.errorMessage = ''
        this.isValid = false
        this.isValidationExecuted = false
        this.isFormElement = true
        this.validate = () => {
            let customValidationResult= this.customValidation(this.value,this.errorMessage)
            let standarValidation = this.required && this.value !== ''

            if(!standarValidation){
                this.errorMessage = 'Este campo es requerido'
            }

            if(standarValidation && !customValidationResult.isValid){
                this.errorMessage = customValidationResult.errorMessage
            }
            
            this.isValid = standarValidation && customValidationResult.isValid
            if(this.isValid){
                this.errorMessage = ''
            }
        }
        this.customValidation = () => {
            return {
                isValid: true,
                errorMessage: ''
            }
        }
        this.isFirstChange = true
    }

    executeValidation(){
        this.validate()
        this.isValidationExecuted = true
    }
            

    getValue(){
        return this.value
    }
    clear(){
        this.value = ''
    }

    static styles = css`
        .mx-error{
            color: var(--red);
            font-size: 12px;
            position: absolute;
            left: 0;
            top: 100%;
            width: 100%;
            display: block;
        }
    `
}
