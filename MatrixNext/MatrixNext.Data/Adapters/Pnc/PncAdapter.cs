using System.Data;
using System.Data.SqlClient;
using Dapper;
using MatrixNext.ViewModels.Pnc;
using MatrixNext.ViewModels.Pnc.DTOs;
using Microsoft.Extensions.Configuration;

namespace MatrixNext.Data.Adapters.Pnc
{
    /// <summary>
    /// Adapter para Producto No Conforme (PNC) usando Dapper
    /// Sistema de Gestión de Calidad ISO 9001
    /// Mapea 16 Stored Procedures + CRUD directo
    /// </summary>
    public class PncAdapter : IPncAdapter
    {
        private readonly string _connectionString;

        public PncAdapter(IConfiguration config)
        {
            _connectionString = config.GetConnectionString("MatrixDb") 
                ?? throw new ArgumentNullException("MatrixDb connection string not found");
        }

        // ============= CONSULTAS PNC =============

        public async Task<List<PncObtenerProductoNoConformeDTO>> ObtenerTodos()
        {
            try
            {
                using var connection = new SqlConnection(_connectionString);
                var result = await connection.QueryAsync<PncObtenerProductoNoConformeDTO>(
                    "PNC_ObtenerProductoNoConformeTodos",
                    commandType: CommandType.StoredProcedure);
                return result.ToList();
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException("Error executing PNC_ObtenerProductoNoConformeTodos", ex);
            }
        }

        public async Task<List<PncObtenerProductoNoConformeDTO>> ObtenerPorJobBook(string jobBook)
        {
            try
            {
                using var connection = new SqlConnection(_connectionString);
                var result = await connection.QueryAsync<PncObtenerProductoNoConformeDTO>(
                    "PNC_ObtenerProductoNoConforme",
                    new { JobBook = jobBook },
                    commandType: CommandType.StoredProcedure);
                return result.ToList();
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException($"Error executing PNC_ObtenerProductoNoConforme for JobBook {jobBook}", ex);
            }
        }

        public async Task<ProductoNoConformeDetalleVM?> ObtenerPorId(int idPnc)
        {
            try
            {
                using var connection = new SqlConnection(_connectionString);
                
                // SP retorna: Id, JobBook, FechaReclamo, IdEstudio, UsuarioReporta, Trabajo, Cliente, 
                //             FuenteReclamo, Catalogado, Descripcion, EmailUsuario
                var pncData = await connection.QuerySingleOrDefaultAsync<dynamic>(
                    "PNC_GetById",
                    new { PNC_id = idPnc },
                    commandType: CommandType.StoredProcedure);

                if (pncData == null)
                    return null;

                // Mapear a ViewModel
                var detalle = new ProductoNoConformeDetalleVM
                {
                    Pnc = new ProductoNoConformeVM
                    {
                        Id = pncData.Id,
                        JobBook = pncData.JobBook,
                        FechaReclamo = pncData.FechaReclamo,
                        IdEstudio = pncData.IdEstudio,
                        Descripcion = pncData.Descripcion,
                        NombreReporta = pncData.UsuarioReporta,
                        NombreEstudio = pncData.Trabajo,
                        NombreCliente = pncData.Cliente,
                        DescripcionFuenteReclamo = pncData.FuenteReclamo,
                        DescripcionCategoria = pncData.Catalogado
                    }
                };

                // Obtener causas y acciones
                var causas = await ObtenerCausas(idPnc);
                foreach (var causa in causas)
                {
                    var acciones = await ObtenerAcciones(idPnc, causa.Id);
                    detalle.Causas.Add(new ProductoNoConformeCausaDetalleVM
                    {
                        Id = causa.Id,
                        IdPNC = causa.IdPNC,
                        CausaRaiz = causa.CausaRaiz ?? string.Empty,
                        Acciones = acciones.Select(a => new ProductoNoConformeAccionVM
                        {
                            Id = a.Id,
                            IdPNC = a.IdPNC,
                            IdCausa = a.IdCausa,
                            TipoAccion = a.TipoAccion ?? 0,
                            Accion = a.Accion ?? string.Empty,
                            FechaPlaneada = a.FechaPlaneada,
                            FechaEjecucion = a.FechaEjecucion,
                            IdResponsableAccion = a.IdResponsableAccion,
                            IdResponsableSeguimiento = a.IdResponsableSeguimiento,
                            EvidenciaCierre = a.EvidenciaCierre,
                            NombreTipoAccion = a.NombreTipoAccion,
                            NombreResponsableAccion = a.NombreResponsableAccion,
                            NombreResponsableSeguimiento = a.NombreResponsableSeguimiento
                        }).ToList()
                    });
                }

                return detalle;
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException($"Error executing PNC_GetById for Id {idPnc}", ex);
            }
        }

        // ============= CONSULTAS CAUSAS =============

        public async Task<List<PncVerCausasDTO>> ObtenerCausas(int idPnc)
        {
            try
            {
                using var connection = new SqlConnection(_connectionString);
                var result = await connection.QueryAsync<PncVerCausasDTO>(
                    "PNC_ProductoNoConformeCausas_Get",
                    new { IdPNC = idPnc },
                    commandType: CommandType.StoredProcedure);
                return result.ToList();
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException($"Error executing PNC_ProductoNoConformeCausas_Get for IdPNC {idPnc}", ex);
            }
        }

        public async Task<List<PncVerCausasDTO>> ObtenerCausasDetalle(int idProducto)
        {
            try
            {
                using var connection = new SqlConnection(_connectionString);
                var result = await connection.QueryAsync<PncVerCausasDTO>(
                    "PNC_Causa_Get",
                    new { idProducto },
                    commandType: CommandType.StoredProcedure);
                return result.ToList();
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException($"Error executing PNC_Causa_Get for idProducto {idProducto}", ex);
            }
        }

        // ============= CONSULTAS ACCIONES =============

        public async Task<List<PncVerAccionesDTO>> ObtenerAcciones(int idPnc, int idCausa)
        {
            try
            {
                using var connection = new SqlConnection(_connectionString);
                var result = await connection.QueryAsync<PncVerAccionesDTO>(
                    "PNC_ProductoNoConformeAcciones_Get",
                    new { IdPNC = idPnc, IdCausa = idCausa },
                    commandType: CommandType.StoredProcedure);
                return result.ToList();
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException($"Error executing PNC_ProductoNoConformeAcciones_Get for IdPNC {idPnc}, IdCausa {idCausa}", ex);
            }
        }

        // ============= NOTIFICACIONES EMAIL =============

        public async Task<PncNotificacionVM?> ObtenerDatosEmailAccion(long idAccion)
        {
            try
            {
                using var connection = new SqlConnection(_connectionString);
                
                // SP retorna: Id, Trabajo, DescripcionPNC, TipodeAccion, Accion, FechaPlaneada,
                //             ResponsableAccion, ResponsableSeguimiento, EmailResponsable, EmailSeguimiento
                var result = await connection.QuerySingleOrDefaultAsync<dynamic>(
                    "PNC_EmailAcciones",
                    new { IdPncDetalle = idAccion },
                    commandType: CommandType.StoredProcedure);

                if (result == null)
                    return null;

                return new PncNotificacionVM
                {
                    IdPNC = result.Id,
                    JobBook = result.Trabajo?.ToString().Split(' ')[0] ?? string.Empty,
                    NombreEstudio = result.Trabajo,
                    DescripcionPNC = result.DescripcionPNC,
                    AccionDescripcion = result.Accion,
                    FechaPlaneada = result.FechaPlaneada,
                    NombreResponsable = result.ResponsableAccion,
                    EmailsDestinatarios = new List<string> { result.EmailResponsable, result.EmailSeguimiento }
                        .Where(e => !string.IsNullOrEmpty(e))
                        .ToList(),
                    TipoNotificacion = TipoNotificacionPncEnum.AccionAsignada
                };
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException($"Error executing PNC_EmailAcciones for IdAccion {idAccion}", ex);
            }
        }

        public async Task<List<string>> ObtenerCorreosNotificacion(long idPnc)
        {
            try
            {
                using var connection = new SqlConnection(_connectionString);
                var result = await connection.QueryAsync<string>(
                    "PNC_EmailNotificacionReporte",
                    new { idPNC = idPnc },
                    commandType: CommandType.StoredProcedure);
                return result.Where(e => !string.IsNullOrWhiteSpace(e)).Distinct().ToList();
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException($"Error executing PNC_EmailNotificacionReporte for IdPNC {idPnc}", ex);
            }
        }

        // ============= CATÁLOGOS =============

        public async Task<List<PncFuenteReclamoVM>> ObtenerFuentesReclamo()
        {
            try
            {
                using var connection = new SqlConnection(_connectionString);
                var sql = "SELECT Id, Descripcion FROM PNC_FuenteReclamo ORDER BY Descripcion";
                var result = await connection.QueryAsync<PncFuenteReclamoVM>(sql);
                return result.ToList();
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException("Error fetching PNC_FuenteReclamo", ex);
            }
        }

        public async Task<List<PncCategoriaVM>> ObtenerCategorias()
        {
            try
            {
                using var connection = new SqlConnection(_connectionString);
                var sql = @"
                    SELECT c.Id, c.Descripcion, c.IdUnidad, c.IdRol,
                           gu.GrupoUnidad AS NombreUnidad
                    FROM PNC_Categorias c
                    LEFT JOIN US_Unidades u ON c.IdUnidad = u.id
                    LEFT JOIN US_GrupoUnidad gu ON u.GrupoUnidadId = gu.id
                    ORDER BY c.Descripcion";
                var result = await connection.QueryAsync<PncCategoriaVM>(sql);
                return result.ToList();
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException("Error fetching PNC_Categorias", ex);
            }
        }

        public async Task<List<PncTipoAccionVM>> ObtenerTiposAccion()
        {
            try
            {
                using var connection = new SqlConnection(_connectionString);
                var sql = "SELECT Id, Accion FROM PNC_TiposDeAccion ORDER BY Id";
                var result = await connection.QueryAsync<PncTipoAccionVM>(sql);
                return result.ToList();
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException("Error fetching PNC_TiposDeAccion", ex);
            }
        }

        // ============= CRUD PNC =============

        public async Task<int> InsertarPnc(ProductoNoConformeVM pnc)
        {
            try
            {
                using var connection = new SqlConnection(_connectionString);
                var sql = @"
                    INSERT INTO PNC_ProductoNoConforme 
                        (IdEstudio, IdTrabajo, JobBook, FechaReclamo, IdReporta, IdUnidad, 
                         IdClienteExterno, FuenteReclamo, Categoria, Tarea, Descripcion, 
                         Cerrado, FechaCierre, Usuario, FechaGrabacion, FechaActualizacion)
                    VALUES 
                        (@IdEstudio, @IdTrabajo, @JobBook, @FechaReclamo, @IdReporta, @IdUnidad,
                         @IdClienteExterno, @FuenteReclamo, @Categoria, @Tarea, @Descripcion,
                         0, NULL, @Usuario, GETDATE(), NULL);
                    SELECT CAST(SCOPE_IDENTITY() AS INT);";

                var id = await connection.ExecuteScalarAsync<int>(sql, pnc);
                return id;
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException("Error inserting PNC_ProductoNoConforme", ex);
            }
        }

        public async Task<bool> ActualizarPnc(ProductoNoConformeVM pnc)
        {
            try
            {
                using var connection = new SqlConnection(_connectionString);
                var sql = @"
                    UPDATE PNC_ProductoNoConforme
                    SET IdEstudio = @IdEstudio,
                        IdTrabajo = @IdTrabajo,
                        JobBook = @JobBook,
                        FechaReclamo = @FechaReclamo,
                        IdReporta = @IdReporta,
                        IdUnidad = @IdUnidad,
                        IdClienteExterno = @IdClienteExterno,
                        FuenteReclamo = @FuenteReclamo,
                        Categoria = @Categoria,
                        Tarea = @Tarea,
                        Descripcion = @Descripcion,
                        FechaActualizacion = GETDATE()
                    WHERE Id = @Id";

                var affected = await connection.ExecuteAsync(sql, pnc);
                return affected > 0;
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException($"Error updating PNC_ProductoNoConforme Id {pnc.Id}", ex);
            }
        }

        public async Task<bool> CerrarPnc(int idPnc, long idUsuario)
        {
            try
            {
                using var connection = new SqlConnection(_connectionString);
                var sql = @"
                    UPDATE PNC_ProductoNoConforme
                    SET Cerrado = 1,
                        FechaCierre = GETDATE(),
                        Usuario = @IdUsuario,
                        FechaActualizacion = GETDATE()
                    WHERE Id = @IdPnc";

                var affected = await connection.ExecuteAsync(sql, new { IdPnc = idPnc, IdUsuario = idUsuario });
                return affected > 0;
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException($"Error closing PNC Id {idPnc}", ex);
            }
        }

        // ============= CRUD CAUSAS =============

        public async Task<int> InsertarCausa(ProductoNoConformeCausaVM causa)
        {
            try
            {
                using var connection = new SqlConnection(_connectionString);
                var sql = @"
                    INSERT INTO PNC_ProductoNoConformeCausas (IdPNC, CausaRaiz)
                    VALUES (@IdPNC, @CausaRaiz);
                    SELECT CAST(SCOPE_IDENTITY() AS INT);";

                var id = await connection.ExecuteScalarAsync<int>(sql, causa);
                return id;
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException("Error inserting PNC_ProductoNoConformeCausas", ex);
            }
        }

        public async Task<bool> ActualizarCausa(ProductoNoConformeCausaVM causa)
        {
            try
            {
                using var connection = new SqlConnection(_connectionString);
                var sql = @"
                    UPDATE PNC_ProductoNoConformeCausas
                    SET CausaRaiz = @CausaRaiz
                    WHERE Id = @Id";

                var affected = await connection.ExecuteAsync(sql, causa);
                return affected > 0;
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException($"Error updating PNC_ProductoNoConformeCausas Id {causa.Id}", ex);
            }
        }

        public async Task<bool> EliminarCausa(int idCausa)
        {
            try
            {
                using var connection = new SqlConnection(_connectionString);
                
                // Primero eliminar acciones relacionadas
                var sqlAcciones = "DELETE FROM PNC_ProductoNoConformeAcciones WHERE IdCausa = @IdCausa";
                await connection.ExecuteAsync(sqlAcciones, new { IdCausa = idCausa });

                // Luego eliminar la causa
                var sql = "DELETE FROM PNC_ProductoNoConformeCausas WHERE Id = @IdCausa";
                var affected = await connection.ExecuteAsync(sql, new { IdCausa = idCausa });
                return affected > 0;
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException($"Error deleting PNC_ProductoNoConformeCausas Id {idCausa}", ex);
            }
        }

        // ============= CRUD ACCIONES =============

        public async Task<int> InsertarAccion(ProductoNoConformeAccionVM accion)
        {
            try
            {
                using var connection = new SqlConnection(_connectionString);
                var sql = @"
                    INSERT INTO PNC_ProductoNoConformeAcciones 
                        (IdPNC, IdCausa, TipoAccion, Accion, FechaPlaneada, FechaEjecucion,
                         IdResponsableAccion, IdResponsableSeguimiento, EvidenciaCierre, PermiteActualizar)
                    VALUES 
                        (@IdPNC, @IdCausa, @TipoAccion, @Accion, @FechaPlaneada, @FechaEjecucion,
                         @IdResponsableAccion, @IdResponsableSeguimiento, @EvidenciaCierre, @PermiteActualizar);
                    SELECT CAST(SCOPE_IDENTITY() AS INT);";

                var id = await connection.ExecuteScalarAsync<int>(sql, accion);
                return id;
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException("Error inserting PNC_ProductoNoConformeAcciones", ex);
            }
        }

        public async Task<bool> ActualizarAccion(ProductoNoConformeAccionVM accion)
        {
            try
            {
                using var connection = new SqlConnection(_connectionString);
                var sql = @"
                    UPDATE PNC_ProductoNoConformeAcciones
                    SET TipoAccion = @TipoAccion,
                        Accion = @Accion,
                        FechaPlaneada = @FechaPlaneada,
                        FechaEjecucion = @FechaEjecucion,
                        IdResponsableAccion = @IdResponsableAccion,
                        IdResponsableSeguimiento = @IdResponsableSeguimiento,
                        EvidenciaCierre = @EvidenciaCierre,
                        PermiteActualizar = @PermiteActualizar
                    WHERE Id = @Id";

                var affected = await connection.ExecuteAsync(sql, accion);
                return affected > 0;
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException($"Error updating PNC_ProductoNoConformeAcciones Id {accion.Id}", ex);
            }
        }

        public async Task<bool> EjecutarAccion(int idAccion, DateTime fechaEjecucion, string evidenciaCierre)
        {
            try
            {
                using var connection = new SqlConnection(_connectionString);
                var sql = @"
                    UPDATE PNC_ProductoNoConformeAcciones
                    SET FechaEjecucion = @FechaEjecucion,
                        EvidenciaCierre = @EvidenciaCierre
                    WHERE Id = @IdAccion";

                var affected = await connection.ExecuteAsync(sql, 
                    new { IdAccion = idAccion, FechaEjecucion = fechaEjecucion, EvidenciaCierre = evidenciaCierre });
                return affected > 0;
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException($"Error executing PNC_ProductoNoConformeAcciones Id {idAccion}", ex);
            }
        }

        public async Task<bool> EliminarAccion(int idAccion)
        {
            try
            {
                using var connection = new SqlConnection(_connectionString);
                var sql = "DELETE FROM PNC_ProductoNoConformeAcciones WHERE Id = @IdAccion";
                var affected = await connection.ExecuteAsync(sql, new { IdAccion = idAccion });
                return affected > 0;
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException($"Error deleting PNC_ProductoNoConformeAcciones Id {idAccion}", ex);
            }
        }

        // ============= VALIDACIONES =============

        public async Task<bool> ExisteAccion(int idPnc, int idCausa, int tipoAccion)
        {
            try
            {
                using var connection = new SqlConnection(_connectionString);
                var sql = @"
                    SELECT COUNT(*) 
                    FROM PNC_ProductoNoConformeAcciones
                    WHERE IdPNC = @IdPNC AND IdCausa = @IdCausa AND TipoAccion = @TipoAccion";

                var count = await connection.ExecuteScalarAsync<int>(sql, 
                    new { IdPNC = idPnc, IdCausa = idCausa, TipoAccion = tipoAccion });
                return count > 0;
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException($"Error checking ExisteAccion for PNC {idPnc}, Causa {idCausa}, Tipo {tipoAccion}", ex);
            }
        }

        public async Task<bool> TodasAccionesEjecutadas(int idPnc)
        {
            try
            {
                using var connection = new SqlConnection(_connectionString);
                var sql = @"
                    SELECT COUNT(*) 
                    FROM PNC_ProductoNoConformeAcciones
                    WHERE IdPNC = @IdPNC AND FechaEjecucion IS NULL";

                var pendientes = await connection.ExecuteScalarAsync<int>(sql, new { IdPNC = idPnc });
                return pendientes == 0;
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException($"Error checking TodasAccionesEjecutadas for PNC {idPnc}", ex);
            }
        }
    }
}
