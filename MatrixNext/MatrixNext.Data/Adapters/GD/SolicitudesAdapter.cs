/// <summary>
/// Adapter para Solicitudes de Documentos con asignaciÃ³n automÃ¡tica
/// Ref: BACKLOG_QA_MODULOS_PENDIENTES.md Â§ Sprint 12.3.1
/// SPs: GD_SolicitudDocumentos_Get, GD_SolicitudDocumentos_Add, GD_Revisiones_Add, GD_Email_Send
/// </summary>
namespace MatrixNext.Data.Adapters.GD
{
    using Dapper;
    using MatrixNext.Data.DTOs.GD;
    using Microsoft.Extensions.Logging;
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Linq;
    using System.Threading.Tasks;

    public class SolicitudesAdapter : ISolicitudesAdapter
    {
        private readonly IDbConnection _connection;
        private readonly ILogger<SolicitudesAdapter> _logger;

        public SolicitudesAdapter(IDbConnection connection, ILogger<SolicitudesAdapter> logger)
        {
            _connection = connection;
            _logger = logger;
        }

        /// <summary>
        /// STUB: SP GD_SolicitudDocumentos_Get no existe en BD legacy.
        /// Retorna lista vacía hasta que se implemente el SP o se migre la lógica.
        /// </summary>
        public Task<IEnumerable<SolicitudDocumentoDto>> ObtenerSolicitudesAsync(long? idProyecto = null, long? idEstado = null, long? idSolicitante = null)
        {
            _logger.LogWarning("[GD] ObtenerSolicitudesAsync: SP GD_SolicitudDocumentos_Get no existe en legacy. IdProyecto: {IdProyecto}, IdEstado: {IdEstado}",
                idProyecto, idEstado);
            return Task.FromResult(Enumerable.Empty<SolicitudDocumentoDto>());
        }

        /// <summary>
        /// STUB: SP GD_SolicitudDocumentos_GetById no existe en BD legacy.
        /// Retorna null hasta que se implemente el SP o se migre la lógica.
        /// </summary>
        public Task<SolicitudDocumentoDto> ObtenerSolicitudAsync(long idSolicitud)
        {
            _logger.LogWarning("[GD] ObtenerSolicitudAsync: SP GD_SolicitudDocumentos_GetById no existe en legacy. IdSolicitud: {IdSolicitud}", idSolicitud);
            return Task.FromResult<SolicitudDocumentoDto>(null!);
        }

        /// <summary>
        /// STUB: SP GD_SolicitudDocumentos_Add no existe en BD legacy.
        /// Retorna 0 hasta que se implemente el SP o se migre la lógica.
        /// </summary>
        public Task<long> CrearSolicitudAsync(SolicitudDocumentoDto solicitud)
        {
            _logger.LogWarning("[GD] CrearSolicitudAsync: SP GD_SolicitudDocumentos_Add no existe en legacy. Proyecto: {IdProyecto}", solicitud.IdProyecto);
            return Task.FromResult(0L);
        }

        /// <summary>
        /// STUB: SP GD_SolicitudDocumentos_Update no existe en BD legacy.
        /// Retorna false hasta que se implemente el SP o se migre la lógica.
        /// </summary>
        public Task<bool> ActualizarSolicitudAsync(SolicitudDocumentoDto solicitud)
        {
            _logger.LogWarning("[GD] ActualizarSolicitudAsync: SP GD_SolicitudDocumentos_Update no existe en legacy. IdSolicitud: {IdSolicitud}", solicitud.IdSolicitud);
            return Task.FromResult(false);
        }

        /// <summary>
        /// STUB: SP GD_SolicitudDocumentos_CambiarEstado no existe en BD legacy.
        /// Retorna false hasta que se implemente el SP o se migre la lógica.
        /// </summary>
        public Task<bool> CambiarEstadoSolicitudAsync(long idSolicitud, long idEstado, long usuarioId, string? observaciones = null)
        {
            _logger.LogWarning("[GD] CambiarEstadoSolicitudAsync: SP GD_SolicitudDocumentos_CambiarEstado no existe en legacy. IdSolicitud: {IdSolicitud}, IdEstado: {IdEstado}",
                idSolicitud, idEstado);
            return Task.FromResult(false);
        }

        public async Task<IEnumerable<RevisorDto>> ObtenerRevisoresAsync(long idSolicitud)
        {
            try
            {
                var parameters = new DynamicParameters();
                parameters.Add("@IdSolicitud", idSolicitud);

                var revisores = await _connection.QueryAsync<RevisorDto>(
                    "GD_Revisiones_Get",
                    parameters,
                    commandType: CommandType.StoredProcedure
                );

                return revisores;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error obteniendo revisores. IdSolicitud: {IdSolicitud}", idSolicitud);
                throw;
            }
        }

        public async Task<bool> AsignarRevisoresAsync(long idSolicitud, List<long> idsRevisores, long usuarioId)
        {
            try
            {
                // Insertar revisores en lote
                int orden = 1;
                foreach (var idRevisor in idsRevisores)
                {
                    var parameters = new DynamicParameters();
                    parameters.Add("@IdSolicitud", idSolicitud);
                    parameters.Add("@IdRevisor", idRevisor);
                    parameters.Add("@OrdenRevision", orden);
                    parameters.Add("@IdEstadoRevision", 1); // 1 = Pendiente
                    parameters.Add("@Obligatorio", true);
                    parameters.Add("@RegistradoPor", usuarioId);

                    await _connection.ExecuteAsync(
                        "GD_Revisiones_Add",
                        parameters,
                        commandType: CommandType.StoredProcedure
                    );

                    orden++;
                }

                _logger.LogInformation("Revisores asignados. IdSolicitud: {IdSolicitud}, Cantidad: {Cantidad}",
                    idSolicitud, idsRevisores.Count);

                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error asignando revisores. IdSolicitud: {IdSolicitud}", idSolicitud);
                return false;
            }
        }

        /// <summary>
        /// STUB: SP GD_ConfiguracionRevision_Get no existe en BD legacy.
        /// Retorna null hasta que se implemente el SP o se migre la lógica.
        /// </summary>
        public Task<ConfiguracionRevisionDto?> ObtenerConfiguracionRevisionAsync(long idProceso)
        {
            _logger.LogWarning("[GD] ObtenerConfiguracionRevisionAsync: SP GD_ConfiguracionRevision_Get no existe en legacy. IdProceso: {IdProceso}", idProceso);
            return Task.FromResult<ConfiguracionRevisionDto?>(null);
        }

        /// <summary>
        /// STUB: SP GD_RevisoresPorDefecto_Get no existe en BD legacy.
        /// Retorna lista vacía hasta que se implemente el SP o se migre la lógica.
        /// </summary>
        public Task<List<long>> ObtenerRevisoresPorDefectoAsync(long idProceso)
        {
            _logger.LogWarning("[GD] ObtenerRevisoresPorDefectoAsync: SP GD_RevisoresPorDefecto_Get no existe en legacy. IdProceso: {IdProceso}", idProceso);
            return Task.FromResult(new List<long>());
        }

        /// <summary>
        /// STUB: SP GD_Email_EnviarNotificacion no existe en BD legacy.
        /// Retorna false hasta que se implemente el SP o se migre la lógica.
        /// </summary>
        public Task<bool> EnviarNotificacionRevisoresAsync(long idSolicitud, string contenido)
        {
            _logger.LogWarning("[GD] EnviarNotificacionRevisoresAsync: SP GD_Email_EnviarNotificacion no existe en legacy. IdSolicitud: {IdSolicitud}", idSolicitud);
            return Task.FromResult(false);
        }

        public async Task<bool> AprobarRevisionAsync(AprobacionRevisionDto aprobacion)
        {
            try
            {
                var parameters = new DynamicParameters();
                parameters.Add("@IdRevision", aprobacion.IdRevision);
                parameters.Add("@DocumentoId", 0); // Mantener compatibilidad con SP legacy
                parameters.Add("@UsuarioId", aprobacion.IdRevisor);
                parameters.Add("@FechaAprobacion", aprobacion.FechaRevision);
                parameters.Add("@TipoRevision", 2); // 2 = Aprobado

                await _connection.ExecuteAsync(
                    "GD_Revisiones_Edit",
                    parameters,
                    commandType: CommandType.StoredProcedure
                );

                // Actualizar comentario si existe
                if (!string.IsNullOrWhiteSpace(aprobacion.ComentarioRevision))
                {
                    await _connection.ExecuteAsync(
                        @"UPDATE GD_Revisiones 
                          SET ComentarioRevision = @Comentario 
                          WHERE IdRevision = @IdRevision",
                        new { aprobacion.IdRevision, Comentario = aprobacion.ComentarioRevision }
                    );
                }

                _logger.LogInformation("RevisiÃ³n {IdRevision} aprobada por usuario {IdRevisor}", 
                    aprobacion.IdRevision, aprobacion.IdRevisor);
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error aprobando revisiÃ³n {IdRevision}", aprobacion.IdRevision);
                throw;
            }
        }

        public async Task<bool> RechazarRevisionAsync(AprobacionRevisionDto rechazo)
        {
            try
            {
                var parameters = new DynamicParameters();
                parameters.Add("@IdRevision", rechazo.IdRevision);
                parameters.Add("@DocumentoId", 0);
                parameters.Add("@UsuarioId", rechazo.IdRevisor);
                parameters.Add("@FechaAprobacion", rechazo.FechaRevision);
                parameters.Add("@TipoRevision", 3); // 3 = Rechazado

                await _connection.ExecuteAsync(
                    "GD_Revisiones_Edit",
                    parameters,
                    commandType: CommandType.StoredProcedure
                );

                // Actualizar comentario (obligatorio para rechazos)
                await _connection.ExecuteAsync(
                    @"UPDATE GD_Revisiones 
                      SET ComentarioRevision = @Comentario 
                      WHERE IdRevision = @IdRevision",
                    new { rechazo.IdRevision, Comentario = rechazo.ComentarioRevision ?? "Rechazado" }
                );

                _logger.LogInformation("RevisiÃ³n {IdRevision} rechazada por usuario {IdRevisor}", 
                    rechazo.IdRevision, rechazo.IdRevisor);
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error rechazando revisiÃ³n {IdRevision}", rechazo.IdRevision);
                throw;
            }
        }

        public async Task<ResumenAprobacionDto> ObtenerResumenAprobacionAsync(long idSolicitud)
        {
            try
            {
                var revisores = await ObtenerRevisoresAsync(idSolicitud);
                // NOTA: GD_ConfiguracionRevision no existe - usar valor por defecto
                ConfiguracionRevisionDto? config = null;

                var resumen = new ResumenAprobacionDto
                {
                    IdSolicitud = idSolicitud,
                    TotalRevisores = revisores.Count(),
                    RevisoresAprobados = revisores.Count(r => r.IdEstadoRevision == 2),
                    RevisoresRechazados = revisores.Count(r => r.IdEstadoRevision == 3),
                    RevisoresPendientes = revisores.Count(r => r.IdEstadoRevision == 1),
                    RequiereUnanimidad = config?.RequiereAprobacionUnanimidad ?? false
                };

                // Determinar estado final
                if (resumen.AlgunoRechazo)
                {
                    resumen.EstadoFinal = 3; // Rechazado
                    resumen.MensajeFinal = $"Solicitud rechazada ({resumen.RevisoresRechazados} revisor(es) rechazÃ³)";
                }
                else if (resumen.TodosAprobados)
                {
                    resumen.EstadoFinal = 2; // Aprobado
                    resumen.MensajeFinal = "Solicitud aprobada por todos los revisores";
                }
                else if (resumen.RequiereUnanimidad)
                {
                    resumen.EstadoFinal = 1; // Pendiente
                    resumen.MensajeFinal = $"Pendiente: {resumen.RevisoresPendientes} revisor(es) faltante(s) - Requiere unanimidad";
                }
                else
                {
                    // MayorÃ­a simple: 50% + 1
                    var aprobacionesNecesarias = (resumen.TotalRevisores / 2) + 1;
                    if (resumen.RevisoresAprobados >= aprobacionesNecesarias)
                    {
                        resumen.EstadoFinal = 2; // Aprobado
                        resumen.MensajeFinal = $"Solicitud aprobada por mayorÃ­a ({resumen.RevisoresAprobados}/{resumen.TotalRevisores})";
                    }
                    else
                    {
                        resumen.EstadoFinal = 1; // Pendiente
                        resumen.MensajeFinal = $"Pendiente: {resumen.RevisoresPendientes} revisor(es) faltante(s)";
                    }
                }

                return resumen;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error obteniendo resumen de aprobaciÃ³n para solicitud {IdSolicitud}", idSolicitud);
                throw;
            }
        }

        public async Task<IEnumerable<HistorialRevisionDto>> ObtenerHistorialRevisionesAsync(long idSolicitud)
        {
            try
            {
                var query = @"
                    SELECT 
                        r.IdRevision,
                        r.IdSolicitud,
                        r.IdRevisor,
                        e.NombreCompleto as NombreRevisor,
                        e.Email as EmailRevisor,
                        r.OrdenRevision,
                        r.TipoRevision,
                        CASE r.TipoRevision
                            WHEN 1 THEN 'Pendiente'
                            WHEN 2 THEN 'Aprobado'
                            WHEN 3 THEN 'Rechazado'
                            ELSE 'Desconocido'
                        END as TipoRevisionTexto,
                        r.FechaAsignacion,
                        r.FechaRevision,
                        r.ComentarioRevision,
                        CASE 
                            WHEN r.TipoRevision = 1 THEN 'Asignado'
                            WHEN r.TipoRevision = 2 THEN 'Aprobado'
                            WHEN r.TipoRevision = 3 THEN 'Rechazado'
                            ELSE 'Asignado'
                        END as Accion,
                        CASE 
                            WHEN r.TipoRevision = 1 THEN 'info'
                            WHEN r.TipoRevision = 2 THEN 'success'
                            WHEN r.TipoRevision = 3 THEN 'danger'
                            ELSE 'secondary'
                        END as AccionClass,
                        CASE 
                            WHEN r.TipoRevision = 1 THEN 'fa-clock'
                            WHEN r.TipoRevision = 2 THEN 'fa-check-circle'
                            WHEN r.TipoRevision = 3 THEN 'fa-times-circle'
                            ELSE 'fa-user'
                        END as AccionIcon,
                        DATEDIFF(DAY, r.FechaAsignacion, ISNULL(r.FechaRevision, GETDATE())) as DiasTranscurridos
                    FROM GD_Revisiones r
                    LEFT JOIN TH_Personas e ON e.id = r.IdRevisor
                    WHERE r.IdSolicitud = @IdSolicitud
                    ORDER BY r.OrdenRevision, r.FechaAsignacion";

                var historial = await _connection.QueryAsync<HistorialRevisionDto>(query, new { IdSolicitud = idSolicitud });

                _logger.LogInformation("Historial obtenido. IdSolicitud: {IdSolicitud}, Eventos: {Count}", 
                    idSolicitud, historial.Count());

                return historial;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error obteniendo historial de revisiones. IdSolicitud: {IdSolicitud}", idSolicitud);
                throw;
            }
        }

        public async Task<TimelineSolicitudDto> ObtenerTimelineSolicitudAsync(long idSolicitud)
        {
            try
            {
                // Obtener datos de la solicitud
                var solicitud = await ObtenerSolicitudAsync(idSolicitud);
                if (solicitud == null)
                {
                    _logger.LogWarning("Solicitud no encontrada. IdSolicitud: {IdSolicitud}", idSolicitud);
                    return null;
                }

                // Obtener historial de revisiones
                var eventos = await ObtenerHistorialRevisionesAsync(idSolicitud);

                var timeline = new TimelineSolicitudDto
                {
                    IdSolicitud = idSolicitud,
                    NumeroSolicitud = solicitud.NumeroSolicitud,
                    FechaSolicitud = solicitud.FechaSolicitud,
                    EstadoActual = solicitud.Estado,
                    Eventos = eventos.ToList()
                };

                _logger.LogInformation("Timeline obtenido. IdSolicitud: {IdSolicitud}, Eventos: {Count}", 
                    idSolicitud, timeline.TotalEventos);

                return timeline;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error obteniendo timeline de solicitud. IdSolicitud: {IdSolicitud}", idSolicitud);
                throw;
            }
        }
    }
}

