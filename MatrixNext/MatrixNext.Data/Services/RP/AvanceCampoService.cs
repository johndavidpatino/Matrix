using Dapper;
using MatrixNext.Data.DTOs.RP;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using ClosedXML.Excel;
using System.Data;

namespace MatrixNext.Data.Services.RP;

/// <summary>
/// Implementación de avance de campo
/// SP: REP_AvanceCampoGeneral, REP_AvanceCampoxCiudad, REP_AvancePorcentualAreas, etc.
/// </summary>
public class AvanceCampoService : IAvanceCampoService
{
    private readonly string _connectionString;
    private readonly ILogger<AvanceCampoService> _logger;

    public AvanceCampoService(IConfiguration configuration, ILogger<AvanceCampoService> logger)
    {
        _connectionString = configuration.GetConnectionString("MatrixConnection") 
            ?? throw new InvalidOperationException("MatrixConnection no configurada");
        _logger = logger;
    }

    /// <summary>
    /// Obtiene avance general de campo para un trabajo
    /// SP: REP_AvanceCampoGeneral
    /// </summary>
    public async Task<AvanceCampoGeneralDto?> ObtenerAvanceGeneralAsync(long trabajoId)
    {
        try
        {
            using var connection = new SqlConnection(_connectionString);
            
            var result = await connection.QueryFirstOrDefaultAsync<AvanceCampoGeneralDto>(
                "REP_AvanceCampoGeneral",
                new { TrabajoId = trabajoId },
                commandType: CommandType.StoredProcedure,
                commandTimeout: 60);

            if (result != null)
            {
                result.TrabajoId = trabajoId;
            }

            return result;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al obtener avance general. TrabajoId: {TrabajoId}", trabajoId);
            throw;
        }
    }

    /// <summary>
    /// Obtiene avance por ciudad
    /// SP: REP_AvanceCampoxCiudad
    /// </summary>
    public async Task<List<AvanceCampoCiudadDto>> ObtenerAvancePorCiudadAsync(long trabajoId)
    {
        try
        {
            using var connection = new SqlConnection(_connectionString);
            
            var result = await connection.QueryAsync<AvanceCampoCiudadDto>(
                "REP_AvanceCampoxCiudad",
                new { TrabajoId = trabajoId },
                commandType: CommandType.StoredProcedure,
                commandTimeout: 60);

            return result.ToList();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al obtener avance por ciudad. TrabajoId: {TrabajoId}", trabajoId);
            throw;
        }
    }

    /// <summary>
    /// Obtiene avance porcentual por áreas
    /// SP: REP_AvancePorcentualAreas
    /// </summary>
    public async Task<List<AvanceAreaDto>> ObtenerAvancePorAreasAsync(long trabajoId)
    {
        try
        {
            using var connection = new SqlConnection(_connectionString);
            
            var result = await connection.QueryAsync<AvanceAreaDto>(
                "REP_AvancePorcentualAreas",
                new { TrabajoId = trabajoId },
                commandType: CommandType.StoredProcedure,
                commandTimeout: 60);

            return result.ToList();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al obtener avance por áreas. TrabajoId: {TrabajoId}", trabajoId);
            throw;
        }
    }

    /// <summary>
    /// Obtiene remanentes por áreas
    /// SP: REP_AvanceAreasRemanentes
    /// </summary>
    public async Task<List<AvanceRemanenteDto>> ObtenerRemanentesAsync(long trabajoId)
    {
        try
        {
            using var connection = new SqlConnection(_connectionString);
            
            var result = await connection.QueryAsync<AvanceRemanenteDto>(
                "REP_AvanceAreasRemanentes",
                new { TrabajoId = trabajoId },
                commandType: CommandType.StoredProcedure,
                commandTimeout: 60);

            return result.ToList();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al obtener remanentes. TrabajoId: {TrabajoId}", trabajoId);
            throw;
        }
    }

    /// <summary>
    /// Obtiene matriz de cumplimiento
    /// SP: REP_MatrizEstimacionCumplimiento
    /// </summary>
    public async Task<List<MatrizCumplimientoDto>> ObtenerMatrizCumplimientoAsync(long trabajoId)
    {
        try
        {
            using var connection = new SqlConnection(_connectionString);
            
            var result = await connection.QueryAsync<MatrizCumplimientoDto>(
                "REP_MatrizEstimacionCumplimiento",
                new { TrabajoId = trabajoId },
                commandType: CommandType.StoredProcedure,
                commandTimeout: 60);

            return result.ToList();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al obtener matriz cumplimiento. TrabajoId: {TrabajoId}", trabajoId);
            throw;
        }
    }

    /// <summary>
    /// Obtiene encuestas anuladas
    /// </summary>
    public async Task<List<EncuestaAnuladaDto>> ObtenerEncuestasAnuladasAsync(long trabajoId)
    {
        try
        {
            using var connection = new SqlConnection(_connectionString);
            
            // Consulta directa a tabla de encuestas anuladas
            var result = await connection.QueryAsync<EncuestaAnuladaDto>(
                @"SELECT ea.Id, c.NombreCiudad as Ciudad, p.NombreCompleto as Encuestador, 
                         ea.Fecha, ea.Motivo
                  FROM OP_EncuestasAnuladas ea
                  LEFT JOIN US_Ciudad c ON ea.IdCiudad = c.Id
                  LEFT JOIN US_Personas p ON ea.IdEncuestador = p.Id
                  WHERE ea.TrabajoId = @TrabajoId
                  ORDER BY ea.Fecha DESC",
                new { TrabajoId = trabajoId },
                commandType: CommandType.Text,
                commandTimeout: 60);

            return result.ToList();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al obtener encuestas anuladas. TrabajoId: {TrabajoId}", trabajoId);
            return new List<EncuestaAnuladaDto>();
        }
    }

    /// <summary>
    /// Verifica si el trabajo tiene datos de estimación
    /// </summary>
    public async Task<bool> TieneDatosEstimacionAsync(long trabajoId)
    {
        try
        {
            using var connection = new SqlConnection(_connectionString);
            
            var count = await connection.ExecuteScalarAsync<int>(
                @"SELECT COUNT(*) FROM OP_EstimacionesProduccionCiudad 
                  WHERE TrabajoId = @TrabajoId",
                new { TrabajoId = trabajoId },
                commandType: CommandType.Text);

            return count > 0;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al verificar datos estimación. TrabajoId: {TrabajoId}", trabajoId);
            return false;
        }
    }

    /// <summary>
    /// Prepara el ViewModel completo para la vista
    /// </summary>
    public async Task<AvanceCampoViewModel> PrepararViewModelAsync(long trabajoId)
    {
        var viewModel = new AvanceCampoViewModel
        {
            TrabajoId = trabajoId,
            IdTrabajoSeleccionado = trabajoId,
            TieneDatos = await TieneDatosEstimacionAsync(trabajoId)
        };

        // Obtener trabajos disponibles
        viewModel.TrabajosDisponibles = await ObtenerTrabajosDisponiblesAsync();

        if (!viewModel.TieneDatos || trabajoId == 0)
        {
            return viewModel;
        }

        // Cargar todos los datos en paralelo
        var taskGeneral = ObtenerAvanceGeneralAsync(trabajoId);
        var taskCiudad = ObtenerAvancePorCiudadAsync(trabajoId);
        var taskAreas = ObtenerAvancePorAreasAsync(trabajoId);
        var taskRemanentes = ObtenerRemanentesAsync(trabajoId);
        var taskMatriz = ObtenerMatrizCumplimientoAsync(trabajoId);
        var taskAnuladas = ObtenerEncuestasAnuladasAsync(trabajoId);

        await Task.WhenAll(taskGeneral, taskCiudad, taskAreas, taskRemanentes, taskMatriz, taskAnuladas);

        var avanceGeneral = await taskGeneral;
        viewModel.AvanceGeneral = avanceGeneral != null ? new List<AvanceCampoGeneralDto> { avanceGeneral } : new List<AvanceCampoGeneralDto>();
        viewModel.AvancePorCiudad = await taskCiudad;
        viewModel.AvancePorAreas = await taskAreas;
        viewModel.Remanentes = await taskRemanentes;
        viewModel.MatrizCumplimiento = await taskMatriz;
        viewModel.EncuestasAnuladas = await taskAnuladas;

        // Mensaje de variación
        if (avanceGeneral?.Variacion != null)
        {
            viewModel.MensajeVariacion = avanceGeneral.Variacion switch
            {
                < 0 => $"Se presenta variación de {avanceGeneral.Variacion:F2}% respecto a la estimación de producción",
                0 => "No se presentan variaciones respecto a la estimación de producción",
                > 0 => $"Se presenta variación de +{avanceGeneral.Variacion:F2}% respecto a la estimación de producción",
                _ => ""
            };
        }

        // Variables disponibles para dropdown
        viewModel.VariablesDisponibles = viewModel.AvancePorAreas
            .Select(x => x.Variable ?? "")
            .Where(x => !string.IsNullOrEmpty(x))
            .Distinct()
            .OrderBy(x => x)
            .ToList();

        _logger.LogInformation("ViewModel avance campo preparado. TrabajoId: {TrabajoId}", trabajoId);
        return viewModel;
    }

    /// <summary>
    /// Obtiene trabajos activos para dropdown
    /// </summary>
    private async Task<Dictionary<long, string>> ObtenerTrabajosDisponiblesAsync()
    {
        try
        {
            using var connection = new SqlConnection(_connectionString);
            
            var result = await connection.QueryAsync<(long Id, string Nombre)>(
                @"SELECT t.Id, t.NombreTrabajo as Nombre 
                  FROM PY_Trabajo t
                  WHERE t.Estado = 1 AND t.FechaFin >= DATEADD(MONTH, -3, GETDATE())
                  ORDER BY t.NombreTrabajo",
                commandType: CommandType.Text);

            return result.ToDictionary(x => x.Id, x => x.Nombre);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al obtener trabajos disponibles");
            return new Dictionary<long, string>();
        }
    }

    /// <summary>
    /// Exporta avance de campo a Excel
    /// </summary>
    public async Task<byte[]> ExportarExcelAsync(long trabajoId)
    {
        var viewModel = await PrepararViewModelAsync(trabajoId);
        
        using var workbook = new XLWorkbook();
        
        // Hoja General
        var wsGeneral = workbook.Worksheets.Add("General");
        var avanceGeneral = viewModel.AvanceGeneral.FirstOrDefault();
        if (avanceGeneral != null)
        {
            wsGeneral.Cell("A1").Value = "TrabajoId";
            wsGeneral.Cell("B1").Value = avanceGeneral.TrabajoId;
            wsGeneral.Cell("A2").Value = "Muestra Total";
            wsGeneral.Cell("B2").Value = avanceGeneral.MuestraTotal;
            wsGeneral.Cell("A3").Value = "Encuestas Realizadas";
            wsGeneral.Cell("B3").Value = avanceGeneral.EncuestasRealizadas;
            wsGeneral.Cell("A4").Value = "Porcentaje Avance";
            wsGeneral.Cell("B4").Value = avanceGeneral.PorcentajeAvance;
            wsGeneral.Cell("A5").Value = "Remanente";
            wsGeneral.Cell("B5").Value = avanceGeneral.Remanente;
            wsGeneral.Cell("A6").Value = "Mensaje";
            wsGeneral.Cell("B6").Value = viewModel.MensajeVariacion;
        }
        
        // Hoja por Ciudad
        if (viewModel.AvancePorCiudad.Any())
        {
            var wsCiudad = workbook.Worksheets.Add("Por Ciudad");
            wsCiudad.Cell(1, 1).InsertTable(viewModel.AvancePorCiudad);
            wsCiudad.Columns().AdjustToContents();
        }
        
        // Hoja por Áreas
        if (viewModel.AvancePorAreas.Any())
        {
            var wsAreas = workbook.Worksheets.Add("Por Áreas");
            wsAreas.Cell(1, 1).InsertTable(viewModel.AvancePorAreas);
            wsAreas.Columns().AdjustToContents();
        }
        
        // Hoja Remanentes
        if (viewModel.Remanentes.Any())
        {
            var wsRem = workbook.Worksheets.Add("Remanentes");
            wsRem.Cell(1, 1).InsertTable(viewModel.Remanentes);
            wsRem.Columns().AdjustToContents();
        }
        
        // Hoja Matriz
        if (viewModel.MatrizCumplimiento.Any())
        {
            var wsMatriz = workbook.Worksheets.Add("Matriz Cumplimiento");
            wsMatriz.Cell(1, 1).InsertTable(viewModel.MatrizCumplimiento);
            wsMatriz.Columns().AdjustToContents();
        }
        
        _logger.LogInformation("Excel avance campo exportado. TrabajoId: {TrabajoId}", trabajoId);
        
        using var stream = new MemoryStream();
        workbook.SaveAs(stream);
        return stream.ToArray();
    }
}
