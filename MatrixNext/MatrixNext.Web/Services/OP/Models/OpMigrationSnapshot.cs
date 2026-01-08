using System.Collections.Immutable;

namespace MatrixNext.Web.Services.OP.Models;

public sealed record OpMigrationSnapshot(
    DateTime LastUpdated,
    string FocusNote,
    ImmutableArray<OpFlowStatus> Flows);
