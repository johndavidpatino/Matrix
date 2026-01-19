using Dapper;
using MatrixNext.Data.DTOs.PY;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Logging;
using System.Data;
using ClosedXML.Excel;

namespace MatrixNext.Data.Services.PY;

public class VariablesControlService : IVariablesControlService
{
    private readonly string _connectionString;
    private readonly ILogger<VariablesControlService> _logger;

    public VariablesControlService(
        string connectionString,
        ILogger<VariablesControlService> logger)
    {
        _connectionString = connectionString;
        _logger = logger;
    }

    #region CRUD

    public async Task<VariablesControlViewModel> PrepararViewModelAsync(long idTrabajo, string? tipoEvaluado = null)
    {
        try
        {
            using var connection = new SqlConnection(_connectionString);

            // Obtener información del trabajo
            var trabajo = await connection.QueryFirstOrDefaultAsync<dynamic>(@"
                SELECT 
                    t.id AS IdTrabajo,
                    t.JobBook,
                    t.NombreTrabajo,
                    t.Modalidad,
                    t.idCOE AS IdCOE,
                    CONCAT(coe.Nombres, ' ', coe.Apellidos) AS NombreCOE,
                    t.GerentePY AS IdGerente,
                    CONCAT(g.Nombres, ' ', g.Apellidos) AS NombreGerente
                FROM PY_Trabajo t
                LEFT JOIN TH_Personas coe ON t.idCOE = coe.id
                LEFT JOIN US_Usuarios g ON t.GerentePY = g.id
                WHERE t.id = @IdTrabajo",
                new { IdTrabajo = idTrabajo });

            if (trabajo == null)
            {
                _logger.LogWarning("Trabajo {IdTrabajo} no encontrado", idTrabajo);
                return new VariablesControlViewModel { IdTrabajo = idTrabajo };
            }

            // Obtener variables ya registradas
            var variables = await ObtenerVariablesControlPorTrabajoAsync(idTrabajo, tipoEvaluado);

            // Obtener empleados con evaluaciones previas
            var empleados = await ObtenerEmpleadosConEvaluacionAsync();

            return new VariablesControlViewModel
            {
                IdTrabajo = trabajo.IdTrabajo,
                JobBook = trabajo.JobBook,
                NombreTrabajo = trabajo.NombreTrabajo,
                Modalidad = trabajo.Modalidad,
                IdCOE = trabajo.IdCOE,
                NombreCOE = trabajo.NombreCOE,
                IdGerente = trabajo.IdGerente,
                NombreGerente = trabajo.NombreGerente,
                VariablesRegistradas = variables,
                EmpleadosDisponibles = empleados,
                TipoEvaluadoSeleccionado = tipoEvaluado
            };
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error preparando ViewModel para trabajo {IdTrabajo}", idTrabajo);
            throw;
        }
    }

    public async Task<(bool success, string message, long id)> CrearVariableControlAsync(VariableControlDto dto, long userId)
    {
        try
        {
            using var connection = new SqlConnection(_connectionString);

            // Validar si ya existe evaluación para este evaluado en este trabajo
            var existe = await connection.ExecuteScalarAsync<int>(@"
                SELECT COUNT(1) 
                FROM PY_Variables_Control 
                WHERE idTrabajo = @IdTrabajo 
                  AND idEvaluado = @IdEvaluado 
                  AND tipoEvaluado = @TipoEvaluado",
                new { dto.IdTrabajo, dto.IdEvaluado, dto.TipoEvaluado });

            if (existe > 0)
            {
                return (false, "Ya existe una evaluación para este empleado en este trabajo", 0);
            }

            // Insertar
            var sql = @"
                INSERT INTO PY_Variables_Control (
                    idTrabajo, idEvaluado, tipoEvaluado,
                    cumpleSeguridad, obsSeguridad,
                    cumpleObtencion, obsObtencion,
                    cumpleObjetivo, obsObjetivo,
                    cumpleAplicacion, obsAplicacion,
                    cumpleDistribucion, obsDistribucion,
                    cumpleCumplimiento, obsCumplimiento,
                    usuario, fechaCreacion
                )
                VALUES (
                    @IdTrabajo, @IdEvaluado, @TipoEvaluado,
                    @CumpleSeguridad, @ObsSeguridad,
                    @CumpleObtencion, @ObsObtencion,
                    @CumpleObjetivo, @ObsObjetivo,
                    @CumpleAplicacion, @ObsAplicacion,
                    @CumpleDistribucion, @ObsDistribucion,
                    @CumpleCumplimiento, @ObsCumplimiento,
                    @Usuario, GETDATE()
                );
                SELECT CAST(SCOPE_IDENTITY() AS BIGINT);";

            var id = await connection.ExecuteScalarAsync<long>(sql, new
            {
                dto.IdTrabajo,
                dto.IdEvaluado,
                dto.TipoEvaluado,
                dto.CumpleSeguridad,
                dto.ObsSeguridad,
                dto.CumpleObtencion,
                dto.ObsObtencion,
                dto.CumpleObjetivo,
                dto.ObsObjetivo,
                dto.CumpleAplicacion,
                dto.ObsAplicacion,
                dto.CumpleDistribucion,
                dto.ObsDistribucion,
                dto.CumpleCumplimiento,
                dto.ObsCumplimiento,
                Usuario = userId
            });

            _logger.LogInformation("Variable de control {Id} creada para trabajo {IdTrabajo} por usuario {UserId}",
                id, dto.IdTrabajo, userId);

            return (true, "Variable de control registrada exitosamente", id);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error creando variable de control para trabajo {IdTrabajo}", dto.IdTrabajo);
            return (false, "Error al registrar la variable de control", 0);
        }
    }

    public async Task<VariableControlDto?> ObtenerVariableControlAsync(long id)
    {
        try
        {
            using var connection = new SqlConnection(_connectionString);

            var sql = @"
                SELECT 
                    vc.id AS Id,
                    vc.idTrabajo AS IdTrabajo,
                    vc.idEvaluado AS IdEvaluado,
                    vc.tipoEvaluado AS TipoEvaluado,
                    vc.cumpleSeguridad AS CumpleSeguridad,
                    vc.obsSeguridad AS ObsSeguridad,
                    vc.cumpleObtencion AS CumpleObtencion,
                    vc.obsObtencion AS ObsObtencion,
                    vc.cumpleObjetivo AS CumpleObjetivo,
                    vc.obsObjetivo AS ObsObjetivo,
                    vc.cumpleAplicacion AS CumpleAplicacion,
                    vc.obsAplicacion AS ObsAplicacion,
                    vc.cumpleDistribucion AS CumpleDistribucion,
                    vc.obsDistribucion AS ObsDistribucion,
                    vc.cumpleCumplimiento AS CumpleCumplimiento,
                    vc.obsCumplimiento AS ObsCumplimiento,
                    vc.usuario AS Usuario,
                    vc.fechaCreacion AS FechaCreacion,
                    CONCAT(p.Nombres, ' ', p.Apellidos) AS NombreEvaluado,
                    CONCAT(u.Nombres, ' ', u.Apellidos) AS NombreUsuario
                FROM PY_Variables_Control vc
                LEFT JOIN TH_Personas p ON vc.idEvaluado = p.id
                LEFT JOIN US_Usuarios u ON vc.usuario = u.id
                WHERE vc.id = @Id";

            return await connection.QueryFirstOrDefaultAsync<VariableControlDto>(sql, new { Id = id });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error obteniendo variable de control {Id}", id);
            throw;
        }
    }

    public async Task<List<VariableControlDto>> ObtenerVariablesControlPorTrabajoAsync(long idTrabajo, string? tipoEvaluado = null)
    {
        try
        {
            using var connection = new SqlConnection(_connectionString);

            var sql = @"
                SELECT 
                    vc.id AS Id,
                    vc.idTrabajo AS IdTrabajo,
                    vc.idEvaluado AS IdEvaluado,
                    vc.tipoEvaluado AS TipoEvaluado,
                    vc.cumpleSeguridad AS CumpleSeguridad,
                    vc.obsSeguridad AS ObsSeguridad,
                    vc.cumpleObtencion AS CumpleObtencion,
                    vc.obsObtencion AS ObsObtencion,
                    vc.cumpleObjetivo AS CumpleObjetivo,
                    vc.obsObjetivo AS ObsObjetivo,
                    vc.cumpleAplicacion AS CumpleAplicacion,
                    vc.obsAplicacion AS ObsAplicacion,
                    vc.cumpleDistribucion AS CumpleDistribucion,
                    vc.obsDistribucion AS ObsDistribucion,
                    vc.cumpleCumplimiento AS CumpleCumplimiento,
                    vc.obsCumplimiento AS ObsCumplimiento,
                    vc.usuario AS Usuario,
                    vc.fechaCreacion AS FechaCreacion,
                    CONCAT(p.Nombres, ' ', p.Apellidos) AS NombreEvaluado,
                    CONCAT(u.Nombres, ' ', u.Apellidos) AS NombreUsuario
                FROM PY_Variables_Control vc
                LEFT JOIN TH_Personas p ON vc.idEvaluado = p.id
                LEFT JOIN US_Usuarios u ON vc.usuario = u.id
                WHERE vc.idTrabajo = @IdTrabajo
                  AND (@TipoEvaluado IS NULL OR vc.tipoEvaluado = @TipoEvaluado)
                ORDER BY vc.fechaCreacion DESC";

            var result = await connection.QueryAsync<VariableControlDto>(sql, new { IdTrabajo = idTrabajo, TipoEvaluado = tipoEvaluado });
            return result.ToList();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error obteniendo variables de control para trabajo {IdTrabajo}", idTrabajo);
            throw;
        }
    }

    #endregion

    #region Reportes

    public async Task<List<ReporteVariableControlDto>> ObtenerReporteVariablesControlAsync(VariablesControlFiltrosDto filtros)
    {
        try
        {
            using var connection = new SqlConnection(_connectionString);

            var result = await connection.QueryAsync<dynamic>(
                "REP_PY_Variables_Control",
                new
                {
                    ano = filtros.Ano,
                    mes = filtros.Mes,
                    idEvaluado = filtros.IdEvaluado
                },
                commandType: CommandType.StoredProcedure);

            // Mapear resultado dinámico a DTO
            return result.Select(r => new ReporteVariableControlDto
            {
                NombreEvaluado = r.GerenteCOEEvaluado,
                TipoEvaluado = r.tipoEvaluado,
                Modalidad = r.ServiceLineTrabajo,
                FechaCreacion = r.FechaEvaluacion,
                NombreTrabajo = r.NombreTrabajo,
                JobBook = r.JobBook,
                TotalCumple = Convert.ToInt32(r.Si ?? 0),
                TotalNoCumple = Convert.ToInt32(r.No ?? 0),
                PorcentajeCumplimiento = r.Si,
                ObsSeguridad = r.obsSeguridad,
                ObsObtencion = r.obsObtencion,
                ObsObjetivo = r.obsObjetivo,
                ObsAplicacion = r.obsAplicacion,
                ObsDistribucion = r.obsDistribucion,
                ObsCumplimiento = r.obsCumplimiento
            }).ToList();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error obteniendo reporte de variables de control");
            throw;
        }
    }

    public async Task<List<ReporteVariableControlPorMesDto>> ObtenerReporteVariablesControlPorMesAsync(VariablesControlFiltrosDto filtros)
    {
        try
        {
            using var connection = new SqlConnection(_connectionString);

            var result = await connection.QueryAsync<dynamic>(
                "REP_PY_Variables_Control_PorMes",
                new
                {
                    ano = filtros.Ano,
                    mes = filtros.Mes,
                    idEvaluado = filtros.IdEvaluado
                },
                commandType: CommandType.StoredProcedure);

            return result.Select(r => new ReporteVariableControlPorMesDto
            {
                NombreEvaluado = r.GerenteCOEEvaluado,
                Ano = Convert.ToInt16(r.ano),
                NombreMes = r.mes,
                PorcentajeCumplimiento = r.Si,
                PorcentajeGeneral = r.No
            }).ToList();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error obteniendo reporte por mes de variables de control");
            throw;
        }
    }

    public async Task<Dictionary<long, string>> ObtenerEmpleadosConEvaluacionAsync()
    {
        try
        {
            using var connection = new SqlConnection(_connectionString);

            var result = await connection.QueryAsync<(long id, string nombre)>(
                "REP_PY_VariablesControlEmpleadosConEvaluacion",
                commandType: CommandType.StoredProcedure);

            return result.ToDictionary(x => x.id, x => x.nombre);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error obteniendo empleados con evaluación");
            throw;
        }
    }

    #endregion

    #region Export Excel

    public async Task<byte[]> ExportarReporteExcelAsync(VariablesControlFiltrosDto filtros, string tipoReporte)
    {
        try
        {
            using var workbook = new XLWorkbook();

            if (tipoReporte == "detallado")
            {
                var data = await ObtenerReporteVariablesControlAsync(filtros);
                var worksheet = workbook.Worksheets.Add("Variables Control");

                // Headers
                worksheet.Cell(1, 1).Value = "Evaluado";
                worksheet.Cell(1, 2).Value = "Tipo";
                worksheet.Cell(1, 3).Value = "Service Line";
                worksheet.Cell(1, 4).Value = "Fecha";
                worksheet.Cell(1, 5).Value = "Trabajo";
                worksheet.Cell(1, 6).Value = "JobBook";
                worksheet.Cell(1, 7).Value = "% Cumple";
                worksheet.Cell(1, 8).Value = "% No Cumple";
                worksheet.Cell(1, 9).Value = "Obs. Seguridad";
                worksheet.Cell(1, 10).Value = "Obs. Obtención";
                worksheet.Cell(1, 11).Value = "Obs. Objetivo";
                worksheet.Cell(1, 12).Value = "Obs. Aplicación";
                worksheet.Cell(1, 13).Value = "Obs. Distribución";
                worksheet.Cell(1, 14).Value = "Obs. Cumplimiento";

                // Data
                int row = 2;
                foreach (var item in data)
                {
                    worksheet.Cell(row, 1).Value = item.NombreEvaluado;
                    worksheet.Cell(row, 2).Value = item.TipoEvaluado;
                    worksheet.Cell(row, 3).Value = item.Modalidad;
                    worksheet.Cell(row, 4).Value = item.FechaCreacion;
                    worksheet.Cell(row, 5).Value = item.NombreTrabajo;
                    worksheet.Cell(row, 6).Value = item.JobBook;
                    worksheet.Cell(row, 7).Value = item.TotalCumple;
                    worksheet.Cell(row, 8).Value = item.TotalNoCumple;
                    worksheet.Cell(row, 9).Value = item.ObsSeguridad;
                    worksheet.Cell(row, 10).Value = item.ObsObtencion;
                    worksheet.Cell(row, 11).Value = item.ObsObjetivo;
                    worksheet.Cell(row, 12).Value = item.ObsAplicacion;
                    worksheet.Cell(row, 13).Value = item.ObsDistribucion;
                    worksheet.Cell(row, 14).Value = item.ObsCumplimiento;
                    row++;
                }

                worksheet.Columns().AdjustToContents();
            }
            else // por mes
            {
                var data = await ObtenerReporteVariablesControlPorMesAsync(filtros);
                var worksheet = workbook.Worksheets.Add("Variables Control Por Mes");

                // Headers
                worksheet.Cell(1, 1).Value = "Evaluado";
                worksheet.Cell(1, 2).Value = "Año";
                worksheet.Cell(1, 3).Value = "Mes";
                worksheet.Cell(1, 4).Value = "% Cumple";
                worksheet.Cell(1, 5).Value = "% No Cumple";

                // Data
                int row = 2;
                foreach (var item in data)
                {
                    worksheet.Cell(row, 1).Value = item.NombreEvaluado;
                    worksheet.Cell(row, 2).Value = item.Ano;
                    worksheet.Cell(row, 3).Value = item.NombreMes;
                    worksheet.Cell(row, 4).Value = item.PorcentajeCumplimiento;
                    worksheet.Cell(row, 5).Value = item.PorcentajeGeneral;
                    row++;
                }

                worksheet.Columns().AdjustToContents();
            }

            using var stream = new MemoryStream();
            workbook.SaveAs(stream);
            return stream.ToArray();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error exportando reporte a Excel");
            throw;
        }
    }

    #endregion
}
