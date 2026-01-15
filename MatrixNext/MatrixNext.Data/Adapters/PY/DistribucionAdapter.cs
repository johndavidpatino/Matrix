/// <summary>
/// Adapter para distribución de entrevistas, variables de control e InHome visits
/// Ref: BACKLOG_QA_MODULOS_PENDIENTES.md § Sprint 12.2.1-12.2.3
/// </summary>
namespace MatrixNext.Data.Adapters.PY;

using Dapper;
using MatrixNext.Data.Models.PY;
using Microsoft.Extensions.Logging;
using System.Data;

public class DistribucionAdapter : IDistribucionAdapter
{
    private readonly IDbConnection _connection;
    private readonly ILogger<DistribucionAdapter> _logger;

    public DistribucionAdapter(IDbConnection connection, ILogger<DistribucionAdapter> logger)
    {
        _connection = connection;
        _logger = logger;
    }

    // ===== SPRINT 12.2.1: Distribución de Entrevistas =====
    
    public async Task<List<DistribucionEntrevistaDto>> ObtenerDistribucionesAsync(long idTrabajo)
    {
        try
        {
            var query = @"
                SELECT 
                    d.IdDistribucion, d.IdTrabajo, t.NumeroTrabajo,
                    d.IdMetodologia, m.Nombre AS NombreMetodologia,
                    d.IdUnidad, u.Nombre AS NombreUnidad,
                    d.Ciudad, d.CantidadAsignada, d.CantidadCompletada,
                    CASE WHEN d.CantidadAsignada > 0 
                         THEN (CAST(ISNULL(d.CantidadCompletada, 0) AS DECIMAL) / d.CantidadAsignada) * 100 
                         ELSE 0 END AS PorcentajeAvance,
                    d.FechaAsignacion, d.AsignadoPor
                FROM PY_DistribucionEntrevistas d
                INNER JOIN PY_Trabajos t ON d.IdTrabajo = t.IdTrabajo
                INNER JOIN PY_Metodologias m ON d.IdMetodologia = m.IdMetodologia
                INNER JOIN OP_Unidades u ON d.IdUnidad = u.IdUnidad
                WHERE d.IdTrabajo = @IdTrabajo
                ORDER BY d.IdMetodologia, d.IdUnidad";

            var distribuciones = await _connection.QueryAsync<DistribucionEntrevistaDto>(query, new { IdTrabajo = idTrabajo });
            _logger.LogInformation("Obtenidas {Count} distribuciones para trabajo {IdTrabajo}", distribuciones.Count(), idTrabajo);
            return distribuciones.ToList();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error obteniendo distribuciones para trabajo {IdTrabajo}", idTrabajo);
            throw;
        }
    }

    public async Task<ResumenDistribucionDto> ObtenerResumenAsync(long idTrabajo)
    {
        try
        {
            var query = @"
                SELECT 
                    @IdTrabajo AS IdTrabajo,
                    t.TotalMuestra AS TotalMuestra,
                    SUM(d.CantidadAsignada) AS TotalDistribuido,
                    SUM(ISNULL(d.CantidadCompletada, 0)) AS TotalCompletado,
                    CASE WHEN t.TotalMuestra > 0 
                         THEN (CAST(SUM(d.CantidadAsignada) AS DECIMAL) / t.TotalMuestra) * 100 
                         ELSE 0 END AS PorcentajeDistribucion,
                    CASE WHEN SUM(d.CantidadAsignada) > 0 
                         THEN (CAST(SUM(ISNULL(d.CantidadCompletada, 0)) AS DECIMAL) / SUM(d.CantidadAsignada)) * 100 
                         ELSE 0 END AS PorcentajeAvance
                FROM PY_Trabajos t
                LEFT JOIN PY_DistribucionEntrevistas d ON t.IdTrabajo = d.IdTrabajo
                WHERE t.IdTrabajo = @IdTrabajo
                GROUP BY t.TotalMuestra";

            var resumen = await _connection.QuerySingleAsync<ResumenDistribucionDto>(query, new { IdTrabajo = idTrabajo });
            _logger.LogInformation("Resumen distribución trabajo {IdTrabajo}: {Distribuido}/{Total}", 
                idTrabajo, resumen.TotalDistribuido, resumen.TotalMuestra);
            return resumen;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error obteniendo resumen distribución trabajo {IdTrabajo}", idTrabajo);
            throw;
        }
    }

    public async Task<bool> DistribuirPorUnidadAsync(DistribuirPorUnidadDto distribucion)
    {
        try
        {
            using (var transaction = _connection.BeginTransaction())
            {
                foreach (var unidad in distribucion.Unidades)
                {
                    await _connection.ExecuteAsync(@"
                        INSERT INTO PY_DistribucionEntrevistas 
                            (IdTrabajo, IdMetodologia, IdUnidad, Ciudad, CantidadAsignada, FechaAsignacion, AsignadoPor)
                        VALUES (@IdTrabajo, @IdMetodologia, @IdUnidad, @Ciudad, @Cantidad, @FechaAsignacion, @AsignadoPor)",
                        new
                        {
                            distribucion.IdTrabajo,
                            distribucion.IdMetodologia,
                            unidad.IdUnidad,
                            unidad.Ciudad,
                            unidad.Cantidad,
                            FechaAsignacion = DateTime.Now,
                            distribucion.AsignadoPor
                        },
                        transaction);
                }
                transaction.Commit();
            }

            _logger.LogInformation("Distribución por unidad completada. Trabajo: {IdTrabajo}, {Count} unidades",
                distribucion.IdTrabajo, distribucion.Unidades.Count);
            return true;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error distribuyendo por unidad. Trabajo: {IdTrabajo}", distribucion.IdTrabajo);
            throw;
        }
    }

    public async Task<List<CuotaDistribucionDto>> ObtenerCuotasAsync(long idDistribucion)
    {
        try
        {
            var query = @"
                SELECT 
                    IdCuota, IdDistribucion, VariableCuota, ValorCuota,
                    CantidadRequerida, CantidadObtenida,
                    CASE WHEN CantidadObtenida >= CantidadRequerida THEN 1 ELSE 0 END AS CumpleCuota
                FROM PY_CuotasDistribucion
                WHERE IdDistribucion = @IdDistribucion
                ORDER BY VariableCuota, ValorCuota";

            var cuotas = await _connection.QueryAsync<CuotaDistribucionDto>(query, new { IdDistribucion = idDistribucion });
            _logger.LogInformation("Obtenidas {Count} cuotas para distribución {IdDistribucion}", cuotas.Count(), idDistribucion);
            return cuotas.ToList();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error obteniendo cuotas distribución {IdDistribucion}", idDistribucion);
            throw;
        }
    }

    public async Task<bool> ValidarSumaDistribucionAsync(long idTrabajo, int sumaDistribucion)
    {
        try
        {
            var totalMuestra = await _connection.ExecuteScalarAsync<int>(
                "SELECT TotalMuestra FROM PY_Trabajos WHERE IdTrabajo = @IdTrabajo",
                new { IdTrabajo = idTrabajo });

            var esValido = sumaDistribucion == totalMuestra;
            _logger.LogInformation("Validación suma distribución: {Suma} == {Total} = {Resultado}",
                sumaDistribucion, totalMuestra, esValido);
            return esValido;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error validando suma distribución trabajo {IdTrabajo}", idTrabajo);
            return false;
        }
    }

    // ===== SPRINT 12.2.2: Variables de Control =====
    
    public async Task<List<VariableControlDto>> ObtenerVariablesControlAsync(long idTrabajo)
    {
        try
        {
            var query = @"
                SELECT 
                    IdVariable, IdTrabajo, NombreVariable, TipoDato,
                    ValorMinimo, ValorMaximo, ValoresPermitidos, Obligatorio,
                    Descripcion, FechaRegistro, RegistradoPor
                FROM PY_VariablesControl
                WHERE IdTrabajo = @IdTrabajo
                ORDER BY NombreVariable";

            var variables = await _connection.QueryAsync<VariableControlDto>(query, new { IdTrabajo = idTrabajo });
            _logger.LogInformation("Obtenidas {Count} variables de control para trabajo {IdTrabajo}", variables.Count(), idTrabajo);
            return variables.ToList();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error obteniendo variables de control trabajo {IdTrabajo}", idTrabajo);
            throw;
        }
    }

    public async Task<long> CrearVariableControlAsync(VariableControlDto variable)
    {
        try
        {
            var parameters = new DynamicParameters();
            parameters.Add("@IdTrabajo", variable.IdTrabajo);
            parameters.Add("@NombreVariable", variable.NombreVariable);
            parameters.Add("@TipoDato", variable.TipoDato);
            parameters.Add("@ValorMinimo", variable.ValorMinimo);
            parameters.Add("@ValorMaximo", variable.ValorMaximo);
            parameters.Add("@ValoresPermitidos", variable.ValoresPermitidos);
            parameters.Add("@Obligatorio", variable.Obligatorio);
            parameters.Add("@Descripcion", variable.Descripcion);
            parameters.Add("@FechaRegistro", DateTime.Now);
            parameters.Add("@RegistradoPor", variable.RegistradoPor);
            parameters.Add("@IdVariable", dbType: DbType.Int64, direction: ParameterDirection.Output);

            await _connection.ExecuteAsync(
                "PY_VariablesControl_Add",
                parameters,
                commandType: CommandType.StoredProcedure);

            var idVariable = parameters.Get<long>("@IdVariable");
            _logger.LogInformation("Variable de control {Id} creada: {Nombre}", idVariable, variable.NombreVariable);
            return idVariable;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error creando variable de control: {Nombre}", variable.NombreVariable);
            throw;
        }
    }

    public async Task<bool> ActualizarVariableControlAsync(VariableControlDto variable)
    {
        try
        {
            var query = @"
                UPDATE PY_VariablesControl
                SET NombreVariable = @NombreVariable,
                    TipoDato = @TipoDato,
                    ValorMinimo = @ValorMinimo,
                    ValorMaximo = @ValorMaximo,
                    ValoresPermitidos = @ValoresPermitidos,
                    Obligatorio = @Obligatorio,
                    Descripcion = @Descripcion
                WHERE IdVariable = @IdVariable";

            var rowsAffected = await _connection.ExecuteAsync(query, variable);
            _logger.LogInformation("Variable de control {Id} actualizada", variable.IdVariable);
            return rowsAffected > 0;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error actualizando variable de control {Id}", variable.IdVariable);
            throw;
        }
    }

    public async Task<bool> EliminarVariableControlAsync(long idVariable)
    {
        try
        {
            var rowsAffected = await _connection.ExecuteAsync(
                "DELETE FROM PY_VariablesControl WHERE IdVariable = @IdVariable",
                new { IdVariable = idVariable });

            _logger.LogInformation("Variable de control {Id} eliminada", idVariable);
            return rowsAffected > 0;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error eliminando variable de control {Id}", idVariable);
            throw;
        }
    }

    // ===== SPRINT 12.2.3: InHome Visit =====
    
    public async Task<List<InHomeVisitDto>> ObtenerInHomeVisitsAsync(long idTrabajo)
    {
        try
        {
            var query = @"
                SELECT 
                    v.IdVisita, v.IdTrabajo, t.NumeroTrabajo,
                    v.LugarVisita, v.FechaProgramada, v.FechaRealizada,
                    v.Estado, v.CantidadParticipantes, v.Recursos, v.Observaciones,
                    v.ResponsableId, CONCAT(e.Nombres, ' ', e.Apellidos) AS NombreResponsable,
                    v.FechaRegistro, v.RegistradoPor
                FROM PY_InHomeVisit v
                INNER JOIN PY_Trabajos t ON v.IdTrabajo = t.IdTrabajo
                LEFT JOIN TH_Empleado e ON v.ResponsableId = e.IdEmpleado
                WHERE v.IdTrabajo = @IdTrabajo
                ORDER BY v.FechaProgramada DESC";

            var visitas = await _connection.QueryAsync<InHomeVisitDto>(query, new { IdTrabajo = idTrabajo });
            _logger.LogInformation("Obtenidas {Count} InHome visits para trabajo {IdTrabajo}", visitas.Count(), idTrabajo);
            return visitas.ToList();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error obteniendo InHome visits trabajo {IdTrabajo}", idTrabajo);
            throw;
        }
    }

    public async Task<long> CrearInHomeVisitAsync(InHomeVisitDto visita)
    {
        try
        {
            var parameters = new DynamicParameters();
            parameters.Add("@IdTrabajo", visita.IdTrabajo);
            parameters.Add("@LugarVisita", visita.LugarVisita);
            parameters.Add("@FechaProgramada", visita.FechaProgramada);
            parameters.Add("@Estado", "Programada");
            parameters.Add("@CantidadParticipantes", visita.CantidadParticipantes);
            parameters.Add("@Recursos", visita.Recursos);
            parameters.Add("@Observaciones", visita.Observaciones);
            parameters.Add("@ResponsableId", visita.ResponsableId);
            parameters.Add("@FechaRegistro", DateTime.Now);
            parameters.Add("@RegistradoPor", visita.RegistradoPor);
            parameters.Add("@IdVisita", dbType: DbType.Int64, direction: ParameterDirection.Output);

            await _connection.ExecuteAsync(
                "PY_InHomeVisit_Save",
                parameters,
                commandType: CommandType.StoredProcedure);

            var idVisita = parameters.Get<long>("@IdVisita");
            _logger.LogInformation("InHome visit {Id} creada: {Lugar} - {Fecha}", 
                idVisita, visita.LugarVisita, visita.FechaProgramada);
            return idVisita;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error creando InHome visit");
            throw;
        }
    }

    public async Task<bool> ActualizarInHomeVisitAsync(InHomeVisitDto visita)
    {
        try
        {
            var query = @"
                UPDATE PY_InHomeVisit
                SET LugarVisita = @LugarVisita,
                    FechaProgramada = @FechaProgramada,
                    CantidadParticipantes = @CantidadParticipantes,
                    Recursos = @Recursos,
                    Observaciones = @Observaciones,
                    ResponsableId = @ResponsableId
                WHERE IdVisita = @IdVisita";

            var rowsAffected = await _connection.ExecuteAsync(query, visita);
            _logger.LogInformation("InHome visit {Id} actualizada", visita.IdVisita);
            return rowsAffected > 0;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error actualizando InHome visit {Id}", visita.IdVisita);
            throw;
        }
    }

    public async Task<bool> CambiarEstadoVisitaAsync(long idVisita, string nuevoEstado, long usuarioId)
    {
        try
        {
            var query = @"
                UPDATE PY_InHomeVisit
                SET Estado = @NuevoEstado,
                    FechaRealizada = CASE WHEN @NuevoEstado = 'Realizada' THEN @FechaRealizada ELSE FechaRealizada END
                WHERE IdVisita = @IdVisita";

            var rowsAffected = await _connection.ExecuteAsync(query, new
            {
                IdVisita = idVisita,
                NuevoEstado = nuevoEstado,
                FechaRealizada = DateTime.Now
            });

            _logger.LogInformation("Estado InHome visit {Id} cambiado a {Estado} por usuario {UserId}",
                idVisita, nuevoEstado, usuarioId);
            return rowsAffected > 0;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error cambiando estado InHome visit {Id}", idVisita);
            throw;
        }
    }
}
