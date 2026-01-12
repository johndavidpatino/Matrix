using System;
using System.Collections.Generic;

namespace MatrixNext.Data.Adapters.PY.Models
{
    /// <summary>
    /// DTO para configuración de trabajo
    /// Mapea a PY_TrabajosConfiguracion
    /// </summary>
    public class TrabajoConfiguracionDto
    {
        public long Id { get; set; }
        public long TrabajoId { get; set; }
        public string? Configuracion { get; set; }
        public DateTime? Fecha { get; set; }
        public string? Usuario { get; set; }
    }

    /// <summary>
    /// Input para guardar configuración de trabajo
    /// </summary>
    public class TrabajoConfiguracionInputDto
    {
        public long? Id { get; set; }
        public long TrabajoId { get; set; }
        public bool ModalidadCuantitativa { get; set; }
        public bool ModalidadQualitativa { get; set; }
        public List<string>? TecnicasActivas { get; set; }
        public List<string>? LineasActivas { get; set; }
        public string? Configuracion { get; set; }
        public long UsuarioId { get; set; }
    }

    /// <summary>
    /// Input para duplicar trabajo completo
    /// </summary>
    public class DuplicarTrabajoInputDto
    {
        public long TrabajoIdOrigen { get; set; }
        public string NombreNuevo { get; set; } = string.Empty;
        public string JobbookNuevo { get; set; } = string.Empty;
        public long ProyectoIdNuevo { get; set; }
        public long ClienteIdNuevo { get; set; }
        public string? TipoModalidad { get; set; }
        public DateTime? FechaInicioNueva { get; set; }
        public DateTime? FechaFinNueva { get; set; }
        public string? Observaciones { get; set; }
        public long UsuarioId { get; set; }
        public bool DuplicarEspecificaciones { get; set; }
        public bool DuplicarMuestra { get; set; }
        public bool DuplicarConfiguracion { get; set; }
        public bool DuplicarHilo { get; set; }
        public bool CopiarDocumentos { get; set; }
    }

    /// <summary>
    /// Resultado de duplicación de trabajo
    /// </summary>
    public class DuplicarTrabajoResultDto
    {
        public long NuevoTrabajoId { get; set; }
        public string JobBookNuevo { get; set; } = string.Empty;
        public bool EspecificacionesDuplicadas { get; set; }
        public bool MuestraDuplicada { get; set; }
        public bool ConfiguracionDuplicada { get; set; }
        public bool HiloDuplicado { get; set; }
        public bool DocumentosCopiadosResult { get; set; }
        public string? ErrorMessage { get; set; }
    }
}
