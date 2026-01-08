using System;

namespace MatrixNext.Web.Models.OP.Dtos
{
    /// <summary>
    /// DTO para representar una planilla de productividad en revisión multirrol.
    /// </summary>
    public class PlanillaProductividadDto
    {
        /// <summary>ID único de la planilla</summary>
        public int PlanillaId { get; set; }

        /// <summary>ID del trabajo asociado</summary>
        public int TrabajoId { get; set; }

        /// <summary>Identificador del centro de costo o actividad</summary>
        public string Concepto { get; set; }

        /// <summary>Cantidad registrada en la planilla</summary>
        public int Cantidad { get; set; }

        /// <summary>Valor unitario de la actividad</summary>
        public decimal ValorUnitario { get; set; }

        /// <summary>Monto total (Cantidad * ValorUnitario)</summary>
        public decimal MontoTotal { get; set; }

        /// <summary>Monto previamente aprobado (o propuesto)</summary>
        public decimal MontoPrevio { get; set; }

        /// <summary>Diferencia respecto a aprobado anterior</summary>
        public decimal Diferencia => MontoTotal - MontoPrevio;

        /// <summary>Estado: 1=Pendiente, 2=Aprobada, 3=Rechazada, 4=En Revisión</summary>
        public int Estado { get; set; }

        /// <summary>Descripción del estado</summary>
        public string EstadoDescripcion 
        { 
            get => Estado switch
            {
                1 => "Pendiente",
                2 => "Aprobada",
                3 => "Rechazada",
                4 => "En Revisión",
                _ => "Desconocido"
            };
        }

        /// <summary>Usuario que realizó la última actualización</summary>
        public string UsuarioActualizacion { get; set; }

        /// <summary>Fecha de última actualización</summary>
        public DateTime? FechaActualizacion { get; set; }

        /// <summary>Observaciones si fue rechazada</summary>
        public string Observaciones { get; set; }

        /// <summary>Tipo de actividad (para filtrado por rol): 1-20=Producción, 21-23=CATI/CAWI, etc.</summary>
        public int TipoActividad { get; set; }
    }
}
