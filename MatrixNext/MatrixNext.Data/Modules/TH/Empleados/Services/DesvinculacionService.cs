using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;
using MatrixNext.Data.Adapters.TH;
using MatrixNext.Data.Models.TH;
using Microsoft.Extensions.Configuration;

namespace MatrixNext.Data.Services.TH
{
    /// <summary>
    /// Servicio para gestión de desvinculaciones de empleados
    /// Migrado desde DesvinculacionesEmpleadosGestionRRHH.aspx
    /// </summary>
    public class DesvinculacionService
    {
        private readonly DesvinculacionDataAdapter _adapter;
        private readonly IConfiguration _configuration;
        private readonly IHttpClientFactory _httpClientFactory;

        public DesvinculacionService(
            DesvinculacionDataAdapter adapter,
            IConfiguration configuration,
            IHttpClientFactory httpClientFactory)
        {
            _adapter = adapter ?? throw new ArgumentNullException(nameof(adapter));
            _configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
            _httpClientFactory = httpClientFactory ?? throw new ArgumentNullException(nameof(httpClientFactory));
        }

        #region Consulta de Desvinculaciones

        /// <summary>
        /// Obtiene listado paginado de procesos de desvinculación
        /// Equivalente a WebMethod DesvinculacionesEmpleadosEstatus
        /// </summary>
        public async Task<(bool success, string message, DesvinculacionesPaginadasDTO data)> 
            ObtenerDesvinculacionesPaginadas(DesvinculacionFiltroDTO filtro)
        {
            try
            {
                if (filtro.PageSize <= 0)
                {
                    filtro.PageSize = 10;
                }

                if (filtro.PageIndex < 0)
                {
                    filtro.PageIndex = 0;
                }

                var rows = await _adapter.ObtenerDesvinculacionesEstatus(
                    filtro.PageIndex,
                    filtro.PageSize,
                    filtro.TextoBuscado
                );

                var totalRegistros = rows.Count > 0 ? rows[0].CantidadTotalFilas : 0;
                var totalPaginas = filtro.PageSize > 0
                    ? (int)Math.Ceiling((double)totalRegistros / filtro.PageSize)
                    : 0;

                var resultado = new DesvinculacionesPaginadasDTO
                {
                    TotalRegistros = totalRegistros,
                    PaginaActual = filtro.PageIndex,
                    TamañoPagina = filtro.PageSize,
                    TotalPaginas = totalPaginas,
                    Desvinculaciones = new List<DesvinculacionEstatusDTO>(rows)
                };

                return (true, "Desvinculaciones obtenidas exitosamente", resultado);
            }
            catch (Exception ex)
            {
                return (false, $"Error al obtener desvinculaciones: {ex.Message}", null);
            }
        }

        #endregion

        #region Empleados Activos

        /// <summary>
        /// Obtiene empleados activos disponibles para desvinculación
        /// Equivalente a WebMethod EmpleadosActivos
        /// </summary>
        public async Task<(bool success, string message, IEnumerable<EmpleadoActivoDTO> data)> 
            ObtenerEmpleadosActivos()
        {
            try
            {
                var empleados = await _adapter.ObtenerEmpleadosActivos();
                return (true, "Empleados activos obtenidos exitosamente", empleados);
            }
            catch (Exception ex)
            {
                return (false, $"Error al obtener empleados activos: {ex.Message}", null);
            }
        }

        #endregion

        #region Iniciar Proceso

        /// <summary>
        /// Inicia proceso de desvinculación
        /// Equivalente a WebMethod IniciarProcesoDesvinculacion
        /// </summary>
        public async Task<(bool success, string message, int desvinculacionId)> 
            IniciarProcesoDesvinculacion(IniciarDesvinculacionDTO datos, long usuarioId)
        {
            // Validaciones
            if (string.IsNullOrWhiteSpace(datos.MotivosDesvinculacion))
            {
                return (false, "El motivo de desvinculación es requerido", 0);
            }

            if (datos.EmpleadoId <= 0)
            {
                return (false, "El ID del empleado es inválido", 0);
            }

            try
            {
                var fechaRegistro = DateTime.UtcNow.AddHours(-5);

                var desvinculacionIdLong = await _adapter.IniciarProcesoDesvinculacion(
                    datos.EmpleadoId,
                    datos.FechaRetiro,
                    datos.MotivosDesvinculacion.Trim(),
                    fechaRegistro,
                    usuarioId
                );

                // Legacy: dispara envío de correo via /Emails/...
                await DispararCorreoLegacyAsync(
                    $"/Emails/DesvinculacionEmpleadoSolicitudDiligenciamientoAreas.aspx?idProcesoDesvinculacion={desvinculacionIdLong}");

                var safeId = desvinculacionIdLong > int.MaxValue ? 0 : (int)desvinculacionIdLong;
                return (true, "Proceso de desvinculación iniciado exitosamente", safeId);
            }
            catch (Exception ex)
            {
                return (false, $"Error al iniciar proceso de desvinculación: {ex.Message}", 0);
            }
        }

        #endregion

        #region Evaluaciones

        /// <summary>
        /// Obtiene evaluaciones de áreas para una desvinculación
        /// Equivalente a WebMethod DesvinculacionEmpleadosEstatusEvaluacionesPor
        /// </summary>
        public async Task<(bool success, string message, IEnumerable<DesvinculacionEvaluacionDTO> data)> 
            ObtenerEvaluacionesPorDesvinculacion(int desvinculacionEmpleadoId)
        {
            if (desvinculacionEmpleadoId <= 0)
            {
                return (false, "El ID de desvinculación es inválido", null);
            }

            try
            {
                var evaluaciones = await _adapter.ObtenerEvaluacionesPorDesvinculacion(desvinculacionEmpleadoId);
                return (true, "Evaluaciones obtenidas exitosamente", evaluaciones);
            }
            catch (Exception ex)
            {
                return (false, $"Error al obtener evaluaciones: {ex.Message}", null);
            }
        }

        #endregion

        #region Gestión Área

        public async Task<(bool success, string message, IEnumerable<DesvinculacionEmpleadoPendientePorEvaluarAreaDTO> data)>
            ProcesosPendientesPorArea(int areaId)
        {
            if (areaId <= 0) return (false, "AreaId inválido", null);
            try
            {
                var data = await _adapter.PendientesPorEvaluarPorArea(areaId);
                return (true, "Procesos pendientes obtenidos", data);
            }
            catch (Exception ex)
            {
                return (false, $"Error al consultar pendientes por área: {ex.Message}", null);
            }
        }

        public async Task<(bool success, string message, IEnumerable<DesvinculacionEmpleadoPendienteEvaluarPorEvaluadorDTO> data)>
            ProcesosPendientesPorEvaluarUsuarioActual(long usuarioId)
        {
            if (usuarioId <= 0) return (false, "Usuario inválido", null);
            try
            {
                var data = await _adapter.PendientesPorEvaluarPorEvaluador(usuarioId);
                return (true, "Procesos pendientes obtenidos", data);
            }
            catch (Exception ex)
            {
                return (false, $"Error al consultar pendientes por evaluador: {ex.Message}", null);
            }
        }

        public async Task<(bool success, string message, IEnumerable<DesvinculacionEmpleadosAreaItemVerificarDTO> data)>
            ItemsVerificarPorArea(int areaId)
        {
            if (areaId <= 0) return (false, "AreaId inválido", null);
            try
            {
                var data = await _adapter.ItemsVerificarPor(areaId);
                return (true, "Items obtenidos", data);
            }
            catch (Exception ex)
            {
                return (false, $"Error al consultar items: {ex.Message}", null);
            }
        }

        public async Task<(bool success, string message, DesvinculacionEmpleadoEmpleadoInfoDTO data)>
            InformacionEmpleadoPor(int desvinculacionEmpleadoId)
        {
            if (desvinculacionEmpleadoId <= 0) return (false, "Id inválido", null);
            try
            {
                var data = await _adapter.ObtenerInformacionEmpleadoPor(desvinculacionEmpleadoId);
                return data == null
                    ? (false, "No se encontró información del empleado", null)
                    : (true, "Información obtenida", data);
            }
            catch (Exception ex)
            {
                return (false, $"Error al consultar información de empleado: {ex.Message}", null);
            }
        }

        public async Task<(bool success, string message)>
            GuardarEvaluacion(GuardarEvaluacionRequestDTO request, long usuarioId)
        {
            if (request == null) return (false, "Request inválido");
            if (request.DesvinculacionEmpleadoId <= 0) return (false, "DesvinculacionEmpleadoId inválido");
            if (request.AreaId <= 0) return (false, "AreaId inválido");
            if (string.IsNullOrWhiteSpace(request.Comentarios)) return (false, "Comentarios requeridos");
            if (usuarioId <= 0) return (false, "Usuario inválido");

            try
            {
                var evaluacion = new DesvinculacionEmpleadoEvaluacionAreaDTO
                {
                    DesvinculacionEmpleadoId = request.DesvinculacionEmpleadoId,
                    AreaId = request.AreaId,
                    Comentarios = request.Comentarios.Trim(),
                    FechaDiligenciamiento = DateTime.UtcNow.AddHours(-5),
                    UsuarioRegistra = usuarioId
                };

                await _adapter.GuardarEvaluacion(evaluacion);

                // Legacy: si no quedan evaluaciones pendientes, finalizar + correo fin
                if (!await TieneEvaluacionesPendientes(request.DesvinculacionEmpleadoId))
                {
                    await _adapter.FinalizarProceso(request.DesvinculacionEmpleadoId);
                    await DispararCorreoLegacyAsync(
                        $"/Emails/DesvinculacionEmpleadoFinProceso.aspx?idProcesoDesvinculacion={request.DesvinculacionEmpleadoId}");
                }

                return (true, "Evaluación guardada");
            }
            catch (Exception ex)
            {
                return (false, $"Error al guardar evaluación: {ex.Message}");
            }
        }

        public async Task<(bool success, string message, IEnumerable<DesvinculacionEmpleadoEvaluacionRealizadaPorEvaluadorDTO> data)>
            EvaluacionesRealizadasPorUsuarioActual(long usuarioId)
        {
            if (usuarioId <= 0) return (false, "Usuario inválido", null);
            try
            {
                var data = await _adapter.EvaluacionesRealizadasPorEvaluador(usuarioId);
                return (true, "Evaluaciones obtenidas", data);
            }
            catch (Exception ex)
            {
                return (false, $"Error al consultar evaluaciones realizadas: {ex.Message}", null);
            }
        }

        private async Task<bool> TieneEvaluacionesPendientes(int desvinculacionEmpleadoId)
        {
            var evaluaciones = await _adapter.ObtenerEvaluacionesPorDesvinculacion(desvinculacionEmpleadoId);
            foreach (var e in evaluaciones)
            {
                if (string.Equals(e.Estado, "Pendiente", StringComparison.OrdinalIgnoreCase))
                {
                    return true;
                }
            }
            return false;
        }

        #endregion

        #region Generación de PDF

        /// <summary>
        /// Genera PDF de formato de desvinculación
        /// Equivalente a WebMethod PDFFormato
        /// </summary>
        public async Task<(bool success, string message, string pdfBase64)> 
            GenerarPDFFormato(int desvinculacionEmpleadoId, string templatePath)
        {
            if (desvinculacionEmpleadoId <= 0)
            {
                return (false, "El ID de desvinculación es inválido", null);
            }

            try
            {
                var infoEmpleado = await _adapter.ObtenerInformacionEmpleadoPor(desvinculacionEmpleadoId);
                if (infoEmpleado == null)
                    return (false, "No se encontró información del empleado", null);

                var evaluaciones = await _adapter.ObtenerEvaluacionesPorDesvinculacion(desvinculacionEmpleadoId);

                // Leer plantilla HTML
                if (!File.Exists(templatePath))
                {
                    return (false, "Plantilla de PDF no encontrada", null);
                }

                var htmlTemplate = await File.ReadAllTextAsync(templatePath, Encoding.UTF8);

                // Reemplazar placeholders principales
                htmlTemplate = htmlTemplate.Replace("@EmployeeName", infoEmpleado.NombreEmpleadoCompleto);
                htmlTemplate = htmlTemplate.Replace("@IdentificacionNumber", infoEmpleado.EmpleadoId.ToString());
                htmlTemplate = htmlTemplate.Replace("@Position", infoEmpleado.Cargo ?? string.Empty);
                htmlTemplate = htmlTemplate.Replace("@DepartureDate", infoEmpleado.FechaRetiro.ToString("dd/MM/yyyy"));

                // Construir sección de evaluaciones
                var htmlEvaluaciones = new StringBuilder();
                const string htmlTemplateEvaluation = "<div class=\"evaluation\"><div class=\"title\">@TitleEvaluation</div><div>Observaciones:</div><div>@Comments</div><div>Evaluador:</div><div>@Evaluator</div><div>Fecha evaluación:</div><div>@DateEvaluation</div></div>";

                foreach (var evaluacion in evaluaciones)
                {
                    var htmlEvaluacion = htmlTemplateEvaluation;
                    htmlEvaluacion = htmlEvaluacion.Replace("@TitleEvaluation", evaluacion.NombreArea ?? string.Empty);
                    htmlEvaluacion = htmlEvaluacion.Replace("@Comments", evaluacion.Comentarios ?? string.Empty);
                    htmlEvaluacion = htmlEvaluacion.Replace("@Evaluator", evaluacion.NombreEvaluadorCompleto);
                    htmlEvaluacion = htmlEvaluacion.Replace("@DateEvaluation", evaluacion.FechaDiligenciamiento.HasValue
                        ? string.Format("{0:dd/MM/yyyy HH:mm}", evaluacion.FechaDiligenciamiento.Value)
                        : string.Empty);
                    htmlEvaluaciones.Append(htmlEvaluacion);
                }

                htmlTemplate = htmlTemplate.Replace("@EvaluationsContent", htmlEvaluaciones.ToString());

                // Legacy: HTMLToPDFGenerator -> POST a URLHTMLToPDFGenerator con { HTMLBase64String }
                var pdfBase64 = await ConvertHtmlToPdfBase64Async(htmlTemplate);
                return (true, "PDF generado exitosamente", pdfBase64);
            }
            catch (Exception ex)
            {
                return (false, $"Error al generar PDF: {ex.Message}", null);
            }
        }

        private async Task<string> ConvertHtmlToPdfBase64Async(string html)
        {
            var url = _configuration["LegacyServices:URLHTMLToPDFGenerator"]
                      ?? _configuration["URLHTMLToPDFGenerator"];

            if (string.IsNullOrWhiteSpace(url))
                throw new InvalidOperationException("No está configurado LegacyServices:URLHTMLToPDFGenerator");

            var htmlBase64 = Convert.ToBase64String(Encoding.UTF8.GetBytes(html));
            var payload = new { HTMLBase64String = htmlBase64 };

            var client = _httpClientFactory.CreateClient();
            var response = await client.PostAsJsonAsync(url, payload);
            response.EnsureSuccessStatusCode();

            var dict = await response.Content.ReadFromJsonAsync<Dictionary<string, object>>();
            if (dict == null || !dict.TryGetValue("pdfBase64String", out var pdfObj) || pdfObj == null)
                throw new InvalidOperationException("Respuesta inválida del servicio HTMLToPDF");

            return pdfObj.ToString();
        }

        private async Task DispararCorreoLegacyAsync(string relativeUrl)
        {
            var baseUrl = _configuration["LegacyServices:WebMatrixBaseUrl"];
            if (string.IsNullOrWhiteSpace(baseUrl))
            {
                // Si no hay baseUrl configurada, no intentamos disparar correos (evita fallos en dev/local).
                return;
            }

            var url = baseUrl.TrimEnd('/') + relativeUrl;
            var client = _httpClientFactory.CreateClient();
            using var resp = await client.GetAsync(url);
            resp.EnsureSuccessStatusCode();
        }

        #endregion
    }
}
