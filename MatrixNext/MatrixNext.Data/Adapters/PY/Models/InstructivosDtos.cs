using System;

namespace MatrixNext.Data.Adapters.PY.Models
{
    /// <summary>
    /// DTO para Especificaciones Técnicas de Trabajo Cuantitativo
    /// Mapea a PY_EspecifTecTrabajo
    /// </summary>
    public class EspecificacionTecnicaDto
    {
        public long Id { get; set; }
        public long TrabajoId { get; set; }
        public string? EspecifacionesCampo { get; set; }
        public string? MaterialApoyo { get; set; }
        public string? Incidencias { get; set; }
        public string? PilotosCampo { get; set; }
        public string? AuditoriaCampo { get; set; }
        public string? PilotosCalidad { get; set; }
        public string? Estadistica { get; set; }
        public string? Critica { get; set; }
        public string? Verificacion { get; set; }
        public string? Procesamiento { get; set; }
        public string? Codificacion { get; set; }
        public string? VCSeguridad { get; set; }
        public string? VCObtencion { get; set; }
        public string? VCGrupoObjetivo { get; set; }
        public string? VCAplicacionInstrumentos { get; set; }
        public string? VCDistribucionCuotas { get; set; }
        public string? VCMetodologia { get; set; }
        public string? OtrasEspecificaciones { get; set; }
        public string? Usuario { get; set; }
        public DateTime? Fecha { get; set; }
        public int? NoVersion { get; set; }
    }

    /// <summary>
    /// DTO para Especificaciones Técnicas de Trabajo Cualitativo
    /// Mapea a PY_EspecifTecTrabajoCuali
    /// </summary>
    public class EspecificacionTecnicaCualiDto
    {
        public long Id { get; set; }
        public long TrabajoId { get; set; }
        public string? Moderador { get; set; }
        public string? EspecificacionesCampo { get; set; }
        public string? MaterialApoyo { get; set; }
        public string? Incidencias { get; set; }
        public string? Auditoria { get; set; }
        public string? VCSeguridad { get; set; }
        public string? VCObtencion { get; set; }
        public string? VCGrupoObjetivo { get; set; }
        public string? VCAplicacionInstrumentos { get; set; }
        public string? VCDistribucionCuotas { get; set; }
        public string? VCMetodologia { get; set; }
        public bool? Incentivos { get; set; }
        public string? PresupuestoIncentivo { get; set; }
        public string? DistribucionIncentivo { get; set; }
        public bool? RegaloClientes { get; set; }
        public bool? CompraIpsos { get; set; }
        public string? PresupuestoCompra { get; set; }
        public string? DistribucionCompra { get; set; }
        public string? ExclusionesyRestricciones { get; set; }
        public string? RecursosPropiedadesCliente { get; set; }
        public string? HabeasData { get; set; }
        public string? OtrasEspecificaciones { get; set; }
        public string? Usuario { get; set; }
        public DateTime? Fecha { get; set; }
        public int? NoVersion { get; set; }
    }

    /// <summary>
    /// DTO para ayudas cualitativos (catálogo)
    /// </summary>
    public class AyudaCualiDto
    {
        public int Id { get; set; }
        public string? Ayuda { get; set; }
    }

    /// <summary>
    /// DTO para tipo reclutamiento cualitativos (catálogo)
    /// </summary>
    public class TipoReclutamientoCualiDto
    {
        public int Id { get; set; }
        public string? Tipo { get; set; }
    }

    /// <summary>
    /// Input para guardar especificación técnica cuantitativa
    /// </summary>
    public class EspecificacionTecnicaInputDto
    {
        public long? Id { get; set; }
        public long TrabajoId { get; set; }
        public string? EspecifacionesCampo { get; set; }
        public string? MaterialApoyo { get; set; }
        public string? Incidencias { get; set; }
        public string? PilotosCampo { get; set; }
        public string? AuditoriaCampo { get; set; }
        public string? PilotosCalidad { get; set; }
        public string? Estadistica { get; set; }
        public string? Critica { get; set; }
        public string? Verificacion { get; set; }
        public string? Procesamiento { get; set; }
        public string? Codificacion { get; set; }
        public string? VCSeguridad { get; set; }
        public string? VCObtencion { get; set; }
        public string? VCGrupoObjetivo { get; set; }
        public string? VCAplicacionInstrumentos { get; set; }
        public string? VCDistribucionCuotas { get; set; }
        public string? VCMetodologia { get; set; }
        public string? OtrasEspecificaciones { get; set; }
        public long UsuarioId { get; set; }
        public int NoVersion { get; set; }
    }

    /// <summary>
    /// Input para guardar especificación técnica cualitativa
    /// </summary>
    public class EspecificacionTecnicaCualiInputDto
    {
        public long? Id { get; set; }
        public long TrabajoId { get; set; }
        public string? Moderador { get; set; }
        public string? EspecificacionesCampo { get; set; }
        public string? MaterialApoyo { get; set; }
        public string? Incidencias { get; set; }
        public string? Auditoria { get; set; }
        public string? VCSeguridad { get; set; }
        public string? VCObtencion { get; set; }
        public string? VCGrupoObjetivo { get; set; }
        public string? VCAplicacionInstrumentos { get; set; }
        public string? VCDistribucionCuotas { get; set; }
        public string? VCMetodologia { get; set; }
        public bool Incentivos { get; set; }
        public string? PresupuestoIncentivo { get; set; }
        public string? DistribucionIncentivo { get; set; }
        public bool RegaloClientes { get; set; }
        public bool CompraIpsos { get; set; }
        public string? PresupuestoCompra { get; set; }
        public string? DistribucionCompra { get; set; }
        public string? ExclusionesyRestricciones { get; set; }
        public string? RecursosPropiedadesCliente { get; set; }
        public string? HabeasData { get; set; }
        public string? OtrasEspecificaciones { get; set; }
        public long UsuarioId { get; set; }
        public int NoVersion { get; set; }
    }
}
