import { GetProcesos, GetFuentes, AddAccionMejora, EditAccionMejora } from '/Scripts/js/acciones-mejora/services/acciones-mejora.services.js'
import { GetAnalisis, GetUserInfo, GetProcesses } from '/Scripts/js/reportes/services/registro-observaciones.service.js'
import { descripciones, GetFormAccionMejora, PlanAccionContentModalEdit } from '/Scripts/js/acciones-mejora/commons/common.js'
import { NotificationManager } from '../reportes/notifications/notifications.js'
import { DeleteActionMejora, GetAccionesMejora, GetAccionMejoraById, GetAuditoriasInternas, TransformAnalisisIndicadores, TransformAuditoriaInterna } from './services/acciones-mejora.services.js'
import { CausaContentModalEdit, ProcesosConstants } from './commons/common.js'


const btnNuevaAccion$ = document.getElementById('btnNuevaAccion')
const accionesMejoraTable$ = document.getElementById('accionesMejoraTable')
const notificationSystem = new NotificationManager('notification-container')

const userInfo = await GetUserInfo()

const isQAUser = userInfo.findIndex(info => info.IdUnidad == 6) > -1



accionesMejoraTable$.columns = [
    { name: 'AccionMejoraId', displayName: 'Id' },
    { name: 'DescripcionAccion', displayName: 'Descripcion' },
    { name: 'FechaIncidente', displayName: 'Fecha del Incidente' },
    { name: 'UsuarioReporta', displayName: 'Usuario que Reporta' },
    { name: 'Proceso', displayName: 'Proceso' },
    { name: 'UsuarioResponsable', displayName: 'Usuario Responsable' },
    { name: 'TipoFuente', displayName: 'Fuente No conformidad' }
]

accionesMejoraTable$.idData = "AccionMejoraId"


accionesMejoraTable$.actions = [
    { name: 'Editar', icon: 'edit', iconColor: '#6c757d', isEditAction: true, fun: EditAction }
]
if (!isQAUser) {
    accionesMejoraTable$.actions.unshift(
        { name: 'Eliminar', icon: 'trash-alt', iconColor: '#dc3545', iconHoverColor: '#dc3545', isDeleteAction: true, fun: DeleteAction }
    )
}

async function DeleteAction(row, callbackSuccess) {

    let modal = document.createElement('mx-modal')
    modal.title = 'Eliminar acción de mejora'
    modal.content = '¿Estás seguro de que deseas eliminar esta acción de mejora?'
    modal.confirmButtonText = 'Eliminar'
    modal.cancelButtonText = 'Cancelar'

    document.body.appendChild(modal)

    modal.cancelAction = async () => {
        callbackSuccess(false)
        return
    }

    modal.confirmAction = async () => {
        let result = await DeleteActionMejora(row.AccionMejoraId)

        if (result.ok) {
            callbackSuccess(true)
            notificationSystem.showSuccess("Acción de mejora eliminada")
            FetchAccionesMejora()
            return
        }
        notificationSystem.showError("No se pudo eliminar la acción de mejora")
        callbackSuccess(false)
    }
}

async function EditAction(row) {
    let result = await GetAccionMejoraById(row.AccionMejoraId)
    let json = await result.json()
    let storedData = json.d.Data
    const modal = document.createElement('mx-modal-container')
    modal.innerHTML = `
        <div class="mx-section mx-section-modal">
            <div class="mx-form-container">
                ${GetFormAccionMejora('Editar Acción de Mejora')}
            </div>
        </div> `
    document.body.appendChild(modal)
    initializeAccionesMejora(modal, storedData)
}

async function DeletePlanAccion(row, callbackSuccess) {
    callbackSuccess(true)
}

async function EditPlanAccion(row, editCallback) {
    let modal = document.createElement('mx-modal-container')
    modal.innerHTML = PlanAccionContentModalEdit(isQAUser)


    document.body.appendChild(modal)

    const modalForm$ = modal.querySelector('mx-form-manager')
    const descripcionPlanEdit$ = modal.querySelector('#descripcionPlanEdit')
    const fechaPlaneadoEdit$ = modal.querySelector('#fechaPlaneadoEdit')
    const eficaciaPlanEdit$ = modal.querySelector('#eficaciaPlanEdit')
    const fechaEjecutadoEdit$ = modal.querySelector('#fechaEjecutadoEdit')
    const fechaRevisionEdit$ = modal.querySelector('#fechaRevisionEdit')

    descripcionPlanEdit$.value = row.DescripcionPlan
    fechaPlaneadoEdit$.value = row.FechaPlaneado ? (new Date(row.FechaPlaneado)).toISOString().split('T')[0] : null
    fechaEjecutadoEdit$.value = row.FechaEjecutado ? (new Date(row.FechaEjecutado)).toISOString().split('T')[0] : null
    if (isQAUser) {
        eficaciaPlanEdit$.value = row.EficaciaPlan
        fechaRevisionEdit$.value = row.FechaRevision
    }


    modalForm$.addEventListener('mxSubmit', e => {
        e.preventDefault()
        let newPlan = {
            PlanAccionId: row.PlanAccionId,
            DescripcionPlan: descripcionPlanEdit$.value,
            FechaPlaneado: fechaPlaneadoEdit$.value,
            FechaEjecutado: fechaEjecutadoEdit$.value ?? null,
            EficaciaPlan: eficaciaPlanEdit$ ? eficaciaPlanEdit$.value : null,
            FechaRevision: fechaRevisionEdit$ ? fechaRevisionEdit$.value : null
        }

        editCallback(newPlan)
        modal.remove()
    })

}

async function DeleteCausa(row, callbackSuccess) {
    callbackSuccess(true)
}
async function EditCausa(row, editCallback) {
    let modal = document.createElement('mx-modal-container')
    modal.title = 'Editar causa'
    modal.innerHTML = CausaContentModalEdit()

    document.body.appendChild(modal)

    const descripcionCausaEdit$ = modal.querySelector('#descripcionCausaEdit')
    descripcionCausaEdit$.value = row.DescripcionCausa

    const modalForm$ = modal.querySelector('mx-form-manager')

    modalForm$.addEventListener('mxSubmit', e => {
        e.preventDefault()
        let newCausa = {
            CausaId: row.CausaId,
            DescripcionCausa: descripcionCausaEdit$.value
        }
        editCallback(newCausa)
        modal.remove()
    })
}

await GetFuentes()
    .then(json => {
        let fuentes = json.d.Data
        accionesMejoraTable$.transformations.push(
            (item) => {
                item.TipoFuente = fuentes.find(f => f.TipoFuenteId == item.tipoFuente).TipoFuenteNombre
                return item
            })
    })

await GetProcesos()
    .then(json => {
        let procesos = json.d.Data
        accionesMejoraTable$.transformations.push(
            (item) => {
                item.Proceso = procesos.find(f => f.ProcesoId == item.Proceso).NombreProceso
                return item
            })
    })

function FetchAccionesMejora() {

    GetAccionesMejora({ usuarioReporta: isQAUser ? null : userInfo[0].id })
        .then(response => response.json())
        .then(json => {
            accionesMejoraTable$.data = json.d.Data.map(item => {
                let newItem = {
                    AccionMejoraId: item.AccionMejoraId,
                    DescripcionAccion: item.DescripcionAccion,
                    FechaIncidente: (new Date(item.FechaIncidente)).toLocaleDateString(),
                    UsuarioReporta: item.UsuarioReporta,
                    Proceso: item.ProcesoId,
                    UsuarioResponsable: item.UsuarioResponsable,
                    tipoFuente: item.TipoFuenteId
                }
                return newItem
            })
        })
}

FetchAccionesMejora()



async function initializeAccionesMejora(modal, storedData = null) {
    const descripcionSelector$ = document.getElementById('descripcionSelector')
    const procesoSelector$ = document.getElementById('procesoSelector')
    const tipoFuenteSelector$ = document.getElementById('tipoFuenteSelector')
    const causasForm$ = document.getElementById('causasForm')
    const planesAccionForm$ = document.getElementById('planesAccionForm')
    const causasTable$ = document.getElementById('causasTable')
    const planesAccionTable$ = document.getElementById('planesAccionTable')
    const analisisIndicadorContainer$ = document.getElementById('analisisIndicadorContainer')
    const usuarioResponsableSelector$ = document.getElementById('usuarioResponsableSelector')
    const fechaSelector$ = document.getElementById('fechaSelector')
    const btnGuardar$ = document.getElementById('btnGuardar')
    const usuarioReportaSelector$ = document.getElementById('usuarioReportaSelector')
    const descripcionNoConformidad$ = document.getElementById('descripcionNoConformidad')
    const btnAccionMejoraClose$ = document.getElementById('btnAccionMejoraClose')
    const correccion$ = document.getElementById('correccion')

    const AddCausa = (e) => {
        e.preventDefault()
        let causa = e.detail.data

        causasTable$.data = [...causasTable$.data, causa]
        causasForm$.reset()
    }
    const AddPlanAccion = (e) => {
        e.preventDefault()
        let planAccion = e.detail.data
        planesAccionTable$.data = [...planesAccionTable$.data, planAccion]
        planesAccionForm$.reset()
    }
    causasForm$.addEventListener('mxSubmit', AddCausa)
    planesAccionForm$.addEventListener('mxSubmit', AddPlanAccion)

    planesAccionTable$.transformations = [
        (item) => {
            item.FechaPlaneado = item.FechaPlaneado ? (new Date(item.FechaPlaneado)).toLocaleDateString() : null
            item.FechaEjecutado = item.FechaEjecutado ? (new Date(item.FechaEjecutado)).toLocaleDateString() : null
            item.FechaRevision = item.FechaRevision ? (new Date(item.FechaRevision)).toLocaleDateString() : null
            return item
        }
    ]

    const LinkToResource = async (processType = ProcesosConstants.INDICADORES, defaultOption = null) => {
        let values = usuarioResponsableSelector$.getValue()
        if (values.length == 0) return;
        let userId = values[0].value

        let result = null

        let items = []

        if (processType == ProcesosConstants.AUDITORIA_INTERNA) {

            result = await GetAuditoriasInternas()
            let json = await result.json()
            items = json.d.Data.map(item => TransformAuditoriaInterna(item))

        }
        else if (processType == ProcesosConstants.INDICADORES) {
            let resultProcess = await GetProcesses()

            result = await GetAnalisis({ usuarioRegistra: userId, pageSize: 1000, pageNumber: 1 })

            let itemsPromises = result.Items.map(async item => {
                return TransformAnalisisIndicadores(item, resultProcess)
            })
            items = await Promise.all(itemsPromises)
        }




        items = items.sort((a, b) => a.label.localeCompare(b.label))

        analisisIndicadorContainer$.innerHTML = ''

        let select = document.createElement('mx-custom-select')

        select.innerHTML = processType == ProcesosConstants.AUDITORIA_INTERNA ? 'Vincular una Auditoría Interna' : 'Vincular un Análisis de Indicador'
        select.id = 'analisisIndicadorSelector'
        select.items = items
        select.canSearch = true
        select.initialOptions = defaultOption ? [{ value: defaultOption }] : []
        analisisIndicadorContainer$.appendChild(select)

        if (defaultOption) {
            select.initialOptions = [{ value: defaultOption }]
        }

    }

    causasTable$.columns = [
        { name: 'DescripcionCausa', displayName: 'Descripción' }
    ]
    planesAccionTable$.columns = [
        { name: 'DescripcionPlan', displayName: 'Descripción' },
        { name: 'FechaPlaneado', displayName: 'Fecha Planeado' },
        { name: 'FechaEjecutado', displayName: 'Fecha Ejecutado' },
        { name: 'EficaciaPlan', displayName: 'Eficacia del Plan' },
        { name: 'FechaRevision', displayName: 'Fecha de Revisión' }
    ]

    if (storedData !== null) {
        causasTable$.columns.unshift({ name: 'CausaId', displayName: 'Id' })
        planesAccionTable$.columns.unshift({ name: 'PlanAccionId', displayName: 'Id' })
    }

    causasTable$.idData = 'CausasId'

    planesAccionTable$.idData = 'PlanAccionId'


    causasTable$.actions = [
        { name: 'Editar', icon: 'edit', iconColor: '#6c757d', isEditAction: true, fun: EditCausa },
        { name: 'Eliminar', icon: 'trash-alt', iconColor: '#dc3545', iconHoverColor: '#dc3545', isDeleteAction: true, fun: DeleteCausa }
    ]
    planesAccionTable$.actions = [
        { name: 'Editar', icon: 'edit', iconColor: '#6c757d', isEditAction: true, fun: EditPlanAccion },
        { name: 'Eliminar', icon: 'trash-alt', iconColor: '#dc3545', iconHoverColor: '#dc3545', isDeleteAction: true, fun: DeletePlanAccion }
    ]


    if (storedData !== null) {
        descripcionSelector$.initialOptions = [{ value: storedData.DescripcionAccion }]

        procesoSelector$.initialOptions = [{ ProcesoId: storedData.ProcesoId }]
        tipoFuenteSelector$.initialOptions = [{ TipoFuenteId: storedData.TipoFuenteId }]
        tipoFuenteSelector$.disabled = false
    }

    let today = new Date()

    fechaSelector$.value = storedData !== null
        ? (new Date(storedData.FechaIncidente)).toISOString().split('T')[0]
        : today.toISOString().split('T')[0]

    usuarioReportaSelector$.initialOptions = storedData !== null
        ? [{ UsuarioId: storedData.UsuarioReporta }]
        : [{ UsuarioId: userInfo[0].id }]

    usuarioResponsableSelector$.initialOptions = storedData !== null ? [{ UsuarioId: storedData.UsuarioResponsable }] : []

    descripcionNoConformidad$.value = storedData !== null ? storedData.Descripcion : ''
    correccion$.value = storedData !== null ? storedData.Correccion : ''
    descripcionSelector$.items = descripciones

    causasTable$.data = storedData !== null
        ? storedData.Causas
        : []
    planesAccionTable$.data = storedData !== null
        ? storedData.PlanesAccion
        : []



    await GetProcesos()
        .then(json => {
            procesoSelector$.keyValue = 'ProcesoId'
            procesoSelector$.keyLabel = 'NombreProceso'
            procesoSelector$.items = json.d.Data
        })


    await GetFuentes()
        .then(json => {
            let fuentes = json.d.Data
            tipoFuenteSelector$.keyValue = 'TipoFuenteId'
            tipoFuenteSelector$.keyLabel = 'TipoFuenteNombre'
            tipoFuenteSelector$.items = fuentes
        })


    usuarioResponsableSelector$.addEventListener('mxChange', e => {
        if (e.detail.length == 0) {
            tipoFuenteSelector$.disabled = true
        } else {
            tipoFuenteSelector$.disabled = false
        }

    })
    if (storedData !== null && (storedData.TipoFuenteId == ProcesosConstants.AUDITORIA_INTERNA || storedData.TipoFuenteId == ProcesosConstants.INDICADORES)) {
        await LinkToResource(storedData?.TipoFuenteId, storedData?.FuenteId)
    }

    tipoFuenteSelector$.addEventListener('mxChange', async e => {
        let values = e.detail
        if (values.length == 0) return;
        let currentValue = values[0].value

        if (currentValue == ProcesosConstants.AUDITORIA_INTERNA || currentValue == ProcesosConstants.INDICADORES) {
            await LinkToResource(currentValue, storedData?.FuenteId)
        }
    })



    const CrearNuevaAccion = async (request) => {
        try {
            let result = await AddAccionMejora(request)
            if (result.ok) {
                notificationSystem.showSuccess('Acción de mejora registrada correctamente')
                modal.remove()
                FetchAccionesMejora()
                return
            }
            if (result.Message) {

                notificationSystem.showError(result.Message)
                return
            }
            notificationSystem.showError('Ocurrió un error al registrar la acción de mejora')
        } catch (error) {
            notificationSystem.showError('Ocurrió un error al registrar la acción de mejora')
        }
    }

    const EditarAccionMejora = async (request) => {
        try {
            let result = await EditAccionMejora(request)
            if (result.ok) {
                notificationSystem.showSuccess('Acción de mejora editada correctamente')
                modal.remove()
                FetchAccionesMejora()
                return
            }
            if (result.Message) {

                notificationSystem.showError(result.Message)
                return
            }
            notificationSystem.showError('Ocurrió un error al editar la acción de mejora')
        } catch (error) {
            notificationSystem.showError('Ocurrió un error al editar la acción de mejora')
        }
    }

    btnGuardar$.addEventListener('click', async e => {
        e.preventDefault()
        btnGuardar$.setLoading(true)
        let causas = causasTable$.data
        let planesAccion = planesAccionTable$.data

        const analisisIndicadorSelector$ = document.getElementById('analisisIndicadorSelector')

        let request = {
            accionMejoraId: storedData?.AccionMejoraId,
            descripcionAccion: descripcionSelector$.getValue().length > 0 ? descripcionSelector$.getValue()[0].value : null,
            fechaIncidente: fechaSelector$.value,
            usuarioReporta: usuarioReportaSelector$.getValue().length > 0 ? usuarioReportaSelector$.getValue()[0].value : null,
            procesoId: procesoSelector$.getValue().length > 0 ? procesoSelector$.getValue()[0].value : null,
            usuarioResponsable: usuarioResponsableSelector$.getValue().length > 0 ? usuarioResponsableSelector$.getValue()[0].value : null,
            descripcion: descripcionNoConformidad$.getValue(),
            correccion: correccion$.value,
            fuenteNoConformidadId: tipoFuenteSelector$.getValue().length > 0 ? tipoFuenteSelector$.getValue()[0].value : null,
            fuenteId: analisisIndicadorSelector$ ? analisisIndicadorSelector$.getValue().length > 0 ? analisisIndicadorSelector$.getValue()[0].value : null : null,
            causas: causas,
            planesAccion: planesAccion.map(plan => {
                let fechaPlaneado = new Date(plan.FechaPlaneado)
                plan.FechaPlaneado = fechaPlaneado.toISOString()
                plan.FechaEjecutado = plan.FechaEjecutado ? (new Date(plan.FechaEjecutado)).toISOString() : null
                plan.FechaRevision = plan.FechaRevision ? (new Date(plan.FechaRevision)).toISOString() : null
                return plan
            }),
        }
        if (storedData === null) {
            await CrearNuevaAccion(request)
            btnGuardar$.setLoading(false)
            modal.remove()
            return
        }
        await EditarAccionMejora(request)
        btnGuardar$.setLoading(false)
        modal.remove()
    })

    btnAccionMejoraClose$.addEventListener('click', () => {
        modal.cancelAction()
    })
}



btnNuevaAccion$.addEventListener('click', e => {
    e.preventDefault()
    const modal = document.createElement('mx-modal-container')
    modal.innerHTML = `
        <div class="mx-section mx-section-modal">
            <div class="mx-form-container">
                ${GetFormAccionMejora('Nueva Acción de Mejora')}
            </div>
        </div> `


    document.body.appendChild(modal)

    initializeAccionesMejora(modal)
})


