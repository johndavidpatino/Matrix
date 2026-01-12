using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using Dapper;
using MatrixNext.Data.Adapters.PY.Models;
using Microsoft.Extensions.Configuration;

namespace MatrixNext.Data.Adapters.PY
{
    /// <summary>
    /// Adapter para Planillas de Moderación e Informes UU
    /// Usa Dapper puro (patrón existente en CoreProject.PlanillaModeracionDapper)
    /// </summary>
    public class PyPlanillasAdapter : IPyPlanillasAdapter
    {
        private readonly string _connectionString;

        public PyPlanillasAdapter(IConfiguration config)
        {
            _connectionString = config.GetConnectionString("MatrixDb")!;
        }

        #region Catálogos

        /// <summary>
        /// Obtiene técnicas por tipo
        /// SP: UU_TecnicasGet(@TipoTecnica NVARCHAR(50))
        /// Legacy: PlanillaModeracionRepository.GetTecnicas(TipoTecnica)
        /// </summary>
        public async Task<List<TecnicaDto>> ObtenerTecnicas(string tipoTecnica)
        {
            using var connection = new SqlConnection(_connectionString);
            var parametros = new DynamicParameters();
            parametros.Add("@TipoTecnica", tipoTecnica);

            var resultado = await connection.QueryAsync<TecnicaDto>(
                "UU_TecnicasGet",
                parametros,
                commandType: CommandType.StoredProcedure);

            return resultado.ToList();
        }

        /// <summary>
        /// Obtiene moderadores
        /// SP: UU_ModeradoresGet
        /// Legacy: PlanillaModeracionRepository.GetModeradores()
        /// </summary>
        public async Task<List<ModeradorDto>> ObtenerModeradores()
        {
            using var connection = new SqlConnection(_connectionString);
            var resultado = await connection.QueryAsync<ModeradorDto>(
                "UU_ModeradoresGet",
                commandType: CommandType.StoredProcedure);

            return resultado.ToList();
        }

        #endregion

        #region Planillas Moderación

        /// <summary>
        /// Crea planilla de moderación
        /// SP: UU_PlanillaModeracion_Add
        /// Legacy: PlanillaModeracionRepository.SavePlanillaModeracion(planillaModeracion)
        /// </summary>
        public async Task<int> CrearPlanillaModeracion(PlanillaModeracionInputDto input)
        {
            using var connection = new SqlConnection(_connectionString);
            var parametros = new DynamicParameters();
            parametros.Add("@IdJob", input.IdJob);
            parametros.Add("@JobDesc", input.JobDesc);
            parametros.Add("@Fecha", input.Fecha);
            parametros.Add("@Hora", input.Hora);
            parametros.Add("@Tecnica", input.Tecnica);
            parametros.Add("@Tiempo", input.Tiempo);
            parametros.Add("@Moderador", input.Moderador);
            parametros.Add("@Rol", input.Rol);
            parametros.Add("@IdUsuarioRegistro", input.IdUsuarioRegistro);
            parametros.Add("@Observaciones", input.Observaciones);
            parametros.Add("@IdCuentasUU", input.IdCuentasUU);
            parametros.Add("@BI_WBSL", input.ServiceLineName);
            parametros.Add("@IdPlanilla", dbType: DbType.Int32, direction: ParameterDirection.Output);

            await connection.ExecuteAsync(
                "UU_PlanillaModeracion_Add",
                parametros,
                commandType: CommandType.StoredProcedure);

            return parametros.Get<int>("@IdPlanilla");
        }

        /// <summary>
        /// Actualiza estado de planilla de moderación
        /// SP: UU_PlanillaModeracion_Update
        /// Legacy: PlanillaModeracionRepository.UpdatePlanillaModeracion(...)
        /// </summary>
        public async Task ActualizarPlanillaModeracion(ActualizarEstadoPlanillaInputDto input)
        {
            using var connection = new SqlConnection(_connectionString);
            var parametros = new DynamicParameters();
            parametros.Add("@IdPlanilla", input.IdPlanilla);
            parametros.Add("@IdEstado", input.IdEstado);
            parametros.Add("@Observaciones", input.Observaciones);
            parametros.Add("@DineroBi", input.BiDinero);
            parametros.Add("@StatusBi", input.BiStatus);
            parametros.Add("@IdUsuarioAprueba", input.IdUsuarioAprueba);
            parametros.Add("@FechaAprobacion", DateTime.Now);
            parametros.Add("@JobEncontradoEnBI", input.JobEncontradoEnBI);

            await connection.ExecuteAsync(
                "UU_PlanillaModeracion_Update",
                parametros,
                commandType: CommandType.StoredProcedure);
        }

        /// <summary>
        /// Obtiene planilla de moderación por ID
        /// SP: UU_PlanillaModeracionGetBy(@IdPlanilla INT)
        /// Legacy: PlanillaModeracionRepository.GetPlanillasModeracionBy(idPlanilla)
        /// </summary>
        public async Task<PlanillaModeracionDto?> ObtenerPlanillaModeracionPorId(int idPlanilla)
        {
            using var connection = new SqlConnection(_connectionString);
            var parametros = new DynamicParameters();
            parametros.Add("@IdPlanilla", idPlanilla);

            var resultado = await connection.QueryFirstOrDefaultAsync<PlanillaModeracionDto>(
                "UU_PlanillaModeracionGetBy",
                parametros,
                commandType: CommandType.StoredProcedure);

            return resultado;
        }

        #endregion

        #region Planillas Informes

        /// <summary>
        /// Crea planilla de informes
        /// SP: UU_PlanillaInformes_Add
        /// Legacy: PlanillaModeracionRepository.SavePlanillaInformes(planillaInformes)
        /// </summary>
        public async Task<int> CrearPlanillaInformes(PlanillaInformesInputDto input)
        {
            using var connection = new SqlConnection(_connectionString);
            var parametros = new DynamicParameters();
            parametros.Add("@IdJob", input.IdJob);
            parametros.Add("@JobDesc", input.JobDesc);
            parametros.Add("@Fecha", input.Fecha);
            parametros.Add("@Tecnica", input.Tecnica);
            parametros.Add("@Muestra", input.Muestra);
            parametros.Add("@IdCuentasUU", input.IdCuentasUU);
            parametros.Add("@Analista", input.Analista);
            parametros.Add("@Observaciones", input.Observaciones);
            parametros.Add("@IdUsuarioRegistro", input.IdUsuarioRegistro);
            parametros.Add("@ServiceLineName", input.ServiceLineName);
            parametros.Add("@IdPlanilla", dbType: DbType.Int32, direction: ParameterDirection.Output);

            await connection.ExecuteAsync(
                "UU_PlanillaInformes_Add",
                parametros,
                commandType: CommandType.StoredProcedure);

            return parametros.Get<int>("@IdPlanilla");
        }

        /// <summary>
        /// Actualiza estado de planilla de informes
        /// SP: UU_PlanillaInformes_Update
        /// Legacy: PlanillaModeracionRepository.UpdatePlanillaInformes(...)
        /// </summary>
        public async Task ActualizarPlanillaInformes(ActualizarEstadoPlanillaInputDto input)
        {
            using var connection = new SqlConnection(_connectionString);
            var parametros = new DynamicParameters();
            parametros.Add("@IdPlanilla", input.IdPlanilla);
            parametros.Add("@IdEstado", input.IdEstado);
            parametros.Add("@Observaciones", input.Observaciones);
            parametros.Add("@DineroBi", input.BiDinero);
            parametros.Add("@StatusBi", input.BiStatus);
            parametros.Add("@IdUsuarioAprueba", input.IdUsuarioAprueba);
            parametros.Add("@FechaAprobacion", DateTime.Now);
            parametros.Add("@JobEncontradoEnBI", input.JobEncontradoEnBI);

            await connection.ExecuteAsync(
                "UU_PlanillaInformes_Update",
                parametros,
                commandType: CommandType.StoredProcedure);
        }

        /// <summary>
        /// Obtiene planilla de informes por ID
        /// SP: UU_PlanillaInformesGetBy(@IdPlanilla INT)
        /// Legacy: PlanillaModeracionRepository.GetPlanillasInformesBy(idPlanilla)
        /// </summary>
        public async Task<PlanillaInformesDto?> ObtenerPlanillaInformesPorId(int idPlanilla)
        {
            using var connection = new SqlConnection(_connectionString);
            var parametros = new DynamicParameters();
            parametros.Add("@IdPlanilla", idPlanilla);

            var resultado = await connection.QueryFirstOrDefaultAsync<PlanillaInformesDto>(
                "UU_PlanillaInformesGetBy",
                parametros,
                commandType: CommandType.StoredProcedure);

            return resultado;
        }

        #endregion

        #region Listado y Exportación

        /// <summary>
        /// Obtiene planillas paginadas con filtros
        /// SP: UU_PlanillasGet(@PageSize INT, @PageIndex INT, @FiltroPlanilla NVARCHAR(100), @IdEstado SMALLINT)
        /// Legacy: PlanillaModeracionRepository.GetPlanillas(pageSize, pageIndex, filtroPlanilla, idEstado)
        /// </summary>
        public async Task<List<PlanillaListDto>> ObtenerPlanillasPaginadas(int pageSize, int pageIndex, string? filtro, short? idEstado)
        {
            using var connection = new SqlConnection(_connectionString);
            var parametros = new DynamicParameters();
            parametros.Add("@PageSize", pageSize);
            parametros.Add("@PageIndex", pageIndex);
            parametros.Add("@FiltroPlanilla", filtro);
            parametros.Add("@IdEstado", idEstado);

            var resultado = await connection.QueryAsync<PlanillaListDto>(
                "UU_PlanillasGet",
                parametros,
                commandType: CommandType.StoredProcedure);

            return resultado.ToList();
        }

        /// <summary>
        /// Obtiene planillas de moderación para exportar
        /// SP: UU_PlanillasModeracionExport(@FechaInicio DATETIME, @FechaFinal DATETIME)
        /// Legacy: PlanillaModeracionRepository.GetPlanillasModeracionToExport(fechaInicio, fechaFinal)
        /// </summary>
        public async Task<List<PlanillaModeracionDto>> ObtenerPlanillasModeracionParaExportar(DateTime fechaInicio, DateTime fechaFinal)
        {
            using var connection = new SqlConnection(_connectionString);
            var parametros = new DynamicParameters();
            parametros.Add("@FechaInicio", fechaInicio);
            parametros.Add("@FechaFinal", fechaFinal);

            var resultado = await connection.QueryAsync<PlanillaModeracionDto>(
                "UU_PlanillasModeracionExport",
                parametros,
                commandType: CommandType.StoredProcedure);

            return resultado.ToList();
        }

        /// <summary>
        /// Obtiene planillas de informes para exportar
        /// SP: UU_PlanillasInformesExport(@FechaInicio DATETIME, @FechaFinal DATETIME)
        /// Legacy: PlanillaModeracionRepository.GetPlanillasInformesToExport(fechaInicio, fechaFinal)
        /// </summary>
        public async Task<List<PlanillaInformesDto>> ObtenerPlanillasInformesParaExportar(DateTime fechaInicio, DateTime fechaFinal)
        {
            using var connection = new SqlConnection(_connectionString);
            var parametros = new DynamicParameters();
            parametros.Add("@FechaInicio", fechaInicio);
            parametros.Add("@FechaFinal", fechaFinal);

            var resultado = await connection.QueryAsync<PlanillaInformesDto>(
                "UU_PlanillasInformesExport",
                parametros,
                commandType: CommandType.StoredProcedure);

            return resultado.ToList();
        }

        #endregion
    }
}
