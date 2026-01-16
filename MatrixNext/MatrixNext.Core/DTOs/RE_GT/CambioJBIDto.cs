using System;

namespace MatrixNext.Core.DTOs.RE_GT
{
    /// <summary>
    /// DTO para cambio de JobBook Interno
    /// </summary>
    public class CambioJBIDto
    {
        public int IdTrabajo { get; set; }
        public int IdFase { get; set; }
        public string NuevoJBI { get; set; }
    }

    /// <summary>
    /// DTO para resultado de obtención de fases
    /// </summary>
    public class FaseDto
    {
        public int IdFase { get; set; }
        public string DescFase { get; set; }
    }

    /// <summary>
    /// DTO para información de trabajo (validación)
    /// </summary>
    public class TrabajoInfoDto
    {
        public int IdTrabajo { get; set; }
        public int IdPropuesta { get; set; }
        public int Alternativa { get; set; }
        public string JobBook { get; set; }
        public string MetCodigo { get; set; }
    }

    /// <summary>
    /// DTO para log de cambios JBI
    /// </summary>
    public class LogCambioJBIDto
    {
        public int IdTrabajo { get; set; }
        public string JBIAnterior { get; set; }
        public string JBINuevo { get; set; }
        public int IdUsuario { get; set; }
        public DateTime FechaCambio { get; set; }
    }
}
