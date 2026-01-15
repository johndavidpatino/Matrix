using System;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;
using Dapper;
using MatrixNext.Data.DTOs.GD;

namespace MatrixNext.Data.Adapters.GD
{
    /// <summary>
    /// Interfaz para operaciones de PNC
    /// Ref: WebMatrix - PNCClass.vb
    /// SPs: PNC_Productos_Get, PNC_GetById, PNC_Productos_Add, PNC_Productos_Causas_Add, etc.
    /// </summary>
    public interface IPncAdapter
    {
        /// <summary>
        /// Obtiene listado de PNC con filtros opcionales
        /// SP: PNC_Productos_Get
        /// </summary>
        Task<IEnumerable<PncDto>> ObtenerPncAsync(
            long? idPnc = null,
            long? idUsuario = null,
            byte? idEstado = null,
            long? idUsuarioRegistra = null);

        /// <summary>
        /// Obtiene un PNC especÃ­fico por ID
        /// SP: PNC_GetById
        /// </summary>
        Task<PncDto> ObtenerPncAsync(long idPnc);

        /// <summary>
        /// Crea nuevo PNC
        /// SP: PNC_Productos_Add
        /// </summary>
        Task<long> CrearPncAsync(PncDto pnc, long usuarioRegistra);

        /// <summary>
        /// Actualiza PNC existente
        /// OperaciÃ³n: UPDATE directo
        /// </summary>
        Task<bool> ActualizarPncAsync(PncDto pnc, long usuarioModifica);

        /// <summary>
        /// Obtiene causas relacionadas a un PNC
        /// SP: PNC_ProductoNoConformeCausas_Get
        /// </summary>
        Task<IEnumerable<PncCausaDto>> ObtenerCausasAsync(long idPnc);

        /// <summary>
        /// Crea causa para PNC (acciÃ³n correctiva)
        /// SP: PNC_Productos_Causas_Add
        /// </summary>
        Task<long> AgregarCausaAsync(PncCausaDto causa, long usuarioRegistra);

        /// <summary>
        /// Obtiene seguimiento de PNC (causas pendientes/vencidas)
        /// SP: PNC_Seguimiento_Get
        /// </summary>
        Task<IEnumerable<PncSeguimientoDto>> ObtenerSeguimientoAsync(long idPnc);

        /// <summary>
        /// Obtiene log de cambios de estado del PNC
        /// SP: PNC_Productos_Log_Get
        /// </summary>
        Task<IEnumerable<PncLogDto>> ObtenerLogAsync(long idPnc);

        /// <summary>
        /// Calcula resumen/estadÃ­sticas de PNC
        /// OperaciÃ³n: Aggregation en base de datos
        /// </summary>
        Task<PncResumenDto> ObtenerResumenAsync();
    }

    /// <summary>
    /// Adaptador para acceso a datos de PNC
    /// </summary>
    public class PncAdapter : IPncAdapter
    {
        private readonly IDbConnection _connection;

        public PncAdapter(IDbConnection connection)
        {
            _connection = connection;
        }

        public async Task<IEnumerable<PncDto>> ObtenerPncAsync(
            long? idPnc = null,
            long? idUsuario = null,
            byte? idEstado = null,
            long? idUsuarioRegistra = null)
        {
            var parameters = new DynamicParameters();
            parameters.Add("@IdProductoNoConforme", idPnc);
            parameters.Add("@IdUsuario", idUsuario);
            parameters.Add("@IdEstado", idEstado);
            parameters.Add("@IdUsuarioRegistra", idUsuarioRegistra);

            return await _connection.QueryAsync<PncDto>(
                "PNC_Productos_Get",
                parameters,
                commandType: CommandType.StoredProcedure);
        }

        public async Task<PncDto> ObtenerPncAsync(long idPnc)
        {
            var parameters = new DynamicParameters();
            parameters.Add("@IdProductoNoConforme", idPnc);

            return await _connection.QueryFirstOrDefaultAsync<PncDto>(
                "PNC_GetById",
                parameters,
                commandType: CommandType.StoredProcedure);
        }

        public async Task<long> CrearPncAsync(PncDto pnc, long usuarioRegistra)
        {
            var parameters = new DynamicParameters();
            parameters.Add("@AsociadoA", pnc.AsociadoA);
            parameters.Add("@IdReferencia", pnc.IdReferencia);
            parameters.Add("@IdProceso", pnc.IdProceso);
            parameters.Add("@IdProcedimiento", pnc.IdProcedimiento);
            parameters.Add("@IdUnidad", pnc.IdUnidad);
            parameters.Add("@IdPersonaIdentifica", pnc.IdPersonaIdentifica);
            parameters.Add("@FechaReclamo", pnc.FechaReclamo ?? DateTime.Now);
            parameters.Add("@IdFuente", pnc.IdFuente);
            parameters.Add("@IdCategoria", pnc.IdCategoria);
            parameters.Add("@IdPersonaResponsable", pnc.IdPersonaResponsable);
            parameters.Add("@IdPersonaInformar", pnc.IdPersonaInformar);
            parameters.Add("@Descripcion", pnc.Descripcion);
            parameters.Add("@IdTarea", pnc.IdTarea);
            parameters.Add("@Observaciones", pnc.Observaciones);
            parameters.Add("@RegistradoPor", usuarioRegistra);
            parameters.Add("@Id", dbType: DbType.Int64, direction: ParameterDirection.Output);

            await _connection.ExecuteAsync(
                "PNC_Productos_Add",
                parameters,
                commandType: CommandType.StoredProcedure);

            return parameters.Get<long>("@Id");
        }

        public async Task<bool> ActualizarPncAsync(PncDto pnc, long usuarioModifica)
        {
            const string query = @"
                UPDATE PNC_Productos
                SET 
                    IdProceso = @IdProceso,
                    IdProcedimiento = @IdProcedimiento,
                    IdUnidad = @IdUnidad,
                    IdFuente = @IdFuente,
                    IdCategoria = @IdCategoria,
                    Descripcion = @Descripcion,
                    Observaciones = @Observaciones,
                    ModificadoPor = @ModificadoPor,
                    FechaModificacion = GETDATE()
                WHERE IdProductoNoConforme = @IdProductoNoConforme";

            var parameters = new DynamicParameters();
            parameters.Add("@IdProductoNoConforme", pnc.IdPnc);
            parameters.Add("@IdProceso", pnc.IdProceso);
            parameters.Add("@IdProcedimiento", pnc.IdProcedimiento);
            parameters.Add("@IdUnidad", pnc.IdUnidad);
            parameters.Add("@IdFuente", pnc.IdFuente);
            parameters.Add("@IdCategoria", pnc.IdCategoria);
            parameters.Add("@Descripcion", pnc.Descripcion);
            parameters.Add("@Observaciones", pnc.Observaciones);
            parameters.Add("@ModificadoPor", usuarioModifica);

            var rowsAffected = await _connection.ExecuteAsync(query, parameters);
            return rowsAffected > 0;
        }

        public async Task<IEnumerable<PncCausaDto>> ObtenerCausasAsync(long idPnc)
        {
            var parameters = new DynamicParameters();
            parameters.Add("@IdProductoNoConforme", idPnc);

            return await _connection.QueryAsync<PncCausaDto>(
                "PNC_ProductoNoConformeCausas_Get",
                parameters,
                commandType: CommandType.StoredProcedure);
        }

        public async Task<long> AgregarCausaAsync(PncCausaDto causa, long usuarioRegistra)
        {
            var parameters = new DynamicParameters();
            parameters.Add("@IdProductoNoConforme", causa.IdPnc);
            parameters.Add("@DescripcionCausa", causa.DescripcionCausa);
            parameters.Add("@AccionCorrectiva", causa.AccionCorrectiva);
            parameters.Add("@IdPersonaResponsable", causa.IdPersonaResponsable);
            parameters.Add("@FechaVencimiento", causa.FechaVencimiento);
            parameters.Add("@RegistradoPor", usuarioRegistra);
            parameters.Add("@Id", dbType: DbType.Int64, direction: ParameterDirection.Output);

            await _connection.ExecuteAsync(
                "PNC_Productos_Causas_Add",
                parameters,
                commandType: CommandType.StoredProcedure);

            return parameters.Get<long>("@Id");
        }

        public async Task<IEnumerable<PncSeguimientoDto>> ObtenerSeguimientoAsync(long idPnc)
        {
            var parameters = new DynamicParameters();
            parameters.Add("@IdProductoNoConforme", idPnc);

            return await _connection.QueryAsync<PncSeguimientoDto>(
                "PNC_Seguimiento_Get",
                parameters,
                commandType: CommandType.StoredProcedure);
        }

        public async Task<IEnumerable<PncLogDto>> ObtenerLogAsync(long idPnc)
        {
            var parameters = new DynamicParameters();
            parameters.Add("@IdProductoNoConforme", idPnc);

            return await _connection.QueryAsync<PncLogDto>(
                "PNC_Productos_Log_Get",
                parameters,
                commandType: CommandType.StoredProcedure);
        }

        public async Task<PncResumenDto> ObtenerResumenAsync()
        {
            const string query = @"
                SELECT
                    COUNT(DISTINCT IdProductoNoConforme) AS TotalPnc,
                    SUM(CASE WHEN IdEstado = 1 THEN 1 ELSE 0 END) AS PncRegistrados,
                    SUM(CASE WHEN IdEstado = 7 THEN 1 ELSE 0 END) AS PncCausaRegistrada,
                    SUM(CASE WHEN IdEstado = 6 THEN 1 ELSE 0 END) AS PncRechazados,
                    SUM(CASE WHEN IdEstado = 1 AND (SELECT COUNT(*) FROM PNC_ProductoNoConformes_Causas WHERE IdProductoNoConforme = PNC_Productos.IdProductoNoConforme) > 0 THEN 1 ELSE 0 END) AS CausasAbiertas,
                    SUM(CASE WHEN IdEstado = 2 THEN 1 ELSE 0 END) AS CausasCerradas,
                    SUM(CASE WHEN DATEDIFF(DAY, GETDATE(), FechaVencimiento) < 0 THEN 1 ELSE 0 END) AS CausasVencidas,
                    SUM(CASE WHEN DATEDIFF(DAY, GETDATE(), FechaVencimiento) BETWEEN 0 AND 3 THEN 1 ELSE 0 END) AS CausasProximasVencer
                FROM PNC_Productos
                WHERE FechaRegistro >= DATEADD(MONTH, -1, GETDATE())";

            return await _connection.QueryFirstOrDefaultAsync<PncResumenDto>(query) 
                   ?? new PncResumenDto();
        }
    }
}

