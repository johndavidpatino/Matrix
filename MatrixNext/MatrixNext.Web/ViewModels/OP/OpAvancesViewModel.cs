namespace MatrixNext.Web.ViewModels.OP;

public sealed class OpAvancesViewModel
{
    public DateTime LastUpdated { get; init; }
    public string FocusNote { get; init; } = string.Empty;
    public IReadOnlyCollection<OpFlowViewModel> Flows { get; init; } = Array.Empty<OpFlowViewModel>();
}
