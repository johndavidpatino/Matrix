using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MatrixNext.Data.Services.MBO;
using MatrixNext.Data.ViewModels;
using MatrixNext.Data.Models.MBO;
using OfficeOpenXml;

namespace MatrixNext.Web.Areas.MBO.Controllers
{
    /// <summary>
    /// Controller para gestión de calidad de campo (MBO Fase 2)
    /// Dashboards de encuestas, calidad, errores y carga masiva de errores desde Excel
    /// </summary>
    [Area("MBO")]
    [Authorize]
    public class CampoController : Controller
    {
        private readonly ICampoService _service;
        private readonly ILogger<CampoController> _logger;

        public CampoController(ICampoService service, ILogger<CampoController> logger)
        {
            _service = service;
            _logger = logger;
        }

        /// <summary>
        /// Dashboard de encuestas realizadas vs meta con semáforo de logro
        /// </summary>
        [HttpGet]
        public async Task<IActionResult> Encuestas(int? año, int? mes, string? sigla)
        {
            try
            {
                // Valores por defecto
                var añoActual = año ?? DateTime.Now.Year;
                var mesActual = mes ?? DateTime.Now.Month;
                var siglaActual = sigla ?? "CO";
                var userId = int.Parse(User.FindFirst("IdUsuario")?.Value ?? "0");

                // Obtener datos en paralelo (Task.WhenAll en service)
                var (encuestas, calidad, estadisticas) = await _service.ObtenerDashboardEncuestasAsync(añoActual, mesActual, siglaActual, userId);

                var viewModel = new CampoEncuestasViewModel
                {
                    Año = añoActual,
                    Mes = mesActual,
                    Sigla = siglaActual,
                    UnidadesDisponibles = new List<UnidadUsuarioDto>
                    {
                        new UnidadUsuarioDto { Sigla = "CO", NombreUnidad = "Colombia" },
                        new UnidadUsuarioDto { Sigla = "EC", NombreUnidad = "Ecuador" },
                        new UnidadUsuarioDto { Sigla = "PE", NombreUnidad = "Perú" },
                        new UnidadUsuarioDto { Sigla = "CL", NombreUnidad = "Chile" }
                    },
                    EncuestasRealizadas = encuestas,
                    CalidadGeneral = calidad,
                    Estadisticas = estadisticas
                };

                _logger.LogInformation("Dashboard encuestas consultado - Año: {Año}, Mes: {Mes}, Sigla: {Sigla}", añoActual, mesActual, siglaActual);

                return View(viewModel);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener dashboard de encuestas - Año: {Año}, Mes: {Mes}, Sigla: {Sigla}", año, mes, sigla);
                TempData["Error"] = "Error al cargar el dashboard de encuestas";
                return RedirectToAction("Index", "Home", new { area = "MBO" });
            }
        }

        /// <summary>
        /// Dashboard de calidad general con semáforo de % calidad
        /// </summary>
        [HttpGet]
        public async Task<IActionResult> Calidad(int? año, int? mes, string? sigla)
        {
            try
            {
                // Valores por defecto
                var añoActual = año ?? DateTime.Now.Year;
                var mesActual = mes ?? DateTime.Now.Month;
                var siglaActual = sigla ?? "CO";
                var userId = int.Parse(User.FindFirst("IdUsuario")?.Value ?? "0");

                // Obtener datos en paralelo (Task.WhenAll en service)
                var (calidad, ciudades, encuestadores) = await _service.ObtenerDashboardCalidadAsync(añoActual, mesActual, siglaActual, userId);

                var viewModel = new CampoCalidadViewModel
                {
                    Año = añoActual,
                    Mes = mesActual,
                    Sigla = siglaActual,
                    UnidadesDisponibles = new List<UnidadUsuarioDto>
                    {
                        new UnidadUsuarioDto { Sigla = "CO", NombreUnidad = "Colombia" },
                        new UnidadUsuarioDto { Sigla = "EC", NombreUnidad = "Ecuador" },
                        new UnidadUsuarioDto { Sigla = "PE", NombreUnidad = "Perú" },
                        new UnidadUsuarioDto { Sigla = "CL", NombreUnidad = "Chile" }
                    },
                    CalidadGeneral = calidad,
                    CalidadPorCiudad = ciudades,
                    CalidadPorEncuestador = encuestadores
                };

                _logger.LogInformation("Dashboard calidad consultado - Año: {Año}, Mes: {Mes}, Sigla: {Sigla}", añoActual, mesActual, siglaActual);

                return View(viewModel);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener dashboard de calidad - Año: {Año}, Mes: {Mes}, Sigla: {Sigla}", año, mes, sigla);
                TempData["Error"] = "Error al cargar el dashboard de calidad";
                return RedirectToAction("Index", "Home", new { area = "MBO" });
            }
        }

        /// <summary>
        /// Vista parcial: Tabla de calidad por ciudad (para AJAX)
        /// </summary>
        [HttpGet]
        public async Task<IActionResult> CalidadCiudad(int año, int mes, string sigla)
        {
            try
            {
                var userId = int.Parse(User.FindFirst("IdUsuario")?.Value ?? "0");
                var (_, ciudades, _) = await _service.ObtenerDashboardCalidadAsync(año, mes, sigla, userId);
                return PartialView("_CalidadCiudad", ciudades);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener calidad por ciudad - Año: {Año}, Mes: {Mes}, Sigla: {Sigla}", año, mes, sigla);
                return PartialView("_CalidadCiudad", Enumerable.Empty<CampoCiudadDto>());
            }
        }

        /// <summary>
        /// Vista parcial: Tabla de calidad por encuestador con semáforo (para AJAX)
        /// </summary>
        [HttpGet]
        public async Task<IActionResult> CalidadEncuestador(int año, int mes, string sigla)
        {
            try
            {
                var userId = int.Parse(User.FindFirst("IdUsuario")?.Value ?? "0");
                var (_, _, encuestadores) = await _service.ObtenerDashboardCalidadAsync(año, mes, sigla, userId);
                return PartialView("_CalidadEncuestador", encuestadores);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener calidad por encuestador - Año: {Año}, Mes: {Mes}, Sigla: {Sigla}", año, mes, sigla);
                return PartialView("_CalidadEncuestador", Enumerable.Empty<CampoEncuestadorDto>());
            }
        }

        /// <summary>
        /// Gestión de errores de campo con CRUD
        /// </summary>
        [HttpGet]
        public async Task<IActionResult> Errores(int? año, int? mes, string? sigla, int? idTrabajo, int? idEncuestador)
        {
            try
            {
                // Valores por defecto
                var añoActual = año ?? DateTime.Now.Year;
                var mesActual = mes ?? DateTime.Now.Month;

                // Obtener errores con filtros
                var errores = await _service.ObtenerErroresAsync(añoActual, mesActual, sigla, idTrabajo, idEncuestador);

                // Obtener catálogos en paralelo (Task.WhenAll en service)
                var (tiposError, ciudades, encuestadores) = await _service.ObtenerCatalogosAsync(sigla ?? "CO", null);

                var viewModel = new CampoErroresViewModel
                {
                    Año = añoActual,
                    Mes = mesActual,
                    Sigla = sigla,
                    IdTrabajo = idTrabajo,
                    IdEncuestador = idEncuestador,
                    Errores = errores?.ToList() ?? new List<CampoErrorDto>(),
                    TiposError = (tiposError as IEnumerable<CampoTipoErrorDto>)?.ToList() ?? new List<CampoTipoErrorDto>(),
                    Ciudades = (ciudades as IEnumerable<CampoCiudadDto>)?.ToList() ?? new List<CampoCiudadDto>(),
                    Encuestadores = (encuestadores as IEnumerable<CampoEncuestadorDto>)?.ToList() ?? new List<CampoEncuestadorDto>()
                };

                _logger.LogInformation("Gestión de errores consultada - Filtros: Año={Año}, Mes={Mes}, Sigla={Sigla}, IdTrabajo={IdTrabajo}, IdEncuestador={IdEncuestador}", 
                    añoActual, mesActual, sigla, idTrabajo, idEncuestador);

                return View(viewModel);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener gestión de errores");
                TempData["Error"] = "Error al cargar la gestión de errores";
                return RedirectToAction("Index", "Home", new { area = "MBO" });
            }
        }

        /// <summary>
        /// Modal para crear error (GET)
        /// </summary>
        [HttpGet]
        public async Task<IActionResult> CrearError()
        {
            try
            {
                var (tiposError, ciudades, encuestadores) = await _service.ObtenerCatalogosAsync("CO", null);

                ViewBag.TiposError = tiposError;
                ViewBag.Ciudades = ciudades;
                ViewBag.Encuestadores = encuestadores;
                
                return PartialView("_CrearEditarError", new CampoErrorDto());
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al abrir modal de creación de error");
                return PartialView("_CrearEditarError", new CampoErrorDto());
            }
        }

        /// <summary>
        /// Crear error (POST)
        /// </summary>
        [HttpPost]
        public async Task<IActionResult> CrearError(CampoErrorDto dto)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return Json(new { success = false, message = "Datos inválidos. Verifique todos los campos." });
                }

                var userId = int.Parse(User.FindFirst("IdUsuario")?.Value ?? "0");
                var (success, message, idError) = await _service.CrearErrorAsync(dto, userId);

                if (success)
                {
                    _logger.LogInformation("Error de campo creado con ID {IdError} por usuario {UserId}", idError, userId);
                }

                return Json(new { success, message, idError });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al crear error de campo - Dto: {@Dto}", dto);
                return Json(new { success = false, message = "Error inesperado al crear el registro" });
            }
        }

        /// <summary>
        /// Modal para editar error (GET)
        /// </summary>
        [HttpGet]
        public async Task<IActionResult> EditarError(int id)
        {
            try
            {
                var error = await _service.ObtenerErrorPorIdAsync(id);
                if (error == null)
                {
                    return NotFound();
                }

                var (tiposError, ciudades, encuestadores) = await _service.ObtenerCatalogosAsync(error.NombreCiudad ?? "CO", error.IdCiudad);

                ViewBag.TiposError = (tiposError as IEnumerable<CampoTipoErrorDto>)?.ToList() ?? new List<CampoTipoErrorDto>();
                ViewBag.Ciudades = (ciudades as IEnumerable<CampoCiudadDto>)?.ToList() ?? new List<CampoCiudadDto>();
                ViewBag.Encuestadores = (encuestadores as IEnumerable<CampoEncuestadorDto>)?.ToList() ?? new List<CampoEncuestadorDto>();

                return PartialView("_CrearEditarError", error);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al abrir modal de edición de error ID {IdError}", id);
                return NotFound();
            }
        }

        /// <summary>
        /// Editar error (POST)
        /// </summary>
        [HttpPost]
        public async Task<IActionResult> EditarError(CampoErrorDto dto)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return Json(new { success = false, message = "Datos inválidos. Verifique todos los campos." });
                }

                var userId = int.Parse(User.FindFirst("IdUsuario")?.Value ?? "0");
                var (success, message) = await _service.ActualizarErrorAsync(dto, userId);

                if (success)
                {
                    _logger.LogInformation("Error de campo {IdError} actualizado por usuario {UserId}", dto.IdError, userId);
                }

                return Json(new { success, message });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al actualizar error de campo ID {IdError}", dto.IdError);
                return Json(new { success = false, message = "Error inesperado al actualizar el registro" });
            }
        }

        /// <summary>
        /// Eliminar error (POST)
        /// </summary>
        [HttpPost]
        public async Task<IActionResult> EliminarError(int id)
        {
            try
            {
                var userId = int.Parse(User.FindFirst("IdUsuario")?.Value ?? "0");
                var (success, message) = await _service.EliminarErrorAsync(id, userId);

                if (success)
                {
                    _logger.LogInformation("Error de campo {IdError} eliminado por usuario {UserId}", id, userId);
                }

                return Json(new { success, message });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al eliminar error de campo ID {IdError}", id);
                return Json(new { success = false, message = "Error inesperado al eliminar el registro" });
            }
        }

        /// <summary>
        /// Vista de carga masiva de errores desde Excel
        /// </summary>
        [HttpGet]
        public IActionResult CargarErroresExcel()
        {
            ViewBag.AñoActual = DateTime.Now.Year;
            ViewBag.MesActual = DateTime.Now.Month;
            return View();
        }

        /// <summary>
        /// Procesar carga masiva de errores desde Excel (POST)
        /// </summary>
        [HttpPost]
        public async Task<IActionResult> CargarErroresExcel(IFormFile archivo, int año, int mes)
        {
            try
            {
                if (archivo == null || archivo.Length == 0)
                {
                    return Json(new { success = false, message = "Debe seleccionar un archivo Excel válido" });
                }

                // Validar extensión
                var extension = Path.GetExtension(archivo.FileName).ToLower();
                if (extension != ".xlsx" && extension != ".xls")
                {
                    return Json(new { success = false, message = "El archivo debe ser de tipo Excel (.xlsx o .xls)" });
                }

                // Validar tamaño (máx 10 MB)
                if (archivo.Length > 10 * 1024 * 1024)
                {
                    return Json(new { success = false, message = "El archivo no debe superar los 10 MB" });
                }

                // Procesar Excel con EPPlus
                var errores = new List<CampoErrorDto>();

                using (var stream = new MemoryStream())
                {
                    await archivo.CopyToAsync(stream);

                    using (var package = new ExcelPackage(stream))
                    {
                        var worksheet = package.Workbook.Worksheets.FirstOrDefault();
                        if (worksheet == null)
                        {
                            return Json(new { success = false, message = "El archivo Excel no contiene hojas de cálculo" });
                        }

                        // Leer desde fila 2 (fila 1 = encabezados)
                        var rowCount = worksheet.Dimension?.Rows ?? 0;
                        if (rowCount < 2)
                        {
                            return Json(new { success = false, message = "El archivo Excel no contiene datos (solo encabezados)" });
                        }

                        for (int row = 2; row <= rowCount; row++)
                        {
                            try
                            {
                                // Leer columnas (ajustar según formato Excel esperado)
                                // Columnas: IdTrabajo, IdEncuestador, IdCiudad, FechaEncuesta, NumeroEncuesta, IdTipoError, Observaciones
                                var error = new CampoErrorDto
                                {
                                    IdTrabajo = worksheet.Cells[row, 1].GetValue<int>(),
                                    IdEncuestador = worksheet.Cells[row, 2].GetValue<int>(),
                                    IdCiudad = worksheet.Cells[row, 3].GetValue<int>(),
                                    FechaEncuesta = worksheet.Cells[row, 4].GetValue<DateTime>(),
                                    NumeroEncuesta = worksheet.Cells[row, 5].GetValue<string>()?.Trim() ?? "",
                                    IdTipoError = worksheet.Cells[row, 6].GetValue<int>(),
                                    Observaciones = worksheet.Cells[row, 7].GetValue<string>()?.Trim() ?? "",
                                    Estado = "Pendiente"
                                };

                                errores.Add(error);
                            }
                            catch (Exception rowEx)
                            {
                                _logger.LogWarning(rowEx, "Error al leer fila {Row} del Excel", row);
                                // Continuar con siguiente fila
                            }
                        }
                    }
                }

                if (errores.Count == 0)
                {
                    return Json(new { success = false, message = "No se pudieron leer registros válidos del archivo Excel" });
                }

                // Llamar al servicio para carga masiva
                var userId = int.Parse(User.FindFirst("IdUsuario")?.Value ?? "0");
                var (success, message, insertados, erroresCount) = await _service.CargarErroresExcelAsync(errores, userId);

                if (success)
                {
                    _logger.LogInformation("Carga masiva Excel completada - Insertados: {Insertados}, Errores: {Errores}, Usuario: {UserId}", 
                        insertados, erroresCount, userId);
                }

                return Json(new 
                { 
                    success, 
                    message, 
                    insertados, 
                    errores = erroresCount,
                    totalRegistros = errores.Count
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al procesar carga masiva de errores desde Excel");
                return Json(new { success = false, message = "Error inesperado al procesar el archivo Excel" });
            }
        }
    }
}
