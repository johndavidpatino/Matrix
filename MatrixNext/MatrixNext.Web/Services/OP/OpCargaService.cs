using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;
using System.IO;
using System.Linq;
using ClosedXML.Excel;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace MatrixNext.Web.Services.OP;

public class OpCargaService : IOpCargaService
{
    private static readonly IReadOnlyList<string> CatiHeaders = new[]
    {
        "TrabajoId",
        "Res_Numero",
        "Per_NumIdentificacionEncu",
        "Per_NumIdentificacionSup",
        "Res_IDM",
        "Res_Ciudad",
        "Res_Fecha",
        "TipoSupervision",
        "TipoActividad"
    };

    private static readonly IReadOnlyList<string> PlanillaHeaders = new[]
    {
        "TrabajoId",
        "Per_NumIdentificacionEncu",
        "Res_Ciudad",
        "Res_Fecha",
        "TipoActividad",
        "Cantidad"
    };

    private static readonly HashSet<int> CatiActividades = new() { 1, 10, 20, 21, 22, 23 };
    private static readonly HashSet<int> PlanillaActividades = new() { 1, 10, 11, 12, 13, 20, 21, 22, 23 };
    private static readonly HashSet<string> AllowedExtensions = new(StringComparer.OrdinalIgnoreCase) { ".xls", ".xlsx" };

    private readonly ILogger<OpCargaService> _logger;
    private readonly string _stagingDirectory;
    private readonly string _connectionString;

    public OpCargaService(
        ILogger<OpCargaService> logger,
        IWebHostEnvironment environment,
        IConfiguration configuration)
    {
        _logger = logger;
        _connectionString = configuration.GetConnectionString("MatrixDb") ?? string.Empty;
        _stagingDirectory = Path.Combine(
            environment.WebRootPath ?? Directory.GetCurrentDirectory(),
            "uploads",
            "op",
            "cargas");
        Directory.CreateDirectory(_stagingDirectory);
    }

    public async Task<OpCargaResult> ProcesarArchivoAsync(
        IFormFile archivo,
        OpCargaTipo tipo,
        CancellationToken cancellationToken = default)
    {
        if (archivo is null || archivo.Length == 0)
        {
            return new OpCargaResult(false, "Selecciona un archivo Excel válido (.xls o .xlsx).");
        }

        var extension = Path.GetExtension(archivo.FileName);
        if (string.IsNullOrWhiteSpace(extension) || !AllowedExtensions.Contains(extension))
        {
            return new OpCargaResult(false, "Solo se admiten archivos con extensión .xls o .xlsx.");
        }

        DataTable worksheet;
        try
        {
            worksheet = await ReadWorksheetAsync(archivo, cancellationToken);
        }
        catch (Exception ex)
        {
            _logger.LogWarning(ex, "Error leyendo el Excel cargado para OP_Cuantitativo");
            return new OpCargaResult(false, $"No se pudo leer el Excel: {ex.Message}");
        }

        if (worksheet.Rows.Count == 0)
        {
            return new OpCargaResult(false, "El archivo no contiene filas con datos.");
        }

        var expectedHeaders = tipo == OpCargaTipo.CatiRMC ? CatiHeaders : PlanillaHeaders;
        if (!ValidateHeaders(worksheet, expectedHeaders, out var missing))
        {
            return new OpCargaResult(
                false,
                $"Faltan columnas obligatorias: {string.Join(", ", missing)}. Usa la plantilla legacy.");
        }

        var validation = tipo switch
        {
            OpCargaTipo.CatiRMC => ValidateCati(worksheet),
            OpCargaTipo.Planillas => await ValidatePlanillasAsync(worksheet, cancellationToken),
            _ => (IsValid: false, Message: "Tipo de carga no soportado.")
        };

        if (!validation.IsValid)
        {
            return new OpCargaResult(false, validation.Message!);
        }

        var copia = await SaveBackupAsync(archivo, cancellationToken);
        var filas = worksheet.Rows.Count;
        return new OpCargaResult(
            true,
            $"Archivo validado con {filas} fila(s). Copia de auditoría: {Path.GetFileName(copia)}.");
    }

    private static bool ValidateHeaders(DataTable table, IReadOnlyList<string> required, out List<string> missing)
    {
        missing = new List<string>();
        var columns = table.Columns.Cast<DataColumn>()
            .Select(column => column.ColumnName?.Trim() ?? string.Empty)
            .Where(name => !string.IsNullOrEmpty(name))
            .ToHashSet(StringComparer.OrdinalIgnoreCase);

        foreach (var header in required)
        {
            if (!columns.Contains(header))
            {
                missing.Add(header);
            }
        }

        return missing.Count == 0;
    }

    private static (bool IsValid, string? Message) ValidateCati(DataTable table)
    {
        for (var index = 0; index < table.Rows.Count; index++)
        {
            var row = table.Rows[index];
            var rowNumber = index + 2;
            if (!TryParseInt(row["TipoActividad"], out var tipo) || !CatiActividades.Contains(tipo))
            {
                return (false, $"TipoActividad inválido en fila {rowNumber}: {row["TipoActividad"]}.");
            }
        }

        return (true, null);
    }

    private async Task<(bool IsValid, string? Message)> ValidatePlanillasAsync(
        DataTable table,
        CancellationToken cancellationToken)
    {
        var festivos = await LoadFestivosAsync(cancellationToken);
        for (var index = 0; index < table.Rows.Count; index++)
        {
            var row = table.Rows[index];
            var rowNumber = index + 2;
            if (!TryParseInt(row["TipoActividad"], out var tipo) || !PlanillaActividades.Contains(tipo))
            {
                return (false, $"TipoActividad inválido en fila {rowNumber}: {row["TipoActividad"]}.");
            }

            if (!TryParseDate(row["Res_Fecha"], out var fecha))
            {
                return (false, $"Fecha inválida en fila {rowNumber}.");
            }

            var today = DateTime.Now;
            var inicioCorte = new DateTime(today.AddMonths(-1).Year, today.AddMonths(-1).Month, 16);
            var finCorte = new DateTime(today.Year, today.Month, 15);
            if (fecha.Date < inicioCorte || fecha.Date > finCorte)
            {
                return (false, $"La fecha {fecha:dd/MM/yyyy} en fila {rowNumber} no está dentro del corte 16-15.");
            }

            if (!TryParseInt(row["Cantidad"], out var cantidad) || cantidad < 1)
            {
                return (false, $"Cantidad inválida en fila {rowNumber}.");
            }

            if ((tipo == 22 || tipo == 23) && fecha.DayOfWeek != DayOfWeek.Sunday)
            {
                var fechaDominical = DateOnly.FromDateTime(fecha);
                if (festivos.Count == 0 || !festivos.Contains(fechaDominical))
                {
                    return (false, $"El registro dominical {fecha:dd/MM/yyyy} debe corresponder a un festivo.");
                }
            }
        }

        return (true, null);
    }

    private async Task<HashSet<DateOnly>> LoadFestivosAsync(CancellationToken cancellationToken)
    {
        var fechas = new HashSet<DateOnly>();
        if (string.IsNullOrWhiteSpace(_connectionString))
        {
            _logger.LogWarning("No se encontró la cadena MatrixDb para validar festivos.");
            return fechas;
        }

        try
        {
            await using var connection = new SqlConnection(_connectionString);
            await connection.OpenAsync(cancellationToken);
            await using var command = new SqlCommand("SELECT festivo FROM _Festivos", connection);
            await using var reader = await command.ExecuteReaderAsync(cancellationToken);
            while (await reader.ReadAsync(cancellationToken))
            {
                if (reader["festivo"] is not DateTime fecha)
                {
                    var raw = reader["festivo"]?.ToString();
                    if (!DateTime.TryParse(raw, CultureInfo.InvariantCulture, DateTimeStyles.None, out fecha))
                    {
                        continue;
                    }
                }

                fechas.Add(DateOnly.FromDateTime(fecha));
            }
        }
        catch (Exception ex)
        {
            _logger.LogWarning(ex, "No se pudieron cargar festivos desde _Festivos.");
        }

        return fechas;
    }

    private static async Task<DataTable> ReadWorksheetAsync(IFormFile archivo, CancellationToken cancellationToken)
    {
        await using var memoryStream = new MemoryStream();
        await archivo.CopyToAsync(memoryStream, cancellationToken);
        memoryStream.Position = 0;

        using var workbook = new XLWorkbook(memoryStream);
        var worksheet = workbook.Worksheets.FirstOrDefault();
        if (worksheet is null)
        {
            throw new InvalidDataException("El archivo no contiene hojas.");
        }

        var range = worksheet.RangeUsed();
        if (range is null)
        {
            throw new InvalidDataException("La hoja no contiene datos.");
        }

        var table = new DataTable();
        var headerRow = range.FirstRowUsed();
        var headerCells = headerRow.Cells().ToList();
        foreach (var cell in headerCells)
        {
            var columnName = cell.GetString().Trim();
            if (string.IsNullOrEmpty(columnName))
            {
                columnName = $"Column{table.Columns.Count + 1}";
            }

            if (table.Columns.Contains(columnName))
            {
                columnName = $"{columnName}_{Guid.NewGuid():N}";
            }

            table.Columns.Add(columnName);
        }

        foreach (var row in range.RowsUsed().Skip(1))
        {
            var newRow = table.NewRow();
            for (var index = 0; index < table.Columns.Count; index++)
            {
                newRow[index] = row.Cell(index + 1).Value;
            }

            table.Rows.Add(newRow);
        }

        return table;
    }

    private static bool TryParseInt(object? value, out int result)
    {
        if (value is null)
        {
            result = 0;
            return false;
        }

        return int.TryParse(value.ToString()?.Trim(), NumberStyles.Integer, CultureInfo.InvariantCulture, out result);
    }

    private static bool TryParseDate(object? value, out DateTime result)
    {
        if (value is null)
        {
            result = default;
            return false;
        }

        if (value is DateTime dt)
        {
            result = dt;
            return true;
        }

        return DateTime.TryParse(value.ToString(), CultureInfo.InvariantCulture, DateTimeStyles.None, out result);
    }

    private async Task<string> SaveBackupAsync(IFormFile archivo, CancellationToken cancellationToken)
    {
        var fileName = $"{Guid.NewGuid():N}{Path.GetExtension(archivo.FileName)}";
        var destination = Path.Combine(_stagingDirectory, fileName);
        await using var targetStream = new FileStream(destination, FileMode.Create, FileAccess.Write, FileShare.None);
        await archivo.CopyToAsync(targetStream, cancellationToken);
        return destination;
    }
}
