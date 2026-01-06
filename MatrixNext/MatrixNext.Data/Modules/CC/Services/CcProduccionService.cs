using ClosedXML.Excel;
using MatrixNext.Data.Modules.CC.Adapters;
using MatrixNext.Data.Modules.CC.DTOs.Produccion;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Globalization;
using System.Threading.Tasks;

namespace MatrixNext.Data.Modules.CC.Services
{
    /// <summary>
    /// Interface para servicios de Producción
    /// </summary>
    public interface ICcProduccionService
    {
        // Producción
        Task<IEnumerable<RegistroProduccionDto>> ObtenerRegistrosProduccionAsync(
            FiltrosRegistroProduccionDto filtros);
        Task<byte[]> ExportarRegistrosProduccionExcelAsync(
            IEnumerable<RegistroProduccionDto> datos);

        // Liquidaciones
        Task<IEnumerable<LiquidacionPlanillaDto>> ObtenerLiquidacionesAsync(
            FiltrosLiquidacionPlanillaDto filtros);
        Task<byte[]> ExportarLiquidacionesExcelAsync(
            IEnumerable<LiquidacionPlanillaDto> datos);

        // Bonificaciones
        Task<IEnumerable<GenerarBonificacionDto>> ObtenerBonificacionesAsync(
            FiltrosGenerarBonificacionDto filtros);
        Task<byte[]> ExportarBonificacionesExcelAsync(
            IEnumerable<GenerarBonificacionDto> datos);

        // Descuentos SS
        Task<IEnumerable<CargueDescuentoSSDto>> ObtenerDescuentosSsAsync(
            FiltrosCargueDescuentoSSDto filtros);
        Task<byte[]> ExportarDescuentosSsExcelAsync(
            IEnumerable<CargueDescuentoSSDto> datos);

        // PST
        Task<IEnumerable<LiquidacionProductividadPstDto>> ObtenerLiquidacionesPstAsync(
            FiltrosLiquidacionProductividadPstDto filtros);
        Task<byte[]> ExportarLiquidacionesPstExcelAsync(
            IEnumerable<LiquidacionProductividadPstDto> datos);

        // Costos PST
        Task<IEnumerable<AsignacionCostosPstDto>> ObtenerAsignacionesCostosAsync(
            FiltrosAsignacionCostosPstDto filtros);
        Task<byte[]> ExportarAsignacionesCostosExcelAsync(
            IEnumerable<AsignacionCostosPstDto> datos);

        // JobBooks
        Task<IEnumerable<EstadoJobBookDto>> ObtenerEstadoJobBooksAsync(
            FiltrosEstadoJobBookDto filtros);
        Task<byte[]> ExportarEstadoJobBooksExcelAsync(
            IEnumerable<EstadoJobBookDto> datos);

        // Revisión Bonificaciones
        Task<IEnumerable<RevisarGeneracionBonificacionDto>> ObtenerRevisarBonificacionesAsync(
            FiltrosRevisarGeneracionBonificacionDto filtros);
        Task<byte[]> ExportarRevisarBonificacionesExcelAsync(
            IEnumerable<RevisarGeneracionBonificacionDto> datos);

        // Anulaciones
        Task<IEnumerable<AnulacionLiquidacionesDto>> ObtenerAnulacionesAsync(
            FiltrosAnulacionLiquidacionesDto filtros);
        Task<byte[]> ExportarAnulacionesExcelAsync(
            IEnumerable<AnulacionLiquidacionesDto> datos);
    }

    /// <summary>
    /// Implementación de servicios de Producción
    /// </summary>
    public class CcProduccionService : ICcProduccionService
    {
        private readonly CcProduccionAdapter _adapter;
        private readonly ILogger<CcProduccionService> _logger;

        public CcProduccionService(CcProduccionAdapter adapter, 
            ILogger<CcProduccionService> logger)
        {
            _adapter = adapter ?? throw new ArgumentNullException(nameof(adapter));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public async Task<IEnumerable<RegistroProduccionDto>> ObtenerRegistrosProduccionAsync(
            FiltrosRegistroProduccionDto filtros)
        {
            try
            {
                _logger.LogInformation(
                    $"Obtener registros de producción: período={filtros?.Periodo}, trabajo={filtros?.IdTrabajo}");

                var resultado = await _adapter.ObtenerRegistrosProduccionAsync(
                    filtros?.Periodo, filtros?.IdTrabajo, filtros?.IdEmpleado,
                    filtros?.IdActividad, filtros?.FechaInicio, filtros?.FechaFin, filtros?.Estado);

                _logger.LogInformation($"Se obtuvieron {resultado.Count} registros de producción");
                return resultado;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener registros de producción");
                throw;
            }
        }

        public async Task<byte[]> ExportarRegistrosProduccionExcelAsync(
            IEnumerable<RegistroProduccionDto> datos)
        {
            try
            {
                _logger.LogInformation("Exportando registros de producción a Excel");

                using (var workbook = new XLWorkbook())
                {
                    var ws = workbook.Worksheets.Add("Registros Producción");
                    
                    // Headers
                    int col = 1;
                    ws.Cell(1, col++).Value = "ID";
                    ws.Cell(1, col++).Value = "Período";
                    ws.Cell(1, col++).Value = "Trabajo";
                    ws.Cell(1, col++).Value = "Actividad";
                    ws.Cell(1, col++).Value = "Empleado";
                    ws.Cell(1, col++).Value = "Cantidad";
                    ws.Cell(1, col++).Value = "Costo Unitario";
                    ws.Cell(1, col++).Value = "Costo Total";
                    ws.Cell(1, col++).Value = "Fecha Producción";
                    ws.Cell(1, col++).Value = "Estado";

                    SetHeaderFormat(ws, 1, col - 1);

                    // Datos
                    int row = 2;
                    int itemCount = datos?.Count() ?? 0;

                    if (datos != null)
                    {
                        foreach (var item in datos)
                        {
                            col = 1;
                            ws.Cell(row, col++).Value = item.IdProduccion;
                            ws.Cell(row, col++).Value = item.Periodo;
                            ws.Cell(row, col++).Value = item.CodigoTrabajo;
                            ws.Cell(row, col++).Value = item.CodigoActividad;
                            ws.Cell(row, col++).Value = item.NombreEmpleado;
                            ws.Cell(row, col++).Value = item.Cantidad;
                            ws.Cell(row, col++).Value = item.CostoUnitario;
                            ws.Cell(row, col++).Value = item.CostoTotal;
                            ws.Cell(row, col++).Value = item.FechaProduccion;
                            ws.Cell(row, col++).Value = item.Estado;

                            row++;
                        }
                    }

                    // Formatear
                    ApplyCurrencyFormat(ws, 7, itemCount);
                    ApplyCurrencyFormat(ws, 8, itemCount);
                    ApplyDateFormat(ws, 9, itemCount);
                    ws.Columns().AdjustToContents();

                    using (var stream = new MemoryStream())
                    {
                        workbook.SaveAs(stream);
                        return stream.ToArray();
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al exportar registros de producción");
                throw;
            }
        }

        public async Task<IEnumerable<LiquidacionPlanillaDto>> ObtenerLiquidacionesAsync(
            FiltrosLiquidacionPlanillaDto filtros)
        {
            try
            {
                _logger.LogInformation($"Obtener liquidaciones: período={filtros?.Periodo}");

                var resultado = await _adapter.ObtenerLiquidacionesAsync(
                    filtros?.Periodo, filtros?.IdTrabajo, filtros?.IdEmpleado, filtros?.Estado);

                _logger.LogInformation($"Se obtuvieron {resultado.Count} liquidaciones");
                return resultado;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener liquidaciones");
                throw;
            }
        }

        public async Task<byte[]> ExportarLiquidacionesExcelAsync(
            IEnumerable<LiquidacionPlanillaDto> datos)
        {
            try
            {
                _logger.LogInformation("Exportando liquidaciones a Excel");

                using (var workbook = new XLWorkbook())
                {
                    var ws = workbook.Worksheets.Add("Liquidaciones");
                    
                    int col = 1;
                    ws.Cell(1, col++).Value = "ID";
                    ws.Cell(1, col++).Value = "Período";
                    ws.Cell(1, col++).Value = "Trabajo";
                    ws.Cell(1, col++).Value = "Empleado";
                    ws.Cell(1, col++).Value = "Salario Base";
                    ws.Cell(1, col++).Value = "Producción";
                    ws.Cell(1, col++).Value = "Bono";
                    ws.Cell(1, col++).Value = "Descuentos SS";
                    ws.Cell(1, col++).Value = "Valor Neto";
                    ws.Cell(1, col++).Value = "Fecha";

                    SetHeaderFormat(ws, 1, col - 1);

                    int row = 2;
                    int itemCount = datos?.Count() ?? 0;

                    if (datos != null)
                    {
                        foreach (var item in datos)
                        {
                            col = 1;
                            ws.Cell(row, col++).Value = item.IdLiquidacion;
                            ws.Cell(row, col++).Value = item.Periodo;
                            ws.Cell(row, col++).Value = item.CodigoTrabajo;
                            ws.Cell(row, col++).Value = item.NombreEmpleado;
                            ws.Cell(row, col++).Value = item.SalarioBase;
                            ws.Cell(row, col++).Value = item.ProduccionGenerada;
                            ws.Cell(row, col++).Value = item.BonoProduccion;
                            ws.Cell(row, col++).Value = item.DescuentosSS;
                            ws.Cell(row, col++).Value = item.ValorNeto;
                            ws.Cell(row, col++).Value = item.FechaLiquidacion;

                            row++;
                        }
                    }

                    ApplyCurrencyFormat(ws, 5, itemCount);
                    ApplyCurrencyFormat(ws, 6, itemCount);
                    ApplyCurrencyFormat(ws, 7, itemCount);
                    ApplyCurrencyFormat(ws, 8, itemCount);
                    ApplyCurrencyFormat(ws, 9, itemCount);
                    ApplyDateFormat(ws, 10, itemCount);
                    ws.Columns().AdjustToContents();

                    using (var stream = new MemoryStream())
                    {
                        workbook.SaveAs(stream);
                        return stream.ToArray();
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al exportar liquidaciones");
                throw;
            }
        }

        public async Task<IEnumerable<GenerarBonificacionDto>> ObtenerBonificacionesAsync(
            FiltrosGenerarBonificacionDto filtros)
        {
            try
            {
                _logger.LogInformation($"Obtener bonificaciones: período={filtros?.Periodo}");

                var resultado = await _adapter.ObtenerBonificacionesAsync(
                    filtros?.Periodo, filtros?.IdTrabajo, filtros?.IdEmpleado, filtros?.Estado);

                _logger.LogInformation($"Se obtuvieron {resultado.Count} bonificaciones");
                return resultado;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener bonificaciones");
                throw;
            }
        }

        public async Task<byte[]> ExportarBonificacionesExcelAsync(
            IEnumerable<GenerarBonificacionDto> datos)
        {
            try
            {
                _logger.LogInformation("Exportando bonificaciones a Excel");

                using (var workbook = new XLWorkbook())
                {
                    var ws = workbook.Worksheets.Add("Bonificaciones");
                    
                    int col = 1;
                    ws.Cell(1, col++).Value = "ID";
                    ws.Cell(1, col++).Value = "Período";
                    ws.Cell(1, col++).Value = "Empleado";
                    ws.Cell(1, col++).Value = "Trabajo";
                    ws.Cell(1, col++).Value = "Salario Base";
                    ws.Cell(1, col++).Value = "Producción";
                    ws.Cell(1, col++).Value = "% Meta";
                    ws.Cell(1, col++).Value = "Bono Calculado";
                    ws.Cell(1, col++).Value = "Bono Final";
                    ws.Cell(1, col++).Value = "Fecha";

                    SetHeaderFormat(ws, 1, col - 1);

                    int row = 2;
                    int itemCount = datos?.Count() ?? 0;

                    if (datos != null)
                    {
                        foreach (var item in datos)
                        {
                            col = 1;
                            ws.Cell(row, col++).Value = item.IdBonificacion;
                            ws.Cell(row, col++).Value = item.Periodo;
                            ws.Cell(row, col++).Value = item.NombreEmpleado;
                            ws.Cell(row, col++).Value = item.NombreTrabajo;
                            ws.Cell(row, col++).Value = item.SalarioBase;
                            ws.Cell(row, col++).Value = item.ProduccionTotal;
                            ws.Cell(row, col++).Value = item.PercentajeMetaBonificacion;
                            ws.Cell(row, col++).Value = item.BonoCalculado;
                            ws.Cell(row, col++).Value = item.BonoFinal;
                            ws.Cell(row, col++).Value = item.FechaGeneracion;

                            row++;
                        }
                    }

                    ApplyCurrencyFormat(ws, 5, itemCount);
                    ApplyCurrencyFormat(ws, 6, itemCount);
                    ApplyPercentageFormat(ws, 7, itemCount);
                    ApplyCurrencyFormat(ws, 8, itemCount);
                    ApplyCurrencyFormat(ws, 9, itemCount);
                    ApplyDateFormat(ws, 10, itemCount);
                    ws.Columns().AdjustToContents();

                    using (var stream = new MemoryStream())
                    {
                        workbook.SaveAs(stream);
                        return stream.ToArray();
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al exportar bonificaciones");
                throw;
            }
        }

        public async Task<IEnumerable<CargueDescuentoSSDto>> ObtenerDescuentosSsAsync(
            FiltrosCargueDescuentoSSDto filtros)
        {
            try
            {
                _logger.LogInformation($"Obtener descuentos SS: período={filtros?.Periodo}");

                var resultado = await _adapter.ObtenerDescuentosSsAsync(
                    filtros?.Periodo, filtros?.IdEmpleado, filtros?.TipoDescuento, filtros?.Estado);

                _logger.LogInformation($"Se obtuvieron {resultado.Count} descuentos");
                return resultado;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener descuentos SS");
                throw;
            }
        }

        public async Task<byte[]> ExportarDescuentosSsExcelAsync(
            IEnumerable<CargueDescuentoSSDto> datos)
        {
            try
            {
                _logger.LogInformation("Exportando descuentos SS a Excel");

                using (var workbook = new XLWorkbook())
                {
                    var ws = workbook.Worksheets.Add("Descuentos SS");
                    
                    int col = 1;
                    ws.Cell(1, col++).Value = "ID";
                    ws.Cell(1, col++).Value = "Período";
                    ws.Cell(1, col++).Value = "Empleado";
                    ws.Cell(1, col++).Value = "Tipo Descuento";
                    ws.Cell(1, col++).Value = "Valor";
                    ws.Cell(1, col++).Value = "Porcentaje";
                    ws.Cell(1, col++).Value = "Fecha";

                    SetHeaderFormat(ws, 1, col - 1);

                    int row = 2;
                    int itemCount = datos?.Count() ?? 0;

                    if (datos != null)
                    {
                        foreach (var item in datos)
                        {
                            col = 1;
                            ws.Cell(row, col++).Value = item.IdDescuento;
                            ws.Cell(row, col++).Value = item.Periodo;
                            ws.Cell(row, col++).Value = item.NombreEmpleado;
                            ws.Cell(row, col++).Value = item.TipoDescuento;
                            ws.Cell(row, col++).Value = item.ValorDescuento;
                            ws.Cell(row, col++).Value = item.PercentajeDescuento;
                            ws.Cell(row, col++).Value = item.FechaCarga;

                            row++;
                        }
                    }

                    ApplyCurrencyFormat(ws, 5, itemCount);
                    ApplyPercentageFormat(ws, 6, itemCount);
                    ApplyDateFormat(ws, 7, itemCount);
                    ws.Columns().AdjustToContents();

                    using (var stream = new MemoryStream())
                    {
                        workbook.SaveAs(stream);
                        return stream.ToArray();
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al exportar descuentos SS");
                throw;
            }
        }

        public async Task<IEnumerable<LiquidacionProductividadPstDto>> ObtenerLiquidacionesPstAsync(
            FiltrosLiquidacionProductividadPstDto filtros)
        {
            try
            {
                _logger.LogInformation($"Obtener liquidaciones PST: período={filtros?.Periodo}");

                var resultado = await _adapter.ObtenerLiquidacionesPstAsync(
                    filtros?.Periodo, filtros?.IdTrabajo, filtros?.IdEmpleado, filtros?.Estado);

                _logger.LogInformation($"Se obtuvieron {resultado.Count} liquidaciones PST");
                return resultado;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener liquidaciones PST");
                throw;
            }
        }

        public async Task<byte[]> ExportarLiquidacionesPstExcelAsync(
            IEnumerable<LiquidacionProductividadPstDto> datos)
        {
            try
            {
                _logger.LogInformation("Exportando liquidaciones PST a Excel");

                using (var workbook = new XLWorkbook())
                {
                    var ws = workbook.Worksheets.Add("Liquidaciones PST");
                    
                    int col = 1;
                    ws.Cell(1, col++).Value = "ID";
                    ws.Cell(1, col++).Value = "Período";
                    ws.Cell(1, col++).Value = "Trabajo";
                    ws.Cell(1, col++).Value = "Empleado";
                    ws.Cell(1, col++).Value = "Valor PST";
                    ws.Cell(1, col++).Value = "Producción";
                    ws.Cell(1, col++).Value = "% Liquidación";
                    ws.Cell(1, col++).Value = "Valor Liquidado";
                    ws.Cell(1, col++).Value = "Fecha";

                    SetHeaderFormat(ws, 1, col - 1);

                    int row = 2;
                    int itemCount = datos?.Count() ?? 0;

                    if (datos != null)
                    {
                        foreach (var item in datos)
                        {
                            col = 1;
                            ws.Cell(row, col++).Value = item.IdLiquidacionPST;
                            ws.Cell(row, col++).Value = item.Periodo;
                            ws.Cell(row, col++).Value = item.CodigoTrabajo;
                            ws.Cell(row, col++).Value = item.NombreEmpleado;
                            ws.Cell(row, col++).Value = item.ValorPST;
                            ws.Cell(row, col++).Value = item.ProduccionGenerada;
                            ws.Cell(row, col++).Value = item.PercentajeLiquidacion;
                            ws.Cell(row, col++).Value = item.ValorLiquidado;
                            ws.Cell(row, col++).Value = item.FechaLiquidacion;

                            row++;
                        }
                    }

                    ApplyCurrencyFormat(ws, 5, itemCount);
                    ApplyCurrencyFormat(ws, 6, itemCount);
                    ApplyPercentageFormat(ws, 7, itemCount);
                    ApplyCurrencyFormat(ws, 8, itemCount);
                    ApplyDateFormat(ws, 9, itemCount);
                    ws.Columns().AdjustToContents();

                    using (var stream = new MemoryStream())
                    {
                        workbook.SaveAs(stream);
                        return stream.ToArray();
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al exportar liquidaciones PST");
                throw;
            }
        }

        public async Task<IEnumerable<AsignacionCostosPstDto>> ObtenerAsignacionesCostosAsync(
            FiltrosAsignacionCostosPstDto filtros)
        {
            try
            {
                _logger.LogInformation($"Obtener asignaciones costos: período={filtros?.Periodo}");

                var resultado = await _adapter.ObtenerAsignacionesCostosAsync(
                    filtros?.Periodo, filtros?.IdTrabajo, filtros?.IdConcepto, filtros?.Estado);

                _logger.LogInformation($"Se obtuvieron {resultado.Count} asignaciones");
                return resultado;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener asignaciones de costos");
                throw;
            }
        }

        public async Task<byte[]> ExportarAsignacionesCostosExcelAsync(
            IEnumerable<AsignacionCostosPstDto> datos)
        {
            try
            {
                _logger.LogInformation("Exportando asignaciones de costos a Excel");

                using (var workbook = new XLWorkbook())
                {
                    var ws = workbook.Worksheets.Add("Asignaciones Costos");
                    
                    int col = 1;
                    ws.Cell(1, col++).Value = "ID";
                    ws.Cell(1, col++).Value = "Período";
                    ws.Cell(1, col++).Value = "Trabajo";
                    ws.Cell(1, col++).Value = "Concepto";
                    ws.Cell(1, col++).Value = "Costo Base";
                    ws.Cell(1, col++).Value = "Costo Asignado";
                    ws.Cell(1, col++).Value = "Fecha";

                    SetHeaderFormat(ws, 1, col - 1);

                    int row = 2;
                    int itemCount = datos?.Count() ?? 0;

                    if (datos != null)
                    {
                        foreach (var item in datos)
                        {
                            col = 1;
                            ws.Cell(row, col++).Value = item.IdAsignacion;
                            ws.Cell(row, col++).Value = item.Periodo;
                            ws.Cell(row, col++).Value = item.CodigoTrabajo;
                            ws.Cell(row, col++).Value = item.NombreConcepto;
                            ws.Cell(row, col++).Value = item.CostoBase;
                            ws.Cell(row, col++).Value = item.CostoAsignado;
                            ws.Cell(row, col++).Value = item.FechaAsignacion;

                            row++;
                        }
                    }

                    ApplyCurrencyFormat(ws, 5, itemCount);
                    ApplyCurrencyFormat(ws, 6, itemCount);
                    ApplyDateFormat(ws, 7, itemCount);
                    ws.Columns().AdjustToContents();

                    using (var stream = new MemoryStream())
                    {
                        workbook.SaveAs(stream);
                        return stream.ToArray();
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al exportar asignaciones de costos");
                throw;
            }
        }

        public async Task<IEnumerable<EstadoJobBookDto>> ObtenerEstadoJobBooksAsync(
            FiltrosEstadoJobBookDto filtros)
        {
            try
            {
                _logger.LogInformation($"Obtener estado jobbooks: trabajo={filtros?.IdTrabajo}");

                var resultado = await _adapter.ObtenerEstadoJobBooksAsync(
                    filtros?.IdTrabajo, filtros?.EstadoActual);

                _logger.LogInformation($"Se obtuvieron {resultado.Count} jobbooks");
                return resultado;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener estado de jobbooks");
                throw;
            }
        }

        public async Task<byte[]> ExportarEstadoJobBooksExcelAsync(
            IEnumerable<EstadoJobBookDto> datos)
        {
            try
            {
                _logger.LogInformation("Exportando jobbooks a Excel");

                using (var workbook = new XLWorkbook())
                {
                    var ws = workbook.Worksheets.Add("JobBooks");
                    
                    int col = 1;
                    ws.Cell(1, col++).Value = "ID";
                    ws.Cell(1, col++).Value = "Trabajo";
                    ws.Cell(1, col++).Value = "JobBook";
                    ws.Cell(1, col++).Value = "Estado";
                    ws.Cell(1, col++).Value = "Apertura";
                    ws.Cell(1, col++).Value = "Cierre";
                    ws.Cell(1, col++).Value = "Monto";

                    SetHeaderFormat(ws, 1, col - 1);

                    int row = 2;
                    int itemCount = datos?.Count() ?? 0;

                    if (datos != null)
                    {
                        foreach (var item in datos)
                        {
                            col = 1;
                            ws.Cell(row, col++).Value = item.IdJobBook;
                            ws.Cell(row, col++).Value = item.CodigoTrabajo;
                            ws.Cell(row, col++).Value = item.NumeroJobBook;
                            ws.Cell(row, col++).Value = item.EstadoActualNombre;
                            ws.Cell(row, col++).Value = item.FechaApertura;
                            ws.Cell(row, col++).Value = item.FechaCierre;
                            ws.Cell(row, col++).Value = item.MontoTotal;

                            row++;
                        }
                    }

                    ApplyDateFormat(ws, 5, itemCount);
                    ApplyDateFormat(ws, 6, itemCount);
                    ApplyCurrencyFormat(ws, 7, itemCount);
                    ws.Columns().AdjustToContents();

                    using (var stream = new MemoryStream())
                    {
                        workbook.SaveAs(stream);
                        return stream.ToArray();
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al exportar jobbooks");
                throw;
            }
        }

        public async Task<IEnumerable<RevisarGeneracionBonificacionDto>> ObtenerRevisarBonificacionesAsync(
            FiltrosRevisarGeneracionBonificacionDto filtros)
        {
            try
            {
                _logger.LogInformation($"Obtener bonificaciones para revisión: período={filtros?.Periodo}");

                var resultado = await _adapter.ObtenerRevisarBonificacionesAsync(
                    filtros?.Periodo, filtros?.IdEmpleado, filtros?.IdTrabajo, filtros?.Aprobada);

                _logger.LogInformation($"Se obtuvieron {resultado.Count} bonificaciones para revisar");
                return resultado;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener bonificaciones para revisión");
                throw;
            }
        }

        public async Task<byte[]> ExportarRevisarBonificacionesExcelAsync(
            IEnumerable<RevisarGeneracionBonificacionDto> datos)
        {
            try
            {
                _logger.LogInformation("Exportando bonificaciones para revisión a Excel");

                using (var workbook = new XLWorkbook())
                {
                    var ws = workbook.Worksheets.Add("Revisión Bonificaciones");
                    
                    int col = 1;
                    ws.Cell(1, col++).Value = "ID";
                    ws.Cell(1, col++).Value = "Período";
                    ws.Cell(1, col++).Value = "Empleado";
                    ws.Cell(1, col++).Value = "Salario";
                    ws.Cell(1, col++).Value = "Producción";
                    ws.Cell(1, col++).Value = "Bono Calc.";
                    ws.Cell(1, col++).Value = "Bono Final";
                    ws.Cell(1, col++).Value = "Generación";
                    ws.Cell(1, col++).Value = "Revisión";
                    ws.Cell(1, col++).Value = "Aprobada";

                    SetHeaderFormat(ws, 1, col - 1);

                    int row = 2;
                    int itemCount = datos?.Count() ?? 0;

                    if (datos != null)
                    {
                        foreach (var item in datos)
                        {
                            col = 1;
                            ws.Cell(row, col++).Value = item.IdBonificacion;
                            ws.Cell(row, col++).Value = item.Periodo;
                            ws.Cell(row, col++).Value = item.NombreEmpleado;
                            ws.Cell(row, col++).Value = item.SalarioBase;
                            ws.Cell(row, col++).Value = item.ProduccionTotal;
                            ws.Cell(row, col++).Value = item.BonoCalculado;
                            ws.Cell(row, col++).Value = item.BonoFinal;
                            ws.Cell(row, col++).Value = item.FechaGeneracion;
                            ws.Cell(row, col++).Value = item.FechaRevision;
                            ws.Cell(row, col++).Value = item.Aprobada ? "Sí" : "No";

                            row++;
                        }
                    }

                    ApplyCurrencyFormat(ws, 4, itemCount);
                    ApplyCurrencyFormat(ws, 5, itemCount);
                    ApplyCurrencyFormat(ws, 6, itemCount);
                    ApplyCurrencyFormat(ws, 7, itemCount);
                    ApplyDateFormat(ws, 8, itemCount);
                    ApplyDateFormat(ws, 9, itemCount);
                    ws.Columns().AdjustToContents();

                    using (var stream = new MemoryStream())
                    {
                        workbook.SaveAs(stream);
                        return stream.ToArray();
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al exportar bonificaciones para revisión");
                throw;
            }
        }

        public async Task<IEnumerable<AnulacionLiquidacionesDto>> ObtenerAnulacionesAsync(
            FiltrosAnulacionLiquidacionesDto filtros)
        {
            try
            {
                _logger.LogInformation($"Obtener anulaciones: período={filtros?.Periodo}");

                var resultado = await _adapter.ObtenerAnulacionesAsync(
                    filtros?.Periodo, filtros?.IdEmpleado, filtros?.IdTrabajo, 
                    filtros?.FechaInicio, filtros?.FechaFin);

                _logger.LogInformation($"Se obtuvieron {resultado.Count} anulaciones");
                return resultado;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener anulaciones");
                throw;
            }
        }

        public async Task<byte[]> ExportarAnulacionesExcelAsync(
            IEnumerable<AnulacionLiquidacionesDto> datos)
        {
            try
            {
                _logger.LogInformation("Exportando anulaciones a Excel");

                using (var workbook = new XLWorkbook())
                {
                    var ws = workbook.Worksheets.Add("Anulaciones");
                    
                    int col = 1;
                    ws.Cell(1, col++).Value = "ID";
                    ws.Cell(1, col++).Value = "Período";
                    ws.Cell(1, col++).Value = "Empleado";
                    ws.Cell(1, col++).Value = "Trabajo";
                    ws.Cell(1, col++).Value = "Valor";
                    ws.Cell(1, col++).Value = "Estado";
                    ws.Cell(1, col++).Value = "Liquidación";
                    ws.Cell(1, col++).Value = "Motivo";
                    ws.Cell(1, col++).Value = "Anulación";
                    ws.Cell(1, col++).Value = "Usuario";

                    SetHeaderFormat(ws, 1, col - 1);

                    int row = 2;
                    int itemCount = datos?.Count() ?? 0;

                    if (datos != null)
                    {
                        foreach (var item in datos)
                        {
                            col = 1;
                            ws.Cell(row, col++).Value = item.IdLiquidacion;
                            ws.Cell(row, col++).Value = item.Periodo;
                            ws.Cell(row, col++).Value = item.NombreEmpleado;
                            ws.Cell(row, col++).Value = item.NombreTrabajo;
                            ws.Cell(row, col++).Value = item.ValorLiquidado;
                            ws.Cell(row, col++).Value = item.EstadoActualNombre;
                            ws.Cell(row, col++).Value = item.FechaLiquidacion;
                            ws.Cell(row, col++).Value = item.Motivoanulacion;
                            ws.Cell(row, col++).Value = item.FechaAnulacion;
                            ws.Cell(row, col++).Value = item.UsuarioAnulacion;

                            row++;
                        }
                    }

                    ApplyCurrencyFormat(ws, 5, itemCount);
                    ApplyDateFormat(ws, 7, itemCount);
                    ApplyDateFormat(ws, 9, itemCount);
                    ws.Columns().AdjustToContents();

                    using (var stream = new MemoryStream())
                    {
                        workbook.SaveAs(stream);
                        return stream.ToArray();
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al exportar anulaciones");
                throw;
            }
        }

        #region Helper Methods

        private void SetHeaderFormat(IXLWorksheet ws, int startCol, int endCol)
        {
            var headerRange = ws.Range(1, startCol, 1, endCol);
            headerRange.Style.Fill.BackgroundColor = XLColor.FromArgb(0x4472C4);
            headerRange.Style.Font.Bold = true;
            headerRange.Style.Font.FontColor = XLColor.White;
            headerRange.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
        }

        private void ApplyCurrencyFormat(IXLWorksheet ws, int column, int itemCount)
        {
            var range = ws.Range(2, column, itemCount + 1, column);
            range.Style.NumberFormat.Format = "#,##0.00";
        }

        private void ApplyDateFormat(IXLWorksheet ws, int column, int itemCount)
        {
            var range = ws.Range(2, column, itemCount + 1, column);
            range.Style.DateFormat.Format = "dd/MM/yyyy";
        }

        private void ApplyPercentageFormat(IXLWorksheet ws, int column, int itemCount)
        {
            var range = ws.Range(2, column, itemCount + 1, column);
            range.Style.NumberFormat.Format = "0.00%";
        }

        #endregion
    }
}
