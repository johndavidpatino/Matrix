using MatrixNext.Web.ViewModels.OP;

namespace MatrixNext.Web.Services.OP;

public class OpPlanillasService : IOpPlanillasService
{
    public Task<PlanillasAprobacionViewModel> ObtenerPlanillasAsync(CancellationToken cancellationToken = default)
    {
        var model = new PlanillasAprobacionViewModel
        {
            StatusTabs = new[]
            {
                new PlanillaStatusViewModel
                {
                    Title = "Pendientes",
                    Count = 18,
                    Description = "Planillas cargadas que necesitan revisión del COE",
                    Badge = "warning",
                    TabId = "pendientes"
                },
                new PlanillaStatusViewModel
                {
                    Title = "En revisión",
                    Count = 7,
                    Description = "Planillas en revisión de Coordinadores",
                    Badge = "primary",
                    TabId = "revision"
                },
                new PlanillaStatusViewModel
                {
                    Title = "Aprobadas",
                    Count = 34,
                    Description = "Planillas aprobadas y en producción",
                    Badge = "success",
                    TabId = "aprobadas"
                }
            },
            Planillas = new[]
            {
                new PlanillaRowViewModel
                {
                    TrabajoId = 4231,
                    TrabajoNombre = "Trabajo 4231 - Encuestas Nacionales",
                    Responsable = "COE 100",
                    Estado = "Pendiente",
                    FechaCarga = DateTime.UtcNow.AddDays(-1),
                    Cantidad = 125,
                    Observaciones = "Esperando validación PMO"
                },
                new PlanillaRowViewModel
                {
                    TrabajoId = 4192,
                    TrabajoNombre = "Trabajo 4192 - Panel Urbano",
                    Responsable = "Coordinador 101",
                    Estado = "Rechazada",
                    FechaCarga = DateTime.UtcNow.AddDays(-2),
                    Cantidad = 84,
                    Observaciones = "Duplicados detectados"
                }
            },
            Productivity = new ProductivitySummaryViewModel
            {
                Corte = "Corte 16-15",
                Nota = "Rol PMO revisando diferencias de cantidades",
                TotalAprobadas = 92,
                TotalPendientes = 14,
                Rol = "PMO"
            },
            Ips = new IpsSummaryViewModel
            {
                Pendientes = 11,
                Atendidas = 27,
                UltimaActualizacion = DateTime.UtcNow.ToString("g"),
                Comentario = "Pendientes por revisión de auditoría"
            }
        };

        return Task.FromResult(model);
    }
}
