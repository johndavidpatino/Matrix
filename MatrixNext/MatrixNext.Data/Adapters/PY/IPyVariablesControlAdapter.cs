using System.Collections.Generic;
using System.Threading.Tasks;
using MatrixNext.Data.Adapters.PY.Models;

namespace MatrixNext.Data.Adapters.PY
{
    public interface IPyVariablesControlAdapter
    {
        Task<List<VariableControlDto>> ObtenerVariablesControlPorTrabajo(long trabajoId, string? modalidad = null);
        Task<VariableControlDto?> ObtenerVariableControlPorId(long id);
        Task<long> GuardarVariableControl(VariableControlInputDto input);
    }
}
