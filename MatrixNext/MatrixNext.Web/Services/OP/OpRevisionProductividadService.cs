using MatrixNext.Web.Infrastructure.Data;
using MatrixNext.Web.Models.OP.Dtos;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Dapper;

namespace MatrixNext.Web.Services.OP
{
    /// <summary>
    /// Servicio para la revisión de productividad multirrol en OP.
    /// Implementa lógica centralizada para revisión por PMO, Coordinador, Campo y MyS/Call.
    /// </summary>
    public class OpRevisionProductividadService : IOpRevisionProductividadService
    {
        private readonly MatrixDbContext _context;
        private readonly ILogger<OpRevisionProductividadService> _logger;

        public OpRevisionProductividadService(
            MatrixDbContext context,
            ILogger<OpRevisionProductividadService> logger)
        {
            _context = context;
            _logger = logger;
        }

        /// <inheritdoc />
        public async Task<List<PlanillaProductividadDto>> ObtenerPlanillasPorRolAsync(int trabajoId, string rol)
        {
            try
            {
                var planillas = new List<PlanillaProductividadDto>();
                
                var connectionString = _context.Database.GetConnectionString();
                using (var connection = new System.Data.SqlClient.SqlConnection(connectionString))
                {
                    // Llamar SP OP_CuantiDapper_Get con filtro por rol
                    var parameters = new DynamicParameters();
                    parameters.Add("@IdTrabajo", trabajoId);
                    parameters.Add("@Rol", rol);
                    parameters.Add("@Estado", 1); // Solo planillas pendientes (1=Pendiente)

                    // Ejecutar SP y mapear a PlanillaProductividadDto
                    var result = await connection.QueryAsync<dynamic>(
                        "OP_CuantiDapper_Get",
                        parameters,
                        commandType: System.Data.CommandType.StoredProcedure,
                        commandTimeout: 30
                    );

                    // Mapear resultados dinámicos a DTO
                    planillas = result.Select(r => new PlanillaProductividadDto
                    {
                        PlanillaId = r.IdPlanilla,
                        TrabajoId = r.IdTrabajo,
                        Concepto = r.Concepto ?? "Sin concepto",
                        Cantidad = r.Cantidad ?? 0,
                        ValorUnitario = r.ValorUnitario ?? 0m,
                        MontoTotal = (r.Cantidad ?? 0) * (r.ValorUnitario ?? 0m),
                        MontoPrevio = r.MontoPrevio ?? 0m,
                        Estado = r.Estado ?? 1,
                        UsuarioActualizacion = r.UsuarioActualizacion ?? "Sistema",
                        FechaActualizacion = r.FechaActualizacion,
                        Observaciones = r.Observaciones,
                        TipoActividad = r.TipoActividad ?? 0
                    }).ToList();
                }

                _logger.LogInformation("Obtenidas {Count} planillas para trabajo {TrabajoId} con rol {Rol}", 
                    planillas.Count, trabajoId, rol);
                return planillas;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error obteniendo planillas para trabajo {TrabajoId}", trabajoId);
                throw;
            }
        }

        /// <inheritdoc />
        public async Task<bool> AprobarPlanillaAsync(int planillaId, decimal montoAutorizado, int usuarioId)
        {
            try
            {
                var connectionString = _context.Database.GetConnectionString();
                using (var connection = new System.Data.SqlClient.SqlConnection(connectionString))
                {
                    var parameters = new DynamicParameters();
                    parameters.Add("@IdPlanilla", planillaId);
                    parameters.Add("@MontoAutorizado", montoAutorizado);
                    parameters.Add("@IdUsuario", usuarioId);
                    parameters.Add("@FechaAprobacion", DateTime.Now);

                    // Ejecutar SP OP_PlanillaProductividad_Aprobar
                    var result = await connection.ExecuteAsync(
                        "OP_PlanillaProductividad_Aprobar",
                        parameters,
                        commandType: System.Data.CommandType.StoredProcedure,
                        commandTimeout: 30
                    );

                    var success = result > 0;
                    if (success)
                    {
                        _logger.LogInformation("Planilla {PlanillaId} aprobada por usuario {UsuarioId} con monto {Monto}", 
                            planillaId, usuarioId, montoAutorizado);
                    }
                    return success;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error aprobando planilla {PlanillaId}", planillaId);
                throw;
            }
        }

        /// <inheritdoc />
        public async Task<bool> RechazarPlanillaAsync(int planillaId, string observacion, int usuarioId)
        {
            try
            {
                var connectionString = _context.Database.GetConnectionString();
                using (var connection = new System.Data.SqlClient.SqlConnection(connectionString))
                {
                    var parameters = new DynamicParameters();
                    parameters.Add("@IdPlanilla", planillaId);
                    parameters.Add("@Observacion", observacion ?? "");
                    parameters.Add("@IdUsuario", usuarioId);
                    parameters.Add("@FechaRechazo", DateTime.Now);

                    // Ejecutar SP OP_PlanillaProductividad_Rechazar
                    var result = await connection.ExecuteAsync(
                        "OP_PlanillaProductividad_Rechazar",
                        parameters,
                        commandType: System.Data.CommandType.StoredProcedure,
                        commandTimeout: 30
                    );

                    var success = result > 0;
                    if (success)
                    {
                        _logger.LogWarning("Planilla {PlanillaId} rechazada por usuario {UsuarioId}. Observación: {Obs}", 
                            planillaId, usuarioId, observacion);
                    }
                    return success;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error rechazando planilla {PlanillaId}", planillaId);
                throw;
            }
        }

        /// <inheritdoc />
        public async Task<(bool Valid, string Message)> ValidarMontosPlanillaAsync(int trabajoId, decimal montoTotal)
        {
            try
            {
                // Obtener máximo autorizado del trabajo
                var trabajo = await _context.Set<dynamic>()
                    .FromSqlRaw("SELECT CCProduccionPST as MaximoPresupuesto FROM TrabajoOPCuanti WHERE IdTrabajo = {0}", trabajoId)
                    .AsNoTracking()
                    .FirstOrDefaultAsync();

                if (trabajo == null)
                {
                    return (false, "No se encontró información del trabajo");
                }

                decimal maximoPresupuesto = trabajo.MaximoPresupuesto ?? 0m;

                if (montoTotal < 0)
                {
                    return (false, "El monto no puede ser negativo");
                }

                if (montoTotal > maximoPresupuesto)
                {
                    return (false, $"El monto ({montoTotal:C}) excede el presupuesto máximo ({maximoPresupuesto:C})");
                }

                return (true, "Validación exitosa");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error validando montos para trabajo {TrabajoId}", trabajoId);
                throw;
            }
        }
    }
}
