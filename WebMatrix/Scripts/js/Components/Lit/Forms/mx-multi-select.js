import { LitElement, html, css, repeat } from '/Scripts/js/libs/lit/lit.js';
import { resetStyles, fontAwesomeStyles } from '../Styles/font-awesome.js';
import '/Scripts/Chosen/chosen.jquery.js'

class MxMultiSelect extends LitElement {
    static properties = {
        name: { type: String },
        value: { type: String },
        options: { type: Array },
        required: { type: Boolean },
        HasEmptyOption: { type: Boolean },
        EmptyOptionLabel: { type: String },
        EmptyOptionValue: { type: String },
        optionsAvailable: { type: Array, state: true },
        optionsSelected: { type: Array, state: true },
        optionsAvailableSearch: { type: Array, state: true },
        isDropdownOpen: { type: Boolean }
    }

    constructor() {
        super()
        this.name = ''
        this.options = []
        this.required = false
        this.HasEmptyOption = false
        this.EmptyOptionLabel = '---Seleccione---'
        this.EmptyOptionValue = ''
        this.optionsAvailable = [];
        this.optionsAvailableSearch = [];
        this.optionsSelected = [];
        this.handleClickOutside = this.handleClickOutside.bind(this);
        this.isDropdownOpen = false;
    }
    async firstUpdated() {
        
    }
    updated(changedProperties) {
        if (changedProperties.has('options')) {
            this.optionsAvailable = this.options;
            this.optionsAvailableSearch = this.options;
        }
    }
    connectedCallback() {
        super.connectedCallback();
        document.addEventListener('click', this.handleClickOutside);
    }

    disconnectedCallback() {
        document.removeEventListener('click', this.handleClickOutside);
        super.disconnectedCallback();
    }


    render() {
        return html`
            <div class="mx-dropdown-multiple-wrapper">
                <label class="mx-dropdown-multiple-label">
                    <slot></slot>
                </label>
                <div class="mx-dropdown-multiple-control-wrapper">
                    <div class="mx-dropdown-multiple-control-options-selected" @click=${this.handleClickSelectedOptions}>
                        ${repeat(this.optionsSelected, option => html`
                            <div class="mx-select-option-selected" data-value=${option.value} data-label=${option.label} @click=${this.handleClickSelectedOption}>
                                ${option.label}
                                <button class="mx-select-option-selected-close" data-value=${option.value} data-label=${option.label} @click=${this.handlerClickCloseSelectedOption}>X</button>
                            </div>`)}
                    </div>
                    <div class="mx-dropdown-multiple-control-options-wrapper inactive">
                        <div class="mx-dropdown-multiple-control-options-searcher">
                            <input type="text" class="mx-dropdown-multiple-control-options-searcher-txt" @input=${this.handlerChangeTxtSearch}>
                            <i class="fa fa-search"></i>
                        </div>
                        <div class="mx-dropdown-multiple-control-options-available">
                            ${repeat(this.optionsAvailableSearch, option => html`<div class="mx-select-option" data-value=${option.value} data-label=${option.label} @click=${this.handlerClickSelectOption}>${option.label}</div>`)}
                        </div>
                    </div>
                </div>
            </div>
        `
    }

    clear() {
        this.value = "";
    }
    get value() {
        return this.optionsSelected;
    }
    get _control() {
        return this.renderRoot.querySelector('.mx-dropdown-multiple-control')
    }

    IsValid() {
        const state = !this.required || this.optionsSelected.length > 0
        this.renderRoot.querySelector(".mx-dropdown-multiple-control-options-selected").classList.toggle('invalid', !state);
        return state;
    }
    handlerClickSelectOption(event) {
        this.optionsSelected = [...this.optionsSelected, { label: event.target.dataset.label, value: event.target.dataset.value }];
        this.optionsAvailable = this.optionsAvailable.filter(x => x.value != event.target.dataset.value);
        this.optionsAvailableSearch = this.optionsAvailable;
        this.IsValid();
    }
    handleClickOutside(event) {
        const elementsAllowed = [
            "mx-dropdown-multiple-control-options-selected",
            "mx-select-option",
            "mx-select-option-selected",
            "mx-select-option-selected-close",
            "mx-dropdown-multiple-control-options-searcher-txt"
        ]
        const commonElements = elementsAllowed.filter(className => event.composedPath()[0].classList.contains(className));

        if (commonElements.length == 0) {
            this.inactiveOptionsAvaliable();
            return;
        }
        this.activeOptionsAvaliable();
    }
    handlerClickCloseSelectedOption(event) {
        this.optionsAvailable = [...this.optionsAvailable, { label: event.target.dataset.label, value: event.target.dataset.value }];
        this.optionsAvailableSearch = this.optionsAvailable;
        this.optionsSelected = this.optionsSelected.filter(x => x.value != event.target.dataset.value);
    }
    handlerChangeTxtSearch(event) {
        if (event.target.value.trim() == "") {
            this.optionsAvailableSearch = this.optionsAvailable;
            return;
        }
        this.optionsAvailableSearch = this.optionsAvailableSearch.filter(option => option.label.toLowerCase().includes(event.target.value.toLowerCase()));
    }
    activeOptionsAvaliable() {
        const controlOptionsWrapper = this.renderRoot.querySelector(".mx-dropdown-multiple-control-options-wrapper");
        controlOptionsWrapper.classList.remove("inactive");
        controlOptionsWrapper.classList.add("active");
        this.isDropdownOpen = true;
    }
    inactiveOptionsAvaliable() {
        const controlOptionsWrapper = this.renderRoot.querySelector(".mx-dropdown-multiple-control-options-wrapper");
        controlOptionsWrapper.classList.remove("active");
        controlOptionsWrapper.classList.add("inactive");
        this.isDropdownOpen = false;
    }

    static styles = [resetStyles, fontAwesomeStyles,css`
    .mx-dropdown-multiple-wrapper{
        display:flex;
        flex-direction:column;
        width:100%;
        position:relative;
    }
    .chosen-container{
        width:100%!important;
    }
    .mx-select-option{

    }
    .mx-dropdown-multiple-control-options-selected{
        display:flex;
        padding:.5rem;
        border:1px solid #d0d0d0;
        column-gap:2px;
        flex-wrap: wrap;
        row-gap:2px;
        min-height:2.5rem;
        box-sizing:border-box;
    }
    .mx-select-option-selected{
        padding:5px;
        border-width:1px;
        border-style:solid;
    }
    .mx-dropdown-multiple-control-options-wrapper{
        border-width:1px;
        border-style:solid;
        width:100%;
        position:absolute;
        background-color:white;
        box-sizing:border-box;
        padding:5px;
    }
    .mx-dropdown-multiple-control-options-wrapper.active{
        visibility:visible;
    }
    .mx-dropdown-multiple-control-options-wrapper.inactive{
        visibility:hidden;
    }
    .mx-dropdown-multiple-control-options-wrapper input{
        width:100%;
        box-sizing:border-box;
    }
    .mx-dropdown-multiple-control-options-available{
        height:100px;
        overflow-y:auto;
    }
    .mx-dropdown-multiple-control-options-searcher{
        display:flex;
    }
    .mx-select-option-selected-close{
        border-style:none;
        background-color:white;
    }
    .fa{
        display:flex;
        align-items: center;
        border-style:solid;
        border-width:1px 1px 1px 0px;
    }
    .mx-dropdown-multiple-control-options-searcher-txt{
        border-width:1px 0px 1px 1px;
    }
    .mx-dropdown-multiple-control-options-selected.invalid
        {
            border: 1px dotted red;
            background-color: #FFF8F8;
        }
    `]

}

customElements.define('mx-multi-select', MxMultiSelect)