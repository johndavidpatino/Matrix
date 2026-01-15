/// <summary>
/// Servicio de procesamiento de carga masiva con ClosedXML
/// Reemplaza OleDb por lectura directa con OpenXml/ClosedXML
/// Ref: BACKLOG_QA_MODULOS_PENDIENTES.md § Sprint 12.1.7
/// </summary>
namespace MatrixNext.Data.Services.OP;

using ClosedXML.Excel;
using MatrixNext.Data.Adapters.OP;
using MatrixNext.Data.Models.OP;
using Microsoft.Extensions.Logging;

public class CargaMasivaService : ICargaMasivaService
{
    private readonly ICargaMasivaAdapter _adapter;
    private readonly ILogger<CargaMasivaService> _logger;

    public CargaMasivaService(
        ICargaMasivaAdapter adapter,
        ILogger<CargaMasivaService> logger)
    {
        _adapter = adapter;
        _logger = logger;
    }

    public async Task<ResumenCargaMasivaDto> ProcesarCatiRmcAsync(
        Stream archivoStream, 
        string nombreArchivo, 
        long usuarioId, 
        bool ejecutar = false)
    {
        _logger.LogInformation("Iniciando procesamiento CATI RMC. Archivo: {Archivo}, Usuario: {UserId}, Ejecutar: {Ejecutar}",
            nombreArchivo, usuarioId, ejecutar);

        var resumen = new ResumenCargaMasivaDto
        {
            TipoCarga = "CATI",
            NombreArchivo = nombreArchivo,
            UsuarioId = usuarioId,
            FechaCarga = DateTime.Now
        };

        try
        {
            // 1. Abrir archivo con ClosedXML
            using var workbook = new XLWorkbook(archivoStream);
            var worksheet = workbook.Worksheet(1); // Primera hoja

            // 2. Validar columnas
            var primeraFila = worksheet.Row(1);
            var columnas = new List<string>();
            for (int col = 1; col <= primeraFila.CellsUsed().Count(); col++)
            {
                columnas.Add(primeraFila.Cell(col).GetValue<string>());
            }

            var (valido, errores) = await _adapter.ValidarColumnasExcelCatiAsync(columnas);
            if (!valido)
            {
                _logger.LogWarning("Estructura de columnas CATI inválida: {Errores}", string.Join(", ", errores));
                resumen.Validaciones.Add(new ResultadoValidacionFilaDto
                {
                    NumeroFila = 1,
                    EsValida = false,
                    Errores = errores
                });
                return resumen;
            }

            // 3. Extraer datos
            var datos = new List<CargaCatiRmcDto>();
            var filaActual = 2; // Comenzar desde fila 2 (después de encabezados)
            while (!worksheet.Row(filaActual).IsEmpty())
            {
                var fila = worksheet.Row(filaActual);
                var dto = new CargaCatiRmcDto
                {
                    TrabajoId = fila.Cell(1).TryGetValue(out long trabajoId) ? trabajoId : 0,
                    Res_Numero = fila.Cell(2).TryGetValue(out int resNum) ? resNum : 0,
                    Per_NumIdentificacionEncu = fila.Cell(3).GetValue<string>(),
                    Per_NumIdentificacionSup = fila.Cell(4).TryGetValue(out string perSup) ? perSup : null,
                    Res_IDM = fila.Cell(5).TryGetValue(out string resIdm) ? resIdm : null,
                    Res_Ciudad = fila.Cell(6).TryGetValue(out string ciudad) ? ciudad : null,
                    Res_Fecha = fila.Cell(7).TryGetValue(out DateTime fecha) ? fecha : (DateTime?)null,
                    TipoSupervision = fila.Cell(8).TryGetValue(out string tipoSup) ? tipoSup : null,
                    TipoActividad = fila.Cell(9).GetValue<string>()
                };

                datos.Add(dto);
                filaActual++;
            }

            resumen.TotalFilas = datos.Count;
            _logger.LogInformation("Extraídas {Count} filas de archivo CATI", datos.Count);

            // 4. Validar cada fila
            var filasValidas = new List<CargaCatiRmcDto>();
            for (int i = 0; i < datos.Count; i++)
            {
                var validacion = await _adapter.ValidarFilaCatiAsync(datos[i], i + 2); // +2 por header y 0-indexed
                resumen.Validaciones.Add(validacion);

                if (validacion.EsValida)
                    filasValidas.Add(datos[i]);
            }

            resumen.FilasValidas = filasValidas.Count;
            resumen.FilasRechazadas = resumen.TotalFilas - resumen.FilasValidas;

            // 5. Si ejecutar = true, insertar datos válidos
            if (ejecutar && filasValidas.Count > 0)
            {
                await _adapter.InsertarCatiRmcAsync(filasValidas, usuarioId);
                _logger.LogInformation("Insertadas {Count} filas CATI válidas", filasValidas.Count);
            }

            return resumen;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error procesando archivo CATI. Archivo: {Archivo}, Usuario: {UserId}", 
                nombreArchivo, usuarioId);
            
            resumen.Validaciones.Add(new ResultadoValidacionFilaDto
            {
                NumeroFila = 0,
                EsValida = false,
                Errores = new List<string> { $"Error crítico: {ex.Message}" }
            });
            
            return resumen;
        }
    }

    public async Task<ResumenCargaMasivaDto> ProcesarPlanillasAsync(
        Stream archivoStream, 
        string nombreArchivo, 
        long usuarioId, 
        bool ejecutar = false)
    {
        _logger.LogInformation("Iniciando procesamiento Planillas. Archivo: {Archivo}, Usuario: {UserId}, Ejecutar: {Ejecutar}",
            nombreArchivo, usuarioId, ejecutar);

        var resumen = new ResumenCargaMasivaDto
        {
            TipoCarga = "Planillas",
            NombreArchivo = nombreArchivo,
            UsuarioId = usuarioId,
            FechaCarga = DateTime.Now
        };

        try
        {
            // 1. Abrir archivo con ClosedXML
            using var workbook = new XLWorkbook(archivoStream);
            var worksheet = workbook.Worksheet(1);

            // 2. Validar columnas
            var primeraFila = worksheet.Row(1);
            var columnas = new List<string>();
            for (int col = 1; col <= primeraFila.CellsUsed().Count(); col++)
            {
                columnas.Add(primeraFila.Cell(col).GetValue<string>());
            }

            var (valido, errores) = await _adapter.ValidarColumnasExcelPlanillasAsync(columnas);
            if (!valido)
            {
                _logger.LogWarning("Estructura de columnas Planillas inválida: {Errores}", string.Join(", ", errores));
                resumen.Validaciones.Add(new ResultadoValidacionFilaDto
                {
                    NumeroFila = 1,
                    EsValida = false,
                    Errores = errores
                });
                return resumen;
            }

            // 3. Extraer datos
            var datos = new List<CargaPlanillaDto>();
            var filaActual = 2;
            while (!worksheet.Row(filaActual).IsEmpty())
            {
                var fila = worksheet.Row(filaActual);
                var dto = new CargaPlanillaDto
                {
                    IdTrabajo = fila.Cell(1).TryGetValue(out long trabajoId) ? trabajoId : 0,
                    IdEmpleado = fila.Cell(2).TryGetValue(out long empleadoId) ? empleadoId : 0,
                    Fecha = fila.Cell(3).TryGetValue(out DateTime fecha) ? fecha : DateTime.MinValue,
                    Cantidad = fila.Cell(4).TryGetValue(out int cantidad) ? cantidad : 0,
                    TipoProductividad = fila.Cell(5).TryGetValue(out string tipo) ? tipo : null,
                    Observaciones = fila.Cell(6).TryGetValue(out string obs) ? obs : null
                };

                datos.Add(dto);
                filaActual++;
            }

            resumen.TotalFilas = datos.Count;
            _logger.LogInformation("Extraídas {Count} filas de archivo Planillas", datos.Count);

            // 4. Validar cada fila
            var filasValidas = new List<CargaPlanillaDto>();
            for (int i = 0; i < datos.Count; i++)
            {
                var validacion = await _adapter.ValidarFilaPlanillaAsync(datos[i], i + 2);
                resumen.Validaciones.Add(validacion);

                if (validacion.EsValida)
                    filasValidas.Add(datos[i]);
            }

            resumen.FilasValidas = filasValidas.Count;
            resumen.FilasRechazadas = resumen.TotalFilas - resumen.FilasValidas;

            // 5. Si ejecutar = true, insertar datos válidos
            if (ejecutar && filasValidas.Count > 0)
            {
                await _adapter.InsertarPlanillasAsync(filasValidas, usuarioId);
                _logger.LogInformation("Insertadas {Count} filas Planillas válidas", filasValidas.Count);
            }

            return resumen;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error procesando archivo Planillas. Archivo: {Archivo}, Usuario: {UserId}", 
                nombreArchivo, usuarioId);
            
            resumen.Validaciones.Add(new ResultadoValidacionFilaDto
            {
                NumeroFila = 0,
                EsValida = false,
                Errores = new List<string> { $"Error crítico: {ex.Message}" }
            });
            
            return resumen;
        }
    }

    public async Task<List<T>> ExtraerDatosExcelAsync<T>(Stream archivoStream, string nombreHoja) where T : class
    {
        _logger.LogInformation("Extrayendo datos de hoja: {Hoja}", nombreHoja);
        
        // Esta implementación se puede usar para extracción genérica
        // Por ahora retorna lista vacía (lógica ya está en ProcesarCatiRmcAsync y ProcesarPlanillasAsync)
        return new List<T>();
    }

    public async Task<ResumenCargaMasivaDto> ValidarFilasAsync<T>(List<T> filas, string tipoCarga, long usuarioId) where T : class
    {
        _logger.LogInformation("Validando {Count} filas de tipo {Tipo}", filas.Count, tipoCarga);
        
        // Esta implementación se puede usar para validación genérica
        // Por ahora retorna resumen vacío (lógica ya está en ProcesarCatiRmcAsync y ProcesarPlanillasAsync)
        return new ResumenCargaMasivaDto { TipoCarga = tipoCarga, UsuarioId = usuarioId };
    }
}
