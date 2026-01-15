using System.ComponentModel.DataAnnotations;

namespace MatrixNext.Data.DTOs.SGC
{
    /// <summary>
    /// DTO para Hallazgo en Auditoría
    /// Mapea desde SGC_AI_HallazgoResult
    /// </summary>
    public class SGCHallazgoDto
    {
        public int Id { get; set; }
        public int SGC_AI_AuditoriaId { get; set; }
        
        [Required(ErrorMessage = "La descripción del hallazgo es requerida")]
        [StringLength(1000, ErrorMessage = "Máximo 1000 caracteres")]
        public string Hallazgo { get; set; }

        /// <summary>
        /// Tipo de hallazgo: 1=Observación, 2=No Conformidad Mayor, 3=No Conformidad Menor
        /// </summary>
        [Required(ErrorMessage = "El tipo de hallazgo es requerido")]
        public byte SGC_AI_TipoHallazgoId { get; set; }

        public string TipoHallazgo { get; set; }
    }

    /// <summary>
    /// DTO para crear/editar hallazgo
    /// </summary>
    public class SGCHallazgoCreateDto
    {
        [Required(ErrorMessage = "La descripción del hallazgo es requerida")]
        [StringLength(1000, ErrorMessage = "Máximo 1000 caracteres")]
        public string Hallazgo { get; set; }

        [Required(ErrorMessage = "El tipo de hallazgo es requerido")]
        public byte TipoHallazgoId { get; set; }
    }
}
