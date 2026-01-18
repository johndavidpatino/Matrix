/// <summary>
/// Adapter para procesamiento de carga masiva de datos CATI y Planillas
/// Utiliza Dapper con fallback a consultas directas
/// Ref: BACKLOG_QA_MODULOS_PENDIENTES.md § Sprint 12.1.7
/// </summary>
namespace MatrixNext.Data.Adapters.OP;

using Dapper;
using MatrixNext.Data.Models.OP;
using Microsoft.Extensions.Logging;
using System.Data;
using System.Data.SqlClient;

public class CargaMasivaAdapter : ICargaMasivaAdapter
{
    private readonly IDbConnection _connection;
    private readonly ILogger<CargaMasivaAdapter> _logger;

    public CargaMasivaAdapter(
        IDbConnection connection,
        ILogger<CargaMasivaAdapter> logger)
    {
        _connection = connection;
        _logger = logger;
    }

    public async Task<(bool Valido, List<string> Errores)> ValidarColumnasExcelCatiAsync(List<string> columnasActuales)
    {
        var errores = new List<string>();
        var columnasRequeridas = new[]
        {
            "TrabajoId", "Res_Numero", "Per_NumIdentificacionEncu", "Per_NumIdentificacionSup",
            "Res_IDM", "Res_Ciudad", "Res_Fecha", "TipoSupervision", "TipoActividad"
        };

        foreach (var columna in columnasRequeridas)
        {
            if (!columnasActuales.Contains(columna, StringComparer.OrdinalIgnoreCase))
                errores.Add($"Columna requerida faltante: {columna}");
        }

        _logger.LogInformation("Validación de columnas CATI: {Resultado}", errores.Count == 0 ? "✓ OK" : "✗ Errores");
        return (errores.Count == 0, errores);
    }

    public async Task<(bool Valido, List<string> Errores)> ValidarColumnasExcelPlanillasAsync(List<string> columnasActuales)
    {
        var errores = new List<string>();
        var columnasRequeridas = new[] { "IdTrabajo", "IdEmpleado", "Fecha", "Cantidad", "TipoProductividad" };

        foreach (var columna in columnasRequeridas)
        {
            if (!columnasActuales.Contains(columna, StringComparer.OrdinalIgnoreCase))
                errores.Add($"Columna requerida faltante: {columna}");
        }

        _logger.LogInformation("Validación de columnas Planillas: {Resultado}", errores.Count == 0 ? "✓ OK" : "✗ Errores");
        return (errores.Count == 0, errores);
    }

    public async Task<ResultadoValidacionFilaDto> ValidarFilaCatiAsync(CargaCatiRmcDto fila, int numFila)
    {
        var resultado = new ResultadoValidacionFilaDto { NumeroFila = numFila };

        try
        {
            // Validación 1: TrabajoId debe existir en PY_Trabajo (CORREGIDO)
            var trabajoExiste = await _connection.ExecuteScalarAsync<bool>(
                "SELECT CASE WHEN COUNT(*) > 0 THEN 1 ELSE 0 END FROM PY_Trabajo WHERE id = @IdTrabajo",
                new { fila.TrabajoId });

            if (!trabajoExiste)
            {
                resultado.Errores.Add($"TrabajoId {fila.TrabajoId} no existe en el sistema");
                resultado.EsValida = false;
                return resultado;
            }

            // Validación 2: TipoActividad debe ser válido (enum)
            var tiposValidos = new[] { "Implementación", "InstruccionarioRespondido", "InstruccionarioCorregido", "Supervisión" };
            if (!tiposValidos.Contains(fila.TipoActividad))
            {
                resultado.Errores.Add($"TipoActividad '{fila.TipoActividad}' no es válido. Válidos: {string.Join(", ", tiposValidos)}");
                resultado.EsValida = false;
                return resultado;
            }

            // Validación 3: Per_NumIdentificacionEncu no puede estar vacío
            if (string.IsNullOrWhiteSpace(fila.Per_NumIdentificacionEncu))
            {
                resultado.Errores.Add("Per_NumIdentificacionEncu no puede estar vacío");
                resultado.EsValida = false;
                return resultado;
            }

            // Validación 4: Res_Fecha debe ser válida
            if (fila.Res_Fecha.HasValue)
            {
                if (fila.Res_Fecha.Value > DateTime.Now.AddDays(1))
                {
                    resultado.Advertencia = $"Res_Fecha ({fila.Res_Fecha:dd/MM/yyyy}) es posterior a hoy";
                }
            }

            resultado.EsValida = true;
            _logger.LogInformation("Fila CATI {NumFila}: Validación exitosa", numFila);
            return resultado;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error validando fila CATI {NumFila}", numFila);
            resultado.Errores.Add("Error en validación. Por favor intente nuevamente.");
            resultado.EsValida = false;
            return resultado;
        }
    }

    public async Task<ResultadoValidacionFilaDto> ValidarFilaPlanillaAsync(CargaPlanillaDto fila, int numFila)
    {
        var resultado = new ResultadoValidacionFilaDto { NumeroFila = numFila };

        try
        {
            // Validación 1: Trabajo existe (CORREGIDO: PY_Trabajo, id)
            var trabajoExiste = await _connection.ExecuteScalarAsync<bool>(
                "SELECT CASE WHEN COUNT(*) > 0 THEN 1 ELSE 0 END FROM PY_Trabajo WHERE id = @IdTrabajo",
                new { fila.IdTrabajo });

            if (!trabajoExiste)
            {
                resultado.Errores.Add($"IdTrabajo {fila.IdTrabajo} no existe");
                resultado.EsValida = false;
                return resultado;
            }

            // Validación 2: Empleado existe (CORREGIDO: TH_Personas, id)
            var empleadoExiste = await _connection.ExecuteScalarAsync<bool>(
                "SELECT CASE WHEN COUNT(*) > 0 THEN 1 ELSE 0 END FROM TH_Personas WHERE id = @IdEmpleado",
                new { fila.IdEmpleado });

            if (!empleadoExiste)
            {
                resultado.Errores.Add($"IdEmpleado {fila.IdEmpleado} no existe");
                resultado.EsValida = false;
                return resultado;
            }

            // Validación 3: Cantidad debe ser positiva
            if (fila.Cantidad <= 0)
            {
                resultado.Errores.Add($"Cantidad ({fila.Cantidad}) debe ser mayor a 0");
                resultado.EsValida = false;
                return resultado;
            }

            // Validación 4: Fecha en corte válido (16-15)
            var corte = await CalcularCorte16_15Async(fila.Fecha);
            if (corte != 1 && corte != 2)
            {
                resultado.Advertencia = $"Fecha ({fila.Fecha:dd/MM/yyyy}) no está en rango de corte 16-15 válido";
            }

            // Validación 5: No es festivo (si está activado)
            var festivos = await ObtenerFestivosAsync(fila.Fecha.Year);
            if (festivos.Contains(fila.Fecha.Date))
            {
                resultado.Advertencia = $"Fecha ({fila.Fecha:dd/MM/yyyy}) es festivo o domingo";
            }

            resultado.EsValida = true;
            _logger.LogInformation("Fila Planilla {NumFila}: Validación exitosa", numFila);
            return resultado;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error validando fila Planilla {NumFila}", numFila);
            resultado.Errores.Add("Error en validación. Por favor intente nuevamente.");
            resultado.EsValida = false;
            return resultado;
        }
    }

    public async Task<List<DateTime>> ObtenerFestivosAsync(int año)
    {
        try
        {
            // NOTA: Tabla _Festivos existe con columna 'festivo' (date)
            var festivos = await _connection.QueryAsync<DateTime>(
                "SELECT festivo FROM _Festivos WHERE YEAR(festivo) = @Ano ORDER BY festivo",
                new { Ano = año });
            
            _logger.LogInformation("Obtenidos {Count} festivos para año {Ano}", festivos.Count(), año);
            return festivos.ToList();
        }
        catch (Exception ex)
        {
            _logger.LogWarning(ex, "No se pudieron obtener festivos para año {Ano}. Continuando sin validación.", año);
            return new List<DateTime>();
        }
    }

    public async Task<List<DateTime>> ObtenerDomingosAsync(int año)
    {
        var domingos = new List<DateTime>();
        var inicio = new DateTime(año, 1, 1);
        var fin = new DateTime(año, 12, 31);

        for (var fecha = inicio; fecha <= fin; fecha = fecha.AddDays(1))
        {
            if (fecha.DayOfWeek == DayOfWeek.Sunday)
                domingos.Add(fecha);
        }

        _logger.LogInformation("Calculados {Count} domingos para año {Ano}", domingos.Count, año);
        return domingos;
    }

    public async Task<int> InsertarCatiRmcAsync(List<CargaCatiRmcDto> datos, long usuarioId)
    {
        try
        {
            int filasInsertadas = 0;
            using (var transaction = _connection.BeginTransaction())
            {
                foreach (var fila in datos)
                {
                    var rowsAffected = await _connection.ExecuteAsync(
                        @"INSERT INTO CatiRMC_RespuestasTmp 
                            (TrabajoId, Res_Numero, Per_NumIdentificacionEncu, Per_NumIdentificacionSup,
                             Res_IDM, Res_Ciudad, Res_Fecha, TipoSupervision, TipoActividad, InsertadoFecha, InsertadoPor)
                          VALUES (@TrabajoId, @Res_Numero, @Per_NumIdentificacionEncu, @Per_NumIdentificacionSup,
                                  @Res_IDM, @Res_Ciudad, @Res_Fecha, @TipoSupervision, @TipoActividad, @InsertadoFecha, @InsertadoPor)",
                        new
                        {
                            fila.TrabajoId,
                            fila.Res_Numero,
                            fila.Per_NumIdentificacionEncu,
                            fila.Per_NumIdentificacionSup,
                            fila.Res_IDM,
                            fila.Res_Ciudad,
                            fila.Res_Fecha,
                            fila.TipoSupervision,
                            fila.TipoActividad,
                            InsertadoFecha = DateTime.Now,
                            InsertadoPor = usuarioId
                        },
                        transaction);
                    
                    filasInsertadas += rowsAffected;
                }
                transaction.Commit();
            }

            _logger.LogInformation("Insertadas {Count} filas CATI en tabla temporal", filasInsertadas);
            return filasInsertadas;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error insertando datos CATI. UsuarioId: {UsuarioId}", usuarioId);
            throw;
        }
    }

    public async Task<int> InsertarPlanillasAsync(List<CargaPlanillaDto> datos, long usuarioId)
    {
        try
        {
            int filasInsertadas = 0;
            using (var transaction = _connection.BeginTransaction())
            {
                foreach (var fila in datos)
                {
                    var rowsAffected = await _connection.ExecuteAsync(
                        @"INSERT INTO CuantiPlanillas_Tmp 
                            (IdTrabajo, IdEmpleado, Fecha, Cantidad, TipoProductividad, Observaciones, InsertadoFecha, InsertadoPor)
                          VALUES (@IdTrabajo, @IdEmpleado, @Fecha, @Cantidad, @TipoProductividad, @Observaciones, @InsertadoFecha, @InsertadoPor)",
                        new
                        {
                            fila.IdTrabajo,
                            fila.IdEmpleado,
                            fila.Fecha,
                            fila.Cantidad,
                            fila.TipoProductividad,
                            fila.Observaciones,
                            InsertadoFecha = DateTime.Now,
                            InsertadoPor = usuarioId
                        },
                        transaction);
                    
                    filasInsertadas += rowsAffected;
                }
                transaction.Commit();
            }

            _logger.LogInformation("Insertadas {Count} filas Planilla en tabla temporal", filasInsertadas);
            return filasInsertadas;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error insertando datos Planilla. UsuarioId: {UsuarioId}", usuarioId);
            throw;
        }
    }

    public async Task<int> CalcularCorte16_15Async(DateTime fecha)
    {
        try
        {
            // Lógica de corte 16-15:
            // Corte 1: 1-15 del mes
            // Corte 2: 16 del mes - 15 del siguiente
            if (fecha.Day >= 1 && fecha.Day <= 15)
                return 1;
            else
                return 2;
        }
        catch (Exception ex)
        {
            _logger.LogWarning(ex, "Error calculando corte 16-15 para fecha {Fecha}", fecha);
            return 0;
        }
    }
}
