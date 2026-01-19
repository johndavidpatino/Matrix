using Dapper;
using MatrixNext.Data.Modules.CC.DTOs.ProcesosInternos;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace MatrixNext.Data.Modules.CC.Adapters
{
    /// <summary>
    /// Adapter para Reportes de Procesos Internos
    /// SP disponibles: CC_ConteosXIdGet, CC_ActividadesXTrabajo, CC_ProduccionXFechas,
    /// CC_ProduccionResumenPersonas, CC_ProduccionResumenXCedula, CC_TrabajosConteo
    /// </summary>
    public class CcProcesosInternosAdapter
    {
        private readonly IDbConnection _dbConnection;

        public CcProcesosInternosAdapter(IDbConnection dbConnection)
        {
            _dbConnection = dbConnection;
        }

        #region Reporte de Conteos

        /// <summary>
        /// Obtiene reporte de conteos de trabajos con filtros
        /// NOTA: SP CC_ReporteConteoTrabajos no existe - usar CC_TrabajosConteo
        /// </summary>
        public async Task<IEnumerable<ReporteConteoDto>> ObtenerReporteConteosAsync(
            FiltrosReporteConteoDto filtros)
        {
            // TODO: SP CC_ReporteConteoTrabajos no existe, usar CC_TrabajosConteo si tiene parámetros similares
            throw new NotImplementedException(
                "SP CC_ReporteConteoTrabajos no existe. Usar CC_TrabajosConteo como alternativa.");
        }

        /// <summary>
        /// Obtiene totales agregados de conteos
        /// NOTA: SP CC_TotalesConteoTrabajos no existe
        /// </summary>
        public async Task<dynamic?> ObtenerTotalesConteosAsync(
            DateTime? fechaInicio = null, DateTime? fechaFin = null)
        {
            // TODO: SP CC_TotalesConteoTrabajos no existe
            throw new NotImplementedException(
                "SP CC_TotalesConteoTrabajos no existe en BD legacy.");
        }

        #endregion

        #region Resumen de Productividad

        /// <summary>
        /// Obtiene resumen de productividad con filtros
        /// Usar SP: CC_ProduccionXFechas como alternativa
        /// </summary>
        public async Task<IEnumerable<ResumenProductividadDto>> ObtenerResumenProductividadAsync(
            FiltrosResumenProductividadDto filtros)
        {
            // Usar CC_ProduccionXFechas que sí existe
            const string sql = "CC_ProduccionXFechas";
            var parameters = new DynamicParameters();
            
            if (filtros.FechaInicio.HasValue)
                parameters.Add("@FechaInicio", filtros.FechaInicio.Value);
            if (filtros.FechaFin.HasValue)
                parameters.Add("@FechaFin", filtros.FechaFin.Value);

            IEnumerable<ResumenProductividadDto>? result = await _dbConnection.QueryAsync<ResumenProductividadDto>(
                sql, parameters, commandType: CommandType.StoredProcedure);
            
            return (result ?? Enumerable.Empty<ResumenProductividadDto>())!;
        }

        /// <summary>
        /// Obtiene datos agregados de productividad global
        /// NOTA: SP CC_ProductividadAgregada no existe
        /// </summary>
        public async Task<ProductividadAgregadaDto?> ObtenerProductividadAgregadaAsync(
            int? periodo = null, DateTime? fechaInicio = null, DateTime? fechaFin = null)
        {
            // TODO: SP CC_ProductividadAgregada no existe
            throw new NotImplementedException(
                "SP CC_ProductividadAgregada no existe en BD legacy.");
        }

        #endregion

        #region Conteo Trabajos (CRUD)

        /// <summary>
        /// Obtiene conteos con filtros
        /// SP: CC_ConteosXIdGet (solo acepta @TrabajoId)
        /// </summary>
        public async Task<IEnumerable<ConteoTrabajoDto>> ObtenerConteosAsync(
            long? idTrabajo = null, DateTime? fechaInicio = null, DateTime? fechaFin = null)
        {
            const string sql = "CC_ConteosXIdGet";
            var parameters = new DynamicParameters();
            
            if (idTrabajo.HasValue)
                parameters.Add("@TrabajoId", idTrabajo.Value);
            // Nota: fechaInicio y fechaFin se ignoran porque el SP no los acepta

            return await _dbConnection.QueryAsync<ConteoTrabajoDto>(
                sql, parameters, commandType: CommandType.StoredProcedure);
        }

        /// <summary>
        /// Obtiene actividades por trabajo
        /// SP: CC_ActividadesXTrabajo
        /// </summary>
        public async Task<IEnumerable<ActividadTrabajoDto>> ObtenerActividadesPorTrabajoAsync(
            long idTrabajo)
        {
            const string sql = "CC_ActividadesXTrabajo";
            var parameters = new DynamicParameters();
            parameters.Add("@TrabajoId", idTrabajo);

            return await _dbConnection.QueryAsync<ActividadTrabajoDto>(
                sql, parameters, commandType: CommandType.StoredProcedure);
        }

        /// <summary>
        /// Guarda preguntas históricas de un trabajo (conteo de cuestionario)
        /// SP: CC_PreguntasHistoricoGuardar
        /// Origen: CoreProject/Clases/CC_FinzOpe/ProcesosInternos.vb - GuardarConteo
        /// </summary>
        public async Task GuardarPreguntasHistoricoAsync(GuardarPreguntasHistoricoRequest request)
        {
            const string sql = "CC_PreguntasHistoricoGuardar";
            var parameters = new DynamicParameters();
            
            // Parámetros exactos del SP legacy
            parameters.Add("@duracion", request.Duracion);
            parameters.Add("@Cerradas", request.Cerradas);
            parameters.Add("@CerradasM", request.CerradasMultiple);
            parameters.Add("@Abiertas", request.Abiertas);
            parameters.Add("@AbiertasM", request.AbiertasMultiple);
            parameters.Add("@Otros", request.Otros);
            parameters.Add("@Demograficos", request.Demograficos);
            parameters.Add("@Paginas", request.Paginas);
            parameters.Add("@Observacion", request.Observaciones ?? string.Empty);
            parameters.Add("@Usuarioid", request.UsuarioId);
            parameters.Add("@Job", request.Job);
            parameters.Add("@TrabajoId", request.TrabajoId);
            parameters.Add("@NombreTrabajo", request.NombreTrabajo);
            parameters.Add("@Unidad", request.Unidad);
            parameters.Add("@Producto", request.Producto);

            await _dbConnection.ExecuteAsync(sql, parameters, commandType: CommandType.StoredProcedure);
        }

        #endregion

        #region Requerimientos Equipo (CRUD)

        /// <summary>
        /// Obtiene requerimientos de equipo
        /// NOTA: SP CC_Requerimientos_Get no existe
        /// </summary>
        public async Task<IEnumerable<RequerimientoEquipoDto>> ObtenerRequerimientosAsync(
            long? idTrabajo = null, byte? estado = null)
        {
            // TODO: SP CC_Requerimientos_Get no existe en BD legacy
            throw new NotImplementedException(
                "SP CC_Requerimientos_Get no existe en BD legacy.");
        }

        /// <summary>
        /// Genera muestra de requerimientos
        /// SP: CC_MuestraGenerarRequerimiento (verificar si existe)
        /// </summary>
        public async Task<IEnumerable<MuestraRequerimientoDto>> GenerarMuestraRequerimientosAsync(
            long idTrabajo)
        {
            // TODO: Verificar si CC_MuestraGenerarRequerimiento existe
            throw new NotImplementedException(
                "SP CC_MuestraGenerarRequerimiento - verificar existencia.");
        }

        /// <summary>
        /// Guarda un requerimiento
        /// NOTA: SP CC_GenerarRequerimientos no existe como CRUD
        /// </summary>
        public async Task<long> GuardarRequerimientoAsync(GuardarRequerimientoRequest request)
        {
            // TODO: SP CC_GenerarRequerimientos no existe para CRUD
            throw new NotImplementedException(
                "SP CC_GenerarRequerimientos no existe para operaciones CRUD.");
        }

        /// <summary>
        /// Elimina un requerimiento
        /// NOTA: SP CC_Requerimientos_Delete no existe
        /// </summary>
        public async Task EliminarRequerimientoAsync(long idRequerimiento)
        {
            // TODO: SP CC_Requerimientos_Delete no existe
            throw new NotImplementedException(
                "SP CC_Requerimientos_Delete no existe en BD legacy.");
        }

        #endregion

        #region Consolidación Producción

        /// <summary>
        /// Obtiene producción pendiente de consolidar
        /// NOTA: SP CC_Produccion_PendienteConsolidar no existe - usar CC_ProduccionXFechas
        /// </summary>
        public async Task<IEnumerable<ProduccionDto>> ObtenerProduccionPendienteAsync(
            int? periodo = null, long? idTrabajo = null)
        {
            // Usar CC_ProduccionXFechas como alternativa
            const string sql = "CC_ProduccionXFechas";
            var parameters = new DynamicParameters();
            
            // CC_ProduccionXFechas acepta @FechaInicio, @FechaFin
            // Calcular fechas del periodo si se especifica
            if (periodo.HasValue)
            {
                var año = periodo.Value / 100;
                var mes = periodo.Value % 100;
                parameters.Add("@FechaInicio", new DateTime(año, mes, 1));
                parameters.Add("@FechaFin", new DateTime(año, mes, DateTime.DaysInMonth(año, mes)));
            }

            return await _dbConnection.QueryAsync<ProduccionDto>(
                sql, parameters, commandType: CommandType.StoredProcedure);
        }

        /// <summary>
        /// Consolida producción
        /// NOTA: SP CC_ConsolidacionProduccion no existe
        /// </summary>
        public async Task ConsolidarProduccionAsync(ConsolidarProduccionRequest request)
        {
            // TODO: SP CC_ConsolidacionProduccion no existe
            throw new NotImplementedException(
                "SP CC_ConsolidacionProduccion no existe en BD legacy.");
        }

        /// <summary>
        /// Obtiene resumen de consolidación
        /// NOTA: SP CC_ResumenConsolidacion no existe
        /// </summary>
        public async Task<ResumenConsolidacionDto?> ObtenerResumenConsolidacionAsync(int periodo)
        {
            // TODO: SP CC_ResumenConsolidacion no existe
            throw new NotImplementedException(
                "SP CC_ResumenConsolidacion no existe en BD legacy.");
        }

        #endregion

        #region Cálculo Jornada Laboral

        /// <summary>
        /// Obtiene jornadas laborales
        /// NOTA: SP CC_CalculoJornada_Get no existe
        /// </summary>
        public async Task<IEnumerable<JornadaLaboralDto>> ObtenerJornadasAsync(
            int? periodo = null, long? idEmpleado = null)
        {
            // TODO: SP CC_CalculoJornada_Get no existe
            throw new NotImplementedException(
                "SP CC_CalculoJornada_Get no existe en BD legacy.");
        }

        /// <summary>
        /// Obtiene ausencias del empleado (integración TH)
        /// NOTA: SP TH_Ausencia_CalculoDias - verificar existencia
        /// </summary>
        public async Task<IEnumerable<AusenciaEmpleadoDto>> ObtenerAusenciasEmpleadoAsync(
            long idEmpleado, DateTime fechaInicio, DateTime fechaFin)
        {
            // TODO: Verificar si TH_Ausencia_CalculoDias existe
            throw new NotImplementedException(
                "SP TH_Ausencia_CalculoDias - verificar existencia en BD.");
        }

        /// <summary>
        /// Calcula y guarda jornada laboral
        /// NOTA: SP CC_CalculoJornada_Insert no existe
        /// </summary>
        public async Task<long> CalcularJornadaAsync(CalcularJornadaRequest request)
        {
            // TODO: SP CC_CalculoJornada_Insert no existe
            throw new NotImplementedException(
                "SP CC_CalculoJornada_Insert no existe en BD legacy.");
        }

        /// <summary>
        /// Obtiene resumen de jornadas
        /// NOTA: SP CC_ResumenJornadas no existe
        /// </summary>
        public async Task<ResumenJornadasDto?> ObtenerResumenJornadasAsync(int periodo)
        {
            // TODO: SP CC_ResumenJornadas no existe
            throw new NotImplementedException(
                "SP CC_ResumenJornadas no existe en BD legacy.");
        }

        #endregion
    }
}
