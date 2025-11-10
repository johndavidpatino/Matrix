import { unidades } from "../filters-data/filters-data.js"


let userInfo = null
let tareasProcess = null

export class ChartRequest {
    request
    constructor(request) {
        this.request = request
    }



    FetchChartData = async function (endPoint) {
        let result = await fetch(endPoint,
            {
                method: 'POST',
                headers: {
                    'Content-Type': 'application/json'
                },
                body: JSON.stringify(this.request)
            }
        )

        let json = await result.json()

        
        let userAreas = this.request.idUnidad ? unidades.filter(un => un.idUnidad == this.request.idUnidad) : unidades

        let response = []
        if (this.request.agruparPor == 3) {
            response = json.d.filter(row => userAreas.findIndex(un => un.NombreUnidad === row.Grupo) > -1)
            return response
        }

        return json.d
    }

}

export const GetUserInfo = async function () {
    if(userInfo) return userInfo
    let result = await fetch('../../../RP_Reportes/IndicadoresRegistroObservaciones.aspx/UserInfo', {
        method: 'POST',
        headers: {
            'Content-Type': 'application/json'
        }
    })

    let json = await result.json()
    userInfo = json.d
    return userInfo
}

export const AddAnalisis = async function ({ usuarioRegistra, fechaRegistro, contenido, periodo, trimestre, indicador, agruparPor, idInstrumento, idTarea, idUnidad, idProceso }) {

    //TODO: Validar request

    let result = await fetch('/RP_Reportes/IndicadoresRegistroObservaciones.aspx/AddAnalisis', {
        method: 'POST',
        headers: {
            'Content-Type': 'application/json'
        },
        body: JSON.stringify({ request: { usuarioRegistra, fechaRegistro, contenido, periodo, trimestre, indicador, agruparPor, idInstrumento, idTarea, idUnidad, idProceso } })
    })

    

    return result
}

export const GetAnalisis = async function ({ indicador, idUnidad, periodo,usuarioRegistra, idTarea, idInstrumento, idProceso,pageSize,pageNumber }) {
    let result = await fetch('/RP_Reportes/IndicadoresRegistroObservaciones.aspx/GetAnalisis', {
        method: 'POST',
        headers: {
            'Content-Type': 'application/json'
        },
        body: JSON.stringify({ request: { indicador, idUnidad, periodo,usuarioRegistra, idTarea, idInstrumento, idProceso,pageSize,pageNumber } })
    })

    let json = await result.json()

    return json.d
}

export const DeleteAnalisis = async function ({ idAnalisis }) {
    let result = await fetch('/RP_Reportes/IndicadoresRegistroObservaciones.aspx/DeleteAnalisis', {
        method: 'POST',
        headers: {
            'Content-Type': 'application/json'
        },
        body: JSON.stringify({ idAnalisis: idAnalisis })
    })

    return result
}

export const GetProcesses = async function () {
    if(tareasProcess !== null) return tareasProcess
    let result = await fetch('IndicadoresCumplimientoTareas.aspx/Procesos', {
        method: 'POST',
        headers: {
            'Content-Type': 'application/json'
        }
    })

    let json = await result.json()

    if(result.ok){
        tareasProcess = json.d
    }

    return json.d
}

export const GetTasks = async function (idProceso) {
    let result = await fetch('IndicadoresCumplimientoTareas.aspx/Tareas', {
        method: 'POST',
        headers: {
            'Content-Type': 'application/json'
        },
        body: JSON.stringify({ idUnidad: idProceso })
    })

    let json = await result.json()

    return json.d
}

export const GetAllTasks = async function () {
    let result = await fetch('IndicadoresCumplimientoTareas.aspx/TodasTareas', {
        method: 'POST',
        headers: {
            'Content-Type': 'application/json'
        }
    })

    let json = await result.json()

    return json.d
}

export const UpdateAnalisis = async function ({ idAnalisis, content }) {
    let result = await fetch('/RP_Reportes/IndicadoresRegistroObservaciones.aspx/UpdateAnalisis', {
        method: 'POST',
        headers: {
            'Content-Type': 'application/json'
        },
        body: JSON.stringify({ idAnalisis: idAnalisis, content: content })
    })

    return result
}

export const ExportDetallesExcel = async function (request) {
    let result = await fetch('/RP_Reportes/Calidad/IndicadoresRegistroObservaciones.aspx/Detalles', {
        method: 'POST',
        headers: {
            'Content-Type': 'application/json; charset=utf-8'
        },
        body: JSON.stringify({ request: request })
    })

    return result
}

export const ExportDatosAgrupadosExcel = async function (request) {
    let result = await fetch('/RP_Reportes/IndicadoresRegistroObservaciones.aspx/ErroresRegistroObservacionesExcel', {
        method: 'POST',
        headers: {
            'Content-Type': 'application/json; charset=utf-8'
        },
        body: JSON.stringify(request)
    })

    return result
}

export const ExportCumplimientoTareasExcel = async function (request) {
    let result = await fetch('IndicadoresCumplimientoTareas.aspx/CumplimientoTareasAgrupadoExcel', {
        method: 'POST',
        headers: {
            'Content-Type': 'application/json; charset=utf-8'
        },
        body: JSON.stringify({request:request})
    })

    return result
}

export const ExportCumplimientoTareasDetallesExcel = async function (request) {
    let result = await fetch('IndicadoresCumplimientoTareas.aspx/CumplimientoTareasDetallesExcel', {
        method: 'POST',
        headers: {
            'Content-Type': 'application/json; charset=utf-8'
        },
        body: JSON.stringify({request:request})
    })

    return result
}


export const ExportCumplimientoTareasCOEExcel = async function (request) {
    let result = await fetch('IndicadoresCumplimientoTareas.aspx/CumplimientoTareasCOEExcel', {
        method: 'POST',
        headers: {
            'Content-Type': 'application/json; charset=utf-8'
        },
        body: JSON.stringify({request:request})
    })

    return result
}

export const ExportCumplimientoTareasCOEDetallesExcel = async function (request) {
    let result = await fetch('IndicadoresCumplimientoTareas.aspx/CumplimientoTareasCOEDetallesExcel', {
        method: 'POST',
        headers: {
            'Content-Type': 'application/json; charset=utf-8'
        },
        body: JSON.stringify({request:request})
    })

    return result
}