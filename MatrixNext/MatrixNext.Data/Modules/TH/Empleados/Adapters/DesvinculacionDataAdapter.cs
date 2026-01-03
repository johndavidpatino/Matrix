using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using Dapper;
using MatrixNext.Data.Models.TH;
using Microsoft.Extensions.Configuration;

namespace MatrixNext.Data.Adapters.TH
{
    /// <summary>
    /// Adapter para desvinculaciones de empleados.
    /// Migrado desde CoreProject.DesvinculacionEmpleadosDapper (Dapper + Stored Procedures).
    /// </summary>
    public class DesvinculacionDataAdapter
    {
        private readonly IConfiguration _configuration;
        private readonly string _connectionString;

        public DesvinculacionDataAdapter(IConfiguration configuration)
        {
            _configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
            _connectionString = _configuration.GetConnectionString("MatrixDb")
                                ?? _configuration.GetConnectionString("MatrixDatabase")
                                ?? throw new InvalidOperationException("ConnectionString 'MatrixDb' no configurada");
        }

        private IDbConnection CreateConnection() => new SqlConnection(_connectionString);

        #region RRHH - Resumen general

        /// <summary>
        /// Equivalente a DesvinculacionEmpleadosDapper.DesvinculacionesResumenGeneral
        /// SP: TH_DesvinculacionEmpleadosEstatus
        /// </summary>
        public async Task<IReadOnlyList<DesvinculacionEstatusDTO>> ObtenerDesvinculacionesEstatus(
            int pageIndex, int pageSize, string? textoBuscado)
        {
            using var db = CreateConnection();
            var parameters = new
            {
                PageIndex = pageIndex,
                PageSize = pageSize,
                TextoBuscado = textoBuscado ?? string.Empty
            };

            var result = await db.QueryAsync<DesvinculacionEstatusDTO>(
                sql: "TH_DesvinculacionEmpleadosEstatus",
                param: parameters,
                commandType: CommandType.StoredProcedure);

            return result.ToList();
        }

        #endregion

        #region RRHH - Empleados activos

        /// <summary>
        /// Equivalente a EmpleadosDapper.EmpleadosActivos
        /// SP: TH_EmpleadosActivos_Get
        /// </summary>
        public async Task<IReadOnlyList<EmpleadoActivoDTO>> ObtenerEmpleadosActivos()
        {
            using var db = CreateConnection();
            var result = await db.QueryAsync<EmpleadoActivoDTO>(
                sql: "TH_EmpleadosActivos_Get",
                commandType: CommandType.StoredProcedure);

            return result.ToList();
        }

        #endregion

        #region RRHH - Iniciar proceso

        /// <summary>
        /// Equivalente a DesvinculacionEmpleadosDapper.DesvinculacionAdd
        /// SP: TH_DesvinculacionEmpleadosAdd
        /// </summary>
        public async Task<long> IniciarProcesoDesvinculacion(
            int empleadoId,
            DateTime fechaRetiro,
            string motivosDesvinculacion,
            DateTime fechaRegistro,
            long usuarioRegistroId)
        {
            using var db = CreateConnection();
            var parameters = new DynamicParameters();
            parameters.Add("@EmpleadoId", empleadoId);
            parameters.Add("@FechaRegistro", fechaRegistro);
            parameters.Add("@FechaRetiro", fechaRetiro);
            parameters.Add("@MotivosDesvinculacion", motivosDesvinculacion);
            parameters.Add("@UsuarioRegistroId", usuarioRegistroId);

            return await db.ExecuteScalarAsync<long>(
                sql: "TH_DesvinculacionEmpleadosAdd",
                param: parameters,
                commandType: CommandType.StoredProcedure);
        }

        #endregion

        #region RRHH - Estatus evaluaciones

        /// <summary>
        /// Equivalente a DesvinculacionEmpleadosDapper.DesvinculacionesEstatusEvaluacionesPor
        /// SP: TH_DesvinculacionEmpleadosEstatusEvaluacionesPorDesvinculacion
        /// </summary>
        public async Task<IReadOnlyList<DesvinculacionEvaluacionDTO>> ObtenerEvaluacionesPorDesvinculacion(int desvinculacionEmpleadoId)
        {
            using var db = CreateConnection();
            var parameters = new { DesvinculacionEmpleadoId = desvinculacionEmpleadoId };

            var result = await db.QueryAsync<DesvinculacionEvaluacionDTO>(
                sql: "TH_DesvinculacionEmpleadosEstatusEvaluacionesPorDesvinculacion",
                param: parameters,
                commandType: CommandType.StoredProcedure);

            return result.ToList();
        }

        #endregion

        #region RRHH - Info empleado (PDF)

        /// <summary>
        /// Equivalente a DesvinculacionEmpleadosDapper.InformacionEmpleadoPor
        /// SP: TH_DesvinculacionesEmpleadosEmpleadoInfo
        /// </summary>
        public async Task<DesvinculacionEmpleadoEmpleadoInfoDTO?> ObtenerInformacionEmpleadoPor(int desvinculacionEmpleadoId)
        {
            using var db = CreateConnection();
            var parameters = new { DesvinculacionEmpleadoId = desvinculacionEmpleadoId };

            return await db.QueryFirstOrDefaultAsync<DesvinculacionEmpleadoEmpleadoInfoDTO>(
                sql: "TH_DesvinculacionesEmpleadosEmpleadoInfo",
                param: parameters,
                commandType: CommandType.StoredProcedure);
        }

        #endregion

        #region Gestión Área - Pendientes / Items / Guardar evaluación

        public async Task<IReadOnlyList<DesvinculacionEmpleadoPendientePorEvaluarAreaDTO>> PendientesPorEvaluarPorArea(int areaId)
        {
            using var db = CreateConnection();
            var result = await db.QueryAsync<DesvinculacionEmpleadoPendientePorEvaluarAreaDTO>(
                sql: "TH_DesvinculacionesEmpleadosPendientesEvaluarPorArea",
                param: new { AreaId = areaId },
                commandType: CommandType.StoredProcedure);
            return result.ToList();
        }

        public async Task<IReadOnlyList<DesvinculacionEmpleadosAreaItemVerificarDTO>> ItemsVerificarPor(int areaId)
        {
            using var db = CreateConnection();
            var result = await db.QueryAsync<DesvinculacionEmpleadosAreaItemVerificarDTO>(
                sql: "TH_DesvinculacionesEmpleadosItemsVerificarPorArea",
                param: new { AreaId = areaId },
                commandType: CommandType.StoredProcedure);
            return result.ToList();
        }

        public async Task<int> GuardarEvaluacion(DesvinculacionEmpleadoEvaluacionAreaDTO evaluacion)
        {
            using var db = CreateConnection();
            return await db.ExecuteScalarAsync<int>(
                sql: "TH_DesvinculacionEmpleadoAreaEvaluacion_Add",
                param: evaluacion,
                commandType: CommandType.StoredProcedure);
        }

        public async Task<IReadOnlyList<DesvinculacionEmpleadoPendienteEvaluarPorEvaluadorDTO>> PendientesPorEvaluarPorEvaluador(long evaluadorId)
        {
            using var db = CreateConnection();
            var result = await db.QueryAsync<DesvinculacionEmpleadoPendienteEvaluarPorEvaluadorDTO>(
                sql: "TH_DesvinculacionesEmpleadosPendientesEvaluarPorEvaluador",
                param: new { EvaluadorId = evaluadorId },
                commandType: CommandType.StoredProcedure);
            return result.ToList();
        }

        public async Task<IReadOnlyList<DesvinculacionEmpleadoEvaluacionRealizadaPorEvaluadorDTO>> EvaluacionesRealizadasPorEvaluador(long evaluadorId)
        {
            using var db = CreateConnection();
            var result = await db.QueryAsync<DesvinculacionEmpleadoEvaluacionRealizadaPorEvaluadorDTO>(
                sql: "TH_DesvinculacionEmpleadosEvaluacionesRealizadasPorEvaluador",
                param: new { EvaluadorId = evaluadorId },
                commandType: CommandType.StoredProcedure);
            return result.ToList();
        }

        public async Task<string?> FinalizarProceso(long desvinculacionEmpleadoId)
        {
            using var db = CreateConnection();
            var parameters = new DynamicParameters();
            parameters.Add("@desvinculacionEmpleadoId", desvinculacionEmpleadoId);

            return await db.ExecuteScalarAsync<string>(
                sql: "TH_DesvinculacionEmpleadoFinalizarProceso",
                param: parameters,
                commandType: CommandType.StoredProcedure);
        }

        #endregion
    }
}
