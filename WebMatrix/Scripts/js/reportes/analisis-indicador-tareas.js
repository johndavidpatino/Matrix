import { indicadores, trimestreOptions } from "./filters-data/filters-data.js"

import { GetUserInfo, AddAnalisis, ExportCumplimientoTareasExcel, ExportCumplimientoTareasDetallesExcel, ExportCumplimientoTareasCOEExcel, ExportCumplimientoTareasCOEDetallesExcel } from "./services/registro-observaciones.service.js"
import { NotificationManager } from "./notifications/notifications.js"
import { RenderAnalisisTable } from "./analisis-table.js"
import { GetFilters } from "./reporte-cumplimiento-tareas-filters.js"


const analisisSubmitBtn$ = document.getElementById("analisisSubmitBtn")
const quarterSelector$ = document.getElementById("quarterSelector")
const analisisText$ = document.getElementById("analisisText")
const btnExportAgrupadosExcel$ = document.getElementById("btnExportAgrupadosExcel")
const btnExportDetallesExcel$ = document.getElementById("btnExportDetallesExcel")
const btnExportAgrupadosCOEExcel$ = document.getElementById("btnExportAgrupadosCOEExcel")
const btnExportDetallesCOEExcel$ = document.getElementById("btnExportDetallesCOEExcel")

const analisisFormInputs = [quarterSelector$,analisisText$]


let request = {
    usuarioRegistra:null,
	fechaRegistro:null,
	contenido:null,
	periodo:null,
	trimestre:null,
    indicador: indicadores.CUMPLIMIENTO_TAREAS,
	agruparPor:1,
	idInstrumento:null,
    idTarea: null,
    idUnidad: null,
    idProceso: null
}


quarterSelector$.options = trimestreOptions



const notificationManager = new NotificationManager("notificationsContainer", 5000); // 5000ms auto-hide delay

analisisSubmitBtn$.addEventListener('mxClick', async (event) => {
    event.preventDefault()
    analisisSubmitBtn$.setLoading(true)

    let filters = GetFilters()
    let userInfo = await GetUserInfo()

    request.usuarioRegistra = userInfo[0].id
    const now = new Date();
    request.fechaRegistro = now
    request.contenido = analisisText$.value.trim()

    request.idUnidad = filters.idUnidad
    request.idTarea = filters.idTarea
    request.periodo = filters.ano
    request.trimestre = quarterSelector$.value.trim()
    request.agruparPor = filters.agruparPor
    request.idProceso = filters.idProceso

    if (validateRequest(request)) {
        try {
            let result = await AddAnalisis(request)
            let json = await result.json()
            
            if(!result.ok){
                if(json.d.Message) notificationManager.showError(json.d.Message);
                else notificationManager.showError("Error al guardar el análisis. Por favor, intente nuevamente.");

                analisisSubmitBtn$.setLoading(false)
                return
            }

            analisisFormInputs.forEach(item => {
                item.clear()
            })
            notificationManager.showSuccess("Análisis guardado exitosamente.");
            analisisSubmitBtn$.setLoading(false)
            RenderAnalisisTable()

        } catch (error) {
            notificationManager.showError("Error al guardar el análisis. Por favor, intente nuevamente.");
            analisisSubmitBtn$.setLoading(false)
            console.error('Error:', error);
        }
    } else {
        const userErrors = validateUserInputs(request);
        const devErrors = validateDevInputs(request);
        analisisSubmitBtn$.setLoading(false)
        if (userErrors.length > 0) {
            notificationManager.showWarning(`Por favor, llena todos los campos requeridos: <br/>${userErrors.join('<br/>')}`);
        }

        if (devErrors.length > 0) {
            console.error('Errors:', devErrors);
            notificationManager.showError("Se ha producido un error. Por favor, contacte al administrador del sistema.");
        }
    }
})


function validateRequest(req) {
    return (
        validateUserInputs(req).length === 0 &&
        validateDevInputs(req).length === 0
    )
}

function validateUserInputs(req) {
    const errors = [];
    if (req.contenido === '') errors.push('- Analisis del indicador requerido');
    if (req.idUnidad === null) errors.push('- Unidad es requerida');
    if (req.periodo === null) errors.push('- Periodo es requerido');
    if (req.trimestre === '') errors.push('- Trimestre es requerido');
    if (req.idProceso === 0) errors.push('- Proceso es requerido');
    if (req.idTarea === 0) errors.push('- Tarea es requerida');
    return errors;
}

function validateDevInputs(req) {
    const errors = [];
    if (!req.usuarioRegistra) errors.push('usuarioRegistra is missing');
    if (!req.fechaRegistro) errors.push('fechaRegistro is missing');
    if (req.agruparPor === null) errors.push('agruparPor is missing');
    return errors;
}

btnExportAgrupadosExcel$.addEventListener('mxClick', async (event) => {
    event.preventDefault()
    let filters = GetFilters()

    let responseFile = await ExportCumplimientoTareasExcel(filters)
    await downloadExcelFile(responseFile, "CumplimientoTareas_Agrupado.xlsx")
    
})

btnExportDetallesExcel$.addEventListener('mxClick', async (event) => {
    event.preventDefault()
    let filters = GetFilters()

    let responseFile = await ExportCumplimientoTareasDetallesExcel(filters)
    await downloadExcelFile(responseFile, "CumplimientoTareas_Detalles.xlsx")
})

btnExportAgrupadosCOEExcel$.addEventListener('mxClick', async (event) => {
    event.preventDefault()
    let filters = GetFilters()

    let responseFile = await ExportCumplimientoTareasCOEExcel(filters)
    await downloadExcelFile(responseFile, "CumplimientoTareasCOE.xlsx")
})

btnExportDetallesCOEExcel$.addEventListener('mxClick', async (event) => {
    event.preventDefault()
    let filters = GetFilters()

    let responseFile = await ExportCumplimientoTareasCOEDetallesExcel(filters)
    await downloadExcelFile(responseFile, "CumplimientoTareasCOE_Detalles.xlsx")
})



async function downloadExcelFile(responseFile, fileName) {
    let json = await responseFile.json()
    var blob = new Blob([Base64ToBytes(json.d)], { type: 'application/octetstream' });

    var link = document.createElement("a");

    link.href = window.URL.createObjectURL(blob);
    link.download = fileName;
    document.body.appendChild(link);
    link.click();
    document.body.removeChild(link);
}

function Base64ToBytes(base64) {
    var s = window.atob(base64);
    var bytes = new Uint8Array(s.length);
    for (var i = 0; i < s.length; i++) {
        bytes[i] = s.charCodeAt(i);
    }
    return bytes;
};