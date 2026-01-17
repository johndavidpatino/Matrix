using MatrixNext.Data.Models.RP;
using MatrixNext.Data.Services;
using MatrixNext.Data.Adapters.RP;
using MatrixNext.Data.Services.Authorization;
using Microsoft.Extensions.Logging;
using ClosedXML.Excel;
using System.IO;

namespace MatrixNext.Data.Services.RP
{
    /// <summary>
    /// Implementación de Service para Reportes
    /// Orquesta Adapters + validaciones + transformaciones
    /// REGLA 6: Validaciones complejas ✓
    /// REGLA 7: Transformación de datos ✓
    /// REGLA 8: Gestión de errores ✓
    /// </summary>
    public class ReportesService : IReportesService
    {
        private readonly IReportesAdapter _adapter;
        private readonly IAuthorizationService _authService;
        private readonly ILogger<ReportesService> _logger;

        public ReportesService(
            IReportesAdapter adapter, 
            IAuthorizationService authService,
            ILogger<ReportesService> logger)
        {
            _adapter = adapter;
            _authService = authService;
            _logger = logger;
        }

        // ============================================
        // GENERACIÓN DE REPORTES
        // ============================================

        public async Task<ApiResponse<ReporteResultadoDTO>> GenerarReporteAsync(
            int reporteId,
            ReporteFiltrosDTO filtros,
            CancellationToken cancellationToken = default)
        {
            try
            {
                _logger.LogInformation($"[ReportesService] Generando reporte {reporteId}");

                // REGLA 6: Validación
                _adapter.ValidarParametros(filtros);
                if (!await ValidarAccesoReporteAsync(reporteId, filtros.UsuarioId ?? 0))
                    return ApiResponse<ReporteResultadoDTO>.Unauthorized("Acceso denegado al reporte");

                // Obtener datos según tipo de reporte
                var datos = await ObtenerDatosReporteAsync(reporteId, filtros);

                // REGLA 7: Transformación
                var resultado = AplicarPaginacion(datos, filtros.PageNumber, filtros.PageSize);

                // Auditoría no-op (fuera de scope Sprint 10)
                // await RegistrarAuditoriaAsync(reporteId, filtros.UsuarioId ?? 0, "GENERACIÓN", $"Registros: {resultado.TotalRegistros}");

                return ApiResponse<ReporteResultadoDTO>.Ok(resultado, "Reporte generado correctamente");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"[ReportesService] Error generando reporte {reporteId}");
                // REGLA 8: Gestión errores
                return ApiResponse<ReporteResultadoDTO>.Error("Error al generar el reporte. Por favor intente nuevamente.");
            }
        }

        public async Task<ApiResponse<ReporteDTO>> ObtenerReporteAsync(int reporteId, int usuarioId)
        {
            try
            {
                _logger.LogInformation($"[ReportesService] Obteniendo reporte {reporteId}");

                var reportes = await _adapter.GetReportesDisponiblesAsync();
                var reporte = reportes.FirstOrDefault(r => r.ReporteId == reporteId);

                if (reporte == null)
                    return ApiResponse<ReporteDTO>.NotFound("Reporte no encontrado");

                if (!await ValidarAccesoReporteAsync(reporteId, usuarioId))
                    return ApiResponse<ReporteDTO>.Unauthorized("Acceso denegado");

                return ApiResponse<ReporteDTO>.Ok(reporte);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"[ReportesService] Error obteniendo reporte {reporteId}");
                return ApiResponse<ReporteDTO>.Error("Error al obtener el reporte. Por favor intente nuevamente.");
            }
        }

        public async Task<ApiResponse<List<ReporteDTO>>> ObtenerReportesDisponiblesAsync()
        {
            try
            {
                _logger.LogInformation("[ReportesService] Obteniendo reportes disponibles");

                var reportes = await _adapter.GetReportesDisponiblesAsync();

                return ApiResponse<List<ReporteDTO>>.Ok(
                    reportes.Where(r => r.Disponible).ToList(),
                    $"{reportes.Count} reportes disponibles");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "[ReportesService] Error obteniendo reportes disponibles");
                return ApiResponse<List<ReporteDTO>>.Error("Error al obtener los reportes disponibles. Por favor intente nuevamente.");
            }
        }

        public async Task<bool> ValidarAccesoReporteAsync(int reporteId, int usuarioId, string tipoAcceso = "Lectura")
        {
            try
            {
                // Validar permiso usando el servicio de autorización
                var accion = tipoAcceso == "Lectura" ? "Ver" : tipoAcceso;
                return await _authService.ValidarPermisoAsync(usuarioId, "Reporte", accion, reporteId);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "[ReportesService] Error validando acceso");
                return false;
            }
        }

        // ============================================
        // FILTROS Y BÚSQUEDA
        // ============================================

        public async Task<ApiResponse<ReporteResultadoDTO>> AplicarFiltrosAvanzadosAsync(
            ReporteFiltrosDTO filtros,
            List<Dictionary<string, object>> datos)
        {
            try
            {
                _logger.LogInformation("[ReportesService] Aplicando filtros avanzados");

                // Validar parámetros
                _adapter.ValidarParametros(filtros);

                // REGLA 7: Transformación
                var datosFiltrados = datos;

                // Filtrar por usuario si se especifica
                if (!string.IsNullOrEmpty(filtros.NombreUsuario))
                {
                    datosFiltrados = datosFiltrados
                        .Where(d => d.ContainsKey("Usuario") && 
                               ((string)d["Usuario"]).Contains(filtros.NombreUsuario, StringComparison.OrdinalIgnoreCase))
                        .ToList();
                }

                // Filtrar por estado
                if (!string.IsNullOrEmpty(filtros.Estado))
                {
                    datosFiltrados = datosFiltrados
                        .Where(d => d.ContainsKey("Estado") && 
                               ((string)d["Estado"]).Equals(filtros.Estado, StringComparison.OrdinalIgnoreCase))
                        .ToList();
                }

                // Aplicar paginación
                var resultado = AplicarPaginacion(datosFiltrados, filtros.PageNumber, filtros.PageSize);

                return ApiResponse<ReporteResultadoDTO>.Ok(resultado, "Filtros aplicados");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "[ReportesService] Error aplicando filtros");
                return ApiResponse<ReporteResultadoDTO>.Error("Error al aplicar los filtros. Por favor intente nuevamente.");
            }
        }

        public ReporteResultadoDTO AplicarPaginacion(
            List<Dictionary<string, object>> datos,
            int pageNumber = 1,
            int pageSize = 50)
        {
            var totalRegistros = datos.Count;
            var totalPaginas = (int)Math.Ceiling((decimal)totalRegistros / pageSize);

            var datosPaginados = datos
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToList();

            return new ReporteResultadoDTO
            {
                Datos = datosPaginados,
                TotalRegistros = totalRegistros,
                Pagina = pageNumber,
                RegistrosPorPagina = pageSize
            };
        }

        // ============================================
        // EXPORTACIÓN DE DATOS
        // ============================================

        public async Task<ReporteExportDTO> PrepararExportExcelAsync(
            int reporteId,
            ReporteFiltrosDTO filtros,
            int usuarioId)
        {
            try
            {
                _logger.LogInformation($"[ReportesService] Preparando export Excel reporte {reporteId}");

                // Obtener datos
                var datos = await ObtenerDatosReporteAsync(reporteId, filtros);

                // REGLA 7: Transformación para Excel
                var contenido = ConvertirAExcelBytes(datos);

                var export = new ReporteExportDTO
                {
                    Nombre = $"Reporte_{reporteId}_{DateTime.Now:yyyyMMdd_HHmmss}.xlsx",
                    FechaGeneracion = DateTime.Now,
                    Usuario = $"Usuario_{usuarioId}",
                    Contenido = contenido,
                    ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet"
                };

                // Auditoría no-op (fuera de scope Sprint 10)
                // await RegistrarAuditoriaAsync(reporteId, usuarioId, "EXPORT_EXCEL", export.Nombre);

                return export;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "[ReportesService] Error preparando export Excel");
                throw;
            }
        }

        /// <summary>
        /// Exportación a PDF no disponible en esta versión
        /// REGLA 6: Sprint 10 solo replica paridad WebMatrix (Excel export solamente)
        /// </summary>
        [Obsolete("PDF export no implementado - fuera de scope Sprint 10", true)]
        public async Task<ReporteExportDTO> PrepararExportPdfAsync(
            int reporteId,
            ReporteFiltrosDTO filtros,
            int usuarioId)
        {
            throw new NotImplementedException("PDF export no está disponible. Use PrepararExportExcelAsync.");
        }

        // ============================================
        // INDICADORES Y DASHBOARDS
        // ============================================

        public async Task<ApiResponse<Dictionary<string, object>>> ObtenerIndicadoresCalidadAsync(
            DateTime fechaDesde,
            DateTime fechaHasta,
            int usuarioId)
        {
            try
            {
                _logger.LogInformation("[ReportesService] Obteniendo indicadores de calidad");

                var datos = await _adapter.GetIndicadoresCalidadAsync(fechaDesde, fechaHasta, usuarioId);

                if (!datos.Any())
                    return ApiResponse<Dictionary<string, object>>.Ok(new Dictionary<string, object>(), 
                        "Sin datos para el período");

                // Transformar a resumen
                var resumen = TransformarIndicadores(datos);

                return ApiResponse<Dictionary<string, object>>.Ok(resumen, "Indicadores obtenidos");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "[ReportesService] Error obteniendo indicadores calidad");
                return ApiResponse<Dictionary<string, object>>.Error("Error al obtener indicadores de calidad. Por favor intente nuevamente.");
            }
        }

        public async Task<ApiResponse<Dictionary<string, object>>> ObtenerIndicadoresCumplimientoAsync(
            DateTime fechaDesde,
            DateTime fechaHasta,
            int usuarioId)
        {
            try
            {
                _logger.LogInformation("[ReportesService] Obteniendo indicadores de cumplimiento");

                var datos = await _adapter.GetIndicadoresCumplimientoAsync(fechaDesde, fechaHasta, usuarioId);

                if (!datos.Any())
                    return ApiResponse<Dictionary<string, object>>.Ok(new Dictionary<string, object>(),
                        "Sin datos para el período");

                // Transformar a resumen
                var resumen = TransformarIndicadores(datos);

                return ApiResponse<Dictionary<string, object>>.Ok(resumen, "Indicadores obtenidos");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "[ReportesService] Error obteniendo indicadores cumplimiento");
                return ApiResponse<Dictionary<string, object>>.Error("Error al obtener indicadores de cumplimiento. Por favor intente nuevamente.");
            }
        }

        // ============================================
        // AUDITORÍA Y LOGGING
        // ============================================

        /// <summary>
        /// Auditoría de reportes no implementada en esta versión
        /// REGLA 6: No existe en WebMatrix legacy - auditoría no se graba para reportes
        /// </summary>
        public async Task RegistrarAuditoriaAsync(int reporteId, int usuarioId, string accion, string? detalles = null)
        {
            // Placeholder: Auditoría genérica podría agregarse en futuro
            _logger.LogInformation(
                $"[Auditoría] ReporteId={reporteId}, Usuario={usuarioId}, Acción={accion}, Detalles={detalles}");

            await Task.CompletedTask;
        }

        // ============================================
        // HELPERS PRIVADOS
        // ============================================

        private async Task<List<Dictionary<string, object>>> ObtenerDatosReporteAsync(
            int reporteId,
            ReporteFiltrosDTO filtros)
        {
            var fechaDesde = filtros.FechaDesde ?? DateTime.Now.AddMonths(-1);
            var fechaHasta = filtros.FechaHasta ?? DateTime.Now;
            var usuarioId = filtros.UsuarioId ?? 0;
            int? proyectoId = int.TryParse(filtros.Proyecto, out var proyectoParsed) ? proyectoParsed : null;

            return reporteId switch
            {
                // Indicadores
                1 => await _adapter.GetIndicadoresCalidadAsync(fechaDesde, fechaHasta, usuarioId),
                2 => await _adapter.GetIndicadoresCumplimientoAsync(fechaDesde, fechaHasta, usuarioId),

                // Operación
                10 => await _adapter.GetReporteActividadesAsync(fechaDesde, fechaHasta, usuarioId),
                11 => await _adapter.GetReporteInconsistenciasAsync(fechaDesde, fechaHasta),
                12 => await _adapter.GetReporteListadoTrabajosAsync(fechaDesde, fechaHasta, proyectoId),

                // Planeación
                20 => await _adapter.GetPlaneacionCampoAsync(fechaDesde, fechaHasta),
                21 => await _adapter.GetPlaneacionEstudiosAsync(fechaDesde, fechaHasta),

                // Recursos
                30 => await _adapter.GetListadoEncuestadoresAsync(),
                31 => await _adapter.GetPersonalSinProduccionAsync(DateTime.Now),

                _ => throw new InvalidOperationException($"Reporte {reporteId} no reconocido")
            };
        }

        private byte[] ConvertirAExcelBytes(List<Dictionary<string, object>> datos)
        {
            try
            {
                _logger.LogInformation("[ReportesService] Convirtiendo datos a Excel con ClosedXML");

                using (var workbook = new XLWorkbook())
                {
                    var worksheet = workbook.Worksheets.Add("Reporte");

                    if (datos.Any())
                    {
                        // Encabezados
                        var columnas = datos.First().Keys.ToList();
                        for (int i = 0; i < columnas.Count; i++)
                        {
                            worksheet.Cell(1, i + 1).Value = columnas[i];
                            worksheet.Cell(1, i + 1).Style.Font.Bold = true;
                            worksheet.Cell(1, i + 1).Style.Fill.BackgroundColor = XLColor.LightGray;
                        }

                        // Datos
                        for (int fila = 0; fila < datos.Count; fila++)
                        {
                            var registro = datos[fila];
                            for (int col = 0; col < columnas.Count; col++)
                            {
                                var valor = registro[columnas[col]];
                                worksheet.Cell(fila + 2, col + 1).Value = valor?.ToString() ?? "";
                            }
                        }

                        // Auto-ajustar columnas
                        worksheet.Columns().AdjustToContents();
                    }

                    using (var ms = new MemoryStream())
                    {
                        workbook.SaveAs(ms);
                        return ms.ToArray();
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "[ReportesService] Error convirtiendo a Excel");
                throw;
            }
        }

        /// <summary>
        /// PDF conversion no disponible - fuera de scope Sprint 10
        /// REGLA 6: Solo paridad WebMatrix (Excel export)
        /// </summary>
        [Obsolete("PDF conversion no implementada - usar Excel export", true)]
        private byte[] ConvertirAPdfBytes(List<Dictionary<string, object>> datos)
        {
            throw new NotImplementedException("PDF conversion no disponible.");
        }

        private Dictionary<string, object> TransformarIndicadores(List<Dictionary<string, object>> datos)
        {
            // REGLA 7: Transformación de indicadores a resumen
            var resumen = new Dictionary<string, object>
            {
                { "TotalRegistros", datos.Count },
                { "FechaGeneracion", DateTime.Now },
                { "Datos", datos }
            };

            return resumen;
        }
    }
}
