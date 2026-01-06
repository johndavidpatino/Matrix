using ClosedXML.Excel;
using MatrixNext.Data.Modules.CC.Adapters;
using MatrixNext.Data.Modules.CC.DTOs.ProcesosInternos;
using Microsoft.Extensions.Logging;

namespace MatrixNext.Data.Modules.CC.Services
{
    /// <summary>
    /// Interface para servicios de Procesos Internos
    /// </summary>
    public interface ICcProcesosInternosService
    {
        // Reporte de Conteos
        Task<IEnumerable<ReporteConteoDto>> ObtenerReporteConteosAsync(
            FiltrosReporteConteoDto filtros);
        
        Task<dynamic> ObtenerTotalesConteosAsync(
            DateTime? fechaInicio = null, DateTime? fechaFin = null);
        
        Task<byte[]> ExportarReporteConteosExcelAsync(
            IEnumerable<ReporteConteoDto> conteos);

        // Resumen de Productividad
        Task<IEnumerable<ResumenProductividadDto>> ObtenerResumenProductividadAsync(
            FiltrosResumenProductividadDto filtros);
        
        Task<ProductividadAgregadaDto?> ObtenerProductividadAgregadaAsync(
            int? periodo = null, DateTime? fechaInicio = null, DateTime? fechaFin = null);
        
        Task<byte[]> ExportarResumenProductividadExcelAsync(
            IEnumerable<ResumenProductividadDto> resumen);
    }

    /// <summary>
    /// Servicio de Procesos Internos
    /// </summary>
    public class CcProcesosInternosService : ICcProcesosInternosService
    {
        private readonly CcProcesosInternosAdapter _adapter;
        private readonly ILogger<CcProcesosInternosService> _logger;

        public CcProcesosInternosService(
            CcProcesosInternosAdapter adapter,
            ILogger<CcProcesosInternosService> logger)
        {
            _adapter = adapter;
            _logger = logger;
        }

        #region Reporte de Conteos

        public async Task<IEnumerable<ReporteConteoDto>> ObtenerReporteConteosAsync(
            FiltrosReporteConteoDto filtros)
        {
            var conteos = await _adapter.ObtenerReporteConteosAsync(filtros);
            _logger.LogInformation($"Obtenidos {conteos.Count()} registros de conteos");
            return conteos;
        }

        public async Task<dynamic> ObtenerTotalesConteosAsync(
            DateTime? fechaInicio = null, DateTime? fechaFin = null)
        {
            var totales = await _adapter.ObtenerTotalesConteosAsync(fechaInicio, fechaFin);
            return totales;
        }

        public async Task<byte[]> ExportarReporteConteosExcelAsync(
            IEnumerable<ReporteConteoDto> conteos)
        {
            using var workbook = new XLWorkbook();
            var worksheet = workbook.Worksheets.Add("Reporte de Conteos");

            // Headers
            var headers = new[]
            {
                "ID Conteo", "Código Trabajo", "Nombre Trabajo",
                "Código Actividad", "Nombre Actividad", "Categoría",
                "Cantidad", "Fecha Conteo", "Usuario", "Estado", "Observaciones"
            };

            for (int i = 0; i < headers.Length; i++)
            {
                var cell = worksheet.Cell(1, i + 1);
                cell.Value = headers[i];
                cell.Style.Font.Bold = true;
                cell.Style.Fill.BackgroundColor = XLColor.LightBlue;
                cell.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
            }

            // Data
            int row = 2;
            foreach (var conteo in conteos)
            {
                worksheet.Cell(row, 1).Value = conteo.IdConteo;
                worksheet.Cell(row, 2).Value = conteo.CodigoTrabajo;
                worksheet.Cell(row, 3).Value = conteo.NombreTrabajo;
                worksheet.Cell(row, 4).Value = conteo.CodigoActividad;
                worksheet.Cell(row, 5).Value = conteo.NombreActividad;
                worksheet.Cell(row, 6).Value = conteo.Categoria ?? "-";
                worksheet.Cell(row, 7).Value = conteo.Cantidad;
                worksheet.Cell(row, 8).Value = conteo.FechaConteo.ToString("dd/MM/yyyy");
                worksheet.Cell(row, 9).Value = conteo.UsuarioRegistro ?? "-";
                worksheet.Cell(row, 10).Value = conteo.EstadoNombre;
                worksheet.Cell(row, 11).Value = conteo.Observaciones ?? "-";

                row++;
            }

            // Auto-fit columns
            worksheet.Columns().AdjustToContents();

            using var stream = new MemoryStream();
            workbook.SaveAs(stream);
            return stream.ToArray();
        }

        #endregion

        #region Resumen de Productividad

        public async Task<IEnumerable<ResumenProductividadDto>> ObtenerResumenProductividadAsync(
            FiltrosResumenProductividadDto filtros)
        {
            var resumen = await _adapter.ObtenerResumenProductividadAsync(filtros);
            _logger.LogInformation($"Obtenidos {resumen.Count()} registros de productividad");
            return resumen;
        }

        public async Task<ProductividadAgregadaDto?> ObtenerProductividadAgregadaAsync(
            int? periodo = null, DateTime? fechaInicio = null, DateTime? fechaFin = null)
        {
            var agregada = await _adapter.ObtenerProductividadAgregadaAsync(periodo, fechaInicio, fechaFin);
            return agregada;
        }

        public async Task<byte[]> ExportarResumenProductividadExcelAsync(
            IEnumerable<ResumenProductividadDto> resumen)
        {
            using var workbook = new XLWorkbook();
            var worksheet = workbook.Worksheets.Add("Resumen Productividad");

            // Headers
            var headers = new[]
            {
                "Período", "Código Trabajo", "Nombre Trabajo",
                "Código Actividad", "Nombre Actividad", "Total Unidades",
                "Total Horas", "Productividad Prom.", "Costo Total",
                "Costo Unitario", "Num. Empleados", "Fecha Inicio", "Fecha Fin"
            };

            for (int i = 0; i < headers.Length; i++)
            {
                var cell = worksheet.Cell(1, i + 1);
                cell.Value = headers[i];
                cell.Style.Font.Bold = true;
                cell.Style.Fill.BackgroundColor = XLColor.LightGreen;
                cell.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
            }

            // Data
            int row = 2;
            decimal totalCostos = 0;
            int totalUnidades = 0;
            int totalHoras = 0;

            foreach (var item in resumen)
            {
                worksheet.Cell(row, 1).Value = item.Periodo;
                worksheet.Cell(row, 2).Value = item.CodigoTrabajo;
                worksheet.Cell(row, 3).Value = item.NombreTrabajo;
                worksheet.Cell(row, 4).Value = item.CodigoActividad ?? "-";
                worksheet.Cell(row, 5).Value = item.NombreActividad ?? "-";
                worksheet.Cell(row, 6).Value = item.TotalUnidades;
                worksheet.Cell(row, 7).Value = item.TotalHoras;
                worksheet.Cell(row, 8).Value = item.ProductividadPromedio;
                worksheet.Cell(row, 9).Value = item.CostoTotal;
                worksheet.Cell(row, 9).Style.NumberFormat.Format = "$#,##0.00";
                worksheet.Cell(row, 10).Value = item.CostoUnitario;
                worksheet.Cell(row, 10).Style.NumberFormat.Format = "$#,##0.00";
                worksheet.Cell(row, 11).Value = item.NumeroEmpleados;
                worksheet.Cell(row, 12).Value = item.FechaInicio?.ToString("dd/MM/yyyy") ?? "-";
                worksheet.Cell(row, 13).Value = item.FechaFin?.ToString("dd/MM/yyyy") ?? "-";

                // Color coding para productividad
                var prodCell = worksheet.Cell(row, 8);
                if (item.ProductividadPromedio < 5)
                    prodCell.Style.Fill.BackgroundColor = XLColor.Red;
                else if (item.ProductividadPromedio >= 10)
                    prodCell.Style.Fill.BackgroundColor = XLColor.LightGreen;

                totalCostos += item.CostoTotal;
                totalUnidades += item.TotalUnidades;
                totalHoras += item.TotalHoras;

                row++;
            }

            // Totals row
            worksheet.Cell(row, 1).Value = "TOTALES:";
            worksheet.Cell(row, 1).Style.Font.Bold = true;
            worksheet.Cell(row, 6).Value = totalUnidades;
            worksheet.Cell(row, 6).Style.Font.Bold = true;
            worksheet.Cell(row, 7).Value = totalHoras;
            worksheet.Cell(row, 7).Style.Font.Bold = true;
            worksheet.Cell(row, 9).Value = totalCostos;
            worksheet.Cell(row, 9).Style.Font.Bold = true;
            worksheet.Cell(row, 9).Style.NumberFormat.Format = "$#,##0.00";

            // Auto-fit columns
            worksheet.Columns().AdjustToContents();

            using var stream = new MemoryStream();
            workbook.SaveAs(stream);
            return stream.ToArray();
        }

        #endregion
    }
}
