using System;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;
using Dapper;
using MatrixNext.Data.DTOs.GD;

namespace MatrixNext.Data.Adapters.GD
{
    /// <summary>
    /// Interfaz para operaciones de CatÃ¡logos
    /// Sprint 12.3.8: CatÃ¡logos EdiciÃ³n con Datos
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
    /// Adaptador para operaciones de CatÃ¡logos
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

        #region Estados - TABLAS NO EXISTEN
        
        /// <summary>
        /// NOTA: Tabla GD_Estados NO EXISTE en CO_Matrix_Intranet
        /// Existe GD_EstadoSolicitud pero con estructura diferente
        /// </summary>

        public Task<IEnumerable<EstadoDto>> ObtenerEstadosAsync(bool soloActivos = false)
        {
            throw new NotImplementedException(
                "Tabla GD_Estados no existe en BD. " +
                "Existe GD_EstadoSolicitud pero con estructura diferente.");
        }

        public Task<IEnumerable<EstadoDto>> ObtenerEstadosPorModuloAsync(string modulo, bool soloActivos = false)
        {
            throw new NotImplementedException(
                "Tabla GD_Estados no existe en BD.");
        }

        public Task<EstadoDto> ObtenerEstadoAsync(long idEstado)
        {
            throw new NotImplementedException(
                "Tabla GD_Estados no existe en BD.");
        }

        public Task<bool> ActualizarEstadoAsync(EstadoDto estado, long usuarioModifica)
        {
            throw new NotImplementedException(
                "Tabla GD_Estados no existe en BD.");
        }

        public Task<bool> DesactivarEstadoAsync(long idEstado, long usuarioModifica)
        {
            throw new NotImplementedException(
                "Tabla GD_Estados no existe en BD.");
        }

        #endregion

        #region Procesos - TABLA NO EXISTE

        /// <summary>
        /// NOTA: Tabla GD_Procesos NO EXISTE en CO_Matrix_Intranet
        /// </summary>

        public Task<IEnumerable<ProcesoDto>> ObtenerProcesosAsync(bool soloActivos = false)
        {
            throw new NotImplementedException(
                "Tabla GD_Procesos no existe en BD.");
        }

        public Task<ProcesoDto> ObtenerProcesoAsync(long idProceso)
        {
            throw new NotImplementedException(
                "Tabla GD_Procesos no existe en BD.");
        }

        public Task<bool> ActualizarProcesoAsync(ProcesoDto proceso, long usuarioModifica)
        {
            throw new NotImplementedException(
                "Tabla GD_Procesos no existe en BD.");
        }

        public Task<bool> DesactivarProcesoAsync(long idProceso, long usuarioModifica)
        {
            throw new NotImplementedException(
                "Tabla GD_Procesos no existe en BD.");
        }

        #endregion

        #region Resumen

        public async Task<CatalogosResumenDto> ObtenerResumenAsync()
        {
            // Solo GD_TipoSolicitud existe - los otros catálogos devuelven 0
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

