using System.Collections.Generic;
using MatrixNext.Web.Services.OP;

namespace MatrixNext.Web.ViewModels.OP;

public sealed class OpCargaWizardViewModel
{
    public OpCargaFormModel Form { get; set; } = new();

    public OpCargaResult? Result { get; set; }

    public IReadOnlyCollection<OpCargaStep> Steps { get; } = new[]
    {
        new OpCargaStep(
            Title: "Paso 1 — Tipo de carga",
            Description: "Elige si vas a cargar respuestas CATI (RespuestasCatiRMCtmp) o planillas de productividad y cantidades."),
        new OpCargaStep(
            Title: "Paso 2 — Archivo Excel",
            Description: "Sube un .xls ó .xlsx que respete los encabezados del sistema legacy para cada tipo."),
        new OpCargaStep(
            Title: "Paso 3 — Validaciones",
            Description: "Validamos la plantilla, el campo TipoActividad y los cortes/festivos sin usar OleDb."),
        new OpCargaStep(
            Title: "Paso 4 — Resumen",
            Description: "Mostramos cuántas filas cumplieron las reglas y guardamos una copia para auditoría.")
    };

    public IReadOnlyCollection<OpCargaTipoSummary> TypeSummaries { get; } = new[]
    {
        new OpCargaTipoSummary(
            Tipo: OpCargaTipo.CatiRMC,
            Title: "Carga CATI",
            Hint: "Requiere columnas CatiRMC como TrabajoId, Res_Numero, Per_NumIdentificacionEncu, TipoActividad."),
        new OpCargaTipoSummary(
            Tipo: OpCargaTipo.Planillas,
            Title: "Planillas de productividad",
            Hint: "Verifica cantidad, corte 16-15 y validaciones sobre festivos/domingos.")
    };

    public IReadOnlyList<string> CatiHeaders { get; } = new[]
    {
        "TrabajoId",
        "Res_Numero",
        "Per_NumIdentificacionEncu",
        "Per_NumIdentificacionSup",
        "Res_IDM",
        "Res_Ciudad",
        "Res_Fecha",
        "TipoSupervision",
        "TipoActividad"
    };

    public IReadOnlyList<string> PlanillaHeaders { get; } = new[]
    {
        "TrabajoId",
        "Per_NumIdentificacionEncu",
        "Res_Ciudad",
        "Res_Fecha",
        "TipoActividad",
        "Cantidad"
    };
}

public sealed record OpCargaStep(string Title, string Description);

public sealed record OpCargaTipoSummary(OpCargaTipo Tipo, string Title, string Hint);
