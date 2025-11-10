import { FetchWrapper } from './FetchWrapper.js'

export const SGC_AuditoriasInternasService = {
    Nueva,
    AuditoriasBy,
    AuditoriaInformeAuditorAdd,
    AuditoriaInformeAuditorBy
}

function Nueva({ auditoriId, areaAuditada, procesoAuditado, fechaLimiteAuditoria, tiposAuditoria, normativasAAuditar }) {
    let nuevaAuditoria = {
        AuditoriId: auditoriId,
        AreaAuditada: areaAuditada,
        ProcesoAuditado: procesoAuditado,
        FechaLimiteAuditoria: fechaLimiteAuditoria,
        TiposAuditoria: tiposAuditoria,
        NormativasAAuditar: normativasAAuditar
    }
    let model = {
        nuevaAuditoria
    }

    return FetchWrapper.post({ URLPart: `SGC_Calidad/AuditoriasInternas/Auditor.aspx/Nueva`, params: model })
}
function AuditoriasBy({ estadoId, anoAuditoria, pageIndex, pageSize }) {
    let model = {
        estadoId,
        anoAuditoria,
        pageIndex,
        pageSize
    }
    return FetchWrapper.post({ URLPart: `SGC_Calidad/AuditoriasInternas/Auditor.aspx/AuditoriasBy`, params: model })
}
function AuditoriaInformeAuditorAdd({ auditoriaId, fechaAuditoria, fortalezas, hallazgos, auditados, fileEvidenciaBase64, fileEvidenciaNameConExtension }) {
    let model = {
        auditoriaInforme: {
            AuditoriaId: auditoriaId,
            FechaAuditoria: fechaAuditoria,
            Fortalezas: fortalezas,
            Hallazgos: hallazgos,
            Auditados: auditados,
            ArchivoEvidencia: {
                NombreArchivoConExtension: fileEvidenciaNameConExtension,
                ArchivoBase64: fileEvidenciaBase64
            }
        }

    }
    return FetchWrapper.post({ URLPart: `SGC_Calidad/AuditoriasInternas/Auditor.aspx/AuditoriaInformeAuditorAdd`, params: model })
}
function AuditoriaInformeAuditorBy({ auditoriaId }) {
    let model = {
        auditoriaId
    }
    return FetchWrapper.post({ URLPart: `SGC_Calidad/AuditoriasInternas/Auditor.aspx/InformeAuditorBy`, params: model })
}
