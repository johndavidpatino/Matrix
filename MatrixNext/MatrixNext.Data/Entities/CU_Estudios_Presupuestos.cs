using System.ComponentModel.DataAnnotations.Schema;

namespace MatrixNext.Data.Entities
{
    [Table("CU_Estudios_Presupuestos")]
    public class CU_Estudios_Presupuestos
    {
        [Column("EstudioId")]
        public long EstudioId { get; set; }

        [Column("PresupuestoId")]
        public long PresupuestoId { get; set; }
    }
}
