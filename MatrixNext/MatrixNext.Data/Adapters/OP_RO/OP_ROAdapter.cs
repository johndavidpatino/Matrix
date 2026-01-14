using Dapper;
using MatrixNext.Data.Models.OP_RO;
using Microsoft.Extensions.Logging;
using System.Data;

namespace MatrixNext.Data.Adapters.OP_RO
{
    /// <summary>
    /// Implementación Adapter para Operational Review
    /// Utiliza Dapper para ejecutar SP contra BD
    /// REGLA 2: Mapeo exacto nombres/parámetros desde CoreProject
    /// REGLA 3: Validación respuestas
    /// REGLA 4: Ejecución SP
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

        public async Task<List<OP_ROReviewDTO>> GetRevisionesAsync(OP_ROFiltrosDTO filtros)
        {
            try
            {
                ValidarFiltros(filtros);
                _logger.LogInformation("[OP_RO] Iniciando GetRevisiones con filtros");

                var parameters = new DynamicParameters();
                parameters.Add("@TipoRevision", filtros.TipoRevision ?? "");
                parameters.Add("@Estado", filtros.Estado ?? "");
                parameters.Add("@FechaDesde", filtros.FechaDesde ?? DateTime.MinValue);
                parameters.Add("@FechaHasta", filtros.FechaHasta ?? DateTime.Now);
                parameters.Add("@UsuarioId", filtros.UsuarioId);
                parameters.Add("@NombreDocumento", filtros.NombreDocumento ?? "");
                parameters.Add("@PageNumber", filtros.PageNumber);
                parameters.Add("@PageSize", filtros.PageSize);

                var result = await _dbConnection.QueryAsync<OP_ROReviewDTO>(
                    "OP_RO_Revisiones_Get",
                    parameters,
                    commandType: CommandType.StoredProcedure,
                    commandTimeout: 300);

                _logger.LogInformation($"[OP_RO] GetRevisiones retornó {result.Count()} registros");
                return result.ToList();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "[OP_RO] Error en GetRevisiones");
                throw;
            }
        }

        public async Task<OP_ROReviewDTO> GetRevisionByIdAsync(int reviewId)
        {
            try
            {
                _logger.LogInformation($"[OP_RO] Iniciando GetRevisionById: {reviewId}");

                var parameters = new DynamicParameters();
                parameters.Add("@ReviewId", reviewId);

                var result = await _dbConnection.QueryFirstOrDefaultAsync<OP_ROReviewDTO>(
                    "OP_RO_Revision_GetById",
                    parameters,
                    commandType: CommandType.StoredProcedure,
                    commandTimeout: 60);

                return result ?? new OP_ROReviewDTO();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"[OP_RO] Error en GetRevisionById: {reviewId}");
                throw;
            }
        }

        // ============================================
        // CUESTIONARIOS
        // ============================================

        public async Task<List<OP_ROCuestionarioDTO>> GetCuestionariosAsync(OP_ROFiltrosDTO filtros)
        {
            try
            {
                ValidarFiltros(filtros);
                _logger.LogInformation("[OP_RO] Iniciando GetCuestionarios");

                var parameters = new DynamicParameters();
                parameters.Add("@Estado", filtros.Estado ?? "");
                parameters.Add("@FechaDesde", filtros.FechaDesde ?? DateTime.MinValue);
                parameters.Add("@FechaHasta", filtros.FechaHasta ?? DateTime.Now);
                parameters.Add("@PageNumber", filtros.PageNumber);
                parameters.Add("@PageSize", filtros.PageSize);

                var result = await _dbConnection.QueryAsync<OP_ROCuestionarioDTO>(
                    "OP_RO_Cuestionarios_Get",
                    parameters,
                    commandType: CommandType.StoredProcedure,
                    commandTimeout: 300);

                return result.ToList();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "[OP_RO] Error en GetCuestionarios");
                throw;
            }
        }

        public async Task<OP_ROCuestionarioDTO> GetCuestionarioByIdAsync(int cuestionarioId)
        {
            try
            {
                _logger.LogInformation($"[OP_RO] Iniciando GetCuestionarioById: {cuestionarioId}");

                // Obtener cuestionario base
                var parameters = new DynamicParameters();
                parameters.Add("@CuestionarioId", cuestionarioId);

                var result = await _dbConnection.QueryFirstOrDefaultAsync<OP_ROCuestionarioDTO>(
                    "OP_RO_Cuestionario_GetById",
                    parameters,
                    commandType: CommandType.StoredProcedure,
                    commandTimeout: 60);

                if (result == null)
                    return new OP_ROCuestionarioDTO();

                // Obtener preguntas (puede ser en SP separado o en multi-resultado)
                result.Preguntas = await ObtenerPreguntasAsync(cuestionarioId);

                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"[OP_RO] Error en GetCuestionarioById: {cuestionarioId}");
                throw;
            }
        }

        public async Task<int> SaveCuestionarioAsync(OP_ROCuestionarioDTO cuestionario)
        {
            try
            {
                ValidarDatos(cuestionario);
                _logger.LogInformation($"[OP_RO] Guardando cuestionario: {cuestionario.Titulo}");

                var parameters = new DynamicParameters();
                parameters.Add("@CuestionarioId", cuestionario.CuestionarioId);
                parameters.Add("@Titulo", cuestionario.Titulo);
                parameters.Add("@Descripcion", cuestionario.Descripcion);
                parameters.Add("@NumeroPreguntas", cuestionario.NumeroPreguntas);
                parameters.Add("@Estado", cuestionario.Estado);
                parameters.Add("@VersionId", cuestionario.VersionId);
                parameters.Add("@IdOutput", dbType: DbType.Int32, direction: ParameterDirection.Output);

                await _dbConnection.ExecuteAsync(
                    "OP_RO_Cuestionario_Save",
                    parameters,
                    commandType: CommandType.StoredProcedure,
                    commandTimeout: 300);

                var id = parameters.Get<int>("@IdOutput");
                _logger.LogInformation($"[OP_RO] Cuestionario guardado con ID: {id}");

                return id;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "[OP_RO] Error en SaveCuestionario");
                throw;
            }
        }

        // ============================================
        // INSTRUCTIVOS
        // ============================================

        public async Task<List<OP_ROInstructivoDTO>> GetInstructivosAsync(OP_ROFiltrosDTO filtros)
        {
            try
            {
                ValidarFiltros(filtros);
                _logger.LogInformation("[OP_RO] Iniciando GetInstructivos");

                var parameters = new DynamicParameters();
                parameters.Add("@Estado", filtros.Estado ?? "");
                parameters.Add("@FechaDesde", filtros.FechaDesde ?? DateTime.MinValue);
                parameters.Add("@FechaHasta", filtros.FechaHasta ?? DateTime.Now);
                parameters.Add("@PageNumber", filtros.PageNumber);
                parameters.Add("@PageSize", filtros.PageSize);

                var result = await _dbConnection.QueryAsync<OP_ROInstructivoDTO>(
                    "OP_RO_Instructivos_Get",
                    parameters,
                    commandType: CommandType.StoredProcedure,
                    commandTimeout: 300);

                return result.ToList();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "[OP_RO] Error en GetInstructivos");
                throw;
            }
        }

        public async Task<OP_ROInstructivoDTO> GetInstructivoByIdAsync(int instructivoId)
        {
            try
            {
                _logger.LogInformation($"[OP_RO] Iniciando GetInstructivoById: {instructivoId}");

                var parameters = new DynamicParameters();
                parameters.Add("@InstructivoId", instructivoId);

                var result = await _dbConnection.QueryFirstOrDefaultAsync<OP_ROInstructivoDTO>(
                    "OP_RO_Instructivo_GetById",
                    parameters,
                    commandType: CommandType.StoredProcedure,
                    commandTimeout: 60);

                if (result == null)
                    return new OP_ROInstructivoDTO();

                result.Pasos = await ObtenerPasosAsync(instructivoId);
                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"[OP_RO] Error en GetInstructivoById: {instructivoId}");
                throw;
            }
        }

        public async Task<int> SaveInstructivoAsync(OP_ROInstructivoDTO instructivo)
        {
            try
            {
                ValidarDatos(instructivo);
                _logger.LogInformation($"[OP_RO] Guardando instructivo: {instructivo.Titulo}");

                var parameters = new DynamicParameters();
                parameters.Add("@InstructivoId", instructivo.InstructivoId);
                parameters.Add("@Titulo", instructivo.Titulo);
                parameters.Add("@Contenido", instructivo.Contenido);
                parameters.Add("@Estado", instructivo.Estado);
                parameters.Add("@VersionId", instructivo.VersionId);
                parameters.Add("@OrdenCampo", instructivo.OrdenCampo);
                parameters.Add("@IdOutput", dbType: DbType.Int32, direction: ParameterDirection.Output);

                await _dbConnection.ExecuteAsync(
                    "OP_RO_Instructivo_Save",
                    parameters,
                    commandType: CommandType.StoredProcedure,
                    commandTimeout: 300);

                var id = parameters.Get<int>("@IdOutput");
                return id;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "[OP_RO] Error en SaveInstructivo");
                throw;
            }
        }

        // ============================================
        // METODOLOGÍAS
        // ============================================

        public async Task<List<OP_ROMetodologiaDTO>> GetMetodologiasAsync(OP_ROFiltrosDTO filtros)
        {
            try
            {
                ValidarFiltros(filtros);
                _logger.LogInformation("[OP_RO] Iniciando GetMetodologias");

                var parameters = new DynamicParameters();
                parameters.Add("@Estado", filtros.Estado ?? "");
                parameters.Add("@FechaDesde", filtros.FechaDesde ?? DateTime.MinValue);
                parameters.Add("@FechaHasta", filtros.FechaHasta ?? DateTime.Now);
                parameters.Add("@PageNumber", filtros.PageNumber);
                parameters.Add("@PageSize", filtros.PageSize);

                var result = await _dbConnection.QueryAsync<OP_ROMetodologiaDTO>(
                    "OP_RO_Metodologias_Get",
                    parameters,
                    commandType: CommandType.StoredProcedure,
                    commandTimeout: 300);

                return result.ToList();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "[OP_RO] Error en GetMetodologias");
                throw;
            }
        }

        public async Task<OP_ROMetodologiaDTO> GetMetodologiaByIdAsync(int metodologiaId)
        {
            try
            {
                _logger.LogInformation($"[OP_RO] Iniciando GetMetodologiaById: {metodologiaId}");

                var parameters = new DynamicParameters();
                parameters.Add("@MetodologiaId", metodologiaId);

                var result = await _dbConnection.QueryFirstOrDefaultAsync<OP_ROMetodologiaDTO>(
                    "OP_RO_Metodologia_GetById",
                    parameters,
                    commandType: CommandType.StoredProcedure,
                    commandTimeout: 60);

                if (result == null)
                    return new OP_ROMetodologiaDTO();

                result.Fases = await ObtenerFasesAsync(metodologiaId);
                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"[OP_RO] Error en GetMetodologiaById: {metodologiaId}");
                throw;
            }
        }

        public async Task<int> SaveMetodologiaAsync(OP_ROMetodologiaDTO metodologia)
        {
            try
            {
                ValidarDatos(metodologia);
                _logger.LogInformation($"[OP_RO] Guardando metodología: {metodologia.Nombre}");

                var parameters = new DynamicParameters();
                parameters.Add("@MetodologiaId", metodologia.MetodologiaId);
                parameters.Add("@Nombre", metodologia.Nombre);
                parameters.Add("@Descripcion", metodologia.Descripcion);
                parameters.Add("@Alcance", metodologia.Alcance);
                parameters.Add("@Estado", metodologia.Estado);
                parameters.Add("@VersionId", metodologia.VersionId);
                parameters.Add("@IdOutput", dbType: DbType.Int32, direction: ParameterDirection.Output);

                await _dbConnection.ExecuteAsync(
                    "OP_RO_Metodologia_Save",
                    parameters,
                    commandType: CommandType.StoredProcedure,
                    commandTimeout: 300);

                var id = parameters.Get<int>("@IdOutput");
                return id;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "[OP_RO] Error en SaveMetodologia");
                throw;
            }
        }

        // ============================================
        // MATERIALES DE AYUDA
        // ============================================

        public async Task<List<OP_ROMaterialAyudaDTO>> GetMaterialesAsync(OP_ROFiltrosDTO filtros)
        {
            try
            {
                ValidarFiltros(filtros);
                _logger.LogInformation("[OP_RO] Iniciando GetMateriales");

                var parameters = new DynamicParameters();
                parameters.Add("@Estado", filtros.Estado ?? "");
                parameters.Add("@FechaDesde", filtros.FechaDesde ?? DateTime.MinValue);
                parameters.Add("@FechaHasta", filtros.FechaHasta ?? DateTime.Now);
                parameters.Add("@PageNumber", filtros.PageNumber);
                parameters.Add("@PageSize", filtros.PageSize);

                var result = await _dbConnection.QueryAsync<OP_ROMaterialAyudaDTO>(
                    "OP_RO_Materiales_Get",
                    parameters,
                    commandType: CommandType.StoredProcedure,
                    commandTimeout: 300);

                return result.ToList();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "[OP_RO] Error en GetMateriales");
                throw;
            }
        }

        public async Task<OP_ROMaterialAyudaDTO> GetMaterialByIdAsync(int materialId)
        {
            try
            {
                _logger.LogInformation($"[OP_RO] Iniciando GetMaterialById: {materialId}");

                var parameters = new DynamicParameters();
                parameters.Add("@MaterialId", materialId);

                var result = await _dbConnection.QueryFirstOrDefaultAsync<OP_ROMaterialAyudaDTO>(
                    "OP_RO_Material_GetById",
                    parameters,
                    commandType: CommandType.StoredProcedure,
                    commandTimeout: 60);

                return result ?? new OP_ROMaterialAyudaDTO();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"[OP_RO] Error en GetMaterialById: {materialId}");
                throw;
            }
        }

        public async Task<int> SaveMaterialAsync(OP_ROMaterialAyudaDTO material)
        {
            try
            {
                ValidarDatos(material);
                _logger.LogInformation($"[OP_RO] Guardando material: {material.Titulo}");

                var parameters = new DynamicParameters();
                parameters.Add("@MaterialId", material.MaterialId);
                parameters.Add("@Titulo", material.Titulo);
                parameters.Add("@Tipo", material.Tipo);
                parameters.Add("@ContenidoUrl", material.ContenidoUrl);
                parameters.Add("@Estado", material.Estado);
                parameters.Add("@VersionId", material.VersionId);
                parameters.Add("@TamanoMB", material.TamanoMB);
                parameters.Add("@IdOutput", dbType: DbType.Int32, direction: ParameterDirection.Output);

                await _dbConnection.ExecuteAsync(
                    "OP_RO_Material_Save",
                    parameters,
                    commandType: CommandType.StoredProcedure,
                    commandTimeout: 300);

                var id = parameters.Get<int>("@IdOutput");
                return id;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "[OP_RO] Error en SaveMaterial");
                throw;
            }
        }

        // ============================================
        // WORKFLOW: APROBACIÓN/RECHAZO
        // ============================================

        public async Task<bool> AprobarRevisionAsync(OP_ROAprobarDTO aprobacion)
        {
            try
            {
                ValidarDatos(aprobacion);
                _logger.LogInformation($"[OP_RO] Aprobando revisión: {aprobacion.ReviewId}");

                var parameters = new DynamicParameters();
                parameters.Add("@ReviewId", aprobacion.ReviewId);
                parameters.Add("@UsuarioRevisorId", aprobacion.UsuarioRevisorId);
                parameters.Add("@Comentarios", aprobacion.Comentarios ?? "");

                var result = await _dbConnection.ExecuteAsync(
                    "OP_RO_Revision_Aprobar",
                    parameters,
                    commandType: CommandType.StoredProcedure,
                    commandTimeout: 60);

                return result > 0;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "[OP_RO] Error en AprobarRevision");
                throw;
            }
        }

        public async Task<bool> RechazarRevisionAsync(OP_RORechazarDTO rechazo)
        {
            try
            {
                ValidarDatos(rechazo);
                _logger.LogInformation($"[OP_RO] Rechazando revisión: {rechazo.ReviewId}");

                var parameters = new DynamicParameters();
                parameters.Add("@ReviewId", rechazo.ReviewId);
                parameters.Add("@UsuarioRevisorId", rechazo.UsuarioRevisorId);
                parameters.Add("@MotivoRechazo", rechazo.MotivoRechazo ?? "");
                parameters.Add("@Comentarios", rechazo.Comentarios ?? "");

                var result = await _dbConnection.ExecuteAsync(
                    "OP_RO_Revision_Rechazar",
                    parameters,
                    commandType: CommandType.StoredProcedure,
                    commandTimeout: 60);

                return result > 0;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "[OP_RO] Error en RechazarRevision");
                throw;
            }
        }

        public async Task<List<HistorialRevisionDTO>> GetHistorialRevisionAsync(int reviewId)
        {
            try
            {
                _logger.LogInformation($"[OP_RO] Obteniendo historial de revisión: {reviewId}");

                var parameters = new DynamicParameters();
                parameters.Add("@ReviewId", reviewId);

                var result = await _dbConnection.QueryAsync<HistorialRevisionDTO>(
                    "OP_RO_Revision_Historial_Get",
                    parameters,
                    commandType: CommandType.StoredProcedure,
                    commandTimeout: 60);

                return result.ToList();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "[OP_RO] Error en GetHistorialRevision");
                throw;
            }
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
            _logger.LogInformation($"[OP_RO] Validando datos de tipo: {tipo.Name}");

            // TODO: Implementar validaciones específicas según tipo DTO
        }

        // ============================================
        // HELPERS PRIVADOS
        // ============================================

        private async Task<List<PreguntaDTO>> ObtenerPreguntasAsync(int cuestionarioId)
        {
            var parameters = new DynamicParameters();
            parameters.Add("@CuestionarioId", cuestionarioId);

            var result = await _dbConnection.QueryAsync<PreguntaDTO>(
                "OP_RO_Preguntas_Get",
                parameters,
                commandType: CommandType.StoredProcedure,
                commandTimeout: 60);

            return result.ToList();
        }

        private async Task<List<PasoInstructivoDTO>> ObtenerPasosAsync(int instructivoId)
        {
            var parameters = new DynamicParameters();
            parameters.Add("@InstructivoId", instructivoId);

            var result = await _dbConnection.QueryAsync<PasoInstructivoDTO>(
                "OP_RO_Pasos_Get",
                parameters,
                commandType: CommandType.StoredProcedure,
                commandTimeout: 60);

            return result.ToList();
        }

        private async Task<List<FaseMetodologiaDTO>> ObtenerFasesAsync(int metodologiaId)
        {
            var parameters = new DynamicParameters();
            parameters.Add("@MetodologiaId", metodologiaId);

            var result = await _dbConnection.QueryAsync<FaseMetodologiaDTO>(
                "OP_RO_Fases_Get",
                parameters,
                commandType: CommandType.StoredProcedure,
                commandTimeout: 60);

            return result.ToList();
        }
    }
}
