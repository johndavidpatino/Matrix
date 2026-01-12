using MatrixNext.Data.Adapters.PY;
using MatrixNext.Data.Adapters.PY.Models;
using MatrixNext.Data.Services.PY.Interfaces;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MatrixNext.Data.Services.PY
{
    public class PyVariablesControlService : IPyVariablesControlService
    {
        private readonly IPyVariablesControlAdapter _adapter;
        public PyVariablesControlService(IPyVariablesControlAdapter adapter) => _adapter = adapter;

        public async Task<List<VariableControlDto>> ObtenerVariablesPorTrabajo(int trabajoId)
        {
            if (trabajoId <= 0) throw new ArgumentException("TrabajoId debe ser > 0", nameof(trabajoId));
            return new List<VariableControlDto>(); // TODO: Implementar en adapter
        }

        public async Task<int> GuardarVariableControl(VariableControlInputDto input)
        {
            if (input == null) throw new ArgumentNullException(nameof(input));
            if (input.TrabajoId <= 0) throw new ArgumentException("TrabajoId es requerido", nameof(input.TrabajoId));
            return 0; // TODO: Implementar en adapter
        }

        public async Task<bool> ValidarVariablesCompletadas(int trabajoId)
        {
            if (trabajoId <= 0) throw new ArgumentException("TrabajoId debe ser > 0", nameof(trabajoId));
            var variables = await ObtenerVariablesPorTrabajo(trabajoId);
            return variables != null && variables.Count > 0;
        }
    }
}
