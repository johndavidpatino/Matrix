import { unidades,indicadores,groupOptionsCumplimientoTareas } from "./filters-data/filters-data.js"

import { ChartRequest, GetUserInfo, GetProcesses, GetTasks } from "./services/registro-observaciones.service.js"
import { RenderSelectOptions } from "./utilities.js"

import "./main-chart.js";
import "./analisis-table.js";

const yearFilter$ = document.getElementById("yearFilter")
const taskFilter$ = document.getElementById("taskFilter")
const processFilter$ = document.getElementById("processFilter") 
const unidadSelector$ = document.getElementById("unidadSelector")
const filterSubmitBtn$ = document.getElementById("filterSubmitBtn")
const analisisSection$ = document.getElementById("analisisSection")
const groupFilter$ = document.getElementById("groupFilter")

const currentYear = new Date().getFullYear();
const startYear = 2020
const years = []

let request = {
    ano: currentYear,
    mes: null,
    proceso: null,
    idTarea: null,
    agruparPor: 'proceso',
    grupoUnidad: null,
    indicador: indicadores.CUMPLIMIENTO_TAREAS,
    idUnidad: null,
    idProceso: null
}

taskFilter$.classList.add('mx-hidden')

yearFilter$.defaultValue = currentYear
yearFilter$.minValue = startYear
yearFilter$.maxValue = currentYear

const userInfo = await GetUserInfo()
const isQAUser = userInfo.find(info => info.IdUnidad == 6)

let userAreas = isQAUser ? unidades : userInfo.map(info => {
    return unidades.find(un => un.idUnidad == info.IdGrupoUnidad)
})

unidadSelector$.HasEmptyOption = isQAUser

unidadSelector$.options = userAreas.map(area => {
    return {
        label: area.NombreUnidad,
        value: area.idUnidad
    }
})



await groupFilter$.setOptions(groupOptionsCumplimientoTareas)


analisisSection$.style.display = isQAUser ? "none" : "block"



GetProcesses()
.then(procesos => {
    processFilter$.setOptions(procesos.map(p => {
        return {
            label: p.Unidad,
            value: p.id
        }
    }))
})


processFilter$.addEventListener('mxChange', async event => {
    let value = event.target.value
    if(value === "") {
        taskFilter$.classList.add('mx-hidden')
        return
    }

    let tasks = await GetTasks(parseInt(value))
    await taskFilter$.setOptions(tasks.map(t => {
        return {
            label: t.Tarea,
            value: t.id
        }
    }))
    taskFilter$.classList.remove('mx-hidden')
})

export const GetFilters = () => {
    request.proceso = parseInt(processFilter$.value) ?? null
    request.ano = parseInt(yearFilter$.value) ?? null
    request.idTarea = parseInt(taskFilter$.value)  ?? null
    request.grupoUnidad =  null
    request.idUnidad = parseInt(unidadSelector$.value) ?? null
    // request.idProceso = parseInt(processFilter$.value) ?? null
    return request
}

const chartContainer$ = document.getElementById("chartContainer")

filterSubmitBtn$.addEventListener('mxClick', async event => {
    event.preventDefault()
    filterSubmitBtn$.setLoading(true)
    let filters = GetFilters() 
    let chartRequest = new ChartRequest({request:filters})
    let data = await chartRequest.FetchChartData('IndicadoresCumplimientoTareas.aspx/IndicadorCumplimientoTareas')
    
    filterSubmitBtn$.setLoading(false)
    
    
    chartContainer$.classList.remove('mx-hidden')
    document.dispatchEvent(new CustomEvent('changeFilters', {detail:{data:data, filters:filters}}))
})

