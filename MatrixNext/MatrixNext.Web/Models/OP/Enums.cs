namespace MatrixNext.Web.Models.OP;

/// <summary>
/// Áreas de producción para registro de actividades OP
/// </summary>
/// <remarks>
/// Valores extraídos de WebMatrix/OP_Cuantitativo/RegistroProduccionOP.aspx.vb
/// </remarks>
public enum EAreas
{
    /// <summary>
    /// Área de Procesamiento de datos
    /// </summary>
    Procesamiento = 23,
    
    /// <summary>
    /// Área de Scripting
    /// </summary>
    Scripting = 18
}

/// <summary>
/// Indica si una actividad es un reproceso
/// </summary>
public enum EReproceso
{
    /// <summary>
    /// No es reproceso
    /// </summary>
    No = 0,
    
    /// <summary>
    /// Es reproceso
    /// </summary>
    Si = 1
}

/// <summary>
/// Tipos de actividades para scripts
/// </summary>
public enum EActividad
{
    /// <summary>
    /// Crear un nuevo script
    /// </summary>
    CrearScript = 36,
    
    /// <summary>
    /// Reutilizar un script existente
    /// </summary>
    ReutilizarScript = 37
}

/// <summary>
/// Estados de solicitudes de presupuesto interno
/// </summary>
public enum EEstadoPresupuestoInterno
{
    /// <summary>
    /// Solicitud pendiente de aprobación
    /// </summary>
    Pendiente = 0,
    
    /// <summary>
    /// Solicitud aprobada
    /// </summary>
    Aprobada = 1,
    
    /// <summary>
    /// Solicitud rechazada
    /// </summary>
    Rechazada = 2
}

/// <summary>
/// Estados de planillas de productividad
/// </summary>
public enum EEstadoPlanilla
{
    /// <summary>
    /// Planilla cargada, pendiente de revisión
    /// </summary>
    Cargada = 0,
    
    /// <summary>
    /// Planilla revisada por COE
    /// </summary>
    Revisada = 10,
    
    /// <summary>
    /// Planilla aprobada (workflow completo)
    /// </summary>
    Aprobada = 20,
    
    /// <summary>
    /// Planilla rechazada
    /// </summary>
    Rechazada = 99
}

/// <summary>
/// Roles de revisión de productividad
/// </summary>
public enum ERolRevision
{
    /// <summary>
    /// PMO (Project Management Office) - Permiso 100
    /// </summary>
    PMO = 100,
    
    /// <summary>
    /// Coordinador - Permiso 135
    /// </summary>
    Coordinador = 135,
    
    /// <summary>
    /// Campo - Permiso 156
    /// </summary>
    Campo = 156,
    
    /// <summary>
    /// MyS/Call - Permiso 157
    /// </summary>
    MySCall = 157
}

/// <summary>
/// Tipos de tarea para IPS (Indicadores de Performance de Servicio)
/// </summary>
public enum ETipoTareaIPS
{
    Instrumentos = 1,
    Codificacion = 2,
    Procesamiento = 3,
    Scripting = 4,
    Estadistica = 5,
    Presentacion = 6,
    Otros = 99
}
