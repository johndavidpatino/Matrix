using System;
using System.Collections.Generic;

namespace MatrixNext.Web.ViewModels.OP;

public sealed record UnidadDto(int Id, string Unidad);

public sealed record ActividadDto(int Cod, string Actividad, int? SubActividadCod, string? SubActividad, bool AplicaTodos);

public sealed record JbeDto(int Id, string Nombre);

public sealed record ProduccionRowViewModel
{
    public int Id { get; init; }
    public string Area { get; init; } = string.Empty;
    public string Actividad { get; init; } = string.Empty;
    public string? SubActividad { get; init; }
    public string Fecha { get; init; } = string.Empty;
    public string HoraInicio { get; init; } = string.Empty;
    public string HoraFin { get; init; } = string.Empty;
    public int? CantidadGeneral { get; init; }
    public int? CantidadEfectivas { get; init; }
    public bool EsReproceso { get; init; }
    public string Observacion { get; init; } = string.Empty;
}

public sealed record GuardarRegistroRequest
{
    public long TrabajoId { get; init; }
    public long UsuarioId { get; init; }
    public int Actividad { get; init; }
    public int Unidad { get; init; }
    public int? SubActividad { get; init; }
    public int TipoJB { get; init; }
    public int? JBId { get; init; }
    public DateTime Fecha { get; init; }
    public TimeSpan HoraInicio { get; init; }
    public TimeSpan HoraFin { get; init; }
    public int? CantidadGeneral { get; init; }
    public int? CantidadEfectivas { get; init; }
    public string Observacion { get; init; } = string.Empty;
    public bool EsReproceso { get; init; }
    public byte? TipoReproceso { get; init; }
    public byte? TipoAplicativoProceso { get; init; }
    public int? CantVarsScript { get; init; }
    public int? CantVarsExport { get; init; }
}

public sealed class ProduccionViewModel
{
    public string Identificacion { get; init; } = string.Empty;
    public long TrabajoId { get; init; }
    public long UsuarioId { get; init; }
    public int? UnidadSeleccionada { get; init; }
    public int? ActividadSeleccionada { get; init; }
    public int? SubActividadSeleccionada { get; init; }
    public int TipoJB { get; init; } = 1;
    public int? JBId { get; init; }
    public DateTime Fecha { get; init; } = DateTime.Today;
    public TimeSpan HoraInicio { get; init; } = TimeSpan.FromHours(8);
    public TimeSpan HoraFin { get; init; } = TimeSpan.FromHours(17);
    public int? CantidadGeneral { get; init; }
    public int? CantidadEfectivas { get; init; }
    public string Observacion { get; init; } = string.Empty;
    public bool EsReproceso { get; init; }
    public byte? TipoReproceso { get; init; }
    public byte? TipoAplicativoProceso { get; init; }
    public int? CantVarsScript { get; init; }
    public int? CantVarsExport { get; init; }
    public IReadOnlyList<UnidadDto> Unidades { get; init; } = Array.Empty<UnidadDto>();
    public IReadOnlyList<ActividadDto> Actividades { get; init; } = Array.Empty<ActividadDto>();
    public IReadOnlyList<ActividadDto> SubActividades { get; init; } = Array.Empty<ActividadDto>();
    public IReadOnlyList<JbeDto> Jbes { get; init; } = Array.Empty<JbeDto>();
    public IReadOnlyList<ProduccionRowViewModel> Registros { get; init; } = Array.Empty<ProduccionRowViewModel>();
}
