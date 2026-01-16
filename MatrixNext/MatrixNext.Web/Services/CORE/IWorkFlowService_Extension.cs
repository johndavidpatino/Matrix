// MatrixNext.Web/Services/CORE/IWorkFlowService.cs - EXTENSIÓN

// Agregar estos métodos a la interfaz IWorkFlowService existente:

/// <summary>
/// Obtiene tareas de WorkFlow por unidad OP (para TraficoTareas consolidada)
/// Filtrado por: unidad, estado, prioridad, búsqueda
/// </summary>
Task<(List<TareasPorUnidadDto> Tareas, int Total)> ObtenerTareasPorUnidadAsync(
    int idUnidad,
    string? estado = null,
    int? prioridad = null,
    string? busqueda = null,
    int page = 1,
    int pageSize = 20);

/// <summary>
/// Obtiene todas las unidades OP disponibles para TraficoTareas
/// Retorna: (5=Crítica, 6=Verificación, 7=Captura, 8=Codificación, 9=DataCleaning, 
///           10=Procesamiento, 11=Scripting, 12=Pilotos, 13=Estadística, 14=Call Center)
/// </summary>
Task<List<UnidadTraficoDto>> ObtenerUnidadesTraficoAsync();

/// <summary>
/// Obtiene información de un trabajo específico incluyendo tipo de proyecto
/// Para validar si mostrar/ocultar btnFichaCuanti
/// </summary>
Task<TrabajoTraficoInfoDto?> ObtenerInformacionTrabajoAsync(long idTrabajo);
