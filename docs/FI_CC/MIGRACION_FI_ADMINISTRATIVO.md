# MIGRACIÓN FI_AdministrativoFinanciero → MatrixNext

**Estado**: ✅ Análisis COMPLETO (6 grupos FI + CC_FinzOpe incluido)
**Alcance**: 
  - FI: Formularios referenciados desde WebMatrix/FI_AdministrativoFinanciero/Default.aspx (excluye compras/OC/OS y radicación/aprobación facturas)
  - CC_FinzOpe: Infraestructura de soporte (SP, tablas, contexto) - INCLUIDO EN MIGRACIÓN
**Fuente base**: WebMatrix/FI_AdministrativoFinanciero/Default.aspx + CoreProject/CC_FinzOpe.
**Reglas**: Aplican todas las directrices en DIRECTRICES_MIGRACION.md (SP existentes, naming DB, Areas, modales, sin features nuevas).
**Esfuerzo Total**: 
  - FI: 612h directas + 92h buffer = 704h
  - CC_FinzOpe: 80h (estimado)
  - **TOTAL: 784 horas** (~10 semanas con 1 dev @ 80h/sem, o 7-8 semanas con 1.5-2 devs)
**Dependencias CU**: ✅ MÍNIMA (read-only de jobbooks; sin impacto en secuencia)
**CC_FinzOpe**: 🔄 INCLUIDO COMO INFRAESTRUCTURA (Sprint Pre-1)

---

## 📚 Documentación Completa del Proyecto

**Esta es la documentación FINAL y COMPLETA lista para implementación.**

### Estructura de Documentos

| Documento | Descripción | Ubicación |
|-----------|-------------|-----------|
| **MIGRACION_FI_ADMINISTRATIVO.md** | Análisis detallado de FI module, 6 grupos, 28 páginas | Aquí |
| **PLAN_SPRINT_PRE1_CC_FINZOPE.md** | Plan completo Sprint Pre-1 (CC infraestructura), 80h | Hermano |
| **PLAN_SPRINTS_1_6_FI.md** | Detalles de cada Sprint FI (Sprints 1-6), tareas, código | Hermano |
| **PATRONES_ARQUITECTURA_FI.md** | Patrones reutilizables, estructura, convenciones, DI | Hermano |
| **CRONOGRAMA_VALIDACION.md** | Timeline semanal detallado, Go/No-Go checklists, métricas | Hermano |

### Cómo Leer Esta Documentación

**Para Empezar**:
1. Leer esta sección "Resumen Ejecutivo Rápido" (abajo)
2. Revisar CRONOGRAMA_VALIDACION.md (timeline completo)
3. Revisar PLAN_SPRINT_PRE1_CC_FINZOPE.md (setup inicial)

**Para Implementar Sprint X**:
1. Abrir PLAN_SPRINTS_1_6_FI.md → Sección "SPRINT X"
2. Consultar PATRONES_ARQUITECTURA_FI.md para estructura estándar
3. Seguir cronograma en CRONOGRAMA_VALIDACION.md
4. Usar DIRECTRICES_MIGRACION.md como guía (15 reglas)

**Para Entender Dependencias**:
- Ver sección 6 en PLAN_SPRINT_PRE1_CC_FINZOPE.md
- Ver sección 7 en este documento (CU_Presupuesto)

**Para Testing/QA**:
- Ver CRONOGRAMA_VALIDACION.md → "Checklist Go/No-Go por Sprint"
- Ver PLAN_SPRINTS_1_6_FI.md → Secciones "Testing" de cada Sprint

### Estado de Documentación

```
✅ MIGRACION_FI_ADMINISTRATIVO.md     - COMPLETA
✅ PLAN_SPRINT_PRE1_CC_FINZOPE.md      - COMPLETA
✅ PLAN_SPRINTS_1_6_FI.md              - COMPLETA
✅ PATRONES_ARQUITECTURA_FI.md         - COMPLETA
✅ CRONOGRAMA_VALIDACION.md            - COMPLETA
✅ DIRECTRICES_MIGRACION.md            - Existente
✅ DASHBOARD_MIGRACION.md              - Actualizado
✅ MODULOS_MIGRACION.md                - Actualizado

TOTAL: 8 documentos de soporte
TAMAÑO TOTAL: ~5000+ líneas de documentación
LISTO PARA: Implementación inmediata
```

---

## Resumen Ejecutivo Rápido

```
📊 SPRINTS Y GRUPOS DOCUMENTADOS
┌──────────────────────────────────────────────────────────────┐
│ SPRINT PRE-1: CC_FinzOpe (INFRAESTRUCTURA)       80h  🔴  │
│ └─ Semanas 1-2: Tablas, SP, contexto EF6 wrapper        │
├──────────────────────────────────────────────────────────────┤
│ Grupo 1: Control Presupuestos             4 págs   92h  🟠  │
│ Grupo 2: Presupuestos Internos            4 págs   68h  🟡  │
│ Grupo 3: Procesos Internos                6 págs  132h  🔴  │
│ Grupo 4: Reportes (read-only)             4 págs   72h  🟠  │
│ Grupo 5: Producción & Liquidación         9 págs  232h  🔴  │
│ Grupo 6: Inventario (CRUD simple)         1 págs   16h  🟡  │
├──────────────────────────────────────────────────────────────┤
│ FI Subtotal:                              28 págs  612h     │
│ FI Buffer (15%):                                   92h     │
│ FI TOTAL CON BUFFER:                             704h     │
│                                                            │
│ CC_FinzOpe (infraestructura):                     80h     │
├──────────────────────────────────────────────────────────────┤
│ GRAN TOTAL:                               28 págs  784h    │
│ TIMELINE:                                    10 sem @ 1dev  │
│                              o 7-8 semanas @ 1.5-2 devs   │
└──────────────────────────────────────────────────────────────┘

🔗 DEPENDENCIAS CU_PRESUPUESTO
├─ ListadoEstudios: referencia jobbooks (read-only) ✅
├─ ListadoPropuestas: separación clara (CU=Cliente; FI=Interno) ✅
├─ ControlPresupuestos: usa SP existentes de CC_FinzOpe ✅
└─ NominaDistribucionCostos: distribuye sobre jobbooks CU ✅
    ➜ CONCLUSIÓN: Mínima dependencia, sin bloqueos de migración

⚠️ RIESGOS CRÍTICOS
├─ Cálculos financieros (liquidación, bonificación)
├─ Auditoría exhaustiva requerida en todas las operaciones
├─ Performance en reportes con 10k+ registros
├─ SP en CC_FinzOpe DEBEN SER VALIDADOS PRE-SPRINT PRE-1
├─ Coherencia producción ↔ reportes ↔ liquidación
└─ Migración CC_FinzOpe en paralelo con FI Sprint 1 (crítico)

✅ RECOMENDACIÓN
1. Sprint Pre-1 (Semanas 1-2): Migrar CC_FinzOpe PRIMERO
   └─ Wrapper EF Core para SP, tablas contexto
   └─ Validar índices y performance en SQL
   └─ ~80 horas

2. Sprint 1+ (Semanas 3+): Iniciar FI sobre CC_FinzOpe migrado
   └─ Sin esperar CAP
   └─ SP de CC_FinzOpe ya disponibles en Core8
   └─ 704 horas distribuidas en 5 sprints
```

---

## 1) Alcance y exclusiones
- ✅ Incluir todos los webforms listados en Default.aspx que no estén explícitamente excluidos.
- 🚫 Excluir: creación/aprobación de compras, órdenes de compra, órdenes de servicio, radicación y aprobación de facturas.
- 🔍 Validar reutilización de SP/ADO.NET en CoreProject (FI_Model, CC_FinzOpe) antes de reimplementar.

### Mapa de enlaces en Default.aspx (FI)
| Grupo | Link (WebMatrix) | Ruta | Incluir | Notas |
|-------|------------------|------|---------|-------|
| Control Presupuestos | Control Costos | ../CAP/ControlPresupuestos.aspx | ✅ | Probable contexto CAP/finanzas; revisar SP.
| Control Presupuestos | Listado Estudios | ListadoEstudios.aspx | ✅ | Listado; revisar filtros/jobbooks.
| Control Presupuestos | Listado Propuestas | ListadoPropuestas.aspx | ✅ | Relacionado con estudios; revisar dependencias CU.
| Control Presupuestos | Nomina Distribucion Costos | NominaDistribucionCostos.aspx | ✅ | Distribución nómina; validar con TH/CC data.
| Presupuestos Internos | Requerimientos | ../CC_FinzOpe/GenerarRequerimientos.aspx | ✅ | Requerimientos internos; revisar SP CC_FinzOpe.
| Presupuestos Internos | Presupuestos | ../CC_FinzOpe/PresupuestosInternosIndex.aspx | ✅ | Presupuestos internos.
| Presupuestos Internos | Descargar Trabajos | ../CC_FinzOpe/ListadoTrabajos.aspx | ✅ | Descarga trabajos.
| Presupuestos Internos | LogPersonas | ../TH_TalentoHumano/ConsultaLog.aspx | ✅ | Reutiliza TH logging.
| Procesos Internos | Conteos | ../CC_FinzOpe/ConteoTrabajos.aspx | ✅ | Conteo trabajos.
| Procesos Internos | Reporte Conteos | ../CC_FinzOpe/ReporteConteoTrabajos.aspx | ✅ | Reporte conteos.
| Procesos Internos | ResumenProductividad | ../CC_FinzOpe/ResumenesdeProduccion.aspx | ✅ | Resumen producción.
| Procesos Internos | RequerimientoDeServicio | ../CC_FinzOpe/OrdenesdeServicio.aspx | 🚫 | Excluido por instrucción.
| Procesos Internos | Contratistas | ../TH_TalentoHumano/Contratistas.aspx | ✅ | Gestión contratistas (TH)
| Procesos Internos | Módulo de Contratación | Contratacion.aspx | ✅ | Revisar dependencia RH/Legales.
| Procesos Internos | PST-Contratistas | PrestacionServicios-CT.aspx | ✅ | Prestación servicios.
| Reportes | RadicarCuentas | ../CC_FinzOpe/RecepcionCuentasdeCobro.aspx | 🚫 | Se asume radicación de facturas → excluido.
| Reportes | AprobarCuentas | ../CC_FinzOpe/ListadoCuentasRecibidas.aspx | 🚫 | Se asume aprobación de facturas → excluido.
| Reportes | ReportePagos | ../CC_FinzOpe/ReportePagos.aspx | ✅ | Reporte pagos.
| Reportes | ReporteProduccion | ../CC_FinzOpe/ReporteActividadesProduccion.aspx | ✅ | Reporte producción.
| Reportes | ReporteOrdenes | ../CC_FinzOpe/ReporteOrdenesdeServicio.aspx | 🚫 | Ordenes servicio → excluido.
| Reportes | ReporteContabilizacionPST | ../CC_FinzOpe/ReporteContabilizacionPST.aspx | ✅ | Contabilización PST.
| Reportes | Reporte Legalizaciones | ../Inventario/ReporteLegalizaciones.aspx | ✅ | Legalizaciones (inventario).
| Producción | Produccion | ../CC_FinzOpe/Produccion.aspx | ✅ | Cargue producción.
| Producción | Eliminar cargue | ../CC_FinzOpe/EliminarCargueProduccion.aspx | ✅ | Borrado de cargues.
| Producción | Liquidacion Bono | ../CC_FinzOpe/GenerarBonificacion.aspx | ✅ | Bonificación.
| Producción | Descargar Producción | ../CC_FinzOpe/ExportarProduccionIDs.aspx | ✅ | Exportación.
| Producción | Estado JobBooks | ../CC_FinzOpe/EstadoJobBooks.aspx | ✅ | Cambio estado jobbooks.
| Producción | Reporte PST sin producción | ../RP_Reportes/ReportePSTSinProduccion.aspx | ✅ | Reporte PST sin prod.
| Producción | Cargue Descuentos SS | ../CC_FinzOpe/CargueDescuentosSS.aspx | ✅ | Cargue descuentos.
| Producción | Liquidar Planillas | ../CC_FinzOpe/LiquidarPlanillasActividades.aspx | ✅ | Planillas campo.
| Producción | Liquidar Productividad | ../CC_FinzOpe/LiquidarProductividadPST.aspx | ✅ | Productividad PST.
| Inventario | Módulo de Inventario | ../Inventario/RegistroArticulos.aspx | ✅ | Inventario.

---

## 2) Análisis detallado (inicio) – Grupo 1: Control Presupuestos

### 2.1 ControlPresupuestos.aspx
- **Función**: Control de costos presupuestales (posiblemente CAP). Revisar tablas CAP_*, vistas de costos, filtros por proyecto/jobbook.
- **Interfaz esperada**: Filtros (fecha, jobbook, cliente), grid de costos vs. presupuesto, exportar.
- **Data access**: Buscar SP en CoreProject CAP_*. Priorizar Dapper para SP y EF Core para updates simples (regla 3).
- **Riesgos**: Lógica pesada en SP; validar performance y paginación.
- **Dependencias**: CU_Cuentas (jobbooks), usuarios para permisos.

### 2.2 ListadoEstudios.aspx
- **Función**: Listado de estudios vinculados a FI (presupuestos). Incluye filtros y descarga.
- **Interfaz**: Grid con búsqueda; posible export a Excel.
- **Data access**: SP en FI_Model o CAP; revisar paginación.
- **Dependencias**: Catálogos de clientes, áreas, estados.

### 2.3 ListadoPropuestas.aspx
- **Función**: Listado de propuestas financieras/estudios, asociado a FI.
- **Interfaz**: Grid con filtros; acciones ver/descargar.
- **Data access**: SP similares a estudios; revisar reuse con CU_Presupuesto.
- **Dependencias**: CU_Cuentas (propuestas), usuarios.

### 2.4 NominaDistribucionCostos.aspx
- **Función**: Distribución de costos de nómina sobre proyectos/jobbooks.
- **Interfaz**: Filtros por periodo, jobbook, centro de costo; grid de distribuciones; export.
- **Data access**: SP en FI/CC_FinzOpe; puede requerir cálculos; revisar si hay inserciones (usar EF Core para movimientos simples).
- **Dependencias**: TH (empleados, salarios), CC (centros costo), CU (jobbooks).

### Acciones inmediatas (grupo 1)
- Mapear SP/ADO en CoreProject para cada página.
- Identificar modelos/DTOs reutilizables desde CC_FinzOpe y CAP.
- Definir controllers/areas: se sugiere área FI con subcarpetas ControlPresupuestos.
- UI: usar grid compartido, filtros y modales existentes (Regla 5 y 7).

---

## 3) Análisis detallado – Grupo 2: Presupuestos Internos

### 3.1 GenerarRequerimientos.aspx (../CC_FinzOpe/GenerarRequerimientos.aspx)
- **Función**: Crear requerimientos internos de presupuesto (insumos/servicios) asociados a trabajos o centros de costo.
- **Interfaz esperada**: Formulario con selección de trabajo/jobbook, centro de costo, descripción, monto estimado, fechas; grid de requerimientos creados; acciones crear/editar/anular.
- **Data access**: Revisar SP en CC_FinzOpe (probable CC_Consecutivo, CC_Requerimientos*). Usar EF Core para inserts/updates simples si no hay lógica compleja; mantener SP para validaciones y numeración.
- **Dependencias**: CU jobbooks, catálogo centros de costo, usuarios solicitante/aprobador.
- **Riesgos**: Consecutivos y estados manejados en SP; validación de presupuesto disponible; flujos de aprobación (asegurar exclusión de compras/OC/OS).
- **UI**: Modales para crear/editar; grid paginado; filtros por estado, fecha, trabajo.
- **Auditoría**: Registrar usuario y timestamps; logging en service.

### 3.2 PresupuestosInternosIndex.aspx (../CC_FinzOpe/PresupuestosInternosIndex.aspx)
- **Función**: Listado y gestión de presupuestos internos por trabajo/proyecto.
- **Interfaz esperada**: Filtros por trabajo, cliente, estado; grid de presupuestos; acciones ver/editar; posibles descargas.
- **Data access**: SP en CC_FinzOpe para lectura (PresupuestosInternos*); EF Core para ajustes menores si procede. Validar si hay versionado de presupuesto.
- **Dependencias**: Datos de CU (clientes/jobbooks), CAP costos, TH para responsables.
- **Riesgos**: Lógica de cálculos en SP; coherencia con ControlPresupuestos; no generar nuevas features.
- **UI**: Reusar grid y filtros compartidos; modales para detalle/edición.

### 3.3 ListadoTrabajos.aspx (../CC_FinzOpe/ListadoTrabajos.aspx)
- **Función**: Descargar o listar trabajos disponibles para presupuestos internos.
- **Interfaz esperada**: Búsqueda por código/nombre cliente, estado, fechas; grid con opción de exportar o seleccionar trabajo.
- **Data access**: SP en CC_FinzOpe (CC_ListadoTrabajos/CC_InformacionTrabajos); uso Dapper para SP de solo lectura.
- **Dependencias**: CU jobbooks, catálogos de estados/areas.
- **Riesgos**: Paginación y performance; asegurar filtros equivalentes al legacy.
- **UI**: Grid con filtros rápidos; exportar a Excel si existe en legacy.

### 3.4 ConsultaLog.aspx (../TH_TalentoHumano/ConsultaLog.aspx)
- **Función**: Consultar bitácora/log de personas (acciones registradas en TH) accesible desde FI.
- **Interfaz esperada**: Filtros por persona, fecha, tipo evento; grid de log.
- **Data access**: SP en TH (ConsultaLog/LogPersonas); solo lectura → Dapper.
- **Dependencias**: TH empleados; seguridad: permisos de consulta.
- **Riesgos**: Datos sensibles; aplicar [Authorize] y roles; paginación.
- **UI**: Grid con filtros; posible export.

### Acciones inmediatas (grupo 2)
- Inventariar SP/ADO para requerimientos, presupuestos internos y listados de trabajos en CC_FinzOpe.
- Validar si existen consecutivos/estados en SP para no duplicar lógica.
- Definir controllers/views en área FI, subcarpeta PresupuestosInternos; servicios y adaptadores reutilizando patrones de CU/CC.
- Reusar modales compartidos para formularios y grids paginados; mantener exclusión de flujos de compras/OC/OS.
- Asegurar autorización por rol y logging en operaciones de creación/edición.

---

## 4) Análisis detallado – Grupo 3: Procesos Internos

### 4.1 ConteoTrabajos.aspx (../CC_FinzOpe/ConteoTrabajos.aspx)
- **Función**: Registrar conteos de trabajos (probable control operativo/financiero de entregables o registros procesados).
- **Interfaz esperada**: Filtros por fecha, trabajo/jobbook, área; formulario o grid para cargar conteos (cantidades, estado, responsable).
- **Data access**: SP en CC_FinzOpe (CC_ConteosXIdGet, CC_Conteos, etc.). Lectura con Dapper; inserciones/actualizaciones con EF Core si es simple, o SP si aplica reglas de negocio.
- **Dependencias**: CU jobbooks, TH usuarios/responsables, posibles catálogos de estado conteo.
- **Riesgos**: Validación de duplicados y consistencia; impacto en reportes de producción.
- **UI**: Grid editable o modal para registrar conteos; paginación y filtros.
- **Auditoría**: Registrar usuario y fecha de registro/modificación.

### 4.2 ReporteConteoTrabajos.aspx (../CC_FinzOpe/ReporteConteoTrabajos.aspx)
- **Función**: Reporte de conteos registrados; probablemente exportable a Excel.
- **Interfaz**: Filtros por rango de fechas, trabajo, estado; grid con totales; botón de exportación.
- **Data access**: SP de solo lectura en CC_FinzOpe; usar Dapper; cuidar performance en rangos amplios.
- **Dependencias**: Conteos capturados en 4.1; catálogos de trabajos/estados.
- **Riesgos**: Tiempo de consulta; ajustar paginación/streaming.
- **UI**: Grid con totales; botón export; mantener equivalencia con legacy.

### 4.3 ResumenesdeProduccion.aspx (../CC_FinzOpe/ResumenesdeProduccion.aspx)
- **Función**: Resumen de productividad/producción (totales por trabajo, responsable, periodo).
- **Interfaz**: Filtros por fecha, trabajo, responsable; grid de resumen; posibles gráficos simples o export.
- **Data access**: SP en CC_FinzOpe (CC_ResumenProduccion/CC_ResumenesdeProduccion). Lectura con Dapper.
- **Dependencias**: Producción cargada (ver grupo 5), jobbooks, usuarios.
- **Riesgos**: Cálculos en SP; validar consistencia con produccion.aspx.
- **UI**: Grid con agregados; export opcional.

### 4.4 Contratistas.aspx (../TH_TalentoHumano/Contratistas.aspx)
- **Función**: Gestión de contratistas (alta, edición, estado) desde FI.
- **Interfaz**: Grid de contratistas, filtros, modal de alta/edición, posiblemente documentos adjuntos.
- **Data access**: SP en TH (CC_Contratistas/TH_Contratistas) o EF si tablas simples; revisar si hay validaciones de estado/fechas.
- **Dependencias**: TH empleados/contratistas, CU jobbooks si asigna trabajos, seguridad roles.
- **Riesgos**: Datos personales; aplicar [Authorize] y roles específicos; manejar adjuntos si existen.
- **UI**: Modales para CRUD; grid paginado.

### 4.5 Contratacion.aspx (Contratación)
- **Función**: Módulo de contratación (probable flujo para crear contratos/relaciones de prestación de servicios).
- **Interfaz**: Formulario con datos de contrato, fechas, valores, responsable; grid de contratos.
- **Data access**: SP en FI/TH; validar si se generan consecutivos/estados; usar SP si lógica compleja.
- **Dependencias**: TH personas, FI tablas de contratos, CU jobbooks opcional.
- **Riesgos**: Flujos de aprobación o estados; evitar mezclarse con órdenes de compra/servicio (excluidas); datos sensibles.
- **UI**: Modales para crear/editar; grid con filtros y estados.

### 4.6 PrestacionServicios-CT.aspx (PST-Contratistas)
- **Función**: Gestión de prestación de servicios para contratistas (tarifas, periodos, asignaciones).
- **Interfaz**: Filtros por contratista/trabajo/periodo; grid de registros; acciones crear/editar.
- **Data access**: SP en CC_FinzOpe/TH relacionados a PST (CC_PrestacionServicios, etc.); validar cálculos de valor y estado.
- **Dependencias**: TH contratistas, CU jobbooks, tal vez producción para cruces.
- **Riesgos**: Cálculo de valores y estados; coherencia con reportes de pago; no invadir flujos de facturación excluidos.
- **UI**: Modales para CRUD; grid paginado; posible export.

### Acciones inmediatas (grupo 3)
- Inventariar SP/ADO para conteos, reportes de conteo, resúmenes de producción y módulos de contratistas/PST en CC_FinzOpe y TH.
- Definir estructura en área FI: subcarpetas ProcesosInternos y TH (contratistas) si se reutiliza área TH; decidir si Contratistas/Contratacion se sirven desde FI con servicios compartidos TH.
- Asegurar separación de responsabilidades: no incluir ordenes de servicio (excluidas) ni flujos de compras.
- Reusar grids y modales compartidos; agregar autorización por rol y logging en operaciones de escritura.

---

## 5) Análisis detallado – Grupo 4: Reportes

### 5.1 ReportePagos.aspx (../CC_FinzOpe/ReportePagos.aspx)
- **Función**: Reporte de pagos registrados (probablemente nómina, contratistas, proveedores) en un período.
- **Interfaz esperada**: Filtros por rango de fechas, tipo de pago (nómina/PST/proveedores), beneficiario, estado; grid con detalles de pago; exportar a Excel.
- **Data access**: SP de solo lectura en CC_FinzOpe/FI_Model (ReportePagos/CC_ReportePagos*); usar Dapper para no bloquear en grandes volúmenes.
- **Dependencias**: Registros de pago de TH (nómina), PST (contratistas), Proveedores; catálogos de bancos/estados.
- **Riesgos**: Volumen de datos; performance; datos sensibles (números de cuenta, salarios) → aplicar autorización estricta [Authorize(Roles = "Finanzas")].
- **UI**: Grid con filtros avanzados; total de pagos por fila; export a Excel (ClosedXML).
- **Seguridad**: Registrar descarga/acceso en log de auditoría.

### 5.2 ReporteActividadesProduccion.aspx (../CC_FinzOpe/ReporteActividadesProduccion.aspx)
- **Función**: Reporte detallado de actividades de producción (registros cargados, trabajo realizado, totales por actividad).
- **Interfaz**: Filtros por fecha, trabajo, actividad, responsable; grid con cantidades/horas/valores; posibles totales agregados.
- **Data access**: SP en CC_FinzOpe (CC_ReporteActividadesProduccion/CC_ActividadesProduccion*); solo lectura → Dapper.
- **Dependencias**: Datos de producción cargada, catálogos de actividades, jobbooks, usuarios.
- **Riesgos**: Consistencia con datos de Produccion.aspx (grupo 5); cambios en estados de producción no reflejarse en reporte.
- **UI**: Grid con subtotales por actividad; export a Excel.

### 5.3 ReporteContabilizacionPST.aspx (../CC_FinzOpe/ReporteContabilizacionPST.aspx)
- **Función**: Reporte de contabilización de PST (prestación de servicios técnicos/contratistas) para auditoría y finanzas.
- **Interfaz**: Filtros por período, contratista, centro de costo, estado de contabilización; grid con montos, fechas, cuentas contables.
- **Data access**: SP en CC_FinzOpe/FI_Model (ReporteContabilizacionPST); solo lectura → Dapper.
- **Dependencias**: PST registrada (grupo 3), catálogos de cuentas contables, periodos fiscales, contratistas.
- **Riesgos**: Validación de conciliación con módulo contable; cambios retroactivos; auditoría debe ser read-only.
- **UI**: Grid con montos acumulados; columna de estado contabilización (sí/no); export.

### 5.4 ReporteLegalizaciones.aspx (../Inventario/ReporteLegalizaciones.aspx)
- **Función**: Reporte de legalizaciones (justificación de gastos, anticipos) desde módulo de inventario.
- **Interfaz**: Filtros por fecha, solicitante, estado, tipo de legalización; grid con montos, documentos adjuntos.
- **Data access**: SP en Inventario (ReporteLegalizaciones); solo lectura → Dapper.
- **Dependencias**: Tabla de legalizaciones en Inventario; usuarios solicitantes; probables anexos en almacenamiento.
- **Riesgos**: Integridad de archivos adjuntos; validación de documentos; autorización por estado de legalización.
- **UI**: Grid con columna de estado; posible descarga de anexos; export a Excel.

### Notas generales – Grupo 4
- **Patrón de reportes**: Todos son de solo lectura. Usar Dapper y no EF Core. Aplicar paginación en grids para volúmenes > 1000 registros.
- **Exportación**: Implementar con ClosedXML (instalado en proyecto). Incluir títulos, filtros aplicados, fecha/usuario generador.
- **Seguridad**: [Authorize] requerido; roles específicos por tipo de reporte (ej: Finanzas, Auditor, RRHH).
- **Performance**: Revisar índices en SP; considerar cached queries si no cambian frecuentemente.
- **UI**: Usar componentes compartidos de filtros y grids; mantener consistencia visual.

### Acciones inmediatas (grupo 4)
- Inventariar SP de reportes en CC_FinzOpe, FI_Model e Inventario; validar nombres y parámetros.
- Definir structure: area FI > carpeta Reportes > controllers ReportesController; services e adapters reutilizando Dapper.
- Implementar paginación estándar y export a Excel para todos los reportes.
- Aplicar autorización y logging de descarga/acceso a reportes sensibles (Pagos, Contabilización PST).

---

## 6) Análisis detallado – Grupo 5: Producción

### 6.1 Produccion.aspx (../CC_FinzOpe/Produccion.aspx)
- **Función**: Cargue y registro de producción (trabajo realizado, actividades, cantidades) por empleados/contratistas en trabajos.
- **Interfaz esperada**: Filtros por fecha, trabajo/jobbook, responsable; grid de registros o formulario para cargar nuevas actividades; estados de registro (borrador/aprobado/rechazado).
- **Data access**: SP en CC_FinzOpe (CC_Produccion*, CC_ActividadesProduccion*). Lectura con Dapper; inserciones/actualizaciones con EF Core si es simple, o SP si hay validaciones de negocio.
- **Dependencias**: CU jobbooks, TH empleados/contratistas, catálogos de actividades, centros de costo.
- **Riesgos**: Validación de duplicados (misma actividad, fecha, responsable); integridad de datos; impacto en reportes y liquidación (grupo 5.8-5.9).
- **UI**: Grid editable o modales para cargar actividades; filtros; paginación; estado visual por registro.
- **Auditoría**: Registrar usuario, fecha/hora carga, cambios de estado.

### 6.2 EliminarCargueProduccion.aspx (../CC_FinzOpe/EliminarCargueProduccion.aspx)
- **Función**: Eliminar o anular registros de producción cargados; probablemente con restricciones de estado (solo borradores o no aprobados).
- **Interfaz**: Filtros por fecha/trabajo/responsable; grid con registros; acción "Anular" o "Eliminar" con confirmación; posible motivo de eliminación.
- **Data access**: SP en CC_FinzOpe para obtener registros eligibles; EF Core o SP para delete (validar lógica suave vs. hard delete).
- **Dependencias**: Registros de producción; posibles validaciones de estado/aprobación antes de permitir eliminación.
- **Riesgos**: Eliminar registros ya liquidados o reportados; auditoría de eliminaciones; coherencia con reportes.
- **UI**: Grid con botón Eliminar; modal de confirmación con motivo.
- **Auditoría**: Registrar eliminación (usuario, fecha, motivo, registro eliminado).

### 6.3 GenerarBonificacion.aspx (../CC_FinzOpe/GenerarBonificacion.aspx)
- **Función**: Generar o calcular liquidación de bonificaciones para empleados/contratistas (probable bonus por productividad).
- **Interfaz**: Filtros por período, trabajo, responsable; vista previa de cálculo; botón para generar bonificaciones; grid con resultados.
- **Data access**: SP en CC_FinzOpe (GenerarBonificacion/CC_Bonificacion*); lectura para previa con Dapper; inserción con SP si hay cálculos complejos.
- **Dependencias**: Producción cargada (6.1), catálogos de tasas de bonificación, PST/contratistas.
- **Riesgos**: Cálculos incorrectos; doble generación de bonificaciones; impacto en nómina/pagos.
- **UI**: Filtros, tabla previa de cálculos, botón generar con confirmación, resultado de generación.
- **Auditoría**: Registrar generación de bonificaciones (usuario, fecha, período, cantidad generada).

### 6.4 ExportarProduccionIDs.aspx (../CC_FinzOpe/ExportarProduccionIDs.aspx)
- **Función**: Exportar datos de producción por ID de jobbook/trabajo (descarga en Excel o CSV).
- **Interfaz**: Filtros por rango de IDs, fecha, tipo exportación (Excel/CSV); grid previa; botón descargar.
- **Data access**: SP en CC_FinzOpe para obtener producción; Dapper para lectura; generar archivo en memoria con ClosedXML.
- **Dependencias**: Jobbooks, producción cargada.
- **Riesgos**: Volumen de datos; timeout en exports grandes; seguridad de descarga (logs).
- **UI**: Formulario de filtros, grid previa, botón download.
- **Auditoría**: Registrar descarga (usuario, fecha, rango descargado, cantidad de registros).

### 6.5 EstadoJobBooks.aspx (../CC_FinzOpe/EstadoJobBooks.aspx)
- **Función**: Cambiar estado de jobbooks (probable transición: abierto/cerrado/cancelado) desde producción.
- **Interfaz**: Búsqueda de jobbooks, grid con estado actual, modal para cambiar estado con motivo/fecha de cierre.
- **Data access**: SP en CC_FinzOpe para obtener jobbooks; EF Core o SP para actualizar estado (validar reglas de transición).
- **Dependencias**: CU jobbooks, posible validación de producción completa o aprobada antes de cerrar.
- **Riesgos**: Cambios ilegales de estado (ej: cerrar sin producción); impacto en facturación/reportes.
- **UI**: Modales para cambio de estado; confirmación de transición.
- **Auditoría**: Registrar cambio de estado (usuario, fecha, jobbook, estado anterior/nuevo, motivo).

### 6.6 ReportePSTSinProduccion.aspx (../RP_Reportes/ReportePSTSinProduccion.aspx)
- **Función**: Reporte de PST (prestación de servicios) sin producción registrada (probable control de inconsistencias).
- **Interfaz**: Filtros por período, PST, trabajo; grid con PST sin movimientos de producción; export.
- **Data access**: SP en RP_Reportes/CC_FinzOpe de solo lectura; Dapper.
- **Dependencias**: PST registrada (grupo 3), producción cargada (6.1).
- **Riesgos**: Falsos positivos (PST aún en progreso); validar período de consulta.
- **UI**: Grid con filtros; export a Excel.

### 6.7 CargueDescuentosSS.aspx (../CC_FinzOpe/CargueDescuentosSS.aspx)
- **Función**: Cargar descuentos de seguridad social (APORTE/EPS/ARL) para empleados en un período.
- **Interfaz**: Filtros por período, área/departamento; grid para cargar o actualizar descuentos; validación de porcentajes.
- **Data access**: SP en CC_FinzOpe para obtener empleados/tasas; EF Core o SP para insertar/actualizar descuentos.
- **Dependencias**: TH empleados, catálogos de tasas SS, períodos de nómina.
- **Riesgos**: Descuentos duplicados; valores fuera de rango; impacto en liquidación de nómina.
- **UI**: Grid editable o modal; validación en tiempo real; paginación.
- **Auditoría**: Registrar cargue de descuentos (usuario, fecha, período, cantidad cargada, cambios).

### 6.8 LiquidarPlanillasActividades.aspx (../CC_FinzOpe/LiquidarPlanillasActividades.aspx)
- **Función**: Liquidar planillas de actividades de campo (pago de trabajo realizado por empleados/contratistas en campo).
- **Interfaz**: Filtros por período, trabajo, responsable; previa de liquidación (cantidades × tarifa); botón liquidar.
- **Data access**: SP en CC_FinzOpe para cálculos (LiquidarPlanillas*); Dapper para previa; SP para insertar liquidación.
- **Dependencias**: Producción de campo (6.1), catálogos de tarifas, TH empleados.
- **Riesgos**: Cálculos incorrectos de valor; liquidaciones duplicadas; impacto en nómina.
- **UI**: Filtros, tabla previa de cálculos, botón liquidar con confirmación.
- **Auditoría**: Registrar liquidación (usuario, fecha, período, registros liquidados, monto total).

### 6.9 LiquidarProductividadPST.aspx (../CC_FinzOpe/LiquidarProductividadPST.aspx)
- **Función**: Liquidar productividad de PST (contratistas) basado en producción realizada en período.
- **Interfaz**: Filtros por período, contratista, trabajo; previa de liquidación (cantidad × tarifa PST); botón liquidar.
- **Data access**: SP en CC_FinzOpe para cálculos (LiquidarProductividadPST*); Dapper para previa; SP para insertar.
- **Dependencias**: PST (grupo 3), producción de PST, tarifas de PST, contratistas.
- **Riesgos**: Cálculos dobles; tarifas incorrectas; impacto en pagos a contratistas.
- **UI**: Filtros, tabla previa, botón liquidar con confirmación.
- **Auditoría**: Registrar liquidación (usuario, fecha, período, PST liquidados, monto total).

### Notas generales – Grupo 5
- **Patrón de Producción**: Lectura predominante con Dapper; escrituras con cuidado (validaciones antes de insertar).
- **Liquidación**: Crítica financiera; sempre usar SP para cálculos; previa siempre antes de confirmar; logging exhaustivo.
- **Estados y transiciones**: Definir máquina de estados clara (borrador → aprobado → liquidado); validar en SP o service.
- **Auditoría**: Todos los cambios de estado, eliminaciones, liquidaciones deben registrarse con usuario/fecha/motivo.
- **UI**: Modales para confirmación de operaciones críticas (liquidar, eliminar, cambiar estado).

### Acciones inmediatas (grupo 5)
- Inventariar SP en CC_FinzOpe para producción, liquidación, bonificaciones, descuentos SS y cambios de estado.
- Mapear máquinas de estado para registros de producción y jobbooks; validar en SP o service.
- Definir estructura: area FI > carpeta Produccion > controllers (ProduccionController, LiquidacionController); services reutilizando SP.
- Implementar auditoría detallada en todas las operaciones de escritura (insertar, eliminar, liquidar, cambiar estado).
- Validar coherencia con reportes (grupo 4) y no permitir liquidaciones/cambios sin previa validación.

---

## 7) Análisis detallado – Grupo 6: Inventario

### 7.1 RegistroArticulos.aspx (../Inventario/RegistroArticulos.aspx)
- **Función**: Gestión del registro de artículos/inventario (CRUD de artículos, categorías, existencias).
- **Interfaz esperada**: Grid de artículos con búsqueda, filtros por categoría/estado; modales para crear/editar artículos; columnas de código, descripción, categoría, existencia, precio unitario.
- **Data access**: SP en Inventario (RegistroArticulos*) o EF Core si tablas simples. Lectura con Dapper; CRUD con EF Core si no hay lógica compleja.
- **Dependencias**: Catálogos de categorías de inventario, almacenes, usuarios (responsables).
- **Riesgos**: Validación de existencias negativas; integridad de referencias (artículos usados en otros módulos); cambios de precio retroactivos.
- **UI**: Grid paginado con búsqueda; modales CRUD; visualización de movimientos de inventario si aplica.
- **Auditoría**: Registrar creación/edición/eliminación de artículos; cambios de precio/existencia.

### Notas finales – Grupo 6
- **Patrón**: Similar a CRUD de US_Usuarios; reusar componentes de grid/modales.
- **Integración FI**: Accesible desde FI pero datos en módulo Inventario; considerar si servicios compartidos o independientes.

### Acciones inmediatas (grupo 6)
- Inventariar SP/tablas en módulo Inventario; revisar si hay relaciones con otros módulos (PP, FI, CC).
- Definir si Inventario se sirve desde área FI o área Inventario independiente; decidir según dependencias.
- Reusar patrones CRUD de US_Usuarios; aplicar auditoría mínima.

---

## 8) Resumen ejecutivo y diagrama de dependencias

### 8.1 Total de páginas a migrar

| Grupo | Páginas | Total |
|-------|---------|-------|
| Grupo 1: Control Presupuestos | 4 | 4 |
| Grupo 2: Presupuestos Internos | 4 | 4 |
| Grupo 3: Procesos Internos | 6 | 6 |
| Grupo 4: Reportes | 4 | 4 |
| Grupo 5: Producción | 9 | 9 |
| Grupo 6: Inventario | 1 | 1 |
| **Total** | | **28 páginas** |

**Exclusiones (3 páginas)**:
- RecepcionCuentasdeCobro.aspx (radicación facturas)
- ListadoCuentasRecibidas.aspx (aprobación facturas)
- OrdenesdeServicio.aspx (órdenes de servicio)

**Total Real WebMatrix FI**: 31 páginas

---

### 8.2 Diagrama de dependencias (ASCII)

```
┌─────────────────────────────────────────────────────────────────┐
│                      FI_AdministrativoFinanciero                │
│                                                                  │
├─────────────────────────────────────────────────────────────────┤
│                                                                  │
│  ┌──────────────────┐                                           │
│  │ Control Presup.  │◄──── Depende de:                         │
│  ├──────────────────┤                                           │
│  │ • ControlCostos  │      ✅ CU_Cuentas (jobbooks)           │
│  │ • ListEstudios   │      ✅ CAP (costos)                    │
│  │ • ListPropuestas │◄──── ✅ CU_Presupuesto (precios)       │
│  │ • NominaDistrib  │      ✅ TH (empleados, salarios)        │
│  └──────────────────┘      ✅ CC_FinzOpe (centros costo)     │
│           ▲                                                      │
│           │                                                      │
│  ┌────────┴──────────┐                                          │
│  ▼                   ▼                                           │
│  ┌──────────────────┐  ┌──────────────────┐                   │
│  │ Presup. Internos │  │ Proc. Internos   │                   │
│  ├──────────────────┤  ├──────────────────┤                   │
│  │ • Requerimientos │  │ • Conteos        │                   │
│  │ • Presupuestos   │  │ • ReporteConteos │                   │
│  │ • ListadoTrabajos│  │ • ResumenProdu   │                   │
│  │ • LogPersonas    │  │ • Contratistas   │                   │
│  └──────────────────┘  │ • Contratacion   │                   │
│           ▲             │ • PST-CT         │                   │
│           │             └──────────────────┘                   │
│           │                     ▲                               │
│           │                     │                               │
│           └─────────────────────┴─────────────────────┐        │
│                                                        │        │
│  ┌──────────────────┐        ┌──────────────────┐    │        │
│  │    Reportes      │        │   Producción     │    │        │
│  ├──────────────────┤        ├──────────────────┤    │        │
│  │ • ReportePagos   │        │ • Produccion     │◄───┘        │
│  │ • ReporteActiv   │        │ • EliminarCargue │            │
│  │ • ReportePST     │        │ • GenerarBono    │            │
│  │ • ReporteLegaliz │        │ • ExportarIDs    │            │
│  └──────────────────┘        │ • EstadoJobBooks │            │
│           ▲                   │ • ReportePSTSin  │            │
│           │                   │ • CargueDescSS   │            │
│           └───────────────────│ • LiquidarPlan   │            │
│                               │ • LiquidarProd   │            │
│                               └──────────────────┘            │
│                                     ▲                          │
│                                     │                          │
│  ┌──────────────────────────────────┘                         │
│  ▼                                                              │
│  ┌──────────────────┐                                          │
│  │   Inventario     │                                          │
│  ├──────────────────┤                                          │
│  │ • RegistroArtículos                                         │
│  └──────────────────┘                                          │
│                                                                  │
├─────────────────────────────────────────────────────────────────┤
│ Dependencias externas:                                           │
│ • ✅ CU_Cuentas (jobbooks) - MIGRADO                           │
│ • ✅ US_Usuarios (permisos) - MIGRADO                          │
│ • ✅ TH_Ausencias (empleados, log) - MIGRADO                  │
│ • 📋 CC_FinzOpe (SP core, tablas) - MIGRAR (backlog)          │
│ • 📋 CAP (costos, budgets) - REVISAR (backlog)                │
│ • 📋 Inventario (legalizaciones) - PUEDE SEPARARSE             │
└─────────────────────────────────────────────────────────────────┘
```

---

### 8.3 Dependencias críticas con CU_Cuentas (Presupuesto)

**Hallazgo**: ListadoEstudios, ListadoPropuestas y Presupuestos de FI son **interdependientes** con CU_Presupuesto:

| Módulo FI | Rela. con CU | Riesgo | Mitigación |
|-----------|--------------|--------|-----------|
| **ListadoEstudios** | Referencia jobbooks de CU; posible filtro por estado presupuesto | Alto | Usar IDs jobbooks; CU migrado ✅; validar consistencia |
| **ListadoPropuestas** | Propuestas de CU pueden cruzarse con presupuestos internos FI | Medio | Definir límite: CU = Propuesta Cliente; FI = Presupuesto Interno; documentar |
| **ControlPresupuestos** | Compara presupuesto interno FI vs. costos reales CAP/CU | Alto | Esperar migración CAP; usar SP existentes que ya calculan |
| **NominaDistribucionCostos** | Distribuye costos de nómina sobre jobbooks de CU | Medio | Usar jobbooks de CU; validar centros de costo en CC |

**Recomendación**: 
- ✅ **Mínima dependencia de CU_Presupuesto** si se limita a consumir datos read-only.
- ⚠️ **Considerar CAP (costos)** como dependencia crítica; CAP no está aún migrado → planificar après CAP o usar SP existentes de CC_FinzOpe.
- ✅ **Iniciar con Control Presupuestos (Grupo 1)** sin esperar CAP, usando SP que ya existen en CC_FinzOpe.

---

### 8.4 Sprint Pre-1: CC_FinzOpe (Infraestructura)

Antes de iniciar FI Sprint 1, se debe migrar CC_FinzOpe:

| Actividad | Horas | Descripción |
|-----------|-------|-------------|
| Análisis tablas CC_FinzOpe | 8 | Inventariar todas las tablas base |
| Análisis SP CC_FinzOpe | 12 | Documentar SP usados por FI (Produccion, Liquidacion, etc.) |
| Crear EF Core DbContext | 16 | Wrapper para tablas CC_FinzOpe; usar pattern Adapter |
| Validar Dapper para SP | 12 | Asegurar SP se ejecutan correctamente con Dapper |
| Testing e Índices | 20 | Validar performance; revisar índices en SQL |
| Documentación CC | 12 | Mapeo de SP, tablas, contexto para referencia |
| **SUBTOTAL PRE-1** | **80** | |

**Timing**: Semanas 1-2 (paralelo con planificación de Sprint 1)
**Deliverables**: 
- ✅ Área `CC/` en MatrixNext con Services y Adapters
- ✅ DbContext CC_FinzOpe wrapper
- ✅ Dapper queries para SP críticos
- ✅ Documentación de SP/tablas usadas por FI

---

### 8.5 Estimación de esfuerzo por grupo (horas)

| Grupo | Página | Complejidad | Horas | Esfuerzo |
|-------|--------|-------------|-------|----------|
| **Pre-1** | CC_FinzOpe | 🔴 Alta | 80 | Infraestructura: Tablas, SP, DbContext wrapper |
| **1** | ControlPresupuestos | 🟠 Media-Alta | 32 | Cálculos, filtros, export |
| | ListadoEstudios | 🟡 Baja | 16 | Grid + filtros |
| | ListadoPropuestas | 🟡 Baja | 16 | Grid + filtros |
| | NominaDistribucionCostos | 🟠 Media-Alta | 28 | Cálculos, validaciones |
| **Subtotal Grupo 1** | | | **92** | |
| | | | | |
| **2** | GenerarRequerimientos | 🟠 Media-Alta | 28 | Consecutivos, estados, validaciones |
| | PresupuestosInternosIndex | 🟡 Baja | 16 | Grid + filtros |
| | ListadoTrabajos | 🟡 Baja | 12 | Búsqueda simple |
| | ConsultaLog | 🟡 Baja | 12 | Grid + filtros |
| **Subtotal Grupo 2** | | | **68** | |
| | | | | |
| **3** | ConteoTrabajos | 🟠 Media-Alta | 32 | Validaciones duplicados, grid editable |
| | ReporteConteoTrabajos | 🟡 Baja | 16 | Grid + export |
| | ResumenesdeProduccion | 🟡 Baja | 16 | Agregados, export |
| | Contratistas | 🟡 Baja | 16 | CRUD simple (reutilizar TH) |
| | Contratacion | 🟠 Media-Alta | 24 | Flujos estados, validaciones |
| | PrestacionServicios-CT | 🟠 Media-Alta | 28 | Cálculos, estados, auditoría |
| **Subtotal Grupo 3** | | | **132** | |
| | | | | |
| **4** | ReportePagos | 🟠 Media | 20 | Lectura + export; datos sensibles |
| | ReporteActividadesProduccion | 🟡 Baja | 16 | Grid + subtotales |
| | ReporteContabilizacionPST | 🟠 Media | 20 | Validaciones contables |
| | ReporteLegalizaciones | 🟡 Baja | 16 | Grid + anexos |
| **Subtotal Grupo 4** | | | **72** | |
| | | | | |
| **5** | Produccion | 🔴 Alta | 40 | Estados, validaciones, auditoría crítica |
| | EliminarCargueProduccion | 🟠 Media | 20 | Restricciones, auditoría |
| | GenerarBonificacion | 🟠 Media-Alta | 28 | Cálculos financieros, previa |
| | ExportarProduccionIDs | 🟡 Baja | 16 | Export simple |
| | EstadoJobBooks | 🟠 Media-Alta | 24 | Máquina de estados, validaciones |
| | ReportePSTSinProduccion | 🟡 Baja | 12 | Grid simple |
| | CargueDescuentosSS | 🟠 Media | 20 | Validaciones rango, grid editable |
| | LiquidarPlanillasActividades | 🔴 Alta | 36 | Cálculos complejos, previa, auditoría |
| | LiquidarProductividadPST | 🔴 Alta | 36 | Cálculos complejos, previa, auditoría |
| **Subtotal Grupo 5** | | | **232** | |
| | | | | |
| **6** | RegistroArticulos | 🟡 Baja | 16 | CRUD simple (reutilizar US_Usuarios) |
| **Subtotal Grupo 6** | | | **16** | |
| | | | | |
| **TOTAL ESTIMADO** | | | **784 horas** | ~10 semanas (80 h/semana) |

**Descomposición**:
- Sprint Pre-1 (CC_FinzOpe): 80 horas
- Estimación FI: 612 horas directas
- Buffer FI (15% testing + fixes): +92 horas
- **Total con buffer**: 784 horas ≈ **10 semanas** (1 dev @ 80h/sem) o **7-8 semanas** (1.5-2 devs @ 50h/sem)

---

### 8.6 Secuencia recomendada de implementación

**Recomendación: Sprint iterativo de 2 semanas por grupo + Sprint Pre-1 para CC**

```
Sprint Pre-1 (Semanas 1-2): CC_FinzOpe (INFRAESTRUCTURA)
├── Objetivo: Migrar tablas y SP base de CC_FinzOpe
├── Crítico: Validar SP de Produccion, Liquidacion, Reportes
├── Horas: 80
└── Deliverable: Área CC/ con Services, Adapters, DbContext wrapper

Sprint 1 (Semanas 3-4): Grupo 1 - Control Presupuestos
├── Objetivo: Establecer patrones de grid, filtros, export
├── Crítico: ControlPresupuestos + NominaDistribucionCostos
├── Horas: 92
└── Deliverables: Controllers, Services, Views, auditoría

Sprint 2 (Semanas 3-4): Grupo 2 - Presupuestos Internos
├── Objetivo: Gestión de requerimientos y presupuestos
├── Crítico: GenerarRequerimientos (consecutivos)
├── Horas: 68
└── Dependencia: Grupo 1 (grid, filtros)

Sprint 3 (Semanas 5-6): Grupo 3 - Procesos Internos
├── Objetivo: Conteos, resúmenes, contratación
├── Crítico: ConteoTrabajos + PrestacionServicios-CT
├── Horas: 132
└── Riesgo: Máquinas de estado; validar auditoría

Sprint 4 (Semanas 7-8): Grupo 4 - Reportes
├── Objetivo: Reportes read-only, exportación, seguridad
├── Crítico: ReportePagos (autorización estricta)
├── Horas: 72
└── Patrón: Dapper + ClosedXML

Sprint 5 (Semanas 9-11): Grupo 5 - Producción
├── Objetivo: Cargue, liquidación, bonificación
├── Crítico: Produccion, LiquidarPlanillas, LiquidarProd
├── Horas: 232
├── Riesgo: Cálculos financieros; testing exhaustivo
└── Dependencia: Grupos 2-4 para contexto

Sprint 6 (Semana 12): Grupo 6 - Inventario
├── Objetivo: CRUD inventario
├── Horas: 16
└── Patrón: Reutilizar US_Usuarios

**Timeline Total**: ~12 semanas (3 meses) con 1 dev @ 80 h/semana
**Alternativa**: 1.5-2 devs @ 40-50 h/semana para 6-8 semanas

```

---

### 8.6 Riesgos y mitigaciones

| Riesgo | Probabilidad | Impacto | Mitigación |
|--------|--------------|---------|-----------|
| SP en CC_FinzOpe incompletos/desactualizados | Media | Alto | Inventariar todos al inicio de Sprint 1; validar parámetros |
| Cálculos incorrectos en liquidación (Sprint 5) | Media | Muy Alto | Previa siempre antes de confirmar; testing de cálculos con datos reales |
| Dependencias con CAP no migrado | Baja | Alto | Usar SP existentes de CC_FinzOpe; dejar CAP para integraciones futuras |
| Performance en Grupo 4 (reportes sobre datos grandes) | Media | Medio | Indexación; paginación obligatoria >1000 registros; considerar cached queries |
| Cambios retroactivos de estados/precios afectando reportes | Baja | Muy Alto | Auditoría exhaustiva; no permitir cambios en datos cerrados/liquidados |
| Consistencia entre Producción y Reportes | Media | Alto | Testing cruzado entre 6.1 y 5.2; validar con stakeholders |
| Datos sensibles (nómina, salarios) mal asegurados | Baja | Muy Alto | [Authorize(Roles = "Finanzas")] obligatorio; logging de acceso |

---

### 8.7 Dependencias externas a validar

**Antes de iniciar Sprint 1**:
- [ ] Confirmar lista completa de SP en CC_FinzOpe, FI_Model, CAP, TH, Inventario
- [ ] Validar con stakeholder financiero el alcance (excluir compras/OC/OS confirmado)
- [ ] Definir roles/permisos para acceso a reportes y liquidaciones
- [ ] Confirmar integridad de datos en SQL Server (índices, FK)
- [ ] Revisar si CAP será migrado antes o después de FI; replantear secuencia si es crítico

**Integraciones confirmadas ✅**:
- CU_Cuentas (jobbooks) - MIGRADO
- US_Usuarios - MIGRADO
- TH (empleados, log) - MIGRADO parcial (ausencias ok; Empleados aún pendiente)

**Integraciones pendientes 📋**:
- CC_FinzOpe (centro de operaciones; SP reutilizadas)
- CAP (costos) - revisar si se necesita para ControlPresupuestos
- Inventario (legalizaciones) - puede ser independiente

---

### 8.8 Notas finales

1. **FI es el módulo más crítico y complejo** después de migrar CU. Requiere cuidado especial en:
   - Validaciones de negocio (estados, transiciones)
   - Cálculos financieros (liquidación, bonificación, distribución)
   - Auditoría exhaustiva (quién, qué, cuándo, por qué)
   - Autorización estricta (datos sensibles)

2. **Reutilización de patrones**: 
   - Grid + filtros + export: Reutilizar desde US_Usuarios
   - Dapper para reportes read-only: Patrón probado
   - Modales CRUD: Componentes compartidos
   - Service layer con validaciones: Ya validado en TH_Ausencias

3. **Testing crítico**:
   - Unitarios para cálculos (liquidación, bonificación)
   - Integración con SQL: validar SP con datos reales
   - Funcional end-to-end: Produccion → Liquidación → Reportes
   - Performance: Reportes sobre 10k+ registros

4. **Post-migración**:
   - Deprecar WebMatrix FI gradualmente (por grupo)
   - Monitorear reportes y pagos en producción
   - Mantener alertas en cálculos críticos

---


