using System;

namespace MatrixNext.Data.Adapters.PY.Models
{
    /// <summary>
    /// DTO para InHome Visit (visitas a domicilio cualitativos)
    /// Mapea a OP_MuestraTrabajosCuali_InHome
    /// </summary>
    public class InHomeVisitDto
    {
        public long Id { get; set; }
        public long TrabajoId { get; set; }
        public long? SegmentoId { get; set; }
        public int? CiudadId { get; set; }
        public long? Moderador { get; set; }
        public string? GrupoObjetivo { get; set; }
        public int? CantidadVisitas { get; set; }
        public string? Direccion { get; set; }
        public DateTime? FechaInicio { get; set; }
        public DateTime? FechaFin { get; set; }
        public decimal? Honorarios { get; set; }
        public decimal? Gastos { get; set; }
        public decimal? Otros { get; set; }
        public string? Observaciones { get; set; }

        // Campos extendidos del SP Get
        public string? CiudadNombre { get; set; }
        public string? ModeradorNombre { get; set; }
    }

    /// <summary>
    /// DTO para log de cambios InHome
    /// Mapea a OP_LogInHomeCuali
    /// </summary>
    public class LogInHomeDto
    {
        public long Id { get; set; }
        public long IdInHome { get; set; }
        public long? IdTrabajo { get; set; }
        public DateTime? Fecha { get; set; }
        public string? Usuario { get; set; }
        public string? Estado { get; set; }
        public string? Observacion { get; set; }
    }

    /// <summary>
    /// Input para crear/actualizar InHome
    /// </summary>
    public class InHomeVisitInputDto
    {
        public long? Id { get; set; }
        public long TrabajoId { get; set; }
        public long? SegmentoId { get; set; }
        public int? CiudadId { get; set; }
        public long? Moderador { get; set; }
        public string? GrupoObjetivo { get; set; }
        public int? CantidadVisitas { get; set; }
        public string? Direccion { get; set; }
        public string? FechaInicio { get; set; }
        public string? FechaFin { get; set; }
        public decimal? Honorarios { get; set; }
        public decimal? Gastos { get; set; }
        public decimal? Otros { get; set; }
        public string? Observaciones { get; set; }
    }
}
