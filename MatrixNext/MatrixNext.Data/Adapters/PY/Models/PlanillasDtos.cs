using System;

namespace MatrixNext.Data.Adapters.PY.Models
{
    /// <summary>
    /// DTO para Técnicas UU
    /// </summary>
    public class TecnicaDto
    {
        public int Id { get; set; }
        public string? Tecnica { get; set; }
        public int? Puntos { get; set; }
        public string? TipoTecnica { get; set; }
    }

    /// <summary>
    /// DTO para Moderadores UU
    /// </summary>
    public class ModeradorDto
    {
        public long Id { get; set; }
        public string? NombreModerador { get; set; }
    }

    /// <summary>
    /// DTO para Planilla Moderación
    /// </summary>
    public class PlanillaModeracionDto
    {
        public int Id { get; set; }
        public string? IdJob { get; set; }
        public string? JobDesc { get; set; }
        public DateTime? Fecha { get; set; }
        public string? Hora { get; set; }
        public int? Tecnica { get; set; }
        public string? Tiempo { get; set; }
        public long? Moderador { get; set; }
        public string? Rol { get; set; }
        public long? IdUsuarioRegistro { get; set; }
        public string? Observaciones { get; set; }
        public long? IdCuentasUU { get; set; }
        public string? BI_WBSL { get; set; }
        public short? IdEstado { get; set; }
        public string? ObservacionesAprobacion { get; set; }
        public string? BI_3320_Moderacion_DineroDisponible { get; set; }
        public string? BI_Status { get; set; }
        public long? IdUsuarioAprueba { get; set; }
        public DateTime? FechaAprobacion { get; set; }

        // Campos extendidos
        public string? TecnicaNombre { get; set; }
        public string? ModeradorNombre { get; set; }
        public string? EstadoNombre { get; set; }
    }

    /// <summary>
    /// DTO para Planilla Informes
    /// </summary>
    public class PlanillaInformesDto
    {
        public int Id { get; set; }
        public string? IdJob { get; set; }
        public string? JobDesc { get; set; }
        public DateTime? Fecha { get; set; }
        public int? Tecnica { get; set; }
        public string? Muestra { get; set; }
        public long? IdCuentasUU { get; set; }
        public long? Analista { get; set; }
        public string? Observaciones { get; set; }
        public long? IdUsuarioRegistro { get; set; }
        public string? ServiceLineName { get; set; }
        public short? IdEstado { get; set; }
        public string? ObservacionesAprobacion { get; set; }
        public string? BI_3320_Moderacion_DineroDisponible { get; set; }
        public string? BI_Status { get; set; }
        public long? IdUsuarioAprueba { get; set; }
        public DateTime? FechaAprobacion { get; set; }

        // Campos extendidos
        public string? TecnicaNombre { get; set; }
        public string? AnalistaNombre { get; set; }
        public string? EstadoNombre { get; set; }
    }

    /// <summary>
    /// DTO para lista paginada de planillas
    /// </summary>
    public class PlanillaListDto
    {
        public int Id { get; set; }
        public string? IdJob { get; set; }
        public string? JobDesc { get; set; }
        public string? TipoPlanilla { get; set; }
        public DateTime? FechaRegistro { get; set; }
        public string? UsuarioRegistro { get; set; }
        public short? IdEstado { get; set; }
        public string? EstadoNombre { get; set; }
    }

    /// <summary>
    /// Input para crear planilla moderación
    /// </summary>
    public class PlanillaModeracionInputDto
    {
        public string IdJob { get; set; } = string.Empty;
        public string JobDesc { get; set; } = string.Empty;
        public DateTime Fecha { get; set; }
        public string Hora { get; set; } = string.Empty;
        public int Tecnica { get; set; }
        public string Tiempo { get; set; } = string.Empty;
        public long Moderador { get; set; }
        public string Rol { get; set; } = string.Empty;
        public string? Observaciones { get; set; }
        public long IdCuentasUU { get; set; }
        public string? ServiceLineName { get; set; }
        public long IdUsuarioRegistro { get; set; }
    }

    /// <summary>
    /// Input para crear planilla informes
    /// </summary>
    public class PlanillaInformesInputDto
    {
        public string IdJob { get; set; } = string.Empty;
        public string JobDesc { get; set; } = string.Empty;
        public DateTime Fecha { get; set; }
        public int Tecnica { get; set; }
        public string Muestra { get; set; } = string.Empty;
        public long IdCuentasUU { get; set; }
        public long Analista { get; set; }
        public string? Observaciones { get; set; }
        public string? ServiceLineName { get; set; }
        public long IdUsuarioRegistro { get; set; }
    }

    /// <summary>
    /// Input para actualizar estado planilla
    /// </summary>
    public class ActualizarEstadoPlanillaInputDto
    {
        public int IdPlanilla { get; set; }
        public short IdEstado { get; set; }
        public string? Observaciones { get; set; }
        public string? BiDinero { get; set; }
        public string? BiStatus { get; set; }
        public long IdUsuarioAprueba { get; set; }
        public bool JobEncontradoEnBI { get; set; }
    }
}
