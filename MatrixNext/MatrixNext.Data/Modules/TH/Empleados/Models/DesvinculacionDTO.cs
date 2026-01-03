using System;
using System.Collections.Generic;

namespace MatrixNext.Data.Models.TH
{
    /// <summary>
    /// DTOs para el módulo de Desvinculaciones de Empleados
    /// Migrado desde DesvinculacionesEmpleadosGestionRRHH.aspx
    /// </summary>

    #region Consulta de Desvinculaciones (RRHH)

    /// <summary>
    /// Modelo equivalente a CoreProject.DesvinculacionEmpleadosDapper.TH_DesvinculacionEmpleadosEstatus
    /// SP: TH_DesvinculacionEmpleadosEstatus
    /// </summary>
    public class DesvinculacionEstatusDTO
    {
        public long DesvinculacionEmpleadoId { get; set; }
        public long EmpleadoId { get; set; }
        public string? Nombres { get; set; }
        public string? Apellidos { get; set; }
        public string? UrlFoto { get; set; }
        public int PorcentajeAvanceDesvinculacion { get; set; }
        public int CantidadTotalFilas { get; set; }

        public string NombreEmpleadoCompleto => $"{Nombres} {Apellidos}".Trim();
    }

    public class DesvinculacionesPaginadasDTO
    {
        public int TotalRegistros { get; set; }
        public int PaginaActual { get; set; }
        public int TamañoPagina { get; set; }
        public int TotalPaginas { get; set; }
        public List<DesvinculacionEstatusDTO> Desvinculaciones { get; set; } = new();
    }

    /// <summary>
    /// DTO para filtros de búsqueda de desvinculaciones
    /// </summary>
    public class DesvinculacionFiltroDTO
    {
        public int PageSize { get; set; } = 10;
        public int PageIndex { get; set; } = 0;
        public string? TextoBuscado { get; set; }
    }

    #endregion

    #region Empleados Activos

    /// <summary>
    /// Equivalente a CoreProject.EmpleadosDapper.EmpleadosActivosoResult
    /// SP: TH_EmpleadosActivos_Get
    /// </summary>
    public class EmpleadoActivoDTO
    {
        public int Id { get; set; }
        public string? Nombres { get; set; }
        public string? Apellidos { get; set; }
        public string NombreCompleto => $"{Nombres} {Apellidos}".Trim();
    }

    #endregion

    #region Iniciar Proceso

    /// <summary>
    /// DTO para iniciar proceso de desvinculación
    /// </summary>
    public class IniciarDesvinculacionDTO
    {
        public int EmpleadoId { get; set; }
        public DateTime FechaRetiro { get; set; }
        public string? MotivosDesvinculacion { get; set; }
    }

    #endregion

    #region Evaluaciones

    public class DesvinculacionEvaluacionDTO
    {
        public long DesvinculacionEmpleadoId { get; set; }
        public string? NombreEvaluador { get; set; }
        public string? ApellidosEvaluador { get; set; }
        public int AreaId { get; set; }
        public string? NombreArea { get; set; }
        public string? NombresEvaluadores { get; set; }
        public DateTime? FechaDiligenciamiento { get; set; }
        public string? Comentarios { get; set; }
        public string? Estado { get; set; }
        public string? EmailsEvaluadores { get; set; }

        public string NombreEvaluadorCompleto => $"{NombreEvaluador} {ApellidosEvaluador}".Trim();
    }

    /// <summary>
    /// Modelo equivalente a DesvinculacionesEmpleadosGestionArea.DesvinculacionEmpleadoEvaluacionModel
    /// </summary>
    public class GuardarEvaluacionRequestDTO
    {
        public int DesvinculacionEmpleadoId { get; set; }
        public int AreaId { get; set; }
        public string? Comentarios { get; set; }
    }

    /// <summary>
    /// Modelo equivalente a CoreProject.DesvinculacionEmpleadosDapper.DesvinculacionEmpleadoEvaluacionArea
    /// SP: TH_DesvinculacionEmpleadoAreaEvaluacion_Add
    /// </summary>
    public class DesvinculacionEmpleadoEvaluacionAreaDTO
    {
        public int DesvinculacionEmpleadoId { get; set; }
        public int AreaId { get; set; }
        public DateTime FechaDiligenciamiento { get; set; }
        public string? Comentarios { get; set; }
        public long UsuarioRegistra { get; set; }
    }

    public class DesvinculacionEmpleadoPendientePorEvaluarAreaDTO
    {
        public int DesvinculacionEmpleadoId { get; set; }
        public long EmpleadoId { get; set; }
        public string? Nombres { get; set; }
        public string? Apellidos { get; set; }
        public DateTime FechaRegistro { get; set; }
        public string NombreEmpleadoCompleto => $"{Nombres} {Apellidos}".Trim();
    }

    public class DesvinculacionEmpleadoPendienteEvaluarPorEvaluadorDTO
    {
        public int DesvinculacionEmpleadoId { get; set; }
        public int EmpleadoId { get; set; }
        public DateTime FechaRegistro { get; set; }
        public int AreaId { get; set; }
        public string? NombreArea { get; set; }
        public string? Nombres { get; set; }
        public string? Apellidos { get; set; }
        public string NombreEmpleadoCompleto => $"{Nombres} {Apellidos}".Trim();
    }

    public class DesvinculacionEmpleadoEvaluacionRealizadaPorEvaluadorDTO
    {
        public long DesvinculacionEmpleadoId { get; set; }
        public long EmpleadoId { get; set; }
        public DateTime FechaDiligenciamiento { get; set; }
        public string? Nombres { get; set; }
        public string? Apellidos { get; set; }
        public string? NombreArea { get; set; }
        public string? Comentarios { get; set; }
        public string NombreEmpleadoCompleto => $"{Nombres} {Apellidos}".Trim();
    }

    public class DesvinculacionEmpleadosAreaItemVerificarDTO
    {
        public int Id { get; set; }
        public int AreaId { get; set; }
        public string? Descripcion { get; set; }
        public bool Activo { get; set; }
    }

    #endregion

    #region PDF

    public class DesvinculacionEmpleadoEmpleadoInfoDTO
    {
        public long EmpleadoId { get; set; }
        public string? Nombres { get; set; }
        public string? Apellidos { get; set; }
        public int CargoId { get; set; }
        public string? Cargo { get; set; }
        public DateTime FechaRetiro { get; set; }
        public string NombreEmpleadoCompleto => $"{Nombres} {Apellidos}".Trim();
    }

    #endregion
}
