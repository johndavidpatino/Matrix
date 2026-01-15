/// <summary>
/// Adapter para Solicitudes de Documentos con asignación automática
/// Ref: BACKLOG_QA_MODULOS_PENDIENTES.md § Sprint 12.3.1
/// SPs: GD_SolicitudDocumentos_Get, GD_SolicitudDocumentos_Add, GD_Revisiones_Add, GD_Email_Send
/// </summary>
namespace MatrixNext.Infrastructure.Adapters.GD
{
    using Dapper;
    using MatrixNext.Core.DTOs.GD;
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

        public async Task<IEnumerable<SolicitudDocumentoDto>> ObtenerSolicitudesAsync(long? idProyecto = null, long? idEstado = null, long? idSolicitante = null)
        {
            try
            {
                var parameters = new DynamicParameters();
                parameters.Add("@IdProyecto", idProyecto);
                parameters.Add("@IdEstado", idEstado);
                parameters.Add("@IdSolicitante", idSolicitante);

                var solicitudes = await _connection.QueryAsync<SolicitudDocumentoDto>(
                    "GD_SolicitudDocumentos_Get",
                    parameters,
                    commandType: CommandType.StoredProcedure
                );

                return solicitudes;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error obteniendo solicitudes. IdProyecto: {IdProyecto}, IdEstado: {IdEstado}",
                    idProyecto, idEstado);
                throw;
            }
        }

        public async Task<SolicitudDocumentoDto> ObtenerSolicitudAsync(long idSolicitud)
        {
            try
            {
                var parameters = new DynamicParameters();
                parameters.Add("@IdSolicitud", idSolicitud);

                var solicitud = await _connection.QueryFirstOrDefaultAsync<SolicitudDocumentoDto>(
                    "GD_SolicitudDocumentos_GetById",
                    parameters,
                    commandType: CommandType.StoredProcedure
                );

                if (solicitud != null)
                {
                    solicitud.Revisores = (await ObtenerRevisoresAsync(idSolicitud)).ToList();
                }

                return solicitud;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error obteniendo solicitud. IdSolicitud: {IdSolicitud}", idSolicitud);
                throw;
            }
        }

        public async Task<long> CrearSolicitudAsync(SolicitudDocumentoDto solicitud)
        {
            try
            {
                var parameters = new DynamicParameters();
                parameters.Add("@IdProyecto", solicitud.IdProyecto);
                parameters.Add("@IdTipoDocumento", solicitud.IdTipoDocumento);
                parameters.Add("@IdProceso", solicitud.IdProceso);
                parameters.Add("@Descripcion", solicitud.Descripcion);
                parameters.Add("@FechaSolicitud", solicitud.FechaSolicitud);
                parameters.Add("@FechaRequerida", solicitud.FechaRequerida);
                parameters.Add("@IdSolicitante", solicitud.IdSolicitante);
                parameters.Add("@Observaciones", solicitud.Observaciones);
                parameters.Add("@RegistradoPor", solicitud.RegistradoPor);
                parameters.Add("@IdSolicitud", dbType: DbType.Int64, direction: ParameterDirection.Output);

                await _connection.ExecuteAsync(
                    "GD_SolicitudDocumentos_Add",
                    parameters,
                    commandType: CommandType.StoredProcedure
                );

                var idSolicitud = parameters.Get<long>("@IdSolicitud");

                _logger.LogInformation("Solicitud creada. IdSolicitud: {IdSolicitud}, Proyecto: {IdProyecto}",
                    idSolicitud, solicitud.IdProyecto);

                return idSolicitud;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creando solicitud. Proyecto: {IdProyecto}", solicitud.IdProyecto);
                throw;
            }
        }

        public async Task<bool> ActualizarSolicitudAsync(SolicitudDocumentoDto solicitud)
        {
            try
            {
                var parameters = new DynamicParameters();
                parameters.Add("@IdSolicitud", solicitud.IdSolicitud);
                parameters.Add("@Descripcion", solicitud.Descripcion);
                parameters.Add("@FechaRequerida", solicitud.FechaRequerida);
                parameters.Add("@Observaciones", solicitud.Observaciones);
                parameters.Add("@ModificadoPor", solicitud.ModificadoPor);

                await _connection.ExecuteAsync(
                    "GD_SolicitudDocumentos_Update",
                    parameters,
                    commandType: CommandType.StoredProcedure
                );

                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error actualizando solicitud. IdSolicitud: {IdSolicitud}", solicitud.IdSolicitud);
                return false;
            }
        }

        public async Task<bool> CambiarEstadoSolicitudAsync(long idSolicitud, long idEstado, long usuarioId, string observaciones = null)
        {
            try
            {
                var parameters = new DynamicParameters();
                parameters.Add("@IdSolicitud", idSolicitud);
                parameters.Add("@IdEstado", idEstado);
                parameters.Add("@ModificadoPor", usuarioId);
                parameters.Add("@Observaciones", observaciones);

                await _connection.ExecuteAsync(
                    "GD_SolicitudDocumentos_CambiarEstado",
                    parameters,
                    commandType: CommandType.StoredProcedure
                );

                _logger.LogInformation("Estado solicitud cambiado. IdSolicitud: {IdSolicitud}, IdEstado: {IdEstado}",
                    idSolicitud, idEstado);

                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error cambiando estado. IdSolicitud: {IdSolicitud}", idSolicitud);
                return false;
            }
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

        public async Task<ConfiguracionRevisionDto> ObtenerConfiguracionRevisionAsync(long idProceso)
        {
            try
            {
                var parameters = new DynamicParameters();
                parameters.Add("@IdProceso", idProceso);

                var config = await _connection.QueryFirstOrDefaultAsync<ConfiguracionRevisionDto>(
                    "GD_ConfiguracionRevision_Get",
                    parameters,
                    commandType: CommandType.StoredProcedure
                );

                return config;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error obteniendo configuración. IdProceso: {IdProceso}", idProceso);
                return null;
            }
        }

        public async Task<List<long>> ObtenerRevisoresPorDefectoAsync(long idProceso)
        {
            try
            {
                var parameters = new DynamicParameters();
                parameters.Add("@IdProceso", idProceso);

                var revisores = await _connection.QueryAsync<long>(
                    "GD_RevisoresPorDefecto_Get",
                    parameters,
                    commandType: CommandType.StoredProcedure
                );

                return revisores.ToList();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error obteniendo revisores por defecto. IdProceso: {IdProceso}", idProceso);
                return new List<long>();
            }
        }

        public async Task<bool> EnviarNotificacionRevisoresAsync(long idSolicitud, string contenido)
        {
            try
            {
                var parameters = new DynamicParameters();
                parameters.Add("@IdSolicitud", idSolicitud);
                parameters.Add("@Contenido", contenido);
                parameters.Add("@TipoNotificacion", "AsignacionRevision");

                await _connection.ExecuteAsync(
                    "GD_Email_EnviarNotificacion",
                    parameters,
                    commandType: CommandType.StoredProcedure
                );

                _logger.LogInformation("Notificación enviada. IdSolicitud: {IdSolicitud}", idSolicitud);

                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error enviando notificación. IdSolicitud: {IdSolicitud}", idSolicitud);
                return false;
            }
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

                _logger.LogInformation("Revisión {IdRevision} aprobada por usuario {IdRevisor}", 
                    aprobacion.IdRevision, aprobacion.IdRevisor);
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error aprobando revisión {IdRevision}", aprobacion.IdRevision);
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

                _logger.LogInformation("Revisión {IdRevision} rechazada por usuario {IdRevisor}", 
                    rechazo.IdRevision, rechazo.IdRevisor);
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error rechazando revisión {IdRevision}", rechazo.IdRevision);
                throw;
            }
        }

        public async Task<ResumenAprobacionDto> ObtenerResumenAprobacionAsync(long idSolicitud)
        {
            try
            {
                var revisores = await ObtenerRevisoresAsync(idSolicitud);
                var config = await _connection.QueryFirstOrDefaultAsync<ConfiguracionRevisionDto>(
                    @"SELECT c.IdConfiguracion, c.IdProceso, c.RequiereAprobacionUnanimidad
                      FROM GD_ConfiguracionRevision c
                      INNER JOIN GD_SolicitudDocumentos s ON s.IdProceso = c.IdProceso
                      WHERE s.IdSolicitud = @IdSolicitud",
                    new { IdSolicitud = idSolicitud }
                );

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
                    resumen.MensajeFinal = $"Solicitud rechazada ({resumen.RevisoresRechazados} revisor(es) rechazó)";
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
                    // Mayoría simple: 50% + 1
                    var aprobacionesNecesarias = (resumen.TotalRevisores / 2) + 1;
                    if (resumen.RevisoresAprobados >= aprobacionesNecesarias)
                    {
                        resumen.EstadoFinal = 2; // Aprobado
                        resumen.MensajeFinal = $"Solicitud aprobada por mayoría ({resumen.RevisoresAprobados}/{resumen.TotalRevisores})";
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
                _logger.LogError(ex, "Error obteniendo resumen de aprobación para solicitud {IdSolicitud}", idSolicitud);
                throw;
            }
        }
    }
}
