using System;

namespace MatrixNext.Data.Adapters.PY.Models
{
    /// <summary>
    /// DTO para Entrevistas Cualitativos
    /// Mapea a OP_MuestraTrabajosCuali_Entrevistas
    /// </summary>
    public class EntrevistaCualiDto
    {
        public long Id { get; set; }
        public long? TrabajoId { get; set; }
        public string? GrupoObjetivo { get; set; }
        public int? CiudadId { get; set; }
        public int? Cantidad { get; set; }
        public DateTime? FechaInicio { get; set; }
        public DateTime? FechaFin { get; set; }

        // Campos extendidos del SP Get
        public string? CiudadNombre { get; set; }
        public string? JobBook { get; set; }
        public string? NombreTrabajo { get; set; }
    }

    /// <summary>
    /// DTO para Distribución de Entrevistas
    /// Mapea a OP_EntrevistasCuali_Distribucion
    /// </summary>
    public class DistribucionEntrevistaDto
    {
        public long Id { get; set; }
        public long IdEntrevista { get; set; }
        public int Numero { get; set; }
        public long? TrabajoId { get; set; }
        public string? GrupoObjetivo { get; set; }
        public int? CiudadId { get; set; }
        public string? Cantidad { get; set; }
        public DateTime? FechaInicio { get; set; }
        public DateTime? FechaFin { get; set; }
        public long? Moderador { get; set; }
        public short? IdEstado { get; set; }

        // Campos extendidos del SP Get
        public string? CiudadNombre { get; set; }
        public string? ModeradorNombre { get; set; }
        public string? DepartamentoId { get; set; }
        public string? Estado { get; set; }
    }

    /// <summary>
    /// DTO para log de entrevistas
    /// Mapea a OP_LogEntrevistasCuali
    /// </summary>
    public class LogEntrevistaCualiDto
    {
        public long Id { get; set; }
        public long? IdDistribucion { get; set; }
        public long? IdEntrevista { get; set; }
        public DateTime? Fecha { get; set; }
        public string? Usuario { get; set; }
        public string? Estado { get; set; }
        public string? Observacion { get; set; }
    }

    /// <summary>
    /// DTO para moderadores cualitativos
    /// </summary>
    public class ModeradorCualiDto
    {
        public long Id { get; set; }
        public string? Nombre { get; set; }
    }

    /// <summary>
    /// Input para crear distribución de entrevista
    /// </summary>
    public class DistribucionEntrevistaInputDto
    {
        public long? Id { get; set; }
        public long IdEntrevista { get; set; }
        public int Numero { get; set; }
        public long TrabajoId { get; set; }
        public string GrupoObjetivo { get; set; } = string.Empty;
        public int CiudadId { get; set; }
        public string Cantidad { get; set; } = "1";
        public string FechaInicio { get; set; } = string.Empty;
        public string FechaFin { get; set; } = string.Empty;
        public long Moderador { get; set; }
        public long UsuarioId { get; set; }
    }
}
