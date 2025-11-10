import { indicadores,instrumentsOptions,tasksOptions } from '/Scripts/js/reportes/filters-data/filters-data.js'
import { GetTasks } from '/Scripts/js/reportes/services/registro-observaciones.service.js'


let procesos = null
let fuentes = null
export const GetProcesos = async () => {

    if(procesos != null) return Promise.resolve(procesos)

    let response = await fetch('/SGC_Calidad/AccionesMejora/SGC_AccionesMejora.aspx/Processes',{
        method: 'POST',
        headers: {
            'Content-Type': 'application/json'
        }
    })
    procesos = await response.json()
    return procesos
}

export const GetFuentes = async () => {
    if(fuentes != null) return Promise.resolve(fuentes)
    let response = await fetch('/SGC_Calidad/AccionesMejora/SGC_AccionesMejora.aspx/Fuentes',{
        method: 'POST',
        headers: {
            'Content-Type': 'application/json'
        }
    })
    fuentes = await response.json()
    return fuentes
}

/**
 * Adds a new "Accion Mejora".
 *
 * @param {Object} request - The request object containing the details of the improvement action.
 * @param {string|null} request.descripcionAccion - Description of the action, or null if not provided.
 * @param {string} request.fechaIncidente - Date of the incident.
 * @param {string|null} request.accionCorrectiva - Corrective action, or null if not provided.
 * @param {string|null} request.oportunidadMejora - Improvement opportunity, or null if not provided.
 * @param {number} request.usuarioReporta - ID of the reporting user.
 * @param {number|null} request.procesoId - ID of the process, or null if not provided.
 * @param {number|null} request.usuarioResponsable - ID of the responsible user, or null if not provided.
 * @param {string|null} request.descripcionNoConformidad - Description of the non-conformity, or null if not provided.
 * @param {string|null} request.correccion - Correction, or null if not provided.
 * @param {number|null} request.fuenteNoConformidadId - ID of the non-conformity source, or null if not provided.
 * @returns {Promise<void>} A promise that resolves when the action is added.
 */
export const AddAccionMejora = async (request) => {
    let result = fetch('/SGC_Calidad/AccionesMejora/SGC_AccionesMejora.aspx/AcccionesMejora',{
        method: 'POST',
        headers: {
            'Content-Type': 'application/json'
        },
        body: JSON.stringify({request:request})
    })

    return result
}

export const GetAccionesMejora = async (request) => {
    let response = await fetch('/SGC_Calidad/AccionesMejora/SGC_AccionesMejora.aspx/GetAccionesMejora',{
        method: 'POST',
        headers: {
            'Content-Type': 'application/json'
        },
        body: JSON.stringify({request:request})
    })
    return response
}

export const DeleteActionMejora = async (accionMejoraId) => {
    let response = await fetch('/SGC_Calidad/AccionesMejora/SGC_AccionesMejora.aspx/DeleteAccionMejora',{
        method: 'POST',
        headers: {
            'Content-Type': 'application/json'
        },
        body: JSON.stringify({accionMejoraId:accionMejoraId})
    })
    return response
}

export const GetAccionMejoraById = async (accionMejoraId) => { 
    let response = await fetch('/SGC_Calidad/AccionesMejora/SGC_AccionesMejora.aspx/GetAccionMejoraById',{
        method: 'POST',
        headers: {
            'Content-Type': 'application/json'
        },
        body: JSON.stringify({accionMejoraId:accionMejoraId})
    })
    return response
}

export const EditAccionMejora = async (request) => {
    let response = await fetch('/SGC_Calidad/AccionesMejora/SGC_AccionesMejora.aspx/UpdateAccionMejora',{
        method: 'POST',
        headers: {
            'Content-Type': 'application/json'
        },
        body: JSON.stringify({request:request})
    })
    return response
}

export const GetAuditoriasInternas = async () => {
    let response = await fetch('/SGC_Calidad/AccionesMejora/SGC_AccionesMejora.aspx/GetAuditoriasInternas',{
        method: 'POST',
        headers: {
            'Content-Type': 'application/json'
        }
    })
    return response
}


export const TransformAnalisisIndicadores = async (item,cumplimientoProcesos) => {
    let filters = []
    let processTasks = {}
    if(item.Indicador == indicadores.REGISTRO_OBSERVACIONES){
        let instrumento = instrumentsOptions.find(i => i.value == item.IdInstrumento)
        let tarea = tasksOptions.find(t => t.value == item.IdTarea)
        filters.push(tarea.label)
        filters.push(instrumento.label)
    }
    if(item.Indicador == indicadores.CUMPLIMIENTO_TAREAS){
        let idProceso = item.IdProceso

        if(!processTasks[idProceso]){
            processTasks[idProceso] = await GetTasks(idProceso)
        }
        
        let tasks = processTasks[idProceso]

        filters.push(cumplimientoProcesos[item.IdProceso].Unidad)
        filters.push(tasks[item.IdTarea].label)
    }
    return {
        value: item.IdAnalisis,
        label: `${item.IdAnalisis} - ${item.Indicador} - ${filters.join(' - ')} 📅 ${item.Periodo} - ${item.Trimestre}`
    }
}

export const TransformAuditoriaInterna = (item) => {
    return {
        value: item.Id,
        label: `${item.Id}. [ ${item.AreaAuditada} - ${item.ProcesoAuditado} ]  📅 ${item.FechaRegistro}`
    }
}