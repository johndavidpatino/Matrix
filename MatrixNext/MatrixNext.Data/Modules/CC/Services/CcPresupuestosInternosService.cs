using ClosedXML.Excel;
using MatrixNext.Data.Modules.CC.Adapters;
using MatrixNext.Data.Modules.CC.DTOs.PresupuestosInternos;
using Microsoft.Extensions.Logging;

namespace MatrixNext.Data.Modules.CC.Services
{
    /// <summary>
    /// Interface para servicios de Presupuestos Internos
    /// </summary>
    public interface ICcPresupuestosInternosService
    {
        Task<IEnumerable<PresupuestoInternoDto>> ObtenerPresupuestosInternosAsync(
            int? periodo = null, string? codigoEmpresa = null, byte? estado = null);
        
        Task<PresupuestoInternoDto?> ObtenerPresupuestoInternoDetalleAsync(
            long idPresupuestoInterno);
        
        Task<IEnumerable<DetallePresupuestoInternoDto>> 
            ObtenerDetallesPresupuestoInternoAsync(long idPresupuestoInterno);
        
        Task<long> GuardarPresupuestoInternoAsync(PresupuestoInternoDto presupuesto);
        
        Task EliminarPresupuestoInternoAsync(long idPresupuestoInterno);
        
        Task<IEnumerable<HistoricoPresupuestoInternoDto>> 
            ObtenerHistoricoPresupuestoInternoAsync(long idPresupuestoInterno);
        
        Task<IEnumerable<ResumenPresupuestoInternoDto>> 
            ObtenerResumenPresupuestosInternosAsync(int? periodo = null);
        
        Task AprobarPresupuestoInternoAsync(long idPresupuestoInterno, string usuarioAprobacion);
        
        Task<long> GuardarDetallePresupuestoInternoAsync(
            DetallePresupuestoInternoDto detalle);
        
        Task EliminarDetallePresupuestoInternoAsync(long idDetalle);
        
        Task<byte[]> ExportarPresupuestosInternosExcelAsync(
            IEnumerable<PresupuestoInternoDto> presupuestos);
    }

    /// <summary>
    /// Servicio de Presupuestos Internos
    /// </summary>
    public class CcPresupuestosInternosService : ICcPresupuestosInternosService
    {
        private readonly CcPresupuestosInternosAdapter _adapter;
        private readonly ILogger<CcPresupuestosInternosService> _logger;

        public CcPresupuestosInternosService(
            CcPresupuestosInternosAdapter adapter,
            ILogger<CcPresupuestosInternosService> logger)
        {
            _adapter = adapter;
            _logger = logger;
        }

        public async Task<IEnumerable<PresupuestoInternoDto>> ObtenerPresupuestosInternosAsync(
            int? periodo = null, string? codigoEmpresa = null, byte? estado = null)
        {
            _logger.LogInformation(
                $"Obtener presupuestos internos - Período: {periodo}, Empresa: {codigoEmpresa}");
            
            return await _adapter.ObtenerPresupuestosInternosAsync(periodo, codigoEmpresa, estado);
        }

        public async Task<PresupuestoInternoDto?> ObtenerPresupuestoInternoDetalleAsync(
            long idPresupuestoInterno)
        {
            _logger.LogInformation($"Obtener detalle presupuesto interno {idPresupuestoInterno}");
            
            var presupuesto = await _adapter.ObtenerPresupuestoInternoDetalleAsync(idPresupuestoInterno);
            
            if (presupuesto != null)
            {
                // Cargar detalles
                presupuesto.Detalles = (await _adapter.ObtenerDetallesPresupuestoInternoAsync(
                    idPresupuestoInterno)).ToList();
            }
            
            return presupuesto;
        }

        public async Task<IEnumerable<DetallePresupuestoInternoDto>> 
            ObtenerDetallesPresupuestoInternoAsync(long idPresupuestoInterno)
        {
            _logger.LogInformation($"Obtener detalles presupuesto interno {idPresupuestoInterno}");
            
            return await _adapter.ObtenerDetallesPresupuestoInternoAsync(idPresupuestoInterno);
        }

        public async Task<long> GuardarPresupuestoInternoAsync(PresupuestoInternoDto presupuesto)
        {
            // Validaciones
            if (presupuesto.MontoTotal < 0)
            {
                _logger.LogWarning("Intento de guardar presupuesto interno con monto negativo");
                throw new ArgumentException("El monto total debe ser positivo");
            }

            if (string.IsNullOrWhiteSpace(presupuesto.CodigoEmpresa))
            {
                throw new ArgumentException("Debe especificar una empresa válida");
            }

            // Validar suma de detalles
            var sumaDetalles = presupuesto.Detalles?.Sum(d => d.MontoAsignado) ?? 0;
            if (sumaDetalles > presupuesto.MontoTotal)
            {
                throw new ArgumentException(
                    $"La suma de detalles ({sumaDetalles:C}) excede el monto total ({presupuesto.MontoTotal:C})");
            }

            _logger.LogInformation(
                $"Guardar presupuesto interno - Empresa: {presupuesto.CodigoEmpresa}, " +
                $"Monto: {presupuesto.MontoTotal}");
            
            var idPresupuesto = await _adapter.GuardarPresupuestoInternoAsync(presupuesto);
            
            // Guardar detalles
            if (presupuesto.Detalles?.Any() == true)
            {
                foreach (var detalle in presupuesto.Detalles)
                {
                    detalle.IdPresupuestoInterno = idPresupuesto;
                    await _adapter.GuardarDetallePresupuestoInternoAsync(detalle);
                }
            }
            
            return idPresupuesto;
        }

        public async Task EliminarPresupuestoInternoAsync(long idPresupuestoInterno)
        {
            _logger.LogInformation($"Eliminar presupuesto interno {idPresupuestoInterno}");
            
            await _adapter.EliminarPresupuestoInternoAsync(idPresupuestoInterno);
        }

        public async Task<IEnumerable<HistoricoPresupuestoInternoDto>> 
            ObtenerHistoricoPresupuestoInternoAsync(long idPresupuestoInterno)
        {
            _logger.LogInformation($"Obtener histórico presupuesto interno {idPresupuestoInterno}");
            
            return await _adapter.ObtenerHistoricoPresupuestoInternoAsync(idPresupuestoInterno);
        }

        public async Task<IEnumerable<ResumenPresupuestoInternoDto>> 
            ObtenerResumenPresupuestosInternosAsync(int? periodo = null)
        {
            _logger.LogInformation($"Obtener resumen presupuestos internos - Período: {periodo}");
            
            return await _adapter.ObtenerResumenPresupuestosInternosAsync(periodo);
        }

        public async Task AprobarPresupuestoInternoAsync(
            long idPresupuestoInterno, string usuarioAprobacion)
        {
            if (string.IsNullOrWhiteSpace(usuarioAprobacion))
            {
                throw new ArgumentException("Debe especificar el usuario de aprobación");
            }

            _logger.LogInformation(
                $"Aprobar presupuesto interno {idPresupuestoInterno} por {usuarioAprobacion}");
            
            await _adapter.AprobarPresupuestoInternoAsync(idPresupuestoInterno, usuarioAprobacion);
        }

        public async Task<byte[]> ExportarPresupuestosInternosExcelAsync(
            IEnumerable<PresupuestoInternoDto> presupuestos)
        {
            _logger.LogInformation($"Exportar {presupuestos.Count()} presupuestos internos a Excel");

            using var workbook = new XLWorkbook();
            var ws = workbook.Worksheets.Add("Presupuestos Internos");

            // Headers
            ws.Cell(1, 1).Value = "ID";
            ws.Cell(1, 2).Value = "Período";
            ws.Cell(1, 3).Value = "Empresa";
            ws.Cell(1, 4).Value = "División";
            ws.Cell(1, 5).Value = "Monto Total";
            ws.Cell(1, 6).Value = "Monto Utilizado";
            ws.Cell(1, 7).Value = "Saldo Disponible";
            ws.Cell(1, 8).Value = "Estado";
            ws.Cell(1, 9).Value = "Fecha Creación";

            // Formato header
            var headerRange = ws.Range(1, 1, 1, 9);
            headerRange.Style.Font.Bold = true;
            headerRange.Style.Fill.BackgroundColor = XLColor.FromHtml("#4472C4");
            headerRange.Style.Font.FontColor = XLColor.White;
            headerRange.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;

            // Datos
            int row = 2;
            foreach (var presupuesto in presupuestos)
            {
                ws.Cell(row, 1).Value = presupuesto.IdPresupuestoInterno;
                ws.Cell(row, 2).Value = presupuesto.Periodo;
                ws.Cell(row, 3).Value = presupuesto.NombreEmpresa ?? "";
                ws.Cell(row, 4).Value = presupuesto.Division ?? "";
                ws.Cell(row, 5).Value = presupuesto.MontoTotal;
                ws.Cell(row, 6).Value = presupuesto.MontoUtilizado;
                ws.Cell(row, 7).Value = presupuesto.SaldoDisponible;
                ws.Cell(row, 8).Value = presupuesto.EstadoNombre ?? "";
                ws.Cell(row, 9).Value = presupuesto.FechaCreacion;

                // Formato moneda
                ws.Cell(row, 5).Style.NumberFormat.Format = "$#,##0.00";
                ws.Cell(row, 6).Style.NumberFormat.Format = "$#,##0.00";
                ws.Cell(row, 7).Style.NumberFormat.Format = "$#,##0.00";

                // Color saldo
                if (presupuesto.SaldoDisponible < presupuesto.MontoTotal * 0.1m)
                    ws.Cell(row, 7).Style.Font.FontColor = XLColor.Red;
                else if (presupuesto.SaldoDisponible > presupuesto.MontoTotal * 0.5m)
                    ws.Cell(row, 7).Style.Font.FontColor = XLColor.Green;

                row++;
            }

            // Auto-ajustar columnas
            ws.Columns().AdjustToContents();

            // Agregar totales
            ws.Cell(row, 4).Value = "TOTALES:";
            ws.Cell(row, 4).Style.Font.Bold = true;
            ws.Cell(row, 5).FormulaA1 = $"=SUM(E2:E{row - 1})";
            ws.Cell(row, 6).FormulaA1 = $"=SUM(F2:F{row - 1})";
            ws.Cell(row, 7).FormulaA1 = $"=SUM(G2:G{row - 1})";

            ws.Cell(row, 5).Style.NumberFormat.Format = "$#,##0.00";
            ws.Cell(row, 6).Style.NumberFormat.Format = "$#,##0.00";
            ws.Cell(row, 7).Style.NumberFormat.Format = "$#,##0.00";
            ws.Cell(row, 5).Style.Font.Bold = true;
            ws.Cell(row, 6).Style.Font.Bold = true;
            ws.Cell(row, 7).Style.Font.Bold = true;

            // Guardar en memoria
            using var stream = new MemoryStream();
            workbook.SaveAs(stream);
            return await Task.FromResult(stream.ToArray());
        }
    }
}
