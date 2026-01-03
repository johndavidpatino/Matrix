using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace MatrixNext.Data.Entities
{
    [Table("CU_SeguimientoPropuestas")]
    public class CU_SeguimientoPropuestas
    {
        [Column("Id")]
        public long Id { get; set; }

        [Column("PropuestaId")]
        public long PropuestaId { get; set; }

        [Column("Fecha")]
        public DateTime Fecha { get; set; }

        [Column("Observacion")]
        public string? Observacion { get; set; }

        [Column("UsuarioId")]
        public long UsuarioId { get; set; }
    }
}
