using ClosedXML.Excel;
using MatrixNext.Data.Modules.CC.Adapters;
using MatrixNext.Data.Modules.CC.DTOs.Reportes;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace MatrixNext.Data.Modules.CC.Services
{
    public interface ICcReportesService
    {
        Task<IEnumerable<ReportePagoDto>> ObtenerPagosAsync(FiltrosReportePagosDto filtros);
        Task<byte[]> ExportarPagosExcelAsync(IEnumerable<ReportePagoDto> pagos);

        Task<IEnumerable<ReporteActividadProduccionDto>> ObtenerActividadesProduccionAsync(FiltrosReporteActividadProduccionDto filtros);
        Task<byte[]> ExportarActividadesProduccionExcelAsync(IEnumerable<ReporteActividadProduccionDto> actividades);

        Task<IEnumerable<ReporteContabilizacionPstDto>> ObtenerContabilizacionPstAsync(FiltrosReporteContabilizacionPstDto filtros);
        Task<byte[]> ExportarContabilizacionPstExcelAsync(IEnumerable<ReporteContabilizacionPstDto> contabilizaciones);

        Task<IEnumerable<ReporteVarianzaPresupuestariaDto>> ObtenerVarianzasPresupuestariasAsync(FiltrosReporteVarianzaPresupuestariaDto filtros);
        Task<byte[]> ExportarVarianzasPresupuestariasExcelAsync(IEnumerable<ReporteVarianzaPresupuestariaDto> varianzas);
    }

    public class CcReportesService : ICcReportesService
    {
        private readonly CcReportesAdapter _adapter;
        private readonly ILogger<CcReportesService> _logger;

        public CcReportesService(CcReportesAdapter adapter, ILogger<CcReportesService> logger)
        {
            _adapter = adapter;
            _logger = logger;
        }

        public async Task<IEnumerable<ReportePagoDto>> ObtenerPagosAsync(FiltrosReportePagosDto filtros)
        {
            filtros ??= new FiltrosReportePagosDto();
            var pagos = await _adapter.ObtenerReportePagosAsync(
                filtros.Periodo,
                filtros.IdTrabajo,
                filtros.IdEmpleado,
                filtros.Estado,
                filtros.FechaInicio,
                filtros.FechaFin);
            _logger.LogInformation("{Count} pagos obtenidos", pagos.Count());
            return pagos;
        }

        public async Task<byte[]> ExportarPagosExcelAsync(IEnumerable<ReportePagoDto> pagos)
        {
            var data = pagos?.ToList() ?? new List<ReportePagoDto>();
            using var workbook = new XLWorkbook();
            var worksheet = workbook.Worksheets.Add("Pagos");

            var headers = new[]
            {
                "Período", "Código Trabajo", "Nombre Trabajo", "Empleado",
                "Valor Pagado", "Fecha Pago", "Estado", "Medio Pago", "Observaciones"
            };

            SetHeaders(worksheet, headers);

            var row = 2;
            foreach (var pago in data)
            {
                worksheet.Cell(row, 1).Value = pago.Periodo;
                worksheet.Cell(row, 2).Value = pago.CodigoTrabajo ?? "-";
                worksheet.Cell(row, 3).Value = pago.NombreTrabajo ?? "-";
                worksheet.Cell(row, 4).Value = pago.NombreEmpleado ?? "-";
                worksheet.Cell(row, 5).Value = pago.ValorPagado;
                worksheet.Cell(row, 6).Value = pago.FechaPago;
                worksheet.Cell(row, 7).Value = pago.Estado ?? "-";
                worksheet.Cell(row, 8).Value = pago.MedioPago ?? "-";
                worksheet.Cell(row, 9).Value = pago.Observaciones ?? "-";
                row++;
            }

            ApplyCurrencyFormat(worksheet, 5, data.Count);
            ApplyDateFormat(worksheet, 6, data.Count);
            worksheet.Columns().AdjustToContents();

            using var stream = new MemoryStream();
            workbook.SaveAs(stream);
            return stream.ToArray();
        }

        public async Task<IEnumerable<ReporteActividadProduccionDto>> ObtenerActividadesProduccionAsync(
            FiltrosReporteActividadProduccionDto filtros)
        {
            filtros ??= new FiltrosReporteActividadProduccionDto();
            var actividades = await _adapter.ObtenerActividadesProduccionAsync(
                filtros.Periodo,
                filtros.IdTrabajo,
                filtros.FechaInicio,
                filtros.FechaFin);
            _logger.LogInformation("{Count} registros de actividad obtenidos", actividades.Count());
            return actividades;
        }

        public async Task<byte[]> ExportarActividadesProduccionExcelAsync(
            IEnumerable<ReporteActividadProduccionDto> actividades)
        {
            var data = actividades?.ToList() ?? new List<ReporteActividadProduccionDto>();
            using var workbook = new XLWorkbook();
            var worksheet = workbook.Worksheets.Add("Actividades");

            var headers = new[]
            {
                "Código Trabajo", "Nombre Trabajo", "Actividad",
                "Cantidad", "Costo Unitario", "Costo Total", "Fecha Registro",
                "Usuario", "Estado"
            };

            SetHeaders(worksheet, headers);

            var row = 2;
            foreach (var actividad in data)
            {
                worksheet.Cell(row, 1).Value = actividad.CodigoTrabajo ?? "-";
                worksheet.Cell(row, 2).Value = actividad.NombreTrabajo ?? "-";
                worksheet.Cell(row, 3).Value = actividad.Actividad ?? "-";
                worksheet.Cell(row, 4).Value = actividad.Cantidad;
                worksheet.Cell(row, 5).Value = actividad.CostoUnitario;
                worksheet.Cell(row, 6).Value = actividad.CostoTotal;
                worksheet.Cell(row, 7).Value = actividad.FechaRegistro;
                worksheet.Cell(row, 8).Value = actividad.UsuarioRegistro ?? "-";
                worksheet.Cell(row, 9).Value = actividad.Estado ?? "-";
                row++;
            }

            ApplyNumberFormat(worksheet, 4, data.Count, "#,##0.00");
            ApplyCurrencyFormat(worksheet, 5, data.Count);
            ApplyCurrencyFormat(worksheet, 6, data.Count);
            ApplyDateFormat(worksheet, 7, data.Count);
            worksheet.Columns().AdjustToContents();

            using var stream = new MemoryStream();
            workbook.SaveAs(stream);
            return stream.ToArray();
        }

        public async Task<IEnumerable<ReporteContabilizacionPstDto>> ObtenerContabilizacionPstAsync(
            FiltrosReporteContabilizacionPstDto filtros)
        {
            filtros ??= new FiltrosReporteContabilizacionPstDto();
            var contabilizaciones = await _adapter.ObtenerContabilizacionPstAsync(
                filtros.Periodo,
                filtros.IdTrabajo,
                filtros.FechaInicio,
                filtros.FechaFin);
            _logger.LogInformation("{Count} registros PST obtenidos", contabilizaciones.Count());
            return contabilizaciones;
        }

        public async Task<byte[]> ExportarContabilizacionPstExcelAsync(
            IEnumerable<ReporteContabilizacionPstDto> contabilizaciones)
        {
            var data = contabilizaciones?.ToList() ?? new List<ReporteContabilizacionPstDto>();
            using var workbook = new XLWorkbook();
            var worksheet = workbook.Worksheets.Add("Contabilización PST");

            var headers = new[]
            {
                "Período", "Código Trabajo", "Nombre Trabajo", "Código PST",
                "Valor Contabilizado", "Fecha Contabilización", "Usuario", "Estado"
            };

            SetHeaders(worksheet, headers);

            var row = 2;
            foreach (var item in data)
            {
                worksheet.Cell(row, 1).Value = item.Periodo;
                worksheet.Cell(row, 2).Value = item.CodigoTrabajo ?? "-";
                worksheet.Cell(row, 3).Value = item.NombreTrabajo ?? "-";
                worksheet.Cell(row, 4).Value = item.CodigoPst ?? "-";
                worksheet.Cell(row, 5).Value = item.ValorContabilizado;
                worksheet.Cell(row, 6).Value = item.FechaContabilizacion;
                worksheet.Cell(row, 7).Value = item.UsuarioContabiliza ?? "-";
                worksheet.Cell(row, 8).Value = item.Estado ?? "-";
                row++;
            }

            ApplyCurrencyFormat(worksheet, 5, data.Count);
            ApplyDateFormat(worksheet, 6, data.Count);
            worksheet.Columns().AdjustToContents();

            using var stream = new MemoryStream();
            workbook.SaveAs(stream);
            return stream.ToArray();
        }

        public async Task<IEnumerable<ReporteVarianzaPresupuestariaDto>> ObtenerVarianzasPresupuestariasAsync(
            FiltrosReporteVarianzaPresupuestariaDto filtros)
        {
            filtros ??= new FiltrosReporteVarianzaPresupuestariaDto();
            var varianzas = await _adapter.ObtenerVarianzasPresupuestariasAsync(
                filtros.Periodo,
                filtros.IdTrabajo);
            _logger.LogInformation("{Count} varianzas obtenidas", varianzas.Count());
            return varianzas;
        }

        public async Task<byte[]> ExportarVarianzasPresupuestariasExcelAsync(
            IEnumerable<ReporteVarianzaPresupuestariaDto> varianzas)
        {
            var data = varianzas?.ToList() ?? new List<ReporteVarianzaPresupuestariaDto>();
            using var workbook = new XLWorkbook();
            var worksheet = workbook.Worksheets.Add("Varianzas");

            var headers = new[]
            {
                "Período", "Código Trabajo", "Nombre Trabajo",
                "Presupuesto", "Ejecutado", "Varianza", "% Varianza"
            };

            SetHeaders(worksheet, headers);

            var row = 2;
            foreach (var item in data)
            {
                worksheet.Cell(row, 1).Value = item.Periodo;
                worksheet.Cell(row, 2).Value = item.CodigoTrabajo ?? "-";
                worksheet.Cell(row, 3).Value = item.NombreTrabajo ?? "-";
                worksheet.Cell(row, 4).Value = item.Presupuesto;
                worksheet.Cell(row, 5).Value = item.Ejecutado;
                worksheet.Cell(row, 6).Value = item.Varianza;
                worksheet.Cell(row, 7).Value = item.PorcentajeVarianza / 100m;
                row++;
            }

            ApplyCurrencyFormat(worksheet, 4, data.Count);
            ApplyCurrencyFormat(worksheet, 5, data.Count);
            ApplyCurrencyFormat(worksheet, 6, data.Count);
            ApplyPercentageFormat(worksheet, 7, data.Count);
            worksheet.Columns().AdjustToContents();

            using var stream = new MemoryStream();
            workbook.SaveAs(stream);
            return stream.ToArray();
        }

        private static void SetHeaders(IXLWorksheet worksheet, string[] headers)
        {
            for (int i = 0; i < headers.Length; i++)
            {
                var cell = worksheet.Cell(1, i + 1);
                cell.Value = headers[i];
                cell.Style.Font.Bold = true;
                cell.Style.Fill.BackgroundColor = XLColor.FromHtml("#0d6efd");
                cell.Style.Font.FontColor = XLColor.White;
                cell.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
            }
        }

        private static void ApplyCurrencyFormat(IXLWorksheet worksheet, int column, int itemCount)
        {
            if (itemCount == 0) return;
            worksheet.Range(2, column, itemCount + 1, column)
                .Style.NumberFormat.Format = "$ #,##0.00";
        }

        private static void ApplyDateFormat(IXLWorksheet worksheet, int column, int itemCount)
        {
            if (itemCount == 0) return;
            worksheet.Range(2, column, itemCount + 1, column)
                .Style.DateFormat.Format = "dd/MM/yyyy";
        }

        private static void ApplyNumberFormat(IXLWorksheet worksheet, int column, int itemCount, string format)
        {
            if (itemCount == 0) return;
            worksheet.Range(2, column, itemCount + 1, column)
                .Style.NumberFormat.Format = format;
        }

        private static void ApplyPercentageFormat(IXLWorksheet worksheet, int column, int itemCount)
        {
            if (itemCount == 0) return;
            worksheet.Range(2, column, itemCount + 1, column)
                .Style.NumberFormat.Format = "0.00%";
        }
    }
}
