using System.Data;
using Dapper;
using Microsoft.Data.SqlClient;
using MatrixNext.Data.DTOs.SGC;

namespace MatrixNext.Data.Adapters.SGC
{
    /// <summary>
    /// Adapter para Acciones de Mejora
    /// Mapea AccionesMejoraDapper de CoreProject
    /// Usa 2 Stored Procedures (Add, Edit) + SQL directo para relaciones
    /// </summary>
    public class SGCAccionMejoraAdapter : ISGCAccionMejoraAdapter
    {
        private readonly IDbConnection _connection;

        public SGCAccionMejoraAdapter(IDbConnection connection)
        {
            _connection = connection;
        }

        /// <summary>
        /// Crear nueva acciÃ³n de mejora
        /// SP: ACM_AccionMejora_Add
        /// </summary>
        public async Task<int> CreateAsync(SGCAccionMejoraCreateDto dto, long userId)
        {
            try
            {
                var parameters = new DynamicParameters();
                parameters.Add("@DescripcionAccion", dto.DescripcionAccion);
                parameters.Add("@FechaIncidente", dto.FechaIncidente);
                parameters.Add("@UsuarioReporta", dto.UsuarioReporta);
                parameters.Add("@ProcesoId", dto.ProcesoId);
                parameters.Add("@UsuarioResponsable", dto.UsuarioResponsable);
                parameters.Add("@Descripcion", dto.Descripcion);
                parameters.Add("@Correccion", dto.Correccion);
                parameters.Add("@FuenteNoConformidadId", dto.FuenteNoConformidadId);
                parameters.Add("@FuenteId", dto.FuenteId);
                parameters.Add("@RegistradoPor", userId);
                parameters.Add("@FechaRegistro", DateTime.Now);

                var result = await _connection.QuerySingleAsync<int>(
                    sql: "ACM_AccionMejora_Add",
                    param: parameters,
                    commandType: CommandType.StoredProcedure
                );

                // Agregar causas y planes si aplica
                if (dto.Causas.Count > 0)
                    await AddCausasAsync(result, dto.Causas);

                if (dto.PlanesAccion.Count > 0)
                    await AddPlanesAccionAsync(result, dto.PlanesAccion);

                return result;
            }
            catch (SqlException ex)
            {
                throw new Exception("Error en BD al crear acciÃ³n mejora. Por favor intente nuevamente.", ex);
            }
        }

        /// <summary>
        /// Obtener acciÃ³n de mejora por ID
        /// </summary>
        public async Task<SGCAccionMejoraDto> GetByIdAsync(int accionMejoraId)
        {
            try
            {
                // NOTA: Columnas FuenteNoConformidadId, FuenteId, FechaRegistro, RegistradoPor NO existen en la tabla
                // Las fuentes se manejan en ACM_AccionesMejora_Fuentes (relación N:M)
                const string sql = @"
                    SELECT 
                        AccionMejoraId,
                        DescripcionAccion,
                        FechaIncidente,
                        UsuarioReporta,
                        ProcesoId,
                        UsuarioResponsable,
                        Descripcion,
                        Correccion,
                        IsDeleted
                    FROM ACM_AccionesMejora
                    WHERE AccionMejoraId = @AccionMejoraId
                        AND IsDeleted = 0
                ";

                var parameters = new DynamicParameters();
                parameters.Add("@AccionMejoraId", accionMejoraId);

                var accion = await _connection.QuerySingleOrDefaultAsync<SGCAccionMejoraDto>(sql, parameters);

                if (accion != null)
                {
                    // Cargar causas y planes
                    accion.Causas = await GetCausasByIdAsync(accionMejoraId);
                    accion.PlanesAccion = await GetPlanesAccionByIdAsync(accionMejoraId);
                }

                return accion;
            }
            catch (SqlException ex)
            {
                throw new Exception("Error al obtener acciÃ³n. Por favor intente nuevamente.", ex);
            }
        }

        /// <summary>
        /// Listar acciones con filtros y paginaciÃ³n
        /// </summary>
        public async Task<List<SGCAccionMejoraDto>> GetByFilterAsync(int? procesoId, long? usuarioResponsable, byte? estadoId, int pageSize, int pageIndex)
        {
            try
            {
                var offset = (pageIndex - 1) * pageSize;

                // NOTA: Columnas FuenteNoConformidadId, FuenteId, FechaRegistro, RegistradoPor NO existen en la tabla
                var sql = @"
                    SELECT 
                        AccionMejoraId,
                        DescripcionAccion,
                        FechaIncidente,
                        UsuarioReporta,
                        ProcesoId,
                        UsuarioResponsable,
                        Descripcion,
                        Correccion,
                        IsDeleted
                    FROM ACM_AccionesMejora
                    WHERE IsDeleted = 0
                ";

                var conditions = new List<string>();
                var parameters = new DynamicParameters();

                if (procesoId.HasValue)
                {
                    conditions.Add("ProcesoId = @ProcesoId");
                    parameters.Add("@ProcesoId", procesoId);
                }

                if (usuarioResponsable.HasValue)
                {
                    conditions.Add("UsuarioResponsable = @UsuarioResponsable");
                    parameters.Add("@UsuarioResponsable", usuarioResponsable);
                }

                if (conditions.Count > 0)
                    sql += " AND " + string.Join(" AND ", conditions);

                sql += @" 
                    ORDER BY FechaIncidente DESC
                    OFFSET @Offset ROWS FETCH NEXT @PageSize ROWS ONLY
                ";

                parameters.Add("@Offset", offset);
                parameters.Add("@PageSize", pageSize);

                var result = await _connection.QueryAsync<SGCAccionMejoraDto>(sql, parameters);
                return result.ToList();
            }
            catch (SqlException ex)
            {
                throw new Exception("Error al filtrar acciones. Por favor intente nuevamente.", ex);
            }
        }

        /// <summary>
        /// Actualizar acciÃ³n de mejora
        /// SP: ACM_AccionesMejora_Edit
        /// </summary>
        public async Task<bool> UpdateAsync(SGCAccionMejoraUpdateDto dto, long userId)
        {
            try
            {
                var parameters = new DynamicParameters();
                parameters.Add("@AccionMejoraId", dto.AccionMejoraId);
                parameters.Add("@DescripcionAccion", dto.DescripcionAccion);
                parameters.Add("@Descripcion", dto.Descripcion);
                parameters.Add("@Correccion", dto.Correccion);
                parameters.Add("@UsuarioResponsable", dto.UsuarioResponsable);
                parameters.Add("@FuenteNoConformidadId", dto.FuenteNoConformidadId);
                parameters.Add("@FuenteId", dto.FuenteId);
                parameters.Add("@ModificadoPor", userId);
                parameters.Add("@FechaModificacion", DateTime.Now);

                var rowsAffected = await _connection.ExecuteAsync(
                    sql: "ACM_AccionesMejora_Edit",
                    param: parameters,
                    commandType: CommandType.StoredProcedure
                );

                return rowsAffected > 0;
            }
            catch (SqlException ex)
            {
                throw new Exception("Error al actualizar acciÃ³n. Por favor intente nuevamente.", ex);
            }
        }

        /// <summary>
        /// Eliminar acciÃ³n (soft delete)
        /// </summary>
        public async Task<bool> DeleteAsync(int accionMejoraId, long userId)
        {
            try
            {
                const string sql = @"
                    UPDATE ACM_AccionesMejora 
                    SET IsDeleted = 1,
                        ModificadoPor = @UsuarioId,
                        FechaModificacion = GETDATE()
                    WHERE AccionMejoraId = @AccionMejoraId
                ";

                var parameters = new DynamicParameters();
                parameters.Add("@AccionMejoraId", accionMejoraId);
                parameters.Add("@UsuarioId", userId);

                var rowsAffected = await _connection.ExecuteAsync(sql, parameters);
                return rowsAffected > 0;
            }
            catch (SqlException ex)
            {
                throw new Exception("Error al eliminar acciÃ³n. Por favor intente nuevamente.", ex);
            }
        }

        /// <summary>
        /// Obtener causas de una acciÃ³n
        /// </summary>
        public async Task<List<SGCCausaDto>> GetCausasByIdAsync(int accionMejoraId)
        {
            try
            {
                // NOTA: La tabla ACM_Causas NO tiene columna IsDeleted
                const string sql = @"
                    SELECT 
                        CausaId,
                        AccionMejoraId,
                        DescripcionCausa
                    FROM ACM_Causas
                    WHERE AccionMejoraId = @AccionMejoraId
                    ORDER BY CausaId
                ";

                var parameters = new DynamicParameters();
                parameters.Add("@AccionMejoraId", accionMejoraId);

                var result = await _connection.QueryAsync<SGCCausaDto>(sql, parameters);
                return result.ToList();
            }
            catch (Exception ex)
            {
                throw new Exception("Error al obtener causas. Por favor intente nuevamente.", ex);
            }
        }

        /// <summary>
        /// Agregar causas a una acciÃ³n
        /// </summary>
        public async Task<bool> AddCausasAsync(int accionMejoraId, List<SGCCausaCreateDto> causas)
        {
            try
            {
                // NOTA: La tabla ACM_Causas NO tiene columna FechaRegistro
                const string sql = @"
                    INSERT INTO ACM_Causas (AccionMejoraId, DescripcionCausa)
                    VALUES (@AccionMejoraId, @DescripcionCausa)
                ";

                foreach (var causa in causas)
                {
                    var parameters = new DynamicParameters();
                    parameters.Add("@AccionMejoraId", accionMejoraId);
                    parameters.Add("@DescripcionCausa", causa.DescripcionCausa);

                    await _connection.ExecuteAsync(sql, parameters);
                }

                return true;
            }
            catch (Exception ex)
            {
                throw new Exception("Error al agregar causas. Por favor intente nuevamente.", ex);
            }
        }

        /// <summary>
        /// Eliminar causa
        /// </summary>
        public async Task<bool> DeleteCausaAsync(int causaId, long userId)
        {
            try
            {
                // NOTA: La tabla ACM_Causas NO tiene columna IsDeleted - usar DELETE físico
                const string sql = @"
                    DELETE FROM ACM_Causas 
                    WHERE CausaId = @CausaId
                ";

                var parameters = new DynamicParameters();
                parameters.Add("@CausaId", causaId);

                var rowsAffected = await _connection.ExecuteAsync(sql, parameters);
                return rowsAffected > 0;
            }
            catch (Exception ex)
            {
                throw new Exception("Error al eliminar causa.", ex);
            }
        }

        /// <summary>
        /// Obtener planes de acciÃ³n de una acciÃ³n
        /// </summary>
        public async Task<List<SGCPlanAccionDto>> GetPlanesAccionByIdAsync(int accionMejoraId)
        {
            try
            {
                // NOTA: La tabla ACM_PlanesAccion NO tiene columna IsDeleted
                const string sql = @"
                    SELECT 
                        PlanAccionId,
                        AccionMejoraId,
                        DescripcionPlan,
                        FechaPlaneado,
                        FechaEjecutado,
                        EficaciaPlan,
                        FechaRevision
                    FROM ACM_PlanesAccion
                    WHERE AccionMejoraId = @AccionMejoraId
                    ORDER BY FechaPlaneado
                ";

                var parameters = new DynamicParameters();
                parameters.Add("@AccionMejoraId", accionMejoraId);

                var result = await _connection.QueryAsync<SGCPlanAccionDto>(sql, parameters);
                return result.ToList();
            }
            catch (Exception ex)
            {
                throw new Exception("Error al obtener planes de acción.", ex);
            }
        }

        /// <summary>
        /// Agregar planes de acciÃ³n
        /// </summary>
        public async Task<bool> AddPlanesAccionAsync(int accionMejoraId, List<SGCPlanAccionCreateDto> planes)
        {
            try
            {
                // NOTA: La tabla ACM_PlanesAccion NO tiene columna FechaRegistro
                const string sql = @"
                    INSERT INTO ACM_PlanesAccion (AccionMejoraId, DescripcionPlan, FechaPlaneado)
                    VALUES (@AccionMejoraId, @DescripcionPlan, @FechaPlaneado)
                ";

                foreach (var plan in planes)
                {
                    var parameters = new DynamicParameters();
                    parameters.Add("@AccionMejoraId", accionMejoraId);
                    parameters.Add("@DescripcionPlan", plan.DescripcionPlan);
                    parameters.Add("@FechaPlaneado", plan.FechaPlaneado);

                    await _connection.ExecuteAsync(sql, parameters);
                }

                return true;
            }
            catch (Exception ex)
            {
                throw new Exception("Error al agregar planes de acción.", ex);
            }
        }

        /// <summary>
        /// Actualizar plan de acciÃ³n
        /// </summary>
        public async Task<bool> UpdatePlanAccionAsync(SGCPlanAccionUpdateDto dto, long userId)
        {
            try
            {
                // NOTA: La tabla ACM_PlanesAccion NO tiene columna FechaModificacion
                const string sql = @"
                    UPDATE ACM_PlanesAccion 
                    SET DescripcionPlan = @DescripcionPlan,
                        FechaPlaneado = @FechaPlaneado,
                        FechaEjecutado = @FechaEjecutado,
                        EficaciaPlan = @EficaciaPlan,
                        FechaRevision = @FechaRevision
                    WHERE PlanAccionId = @PlanAccionId
                ";

                var parameters = new DynamicParameters();
                parameters.Add("@PlanAccionId", dto.PlanAccionId);
                parameters.Add("@DescripcionPlan", dto.DescripcionPlan);
                parameters.Add("@FechaPlaneado", dto.FechaPlaneado);
                parameters.Add("@FechaEjecutado", dto.FechaEjecutado);
                parameters.Add("@EficaciaPlan", dto.EficaciaPlan);
                parameters.Add("@FechaRevision", dto.FechaRevision);

                var rowsAffected = await _connection.ExecuteAsync(sql, parameters);
                return rowsAffected > 0;
            }
            catch (Exception ex)
            {
                throw new Exception("Error al actualizar plan.", ex);
            }
        }

        /// <summary>
        /// Eliminar plan de acciÃ³n
        /// </summary>
        public async Task<bool> DeletePlanAccionAsync(int planAccionId, long userId)
        {
            try
            {
                // NOTA: La tabla ACM_PlanesAccion NO tiene columna IsDeleted - usar DELETE físico
                const string sql = @"
                    DELETE FROM ACM_PlanesAccion 
                    WHERE PlanAccionId = @PlanAccionId
                ";

                var parameters = new DynamicParameters();
                parameters.Add("@PlanAccionId", planAccionId);

                var rowsAffected = await _connection.ExecuteAsync(sql, parameters);
                return rowsAffected > 0;
            }
            catch (Exception ex)
            {
                throw new Exception("Error al eliminar plan.", ex);
            }
        }

        /// <summary>
        /// Obtener procesos (catÃ¡logo)
        /// </summary>
        public async Task<List<SGCProcesoDto>> GetProcesosAsync()
        {
            try
            {
                // NOTA: La tabla ACM_Procesos NO tiene columna IsDeleted
                const string sql = @"
                    SELECT 
                        ProcesoId,
                        NombreProceso
                    FROM ACM_Procesos
                    ORDER BY NombreProceso
                ";

                var result = await _connection.QueryAsync<SGCProcesoDto>(sql);
                return result.ToList();
            }
            catch (Exception ex)
            {
                throw new Exception("Error al obtener procesos.", ex);
            }
        }

        /// <summary>
        /// Obtener fuentes de no conformidad (catÃ¡logo)
        /// </summary>
        public async Task<List<SGCFuenteNoConformidadDto>> GetFuentesNoConformidadAsync()
        {
            try
            {
                // NOTA: Tabla ACM_FuentesNoConformidad NO EXISTE - usar ACM_TiposFuente
                const string sql = @"
                    SELECT 
                        TipoFuenteId AS FuenteNoConformidadId,
                        TipoFuenteNombre AS NombreFuente
                    FROM ACM_TiposFuente
                    ORDER BY TipoFuenteNombre
                ";

                var result = await _connection.QueryAsync<SGCFuenteNoConformidadDto>(sql);
                return result.ToList();
            }
            catch (Exception ex)
            {
                throw new Exception("Error al obtener fuentes.", ex);
            }
        }

        /// <summary>
        /// Obtener fuentes especÃ­ficas por tipo
        /// </summary>
        public async Task<List<SGCFuenteDto>> GetFuentesByTypeAsync(int fuenteNoConformidadId)
        {
            try
            {
                // NOTA: Tabla ACM_Fuentes NO EXISTE - usar ACM_AccionesMejora_Fuentes
                // La estructura real es: AccionMejoraFuenteId, AccionMejoraId, TipoFuenteId, FuenteId
                const string sql = @"
                    SELECT DISTINCT
                        FuenteId,
                        TipoFuenteId AS FuenteNoConformidadId,
                        CAST(FuenteId AS NVARCHAR(50)) AS NombreFuente
                    FROM ACM_AccionesMejora_Fuentes
                    WHERE TipoFuenteId = @FuenteNoConformidadId
                    ORDER BY FuenteId
                ";

                var parameters = new DynamicParameters();
                parameters.Add("@FuenteNoConformidadId", fuenteNoConformidadId);

                var result = await _connection.QueryAsync<SGCFuenteDto>(sql, parameters);
                return result.ToList();
            }
            catch (Exception ex)
            {
                throw new Exception("Error al obtener fuentes por tipo.", ex);
            }
        }
    }
}

