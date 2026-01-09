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
        public async Task<List<PlanillaProductividadDto>> ObtenerPlanillasPorRolAsync(int trabajoId, string rol, int usuarioId)
        {
            try
            {
                if (trabajoId <= 0)
                {
                    return new List<PlanillaProductividadDto>();
                }

                var now = DateTime.Now;
                var inicioCorteFecha = new DateTime(now.Year, now.AddMonths(-1).Month, 16);
                if (now.Month == 1)
                {
                    inicioCorteFecha = inicioCorteFecha.AddYears(-1);
                }
                var finCorteFecha = new DateTime(now.Year, now.Month, 15);

                var rolNormalizado = (rol ?? string.Empty).Trim().ToUpperInvariant();
                long? pmoId = null;
                long? coordinadorId = null;
                int? metodologiaAgrupada = null;
                bool? aprobadoCoordinador = null;
                bool? aprobadoJefe = null;
                bool? aprobadoPMO = null;

                if (rolNormalizado == "PMO")
                {
                    pmoId = usuarioId;
                    aprobadoJefe = true;
                    aprobadoPMO = false;
                }
                else if (rolNormalizado == "COORDINADOR")
                {
                    coordinadorId = usuarioId;
                    aprobadoCoordinador = false;
                }
                else if (rolNormalizado == "CAMPO")
                {
                    metodologiaAgrupada = 2;
                    aprobadoJefe = false;
                }
                else if (rolNormalizado.Contains("MYS") || rolNormalizado.Contains("CALL"))
                {
                    metodologiaAgrupada = 1;
                    aprobadoJefe = false;
                }

                var connectionString = _context.Database.GetConnectionString();
                using (var connection = new System.Data.SqlClient.SqlConnection(connectionString))
                {
                    var rows = await connection.QueryAsync<ProductividadRow>(
                        "OP_CuantiProduccionProductividad_GET",
                        new
                        {
                            FIni = inicioCorteFecha,
                            FFin = finCorteFecha,
                            Coordinador = coordinadorId,
                            PMO = pmoId,
                            TrabajoId = trabajoId,
                            MetodologiaAgrupada = metodologiaAgrupada,
                            AprobadoCoordinador = aprobadoCoordinador,
                            AprobadoJefe = aprobadoJefe,
                            AprobadoPMO = aprobadoPMO,
                            EnProduccion = (bool?)null
                        },
                        commandType: System.Data.CommandType.StoredProcedure,
                        commandTimeout: 30);

                    var planillas = rows.Select(row =>
                    {
                        var cantidadRevision = ObtenerCantidadRevision(row, rolNormalizado);
                        var observaciones = ObtenerObservacionesRevision(row, rolNormalizado);
                        var fechaRevision = ObtenerFechaRevision(row, rolNormalizado);
                        var usuarioRevision = ObtenerUsuarioRevision(row, rolNormalizado);
                        var valorUnitario = row.VrUnitario ?? 0m;
                        var cantidadBase = row.Cantidad ?? 0;
                        var montoTotal = row.VrTotal ?? (cantidadBase * valorUnitario);
                        var montoPrevio = ObtenerMontoPrevio(row, rolNormalizado, valorUnitario);

                        return new PlanillaProductividadDto
                        {
                            PlanillaId = (int)row.Id,
                            TrabajoId = (int)row.TrabajoId,
                            Concepto = row.CargoMatrix ?? row.Cargo?.ToString() ?? "Sin concepto",
                            Cantidad = cantidadBase,
                            ValorUnitario = valorUnitario,
                            MontoTotal = montoTotal,
                            MontoPrevio = montoPrevio,
                            Estado = cantidadRevision.HasValue ? 2 : 1,
                            UsuarioActualizacion = usuarioRevision ?? "Sistema",
                            FechaActualizacion = fechaRevision,
                            Observaciones = observaciones ?? string.Empty,
                            TipoActividad = row.IdMetodologia ?? 0
                        };
                    }).ToList();

                    _logger.LogInformation("Obtenidas {Count} planillas para trabajo {TrabajoId} con rol {Rol}",
                        planillas.Count, trabajoId, rol);
                    return planillas;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error obteniendo planillas para trabajo {TrabajoId}", trabajoId);
                throw;
            }
        }

        private static int? ObtenerCantidadRevision(ProductividadRow row, string rolNormalizado)
        {
            if (rolNormalizado == "PMO")
            {
                return row.CantidadPMO;
            }

            if (rolNormalizado == "COORDINADOR")
            {
                return row.CantidadCoordinador;
            }

            if (rolNormalizado == "CAMPO" || rolNormalizado.Contains("MYS") || rolNormalizado.Contains("CALL"))
            {
                return row.CantidadJefe;
            }

            return row.CantidadPMO ?? row.CantidadJefe ?? row.CantidadCoordinador;
        }

        private static string? ObtenerObservacionesRevision(ProductividadRow row, string rolNormalizado)
        {
            if (rolNormalizado == "PMO")
            {
                return row.ObservacionesPMO;
            }

            if (rolNormalizado == "COORDINADOR")
            {
                return row.ObservacionesCoordinador;
            }

            if (rolNormalizado == "CAMPO" || rolNormalizado.Contains("MYS") || rolNormalizado.Contains("CALL"))
            {
                return row.ObservacionesJefe;
            }

            return row.ObservacionesPMO ?? row.ObservacionesJefe ?? row.ObservacionesCoordinador;
        }

        private static DateTime? ObtenerFechaRevision(ProductividadRow row, string rolNormalizado)
        {
            if (rolNormalizado == "PMO")
            {
                return row.FechaRevisaPMO;
            }

            if (rolNormalizado == "COORDINADOR")
            {
                return row.FechaRevisaCoordinador;
            }

            if (rolNormalizado == "CAMPO" || rolNormalizado.Contains("MYS") || rolNormalizado.Contains("CALL"))
            {
                return row.FechaRevisaJefe;
            }

            return row.FechaRevisaPMO ?? row.FechaRevisaJefe ?? row.FechaRevisaCoordinador;
        }

        private static string? ObtenerUsuarioRevision(ProductividadRow row, string rolNormalizado)
        {
            if (rolNormalizado == "PMO")
            {
                return row.PMO;
            }

            if (rolNormalizado == "COORDINADOR")
            {
                return row.Coordinador;
            }

            if (rolNormalizado == "CAMPO" || rolNormalizado.Contains("MYS") || rolNormalizado.Contains("CALL"))
            {
                return row.Coordinador ?? row.PMO;
            }

            return row.PMO ?? row.Coordinador;
        }

        private static decimal ObtenerMontoPrevio(ProductividadRow row, string rolNormalizado, decimal valorUnitario)
        {
            if (rolNormalizado == "PMO")
            {
                return (row.CantidadJefe ?? 0) * valorUnitario;
            }

            if (rolNormalizado == "CAMPO" || rolNormalizado.Contains("MYS") || rolNormalizado.Contains("CALL"))
            {
                return (row.CantidadCoordinador ?? 0) * valorUnitario;
            }

            return 0m;
        }

        /// <inheritdoc />
        public async Task<bool> AprobarPlanillaAsync(int planillaId, decimal montoAutorizado, int usuarioId, string rol)
        {
            try
            {
                var rolNormalizado = (rol ?? string.Empty).Trim().ToUpperInvariant();
                var cantidadAutorizada = (int)Math.Round(montoAutorizado, 0, MidpointRounding.AwayFromZero);
                var fecha = DateTime.UtcNow.AddHours(-5);

                var connectionString = _context.Database.GetConnectionString();
                using (var connection = new System.Data.SqlClient.SqlConnection(connectionString))
                {
                    string sql;
                    if (rolNormalizado == "PMO")
                    {
                        sql = @"UPDATE CC_ProduccionCargaPST
                               SET CantidadPMO = @Cantidad,
                                   PMORevisa = @UsuarioId,
                                   FechaRevisaPMO = @Fecha
                               WHERE Id = @Id";
                    }
                    else if (rolNormalizado == "COORDINADOR")
                    {
                        sql = @"UPDATE CC_ProduccionCargaPST
                               SET CantidadCoordinador = @Cantidad,
                                   CoordinadorRevisa = @UsuarioId,
                                   FechaRevisaCoordinador = @Fecha
                               WHERE Id = @Id";
                    }
                    else if (rolNormalizado == "CAMPO" || rolNormalizado.Contains("MYS") || rolNormalizado.Contains("CALL"))
                    {
                        sql = @"UPDATE CC_ProduccionCargaPST
                               SET CantidadJefe = @Cantidad,
                                   JefeRevisa = @UsuarioId,
                                   FechaRevisaJefe = @Fecha
                               WHERE Id = @Id";
                    }
                    else
                    {
                        return false;
                    }

                    var result = await connection.ExecuteAsync(
                        sql,
                        new
                        {
                            Id = planillaId,
                            Cantidad = cantidadAutorizada,
                            UsuarioId = usuarioId,
                            Fecha = fecha
                        },
                        commandType: System.Data.CommandType.Text,
                        commandTimeout: 30);

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
        public async Task<bool> RechazarPlanillaAsync(int planillaId, string observacion, int usuarioId, string rol)
        {
            try
            {
                var rolNormalizado = (rol ?? string.Empty).Trim().ToUpperInvariant();
                var fecha = DateTime.UtcNow.AddHours(-5);
                var observacionFinal = observacion ?? string.Empty;

                var connectionString = _context.Database.GetConnectionString();
                using (var connection = new System.Data.SqlClient.SqlConnection(connectionString))
                {
                    string sql;
                    if (rolNormalizado == "PMO")
                    {
                        sql = @"UPDATE CC_ProduccionCargaPST
                               SET CantidadPMO = 0,
                                   PMORevisa = @UsuarioId,
                                   FechaRevisaPMO = @Fecha,
                                   ObservacionesPMO = @Observacion
                               WHERE Id = @Id";
                    }
                    else if (rolNormalizado == "COORDINADOR")
                    {
                        sql = @"UPDATE CC_ProduccionCargaPST
                               SET CantidadCoordinador = 0,
                                   CoordinadorRevisa = @UsuarioId,
                                   FechaRevisaCoordinador = @Fecha,
                                   ObservacionesCoordinador = @Observacion
                               WHERE Id = @Id";
                    }
                    else if (rolNormalizado == "CAMPO" || rolNormalizado.Contains("MYS") || rolNormalizado.Contains("CALL"))
                    {
                        sql = @"UPDATE CC_ProduccionCargaPST
                               SET CantidadJefe = 0,
                                   JefeRevisa = @UsuarioId,
                                   FechaRevisaJefe = @Fecha,
                                   ObservacionesJefe = @Observacion
                               WHERE Id = @Id";
                    }
                    else
                    {
                        return false;
                    }

                    var result = await connection.ExecuteAsync(
                        sql,
                        new
                        {
                            Id = planillaId,
                            UsuarioId = usuarioId,
                            Fecha = fecha,
                            Observacion = observacionFinal
                        },
                        commandType: System.Data.CommandType.Text,
                        commandTimeout: 30);

                    var success = result > 0;
                    if (success)
                    {
                        _logger.LogWarning("Planilla {PlanillaId} rechazada por usuario {UsuarioId}. Observacion: {Obs}",
                            planillaId, usuarioId, observacionFinal);
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
                if (montoTotal < 0)
                {
                    return (false, "El monto no puede ser negativo");
                }

                var connectionString = _context.Database.GetConnectionString();
                using (var connection = new System.Data.SqlClient.SqlConnection(connectionString))
                {
                    var maximoPresupuesto = await connection.ExecuteScalarAsync<decimal?>(
                        "SELECT SUM(ValorTotal) FROM CC_PresupuestoInterno WHERE TrabajoId = @TrabajoId AND ISNULL(Activo, 1) = 1",
                        new { TrabajoId = trabajoId });

                    if (!maximoPresupuesto.HasValue)
                    {
                        return (false, "No se encontró información del presupuesto");
                    }

                    if (montoTotal > maximoPresupuesto.Value)
                    {
                        return (false, $"El monto ({montoTotal:C}) excede el presupuesto máximo ({maximoPresupuesto.Value:C})");
                    }
                }

                return (true, "Validación exitosa");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error validando montos para trabajo {TrabajoId}", trabajoId);
                throw;
            }
        }

        private sealed class ProductividadRow
        {
            public long Id { get; init; }
            public long TrabajoId { get; init; }
            public int? Cantidad { get; init; }
            public int? Cargo { get; init; }
            public string? CargoMatrix { get; init; }
            public decimal? VrUnitario { get; init; }
            public decimal? VrTotal { get; init; }
            public int? CantidadCoordinador { get; init; }
            public int? CantidadJefe { get; init; }
            public int? CantidadPMO { get; init; }
            public DateTime? FechaRevisaCoordinador { get; init; }
            public DateTime? FechaRevisaJefe { get; init; }
            public DateTime? FechaRevisaPMO { get; init; }
            public string? ObservacionesCoordinador { get; init; }
            public string? ObservacionesJefe { get; init; }
            public string? ObservacionesPMO { get; init; }
            public string? PMO { get; init; }
            public string? Coordinador { get; init; }
            public int? IdMetodologia { get; init; }
        }
    }
}
