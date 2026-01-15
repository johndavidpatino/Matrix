using System;
using System.ComponentModel.DataAnnotations;

namespace MatrixNext.Data.DTOs.ES
{
    /// <summary>
    /// DTO para Input de creación/edición de Diseño Muestral
    /// </summary>
    public class ESDisenoMuestralInputDto
    {
        [Required(ErrorMessage = "El brief es requerido")]
        public long BriefId { get; set; }

        public bool MuestroProbabilistico { get; set; }

        // Campos de selección
        public bool Objetivo { get; set; }
        public bool Poblacion { get; set; }
        public bool Mercado { get; set; }
        public bool Marco { get; set; }
        public bool Tecnica { get; set; }
        public bool Diseno { get; set; }
        public bool Tamano { get; set; }
        public bool Fiabilidad { get; set; }
        public bool Desagregacion { get; set; }
        public bool Fuente { get; set; }
        public bool Ponderacion { get; set; }
        public bool Variable { get; set; }

        // Campos de texto
        [StringLength(4000)]
        public string ObjetivoT { get; set; }

        [StringLength(4000)]
        public string PoblacionT { get; set; }

        [StringLength(4000)]
        public string MercadoT { get; set; }

        [StringLength(4000)]
        public string MarcoT { get; set; }

        [StringLength(4000)]
        public string TecnicaT { get; set; }

        [StringLength(4000)]
        public string DisenoT { get; set; }

        [StringLength(4000)]
        public string TamanoT { get; set; }

        [StringLength(4000)]
        public string FiabilidadT { get; set; }

        [StringLength(4000)]
        public string DesagregacionT { get; set; }

        [StringLength(4000)]
        public string FuenteT { get; set; }

        [StringLength(4000)]
        public string PonderacionT { get; set; }

        [StringLength(4000)]
        public string VariableT { get; set; }

        [StringLength(4000)]
        public string Observaciones { get; set; }

        [StringLength(4000)]
        public string ObservacionesT { get; set; }
    }

    /// <summary>
    /// DTO de salida para listado y consulta de Diseño Muestral
    /// Tabla: ES_DisenoMuestral
    /// </summary>
    public class ESDisenoMuestralOutputDto
    {
        public long Id { get; set; }
        public long BriefId { get; set; }
        public DateTime Fecha { get; set; }
        public bool MuestroProbabilistico { get; set; }
        
        // Campos de selección
        public bool Objetivo { get; set; }
        public bool Poblacion { get; set; }
        public bool Mercado { get; set; }
        public bool Marco { get; set; }
        public bool Tecnica { get; set; }
        public bool Diseno { get; set; }
        public bool Tamano { get; set; }
        public bool Fiabilidad { get; set; }
        public bool Desagregacion { get; set; }
        public bool Fuente { get; set; }
        public bool Ponderacion { get; set; }
        public bool Variable { get; set; }

        // Campos de texto
        public string ObjetivoT { get; set; }
        public string PoblacionT { get; set; }
        public string MercadoT { get; set; }
        public string MarcoT { get; set; }
        public string TecnicaT { get; set; }
        public string DisenoT { get; set; }
        public string TamanoT { get; set; }
        public string FiabilidadT { get; set; }
        public string DesagregacionT { get; set; }
        public string FuenteT { get; set; }
        public string PonderacionT { get; set; }
        public string VariableT { get; set; }
        public string Observaciones { get; set; }
        public string ObservacionesT { get; set; }
        public int NumVersion { get; set; }
        public int NoVersion { get; set; }

        // Propiedades de navegación
        public string BriefObjetivo { get; set; }
        public string PropuestaNombre { get; set; }
    }
}
