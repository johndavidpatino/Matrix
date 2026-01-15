/// <summary>
/// DTO para Normativas ISO/9001 utilizadas en auditorías
/// </summary>
namespace MatrixNext.Data.DTOs.SGC
{
    public class SGCNormativaDto
    {
        public byte NormativaId { get; set; }
        public string Codigo { get; set; } = string.Empty;
        public string Descripcion { get; set; } = string.Empty;
    }

    public class SGCTipoAuditoriaDto
    {
        public byte TipoAuditoriaId { get; set; }
        public string Descripcion { get; set; } = string.Empty;
    }

    public class SGCTipoHallazgoDto
    {
        public byte TipoHallazgoId { get; set; }
        public string Descripcion { get; set; } = string.Empty;
    }

    public class SGCEstadoAuditoriaDto
    {
        public byte EstadoId { get; set; }
        public string Descripcion { get; set; } = string.Empty;
    }

    public class SGCProcesoDto
    {
        public int ProcesoId { get; set; }
        public string NombreProceso { get; set; } = string.Empty;
    }

    public class SGCFuenteNoConformidadDto
    {
        public int FuenteNoConformidadId { get; set; }
        public string Descripcion { get; set; } = string.Empty;
    }

    public class SGCFuenteDto
    {
        public int FuenteId { get; set; }
        public string Descripcion { get; set; } = string.Empty;
        public int? RefernciaId { get; set; }
    }
}
