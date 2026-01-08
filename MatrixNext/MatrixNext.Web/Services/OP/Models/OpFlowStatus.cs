namespace MatrixNext.Web.Services.OP.Models;

public sealed record OpFlowStatus(
    string Title,
    string WebForms,
    string CoreProjectDependencies,
    string Status,
    string NextAction,
    string ReferenceDoc);
