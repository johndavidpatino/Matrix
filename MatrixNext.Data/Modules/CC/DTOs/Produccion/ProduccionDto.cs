namespace MatrixNext.Data.Modules.CC.DTOs.Produccion
{
    /// <summary>
    /// DTO para registro de producción
    /// </summary>
    public class RegistroProduccionDto
    {
        public long IdProduccion { get; set; }
        public int Periodo { get; set; }
        public long IdTrabajo { get; set; }
        public string CodigoTrabajo { get; set; }
        public string NombreTrabajo { get; set; }
        public long IdActividad { get; set; }
        public string CodigoActividad { get; set; }
        public string DescripcionActividad { get; set; }
        public decimal Cantidad { get; set; }
        public decimal CostoUnitario { get; set; }
        public decimal CostoTotal => Cantidad * CostoUnitario;
        public long IdEmpleado { get; set; }
        public string NombreEmpleado { get; set; }
        public DateTime FechaProduccion { get; set; }
        public byte Estado { get; set; }
        public string Observaciones { get; set; }
        public DateTime FechaRegistro { get; set; }
    }

    /// <summary>
    /// DTO para liquidación de planillas por actividades
    /// </summary>
    public class LiquidacionPlanillaDto
    {
        public long IdLiquidacion { get; set; }
        public int Periodo { get; set; }
        public long IdTrabajo { get; set; }
        public string CodigoTrabajo { get; set; }
        public string NombreTrabajo { get; set; }
        public long IdEmpleado { get; set; }
        public string NombreEmpleado { get; set; }
        public decimal SalarioBase { get; set; }
        public decimal ProduccionGenerada { get; set; }
        public decimal BonoProduccion { get; set; }
        public decimal DescuentosSS { get; set; }
        public decimal ValorNeto { get; set; }
        public byte Estado { get; set; }
        public DateTime FechaLiquidacion { get; set; }
        public string Observaciones { get; set; }
    }

    /// <summary>
    /// DTO para generación de bonificación
    /// </summary>
    public class GenerarBonificacionDto
    {
        public long IdBonificacion { get; set; }
        public int Periodo { get; set; }
        public long IdEmpleado { get; set; }
        public string NombreEmpleado { get; set; }
        public long IdTrabajo { get; set; }
        public string NombreTrabajo { get; set; }
        public decimal SalarioBase { get; set; }
        public decimal ProduccionTotal { get; set; }
        public decimal PercentajeMetaBonificacion { get; set; }
        public decimal BonoCalculado { get; set; }
        public decimal BonoFinal { get; set; }
        public byte Estado { get; set; }
        public DateTime FechaGeneracion { get; set; }
        public string Observaciones { get; set; }
    }

    /// <summary>
    /// DTO para carga de descuentos de seguridad social
    /// </summary>
    public class CargueDescuentoSSDto
    {
        public long IdDescuento { get; set; }
        public int Periodo { get; set; }
        public long IdEmpleado { get; set; }
        public string NombreEmpleado { get; set; }
        public string TipoDescuento { get; set; }
        public decimal ValorDescuento { get; set; }
        public decimal PercentajeDescuento { get; set; }
        public byte Estado { get; set; }
        public DateTime FechaCarga { get; set; }
        public string Observaciones { get; set; }
    }

    /// <summary>
    /// DTO para liquidación de productividad PST
    /// </summary>
    public class LiquidacionProductividadPstDto
    {
        public long IdLiquidacionPST { get; set; }
        public int Periodo { get; set; }
        public long IdTrabajo { get; set; }
        public string CodigoTrabajo { get; set; }
        public string NombreTrabajo { get; set; }
        public long IdEmpleado { get; set; }
        public string NombreEmpleado { get; set; }
        public decimal ValorPST { get; set; }
        public decimal ProduccionGenerada { get; set; }
        public decimal PercentajeLiquidacion { get; set; }
        public decimal ValorLiquidado { get; set; }
        public byte Estado { get; set; }
        public DateTime FechaLiquidacion { get; set; }
        public string Observaciones { get; set; }
    }

    /// <summary>
    /// DTO para asignación de costos a PST
    /// </summary>
    public class AsignacionCostosPstDto
    {
        public long IdAsignacion { get; set; }
        public int Periodo { get; set; }
        public long IdTrabajo { get; set; }
        public string CodigoTrabajo { get; set; }
        public string NombreTrabajo { get; set; }
        public long IdConcepto { get; set; }
        public string NombreConcepto { get; set; }
        public decimal CostoBase { get; set; }
        public decimal CostoAsignado { get; set; }
        public byte Estado { get; set; }
        public DateTime FechaAsignacion { get; set; }
        public string Observaciones { get; set; }
    }

    /// <summary>
    /// DTO para cambio de estado de jobbooks
    /// </summary>
    public class EstadoJobBookDto
    {
        public long IdJobBook { get; set; }
        public long IdTrabajo { get; set; }
        public string CodigoTrabajo { get; set; }
        public string NombreTrabajo { get; set; }
        public string NumeroJobBook { get; set; }
        public byte EstadoActual { get; set; }
        public string EstadoActualNombre { get; set; }
        public DateTime FechaApertura { get; set; }
        public DateTime? FechaCierre { get; set; }
        public decimal MontoTotal { get; set; }
        public string Observaciones { get; set; }
    }

    /// <summary>
    /// DTO para revisión de generación de bonificación
    /// </summary>
    public class RevisarGeneracionBonificacionDto
    {
        public long IdBonificacion { get; set; }
        public int Periodo { get; set; }
        public long IdEmpleado { get; set; }
        public string NombreEmpleado { get; set; }
        public decimal SalarioBase { get; set; }
        public decimal ProduccionTotal { get; set; }
        public decimal BonoCalculado { get; set; }
        public decimal BonoFinal { get; set; }
        public byte Estado { get; set; }
        public DateTime FechaGeneracion { get; set; }
        public string UsuarioGeneracion { get; set; }
        public string UsuarioRevision { get; set; }
        public DateTime? FechaRevision { get; set; }
        public bool Aprobada { get; set; }
    }

    /// <summary>
    /// DTO para anulación de liquidaciones
    /// </summary>
    public class AnulacionLiquidacionesDto
    {
        public long IdLiquidacion { get; set; }
        public int Periodo { get; set; }
        public long IdEmpleado { get; set; }
        public string NombreEmpleado { get; set; }
        public long IdTrabajo { get; set; }
        public string NombreTrabajo { get; set; }
        public decimal ValorLiquidado { get; set; }
        public byte EstadoActual { get; set; }
        public string EstadoActualNombre { get; set; }
        public DateTime FechaLiquidacion { get; set; }
        public string Motivoanulacion { get; set; }
        public DateTime FechaAnulacion { get; set; }
        public string UsuarioAnulacion { get; set; }
    }

    /// <summary>
    /// Filtros para búsqueda de registros de producción
    /// </summary>
    public class FiltrosRegistroProduccionDto
    {
        public int? Periodo { get; set; }
        public long? IdTrabajo { get; set; }
        public long? IdEmpleado { get; set; }
        public long? IdActividad { get; set; }
        public DateTime? FechaInicio { get; set; }
        public DateTime? FechaFin { get; set; }
        public byte? Estado { get; set; }
    }

    /// <summary>
    /// Filtros para búsqueda de liquidaciones de planillas
    /// </summary>
    public class FiltrosLiquidacionPlanillaDto
    {
        public int? Periodo { get; set; }
        public long? IdTrabajo { get; set; }
        public long? IdEmpleado { get; set; }
        public byte? Estado { get; set; }
    }

    /// <summary>
    /// Filtros para búsqueda de bonificaciones
    /// </summary>
    public class FiltrosGenerarBonificacionDto
    {
        public int? Periodo { get; set; }
        public long? IdTrabajo { get; set; }
        public long? IdEmpleado { get; set; }
        public byte? Estado { get; set; }
    }

    /// <summary>
    /// Filtros para búsqueda de descuentos SS
    /// </summary>
    public class FiltrosCargueDescuentoSSDto
    {
        public int? Periodo { get; set; }
        public long? IdEmpleado { get; set; }
        public string TipoDescuento { get; set; }
        public byte? Estado { get; set; }
    }

    /// <summary>
    /// Filtros para búsqueda de liquidaciones PST
    /// </summary>
    public class FiltrosLiquidacionProductividadPstDto
    {
        public int? Periodo { get; set; }
        public long? IdTrabajo { get; set; }
        public long? IdEmpleado { get; set; }
        public byte? Estado { get; set; }
    }

    /// <summary>
    /// Filtros para búsqueda de asignaciones de costos PST
    /// </summary>
    public class FiltrosAsignacionCostosPstDto
    {
        public int? Periodo { get; set; }
        public long? IdTrabajo { get; set; }
        public long? IdConcepto { get; set; }
        public byte? Estado { get; set; }
    }

    /// <summary>
    /// Filtros para búsqueda de jobbooks
    /// </summary>
    public class FiltrosEstadoJobBookDto
    {
        public long? IdTrabajo { get; set; }
        public byte? EstadoActual { get; set; }
    }

    /// <summary>
    /// Filtros para búsqueda de bonificaciones en revisión
    /// </summary>
    public class FiltrosRevisarGeneracionBonificacionDto
    {
        public int? Periodo { get; set; }
        public long? IdEmpleado { get; set; }
        public long? IdTrabajo { get; set; }
        public bool? Aprobada { get; set; }
    }

    /// <summary>
    /// Filtros para búsqueda de anulaciones
    /// </summary>
    public class FiltrosAnulacionLiquidacionesDto
    {
        public int? Periodo { get; set; }
        public long? IdEmpleado { get; set; }
        public long? IdTrabajo { get; set; }
        public DateTime? FechaInicio { get; set; }
        public DateTime? FechaFin { get; set; }
    }
}
