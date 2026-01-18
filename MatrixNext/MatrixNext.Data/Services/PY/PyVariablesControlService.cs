using MatrixNext.Data.Adapters.PY;
using MatrixNext.Data.Adapters.PY.Models;
using MatrixNext.Data.Services.PY.Interfaces;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MatrixNext.Data.Services.PY
{
    /// <summary>
    /// Servicio de variables de control PY.
    /// Acceso a datos: EF Core (entidad PY_Variables_Control pendiente de registrar en DbContext)
    /// </summary>
    public class PyVariablesControlService : IPyVariablesControlService
    {
        private readonly IPyVariablesControlAdapter _adapter;
        private readonly ILogger<PyVariablesControlService> _logger;

        public PyVariablesControlService(
            IPyVariablesControlAdapter adapter,
            ILogger<PyVariablesControlService> logger)
        {
            _adapter = adapter;
            _logger = logger;
        }

        /// <summary>
        /// Obtiene variables de control por trabajo.
        /// Llama a adapter EF Core: ObtenerVariablesControlPorTrabajo
        /// </summary>
        public async Task<List<VariableControlDto>> ObtenerVariablesPorTrabajo(int trabajoId)
        {
            if (trabajoId <= 0) throw new ArgumentException("TrabajoId debe ser > 0", nameof(trabajoId));
            
            try
            {
                var variables = await _adapter.ObtenerVariablesControlPorTrabajo(trabajoId);
                return variables ?? new List<VariableControlDto>();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error obteniendo variables de control. TrabajoId: {TrabajoId}", trabajoId);
                return new List<VariableControlDto>();
            }
        }

        /// <summary>
        /// Guarda variable de control.
        /// Llama a adapter EF Core: GuardarVariableControl
        /// </summary>
        public async Task<int> GuardarVariableControl(VariableControlInputDto input)
        {
            if (input == null) throw new ArgumentNullException(nameof(input));
            if (input.TrabajoId <= 0) throw new ArgumentException("TrabajoId es requerido", nameof(input.TrabajoId));
            
            try
            {
                var id = await _adapter.GuardarVariableControl(input);
                _logger.LogInformation("Variable de control guardada. Id: {Id}, TrabajoId: {TrabajoId}", 
                    id, input.TrabajoId);
                return (int)id;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error guardando variable de control. TrabajoId: {TrabajoId}", input.TrabajoId);
                return 0;
            }
        }

        /// <summary>
        /// Valida si las variables de control están completadas para un trabajo.
        /// </summary>
        public async Task<bool> ValidarVariablesCompletadas(int trabajoId)
        {
            if (trabajoId <= 0) throw new ArgumentException("TrabajoId debe ser > 0", nameof(trabajoId));
            
            try
            {
                var variables = await ObtenerVariablesPorTrabajo(trabajoId);
                return variables != null && variables.Count > 0;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error validando variables. TrabajoId: {TrabajoId}", trabajoId);
                return false;
            }
        }
    }
}
