using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using Dapper;
using MatrixNext.Data.Adapters.PY.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace MatrixNext.Data.Adapters.PY
{
    /// <summary>
    /// Adapter para Planillas de Moderación e Informes UU
    /// NOTA: Los SP UU_* no existen en BD legacy - métodos son stubs.
    /// </summary>
    public class PyPlanillasAdapter : IPyPlanillasAdapter
    {
        private readonly string _connectionString;
        private readonly ILogger<PyPlanillasAdapter> _logger;

        public PyPlanillasAdapter(IConfiguration config, ILogger<PyPlanillasAdapter> logger)
        {
            _connectionString = config.GetConnectionString("MatrixDb")!;
            _logger = logger;
        }

        #region Catálogos

        /// <summary>
        /// STUB: SP UU_TecnicasGet no existe en BD legacy.
        /// Retorna lista vacía hasta que se implemente el SP o se migre la lógica.
        /// </summary>
        public Task<List<TecnicaDto>> ObtenerTecnicas(string tipoTecnica)
        {
            _logger.LogWarning("[PY] ObtenerTecnicas: SP UU_TecnicasGet no existe en legacy. TipoTecnica: {TipoTecnica}", tipoTecnica);
            return Task.FromResult(new List<TecnicaDto>());
        }

        /// <summary>
        /// STUB: SP UU_ModeradoresGet no existe en BD legacy.
        /// Retorna lista vacía hasta que se implemente el SP o se migre la lógica.
        /// </summary>
        public Task<List<ModeradorDto>> ObtenerModeradores()
        {
            _logger.LogWarning("[PY] ObtenerModeradores: SP UU_ModeradoresGet no existe en legacy");
            return Task.FromResult(new List<ModeradorDto>());
        }

        #endregion

        #region Planillas Moderación

        /// <summary>
        /// STUB: SP UU_PlanillaModeracion_Add no existe en BD legacy.
        /// Retorna 0 hasta que se implemente el SP o se migre la lógica.
        /// </summary>
        public Task<int> CrearPlanillaModeracion(PlanillaModeracionInputDto input)
        {
            _logger.LogWarning("[PY] CrearPlanillaModeracion: SP UU_PlanillaModeracion_Add no existe en legacy. IdJob: {IdJob}", input.IdJob);
            return Task.FromResult(0);
        }

        /// <summary>
        /// STUB: SP UU_PlanillaModeracion_Update no existe en BD legacy.
        /// No realiza acción hasta que se implemente el SP o se migre la lógica.
        /// </summary>
        public Task ActualizarPlanillaModeracion(ActualizarEstadoPlanillaInputDto input)
        {
            _logger.LogWarning("[PY] ActualizarPlanillaModeracion: SP UU_PlanillaModeracion_Update no existe en legacy. IdPlanilla: {IdPlanilla}", input.IdPlanilla);
            return Task.CompletedTask;
        }

        /// <summary>
        /// STUB: SP UU_PlanillaModeracionGetBy no existe en BD legacy.
        /// Retorna null hasta que se implemente el SP o se migre la lógica.
        /// </summary>
        public Task<PlanillaModeracionDto?> ObtenerPlanillaModeracionPorId(int idPlanilla)
        {
            _logger.LogWarning("[PY] ObtenerPlanillaModeracionPorId: SP UU_PlanillaModeracionGetBy no existe en legacy. IdPlanilla: {IdPlanilla}", idPlanilla);
            return Task.FromResult<PlanillaModeracionDto?>(null);
        }

        #endregion

        #region Planillas Informes

        /// <summary>
        /// STUB: SP UU_PlanillaInformes_Add no existe en BD legacy.
        /// Retorna 0 hasta que se implemente el SP o se migre la lógica.
        /// </summary>
        public Task<int> CrearPlanillaInformes(PlanillaInformesInputDto input)
        {
            _logger.LogWarning("[PY] CrearPlanillaInformes: SP UU_PlanillaInformes_Add no existe en legacy. IdJob: {IdJob}", input.IdJob);
            return Task.FromResult(0);
        }

        /// <summary>
        /// STUB: SP UU_PlanillaInformes_Update no existe en BD legacy.
        /// No realiza acción hasta que se implemente el SP o se migre la lógica.
        /// </summary>
        public Task ActualizarPlanillaInformes(ActualizarEstadoPlanillaInputDto input)
        {
            _logger.LogWarning("[PY] ActualizarPlanillaInformes: SP UU_PlanillaInformes_Update no existe en legacy. IdPlanilla: {IdPlanilla}", input.IdPlanilla);
            return Task.CompletedTask;
        }

        /// <summary>
        /// STUB: SP UU_PlanillaInformesGetBy no existe en BD legacy.
        /// Retorna null hasta que se implemente el SP o se migre la lógica.
        /// </summary>
        public Task<PlanillaInformesDto?> ObtenerPlanillaInformesPorId(int idPlanilla)
        {
            _logger.LogWarning("[PY] ObtenerPlanillaInformesPorId: SP UU_PlanillaInformesGetBy no existe en legacy. IdPlanilla: {IdPlanilla}", idPlanilla);
            return Task.FromResult<PlanillaInformesDto?>(null);
        }

        #endregion

        #region Listado y Exportación

        /// <summary>
        /// STUB: SP UU_PlanillasGet no existe en BD legacy.
        /// Retorna lista vacía hasta que se implemente el SP o se migre la lógica.
        /// </summary>
        public Task<List<PlanillaListDto>> ObtenerPlanillasPaginadas(int pageSize, int pageIndex, string? filtro, short? idEstado)
        {
            _logger.LogWarning("[PY] ObtenerPlanillasPaginadas: SP UU_PlanillasGet no existe en legacy. PageSize: {PageSize}, PageIndex: {PageIndex}", pageSize, pageIndex);
            return Task.FromResult(new List<PlanillaListDto>());
        }

        /// <summary>
        /// STUB: SP UU_PlanillasModeracionExport no existe en BD legacy.
        /// Retorna lista vacía hasta que se implemente el SP o se migre la lógica.
        /// </summary>
        public Task<List<PlanillaModeracionDto>> ObtenerPlanillasModeracionParaExportar(DateTime fechaInicio, DateTime fechaFinal)
        {
            _logger.LogWarning("[PY] ObtenerPlanillasModeracionParaExportar: SP UU_PlanillasModeracionExport no existe en legacy. FechaInicio: {FechaInicio}, FechaFinal: {FechaFinal}", fechaInicio, fechaFinal);
            return Task.FromResult(new List<PlanillaModeracionDto>());
        }

        /// <summary>
        /// STUB: SP UU_PlanillasInformesExport no existe en BD legacy.
        /// Retorna lista vacía hasta que se implemente el SP o se migre la lógica.
        /// </summary>
        public Task<List<PlanillaInformesDto>> ObtenerPlanillasInformesParaExportar(DateTime fechaInicio, DateTime fechaFinal)
        {
            _logger.LogWarning("[PY] ObtenerPlanillasInformesParaExportar: SP UU_PlanillasInformesExport no existe en legacy. FechaInicio: {FechaInicio}, FechaFinal: {FechaFinal}", fechaInicio, fechaFinal);
            return Task.FromResult(new List<PlanillaInformesDto>());
        }

        #endregion
    }
}
