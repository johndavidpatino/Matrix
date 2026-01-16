// MatrixNext.Core/DTOs/CORE/TrabajoTraficoInfoDto.cs

namespace MatrixNext.Core.DTOs.CORE
{
    /// <summary>
    /// DTO para información detallada de un trabajo en TraficoTareas
    /// Sprint 17 - RE_GT TraficoTareas
    /// </summary>
    public class TrabajoTraficoInfoDto
    {
        /// <summary>ID del trabajo</summary>
        public long IdTrabajo { get; set; }

        /// <summary>Nombre/descripción del trabajo</summary>
        public string NombreTrabajo { get; set; } = string.Empty;

        /// <summary>JobBook del trabajo</summary>
        public string JobBook { get; set; } = string.Empty;

        /// <summary>¿Es proyecto cualitativo?</summary>
        public bool EsProyectoCualitativo { get; set; }

        /// <summary>ID del proyecto</summary>
        public int IdProyecto { get; set; }

        /// <summary>ID de unidad OP</summary>
        public int IdUnidad { get; set; }
    }
}
