import { unidades, tasks, instruments, indicadores,trimestreOptions } from "./filters-data/filters-data.js";
import { GetAnalisis, GetUserInfo, DeleteAnalisis, GetProcesses, GetAllTasks, UpdateAnalisis} from "./services/registro-observaciones.service.js";
import { NotificationManager } from "./notifications/notifications.js";

const trimesterFilter$ = document.getElementById("trimesterFilter");
const analisisTableLit$ = document.getElementById("analisisTableLit");
const analisisContainer$ = document.getElementById("analisisContainer");

let filters = {}



trimesterFilter$.options = trimestreOptions

const notificationManager = new NotificationManager("notificationsContainer", 5000);

let procesos = await GetProcesses()

let processTasks = await GetAllTasks()


document.addEventListener('changeFilters', async event => {
    filters = event.detail.filters
    RenderAnalisisTable()
});

const userInfo = await GetUserInfo()

const isQAUser = userInfo.find(info => info.IdUnidad == 6)


const GetAnalisisFormat = function(analisis,indicador){
    if(indicador === indicadores.REGISTRO_OBSERVACIONES){
        return GetAnalisisFormatRegistroObservaciones(analisis)
    }
    if(indicador === indicadores.CUMPLIMIENTO_TAREAS){
        return GetAnalisisFormatCumplimientoTareas(analisis)
    }
    return {
        ...analisis,
        Filtros: ''
    }
}

const GetAnalisisFormatRegistroObservaciones = function(analisis){
    let task = analisis.IdTarea ? tasks.find(t => t.IdTarea == analisis.IdTarea) : null;
    let instrument = analisis.IdInstrumento ? instruments.find(i => i.IdInstrumento == analisis.IdInstrumento) : null;

    let filters = []
    if(task) filters.push(tasks.find(t => t.IdTarea == task.IdTarea)?.NombreTarea ?? '')
    if(instrument) filters.push(instruments.find(i => i.IdInstrumento == instrument.IdInstrumento)?.NombreInstrumento ?? '')

    return {
        ...analisis,
        Filtros: filters.join(' - ')
    };
}

const GetAnalisisFormatCumplimientoTareas = function(analisis){
    
    let filters = []

    let task = analisis.IdTarea ? processTasks.find(t => t.id == analisis.IdTarea) : null;
    let process = procesos.find(p => p.id == analisis.IdProceso)
 
    if(process) filters.push(process.Unidad)
    if(task) filters.push(task.Tarea)
    return {
        ...analisis,
        Filtros: filters.join(' - ')
    };
}

export const RenderAnalisisTable = async function(pageNumber = 1){    
    let userInfo = await GetUserInfo();

    let isQAUser = userInfo.find(info => info.IdUnidad == 6)

    let indicador = filters.indicador
    let idUnidad = filters.idUnidad ?? null
    let usuarioRegistra = isQAUser ? null : userInfo[0].id  
    let periodo = filters.ano
    let idTarea = filters.idTarea
    let idInstrumento = filters.idInstrumento

    let analisis = await GetAnalisis({indicador:indicador, idUnidad:idUnidad, periodo:periodo,usuarioRegistra:usuarioRegistra, idTarea:idTarea, idInstrumento:idInstrumento,pageSize:5,pageNumber:pageNumber});

    

    let distinctAnalisis = analisis.Items.map(item => {
        item['NombreUnidad'] = unidades.find(un => un.idUnidad == item.IdUnidad)?.NombreUnidad
        item.FechaRegistro = new Date(item.FechaRegistro).toLocaleDateString()
        return GetAnalisisFormat(item,indicador)
    });

    const selectedTrimester = trimesterFilter$.value;
    if (selectedTrimester) {
        distinctAnalisis = distinctAnalisis.filter(item => item.Trimestre === selectedTrimester);
    }

    analisisTableLit$.data = distinctAnalisis


    analisisTableLit$.pagination = {
        pageNumber: analisis.PageNumber,
        pageSize: analisis.PageSize,
        totalCount: analisis.TotalCount,
        totalPages: Math.ceil(analisis.TotalCount / analisis.PageSize),
        nextPage: analisis.NextPage,
        previousPage: analisis.PreviousPage
    }

}

analisisTableLit$.addEventListener('page-change', async event => {
    RenderAnalisisTable(event.detail.page)
})

const DeleteAction = async (row, callbackSuccess) => {
    let modal = document.createElement('mx-modal')
    modal.title = 'Eliminar análisis'
    modal.content = '¿Estás seguro de que deseas eliminar este análisis?'
    modal.confirmButtonText = 'Eliminar'
    modal.cancelButtonText = 'Cancelar'

    analisisContainer$.appendChild(modal)
    modal.cancelAction = async () => {
        callbackSuccess(false)
        return false
    }
    modal.confirmAction = async () => {
        let result = await DeleteAnalisis({idAnalisis:row.IdAnalisis});
        if(result.ok){
            notificationManager.showSuccess("Análisis eliminado exitosamente.");
            callbackSuccess(true)
        } else {
            notificationManager.showError("Error al eliminar el análisis. Por favor, intente nuevamente.");
            callbackSuccess(false)
        }
    }


}

const EditAction = async (row,callbackSuccess) => {
    let modalContainer = document.createElement('mx-modal-container')
    document.body.appendChild(modalContainer)
    modalContainer.innerHTML = `
        <div class="mx-card mx-edit-analisis">
            <div class="mx-card-header">
                <h3>Editar análisis</h3>
            </div>
            <div class="mx-card-body">
                <mx-text-area id="txtEditAnalisis">
                    <span>Contenido</span>
                </mx-text-area>
            </div>
            <div class="mx-card-footer">
                <mx-button id="btnCancelEditAnalisis" variant="outline">Cancelar</mx-button>
                <mx-button id="btnEditAnalisis">Editar</mx-button>
            </div>
        </div>
    `
    
    let btnEditAnalisis = modalContainer.querySelector('#btnEditAnalisis')
    let btnCancelEditAnalisis = modalContainer.querySelector('#btnCancelEditAnalisis')
    let txtEditAnalisis = modalContainer.querySelector('#txtEditAnalisis')

    console.log(txtEditAnalisis)
    txtEditAnalisis.value = row.Contenido

    btnEditAnalisis.addEventListener('mxClick', async (event) => {
        event.preventDefault()
        btnEditAnalisis.setLoading(true)
        let result = await UpdateAnalisis({idAnalisis:row.IdAnalisis, content:txtEditAnalisis.value})
        if(result.ok){
            notificationManager.showSuccess("Análisis actualizado exitosamente.");
            RenderAnalisisTable()
            modalContainer.remove()
            callbackSuccess(true)
        } else {
            notificationManager.showError("Error al actualizar el análisis. Por favor, intente nuevamente.");
            callbackSuccess(false)
        }
        btnEditAnalisis.setLoading(false)
    })
    btnCancelEditAnalisis.addEventListener('mxClick', (event) => {
        event.preventDefault()
        modalContainer.remove()
    })
}

const ShowActionsAction = async (row) => {
    console.log(row)
}

trimesterFilter$.addEventListener('mxChange', RenderAnalisisTable);

analisisTableLit$.columns = [
    {name: 'IdAnalisis', displayName: 'Id'},
    {name:'Periodo', displayName:'Periodo'},
    {name:'Trimestre', displayName:'Trimestre'},
    {name:'NombreUnidad', displayName:'Unidad'},
    {name:'Indicador', displayName:'Indicador'},
    {name:'UsuarioRegistra', displayName:'Usuario'},
    {name:'Filtros', displayName:'Filtros'},
    {name:'FechaRegistro', displayName:'Fecha'},
    {name:'Contenido', displayName:'Contenido', maxLines: 2}
]

analisisTableLit$.idData = 'IdAnalisis'

analisisTableLit$.actions = [
    {name:'Ver acciones de mejora',icon:'comment',iconColor: '#6c757d', fun: ShowActionsAction}
]

if(!isQAUser) {
    analisisTableLit$.actions.unshift(
        {name:'Eliminar',icon:'trash-alt',iconColor:'#dc3545',iconHoverColor:'#dc3545', isDeleteAction: true,fun: DeleteAction},
        {name:'Editar',icon:'edit',iconColor: '#6c757d', idEditAction: true, fun: EditAction}
    )
}

RenderAnalisisTable()