using MatrixNext.Data.Models.ViewModels.Pnc;
using MatrixNext.Data.Models.ViewModels.Pnc.DTOs;

namespace MatrixNext.Data.Adapters.Pnc
{
    /// <summary>
    /// Interface para Adapter de Producto No Conforme (PNC)
    /// Sistema de Gestión de Calidad ISO 9001
    /// Mapea 16 Stored Procedures del legacy
    /// </summary>
    public interface IPncAdapter
    {
        // ============= CONSULTAS PNC =============
        
        /// <summary>
        /// Obtener todos los PNC (sin filtros)
        /// SP: PNC_ObtenerProductoNoConformeTodos
        /// </summary>
        Task<List<PncObtenerProductoNoConformeDTO>> ObtenerTodos();

        /// <summary>
        /// Obtener PNC por JobBook (con LIKE)
        /// SP: PNC_ObtenerProductoNoConforme
        /// </summary>
        Task<List<PncObtenerProductoNoConformeDTO>> ObtenerPorJobBook(string jobBook);

        /// <summary>
        /// Obtener PNC por ID con información completa
        /// SP: PNC_GetById
        /// </summary>
        Task<ProductoNoConformeDetalleVM?> ObtenerPorId(int idPnc);

        // ============= CONSULTAS CAUSAS =============

        /// <summary>
        /// Obtener causas de un PNC
        /// SP: PNC_ProductoNoConformeCausas_Get
        /// </summary>
        Task<List<PncVerCausasDTO>> ObtenerCausas(int idPnc);

        /// <summary>
        /// Obtener causas con detalle (para sistema avanzado)
        /// SP: PNC_Causa_Get
        /// </summary>
        Task<List<PncVerCausasDTO>> ObtenerCausasDetalle(int idProducto);

        // ============= CONSULTAS ACCIONES =============

        /// <summary>
        /// Obtener acciones de una causa específica
        /// SP: PNC_ProductoNoConformeAcciones_Get
        /// </summary>
        Task<List<PncVerAccionesDTO>> ObtenerAcciones(int idPnc, int idCausa);

        // ============= NOTIFICACIONES EMAIL =============

        /// <summary>
        /// Obtener información de acción para email recordatorio
        /// SP: PNC_EmailAcciones
        /// </summary>
        Task<PncNotificacionVM?> ObtenerDatosEmailAccion(long idAccion);

        /// <summary>
        /// Obtener correos a notificar cuando se crea un PNC
        /// SP: PNC_EmailNotificacionReporte
        /// </summary>
        Task<List<string>> ObtenerCorreosNotificacion(long idPnc);

        // ============= CATÁLOGOS =============

        /// <summary>
        /// Obtener catálogo de fuentes de reclamo
        /// Query directo a PNC_FuenteReclamo
        /// </summary>
        Task<List<PncFuenteReclamoVM>> ObtenerFuentesReclamo();

        /// <summary>
        /// Obtener catálogo de categorías
        /// Query directo a PNC_Categorias
        /// </summary>
        Task<List<PncCategoriaVM>> ObtenerCategorias();

        /// <summary>
        /// Obtener catálogo de tipos de acción
        /// Query directo a PNC_TiposDeAccion
        /// </summary>
        Task<List<PncTipoAccionVM>> ObtenerTiposAccion();

        // ============= CRUD PNC =============

        /// <summary>
        /// Insertar nuevo PNC
        /// Direct INSERT con Identity return
        /// </summary>
        Task<int> InsertarPnc(ProductoNoConformeVM pnc);

        /// <summary>
        /// Actualizar PNC existente
        /// Direct UPDATE
        /// </summary>
        Task<bool> ActualizarPnc(ProductoNoConformeVM pnc);

        /// <summary>
        /// Cerrar PNC (marcar como cerrado)
        /// UPDATE Cerrado=1, FechaCierre=GETDATE()
        /// </summary>
        Task<bool> CerrarPnc(int idPnc, long idUsuario);

        // ============= CRUD CAUSAS =============

        /// <summary>
        /// Insertar causa a un PNC
        /// Direct INSERT con Identity return
        /// </summary>
        Task<int> InsertarCausa(ProductoNoConformeCausaVM causa);

        /// <summary>
        /// Actualizar causa existente
        /// Direct UPDATE
        /// </summary>
        Task<bool> ActualizarCausa(ProductoNoConformeCausaVM causa);

        /// <summary>
        /// Eliminar causa
        /// Direct DELETE
        /// </summary>
        Task<bool> EliminarCausa(int idCausa);

        // ============= CRUD ACCIONES =============

        /// <summary>
        /// Insertar acción a una causa
        /// Direct INSERT con Identity return
        /// </summary>
        Task<int> InsertarAccion(ProductoNoConformeAccionVM accion);

        /// <summary>
        /// Actualizar acción existente
        /// Direct UPDATE
        /// </summary>
        Task<bool> ActualizarAccion(ProductoNoConformeAccionVM accion);

        /// <summary>
        /// Marcar acción como ejecutada
        /// UPDATE FechaEjecucion, EvidenciaCierre
        /// </summary>
        Task<bool> EjecutarAccion(int idAccion, DateTime fechaEjecucion, string evidenciaCierre);

        /// <summary>
        /// Eliminar acción
        /// Direct DELETE
        /// </summary>
        Task<bool> EliminarAccion(int idAccion);

        // ============= VALIDACIONES =============

        /// <summary>
        /// Validar si existe acción de un tipo específico
        /// Para validar acción inmediata obligatoria
        /// </summary>
        Task<bool> ExisteAccion(int idPnc, int idCausa, int tipoAccion);

        /// <summary>
        /// Validar si todas las acciones de un PNC están ejecutadas
        /// Para permitir cierre
        /// </summary>
        Task<bool> TodasAccionesEjecutadas(int idPnc);
    }
}
