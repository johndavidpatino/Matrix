import { FetchWrapper } from './FetchWrapper.js'

export const UsuariosService = {
    UsuariosXRol,
    UsuarioTieneRolCalidad
}

function UsuariosXRol(rolId) {
    let model = {
        rolId
    }

    return FetchWrapper.post({ URLPart: `SGC_Calidad/AuditoriasInternas/NuevaAuditoria.aspx/UsuariosXRol`, params: model })
}
function UsuarioTieneRolCalidad() {
    let model = {
    }

    return FetchWrapper.post({ URLPart: `SGC_Calidad/AuditoriasInternas/Auditor.aspx/UsuarioTieneRolCalidad`, params: model })
}