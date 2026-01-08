using System.Collections.Immutable;
using MatrixNext.Web.Services.OP.Models;
using Microsoft.Extensions.Logging;

namespace MatrixNext.Web.Services.OP;

public class OpAvancesService : IOpAvancesService
{
    private readonly ILogger<OpAvancesService> _logger;

    private static readonly ImmutableArray<OpFlowStatus> _defaultFlows
        = ImmutableArray.Create(
            new OpFlowStatus(
                Title: "Portal COE y navegacion general",
                WebForms: "Trabajos.aspx / TrabajosCoordinador.aspx / TrabajosCallCenter.aspx / ConsultaTrabajos.aspx",
                CoreProjectDependencies: "TrabajoOPCuanti, PlaneacionProduccion, CoordinacionCampoPersonal, EnviarCorreo, GD",
                Status: "Pendiente",
                NextAction: "Diseñar controlador + Razor/Blazor principal enfocando navegacion y sesiones compartidas.",
                ReferenceDoc: "docs/OP/OP_CUANTITATIVO_AVANCE.md"),
            new OpFlowStatus(
                Title: "Tráfico de encuestas",
                WebForms: "TraficoEncuestas.aspx",
                CoreProjectDependencies: "TraficoEncuestas, CoordinacionCampo, OP_CuantiDapper (envío/recepción)",
                Status: "Pendiente",
                NextAction: "Mapear permisos 117-120 y crear vista de envío/recepción con exportes ClosedXML.",
                ReferenceDoc: "docs/OP/OP_CUANTITATIVO_AVANCE.md"),
            new OpFlowStatus(
                Title: "Carga masiva CATI",
                WebForms: "ImportarDatos.aspx",
                CoreProjectDependencies: "CatiRMC_* stored procedures, DbContexts OP_Cuanti/OP_Cuanti2, ExcelValidationService",
                Status: "Pendiente",
                NextAction: "Validar OpenXml + Blob storage, luego construir wizard y bulk copy orientado a Dapper/EF.",
                ReferenceDoc: "docs/OP/OP_CUANTITATIVO_AVANCE.md"),
            new OpFlowStatus(
                Title: "Carga de planillas de productividad",
                WebForms: "ImportarPlanillas.aspx",
                CoreProjectDependencies: "OP_CuantiDapper (planillas + producción), tabla _Festivos, NominaWindow helpers",
                Status: "Pendiente",
                NextAction: "Crear helper de ventana de nómina (16-15) y servicio de carga para SqlBulkCopy + validaciones.",
                ReferenceDoc: "docs/OP/OP_CUANTITATIVO_AVANCE.md"),
            new OpFlowStatus(
                Title: "Control IPS y observaciones",
                WebForms: "IPS.aspx",
                CoreProjectDependencies: "RevisionIPS, EjecucionIPS, IPSClass, ClosedXML export",
                Status: "Pendiente",
                NextAction: "Migrar grid editable y notificaciones con infraestructura de notificaciones compartida.",
                ReferenceDoc: "docs/OP/OP_CUANTITATIVO_AVANCE.md")
        );

    public OpAvancesService(ILogger<OpAvancesService> logger)
    {
        _logger = logger;
    }

    public Task<OpMigrationSnapshot> GetSnapshotAsync(CancellationToken cancellationToken = default)
    {
        _logger.LogDebug("Solicitando snapshot de avances para OP_Cuantitativo");

        var snapshot = new OpMigrationSnapshot(
            LastUpdated: DateTime.UtcNow,
            FocusNote: "Mantener las directrices de nomenclatura y reuso de CoreProject; cada SP debe invocarse sin cambiar nombre (DirectricesMigration.md, Regla 1-4).",
            Flows: _defaultFlows);

        return Task.FromResult(snapshot);
    }
}
