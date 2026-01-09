using Dapper;
using MatrixNext.Data.Modules.CC.DTOs.Produccion;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MatrixNext.Data.Modules.CC.Adapters
{
    /// <summary>
    /// Adapter para consultas de Producción usando Dapper
    /// </summary>
    public class CcProduccionAdapter
    {
        private readonly IDbConnection _connection;

        public CcProduccionAdapter(IDbConnection connection)
        {
            _connection = connection ?? throw new ArgumentNullException(nameof(connection));
        }

        /// <summary>
        /// Obtiene registros de producción con filtros opcionales
        /// </summary>
        public async Task<List<RegistroProduccionDto>> ObtenerRegistrosProduccionAsync(
            int? periodo, long? idTrabajo, long? idEmpleado, 
            long? idActividad, DateTime? fechaInicio, DateTime? fechaFin, byte? estado)
        {
            var parameters = new DynamicParameters();
            var sql = new StringBuilder(@"
                SELECT
                    P.Id AS IdProduccion,
                    (YEAR(P.Fecha) * 100 + MONTH(P.Fecha)) AS Periodo,
                    P.TrabajoId AS IdTrabajo,
                    T.JobBook AS CodigoTrabajo,
                    T.NombreTrabajo AS NombreTrabajo,
                    A.Id AS IdActividad,
                    CAST(A.Id AS varchar(20)) AS CodigoActividad,
                    A.ActNombre AS DescripcionActividad,
                    P.Cantida AS Cantidad,
                    P.VrUnitario AS CostoUnitario,
                    P.PersonaId AS IdEmpleado,
                    LTRIM(RTRIM(ISNULL(TH.Nombres, '') + ' ' + ISNULL(TH.Apellidos, ''))) AS NombreEmpleado,
                    P.Fecha AS FechaProduccion,
                    CAST(1 AS tinyint) AS Estado,
                    CAST(NULL AS varchar(250)) AS Observaciones,
                    P.Fecha AS FechaRegistro
                FROM CC_Produccion P
                LEFT JOIN PY_Trabajo T ON T.Id = P.TrabajoId
                LEFT JOIN IQ_Actividades A ON A.Id = P.PresupuestoId
                LEFT JOIN TH_Personas TH ON TH.Id = P.PersonaId
                WHERE 1 = 1");

            if (periodo.HasValue)
            {
                sql.Append(" AND (YEAR(P.Fecha) * 100 + MONTH(P.Fecha)) = @Periodo");
                parameters.Add("@Periodo", periodo.Value);
            }
            if (idTrabajo.HasValue)
            {
                sql.Append(" AND P.TrabajoId = @IdTrabajo");
                parameters.Add("@IdTrabajo", idTrabajo.Value);
            }
            if (idEmpleado.HasValue)
            {
                sql.Append(" AND P.PersonaId = @IdEmpleado");
                parameters.Add("@IdEmpleado", idEmpleado.Value);
            }
            if (idActividad.HasValue)
            {
                sql.Append(" AND A.Id = @IdActividad");
                parameters.Add("@IdActividad", idActividad.Value);
            }
            if (fechaInicio.HasValue)
            {
                sql.Append(" AND P.Fecha >= @FechaInicio");
                parameters.Add("@FechaInicio", fechaInicio.Value.Date);
            }
            if (fechaFin.HasValue)
            {
                sql.Append(" AND P.Fecha <= @FechaFin");
                parameters.Add("@FechaFin", fechaFin.Value.Date);
            }

            var result = await _connection.QueryAsync<RegistroProduccionDto>(
                sql.ToString(),
                parameters,
                commandType: CommandType.Text
            );

            return result.ToList();
        }

        /// <summary>
        /// Obtiene liquidaciones de planillas con filtros opcionales
        /// </summary>
        public async Task<List<LiquidacionPlanillaDto>> ObtenerLiquidacionesAsync(
            int? periodo, long? idTrabajo, long? idEmpleado, byte? estado)
        {
            var parameters = new DynamicParameters();
            var sql = new StringBuilder(@"
                SELECT
                    MIN(P.Id) AS IdLiquidacion,
                    (YEAR(P.Fecha) * 100 + MONTH(P.Fecha)) AS Periodo,
                    P.TrabajoId AS IdTrabajo,
                    T.JobBook AS CodigoTrabajo,
                    T.NombreTrabajo AS NombreTrabajo,
                    P.PersonaId AS IdEmpleado,
                    LTRIM(RTRIM(ISNULL(TH.Nombres, '') + ' ' + ISNULL(TH.Apellidos, ''))) AS NombreEmpleado,
                    CAST(0 AS decimal(18, 2)) AS SalarioBase,
                    SUM(P.Total) AS ProduccionGenerada,
                    SUM(ISNULL(P.VrProvisionBono, 0)) AS BonoProduccion,
                    CAST(0 AS decimal(18, 2)) AS DescuentosSS,
                    SUM(P.Total) + SUM(ISNULL(P.VrProvisionBono, 0)) AS ValorNeto,
                    CAST(1 AS tinyint) AS Estado,
                    MAX(ISNULL(P.FechaLiquidacion, P.Fecha)) AS FechaLiquidacion,
                    CAST(NULL AS varchar(250)) AS Observaciones
                FROM CC_Produccion P
                LEFT JOIN PY_Trabajo T ON T.Id = P.TrabajoId
                LEFT JOIN TH_Personas TH ON TH.Id = P.PersonaId
                WHERE 1 = 1");

            if (periodo.HasValue)
            {
                sql.Append(" AND (YEAR(P.Fecha) * 100 + MONTH(P.Fecha)) = @Periodo");
                parameters.Add("@Periodo", periodo.Value);
            }
            if (idTrabajo.HasValue)
            {
                sql.Append(" AND P.TrabajoId = @IdTrabajo");
                parameters.Add("@IdTrabajo", idTrabajo.Value);
            }
            if (idEmpleado.HasValue)
            {
                sql.Append(" AND P.PersonaId = @IdEmpleado");
                parameters.Add("@IdEmpleado", idEmpleado.Value);
            }

            sql.Append(@"
                GROUP BY
                    (YEAR(P.Fecha) * 100 + MONTH(P.Fecha)),
                    P.TrabajoId,
                    T.JobBook,
                    T.NombreTrabajo,
                    P.PersonaId,
                    TH.Nombres,
                    TH.Apellidos");

            var result = await _connection.QueryAsync<LiquidacionPlanillaDto>(
                sql.ToString(),
                parameters,
                commandType: CommandType.Text
            );

            return result.ToList();
        }

        /// <summary>
        /// Obtiene bonificaciones generadas con filtros opcionales
        /// </summary>
        public async Task<List<GenerarBonificacionDto>> ObtenerBonificacionesAsync(
            int? periodo, long? idTrabajo, long? idEmpleado, byte? estado)
        {
            var parameters = new DynamicParameters();
            var sql = new StringBuilder(@"
                SELECT
                    MIN(P.Id) AS IdBonificacion,
                    (YEAR(P.Fecha) * 100 + MONTH(P.Fecha)) AS Periodo,
                    P.PersonaId AS IdEmpleado,
                    LTRIM(RTRIM(ISNULL(TH.Nombres, '') + ' ' + ISNULL(TH.Apellidos, ''))) AS NombreEmpleado,
                    P.TrabajoId AS IdTrabajo,
                    T.NombreTrabajo AS NombreTrabajo,
                    CAST(0 AS decimal(18, 2)) AS SalarioBase,
                    SUM(P.Total) AS ProduccionTotal,
                    CAST(0 AS decimal(18, 2)) AS PercentajeMetaBonificacion,
                    SUM(ISNULL(P.VrProvisionBono, 0)) AS BonoCalculado,
                    SUM(ISNULL(P.VrProvisionBono, 0)) AS BonoFinal,
                    CAST(1 AS tinyint) AS Estado,
                    MAX(ISNULL(P.FechaLiquidacion, P.Fecha)) AS FechaGeneracion,
                    CAST(NULL AS varchar(250)) AS Observaciones
                FROM CC_Produccion P
                LEFT JOIN PY_Trabajo T ON T.Id = P.TrabajoId
                LEFT JOIN TH_Personas TH ON TH.Id = P.PersonaId
                WHERE 1 = 1");

            if (periodo.HasValue)
            {
                sql.Append(" AND (YEAR(P.Fecha) * 100 + MONTH(P.Fecha)) = @Periodo");
                parameters.Add("@Periodo", periodo.Value);
            }
            if (idTrabajo.HasValue)
            {
                sql.Append(" AND P.TrabajoId = @IdTrabajo");
                parameters.Add("@IdTrabajo", idTrabajo.Value);
            }
            if (idEmpleado.HasValue)
            {
                sql.Append(" AND P.PersonaId = @IdEmpleado");
                parameters.Add("@IdEmpleado", idEmpleado.Value);
            }

            sql.Append(@"
                GROUP BY
                    (YEAR(P.Fecha) * 100 + MONTH(P.Fecha)),
                    P.PersonaId,
                    TH.Nombres,
                    TH.Apellidos,
                    P.TrabajoId,
                    T.NombreTrabajo");

            var result = await _connection.QueryAsync<GenerarBonificacionDto>(
                sql.ToString(),
                parameters,
                commandType: CommandType.Text
            );

            return result.ToList();
        }

        /// <summary>
        /// Obtiene descuentos de seguridad social con filtros opcionales
        /// </summary>
        public async Task<List<CargueDescuentoSSDto>> ObtenerDescuentosSsAsync(
            int? periodo, long? idEmpleado, string? tipoDescuento, byte? estado)
        {
            var parameters = new DynamicParameters();
            var sql = new StringBuilder(@"
                SELECT
                    D.Id AS IdDescuento,
                    (YEAR(D.Fecha) * 100 + MONTH(D.Fecha)) AS Periodo,
                    D.Cedula AS IdEmpleado,
                    LTRIM(RTRIM(ISNULL(TH.Nombres, '') + ' ' + ISNULL(TH.Apellidos, ''))) AS NombreEmpleado,
                    CASE
                        WHEN D.ValorSS IS NOT NULL THEN 'SS'
                        WHEN D.ValorICA IS NOT NULL THEN 'ICA'
                        ELSE 'Descuento'
                    END AS TipoDescuento,
                    COALESCE(D.Descuento, D.ValorSS, D.ValorICA, 0) AS ValorDescuento,
                    CAST(0 AS decimal(18, 6)) AS PercentajeDescuento,
                    CAST(1 AS tinyint) AS Estado,
                    D.Fecha AS FechaCarga,
                    CAST(NULL AS varchar(250)) AS Observaciones
                FROM CC_ProduccionDescuentosSS D
                LEFT JOIN TH_Personas TH ON TH.Id = D.Cedula
                WHERE 1 = 1");

            if (periodo.HasValue)
            {
                sql.Append(" AND (YEAR(D.Fecha) * 100 + MONTH(D.Fecha)) = @Periodo");
                parameters.Add("@Periodo", periodo.Value);
            }
            if (idEmpleado.HasValue)
            {
                sql.Append(" AND D.Cedula = @IdEmpleado");
                parameters.Add("@IdEmpleado", idEmpleado.Value);
            }
            if (!string.IsNullOrWhiteSpace(tipoDescuento))
            {
                sql.Append(@"
                    AND (CASE
                        WHEN D.ValorSS IS NOT NULL THEN 'SS'
                        WHEN D.ValorICA IS NOT NULL THEN 'ICA'
                        ELSE 'Descuento'
                    END) = @TipoDescuento");
                parameters.Add("@TipoDescuento", tipoDescuento);
            }

            var result = await _connection.QueryAsync<CargueDescuentoSSDto>(
                sql.ToString(),
                parameters,
                commandType: CommandType.Text
            );

            return result.ToList();
        }

        /// <summary>
        /// Obtiene liquidaciones de productividad PST con filtros opcionales
        /// </summary>
        public async Task<List<LiquidacionProductividadPstDto>> ObtenerLiquidacionesPstAsync(
            int? periodo, long? idTrabajo, long? idEmpleado, byte? estado)
        {
            var parameters = new DynamicParameters();
            var sql = new StringBuilder(@"
                SELECT
                    P.Id AS IdLiquidacionPST,
                    (YEAR(COALESCE(P.FechaEjecucion, P.FechaCarga)) * 100 + MONTH(COALESCE(P.FechaEjecucion, P.FechaCarga))) AS Periodo,
                    P.TrabajoId AS IdTrabajo,
                    T.JobBook AS CodigoTrabajo,
                    T.NombreTrabajo AS NombreTrabajo,
                    P.Cedula AS IdEmpleado,
                    LTRIM(RTRIM(ISNULL(TH.Nombres, '') + ' ' + ISNULL(TH.Apellidos, ''))) AS NombreEmpleado,
                    CAST(0 AS decimal(18, 2)) AS ValorPST,
                    ISNULL(P.Cantidad, 0) AS ProduccionGenerada,
                    CAST(0 AS decimal(18, 2)) AS PercentajeLiquidacion,
                    CAST(0 AS decimal(18, 2)) AS ValorLiquidado,
                    CAST(CASE WHEN P.EnProduccion = 1 THEN 1 ELSE 0 END AS tinyint) AS Estado,
                    COALESCE(P.FechaEjecucion, P.FechaCarga) AS FechaLiquidacion,
                    COALESCE(P.ObservacionesPMO, P.ObservacionesJefe, P.ObservacionesCoordinador) AS Observaciones
                FROM CC_ProduccionCargaPST P
                LEFT JOIN PY_Trabajo T ON T.Id = P.TrabajoId
                LEFT JOIN TH_Personas TH ON TH.Id = P.Cedula
                WHERE 1 = 1");

            if (periodo.HasValue)
            {
                sql.Append(" AND (YEAR(COALESCE(P.FechaEjecucion, P.FechaCarga)) * 100 + MONTH(COALESCE(P.FechaEjecucion, P.FechaCarga))) = @Periodo");
                parameters.Add("@Periodo", periodo.Value);
            }
            if (idTrabajo.HasValue)
            {
                sql.Append(" AND P.TrabajoId = @IdTrabajo");
                parameters.Add("@IdTrabajo", idTrabajo.Value);
            }
            if (idEmpleado.HasValue)
            {
                sql.Append(" AND P.Cedula = @IdEmpleado");
                parameters.Add("@IdEmpleado", idEmpleado.Value);
            }
            if (estado.HasValue)
            {
                sql.Append(" AND (CASE WHEN P.EnProduccion = 1 THEN 1 ELSE 0 END) = @Estado");
                parameters.Add("@Estado", estado.Value);
            }

            var result = await _connection.QueryAsync<LiquidacionProductividadPstDto>(
                sql.ToString(),
                parameters,
                commandType: CommandType.Text
            );

            return result.ToList();
        }

        /// <summary>
        /// Obtiene asignaciones de costos a PST con filtros opcionales
        /// </summary>
        public async Task<List<AsignacionCostosPstDto>> ObtenerAsignacionesCostosAsync(
            int? periodo, long? idTrabajo, long? idConcepto, byte? estado)
        {
            var parameters = new DynamicParameters();
            var sql = new StringBuilder(@"
                SELECT
                    DP.Id AS IdAsignacion,
                    (YEAR(CPI.FechaCreacion) * 100 + MONTH(CPI.FechaCreacion)) AS Periodo,
                    CPI.TrabajoId AS IdTrabajo,
                    T.JobBook AS CodigoTrabajo,
                    T.NombreTrabajo AS NombreTrabajo,
                    DP.CargoCC AS IdConcepto,
                    CA.Descripcion AS NombreConcepto,
                    ISNULL(DP.ValorUnitario, 0) AS CostoBase,
                    ISNULL(DP.TotalCosto, 0) AS CostoAsignado,
                    CAST(1 AS tinyint) AS Estado,
                    CPI.FechaCreacion AS FechaAsignacion,
                    CAST(NULL AS varchar(250)) AS Observaciones
                FROM CC_PresupuestoInternoDetalle DP
                INNER JOIN CC_PresupuestoInterno CPI ON CPI.Id = DP.Id_PresupuestoInterno
                LEFT JOIN PY_Trabajo T ON T.Id = CPI.TrabajoId
                LEFT JOIN CC_CargosActividades CA ON CA.Id = DP.CargoCC
                WHERE 1 = 1");

            if (periodo.HasValue)
            {
                sql.Append(" AND (YEAR(CPI.FechaCreacion) * 100 + MONTH(CPI.FechaCreacion)) = @Periodo");
                parameters.Add("@Periodo", periodo.Value);
            }
            if (idTrabajo.HasValue)
            {
                sql.Append(" AND CPI.TrabajoId = @IdTrabajo");
                parameters.Add("@IdTrabajo", idTrabajo.Value);
            }
            if (idConcepto.HasValue)
            {
                sql.Append(" AND DP.CargoCC = @IdConcepto");
                parameters.Add("@IdConcepto", idConcepto.Value);
            }

            var result = await _connection.QueryAsync<AsignacionCostosPstDto>(
                sql.ToString(),
                parameters,
                commandType: CommandType.Text
            );

            return result.ToList();
        }

        /// <summary>
        /// Obtiene estado de jobbooks con filtros opcionales
        /// </summary>
        public async Task<List<EstadoJobBookDto>> ObtenerEstadoJobBooksAsync(
            long? idTrabajo, byte? estadoActual)
        {
            var parameters = new DynamicParameters();
            var sql = new StringBuilder(@"
                SELECT
                    ISNULL(T.Id, 0) AS IdJobBook,
                    ISNULL(T.Id, 0) AS IdTrabajo,
                    T.JobBook AS CodigoTrabajo,
                    T.NombreTrabajo AS NombreTrabajo,
                    J.JobBook AS NumeroJobBook,
                    CAST(CASE
                        WHEN UPPER(ISNULL(J.Estado, '')) IN ('CERRADO', 'CERRADA', 'CLOSED', '2') THEN 2
                        ELSE 1
                    END AS tinyint) AS EstadoActual,
                    J.Estado AS EstadoActualNombre,
                    CAST(NULL AS datetime) AS FechaApertura,
                    CAST(NULL AS datetime) AS FechaCierre,
                    CAST(0 AS decimal(18, 2)) AS MontoTotal,
                    CAST(NULL AS varchar(250)) AS Observaciones
                FROM CC_ProduccionJobsAbiertos J
                LEFT JOIN PY_Trabajo T ON T.JobBook = J.JobBook
                WHERE 1 = 1");

            if (idTrabajo.HasValue)
            {
                sql.Append(" AND T.Id = @IdTrabajo");
                parameters.Add("@IdTrabajo", idTrabajo.Value);
            }
            if (estadoActual.HasValue)
            {
                sql.Append(@" AND (CASE
                        WHEN UPPER(ISNULL(J.Estado, '')) IN ('CERRADO', 'CERRADA', 'CLOSED', '2') THEN 2
                        ELSE 1
                    END) = @EstadoActual");
                parameters.Add("@EstadoActual", estadoActual.Value);
            }

            var result = await _connection.QueryAsync<EstadoJobBookDto>(
                sql.ToString(),
                parameters,
                commandType: CommandType.Text
            );

            return result.ToList();
        }

        /// <summary>
        /// Obtiene bonificaciones para revisión con filtros opcionales
        /// </summary>
        public async Task<List<RevisarGeneracionBonificacionDto>> ObtenerRevisarBonificacionesAsync(
            int? periodo, long? idEmpleado, long? idTrabajo, bool? aprobada)
        {
            var parameters = new DynamicParameters();
            var sql = new StringBuilder(@"
                SELECT
                    MIN(P.Id) AS IdBonificacion,
                    (YEAR(P.Fecha) * 100 + MONTH(P.Fecha)) AS Periodo,
                    P.PersonaId AS IdEmpleado,
                    LTRIM(RTRIM(ISNULL(TH.Nombres, '') + ' ' + ISNULL(TH.Apellidos, ''))) AS NombreEmpleado,
                    CAST(0 AS decimal(18, 2)) AS SalarioBase,
                    SUM(P.Total) AS ProduccionTotal,
                    SUM(ISNULL(P.VrProvisionBono, 0)) AS BonoCalculado,
                    SUM(ISNULL(P.VrProvisionBono, 0)) AS BonoFinal,
                    CAST(1 AS tinyint) AS Estado,
                    MAX(ISNULL(P.FechaLiquidacion, P.Fecha)) AS FechaGeneracion,
                    CAST(NULL AS varchar(100)) AS UsuarioGeneracion,
                    CAST(NULL AS varchar(100)) AS UsuarioRevision,
                    CAST(NULL AS datetime) AS FechaRevision,
                    CAST(0 AS bit) AS Aprobada
                FROM CC_Produccion P
                LEFT JOIN TH_Personas TH ON TH.Id = P.PersonaId
                WHERE 1 = 1");

            if (periodo.HasValue)
            {
                sql.Append(" AND (YEAR(P.Fecha) * 100 + MONTH(P.Fecha)) = @Periodo");
                parameters.Add("@Periodo", periodo.Value);
            }
            if (idEmpleado.HasValue)
            {
                sql.Append(" AND P.PersonaId = @IdEmpleado");
                parameters.Add("@IdEmpleado", idEmpleado.Value);
            }
            if (idTrabajo.HasValue)
            {
                sql.Append(" AND P.TrabajoId = @IdTrabajo");
                parameters.Add("@IdTrabajo", idTrabajo.Value);
            }

            sql.Append(@"
                GROUP BY
                    (YEAR(P.Fecha) * 100 + MONTH(P.Fecha)),
                    P.PersonaId,
                    TH.Nombres,
                    TH.Apellidos");

            var result = await _connection.QueryAsync<RevisarGeneracionBonificacionDto>(
                sql.ToString(),
                parameters,
                commandType: CommandType.Text
            );

            return result.ToList();
        }

        /// <summary>
        /// Obtiene anulaciones de liquidaciones con filtros opcionales
        /// </summary>
        public async Task<List<AnulacionLiquidacionesDto>> ObtenerAnulacionesAsync(
            int? periodo, long? idEmpleado, long? idTrabajo, 
            DateTime? fechaInicio, DateTime? fechaFin)
        {
            var parameters = new DynamicParameters();
            var sql = new StringBuilder(@"
                SELECT
                    P.IdPlanilla AS IdLiquidacion,
                    (YEAR(P.FechaLiquidacion) * 100 + MONTH(P.FechaLiquidacion)) AS Periodo,
                    CAST(NULL AS bigint) AS IdEmpleado,
                    CAST(NULL AS varchar(200)) AS NombreEmpleado,
                    CAST(NULL AS bigint) AS IdTrabajo,
                    CAST(NULL AS varchar(200)) AS NombreTrabajo,
                    CAST(0 AS decimal(18, 2)) AS ValorLiquidado,
                    CAST(3 AS tinyint) AS EstadoActual,
                    CAST('Anulada' AS varchar(100)) AS EstadoActualNombre,
                    P.FechaLiquidacion AS FechaLiquidacion,
                    CAST(NULL AS varchar(250)) AS Motivoanulacion,
                    P.FechaLiquidacion AS FechaAnulacion,
                    CAST(NULL AS varchar(100)) AS UsuarioAnulacion
                FROM CC_PlanillasAProduccion P
                WHERE 1 = 1");

            if (periodo.HasValue)
            {
                sql.Append(" AND (YEAR(P.FechaLiquidacion) * 100 + MONTH(P.FechaLiquidacion)) = @Periodo");
                parameters.Add("@Periodo", periodo.Value);
            }
            if (fechaInicio.HasValue)
            {
                sql.Append(" AND P.FechaLiquidacion >= @FechaInicio");
                parameters.Add("@FechaInicio", fechaInicio.Value.Date);
            }
            if (fechaFin.HasValue)
            {
                sql.Append(" AND P.FechaLiquidacion <= @FechaFin");
                parameters.Add("@FechaFin", fechaFin.Value.Date);
            }

            var result = await _connection.QueryAsync<AnulacionLiquidacionesDto>(
                sql.ToString(),
                parameters,
                commandType: CommandType.Text
            );

            return result.ToList();
        }
    }
}
