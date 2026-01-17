using ClosedXML.Excel;
using MatrixNext.Web.Infrastructure.Data;
using Dapper;
using System.Data;
using Microsoft.EntityFrameworkCore;

namespace MatrixNext.Web.Services.OP;

/// <summary>
/// Implementación de servicio de importación bulk de muestras
/// Soporta Excel (.xlsx) y CSV
/// Ref: WebMatrix Bulk Upload + ClosedXML
/// </summary>
public class OpBulkImportService : IOpBulkImportService
{
    private readonly MatrixDbContext _dbContext;
    private readonly ILogger<OpBulkImportService> _logger;
    private readonly IConfiguration _configuration;

    // Columnas requeridas en Excel
    private static readonly List<string> ColumnasRequeridas = new()
    {
        "IdMuestra", "NombreMuestra", "Distrito", "Municipio", "Vereda",
        "Estrato", "Observaciones"
    };

    public OpBulkImportService(
        MatrixDbContext dbContext,
        ILogger<OpBulkImportService> logger,
        IConfiguration configuration)
    {
        _dbContext = dbContext;
        _logger = logger;
        _configuration = configuration;
    }

    public async Task<(bool Valid, List<string> Errors)> ValidarArchivoAsync(Stream archivoStream, string nombreArchivo)
    {
        var errores = new List<string>();

        try
        {
            if (archivoStream == null || archivoStream.Length == 0)
            {
                errores.Add("Archivo vacío");
                return (false, errores);
            }

            if (!nombreArchivo.EndsWith(".xlsx", StringComparison.OrdinalIgnoreCase) &&
                !nombreArchivo.EndsWith(".csv", StringComparison.OrdinalIgnoreCase))
            {
                errores.Add("Formato no soportado. Use .xlsx o .csv");
                return (false, errores);
            }

            // Validar tamaño máximo (50MB)
            const long maxSize = 50 * 1024 * 1024;
            if (archivoStream.Length > maxSize)
            {
                errores.Add("Archivo demasiado grande (máximo 50MB)");
                return (false, errores);
            }

            if (nombreArchivo.EndsWith(".xlsx", StringComparison.OrdinalIgnoreCase))
            {
                errores.AddRange(await ValidarExcelAsync(archivoStream));
            }
            else if (nombreArchivo.EndsWith(".csv", StringComparison.OrdinalIgnoreCase))
            {
                errores.AddRange(ValidarCsv(archivoStream));
            }

            return (errores.Count == 0, errores);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error validando archivo {Nombre}", nombreArchivo);
            errores.Add("Error al leer el archivo. Verifique que el formato sea correcto.");
            return (false, errores);
        }
    }

    public async Task<(bool Success, int Insertados, int Errores, List<string> Mensajes)> ImportarMuestrasAsync(
        Stream archivoStream, string nombreArchivo, long trabajoId, long usuarioId)
    {
        var mensajes = new List<string>();
        int insertados = 0;
        int errores = 0;

        try
        {
            var (valid, validationErrors) = await ValidarArchivoAsync(archivoStream, nombreArchivo);
            if (!valid)
            {
                mensajes.AddRange(validationErrors);
                return (false, 0, validationErrors.Count, mensajes);
            }

            archivoStream.Seek(0, SeekOrigin.Begin);

            var muestras = new List<MuestraImportDto>();
            
            if (nombreArchivo.EndsWith(".xlsx", StringComparison.OrdinalIgnoreCase))
            {
                muestras = await LeerExcelAsync(archivoStream);
            }
            else if (nombreArchivo.EndsWith(".csv", StringComparison.OrdinalIgnoreCase))
            {
                muestras = LeerCsv(archivoStream);
            }

            if (!muestras.Any())
            {
                mensajes.Add("Archivo vacío o sin datos válidos");
                return (false, 0, 1, mensajes);
            }

            // Procesar e insertar muestras
            foreach (var muestra in muestras)
            {
                try
                {
                    var inserted = await InsertarMuestraAsync(muestra, trabajoId, usuarioId);
                    if (inserted)
                        insertados++;
                    else
                        errores++;
                }
                catch (Exception ex)
                {
                    errores++;
                    _logger.LogWarning(ex, "Error insertando muestra {Muestra}", muestra.IdMuestra);
                    mensajes.Add($"Error en muestra {muestra.IdMuestra}: Error al procesar el registro.");
                }
            }

            mensajes.Add($"Import completado: {insertados} exitosos, {errores} errores");

            _logger.LogInformation("Import {Nombre}: {Insertados} insertados, {Errores} errores",
                nombreArchivo, insertados, errores);

            return (errores == 0, insertados, errores, mensajes);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error importando muestras {Nombre}", nombreArchivo);
            mensajes.Add("Error durante la importación. Por favor intente nuevamente.");
            return (false, insertados, errores + 1, mensajes);
        }
    }

    public async Task<byte[]> GenerarPlantillaExcelAsync()
    {
        using var workbook = new XLWorkbook();
        var ws = workbook.Worksheets.Add("Muestras");

        // Encabezados
        ws.Cell(1, 1).Value = "IdMuestra";
        ws.Cell(1, 2).Value = "NombreMuestra";
        ws.Cell(1, 3).Value = "Distrito";
        ws.Cell(1, 4).Value = "Municipio";
        ws.Cell(1, 5).Value = "Vereda";
        ws.Cell(1, 6).Value = "Estrato";
        ws.Cell(1, 7).Value = "Observaciones";

        // Estilos
        var headerRow = ws.Row(1);
        headerRow.Style.Fill.BackgroundColor = XLColor.DarkBlue;
        headerRow.Style.Font.FontColor = XLColor.White;
        headerRow.Style.Font.Bold = true;

        // Ancho de columnas
        ws.Column(1).Width = 12;
        ws.Column(2).Width = 20;
        ws.Column(3).Width = 15;
        ws.Column(4).Width = 15;
        ws.Column(5).Width = 15;
        ws.Column(6).Width = 10;
        ws.Column(7).Width = 30;

        // Fila de ejemplo
        ws.Cell(2, 1).Value = "M001";
        ws.Cell(2, 2).Value = "Muestra Test";
        ws.Cell(2, 3).Value = "Distrito 1";
        ws.Cell(2, 4).Value = "Municipio 1";
        ws.Cell(2, 5).Value = "Vereda 1";
        ws.Cell(2, 6).Value = "3";

        using var ms = new MemoryStream();
        workbook.SaveAs(ms);
        return ms.ToArray();
    }

    public async Task<List<ImportHistorialVm>> ObtenerHistorialImportsAsync(long trabajoId)
    {
        try
        {
            await using var connection = _dbContext.Database.GetDbConnection();
            if (connection.State != ConnectionState.Open)
                await connection.OpenAsync();

            var historial = await connection.QueryAsync<ImportHistorialVm>(@"
                SELECT TOP 100
                    Id,
                    TrabajoId,
                    NombreArchivo,
                    RegistrosProcessados,
                    RegistrosExitosos,
                    RegistrosError,
                    FechaImport,
                    UsuarioId,
                    Observaciones
                FROM OP_ImportHistorial
                WHERE TrabajoId = @TrabajoId
                ORDER BY FechaImport DESC",
                new { TrabajoId = trabajoId });

            return historial.ToList();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error obteniendo historial imports trabajo {TrabajoId}", trabajoId);
            return new List<ImportHistorialVm>();
        }
    }

    #region Métodos privados

    private async Task<List<string>> ValidarExcelAsync(Stream stream)
    {
        var errores = new List<string>();

        try
        {
            using var workbook = new XLWorkbook(stream);
            var ws = workbook.Worksheet(1);

            // Validar encabezados
            var encabezados = ws.Row(1).Cells(1, ColumnasRequeridas.Count)
                .Select(c => c.Value.ToString() ?? string.Empty)
                .ToList();

            foreach (var col in ColumnasRequeridas)
            {
                if (!encabezados.Contains(col))
                    errores.Add($"Falta columna requerida: {col}");
            }

            // Validar datos
            int filaActual = 2;
            foreach (var row in ws.RowsUsed().Skip(1))
            {
                if (string.IsNullOrWhiteSpace(row.Cell(1).Value.ToString()))
                    errores.Add($"Fila {filaActual}: IdMuestra requerido");

                filaActual++;
                if (errores.Count >= 10) break; // Limitar a 10 errores
            }
        }
        catch (Exception ex)
        {
            errores.Add("Error al leer archivo Excel. Verifique el formato.");
        }

        return errores;
    }

    private List<string> ValidarCsv(Stream stream)
    {
        var errores = new List<string>();
        try
        {
            using var reader = new StreamReader(stream);
            var headerLine = reader.ReadLine();
            if (string.IsNullOrEmpty(headerLine))
            {
                errores.Add("Archivo CSV vacío");
                return errores;
            }

            var headers = headerLine.Split(',');
            foreach (var col in ColumnasRequeridas)
            {
                if (!headers.Contains(col))
                    errores.Add($"Falta columna requerida: {col}");
            }
        }
        catch (Exception ex)
        {
            errores.Add("Error al leer archivo CSV. Verifique el formato.");
        }

        return errores;
    }

    private async Task<List<MuestraImportDto>> LeerExcelAsync(Stream stream)
    {
        var muestras = new List<MuestraImportDto>();

        using var workbook = new XLWorkbook(stream);
        var ws = workbook.Worksheet(1);

        foreach (var row in ws.RowsUsed().Skip(1))
        {
            var muestra = new MuestraImportDto
            {
                IdMuestra = row.Cell(1).Value.ToString() ?? string.Empty,
                NombreMuestra = row.Cell(2).Value.ToString() ?? string.Empty,
                Distrito = row.Cell(3).Value.ToString() ?? string.Empty,
                Municipio = row.Cell(4).Value.ToString() ?? string.Empty,
                Vereda = row.Cell(5).Value.ToString() ?? string.Empty,
                Estrato = row.Cell(6).Value.ToString() ?? string.Empty,
                Observaciones = row.Cell(7).Value.ToString()
            };

            if (!string.IsNullOrWhiteSpace(muestra.IdMuestra))
                muestras.Add(muestra);
        }

        return muestras;
    }

    private List<MuestraImportDto> LeerCsv(Stream stream)
    {
        var muestras = new List<MuestraImportDto>();

        using var reader = new StreamReader(stream);
        var headerLine = reader.ReadLine();
        if (string.IsNullOrEmpty(headerLine))
            return muestras;

        var headers = headerLine.Split(',');

        string? line;
        while ((line = reader.ReadLine()) != null)
        {
            var valores = line.Split(',');
            var muestra = new MuestraImportDto
            {
                IdMuestra = valores[0].Trim(),
                NombreMuestra = valores.Length > 1 ? valores[1].Trim() : string.Empty,
                Distrito = valores.Length > 2 ? valores[2].Trim() : string.Empty,
                Municipio = valores.Length > 3 ? valores[3].Trim() : string.Empty,
                Vereda = valores.Length > 4 ? valores[4].Trim() : string.Empty,
                Estrato = valores.Length > 5 ? valores[5].Trim() : string.Empty,
                Observaciones = valores.Length > 6 ? valores[6].Trim() : null
            };

            if (!string.IsNullOrWhiteSpace(muestra.IdMuestra))
                muestras.Add(muestra);
        }

        return muestras;
    }

    private async Task<bool> InsertarMuestraAsync(MuestraImportDto muestra, long trabajoId, long usuarioId)
    {
        try
        {
            await using var connection = _dbContext.Database.GetDbConnection();
            if (connection.State != ConnectionState.Open)
                await connection.OpenAsync();

            var parameters = new DynamicParameters();
            parameters.Add("@IdMuestra", muestra.IdMuestra);
            parameters.Add("@NombreMuestra", muestra.NombreMuestra);
            parameters.Add("@Distrito", muestra.Distrito);
            parameters.Add("@Municipio", muestra.Municipio);
            parameters.Add("@Vereda", muestra.Vereda);
            parameters.Add("@Estrato", int.TryParse(muestra.Estrato, out var estrato) ? estrato : DBNull.Value);
            parameters.Add("@Observaciones", muestra.Observaciones ?? (object)DBNull.Value);
            parameters.Add("@TrabajoId", trabajoId);
            parameters.Add("@RegistradoPor", usuarioId);

            var rowsAffected = await connection.ExecuteAsync(@"
                INSERT INTO OP_MuestraTrabajos
                (IdMuestra, NombreMuestra, Distrito, Municipio, Vereda, Estrato, Observaciones, TrabajoId, RegistradoPor, FechaRegistro)
                VALUES (@IdMuestra, @NombreMuestra, @Distrito, @Municipio, @Vereda, @Estrato, @Observaciones, @TrabajoId, @RegistradoPor, GETDATE())",
                parameters);

            return rowsAffected > 0;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error insertando muestra {IdMuestra}", muestra.IdMuestra);
            throw;
        }
    }

    #endregion

    #region DTOs privados

    private class MuestraImportDto
    {
        public string IdMuestra { get; set; } = string.Empty;
        public string NombreMuestra { get; set; } = string.Empty;
        public string Distrito { get; set; } = string.Empty;
        public string Municipio { get; set; } = string.Empty;
        public string Vereda { get; set; } = string.Empty;
        public string Estrato { get; set; } = string.Empty;
        public string? Observaciones { get; set; }
    }

    #endregion
}
