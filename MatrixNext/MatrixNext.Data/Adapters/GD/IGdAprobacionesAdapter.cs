using System.Collections.Generic;
using System.Threading.Tasks;

namespace MatrixNext.Data.Adapters.GD
{
    public interface IGdAprobacionesAdapter
    {
        Task<IEnumerable<dynamic>> RevisionesGetRev(object parameters);
        Task<bool> RevisionesEdit(object parameters);
    }
}
