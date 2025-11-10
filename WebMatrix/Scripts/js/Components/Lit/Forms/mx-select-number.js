
import { MxSelect } from './mx-select.js'

class MxSelectNumber extends MxSelect {
    static properties = {
        minValue: { type: Number, attribute: true },
        maxValue: { type: Number, attribute: true },
        defaultValue: { type: Number, attribute: true }
    }

    constructor(){
        super()
    }

    updated(changedProperties){
        if(changedProperties.has('minValue') || changedProperties.has('maxValue')){
            this.options = Array.from({ length: this.maxValue - this.minValue + 1 }, (_, i) => this.minValue + i)
            .map(item => ({ value: item, label: item }))
            this.setValue(this.defaultValue)
        }
    }
 
}

customElements.define('mx-select-number', MxSelectNumber)
