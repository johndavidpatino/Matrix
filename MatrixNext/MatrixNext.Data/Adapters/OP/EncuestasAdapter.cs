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
    /// Adapter para acceso a datos de encuestas (activación/anulación)
    /// </summary>
    public class EncuestasAdapter : IEncuestasAdapter
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<EncuestasAdapter> _logger;

        public EncuestasAdapter(ApplicationDbContext context, ILogger<EncuestasAdapter> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<IEnumerable<EncuestaAnuladaDto>> ObtenerEncuestasAnuladasAsync(long trabajoId)
        {
            try
            {
                using var connection = _context.CreateConnection();
                
                // Consulta directa a tabla OP_EncuestasAnuladas
                // CORREGIDO: PY_Trabajos → PY_Trabajo, GN_Unidades → US_Unidades
                var query = @"
                    SELECT 
                        ea.id AS Id,
                        ea.TrabajoId,
                        ea.NumeroEncuesta,
                        ea.Observacion,
                        ea.Fecha,
                        ea.UsuarioId,
                        ea.UnidadId,
                        t.NombreTrabajo AS NombreTrabajo,
                        u.NombreUsuario AS NombreUsuario,
                        un.Nombre AS NombreUnidad
                    FROM OP_EncuestasAnuladas ea
                    LEFT JOIN PY_Trabajo t ON ea.TrabajoId = t.id
                    LEFT JOIN US_Usuarios u ON ea.UsuarioId = u.Id
                    LEFT JOIN US_Unidades un ON ea.UnidadId = un.id
                    WHERE ea.TrabajoId = @TrabajoId
                    ORDER BY ea.Fecha DESC";

                return await connection.QueryAsync<EncuestaAnuladaDto>(query, new { TrabajoId = trabajoId });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error obteniendo encuestas anuladas para trabajo {TrabajoId}", trabajoId);
                throw;
            }
        }

        public async Task<bool> ExisteEncuestaAnuladaAsync(long trabajoId, long numeroEncuesta)
        {
            try
            {
                using var connection = _context.CreateConnection();
                
                var parameters = new DynamicParameters();
                parameters.Add("@TrabajoId", trabajoId);
                parameters.Add("@NoEncuesta", numeroEncuesta);

                // Ejecutar SP exactamente como en WebMatrix/CoreProject
                var result = await connection.QueryFirstOrDefaultAsync<short?>(
                    "OP_ExisteEncuestaAnulada",
                    parameters,
                    commandType: CommandType.StoredProcedure
                );

                return result.HasValue && result.Value > 0;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error verificando existencia de encuesta anulada. Trabajo: {TrabajoId}, Encuesta: {NumeroEncuesta}", 
                    trabajoId, numeroEncuesta);
                throw;
            }
        }

        public async Task<bool> ExisteEncuestaAnuladaGestionCampoAsync(long trabajoId, long numeroEncuesta)
        {
            try
            {
                using var connection = _context.CreateConnection();
                
                var parameters = new DynamicParameters();
                parameters.Add("@TrabajoId", trabajoId);
                parameters.Add("@NoEncuesta", numeroEncuesta);

                // Ejecutar SP de gestión de campo
                var result = await connection.QueryFirstOrDefaultAsync<short?>(
                    "OP_GestionCampo_ExisteEncuesta",
                    parameters,
                    commandType: CommandType.StoredProcedure
                );

                return result.HasValue && result.Value > 0;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error verificando existencia de encuesta en gestión de campo. Trabajo: {TrabajoId}, Encuesta: {NumeroEncuesta}", 
                    trabajoId, numeroEncuesta);
                throw;
            }
        }

        public async Task<long> AnularEncuestaAsync(EncuestaAnuladaDto dto)
        {
            try
            {
                using var connection = _context.CreateConnection();
                
                // Insertar en tabla OP_EncuestasAnuladas (EF-style como en CoreProject)
                var query = @"
                    INSERT INTO OP_EncuestasAnuladas (TrabajoId, NumeroEncuesta, Observacion, Fecha, UsuarioId, UnidadId)
                    VALUES (@TrabajoId, @NumeroEncuesta, @Observacion, @Fecha, @UsuarioId, @UnidadId);
                    SELECT CAST(SCOPE_IDENTITY() AS BIGINT);";

                var id = await connection.ExecuteScalarAsync<long>(query, dto);
                
                _logger.LogInformation("Encuesta anulada. Id: {Id}, Trabajo: {TrabajoId}, Encuesta: {NumeroEncuesta}", 
                    id, dto.TrabajoId, dto.NumeroEncuesta);
                
                return id;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error anulando encuesta. Trabajo: {TrabajoId}, Encuesta: {NumeroEncuesta}", 
                    dto.TrabajoId, dto.NumeroEncuesta);
                throw;
            }
        }

        public async Task AnularEncuestaGestionCampoAsync(long trabajoId, long numeroEncuesta, string observacion)
        {
            try
            {
                using var connection = _context.CreateConnection();
                
                var parameters = new DynamicParameters();
                parameters.Add("@TrabajoId", trabajoId);
                parameters.Add("@NoEncuesta", numeroEncuesta);
                parameters.Add("@Observacion", observacion);

                // Ejecutar SP de gestión de campo
                await connection.ExecuteAsync(
                    "OP_GestionCampo_AnularEncuesta",
                    parameters,
                    commandType: CommandType.StoredProcedure
                );

                _logger.LogInformation("Encuesta anulada en gestión de campo. Trabajo: {TrabajoId}, Encuesta: {NumeroEncuesta}", 
                    trabajoId, numeroEncuesta);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error anulando encuesta en gestión de campo. Trabajo: {TrabajoId}, Encuesta: {NumeroEncuesta}", 
                    trabajoId, numeroEncuesta);
                throw;
            }
        }

        public async Task ActivarEncuestaAsync(long numeroEncuesta, long trabajoId)
        {
            try
            {
                using var connection = _context.CreateConnection();
                
                var parameters = new DynamicParameters();
                parameters.Add("@numeroEncuesta", numeroEncuesta);
                parameters.Add("@IdTrabajo", trabajoId);

                // Ejecutar SP exactamente como en CoreProject
                await connection.ExecuteAsync(
                    "OP_ActivarEncuesta_Del",
                    parameters,
                    commandType: CommandType.StoredProcedure
                );

                _logger.LogInformation("Encuesta activada (anulación eliminada). Trabajo: {TrabajoId}, Encuesta: {NumeroEncuesta}", 
                    trabajoId, numeroEncuesta);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error activando encuesta. Trabajo: {TrabajoId}, Encuesta: {NumeroEncuesta}", 
                    trabajoId, numeroEncuesta);
                throw;
            }
        }

        public async Task ActualizarGestionCampoActivacionAsync(long trabajoId, long numeroEncuesta, string observacion, long usuarioId)
        {
            try
            {
                using var connection = _context.CreateConnection();
                
                var parameters = new DynamicParameters();
                parameters.Add("@trabajoId", trabajoId);
                parameters.Add("@numeroEncuesta", numeroEncuesta);
                parameters.Add("@observacion", observacion);
                parameters.Add("@idUsuario", usuarioId);

                // Ejecutar SP de gestión de campo para activación
                await connection.ExecuteAsync(
                    "OP_GestionCampo_ActivarEncuesta",
                    parameters,
                    commandType: CommandType.StoredProcedure
                );

                _logger.LogInformation("Gestión de campo actualizada para activación. Trabajo: {TrabajoId}, Encuesta: {NumeroEncuesta}", 
                    trabajoId, numeroEncuesta);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error actualizando gestión de campo para activación. Trabajo: {TrabajoId}, Encuesta: {NumeroEncuesta}", 
                    trabajoId, numeroEncuesta);
                throw;
            }
        }
    }
}
