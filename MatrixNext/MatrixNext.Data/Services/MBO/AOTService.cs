using MatrixNext.Data.ViewModels.MBO;
using MatrixNext.Data.Models.MBO;
using MatrixNext.Data.Adapters.MBO;
using Microsoft.Extensions.Logging;

namespace MatrixNext.Data.Services.MBO;

/// <summary>
/// Implementación del servicio de lógica de negocio para AOT
/// </summary>
public class AOTService : IAOTService
{
    private readonly IAOTAdapter _adapter;
    private readonly ILogger<AOTService> _logger;

    public AOTService(IAOTAdapter adapter, ILogger<AOTService> logger)
    {
        _adapter = adapter;
        _logger = logger;
    }

    public async Task<IEnumerable<UnidadUsuarioDto>> ObtenerUnidadesUsuarioAsync(int usuarioId)
    {
        try
        {
            return await _adapter.ObtenerUnidadesUsuarioAsync(usuarioId);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error en servicio obteniendo unidades para usuario {UsuarioId}", usuarioId);
            throw;
        }
    }

    public async Task<AOTDireccionViewModel> ObtenerDatosDireccionAsync(int año, int mes, string sigla, int usuarioId)
    {
        try
        {
            _logger.LogInformation("Obteniendo datos AOT Dirección. Año: {Año}, Mes: {Mes}, Sigla: {Sigla}, Usuario: {UsuarioId}",
                año, mes, sigla, usuarioId);

            // Ejecutar queries en paralelo para optimizar rendimiento
            var tasks = new List<Task>
            {
                _adapter.ObtenerUnidadesUsuarioAsync(usuarioId),
                _adapter.ObtenerBudgetEjecucionAsync(año, mes, sigla),
                _adapter.ObtenerMetaTotalAsync(sigla),
                _adapter.ObtenerEjecucionTotalAsync(año, mes, sigla),
                _adapter.ObtenerBudgetPorUnidadAsync(año, mes, sigla),
                _adapter.ObtenerAOTAcquisitionAsync(sigla)
            };

            await Task.WhenAll(tasks);

            var viewModel = new AOTDireccionViewModel
            {
                AñoSeleccionado = año,
                MesSeleccionado = mes,
                SiglaSeleccionada = sigla,
                UnidadesDisponibles = await (Task<IEnumerable<UnidadUsuarioDto>>)tasks[0],
                BudgetEjecucion = await (Task<AOTBudgetEjecucionDto?>)tasks[1],
                MetaTotal = await (Task<AOTMetaTotalDto?>)tasks[2],
                EjecucionTotal = await (Task<AOTEjecucionTotalDto?>)tasks[3],
                UnidadesDetalle = await (Task<IEnumerable<AOTUnidadDto>>)tasks[4],
                Acquisition = await (Task<AOTAcquisitionDto?>)tasks[5]
            };

            return viewModel;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error obteniendo datos AOT Dirección. Año: {Año}, Mes: {Mes}, Sigla: {Sigla}",
                año, mes, sigla);
            throw;
        }
    }

    public async Task<AOTGerenciaViewModel> ObtenerDatosGerenciaAsync(int año, int mes, string sigla, int usuarioId)
    {
        try
        {
            _logger.LogInformation("Obteniendo datos AOT Gerencia. Año: {Año}, Mes: {Mes}, Sigla: {Sigla}, Usuario: {UsuarioId}",
                año, mes, sigla, usuarioId);

            // Ejecutar queries en paralelo
            var tasks = new List<Task>
            {
                _adapter.ObtenerUnidadesUsuarioAsync(usuarioId),
                _adapter.ObtenerBudgetEjecucionAsync(año, mes, sigla),
                _adapter.ObtenerMetaTotalAsync(sigla),
                _adapter.ObtenerEjecucionTotalAsync(año, mes, sigla),
                _adapter.ObtenerBudgetPorUnidadAsync(año, mes, sigla)
            };

            await Task.WhenAll(tasks);

            var viewModel = new AOTGerenciaViewModel
            {
                AñoSeleccionado = año,
                MesSeleccionado = mes,
                SiglaSeleccionada = sigla,
                UnidadesDisponibles = await (Task<IEnumerable<UnidadUsuarioDto>>)tasks[0],
                BudgetEjecucion = await (Task<AOTBudgetEjecucionDto?>)tasks[1],
                MetaTotal = await (Task<AOTMetaTotalDto?>)tasks[2],
                EjecucionTotal = await (Task<AOTEjecucionTotalDto?>)tasks[3],
                UnidadesDetalle = await (Task<IEnumerable<AOTUnidadDto>>)tasks[4]
            };

            return viewModel;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error obteniendo datos AOT Gerencia. Año: {Año}, Mes: {Mes}, Sigla: {Sigla}",
                año, mes, sigla);
            throw;
        }
    }

    public async Task<AOTPorGerentesViewModel> ObtenerDatosPorGerentesAsync(int año, int mes, string sigla, int usuarioId)
    {
        try
        {
            _logger.LogInformation("Obteniendo datos AOT por Gerentes. Año: {Año}, Mes: {Mes}, Sigla: {Sigla}, Usuario: {UsuarioId}",
                año, mes, sigla, usuarioId);

            // Ejecutar queries en paralelo
            var unidadesTask = _adapter.ObtenerUnidadesUsuarioAsync(usuarioId);
            var gerentesTask = _adapter.ObtenerAOTPorGerenteAsync(año, mes, sigla);

            await Task.WhenAll(unidadesTask, gerentesTask);

            var viewModel = new AOTPorGerentesViewModel
            {
                AñoSeleccionado = año,
                MesSeleccionado = mes,
                SiglaSeleccionada = sigla,
                UnidadesDisponibles = await unidadesTask,
                GerentesDetalle = await gerentesTask
            };

            return viewModel;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error obteniendo datos AOT por Gerentes. Año: {Año}, Mes: {Mes}, Sigla: {Sigla}",
                año, mes, sigla);
            throw;
        }
    }

    public async Task<AOTUnidadViewModel> ObtenerDatosUnidadAsync(int año, int mes, string sigla, int usuarioId)
    {
        try
        {
            _logger.LogInformation("Obteniendo datos AOT Unidad. Año: {Año}, Mes: {Mes}, Sigla: {Sigla}, Usuario: {UsuarioId}",
                año, mes, sigla, usuarioId);

            // Ejecutar queries en paralelo
            var unidadesTask = _adapter.ObtenerUnidadesUsuarioAsync(usuarioId);
            var budgetTask = _adapter.ObtenerBudgetEjecucionAsync(año, mes, sigla);
            var metaTask = _adapter.ObtenerMetaTotalAsync(sigla);

            await Task.WhenAll(unidadesTask, budgetTask, metaTask);

            var unidades = await unidadesTask;
            var unidadInfo = unidades.FirstOrDefault(u => u.Sigla == sigla);

            var viewModel = new AOTUnidadViewModel
            {
                AñoSeleccionado = año,
                MesSeleccionado = mes,
                SiglaSeleccionada = sigla,
                UnidadInfo = unidadInfo,
                BudgetEjecucion = await budgetTask,
                MetaTotal = await metaTask
            };

            return viewModel;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error obteniendo datos AOT Unidad. Año: {Año}, Mes: {Mes}, Sigla: {Sigla}",
                año, mes, sigla);
            throw;
        }
    }
}
