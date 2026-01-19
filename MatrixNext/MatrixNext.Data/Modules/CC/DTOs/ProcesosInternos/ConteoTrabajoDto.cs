namespace MatrixNext.Data.Modules.CC.DTOs.ProcesosInternos
{
    /// <summary>
    /// DTO para obtener conteos/preguntas históricas de trabajos
    /// Mapea a tabla: IQ_PreguntasHistorico (SP: CC_ConteosXIdGet)
    /// </summary>
    public class ConteoTrabajoDto
    {
        public long Id { get; set; }
        public string? JobBook { get; set; }
        public long IdTrabajo { get; set; }
        public string? Nombre { get; set; }
        public double Unidad { get; set; }
        public string? Producto { get; set; }
        public long Duracion { get; set; }
        public long CerradasRU { get; set; }
        public long CerradasRM { get; set; }
        public long Abiertas { get; set; }
        public long AbiertasMul { get; set; }
        public long Otros { get; set; }
        public long Demograficos { get; set; }
        public long Paginas { get; set; }
        public string? Observacion { get; set; }
        public DateTime Fecha { get; set; }
        public long UsuarioId { get; set; }
    }

    /// <summary>
    /// Request para guardar preguntas históricas de un trabajo
    /// SP: CC_PreguntasHistoricoGuardar
    /// Origen: WebMatrix/CC_FinzOpe/ConteoTrabajos.aspx.vb - btnGuardarPreguntas_Click
    /// </summary>
    public class GuardarPreguntasHistoricoRequest
    {
        /// <summary>Código JobBook del trabajo</summary>
        public string Job { get; set; } = string.Empty;
        
        /// <summary>ID del trabajo</summary>
        public long TrabajoId { get; set; }
        
        /// <summary>Nombre del trabajo</summary>
        public string NombreTrabajo { get; set; } = string.Empty;
        
        /// <summary>Unidad de medida</summary>
        public double Unidad { get; set; }
        
        /// <summary>Código de producto</summary>
        public string? Producto { get; set; }
        
        /// <summary>Duración real del cuestionario</summary>
        public long Duracion { get; set; }
        
        /// <summary>Preguntas cerradas respuesta única</summary>
        public long Cerradas { get; set; }
        
        /// <summary>Preguntas cerradas respuesta múltiple</summary>
        public long CerradasMultiple { get; set; }
        
        /// <summary>Preguntas abiertas</summary>
        public long Abiertas { get; set; }
        
        /// <summary>Preguntas abiertas múltiples</summary>
        public long AbiertasMultiple { get; set; }
        
        /// <summary>Otras preguntas</summary>
        public long Otros { get; set; }
        
        /// <summary>Preguntas demográficas</summary>
        public long Demograficos { get; set; }
        
        /// <summary>Número de páginas</summary>
        public long Paginas { get; set; }
        
        /// <summary>Observaciones</summary>
        public string? Observaciones { get; set; }
        
        /// <summary>ID del usuario que registra</summary>
        public long UsuarioId { get; set; }
    }

    /// <summary>
    /// DTO para actividades por trabajo
    /// SP: CC_ActividadesXTrabajo
    /// </summary>
    public class ActividadTrabajoDto
    {
        public long IdActividad { get; set; }
        public string CodigoActividad { get; set; } = string.Empty;
        public string NombreActividad { get; set; } = string.Empty;
        public string? Descripcion { get; set; }
        public byte Estado { get; set; }
    }
}
