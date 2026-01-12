using MatrixNext.Data.Models.ViewModels.Pnc;

namespace MatrixNext.Data.Services.Pnc
{
    /// <summary>
    /// Interface para servicio de Producto No Conforme (PNC)
    /// Sistema de Gestión de Calidad ISO 9001
    /// Lógica de negocio, validaciones y orquestación
    /// </summary>
    public interface IPncService
    {
        // ============= CONSULTAS =============

        /// <summary>
        /// Obtener todos los PNC con filtros opcionales
        /// </summary>
        Task<(bool success, PncFiltrosVM data, string message)> ObtenerPnc(PncFiltrosVM filtros);

        /// <summary>
        /// Obtener PNC por ID con detalle completo (causas + acciones)
        /// </summary>
        Task<(bool success, ProductoNoConformeDetalleVM? data, string message)> ObtenerPncById(int idPnc);

        /// <summary>
        /// Obtener datos para seguimiento (KPIs y dashboard)
        /// </summary>
        Task<(bool success, PncSeguimientoVM data, string message)> ObtenerSeguimiento();

        // ============= CATÁLOGOS =============

        /// <summary>
        /// Obtener todos los catálogos necesarios para formularios
        /// </summary>
        Task<(bool success, PncCatalogosDto data, string message)> ObtenerCatalogos();

        // ============= CRUD PNC =============

        /// <summary>
        /// Crear nuevo PNC con validaciones de negocio
        /// Envía email de notificación
        /// </summary>
        Task<(bool success, int id, string message)> CrearPnc(CrearPncVM modelo, long idUsuario);

        /// <summary>
        /// Actualizar PNC existente
        /// Solo si no está cerrado
        /// </summary>
        Task<(bool success, string message)> ActualizarPnc(ProductoNoConformeVM pnc, long idUsuario);

        /// <summary>
        /// Cerrar PNC
        /// Valida que todas las acciones estén ejecutadas
        /// Envía email de notificación
        /// </summary>
        Task<(bool success, string message)> CerrarPnc(int idPnc, long idUsuario);

        // ============= CAUSAS =============

        /// <summary>
        /// Agregar causa a un PNC
        /// Envía email de notificación
        /// </summary>
        Task<(bool success, int id, string message)> AgregarCausa(AgregarCausaPncVM modelo, long idUsuario);

        /// <summary>
        /// Actualizar causa existente
        /// </summary>
        Task<(bool success, string message)> ActualizarCausa(ProductoNoConformeCausaVM causa);

        /// <summary>
        /// Eliminar causa
        /// Valida que no tenga acciones
        /// </summary>
        Task<(bool success, string message)> EliminarCausa(int idCausa);

        // ============= ACCIONES =============

        /// <summary>
        /// Agregar acción a una causa
        /// Valida acción inmediata obligatoria
        /// Envía email de asignación
        /// </summary>
        Task<(bool success, int id, string message)> AgregarAccion(AgregarAccionPncVM modelo, long idUsuario);

        /// <summary>
        /// Actualizar acción existente
        /// Solo si no está ejecutada
        /// </summary>
        Task<(bool success, string message)> ActualizarAccion(ProductoNoConformeAccionVM accion);

        /// <summary>
        /// Ejecutar/cerrar acción (marcar como completada)
        /// Registra evidencia de cierre
        /// </summary>
        Task<(bool success, string message)> EjecutarAccion(CerrarAccionPncVM modelo, long idUsuario);

        /// <summary>
        /// Eliminar acción
        /// Solo si permite actualización
        /// </summary>
        Task<(bool success, string message)> EliminarAccion(int idAccion);

        // ============= VALIDACIONES =============

        /// <summary>
        /// Validar si un PNC puede ser cerrado
        /// Verifica que tenga causas y todas las acciones ejecutadas
        /// </summary>
        Task<(bool canClose, string reason)> ValidarCierrePnc(int idPnc);

        /// <summary>
        /// Validar si una causa tiene acción inmediata
        /// REGLA ISO 9001: Toda causa debe tener al menos 1 acción inmediata
        /// </summary>
        Task<(bool hasImmediate, string message)> ValidarAccionInmediata(int idPnc, int idCausa);

        // ============= NOTIFICACIONES =============

        /// <summary>
        /// Procesar notificaciones de acciones vencidas o próximas a vencer
        /// Ejecutado por BackgroundService
        /// </summary>
        Task<int> ProcesarNotificacionesAcciones();
    }

    /// <summary>
    /// DTO para retornar todos los catálogos en una sola llamada
    /// </summary>
    public class PncCatalogosDto
    {
        public List<PncFuenteReclamoVM> FuentesReclamo { get; set; } = new();
        public List<PncCategoriaVM> Categorias { get; set; } = new();
        public List<PncTipoAccionVM> TiposAccion { get; set; } = new();
    }
}
