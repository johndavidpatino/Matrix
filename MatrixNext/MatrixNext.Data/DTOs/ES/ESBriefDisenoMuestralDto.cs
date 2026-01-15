using System;
using System.ComponentModel.DataAnnotations;

namespace MatrixNext.Data.DTOs.ES
{
    /// <summary>
    /// DTO para Input de creación/edición de Brief de Diseño Muestral
    /// </summary>
    public class ESBriefDisenoMuestralInputDto
    {
        [Required(ErrorMessage = "La propuesta es requerida")]
        public long PropuestaId { get; set; }

        [Required(ErrorMessage = "El objetivo es requerido")]
        [StringLength(4000)]
        public string Objetivo { get; set; }

        [StringLength(4000)]
        public string Poblacion { get; set; }

        [StringLength(4000)]
        public string Capacidad { get; set; }

        [StringLength(4000)]
        public string Metodologia { get; set; }

        [StringLength(4000)]
        public string NivelesDesagregacion { get; set; }

        [StringLength(4000)]
        public string PosiblesMarcos { get; set; }

        [StringLength(4000)]
        public string Variable { get; set; }

        [StringLength(4000)]
        public string Observaciones { get; set; }
    }

    /// <summary>
    /// DTO de salida para listado y consulta de Brief de Diseño Muestral
    /// Tabla: ES_BriefDisenoMuestral
    /// </summary>
    public class ESBriefDisenoMuestralOutputDto
    {
        public long Id { get; set; }
        public long IdPropuesta { get; set; }
        public long PropuestaId { get; set; }
        public DateTime Fecha { get; set; }
        public string Objetivo { get; set; }
        public string Poblacion { get; set; }
        public string Capacidad { get; set; }
        public string Metodologia { get; set; }
        public string NivelesDesagregacion { get; set; }
        public string PosiblesMarcos { get; set; }
        public string Variable { get; set; }
        public string Observaciones { get; set; }
        public int NoVersion { get; set; }
        public bool Aprobado { get; set; }
        public long? UsuarioGenera { get; set; }
        public DateTime? FechaGenera { get; set; }
        public long? UsuarioAprobacion { get; set; }
        public DateTime? FechaAprobacion { get; set; }

        // Propiedades de navegación
        public string PropuestaNombre { get; set; }
        public string ClienteNombre { get; set; }
    }
}
