# MIGRACIÓN FI_AdministrativoFinanciero → MatrixNext

**Estado**: Análisis inicial (en progreso)
**Alcance**: Formularios referenciados desde WebMatrix/FI_AdministrativoFinanciero/Default.aspx, excluyendo creación/aprobación de compras, órdenes de compra/servicio y radicación/aprobación de facturas.
**Fuente base**: WebMatrix/FI_AdministrativoFinanciero/Default.aspx (menú de navegación).
**Reglas**: Aplican todas las directrices en DIRECTRICES_MIGRACION.md (SP existentes, naming DB, Areas, modales, sin features nuevas).

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

## 3) Próximos grupos propuestos
- Grupo 2: Presupuestos Internos (Requerimientos, Presupuestos, ListadoTrabajos, LogPersonas).
- Grupo 3: Procesos Internos (Conteos, ReporteConteos, ResumenProductividad, Contratistas, Contratacion, PrestacionServicios-CT).
- Grupo 4: Reportes (ReportePagos, ReporteProduccion, ReporteContabilizacionPST, Reporte Legalizaciones).
- Grupo 5: Producción (Produccion, EliminarCargue, GenerarBonificacion, ExportarProduccionIDs, EstadoJobBooks, ReportePSTSinProduccion, CargueDescuentosSS, LiquidarPlanillasActividades, LiquidarProductividadPST).
- Grupo 6: Inventario (RegistroArticulos).

> ¿Confirmas que avance con el **Grupo 2: Presupuestos Internos** con el mismo nivel de detalle? (Se mantienen exclusiones de compras/OC/OS y radicación/aprobación de facturas.)
