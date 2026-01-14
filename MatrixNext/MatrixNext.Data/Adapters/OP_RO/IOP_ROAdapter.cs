using Dapper;
using MatrixNext.Data.Models.OP_RO;
using System.Data;

namespace MatrixNext.Data.Adapters.OP_RO
{
    /// <summary>
    /// Interfaz Adapter para Operational Review
    /// Responsable: Ejecutar SP para gestión de revisiones
    /// REGLA 2: Mapeo exacto de SP según CoreProject
    /// REGLA 4: Ejecución de SP contra BD
    /// </summary>
    public interface IOP_ROAdapter
    {
        // ============================================
        // CONSULTAS GENERALES
        // ============================================

        /// <summary>
        /// Obtiene listado de revisiones con filtros
        /// SP: OP_RO_Revisiones_Get
        /// </summary>
        Task<List<OP_ROReviewDTO>> GetRevisionesAsync(OP_ROFiltrosDTO filtros);

        /// <summary>
        /// Obtiene una revisión específica por ID
        /// SP: OP_RO_Revision_GetById
        /// </summary>
        Task<OP_ROReviewDTO> GetRevisionByIdAsync(int reviewId);

        // ============================================
        // CUESTIONARIOS
        // ============================================

        /// <summary>
        /// Obtiene listado de cuestionarios
        /// SP: OP_RO_Cuestionarios_Get
        /// </summary>
        Task<List<OP_ROCuestionarioDTO>> GetCuestionariosAsync(OP_ROFiltrosDTO filtros);

        /// <summary>
        /// Obtiene cuestionario con preguntas
        /// SP: OP_RO_Cuestionario_GetById
        /// </summary>
        Task<OP_ROCuestionarioDTO> GetCuestionarioByIdAsync(int cuestionarioId);

        /// <summary>
        /// Guarda un cuestionario
        /// SP: OP_RO_Cuestionario_Save
        /// </summary>
        Task<int> SaveCuestionarioAsync(OP_ROCuestionarioDTO cuestionario);

        // ============================================
        // INSTRUCTIVOS
        // ============================================

        /// <summary>
        /// Obtiene listado de instructivos
        /// SP: OP_RO_Instructivos_Get
        /// </summary>
        Task<List<OP_ROInstructivoDTO>> GetInstructivosAsync(OP_ROFiltrosDTO filtros);

        /// <summary>
        /// Obtiene instructivo con pasos
        /// SP: OP_RO_Instructivo_GetById
        /// </summary>
        Task<OP_ROInstructivoDTO> GetInstructivoByIdAsync(int instructivoId);

        /// <summary>
        /// Guarda un instructivo
        /// SP: OP_RO_Instructivo_Save
        /// </summary>
        Task<int> SaveInstructivoAsync(OP_ROInstructivoDTO instructivo);

        // ============================================
        // METODOLOGÍAS
        // ============================================

        /// <summary>
        /// Obtiene listado de metodologías
        /// SP: OP_RO_Metodologias_Get
        /// </summary>
        Task<List<OP_ROMetodologiaDTO>> GetMetodologiasAsync(OP_ROFiltrosDTO filtros);

        /// <summary>
        /// Obtiene metodología con fases
        /// SP: OP_RO_Metodologia_GetById
        /// </summary>
        Task<OP_ROMetodologiaDTO> GetMetodologiaByIdAsync(int metodologiaId);

        /// <summary>
        /// Guarda una metodología
        /// SP: OP_RO_Metodologia_Save
        /// </summary>
        Task<int> SaveMetodologiaAsync(OP_ROMetodologiaDTO metodologia);

        // ============================================
        // MATERIALES DE AYUDA
        // ============================================

        /// <summary>
        /// Obtiene listado de materiales de ayuda
        /// SP: OP_RO_Materiales_Get
        /// </summary>
        Task<List<OP_ROMaterialAyudaDTO>> GetMaterialesAsync(OP_ROFiltrosDTO filtros);

        /// <summary>
        /// Obtiene material de ayuda específico
        /// SP: OP_RO_Material_GetById
        /// </summary>
        Task<OP_ROMaterialAyudaDTO> GetMaterialByIdAsync(int materialId);

        /// <summary>
        /// Guarda un material de ayuda
        /// SP: OP_RO_Material_Save
        /// </summary>
        Task<int> SaveMaterialAsync(OP_ROMaterialAyudaDTO material);

        // ============================================
        // WORKFLOW: APROBACIÓN/RECHAZO
        // ============================================

        /// <summary>
        /// Aprueba una revisión
        /// SP: OP_RO_Revision_Aprobar
        /// </summary>
        Task<bool> AprobarRevisionAsync(OP_ROAprobarDTO aprobacion);

        /// <summary>
        /// Rechaza una revisión
        /// SP: OP_RO_Revision_Rechazar
        /// </summary>
        Task<bool> RechazarRevisionAsync(OP_RORechazarDTO rechazo);

        /// <summary>
        /// Obtiene historial de una revisión
        /// SP: OP_RO_Revision_Historial_Get
        /// </summary>
        Task<List<HistorialRevisionDTO>> GetHistorialRevisionAsync(int reviewId);

        // ============================================
        // VALIDACIONES
        // ============================================

        /// <summary>
        /// Valida parámetros de filtros
        /// </summary>
        void ValidarFiltros(OP_ROFiltrosDTO filtros);

        /// <summary>
        /// Valida datos antes de guardar
        /// </summary>
        void ValidarDatos(object dto);
    }
}
