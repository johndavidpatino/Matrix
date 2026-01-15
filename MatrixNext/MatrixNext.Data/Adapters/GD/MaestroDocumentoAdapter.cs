/// <summary>
/// Adapter para Maestro de Documentos (GD_MaestroDocumentos)
/// Ref: BACKLOG_QA_MODULOS_PENDIENTES.md Â§ Sprint 12.3.5
/// </summary>
namespace MatrixNext.Data.Adapters.GD
{
    using Dapper;
    using MatrixNext.Data.DTOs.GD;
    using Microsoft.Extensions.Logging;
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Data.Common;
    using System.Linq;
    using System.Threading.Tasks;

    public interface IMaestroDocumentoAdapter
    {
        Task<IEnumerable<MaestroDocumentoDto>> ObtenerMaestrosAsync(long? idProceso = null, bool? activos = null);
        Task<MaestroDocumentoDto> ObtenerMaestroAsync(long idMaestro);
        Task<long> CrearMaestroTipo1ConstruccionAsync(MaestroTipo1ConstruccionDto maestro);
        Task<long> CrearMaestroTipo2ActualizacionAsync(MaestroTipo2ActualizacionDto maestro);
        Task<bool> AnularMaestroTipo3Async(MaestroTipo3AnulacionDto anulacion);
        Task<bool> ActualizarMaestroAsync(MaestroDocumentoDto maestro);
        Task<bool> DesactivarMaestroAsync(long idMaestro, long usuarioId);
        Task<ResumenMaestrosDto> ObtenerResumenMaestrosAsync();
    }

    public class MaestroDocumentoAdapter : IMaestroDocumentoAdapter
    {
        private readonly DbConnection _connection;
        private readonly ILogger<MaestroDocumentoAdapter> _logger;

        public MaestroDocumentoAdapter(DbConnection connection, ILogger<MaestroDocumentoAdapter> logger)
        {
            _connection = connection;
            _logger = logger;
        }

        public async Task<IEnumerable<MaestroDocumentoDto>> ObtenerMaestrosAsync(long? idProceso = null, bool? activos = null)
        {
            try
            {
                var query = @"
                    SELECT 
                        m.IdDocumento as IdMaestro,
                        m.Documento as NombreDocumento,
                        m.Codigo as CodigoDocumento,
                        m.IdProceso,
                        p.Proceso,
                        m.IdTipoSolicitud,
                        ts.Descripcion as TipoSolicitud,
                        m.Activo,
                        m.Controlado,
                        m.URL,
                        m.TiempoRetencion,
                        m.DisposicionFinal,
                        m.RegistradoPor,
                        m.ModificadoPor,
                        m.FechaRegistro,
                        m.FechaModificacion
                    FROM GD_MaestroDocumentos m
                    LEFT JOIN GD_Procesos p ON p.IdProceso = m.IdProceso
                    LEFT JOIN GD_TipoSolicitud ts ON ts.IdTipoSolicitud = m.IdTipoSolicitud
                    WHERE 1=1";

                var parameters = new DynamicParameters();

                if (idProceso.HasValue && idProceso > 0)
                {
                    query += " AND m.IdProceso = @IdProceso";
                    parameters.Add("@IdProceso", idProceso);
                }

                if (activos.HasValue)
                {
                    query += " AND m.Activo = @Activo";
                    parameters.Add("@Activo", activos);
                }

                query += " ORDER BY m.Documento";

                var maestros = await _connection.QueryAsync<MaestroDocumentoDto>(query, parameters);

                _logger.LogInformation("Maestros obtenidos. IdProceso: {IdProceso}, Activos: {Activos}, Cantidad: {Cantidad}", 
                    idProceso, activos, maestros.Count());

                return maestros;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error obteniendo maestros. IdProceso: {IdProceso}", idProceso);
                throw;
            }
        }

        public async Task<MaestroDocumentoDto> ObtenerMaestroAsync(long idMaestro)
        {
            try
            {
                var query = @"
                    SELECT 
                        m.IdDocumento as IdMaestro,
                        m.Documento as NombreDocumento,
                        m.Codigo as CodigoDocumento,
                        m.IdProceso,
                        p.Proceso,
                        m.IdTipoSolicitud,
                        ts.Descripcion as TipoSolicitud,
                        m.Activo,
                        m.Controlado,
                        m.URL,
                        m.TiempoRetencion,
                        m.DisposicionFinal,
                        m.RegistradoPor,
                        m.ModificadoPor,
                        m.FechaRegistro,
                        m.FechaModificacion
                    FROM GD_MaestroDocumentos m
                    LEFT JOIN GD_Procesos p ON p.IdProceso = m.IdProceso
                    LEFT JOIN GD_TipoSolicitud ts ON ts.IdTipoSolicitud = m.IdTipoSolicitud
                    WHERE m.IdDocumento = @IdMaestro";

                var maestro = await _connection.QueryFirstOrDefaultAsync<MaestroDocumentoDto>(query, new { IdMaestro = idMaestro });

                return maestro;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error obteniendo maestro. IdMaestro: {IdMaestro}", idMaestro);
                throw;
            }
        }

        public async Task<long> CrearMaestroTipo1ConstruccionAsync(MaestroTipo1ConstruccionDto maestro)
        {
            try
            {
                // SP: GD_MaestroDocumentos_Add (Tipo 1 = ConstrucciÃ³n)
                var parameters = new DynamicParameters();
                parameters.Add("@Documento", maestro.NombreDocumento);
                parameters.Add("@Codigo", maestro.CodigoDocumento);
                parameters.Add("@IdProceso", maestro.IdProceso);
                parameters.Add("@IdTipoSolicitud", 1); // Tipo 1 = ConstrucciÃ³n
                parameters.Add("@Activo", true);
                parameters.Add("@Controlado", maestro.Controlado);
                parameters.Add("@TiempoRetencion", maestro.TiempoRetencionAnios);
                parameters.Add("@DisposicionFinal", maestro.DisposicionFinal);
                parameters.Add("@URL", maestro.URL);
                parameters.Add("@RegistradoPor", maestro.RegistradoPor);
                parameters.Add("@IdMaestro", dbType: DbType.Int64, direction: ParameterDirection.Output);

                await _connection.ExecuteAsync(
                    "GD_MaestroDocumentos_Add",
                    parameters,
                    commandType: CommandType.StoredProcedure
                );

                var idMaestro = parameters.Get<long>("@IdMaestro");

                // Crear documento controlado si aplica
                if (maestro.Controlado)
                {
                    await _connection.ExecuteAsync(
                        @"INSERT INTO GD_DocumentosControlados 
                          (IdDocumento, Activo, URL, RegistradoPor) 
                          VALUES (@IdDocumento, 1, @URL, @RegistradoPor)",
                        new { IdDocumento = idMaestro, maestro.URL, maestro.RegistradoPor }
                    );
                }

                _logger.LogInformation("Maestro Tipo 1 (ConstrucciÃ³n) creado. IdMaestro: {IdMaestro}, Nombre: {Nombre}", 
                    idMaestro, maestro.NombreDocumento);

                return idMaestro;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creando maestro Tipo 1. Nombre: {Nombre}", maestro.NombreDocumento);
                throw;
            }
        }

        public async Task<long> CrearMaestroTipo2ActualizacionAsync(MaestroTipo2ActualizacionDto maestro)
        {
            try
            {
                // SP: GD_MaestroDocumentos_Add (Tipo 2 = ActualizaciÃ³n)
                var parameters = new DynamicParameters();
                parameters.Add("@Documento", maestro.NombreDocumento);
                parameters.Add("@Codigo", maestro.CodigoDocumento);
                parameters.Add("@IdProceso", maestro.IdProceso);
                parameters.Add("@IdTipoSolicitud", 2); // Tipo 2 = ActualizaciÃ³n
                parameters.Add("@IdMaestroExistente", maestro.IdMaestroExistente);
                parameters.Add("@Activo", true);
                parameters.Add("@Controlado", maestro.MantenerControlado);
                parameters.Add("@TiempoRetencion", maestro.TiempoRetencion);
                parameters.Add("@URL", maestro.URL);
                parameters.Add("@VersionNumero", maestro.VersionNumero);
                parameters.Add("@MotivoCambio", maestro.MotivoCambio);
                parameters.Add("@RegistradoPor", maestro.RegistradoPor);
                parameters.Add("@IdMaestro", dbType: DbType.Int64, direction: ParameterDirection.Output);

                await _connection.ExecuteAsync(
                    "GD_MaestroDocumentos_Add2",
                    parameters,
                    commandType: CommandType.StoredProcedure
                );

                var idMaestro = parameters.Get<long>("@IdMaestro");

                _logger.LogInformation("Maestro Tipo 2 (ActualizaciÃ³n) creado. IdMaestro: {IdMaestro}, IdMaestroExistente: {IdMaestroExistente}, VersiÃ³n: {Version}", 
                    idMaestro, maestro.IdMaestroExistente, maestro.VersionNumero);

                return idMaestro;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creando maestro Tipo 2. IdMaestroExistente: {IdMaestroExistente}", maestro.IdMaestroExistente);
                throw;
            }
        }

        public async Task<bool> AnularMaestroTipo3Async(MaestroTipo3AnulacionDto anulacion)
        {
            try
            {
                // Desactivar maestro
                var result = await _connection.ExecuteAsync(
                    @"UPDATE GD_MaestroDocumentos 
                      SET Activo = 0, ModificadoPor = @ModificadoPor, FechaModificacion = GETDATE()
                      WHERE IdDocumento = @IdMaestro",
                    new { anulacion.IdMaestroAnular, anulacion.UsuarioAnulacion }
                );

                if (result == 0)
                {
                    _logger.LogWarning("Maestro no encontrado para anular. IdMaestro: {IdMaestro}", anulacion.IdMaestroAnular);
                    return false;
                }

                // Desactivar documentos controlados si aplica
                if (anulacion.DesactivarDocumentosControlados)
                {
                    await _connection.ExecuteAsync(
                        @"UPDATE GD_DocumentosControlados 
                          SET Activo = 0 
                          WHERE IdDocumento = @IdDocumento",
                        new { IdDocumento = anulacion.IdMaestroAnular }
                    );
                }

                // Guardar auditorÃ­a de anulaciÃ³n
                await _connection.ExecuteAsync(
                    @"INSERT INTO GD_AuditorÃ­a (IdEntidad, TipoEntidad, Accion, DescripciÃ³n, UsuarioId, FechaAccion)
                      VALUES (@IdMaestro, 'MaestroDocumento', 'AnulaciÃ³n', @Motivo, @UsuarioId, GETDATE())",
                    new { anulacion.IdMaestroAnular, anulacion.MotivoAnulacion, anulacion.UsuarioAnulacion }
                );

                _logger.LogInformation("Maestro Tipo 3 (AnulaciÃ³n) completada. IdMaestro: {IdMaestro}, Motivo: {Motivo}", 
                    anulacion.IdMaestroAnular, anulacion.MotivoAnulacion);

                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error anulando maestro. IdMaestro: {IdMaestro}", anulacion.IdMaestroAnular);
                throw;
            }
        }

        public async Task<bool> ActualizarMaestroAsync(MaestroDocumentoDto maestro)
        {
            try
            {
                var result = await _connection.ExecuteAsync(
                    @"UPDATE GD_MaestroDocumentos 
                      SET Documento = @Documento,
                          Codigo = @Codigo,
                          TiempoRetencion = @TiempoRetencion,
                          DisposicionFinal = @DisposicionFinal,
                          ModificadoPor = @ModificadoPor,
                          FechaModificacion = GETDATE()
                      WHERE IdDocumento = @IdMaestro",
                    new 
                    { 
                        maestro.NombreDocumento, 
                        maestro.CodigoDocumento, 
                        maestro.TiempoRetencion,
                        maestro.DisposicionFinal,
                        maestro.ModificadoPor,
                        maestro.IdMaestro
                    }
                );

                _logger.LogInformation("Maestro actualizado. IdMaestro: {IdMaestro}", maestro.IdMaestro);

                return result > 0;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error actualizando maestro. IdMaestro: {IdMaestro}", maestro.IdMaestro);
                throw;
            }
        }

        public async Task<bool> DesactivarMaestroAsync(long idMaestro, long usuarioId)
        {
            try
            {
                var result = await _connection.ExecuteAsync(
                    @"UPDATE GD_MaestroDocumentos 
                      SET Activo = 0, ModificadoPor = @UsuarioId, FechaModificacion = GETDATE()
                      WHERE IdDocumento = @IdMaestro",
                    new { idMaestro, usuarioId }
                );

                _logger.LogInformation("Maestro desactivado. IdMaestro: {IdMaestro}", idMaestro);

                return result > 0;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error desactivando maestro. IdMaestro: {IdMaestro}", idMaestro);
                throw;
            }
        }

        public async Task<ResumenMaestrosDto> ObtenerResumenMaestrosAsync()
        {
            try
            {
                var query = @"
                    SELECT 
                        COUNT(DISTINCT m.IdDocumento) as TotalMaestros,
                        SUM(CASE WHEN m.IdTipoSolicitud = 1 THEN 1 ELSE 0 END) as TotalConstruccion,
                        SUM(CASE WHEN m.IdTipoSolicitud = 2 THEN 1 ELSE 0 END) as TotalActualizacion,
                        SUM(CASE WHEN m.IdTipoSolicitud = 3 THEN 1 ELSE 0 END) as TotalAnulacion,
                        SUM(CASE WHEN m.Activo = 1 THEN 1 ELSE 0 END) as MaestrosActivos,
                        SUM(CASE WHEN m.Activo = 0 THEN 1 ELSE 0 END) as MaestrosInactivos,
                        COUNT(DISTINCT dc.Id) as DocumentosControlados
                    FROM GD_MaestroDocumentos m
                    LEFT JOIN GD_DocumentosControlados dc ON dc.IdDocumento = m.IdDocumento AND dc.Activo = 1";

                var resumen = await _connection.QueryFirstOrDefaultAsync<ResumenMaestrosDto>(query);

                _logger.LogInformation("Resumen de maestros obtenido. Total: {Total}", resumen?.TotalMaestros ?? 0);

                return resumen ?? new ResumenMaestrosDto();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error obteniendo resumen de maestros");
                throw;
            }
        }
    }
}


