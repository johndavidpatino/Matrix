using System;
using System.ComponentModel.DataAnnotations;

namespace MatrixNext.Data.DTOs.ES
{
    /// <summary>
    /// DTO para Input de creación/edición de Metodología de Campo
    /// </summary>
    public class ESMetodologiaCampoInputDto
    {
        [Required(ErrorMessage = "El trabajo es requerido")]
        public long TrabajoId { get; set; }

        [Required(ErrorMessage = "El nombre del estudio es requerido")]
        [StringLength(500)]
        public string NombreEstudio { get; set; }

        // Campos de selección
        public bool Objetivo { get; set; }
        public bool Mercado { get; set; }
        public bool Marco { get; set; }
        public bool Tecnica { get; set; }
        public bool Diseno { get; set; }
        public bool Instrucciones { get; set; }
        public bool Distribucion { get; set; }
        public bool NivelConfianza { get; set; }
        public bool MargenError { get; set; }
        public bool Desagregacion { get; set; }
        public bool Fuente { get; set; }
        public bool Variables { get; set; }
        public bool Tasa { get; set; }
        public bool Procedimiento { get; set; }

        // Campos de texto
        [StringLength(4000)]
        public string ObjetivoT { get; set; }

        [StringLength(4000)]
        public string MercadoT { get; set; }

        [StringLength(4000)]
        public string MarcoT { get; set; }

        [StringLength(4000)]
        public string TecnicaT { get; set; }

        [StringLength(4000)]
        public string DisenoT { get; set; }

        [StringLength(4000)]
        public string InstruccionesT { get; set; }

        [StringLength(4000)]
        public string DistribucionT { get; set; }

        [StringLength(4000)]
        public string NivelConfianzaT { get; set; }

        [StringLength(4000)]
        public string MargenErrorT { get; set; }

        [StringLength(4000)]
        public string DesagregacionT { get; set; }

        [StringLength(4000)]
        public string FuenteT { get; set; }

        [StringLength(4000)]
        public string VariablesT { get; set; }

        [StringLength(4000)]
        public string TasaT { get; set; }

        [StringLength(4000)]
        public string ProcedimientoT { get; set; }
    }

    /// <summary>
    /// DTO de salida para listado y consulta de Metodología de Campo
    /// Tabla: ES_MetodologiaCampo
    /// </summary>
    public class ESMetodologiaCampoOutputDto
    {
        public long Id { get; set; }
        public long TrabajoId { get; set; }
        public string NombreEstudio { get; set; }
        public DateTime Fecha { get; set; }

        // Campos de selección
        public bool Objetivo { get; set; }
        public bool Mercado { get; set; }
        public bool Marco { get; set; }
        public bool Tecnica { get; set; }
        public bool Diseno { get; set; }
        public bool Instrucciones { get; set; }
        public bool Distribucion { get; set; }
        public bool NivelConfianza { get; set; }
        public bool MargenError { get; set; }
        public bool Desagregacion { get; set; }
        public bool Fuente { get; set; }
        public bool Variables { get; set; }
        public bool Tasa { get; set; }
        public bool Procedimiento { get; set; }

        // Campos de texto
        public string ObjetivoT { get; set; }
        public string MercadoT { get; set; }
        public string MarcoT { get; set; }
        public string TecnicaT { get; set; }
        public string DisenoT { get; set; }
        public string InstruccionesT { get; set; }
        public string DistribucionT { get; set; }
        public string NivelConfianzaT { get; set; }
        public string MargenErrorT { get; set; }
        public string DesagregacionT { get; set; }
        public string FuenteT { get; set; }
        public string VariablesT { get; set; }
        public string TasaT { get; set; }
        public string ProcedimientoT { get; set; }
        public byte NoVersion { get; set; }
        public byte NumVersion { get; set; }
        public long Usuario { get; set; }
        public bool Aprobado { get; set; }

        // Propiedades de navegación
        public string TrabajoNombre { get; set; }
        public string UsuarioNombre { get; set; }
        public DateTime? FechaAprobacion { get; set; }
        public string EstadoAprobacion { get; set; }
    }
}
