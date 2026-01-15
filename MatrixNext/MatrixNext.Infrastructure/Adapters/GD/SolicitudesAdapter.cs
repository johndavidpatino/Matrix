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
    }
}
