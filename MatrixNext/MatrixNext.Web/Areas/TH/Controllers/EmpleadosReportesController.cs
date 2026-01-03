using System;
using MatrixNext.Data.Helpers;
using MatrixNext.Data.Modules.TH.Empleados.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace MatrixNext.Web.Areas.TH.Controllers
{
    /// <summary>
    /// Controlador para reportes de empleados
    /// Equivalente a EmpleadosReporteDiligenciamiento.aspx y EmpleadosReporteGeneral.aspx
    /// Área: TH, Ruta base: /TH/EmpleadosReportes
    /// </summary>
    [Area("TH")]
    [Route("TH/[controller]")]
    [Authorize] // REGLA 11: Siempre requerir autenticación
    public class EmpleadosReportesController : Controller
    {
        private readonly EmpleadoService _service;
        private readonly ILogger<EmpleadosReportesController> _logger;

        public EmpleadosReportesController(EmpleadoService service, ILogger<EmpleadosReportesController> logger)
        {
            _service = service ?? throw new ArgumentNullException(nameof(service));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        #region Reporte de Diligenciamiento

        /// <summary>
        /// GET: /TH/EmpleadosReportes/Diligenciamiento
        /// Vista del reporte de estado de diligenciamiento
        /// Equivalente a EmpleadosReporteDiligenciamiento.aspx
        /// </summary>
        [HttpGet("Diligenciamiento")]
        public IActionResult Diligenciamiento()
        {
            try
            {
                ViewData["Title"] = "Reporte de Diligenciamiento de Empleados";
                return View();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al cargar vista de reporte de diligenciamiento");
                return View("Error");
            }
        }

        /// <summary>
        /// GET: /TH/EmpleadosReportes/Diligenciamiento/Data
        /// Datos JSON para el reporte de diligenciamiento
        /// Equivalente a WebMethod getReporteDiligenciamiento
        /// </summary>
        [HttpGet("Diligenciamiento/Data")]
        public async Task<IActionResult> GetDiligenciamientoData()
        {
            try
            {
                var (success, message, data) = await _service.ObtenerReporteDiligenciamiento();

                if (!success)
                {
                    return Json(new { success = false, message });
                }

                return Json(new { success = true, data });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener datos de reporte de diligenciamiento");
                return Json(new { success = false, message = "Ocurrió un error inesperado" });
            }
        }

        #endregion

        #region Reportes Generales (Excel)

        /// <summary>
        /// GET: /TH/EmpleadosReportes/General
        /// Vista para seleccionar y generar reportes Excel
        /// Equivalente a EmpleadosReporteGeneral.aspx
        /// </summary>
        [HttpGet("General")]
        public IActionResult General()
        {
            try
            {
                ViewData["Title"] = "Reportes Generales de Empleados";
                return View();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al cargar vista de reportes generales");
                return View("Error");
            }
        }

        /// <summary>
        /// POST: /TH/EmpleadosReportes/General/Export
        /// Genera y descarga reporte Excel según tipo solicitado
        /// Equivalente a btnGenerar_Click en EmpleadosReporteGeneral.aspx
        /// </summary>
        [HttpPost("General/Export")]
        public async Task<IActionResult> ExportGeneral([FromBody] ExportRequest request)
        {
            try
            {
                if (request == null || request.TipoReporte < 1 || request.TipoReporte > 5)
                {
                    return Json(new { success = false, message = "Tipo de reporte inválido" });
                }

                string fileName;
                string sheetName;
                string? columnNames;
                MemoryStream excelStream;

                switch (request.TipoReporte)
                {
                    case 1: // Información General
                        fileName = "RRHH-BD-Empleados-InformacionGeneral.xlsx";
                        sheetName = "InformacionGeneral";
                        columnNames = "TipoIdentificacion;id;Nombres;Apellidos;nombrePreferido;FechaNacimiento;Edad;" +
                                     "Genero;EstadoCivil;GrupoSanguineo;Nacionalidad;EmployeeId;BUNameITalent;" +
                                     "jobFunction;JefeInmediato;Sede;correoIpsos;FechaIngresoIpsos;TipoContrato;" +
                                     "Empresa;observaciones;SalarioActual;Banco;TipoCuenta;NumeroCuenta;EPS;" +
                                     "FondoPensiones;FondoCesantias;CajaCompensacion;ARL;NivelIngles;" +
                                     "CiudadResidencia;DireccionResidencia;BarrioResidencia;Localidad;NSE;" +
                                     "TelefonoFijo;TelefonoCelular;EmailPersonal;fechaCreacion;" +
                                     "fechaUltimaActualizacion;banda;level;Area;Cargo;Usuario;TallaCamiseta;" +
                                     "Ciudad_Municipio_Nacimiento;Departamento_Nacimiento";

                        var (success1, message1, data1) = await _service.ObtenerReporteInformacionGeneral();
                        if (!success1 || data1 == null)
                        {
                            return Json(new { success = false, message = message1 });
                        }
                        excelStream = ExcelHelper.GenerateExcel(data1, sheetName, columnNames);
                        break;

                    case 2: // Hijos
                        fileName = "RRHH-BD-Empleados-Hijos.xlsx";
                        sheetName = "Hijos";
                        columnNames = "CedulaEmpleado;Empleado;NombreHijo;Genero;FechaNacimiento";

                        var (success2, message2, data2) = await _service.ObtenerReporteHijos();
                        if (!success2 || data2 == null)
                        {
                            return Json(new { success = false, message = message2 });
                        }
                        excelStream = ExcelHelper.GenerateExcel(data2, sheetName, columnNames);
                        break;

                    case 3: // Educación
                        fileName = "RRHH-BD-Empleados-Educacion.xlsx";
                        sheetName = "Educacion";
                        columnNames = "CedulaEmpleado;Empleado;Titulo;Institucion;Pais;Ciudad;FechaInicio;FechaFin;Modalidad;Tipo;Estado";

                        var (success3, message3, data3) = await _service.ObtenerReporteEducacion();
                        if (!success3 || data3 == null)
                        {
                            return Json(new { success = false, message = message3 });
                        }
                        excelStream = ExcelHelper.GenerateExcel(data3, sheetName, columnNames);
                        break;

                    case 4: // Experiencia Laboral
                        fileName = "RRHH-BD-Empleados-ExperienciaLaboral.xlsx";
                        sheetName = "ExperienciaLaboral";
                        columnNames = "CedulaEmpleado;Empleado;Empresa;FechaInicio;FechaFin;Cargo;EnInvestigacionMercados";

                        var (success4, message4, data4) = await _service.ObtenerReporteExperienciaLaboral();
                        if (!success4 || data4 == null)
                        {
                            return Json(new { success = false, message = message4 });
                        }
                        excelStream = ExcelHelper.GenerateExcel(data4, sheetName, columnNames);
                        break;

                    case 5: // Contactos de Emergencia
                        fileName = "RRHH-BD-Empleados-ContactosEmergencia.xlsx";
                        sheetName = "ContactosEmergencia";
                        columnNames = "CedulaEmpleado;Empleado;ContactoEmergencia;telefonoCelular;parentescoTxt";

                        var (success5, message5, data5) = await _service.ObtenerReporteContactosEmergencia();
                        if (!success5 || data5 == null)
                        {
                            return Json(new { success = false, message = message5 });
                        }
                        excelStream = ExcelHelper.GenerateExcel(data5, sheetName, columnNames);
                        break;

                    default:
                        return Json(new { success = false, message = "Tipo de reporte no soportado" });
                }

                // Retornar archivo Excel
                return File(
                    excelStream.ToArray(),
                    "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
                    fileName
                );
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al generar reporte Excel tipo {TipoReporte}", request?.TipoReporte);
                return StatusCode(500, new { success = false, message = "Ocurrió un error inesperado al generar el reporte" });
            }
        }

        #endregion

        #region DTOs de Request

        public class ExportRequest
        {
            public int TipoReporte { get; set; }
        }

        #endregion
    }
}
