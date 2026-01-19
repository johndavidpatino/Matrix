using Dapper;
using MatrixNext.Data.DTOs.INV;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using ClosedXML.Excel;
using System.Data;

namespace MatrixNext.Data.Services.INV;

/// <summary>
/// Implementación de reportes de inventario
/// SP: INV_ReporteLegalizaciones, INV_ReporteRemanente
/// </summary>
public class ReportesInvService : IReportesInvService
{
    private readonly string _connectionString;
    private readonly ILogger<ReportesInvService> _logger;

    public ReportesInvService(IConfiguration configuration, ILogger<ReportesInvService> logger)
    {
        _connectionString = configuration.GetConnectionString("MatrixConnection") 
            ?? throw new InvalidOperationException("MatrixConnection no configurada");
        _logger = logger;
    }

    /// <summary>
    /// Obtiene reporte de legalizaciones con filtros
    /// SP: INV_ReporteLegalizaciones
    /// </summary>
    public async Task<IEnumerable<ReporteLegalizacionDto>> ObtenerReporteLegalizacionesAsync(ReporteLegalizacionFiltrosDto filtros)
    {
        try
        {
            using var connection = new SqlConnection(_connectionString);
            
            var parameters = new DynamicParameters();
            parameters.Add("@FechaInicio", filtros.FechaInicio);
            parameters.Add("@FechaFin", filtros.FechaFin);
            parameters.Add("@UsuarioAsignado", filtros.UsuarioAsignado);
            parameters.Add("@Articulo", filtros.Articulo);
            parameters.Add("@BU", filtros.BU);
            parameters.Add("@JobBookCodigo", filtros.JobBookCodigo);
            parameters.Add("@TodosCampos", filtros.TodosCampos);

            var result = await connection.QueryAsync<ReporteLegalizacionDto>(
                "INV_ReporteLegalizaciones",
                parameters,
                commandType: CommandType.StoredProcedure,
                commandTimeout: 120);

            _logger.LogInformation("Reporte legalizaciones obtenido: {Count} registros", result.Count());
            return result;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al obtener reporte de legalizaciones");
            throw;
        }
    }

    /// <summary>
    /// Obtiene reporte de remanente con filtros
    /// SP: INV_ReporteRemanente
    /// </summary>
    public async Task<IEnumerable<ReporteRemanenteDto>> ObtenerReporteRemanenteAsync(ReporteRemanenteFiltrosDto filtros)
    {
        try
        {
            using var connection = new SqlConnection(_connectionString);
            
            var parameters = new DynamicParameters();
            parameters.Add("@IdConsumible", filtros.IdConsumible);
            parameters.Add("@Articulo", filtros.Articulo);
            parameters.Add("@TipoProducto", filtros.TipoProducto);
            parameters.Add("@JobBook", filtros.JobBook);

            var result = await connection.QueryAsync<ReporteRemanenteDto>(
                "INV_ReporteRemanente",
                parameters,
                commandType: CommandType.StoredProcedure,
                commandTimeout: 120);

            _logger.LogInformation("Reporte remanente obtenido: {Count} registros", result.Count());
            return result;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al obtener reporte de remanente");
            throw;
        }
    }

    /// <summary>
    /// Obtiene lista de Business Units para dropdown
    /// </summary>
    public async Task<IEnumerable<(int Id, string Nombre)>> ObtenerBUsAsync()
    {
        try
        {
            using var connection = new SqlConnection(_connectionString);
            
            var result = await connection.QueryAsync<(int Id, string Nombre)>(
                "SELECT Id, Nombre FROM INV_BU ORDER BY Nombre",
                commandType: CommandType.Text);

            return result;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al obtener BUs");
            return Enumerable.Empty<(int, string)>();
        }
    }

    /// <summary>
    /// Obtiene tipos de artículo para dropdown
    /// </summary>
    public async Task<IEnumerable<(long Id, string Nombre)>> ObtenerTiposArticuloAsync()
    {
        try
        {
            using var connection = new SqlConnection(_connectionString);
            
            // Solo los tipos que aplican para consumibles (7=Obsequios, 8=Bonos, 9=Transporte)
            var result = await connection.QueryAsync<(long Id, string Nombre)>(
                "SELECT Id, Articulo as Nombre FROM INV_Articulo WHERE Id IN (7, 8, 9) ORDER BY Articulo",
                commandType: CommandType.Text);

            return result;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al obtener tipos de artículo");
            return Enumerable.Empty<(long, string)>();
        }
    }

    /// <summary>
    /// Obtiene tipos de producto para dropdown
    /// </summary>
    public async Task<IEnumerable<(long Id, string Nombre)>> ObtenerTiposProductoAsync()
    {
        try
        {
            using var connection = new SqlConnection(_connectionString);
            
            var result = await connection.QueryAsync<(long Id, string Nombre)>(
                "SELECT Id, TipoProducto as Nombre FROM INV_TipoProducto ORDER BY TipoProducto",
                commandType: CommandType.Text);

            return result;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al obtener tipos de producto");
            return Enumerable.Empty<(long, string)>();
        }
    }

    /// <summary>
    /// Exporta reporte de legalizaciones a Excel
    /// </summary>
    public async Task<byte[]> ExportarLegalizacionesExcelAsync(ReporteLegalizacionFiltrosDto filtros)
    {
        var datos = await ObtenerReporteLegalizacionesAsync(filtros);
        
        using var workbook = new XLWorkbook();
        var ws = workbook.Worksheets.Add("Legalizaciones");
        
        // Encabezados
        var headers = new[] { "Id", "Artículo", "TipoProducto", "Producto", "TipoBono", 
            "FechaEntrega", "UsuarioAsignado", "Cédula", "TipoCargo", "Unidades", 
            "ValorCarrera", "ValorTotal", "JobBookCodigo", "JobBookNombre", "BU", 
            "Observación", "Firmas", "Devoluciones", "NotasCredito", "DescuentoNomina", 
            "Legalizado", "FechaLegalizacion" };
        
        for (int i = 0; i < headers.Length; i++)
        {
            ws.Cell(1, i + 1).Value = headers[i];
            ws.Cell(1, i + 1).Style.Font.Bold = true;
        }
        
        // Datos
        int row = 2;
        foreach (var item in datos)
        {
            ws.Cell(row, 1).Value = item.Id;
            ws.Cell(row, 2).Value = item.Articulo;
            ws.Cell(row, 3).Value = item.TipoProducto;
            ws.Cell(row, 4).Value = item.Producto;
            ws.Cell(row, 5).Value = item.TipoBono;
            ws.Cell(row, 6).Value = item.FechaEntrega;
            ws.Cell(row, 7).Value = item.UsuarioAsignado;
            ws.Cell(row, 8).Value = item.Cedula;
            ws.Cell(row, 9).Value = item.TipoCargo;
            ws.Cell(row, 10).Value = item.Unidades;
            ws.Cell(row, 11).Value = item.ValorCarrera;
            ws.Cell(row, 12).Value = item.ValorTotal;
            ws.Cell(row, 13).Value = item.JobBookCodigo;
            ws.Cell(row, 14).Value = item.JobBookNombre;
            ws.Cell(row, 15).Value = item.BU;
            ws.Cell(row, 16).Value = item.Observacion;
            ws.Cell(row, 17).Value = item.Firmas;
            ws.Cell(row, 18).Value = item.Devoluciones;
            ws.Cell(row, 19).Value = item.NotasCredito;
            ws.Cell(row, 20).Value = item.DescuentoNomina;
            ws.Cell(row, 21).Value = item.Legalizado == true ? "Sí" : "No";
            ws.Cell(row, 22).Value = item.FechaLegalizacion;
            row++;
        }
        
        ws.Columns().AdjustToContents();
        _logger.LogInformation("Excel de legalizaciones generado: {Rows} filas", row - 2);
        
        using var stream = new MemoryStream();
        workbook.SaveAs(stream);
        return stream.ToArray();
    }

    /// <summary>
    /// Exporta reporte de remanente a Excel
    /// </summary>
    public async Task<byte[]> ExportarRemanenteExcelAsync(ReporteRemanenteFiltrosDto filtros)
    {
        var datos = await ObtenerReporteRemanenteAsync(filtros);
        
        using var workbook = new XLWorkbook();
        var ws = workbook.Worksheets.Add("Remanente");
        
        // Encabezados
        var headers = new[] { "IdConsumible", "Artículo", "TipoProducto", "Producto", 
            "TipoObsequio", "EstadoProducto", "TipoBono", "Fecha", 
            "JobBookCodigo", "JobBookNombre", "Total", "Disponible" };
        
        for (int i = 0; i < headers.Length; i++)
        {
            ws.Cell(1, i + 1).Value = headers[i];
            ws.Cell(1, i + 1).Style.Font.Bold = true;
        }
        
        // Datos
        int row = 2;
        foreach (var item in datos)
        {
            ws.Cell(row, 1).Value = item.IdConsumible;
            ws.Cell(row, 2).Value = item.Articulo;
            ws.Cell(row, 3).Value = item.TipoProducto;
            ws.Cell(row, 4).Value = item.Producto;
            ws.Cell(row, 5).Value = item.TipoObsequio;
            ws.Cell(row, 6).Value = item.EstadoProducto;
            ws.Cell(row, 7).Value = item.TipoBono;
            ws.Cell(row, 8).Value = item.Fecha;
            ws.Cell(row, 9).Value = item.JobBookCodigo;
            ws.Cell(row, 10).Value = item.JobBookNombre;
            ws.Cell(row, 11).Value = item.Total;
            ws.Cell(row, 12).Value = item.Disponible;
            row++;
        }
        
        ws.Columns().AdjustToContents();
        _logger.LogInformation("Excel de remanente generado: {Rows} filas", row - 2);
        
        using var stream = new MemoryStream();
        workbook.SaveAs(stream);
        return stream.ToArray();
    }
}
