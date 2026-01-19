using Dapper;
using MatrixNext.Data.DTOs.PY;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Logging;
using System.Data;

namespace MatrixNext.Data.Services.PY;

public class DuplicarTrabajoService : IDuplicarTrabajoService
{
    private readonly string _connectionString;
    private readonly ILogger<DuplicarTrabajoService> _logger;

    public DuplicarTrabajoService(string connectionString, ILogger<DuplicarTrabajoService> logger)
    {
        _connectionString = connectionString;
        _logger = logger;
    }

    public async Task<DuplicarTrabajoViewModel> PrepararViewModelAsync(long idTrabajo)
    {
        try
        {
            using var connection = new SqlConnection(_connectionString);

            var sql = @"
                SELECT 
                    t.id AS IdTrabajo,
                    t.ProyectoId AS IdProyecto,
                    t.JobBook,
                    t.NombreTrabajo,
                    t.Modalidad,
                    t.Muestra,
                    t.FechaTentativaInicioCampo,
                    t.FechaTentativaFinalizacion,
                    t.NoMedicion,
                    p.TipoProyectoId,
                    c.Nombre AS NombreCliente,
                    CONCAT(u.Nombres, ' ', u.Apellidos) AS GerenteProyecto
                FROM PY_Trabajo t
                INNER JOIN PY_Proyectos p ON t.ProyectoId = p.id
                LEFT JOIN CU_Clientes c ON p.ClienteId = c.IdCliente
                LEFT JOIN US_Usuarios u ON p.GerentePY = u.id
                WHERE t.id = @IdTrabajo";

            var result = await connection.QueryFirstOrDefaultAsync<DuplicarTrabajoViewModel>(sql, new { IdTrabajo = idTrabajo });

            if (result == null)
            {
                _logger.LogWarning("Trabajo {IdTrabajo} no encontrado", idTrabajo);
                return new DuplicarTrabajoViewModel { IdTrabajo = idTrabajo };
            }

            return result;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error preparando ViewModel para duplicar trabajo {IdTrabajo}", idTrabajo);
            throw;
        }
    }

    public async Task<(bool success, string message, long? idNuevo)> DuplicarTrabajoAsync(DuplicarTrabajoDto dto, long userId)
    {
        using var connection = new SqlConnection(_connectionString);
        await connection.OpenAsync();
        using var transaction = connection.BeginTransaction();

        try
        {
            // 1. Obtener info del trabajo original
            var trabajoOrigen = await connection.QueryFirstOrDefaultAsync<dynamic>(@"
                SELECT * FROM PY_Trabajo WHERE id = @Id", 
                new { Id = dto.IdTrabajoOrigen }, 
                transaction);

            if (trabajoOrigen == null)
            {
                return (false, "Trabajo origen no encontrado", null);
            }

            // 2. Calcular fechas (si SumarUnMes = true)
            var fechaInicio = dto.FechaTentativaInicioCampo ?? trabajoOrigen.FechaTentativaInicioCampo;
            var fechaFin = dto.FechaTentativaFinalizacion ?? trabajoOrigen.FechaTentativaFinalizacion;

            if (dto.SumarUnMes)
            {
                fechaInicio = fechaInicio?.AddMonths(1);
                fechaFin = fechaFin?.AddMonths(1);
            }

            // 3. Crear nuevo trabajo (INSERT duplicando campos)
            var sqlInsert = @"
                INSERT INTO PY_Trabajo (
                    ProyectoId, OP_MetodologiaId, PresupuestoId, NombreTrabajo, Muestra,
                    FechaTentativaInicioCampo, FechaTentativaFinalizacion, FechaCierre,
                    Unidad, JobBook, TipoRecoleccionId, Estado, IdPropuesta, Alternativa,
                    MetCodigo, Fase, NoMedicion
                )
                SELECT 
                    ProyectoId, OP_MetodologiaId, PresupuestoId, @NombreNuevo, Muestra,
                    @FechaInicio, @FechaFin, NULL,
                    Unidad, JobBook, TipoRecoleccionId, Estado, IdPropuesta, Alternativa,
                    MetCodigo, Fase, @NoMedicion
                FROM PY_Trabajo
                WHERE id = @IdOrigen;
                SELECT CAST(SCOPE_IDENTITY() AS BIGINT);";

            var idNuevo = await connection.ExecuteScalarAsync<long>(sqlInsert, new
            {
                NombreNuevo = dto.NombreNuevo,
                FechaInicio = fechaInicio,
                FechaFin = fechaFin,
                NoMedicion = dto.NumeroMedicion ?? trabajoOrigen.NoMedicion,
                IdOrigen = dto.IdTrabajoOrigen
            }, transaction);

            _logger.LogInformation("Trabajo duplicado. Origen: {IdOrigen}, Nuevo: {IdNuevo}", dto.IdTrabajoOrigen, idNuevo);

            // 4. Duplicar TrabajoConfiguracion
            await connection.ExecuteAsync(@"
                INSERT INTO OP_TrabajoConfiguracion (IdTrabajo, FechaInicio, FechaFin, PorcentajeVerificacion, UnidadCritica)
                SELECT @IdNuevo, @FechaInicio, @FechaFin, PorcentajeVerificacion, UnidadCritica
                FROM OP_TrabajoConfiguracion
                WHERE IdTrabajo = @IdOrigen",
                new { IdNuevo = idNuevo, IdOrigen = dto.IdTrabajoOrigen, FechaInicio = fechaInicio, FechaFin = fechaFin },
                transaction);

            // 5. Duplicar Muestra (ciudades)
            await connection.ExecuteAsync(@"
                INSERT INTO CC_MuestraxEstudio (IdTrabajo, CiudadId, Cantidad, Coordinador, FechaInicio, FechaFin)
                SELECT @IdNuevo, CiudadId, Cantidad, Coordinador, @FechaInicio, @FechaFin
                FROM CC_MuestraxEstudio
                WHERE IdTrabajo = @IdOrigen",
                new { IdNuevo = idNuevo, IdOrigen = dto.IdTrabajoOrigen, FechaInicio = fechaInicio, FechaFin = fechaFin },
                transaction);

            // 6. Duplicar Especificaciones (si aplica)
            if (dto.DuplicarEspecificaciones)
            {
                await connection.ExecuteAsync(@"
                    INSERT INTO PY_EspecifTecTrabajo (
                        TrabajoId, NoVersion, NumeroEncuestas, TiempoPromedio, Etapas, 
                        RegistradoPor, FechaRegistro
                    )
                    SELECT 
                        @IdNuevo, NoVersion, NumeroEncuestas, TiempoPromedio, Etapas,
                        @UserId, GETDATE()
                    FROM PY_EspecifTecTrabajo
                    WHERE TrabajoId = @IdOrigen",
                    new { IdNuevo = idNuevo, IdOrigen = dto.IdTrabajoOrigen, UserId = userId },
                    transaction);

                // Duplicar FichaCuantitativo si existe
                await connection.ExecuteAsync(@"
                    INSERT INTO OP_FichaCuantitativo (
                        IdTrabajo, GrupoObjetivo, CubrimientoGeografico, MarcoMuestral, 
                        DistribucionMuestra, Cuotas, NivelDesagregacionResultados, Ponderacion,
                        RequerimientosEspeciales, OtrasObservaciones, IncentivoEconomico,
                        PresupuestoIncentivo, RegalosCliente, CompraIpsos, Presupuesto
                    )
                    SELECT 
                        @IdNuevo, GrupoObjetivo, CubrimientoGeografico, MarcoMuestral,
                        DistribucionMuestra, Cuotas, NivelDesagregacionResultados, Ponderacion,
                        RequerimientosEspeciales, OtrasObservaciones, IncentivoEconomico,
                        PresupuestoIncentivo, RegalosCliente, CompraIpsos, Presupuesto
                    FROM OP_FichaCuantitativo
                    WHERE IdTrabajo = @IdOrigen",
                    new { IdNuevo = idNuevo, IdOrigen = dto.IdTrabajoOrigen },
                    transaction);
            }

            // 7. Agregar estimación automática de planeación
            try
            {
                await connection.ExecuteAsync(
                    "CC_AgregarEstimacionAutomatica",
                    new
                    {
                        idTrabajo = idNuevo,
                        idUsuario = userId,
                        Analista = true,
                        Operador = true,
                        Tabulador = true,
                        Codificador = true,
                        Coordinador = true,
                        Programador = true,
                        Supervisor = false,
                        Encuestador = false
                    },
                    transaction,
                    commandType: CommandType.StoredProcedure);
            }
            catch (Exception ex)
            {
                _logger.LogWarning(ex, "Error agregando estimación automática para trabajo {IdNuevo}", idNuevo);
                // No abortar transacción, es opcional
            }

            // 8. Duplicar documentos (si aplica) - solo registros en BD, archivos físicos no
            if (dto.DuplicarDocumentos)
            {
                await connection.ExecuteAsync(@"
                    INSERT INTO GD_DocumentosTrabajo (IdTrabajo, TipoDocumento, NombreArchivo, Ruta, RegistradoPor, FechaRegistro)
                    SELECT @IdNuevo, TipoDocumento, NombreArchivo, Ruta, @UserId, GETDATE()
                    FROM GD_DocumentosTrabajo
                    WHERE IdTrabajo = @IdOrigen",
                    new { IdNuevo = idNuevo, IdOrigen = dto.IdTrabajoOrigen, UserId = userId },
                    transaction);
            }

            await transaction.CommitAsync();

            return (true, $"Trabajo duplicado exitosamente. ID nuevo trabajo: {idNuevo}", idNuevo);
        }
        catch (Exception ex)
        {
            await transaction.RollbackAsync();
            _logger.LogError(ex, "Error duplicando trabajo {IdOrigen}", dto.IdTrabajoOrigen);
            return (false, "Error al duplicar el trabajo", null);
        }
    }
}
