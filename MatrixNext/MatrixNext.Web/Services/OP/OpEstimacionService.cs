using Dapper;
using MatrixNext.Web.Infrastructure.Data;
using MatrixNext.Web.Models.OP;
using Microsoft.EntityFrameworkCore;
using System.Data;

namespace MatrixNext.Web.Services.OP
{
    /// <summary>
    /// Implementación del servicio de estimación de producción
    /// </summary>
    public class OpEstimacionService : IOpEstimacionService
    {
        private readonly MatrixDbContext _dbContext;
        private readonly ILogger<OpEstimacionService> _logger;

        public OpEstimacionService(MatrixDbContext dbContext, ILogger<OpEstimacionService> logger)
        {
            _dbContext = dbContext;
            _logger = logger;
        }

        /// <inheritdoc />
        public async Task<List<EstimacionCiudadListItemVM>> ObtenerEstimacionesPorTrabajoAsync(long trabajoId)
        {
            try
            {
                // Query EF Core para estimaciones con joins necesarios
                var estimaciones = await _dbContext.Database
                    .SqlQueryRaw<EstimacionCiudadDto>(@"
                        SELECT 
                            e.id AS Id,
                            d.DivMuniNombre AS Ciudad,
                            e.FechaEstimacion,
                            CONCAT(u.Nombres, ' ', u.Apellidos) AS UsuarioNombre,
                            e.Observaciones,
                            e.Activa,
                            e.Bloqueada
                        FROM OP_EstimacionesProduccionCiudad e
                        LEFT JOIN C_Divipola d ON e.CiudadId = d.DivMuniCodigo
                        LEFT JOIN US_Usuarios u ON e.UsuarioEstimacion = u.id
                        WHERE e.TrabajoId = {0}
                        ORDER BY e.FechaEstimacion DESC",
                        trabajoId)
                    .ToListAsync();

                return estimaciones.Select(e => new EstimacionCiudadListItemVM
                {
                    Id = e.Id,
                    Ciudad = e.Ciudad,
                    FechaEstimacion = e.FechaEstimacion,
                    UsuarioNombre = e.UsuarioNombre,
                    Observaciones = e.Observaciones,
                    Activa = e.Activa,
                    Bloqueada = e.Bloqueada
                }).ToList();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener estimaciones del trabajo {TrabajoId}", trabajoId);
                return new List<EstimacionCiudadListItemVM>();
            }
        }

        public async Task<EstimacionDetalleVM?> ObtenerEstimacionDetalleAsync(long estimacionId)
        {
            try
            {
                await using var connection = _dbContext.Database.GetDbConnection();
                if (connection.State != ConnectionState.Open)
                    await connection.OpenAsync();

                // Obtener datos de la estimación
                var estimacionData = await connection.QueryFirstOrDefaultAsync<EstimacionCiudadDto>(@"
                    SELECT 
                        e.id AS Id,
                        e.TrabajoId,
                        e.CiudadId,
                        d.DivMuniNombre AS Ciudad,
                        e.FechaEstimacion,
                        e.Observaciones,
                        e.Activa,
                        e.Bloqueada
                    FROM OP_EstimacionesProduccionCiudad e
                    LEFT JOIN C_Divipola d ON e.CiudadId = d.DivMuniCodigo
                    WHERE e.id = @EstimacionId",
                    new { EstimacionId = estimacionId });

                if (estimacionData == null)
                    return null;

                // Obtener planeación diaria
                var planeacionDias = await connection.QueryAsync<PlaneacionDiaDto>(@"
                    SELECT id AS Id, Fecha, Cantidad
                    FROM OP_EstimacionProduccion
                    WHERE EstimacionId = @EstimacionId
                    ORDER BY Fecha",
                    new { EstimacionId = estimacionId });

                return new EstimacionDetalleVM
                {
                    IdEstimacion = estimacionData.Id,
                    IdTrabajo = estimacionData.TrabajoId,
                    CiudadId = estimacionData.CiudadId,
                    CiudadNombre = estimacionData.Ciudad,
                    FechaEstimacion = estimacionData.FechaEstimacion,
                    Observaciones = estimacionData.Observaciones,
                    Activa = estimacionData.Activa,
                    Bloqueada = estimacionData.Bloqueada,
                    PlaneacionDias = planeacionDias.Select(p => new PlaneacionDiaVM
                    {
                        Id = p.Id,
                        Fecha = p.Fecha,
                        Cantidad = p.Cantidad
                    }).ToList()
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener detalle de estimación {EstimacionId}", estimacionId);
                return null;
            }
        }

        public async Task<long> CrearEstimacionAsync(CrearEstimacionVM model, long usuarioId)
        {
            try
            {
                await using var connection = _dbContext.Database.GetDbConnection();
                if (connection.State != ConnectionState.Open)
                    await connection.OpenAsync();

                // Primero insertar el registro de estimación de ciudad
                var estimacionId = await connection.QuerySingleAsync<long>(@"
                    INSERT INTO OP_EstimacionesProduccionCiudad 
                        (TrabajoId, FechaEstimacion, CiudadId, UsuarioEstimacion, Observaciones, Bloqueada, Activa)
                    VALUES 
                        (@TrabajoId, GETDATE(), @CiudadId, @UsuarioId, @Observaciones, 1, 0);
                    SELECT CAST(SCOPE_IDENTITY() as bigint);",
                    new
                    {
                        model.TrabajoId,
                        model.CiudadId,
                        UsuarioId = usuarioId,
                        model.Observaciones
                    });

                // Ejecutar SP para generar planeación automática
                // Mapea a OP_PlaneaccionProduccionManual
                await connection.ExecuteAsync(
                    "OP_PlaneaccionProduccionManual",
                    new
                    {
                        trabajoId = model.TrabajoId,
                        usuarioId = usuarioId,
                        lun = model.Lunes,
                        mar = model.Martes,
                        mie = model.Miercoles,
                        jue = model.Jueves,
                        vie = model.Viernes,
                        sab = model.Sabado,
                        dom = model.Domingo,
                        fest = !model.ExcluirFestivos, // SP usa 'fest' para incluir, no excluir
                        IdEstimacion = estimacionId
                    },
                    commandType: CommandType.StoredProcedure);

                _logger.LogInformation("Estimación {EstimacionId} creada para trabajo {TrabajoId}, ciudad {CiudadId}",
                    estimacionId, model.TrabajoId, model.CiudadId);

                return estimacionId;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al crear estimación para trabajo {TrabajoId}", model.TrabajoId);
                throw;
            }
        }

        public async Task ActualizarCantidadDiaAsync(long planeacionId, short cantidad)
        {
            try
            {
                await using var connection = _dbContext.Database.GetDbConnection();
                if (connection.State != ConnectionState.Open)
                    await connection.OpenAsync();

                await connection.ExecuteAsync(@"
                    UPDATE OP_EstimacionProduccion
                    SET Cantidad = @Cantidad
                    WHERE id = @PlaneacionId",
                    new { PlaneacionId = planeacionId, Cantidad = cantidad });

                _logger.LogInformation("Cantidad de planeación {PlaneacionId} actualizada a {Cantidad}",
                    planeacionId, cantidad);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al actualizar cantidad de planeación {PlaneacionId}", planeacionId);
                throw;
            }
        }

        public async Task ActualizarCantidadesBatchAsync(List<PlaneacionDiaVM> actualizaciones)
        {
            try
            {
                await using var connection = _dbContext.Database.GetDbConnection();
                if (connection.State != ConnectionState.Open)
                    await connection.OpenAsync();

                await using var transaction = await connection.BeginTransactionAsync();
                try
                {
                    foreach (var actualizacion in actualizaciones)
                    {
                        await connection.ExecuteAsync(@"
                            UPDATE OP_EstimacionProduccion
                            SET Cantidad = @Cantidad
                            WHERE id = @Id",
                            new { actualizacion.Id, actualizacion.Cantidad },
                            transaction: transaction);
                    }

                    await transaction.CommitAsync();
                    _logger.LogInformation("Batch de {Count} cantidades actualizado correctamente",
                        actualizaciones.Count);
                }
                catch
                {
                    await transaction.RollbackAsync();
                    throw;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al actualizar batch de cantidades");
                throw;
            }
        }

        public async Task<bool> ActivarEstimacionAsync(long estimacionId)
        {
            try
            {
                await using var connection = _dbContext.Database.GetDbConnection();
                if (connection.State != ConnectionState.Open)
                    await connection.OpenAsync();

                // Ejecutar SP que desactiva otras estimaciones de la misma ciudad y activa la solicitada
                await connection.ExecuteAsync(
                    "OP_Planeacion_ActivarEstimacion",
                    new { idEstimacion = estimacionId },
                    commandType: CommandType.StoredProcedure);

                _logger.LogInformation("Estimación {EstimacionId} activada correctamente", estimacionId);
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al activar estimación {EstimacionId}", estimacionId);
                return false;
            }
        }

        public async Task<(bool esValido, long sumaEstimada, long muestraEsperada)> ValidarEstimacionVsMuestraAsync(long estimacionId)
        {
            try
            {
                await using var connection = _dbContext.Database.GetDbConnection();
                if (connection.State != ConnectionState.Open)
                    await connection.OpenAsync();

                // Obtener la estimación para saber trabajo y ciudad
                var estimacion = await connection.QueryFirstOrDefaultAsync<dynamic>(@"
                    SELECT TrabajoId, CiudadId
                    FROM OP_EstimacionesProduccionCiudad
                    WHERE id = @EstimacionId",
                    new { EstimacionId = estimacionId });

                if (estimacion == null)
                    return (false, 0, 0);

                // Suma de cantidades estimadas
                var sumaEstimada = await connection.QueryFirstOrDefaultAsync<long?>(@"
                    SELECT SUM(CAST(Cantidad AS bigint))
                    FROM OP_EstimacionProduccion
                    WHERE EstimacionId = @EstimacionId",
                    new { EstimacionId = estimacionId }) ?? 0;

                // Obtener muestra esperada de la ciudad
                // Mapea a CoordinacionCampo.ObtenerMuestraxTrabajoYCiudad
                var muestraEsperada = await connection.QueryFirstOrDefaultAsync<long?>(@"
                    SELECT CAST(Muestra AS bigint)
                    FROM CoordinacionCampo_MuestraXEstudio
                    WHERE IdTrabajo = @TrabajoId AND CiudadId = @CiudadId",
                    new { TrabajoId = (long)estimacion.TrabajoId, CiudadId = (int)estimacion.CiudadId }) ?? 0;

                var esValido = sumaEstimada == muestraEsperada;

                _logger.LogInformation(
                    "Validación estimación {EstimacionId}: suma={SumaEstimada}, muestra={MuestraEsperada}, válido={EsValido}",
                    estimacionId, sumaEstimada, muestraEsperada, esValido);

                return (esValido, sumaEstimada, muestraEsperada);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al validar estimación {EstimacionId}", estimacionId);
                return (false, 0, 0);
            }
        }

        #region DTOs Internos

        private class EstimacionCiudadDto
        {
            public long Id { get; set; }
            public long TrabajoId { get; set; }
            public int CiudadId { get; set; }
            public string? Ciudad { get; set; }
            public DateTime? FechaEstimacion { get; set; }
            public string? UsuarioNombre { get; set; }
            public string? Observaciones { get; set; }
            public bool Activa { get; set; }
            public bool Bloqueada { get; set; }
        }

        private class PlaneacionDiaDto
        {
            public long Id { get; set; }
            public DateTime Fecha { get; set; }
            public short Cantidad { get; set; }
        }

        #endregion
    }
}
