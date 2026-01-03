using System;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using MatrixNext.Data.Modules.TH.Empleados.Models;
using MatrixNext.Data.Modules.TH.Empleados.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace MatrixNext.Web.Areas.TH.Controllers
{
    /// <summary>
    /// Controlador para administración de empleados
    /// Equivalente a EmpleadosAdmin.aspx en WebMatrix
    /// Área: TH, Ruta base: /TH/Empleados
    /// </summary>
    [Area("TH")]
    [Route("TH/[controller]")]
    [Authorize] // REGLA 11: Siempre requerir autenticación
    public class EmpleadosController : Controller
    {
        private readonly EmpleadoService _service;
        private readonly ILogger<EmpleadosController> _logger;

        public EmpleadosController(EmpleadoService service, ILogger<EmpleadosController> logger)
        {
            _service = service ?? throw new ArgumentNullException(nameof(service));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        /// <summary>
        /// Helper para obtener ID del usuario actual desde Claims
        /// </summary>
        private long GetCurrentUserId()
        {
            var idClaim = User?.FindFirst(ClaimTypes.NameIdentifier)?.Value
                          ?? User?.FindFirst("sub")?.Value
                          ?? User?.FindFirst("userId")?.Value;

            if (long.TryParse(idClaim, out long userId))
            {
                return userId;
            }

            throw new InvalidOperationException("ID de usuario autenticado no disponible");
        }

        #region Vistas Principales

        /// <summary>
        /// GET: /TH/Empleados
        /// Vista principal con listado y filtros
        /// </summary>
        [HttpGet("")]
        public IActionResult Index()
        {
            try
            {
                ViewData["Title"] = "Administración de Empleados";
                return View();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al cargar vista Index de empleados");
                return View("Error");
            }
        }

        #endregion

        #region API JSON - Búsqueda y Consulta

        /// <summary>
        /// POST: /TH/Empleados/Search
        /// Busca empleados con filtros (equivalente a getEmpleados WebMethod)
        /// </summary>
        [HttpPost("Search")]
        public async Task<IActionResult> Search([FromBody] EmpleadoFiltroDTO filtro)
        {
            try
            {
                var (success, message, data) = await _service.ObtenerEmpleados(filtro);

                if (!success)
                {
                    return Json(new { success = false, message });
                }

                return Json(new { success = true, data });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error en búsqueda de empleados");
                return Json(new { success = false, message = "Ocurrió un error inesperado" });
            }
        }

        /// <summary>
        /// GET: /TH/Empleados/{identificacion}
        /// Obtiene empleado por identificación (equivalente a getEmpleadoPorIdentificacion)
        /// </summary>
        [HttpGet("{identificacion}")]
        public async Task<IActionResult> GetEmpleado(long identificacion)
        {
            try
            {
                var (success, message, data) = await _service.ObtenerEmpleadoPorIdentificacion(identificacion.ToString());

                if (!success)
                {
                    return Json(new { success = false, message });
                }

                return Json(new { success = true, data });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error al obtener empleado {identificacion}");
                return Json(new { success = false, message = "Ocurrió un error inesperado" });
            }
        }

        #endregion

        #region Actualización de Datos Maestros

        /// <summary>
        /// PUT: /TH/Empleados/DatosGenerales
        /// Actualiza datos generales del empleado (equivalente a updateDatosGenerales)
        /// Incluye manejo de foto en base64
        /// </summary>
        [HttpPut("DatosGenerales")]
        public async Task<IActionResult> ActualizarDatosGenerales([FromBody] ActualizarDatosGeneralesDTO datos)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return Json(new { success = false, message = "Datos inválidos" });
                }

                var usuarioId = GetCurrentUserId();
                var (success, message) = await _service.ActualizarDatosGenerales(datos, usuarioId);

                return Json(new { success, message });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al actualizar datos generales");
                return Json(new { success = false, message = "Ocurrió un error inesperado" });
            }
        }

        /// <summary>
        /// PUT: /TH/Empleados/DatosLaborales
        /// Actualiza datos laborales del empleado (equivalente a updateDatosLaborales)
        /// </summary>
        [HttpPut("DatosLaborales")]
        public async Task<IActionResult> ActualizarDatosLaborales([FromBody] ActualizarDatosLaboralesDTO datos)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return Json(new { success = false, message = "Datos inválidos" });
                }

                var (success, message) = await _service.ActualizarDatosLaborales(datos);

                return Json(new { success, message });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al actualizar datos laborales");
                return Json(new { success = false, message = "Ocurrió un error inesperado" });
            }
        }

        /// <summary>
        /// PUT: /TH/Empleados/DatosPersonales
        /// Actualiza datos personales del empleado (equivalente a updateDatosPersonales)
        /// </summary>
        [HttpPut("DatosPersonales")]
        public async Task<IActionResult> ActualizarDatosPersonales([FromBody] ActualizarDatosPersonalesDTO datos)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return Json(new { success = false, message = "Datos inválidos" });
                }

                var (success, message) = await _service.ActualizarDatosPersonales(datos);

                return Json(new { success, message });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al actualizar datos personales");
                return Json(new { success = false, message = "Ocurrió un error inesperado" });
            }
        }

        /// <summary>
        /// PUT: /TH/Empleados/Nomina
        /// Actualiza información de nómina del empleado (equivalente a updateNomina)
        /// </summary>
        [HttpPut("Nomina")]
        public async Task<IActionResult> ActualizarNomina([FromBody] ActualizarNominaDTO datos)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return Json(new { success = false, message = "Datos inválidos" });
                }

                var (success, message) = await _service.ActualizarNomina(datos);

                return Json(new { success, message });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al actualizar nómina");
                return Json(new { success = false, message = "Ocurrió un error inesperado" });
            }
        }

        /// <summary>
        /// PUT: /TH/Empleados/NivelIngles
        /// Actualiza nivel de inglés del empleado (equivalente a updateNivelIngles)
        /// </summary>
        [HttpPut("NivelIngles")]
        public async Task<IActionResult> ActualizarNivelIngles([FromBody] ActualizarNivelInglesDTO datos)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return Json(new { success = false, message = "Datos inválidos" });
                }

                var (success, message) = await _service.ActualizarNivelIngles(datos);

                return Json(new { success, message });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al actualizar nivel de inglés");
                return Json(new { success = false, message = "Ocurrió un error inesperado" });
            }
        }

        #endregion

        #region Experiencia Laboral

        /// <summary>
        /// GET: /TH/Empleados/{identificacion}/experiencia
        /// Obtiene experiencias laborales (equivalente a getExperienciasLaboralesPorIdentificacion)
        /// </summary>
        [HttpGet("{identificacion}/experiencia")]
        public async Task<IActionResult> GetExperiencias(long identificacion)
        {
            try
            {
                var (success, message, data) = await _service.ObtenerExperienciaLaboral(identificacion.ToString());

                if (!success)
                {
                    return Json(new { success = false, message });
                }

                return Json(new { success = true, data });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error al obtener experiencias de {identificacion}");
                return Json(new { success = false, message = "Ocurrió un error inesperado" });
            }
        }

        /// <summary>
        /// POST: /TH/Empleados/{identificacion}/experiencia
        /// Agrega experiencia laboral (equivalente a addExperienciaLaboral)
        /// </summary>
        [HttpPost("{identificacion}/experiencia")]
        public async Task<IActionResult> AddExperiencia(long identificacion, [FromBody] ExperienciaRequest request)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return Json(new { success = false, message = "Datos inválidos" });
                }

                var dto = new ExperienciaLaboralDTO
                {
                    Identificacion = identificacion.ToString(),
                    Empresa = request.Empresa,
                    FechaInicio = request.FechaInicio,
                    FechaFin = request.FechaFin == default ? null : request.FechaFin,
                    Cargo = request.Cargo
                };

                var (success, message) = await _service.GuardarExperienciaLaboral(dto);

                return Json(new { success, message });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error al agregar experiencia para {identificacion}");
                return Json(new { success = false, message = "Ocurrió un error inesperado" });
            }
        }

        /// <summary>
        /// DELETE: /TH/Empleados/experiencia/{id}
        /// Elimina experiencia laboral (equivalente a deleteExperienciaLaboral)
        /// </summary>
        [HttpDelete("experiencia/{id}")]
        public async Task<IActionResult> DeleteExperiencia(int id)
        {
            try
            {
                var (success, message) = await _service.EliminarExperienciaLaboral(id);
                return Json(new { success, message });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error al eliminar experiencia {id}");
                return Json(new { success = false, message = "Ocurrió un error inesperado" });
            }
        }

        #endregion

        #region Educación

        [HttpGet("{identificacion}/educacion")]
        public async Task<IActionResult> GetEducacion(long identificacion)
        {
            try
            {
                var (success, message, data) = await _service.ObtenerEducacion(identificacion.ToString());
                return Json(new { success, message, data });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error al obtener educación de {identificacion}");
                return Json(new { success = false, message = "Ocurrió un error inesperado" });
            }
        }

        [HttpPost("{identificacion}/educacion")]
        public async Task<IActionResult> AddEducacion(long identificacion, [FromBody] EducacionRequest request)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return Json(new { success = false, message = "Datos inválidos" });
                }

                var dto = new EducacionDTO
                {
                    Identificacion = identificacion.ToString(),
                    NivelEducativo = request.Tipo.ToString(),
                    TituloObtenido = request.Titulo,
                    Institucion = request.Institucion,
                    FechaInicio = request.FechaInicio,
                    FechaFin = request.FechaFin,
                    Graduado = request.Estado == 1,
                    TarjetaProfesional = null
                };

                var (success, message) = await _service.GuardarEducacion(dto);

                return Json(new { success, message });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error al agregar educación para {identificacion}");
                return Json(new { success = false, message = "Ocurrió un error inesperado" });
            }
        }

        [HttpDelete("educacion/{id}")]
        public async Task<IActionResult> DeleteEducacion(int id)
        {
            try
            {
                var (success, message) = await _service.EliminarEducacion(id);
                return Json(new { success, message });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error al eliminar educación {id}");
                return Json(new { success = false, message = "Ocurrió un error inesperado" });
            }
        }

        #endregion

        #region Hijos

        [HttpGet("{identificacion}/hijos")]
        public async Task<IActionResult> GetHijos(long identificacion)
        {
            try
            {
                var (success, message, data) = await _service.ObtenerHijos(identificacion.ToString());
                return Json(new { success, message, data });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error al obtener hijos de {identificacion}");
                return Json(new { success = false, message = "Ocurrió un error inesperado" });
            }
        }

        [HttpPost("{identificacion}/hijos")]
        public async Task<IActionResult> AddHijo(long identificacion, [FromBody] HijoRequest request)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return Json(new { success = false, message = "Datos inválidos" });
                }

                var dto = new HijoDTO
                {
                    IdentificacionEmpleado = identificacion.ToString(),
                    NombreCompleto = $"{request.Nombres} {request.Apellidos}",
                    Genero = request.Genero.ToString(),
                    FechaNacimiento = request.FechaNacimiento
                };

                var (success, message) = await _service.GuardarHijo(dto);

                return Json(new { success, message });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error al agregar hijo para {identificacion}");
                return Json(new { success = false, message = "Ocurrió un error inesperado" });
            }
        }

        [HttpDelete("hijos/{id}")]
        public async Task<IActionResult> DeleteHijo(int id)
        {
            try
            {
                var (success, message) = await _service.EliminarHijo(id);
                return Json(new { success, message });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error al eliminar hijo {id}");
                return Json(new { success = false, message = "Ocurrió un error inesperado" });
            }
        }

        #endregion

        #region Contactos de Emergencia

        [HttpGet("{identificacion}/contactos-emergencia")]
        public async Task<IActionResult> GetContactosEmergencia(long identificacion)
        {
            try
            {
                var (success, message, data) = await _service.ObtenerContactosEmergencia(identificacion.ToString());
                return Json(new { success, message, data });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error al obtener contactos de emergencia de {identificacion}");
                return Json(new { success = false, message = "Ocurrió un error inesperado" });
            }
        }

        [HttpPost("{identificacion}/contactos-emergencia")]
        public async Task<IActionResult> AddContactoEmergencia(long identificacion, [FromBody] ContactoEmergenciaRequest request)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return Json(new { success = false, message = "Datos inválidos" });
                }

                var dto = new ContactoEmergenciaDTO
                {
                    IdentificacionEmpleado = identificacion.ToString(),
                    NombreCompleto = $"{request.Nombres} {request.Apellidos}",
                    Parentesco = request.Parentesco.ToString(),
                    Telefono = request.TelefonoFijo?.ToString() ?? request.TelefonoCelular?.ToString() ?? string.Empty,
                    Celular = request.TelefonoCelular?.ToString(),
                    Direccion = string.Empty
                };

                var (success, message) = await _service.GuardarContactoEmergencia(dto);

                return Json(new { success, message });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error al agregar contacto de emergencia para {identificacion}");
                return Json(new { success = false, message = "Ocurrió un error inesperado" });
            }
        }

        [HttpDelete("contactos-emergencia/{id}")]
        public async Task<IActionResult> DeleteContactoEmergencia(int id)
        {
            try
            {
                var (success, message) = await _service.EliminarContactoEmergencia(id);
                return Json(new { success, message });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error al eliminar contacto de emergencia {id}");
                return Json(new { success = false, message = "Ocurrió un error inesperado" });
            }
        }

        #endregion

        #region Promociones

        [HttpGet("{identificacion}/promociones")]
        public async Task<IActionResult> GetPromociones(long identificacion)
        {
            try
            {
                var (success, message, data) = await _service.ObtenerPromociones(identificacion.ToString());
                return Json(new { success, message, data });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error al obtener promociones de {identificacion}");
                return Json(new { success = false, message = "Ocurrió un error inesperado" });
            }
        }

        [HttpPost("{identificacion}/promociones")]
        public async Task<IActionResult> AddPromocion(long identificacion, [FromBody] PromocionRequest request)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return Json(new { success = false, message = "Datos inválidos" });
                }

                var usuarioId = GetCurrentUserId();

                var (empleadoEncontrado, _, empleadoDetalle) = await _service.ObtenerEmpleadoPorIdentificacion(identificacion.ToString());
                if (!empleadoEncontrado || empleadoDetalle?.IdCargo is null)
                {
                    return Json(new { success = false, message = "No se pudo obtener el cargo actual del empleado" });
                }

                var dto = new PromocionDTO
                {
                    Identificacion = identificacion.ToString(),
                    CargoAnterior = empleadoDetalle.IdCargo.Value,
                    NombreCargoAnterior = empleadoDetalle.NombreCargo ?? string.Empty,
                    CargoNuevo = request.NuevoCargoId,
                    NombreCargoNuevo = string.Empty,
                    FechaCambio = request.FechaPromocion,
                    Motivo = null,
                    Observaciones = null
                };

                var (success, message) = await _service.GuardarPromocion(dto, usuarioId.ToString());

                return Json(new { success, message });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error al agregar promoción para {identificacion}");
                return Json(new { success = false, message = "Ocurrió un error inesperado" });
            }
        }

        // TODO: Implementar EliminarPromocion en el servicio
        /*
        [HttpDelete("promociones/{id}")]
        public async Task<IActionResult> DeletePromocion(int id)
        {
            try
            {
                var (success, message) = await _service.EliminarPromocion(id);
                return Json(new { success, message });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error al eliminar promoción {id}");
                return Json(new { success = false, message = "Ocurrió un error inesperado" });
            }
        }
        */

        #endregion

        #region Salarios

        [HttpGet("{identificacion}/salarios")]
        public async Task<IActionResult> GetSalarios(long identificacion)
        {
            try
            {
                var (success, message, data) = await _service.ObtenerSalarios(identificacion.ToString());
                return Json(new { success, message, data });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error al obtener salarios de {identificacion}");
                return Json(new { success = false, message = "Ocurrió un error inesperado" });
            }
        }

        [HttpPost("{identificacion}/salarios")]
        public async Task<IActionResult> AddSalario(long identificacion, [FromBody] SalarioRequest request)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return Json(new { success = false, message = "Datos inválidos" });
                }

                var usuarioId = GetCurrentUserId();

                var (salariosOk, _, salarios) = await _service.ObtenerSalarios(identificacion.ToString());
                var salarioAnterior = salariosOk
                    ? salarios?.OrderByDescending(s => s.FechaCambio).FirstOrDefault()?.SalarioNuevo ?? 0m
                    : 0m;

                if (salarioAnterior <= 0)
                {
                    return Json(new { success = false, message = "No se pudo determinar el salario anterior" });
                }

                if (salarioAnterior == request.Salario)
                {
                    return Json(new { success = false, message = "El salario nuevo debe ser diferente al salario anterior" });
                }

                var dto = new SalarioDTO
                {
                    Identificacion = identificacion.ToString(),
                    SalarioAnterior = salarioAnterior,
                    SalarioNuevo = request.Salario,
                    FechaCambio = request.FechaAplicacion,
                    Motivo = request.MotivoCambio?.ToString(),
                    Observaciones = null
                };

                var (success, message) = await _service.GuardarSalario(dto, usuarioId.ToString());

                return Json(new { success, message });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error al agregar salario para {identificacion}");
                return Json(new { success = false, message = "Ocurrió un error inesperado" });
            }
        }

        // TODO: Implementar EliminarSalario en el servicio
        /*
        [HttpDelete("salarios/{id}")]
        public async Task<IActionResult> DeleteSalario(int id)
        {
            try
            {
                var (success, message) = await _service.EliminarSalario(id);
                return Json(new { success, message });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error al eliminar salario {id}");
                return Json(new { success = false, message = "Ocurrió un error inesperado" });
            }
        }
        */

        #endregion

        #region Retiro y Reintegro

        /// <summary>
        /// POST: /TH/Empleados/{identificacion}/retiro
        /// Retira empleado (equivalente a retirarEmpleado)
        /// </summary>
        [HttpPost("{identificacion}/retiro")]
        public async Task<IActionResult> Retirar(long identificacion, [FromBody] RetiroRequest request)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return Json(new { success = false, message = "Datos inválidos" });
                }

                var usuarioId = GetCurrentUserId();

                var (success, message) = await _service.RetirarEmpleado(
                    identificacion.ToString(),
                    request.FechaRetiro,
                    request.Observacion,
                    usuarioId.ToString()
                );

                return Json(new { success, message });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error al retirar empleado {identificacion}");
                return Json(new { success = false, message = "Ocurrió un error inesperado" });
            }
        }

        /// <summary>
        /// POST: /TH/Empleados/{identificacion}/reintegro
        /// Reintegra empleado (equivalente a reintegrarEmpleado)
        /// </summary>
        [HttpPost("{identificacion}/reintegro")]
        public async Task<IActionResult> Reintegrar(long identificacion, [FromBody] ReintegroRequest request)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return Json(new { success = false, message = "Datos inválidos" });
                }

                var usuarioId = GetCurrentUserId();

                var (success, message) = await _service.ReintegrarEmpleado(
                    identificacion.ToString(),
                    request.FechaReintegro,
                    usuarioId.ToString()
                );

                return Json(new { success, message });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error al reintegrar empleado {identificacion}");
                return Json(new { success = false, message = "Ocurrió un error inesperado" });
            }
        }

        #endregion

        #region DTOs de Request

        public class ExperienciaRequest
        {
            public string Empresa { get; set; }
            public DateTime FechaInicio { get; set; }
            public DateTime FechaFin { get; set; }
            public string Cargo { get; set; }
            public bool EsInvestigacion { get; set; }
        }

        public class EducacionRequest
        {
            public ushort Tipo { get; set; }
            public string Titulo { get; set; }
            public string Institucion { get; set; }
            public string Pais { get; set; }
            public string Ciudad { get; set; }
            public DateTime FechaInicio { get; set; }
            public DateTime? FechaFin { get; set; }
            public ushort Modalidad { get; set; }
            public ushort Estado { get; set; }
        }

        public class HijoRequest
        {
            public string Nombres { get; set; }
            public string Apellidos { get; set; }
            public byte Genero { get; set; }
            public DateTime FechaNacimiento { get; set; }
        }

        public class ContactoEmergenciaRequest
        {
            public string Nombres { get; set; }
            public string Apellidos { get; set; }
            public byte Parentesco { get; set; }
            public long? TelefonoFijo { get; set; }
            public long? TelefonoCelular { get; set; }
        }

        public class PromocionRequest
        {
            public byte NuevaAreaId { get; set; }
            public byte NuevaBandaId { get; set; }
            public short NuevoCargoId { get; set; }
            public byte NuevoLevelId { get; set; }
            public DateTime FechaPromocion { get; set; }
        }

        public class SalarioRequest
        {
            public DateTime FechaAplicacion { get; set; }
            public ushort? MotivoCambio { get; set; }
            public decimal Salario { get; set; }
            public ushort? Tipo { get; set; }
        }

        public class RetiroRequest
        {
            public DateTime FechaRetiro { get; set; }
            public string Observacion { get; set; }
        }

        public class ReintegroRequest
        {
            public DateTime FechaReintegro { get; set; }
        }

        #endregion
    }
}
