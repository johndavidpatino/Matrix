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
    public class GdMaestroAdapter : IGdMaestroAdapter
    {
        private readonly string _connectionString;

        public GdMaestroAdapter(IConfiguration config)
        {
            _connectionString = config.GetConnectionString("MatrixDb")!;
        }

        public async Task<List<MaestroDocumentoDto>> ObtenerMaestros()
        {
            using var connection = new SqlConnection(_connectionString);
            var rows = await connection.QueryAsync(
                "GD_MaestroDocumentos_Get",
                commandType: CommandType.StoredProcedure);

            return rows.Select(r => new MaestroDocumentoDto
            {
                Id = (int)r.IdDocumento,
                Nombre = (string)r.Documento,
                Codigo = r.Codigo is string codigo ? codigo : string.Empty,
                Activo = true
            }).ToList();
        }

        public async Task<MaestroDocumentoDto?> ObtenerMaestroById(int idMaestro)
        {
            using var connection = new SqlConnection(_connectionString);
            var result = await connection.QueryFirstOrDefaultAsync(
                "GD_GD_MaestroDocumentos_Get2",
                new
                {
                    IdDocumento = idMaestro,
                    Documento = (string?)null,
                    Controlado = (bool?)null,
                    Activo = (bool?)null,
                    Codigo = (string?)null,
                    IdProceso = (short?)null,
                    Responsable = (string?)null,
                    URL = (string?)null,
                    Cierre = (bool?)null,
                    URLOtroServidor = (string?)null,
                    TipoArchivo = (string?)null,
                    Recuperacion = (bool?)null,
                    URLRecuperacion = (string?)null
                },
                commandType: CommandType.StoredProcedure);

            if (result == null)
            {
                return null;
            }

            return new MaestroDocumentoDto
            {
                Id = (int)result.IdDocumento,
                Nombre = (string)result.Documento,
                Codigo = result.Codigo is string codigo ? codigo : string.Empty,
                IdProceso = result.IdProceso is short proc ? proc : 0,
                ProcesoNombre = string.Empty,
                ResponsableNombre = result.Responsable is string resp ? resp : string.Empty,
                Activo = result.Activo is bool activo ? activo : true
            };
        }

        public async Task<DocumentoControlledDto?> ObtenerControlledDocById(int idMaestro)
        {
            using var connection = new SqlConnection(_connectionString);
            const string sql = @"SELECT TOP 1 Id, DocumentoId, UbicacionArchivo, MetodoRecuperacion, TiempoRetencion, DisposicionFinal, Activo, FechaRegistro
                                  FROM GD_DocumentosControlados
                                  WHERE DocumentoId = @IdDocumento";

            var row = await connection.QueryFirstOrDefaultAsync(sql, new { IdDocumento = idMaestro });
            if (row == null)
            {
                return null;
            }

            int tiempoRetencion = 0;
            if (row.TiempoRetencion != null)
            {
                if (int.TryParse(row.TiempoRetencion.ToString(), out int tr))
                {
                    tiempoRetencion = tr;
                }
            }

            return new DocumentoControlledDto
            {
                Id = (int)row.Id,
                IdMaestro = (int)row.DocumentoId,
                Ubicacion = row.UbicacionArchivo,
                MetodoRecuperacion = row.MetodoRecuperacion,
                TiempoRetencion = tiempoRetencion,
                DisposicionFinal = row.DisposicionFinal,
                Activo = row.Activo,
                FechaRegistro = row.FechaRegistro
            };
        }

        public async Task<int> CrearMaestroConControlled(MaestroDocumentoDto dto)
        {
            using var connection = new SqlConnection(_connectionString);
            await connection.OpenAsync();
            using var transaction = connection.BeginTransaction();

            try
            {
                var maestroId = await connection.ExecuteScalarAsync<int>(
                    "GD_MaestroDocumentos_Add2",
                    new
                    {
                        doc = dto.Nombre,
                        controlado = true,
                        activo = dto.Activo,
                        codigo = dto.Codigo,
                        idProc = dto.IdProceso,
                        Responsable = dto.IdResponsable.ToString()
                    },
                    commandType: CommandType.StoredProcedure,
                    transaction: transaction);

                var ctrl = dto.ControlledDoc;
                await connection.ExecuteAsync(
                    "GD_DocumentosControlados_Add",
                    new
                    {
                        docId = maestroId,
                        activo = ctrl.Activo,
                        ubiArchivo = ctrl.Ubicacion,
                        metRecuperacion = ctrl.MetodoRecuperacion,
                        tiempoRetencion = ctrl.TiempoRetencion.ToString(),
                        dispoFinal = ctrl.DisposicionFinal
                    },
                    commandType: CommandType.StoredProcedure,
                    transaction: transaction);

                transaction.Commit();
                return maestroId;
            }
            catch
            {
                transaction.Rollback();
                throw;
            }
        }

        public async Task<bool> ActualizarMaestroConstitucion(int idMaestro, MaestroDocumentoDto dto)
        {
            using var connection = new SqlConnection(_connectionString);
            await connection.OpenAsync();
            using var transaction = connection.BeginTransaction();

            try
            {
                // Reutiliza SP de controlado: si existe, hace update con version incrementada
                await connection.ExecuteAsync(
                    "GD_DocumentosControlados_Add",
                    new
                    {
                        docId = idMaestro,
                        activo = dto.ControlledDoc.Activo,
                        ubiArchivo = dto.ControlledDoc.Ubicacion,
                        metRecuperacion = dto.ControlledDoc.MetodoRecuperacion,
                        tiempoRetencion = dto.ControlledDoc.TiempoRetencion.ToString(),
                        dispoFinal = dto.ControlledDoc.DisposicionFinal
                    },
                    commandType: CommandType.StoredProcedure,
                    transaction: transaction);

                transaction.Commit();
                return true;
            }
            catch
            {
                transaction.Rollback();
                throw;
            }
        }

        public async Task<bool> ActualizarMaestroActualizacion(int idMaestro, MaestroDocumentoDto dto)
        {
            using var connection = new SqlConnection(_connectionString);
            var affected = await connection.ExecuteAsync(
                "GD_DocumentosControlados_Add",
                new
                {
                    docId = idMaestro,
                    activo = dto.ControlledDoc.Activo,
                    ubiArchivo = dto.ControlledDoc.Ubicacion,
                    metRecuperacion = dto.ControlledDoc.MetodoRecuperacion,
                    tiempoRetencion = dto.ControlledDoc.TiempoRetencion.ToString(),
                    dispoFinal = dto.ControlledDoc.DisposicionFinal
                },
                commandType: CommandType.StoredProcedure);

            return affected > 0;
        }

        public async Task<bool> AnularMaestro(int idMaestro)
        {
            using var connection = new SqlConnection(_connectionString);
            var affected = await connection.ExecuteAsync(
                "GD_DocumentosMaestros_Update",
                new { docId = idMaestro },
                commandType: CommandType.StoredProcedure);
            return affected > 0;
        }

        public async Task<bool> AnularControlado(int idMaestro)
        {
            using var connection = new SqlConnection(_connectionString);
            var affected = await connection.ExecuteAsync(
                "GD_DocumentosControlados_Activo",
                new { docId = idMaestro },
                commandType: CommandType.StoredProcedure);
            return affected > 0;
        }

        public async Task<List<TipoSolicitudDto>> ObtenerTiposSolicitud()
        {
            using var connection = new SqlConnection(_connectionString);
            var rows = await connection.QueryAsync("GD_TipoSolicitud_Get", commandType: CommandType.StoredProcedure);
            return rows.Select(r => new TipoSolicitudDto
            {
                Id = (int)r.id,
                Nombre = (string)r.Tipo
            }).ToList();
        }

        public async Task<List<ProcesoDto>> ObtenerProcesos()
        {
            using var connection = new SqlConnection(_connectionString);
            var rows = await connection.QueryAsync("GD_Procesos_Get", commandType: CommandType.StoredProcedure);
            return rows.Select(r => new ProcesoDto
            {
                Id = (int)r.IdProceso,
                Nombre = (string)r.Proceso
            }).ToList();
        }

        public async Task<List<UsuarioDto>> ObtenerUsuarios()
        {
            using var connection = new SqlConnection(_connectionString);
            var rows = await connection.QueryAsync("GD_US_Usuarios_Get", commandType: CommandType.StoredProcedure);
            return rows.Select(r => new UsuarioDto
            {
                Id = (int)r.id,
                Usuario = (string)r.Usuario,
                Nombres = r.Nombres is string n ? n : string.Empty,
                Apellidos = r.Apellidos is string a ? a : string.Empty,
                Email = r.Email is string e ? e : string.Empty,
                Activo = r.Activo is bool act && act
            }).ToList();
        }
    }
}
