using MatrixNext.Web.ViewModels.OP;

namespace MatrixNext.Web.Services.OP;

public interface IOpIFieldService
{
    Task<IReadOnlyList<IFieldProjectDto>> GetProjectsAsync(int tipo);
    Task<IFieldProjectDto?> GetProjectAsync(int projectId);
    Task<IReadOnlyList<IFieldConfigRow>> GetProjectConfigAsync(int projectId);
    Task<IReadOnlyList<IFieldPendingRow>> GetPendientesAsync(int projectId);
    Task UpdateProjectJobBookAsync(int projectId, int trabajoId);
    Task InsertConfigItemsAsync(IEnumerable<IFieldAddConfigInput> inputs);
    Task RemoveConfigItemAsync(int configId);
}
