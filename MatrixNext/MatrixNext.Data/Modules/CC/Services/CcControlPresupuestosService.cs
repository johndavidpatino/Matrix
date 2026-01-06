using ClosedXML.Excel;
using MatrixNext.Data.Modules.CC.Adapters;
using MatrixNext.Data.Modules.CC.DTOs.ControlPresupuestos;
using Microsoft.Extensions.Logging;

namespace MatrixNext.Data.Modules.CC.Services
{
    /// <summary>
    /// Interface para servicios de Control de Presupuestos
    /// </summary>
    public interface ICcControlPresupuestosService
    {
        // Presupuestos
        Task<IEnumerable<PresupuestoDto>> ObtenerPresupuestosAsync(
            int? periodo = null, long? idTrabajo = null, byte? estado = null);
        
        Task<IEnumerable<DetallePresupuestoDto>> ObtenerDetallePresupuestoAsync(
            long idPresupuesto);
        
        Task<long> GuardarPresupuestoAsync(PresupuestoDto presupuesto);
        Task EliminarPresupuestoAsync(long idPresupuesto);
        
        // Export
        Task<byte[]> ExportarPresupuestosExcelAsync(
            IEnumerable<PresupuestoDto> presupuestos);
        
        // Verificación
        Task<IEnumerable<VerificacionPresupuestoDto>> 
            ObtenerVerificacionPresupuestosAsync(int? periodo = null);
        
        // Nómina y Distribución
        Task<IEnumerable<NominaDistribucionDto>> 
            ObtenerNominaDistribucionAsync(int periodo, long? idEmpleado = null);
        
        Task<long> GuardarDistribucionCostoAsync(
            DistribucionPorCentroDto distribucion);
        
        // Asignación
        Task<IEnumerable<AsignacionPresupuestoDto>> 
            ObtenerActividadesPresupuestadasAsync(long idPresupuesto);
        
        Task<long> GuardarAsignacionPresupuestoAsync(
            AsignacionPresupuestoDto asignacion);
    }

    /// <summary>
    /// Servicio de Control de Presupuestos
    /// </summary>
    public class CcControlPresupuestosService : ICcControlPresupuestosService
    {
        private readonly CcControlPresupuestosAdapter _adapter;
        private readonly ILogger<CcControlPresupuestosService> _logger;

        public CcControlPresupuestosService(
            CcControlPresupuestosAdapter adapter,
            ILogger<CcControlPresupuestosService> logger)
        {
            _adapter = adapter;
            _logger = logger;
        }

        public async Task<IEnumerable<PresupuestoDto>> ObtenerPresupuestosAsync(
            int? periodo = null, long? idTrabajo = null, byte? estado = null)
        {
            _logger.LogInformation(
                $"Obtener presupuestos - Período: {periodo}, Trabajo: {idTrabajo}, Estado: {estado}");
            
            return await _adapter.ObtenerPresupuestosAsync(periodo, idTrabajo, estado);
        }

        public async Task<IEnumerable<DetallePresupuestoDto>> 
            ObtenerDetallePresupuestoAsync(long idPresupuesto)
        {
            _logger.LogInformation($"Obtener detalles presupuesto {idPresupuesto}");
            
            return await _adapter.ObtenerDetallePresupuestoAsync(idPresupuesto);
        }

        public async Task<long> GuardarPresupuestoAsync(PresupuestoDto presupuesto)
        {
            // Validaciones
            if (presupuesto.MontoPresupuesto < 0)
            {
                _logger.LogWarning("Intento de guardar presupuesto con monto negativo");
                throw new ArgumentException("El monto del presupuesto debe ser positivo");
            }

            if (presupuesto.IdTrabajo <= 0)
            {
                throw new ArgumentException("Debe especificar un trabajo válido");
            }

            // Validar suma de detalles
            var sumaDetalles = presupuesto.Detalles?.Sum(d => d.Subtotal) ?? 0;
            if (sumaDetalles != presupuesto.MontoPresupuesto)
            {
                _logger.LogWarning(
                    $"Advertencia: suma de detalles ({sumaDetalles}) " +
                    $"no coincide con monto presupuesto ({presupuesto.MontoPresupuesto})");
            }

            _logger.LogInformation(
                $"Guardar presupuesto - Trabajo: {presupuesto.IdTrabajo}, " +
                $"Monto: {presupuesto.MontoPresupuesto}");
            
            var idPresupuesto = await _adapter.GuardarPresupuestoAsync(presupuesto);
            
            // Guardar detalles
            if (presupuesto.Detalles?.Any() == true)
            {
                foreach (var detalle in presupuesto.Detalles)
                {
                    detalle.IdPresupuesto = idPresupuesto;
                    await _adapter.GuardarDetallePresupuestoAsync(detalle);
                }
            }
            
            return idPresupuesto;
        }

        public async Task EliminarPresupuestoAsync(long idPresupuesto)
        {
            _logger.LogInformation($"Eliminar presupuesto {idPresupuesto}");
            
            await _adapter.EliminarPresupuestoAsync(idPresupuesto);
        }

        public async Task<IEnumerable<VerificacionPresupuestoDto>> 
            ObtenerVerificacionPresupuestosAsync(int? periodo = null)
        {
            _logger.LogInformation($"Obtener verificación presupuestos - Período: {periodo}");
            
            return await _adapter.ObtenerVerificacionPresupuestosAsync(periodo);
        }

        public async Task<IEnumerable<NominaDistribucionDto>> 
            ObtenerNominaDistribucionAsync(int periodo, long? idEmpleado = null)
        {
            _logger.LogInformation(
                $"Obtener nómina distribución - Período: {periodo}, Empleado: {idEmpleado}");
            
            return await _adapter.ObtenerNominaDistribucionAsync(periodo, idEmpleado);
        }

        public async Task<long> GuardarDistribucionCostoAsync(
            DistribucionPorCentroDto distribucion)
        {
            if (distribucion.PorcentajeDistribucion < 0 || 
                distribucion.PorcentajeDistribucion > 100)
            {
                throw new ArgumentException(
                    "El porcentaje de distribución debe estar entre 0 y 100");
            }

            _logger.LogInformation(
                $"Guardar distribución costo - Centro: {distribucion.IdCentroCosto}, " +
                $"Porcentaje: {distribucion.PorcentajeDistribucion}%");
            
            return await _adapter.GuardarDistribucionCostoAsync(distribucion);
        }

        public async Task<IEnumerable<AsignacionPresupuestoDto>> 
            ObtenerActividadesPresupuestadasAsync(long idPresupuesto)
        {
            _logger.LogInformation(
                $"Obtener actividades presupuestadas - Presupuesto: {idPresupuesto}");
            
            return await _adapter.ObtenerActividadesPresupuestadasAsync(idPresupuesto);
        }

        public async Task<long> GuardarAsignacionPresupuestoAsync(
            AsignacionPresupuestoDto asignacion)
        {
            if (asignacion.MontoAsignado < 0)
            {
                throw new ArgumentException("El monto asignado debe ser positivo");
            }

            if (asignacion.MontoAsignado > asignacion.MontoAsignado)
            {
                throw new ArgumentException("No se puede sobreasignar presupuesto");
            }

            _logger.LogInformation(
                $"Guardar asignación presupuesto - Actividad: {asignacion.IdActividad}, " +
                $"Monto: {asignacion.MontoAsignado}");
            
            return await _adapter.GuardarAsignacionPresupuestoAsync(asignacion);
        }

        public async Task<byte[]> ExportarPresupuestosExcelAsync(
            IEnumerable<PresupuestoDto> presupuestos)
        {
            _logger.LogInformation($"Exportar {presupuestos.Count()} presupuestos a Excel");

            using var workbook = new XLWorkbook();
            var ws = workbook.Worksheets.Add("Presupuestos");

            // Headers
            ws.Cell(1, 1).Value = "ID";
            ws.Cell(1, 2).Value = "Período";
            ws.Cell(1, 3).Value = "Código Trabajo";
            ws.Cell(1, 4).Value = "Nombre Trabajo";
            ws.Cell(1, 5).Value = "Monto Presupuesto";
            ws.Cell(1, 6).Value = "Monto Realizado";
            ws.Cell(1, 7).Value = "Varianza";
            ws.Cell(1, 8).Value = "Estado";
            ws.Cell(1, 9).Value = "Fecha Registro";

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
                ws.Cell(row, 1).Value = presupuesto.IdPresupuesto;
                ws.Cell(row, 2).Value = presupuesto.Periodo;
                ws.Cell(row, 3).Value = presupuesto.CodigoTrabajo ?? "";
                ws.Cell(row, 4).Value = presupuesto.NombreTrabajo ?? "";
                ws.Cell(row, 5).Value = presupuesto.MontoPresupuesto;
                ws.Cell(row, 6).Value = presupuesto.MontoRealizado;
                ws.Cell(row, 7).Value = presupuesto.Varianza;
                ws.Cell(row, 8).Value = presupuesto.EstadoNombre ?? "";
                ws.Cell(row, 9).Value = presupuesto.FechaRegistro;

                // Formato moneda
                ws.Cell(row, 5).Style.NumberFormat.Format = "$#,##0.00";
                ws.Cell(row, 6).Style.NumberFormat.Format = "$#,##0.00";
                ws.Cell(row, 7).Style.NumberFormat.Format = "$#,##0.00";

                // Color varianza
                if (presupuesto.Varianza > 0)
                    ws.Cell(row, 7).Style.Font.FontColor = XLColor.Red;
                else if (presupuesto.Varianza < 0)
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
            return stream.ToArray();
        }
    }
}
