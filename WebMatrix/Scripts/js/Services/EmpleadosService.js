import { FetchWrapper } from './FetchWrapper.js'

export const EmpleadosService = {
    EmpleadosActivos
}

function EmpleadosActivos() {
    let model = {
    }

    return FetchWrapper.post({ URLPart: `TH_TalentoHumano/DesvinculacionesEmpleadosGestionRRHH.aspx/EmpleadosActivos`, params: model })
}