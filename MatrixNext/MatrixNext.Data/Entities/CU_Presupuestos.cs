using System;
using System.Collections.Generic;

namespace MatrixNext.Data.Entities
{
    public class CU_Presupuestos
    {
        public long Id { get; set; }
        public long? PropuestaId { get; set; }
        public double? Valor { get; set; }
        public long? Muestra { get; set; }
        public int? ProductoId { get; set; }
        public double? GrossMargin { get; set; }
        public bool? UsadoPropuesta { get; set; }
        public long? Alternativa { get; set; }
        public string? JobBook { get; set; }
        public byte? EstadoId { get; set; }
        public string? Nombre { get; set; }
        public bool? Aprobado { get; set; }
        public bool? ParaRevisar { get; set; }
        public bool? Visible { get; set; }
        public bool? Nacional { get; set; }

        public virtual ICollection<CU_Estudios_Presupuestos> CU_Estudios_Presupuestos { get; set; }
            = new HashSet<CU_Estudios_Presupuestos>();

        public virtual CU_Propuestas? CU_Propuestas { get; set; }
    }
}
