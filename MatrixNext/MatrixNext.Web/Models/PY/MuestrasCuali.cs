using System;
using System.Collections.Generic;
using MatrixNext.Web.Models;

namespace MatrixNext.Web.Models.PY
{
    /// <summary>
    /// Entidad para registrar muestras obtenidas en trabajos cualitativos.
    /// Referencia unitaria de un participante entrevistado/encuestado.
    /// Ref: VALIDACION_EVIDENCIAS_PY_CORE.md § 1.1 (CoordinacionCampo)
    /// </summary>
    public class MuestrasCuali : BaseEntity
    {
        /// <summary>FK a TrabajosCuali</summary>
        public long IdTrabajoCuali { get; set; }

        /// <summary>FK a SegmentosCuali</summary>
        public long? IdSegmento { get; set; }

        /// <summary>FK a SesionesCuali (si fue entrevistado en una sesión específica)</summary>
        public long? IdSesion { get; set; }

        /// <summary>Número de muestra único (secuencial o aleatorio)</summary>
        public string? NumeroMuestra { get; set; }

        /// <summary>Nombre completo del participante</summary>
        public string? NombreParticipante { get; set; }

        /// <summary>Teléfono de contacto</summary>
        public string? Telefono { get; set; }

        /// <summary>Correo electrónico de contacto</summary>
        public string? Email { get; set; }

        /// <summary>Dirección del participante</summary>
        public string? Direccion { get; set; }

        /// <summary>Edad del participante</summary>
        public int? Edad { get; set; }

        /// <summary>Género: Masculino, Femenino, Otro</summary>
        public string? Genero { get; set; }

        /// <summary>Estrato socioeconómico: 1-6</summary>
        public int? Estrato { get; set; }

        /// <summary>Ocupación o profesión</summary>
        public string? Ocupacion { get; set; }

        /// <summary>Estado de la muestra: Planeada, Contactada, Confirmada, Entrevistada, No Disponible, Rechazada</summary>
        public string? Estado { get; set; } = "Planeada";

        /// <summary>Fecha de primer contacto</summary>
        public DateTime? FechaContacto { get; set; }

        /// <summary>Fecha de ejecución de la entrevista/sesión</summary>
        public DateTime? FechaEjecucion { get; set; }

        /// <summary>Duración de la entrevista en minutos</summary>
        public int? DuracionEntrevista { get; set; }

        /// <summary>Calidad de la entrevista: Excelente, Buena, Regular, Deficiente</summary>
        public string? CalidadDatos { get; set; }

        /// <summary>Motivo si fue rechazada o no disponible</summary>
        public string? MotivoRechazo { get; set; }

        /// <summary>Notas adicionales sobre la muestra</summary>
        public string? Notas { get; set; }

        // Navegación
        /// <summary>Trabajo cualitativo al que pertenece la muestra</summary>
        public virtual TrabajosCuali? TrabajoCuali { get; set; }

        /// <summary>Segmento al que pertenece la muestra</summary>
        public virtual SegmentosCuali? Segmento { get; set; }

        /// <summary>Sesión en la que se tomó la muestra</summary>
        public virtual SesionesCuali? Sesion { get; set; }

        /// <summary>Entrevistador que realizó la entrevista</summary>
        public virtual EntrevistadorasCuali? Entrevistador { get; set; }
        public long? IdEntrevistador { get; set; }
    }
}
