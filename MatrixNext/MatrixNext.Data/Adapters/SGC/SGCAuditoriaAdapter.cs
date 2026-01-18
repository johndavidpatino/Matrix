using System.Data;
using Dapper;
using Microsoft.Data.SqlClient;
using MatrixNext.Data.DTOs.SGC;

namespace MatrixNext.Data.Adapters.SGC
{
    /// <summary>
    /// Adapter para Auditorías Internas
    /// Mapea SGC_AuditoriasInternasDapper de CoreProject
    /// Usa 8 Stored Procedures exactos de SQL Server
    /// </summary>
    public class SGCAuditoriaAdapter : ISGCAuditoriaAdapter
    {
        private readonly IDbConnection _connection;

        public SGCAuditoriaAdapter(IDbConnection connection)
        {
            _connection = connection;
        }

        /// <summary>
        /// Crear nueva auditoría
        /// SP: SGC_AuditoriasInternas_Add
        /// </summary>
        public async Task<int> CreateAsync(SGCAuditoriaCreateDto dto, long userId)
        {
            try
            {
                var parameters = new DynamicParameters();
                parameters.Add("@AuditorId", dto.AuditorId);
                parameters.Add("@AreaAuditada", dto.AreaAuditada);
                parameters.Add("@ProcesoAuditado", dto.ProcesoAuditado);
                parameters.Add("@FechaLimiteAuditoria", dto.FechaLimiteAuditoria);
                parameters.Add("@TiposAuditoria", string.Join(",", dto.TiposAuditoria));
                parameters.Add("@NormativasAAuditar", string.Join(",", dto.NormativasAAuditar));
                parameters.Add("@FechaRegistro", DateTime.Now);
                parameters.Add("@UsuarioRegistraId", userId);

                var result = await _connection.QuerySingleAsync<int>(
                    sql: "SGC_AuditoriasInternas_Add",
                    param: parameters,
                    commandType: CommandType.StoredProcedure
                );

                return result;
            }
            catch (SqlException ex)
            {
                throw new Exception("Error en BD al crear auditoría.", ex);
            }
        }

        /// <summary>
        /// Obtener auditoría por ID
        /// SP: SGC_AI_AuditoriasBy
        /// </summary>
        public async Task<SGCAuditoriaDto> GetByIdAsync(int auditoriaId)
        {
            try
            {
                var parameters = new DynamicParameters();
                parameters.Add("@AuditoriasId", auditoriaId);
                parameters.Add("@AuditorId", null);
                parameters.Add("@EstadoId", null);
                parameters.Add("@AnoAuditoria", null);
                parameters.Add("@AuditadoId", null);
                parameters.Add("@pageSize", 1);
                parameters.Add("@pageIndex", 1);

                var result = await _connection.QuerySingleOrDefaultAsync<SGCAuditoriaDto>(
                    sql: "SGC_AI_AuditoriasBy",
                    param: parameters,
                    commandType: CommandType.StoredProcedure
                );

                return result;
            }
            catch (SqlException ex)
            {
                throw new Exception($"Error al obtener auditoría {auditoriaId}.", ex);
            }
        }

        /// <summary>
        /// Listar auditorías con filtros y paginación
        /// SP: SGC_AI_AuditoriasBy
        /// </summary>
        public async Task<List<SGCAuditoriaDto>> GetByFilterAsync(byte? estadoId, long? auditorId, int? anoAuditoria, long? auditadoId, int pageSize, int pageIndex)
        {
            try
            {
                var parameters = new DynamicParameters();
                parameters.Add("@AuditorId", auditorId);
                parameters.Add("@EstadoId", estadoId);
                parameters.Add("@AnoAuditoria", anoAuditoria);
                parameters.Add("@AuditadoId", auditadoId);
                parameters.Add("@pageSize", pageSize);
                parameters.Add("@pageIndex", pageIndex);

                var result = await _connection.QueryAsync<SGCAuditoriaDto>(
                    sql: "SGC_AI_AuditoriasBy",
                    param: parameters,
                    commandType: CommandType.StoredProcedure
                );

                return result.ToList();
            }
            catch (SqlException ex)
            {
                throw new Exception("Error al filtrar auditorías.", ex);
            }
        }

        /// <summary>
        /// Actualizar estado de auditoría
        /// Registra en SGC_AI_Auditorias_EstadosLog
        /// </summary>
        public async Task<bool> UpdateEstadoAsync(int auditoriaId, byte nuevoEstadoId, long userId)
        {
            try
            {
                // NOTA: La columna en la tabla es UsuarioRegistro (no UsuarioRegistra)
                const string sql = @"
                    UPDATE SGC_AI_Auditorias 
                    SET SGC_AI_EstadoId = @NuevoEstadoId
                    WHERE Id = @AuditoriaId;
                    
                    INSERT INTO SGC_AI_Auditorias_EstadosLog (SGC_AI_AuditoriaId, SGC_AI_Estado, FechaRegistro, UsuarioRegistro)
                    VALUES (@AuditoriaId, @NuevoEstadoId, GETDATE(), @UsuarioRegistro);
                ";

                var parameters = new DynamicParameters();
                parameters.Add("@AuditoriaId", auditoriaId);
                parameters.Add("@NuevoEstadoId", nuevoEstadoId);
                parameters.Add("@UsuarioRegistro", userId);

                var rowsAffected = await _connection.ExecuteAsync(sql, parameters);
                return rowsAffected > 0;
            }
            catch (SqlException ex)
            {
                throw new Exception("Error al actualizar estado.", ex);
            }
        }

        /// <summary>
        /// Crear informe del auditor
        /// SP: SGC_AI_AuditoriaInforme_Add
        /// </summary>
        public async Task<int> CreateInformeAsync(SGCAuditoriaInformeCreateDto dto, long userId)
        {
            try
            {
                // Mapear hallazgos a XML
                var hallazgosXml = MapHallazgosToXml(dto.Hallazgos);
                var auditadosStr = string.Join(",", dto.AuditadosIds);

                var parameters = new DynamicParameters();
                parameters.Add("@AuditoriaId", dto.AuditoriaId);
                parameters.Add("@FechaAuditoria", dto.FechaAuditoria);
                parameters.Add("@Fortalezas", dto.Fortalezas);
                parameters.Add("@Auditados", auditadosStr);
                parameters.Add("@Hallazgos", hallazgosXml);
                parameters.Add("@ArchivoInformeAuditoriaNombre", dto.ArchivoNombre ?? "");
                parameters.Add("@ArchivoInformeAuditoriaId", Guid.NewGuid().ToString());
                parameters.Add("@ArchivoInformeAuditoriaTamanoBytes", dto.ArchivoBase64?.Length ?? 0);
                parameters.Add("@UsuarioRegistra", userId);
                parameters.Add("@FechaRegistro", DateTime.Now);

                var result = await _connection.ExecuteAsync(
                    sql: "SGC_AI_AuditoriaInforme_Add",
                    param: parameters,
                    commandType: CommandType.StoredProcedure
                );

                return result;
            }
            catch (SqlException ex)
            {
                throw new Exception("Error al crear informe de auditoría.", ex);
            }
        }

        /// <summary>
        /// Obtener informe auditor por auditoría
        /// SP: SGC_AI_Auditorias_InformeAuditorByAuditoriaId
        /// </summary>
        public async Task<SGCAuditoriaInformeDto> GetInformeByIdAsync(int auditoriaId)
        {
            try
            {
                var parameters = new DynamicParameters();
                parameters.Add("@AuditoriaId", auditoriaId);

                var informe = await _connection.QuerySingleOrDefaultAsync<SGCAuditoriaInformeDto>(
                    sql: "SGC_AI_Auditorias_InformeAuditorByAuditoriaId",
                    param: parameters,
                    commandType: CommandType.StoredProcedure
                );

                if (informe != null)
                {
                    // Cargar auditados y hallazgos relacionados
                    informe.Auditados = await GetAuditadosByIdAsync(auditoriaId);
                    informe.Hallazgos = await GetHallazgosByIdAsync(auditoriaId);
                }

                return informe;
            }
            catch (SqlException ex)
            {
                throw new Exception($"Error al obtener informe de auditoría {auditoriaId}.", ex);
            }
        }

        /// <summary>
        /// Obtener auditados de una auditoría
        /// SP: SGC_AI_Auditorias_InformeAuditor_AuditadosByAuditoriaId
        /// </summary>
        public async Task<List<SGCAuditadoDto>> GetAuditadosByIdAsync(int auditoriaId)
        {
            try
            {
                var parameters = new DynamicParameters();
                parameters.Add("@AuditoriaId", auditoriaId);

                var result = await _connection.QueryAsync<SGCAuditadoDto>(
                    sql: "SGC_AI_Auditorias_InformeAuditor_AuditadosByAuditoriaId",
                    param: parameters,
                    commandType: CommandType.StoredProcedure
                );

                return result.ToList();
            }
            catch (SqlException ex)
            {
                throw new Exception("Error al obtener auditados.", ex);
            }
        }

        /// <summary>
        /// Obtener hallazgos de una auditoría
        /// SP: SGC_AI_Auditorias_InformeAuditor_HallazgosByAuditoriaId
        /// </summary>
        public async Task<List<SGCHallazgoDto>> GetHallazgosByIdAsync(int auditoriaId)
        {
            try
            {
                var parameters = new DynamicParameters();
                parameters.Add("@AuditoriaId", auditoriaId);

                var result = await _connection.QueryAsync<SGCHallazgoDto>(
                    sql: "SGC_AI_Auditorias_InformeAuditor_HallazgosByAuditoriaId",
                    param: parameters,
                    commandType: CommandType.StoredProcedure
                );

                return result.ToList();
            }
            catch (SqlException ex)
            {
                throw new Exception("Error al obtener hallazgos.", ex);
            }
        }

        /// <summary>
        /// Obtener normativas disponibles (catálogo)
        /// </summary>
        public async Task<List<SGCNormativaDto>> GetNormativasAsync()
        {
            try
            {
                // NOTA: La tabla SGC_Normativas NO tiene columna IsDeleted
                const string sql = @"
                    SELECT 
                        Id,
                        Estandar
                    FROM SGC_Normativas
                    ORDER BY Estandar
                ";

                var result = await _connection.QueryAsync<SGCNormativaDto>(sql);
                return result.ToList();
            }
            catch (Exception ex)
            {
                throw new Exception("Error al obtener normativas.", ex);
            }
        }

        /// <summary>
        /// Obtener tipos de auditoría disponibles (catálogo)
        /// </summary>
        public async Task<List<SGCTipoAuditoriaDto>> GetTiposAuditoriaAsync()
        {
            try
            {
                // NOTA: La tabla SGC_AI_Tipos NO tiene columna IsDeleted
                const string sql = @"
                    SELECT 
                        Id,
                        TipoAuditoria
                    FROM SGC_AI_Tipos
                    ORDER BY TipoAuditoria
                ";

                var result = await _connection.QueryAsync<SGCTipoAuditoriaDto>(sql);
                return result.ToList();
            }
            catch (Exception ex)
            {
                throw new Exception("Error al obtener tipos de auditoría.", ex);
            }
        }

        /// <summary>
        /// Obtener tipos de hallazgo disponibles (catálogo)
        /// </summary>
        public async Task<List<SGCTipoHallazgoDto>> GetTiposHallazgoAsync()
        {
            try
            {
                // NOTA: Tabla es SGC_AI_TiposHallazgos (plural), columna es TipoHallazgo (no Nombre), sin IsDeleted
                const string sql = @"
                    SELECT 
                        Id,
                        TipoHallazgo AS Nombre
                    FROM SGC_AI_TiposHallazgos
                    ORDER BY TipoHallazgo
                ";

                var result = await _connection.QueryAsync<SGCTipoHallazgoDto>(sql);
                return result.ToList();
            }
            catch (Exception ex)
            {
                throw new Exception("Error al obtener tipos de hallazgo.", ex);
            }
        }

        /// <summary>
        /// Obtener estados de auditoría disponibles (catálogo)
        /// </summary>
        public async Task<List<SGCEstadoAuditoriaDto>> GetEstadosAsync()
        {
            try
            {
                // NOTA: Tabla SGC_AI_Estados solo tiene Id y EstadoAuditoria (no Nombre, Descripcion ni IsDeleted)
                const string sql = @"
                    SELECT 
                        Id,
                        EstadoAuditoria AS Nombre,
                        EstadoAuditoria AS Descripcion
                    FROM SGC_AI_Estados
                    ORDER BY Id
                ";

                var result = await _connection.QueryAsync<SGCEstadoAuditoriaDto>(sql);
                return result.ToList();
            }
            catch (Exception ex)
            {
                throw new Exception("Error al obtener estados.", ex);
            }
        }

        /// <summary>
        /// Mapear hallazgos a formato XML para SP
        /// </summary>
        private string MapHallazgosToXml(List<SGCHallazgoCreateDto> hallazgos)
        {
            var xml = "<Hallazgos>";
            foreach (var h in hallazgos)
            {
                xml += "<Hallazgo>";
                xml += $"<Descripcion>{System.Xml.XmlConvert.EncodeLocalName(h.Hallazgo)}</Descripcion>";
                xml += $"<TipoId>{h.TipoHallazgoId}</TipoId>";
                xml += "</Hallazgo>";
            }
            xml += "</Hallazgos>";
            return xml;
        }
    }
}
