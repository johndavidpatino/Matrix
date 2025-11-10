import { LitElement, html, css,repeat } from '/Scripts/js/libs/lit/lit.js';
import { fontAwesomeStyles,resetStyles } from '../Styles/font-awesome.js';
import { MxFormField } from './mx-form-field.js';

export class MxCustomSelect extends MxFormField {
  static properties = {
    items: { type: Array },
    keyLabel: { type: String },
    keyValue: { type: String },
    activeList: { type: Boolean },
    canSearch: { type: Boolean },
    placeholder: { type: String },
    isMultiple: { type: Boolean },
    initialOptions: { type: Array },
    isLoading: { type: Boolean },
    disabled: { type: Boolean },
    selectedOptions: { type: Array },
    isFocus: { type: Boolean },
    filteredItems: { type: Array },
    searchValue: { type: String },
    selectableOptions: { type: Array },
  };

  constructor() {
    super();
    this.items = [];
    this.keyLabel = 'label';
    this.keyValue = 'value';
    this.activeList = false;
    this.canSearch = false;
    this.placeholder = '--Seleccione--';
    this.isMultiple = false;
    this.initialOptions = [];
    this.isLoading = false;
    this.disabled = false;
    this.selectedOptions = [];
    this.isFocus = false;
    this.filteredItems = [];
    this.searchValue = '';
    this.selectableOptions = [];
  }

  connectedCallback() {
    super.connectedCallback();
    this.updateSelectableOptions();
  }

  updated(changedProperties) {
    if (changedProperties.has('items') || changedProperties.has('initialOptions')) {
      this.updateSelectableOptions(changedProperties.has('initialOptions'));
    }
  }

  updateSelectableOptions(reload = false) {
    if (reload || (this.selectableOptions.length === 0 && this.items.length > 0)) {
      this.selectableOptions = this.items.map(item => {
        item['value'] = item[this.keyValue];
        item['label'] = item[this.keyLabel];
        item['checked'] = this.initialOptions.some(initialOption => initialOption[this.keyValue] == item[this.keyValue]);
        return item;
      })
      this.updateSelectedOptions();

      if (this.initialOptions.length > 0) {
        this.dispatchEvent(new CustomEvent('selectedOptionsChange', {
          detail: structuredClone(this.selectedOptions)
        }));
      }
    }
  }

  toggleList() {
    if(this.disabled) return;
    this.activeList = !this.activeList;
    if (!this.activeList) {
      this.filteredItems = [...this.selectableOptions];
    }
  }

  focusHandler(active) {
    this.isFocus = active;
  }
  
  changeInput(e) {
    const { value, checked } = e.target;

    this.selectableOptions.forEach(option => {
      if (this.isMultiple) {
        if (option.value == value) {
          option.checked = checked;
        }
      } else {
        option.checked = option.value == value && checked;
      }
    });
    
    this.updateSelectedOptions();

    this.dispatchEvent(new CustomEvent('selectedOptionsChange', {
      detail: structuredClone(this.selectedOptions)
    }));

    if (!this.isMultiple) {
      this.filteredItems = [...this.selectableOptions];
      this.activeList = false;
    }
    this.dispatchEvent(new CustomEvent('mxChange', {
      detail: structuredClone(this.selectedOptions)
    }));
  }

  updateSelectedOptions() {
    this.selectedOptions = this.items.filter(opt => {
      const refOption = this.selectableOptions.find(so => so.value === opt[this.keyValue]);
      return refOption ? refOption.checked : false;
    });
    this.filteredItems = [...this.selectableOptions];
  }

  searchHandle (e) {
    const { value } = e.target;
    if (value === '') {
      this.filteredItems = [...this.selectableOptions];
      return;
    }

    this.filteredItems = this.selectableOptions.filter(item => 
      item.label.toLowerCase().includes(value.toLowerCase())
    );
  }

  getCountSelectedOptions() {
    return this.selectedOptions.length;
  }

  removeItem(item) {
    const option = this.selectableOptions.find(opt => opt.value === item[this.keyValue]);
    if (option) {
      option.checked = false;
      this.filteredItems = [...this.selectableOptions];
      this.updateSelectedOptions();
    }
  }

  firstUpdated(){
    window.addEventListener('mousedown', this.handleClickOutside);
  }

  handleClickOutside = (e) => {
    if(!this.contains(e.target)){      
      this.activeList = false;
    }
  }

  getValue(){
    return this.selectedOptions
  }

  get searchInput(){
    return this.shadowRoot.querySelector('.select-search-input');
  }
  render() {
    return html`
      <div class="select-wrapper ${this.disabled ? 'disabled' : ''}${this.selectedOptions.length > 0 ?'.mx-filled':''}">
        <span class="select-label">
          <slot></slot>
        </span>
        <div class="select">
          <label class="select-button-wrapper">
            <button type="button" class="select-button" @click="${this.toggleList}">
              <div class="select-selection">
                ${this.isLoading
                  ? html`<div class="spinner"></div>`
                  : this.selectedOptions.length === 1
                  ? this.renderSelectedOption(this.selectedOptions[0])
                  : this.selectedOptions.length > 1
                  ? html`<span class="select-selection-text">${this.selectedOptions.length} seleccionados</span>`
                  : html`<div class="select-placeholder">${this.placeholder}</div>`
                }
              </div>
              <div class="select-icon">
                <i class="fa fa-chevron-down"></i>
              </div>
            </button>
          </label>
          
          <div class="select-list ${this.activeList ? 'active' : ''}">
            ${this.canSearch
              ? html`
                <div class="select-search">
                  <input type="text" placeholder="Buscar..." class="select-search-input" @input="${this.searchHandle}"/>
                </div>
              `
              : ''
            }
            ${repeat(this.filteredItems, option => html`
              <div class="select-item">
                <label>
                  <span class="select-item-checkbox">
                    <svg viewBox="0 0 16 16" fill="none" xmlns="http://www.w3.org/2000/svg">
                      <path d="M12.4733 4.80667C12.4114 4.74418 12.3376 4.69458 12.2564 4.66074C12.1752 4.62689 12.088 4.60947 12 4.60947C11.912 4.60947 11.8249 4.62689 11.7436 4.66074C11.6624 4.69458 11.5886 4.74418 11.5267 4.80667L6.56001 9.78L4.47334 7.68667C4.40899 7.62451 4.33303 7.57563 4.2498 7.54283C4.16656 7.51003 4.07768 7.49394 3.98822 7.49549C3.89877 7.49703 3.8105 7.51619 3.72844 7.55185C3.64639 7.58751 3.57217 7.63898 3.51001 7.70333C3.44785 7.76768 3.39897 7.84364 3.36617 7.92688C3.33337 8.01011 3.31728 8.099 3.31883 8.18845C3.32038 8.2779 3.33953 8.36618 3.37519 8.44823C3.41085 8.53028 3.46233 8.60451 3.52667 8.66667L6.08667 11.2267C6.14865 11.2892 6.22238 11.3387 6.30362 11.3726C6.38486 11.4064 6.472 11.4239 6.56001 11.4239C6.64802 11.4239 6.73515 11.4064 6.81639 11.3726C6.89763 11.3387 6.97137 11.2892 7.03334 11.2267L12.4733 5.78667C12.541 5.72424 12.595 5.64847 12.632 5.56414C12.6689 5.4798 12.688 5.38873 12.688 5.29667C12.688 5.2046 12.6689 5.11353 12.632 5.02919C12.595 4.94486 12.541 4.86909 12.4733 4.80667Z" fill="currentColor"/>
                    </svg>                                        
                  </span>
                  <input class="select-item-input${option.checked?' checked':''}" type="checkbox" .value="${option.value}" .checked="${option.checked}" @change="${this.changeInput}">
                  ${this.renderOption(option)}
                </label>
              </div>
            `)}
          </div>
        </div>
    </div>
        
    `;
  }

  renderOption(option){
    return html`<span class="select-item-text">${option.label}</span>`;
  }

  renderSelectedOption(option){
    return html`<span class="select-item-text">${option.label}</span>`;
  }
  static styles = [resetStyles, fontAwesomeStyles, css`
    
.select {
    width: 100%;
    position: relative;
    --bg-color: white;
}
.select-wrapper{
    width: 100%;
}
.select-wrapper.disabled{
    width: 100%;
    pointer-events: none;
    opacity: .2;
}
.select-label{
    min-height: 1em;
    display: block;
    font-size: 12px;
    padding-bottom: 4px;
    text-wrap: nowrap;
}
.select-label:empty{
    height: 0;
    padding-bottom: 0;
    display: none;
}
.select-button-wrapper{
    display: grid;
    grid-template-columns: 1fr;
    align-items: center;
    justify-content: start;
    border-radius: 3px;
    border: 1px solid #d0d0d0;
    padding-inline: .5rem;
    position: relative;
    height: 40px;
}

.select-selection{
    width: 100%;
    flex-grow: 1;
    white-space: nowrap;
    max-width: calc(100% - 18px);
    overflow: hidden;
    text-overflow: ellipsis;
    font-size: 14px;
    padding-right: 18px;
    color: #50627E;
}
.select-button{
    color: #50627E;
    padding-inline: 12px;
    width: 100%;
    border:none;
    outline: none;
    background-color: #fff;
    text-align: left;
    user-select: none;
    overflow: hidden;
}
.select-list {
    position: absolute;
    top: calc(100% + 4px);
    left: 0;
    min-width: 100%;
    background-color: #fff;
    z-index: 10;
    border-bottom-left-radius: 4px;
    border-bottom-right-radius: 4px;
    border: 1px solid #BBBDCC;
    max-height: 200px;
    overflow-y: auto;
    padding: .5rem;
    border-radius: 8px;
    transform: scaleY(0);
    box-shadow: 0 1px 8px #696c8036;
    transition: transform .05s;
    transform-origin: top;
    transition-timing-function: ease-in;
}
.select-list.active{
    transform: scaleY(1);
}
.select-item{
    user-select: none;
    display: flex;
    align-items: center;
}
.select-item>label{
    display: grid;
    grid-template-columns: auto 1fr;
    grid-template-rows: auto 1fr;
    grid-template-areas: "checkbox label"
    "empty label";
    align-items: center;
    padding: .5rem;
    column-gap: .25rem;
    width: 100%;
}
.select-item label:hover{
    background-color: #E9F0FD;
    border-radius: 4px;
}

.select-item-input{
    display: none;
}

.select-item-checkbox{
    grid-area: checkbox;
    display: inline-flex;
    width: 0;
    height: 1rem;
    border-radius: 2px;
    color: var(--primary);
    & svg{
        transition: transform .3s;
        transform: scale(0);
    }
}

.select-item:has(.checked) .select-item-checkbox{
    width: 1rem;
    & svg{
        transform: scale(1.3);
    }
}

.select-item-text {
    grid-area: label;
    justify-self: start;
    text-align: left;
    font-size: 12px;
    color: #606F7D;
}

.select-placeholder.filled{
    position: absolute;
    bottom: 100%;
    left: .25rem;
    font-size: .8em;
    transform: translateY(50%);
    z-index: 2;
}

.select-placeholder.filled::before{
    content: "";
    position: absolute;
    display: block;
    width: calc(100% + 2px);
    height: 2px;
    background-color: var(--bg-color);
    left: 50%;
    top: 50%;
    transform: translateY(calc(-50% + .5px)) translateX(-50%);
    z-index: -1;
}

.select-search{
    width: 100%;
    heigth: 20px;
    position: sticky;
    top: 0;

}
.select-search-input{
    padding-inline: .5rem;
    padding-block: .25rem;
    border-radius: 4px;
    border: 1px solid #E0E3EE;
    outline: none;
    width: 100%;
}

.select-icon{
    position: absolute;
    right: .5rem;
    top: 50%;
    display: flex;
    align-items: center;
    transform: translateY(-50%);
    color: #606F7D;
}

.select-selection-text{
    font-weight: 400;
    font-size: 14px;
    color:#50627E; 
}

.select-selected-button{
    border: none;
    background-color: transparent;
    font-size: 12px;
    color: var(--primary);
    cursor: pointer;
    
}

.select-selected-options{
    position: relative;
    display: inline-flex;
}

.select-selected-list{
    position: absolute;
    top: 100%;
    left: 0;
    z-index: 10;
    padding: .5rem;
    margin: 0;
    background-color: #fff;
    border-radius: 8px;

    width: 100%;
    min-width: 300px;
    max-height: 300px;
    overflow-y: auto;
    transform: scaleY(0);
    transform-origin: top;
    transition: all .3s;
}

.select-selected-options:hover .select-selected-list{
    display: initial;
    transform: scaleY(1);
}

.select-selected-item{
    list-style: none;
    font-size: 12px;
    display: flex;
    gap: 4px;
    align-items: center;
    justify-content: space-between;
    background-color: #fff;
    padding-block: .2rem;
    padding-inline: .5rem;
    border-radius: 2px;
}
.select-selected-item:not(:first-child){
    margin-top: .5rem;
}
.delete-button {
    border-radius: 999px;
    display: flex;
    background-color: transparent;
    border: none;
    height: 1.8rem;
    width: 1.8rem;
    align-items: center;
    justify-content: center;
    flex-shrink: 0;
}

.delete-button:hover{
    background-color: #E9F0FD;
}

.select-placeholder{
    color: #BBBDCC;
}

.select-wrapper.secondary{
    .select-button-wrapper{
        border: none;
    }
    .select-selection-text{
        font-weight: 500;
        color: #3070b6;
    }
}


.select-wrapper.navbar-style{
    width: fit-content;
    height: 100%;
    .select{
        height: 100%;
    }
    .select-selection{
        padding-right: 10px;
    }
    .select-button-wrapper{
        border: none;
        height: 100%;
        display: flex;
        align-items: center;
    }
    .select-selection-text{
        font-weight: 500;
        color: #1B212D;
    }

}

.spinner {
  width: 20px;
  height: 20px;
  border: 2px solid #f3f3f3;
  border-top: 2px solid #3498db;
  border-radius: 50%;
  animation: spin 1s linear infinite;
}

@keyframes spin {
  0% { transform: rotate(0deg); }
  100% { transform: rotate(360deg); }
}

.select-icon {
  position: absolute;
  right: .5rem;
  top: 50%;
  display: flex;
  align-items: center;
  transform: translateY(-50%);
  color: #1B212D;
}
.select-wrapper.disabled{
  pointer-events: none;
  opacity: .4;
}

  `];
}

customElements.define('mx-custom-select', MxCustomSelect);