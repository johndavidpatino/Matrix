using System;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;
using Dapper;
using MatrixNext.Core.DTOs.GD;

namespace MatrixNext.Infrastructure.Adapters.GD
{
    /// <summary>
    /// Interfaz para operaciones de Catálogos
    /// Sprint 12.3.8: Catálogos Edición con Datos
    /// SPs: GD_TipoSolicitud_*, GD_Estados_*, GD_Procesos_*
    /// </summary>
    public interface ICatalogosAdapter
    {
        // ========== Tipos de Solicitud ==========
        Task<IEnumerable<TipoSolicitudDto>> ObtenerTiposSolicitudAsync(bool soloActivos = false);
        Task<TipoSolicitudDto> ObtenerTipoSolicitudAsync(long idTipoSolicitud);
        Task<bool> ActualizarTipoSolicitudAsync(TipoSolicitudDto tipoSolicitud, long usuarioModifica);
        Task<bool> DesactivarTipoSolicitudAsync(long idTipoSolicitud, long usuarioModifica);

        // ========== Estados ==========
        Task<IEnumerable<EstadoDto>> ObtenerEstadosAsync(bool soloActivos = false);
        Task<IEnumerable<EstadoDto>> ObtenerEstadosPorModuloAsync(string modulo, bool soloActivos = false);
        Task<EstadoDto> ObtenerEstadoAsync(long idEstado);
        Task<bool> ActualizarEstadoAsync(EstadoDto estado, long usuarioModifica);
        Task<bool> DesactivarEstadoAsync(long idEstado, long usuarioModifica);

        // ========== Procesos ==========
        Task<IEnumerable<ProcesoDto>> ObtenerProcesosAsync(bool soloActivos = false);
        Task<ProcesoDto> ObtenerProcesoAsync(long idProceso);
        Task<bool> ActualizarProcesoAsync(ProcesoDto proceso, long usuarioModifica);
        Task<bool> DesactivarProcesoAsync(long idProceso, long usuarioModifica);

        // ========== Resumen ==========
        Task<CatalogosResumenDto> ObtenerResumenAsync();
    }

    /// <summary>
    /// Adaptador para operaciones de Catálogos
    /// </summary>
    public class CatalogosAdapter : ICatalogosAdapter
    {
        private readonly IDbConnection _connection;

        public CatalogosAdapter(IDbConnection connection)
        {
            _connection = connection;
        }

        #region Tipos de Solicitud

        public async Task<IEnumerable<TipoSolicitudDto>> ObtenerTiposSolicitudAsync(bool soloActivos = false)
        {
            var query = "SELECT * FROM GD_TipoSolicitud";
            
            if (soloActivos)
                query += " WHERE Activo = 1";
            
            query += " ORDER BY Orden, Nombre";

            return await _connection.QueryAsync<TipoSolicitudDto>(query);
        }

        public async Task<TipoSolicitudDto> ObtenerTipoSolicitudAsync(long idTipoSolicitud)
        {
            const string query = "SELECT * FROM GD_TipoSolicitud WHERE IdTipoSolicitud = @IdTipoSolicitud";
            var parameters = new DynamicParameters();
            parameters.Add("@IdTipoSolicitud", idTipoSolicitud);

            return await _connection.QueryFirstOrDefaultAsync<TipoSolicitudDto>(query, parameters);
        }

        public async Task<bool> ActualizarTipoSolicitudAsync(TipoSolicitudDto tipoSolicitud, long usuarioModifica)
        {
            const string query = @"
                UPDATE GD_TipoSolicitud
                SET 
                    Nombre = @Nombre,
                    Descripcion = @Descripcion,
                    Activo = @Activo,
                    Orden = @Orden,
                    ModificadoPor = @ModificadoPor,
                    FechaModificacion = GETDATE()
                WHERE IdTipoSolicitud = @IdTipoSolicitud";

            var parameters = new DynamicParameters();
            parameters.Add("@IdTipoSolicitud", tipoSolicitud.IdTipoSolicitud);
            parameters.Add("@Nombre", tipoSolicitud.Nombre);
            parameters.Add("@Descripcion", tipoSolicitud.Descripcion);
            parameters.Add("@Activo", tipoSolicitud.Activo);
            parameters.Add("@Orden", tipoSolicitud.Orden);
            parameters.Add("@ModificadoPor", usuarioModifica);

            var rowsAffected = await _connection.ExecuteAsync(query, parameters);
            return rowsAffected > 0;
        }

        public async Task<bool> DesactivarTipoSolicitudAsync(long idTipoSolicitud, long usuarioModifica)
        {
            const string query = @"
                UPDATE GD_TipoSolicitud
                SET Activo = 0, ModificadoPor = @ModificadoPor, FechaModificacion = GETDATE()
                WHERE IdTipoSolicitud = @IdTipoSolicitud";

            var parameters = new DynamicParameters();
            parameters.Add("@IdTipoSolicitud", idTipoSolicitud);
            parameters.Add("@ModificadoPor", usuarioModifica);

            var rowsAffected = await _connection.ExecuteAsync(query, parameters);
            return rowsAffected > 0;
        }

        #endregion

        #region Estados

        public async Task<IEnumerable<EstadoDto>> ObtenerEstadosAsync(bool soloActivos = false)
        {
            var query = "SELECT * FROM GD_Estados";
            
            if (soloActivos)
                query += " WHERE Activo = 1";
            
            query += " ORDER BY Modulo, Orden, Nombre";

            return await _connection.QueryAsync<EstadoDto>(query);
        }

        public async Task<IEnumerable<EstadoDto>> ObtenerEstadosPorModuloAsync(string modulo, bool soloActivos = false)
        {
            var query = "SELECT * FROM GD_Estados WHERE Modulo = @Modulo";
            
            if (soloActivos)
                query += " AND Activo = 1";
            
            query += " ORDER BY Orden, Nombre";

            var parameters = new DynamicParameters();
            parameters.Add("@Modulo", modulo);

            return await _connection.QueryAsync<EstadoDto>(query, parameters);
        }

        public async Task<EstadoDto> ObtenerEstadoAsync(long idEstado)
        {
            const string query = "SELECT * FROM GD_Estados WHERE IdEstado = @IdEstado";
            var parameters = new DynamicParameters();
            parameters.Add("@IdEstado", idEstado);

            return await _connection.QueryFirstOrDefaultAsync<EstadoDto>(query, parameters);
        }

        public async Task<bool> ActualizarEstadoAsync(EstadoDto estado, long usuarioModifica)
        {
            const string query = @"
                UPDATE GD_Estados
                SET 
                    Nombre = @Nombre,
                    Descripcion = @Descripcion,
                    Modulo = @Modulo,
                    Color = @Color,
                    Icono = @Icono,
                    Activo = @Activo,
                    Orden = @Orden,
                    ModificadoPor = @ModificadoPor,
                    FechaModificacion = GETDATE()
                WHERE IdEstado = @IdEstado";

            var parameters = new DynamicParameters();
            parameters.Add("@IdEstado", estado.IdEstado);
            parameters.Add("@Nombre", estado.Nombre);
            parameters.Add("@Descripcion", estado.Descripcion);
            parameters.Add("@Modulo", estado.Modulo);
            parameters.Add("@Color", estado.Color);
            parameters.Add("@Icono", estado.Icono);
            parameters.Add("@Activo", estado.Activo);
            parameters.Add("@Orden", estado.Orden);
            parameters.Add("@ModificadoPor", usuarioModifica);

            var rowsAffected = await _connection.ExecuteAsync(query, parameters);
            return rowsAffected > 0;
        }

        public async Task<bool> DesactivarEstadoAsync(long idEstado, long usuarioModifica)
        {
            const string query = @"
                UPDATE GD_Estados
                SET Activo = 0, ModificadoPor = @ModificadoPor, FechaModificacion = GETDATE()
                WHERE IdEstado = @IdEstado";

            var parameters = new DynamicParameters();
            parameters.Add("@IdEstado", idEstado);
            parameters.Add("@ModificadoPor", usuarioModifica);

            var rowsAffected = await _connection.ExecuteAsync(query, parameters);
            return rowsAffected > 0;
        }

        #endregion

        #region Procesos

        public async Task<IEnumerable<ProcesoDto>> ObtenerProcesosAsync(bool soloActivos = false)
        {
            var query = "SELECT * FROM GD_Procesos";
            
            if (soloActivos)
                query += " WHERE Activo = 1";
            
            query += " ORDER BY Orden, Nombre";

            return await _connection.QueryAsync<ProcesoDto>(query);
        }

        public async Task<ProcesoDto> ObtenerProcesoAsync(long idProceso)
        {
            const string query = "SELECT * FROM GD_Procesos WHERE IdProceso = @IdProceso";
            var parameters = new DynamicParameters();
            parameters.Add("@IdProceso", idProceso);

            return await _connection.QueryFirstOrDefaultAsync<ProcesoDto>(query, parameters);
        }

        public async Task<bool> ActualizarProcesoAsync(ProcesoDto proceso, long usuarioModifica)
        {
            const string query = @"
                UPDATE GD_Procesos
                SET 
                    Nombre = @Nombre,
                    Descripcion = @Descripcion,
                    Codigo = @Codigo,
                    IdResponsable = @IdResponsable,
                    Version = @Version,
                    Activo = @Activo,
                    Orden = @Orden,
                    ModificadoPor = @ModificadoPor,
                    FechaModificacion = GETDATE()
                WHERE IdProceso = @IdProceso";

            var parameters = new DynamicParameters();
            parameters.Add("@IdProceso", proceso.IdProceso);
            parameters.Add("@Nombre", proceso.Nombre);
            parameters.Add("@Descripcion", proceso.Descripcion);
            parameters.Add("@Codigo", proceso.Codigo);
            parameters.Add("@IdResponsable", proceso.IdResponsable);
            parameters.Add("@Version", proceso.Version);
            parameters.Add("@Activo", proceso.Activo);
            parameters.Add("@Orden", proceso.Orden);
            parameters.Add("@ModificadoPor", usuarioModifica);

            var rowsAffected = await _connection.ExecuteAsync(query, parameters);
            return rowsAffected > 0;
        }

        public async Task<bool> DesactivarProcesoAsync(long idProceso, long usuarioModifica)
        {
            const string query = @"
                UPDATE GD_Procesos
                SET Activo = 0, ModificadoPor = @ModificadoPor, FechaModificacion = GETDATE()
                WHERE IdProceso = @IdProceso";

            var parameters = new DynamicParameters();
            parameters.Add("@IdProceso", idProceso);
            parameters.Add("@ModificadoPor", usuarioModifica);

            var rowsAffected = await _connection.ExecuteAsync(query, parameters);
            return rowsAffected > 0;
        }

        #endregion

        #region Resumen

        public async Task<CatalogosResumenDto> ObtenerResumenAsync()
        {
            const string query = @"
                SELECT
                    (SELECT COUNT(*) FROM GD_TipoSolicitud) AS TotalTiposSolicitud,
                    (SELECT COUNT(*) FROM GD_TipoSolicitud WHERE Activo = 1) AS TiposActivos,
                    (SELECT COUNT(*) FROM GD_Estados) AS TotalEstados,
                    (SELECT COUNT(*) FROM GD_Estados WHERE Activo = 1) AS EstadosActivos,
                    (SELECT COUNT(*) FROM GD_Procesos) AS TotalProcesos,
                    (SELECT COUNT(*) FROM GD_Procesos WHERE Activo = 1) AS ProcesosActivos";

            return await _connection.QueryFirstOrDefaultAsync<CatalogosResumenDto>(query) 
                   ?? new CatalogosResumenDto();
        }

        #endregion
    }
}
