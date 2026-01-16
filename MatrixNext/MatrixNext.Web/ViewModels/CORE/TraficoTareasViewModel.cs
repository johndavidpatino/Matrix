// MatrixNext.Web/ViewModels/TraficoTareasViewModel.cs

using MatrixNext.Core.DTOs.CORE;

namespace MatrixNext.Web.ViewModels.CORE
{
    /// <summary>
    /// ViewModel para vista TraficoTareas.cshtml (TraficoTareas consolidada)
    /// Sprint 17 - RE_GT UI consolidada
    /// Contiene: listado de tareas + filtros + unidades disponibles
    /// </summary>
    public class TraficoTareasViewModel
    {
        /// <summary>Listado de tareas para la unidad actual</summary>
        public List<TareasPorUnidadDto> Tareas { get; set; } = new();

        /// <summary>Total de registros (para calcular páginas)</summary>
        public int TotalRegistros { get; set; }

        /// <summary>Página actual (1-based)</summary>
        public int PaginaActual { get; set; } = 1;

        /// <summary>Cantidad de registros por página</summary>
        public int RegistrosPorPagina { get; set; } = 25;

        /// <summary>Total de páginas</summary>
        public int TotalPaginas => (int)Math.Ceiling((double)TotalRegistros / RegistrosPorPagina);

        /// <summary>Filtro: Unidad OP (5=Crítica, 6=Verificación, etc)</summary>
        public int? FiltroUnidad { get; set; }

        /// <summary>Filtro: Estado (Creada, EnProgreso, Completada, Anulada)</summary>
        public string? FiltroEstado { get; set; }

        /// <summary>Filtro: Prioridad (1=Normal, 2=Alta, 3=Baja)</summary>
        public int? FiltroPrioridad { get; set; }

        /// <summary>Filtro: Búsqueda de texto (nombre trabajo, jobbook, etc)</summary>
        public string? FiltroBusqueda { get; set; }

        /// <summary>Unidades disponibles para dropdown de filtro</summary>
        public List<UnidadTraficoDto> UnidadesDisponibles { get; set; } = new();

        /// <summary>Unidad actual seleccionada (para validación de permisos)</summary>
        public int IdUnidadActual { get; set; }

        /// <summary>Nombre unidad actual (para display)</summary>
        public string? NombreUnidadActual { get; set; }

        /// <summary>Rol del usuario en sesión (para validación y visibilidad botones)</summary>
        public int IdRolUsuario { get; set; }

        /// <summary>¿Mostrar botón "Personal Asignado"? (solo unidades 11, 14)</summary>
        public bool MostrarPersonalAsignado => IdUnidadActual is 11 or 14;

        /// <summary>URLRetorno (enum int para navegación)</summary>
        public int? URLRetorno { get; set; }

        /// <summary>Trabajo actualmente seleccionado (si aplica)</summary>
        public long? IdTrabajoSeleccionado { get; set; }

        /// <summary>Información del trabajo seleccionado (para Accordion 1)</summary>
        public string? NombreTrabajoSeleccionado { get; set; }

        /// <summary>¿Mostrar Accordion 0 (listado) o Accordion 1 (gestión)?</summary>
        public bool MostrarListado { get; set; } = true;

        /// <summary>Propiedades calculadas</summary>
        public bool HayRegistros => Tareas.Any();
        public int TareasUrgentes => Tareas.Count(t => t.EsUrgente);
        public int TareasVencidas => Tareas.Count(t => t.EsVencida);
        public int TareasEnProgreso => Tareas.Count(t => t.Estado == "EnProgreso");
        public int TareasCompletadas => Tareas.Count(t => t.Estado == "Completada");
    }

    /// <summary>
    /// DTO para unidades OP disponibles (dropdown de filtro)
    /// Ya definida en MatrixNext.Core.DTOs.CORE.UnidadTraficoDto
    /// Se reutiliza desde Core
    /// </summary>

    /// <summary>
    /// Enum para URLRetorno (mapeo a navegación de retorno)
    /// </summary>
    public enum URLRetornoEnum : int
    {
        RE_GT_TraficoTareas_Scripting = 1,
        RE_GT_TraficoTareas_Pilotos = 2,
        RE_GT_TraficoTareas_Critica = 3,
        RE_GT_TraficoTareas_Verificacion = 4,
        RE_GT_TraficoTareas_Captura = 5,
        RE_GT_TraficoTareas_Codificacion = 6,
        RE_GT_TraficoTareas_Datacleaning = 7,
        RE_GT_TraficoTareas_Procesamiento = 8,
        RE_GT_TraficoTareas_Estadistica = 9,
        CORE_ListaTrabajosTareas = 10,
        RE_GT_TrabajosPorGerencia = 11,
        RE_GT_TraficoEncuestasRMC = 12,
        RE_GT_CallCenter = 13,
        Default = 0
    }

    /// <summary>
    /// Helper para resolver URL de retorno basado en URLRetornoEnum
    /// </summary>
    public static class URLRetornoHelper
    {
        public static string? ObtenerUrlRetorno(URLRetornoEnum? urlRetorno, string baseUrl)
        {
            return urlRetorno switch
            {
                URLRetornoEnum.RE_GT_TraficoTareas_Critica => $"{baseUrl}/RE_GT/HomeGestionTratamiento",
                URLRetornoEnum.RE_GT_TraficoTareas_Verificacion => $"{baseUrl}/RE_GT/HomeGestionTratamiento",
                URLRetornoEnum.RE_GT_TraficoTareas_Captura => $"{baseUrl}/RE_GT/HomeGestionTratamiento",
                URLRetornoEnum.RE_GT_TraficoTareas_Codificacion => $"{baseUrl}/RE_GT/HomeGestionTratamiento",
                URLRetornoEnum.RE_GT_TraficoTareas_Datacleaning => $"{baseUrl}/RE_GT/HomeGestionTratamiento",
                URLRetornoEnum.RE_GT_TraficoTareas_Procesamiento => $"{baseUrl}/RE_GT/HomeGestionTratamiento",
                URLRetornoEnum.RE_GT_TraficoTareas_Estadistica => $"{baseUrl}/ES_Estadistica/Default",
                URLRetornoEnum.CORE_ListaTrabajosTareas => $"{baseUrl}/CORE/GestionTareas/Gestion-Tareas-Trabajos",
                URLRetornoEnum.RE_GT_TrabajosPorGerencia => $"{baseUrl}/RP_Reportes/TrabajosPorGerencia",
                URLRetornoEnum.RE_GT_TraficoEncuestasRMC => $"{baseUrl}/RE_GT/TraficoEncuestas?UnidadId=38",
                URLRetornoEnum.RE_GT_TraficoTareas_Scripting => $"{baseUrl}/RE_GT/HomeRecoleccion",
                URLRetornoEnum.RE_GT_TraficoTareas_Pilotos => $"{baseUrl}/RE_GT/HomeRecoleccion",
                URLRetornoEnum.RE_GT_CallCenter => $"{baseUrl}/RE_GT/HomeRecoleccion",
                _ => $"{baseUrl}/RE_GT/HomeRecoleccion"
            };
        }
    }
}
