namespace MatrixNext.Data.Modules.TH.Empleados.Models
{
    /// <summary>
    /// DTO para Área/Service Line
    /// Corresponde a TH_Area_Get_Result
    /// </summary>
    public class AreaServiceLineDTO
    {
        public int Id { get; set; }
        public string Nombre { get; set; } = string.Empty;
        public string? Codigo { get; set; }
        public bool Activo { get; set; }
    }

    /// <summary>
    /// DTO para Grupo Sanguíneo
    /// Corresponde a TH_GruposSanguineos
    /// </summary>
    public class GrupoSanguineoDTO
    {
        public int Id { get; set; }
        public string Descripcion { get; set; } = string.Empty;
        public string? Abreviatura { get; set; }
    }

    /// <summary>
    /// DTO para Cargo
    /// Corresponde a TH_Cargos_Get_Result
    /// </summary>
    public class CargoDTO
    {
        public int Id { get; set; }
        public string Nombre { get; set; } = string.Empty;
        public int? AreaId { get; set; }
        public string? AreaNombre { get; set; }
        public bool Activo { get; set; }
    }

    /// <summary>
    /// DTO para Estado Civil
    /// Corresponde a TH_EstadosCiviles
    /// </summary>
    public class EstadoCivilDTO
    {
        public int Id { get; set; }
        public string Descripcion { get; set; } = string.Empty;
    }

    /// <summary>
    /// DTO para Banco
    /// </summary>
    public class BancoDTO
    {
        public int Id { get; set; }
        public string Nombre { get; set; } = string.Empty;
        public string? Codigo { get; set; }
        public bool Activo { get; set; }
    }

    /// <summary>
    /// DTO para Tipo de Cuenta
    /// </summary>
    public class TipoCuentaDTO
    {
        public int Id { get; set; }
        public string Descripcion { get; set; } = string.Empty;
    }

    /// <summary>
    /// DTO para EPS (Entidad Promotora de Salud)
    /// </summary>
    public class EpsDTO
    {
        public int Id { get; set; }
        public string Nombre { get; set; } = string.Empty;
        public string? Codigo { get; set; }
        public bool Activo { get; set; }
    }

    /// <summary>
    /// DTO para Fondo de Pensiones
    /// </summary>
    public class FondoPensionesDTO
    {
        public int Id { get; set; }
        public string Nombre { get; set; } = string.Empty;
        public string? Codigo { get; set; }
        public bool Activo { get; set; }
    }

    /// <summary>
    /// DTO para Fondo de Cesantías
    /// </summary>
    public class FondoCesantiasDTO
    {
        public int Id { get; set; }
        public string Nombre { get; set; } = string.Empty;
        public string? Codigo { get; set; }
        public bool Activo { get; set; }
    }

    /// <summary>
    /// DTO para Caja de Compensación
    /// </summary>
    public class CajaCompensacionDTO
    {
        public int Id { get; set; }
        public string Nombre { get; set; } = string.Empty;
        public string? Codigo { get; set; }
        public bool Activo { get; set; }
    }

    /// <summary>
    /// DTO para ARL (Administradora de Riesgos Laborales)
    /// </summary>
    public class ArlDTO
    {
        public int Id { get; set; }
        public string Nombre { get; set; } = string.Empty;
        public string? Codigo { get; set; }
        public bool Activo { get; set; }
    }

    /// <summary>
    /// DTO para Nivel de Inglés
    /// </summary>
    public class NivelInglesDTO
    {
        public int Id { get; set; }
        public string Descripcion { get; set; } = string.Empty;
        public string? Nivel { get; set; } // A1, A2, B1, B2, C1, C2
    }

    /// <summary>
    /// DTO para Sede
    /// </summary>
    public class SedeDTO
    {
        public int Id { get; set; }
        public string Nombre { get; set; } = string.Empty;
        public string? Ciudad { get; set; }
        public bool Activa { get; set; }
    }

    /// <summary>
    /// DTO para Tipo de Contrato
    /// </summary>
    public class TipoContratoDTO
    {
        public int Id { get; set; }
        public string Descripcion { get; set; } = string.Empty;
    }

    /// <summary>
    /// DTO para NSE (Nivel Socioeconómico)
    /// </summary>
    public class NseDTO
    {
        public int Id { get; set; }
        public string Descripcion { get; set; } = string.Empty;
        public string? Nivel { get; set; } // 1, 2, 3, 4, 5, 6
    }

    /// <summary>
    /// DTO para Talla de Camiseta
    /// </summary>
    public class TallaCamisetaDTO
    {
        public int Id { get; set; }
        public string Descripcion { get; set; } = string.Empty; // XS, S, M, L, XL, XXL
    }

    /// <summary>
    /// DTO para Banda (nivel jerárquico)
    /// </summary>
    public class BandaDTO
    {
        public int Id { get; set; }
        public string Descripcion { get; set; } = string.Empty;
        public int? Nivel { get; set; }
    }

    /// <summary>
    /// DTO para Level (nivel dentro de banda)
    /// </summary>
    public class LevelDTO
    {
        public int Id { get; set; }
        public string Descripcion { get; set; } = string.Empty;
        public int? BandaId { get; set; }
    }
}
