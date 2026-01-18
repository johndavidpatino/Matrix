# FUNCIONALIDADES POR MÓDULO - MatrixNext

**Fecha**: 2026-01-17  
**FASE 10**: Consolidación para Ayudas UX  
**Propósito**: Información para tooltips, badges y alertas contextuales

---

## 📋 ÍNDICE DE MÓDULOS

| # | Área | Nombre | Vistas | SP Clave | Prioridad UX |
|---|------|--------|--------|----------|--------------|
| 1 | TH | Talento Humano | 7 | 15+ | 🔴 Alta |
| 2 | US | Usuarios | 4 | 8 | 🔴 Alta |
| 3 | PY | Proyectos | 12 | 25+ | 🔴 Alta |
| 4 | OP | Operaciones | 39 | 50+ | 🔴 Alta |
| 5 | CU | Cuentas | 5 | 12 | 🟡 Media |
| 6 | CC | Costos/Control | 22 | 30+ | 🟡 Media |
| 7 | GD | Gestión Documental | 7 | 18 | 🟡 Media |
| 8 | RP | Reportes | 3 | 5 | 🟢 Baja |
| 9 | SGC | Sistema Gestión Calidad | 2 | 8 | 🟢 Baja |
| 10 | ES | Estadística | 4 | 6 | 🟢 Baja |
| 11 | IT | Tecnología/iField | 2 | 4 | 🟢 Baja |
| 12 | MBO | Propuestas/AOT | 10 | 8 | 🟡 Media |
| 13 | RE_GT | Recolección/Tratamiento | 4 | 10 | 🟡 Media |
| 14 | PC | Producto Cliente | 1 | 4 | 🟢 Baja |
| 15 | INV | Inventario | 5 | 12 | 🟡 Media |
| 16 | CORE | Workflow/Tareas | 9 | 20+ | 🔴 Alta |
| 17 | EQ | EasyQuote | 4 | 6 | 🟡 Media |

---

## 1️⃣ TH - TALENTO HUMANO

### Funcionalidades Principales

| Vista | Funcionalidad | SP Clave |
|-------|---------------|----------|
| **Empleados/Index** | Administración de empleados: datos generales, laborales, personales, nómina | `TH_Empleado_*`, `TH_Busqueda_Empleado` |
| **Ausencias/Index** | Solicitud de vacaciones, permisos, licencias, incapacidades | `TH_Ausencia_*`, `TH_CausarVacaciones` |
| **GestionAusencia/Index** | Aprobación/rechazo de solicitudes por RRHH | `TH_AprobarAusencia`, `TH_RechazarAusencia` |
| **Desvinculaciones/Index** | Proceso de retiro de empleados | `TH_Desvinculacion_*`, `TH_ProcesarRetiro` |

### Sugerencias UX

| Vista | Elemento | Tipo | Texto Sugerido |
|-------|----------|------|----------------|
| Empleados/Index | Campo Identificación | ℹ️ Tooltip | "Número de cédula sin puntos ni guiones" |
| Empleados/Index | Botón Guardar | ⚠️ Alerta | "Los cambios en nómina requieren aprobación de RRHH" |
| Empleados/Index | Estado Activo/Inactivo | 📊 Badge | Verde="Activo", Rojo="Inactivo", Amarillo="En retiro" |
| Ausencias/Index | Días disponibles | 📊 Badge | "Días disponibles: X" con color según cantidad |
| Ausencias/Index | Campo Fecha Inicio | ℹ️ Tooltip | "No puede ser fecha pasada ni fin de semana" |
| Ausencias/Index | Selector Tipo | ℹ️ Tooltip | "Vacación requiere mínimo 5 días de anticipación" |
| GestionAusencia/Index | Estado solicitud | 📊 Badge | Pendiente=Amarillo, Aprobada=Verde, Rechazada=Rojo |
| GestionAusencia/Index | Botón Aprobar | 💡 Tip | "Al aprobar vacaciones se descontarán automáticamente los días" |
| Desvinculaciones/Index | Proceso activo | ⚠️ Alerta | "Este proceso no puede revertirse una vez finalizado" |

---

## 2️⃣ US - USUARIOS

### Funcionalidades Principales

| Vista | Funcionalidad | SP Clave |
|-------|---------------|----------|
| **Usuarios/Index** | CRUD de usuarios del sistema | `US_Usuario_*`, `US_UsuariosActivos_Get` |
| **Roles/Index** | Gestión de roles y permisos | `US_Rol_*`, `US_RolesPermisos_Get` |
| **Permisos/Index** | Asignación de permisos a usuarios | `US_Permiso_*`, `US_UsuarioPermisos_Get` |
| **GrupoUnidad/Index** | Grupos y unidades organizacionales | `US_GrupoUnidad_*` |

### Sugerencias UX

| Vista | Elemento | Tipo | Texto Sugerido |
|-------|----------|------|----------------|
| Usuarios/Index | Usuario nuevo | 💡 Tip | "La contraseña se enviará por correo al usuario" |
| Usuarios/Index | Estado usuario | 📊 Badge | Activo=Verde, Bloqueado=Rojo, Pendiente=Amarillo |
| Usuarios/Index | Último acceso | ℹ️ Tooltip | "Fecha del último inicio de sesión" |
| Roles/Index | Rol con permisos | ℹ️ Tooltip | "Este rol tiene X permisos asignados" |
| Roles/Index | Eliminar rol | ⚠️ Alerta | "No puede eliminar roles con usuarios asignados" |
| Permisos/Index | Permiso heredado | 📊 Badge | Azul="Heredado de rol", Verde="Directo" |

---

## 3️⃣ PY - PROYECTOS

### Funcionalidades Principales

| Vista | Funcionalidad | SP Clave |
|-------|---------------|----------|
| **Proyectos/Index** | Maestro de proyectos | `PY_Proyectos_*`, `PY_TiposProyectos_Get` |
| **Trabajos/Index** | Trabajos cuantitativos por proyecto | `PY_Trabajos_*`, `PY_TrabajosDuplicar` |
| **TrabajosCuali/Index** | Trabajos cualitativos | `PY_TrabajosCuali_*`, `PY_SegmentosCuali_*` |
| **Asignaciones/Index** | Asignar responsables a proyectos | `PY_AsignacionProyectos_*` |
| **SegmentosCuali/Index** | Segmentos para cualitativos | `PY_SegmentosCuali_*` |
| **SesionesCuali/Index** | Programación de sesiones | `PY_SesionesCuali_*` |
| **Instructivos/Index** | Carga de instructivos | `PY_Instructivos_*` |
| **ControlCalidad/Index** | Control de calidad del proyecto | `PY_ControlCalidad_*` |

### Sugerencias UX

| Vista | Elemento | Tipo | Texto Sugerido |
|-------|----------|------|----------------|
| Proyectos/Index | JobBook | ℹ️ Tooltip | "Código único del proyecto en formato XXXXX-XXX" |
| Proyectos/Index | Estado proyecto | 📊 Badge | Activo=Verde, Cerrado=Gris, En pausa=Amarillo |
| Trabajos/Index | Metodología | ℹ️ Tooltip | "F2F=Cara a cara, CATI=Telefónico, Online=Digital" |
| Trabajos/Index | Duplicar trabajo | 💡 Tip | "Se copiarán parámetros, NO datos de campo" |
| TrabajosCuali/Index | Segmentos | 📊 Badge | Muestra total de entrevistas por segmento |
| Asignaciones/Index | Rol asignado | ℹ️ Tooltip | "GP=Gerente Proyecto, PM=Project Manager, COE=Coord. Operaciones" |
| SegmentosCuali/Index | Cuota | ⚠️ Alerta | "La suma de cuotas debe coincidir con la muestra total" |
| Instructivos/Index | Archivo | ℹ️ Tooltip | "Formatos permitidos: PDF, DOCX, XLSX (máx 10MB)" |

---

## 4️⃣ OP - OPERACIONES

### Funcionalidades Principales

| Vista | Funcionalidad | SP Clave |
|-------|---------------|----------|
| **Trabajos/Index** | Portal COE: gestión de trabajos | `OP_Trabajos_*`, `OP_TrabajoCOE_Get` |
| **TrabajosCoordinador/Index** | Portal Coordinador | `OP_TrabajosCoord_*` |
| **TrabajosCallCenter/Index** | Portal Call Center | `OP_TrabajosCallCenter_*` |
| **MuestraTrabajos/Index** | Gestión de muestra por trabajo | `OP_Muestra_*`, `OP_MuestraCiudades_Get` |
| **EstimacionProduccion/Index** | Estimación de producción por ciudad | `OP_Estimacion_*`, `OP_PlaneacionProduccion_*` |
| **Trafico/Index** | Tráfico de encuestas entre unidades | `OP_TraficoEncuestas_*` |
| **Ips/Index** | Control IPS por tarea | `OP_IPS_*`, `OP_ObservacionesIPS_*` |
| **Avances/Index** | Avances de campo | `OP_Avances_*`, `OP_AvanceDiario_Get` |
| **Portal/Index** | Portal COE consolidado | `OP_Dashboard_*` |

### Sugerencias UX

| Vista | Elemento | Tipo | Texto Sugerido |
|-------|----------|------|----------------|
| Trabajos/Index | Unidad | 📊 Badge | RMC=Azul, Captura=Verde, Verificación=Naranja, Crítica=Rojo |
| Trabajos/Index | Estado trabajo | 📊 Badge | En curso=Verde, Cerrado=Gris, Pausado=Amarillo |
| Trabajos/Index | Cierre | ⚠️ Alerta | "El cierre sincroniza documentos y no puede revertirse" |
| MuestraTrabajos/Index | Muestra total | ℹ️ Tooltip | "Suma de todas las ciudades incluidas en el estudio" |
| EstimacionProduccion/Index | Planeación | 💡 Tip | "La planeación automática distribuye según muestra" |
| Trafico/Index | Envío | ℹ️ Tooltip | "Cantidad máxima disponible para enviar" |
| Trafico/Index | Recepción | ⚠️ Alerta | "Si la cantidad recibida difiere, agregue observación" |
| Ips/Index | Estado tarea | 📊 Badge | Pendiente=Amarillo, Completada=Verde, Vencida=Rojo |
| Ips/Index | Observación | ℹ️ Tooltip | "Registre cualquier novedad que afecte la entrega" |
| Avances/Index | % Avance | 📊 Badge | <50%=Rojo, 50-80%=Amarillo, >80%=Verde |

---

## 5️⃣ CU - CUENTAS

### Funcionalidades Principales

| Vista | Funcionalidad | SP Clave |
|-------|---------------|----------|
| **Cuentas/Index** | Maestro de cuentas (clientes) | `CU_Cuentas_*`, `CU_ClientesActivos_Get` |
| **Brief/Index** | Briefs de proyectos | `CU_Brief_*` |
| **Estudios/Index** | Estudios por cuenta | `CU_Estudios_*` |
| **Propuestas/Index** | Propuestas comerciales | `CU_Propuestas_*` |
| **Presupuesto/Index** | Presupuestos por cuenta | `CU_Presupuesto_*` |

### Sugerencias UX

| Vista | Elemento | Tipo | Texto Sugerido |
|-------|----------|------|----------------|
| Cuentas/Index | NIT | ℹ️ Tooltip | "Número de identificación tributaria con dígito de verificación" |
| Cuentas/Index | Estado cuenta | 📊 Badge | Activa=Verde, Inactiva=Rojo, Prospecto=Azul |
| Brief/Index | Fecha límite | ⚠️ Alerta | "El brief debe completarse antes de crear propuesta" |
| Propuestas/Index | Estado propuesta | 📊 Badge | Borrador=Gris, Enviada=Azul, Aprobada=Verde, Rechazada=Rojo |
| Presupuesto/Index | Monto | ℹ️ Tooltip | "Valores en COP antes de impuestos" |

---

## 6️⃣ CC - COSTOS/CONTROL

### Funcionalidades Principales

| Vista | Funcionalidad | SP Clave |
|-------|---------------|----------|
| **ControlPresupuestos/Index** | Control de presupuestos internos | `CC_ControlPresupuestos_*` |
| **LiquidarPlanillasActividades/Index** | Liquidación de planillas | `CC_LiquidarPlanillas_*` |
| **GenerarBonificacion/Index** | Generación de bonificaciones | `CC_Bonificacion_*` |
| **PresupuestosInternos/Index** | Presupuestos internos por trabajo | `CC_PresupuestosInternos_*` |
| **ConsolidacionProduccion/Index** | Consolidación de producción | `CC_ConsolidacionProduccion_*` |

### Sugerencias UX

| Vista | Elemento | Tipo | Texto Sugerido |
|-------|----------|------|----------------|
| ControlPresupuestos/Index | Varianza | 📊 Badge | <10%=Verde, 10-20%=Amarillo, >20%=Rojo |
| ControlPresupuestos/Index | Ejecutado vs Aprobado | ℹ️ Tooltip | "Porcentaje de ejecución del presupuesto" |
| LiquidarPlanillasActividades/Index | Corte | ℹ️ Tooltip | "Período de liquidación: día 16 al 15 del mes siguiente" |
| LiquidarPlanillasActividades/Index | Aprobar | ⚠️ Alerta | "Una vez aprobada, la planilla no puede modificarse" |
| GenerarBonificacion/Index | Tipo | ℹ️ Tooltip | "Dominical, Festivo, Nocturno, Horas extra" |

---

## 7️⃣ GD - GESTIÓN DOCUMENTAL

### Funcionalidades Principales

| Vista | Funcionalidad | SP Clave |
|-------|---------------|----------|
| **DocumentosMaestro/Index** | Maestro de documentos del SGC | `GD_MaestroDocumentos_*` |
| **Solicitudes/Index** | Solicitudes de construcción/actualización/anulación | `GD_SolDocumentos_*`, `GD_Revisiones_*` |
| **Aprobaciones/Index** | Aprobación de documentos | `GD_Revisiones_Edit`, `GD_AprobarDocumento` |
| **Repositorio/Index** | Repositorio de archivos con versionamiento | `GD_RepositorioDocumentos_*` |
| **Pnc/Index** | Productos No Conformes | `GD_PNC_*` |

### Sugerencias UX

| Vista | Elemento | Tipo | Texto Sugerido |
|-------|----------|------|----------------|
| DocumentosMaestro/Index | Código documento | ℹ️ Tooltip | "Formato: ÁREA-TIPO-CONSECUTIVO (ej: GD-PR-001)" |
| DocumentosMaestro/Index | Estado documento | 📊 Badge | Vigente=Verde, Obsoleto=Rojo, En revisión=Amarillo |
| Solicitudes/Index | Tipo solicitud | ℹ️ Tooltip | "Construcción=Nuevo, Actualización=Modificar, Anulación=Eliminar" |
| Solicitudes/Index | Revisores | 💡 Tip | "Asigne al menos un revisor por área relacionada" |
| Aprobaciones/Index | Rechazar | ⚠️ Alerta | "Indique el motivo del rechazo para notificar al solicitante" |
| Repositorio/Index | Versión | 📊 Badge | Número de versión con fecha de última modificación |
| Pnc/Index | Severidad | 📊 Badge | Crítico=Rojo, Mayor=Naranja, Menor=Amarillo |

---

## 8️⃣ RP - REPORTES

### Funcionalidades Principales

| Vista | Funcionalidad | SP Clave |
|-------|---------------|----------|
| **Reportes/Index** | Generador de reportes | `RP_Reportes_*`, `RP_ConsultaReporte` |
| **Reportes/Generar** | Ejecución de reportes | `RP_EjecutarReporte` |
| **Reportes/Detalle** | Visualización de resultados | `RP_ResultadoReporte_Get` |

### Sugerencias UX

| Vista | Elemento | Tipo | Texto Sugerido |
|-------|----------|------|----------------|
| Reportes/Index | Filtros | 💡 Tip | "Use filtros para reducir el tiempo de generación" |
| Reportes/Generar | Formato | ℹ️ Tooltip | "Excel para datos, PDF para presentación" |
| Reportes/Generar | Procesando | ⚠️ Alerta | "Reportes grandes pueden tardar varios minutos" |

---

## 9️⃣ SGC - SISTEMA GESTIÓN CALIDAD

### Funcionalidades Principales

| Vista | Funcionalidad | SP Clave |
|-------|---------------|----------|
| **Auditorias/Index** | Auditorías internas | `SGC_AuditoriasInternas_*`, `SGC_AI_Auditorias_*` |
| **AccionesMejora/Index** | Acciones correctivas | `ACM_AccionesMejora_*` |

### Sugerencias UX

| Vista | Elemento | Tipo | Texto Sugerido |
|-------|----------|------|----------------|
| Auditorias/Index | Estado auditoría | 📊 Badge | Programada=Azul, En ejecución=Amarillo, Completada=Verde, Cancelada=Rojo |
| Auditorias/Index | Fecha límite | ⚠️ Alerta | "Auditorías vencidas requieren reprogramación" |
| AccionesMejora/Index | Tipo hallazgo | ℹ️ Tooltip | "NC=No Conformidad, OBS=Observación, OM=Oportunidad Mejora" |
| AccionesMejora/Index | Seguimiento | 💡 Tip | "Registre avances periódicos hasta el cierre" |

---

## 🔟 ES - ESTADÍSTICA

### Funcionalidades Principales

| Vista | Funcionalidad | SP Clave |
|-------|---------------|----------|
| **Home/Index** | Dashboard de estadística | `ES_Dashboard_*` |
| **DisenoMuestral/Index** | Diseño de muestras | `ES_DisenoMuestral_*` |
| **BriefDisenoMuestral/Index** | Brief para diseño | `ES_BriefDisenoMuestral_*` |
| **MetodologiaCampo/Index** | Metodología de campo | `ES_MetodologiaCampo_*` |

### Sugerencias UX

| Vista | Elemento | Tipo | Texto Sugerido |
|-------|----------|------|----------------|
| DisenoMuestral/Index | Confianza | ℹ️ Tooltip | "Nivel de confianza estadístico (típico: 95%)" |
| DisenoMuestral/Index | Error máximo | ℹ️ Tooltip | "Margen de error permitido en los resultados" |
| BriefDisenoMuestral/Index | Completar | 💡 Tip | "Complete todos los campos para cálculo automático de muestra" |

---

## 1️⃣1️⃣ IT - TECNOLOGÍA/IFIELD

### Funcionalidades Principales

| Vista | Funcionalidad | SP Clave |
|-------|---------------|----------|
| **IT/Index** | Configuración iField | `IT_iFieldSettings_*` |
| **SyncIssues/Index** | Problemas de sincronización | `IT_SyncIssues_*` |

### Sugerencias UX

| Vista | Elemento | Tipo | Texto Sugerido |
|-------|----------|------|----------------|
| IT/Index | Proyecto iField | ℹ️ Tooltip | "ID del proyecto en plataforma iField" |
| IT/Index | Sincronizar | ⚠️ Alerta | "La sincronización puede tardar según volumen de datos" |
| SyncIssues/Index | Estado | 📊 Badge | Pendiente=Amarillo, Resuelto=Verde, Escalado=Rojo |

---

## 1️⃣2️⃣ MBO - PROPUESTAS/AOT

### Funcionalidades Principales

| Vista | Funcionalidad | SP Clave |
|-------|---------------|----------|
| **Home/Index** | Dashboard MBO | `MBO_Dashboard_*` |
| **Propuestas/**** | Estado de propuestas por unidad/gerencia | `MBO_Propuestas_*` |
| **AOT/**** | Allocation of Time (AOT) | `MBO_AOT_*` |
| **Campo/**** | Gestión de errores de campo | `MBO_Campo_*` |

### Sugerencias UX

| Vista | Elemento | Tipo | Texto Sugerido |
|-------|----------|------|----------------|
| Propuestas/EstadoUnidad | Tasa conversión | 📊 Badge | Verde=>50%, Amarillo=30-50%, Rojo=<30% |
| AOT/Gerencia | Horas asignadas | ℹ️ Tooltip | "Horas billables asignadas vs disponibles" |
| Campo/Errores | Tipo error | 📊 Badge | Crítico=Rojo, Medio=Amarillo, Leve=Verde |

---

## 1️⃣3️⃣ RE_GT - RECOLECCIÓN/TRATAMIENTO

### Funcionalidades Principales

| Vista | Funcionalidad | SP Clave |
|-------|---------------|----------|
| **Recoleccion/Index** | Gestión de recolección | `RE_GT_Recoleccion_*` |
| **GestionTratamiento/Index** | Tratamiento de datos | `RE_GT_Tratamiento_*` |
| **AsignacionCampo/Index** | Asignación de personal de campo | `RE_GT_AsignacionCampo_*` |
| **CambioJBI/Index** | Cambios de JBI | `RE_GT_CambioJBI_*` |

### Sugerencias UX

| Vista | Elemento | Tipo | Texto Sugerido |
|-------|----------|------|----------------|
| Recoleccion/Index | Estado | 📊 Badge | Pendiente=Amarillo, Recolectado=Verde, Rechazado=Rojo |
| GestionTratamiento/Index | Prioridad | ℹ️ Tooltip | "Alta=Entrega <3 días, Media=3-7 días, Baja=>7 días" |
| AsignacionCampo/Index | Encuestador | 💡 Tip | "Verifique disponibilidad antes de asignar" |

---

## 1️⃣4️⃣ PC - PRODUCTO CLIENTE

### Funcionalidades Principales

| Vista | Funcionalidad | SP Clave |
|-------|---------------|----------|
| **ProductoInterno/Index** | Productos internos del cliente | `PC_ProductoInterno_*` |

### Sugerencias UX

| Vista | Elemento | Tipo | Texto Sugerido |
|-------|----------|------|----------------|
| ProductoInterno/Index | Estado | 📊 Badge | Disponible=Verde, En uso=Azul, Agotado=Rojo |
| ProductoInterno/Index | Recibir | ℹ️ Tooltip | "Registre cantidad y lote del producto recibido" |

---

## 1️⃣5️⃣ INV - INVENTARIO

### Funcionalidades Principales

| Vista | Funcionalidad | SP Clave |
|-------|---------------|----------|
| **RegistroArticulos/Index** | Registro de artículos | `INV_Articulos_*` |
| **Asignaciones/Index** | Asignación de equipos | `INV_Asignaciones_*` |
| **Legalizaciones/Index** | Legalización de asignaciones | `INV_Legalizaciones_*` |
| **MantenimientoEquipos/Index** | Mantenimiento preventivo/correctivo | `INV_Mantenimiento_*` |
| **StockConsumibles/Index** | Stock de consumibles | `INV_StockConsumibles_*` |

### Sugerencias UX

| Vista | Elemento | Tipo | Texto Sugerido |
|-------|----------|------|----------------|
| RegistroArticulos/Index | Serial | ℹ️ Tooltip | "Número de serie único del equipo" |
| RegistroArticulos/Index | Estado | 📊 Badge | Disponible=Verde, Asignado=Azul, En reparación=Amarillo, Dado de baja=Rojo |
| Asignaciones/Index | Responsable | ⚠️ Alerta | "El empleado debe firmar acta de recibo" |
| Legalizaciones/Index | Pendiente | 📊 Badge | Cantidad de asignaciones sin legalizar |
| MantenimientoEquipos/Index | Próximo mtto | ⚠️ Alerta | "Equipos con mantenimiento vencido" |
| StockConsumibles/Index | Stock mínimo | 📊 Badge | Verde=>mínimo, Amarillo=cerca, Rojo=bajo mínimo |

---

## 1️⃣6️⃣ CORE - WORKFLOW/TAREAS

### Funcionalidades Principales

| Vista | Funcionalidad | SP Clave |
|-------|---------------|----------|
| **GestionTareas/Index** | Gestión de tareas operativas | `CORE_WorkFlow_*`, `CORE_Tareas_*` |
| **WorkFlow/Index** | Configuración de workflows | `CORE_Configuracion_*` |
| **TareasConfig/Index** | Catálogo de tareas | `CORE_Tareas_Config_*` |
| **TareasPrevias/Index** | Dependencias entre tareas | `CORE_TareasPrevias_*` |
| **TareasXHilo/Index** | Mapeo tareas a hilos | `CORE_TareasXHilo_*` |
| **TareasDocumentos/Index** | Documentos requeridos por tarea | `CORE_TareasDocumentos_*` |
| **Indicadores/Index** | Indicadores de gestión | `CORE_Indicadores_*` |
| **WorkFlowDashboard/Index** | Dashboard de workflows | `CORE_Dashboard_*` |

### Sugerencias UX

| Vista | Elemento | Tipo | Texto Sugerido |
|-------|----------|------|----------------|
| GestionTareas/Index | Estado tarea | 📊 Badge | Pendiente=Amarillo, En curso=Azul, Completada=Verde, Vencida=Rojo |
| GestionTareas/Index | Responsable | ℹ️ Tooltip | "Usuario asignado para ejecutar la tarea" |
| GestionTareas/Index | Fecha vencimiento | ⚠️ Alerta | "Tareas vencidas afectan indicadores del área" |
| WorkFlow/Index | Precedencia | ℹ️ Tooltip | "La tarea no puede iniciarse hasta completar las previas" |
| WorkFlow/Index | Hilo | ℹ️ Tooltip | "Tipo de proceso asociado (producción, calidad, etc.)" |
| TareasConfig/Index | Duplicar | 💡 Tip | "Use duplicar para crear tareas similares rápidamente" |
| Indicadores/Index | SLA | 📊 Badge | Cumple=Verde, Alerta=Amarillo, Incumple=Rojo |

---

## 1️⃣7️⃣ EQ - EASYQUOTE

### Funcionalidades Principales

| Vista | Funcionalidad | SP Clave |
|-------|---------------|----------|
| **EasyQuote/Index** | Cotizador de estudios | `EQ_Cotizacion_*` |
| **MaestrasAdmin/Index** | Administración de maestras | `EQ_Maestras_*` |
| **EasyQuoteSeed/Index** | Datos semilla | `EQ_Seed_*` |

### Sugerencias UX

| Vista | Elemento | Tipo | Texto Sugerido |
|-------|----------|------|----------------|
| EasyQuote/Index | Metodología | ℹ️ Tooltip | "F2F=Cara a cara, CATI=Telefónico, Online=Digital, Auto=Autoaplicado" |
| EasyQuote/Index | Penetración | ℹ️ Tooltip | "Porcentaje de elegibles que califican para la encuesta" |
| EasyQuote/Index | Duración | ℹ️ Tooltip | "Tiempo estimado de aplicación del cuestionario en minutos" |
| EasyQuote/Index | Costo estimado | 📊 Badge | Muestra desglose por rubro |
| MaestrasAdmin/Index | Parámetro | ⚠️ Alerta | "Cambios en parámetros afectan todas las cotizaciones futuras" |

---

## 📊 RESUMEN DE AYUDAS UX SUGERIDAS

| Tipo | Ícono | Cantidad Total | Uso Principal |
|------|-------|----------------|---------------|
| **Tooltip** | ℹ️ | 45+ | Explicación de campos y acciones |
| **Badge** | 📊 | 40+ | Estados, métricas, indicadores |
| **Alerta** | ⚠️ | 25+ | Advertencias, validaciones críticas |
| **Tip** | 💡 | 15+ | Mejores prácticas, sugerencias |

---

## 🎨 ESTÁNDARES VISUALES

### Colores de Badge por Estado

| Estado | Color Bootstrap | Clase |
|--------|-----------------|-------|
| Activo/Aprobado/Completado | Verde | `badge-success` |
| Pendiente/En proceso | Amarillo | `badge-warning` |
| Inactivo/Rechazado/Error | Rojo | `badge-danger` |
| Informativo/En curso | Azul | `badge-primary` |
| Neutral/Borrador | Gris | `badge-secondary` |

### Iconos Sugeridos (Bootstrap Icons)

| Tipo | Icono | Clase |
|------|-------|-------|
| Tooltip info | ℹ️ | `bi-info-circle` |
| Alerta | ⚠️ | `bi-exclamation-triangle` |
| Tip | 💡 | `bi-lightbulb` |
| Éxito | ✅ | `bi-check-circle` |
| Error | ❌ | `bi-x-circle` |

---

*Documento generado como parte de FASE 10: Consolidar Funcionalidades para Ayudas UX*
*Fecha: 2026-01-17*
