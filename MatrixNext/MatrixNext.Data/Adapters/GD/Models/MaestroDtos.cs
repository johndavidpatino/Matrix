using System;
using System.Collections.Generic;

namespace MatrixNext.Data.Adapters.GD.Models
{
    public class MaestroDocumentoDto
    {
        public int Id { get; set; }
        public string Nombre { get; set; } = string.Empty;
        public string Codigo { get; set; } = string.Empty;
        public int IdProceso { get; set; }
        public int IdResponsable { get; set; }
        public int TipoSolicitud { get; set; }
        public bool Activo { get; set; } = true;
        public string ProcesoNombre { get; set; } = string.Empty;
        public string ResponsableNombre { get; set; } = string.Empty;
        public string TipoNombre { get; set; } = string.Empty;
        public DocumentoControlledDto ControlledDoc { get; set; } = new();
    }

    public class DocumentoControlledDto
    {
        public int Id { get; set; }
        public int IdMaestro { get; set; }
        public string Ubicacion { get; set; } = string.Empty;
        public string MetodoRecuperacion { get; set; } = string.Empty;
        public int TiempoRetencion { get; set; }
        public string DisposicionFinal { get; set; } = string.Empty;
        public bool Activo { get; set; } = true;
        public DateTime? FechaRegistro { get; set; }
    }

    public class MaestroFormDataDto
    {
        public List<TipoSolicitudDto> TiposSolicitud { get; set; } = new();
        public List<ProcesoDto> Procesos { get; set; } = new();
        public List<UsuarioDto> Usuarios { get; set; } = new();
    }

    public class TipoSolicitudDto
    {
        public int Id { get; set; }
        public string Nombre { get; set; } = string.Empty;
    }

    public class ProcesoDto
    {
        public int Id { get; set; }
        public string Nombre { get; set; } = string.Empty;
    }

    public class UsuarioDto
    {
        public int Id { get; set; }
        public string Usuario { get; set; } = string.Empty;
        public string Nombres { get; set; } = string.Empty;
        public string Apellidos { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public bool Activo { get; set; }
    }
}
