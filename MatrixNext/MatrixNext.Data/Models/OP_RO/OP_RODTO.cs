namespace MatrixNext.Data.Models.OP_RO
{
    /// <summary>
    /// DTO para Operational Review (OP_RO)
    /// Sprint 11A - Revisiones operacionales
    /// 4 tipos de documentos: Cuestionario, Instructivo, Metodología, Material
    /// Estado workflow: Pendiente → Aprobado/Rechazado
    /// </summary>

    /// <summary>
    /// Información base de una revisión operacional
    /// </summary>
    public class OP_ROReviewDTO
    {
        public int ReviewId { get; set; }
        public string? TipoRevision { get; set; } // Cuestionario, Instructivo, Metodología, Material
        public string? NombreDocumento { get; set; }
        public string? Descripcion { get; set; }
        public int EstudoId { get; set; }
        public int UsuarioCreadorId { get; set; }
        public string? UsuarioCreador { get; set; }
        public DateTime FechaCreacion { get; set; }
        public DateTime? FechaUltimaModificacion { get; set; }
        public string? Estado { get; set; } // Pendiente, Aprobado, Rechazado
        public bool Disponible { get; set; }
    }

    /// <summary>
    /// Cuestionario para revisión
    /// </summary>
    public class OP_ROCuestionarioDTO
    {
        public int CuestionarioId { get; set; }
        public string? Titulo { get; set; }
        public string? Descripcion { get; set; }
        public int NumeroPreguntas { get; set; }
        public string? Estado { get; set; }
        public int VersionId { get; set; }
        public DateTime FechaCreacion { get; set; }
        public List<PreguntaDTO> Preguntas { get; set; } = new();
    }

    /// <summary>
    /// Instructivo para revisión
    /// </summary>
    public class OP_ROInstructivoDTO
    {
        public int InstructivoId { get; set; }
        public string? Titulo { get; set; }
        public string? Contenido { get; set; }
        public string? Estado { get; set; }
        public int VersionId { get; set; }
        public DateTime FechaCreacion { get; set; }
        public int OrdenCampo { get; set; } // Secuencia en operación de campo
        public List<PasoInstructivoDTO> Pasos { get; set; } = new();
    }

    /// <summary>
    /// Metodología para revisión
    /// </summary>
    public class OP_ROMetodologiaDTO
    {
        public int MetodologiaId { get; set; }
        public string? Nombre { get; set; }
        public string? Descripcion { get; set; }
        public string? Alcance { get; set; }
        public string? Estado { get; set; }
        public int VersionId { get; set; }
        public DateTime FechaCreacion { get; set; }
        public List<FaseMetodologiaDTO> Fases { get; set; } = new();
    }

    /// <summary>
    /// Material de ayuda para revisión
    /// </summary>
    public class OP_ROMaterialAyudaDTO
    {
        public int MaterialId { get; set; }
        public string? Titulo { get; set; }
        public string? Tipo { get; set; } // Guía, Plantilla, Referencia, Ejemplo
        public string? ContenidoUrl { get; set; } // URL a archivo o contenido
        public string? Estado { get; set; }
        public int VersionId { get; set; }
        public DateTime FechaCreacion { get; set; }
        public double TamanoMB { get; set; }
    }

    // ============================================
    // COMPONENTES ANIDADOS
    // ============================================

    public class PreguntaDTO
    {
        public int PreguntaId { get; set; }
        public string? Texto { get; set; }
        public string? Tipo { get; set; } // Abierta, Cerrada, Múltiple
        public List<OpcionDTO> Opciones { get; set; } = new();
        public int Orden { get; set; }
    }

    public class OpcionDTO
    {
        public int OpcionId { get; set; }
        public string? Texto { get; set; }
        public int Orden { get; set; }
    }

    public class PasoInstructivoDTO
    {
        public int PasoId { get; set; }
        public string? Descripcion { get; set; }
        public int Orden { get; set; }
        public string? Imagen { get; set; } // URL a imagen del paso
    }

    public class FaseMetodologiaDTO
    {
        public int FaseId { get; set; }
        public string? Nombre { get; set; }
        public string? Descripcion { get; set; }
        public int Orden { get; set; }
        public List<ActividadDTO> Actividades { get; set; } = new();
    }

    public class ActividadDTO
    {
        public int ActividadId { get; set; }
        public string? Descripcion { get; set; }
        public string? Responsable { get; set; }
        public int Orden { get; set; }
    }

    // ============================================
    // WORKFLOW DE REVISIÓN
    // ============================================

    /// <summary>
    /// DTO para solicitud de revisión
    /// Estado: Pendiente → Aprobado/Rechazado
    /// </summary>
    public class OP_ROSolicitudRevisionDTO
    {
        public int SolicitudId { get; set; }
        public int ReviewId { get; set; }
        public string? TipoRevision { get; set; }
        public string? NombreDocumento { get; set; }
        public int UsuarioSolicitanteId { get; set; }
        public string? UsuarioSolicitante { get; set; }
        public DateTime FechaSolicitud { get; set; }
        public string? EstadoActual { get; set; } // Pendiente, Aprobado, Rechazado
        public int? UsuarioRevisorId { get; set; }
        public string? UsuarioRevisor { get; set; }
        public DateTime? FechaRevision { get; set; }
        public string? Comentarios { get; set; }
        public List<HistorialRevisionDTO> Historial { get; set; } = new();
    }

    /// <summary>
    /// Historial de cambios en revisión
    /// </summary>
    public class HistorialRevisionDTO
    {
        public int HistorialId { get; set; }
        public DateTime Fecha { get; set; }
        public string? Usuario { get; set; }
        public string? Accion { get; set; } // CREACIÓN, ENVÍO_REVISIÓN, APROBACIÓN, RECHAZO
        public string? Detalles { get; set; }
        public string? EstadoAnterior { get; set; }
        public string? EstadoNuevo { get; set; }
    }

    // ============================================
    // FILTROS Y BÚSQUEDA
    // ============================================

    /// <summary>
    /// DTO para filtros en búsqueda de revisiones
    /// </summary>
    public class OP_ROFiltrosDTO
    {
        public string? TipoRevision { get; set; }
        public string? Estado { get; set; }
        public DateTime? FechaDesde { get; set; }
        public DateTime? FechaHasta { get; set; }
        public int? UsuarioId { get; set; }
        public string? NombreDocumento { get; set; }
        public int PageNumber { get; set; } = 1;
        public int PageSize { get; set; } = 50;
    }

    /// <summary>
    /// DTO para resultado paginado
    /// </summary>
    public class OP_ROResultadoDTO
    {
        public List<OP_ROReviewDTO> Datos { get; set; } = new();
        public int TotalRegistros { get; set; }
        public int Pagina { get; set; }
        public int RegistrosPorPagina { get; set; }
        public int TotalPaginas { get; set; }
        public bool TienePaginas { get; set; }
    }

    // ============================================
    // ACCIONES DE WORKFLOW
    // ============================================

    /// <summary>
    /// DTO para aprobar revisión
    /// </summary>
    public class OP_ROAprobarDTO
    {
        public int ReviewId { get; set; }
        public int UsuarioRevisorId { get; set; }
        public string? Comentarios { get; set; }
    }

    /// <summary>
    /// DTO para rechazar revisión
    /// </summary>
    public class OP_RORechazarDTO
    {
        public int ReviewId { get; set; }
        public int UsuarioRevisorId { get; set; }
        public string? MotivoRechazo { get; set; }
        public string? Comentarios { get; set; }
    }

    // ============================================
    // CONSTANTES
    // ============================================

    public static class TiposRevision
    {
        public const string CUESTIONARIO = "Cuestionario";
        public const string INSTRUCTIVO = "Instructivo";
        public const string METODOLOGIA = "Metodología";
        public const string MATERIAL_AYUDA = "MaterialAyuda";
    }

    public static class EstadosRevision
    {
        public const string PENDIENTE = "Pendiente";
        public const string APROBADO = "Aprobado";
        public const string RECHAZADO = "Rechazado";
        public const string EN_REVISIÓN = "EnRevisión";
        public const string CANCELADO = "Cancelado";
    }

    public static class AccionesAuditoria
    {
        public const string CREACIÓN = "CREACIÓN";
        public const string ENVÍO_REVISIÓN = "ENVÍO_REVISIÓN";
        public const string APROBACIÓN = "APROBACIÓN";
        public const string RECHAZO = "RECHAZO";
        public const string MODIFICACIÓN = "MODIFICACIÓN";
        public const string CANCELACIÓN = "CANCELACIÓN";
    }
}

