Public Enum TipoTecnica
	Cualitativo = 1
	Cuantitativo = 2
End Enum

Public Enum TipoProceso
    ControlCalidadCampo = 1
    ControlCalidadTranscripcion = 2
    ControlCalidadInforme = 3
    EvaluacionEntrevistadora = 4
    EvaluacionModeradora = 5
End Enum

Public Enum EstadoPropuesta
    Creada = 1
    Enviada = 2
    Vendida = 3
    Cancelada = 4
    Cerrada = 5
    Perdida = 6
    PorError = 7
End Enum

Public Enum TipoFicha
    Cuanti = 1
    Sesiones = 2
    Observaciones = 3
    Entrevistas = 4
End Enum

Public Enum ListaRoles
    Presidente = 1
    Vicepresidente = 2
    DirectorUnidad = 3
    CoordinadorUnidad = 4
    GerenteCuentas = 5
    GerenteProyectos = 6
    AnalistaProyectos = 7
    AsistenteProyectos = 8
    GerenteOperaciones = 9
    COE = 10
    Moderador = 11
    Entrevistador = 12
    Observador = 13
    Transcriptor = 14
    CoordinadorDeCampo = 15
    SupervisordeCritica = 16
    SupervisordeVerificación = 17
    SupervisordeCaptura = 18
    SupervisordeCodificación = 19
    SupervisordeProcesamiento = 20
    SupervisordeDataCleaning = 21
    CoordinadordeCritica = 22
    CoordinadordeVerificación = 23
    CoordinadordeCaptura = 24
    CoordinadordeCodificación = 25
    CoordinadordeProcesamiento = 26
    CoordinadordeDataCleaning = 27
    CoordinadorScripting = 28
    ProgramadorScripting = 29
    CoordinadorPilotos = 30
    PilotosdeScripting = 31
    CoordinadorNacional_RegionaldeCampo = 32
    AprobacionRSCoe = 51
    AprobacionRSGerente = 52
    AprobacionRSAdmin = 53
End Enum

Public Enum UnidadesCore
    Cuentas = 1
    Proyectos = 2
    COE = 3
    Campo = 4
    Critica = 5
    Verificacion = 6
    Captura = 7
    Codificación = 8
    DataCleaning = 9
    Procesamiento = 10
    Scripting = 11
    Pilotos = 12
    Estadística = 13
End Enum

Public Enum TipoGrupoUnidad
    Comercial = 1
    Operativa = 2
    Administrativa = 3
    Apoyo = 4
End Enum

Public Enum UrlOriginal
    PY_Proyectos_Trabajos = 1
    CORE_ListaTrabajosTareas = 2
    RE_GT_TraficoTareas = 3
    OP_Cuantitativo_Trabajos = 4
    RE_GT_TraficoTareas_Scripting = 5
    RE_GT_TraficoTareas_Pilotos = 6
    RE_GT_TraficoTareas_Critica = 7
    RE_GT_TraficoTareas_Verificacion = 8
    RE_GT_TraficoTareas_Captura = 9
    RE_GT_TraficoTareas_Codificacion = 10
    RE_GT_TraficoTareas_Datacleaning = 11
    RE_GT_TraficoTareas_Procesamiento = 12
    OP_Cualitativo_Trabajos = 13
    OP_Cualitativo_TrabajosCoordinador = 14
    RE_GT_TraficoTareas_Estadistica = 15
    RE_GT_TrabajosPorGerencia = 16
    RE_GT_TraficoEncuestasRMC = 17
    RE_GT_CallCenter = 18
    RP_Reportes_TrabajosPorGrupoBU = 19
End Enum

Public Enum FI_TipoCCOrdenes
    JBE = 1
    JBI = 2
    CC = 3
End Enum