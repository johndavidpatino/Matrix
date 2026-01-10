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
    public class GdRepositorioAdapter : IGdRepositorioAdapter
    {
        private readonly string _connectionString;

        public GdRepositorioAdapter(IConfiguration config)
        {
            _connectionString = config.GetConnectionString("MatrixDb")!;
        }

        public async Task<List<RepositorioListDto>> ObtenerDocumentos(int idContenedor, int tipoContenedor)
        {
            _ = tipoContenedor; // TipoContenedor reservado para futuros filtros
            using var connection = new SqlConnection(_connectionString);
            var rows = await connection.QueryAsync(
                "GD_RepositorioDocumentos_GetXTrabajo",
                new
                {
                    Id = (int?)null,
                    Nombre = (string?)null,
                    Url = (string?)null,
                    DocumentoId = (int?)null,
                    Version = (double?)null,
                    Fecha = (System.DateTime?)null,
                    Comentarios = (string?)null,
                    UsuarioId = (long?)null,
                    IdContenedor = idContenedor,
                    esRecuperacion = (bool?)null
                },
                commandType: CommandType.StoredProcedure);

            return rows.Select(r => new RepositorioListDto
            {
                Id = (int)r.IdDocumentoRepositorio,
                NombreArchivo = r.Nombre,
                Version = (decimal)r.Version,
                FechaRegistro = (System.DateTime)r.Fecha,
                Comentarios = r.Comentarios,
                RegistradoPor = string.Empty
            }).ToList();
        }

        public async Task<RepositorioDocumentoDto?> ObtenerDocumentoById(int id)
        {
            using var connection = new SqlConnection(_connectionString);
            var row = await connection.QueryFirstOrDefaultAsync(
                "GD_RepositorioDocumentos_Get",
                new
                {
                    Id = id,
                    Nombre = (string?)null,
                    Url = (string?)null,
                    DocumentoId = (int?)null,
                    Version = (double?)null,
                    Fecha = (System.DateTime?)null,
                    Comentarios = (string?)null,
                    UsuarioId = (long?)null,
                    IdContenedor = (int?)null
                },
                commandType: CommandType.StoredProcedure);

            if (row == null)
            {
                return null;
            }

            return new RepositorioDocumentoDto
            {
                Id = (int)row.IdDocumentoRepositorio,
                IdContenedor = (int)row.IdContenedor,
                IdDocumento = (int)row.DocumentoId,
                UrlArchivo = row.Url,
                Version = (decimal)row.Version,
                Comentarios = row.Comentarios,
                UsuarioId = (int)row.UsuarioId,
                FechaRegistro = (System.DateTime)row.Fecha,
                NombreArchivo = row.Nombre
            };
        }

        public async Task<decimal> ObtenerProximaVersion(int idContenedor, int idDocumento)
        {
            using var connection = new SqlConnection(_connectionString);
            const string sql = @"SELECT ISNULL(MAX(Version),0)+1 as Proxima FROM GD_RepositorioDocumentos WHERE IdContenedor=@IdContenedor AND DocumentoId=@IdDocumento";
            var next = await connection.ExecuteScalarAsync<decimal>(sql, new { IdContenedor = idContenedor, IdDocumento = idDocumento });
            return next;
        }

        public async Task<int> GuardarDocumento(UploadDocumentoDto dto, decimal version)
        {
            using var connection = new SqlConnection(_connectionString);
            var id = await connection.ExecuteScalarAsync<int>(
                "GD_GD_RepositorioDocumentos_Add",
                new
                {
                    Nombre = System.IO.Path.GetFileName(dto.UrlArchivo),
                    Url = dto.UrlArchivo,
                    DocumentoId = dto.IdDocumento,
                    Version = version,
                    Fecha = System.DateTime.UtcNow.AddHours(-5),
                    Comentarios = dto.Comentarios,
                    UsuarioId = dto.UsuarioId,
                    IdContenedor = dto.IdContenedor
                },
                commandType: CommandType.StoredProcedure);
            return id;
        }

        public async Task<bool> EliminarDocumento(int id)
        {
            using var connection = new SqlConnection(_connectionString);
            var affected = await connection.ExecuteAsync(
                "GD_EscanerDocumentos_Del",
                new { Id = id, IdTrabajo = (int?)null, IdDocumento = (int?)null },
                commandType: CommandType.StoredProcedure);
            return affected > 0;
        }

        public async Task<List<RepositorioDocumentoDto>> ObtenerDocumentosContenedor(int idContenedor)
        {
            using var connection = new SqlConnection(_connectionString);
            var rows = await connection.QueryAsync(
                "GD_RepositorioDocumentos_Get",
                new
                {
                    Id = (int?)null,
                    Nombre = (string?)null,
                    Url = (string?)null,
                    DocumentoId = (int?)null,
                    Version = (double?)null,
                    Fecha = (System.DateTime?)null,
                    Comentarios = (string?)null,
                    UsuarioId = (long?)null,
                    IdContenedor = idContenedor
                },
                commandType: CommandType.StoredProcedure);

            return rows.Select(r => new RepositorioDocumentoDto
            {
                Id = (int)r.IdDocumentoRepositorio,
                IdContenedor = (int)r.IdContenedor,
                IdDocumento = (int)r.DocumentoId,
                UrlArchivo = r.Url,
                Version = (decimal)r.Version,
                Comentarios = r.Comentarios,
                UsuarioId = (int)r.UsuarioId,
                FechaRegistro = (System.DateTime)r.Fecha,
                NombreArchivo = r.Nombre
            }).ToList();
        }
    }
}
