import { filtersGroup, unidades, indicadores, tasksOptions, instrumentsOptions, groupOptions } from "./filters-data/filters-data.js"

import { ChartRequest, GetUserInfo } from "./services/registro-observaciones.service.js"

import "./analisis-table.js";

const yearFilter$ = document.getElementById("yearFilter")
const taskFilter$ = document.getElementById("taskFilter")
const instFilter$ = document.getElementById("instFilter") 
const filterSubmitBtn$ = document.getElementById("filterSubmitBtn")
const unidadSelector$ = document.getElementById("unidadSelector")
const analisisSection$ = document.getElementById("analisisSection")
const groupFilter$ = document.getElementById("groupFilter")

const currentYear = new Date().getFullYear();
const startYear = 2020
let request = {
    idTarea: null,
    ano: currentYear,
    mes: null,
    agruparPor: 3,
    idInstrumento: null,
    indicador:indicadores.REGISTRO_BOSERVACIONES,
    idUnidad: null
}



taskFilter$.options =tasksOptions
instFilter$.options =instrumentsOptions
groupFilter$.options =groupOptions


yearFilter$.minValue = startYear
yearFilter$.maxValue = currentYear
yearFilter$.defaultValue = currentYear

const userInfo = await GetUserInfo()
const isQAUser = userInfo.find(info => info.IdUnidad == 6)

analisisSection$.style.display = isQAUser ? "none" : "block"

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


taskFilter$.addEventListener('mxChange',event =>{
    let value = event.detail.value
    if(value === "") {
        instFilter$.classList.add('mx-hidden')
        return
    }
    let taskValue = parseInt(value)
    if(taskValue === 1){
        instFilter$.classList.remove('mx-hidden')
    }
    else {
        request.idInstrumento = null
        instFilter$.classList.add('mx-hidden')
    }
})




const chartContainer$ = document.getElementById("chartContainer")

filterSubmitBtn$.addEventListener('mxClick', async event => {
    event.preventDefault()
    filterSubmitBtn$.setLoading(true)
    let filters = GetFilters()  
    let chartRequest = new ChartRequest(filters)
    let data = await chartRequest.FetchChartData('IndicadoresRegistroObservaciones.aspx/ErroresRegistroObservaciones')
    
    // Show the chart container
    chartContainer$.classList.remove('mx-hidden')
    document.dispatchEvent(new CustomEvent('changeFilters', {detail:{data:data, filters:filters}}))

    filterSubmitBtn$.setLoading(false)
})

export const GetFilters = () => {
    request.idUnidad = parseInt(unidadSelector$.value) ?? null
    request.ano = parseInt(yearFilter$.value) ?? null
    request.idTarea = parseInt(taskFilter$.value) ?? null
    request.idInstrumento = parseInt(instFilter$.value) ?? null
    request.agruparPor = filtersGroup[request.idTarea]  ?? 1
    request.indicador = 'registro de observaciones'
    request.agruparPor = groupFilter$.value

    return request
}

