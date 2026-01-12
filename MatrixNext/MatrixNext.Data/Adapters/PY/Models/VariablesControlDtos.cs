using System;

namespace MatrixNext.Data.Adapters.PY.Models
{
    /// <summary>
    /// DTO para Variables de Control por trabajo
    /// Mapea a PY_Variables_Control
    /// </summary>
    public class VariableControlDto
    {
        public long Id { get; set; }
        public long TrabajoId { get; set; }
        public string? Modalidad { get; set; }
        public string? VariableControl { get; set; }
    }

    /// <summary>
    /// Input para guardar variable de control
    /// </summary>
    public class VariableControlInputDto
    {
        public long? Id { get; set; }
        public long TrabajoId { get; set; }
        public string Modalidad { get; set; } = string.Empty;
        public string VariableControl { get; set; } = string.Empty;
    }
}
