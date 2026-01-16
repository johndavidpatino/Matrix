using System;

namespace MatrixNext.Data.DTOs.RE_GT
{
    /// <summary>
    /// DTO para asignación de trabajo a coordinador de campo
    /// </summary>
    public class AsignacionCampoDto
    {
        public int IdTrabajo { get; set; }
        public int IdCOE { get; set; }
        public int? IdPersona { get; set; }
    }

    /// <summary>
    /// DTO con información del trabajo para asignación
    /// </summary>
    public class TrabajoAsignacionDto
    {
        public int IdTrabajo { get; set; }
        public string Propuesta { get; set; }
        public string Alternativa { get; set; }
        public string JobBook { get; set; }
        public string MetCodigo { get; set; }
        public int IdCOEActual { get; set; }
        public string COEActualNombre { get; set; }
        public string Estado { get; set; }
    }

    /// <summary>
    /// DTO con información de usuario COE
    /// </summary>
    public class UsuarioCOEDto
    {
        public int IdPersona { get; set; }
        public string Nombre { get; set; }
        public int IdCOE { get; set; }
        public string COENombre { get; set; }
    }

    /// <summary>
    /// DTO para log de cambios de asignación
    /// </summary>
    public class LogAsignacionCampoDto
    {
        public int IdTrabajo { get; set; }
        public int COEAnterior { get; set; }
        public int COENuevo { get; set; }
        public int? PersonaAnterior { get; set; }
        public int? PersonaNueva { get; set; }
        public int IdUsuario { get; set; }
        public DateTime FechaCambio { get; set; }
    }

    /// <summary>
    /// DTO para búsqueda en grid
    /// </summary>
    public class BusquedaAsignacionDto
    {
        public string NombrePropuesta { get; set; }
        public string JobBook { get; set; }
        public string MetCodigo { get; set; }
        public int PageIndex { get; set; } = 0;
        public int PageSize { get; set; } = 10;
    }
}
