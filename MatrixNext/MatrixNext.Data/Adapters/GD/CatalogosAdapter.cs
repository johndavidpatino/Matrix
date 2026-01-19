using System;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;
using Dapper;
using MatrixNext.Data.DTOs.GD;

namespace MatrixNext.Data.Adapters.GD
{
    /// <summary>
    /// Interfaz para operaciones de Catálogos
    /// Sprint 12.3.8: Catálogos Edición con Datos
    /// NOTA: Solo GD_TipoSolicitud existe en BD. GD_Estados y GD_Procesos NO EXISTEN.
    /// </summary>
    public interface ICatalogosAdapter
    {
        // ========== Tipos de Solicitud (FUNCIONAL) ==========
        Task<IEnumerable<TipoSolicitudDto>> ObtenerTiposSolicitudAsync(bool soloActivos = false);
        Task<TipoSolicitudDto> ObtenerTipoSolicitudAsync(long idTipoSolicitud);
        Task<bool> ActualizarTipoSolicitudAsync(TipoSolicitudDto tipoSolicitud, long usuarioModifica);
        Task<bool> DesactivarTipoSolicitudAsync(long idTipoSolicitud, long usuarioModifica);

        // ========== Resumen ==========
        Task<CatalogosResumenDto> ObtenerResumenAsync();
    }

    /// <summary>
    /// Adaptador para operaciones de Catálogos GD
    /// NOTA: Solo GD_TipoSolicitud implementado. Estados y Procesos removidos (tablas no existen).
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

        #region Resumen

        public async Task<CatalogosResumenDto> ObtenerResumenAsync()
        {
            // Solo GD_TipoSolicitud existe
            const string query = @"
                SELECT
                    (SELECT COUNT(*) FROM GD_TipoSolicitud) AS TotalTiposSolicitud,
                    (SELECT COUNT(*) FROM GD_TipoSolicitud WHERE Activo = 1) AS TiposActivos,
                    0 AS TotalEstados,
                    0 AS EstadosActivos,
                    0 AS TotalProcesos,
                    0 AS ProcesosActivos";

            return await _connection.QueryFirstOrDefaultAsync<CatalogosResumenDto>(query) 
                   ?? new CatalogosResumenDto();
        }

        #endregion
    }
}