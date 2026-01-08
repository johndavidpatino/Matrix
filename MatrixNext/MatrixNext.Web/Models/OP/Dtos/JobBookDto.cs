namespace MatrixNext.Web.Models.OP.Dtos
{
    /// <summary>
    /// DTO para representar un JobBook en búsquedas de registro de producción.
    /// </summary>
    public class JobBookDto
    {
        /// <summary>ID único del JobBook</summary>
        public int JobBookId { get; set; }

        /// <summary>Código del JobBook (ej: CALI-2024-001)</summary>
        public string Codigo { get; set; }

        /// <summary>Nombre/descripción del JobBook</summary>
        public string Nombre { get; set; }

        /// <summary>Tipo: JBE (Encuesta), JBI (Interno), CC (Centro de Costo)</summary>
        public string Tipo { get; set; }

        /// <summary>ID del trabajo asociado</summary>
        public int TrabajoId { get; set; }

        /// <summary>Estado actual del JobBook</summary>
        public string Estado { get; set; }

        /// <summary>Información adicional para display</summary>
        public string DisplayText => $"{Codigo} - {Nombre}";
    }
}
