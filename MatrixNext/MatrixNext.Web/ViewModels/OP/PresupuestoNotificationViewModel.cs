namespace MatrixNext.Web.ViewModels.OP;

public sealed record PresupuestoNotificationRow(long TrabajoId, string Tipo, string Usuario, DateTime Fecha, string Observacion);

public sealed record ProduccionSummary(int TotalRegistros, int RegistrosHoy, DateTime? UltimaActualizacion);
