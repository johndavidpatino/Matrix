using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using Dapper;
using MatrixNext.Data.Adapters.GD.Models;
using Microsoft.Extensions.Configuration;

namespace MatrixNext.Data.Adapters.GD
{
    public class GdSolicitudesAdapter : IGdSolicitudesAdapter
    {
        private readonly string _connectionString;

        public GdSolicitudesAdapter(IConfiguration config)
        {
            _connectionString = config.GetConnectionString("MatrixDb")!;
        }

        /// <summary>
        /// Obtiene lista de solicitudes con estado
        /// </summary>
        public async Task<List<SolicitudListDto>> ObtenerSolicitudes()
        {
            using var connection = new SqlConnection(_connectionString);
            var resultado = await connection.QueryAsync<SolicitudListDto>(
                "GD_SolDocumentos_Get",
                commandType: CommandType.StoredProcedure);
            return resultado.ToList();
        }

        /// <summary>
        /// Obtiene solicitud por ID
        /// </summary>
        public async Task<SolicitudDocumentoDto?> ObtenerSolicitudById(int id)
        {
            using var connection = new SqlConnection(_connectionString);
            var parametros = new DynamicParameters();
            parametros.Add("@Id", id);

            var resultado = await connection.QueryFirstOrDefaultAsync<SolicitudDocumentoDto>(
                "GD_SolDocumentos_Get",
                parametros,
                commandType: CommandType.StoredProcedure);
            return resultado;
        }

        /// <summary>
        /// Crea solicitud y retorna el ID generado
        /// </summary>
        public async Task<int> CrearSolicitud(SolicitudDocumentoDto dto)
        {
            using var connection = new SqlConnection(_connectionString);
            var parametros = new DynamicParameters();
            parametros.Add("@FechaSolicitud", dto.FechaRegistro == default ? DateTime.Now : dto.FechaRegistro);
            parametros.Add("@Solicitante", dto.IdSolicitante);
            parametros.Add("@Area", dto.Area);
            parametros.Add("@Cargo", dto.Cargo ?? string.Empty);
            parametros.Add("@Tipoid", dto.TipoSolicitud);
            parametros.Add("@DocumentoId", dto.IdDocumento);
            parametros.Add("@NombreDocumento", dto.NombreDocumento ?? string.Empty);
            parametros.Add("@Codigo", dto.Codigo ?? string.Empty);
            parametros.Add("@AreaUso", dto.AreaUso ?? string.Empty);
            parametros.Add("@SitioAcceso", dto.SitioAcceso ?? string.Empty);
            parametros.Add("@RazonSolicitud", dto.Razon ?? string.Empty);
            parametros.Add("@DescripcionSolicitud", dto.Descripcion ?? string.Empty);
            parametros.Add("@IdEstado", dto.IdEstado);
            parametros.Add("@FechaEstado", DateTime.Now);
            parametros.Add("@Comentarios", dto.Comentarios ?? string.Empty);
            parametros.Add("@Modificacion", "");

            var id = await connection.ExecuteScalarAsync<int>(
                "GD_SolDocumentos_Add",
                parametros,
                commandType: CommandType.StoredProcedure);
            return id;
        }

        /// <summary>
        /// Crea registro de revisión para un revisor
        /// </summary>
        public async Task<bool> CrearRevision(int idSolicitud, int idDocumentoControlado, int idRevisor)
        {
            using var connection = new SqlConnection(_connectionString);
            var parametros = new DynamicParameters();
            parametros.Add("@SolicitudId", idSolicitud);
            parametros.Add("@DocumentoId", idDocumentoControlado);
            parametros.Add("@UsuarioId", idRevisor);
            parametros.Add("@TipoRevision", 1); // 1 = revisión normal

            var resultado = await connection.ExecuteAsync(
                "GD_Revisiones_Add",
                parametros,
                commandType: CommandType.StoredProcedure);
            return resultado > 0;
        }

        /// <summary>
        /// Obtiene revisores con status pendiente en una solicitud
        /// </summary>
        public async Task<List<RevisionDto>> ObtenerRevisoresPendientes(int idSolicitud)
        {
            using var connection = new SqlConnection(_connectionString);
            var parametros = new DynamicParameters();
            parametros.Add("@SolicitudId", idSolicitud);

            var resultado = await connection.QueryAsync<RevisionDto>(
                "GD_Revisiones_Get",
                parametros,
                commandType: CommandType.StoredProcedure);
            return resultado.Where(r => r.Estado == 0).ToList(); // 0 = pendiente
        }

        /// <summary>
        /// Obtiene cantidad de revisores aprobados
        /// </summary>
        public async Task<int> ObtenerRevisoresAprobados(int idSolicitud)
        {
            using var connection = new SqlConnection(_connectionString);
            var parametros = new DynamicParameters();
            parametros.Add("@SolicitudId", idSolicitud);

            var resultado = await connection.QueryAsync<RevisionDto>(
                "GD_Revisiones_Get",
                parametros,
                commandType: CommandType.StoredProcedure);
            return resultado.Count(r => r.Estado == 1); // 1 = aprobado
        }

        /// <summary>
        /// Obtiene cantidad total de revisores asignados
        /// </summary>
        public async Task<int> ObtenerTotalRevisores(int idSolicitud)
        {
            using var connection = new SqlConnection(_connectionString);
            var parametros = new DynamicParameters();
            parametros.Add("@SolicitudId", idSolicitud);

            var resultado = await connection.QueryAsync<RevisionDto>(
                "GD_Revisiones_Get",
                parametros,
                commandType: CommandType.StoredProcedure);
            return resultado.Count();
        }

        /// <summary>
        /// Obtiene listado de documentos para dropdown
        /// </summary>
        public async Task<List<MaestroListDto>> ObtenerDocumentos()
        {
            using var connection = new SqlConnection(_connectionString);
            var resultado = await connection.QueryAsync<MaestroListDto>(
                "GD_MaestroDocumentos_Get",
                commandType: CommandType.StoredProcedure);
            return resultado.ToList();
        }

        /// <summary>
        /// Obtiene listado de usuarios para dropdown de revisores
        /// </summary>
        public async Task<List<UsuarioDto>> ObtenerUsuarios()
        {
            using var connection = new SqlConnection(_connectionString);
            var resultado = await connection.QueryAsync<UsuarioDto>(
                "GD_US_Usuarios_Get",
                commandType: CommandType.StoredProcedure);
            return resultado.ToList();
        }

        /// <summary>
        /// Obtiene listado de estados de solicitud
        /// </summary>
        public async Task<List<EstadoSolicitudDto>> ObtenerEstados()
        {
            using var connection = new SqlConnection(_connectionString);
            var resultado = await connection.QueryAsync<EstadoSolicitudDto>(
                "GD_EstadoSolicitud_Get_F",
                commandType: CommandType.StoredProcedure);
            return resultado.ToList();
        }

        /// <summary>
        /// Obtiene listado de tipos de solicitud
        /// </summary>
        public async Task<List<TipoSolicitudDto>> ObtenerTiposSolicitud()
        {
            using var connection = new SqlConnection(_connectionString);
            var resultado = await connection.QueryAsync<TipoSolicitudDto>(
                "GD_TipoSolicitud_Get",
                commandType: CommandType.StoredProcedure);
            return resultado.ToList();
        }
    }
}
