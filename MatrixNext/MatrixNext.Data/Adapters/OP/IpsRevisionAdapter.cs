using Dapper;
using MatrixNext.Data.Context;
using MatrixNext.Data.Models.OP;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace MatrixNext.Data.Adapters.OP
{
    /// <summary>
    /// Adapter para acceso a datos de revisiones IPS por tarea
    /// </summary>
    public class IpsRevisionAdapter : IIpsRevisionAdapter
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<IpsRevisionAdapter> _logger;

        public IpsRevisionAdapter(ApplicationDbContext context, ILogger<IpsRevisionAdapter> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<IEnumerable<IpsRevisionDto>> ObtenerRevisionesAsync(long trabajoId)
        {
            try
            {
                using var connection = _context.CreateConnection();

                var parameters = new DynamicParameters();
                parameters.Add("@TrabajoId", trabajoId);

                // Ejecutar SP: OP_IPS_Revision_Get
                var result = await connection.QueryAsync<IpsRevisionDto>(
                    "OP_IPS_Revision_Get",
                    parameters,
                    commandType: CommandType.StoredProcedure
                );

                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error obteniendo revisiones IPS para trabajo {TrabajoId}", trabajoId);
                throw;
            }
        }

        public async Task<IpsRevisionDto?> ObtenerRevisionAsync(long revisionId)
        {
            try
            {
                using var connection = _context.CreateConnection();

                var parameters = new DynamicParameters();
                parameters.Add("@RevisionId", revisionId);

                // Query directa (SP no tiene overload para ID único, usar query)
                var query = @"
                    SELECT 
                        id AS Id,
                        TrabajoId,
                        Pregunta,
                        Observacion,
                        DescripcionObservacion,
                        RespuestaProgramador,
                        TipoTarea,
                        RegistradoPor,
                        FechaRegistro,
                        ModificadoPor,
                        FechaModificacion
                    FROM OP_IPS_Revision
                    WHERE id = @RevisionId";

                var result = await connection.QueryFirstOrDefaultAsync<IpsRevisionDto>(query, parameters);

                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error obteniendo revisión IPS {RevisionId}", revisionId);
                throw;
            }
        }

        public async Task<long> CrearRevisionAsync(IpsRevisionCreateUpdateDto dto, long usuarioId)
        {
            try
            {
                using var connection = _context.CreateConnection();

                var parameters = new DynamicParameters();
                parameters.Add("@TrabajoId", dto.TrabajoId);
                parameters.Add("@Pregunta", dto.Pregunta);
                parameters.Add("@Observacion", dto.Observacion ?? "");
                parameters.Add("@DescripcionObservacion", dto.DescripcionObservacion ?? "");
                parameters.Add("@RespuestaProgramador", dto.RespuestaProgramador ?? "");
                parameters.Add("@RegistradoPor", usuarioId);
                parameters.Add("@FechaRegistro", DateTime.Now);

                // Ejecutar SP: OP_IPS_Revision_Add
                await connection.ExecuteAsync(
                    "OP_IPS_Revision_Add",
                    parameters,
                    commandType: CommandType.StoredProcedure
                );

                // Obtener el ID insertado (asumir que SP retorna último ID)
                var query = "SELECT MAX(id) FROM OP_IPS_Revision WHERE TrabajoId = @TrabajoId AND RegistradoPor = @RegistradoPor";
                var id = await connection.ExecuteScalarAsync<long>(query, new { dto.TrabajoId, RegistradoPor = usuarioId });

                _logger.LogInformation("Revisión IPS creada. ID: {Id}, Trabajo: {TrabajoId}, Usuario: {UsuarioId}", 
                    id, dto.TrabajoId, usuarioId);

                return id;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creando revisión IPS. Trabajo: {TrabajoId}, Usuario: {UsuarioId}", 
                    dto.TrabajoId, usuarioId);
                throw;
            }
        }

        public async Task<bool> ActualizarRevisionAsync(IpsRevisionCreateUpdateDto dto, long usuarioId)
        {
            try
            {
                using var connection = _context.CreateConnection();

                var parameters = new DynamicParameters();
                parameters.Add("@Id", dto.Id);
                parameters.Add("@Pregunta", dto.Pregunta);
                parameters.Add("@Observacion", dto.Observacion ?? "");
                parameters.Add("@DescripcionObservacion", dto.DescripcionObservacion ?? "");
                parameters.Add("@RespuestaProgramador", dto.RespuestaProgramador ?? "");
                parameters.Add("@ModificadoPor", usuarioId);
                parameters.Add("@FechaModificacion", DateTime.Now);

                // Ejecutar SP: OP_IPS_Revision_Edit
                var result = await connection.ExecuteAsync(
                    "OP_IPS_Revision_Edit",
                    parameters,
                    commandType: CommandType.StoredProcedure
                );

                _logger.LogInformation("Revisión IPS actualizada. ID: {Id}, Usuario: {UsuarioId}", 
                    dto.Id, usuarioId);

                return result > 0;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error actualizando revisión IPS. ID: {Id}, Usuario: {UsuarioId}", 
                    dto.Id, usuarioId);
                throw;
            }
        }

        public async Task<bool> EliminarRevisionAsync(long revisionId, long usuarioId)
        {
            try
            {
                using var connection = _context.CreateConnection();

                var parameters = new DynamicParameters();
                parameters.Add("@Id", revisionId);
                parameters.Add("@ModificadoPor", usuarioId);

                // Ejecutar SP: OP_IPS_Revision_Del
                var result = await connection.ExecuteAsync(
                    "OP_IPS_Revision_Del",
                    parameters,
                    commandType: CommandType.StoredProcedure
                );

                _logger.LogInformation("Revisión IPS eliminada. ID: {RevisionId}, Usuario: {UsuarioId}", 
                    revisionId, usuarioId);

                return result > 0;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error eliminando revisión IPS. ID: {RevisionId}, Usuario: {UsuarioId}", 
                    revisionId, usuarioId);
                throw;
            }
        }
    }
}
