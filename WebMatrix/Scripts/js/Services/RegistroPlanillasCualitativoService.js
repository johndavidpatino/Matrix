import { FetchWrapper } from './FetchWrapper.js'

export const RegistroPlanillasCualitativoService = {
    GetTecnicas,
    GetModeradores,
    GetPlanillaModeracionById,
    SaveModeracion,
    SaveInformes,
    GetPlanillaInformesById,
    GetJobsBy,
    SaveStatusAprobacionModeracion,
    GetPlanillas,
    SaveStatusAprobacionInformes,
    ExportExcelPlanillasBy,
    GetGerentesCuentasUU
}

function GetTecnicas({ TipoTecnica: TipoTecnica }) {
    let model = {
        TipoTecnica: TipoTecnica
    }

    return FetchWrapper.post({ URLPart: `PY_Proyectos/RegistroPlanillasCualitativo.aspx/Tecnicas`, params: model })
}
function GetModeradores() {
    let model = {
    }

    return FetchWrapper.post({ URLPart: `PY_Proyectos/RegistroPlanillasCualitativo.aspx/Moderadores`, params: model })
}

function GetPlanillaModeracionById({ idPlanilla: idPlanilla }) {
    let model = {
        idPlanilla: idPlanilla
    }

    return FetchWrapper.post({ URLPart: `PY_Proyectos/RegistroPlanillasCualitativo.aspx/PlanillaModeracionBy`, params: model })
}

function GetPlanillasInformes() {
    let model = {}

    return FetchWrapper.post({ URLPart: `PY_Proyectos/RegistroPlanillasCualitativo.aspx/PlanillasInformes`, params: model })
}

function GetPlanillaInformesById({ idPlanilla: idPlanilla }) {
    let model = {
        idPlanilla: idPlanilla
    }

    return FetchWrapper.post({ URLPart: `PY_Proyectos/RegistroPlanillasCualitativo.aspx/PlanillaInformeBy`, params: model })
}

function GetPlanillas({ pageSize, pageIndex, filtroPlanilla, idEstado }) {
    let model = {
        pageSize: pageSize,
        pageIndex: pageIndex,
        filtroPlanilla: filtroPlanilla,
        idEstado: idEstado,
    }

    return FetchWrapper.post({ URLPart: `PY_Proyectos/RegistroPlanillasCualitativo.aspx/PlanillasGet`, params: model })
}

function SaveModeracion({ IdJob, jobDesc, fecha, hora, tecnica, tiempo, moderador, rol, Observaciones, IdCuentasUU, ServiceLineName }) {
    let model = {
        IdJob: IdJob,
        jobDesc: jobDesc,
        fecha: fecha,
        hora: hora,
        tecnica: tecnica,
        tiempo: tiempo,
        moderador: moderador,
        rol: rol,
        Observaciones: Observaciones,
        IdCuentasUU: IdCuentasUU,
        ServiceLineName : ServiceLineName
    }

    return FetchWrapper.post({ URLPart: `PY_Proyectos/RegistroPlanillasCualitativo.aspx/SavePlanillaModeracion`, params: model })
}

function SaveInformes({ IdJob, jobDesc, fecha, tecnica, muestra, IdCuentasUU, analista, Observaciones, ServiceLineName }) {
    let model = {
        IdJob: IdJob,
        jobDesc: jobDesc,
        fecha: fecha,
        tecnica: tecnica,
        muestra: muestra,
        IdCuentasUU: IdCuentasUU,
        analista: analista,
        Observaciones: Observaciones,
        ServiceLineName: ServiceLineName
    }

    return FetchWrapper.post({ URLPart: `PY_Proyectos/RegistroPlanillasCualitativo.aspx/SavePlanillaInformes`, params: model })
}

function GetJobsBy({ termToSearch: termToSearch }) {
    let model = {
        termToSearch: termToSearch
    }

    return FetchWrapper.post({ URLPart: `PY_Proyectos/RegistroPlanillasCualitativo.aspx/GetJobsBy`, params: model })
}

function SaveStatusAprobacionModeracion({ idPlanilla, idJob, idEstado, observaciones, biStatus, biDinero}) {
    let model = {
        idPlanilla: idPlanilla,
        idJob: idJob,
        idEstado: idEstado,
        observaciones: observaciones,
        biStatus: biStatus,
        biDinero: biDinero
    }

    return FetchWrapper.post({ URLPart: `PY_Proyectos/RegistroPlanillasCualitativo.aspx/SaveStatusAprobacionModerarion`, params: model })
}

function SaveStatusAprobacionInformes({ idPlanilla, idJob, idEstado, observaciones, biStatus, biDinero }) {
    let model = {
        idPlanilla: idPlanilla,
        idJob: idJob,
        idEstado: idEstado,
        observaciones: observaciones,
        biStatus: biStatus,
        biDinero: biDinero,
    }

    return FetchWrapper.post({ URLPart: `PY_Proyectos/RegistroPlanillasCualitativo.aspx/SaveStatusAprobacionInformes`, params: model })
}

function ExportExcelPlanillasBy({ fechaInicio: fechaInicio, fechaFinal: fechaFinal, tipoPlanilla: tipoPlanilla }) {
    let model = {
        fechaInicio: fechaInicio,
        fechaFinal: fechaFinal,
        tipoPlanilla: tipoPlanilla
    }

    return FetchWrapper.post({ URLPart: `PY_Proyectos/RegistroPlanillasCualitativo.aspx/ExportExcelPlanillasBy`, params: model })
}
function GetGerentesCuentasUU() {
    let model = {
    }

    return FetchWrapper.post({ URLPart: `PY_Proyectos/RegistroPlanillasCualitativo.aspx/GerentesCuentasUU`, params: model })
}