import { html } from '/Scripts/js/libs/lit/lit.js';
export const descripciones = [
    { label: 'Oportunidad de mejora', value: 'Oportunidad de mejora' },
    { label: 'Accion correctiva', value: 'Accion correctiva' }
]

export const GetFormAccionMejora = (title) => {
    return `
    <header class="mx-modal-form-header">
        <mx-typo type="title">${title}</mx-typo>
        <button class="mx-button-icon" type="button" id="btnAccionMejoraClose">
            <svg xmlns="http://www.w3.org/2000/svg" fill="none" viewBox="0 0 24 24" stroke-width="1.5" stroke="currentColor" class="size-6">
                <path stroke-linecap="round" stroke-linejoin="round" d="M6 18 18 6M6 6l12 12" />
            </svg>
        </button>
    </header>
    <div class="mx-form-group">
        <mx-custom-select id="descripcionSelector">
            <mx-typo tag="span" type="detail">Descripción</mx-typo>
        </mx-custom-select>
        <mx-field-date id="fechaSelector">
            <mx-typo tag="span" type="detail">Fecha Incidente</mx-typo>
        </mx-field-date>
    </div>
    <div class="mx-form-group">
        <mx-user-select id="usuarioReportaSelector" canSearch>
            <mx-typo tag="span" type="detail">Usuario que reporta</mx-typo>
        </mx-user-select>
        <mx-user-select id="usuarioResponsableSelector" canSearch>
            <mx-typo tag="span" type="detail">Usuario responsable</mx-typo>
        </mx-user-select>
        <mx-custom-select id="procesoSelector">
            <mx-typo tag="span" type="detail">Proceso</mx-typo>
        </mx-custom-select>
    </div>
    <mx-custom-select id="tipoFuenteSelector" disabled>
        <mx-typo tag="span" type="detail" >Fuente de la No Conformidad</mx-typo>
    </mx-custom-select>
    <div id="analisisIndicadorContainer">

    </div>
    <div class="mx-form-group">
        <mx-text-area id="correccion">
            <mx-typo tag="span" type="detail">Corrección</mx-typo>
        </mx-text-area>
        <mx-text-area id="descripcionNoConformidad">
            <mx-typo tag="span" type="detail">Descripción de la No Conformidad / Oportunidad de mejora</mx-typo>
        </mx-text-area>
    </div>
    <mx-typo tag="span" type="body">Causas</mx-typo>
    <mx-form-manager id="causasForm">
        <div class="mx-form-group">
            <mx-field-text id="causa" name="DescripcionCausa">
                ¿Por qué?
            </mx-field-text>
            <mx-button type="submit" id="btnAgregarCausa">Agregar</mx-button>
        </div>
    </mx-form-manager>

    <mx-table id="causasTable">
    </mx-table>
    <mx-typo tag="span" type="body">Planes de acción</mx-typo>
    <mx-form-manager id="planesAccionForm">
        <div class="mx-form-group">
            <mx-field-text name="DescripcionPlan">
                Plan de Acción
            </mx-field-text>
            <mx-field-date name="FechaPlaneado">
                Planeado para
            </mx-field-date>
            <mx-button type="submit" id="btnAgregarPlanAccion">Agregar</mx-button>
        </div>
    </mx-form-manager>
    <mx-table id="planesAccionTable">
    </mx-table>
    <div class="mx-form-group">
        <mx-button id="btnGuardar">Guardar</mx-button>
    </div>
`
}
export class Subject {

    constructor(){
        this.observers = []
    }

    Suscribe(observer){
        this.observers.push(observer)
    }

    Unsuscribe(observer){
        this.observers = this.observers.filter(obs => obs !== observer);
    }

    Notify(data){
        this.observers.forEach(observer => observer.Update(data))
    }
}

export class Observer {
    constructor(){

    }

    Update(data){
        console.log(data)
    }
}


function planeadoValidation(value){
    let date = new Date(value)
    let today = new Date()
    if(date < today){
        return {
            isValid: false,
            errorMessage: 'La fecha no puede ser menor a la fecha actual'
        }
    }
    return {
        isValid: true,
        errorMessage: ''
    }
}

export const PlanAccionContentModalEdit = (isQAUser = false) => {

    return `<div class="mx-section mx-section-modal--sm">
                <div class="mx-form-container">
                    <mx-form-manager id="planAccionFormEdit">
                        <div class="mx-form-group">
                            <mx-field-text name="DescripcionPlan" id="descripcionPlanEdit">
                                Descripción
                            </mx-field-text>
                            <mx-field-date name="FechaPlaneado" id="fechaPlaneadoEdit">
                                Fecha Planeado
                            </mx-field-date>
                            <mx-field-date name="FechaEjecutado" id="fechaEjecutadoEdit">
                                Fecha Ejecutado
                            </mx-field-date>
                            ${isQAUser? `
                            <mx-field-text name="EficaciaPlan" id="eficaciaPlanEdit">
                                Eficacia del Plan
                            </mx-field-text>
                            <mx-field-date name="FechaRevision" id="fechaRevisionEdit">
                                Fecha Revisión
                            </mx-field-date>
                            ` 
                            :''}
                            <mx-button type="submit" id="btnGuardarPlanAccion">Guardar</mx-button>
                        </div>
                    </mx-form-manager>
                </div>
            </div>`
}

export const CausaContentModalEdit = () => {
    return `<div class="mx-section mx-section-modal--sm">
                <div class="mx-form-container">
                    <mx-form-manager id="causaFormEdit">
                        <div class="mx-form-group">
                            <mx-field-text name="DescripcionCausa" id="descripcionCausaEdit">
                                Descripción
                            </mx-field-text>
                            <mx-button type="submit" id="btnGuardarCausa">Guardar</mx-button>
                        </div>
                    </mx-form-manager>
                </div>
            </div>`
}

export const ProcesosConstants = {
    INDICADORES: 4,
    AUDITORIA_INTERNA: 2,
}