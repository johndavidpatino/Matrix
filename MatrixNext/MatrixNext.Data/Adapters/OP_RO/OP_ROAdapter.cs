using Dapper;
using MatrixNext.Data.Models.OP_RO;
using Microsoft.Extensions.Logging;
using System.Data;

namespace MatrixNext.Data.Adapters.OP_RO
{
    /// <summary>
    /// Implementación Adapter para Operational Review
    /// NOTA: Todos los SP de este módulo NO EXISTEN en la BD legacy (CO_Matrix_Intranet)
    /// Los métodos retornan valores vacíos/default hasta que se creen los SP correspondientes
    /// </summary>
    public class OP_ROAdapter : IOP_ROAdapter
    {
        private readonly IDbConnection _dbConnection;
        private readonly ILogger<OP_ROAdapter> _logger;

        public OP_ROAdapter(IDbConnection dbConnection, ILogger<OP_ROAdapter> logger)
        {
            _dbConnection = dbConnection;
            _logger = logger;
        }

        // ============================================
        // CONSULTAS GENERALES
        // ============================================

        /// <summary>
        /// STUB: SP OP_RO_Revisiones_Get no existe en BD legacy
        /// </summary>
        public Task<List<OP_ROReviewDTO>> GetRevisionesAsync(OP_ROFiltrosDTO filtros)
        {
            ValidarFiltros(filtros);
            _logger.LogWarning("[OP_RO] GetRevisionesAsync: SP 'OP_RO_Revisiones_Get' no existe en BD legacy. Retornando lista vacía.");
            return Task.FromResult(new List<OP_ROReviewDTO>());
        }

        /// <summary>
        /// STUB: SP OP_RO_Revision_GetById no existe en BD legacy
        /// </summary>
        public Task<OP_ROReviewDTO> GetRevisionByIdAsync(int reviewId)
        {
            _logger.LogWarning("[OP_RO] GetRevisionByIdAsync: SP 'OP_RO_Revision_GetById' no existe en BD legacy. Retornando DTO vacío. ReviewId={ReviewId}", reviewId);
            return Task.FromResult(new OP_ROReviewDTO());
        }

        // ============================================
        // CUESTIONARIOS
        // ============================================

        /// <summary>
        /// STUB: SP OP_RO_Cuestionarios_Get no existe en BD legacy
        /// </summary>
        public Task<List<OP_ROCuestionarioDTO>> GetCuestionariosAsync(OP_ROFiltrosDTO filtros)
        {
            ValidarFiltros(filtros);
            _logger.LogWarning("[OP_RO] GetCuestionariosAsync: SP 'OP_RO_Cuestionarios_Get' no existe en BD legacy. Retornando lista vacía.");
            return Task.FromResult(new List<OP_ROCuestionarioDTO>());
        }

        /// <summary>
        /// STUB: SP OP_RO_Cuestionario_GetById no existe en BD legacy
        /// </summary>
        public Task<OP_ROCuestionarioDTO> GetCuestionarioByIdAsync(int cuestionarioId)
        {
            _logger.LogWarning("[OP_RO] GetCuestionarioByIdAsync: SP 'OP_RO_Cuestionario_GetById' no existe en BD legacy. Retornando DTO vacío. CuestionarioId={CuestionarioId}", cuestionarioId);
            return Task.FromResult(new OP_ROCuestionarioDTO());
        }

        /// <summary>
        /// STUB: SP OP_RO_Cuestionario_Save no existe en BD legacy
        /// </summary>
        public Task<int> SaveCuestionarioAsync(OP_ROCuestionarioDTO cuestionario)
        {
            ValidarDatos(cuestionario);
            _logger.LogWarning("[OP_RO] SaveCuestionarioAsync: SP 'OP_RO_Cuestionario_Save' no existe en BD legacy. Retornando 0.");
            return Task.FromResult(0);
        }

        // ============================================
        // INSTRUCTIVOS
        // ============================================

        /// <summary>
        /// STUB: SP OP_RO_Instructivos_Get no existe en BD legacy
        /// </summary>
        public Task<List<OP_ROInstructivoDTO>> GetInstructivosAsync(OP_ROFiltrosDTO filtros)
        {
            ValidarFiltros(filtros);
            _logger.LogWarning("[OP_RO] GetInstructivosAsync: SP 'OP_RO_Instructivos_Get' no existe en BD legacy. Retornando lista vacía.");
            return Task.FromResult(new List<OP_ROInstructivoDTO>());
        }

        /// <summary>
        /// STUB: SP OP_RO_Instructivo_GetById no existe en BD legacy
        /// </summary>
        public Task<OP_ROInstructivoDTO> GetInstructivoByIdAsync(int instructivoId)
        {
            _logger.LogWarning("[OP_RO] GetInstructivoByIdAsync: SP 'OP_RO_Instructivo_GetById' no existe en BD legacy. Retornando DTO vacío. InstructivoId={InstructivoId}", instructivoId);
            return Task.FromResult(new OP_ROInstructivoDTO());
        }

        /// <summary>
        /// STUB: SP OP_RO_Instructivo_Save no existe en BD legacy
        /// </summary>
        public Task<int> SaveInstructivoAsync(OP_ROInstructivoDTO instructivo)
        {
            ValidarDatos(instructivo);
            _logger.LogWarning("[OP_RO] SaveInstructivoAsync: SP 'OP_RO_Instructivo_Save' no existe en BD legacy. Retornando 0.");
            return Task.FromResult(0);
        }

        // ============================================
        // METODOLOGÍAS
        // ============================================

        /// <summary>
        /// STUB: SP OP_RO_Metodologias_Get no existe en BD legacy
        /// </summary>
        public Task<List<OP_ROMetodologiaDTO>> GetMetodologiasAsync(OP_ROFiltrosDTO filtros)
        {
            ValidarFiltros(filtros);
            _logger.LogWarning("[OP_RO] GetMetodologiasAsync: SP 'OP_RO_Metodologias_Get' no existe en BD legacy. Retornando lista vacía.");
            return Task.FromResult(new List<OP_ROMetodologiaDTO>());
        }

        /// <summary>
        /// STUB: SP OP_RO_Metodologia_GetById no existe en BD legacy
        /// </summary>
        public Task<OP_ROMetodologiaDTO> GetMetodologiaByIdAsync(int metodologiaId)
        {
            _logger.LogWarning("[OP_RO] GetMetodologiaByIdAsync: SP 'OP_RO_Metodologia_GetById' no existe en BD legacy. Retornando DTO vacío. MetodologiaId={MetodologiaId}", metodologiaId);
            return Task.FromResult(new OP_ROMetodologiaDTO());
        }

        /// <summary>
        /// STUB: SP OP_RO_Metodologia_Save no existe en BD legacy
        /// </summary>
        public Task<int> SaveMetodologiaAsync(OP_ROMetodologiaDTO metodologia)
        {
            ValidarDatos(metodologia);
            _logger.LogWarning("[OP_RO] SaveMetodologiaAsync: SP 'OP_RO_Metodologia_Save' no existe en BD legacy. Retornando 0.");
            return Task.FromResult(0);
        }

        // ============================================
        // MATERIALES DE AYUDA
        // ============================================

        /// <summary>
        /// STUB: SP OP_RO_Materiales_Get no existe en BD legacy
        /// </summary>
        public Task<List<OP_ROMaterialAyudaDTO>> GetMaterialesAsync(OP_ROFiltrosDTO filtros)
        {
            ValidarFiltros(filtros);
            _logger.LogWarning("[OP_RO] GetMaterialesAsync: SP 'OP_RO_Materiales_Get' no existe en BD legacy. Retornando lista vacía.");
            return Task.FromResult(new List<OP_ROMaterialAyudaDTO>());
        }

        /// <summary>
        /// STUB: SP OP_RO_Material_GetById no existe en BD legacy
        /// </summary>
        public Task<OP_ROMaterialAyudaDTO> GetMaterialByIdAsync(int materialId)
        {
            _logger.LogWarning("[OP_RO] GetMaterialByIdAsync: SP 'OP_RO_Material_GetById' no existe en BD legacy. Retornando DTO vacío. MaterialId={MaterialId}", materialId);
            return Task.FromResult(new OP_ROMaterialAyudaDTO());
        }

        /// <summary>
        /// STUB: SP OP_RO_Material_Save no existe en BD legacy
        /// </summary>
        public Task<int> SaveMaterialAsync(OP_ROMaterialAyudaDTO material)
        {
            ValidarDatos(material);
            _logger.LogWarning("[OP_RO] SaveMaterialAsync: SP 'OP_RO_Material_Save' no existe en BD legacy. Retornando 0.");
            return Task.FromResult(0);
        }

        // ============================================
        // WORKFLOW: APROBACIÓN/RECHAZO
        // ============================================

        /// <summary>
        /// STUB: SP OP_RO_Revision_Aprobar no existe en BD legacy
        /// </summary>
        public Task<bool> AprobarRevisionAsync(OP_ROAprobarDTO aprobacion)
        {
            ValidarDatos(aprobacion);
            _logger.LogWarning("[OP_RO] AprobarRevisionAsync: SP 'OP_RO_Revision_Aprobar' no existe en BD legacy. Retornando false. ReviewId={ReviewId}", aprobacion.ReviewId);
            return Task.FromResult(false);
        }

        /// <summary>
        /// STUB: SP OP_RO_Revision_Rechazar no existe en BD legacy
        /// </summary>
        public Task<bool> RechazarRevisionAsync(OP_RORechazarDTO rechazo)
        {
            ValidarDatos(rechazo);
            _logger.LogWarning("[OP_RO] RechazarRevisionAsync: SP 'OP_RO_Revision_Rechazar' no existe en BD legacy. Retornando false. ReviewId={ReviewId}", rechazo.ReviewId);
            return Task.FromResult(false);
        }

        /// <summary>
        /// STUB: SP OP_RO_Revision_Historial_Get no existe en BD legacy
        /// </summary>
        public Task<List<HistorialRevisionDTO>> GetHistorialRevisionAsync(int reviewId)
        {
            _logger.LogWarning("[OP_RO] GetHistorialRevisionAsync: SP 'OP_RO_Revision_Historial_Get' no existe en BD legacy. Retornando lista vacía. ReviewId={ReviewId}", reviewId);
            return Task.FromResult(new List<HistorialRevisionDTO>());
        }

        // ============================================
        // VALIDACIONES
        // ============================================

        public void ValidarFiltros(OP_ROFiltrosDTO filtros)
        {
            if (filtros == null)
                throw new ArgumentNullException(nameof(filtros));

            if (filtros.FechaDesde > filtros.FechaHasta)
                throw new InvalidOperationException("FechaDesde no puede ser mayor a FechaHasta");

            if (filtros.PageNumber < 1)
                throw new ArgumentException("PageNumber debe ser > 0");

            if (filtros.PageSize < 1 || filtros.PageSize > 1000)
                throw new ArgumentException("PageSize debe estar entre 1 y 1000");
        }

        public void ValidarDatos(object dto)
        {
            if (dto == null)
                throw new ArgumentNullException(nameof(dto));

            // Validaciones específicas por tipo
            var tipo = dto.GetType();
            _logger.LogDebug("[OP_RO] Validando datos de tipo: {TipoNombre}", tipo.Name);
        }

        // ============================================
        // HELPERS PRIVADOS (STUBS)
        // ============================================

        /// <summary>
        /// STUB: SP OP_RO_Preguntas_Get no existe en BD legacy
        /// </summary>
        private Task<List<PreguntaDTO>> ObtenerPreguntasAsync(int cuestionarioId)
        {
            _logger.LogWarning("[OP_RO] ObtenerPreguntasAsync: SP 'OP_RO_Preguntas_Get' no existe en BD legacy. Retornando lista vacía. CuestionarioId={CuestionarioId}", cuestionarioId);
            return Task.FromResult(new List<PreguntaDTO>());
        }

        /// <summary>
        /// STUB: SP OP_RO_Pasos_Get no existe en BD legacy
        /// </summary>
        private Task<List<PasoInstructivoDTO>> ObtenerPasosAsync(int instructivoId)
        {
            _logger.LogWarning("[OP_RO] ObtenerPasosAsync: SP 'OP_RO_Pasos_Get' no existe en BD legacy. Retornando lista vacía. InstructivoId={InstructivoId}", instructivoId);
            return Task.FromResult(new List<PasoInstructivoDTO>());
        }

        /// <summary>
        /// STUB: SP OP_RO_Fases_Get no existe en BD legacy
        /// </summary>
        private Task<List<FaseMetodologiaDTO>> ObtenerFasesAsync(int metodologiaId)
        {
            _logger.LogWarning("[OP_RO] ObtenerFasesAsync: SP 'OP_RO_Fases_Get' no existe en BD legacy. Retornando lista vacía. MetodologiaId={MetodologiaId}", metodologiaId);
            return Task.FromResult(new List<FaseMetodologiaDTO>());
        }
    }
}
