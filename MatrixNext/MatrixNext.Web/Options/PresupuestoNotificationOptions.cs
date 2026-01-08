namespace MatrixNext.Web.Options;

public sealed class PresupuestoNotificationOptions
{
    public string[] Recipients { get; set; } = Array.Empty<string>();
    public string SubjectTemplate { get; set; } = "Solicitud de presupuesto {Tipo} registrada - Trabajo {TrabajoId}";
    public string BodyTemplate { get; set; } =
        "Se registró una solicitud de presupuesto {Tipo} para el trabajo {TrabajoId}.\r\n" +
        "Hora UTC: {FechaUtc}\r\n" +
        "Observación: {Observacion}";
}
