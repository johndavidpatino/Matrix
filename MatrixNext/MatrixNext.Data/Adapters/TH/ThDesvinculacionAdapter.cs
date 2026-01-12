using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Dapper;
using MatrixNext.Data.Adapters.TH.Models;
using MatrixNext.Data.Context;
using Microsoft.Extensions.Logging;

namespace MatrixNext.Data.Adapters.TH
{
    /// <summary>
    /// Adaptador para gestión de Desvinculaciones de Empleados
    /// Maneja el flujo de desvinculación con evaluaciones de RRHH y Áreas
    /// </summary>
    public class ThDesvinculacionAdapter : IThDesvinculacionAdapter
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<ThDesvinculacionAdapter> _logger;

        public ThDesvinculacionAdapter(ApplicationDbContext context, ILogger<ThDesvinculacionAdapter> logger)
        {
            _context = context;
            _logger = logger;
        }

        /// <summary>
        /// Obtiene lista de desvinculaciones con paginación
        /// </summary>
        public async Task<List<DesvinculacionDto>> ObtenerDesvinculaciones(int pageSize, int pageIndex, string textoBuscado)
        {
            try
            {
                var desvinculaciones = new List<DesvinculacionDto>();

                var resultado = await _context.Database.GetDbConnection().QueryAsync<dynamic>(
                    "TH_Desvinculacion_Get",
                    new
                    {
                        pageSize = pageSize,
                        pageIndex = pageIndex,
                        textoBuscado = textoBuscado
                    },
                    commandType: System.Data.CommandType.StoredProcedure
                );

                foreach (var row in resultado)
                {
                    desvinculaciones.Add(new DesvinculacionDto
                    {
                        Id = row.Id,
                        EmpleadoId = row.EmpleadoId,
                        FechaRetiro = row.FechaRetiro,
                        MotivosDesvinculacion = row.MotivosDesvinculacion,
                        Estado = row.Estado
                    });
                }

                _logger.LogInformation($"Obtenidas {desvinculaciones.Count} desvinculaciones");
                return desvinculaciones;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener desvinculaciones");
                throw;
            }
        }

        /// <summary>
        /// Inicia proceso de desvinculación para un empleado
        /// </summary>
        public async Task<long> IniciarProcesoDesvinculacion(DesvinculacionInputDto input)
        {
            try
            {
                var newId = await _context.Database.GetDbConnection().QueryFirstOrDefaultAsync<long?>(
                    "TH_Desvinculacion_Iniciar",
                    new
                    {
                        empleadoId = input.EmpleadoId,
                        fechaRetiro = input.FechaRetiro,
                        motivosDesvinculacion = input.MotivosDesvinculacion
                    },
                    commandType: System.Data.CommandType.StoredProcedure
                );

                _logger.LogInformation($"Proceso de desvinculación iniciado con ID {newId}");
                return newId ?? 0;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al iniciar proceso de desvinculación");
                throw;
            }
        }

        /// <summary>
        /// Obtiene evaluaciones pendientes para una desvinculación
        /// </summary>
        public async Task<List<dynamic>> ObtenerEvaluacionesDesvinculacion(long desvinculacionId)
        {
            try
            {
                var evaluaciones = await _context.Database.GetDbConnection().QueryAsync<dynamic>(
                    "TH_Desvinculacion_Evaluaciones_Get",
                    new { desvinculacionId = desvinculacionId },
                    commandType: System.Data.CommandType.StoredProcedure
                );

                return evaluaciones.ToList();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error al obtener evaluaciones de desvinculación {desvinculacionId}");
                throw;
            }
        }

        /// <summary>
        /// Guarda evaluación de desvinculación (RRHH o Área)
        /// </summary>
        public async Task<bool> GuardarEvaluacionDesvinculacion(DesvinculacionEvaluacionInputDto input, string usuario)
        {
            try
            {
                var resultado = await _context.Database.GetDbConnection().ExecuteAsync(
                    "TH_Desvinculacion_Evaluacion_Save",
                    new
                    {
                        desvinculacionEmpleadoId = input.DesvinculacionEmpleadoId,
                        observaciones = input.Observaciones,
                        aprobado = input.Aprobado,
                        usuario = usuario
                    },
                    commandType: System.Data.CommandType.StoredProcedure
                );

                _logger.LogInformation($"Evaluación guardada para desvinculación {input.DesvinculacionEmpleadoId}");
                return resultado > 0;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error al guardar evaluación de desvinculación");
                throw;
            }
        }

        /// <summary>
        /// Finaliza el proceso de desvinculación
        /// </summary>
        public async Task<bool> FinalizarProcesoDesvinculacion(long desvinculacionId)
        {
            try
            {
                var resultado = await _context.Database.GetDbConnection().ExecuteAsync(
                    "TH_Desvinculacion_Finalizar",
                    new { desvinculacionId = desvinculacionId },
                    commandType: System.Data.CommandType.StoredProcedure
                );

                _logger.LogInformation($"Proceso de desvinculación {desvinculacionId} finalizado");
                return resultado > 0;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error al finalizar desvinculación {desvinculacionId}");
                throw;
            }
        }

        /// <summary>
        /// Genera PDF del acta de desvinculación
        /// </summary>
        public async Task<string> GenerarPDFDesvinculacion(long desvinculacionId)
        {
            try
            {
                var pdfBase64 = await _context.Database.GetDbConnection().QueryFirstOrDefaultAsync<string>(
                    "TH_Desvinculacion_GenerarPDF",
                    new { desvinculacionId = desvinculacionId },
                    commandType: System.Data.CommandType.StoredProcedure
                );

                _logger.LogInformation($"PDF generado para desvinculación {desvinculacionId}");
                return pdfBase64;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error al generar PDF de desvinculación {desvinculacionId}");
                throw;
            }
        }
    }
}
