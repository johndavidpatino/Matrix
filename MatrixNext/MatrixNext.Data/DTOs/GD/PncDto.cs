using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace MatrixNext.Data.DTOs.GD
{
    /// <summary>
    /// DTO base para Producto No Conforme (PNC)
    /// Ref: WebMatrix - PNC_Productos.aspx + PNCClass
    /// SPs mapeados: PNC_Productos_Get, PNC_GetById, PNC_Productos_Add
    /// </summary>
    public class PncDto
    {
        [Display(Name = "ID PNC")]
        public long IdPnc { get; set; }

        [Required(ErrorMessage = "El asociado es obligatorio")]
        [Display(Name = "Asociado A")]
        public int? AsociadoA { get; set; } // 1=JBE, 2=JBI, 3=Actividad

        [Display(Name = "Tipo AsociaciÃ³n")]
        public string AsociadoATexto { get; set; } // DescripciÃ³n: "Trabajo", "Actividad"

        [Required(ErrorMessage = "El ID de referencia es obligatorio")]
        [Display(Name = "ID Referencia")]
        public long? IdReferencia { get; set; } // IdTrabajo, IdActividad

        [Display(Name = "Proyecto/Trabajo")]
        public string ProyectoTrabajo { get; set; }

        [Required(ErrorMessage = "El proceso es obligatorio")]
        [Display(Name = "Proceso")]
        public long? IdProceso { get; set; }

        [Display(Name = "Nombre Proceso")]
        public string NombreProceso { get; set; }

        [Display(Name = "Procedimiento")]
        public long? IdProcedimiento { get; set; }

        [Display(Name = "Nombre Procedimiento")]
        public string NombreProcedimiento { get; set; }

        [Display(Name = "Unidad")]
        public long? IdUnidad { get; set; }

        [Display(Name = "Nombre Unidad")]
        public string NombreUnidad { get; set; }

        [Display(Name = "Persona que Identifica")]
        public long? IdPersonaIdentifica { get; set; }

        [Display(Name = "Nombre Persona")]
        public string PersonaIdentifica { get; set; }

        [Display(Name = "Fecha del Reclamo")]
        public DateTime? FechaReclamo { get; set; }

        [Display(Name = "Fuente")]
        public long? IdFuente { get; set; } // 1=Cliente, 2=Auditoria, 3=Interno

        [Display(Name = "Nombre Fuente")]
        public string NombreFuente { get; set; }

        [Display(Name = "CategorÃ­a")]
        public long? IdCategoria { get; set; }

        [Display(Name = "Nombre CategorÃ­a")]
        public string NombreCategoria { get; set; }

        [Display(Name = "Persona Responsable")]
        public long? IdPersonaResponsable { get; set; }

        [Display(Name = "Nombre Responsable")]
        public string PersonaResponsable { get; set; }

        [Display(Name = "Persona a Informar")]
        public long? IdPersonaInformar { get; set; }

        [Display(Name = "Nombre Persona Informar")]
        public string PersonaInformar { get; set; }

        [Required(ErrorMessage = "La descripciÃ³n es obligatoria")]
        [StringLength(2000, ErrorMessage = "La descripciÃ³n no puede exceder 2000 caracteres")]
        [Display(Name = "DescripciÃ³n")]
        public string Descripcion { get; set; }

        [Display(Name = "Tarea Relacionada")]
        public long? IdTarea { get; set; }

        [Display(Name = "Nombre Tarea")]
        public string NombreTarea { get; set; }

        [Display(Name = "Estado")]
        public byte? IdEstado { get; set; } // 1=Registrado, 7=CausaRegistrada, 6=Rechazado, etc.

        [Display(Name = "Nombre Estado")]
        public string NombreEstado { get; set; }

        [Display(Name = "Observaciones")]
        public string Observaciones { get; set; }

        // AuditorÃ­a
        [Display(Name = "Registrado Por")]
        public long? RegistradoPor { get; set; }

        [Display(Name = "Usuario Registro")]
        public string UsuarioRegistro { get; set; }

        [Display(Name = "Fecha Registro")]
        public DateTime? FechaRegistro { get; set; }

        [Display(Name = "Modificado Por")]
        public long? ModificadoPor { get; set; }

        [Display(Name = "Usuario ModificaciÃ³n")]
        public string UsuarioModificacion { get; set; }

        [Display(Name = "Fecha ModificaciÃ³n")]
        public DateTime? FechaModificacion { get; set; }

        // Propiedades computadas
        public string EstadoClass => IdEstado switch
        {
            6 => "danger",     // Rechazado
            7 => "success",    // CausaRegistrada
            _ => "secondary"   // Otros
        };

        public string EstadoIcon => IdEstado switch
        {
            6 => "ban",        // Rechazado
            7 => "check-circle", // CausaRegistrada
            _ => "circle"      // Otros
        };

        public bool PuedeEditar => IdEstado == 1; // Solo si estÃ¡ en estado Registrado
        public bool PuedeRegistrarCausa => IdEstado == 1; // Solo si estÃ¡ en estado Registrado
    }

    /// <summary>
    /// DTO para registro de Causa en PNC
    /// Ref: PNCClass.agregarCausa()
    /// SP: PNC_Productos_Causas_Add
    /// </summary>
    public class PncCausaDto
    {
        [Display(Name = "ID Causa")]
        public long IdCausa { get; set; }

        [Required(ErrorMessage = "El PNC es obligatorio")]
        [Display(Name = "ID PNC")]
        public long? IdPnc { get; set; }

        [Required(ErrorMessage = "La descripciÃ³n de causa es obligatoria")]
        [StringLength(1000, ErrorMessage = "La descripciÃ³n no puede exceder 1000 caracteres")]
        [Display(Name = "DescripciÃ³n Causa")]
        public string DescripcionCausa { get; set; }

        [Required(ErrorMessage = "La acciÃ³n correctiva es obligatoria")]
        [StringLength(2000, ErrorMessage = "La acciÃ³n no puede exceder 2000 caracteres")]
        [Display(Name = "AcciÃ³n Correctiva")]
        public string AccionCorrectiva { get; set; }

        [Required(ErrorMessage = "La persona responsable es obligatoria")]
        [Display(Name = "Persona Responsable")]
        public long? IdPersonaResponsable { get; set; }

        [Display(Name = "Nombre Responsable")]
        public string PersonaResponsable { get; set; }

        [Display(Name = "Fecha Vencimiento")]
        public DateTime? FechaVencimiento { get; set; }

        [Display(Name = "Estado")]
        public byte? IdEstado { get; set; } // 1=Abierta, 2=Cerrada

        [Display(Name = "Nombre Estado")]
        public string NombreEstado { get; set; }

        [Display(Name = "Observaciones")]
        public string Observaciones { get; set; }

        // AuditorÃ­a
        [Display(Name = "Registrado Por")]
        public long? RegistradoPor { get; set; }

        [Display(Name = "Usuario Registro")]
        public string UsuarioRegistro { get; set; }

        [Display(Name = "Fecha Registro")]
        public DateTime? FechaRegistro { get; set; }

        public string EstadoClass => IdEstado switch
        {
            1 => "warning",    // Abierta
            2 => "success",    // Cerrada
            _ => "secondary"
        };

        public bool PuedeEditar => IdEstado == 1; // Solo si estÃ¡ abierta
    }

    /// <summary>
    /// DTO para Seguimiento de PNC
    /// Ref: SP: PNC_Seguimiento_Get
    /// </summary>
    public class PncSeguimientoDto
    {
        [Display(Name = "ID Seguimiento")]
        public long IdSeguimiento { get; set; }

        [Display(Name = "ID PNC")]
        public long? IdPnc { get; set; }

        [Display(Name = "Proyecto/Trabajo")]
        public string ProyectoTrabajo { get; set; }

        [Display(Name = "DescripciÃ³n PNC")]
        public string DescripcionPnc { get; set; }

        [Display(Name = "Estado PNC")]
        public string EstadoPnc { get; set; }

        [Display(Name = "ID Causa")]
        public long? IdCausa { get; set; }

        [Display(Name = "DescripciÃ³n Causa")]
        public string DescripcionCausa { get; set; }

        [Display(Name = "AcciÃ³n Correctiva")]
        public string AccionCorrectiva { get; set; }

        [Display(Name = "Responsable")]
        public string Responsable { get; set; }

        [Display(Name = "Fecha Vencimiento")]
        public DateTime? FechaVencimiento { get; set; }

        [Display(Name = "Estado Causa")]
        public string EstadoCausa { get; set; }

        [Display(Name = "DÃ­as Restantes")]
        public int? DiasRestantes { get; set; }

        [Display(Name = "Fecha Registro")]
        public DateTime? FechaRegistro { get; set; }

        public string DiasClass => DiasRestantes switch
        {
            < 0 => "danger",       // Vencido
            <= 3 => "warning",     // PrÃ³ximo a vencer
            _ => "success"         // Dentro de plazo
        };

        public bool EstaVencida => DiasRestantes < 0;
    }

    /// <summary>
    /// DTO para Resumen/EstadÃ­sticas de PNC
    /// Ref: CÃ¡lculos locales (no SP especÃ­fico)
    /// </summary>
    public class PncResumenDto
    {
        [Display(Name = "Total PNC")]
        public int TotalPnc { get; set; }

        [Display(Name = "PNC Registrados")]
        public int PncRegistrados { get; set; }

        [Display(Name = "PNC Causa Registrada")]
        public int PncCausaRegistrada { get; set; }

        [Display(Name = "PNC Rechazados")]
        public int PncRechazados { get; set; }

        [Display(Name = "Causas Abiertas")]
        public int CausasAbiertas { get; set; }

        [Display(Name = "Causas Cerradas")]
        public int CausasCerradas { get; set; }

        [Display(Name = "Causas Vencidas")]
        public int CausasVencidas { get; set; }

        [Display(Name = "Causas PrÃ³ximas a Vencer")]
        public int CausasProximasVencer { get; set; } // PrÃ³ximos 3 dÃ­as

        public double PorcentajeResolucion => TotalPnc > 0 ? (PncCausaRegistrada * 100.0 / TotalPnc) : 0;
    }

    /// <summary>
    /// DTO para Log de cambios de estado en PNC
    /// Ref: SP: PNC_Productos_Log_Get
    /// Tabla: PNC_Productos_Log
    /// </summary>
    public class PncLogDto
    {
        [Display(Name = "ID Log")]
        public long IdLog { get; set; }

        [Display(Name = "ID PNC")]
        public long? IdPnc { get; set; }

        [Display(Name = "Estado Anterior")]
        public byte? EstadoAnterior { get; set; }

        [Display(Name = "Nombre Estado Anterior")]
        public string NombreEstadoAnterior { get; set; }

        [Display(Name = "Estado Nuevo")]
        public byte? EstadoNuevo { get; set; }

        [Display(Name = "Nombre Estado Nuevo")]
        public string NombreEstadoNuevo { get; set; }

        [Display(Name = "Motivo Cambio")]
        public string MotivoCambio { get; set; }

        [Display(Name = "Usuario")]
        public long? IdUsuario { get; set; }

        [Display(Name = "Nombre Usuario")]
        public string NombreUsuario { get; set; }

        [Display(Name = "Fecha Cambio")]
        public DateTime? FechaCambio { get; set; }

        public string Accion => $"{NombreEstadoAnterior} â†’ {NombreEstadoNuevo}";
    }
}

