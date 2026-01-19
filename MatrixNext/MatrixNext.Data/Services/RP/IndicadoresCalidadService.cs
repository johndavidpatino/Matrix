using Dapper;
using MatrixNext.Data.DTOs.RP;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using ClosedXML.Excel;
using System.Data;

namespace MatrixNext.Data.Services.RP;

/// <summary>
/// Implementación de indicadores de calidad
/// SP: REP_Diligenciamiento_Esquema_Analisis, REP_Porcentaje_Diligenciamiento_Brief, REP_Envio_Propuestas_48Horas
/// </summary>
public class IndicadoresCalidadService : IIndicadoresCalidadService
{
    private readonly string _connectionString;
    private readonly ILogger<IndicadoresCalidadService> _logger;

    public IndicadoresCalidadService(IConfiguration configuration, ILogger<IndicadoresCalidadService> logger)
    {
        _connectionString = configuration.GetConnectionString("MatrixConnection") 
            ?? throw new InvalidOperationException("MatrixConnection no configurada");
        _logger = logger;
    }

    /// <summary>
    /// Obtiene indicadores de esquema de análisis
    /// SP: REP_Diligenciamiento_Esquema_Analisis
    /// </summary>
    public async Task<(List<EsquemaAnalisisResumenDto> Resumen, List<EsquemaAnalisisDto> Detalle)> 
        ObtenerEsquemaAnalisisAsync(short? año, short? mes, short? estado, string? usuario)
    {
        try
        {
            using var connection = new SqlConnection(_connectionString);
            
            var parameters = new DynamicParameters();
            parameters.Add("@wAno", año);
            parameters.Add("@wMes", mes);
            parameters.Add("@wEstado", estado);
            parameters.Add("@wUsuario", usuario);

            var detalle = (await connection.QueryAsync<EsquemaAnalisisDto>(
                "REP_Diligenciamiento_Esquema_Analisis",
                parameters,
                commandType: CommandType.StoredProcedure,
                commandTimeout: 120)).ToList();

            // Agrupar para resumen
            var resumen = detalle
                .GroupBy(x => new { x.GerenteCuentas, x.MesPDC, x.AñoPDC })
                .Select(g => new EsquemaAnalisisResumenDto
                {
                    Gerente = g.Key.GerenteCuentas,
                    Mes = g.Key.MesPDC,
                    Año = g.Key.AñoPDC,
                    Base = g.Count(),
                    Cumplimiento = g.Count(x => x.TieneEsquemaAnalisis == "Sí"),
                    Porcentaje = g.Count() > 0 
                        ? ((double)g.Count(x => x.TieneEsquemaAnalisis == "Sí") / g.Count()).ToString("P1") 
                        : "0%"
                })
                .OrderBy(x => x.Gerente)
                .ToList();

            _logger.LogInformation("Esquema análisis obtenido: {Count} registros", detalle.Count);
            return (resumen, detalle);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al obtener esquema de análisis");
            throw;
        }
    }

    /// <summary>
    /// Obtiene indicadores de diligenciamiento de Brief
    /// SP: REP_Porcentaje_Diligenciamiento_Brief
    /// </summary>
    public async Task<(List<DiligenciamientoBriefResumenDto> Resumen, List<DiligenciamientoBriefDto> Detalle)> 
        ObtenerDiligenciamientoBriefAsync(short? año, short? mes, string? usuario)
    {
        try
        {
            using var connection = new SqlConnection(_connectionString);
            
            var parameters = new DynamicParameters();
            parameters.Add("@wAno", año);
            parameters.Add("@wMes", mes);
            parameters.Add("@wUsuario", usuario);

            var rawDetalle = await connection.QueryAsync<dynamic>(
                "REP_Porcentaje_Diligenciamiento_Brief",
                parameters,
                commandType: CommandType.StoredProcedure,
                commandTimeout: 120);

            var detalle = rawDetalle.Select(x => new DiligenciamientoBriefDto
            {
                IdBrief = x.IdBrief,
                PorcentajeDiligenciamiento = ((double)(x.PorcentajeDiligenciamiento ?? 0) / 100).ToString("P1"),
                FechaCreacionBrief = x.FechaCreacionBrief,
                Año = x.Año,
                Mes = x.Mes,
                Usuario = x.Usuario
            }).ToList();

            // Agrupar para resumen
            var resumen = rawDetalle
                .GroupBy(x => new { Usuario = (string)x.Usuario, Mes = (int?)x.Mes, Año = (int?)x.Año })
                .Select(g => new DiligenciamientoBriefResumenDto
                {
                    Gerente = g.Key.Usuario,
                    Mes = g.Key.Mes,
                    Año = g.Key.Año,
                    Base = g.Count(),
                    Porcentaje = g.Count() > 0 
                        ? (g.Average(x => (double)(x.PorcentajeDiligenciamiento ?? 0)) / 100).ToString("P1") 
                        : "0%"
                })
                .OrderBy(x => x.Gerente)
                .ToList();

            _logger.LogInformation("Diligenciamiento Brief obtenido: {Count} registros", detalle.Count);
            return (resumen, detalle);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al obtener diligenciamiento Brief");
            throw;
        }
    }

    /// <summary>
    /// Obtiene indicadores de envío propuestas 48 horas
    /// SP: REP_Envio_Propuestas_48Horas
    /// </summary>
    public async Task<(List<EnvioPropuestas48HorasResumenDto> Resumen, List<EnvioPropuestas48HorasDto> Detalle)> 
        ObtenerEnvioPropuestas48HorasAsync(short? año, short? mes, short? estado, string? usuario)
    {
        try
        {
            using var connection = new SqlConnection(_connectionString);
            
            var parameters = new DynamicParameters();
            parameters.Add("@wAno", año);
            parameters.Add("@wMes", mes);
            parameters.Add("@wEstado", estado);
            parameters.Add("@wUsuario", usuario);

            var detalle = (await connection.QueryAsync<EnvioPropuestas48HorasDto>(
                "REP_Envio_Propuestas_48Horas",
                parameters,
                commandType: CommandType.StoredProcedure,
                commandTimeout: 120)).ToList();

            // Agrupar para resumen
            var resumen = detalle
                .GroupBy(x => new { x.GerenteCuentas, x.MesCreacionBrief, x.AnoCreacionBrief })
                .Select(g => new EnvioPropuestas48HorasResumenDto
                {
                    Gerente = g.Key.GerenteCuentas,
                    Mes = g.Key.MesCreacionBrief,
                    Año = g.Key.AnoCreacionBrief,
                    Base = g.Count(),
                    Cumplen = g.Count(x => x.CumpleEnvio48Horas == "Sí"),
                    Porcentaje = g.Count() > 0 
                        ? ((double)g.Count(x => x.CumpleEnvio48Horas == "Sí") / g.Count()).ToString("P1") 
                        : "0%"
                })
                .OrderBy(x => x.Gerente)
                .ToList();

            _logger.LogInformation("Propuestas 48h obtenido: {Count} registros", detalle.Count);
            return (resumen, detalle);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al obtener propuestas 48 horas");
            throw;
        }
    }

    /// <summary>
    /// Obtiene años disponibles para dropdown
    /// </summary>
    public async Task<List<int>> ObtenerAñosDisponiblesAsync()
    {
        // Retorna últimos 5 años
        var añoActual = DateTime.Now.Year;
        return await Task.FromResult(Enumerable.Range(añoActual - 4, 5).OrderByDescending(x => x).ToList());
    }

    /// <summary>
    /// Obtiene usuarios (gerentes) disponibles para dropdown
    /// </summary>
    public async Task<List<string>> ObtenerUsuariosDisponiblesAsync(int tipoReporte)
    {
        try
        {
            using var connection = new SqlConnection(_connectionString);
            
            // Obtener usuarios únicos del año actual
            var año = (short)DateTime.Now.Year;
            
            IEnumerable<string> usuarios = tipoReporte switch
            {
                1 => (await ObtenerEsquemaAnalisisAsync(año, null, null, null)).Detalle
                    .Select(x => x.GerenteCuentas ?? "")
                    .Where(x => !string.IsNullOrEmpty(x))
                    .Distinct(),
                2 => (await ObtenerDiligenciamientoBriefAsync(año, null, null)).Detalle
                    .Select(x => x.Usuario ?? "")
                    .Where(x => !string.IsNullOrEmpty(x))
                    .Distinct(),
                3 => (await ObtenerEnvioPropuestas48HorasAsync(año, null, null, null)).Detalle
                    .Select(x => x.GerenteCuentas ?? "")
                    .Where(x => !string.IsNullOrEmpty(x))
                    .Distinct(),
                _ => Enumerable.Empty<string>()
            };

            return usuarios.OrderBy(x => x).ToList();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al obtener usuarios disponibles");
            return new List<string>();
        }
    }

    /// <summary>
    /// Prepara el ViewModel completo para la vista
    /// </summary>
    public async Task<IndicadoresCalidadViewModel> PrepararViewModelAsync(IndicadoresCalidadFiltrosDto filtros)
    {
        var viewModel = new IndicadoresCalidadViewModel
        {
            AñoSeleccionado = filtros.Año ?? (short)DateTime.Now.Year,
            MesSeleccionado = filtros.Mes,
            TipoReporteSeleccionado = filtros.TipoReporte,
            AñosDisponibles = await ObtenerAñosDisponiblesAsync()
        };

        switch (filtros.TipoReporte)
        {
            case 1: // Esquema Análisis
                var (resumenEsquema, detalleEsquema) = await ObtenerEsquemaAnalisisAsync(
                    filtros.Año, filtros.Mes, filtros.Estado, filtros.Usuario);
                viewModel.ResumenEsquema = resumenEsquema;
                viewModel.DetalleEsquema = detalleEsquema;
                viewModel.UsuariosDisponibles = detalleEsquema
                    .Select(x => x.GerenteCuentas ?? "")
                    .Where(x => !string.IsNullOrEmpty(x))
                    .Distinct()
                    .OrderBy(x => x)
                    .ToList();
                break;
                
            case 2: // Brief
                var (resumenBrief, detalleBrief) = await ObtenerDiligenciamientoBriefAsync(
                    filtros.Año, filtros.Mes, filtros.Usuario);
                viewModel.ResumenBrief = resumenBrief;
                viewModel.DetalleBrief = detalleBrief;
                viewModel.UsuariosDisponibles = detalleBrief
                    .Select(x => x.Usuario ?? "")
                    .Where(x => !string.IsNullOrEmpty(x))
                    .Distinct()
                    .OrderBy(x => x)
                    .ToList();
                break;
                
            case 3: // Propuestas 48h
                var (resumenProp, detalleProp) = await ObtenerEnvioPropuestas48HorasAsync(
                    filtros.Año, filtros.Mes, filtros.Estado, filtros.Usuario);
                viewModel.ResumenPropuestas = resumenProp;
                viewModel.DetallePropuestas = detalleProp;
                viewModel.UsuariosDisponibles = detalleProp
                    .Select(x => x.GerenteCuentas ?? "")
                    .Where(x => !string.IsNullOrEmpty(x))
                    .Distinct()
                    .OrderBy(x => x)
                    .ToList();
                break;
        }

        return viewModel;
    }

    /// <summary>
    /// Exporta indicadores a Excel
    /// </summary>
    public async Task<byte[]> ExportarExcelAsync(IndicadoresCalidadFiltrosDto filtros)
    {
        var viewModel = await PrepararViewModelAsync(filtros);
        
        using var workbook = new XLWorkbook();
        
        switch (filtros.TipoReporte)
        {
            case 1:
                var wsResumen1 = workbook.Worksheets.Add("Resumen");
                wsResumen1.Cell(1, 1).InsertTable(viewModel.ResumenEsquema);
                var wsDetalle1 = workbook.Worksheets.Add("Detalle");
                wsDetalle1.Cell(1, 1).InsertTable(viewModel.DetalleEsquema);
                break;
                
            case 2:
                var wsResumen2 = workbook.Worksheets.Add("Resumen");
                wsResumen2.Cell(1, 1).InsertTable(viewModel.ResumenBrief);
                var wsDetalle2 = workbook.Worksheets.Add("Detalle");
                wsDetalle2.Cell(1, 1).InsertTable(viewModel.DetalleBrief);
                break;
                
            case 3:
                var wsResumen3 = workbook.Worksheets.Add("Resumen");
                wsResumen3.Cell(1, 1).InsertTable(viewModel.ResumenPropuestas);
                var wsDetalle3 = workbook.Worksheets.Add("Detalle");
                wsDetalle3.Cell(1, 1).InsertTable(viewModel.DetallePropuestas);
                break;
        }
        
        foreach (var ws in workbook.Worksheets)
        {
            ws.Columns().AdjustToContents();
        }
        
        using var stream = new MemoryStream();
        workbook.SaveAs(stream);
        return stream.ToArray();
    }
}
