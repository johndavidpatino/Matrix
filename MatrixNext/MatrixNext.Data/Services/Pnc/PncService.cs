using MatrixNext.Data.Adapters.Pnc;
using MatrixNext.Data.Models.ViewModels.Pnc;
using MatrixNext.Data.Models.ViewModels.Pnc.DTOs;
using MatrixNext.Data.Services;
using Microsoft.Extensions.Logging;

namespace MatrixNext.Data.Services.Pnc
{
    /// <summary>
    /// Servicio de Producto No Conforme (PNC)
    /// Sistema de GestiÃ³n de Calidad ISO 9001
    /// Implementa lÃ³gica de negocio, validaciones y orquestaciÃ³n
    /// </summary>
    public class PncService : IPncService
    {
        private readonly IPncAdapter _adapter;
        private readonly IEmailQueueService _emailQueue;
        private readonly ILogger<PncService> _logger;

        public PncService(
            IPncAdapter adapter,
            IEmailQueueService emailQueue,
            ILogger<PncService> logger)
        {
            _adapter = adapter;
            _emailQueue = emailQueue;
            _logger = logger;
        }

        // ============= CONSULTAS =============

        public async Task<(bool success, PncFiltrosVM data, string message)> ObtenerPnc(PncFiltrosVM filtros)
        {
            try
            {
                List<PncObtenerProductoNoConformeDTO> pncs;

                // Aplicar filtros
                if (!string.IsNullOrWhiteSpace(filtros.JobBook))
                {
                    pncs = await _adapter.ObtenerPorJobBook(filtros.JobBook);
                }
                else
                {
                    pncs = await _adapter.ObtenerTodos();
                }

                // Filtros adicionales en memoria (mÃ¡s eficiente que SQL dinÃ¡mico)
                var query = pncs.AsQueryable();

                if (filtros.FechaDesde.HasValue)
                    query = query.Where(p => p.FechaReclamo >= filtros.FechaDesde.Value);

                if (filtros.FechaHasta.HasValue)
                    query = query.Where(p => p.FechaReclamo <= filtros.FechaHasta.Value);

                if (filtros.IdFuenteReclamo.HasValue)
                    query = query.Where(p => p.FuenteReclamo == filtros.IdFuenteReclamo.Value);

                if (filtros.IdCategoria.HasValue)
                    query = query.Where(p => p.Categoria == filtros.IdCategoria.Value);

                if (filtros.Estado.HasValue)
                {
                    if (filtros.Estado == EstadoPncEnum.Abiertos)
                        query = query.Where(p => p.Cerrado == false);
                    else if (filtros.Estado == EstadoPncEnum.Cerrados)
                        query = query.Where(p => p.Cerrado == true);
                }

                // Mapear a ListadoVM
                var resultados = query.Select(p => new ProductoNoConformeListadoVM
                {
                    Id = p.Id,
                    JobBook = p.JobBook ?? string.Empty,
                    NombreEstudio = p.NombreEstudio,
                    NombreCliente = p.NombreCliente,
                    FechaReclamo = p.FechaReclamo ?? DateTime.MinValue,
                    NombreReporta = p.NombreReporta,
                    FuenteReclamo = p.DescripcionFuenteReclamo,
                    Categoria = p.DescripcionCategoria,
                    Cerrado = p.Cerrado ?? false,
                    FechaCierre = p.FechaCierre,
                    DescripcionCorta = p.Descripcion != null && p.Descripcion.Length > 0 
                        ? p.Descripcion.Substring(0, Math.Min(100, p.Descripcion.Length)) 
                        : string.Empty
                }).ToList();

                // PaginaciÃ³n
                filtros.TotalRegistros = resultados.Count;
                filtros.Resultados = resultados
                    .Skip((filtros.PaginaActual - 1) * filtros.RegistrosPorPagina)
                    .Take(filtros.RegistrosPorPagina)
                    .ToList();

                return (true, filtros, $"{filtros.TotalRegistros} PNC encontrados");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener PNC con filtros");
                return (false, filtros, "Error al obtener PNC. Por favor intente nuevamente.");
            }
        }

        public async Task<(bool success, ProductoNoConformeDetalleVM? data, string message)> ObtenerPncById(int idPnc)
        {
            try
            {
                if (idPnc <= 0)
                    return (false, null, "ID de PNC invÃ¡lido");

                var detalle = await _adapter.ObtenerPorId(idPnc);
                if (detalle == null)
                    return (false, null, "PNC no encontrado");

                return (true, detalle, "PNC obtenido correctamente");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener PNC {IdPnc}", idPnc);
                return (false, null, "Error al obtener PNC. Por favor intente nuevamente.");
            }
        }

        public async Task<(bool success, PncSeguimientoVM data, string message)> ObtenerSeguimiento()
        {
            try
            {
                var seguimiento = new PncSeguimientoVM();
                var todosPnc = await _adapter.ObtenerTodos();

                // Calcular KPIs
                seguimiento.TotalPncActivos = todosPnc.Count(p => p.Cerrado == false);
                
                var inicioMes = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);
                seguimiento.PncCerradosEsteMes = todosPnc.Count(p => 
                    p.Cerrado == true && p.FechaCierre >= inicioMes);

                // Para acciones vencidas necesitamos consultar detalle
                // TODO: Optimizar con query directo a BD
                var accionesVencidas = 0;
                var accionesProximasVencer = 0;

                foreach (var pnc in todosPnc.Where(p => p.Cerrado == false))
                {
                    var causas = await _adapter.ObtenerCausas(pnc.Id);
                    foreach (var causa in causas)
                    {
                        var acciones = await _adapter.ObtenerAcciones(pnc.Id, causa.Id);
                        accionesVencidas += acciones.Count(a => 
                            a.FechaPlaneada < DateTime.Now && a.FechaEjecucion == null);
                        accionesProximasVencer += acciones.Count(a =>
                            a.FechaPlaneada.HasValue &&
                            (a.FechaPlaneada.Value - DateTime.Now).Days <= 3 &&
                            a.FechaEjecucion == null);
                    }
                }

                seguimiento.AccionesVencidas = accionesVencidas;
                seguimiento.AccionesProximasVencer = accionesProximasVencer;

                return (true, seguimiento, "Seguimiento obtenido correctamente");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener seguimiento PNC");
                return (false, new PncSeguimientoVM(), "Error al obtener seguimiento. Por favor intente nuevamente.");
            }
        }

        // ============= CATÃLOGOS =============

        public async Task<(bool success, PncCatalogosDto data, string message)> ObtenerCatalogos()
        {
            try
            {
                var catalogos = new PncCatalogosDto
                {
                    FuentesReclamo = await _adapter.ObtenerFuentesReclamo(),
                    Categorias = await _adapter.ObtenerCategorias(),
                    TiposAccion = await _adapter.ObtenerTiposAccion()
                };

                return (true, catalogos, "CatÃ¡logos obtenidos correctamente");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener catÃ¡logos PNC");
                return (false, new PncCatalogosDto(), "Error al obtener catálogos. Por favor intente nuevamente.");
            }
        }

        // ============= CRUD PNC =============

        public async Task<(bool success, int id, string message)> CrearPnc(CrearPncVM modelo, long idUsuario)
        {
            try
            {
                // VALIDACIÃ“N 1: Datos requeridos
                if (string.IsNullOrWhiteSpace(modelo.JobBook))
                    return (false, 0, "JobBook es requerido");

                if (modelo.IdReporta <= 0)
                    return (false, 0, "Debe seleccionar quiÃ©n reporta");

                if (modelo.FuenteReclamo <= 0)
                    return (false, 0, "Fuente de reclamo es requerida");

                if (modelo.Categoria <= 0)
                    return (false, 0, "CategorÃ­a es requerida");

                if (string.IsNullOrWhiteSpace(modelo.Descripcion))
                    return (false, 0, "DescripciÃ³n del problema es requerida");

                // VALIDACIÃ“N 2: FechaReclamo no puede ser futura (REGLA ISO 9001)
                if (modelo.FechaReclamo > DateTime.Now)
                    return (false, 0, "La fecha del reclamo no puede ser futura");

                // Mapear a ViewModel
                var pnc = new ProductoNoConformeVM
                {
                    IdEstudio = modelo.IdEstudio,
                    IdTrabajo = modelo.IdTrabajo,
                    JobBook = modelo.JobBook,
                    FechaReclamo = modelo.FechaReclamo,
                    IdReporta = modelo.IdReporta,
                    IdUnidad = modelo.IdUnidad,
                    IdClienteExterno = modelo.IdClienteExterno,
                    FuenteReclamo = modelo.FuenteReclamo,
                    Categoria = modelo.Categoria,
                    Tarea = modelo.Tarea,
                    Descripcion = modelo.Descripcion,
                    Usuario = idUsuario,
                    FechaGrabacion = DateTime.Now
                };

                // Insertar PNC
                var idPnc = await _adapter.InsertarPnc(pnc);
                if (idPnc <= 0)
                    return (false, 0, "Error al crear PNC");

                // Insertar causas si vienen en el modelo (opcional)
                if (modelo.Causas != null && modelo.Causas.Any())
                {
                    foreach (var causaTexto in modelo.Causas.Where(c => !string.IsNullOrWhiteSpace(c)))
                    {
                        var causa = new ProductoNoConformeCausaVM
                        {
                            IdPNC = idPnc,
                            CausaRaiz = causaTexto
                        };
                        await _adapter.InsertarCausa(causa);
                    }
                }

                // Enviar notificaciÃ³n email (fire-and-forget)
                _ = Task.Run(async () =>
                {
                    try
                    {
                        await EnviarNotificacionNuevoPnc(idPnc);
                    }
                    catch (Exception ex)
                    {
                        _logger.LogError(ex, "Error al enviar notificaciÃ³n PNC {IdPnc}", idPnc);
                    }
                });

                _logger.LogInformation("PNC creado: {IdPnc} por usuario {IdUsuario}", idPnc, idUsuario);
                return (true, idPnc, "PNC creado correctamente");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al crear PNC");
                return (false, 0, "Error al crear PNC. Por favor intente nuevamente.");
            }
        }

        public async Task<(bool success, string message)> ActualizarPnc(ProductoNoConformeVM pnc, long idUsuario)
        {
            try
            {
                // Validar que PNC existe
                var existente = await _adapter.ObtenerPorId(pnc.Id);
                if (existente == null)
                    return (false, "PNC no encontrado");

                // VALIDACIÃ“N: No permitir actualizar PNC cerrado
                if (existente.Pnc.Cerrado)
                    return (false, "No se puede actualizar un PNC cerrado");

                // Validar datos
                if (string.IsNullOrWhiteSpace(pnc.JobBook))
                    return (false, "JobBook es requerido");

                if (pnc.FechaReclamo > DateTime.Now)
                    return (false, "La fecha del reclamo no puede ser futura");

                pnc.FechaActualizacion = DateTime.Now;
                var actualizado = await _adapter.ActualizarPnc(pnc);

                if (!actualizado)
                    return (false, "Error al actualizar PNC");

                _logger.LogInformation("PNC actualizado: {Id} por usuario {IdUsuario}", pnc.Id, idUsuario);
                return (true, "PNC actualizado correctamente");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al actualizar PNC {Id}", pnc.Id);
                return (false, "Error al procesar la solicitud. Por favor intente nuevamente.");
            }
        }

        public async Task<(bool success, string message)> CerrarPnc(int idPnc, long idUsuario)
        {
            try
            {
                // Validar que puede cerrarse
                var (canClose, reason) = await ValidarCierrePnc(idPnc);
                if (!canClose)
                    return (false, reason);

                var cerrado = await _adapter.CerrarPnc(idPnc, idUsuario);
                if (!cerrado)
                    return (false, "Error al cerrar PNC");

                // Enviar notificaciÃ³n (fire-and-forget)
                _ = Task.Run(async () =>
                {
                    try
                    {
                        await EnviarNotificacionPncCerrado(idPnc);
                    }
                    catch (Exception ex)
                    {
                        _logger.LogError(ex, "Error al enviar notificaciÃ³n cierre PNC {IdPnc}", idPnc);
                    }
                });

                _logger.LogInformation("PNC cerrado: {IdPnc} por usuario {IdUsuario}", idPnc, idUsuario);
                return (true, "PNC cerrado correctamente");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al cerrar PNC {IdPnc}", idPnc);
                return (false, "Error al procesar la solicitud. Por favor intente nuevamente.");
            }
        }

        // ============= CAUSAS =============

        public async Task<(bool success, int id, string message)> AgregarCausa(AgregarCausaPncVM modelo, long idUsuario)
        {
            try
            {
                // Validar PNC existe
                var pnc = await _adapter.ObtenerPorId(modelo.IdPNC);
                if (pnc == null)
                    return (false, 0, "PNC no encontrado");

                if (pnc.Pnc.Cerrado)
                    return (false, 0, "No se pueden agregar causas a un PNC cerrado");

                if (string.IsNullOrWhiteSpace(modelo.CausaRaiz))
                    return (false, 0, "La causa raÃ­z es requerida");

                var causa = new ProductoNoConformeCausaVM
                {
                    IdPNC = modelo.IdPNC,
                    CausaRaiz = modelo.CausaRaiz
                };

                var idCausa = await _adapter.InsertarCausa(causa);
                if (idCausa <= 0)
                    return (false, 0, "Error al agregar causa");

                _logger.LogInformation("Causa agregada: {IdCausa} al PNC {IdPnc}", idCausa, modelo.IdPNC);
                return (true, idCausa, "Causa agregada correctamente");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al agregar causa al PNC {IdPnc}", modelo.IdPNC);
                return (false, 0, "Error al agregar causa. Por favor intente nuevamente.");
            }
        }

        public async Task<(bool success, string message)> ActualizarCausa(ProductoNoConformeCausaVM causa)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(causa.CausaRaiz))
                    return (false, "La causa raíz es requerida");

                var actualizado = await _adapter.ActualizarCausa(causa);
                if (!actualizado)
                    return (false, "Error al procesar la solicitud. Por favor intente nuevamente.");

                return (true, "Causa actualizada correctamente");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al actualizar causa");
                return (false, "Error al actualizar causa. Por favor intente nuevamente.");
            }
        }

        public async Task<(bool success, string message)> EliminarCausa(int idCausa)
        {
            try
            {
                // Nota: El Adapter ya maneja el CASCADE DELETE de acciones
                var eliminado = await _adapter.EliminarCausa(idCausa);
                if (!eliminado)
                    return (false, "Error al eliminar causa");

                _logger.LogInformation("Causa eliminada: {IdCausa}", idCausa);
                return (true, "Causa eliminada correctamente");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al eliminar causa {IdCausa}", idCausa);
                return (false, "Error al procesar la solicitud. Por favor intente nuevamente.");
            }
        }

        // ============= ACCIONES =============

        public async Task<(bool success, int id, string message)> AgregarAccion(AgregarAccionPncVM modelo, long idUsuario)
        {
            try
            {
                // Validar datos
                if (modelo.IdPNC <= 0 || modelo.IdCausa <= 0)
                    return (false, 0, "PNC y Causa son requeridos");

                if (string.IsNullOrWhiteSpace(modelo.Accion))
                    return (false, 0, "La acciÃ³n es requerida");

                if (modelo.FechaPlaneada <= DateTime.Now)
                    return (false, 0, "La fecha planeada debe ser futura");

                // VALIDACIÃ“N CRÃTICA: AcciÃ³n Inmediata OBLIGATORIA (ISO 9001)
                if (modelo.TipoAccion == (int)TipoAccionEnum.Inmediata)
                {
                    var existe = await _adapter.ExisteAccion(modelo.IdPNC, modelo.IdCausa, (int)TipoAccionEnum.Inmediata);
                    if (existe)
                        return (false, 0, "Esta causa ya tiene una acciÃ³n inmediata registrada");
                }

                var accion = new ProductoNoConformeAccionVM
                {
                    IdPNC = modelo.IdPNC,
                    IdCausa = modelo.IdCausa,
                    TipoAccion = modelo.TipoAccion,
                    Accion = modelo.Accion,
                    FechaPlaneada = modelo.FechaPlaneada,
                    IdResponsableAccion = modelo.IdResponsableAccion,
                    IdResponsableSeguimiento = modelo.IdResponsableSeguimiento,
                    PermiteActualizar = true
                };

                var idAccion = await _adapter.InsertarAccion(accion);
                if (idAccion <= 0)
                    return (false, 0, "Error al agregar acciÃ³n");

                // Enviar notificaciÃ³n a responsables (fire-and-forget)
                _ = Task.Run(async () =>
                {
                    try
                    {
                        await EnviarNotificacionAccionAsignada(idAccion);
                    }
                    catch (Exception ex)
                    {
                        _logger.LogError(ex, "Error al enviar notificaciÃ³n acciÃ³n {IdAccion}", idAccion);
                    }
                });

                _logger.LogInformation("AcciÃ³n agregada: {IdAccion} a causa {IdCausa}", idAccion, modelo.IdCausa);
                return (true, idAccion, "AcciÃ³n agregada correctamente");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al agregar acciÃ³n");
                return (false, 0, "Error al agregar acción. Por favor intente nuevamente.");
            }
        }

        public async Task<(bool success, string message)> ActualizarAccion(ProductoNoConformeAccionVM accion)
        {
            try
            {
                // VALIDACIÓN: No actualizar si ya está ejecutada
                if (accion.FechaEjecucion.HasValue)
                    return (false, "No se puede actualizar una acción ya ejecutada");

                if (string.IsNullOrWhiteSpace(accion.Accion))
                    return (false, "La acción es requerida");

                var actualizado = await _adapter.ActualizarAccion(accion);
                if (!actualizado)
                    return (false, "Error al procesar la solicitud. Por favor intente nuevamente.");

                return (true, "Acción actualizada correctamente");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al actualizar acción");
                return (false, "Error al actualizar acción. Por favor intente nuevamente.");
            }
        }

        public async Task<(bool success, string message)> EjecutarAccion(CerrarAccionPncVM modelo, long idUsuario)
        {
            try
            {
                if (modelo.FechaEjecucion > DateTime.Now)
                    return (false, "La fecha de ejecuciÃ³n no puede ser futura");

                if (string.IsNullOrWhiteSpace(modelo.EvidenciaCierre))
                    return (false, "La evidencia de cierre es requerida");

                var ejecutado = await _adapter.EjecutarAccion(modelo.IdAccion, modelo.FechaEjecucion, modelo.EvidenciaCierre);
                if (!ejecutado)
                    return (false, "Error al ejecutar acciÃ³n");

                _logger.LogInformation("AcciÃ³n ejecutada: {IdAccion} por usuario {IdUsuario}", modelo.IdAccion, idUsuario);
                return (true, "AcciÃ³n ejecutada correctamente");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al ejecutar acciÃ³n {IdAccion}", modelo.IdAccion);
                return (false, "Error al procesar la solicitud. Por favor intente nuevamente.");
            }
        }

        public async Task<(bool success, string message)> EliminarAccion(int idAccion)
        {
            try
            {
                var eliminado = await _adapter.EliminarAccion(idAccion);
                if (!eliminado)
                    return (false, "Error al eliminar acciÃ³n");

                _logger.LogInformation("AcciÃ³n eliminada: {IdAccion}", idAccion);
                return (true, "AcciÃ³n eliminada correctamente");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al eliminar acciÃ³n {IdAccion}", idAccion);
                return (false, "Error al procesar la solicitud. Por favor intente nuevamente.");
            }
        }

        // ============= VALIDACIONES =============

        public async Task<(bool canClose, string reason)> ValidarCierrePnc(int idPnc)
        {
            try
            {
                var detalle = await _adapter.ObtenerPorId(idPnc);
                if (detalle == null)
                    return (false, "PNC no encontrado");

                if (detalle.Pnc.Cerrado)
                    return (false, "El PNC ya estÃ¡ cerrado");

                if (detalle.TotalCausas == 0)
                    return (false, "El PNC debe tener al menos una causa registrada");

                if (detalle.TotalAcciones == 0)
                    return (false, "Debe registrar acciones para las causas");

                if (detalle.AccionesPendientes > 0)
                    return (false, $"Hay {detalle.AccionesPendientes} acciones pendientes de ejecutar");

                // Validar que cada causa tenga acciÃ³n inmediata
                foreach (var causa in detalle.Causas)
                {
                    if (!causa.TieneAccionInmediata)
                        return (false, $"La causa '{causa.CausaRaiz}' no tiene acciÃ³n inmediata");
                }

                return (true, "El PNC puede ser cerrado");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al validar cierre PNC {IdPnc}", idPnc);
                return (false, "Error al procesar la solicitud. Por favor intente nuevamente.");
            }
        }

        public async Task<(bool hasImmediate, string message)> ValidarAccionInmediata(int idPnc, int idCausa)
        {
            try
            {
                var existe = await _adapter.ExisteAccion(idPnc, idCausa, (int)TipoAccionEnum.Inmediata);
                
                if (existe)
                    return (true, "La causa tiene acciÃ³n inmediata");
                else
                    return (false, "ADVERTENCIA: Debe registrar una acciÃ³n inmediata (obligatoria segÃºn ISO 9001)");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al validar acciÃ³n inmediata");
                return (false, "Error al procesar la solicitud. Por favor intente nuevamente.");
            }
        }

        // ============= NOTIFICACIONES (helpers privados) =============

        private async Task EnviarNotificacionNuevoPnc(int idPnc)
        {
            try
            {
                var emails = await _adapter.ObtenerCorreosNotificacion(idPnc);
                if (!emails.Any())
                {
                    _logger.LogWarning("No hay destinatarios para notificaciÃ³n PNC {IdPnc}", idPnc);
                    return;
                }

                var detalle = await _adapter.ObtenerPorId(idPnc);
                if (detalle == null) return;

                var asunto = $"Nuevo PNC Registrado - {detalle.Pnc.JobBook}";
                var cuerpo = $@"
                    <h2>Nuevo Producto No Conforme Registrado</h2>
                    <p><strong>JobBook:</strong> {detalle.Pnc.JobBook}</p>
                    <p><strong>Estudio:</strong> {detalle.Pnc.NombreEstudio}</p>
                    <p><strong>Fecha Reclamo:</strong> {detalle.Pnc.FechaReclamo:dd/MM/yyyy}</p>
                    <p><strong>Reporta:</strong> {detalle.Pnc.NombreReporta}</p>
                    <p><strong>DescripciÃ³n:</strong> {detalle.Pnc.Descripcion}</p>
                    <p>Por favor revise el PNC y registre las acciones correspondientes.</p>
                ";

                await _emailQueue.QueueEmailMultipleAsync(emails, asunto, cuerpo);
                _logger.LogInformation("NotificaciÃ³n enviada para PNC {IdPnc} a {Count} destinatarios", idPnc, emails.Count);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al enviar notificaciÃ³n nuevo PNC {IdPnc}", idPnc);
                throw;
            }
        }

        private async Task EnviarNotificacionAccionAsignada(int idAccion)
        {
            try
            {
                var datosEmail = await _adapter.ObtenerDatosEmailAccion(idAccion);
                if (datosEmail == null || !datosEmail.EmailsDestinatarios.Any())
                {
                    _logger.LogWarning("No hay destinatarios para notificaciÃ³n acciÃ³n {IdAccion}", idAccion);
                    return;
                }

                var asunto = $"AcciÃ³n PNC Asignada - {datosEmail.JobBook}";
                var cuerpo = $@"
                    <h2>AcciÃ³n PNC Asignada</h2>
                    <p><strong>PNC:</strong> {datosEmail.JobBook} - {datosEmail.NombreEstudio}</p>
                    <p><strong>DescripciÃ³n PNC:</strong> {datosEmail.DescripcionPNC}</p>
                    <p><strong>AcciÃ³n:</strong> {datosEmail.AccionDescripcion}</p>
                    <p><strong>Fecha Planeada:</strong> {datosEmail.FechaPlaneada:dd/MM/yyyy}</p>
                    <p><strong>Responsable:</strong> {datosEmail.NombreResponsable}</p>
                    <p>Por favor complete la acciÃ³n antes de la fecha planeada.</p>
                ";

                await _emailQueue.QueueEmailMultipleAsync(datosEmail.EmailsDestinatarios, asunto, cuerpo);
                _logger.LogInformation("NotificaciÃ³n enviada para acciÃ³n {IdAccion}", idAccion);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al enviar notificaciÃ³n acciÃ³n {IdAccion}", idAccion);
                throw;
            }
        }

        private async Task EnviarNotificacionPncCerrado(int idPnc)
        {
            try
            {
                var emails = await _adapter.ObtenerCorreosNotificacion(idPnc);
                if (!emails.Any()) return;

                var detalle = await _adapter.ObtenerPorId(idPnc);
                if (detalle == null) return;

                var asunto = $"PNC Cerrado - {detalle.Pnc.JobBook}";
                var cuerpo = $@"
                    <h2>Producto No Conforme Cerrado</h2>
                    <p><strong>JobBook:</strong> {detalle.Pnc.JobBook}</p>
                    <p><strong>Estudio:</strong> {detalle.Pnc.NombreEstudio}</p>
                    <p><strong>Fecha Cierre:</strong> {DateTime.Now:dd/MM/yyyy}</p>
                    <p><strong>Total Causas:</strong> {detalle.TotalCausas}</p>
                    <p><strong>Total Acciones:</strong> {detalle.TotalAcciones}</p>
                    <p>Todas las acciones correctivas han sido completadas exitosamente.</p>
                ";

                await _emailQueue.QueueEmailMultipleAsync(emails, asunto, cuerpo);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al enviar notificaciÃ³n cierre PNC {IdPnc}", idPnc);
                throw;
            }
        }

        // ============= NOTIFICACIONES PROGRAMADAS =============

        public async Task<int> ProcesarNotificacionesAcciones()
        {
            try
            {
                var emailsEnviados = 0;
                var todosPnc = await _adapter.ObtenerTodos();

                foreach (var pnc in todosPnc.Where(p => p.Cerrado == false))
                {
                    var causas = await _adapter.ObtenerCausas(pnc.Id);
                    foreach (var causa in causas)
                    {
                        var acciones = await _adapter.ObtenerAcciones(pnc.Id, causa.Id);
                        
                        // Acciones vencidas
                        var vencidas = acciones.Where(a => 
                            a.FechaPlaneada.HasValue &&
                            a.FechaPlaneada.Value < DateTime.Now && 
                            a.FechaEjecucion == null).ToList();

                        // Acciones prÃ³ximas a vencer (3 dÃ­as)
                        var proximasVencer = acciones.Where(a =>
                            a.FechaPlaneada.HasValue &&
                            (a.FechaPlaneada.Value - DateTime.Now).Days <= 3 &&
                            a.FechaEjecucion == null).ToList();

                        foreach (var accion in vencidas.Concat(proximasVencer))
                        {
                            await EnviarNotificacionAccionAsignada(accion.Id);
                            emailsEnviados++;
                        }
                    }
                }

                _logger.LogInformation("Procesadas {Count} notificaciones de acciones PNC", emailsEnviados);
                return emailsEnviados;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al procesar notificaciones de acciones");
                return 0;
            }
        }
    }
}

