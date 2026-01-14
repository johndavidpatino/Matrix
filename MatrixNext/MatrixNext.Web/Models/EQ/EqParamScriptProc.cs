using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MatrixNext.Web.Models.EQ
{
    /// <summary>
    /// Horas estimadas de scripting, procesamiento, harmoni y graficacion por duracion
    /// Mapea desde Excel Parametros: horas script/proc por duracion 5-60 min
    /// </summary>
    [Table("eq_param_script_proc")]
    public class EqParamScriptProc
    {
        [Key]
        public int Id { get; set; }

        [Range(5, 60)]
        public int DuracionMin { get; set; }

        public decimal HorasScript { get; set; }

        public decimal HorasProc { get; set; }

        public decimal HorasHarmoni { get; set; }

        public decimal HorasGraficacion { get; set; }

        public DateTime FechaCreacion { get; set; } = DateTime.UtcNow;

        public DateTime FechaModificacion { get; set; } = DateTime.UtcNow;
    }
}

