/// <summary>
/// DTOs para Maestro de Documentos (GD_MaestroDocumentos)
/// Tipos: 1=Construcción, 2=Actualización, 3=Anulación
/// Ref: BACKLOG_QA_MODULOS_PENDIENTES.md § Sprint 12.3.5
/// </summary>
namespace MatrixNext.Core.DTOs.GD
{
    using System;
    using System.Collections.Generic;

    public class MaestroDocumentoDto
    {
        public long IdMaestro { get; set; }
        public string NombreDocumento { get; set; }
        public string CodigoDocumento { get; set; }
        public long IdProceso { get; set; }
        public string Proceso { get; set; }
        public long IdTipoSolicitud { get; set; } // 1=Construcción, 2=Actualización, 3=Anulación
        public string TipoSolicitud { get; set; }
        public bool Activo { get; set; }
        public bool Controlado { get; set; }
        public string URL { get; set; }
        public int? TiempoRetencion { get; set; }
        public string DisposicionFinal { get; set; }
        public DateTime FechaRegistro { get; set; }
        public long RegistradoPor { get; set; }
        public DateTime? FechaModificacion { get; set; }
        public long? ModificadoPor { get; set; }
    }

    /// <summary>
    /// DTO para Tipo 1 (Construcción): Crear maestro + documento controlado
    /// </summary>
    public class MaestroTipo1ConstruccionDto : MaestroDocumentoDto
    {
        public int TiempoRetencionAños { get; set; } = 5; // Default
        public bool RequiereRevision { get; set; } = true;
        public bool RequiereAprobacion { get; set; } = true;
        public List<long> RevisoresIniciales { get; set; } = new();
    }

    /// <summary>
    /// DTO para Tipo 2 (Actualización): Crear nueva versión o actualizar existente
    /// </summary>
    public class MaestroTipo2ActualizacionDto : MaestroDocumentoDto
    {
        public long IdMaestroExistente { get; set; } // Documento a actualizar
        public string MaestroExistenteNombre { get; set; }
        public bool CrearNuevaVersion { get; set; } = true;
        public string VersionNumero { get; set; } // 1.0, 1.1, 2.0, etc
        public string MotivoCambio { get; set; }
        public bool MantenerControlado { get; set; } = true;
    }

    /// <summary>
    /// DTO para Tipo 3 (Anulación): Marcar maestro como inactivo y documentos no controlados
    /// </summary>
    public class MaestroTipo3AnulacionDto
    {
        public long IdMaestroAnular { get; set; }
        public string NombreMaestro { get; set; }
        public string MotivoAnulacion { get; set; }
        public string NumeroResolucion { get; set; }
        public DateTime FechaAnulacion { get; set; } = DateTime.Now;
        public long UsuarioAnulacion { get; set; }
        public bool DesactivarDocumentosControlados { get; set; } = true;
    }

    /// <summary>
    /// DTO para resumen de maestros por tipo
    /// </summary>
    public class ResumenMaestrosDto
    {
        public int TotalMaestros { get; set; }
        public int TotalConstruccion { get; set; }
        public int TotalActualizacion { get; set; }
        public int TotalAnulacion { get; set; }
        public int MaestrosActivos { get; set; }
        public int MaestrosInactivos { get; set; }
        public int DocumentosControlados { get; set; }
    }
}
