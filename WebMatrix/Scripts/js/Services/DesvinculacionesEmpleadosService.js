import { FetchWrapper } from './FetchWrapper.js'

export const DesvinculacionesEmpleadosService = {
    ProcesosDesvinculacionPendientesPorArea,
    ProcesosDesvinculacionPendientesPorEvaluarUsuarioActual,
    ItemsAVerificarPor,
    InformacionEmpleadoPor,
    GuardarEvaluacion,
    EvaluacionesRealizadasPorUsuarioActual,
    DesvinculacionesEmpleadosEstatus,
    DesvinculacionEmpleadosEstatusEvaluaciones,
    DownloadPDFFormat,
    IniciarProcesoDesvinculacion
}

function ProcesosDesvinculacionPendientesPorArea({ AreaId }) {
    let model = {
        AreaId,
    }

    return FetchWrapper.post({ URLPart: `TH_TalentoHumano/DesvinculacionesEmpleadosGestionArea.aspx/ProcesosDesvinculacionPendientesPorArea`, params: model })
}
function ProcesosDesvinculacionPendientesPorEvaluarUsuarioActual() {
    let model = {
    }

    return FetchWrapper.post({ URLPart: `TH_TalentoHumano/DesvinculacionesEmpleadosGestionArea.aspx/ProcesosDesvinculacionPendientesPorEvaluarUsuarioActual`, params: model })
}

function ItemsAVerificarPor({ AreaId }) {
    let model = {
        AreaId,
    }

    return FetchWrapper.post({ URLPart: `TH_TalentoHumano/DesvinculacionesEmpleadosGestionArea.aspx/ProcesosDesvinculacionItemsVerificarPor`, params: model })
}

function InformacionEmpleadoPor({ DesvinculacionEmpleadoId }) {
    let model = {
        DesvinculacionEmpleadoId,
    }

    return FetchWrapper.post({ URLPart: `TH_TalentoHumano/DesvinculacionesEmpleadosGestionArea.aspx/InformacionEmpleadoPor`, params: model })
}

function GuardarEvaluacion({ Evaluacion }) {
    let model = {
        DesvinculacionEmpleadoEvaluacion: Evaluacion
    }

    return FetchWrapper.post({ URLPart: `TH_TalentoHumano/DesvinculacionesEmpleadosGestionArea.aspx/GuardarEvaluacion`, params: model })
}

function EvaluacionesRealizadasPorUsuarioActual() {
    let model = {
    }

    return FetchWrapper.post({ URLPart: `TH_TalentoHumano/DesvinculacionesEmpleadosGestionArea.aspx/EvaluacionesRealizadasPorUsuarioActual`, params: model })
}

function DesvinculacionesEmpleadosEstatus({ pageSize, pageIndex, textoBuscado }) {
    let model = {
        pageSize,
        pageIndex,
        textoBuscado
    }

    return FetchWrapper.post({ URLPart: `TH_TalentoHumano/DesvinculacionesEmpleadosGestionRRHH.aspx/DesvinculacionesEmpleadosEstatus`, params: model })
}

function DesvinculacionEmpleadosEstatusEvaluaciones({ desvinculacionEmpleadoId }) {
    let model = {
        desvinculacionEmpleadoId,
    }

    return FetchWrapper.post({ URLPart: `TH_TalentoHumano/DesvinculacionesEmpleadosGestionRRHH.aspx/DesvinculacionEmpleadosEstatusEvaluacionesPor`, params: model })
}
function DownloadPDFFormat({ desvinculacionEmpleadoId }) {
    let model = {
        desvinculacionEmpleadoId,
    }

    return FetchWrapper.post({ URLPart: `TH_TalentoHumano/DesvinculacionesEmpleadosGestionRRHH.aspx/PDFFormato`, params: model })
}
function IniciarProcesoDesvinculacion({ empleadoId, fechaRetiro, motivosDesvinculacion }) {
    let model = {
        empleadoId,
        fechaRetiro,
        motivosDesvinculacion
    }

    return FetchWrapper.post({ URLPart: `TH_TalentoHumano/DesvinculacionesEmpleadosGestionRRHH.aspx/IniciarProcesoDesvinculacion`, params: model })
}