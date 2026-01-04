# ANÁLISIS CU_PRESUPUESTO - MIGRACIÓN A MATRIXNEXT

**Documento de Análisis Técnico - Fase 2**  
**Versión**: 1.0  
**Fecha de Creación**: 2026-01-03  
**Módulo**: CU_Cuentas (Presupuesto de Proyectos)  
**Alcance**: Fase 2 - Presupuesto.aspx  
**Analista**: GitHub Copilot  
**Estado**: 🔄 EN ANÁLISIS

---

## 📋 ÍNDICE

1. [Resumen Ejecutivo](#1️⃣-resumen-ejecutivo)
2. [Inventario del Legado](#2️⃣-inventario-del-legado-tabla)
3. [Flujos Funcionales](#3️⃣-flujos-funcionales-detallado)
4. [Mapa de Migración 1:1](#4️⃣-mapa-de-migración-11-tabla)
5. [Base de Datos y Stored Procedures](#5️⃣-base-de-datos-y-stored-procedures)
6. [Riesgos y Consideraciones](#6️⃣-riesgos-y-consideraciones)
7. [Componentes Reutilizables](#7️⃣-componentes-reutilizables-matrixnext-existentes)
8. [Backlog Inicial](#8️⃣-backlog-inicial-priorización)
9. [Checklist de Verificación](#9️⃣-checklist-de-verificación-pre-migración)
10. [Decisiones Técnicas Clave](#🔟-decisiones-técnicas-clave)
11. [Estimación Preliminar](#1️⃣1️⃣-estimación-preliminar)
12. [Próximos Pasos](#1️⃣2️⃣-próximos-pasos-post-análisis)

---

## 1️⃣ RESUMEN EJECUTIVO

### Propósito del Módulo

**Presupuesto.aspx** es el módulo **central de cotización y presupuestación** de proyectos de investigación de mercados en WebMatrix. Su propósito es:

1. **Gestión de Alternativas Presupuestales**: Crear múltiples alternativas de presupuesto para una misma propuesta comercial, permitiendo:
   - Diferentes enfoques metodológicos (F2F, CATI, Online)
   - Variaciones de técnicas (Entrevistas, Grupos Focales, Mystery Shopper, etc.)
   - Fases diferenciadas (Nacional vs. Regional)
   - Comparación de costos y márgenes entre alternativas

2. **Presupuestación Detallada con IQuote (Sistema Interno de Cotización)**:
   - **Configuración de Parámetros**: Grupo objetivo, preguntas, duración cuestionario, incidencia, productividad
   - **Muestra Estadística**: Definición de muestra por NSE, región, edad, género (F2F, CATI, Online)
   - **Procesos de Investigación**: Campo, verificación, crítica, codificación, data processing
   - **Actividades Subcontratadas**: Logística, compra de productos, reclutamiento, alquiler
   - **Análisis Estadístico**: Modelos avanzados (factoriales, clusters, regresiones, etc.)
   - **Horas Profesionales**: Estimación de horas por rol (Senior PM, PM, Analysts, etc.)
   
3. **Cálculo Automatizado de Costos**:
   - **Costos de Operación**: Basados en muestra, productividad, días de campo, estructura salarial
   - **Viáticos**: Hoteles, transporte, alimentación por ciudad/región
   - **Gross Margin**: Cálculo automático de margen de utilidad sobre costo operativo
   - **Valor de Venta**: Precio final al cliente con markup configurado

4. **Workflow de Revisión y Aprobación**:
   - Marca de revisión por Gerente de Operaciones
   - Solicitud de autorización de Gross Margin no estándar
   - Exportación a JBI (JobBook Interno) y JBE (JobBook Externo)
   - Envío de presupuestos a clientes

5. **Importación/Duplicación de Presupuestos**: Reutilización de presupuestos de propuestas anteriores para agilizar cotización

### Roles de Usuario

Basado en evidencia concreta del código:

| Rol | Permisos | Evidencia |
|-----|----------|-----------|
| **Gerente de Cuentas** | Crear/editar alternativas, presupuestos, muestra, parámetros | `Presupuesto.aspx.vb` (línea 563): `LoadInfoJobBook()` carga datos de `Session("InfoJobBook")` que contiene `IdUsuario` |
| **Gerente de Operaciones** | Marcar presupuestos como revisados/no revisados | `gvPresupuestos_RowCommand` (líneas 221-241): Comandos `ReviewP` y `UndoReviewP` |
| **Directores** | Autorizar cambios de Gross Margin | Inferido por `gvSolicitudes` (línea 978) que muestra solicitudes de autorización |
| **Sistema (Unidad)** | Restricción de visibilidad por unidad de negocio | `SavePresupuesto()` (línea 902): `ParUnidad = oUni.ObtenerUnidadXid(...)` |

### Dependencias Clave

| Dependencia | Tipo | Descripción | Evidencia |
|-------------|------|-------------|-----------|
| **Propuestas** (Fase 1) | Módulo | Presupuesto **depende** de que exista una Propuesta aprobada | `LoadInfoJobBook()` (línea 563) carga `InfoJobBook` que vincula a propuesta |
| **CoreProject.Cotizador.General** | Clase de Negocio | Motor de cálculo de presupuestos (IQuote) | `SavePresupuesto()` (línea 898): `New CoreProject.Cotizador.General` |
| **IQ_Parametros** | Entidad | Tabla principal de parámetros de presupuesto | `SavePresupuesto()` (líneas 898-976): Guardado completo de parámetros |
| **IQ_DatosGeneralesPresupuesto** | Entidad | Datos generales de alternativa (descripción, días, mediciones) | `SaveGeneralValues()` (líneas 253-282): IQDG con campos de alternativa |
| **IQ_Muestra_1** | Entidad | Detalle de muestra estadística (F2F, CATI, Online) | `SaveMuestra()` (línea 1030) |
| **UC_Header_Presupuesto.ascx** | UserControl | Formulario complejo con 50+ controles (preguntas, procesos, configuraciones) | `btnAddPresupuestos_Click` (líneas 46-65): Acceso a controles del UC |
| **Session State** | Aplicación | `Session("InfoJobBook")`, `Session("IDUsuario")` | Uso extensivo en toda la página |
| **ClosedXML.Excel** | Librería | Importación de muestra desde Excel | `Presupuesto.aspx.vb` (línea 2): `Imports ClosedXML.Excel` |

| **DevExpress** | Librería Legacy | GridViews personalizados (si aplica) | ⚠️ POR CONFIRMAR en código completo |

### Complejidad Estimada

**🔴 ALTA**

| Factor | Nivel | Justificación |
|--------|-------|---------------|
| **Lógica de Negocio** | 🔴 Alta | Algoritmos complejos de cotización (costos, productividad, gross margin, viáticos) en `CoreProject.Cotizador.General` |
| **Volumen de Código** | 🔴 Alta | **3,309 líneas** en `.vb` (más de 6x Default.aspx), **1,568 líneas** en `.aspx` |
| **UserControl Anidado** | 🔴 Alta | `UC_Header_Presupuesto.ascx` con **744 líneas** de controles (50+ campos de entrada) |
| **Grids Dinámicos** | 🔴 Alta | 14+ GridViews (presupuestos, muestra F2F, CATI, Online, JBI, JBE, costos, viáticos, etc.) |
| **Maestro-Detalle Complejo** | 🔴 Alta | Alternativa (maestro) → Presupuestos (detalle) → Muestra (sub-detalle) → Actividades (sub-detalle) |
| **Dependencias de ViewState** | 🔴 Alta | UpdatePanels extensivos para evitar postbacks completos |
| **Cálculos en Tiempo Real** | 🔴 Alta | JavaScript client-side + Server-side para totales, productividad, días de campo |
| **Importación de Datos** | 🟠 Media | Excel (ClosedXML), Duplicación de alternativas completas |
| **Exportación Especializada** | 🟠 Media | JBI, JBE (formatos específicos de JobBook) |


**Complejidad Total**: **ALTA** (significativamente más compleja que Fase 1 por:
- Lógica algorítmica de costos y márgenes
- UserControl con 50+ campos
- 14+ grids interrelacionados
- Importación/exportación especializada
- Dependencia del motor IQuote)

---

## 2️⃣ INVENTARIO DEL LEGADO (TABLA)

| Archivo | Funcionalidad Principal | Eventos/Postbacks Relevantes | Dependencias (SP/Clases) | Session/ViewState/QueryString | Evidencia |
|---------|-------------------------|------------------------------|--------------------------|-------------------------------|-----------|
| **Presupuesto.aspx** (1,568 LOC) + **Presupuesto.aspx.vb** (3,309 LOC) | **Cotización completa de proyectos de investigación**:<br><br>1️⃣ **Gestión de Alternativas** (Datos generales: descripción, días, mediciones)<br>2️⃣ **Presupuestos por Técnica** (F2F, CATI, Online) con parámetros IQuote<br>3️⃣ **Muestra Estadística** (NSE, región, edad, género, dificultad)<br>4️⃣ **Actividades Subcontratadas** (Logística, reclutamiento, etc.)<br>5️⃣ **Análisis Estadístico** (Modelos avanzados)<br>6️⃣ **Horas Profesionales** (Por rol y actividad)<br>7️⃣ **Cálculo Automatizado** (Costos, gross margin, valor venta)<br>8️⃣ **Revisión/Aprobación** (Workflow)<br>9️⃣ **Importación/Exportación** (Excel, JBI/JBE)<br>🔟 **Duplicación** (Copiar alternativas completas) | **Eventos Principales**:<br>• `Page_Load` (línea 30): Carga `InfoJobBook` desde Session<br>• `btnGuardar_Click` (línea 38): Guarda presupuesto completo (llama 10+ métodos)<br>• `btnAddPresupuestos_Click` (línea 46): Abre modal para nuevo presupuesto<br>• `btnNewAlternativa_Click` (línea 67): Crea nueva alternativa<br>• `btnSaveGeneral_Click` (línea 245): Guarda datos generales de alternativa<br>• `ddlAlternativa_SelectedIndexChanged` (línea 111): Carga presupuestos de alternativa<br>• `ddlTecnica_SelectedIndexChanged` (línea 115): Filtra presupuestos por técnica + carga muestra<br>• `ddlMetodologia_SelectedIndexChanged` (línea 146): Habilita/deshabilita controles según metodología<br>• `gvPresupuestos_RowCommand` (línea 159): CRUD presupuestos (Edit, Delete, Copy, Review, Details, Simulator, Exec, CalcProfessionalTime, JBI, JBE)<br>• `gvMuestraF2F_RowCommand`, `gvMuestraCATI_RowCommand`, `gvMuestraOnline_RowCommand`: CRUD muestra<br>• `btnImportar_Click` (línea 284): Importa presupuestos de otra propuesta<br>• `btnLoadDataExcel_Click`: Importa muestra desde Excel<br>• `btnRevision_Click` (línea 325): Marca alternativa para revisión<br>• `btnDuplicarAlternativa_Click` (línea 316): Duplica alternativa completa | **Stored Procedures (Confirmados vía Entity Framework)**:<br>• `CU_Presupuesto_Get` (línea 566 en CU_Model.Context.vb): `@id`, `@propuestaId` → Obtiene presupuesto<br>• `CU_Estudios_Presupuestos_Asignados_Get` (línea 574): Presupuestos asignados a estudio<br>• `CU_Presupuestos_JobBook_Edit` (línea 632): Actualiza JobBook en presupuesto<br>• `CU_PresupuestosRevisionPorGerenteOperaciones` (línea 904): Listado para revisión<br><br>**Clases de Negocio**:<br>• `CoreProject.Cotizador.General` (líneas 898, 1139, etc.): **Motor IQuote** - 30+ métodos:<br>&nbsp;&nbsp;- `GetPresupuesto()`, `GetExistsPresupuesto()`<br>&nbsp;&nbsp;- `PutSaveParametros()` (guarda `IQ_Parametros`)<br>&nbsp;&nbsp;- `PutDatosGenerales()` (guarda `IQ_DatosGeneralesPresupuesto`)<br>&nbsp;&nbsp;- `PutMuestra()`, `GetMuestraF2F()`, `GetMuestraCati()`, `GetMuestraOnline()`<br>&nbsp;&nbsp;- `PutPreguntas()`, `PutProcesos()`, `PutActividadesSubcontratadas()`<br>&nbsp;&nbsp;- `PutModelosEstadistica()`, `PutHorasProfesionales()`<br>&nbsp;&nbsp;- `PutValorVenta()`, `PutOP()` (Gross Margin)<br>&nbsp;&nbsp;- `GetCalculoDiasCampo()`, `GetCalculoProductividad()`<br>&nbsp;&nbsp;- `GetSimulador()` (cálculo completo de costos)<br>&nbsp;&nbsp;- `GetTotalMuestra()`, `GetUltimaAlternativa()`<br>• `CoreProject.Presupuesto` (línea 1171): Clase de acceso a datos<br>• `CoreProject.US.Unidades` (línea 902): Unidades de negocio<br>• `CU_JobBook.DAL` (línea 293): Info de propuestas para importar<br><br>**Entidades EF**:<br>• `IQ_Parametros` (línea 898): 60+ propiedades de configuración<br>• `IQ_DatosGeneralesPresupuesto` (línea 253)<br>• `IQ_Preguntas` (línea 1197)<br>• `IQ_ProcesosPresupuesto` (línea 1217)<br>• `IQ_Muestra_1` (línea 1030) | **Session**:<br>• `Session("InfoJobBook")` (línea 31): `oJobBook` con IdPropuesta, IdUnidad, etc.<br>• `Session("IDUsuario")` (línea 919): Int64 (usuario actual)<br><br>**ViewState** (implícito por UpdatePanels):<br>⚠️ **USO EXTENSIVO** en todos los grids para evitar postbacks<br><br>**HiddenFields**:<br>• `hfPropuesta.Value` (líneas 168, 201, etc.): IdPropuesta (Int64)<br>• `hfNewAlternativa.Value` (líneas 71, 256, 280): Boolean (indica si es nueva)<br>• `hfOPS.Value` (línea 199): Operational Performance Score<br>• `hfMetCodigoJBI`, `hfFaseJBI`, `hfMetCodigoJBE`, `hfFaseJBE`: Para modales JBI/JBE<br>• `hfMetCodigoCopiar`, `hfFaseCopiar`: Para copiar presupuesto<br>• `hfMetCodigoCostos`, `hfFaseCostos`: Para modal de costos<br><br>**QueryString**:<br>⚠️ **NO ENCONTRADO** en extract (validar código completo) | ✅ **CONFIRMADO**<br><br>⚠️ **PENDIENTES DE VALIDACIÓN**:<br>1. Código completo de métodos auxiliares (3,309 líneas)<br>2. Stored Procedures adicionales en `Cotizador.General`<br>3. DevExpress components (si se usan)<br>4. JavaScript client-side para cálculos |

### UserControl Anidado (Dependencia Crítica)

| Archivo | Funcionalidad | Controles Clave | Evidencia |
|---------|---------------|-----------------|-----------|
| **UC_Header_Presupuesto.ascx** (744 LOC) | **Formulario extendido** con parámetros de presupuesto:<br><br>• Grupo Objetivo (TextBox multi-línea)<br>• Preguntas: Cerradas, Cerradas múltiples, Abiertas, Abiertas múltiples, Otros, Demográficos<br>• Incidencia (DropDownList)<br>• Productividad (TextBox)<br>• Probabilístico (CheckBox)<br>• F2F Virtual (CheckBox)<br>• Complejidad Cuestionario (DropDownList)<br>• Encuestadores por punto<br>• Procesos: Campo, Verificación, Crítica, Codificación, DataClean, TopLines, Tablas, Archivos, Scripting<br>• Interceptación %, Reclutamiento %<br>• Producto: Tipo, Lotes, Unidades, Valor<br>• CLT: Tipo, Alquiler equipos<br>• Data Processing: Transformación, Unificación, Complejidad, Ponderación, Inputs/Outputs<br>• Product Testing: Apoyos, Compra, Neutralizador, Visitas, Celdas, Productos<br>• Grids: Análisis Estadístico, Actividades Subcontratadas, Horas Profesionales | `txtGrupoObjetivo` (línea 10)<br>`txtCerradas`, `txtCerradasMultiples`, `txtAbiertas`, `txtAbiertasMultiples`, `txtOtros`, `txtDemograficos` (líneas 17-47)<br>`ddlIncidencia` (línea 61)<br>`txtProductividad` (línea 66)<br>`chbProbabilistico` (línea 71)<br>`chbF2fVirtual` (línea 76)<br>`ddlComplejidadCuestionario` (línea 79)<br>`gvAnalisisEstadisticos`, `gvActividadesSubcontratadas`, `gvProfessionalTime`<br><br>⚠️ **MÁS DE 50 CONTROLES** - Ver código completo para inventario exhaustivo | ✅ **CONFIRMADO**<br><br>Acceso en `Presupuesto.aspx.vb`:<br>`DirectCast(UCHeader.FindControl("txtCerradas"), TextBox).Text` (líneas 48-52, 919-976)<br><br>**Métodos expuestos**:<br>• `UCHeader.ClearControls()` (línea 47)<br>• `UCHeader.ActividadesSubcontratadas()` (línea 1272)<br>• `UCHeader.AnalisisEstadisticos()` (línea 1277)<br>• `UCHeader.HorasProfesionales()` (línea 1282) |

### Grids Identificados (14+ GridViews)

| Grid | Función | DataSource | Comandos | Evidencia |
|------|---------|------------|----------|-----------|
| `gvPresupuestos` | Lista presupuestos de alternativa por técnica | `CargarPresupuestos()` (línea 658) | `EditP`, `DeleteP`, `CopyP`, `ReviewP`, `UndoReviewP`, `DetailsP`, `SimulatorP`, `ExecP`, `CalcProfessionalTimeP`, `JBEP`, `JBIP` | Línea 298-373 (.aspx) |
| `gvMuestraF2F` | Muestra Face-to-Face (NSE, región, etc.) | `oCot.GetMuestraF2F()` (línea 1047) | `DelMuestra` | Línea 470-485 |
| `gvMuestraCATI` | Muestra CATI (teléfono) | `oCot.GetMuestraCati()` (línea 1053) | `DelMuestra` | Línea 486-497 |
| `gvMuestraOnline` | Muestra Online (web surveys) | `oCot.GetMuestraOnline()` (línea 1059) | `DelMuestra` | Línea 498-512 |
| `GVJBI` | JobBook Interno (costos internos) | `CargarCostosJBI()` | N/A (readonly) | Línea 624-629 |
| `GVJBE` | JobBook Externo (precios cliente) | `CargarCostosJBE()` | N/A (readonly) | Línea 637-642 |
| `gvSolicitudes` | Solicitudes de autorización de GM | ⚠️ POR CONFIRMAR | N/A | Línea 978-986 |
| `gvDataSearchImport` | Búsqueda de propuestas para importar | `CU_JobBook.DAL.InfoJobBookGet()` (línea 293) | `SelectProp` | Línea 1056-1072 |
| `gvPresupuestosImport` | Presupuestos a importar | ⚠️ POR CONFIRMAR | `ImportAlternativa` | Línea 1157-1170 |
| `gvControlCostos` | Control de costos vs ejecutado | ⚠️ POR CONFIRMAR | N/A | Línea 1290-1322 |
| `gvDetallesOperaciones` | Detalle operaciones | ⚠️ POR CONFIRMAR | N/A | Línea 1332-1362 |
| `gvViaticos` | Viáticos (hoteles, transporte) | ⚠️ POR CONFIRMAR | N/A | Línea 1372-1385 |
| `gvPYGPresupuesto` | P&G (Profit & Growth) por presupuesto | ⚠️ POR CONFIRMAR | N/A | Línea 1395-1401 |
| `gvPYGAlternativa` | P&G por alternativa | ⚠️ POR CONFIRMAR | N/A | Línea 1411-1417 |

---

## 3️⃣ FLUJOS FUNCIONALES (DETALLADO)

### FLUJO 1: Crear Nueva Alternativa de Presupuesto

```
PASO 1: Usuario hace clic en "Nueva Alternativa"
├─ Evidencia: btnNewAlternativa_Click (Presupuesto.aspx.vb, línea 67)
├─ Validación: ValidarNuevaAlternativa() (línea 68)
│   ├─ Verifica que Session("InfoJobBook") exista y contenga JobBook válido
│   └─ Si falta JobBook: Muestra warning "recuerde guardar número de JobBook en área de propuesta"
├─ Acción:
│   ├─ Oculta panel de presupuestos: pnlPresupuestos.Visible = False (línea 69)
│   ├─ Muestra panel general: pnlGeneral.Visible = True (línea 70)
│   ├─ Marca como nueva: hfNewAlternativa.Value = True (línea 71)
│   ├─ Limpia campos:
│   │   ├─ txtDescripcionAlternativa.Text = String.Empty
│   │   ├─ txtObservacionesGeneral.Text = String.Empty
│   │   ├─ txtDiasCampo.Text = 10 (días de campo por defecto)
│   │   ├─ txtDiasDiseno.Text = 5
│   │   ├─ txtDiasInformes.Text = 3
│   │   ├─ txtDiasProceso.Text = 7
│   │   ├─ txtDiasTotal.Text = 25 (suma de los anteriores)
│   │   ├─ txtNoMediciones.Text = 1
│   │   └─ txtPeriodicidad.Text = 1 (mes)
│   └─ Limpia número IQuote: lblNumIQuote.Text = ""
└─ Estado: Panel general visible, campos listos para entrada

PASO 2: Usuario completa datos generales de alternativa
├─ Campos Requeridos:
│   ├─ txtDescripcionAlternativa (max 300 caracteres)
│   ├─ txtDiasCampo, txtDiasDiseno, txtDiasInformes, txtDiasProceso
│   ├─ txtNoMediciones (número de olas/waves)
│   ├─ txtPeriodicidad (meses entre mediciones)
│   ├─ chbObserver (Checked = Observer project, Unchecked = Full service)
│   └─ txtObservacionesGeneral (opcional)
├─ JavaScript Client-Side:
│   └─ TotalDias() (línea 9 en .aspx): Calcula automáticamente txtDiasTotal
│       └─ Formula: Diseño + Campo + Proceso + Informes
└─ Evidencia: Controles en líneas 69-85 (.aspx.vb)

PASO 3: Usuario hace clic en "Guardar Datos Generales"
├─ Evidencia: btnSaveGeneral_Click (línea 245)
├─ Validación: ValidateSaveGeneral() (línea 246)
│   ├─ Descripción no vacía
│   ├─ Días son numéricos > 0
│   └─ Si falla: Muestra error y sale
├─ Acción: SaveGeneralValues() (línea 250-282)
│   ├─ Crea/actualiza IQ_DatosGeneralesPresupuesto:
│   │   ├─ IQDG.IdPropuesta = hfPropuesta.Value
│   │   ├─ Si nueva: IQDG.ParAlternativa = GetUltimaAlternativa() + 1 (línea 256)
│   │   ├─ Si edición: IQDG.ParAlternativa = ddlAlternativa.SelectedValue
│   │   ├─ IQDG.Descripcion, Observaciones, Días..., Mediciones
│   │   ├─ IQDG.TipoPresupuesto = 2 (Observer) | 1 (Full service) (líneas 264-268)
│   │   └─ IQDG.Plazo, Saldo, Anticipo, TasaCambio (valores fijos: 30/30/70/4000)
│   ├─ Guarda: oCot.PutDatosGenerales(IQDG) (línea 271)
│   ├─ Actualiza: NumAlternativas() - Refresca dropdown ddlAlternativa
│   ├─ Selecciona nueva alternativa en dropdown
│   └─ Carga datos: CargarAlternativa(ddlAlternativa.SelectedValue)
├─ Resultado:
│   ├─ Panel general oculto, panel presupuestos visible
│   ├─ Botón "Nueva alternativa" cambia a "Nueva"
│   └─ hfNewAlternativa.Value = False
└─ Estado: Alternativa creada, lista para agregar presupuestos

⚠️ RIESGO IDENTIFICADO:
- SaveGeneralValues() guarda valores fijos (Plazo=30, TasaCambio=4000) que podrían requerir configuración dinámica
- TotalDias() es cálculo JavaScript, no validado server-side
```

### FLUJO 2: Crear Presupuesto con Parámetros IQuote

```
PASO 1: Usuario hace clic en "Agregar Presupuesto"
├─ Evidencia: btnAddPresupuestos_Click (línea 46)
├─ Acción:
│   ├─ Limpia UserControl: UCHeader.ClearControls() (línea 47)
│   ├─ Limpia grids de detalle:
│   │   ├─ gvAnalisisEstadisticos: ObtenerAnalisisEstadistico(0,0,0,0) + DataBind() (línea 48)
│   │   ├─ gvActividadesSubcontratadas: ObtenerActividadesSubcontratadas(0,0,0,0) (línea 50)
│   │   └─ gvProfessionalTime: ObtenerHorasProfesionales(0,0,0,0) (línea 52)
│   ├─ Reinicia dropdowns:
│   │   ├─ ddlFase.SelectedIndex = 0 (línea 53)
│   │   ├─ ddlTecnica.SelectedIndex = 0
│   │   └─ ddlMetodologia.SelectedIndex = 0 (si tiene items)
│   ├─ Oculta grids de muestra:
│   │   ├─ gvMuestraCATI.Visible = False
│   │   ├─ gvMuestraF2F.Visible = False
│   │   ├─ gvMuestraOnline.Visible = False
│   │   └─ lblTotalMuestra.Text = ""
│   └─ Muestra modal: lkb1_ModalPopupExtender.Show() (línea 65)
└─ Estado: Modal de presupuesto abierto, campos limpios

PASO 2: Usuario selecciona Técnica
├─ Evidencia: ddlTecnica_SelectedIndexChanged (línea 115)
├─ Opciones de Técnica (inferidas de código):
│   ├─ 100 = Face-to-Face (F2F)
│   ├─ 200 = CATI (Computer Assisted Telephone Interview)
│   └─ 300 = Online (Web surveys)
├─ Acción por Técnica:
│   ├─ F2F (100):
│   │   ├─ Carga metodologías F2F en ddlMetodologia
│   │   ├─ Habilita: ddlIncidencia (línea 891)
│   │   ├─ Si Metodología = 140, 130, 160: Muestra muestra Online
│   │   └─ Si no: Muestra muestra F2F (NSE, dificultad, poblacional)
│   ├─ CATI (200):
│   │   ├─ Carga metodologías CATI
│   │   ├─ Habilita: ddlIncidencia
│   │   └─ Muestra muestra CATI (ciudades, NSE)
│   └─ Online (300):
│   │   ├─ Carga metodologías Online
│   │   ├─ Deshabilita: ddlIncidencia
│   │   └─ Muestra muestra Online
│   └─ Carga presupuestos existentes: CargarPresupuestos(alternativa, tecnica) (línea 144)
├─ Controles Habilitados/Deshabilitados:
│   └─ ddlIncidencia.Enabled = True (solo F2F y CATI) vs. False (Online)
└─ Estado: Metodologías filtradas, muestra adecuada visible

PASO 3: Usuario selecciona Metodología
├─ Evidencia: ddlMetodologia_SelectedIndexChanged (línea 146)
├─ Acción:
│   ├─ Recarga muestra según metodología específica (línea 157)
│   └─ Actualiza controles específicos de metodología (⚠️ revisar código completo)
└─ Estado: Formulario adaptado a metodología

PASO 4: Usuario completa formulario en UC_Header_Presupuesto
├─ Campos Obligatorios (validados en ValidateSaveGeneralPresupuesto, línea 1110):
│   ├─ ddlTecnica.SelectedValue != "0"
│   ├─ ddlMetodologia.SelectedValue != "0"
│   ├─ ddlFase.SelectedValue != "0" (Nacional vs Regional)
│   ├─ txtDuracionMinutos: IsNumeric
│   ├─ txtGrupoObjetivo.Text.Length >= 3
│   ├─ Total preguntas > 0 (Cerradas + Cerradas Múltiples + Abiertas + Abiertas Múltiples + Otros)
│   └─ ddlIncidencia.SelectedIndex != 0 (si está habilitado)
├─ Campos Opcionales:
│   ├─ Preguntas demográficas (default = 15)
│   ├─ Procesos DP (DataClean, TopLines, Tablas, Archivos)
│   ├─ Encuestadores por punto
│   ├─ Productividad (se calcula auto si vacío)
│   ├─ Configuraciones Product Testing (si aplica)
│   ├─ Configuraciones CLT (si aplica)
│   └─ Data Processing inputs/outputs
├─ Validaciones Numéricas (ValidarDatosNumericosPresupuesto, línea 1163):
│   └─ Todos los TextBox numéricos: Si !IsNumeric → Asigna "0"
└─ Estado: 50+ campos completados, listos para guardar

PASO 5: Usuario hace clic en "Guardar Presupuesto"
├─ Evidencia: btnGuardar_Click (línea 38)
├─ Validación: ValidateSavePresupuesto() (línea 39)
│   ├─ Llama ValidarDatosNumericosPresupuesto() (asigna 0 a campos inválidos)
│   ├─ Llama ValidateSaveGeneralPresupuesto() (valida campos obligatorios)
│   └─ Si falla: Exit Sub sin guardar
├─ Acción Principal: SavePresupuesto() (línea 877-1176)
│   ├─ Validaciones pre-guardado (líneas 878-891):
│   │   ├─ ddlFase.SelectedIndex != 0
│   │   ├─ ddlMetodologia.SelectedIndex != 0
│   │   ├─ ddlTecnica.SelectedIndex != 0
│   │   └─ Si F2F/CATI: ddlIncidencia.SelectedIndex != 0
│   ├─ Verifica si presupuesto existe (línea 898):
│   │   └─ oCot.GetExistsPresupuesto(propuesta, alternativa, metodologia, fase)
│   ├─ Carga o crea IQ_Parametros:
│   │   ├─ Si existe: IQP = oCot.GetPresupuesto(...)
│   │   ├─ Si nuevo (línea 900):
│   │   │   ├─ IQP.ParUnidad = Unidad del usuario (de InfoJobBook)
│   │   │   └─ IQP.ParFechaCreacion = Date.UtcNow.AddHours(-5) (Colombia)
│   │   └─ Asigna 60+ propiedades de IQP (líneas 911-976):
│   │       ├─ IdPropuesta, ParAlternativa, MetCodigo, ParNacional, TecCodigo
│   │       ├─ ParNomPresupuesto (descripción alternativa, max 300 chars)
│   │       ├─ ParTotalPreguntas (suma de todos los tipos)
│   │       ├─ ParEncuestadoresPunto, ParTiempoEncuesta, ParValorDolar (4000)
│   │       ├─ ParNProcesosDC, ParNProcesosTopLines, ParNProcesosTablas, ParNProcesosBases
│   │       ├─ ParGrupoObjetivo, ParIncidencia, ParProbabilistico
│   │       ├─ ParPorcentajeIntercep, ParPorcentajeRecluta
│   │       ├─ ParUnidadesProducto, ParValorUnitarioProd, ParTipoCLT, ParAlquilerEquipos
│   │       ├─ ParAccesoInternet, ParObservaciones
│   │       ├─ ParUsaTablet = 1, ParUsaPapel = 0, ParAñoSiguiente = 1
│   │       ├─ Complejidad, F2FVirtual, ComplejidadCodificacion
│   │       ├─ DPTransformacion, DPUnificacion, DPComplejidad, DPPonderacion
│   │       ├─ DPIn/Out: Interna, Cliente, Panel, Externo, GMU, Otro, WebDelivery
│   │       ├─ PTApoyosPunto, PTCompra, PTNeutralizador, PTTipoProducto
│   │       ├─ PTLotes, PTVisitas, PTCeldas, PTProductosEvaluar
│   │       ├─ DPComplejidadCuestionario, ParProductividad
│   │       └─ Usuario = Session("IDUsuario")
│   ├─ Guarda parámetros: oCot.PutSaveParametros(IQP, NewPresupuesto) (línea 977)
│   ├─ Recalcula días de campo (línea 979-986):
│   │   ├─ Si no está revisado: txtDiasCampo = oCot.GetCalculoDiasCampo()
│   │   ├─ Si cambió: UpdatePanel manual + SaveGeneralValues()
│   │   └─ Actualiza IQ_DatosGeneralesPresupuesto con nuevo valor
│   ├─ Guarda componentes relacionados (líneas 987-991):
│   │   ├─ SavePreguntas() → IQ_Preguntas (tabla separada con desglose)
│   │   ├─ SaveProcesos(IQP) → IQ_ProcesosPresupuesto (tabla N:N)
│   │   ├─ SaveActSubcontratadas() → IQ_ActividadesSubcontratadas
│   │   ├─ SaveActEstadistica() → IQ_ModelosEstadistica
│   │   └─ SaveHorasProfesionales() → IQ_HorasProfesionales
│   ├─ Si tiene muestra > 0: EfectuarCalculos() (línea 992-994)
│   │   ├─ Calcula productividad (si no ingresada): oCot.GetCalculoProductividad()
│   │   ├─ Calcula valor de venta: oCot.PutValorVenta()
│   │   ├─ Calcula Operational Performance: oCot.GetSimulador()
│   │   └─ Guarda OP: oCot.PutOP()
│   └─ Actualiza botón revisión: btnRevision.Visible = True (si no está para revisar)
├─ Post-guardado:
│   ├─ Recarga grid: CargarPresupuestos(ddlAlternativa, ddlTecnica) (línea 41)
│   └─ Muestra mensaje: ShowWarning(Information, "Registro guardado") (línea 43)
└─ Resultado: Presupuesto guardado con todos sus componentes

⚠️ RIESGOS IDENTIFICADOS:
1. **IQ_Parametros tiene 60+ propiedades** → Migración compleja. Deben migrarse todas.
2. **Cálculos automáticos** (días campo, productividad, OP) → Lógica en CoreProject.Cotizador.General debe migrarse intacta
3. **UpdatePanel manual** (línea 982-985) → Eliminar en MVC, usar AJAX simple
4. **Guardado multi-tabla** (7 tablas) → Implementar transacción EF Core para garantizar atomicidad
5. **Validaciones dispersas** → Centralizar en FluentValidation
```

### FLUJO 3: Agregar Muestra Estadística (F2F, CATI, Online)

```
PASO 1: Usuario hace clic en "Agregar" (botón en sección Muestra)
├─ Evidencia: btnAddMuestra_Click (Presupuesto.aspx.vb, línea 329)
├─ Precondiciones: Presupuesto debe estar guardado con Técnica, Metodología, Fase seleccionadas
├─ Validaciones (líneas 330-362):
│   ├─ Si ddlCiudad habilitado Y (SelectedValue = "0" O SelectedIndex = 0):
│   │   └─ Error: "Seleccione primero la ciudad" + Exit
│   ├─ Si ddlDificultadMuestra.SelectedValue = "0":
│   │   └─ Error: "Seleccione el tipo de muestra antes de continuar" + Exit
│   ├─ Si !IsNumeric(txtCantidadMuestra.Text):
│   │   └─ Error: "Digite la cantidad antes de continuar" + Exit
│   ├─ Si ddlFase.SelectedIndex = 0:
│   │   └─ Error: "Por favor seleccione la fase antes de continuar" + Exit
│   ├─ Si ddlMetodologia.SelectedIndex = 0:
│   │   └─ Error: "Por favor seleccione la metodología antes de continuar" + Exit
│   ├─ Si ddlTecnica.SelectedIndex = 0:
│   │   └─ Error: "Por favor seleccione la técnica antes de continuar" + Exit
│   └─ Si (Técnica = F2F 100 O CATI 200) Y ddlIncidencia.SelectedIndex = 0:
│       └─ Error: "Por favor seleccione la incidencia antes de continuar" + Exit
└─ Estado: Validaciones pasadas

PASO 2: Crea entidad IQ_Muestra_1 (líneas 363-378)
├─ IQM.IdPropuesta = hfPropuesta.Value
├─ IQM.ParAlternativa = ddlAlternativa.SelectedValue
├─ IQM.ParNacional = ddlFase.SelectedValue (1=Nacional, 2=Regional)
├─ IQM.MetCodigo = ddlMetodologia.SelectedValue
├─ IQM.MuCantidad = txtCantidadMuestra.Text (número de encuestas)
├─ IQM.MuIdentificador = ddlDificultadMuestra.SelectedValue
│   └─ Para F2F: NSE (1=Alto, 2=Medio, 3=Bajo) o Dificultad (4=Alta, 5=Baja)
│   └─ Para CATI: Tipo de muestra (ciudades, NSE)
│   └─ Para Online: Dificultad (Alta/Baja)
├─ Si ddlCiudad.Enabled = False:
│   ├─ IQM.CiuCodigo = 0 (muestra nacional sin ciudad específica)
│   └─ IQM.DeptCodigo = 0
└─ Si ddlCiudad.Enabled = True:
    └─ IQM.CiuCodigo = ddlCiudad.SelectedValue (CODANE ciudad)

PASO 3: Limpia controles de entrada (líneas 374-377)
├─ txtCantidadMuestra.Text = 0
├─ ddlDificultadMuestra.SelectedIndex = 0
└─ Try: ddlCiudad.ClearSelection() (puede fallar si no tiene items)

PASO 4: Guarda muestra: SaveMuestra(IQM) (línea 379)
├─ Evidencia: SaveMuestra() (línea 1030-1040)
├─ Validación: ValidateSavePresupuesto() (si falla, Exit Sub)
├─ Verifica si presupuesto existe:
│   ├─ Si NO existe: SavePresupuesto() + flag = True (guarda presupuesto primero)
│   └─ Si existe: Continúa
├─ Acción: oCot.PutMuestra(Muestra) (guarda en tabla IQ_Muestra)
├─ Recarga: CargarMuestra() (línea 1042)
│   ├─ Oculta todos los grids: gvMuestraCATI, gvMuestraF2F, gvMuestraOnline = False
│   ├─ Según técnica seleccionada:
│   │   ├─ F2F (100) + Metodología != 140, 130, 160: MuestraF2F() (línea 1046)
│   │   │   └─ gvMuestraF2F: Columnas = CODANE, Ciudad, NSE5y6, NSE4, NSE123, Total
│   │   ├─ F2F (100) + Metodología = 140, 130, 160: MuestraOnline()
│   │   ├─ CATI (200): MuestraCATI() (línea 1052)
│   │   │   └─ gvMuestraCATI: Columnas = TipoMuestra, Cantidad
│   │   └─ Online (300): MuestraOnline() (línea 1058)
│   │       └─ gvMuestraOnline: Columnas = CODANE, Ciudad, AltaDificultad, BajaDificultad, Total
│   └─ lblTotalMuestra.Text = oCot.GetTotalMuestra() (suma todas las líneas)
└─ Si flag = True: SavePresupuesto() (recalcula costos con nueva muestra)

PASO 5: Reabre modal
└─ lkb1_ModalPopupExtender.Show() (línea 380)

⚠️ RIESGOS IDENTIFICADOS:
1. **Muestra F2F**: Desglose por NSE (5y6, 4, 123) requiere lógica especial en UI
2. **Muestra CATI**: Solo cantidad + tipo (sin ciudad), diferente estructura
3. **Muestra Online**: Dificultad (Alta/Baja) sin NSE
4. **Validación dependiente**: ddlCiudad solo habilitado para ciertas metodologías
5. **Recálculo automático**: SavePresupuesto() se ejecuta automáticamente al agregar muestra (puede ser lento)
```

### FLUJO 4: Eliminar Línea de Muestra

```
PASO 1: Usuario hace clic en icono "Borrar" (🗑️) en grid de muestra
├─ Confirmación JavaScript: confirm('¿Está seguro de borrar esta muestra?')
└─ Si acepta: Ejecuta RowCommand

PASO 2: Según Técnica, ejecuta comando específico
├─ F2F: gvMuestraF2F_RowCommand (línea 385-395)
│   ├─ Crea IQ_Muestra_1 con:
│   │   ├─ IdPropuesta, ParAlternativa, ParNacional, MetCodigo (de hiddenfields/dropdowns)
│   │   └─ CiuCodigo = gvMuestraF2F.DataKeys(rowIndex)("Codigo") (CODANE de ciudad)
│   ├─ Elimina: oCot.DELMuestra(muestra)
│   ├─ Recarga: CargarMuestra()
│   └─ Reabre modal: lkb1_ModalPopupExtender.Show()
├─ CATI: gvMuestraCATI_RowCommand (línea 398-408)
│   ├─ MuIdentificador = gvMuestraCATI.DataKeys(rowIndex)("IDENTIFICADOR")
│   ├─ CiuCodigo = 0 (CATI no usa ciudad específica)
│   └─ Resto igual que F2F
└─ Online: gvMuestraOnline_RowCommand (línea 410-420)
    ├─ CiuCodigo = gvMuestraOnline.DataKeys(rowIndex)("Codigo")
    └─ Resto igual que F2F

⚠️ RIESGO IDENTIFICADO:
- Eliminación directa sin validar si presupuesto ya tiene costos calculados
- No pregunta si desea recalcular costos automáticamente
```

### FLUJO 5: Editar Presupuesto Existente

```
PASO 1: Usuario hace clic en icono "Editar" (✏️) en grid de presupuestos
├─ Evidencia: gvPresupuestos_RowCommand, CommandName = "EditP" (línea 159, código no mostrado en extract)
├─ Acción inferida (por patrón de código):
│   ├─ Carga datos del presupuesto seleccionado
│   ├─ Llena UserControl: UCHeader con valores existentes
│   ├─ Llena muestra en grid correspondiente (F2F/CATI/Online)
│   └─ Abre modal: lkb1_ModalPopupExtender.Show()
└─ Estado: Modal abierto con datos precargados

PASO 2: Usuario modifica valores
└─ Edita campos en UC_Header_Presupuesto y/o muestra

PASO 3: Usuario hace clic en "Guardar"
├─ Ejecuta: btnGuardar_Click (línea 38)
├─ SavePresupuesto() detecta presupuesto existente (línea 898):
│   └─ IQP = oCot.GetPresupuesto(propuesta, alternativa, metodologia, fase)
├─ Actualiza propiedades de IQP (líneas 911-976)
├─ Guarda: oCot.PutSaveParametros(IQP, NewPresupuesto=False) (línea 977)
└─ Recalcula: EfectuarCalculos() si tiene muestra (línea 992)

⚠️ NOTA:
- NO se encontró código explícito de "EditP" en extract
- Requiere revisión de código completo para confirmar implementación exacta
- Inferencia basada en patrón estándar de CRUD
```

### FLUJO 6: Marcar Presupuesto como Revisado/No Revisado

```
PASO 1A: Usuario hace clic en icono "Marcar como Revisado" (☑️)
├─ Evidencia: gvPresupuestos_RowCommand, CommandName = "ReviewP" (líneas 221-232)
├─ Confirmación: confirm('¿Está seguro marcar como revisado este presupuesto?')
├─ Acción:
│   ├─ Carga presupuesto: IQP = oCot.GetPresupuesto(...) usando DataKeys del grid:
│   │   └─ MetCodigo = gvPresupuestos.DataKeys(rowIndex)("MetCodigo")
│   │   └─ Nacional = gvPresupuestos.DataKeys(rowIndex)("NACIONAL")
│   ├─ Actualiza campos:
│   │   ├─ IQP.ParFechaRevision = Date.UtcNow.AddHours(-5) (Colombia)
│   │   ├─ IQP.ParRevisado = True
│   │   └─ IQP.ParRevisadoPor = Session("IDUsuario").ToString
│   ├─ Guarda: oCot.PutSaveParametros(IQP, False)
│   └─ Recarga grid: CargarPresupuestos(alternativa, tecnica) (línea 232)
└─ Resultado: Presupuesto marcado como revisado, icono cambia a "Desmarcar"

PASO 1B: Usuario hace clic en icono "Desmarcar Revisión" (↶)
├─ Evidencia: gvPresupuestos_RowCommand, CommandName = "UndoReviewP" (líneas 233-243)
├─ Confirmación: confirm('¿Está seguro de quitar la marca de revisión este presupuesto?')
├─ Acción:
│   ├─ Carga presupuesto: IQP = oCot.GetPresupuesto(...)
│   ├─ Limpia campos:
│   │   ├─ IQP.ParFechaRevision = Nothing
│   │   ├─ IQP.ParRevisado = Nothing
│   │   └─ IQP.ParRevisadoPor = Nothing
│   ├─ Guarda: oCot.PutSaveParametros(IQP, False)
│   └─ Recarga grid: CargarPresupuestos(alternativa, tecnica)
└─ Resultado: Revisión quitada, icono cambia a "Marcar como Revisado"

⚠️ RIESGO IDENTIFICADO:
- Marca de revisión es manual, no valida si presupuesto cumple estándares
- Cualquier usuario con acceso puede marcar/desmarcar (validar permisos en migración)
```

### FLUJO 7: Ver JobBook Externo (JBE) / JobBook Interno (JBI)

```
PASO 1A: Usuario hace clic en botón "JBE" en grid de presupuestos
├─ Evidencia: gvPresupuestos_RowCommand, CommandName = "JBEP" (líneas 207-212)
├─ Acción:
│   ├─ Almacena: hfMetCodigoJBE.Value, hfFaseJBE.Value (de DataKeys)
│   ├─ Carga datos: CargarCostosJBE(propuesta, alternativa, metodologia, fase) (línea 211)
│   │   └─ Evidencia: CargarCostosJBE() (líneas 1518-1525)
│   │       ├─ Si chbObserver.Checked = True:
│   │       │   └─ DataSource = oCot.GetCostosJobBookExternoObserver()
│   │       ├─ Si no:
│   │       │   └─ DataSource = oCot.GetCostosJobBookExterno()
│   │       └─ Retorna DataSet con estructura:
│   │           ├─ Columna 0: Concepto (texto)
│   │           └─ Columna 1: Valor (decimal)
│   ├─ Formatea grid: GVJBE_RowDataBound (líneas 422-441)
│   │   ├─ Si concepto contiene "PORCENTAJE": Formato = "P2" (ej: 35.50%)
│   │   ├─ Si no: Formato = "C0" (ej: $1,234,567)
│   │   ├─ Si concepto contiene "TOTAL", "GROSS", "VENTA": Font.Bold = True
│   │   └─ Alineación derecha para valores
│   └─ Muestra modal: ModalPopupExtenderJBE.Show() (línea 212)
└─ Estado: Modal con JBE visible (costos para cliente)

PASO 1B: Usuario hace clic en botón "JBI" en grid de presupuestos
├─ Evidencia: gvPresupuestos_RowCommand, CommandName = "JBIP" (líneas 201-206)
├─ Acción:
│   ├─ Almacena: hfMetCodigoJBI.Value, hfFaseJBI.Value
│   ├─ Carga datos: CargarCostosJBI(propuesta, alternativa, metodologia, fase)
│   │   └─ Evidencia: CargarCostosJBI() (líneas 1527-1531)
│   │       └─ DataSource = oCot.GetCostosJobBookInterno() (costos reales internos)
│   ├─ Formatea grid: GVJBI_RowDataBound (líneas 443-458)
│   │   └─ Mismo formato que JBE (porcentajes, moneda, negritas)
│   └─ Muestra modal: ModalPopupExtenderJBI.Show()
└─ Estado: Modal con JBI visible (costos internos)

⚠️ NOTA IMPORTANTE:
- **JBE (JobBook Externo)**: Precios para cliente (con markup y gross margin)
- **JBI (JobBook Interno)**: Costos reales (sin markup, para análisis interno)
- Diferencia clave: JBE incluye márgenes, JBI muestra costo puro operativo
```

### FLUJO 8: Simular Gross Margin y Valor de Venta

```
PASO 1: Usuario hace clic en "Ajustes de venta y gross margin" (💲) en grid
├─ Evidencia: gvPresupuestos_RowCommand, CommandName = "DetailsP" (código no en extract)
├─ Acción inferida:
│   ├─ Carga modal con campos:
│   │   ├─ txtValorVentaSimular: Para ingresar nuevo valor de venta
│   │   ├─ txtNuevoGM: Para ingresar nuevo Gross Margin deseado
│   │   └─ txtGMOpera: Para ingresar Gross Margin de Operaciones
│   └─ Muestra modal: ModalPopupExtenderGM.Show()

PASO 2A: Simular GM basado en valor de venta
├─ Evidencia: btnSimular_Click (líneas 460-469)
├─ Usuario ingresa: txtValorVentaSimular (ej: $10,000,000)
├─ Hace clic: btnSimular
├─ Validación: Si txtValorVentaSimular vacío → Error
├─ Cálculo:
│   └─ lblGMsimulado.Text = (oCot.GetSimularGM(...) * 100).ToString("N2")
│       └─ Parámetros: propuesta, alternativa, metodologia, fase, valorVenta, modo=1
├─ Resultado: Muestra GM calculado (ej: "35.50 %")
└─ Modal permanece abierto: ModalPopupExtenderGM.Show()

PASO 2B: Simular valor de venta basado en GM deseado
├─ Evidencia: btnSimValorVenta_Click (líneas 471-488)
├─ Usuario ingresa:
│   ├─ txtNuevoGM (ej: 40 = 40%)
│   └─ txtGMOpera (opcional, ej: 35 = 35%)
├─ Hace clic: btnSimValorVenta
├─ Validación: Si ambos vacíos → Error
├─ Cálculos:
│   ├─ gmUni = txtNuevoGM / 100 (o -1 si vacío)
│   ├─ gmOpe = txtGMOpera / 100 (o -1 si vacío)
│   ├─ lblValorVentaSimulado = oCot.GetSimularVenta(...).ToString("C")
│   ├─ Recarga JBI: CargarJBI(..., gmUni, gmOpe, simulacion=True)
│   └─ Recarga JBE: CargarJBE(..., gmUni, gmOpe, simulacion=True)
├─ Resultado: Muestra valor de venta necesario + JBI/JBE actualizados
└─ Modal permanece abierto

⚠️ RIESGOS IDENTIFICADOS:
1. **Simulación vs Guardado**: Simulación NO guarda, requiere botón "Modificar GM" separado
2. **Autorización requerida**: Si GM < tope configurado → Requiere contraseña de usuario autorizado
3. **Lógica compleja**: Función AjustarGrossMargin() (líneas 1244-1447) con validaciones de seguridad
4. **Email de solicitud**: Si no autorizado, envía correo a DirectorOPS para aprobación
```

### FLUJO 9: Ver Ejecución vs Presupuestado (Control de Costos)

```
PASO 1: Usuario hace clic en "Ver ejecución" (ℹ️) en grid de presupuestos
├─ Evidencia: gvPresupuestos_RowCommand, CommandName = "ExecP" (líneas 195-200)
├─ Acción:
│   ├─ Almacena: hfMetCodigoCostos.Value, hfFaseCostos.Value
│   ├─ Carga detalles: CargarDetalleCostos(propuesta, alternativa, metodologia, fase)
│   │   └─ Evidencia: CargarDetalleCostos() (líneas 1533-1556)
│   │       ├─ gvControlCostos: oCot.GetCostos(..., tipo=1)
│   │       │   └─ Muestra resumen de costos por categoría
│   │       ├─ gvDetallesOperaciones: oCot.GetCostos(..., tipo=2)
│   │       │   └─ Evidencia: gvDetallesOperaciones_RowDataBound (líneas 1604-1625)
│   │       │       ├─ Totales: _Presupuestado2, _TotalHoras
│   │       │       ├─ Footer con TOTALES
│   │       │       └─ Si hfOPS.Value = 0: Oculta columnas 11, 12, 13 (datos operacionales)
│   │       ├─ gvViaticos: oCot.GetViaticos(...)
│   │       │   └─ Muestra hoteles, transporte, alimentación por ciudad
│   │       ├─ gvPYGPresupuesto: oCot.GetPyG(..., específico de presupuesto)
│   │       │   └─ Profit & Growth de este presupuesto
│   │       └─ gvPYGAlternativa: oCot.GetPyG(..., toda alternativa)
│   │           └─ Profit & Growth de toda la alternativa
│   └─ Muestra modal: ModalPopupExtenderExecution.Show()
└─ Estado: Modal con 5 grids de análisis financiero

⚠️ NOTA OPERACIONAL:
- Si hfOPS.Value = 1 (modo revisión OPS): Muestra TabPanel2 (datos sensibles)
- Si hfOPS.Value = 0 (modo normal): Oculta datos operacionales internos
- Útil para Gerentes de Operaciones que revisan ejecución vs presupuesto
```

---

## 4️⃣ MAPA DE MIGRACIÓN 1:1 (TABLA)

### Convenciones de Mapeo

- **Área**: `CU` (Cuentas) - Estructura modular ya existente en MatrixNext
- **Patrón**: Controller → Service → DataAdapter → Stored Procedures/EF
- **Modales**: Preferir modales Bootstrap para CRUD de detalles (muestra, actividades)
- **AJAX**: Reemplazar UpdatePanels con fetch API / jQuery AJAX

---

| WebForm Original | Funcionalidad | Ruta MVC | Controller | Action(s) | View | ViewModel(s) | Service/DAL | Componentes Reutilizables | Nota de Paridad |
|------------------|---------------|----------|------------|-----------|------|--------------|-------------|---------------------------|-----------------|
| **Presupuesto.aspx** (Listado de Alternativas) | Ver alternativas de presupuesto de una propuesta | `/CU/Presupuesto/{propuestaId}` | `PresupuestoController` | `Index(long propuestaId)` (GET) | `Index.cshtml` | `PresupuestoIndexViewModel` con:<br>• `List<AlternativaViewModel>`<br>• `InfoJobBookViewModel`<br>• Filtros | `PresupuestoService.ObtenerAlternativas()`<br>`PresupuestoDataAdapter` | ❌ **NO HAY** en Shared<br>✅ Crear `_AlternativaCard.cshtml` | • Panel lateral con alternativas<br>• Cada alternativa: Descripción, Días, Mediciones<br>• Botones: Nueva, Duplicar, Importar |
| **Presupuesto.aspx** (Panel Datos Generales) | Crear/editar datos generales de alternativa | `/CU/Presupuesto/EditarAlternativa` | `PresupuestoController` | `EditarAlternativa(long propuestaId, int? alternativaId)` (GET/POST) | `_ModalEditarAlternativa.cshtml` (Partial) | `EditarAlternativaViewModel` con:<br>• Descripcion (string, max 300)<br>• DiasCampo, DiasDiseno, DiasInformes, DiasProceso (int)<br>• NumMediciones, Periodicidad (int)<br>• TipoPresupuesto (Observer/Full) | `PresupuestoService.CrearAlternativa()`<br>`PresupuestoService.ActualizarAlternativa()` | ✅ `_Modal.cshtml` (ya existe en TH)<br>❌ **CREAR** `_NumericInput.cshtml` | • Modal Bootstrap con validaciones<br>• Cálculo automático Total Días (JS)<br>• Si nueva: Asigna número siguiente |
| **Presupuesto.aspx** (Modal Presupuesto - Form) | Crear/editar presupuesto con parámetros IQuote | `/CU/Presupuesto/EditarPresupuesto` | `PresupuestoController` | `EditarPresupuesto(long propuestaId, int alternativaId, int? metodologiaId, int? faseId)` (GET/POST) | `_ModalEditarPresupuesto.cshtml` (Partial **MUY GRANDE**) | `EditarPresupuestoViewModel` con:<br>• **IQParametrosViewModel** (60+ props)<br>• TecnicaId, MetodologiaId, FaseId<br>• DuracionMinutos, Complejidad<br>• **PreguntasViewModel**<br>• **ProcesosViewModel**<br>• **ConfiguracionesViewModel** (DP, PT, CLT) | `PresupuestoService.GuardarPresupuesto()`<br>`IQuoteCalculator` (nueva clase para lógica IQuote) | ❌ **CREAR**:<br>• `_PresupuestoFormTabs.cshtml` (tabs para organizar 60+ campos)<br>• `_PreguntasPanel.cshtml`<br>• `_ProcesosPanel.cshtml`<br>• `_ConfiguracionesPanel.cshtml` | • Dividir en TABS para UX:<br>&nbsp;&nbsp;1. General (Técnica, Metodología, Grupo Objetivo)<br>&nbsp;&nbsp;2. Preguntas & Procesos<br>&nbsp;&nbsp;3. Configuraciones Avanzadas (DP, PT, CLT)<br>&nbsp;&nbsp;4. Muestra<br>&nbsp;&nbsp;5. Actividades & Análisis<br>• Validaciones con FluentValidation<br>• AJAX para guardar sin cerrar modal |
| **Presupuesto.aspx** (Grid Presupuestos) | Listar presupuestos de alternativa por técnica | `/CU/Presupuesto/ObtenerPresupuestos` (AJAX) | `PresupuestoController` | `ObtenerPresupuestos(long propuestaId, int alternativaId, int? tecnicaId)` (GET JSON) | JSON (sin view) | `List<PresupuestoGridItemViewModel>` con:<br>• Id, MetodologiaId, Fase<br>• Muestra, Valor, GrossMargin<br>• Revisado, FechaRevision<br>• Acciones (Edit, Delete, Copy, etc.) | `PresupuestoService.ObtenerPresupuestosPorAlternativa()` | ✅ Usar DataTables.js o ag-Grid (client-side) | • Grid con 11 acciones:<br>&nbsp;&nbsp;Review, Edit, Delete, Copy, Details, Simulator, Exec, CalcProf, JBI, JBE<br>• Filtro por Técnica (dropdown)<br>• Iconos Font Awesome |
| **Presupuesto.aspx** (Muestra F2F) | Agregar/eliminar muestra Face-to-Face | `/CU/Presupuesto/Muestra/F2F` | `PresupuestoController` | `AgregarMuestraF2F(AgregarMuestraF2FRequest)` (POST JSON)<br>`EliminarMuestraF2F(long id)` (DELETE) | Partial en modal presupuesto | `AgregarMuestraF2FRequest` con:<br>• CiudadCodigo (CODANE)<br>• DificultadId<br>• Cantidad<br><br>`MuestraF2FViewModel` (para grid) | `PresupuestoService.AgregarMuestra()`<br>`PresupuestoService.EliminarMuestra()` | ❌ **CREAR**:<br>• `_MuestraF2FForm.cshtml`<br>• `_MuestraF2FGrid.cshtml` | • Form inline: Ciudad (dropdown con Chosen.js), Dificultad, Cantidad<br>• Grid: CODANE, Ciudad, NSE5y6, NSE4, NSE123, Total<br>• Eliminar con confirm (SweetAlert2) |
| **Presupuesto.aspx** (Muestra CATI) | Agregar/eliminar muestra CATI | `/CU/Presupuesto/Muestra/CATI` | `PresupuestoController` | `AgregarMuestraCATI(AgregarMuestraCATIRequest)` (POST JSON)<br>`EliminarMuestraCATI(long id)` (DELETE) | Partial en modal presupuesto | `AgregarMuestraCATIRequest` con:<br>• TipoMuestra<br>• Cantidad<br><br>`MuestraCATIViewModel` | `PresupuestoService.AgregarMuestra()` | ❌ **CREAR**:<br>• `_MuestraCATIForm.cshtml`<br>• `_MuestraCATIGrid.cshtml` | • Form: TipoMuestra (dropdown), Cantidad<br>• Grid: TipoMuestra, Cantidad<br>• **NO requiere ciudad** |
| **Presupuesto.aspx** (Muestra Online) | Agregar/eliminar muestra Online | `/CU/Presupuesto/Muestra/Online` | `PresupuestoController` | `AgregarMuestraOnline(AgregarMuestraOnlineRequest)` (POST JSON)<br>`EliminarMuestraOnline(long id)` (DELETE) | Partial en modal presupuesto | `AgregarMuestraOnlineRequest` con:<br>• CiudadCodigo<br>• Dificultad (Alta/Baja)<br>• Cantidad<br><br>`MuestraOnlineViewModel` | `PresupuestoService.AgregarMuestra()` | ❌ **CREAR**:<br>• `_MuestraOnlineForm.cshtml`<br>• `_MuestraOnlineGrid.cshtml` | • Grid: CODANE, Ciudad, AltaDificultad, BajaDificultad, Total |
| **Presupuesto.aspx** (Actividades Subcontratadas) | Gestionar actividades tercerizadas | `/CU/Presupuesto/Actividades` | `PresupuestoController` | `ObtenerActividades(long presupuestoId)` (GET JSON)<br>`GuardarActividades(List<ActividadViewModel>)` (POST JSON) | Partial en modal presupuesto | `ActividadSubcontratadaViewModel` con:<br>• ActividadId<br>• Descripcion<br>• ValorEstimado | `PresupuestoService.GuardarActividades()` | ❌ **CREAR**:<br>• `_ActividadesGrid.cshtml` (editable inline) | • Grid editable inline (similar a Excel)<br>• Agregar/Eliminar filas dinámicamente (JS) |
| **Presupuesto.aspx** (Análisis Estadístico) | Gestionar modelos estadísticos | `/CU/Presupuesto/AnalisisEstadistico` | `PresupuestoController` | `ObtenerAnalisis(long presupuestoId)` (GET JSON)<br>`GuardarAnalisis(List<AnalisisViewModel>)` (POST JSON) | Partial en modal presupuesto | `AnalisisEstadisticoViewModel` con:<br>• ModeloId<br>• Descripcion<br>• HorasEstimadas | `PresupuestoService.GuardarAnalisis()` | ❌ **CREAR**:<br>• `_AnalisisEstadisticoGrid.cshtml` | • Dropdown de modelos: Factorial, Cluster, Regresión, etc.<br>• Grid editable |
| **Presupuesto.aspx** (Horas Profesionales) | Asignar horas por rol | `/CU/Presupuesto/HorasProfesionales` | `PresupuestoController` | `ObtenerHoras(long presupuestoId)` (GET JSON)<br>`GuardarHoras(List<HoraViewModel>)` (POST JSON)<br>`CalcularHorasAutomaticamente(long presupuestoId)` (POST) | Partial en modal presupuesto | `HoraProfesionalViewModel` con:<br>• RolId, NombreRol<br>• HorasDiseño, HorasCampo, HorasProceso, HorasInformes | `PresupuestoService.GuardarHoras()`<br>`PresupuestoService.CalcularHorasAutomaticas()` | ❌ **CREAR**:<br>• `_HorasProfesionalesGrid.cshtml` | • Grid: Rol, Horas por etapa, Total<br>• Botón "Calcular Automáticamente" (usa algoritmo IQuote) |
| **Presupuesto.aspx** (Marcar Revisado) | Marcar presupuesto como revisado | `/CU/Presupuesto/MarcarRevisado` | `PresupuestoController` | `MarcarRevisado(long presupuestoId, bool revisado)` (POST JSON) | JSON (sin view) | - | `PresupuestoService.MarcarRevisado()` | N/A | • AJAX desde grid<br>• Actualiza campos: ParRevisado, ParFechaRevision, ParRevisadoPor |
| **Presupuesto.aspx** (Copiar Presupuesto) | Copiar presupuesto a otra alternativa | `/CU/Presupuesto/Copiar` | `PresupuestoController` | `MostrarModalCopiar(long presupuestoId)` (GET Partial)<br>`CopiarPresupuesto(CopiarPresupuestoRequest)` (POST) | `_ModalCopiarPresupuesto.cshtml` | `CopiarPresupuestoRequest` con:<br>• PresupuestoId<br>• AlternativaDestinoId | `PresupuestoService.CopiarPresupuesto()` | ✅ `_Modal.cshtml` | • Modal con dropdown de alternativas disponibles<br>• Copia completa: Parámetros + Muestra + Actividades + Análisis + Horas |
| **Presupuesto.aspx** (Eliminar Presupuesto) | Borrar presupuesto | `/CU/Presupuesto/Eliminar` | `PresupuestoController` | `Eliminar(long presupuestoId)` (DELETE) | JSON (confirmación previa) | - | `PresupuestoService.EliminarPresupuesto()` | SweetAlert2 (confirm) | • Confirm: "¿Está seguro de borrar esta fase?"<br>• Elimina cascada: Parámetros, Muestra, Actividades, etc. |
| **Presupuesto.aspx** (JBE - JobBook Externo) | Ver costos para cliente | `/CU/Presupuesto/JBE` | `PresupuestoController` | `ObtenerJBE(long presupuestoId)` (GET Partial) | `_ModalJBE.cshtml` | `JobBookExternoViewModel` con:<br>• `List<ConceptoValorDTO>` (Concepto, Valor) | `PresupuestoService.GenerarJBE()`<br>(llama `Cotizador.General.GetCostosJobBookExterno()`) | ❌ **CREAR**:<br>• `_JobBookGrid.cshtml` (reutilizable para JBI/JBE) | • Modal con grid:<br>&nbsp;&nbsp;- Conceptos (Costo Campo, Viaticos, Prof Fees, etc.)<br>&nbsp;&nbsp;- Formato: Moneda ($) o Porcentaje (%)<br>&nbsp;&nbsp;- Negritas en TOTAL, GROSS, VENTA |
| **Presupuesto.aspx** (JBI - JobBook Interno) | Ver costos internos | `/CU/Presupuesto/JBI` | `PresupuestoController` | `ObtenerJBI(long presupuestoId)` (GET Partial) | `_ModalJBI.cshtml` | `JobBookInternoViewModel` con:<br>• `List<ConceptoValorDTO>` | `PresupuestoService.GenerarJBI()`<br>(llama `Cotizador.General.GetCostosJobBookInterno()`) | ✅ Reutilizar `_JobBookGrid.cshtml` | • Mismo formato que JBE pero con costos reales (sin markup) |
| **Presupuesto.aspx** (Simulador GM) | Simular Gross Margin y Valor Venta | `/CU/Presupuesto/SimularGM` | `PresupuestoController` | `MostrarSimulador(long presupuestoId)` (GET Partial)<br>`SimularGM(SimularGMRequest)` (POST JSON)<br>`SimularVenta(SimularVentaRequest)` (POST JSON) | `_ModalSimuladorGM.cshtml` | `SimularGMRequest` con:<br>• ValorVenta<br><br>`SimularVentaRequest` con:<br>• GrossMarginDeseado<br>• GrossMarginOperaciones<br><br>`SimuladorResultadoViewModel` | `PresupuestoService.SimularGM()`<br>`PresupuestoService.SimularVenta()` | ❌ **CREAR**:<br>• `_SimuladorGM.cshtml` con:<br>&nbsp;&nbsp;- Form interactivo<br>&nbsp;&nbsp;- Resultado en tiempo real | • Modal con 2 tabs:<br>&nbsp;&nbsp;1. Simular GM (ingresa valor venta → calcula GM)<br>&nbsp;&nbsp;2. Simular Venta (ingresa GM → calcula valor venta)<br>• Muestra JBI/JBE actualizados con simulación |
| **Presupuesto.aspx** (Ajustar GM) | Modificar GM definitivamente | `/CU/Presupuesto/AjustarGM` | `PresupuestoController` | `AjustarGM(AjustarGMRequest)` (POST) | JSON (autorizacion previa) | `AjustarGMRequest` con:<br>• PresupuestoId<br>• NuevoGM<br>• GMOperaciones<br>• TipoCalculo (1=individual, 2=alternativa)<br>• Password (si GM < tope) | `PresupuestoService.AjustarGM()`<br>`PresupuestoService.ValidarAutorizacionGM()` | ❌ **CREAR**:<br>• `_ModalAutorizacionGM.cshtml` | • Si GM < tope configurado:<br>&nbsp;&nbsp;- Requiere password de usuario autorizado<br>&nbsp;&nbsp;- O envía email a DirectorOPS para aprobación<br>• Si autorizado: Actualiza IQ_Parametros.ParGrossMargin |
| **Presupuesto.aspx** (Ver Ejecución) | Ver costos ejecutados vs presupuestados | `/CU/Presupuesto/Ejecucion` | `PresupuestoController` | `ObtenerEjecucion(long presupuestoId)` (GET Partial) | `_ModalEjecucion.cshtml` | `EjecucionViewModel` con:<br>• `List<ControlCostoDTO>`<br>• `List<DetalleOperacionDTO>`<br>• `List<ViaticoDTO>`<br>• `List<PyGDTO>` (Profit & Growth) | `PresupuestoService.ObtenerControlCostos()` | ❌ **CREAR**:<br>• `_ControlCostosGrid.cshtml`<br>• `_DetallesOperacionesGrid.cshtml`<br>• `_ViaticosGrid.cshtml`<br>• `_PyGGrid.cshtml` | • Modal con 4 tabs:<br>&nbsp;&nbsp;1. Control Costos (resumen)<br>&nbsp;&nbsp;2. Detalle Operaciones (con horas)<br>&nbsp;&nbsp;3. Viáticos<br>&nbsp;&nbsp;4. P&G (presupuesto + alternativa)<br>• Solo visible si usuario tiene permiso OPS |
| **Presupuesto.aspx** (Calcular Horas Auto) | Calcular horas profesionales automáticamente | `/CU/Presupuesto/CalcularHorasAutomaticas` | `PresupuestoController` | `CalcularHorasAutomaticas(long presupuestoId)` (POST) | JSON (confirmación previa) | - | `PresupuestoService.CalcularHorasAutomaticas()`<br>(usa algoritmo IQuote en `Cotizador.General`) | SweetAlert2 (confirm) | • Confirm: "Calculará automáticamente las horas reemplazando las existentes. ¿Desea continuar?"<br>• Usa algoritmo basado en: Muestra, Complejidad, Técnica, Metodología |
| **Presupuesto.aspx** (Duplicar Alternativa) | Copiar alternativa completa | `/CU/Presupuesto/DuplicarAlternativa` | `PresupuestoController` | `DuplicarAlternativa(long propuestaId, int alternativaId)` (POST) | JSON (confirmación previa) | - | `PresupuestoService.DuplicarAlternativa()` | SweetAlert2 (confirm) | • Copia: Datos Generales + Todos los Presupuestos + Muestra + Actividades<br>• Asigna nuevo número de alternativa |
| **Presupuesto.aspx** (Importar Presupuestos) | Importar presupuestos de otra propuesta | `/CU/Presupuesto/Importar` | `PresupuestoController` | `MostrarModalImportar()` (GET Partial)<br>`BuscarPropuestas(BuscarPropuestasRequest)` (POST JSON)<br>`ImportarAlternativa(ImportarRequest)` (POST) | `_ModalImportarPresupuestos.cshtml` | `BuscarPropuestasRequest` con:<br>• Titulo, JobBook, IdPropuesta<br><br>`ImportarRequest` con:<br>• PropuestaOrigenId<br>• AlternativaOrigenId<br>• PropuestaDestinoId | `CuentaService.BuscarPropuestas()`<br>`PresupuestoService.ImportarAlternativa()` | ✅ `_Modal.cshtml`<br>❌ **CREAR**:<br>• `_BuscadorPropuestas.cshtml` | • Modal con 2 pasos:<br>&nbsp;&nbsp;1. Buscar propuesta (por título, jobbook, id)<br>&nbsp;&nbsp;2. Seleccionar alternativa a importar<br>• Importa completa: Todos los presupuestos + muestra + actividades |
| **Presupuesto.aspx** (Importar Muestra Excel) | Importar muestra desde archivo Excel | `/CU/Presupuesto/ImportarMuestraExcel` | `PresupuestoController` | `MostrarModalImportarExcel()` (GET Partial)<br>`ImportarMuestraExcel(IFormFile archivo, string hoja)` (POST) | `_ModalImportarMuestraExcel.cshtml` | `ImportarMuestraExcelRequest` con:<br>• Archivo (IFormFile)<br>• HojaSeleccionada (NSE Poblacional, NSE, Dificultad) | `PresupuestoService.ImportarMuestraDesdeExcel()`<br>(usa ClosedXML o EPPlus) | ❌ **CREAR**:<br>• `_FileUploadForm.cshtml` (reutilizable) | • Download template Excel<br>• Upload archivo<br>• Dropdown: Seleccionar hoja a importar<br>• Validaciones: Estructura correcta, no datos extras |
| **Presupuesto.aspx** (Enviar a Revisión) | Marcar alternativa para revisión OPS | `/CU/Presupuesto/EnviarRevision` | `PresupuestoController` | `EnviarRevision(long propuestaId, List<int> alternativasIds)` (POST) | JSON | - | `PresupuestoService.MarcarParaRevisar()`<br>`EmailService.EnviarNotificacionRevision()` | N/A | • Marca CU_Presupuestos.ParaRevisar = True<br>• Envía email a GerenteOperaciones con link |

---

### Nuevas Clases/Servicios Requeridos

#### Backend (MatrixNext.Data)

| Clase | Ubicación | Propósito | Métodos Clave |
|-------|-----------|-----------|---------------|
| `PresupuestoService` | `Services/CU/` | Lógica de negocio de presupuestos | • `ObtenerAlternativas()`<br>• `CrearAlternativa()`<br>• `GuardarPresupuesto()`<br>• `AgregarMuestra()`<br>• `CopiarPresupuesto()`<br>• `DuplicarAlternativa()`<br>• `ImportarAlternativa()`<br>• `GenerarJBI()`, `GenerarJBE()`<br>• `SimularGM()`, `AjustarGM()`<br>• `CalcularHorasAutomaticas()` |
| `PresupuestoDataAdapter` | `Adapters/CU/` | Acceso a datos (Dapper + EF) | • Métodos CRUD para IQ_Parametros, IQ_DatosGeneralesPresupuesto<br>• Ejecución de SPs: `CU_Presupuesto_Get`, `CU_PresupuestosRevisionPorGerenteOperaciones` |
| `IQuoteCalculator` | `Services/CU/` | **MIGRACIÓN** de `CoreProject.Cotizador.General` | • `CalcularCostos()`<br>• `CalcularProductividad()`<br>• `CalcularDiasCampo()`<br>• `CalcularGrossMargin()`<br>• `CalcularValorVenta()`<br>• `CalcularHorasProfesionales()` |
| `MuestraService` | `Services/CU/` | Gestión de muestra estadística | • `AgregarMuestraF2F()`, `AgregarMuestraCATI()`, `AgregarMuestraOnline()`<br>• `EliminarMuestra()`<br>• `ImportarMuestraDesdeExcel()` |
| `ActividadService` | `Services/CU/` | Actividades subcontratadas y análisis | • `GuardarActividades()`<br>• `GuardarAnalisisEstadistico()`<br>• `GuardarHorasProfesionales()` |

#### ViewModels (MatrixNext.Web/Areas/CU/Models)

| ViewModel | Propósito | Propiedades Clave |
|-----------|-----------|-------------------|
| `PresupuestoIndexViewModel` | Vista principal de presupuestos | • `List<AlternativaViewModel>`<br>• `InfoJobBookViewModel`<br>• `PermisosPanelViewModel` |
| `AlternativaViewModel` | Card de alternativa | • Id, Descripcion<br>• DiasCampo, DiasDiseno, DiasInformes, DiasProceso<br>• NumMediciones, Periodicidad<br>• CantidadPresupuestos, ValorTotal |
| `EditarPresupuestoViewModel` | Form completo de presupuesto | • **IQParametrosViewModel** (60+ props)<br>• **PreguntasViewModel**<br>• **ProcesosViewModel**<br>• **ConfiguracionesViewModel**<br>• `List<MuestraViewModel>`<br>• `List<ActividadViewModel>`<br>• `List<AnalisisViewModel>`<br>• `List<HoraProfesionalViewModel>` |
| `IQParametrosViewModel` | Parámetros IQuote | • TecnicaId, MetodologiaId, FaseId<br>• GrupoObjetivo, TotalPreguntas<br>• TiempoEncuesta, Incidencia, Productividad<br>• Complejidad, F2FVirtual<br>• DPTransformacion, DPUnificacion, etc. (mapea 1:1 con IQ_Parametros) |
| `JobBookExternoViewModel` | JBE | • `List<ConceptoValorDTO>` (Concepto, Valor, EsPorcentaje) |

---

### Consideraciones Especiales de Migración

#### 1️⃣ UserControl `UC_Header_Presupuesto.ascx` → Componente Modular

**Estrategia**:
- **NO** migrar como un componente monolítico de 50+ controles
- **DIVIDIR** en componentes especializados:

```
_PresupuestoFormTabs.cshtml (contenedor)
├── Tab 1: General
│   ├── _TecnicaMetodologiaPanel.cshtml
│   └── _GrupoObjetivoPanel.cshtml
├── Tab 2: Preguntas & Procesos
│   ├── _PreguntasPanel.cshtml (Cerradas, Abiertas, etc.)
│   └── _ProcesosPanel.cshtml (CheckBoxes: Campo, Verificación, etc.)
├── Tab 3: Configuraciones Avanzadas
│   ├── _DataProcessingPanel.cshtml (DP*)
│   ├── _ProductTestingPanel.cshtml (PT*)
│   └── _CLTPanel.cshtml (CLT)
├── Tab 4: Muestra
│   └── _MuestraPanel.cshtml (dinámico según técnica)
└── Tab 5: Actividades & Análisis
    ├── _ActividadesGrid.cshtml
    ├── _AnalisisEstadisticoGrid.cshtml
    └── _HorasProfesionalesGrid.cshtml
```

#### 2️⃣ Cálculos IQuote → Servicio Separado

**Problema**: `CoreProject.Cotizador.General` tiene lógica compleja de cálculos (400+ líneas)

**Solución**:
- Crear `IQuoteCalculator` como **servicio independiente**
- **NO** migrar código VB línea por línea
- **REFACTORIZAR** con tests unitarios:
  ```csharp
  public class IQuoteCalculator
  {
      public decimal CalcularProductividad(int tecnica, int metodologia, int totalPreguntas, int duracionMinutos) { ... }
      public int CalcularDiasCampo(int muestra, decimal productividad, int encuestadoresPunto) { ... }
      public decimal CalcularGrossMargin(decimal costoOperativo, decimal valorVenta) { ... }
      // ... más métodos
  }
  ```

#### 3️⃣ Grids Dinámicos → DataTables.js o ag-Grid

**Problema**: 14 GridViews con funcionalidades diferentes

**Solución**:
- **Presupuestos Principal**: ag-Grid (Enterprise) para acciones complejas
- **Muestra, Actividades, Análisis**: DataTables.js (open source)
- **Edición Inline**: ag-Grid con editores custom

#### 4️⃣ Modales vs Páginas

**Decisión**:
- ✅ **Modales**: Crear/Editar Presupuesto (a pesar de ser grande, mantener contexto)
- ✅ **Modales**: JBI, JBE, Simulador, Ejecución (visualización)
- ❌ **Página completa**: NO usar (todo en Index con modales)

---

## 5️⃣ BASE DE DATOS Y STORED PROCEDURES

### 5.1 Tablas Principales

#### 5.1.1 `CU_Presupuestos`
**Propósito:** Tabla principal de presupuestos, almacena metadata de cada alternativa de presupuesto.

| Columna        | Tipo           | PK/FK | Nullable | Descripción                                           |
|----------------|----------------|-------|----------|-------------------------------------------------------|
| Id             | bigint         | PK    | NO       | Identificador único del presupuesto                   |
| PropuestaId    | bigint         | FK    | YES      | FK a `CU_Propuestas`                                  |
| Valor          | float          | -     | YES      | Valor total del presupuesto calculado                 |
| Muestra        | bigint         | -     | YES      | Tamaño total de muestra estadística                   |
| ProductoId     | int            | FK    | YES      | FK a tabla de productos                               |
| GrossMargin    | float          | -     | YES      | Margen bruto (%) calculado                            |
| UsadoPropuesta | bit            | -     | YES      | Indica si está marcado para envío en propuesta        |
| Alternativa    | bigint         | -     | YES      | Número de alternativa (1, 2, 3...)                    |
| JobBook        | nvarchar(MAX)  | -     | YES      | Número de JobBook asignado                            |
| EstadoId       | tinyint        | FK    | YES      | Estado del presupuesto                                |
| Nombre         | nvarchar(MAX)  | -     | YES      | Nombre descriptivo de la alternativa                  |
| Aprobado       | bit            | -     | YES      | Indica si fue aprobado internamente                   |
| ParaRevisar    | bit            | -     | YES      | Flag para revisión por gerente de operaciones         |
| Visible        | bit            | -     | YES      | Determina visibilidad en listados                     |
| Nacional       | bit            | -     | YES      | Indica si es de alcance nacional                      |

**Relaciones:**
- FK: `PropuestaId` → `CU_Propuestas.Id`
- Relación 1:N con `CU_Estudios_Presupuestos` (un presupuesto puede asignarse a múltiples estudios)
- Relación 1:1 con `IQ_Parametros` mediante clave compuesta (IdPropuesta, ParAlternativa)

**Observaciones:** Verificado en [CU_Presupuestos.vb](c:/Users/johnd/source/repos/johndavidpatino/Matrix/CoreProject/CU_Presupuestos.vb#L1-L32)

---

#### 5.1.2 `IQ_Parametros`
**Propósito:** Tabla CENTRAL del sistema IQuote. Almacena los ~110 parámetros de configuración para cálculos de presupuesto.

**Clave Compuesta PK:**
- `IdPropuesta` (bigint)
- `ParAlternativa` (int)
- `MetCodigo` (int) - Código de metodología
- `ParNacional` (int) - Alcance (nacional/local)

**Propiedades críticas (60+ columnas):**

| Grupo                    | Columnas                                                                                                    |
|--------------------------|-------------------------------------------------------------------------------------------------------------|
| **Identificación**       | IdPropuesta, ParAlternativa, MetCodigo, ParNacional, ParNomPresupuesto, NoIQuote, Pr_ProductCode, Pr_Offeringcode |
| **Metodología**          | TipoProyecto, TecCodigo, ParProbabilistico, ParDicultadTargetCualitativo                                    |
| **Cuestionario**         | ParTotalPreguntas, ParPaginasEncuesta, ParHorasEntrevista, ParTiempoEncuesta                                |
| **Muestra**              | ParGrupoObjetivo, ParIncidencia, ParProductividad, ParProductividadOriginal, ParContactosNoEfectivos, ParContactosNoEfectivosOriginales |
| **Trabajo de Campo**     | ParDiasEncuestador, ParDiasSupervisor, ParDiasCoordinador, ParUnidad, ParNumAsistentesSesion, ParEncuestadoresPunto |
| **Procesos**             | ParNProcesosDC, ParNProcesosTopLines, ParNProcesosTablas, ParNProcesosBases                                |
| **Financiero**           | ParGrossMargin, ParValorVenta, ParCostoDirecto, ParActSubGasto, ParActSubCosto, ParValorDolar              |
| **Logística**            | ParUsaLista, ParUsaTablet, ParUsaPapel, ParDispPropio, ParViaticosReclutamiento, ParViaticosModeracion, ParViaticosInforme |
| **Cualitativo**          | ParEditaVideo, ParTransmiteInternet, ParQAP, ParPorcentajeIntercep, ParPorcentajeRecluta                   |
| **Product Testing**      | ParUnidadesProducto, ParValorUnitarioProd, PTApoyosPunto, PTCompra, PTNeutralizador, PTTipoProducto, PTLotes, PTVisitas, PTCeldas, PTProductosEvaluar |
| **CLT**                  | ParTipoCLT, ParAlquilerEquipos, ParApoyoLogistico, ParAccesoInternet                                       |
| **Data Processing**      | DPTransformacion, DPUnificacion, DPComplejidad, DPPonderacion, DPComplejidadCuestionario                   |
| **Fuentes DP (Input)**   | DPInInterna, DPInCliente, DPInPanel, DPInExterno, DPInGMU, DPInOtro                                        |
| **Salidas DP (Output)**  | DPOutCliente, DPOutWebDelivery, DPOutExterno, DPOutGMU, DPOutOtro                                          |
| **Subcontratación**      | ParSubcontratar, ParPorcentajeSub                                                                          |
| **Control**              | Usuario, ParFechaCreacion, ParAprobado, ParFechaAprobacion, ParRevisado, ParRevisadoPor, ParFechaRevision |
| **Estado**               | ParPresupuestoEnUso, ParUsuarioTieneUso, ParFactorAjustado, ParNumJobBook, ParAñoSiguiente                 |
| **Otros**                | ParObservaciones, TipoPresupuesto, Complejidad, ComplejidadCodificacion, F2FVirtual, OP                    |

**Relaciones:**
- Relación 1:N con `IQ_Muestra_1` (clave compuesta)
- Relación 1:N con `IQ_CostoActividades` (clave compuesta)
- Relación 1:N con `IQ_ProcesosPresupuesto` (clave compuesta)
- Relación 1:1 con `IQ_Preguntas` (clave compuesta)

**Observaciones:** Verificado en [IQ_Parametros.vb](c:/Users/johnd/source/repos/johndavidpatino/Matrix/CoreProject/IQ_Parametros.vb#L1-L113)  
⚠️ **Complejidad ALTA:** Esta tabla es el corazón del motor de cálculo IQuote. Contiene 110+ propiedades que alimentan algoritmos de costeo.

---

#### 5.1.3 `IQ_DatosGeneralesPresupuesto`
**Propósito:** Información general/descriptiva del presupuesto (complementa a `IQ_Parametros`).

**Clave Compuesta PK:**
- `IdPropuesta` (bigint)
- `ParAlternativa` (int)

| Columna           | Tipo           | Nullable | Descripción                                    |
|-------------------|----------------|----------|------------------------------------------------|
| Descripcion       | nvarchar(MAX)  | YES      | Descripción general del proyecto               |
| Observaciones     | nvarchar(MAX)  | YES      | Observaciones adicionales                      |
| DiasCampo         | int            | NO       | Días estimados de trabajo de campo             |
| DiasDiseno        | int            | YES      | Días de diseño metodológico                    |
| DiasProcesamiento | int            | YES      | Días de procesamiento de datos                 |
| DiasInformes      | int            | YES      | Días de elaboración de informes                |
| Anticipo          | int            | YES      | Porcentaje de anticipo                         |
| Saldo             | int            | YES      | Porcentaje de saldo                            |
| Plazo             | int            | YES      | Plazo de pago (días)                           |
| TasaCambio        | real           | YES      | Tasa de cambio USD                             |
| NumeroMediciones  | int            | YES      | Cantidad de mediciones (tracking)              |
| MesesMediciones   | int            | YES      | Meses entre mediciones                         |
| TipoPresupuesto   | tinyint        | YES      | Tipo de presupuesto (1=Nuevo, 2=Tracking...)   |
| NoIQuote          | nvarchar(50)   | YES      | Número de IQuote generado                      |

**Observaciones:** Verificado en [IQ_DatosGeneralesPresupuesto.vb](c:/Users/johnd/source/repos/johndavidpatino/Matrix/CoreProject/IQ_DatosGeneralesPresupuesto.vb#L1-L28)

---

#### 5.1.4 `IQ_Muestra_1`
**Propósito:** Detalle de distribución geográfica de muestra por metodología.

**Clave Compuesta PK:**
- `IdPropuesta` (bigint)
- `ParAlternativa` (int)
- `MetCodigo` (int) - Metodología (1=F2F, 2=CATI, 3=Online)
- `CiuCodigo` (int) - Código de ciudad
- `MuIdentificador` (int) - Identificador único de línea
- `ParNacional` (int)

| Columna        | Tipo | Nullable | Descripción                              |
|----------------|------|----------|------------------------------------------|
| DeptCodigo     | int  | NO       | Código de departamento                   |
| MuCantidad     | int  | NO       | Cantidad de encuestas para esta ciudad   |

**Relaciones:**
- FK: Clave compuesta → `IQ_Parametros`
- Relación con tablas maestras de geografía (Ciudades, Departamentos)

**Observaciones:** Verificado en [IQ_Muestra_1.vb](c:/Users/johnd/source/repos/johndavidpatino/Matrix/CoreProject/IQ_Muestra_1.vb#L1-L26)  
💡 **Nota:** Una alternativa puede tener múltiples líneas de muestra (una por ciudad/metodología).

---

#### 5.1.5 `IQ_Preguntas`
**Propósito:** Clasificación de preguntas del cuestionario (para cálculo de tiempos de diseño).

**Clave Compuesta PK:**
- `IdPropuesta` (bigint)
- `ParAlternativa` (int)
- `MetCodigo` (int)
- `ParNacional` (int)

| Columna                  | Tipo | Nullable | Descripción                            |
|--------------------------|------|----------|----------------------------------------|
| PregCerradas             | int  | NO       | Cantidad de preguntas cerradas         |
| PregCerradasMultiples    | int  | NO       | Preguntas cerradas de respuesta múltiple|
| PregAbiertas             | int  | NO       | Preguntas abiertas                     |
| PregAbiertasMultiples    | int  | NO       | Preguntas abiertas de respuesta múltiple|
| PregOtras                | int  | NO       | Otras preguntas (baterías, grillas)    |
| PregDemograficos         | int  | NO       | Preguntas demográficas                 |

**Relaciones:**
- FK: Clave compuesta → `IQ_Parametros` (relación 1:1)

**Observaciones:** Verificado en [IQ_Preguntas.vb](c:/Users/johnd/source/repos/johndavidpatino/Matrix/CoreProject/IQ_Preguntas.vb#L1-L26)

---

#### 5.1.6 `IQ_ProcesosPresupuesto`
**Propósito:** Procesos de Data Processing asignados al presupuesto.

**Clave Compuesta PK:**
- `IdPropuesta` (bigint)
- `ParAlternativa` (int)
- `MetCodigo` (int)
- `ProcCodigo` (int) - FK a tabla de Procesos
- `ParNacional` (int)

| Columna    | Tipo   | Nullable | Descripción                                  |
|------------|--------|----------|----------------------------------------------|
| Porcentaje | float  | YES      | Porcentaje de complejidad del proceso (0-100)|

**Relaciones:**
- FK: Clave compuesta → `IQ_Parametros`
- FK: `ProcCodigo` → `IQ_Procesos.ProcCodigo` (tabla maestra de procesos)

**Observaciones:** Verificado en [IQ_ProcesosPresupuesto.vb](c:/Users/johnd/source/repos/johndavidpatino/Matrix/CoreProject/IQ_ProcesosPresupuesto.vb#L1-L24)

---

#### 5.1.7 `IQ_CostoActividades`
**Propósito:** Detalle de actividades operacionales con costos y unidades.

**Clave Compuesta PK:**
- `IdPropuesta` (bigint)
- `ParAlternativa` (int)
- `MetCodigo` (int)
- `ActCodigo` (int) - FK a tabla de Actividades
- `ParNacional` (int)

| Columna                | Tipo           | Nullable | Descripción                            |
|------------------------|----------------|----------|----------------------------------------|
| CaCosto                | decimal(18,2)  | NO       | Costo unitario de la actividad         |
| CaUnidades             | int            | YES      | Cantidad de unidades                   |
| CaDescripcionUnidades  | nvarchar(MAX)  | YES      | Descripción de unidades (días, horas)  |
| Horas                  | int            | YES      | Horas asignadas                        |

**Relaciones:**
- FK: Clave compuesta → `IQ_Parametros`
- FK: `ActCodigo` → `IQ_Actividades` (tabla maestra de actividades)

**Observaciones:** Verificado en [IQ_CostoActividades.vb](c:/Users/johnd/source/repos/johndavidpatino/Matrix/CoreProject/IQ_CostoActividades.vb#L1-L25)

---

#### 5.1.8 `IQ_ControlCostos`
**Propósito:** Control de gastos autorizados vs ejecutados durante el proyecto.

**Clave Compuesta:**
- `IdPropuesta` (bigint)
- `ParAlternativa` (int)
- `MetCodigo` (int)
- `ParNacional` (int)
- `ID` (int) - PK autoincremental

| Columna          | Tipo           | Nullable | Descripción                            |
|------------------|----------------|----------|----------------------------------------|
| Consecutivo      | int            | NO       | Número de autorización                 |
| ValorAutorizado  | decimal(18,2)  | NO       | Valor autorizado para el gasto         |
| ValorEjecutado   | decimal(18,2)  | YES      | Valor ejecutado/gastado                |
| Fecha            | datetime       | YES      | Fecha de autorización                  |
| Usuario          | decimal(18,0)  | YES      | ID del usuario que autoriza            |
| Observacion      | nvarchar(MAX)  | YES      | Descripción del gasto                  |
| ValorProduccion  | decimal(18,2)  | YES      | Valor en producción (post-ejecución)   |

**Observaciones:** Verificado en [IQ_ControlCostos.vb](c:/Users/johnd/source/repos/johndavidpatino/Matrix/CoreProject/IQ_ControlCostos.vb#L1-L28)

---

### 5.2 Stored Procedures Principales

#### 5.2.1 `CU_Presupuesto_Get`
**Propósito:** Obtiene lista de presupuestos filtrados.

```sql
EXEC CU_Presupuesto_Get 
    @id bigint = NULL,
    @propuestaId bigint = NULL
```

**Retorna:** `CU_Presupuesto_Get_Result` con:
- Campos de `CU_Presupuestos` + joins con `CU_Propuestas`, estados, etc.

**Uso:** Grid principal de presupuestos (gvPresupuestos) - Línea 159 en Presupuesto.aspx.vb

**Observaciones:** Verificado en [CU_Model.Context.vb](c:/Users/johnd/source/repos/johndavidpatino/Matrix/CoreProject/CU_Model.Context.vb#L566-L572)

---

#### 5.2.2 `CU_Estudios_Presupuestos_Asignados_Get`
**Propósito:** Obtiene presupuestos asignados a un estudio específico.

```sql
EXEC CU_Estudios_Presupuestos_Asignados_Get
    @estudioId bigint = NULL,
    @presupuestoId bigint = NULL,
    @propuestaId bigint = NULL
```

**Retorna:** `CU_Presupuesto_Get_Result`

**Uso:** Modal de asignación de presupuestos a estudios

**Observaciones:** Verificado en [CU_Model.Context.vb](c:/Users/johnd/source/repos/johndavidpatino/Matrix/CoreProject/CU_Model.Context.vb#L574-L582)

---

#### 5.2.3 `CU_Presupuestos_JobBook_Edit`
**Propósito:** Actualiza el número de JobBook de un presupuesto.

```sql
EXEC CU_Presupuestos_JobBook_Edit
    @id bigint,
    @jobBook nvarchar(MAX)
```

**Retorna:** Código de resultado (int)

**Uso:** Después de generación de JobBook (líneas 1518-1531)

**Observaciones:** Verificado en [CU_Model.Context.vb](c:/Users/johnd/source/repos/johndavidpatino/Matrix/CoreProject/CU_Model.Context.vb#L632-L638)

---

#### 5.2.4 `CU_PresupuestosRevisionPorGerenteOperaciones`
**Propósito:** Obtiene presupuestos pendientes de revisión por gerente de operaciones.

```sql
EXEC CU_PresupuestosRevisionPorGerenteOperaciones
    @usuarioID bigint = NULL,
    @revisados bit = NULL,
    @tituloPropuesta nvarchar(MAX) = NULL,
    @idPropuesta bigint = NULL,
    @idTrabajo bigint = NULL,
    @jobbook nvarchar(MAX) = NULL
```

**Retorna:** `CU_PresupuestosRevisionPorGerenteOperaciones_Result` con:
- Presupuestos marcados con `ParaRevisar = 1`
- Filtrados por usuario autorizado

**Uso:** Flujo de autorización/revisión (líneas 1244-1447 - AjustarGrossMargin)

**Observaciones:** Verificado en [CU_Model.Context.vb](c:/Users/johnd/source/repos/johndavidpatino/Matrix/CoreProject/CU_Model.Context.vb#L904-L918)

---

#### 5.2.5 `IQ_UpdateParNumJobBook`
**Propósito:** Actualiza el número de JobBook en `IQ_Parametros`.

```sql
EXEC IQ_UpdateParNumJobBook
    @jobBook nvarchar(MAX),
    @idPropuesta bigint,
    @alternativa int
```

**Retorna:** Código de resultado (int)

**Uso:** Sincronización entre `CU_Presupuestos.JobBook` e `IQ_Parametros.ParNumJobBook`

**Observaciones:** Verificado en [CU_Model.Context.vb](c:/Users/johnd/source/repos/johndavidpatino/Matrix/CoreProject/CU_Model.Context.vb#L940-L948)

---

### 5.3 Estrategia de Acceso a Datos

#### **Entity Framework Core para operaciones CRUD estándar:**
- `CU_Presupuestos` → Operaciones básicas (Insert, Update, Delete, Get por ID)
- `IQ_Parametros` → Inserts/Updates con 110+ propiedades
- `IQ_DatosGeneralesPresupuesto` → CRUD simple
- Relaciones 1:N manejadas con Include/ThenInclude (eager loading)

#### **Dapper para consultas complejas y SPs:**
- Stored Procedures existentes (`CU_Presupuesto_Get`, `CU_PresupuestosRevisionPorGerenteOperaciones`)
- Queries con múltiples joins y agregaciones
- Reportes de JobBook (JBI/JBE) con queries optimizados

#### **Repository Pattern:**
```csharp
IPresupuestoDataAdapter
├── GetPresupuestosAsync(filtros) → Dapper → CU_Presupuesto_Get
├── GetByIdAsync(id) → EF Core
├── CreateAsync(presupuesto, parametros, muestra, preguntas) → EF Core + Transaction
├── UpdateAsync(...) → EF Core + Transaction
├── DeleteAsync(id) → EF Core
├── GetPresupuestosParaRevisionAsync(usuarioId) → Dapper → CU_PresupuestosRevisionPorGerenteOperaciones
└── UpdateJobBookAsync(id, jobBook) → Dapper → CU_Presupuestos_JobBook_Edit
```

#### **Transacciones:**
- Uso de `TransactionScope` o `BeginTransaction()` para operaciones que afectan múltiples tablas:
  - SavePresupuesto (IQ_Parametros + IQ_Muestra_1 + IQ_Preguntas + IQ_ProcesosPresupuesto + CU_Presupuestos)
  - Delete (cascade a tablas IQ_*)

---

### 5.4 Migraciones EF Core

**Tablas a incluir en DbContext:**
```csharp
public DbSet<CU_Presupuestos> Presupuestos { get; set; }
public DbSet<IQ_Parametros> IQParametros { get; set; }
public DbSet<IQ_DatosGeneralesPresupuesto> IQDatosGenerales { get; set; }
public DbSet<IQ_Muestra_1> IQMuestra { get; set; }
public DbSet<IQ_Preguntas> IQPreguntas { get; set; }
public DbSet<IQ_ProcesosPresupuesto> IQProcesos { get; set; }
public DbSet<IQ_CostoActividades> IQCostoActividades { get; set; }
public DbSet<IQ_ControlCostos> IQControlCostos { get; set; }
```

**Configuración de Fluent API:**
- Claves compuestas para todas las tablas IQ_* (HasKey con expresión lambda)
- Relaciones 1:N con cascada configurada
- Precision/Scale para campos decimal
- MaxLength para nvarchar(50), etc.

**⚠️ IMPORTANTE:** NO generar migraciones automáticas. Estas tablas ya existen en producción.  
Usar **Code-First from Database** para reverse-engineer y validar mapeos.

---

## 6️⃣ RIESGOS Y CONSIDERACIONES TÉCNICAS

### 6.1 Riesgos de Complejidad ALTA 🔴

#### 6.1.1 Motor de Cálculo IQuote (`CoreProject.Cotizador.General`)

**Descripción del Riesgo:**
- Clase `Cotizador.General` con **~605 líneas** de lógica de negocio compleja
- **30+ métodos** de cálculo interconectados
- Algoritmos propietarios para costeo, productividad, gross margin, días de campo
- Dependencias con ~110 propiedades de `IQ_Parametros`
- Lógica distribuida entre VB.NET (Presupuesto.aspx.vb líneas 877-1176) y clase `Cotizador.General`

**Evidencia:**
```vb
' Presupuesto.aspx.vb - Líneas 181-253
Dim oCot As New CoreProject.Cotizador.General
' Métodos utilizados:
' - GetPresupuesto(), GetProcesos(), GetAnalisisEstadisticos()
' - GetActividadesSubcontratadas(), GetHorasProfesionales()
' - GetCalculoProductividad(), GetMuestraF2F/CATI/Online()
' - PutPresupuesto(), PutGeneral(), PutMuestra(), etc.
```

**Impacto:**
- 🔴 **CRÍTICO**: Sin este motor, los presupuestos no se pueden calcular
- ⏱️ **Tiempo estimado**: 40-60 horas de migración + testing exhaustivo
- 🧪 **Testing**: Requiere casos de prueba con datos reales para validar paridad de cálculos

**Estrategia de Mitigación:**
1. **Fase 1 - Extracción**: Crear `IQuoteCalculator` en MatrixNext.Data/Services/CU/
2. **Fase 2 - Migración incremental**: Migrar método por método con unit tests
3. **Fase 3 - Validación paralela**: Ejecutar ambas versiones (legacy vs nueva) y comparar resultados
4. **Fase 4 - Documentación**: Documentar cada algoritmo (muchos sin comentarios)

**Métodos críticos a migrar:**
- `GetCalculoProductividad()` - Algoritmo de productividad F2F/CATI/Online
- `CalcularCostoDirecto()` - Suma de costos de todas las actividades
- `CalcularGrossMargin()` - Cálculo de margen según fórmula: GM = (ValorVenta - CostoDirecto) / ValorVenta
- `CalcularDiasCampo()` - Lógica basada en muestra, productividad, incidencia
- `CalcularHorasProfesionales()` - Distribución de horas por rol y fase

---

#### 6.1.2 UserControl Monolítico (`UC_Header_Presupuesto.ascx`)

**Descripción del Riesgo:**
- **744 líneas** de markup HTML/ASP.NET en un solo UserControl
- **50+ controles** (TextBoxes, DropDownLists, CheckBoxes, RadioButtons)
- Sin separación de concerns (todo en un archivo)
- Acoplamiento fuerte con `Presupuesto.aspx.vb` mediante FindControl()

**Evidencia:**
```vb
' Presupuesto.aspx.vb - Acceso directo a controles del UserControl
DirectCast(UCHeader.FindControl("txtGrupoObjetivo"), TextBox).Text = p.ParGrupoObjetivo
DirectCast(UCHeader.FindControl("txtProductividad"), TextBox).Text = p.ParProductividad.ToString
DirectCast(UCHeader.FindControl("chbProcessCampo"), CheckBox).Checked = True
' ... 60+ líneas de FindControl()
```

**Estructura del UserControl:**
- Panel Preguntas (6 tipos de preguntas)
- Panel Procesos (10+ checkboxes de procesos DP)
- Panel Data Processing (20+ campos de configuración DP)
- Panel Product Testing (15+ campos de PT)
- Panel CLT (8+ campos de CLT)

**Impacto:**
- 🟡 **MEDIO-ALTO**: No bloquea funcionalidad, pero dificulta mantenimiento
- 📏 **UX**: Form muy largo, difícil de navegar (scrolling excesivo)
- 🧩 **Reutilización**: Imposible reutilizar secciones por separado

**Estrategia de Mitigación:**
1. **Modularizar en Partial Views**:
   - `_PreguntasPanel.cshtml` (Panel de preguntas)
   - `_ProcesosPanel.cshtml` (Procesos DP)
   - `_ConfiguracionDPPanel.cshtml` (Data Processing config)
   - `_ConfiguracionPTPanel.cshtml` (Product Testing)
   - `_ConfiguracionCLTPanel.cshtml` (CLT)
2. **Tabs/Accordion**: Organizar en pestañas para reducir scrolling
3. **ViewModels estructurados**: Crear `PreguntasViewModel`, `ProcesosViewModel`, etc.
4. **Validación del lado cliente**: FluentValidation + jQuery Unobtrusive Validation

---

### 6.2 Riesgos de Complejidad MEDIA 🟡

#### 6.2.1 ViewState y Postbacks

**Descripción del Riesgo:**
- WebForms usa ViewState para mantener estado entre postbacks
- Cada postback envía ViewState serializado (puede ser 50KB-200KB)
- 14 GridViews con datos en ViewState
- Lógica dependiente de eventos de servidor (SelectedIndexChanged, RowCommand, etc.)

**Evidencia:**
```vb
' Eventos típicos de postback:
Protected Sub ddlAlternativa_SelectedIndexChanged(sender As Object, e As EventArgs)
Protected Sub gvPresupuestos_RowCommand(sender As Object, e As GridViewCommandEventArgs)
Protected Sub btnAddMuestra_Click(sender As Object, e As EventArgs)
```

**Impacto:**
- 🟡 **MEDIO**: Requiere repensar flujo de datos
- 📦 **Payload**: Reducir tamaño de respuestas eliminando ViewState
- ⚡ **Performance**: Mejorar experiencia con AJAX parcial

**Estrategia de Mitigación:**
1. **Eliminar ViewState**: No existe en MVC
2. **AJAX con fetch API**: Reemplazar postbacks con llamadas AJAX
3. **State management client-side**: Usar JavaScript para estado temporal
4. **Server-side session**: Solo para datos críticos (no grids completos)

---

#### 6.2.2 UpdatePanels y AJAX Control Toolkit

**Descripción del Riesgo:**
- 2 UpdatePanels identificados (líneas 978-980, 1349, 1384)
- Dependencia de AjaxControlToolkit (deprecated)
- ModalPopupExtender, CalendarExtender, etc. no disponibles en Core

**Evidencia:**
```vb
UPanelGeneral.UpdateMode = UpdatePanelUpdateMode.Conditional
UpdatePanel2.Update()
```

**Impacto:**
- 🟡 **MEDIO**: Funcionalidad replicable con alternativas modernas
- 🔄 **Cambio de paradigma**: De server-side AJAX a client-side

**Estrategia de Mitigación:**
1. **Bootstrap Modals**: Reemplazar ModalPopupExtender
2. **jQuery UI Datepicker o Flatpickr**: Reemplazar CalendarExtender
3. **fetch API + partial views**: Reemplazar UpdatePanels
4. **SignalR**: Si se requiere actualización en tiempo real (no parece el caso)

---

#### 6.2.3 Claves Compuestas en Tablas IQ_*

**Descripción del Riesgo:**
- Todas las tablas IQ_* usan claves compuestas de 4-6 campos
- EF Core requiere configuración explícita con Fluent API
- Queries más complejos (joins con múltiples campos)
- Imposibilidad de usar ID simple para routing

**Ejemplo:**
```csharp
// IQ_Parametros PK:
HasKey(p => new { 
    p.IdPropuesta, 
    p.ParAlternativa, 
    p.MetCodigo, 
    p.ParNacional 
});
```

**Impacto:**
- 🟡 **MEDIO**: Aumenta complejidad de mapeo, pero manejable
- 🔍 **Queries**: Más verbosos con múltiples condiciones en WHERE

**Estrategia de Mitigación:**
1. **Fluent API exhaustiva**: Documentar todas las PKs compuestas
2. **DTOs para queries**: Usar objetos intermedios para simplificar queries
3. **Extension methods**: Crear métodos de extensión para filtros comunes
4. **Dapper para queries complejos**: Usar SQL raw cuando sea más legible

---

### 6.3 Riesgos de Complejidad BAJA 🟢

#### 6.3.1 GridViews con Muchas Columnas

**Impacto:** 🟢 **BAJO** - Solución directa con DataTables.js o ag-Grid

#### 6.3.2 Importación de Excel (ClosedXML)

**Impacto:** 🟢 **BAJO** - ClosedXML es compatible con .NET Core, migración 1:1

#### 6.3.3 DevExpress Controls

**Evidencia:** `Imports DevExpress.Web.Internal.XmlProcessor` (línea 11)

**Impacto:** 🟢 **BAJO** - Parece uso mínimo, reemplazable con controles estándar

---

### 6.4 Riesgos No Técnicos

#### 6.4.1 Conocimiento del Negocio

**Riesgo:**
- Fórmulas de costeo sin documentación formal
- Reglas de negocio implícitas en código
- Conocimiento concentrado en usuarios clave

**Mitigación:**
- Sesiones de trabajo con usuarios expertos (Product Owners, Gerentes de Operaciones)
- Documentar reglas de negocio en confluence/wiki
- Testing con datos reales y validación de resultados

---

#### 6.4.2 Datos de Prueba

**Riesgo:**
- Testing requiere datos reales (presupuestos históricos)
- Datos sensibles (valores comerciales, gross margins)
- Base de datos de prueba debe tener datos representativos

**Mitigación:**
- Anonimizar datos de producción para ambiente QA
- Crear dataset mínimo viable para testing unitario
- Validar con usuarios que resultados sean correctos

---

### 6.5 Resumen de Riesgos por Prioridad

| Prioridad | Riesgo | Complejidad | Esfuerzo (h) | Mitigación |
|-----------|--------|-------------|--------------|------------|
| P0 🔴 | Motor IQuote (Cotizador.General) | ALTA | 40-60 | Migración incremental con testing paralelo |
| P1 🟡 | UserControl monolítico (744 LOC) | MEDIA | 16-24 | Modularizar en partial views + tabs |
| P1 🟡 | ViewState elimination | MEDIA | 12-16 | AJAX + client-side state management |
| P1 🟡 | UpdatePanels → fetch API | MEDIA | 8-12 | Bootstrap modals + fetch |
| P2 🟡 | Claves compuestas EF Core | MEDIA | 8-12 | Fluent API + extension methods |
| P3 🟢 | GridViews → DataTables/ag-Grid | BAJA | 8-12 | Implementación directa |
| P3 🟢 | Excel import (ClosedXML) | BAJA | 2-4 | Migración 1:1 |

---

### 6.6 Decisiones Técnicas Pendientes

| # | Decisión | Opciones | Impacto | Responsable | Deadline |
|---|----------|----------|---------|-------------|----------|
| DT-01 | ¿Usar ag-Grid Enterprise o DataTables? | A) ag-Grid (licencia requerida, más features)<br>B) DataTables.js (gratis, suficiente) | 🟡 MEDIO | Tech Lead | Antes de Sprint 1 |
| DT-02 | ¿Migrar todo Cotizador.General o refactorizar? | A) Migración 1:1 (más rápido)<br>B) Refactorizar lógica (más limpio) | 🔴 ALTO | Tech Lead + PO | Antes de Sprint 2 |
| DT-03 | ¿Cómo manejar concurrencia en edición? | A) Optimistic concurrency (RowVersion)<br>B) Pessimistic locking<br>C) Last-write-wins | 🟡 MEDIO | Tech Lead | Sprint 2 |

---

## 7️⃣ COMPONENTES REUTILIZABLES Y PATRONES

### 7.1 Componentes Compartidos Existentes en MatrixNext

**Verificados en `/Views/Shared/Components/`:**

| Componente | Ubicación | Propósito | Aplicable a Presupuesto |
|------------|-----------|-----------|-------------------------|
| `_DatePicker.cshtml` | Views/Shared/Components/ | Selector de fechas con validación | ✅ SÍ - Para fechas de revisión, aprobación |
| `_SearchSelect.cshtml` | Views/Shared/Components/ | Dropdown con búsqueda (Chosen.js style) | ✅ SÍ - Para ciudades, metodologías, técnicas |
| `_Dropzone.cshtml` | Views/Shared/Components/ | Upload de archivos drag & drop | ✅ SÍ - Para importar Excel de muestra |
| `_QuillEditor.cshtml` | Views/Shared/Components/ | Editor WYSIWYG | ✅ SÍ - Para descripción, observaciones |
| `_modal.cshtml` | Views/Shared/layouts/ | Modal Bootstrap genérico | ✅ SÍ - Para todos los modales (JBI, JBE, Presupuesto) |

**Librerías JavaScript Existentes (wwwroot/lib/):**
- ✅ Bootstrap
- ✅ jQuery
- ✅ jQuery Validation
- ✅ jQuery Validation Unobtrusive

---

### 7.2 Componentes Nuevos a Crear

#### 7.2.1 Componentes de UI (Partial Views)

| Componente | Ubicación | Responsabilidad | Inputs | Outputs |
|------------|-----------|-----------------|--------|---------|
| `_PresupuestoCard.cshtml` | Areas/CU/Views/Shared/ | Tarjeta de alternativa en sidebar | `AlternativaViewModel` | Card HTML con acciones (Edit, Duplicate, Delete) |
| `_PresupuestoFormTabs.cshtml` | Areas/CU/Views/Shared/ | Tabs del formulario principal | `EditarPresupuestoViewModel` | Estructura de tabs (General, Preguntas, Muestra, etc.) |
| `_PreguntasPanel.cshtml` | Areas/CU/Views/Shared/ | Panel de clasificación de preguntas | `PreguntasViewModel` | 6 inputs numéricos con total automático |
| `_ProcesosPanel.cshtml` | Areas/CU/Views/Shared/ | Panel de procesos DP | `List<ProcesoViewModel>` | Checkboxes + porcentajes dinámicos |
| `_MuestraF2FGrid.cshtml` | Areas/CU/Views/Shared/ | Grid de muestra Face-to-Face | `List<MuestraF2FViewModel>` | DataTable con NSE5y6, NSE4, NSE123, Total |
| `_MuestraCATIGrid.cshtml` | Areas/CU/Views/Shared/ | Grid de muestra CATI | `List<MuestraCATIViewModel>` | DataTable simple (TipoMuestra, Cantidad) |
| `_MuestraOnlineGrid.cshtml` | Areas/CU/Views/Shared/ | Grid de muestra Online | `List<MuestraOnlineViewModel>` | DataTable con metodología online |
| `_ActividadesGrid.cshtml` | Areas/CU/Views/Shared/ | Grid de actividades subcontratadas | `List<ActividadViewModel>` | DataTable editable inline |
| `_AnalisisEstadisticoGrid.cshtml` | Areas/CU/Views/Shared/ | Grid de análisis estadísticos | `List<AnalisisViewModel>` | DataTable con tipos de análisis |
| `_HorasProfesionalesGrid.cshtml` | Areas/CU/Views/Shared/ | Grid de horas profesionales | `List<HoraProfesionalViewModel>` | DataTable con cargos y horas |
| `_JobBookModal.cshtml` | Areas/CU/Views/Shared/ | Modal genérico para JBI/JBE | `JobBookViewModel` | Modal con grid de conceptos/valores |
| `_ConfiguracionDPPanel.cshtml` | Areas/CU/Views/Shared/ | Configuraciones de Data Processing | `ConfiguracionDPViewModel` | Checkboxes + selects (Transformación, Unificación, Complejidad) |
| `_ConfiguracionPTPanel.cshtml` | Areas/CU/Views/Shared/ | Configuraciones de Product Testing | `ConfiguracionPTViewModel` | Inputs para Lotes, Visitas, Celdas, Productos |
| `_ConfiguracionCLTPanel.cshtml` | Areas/CU/Views/Shared/ | Configuraciones de CLT | `ConfiguracionCLTViewModel` | Tipo CLT, Alquiler Equipos, Apoyo Logístico |
| `_NumericInput.cshtml` | Views/Shared/Components/ | Input numérico con validación | `name, label, min, max, required` | Input HTML5 type="number" con estilos |
| `_PercentageInput.cshtml` | Views/Shared/Components/ | Input de porcentaje (0-100) | `name, label, value` | Input con % suffix y validación |
| `_CurrencyInput.cshtml` | Views/Shared/Components/ | Input de moneda | `name, label, currency` | Input formateado con $ prefix |

---

#### 7.2.2 Servicios Backend

| Servicio | Namespace | Responsabilidad | Métodos Principales |
|----------|-----------|-----------------|---------------------|
| `PresupuestoService` | MatrixNext.Data.Services.CU | Lógica de negocio de presupuestos | `ObtenerAlternativas()`, `GuardarPresupuesto()`, `CopiarPresupuesto()`, `DuplicarAlternativa()`, `ImportarAlternativa()`, `GenerarJBI()`, `GenerarJBE()` |
| `IQuoteCalculator` | MatrixNext.Data.Services.CU | Motor de cálculo IQuote migrado | `CalcularCostos()`, `CalcularProductividad()`, `CalcularGrossMargin()`, `CalcularDiasCampo()`, `CalcularHorasProfesionales()` |
| `MuestraService` | MatrixNext.Data.Services.CU | Gestión de muestra estadística | `AgregarMuestraF2F()`, `AgregarMuestraCATI()`, `AgregarMuestraOnline()`, `EliminarMuestra()`, `ImportarMuestraDesdeExcel()` |
| `ActividadService` | MatrixNext.Data.Services.CU | Actividades y análisis | `GuardarActividades()`, `GuardarAnalisisEstadistico()`, `GuardarHorasProfesionales()` |
| `PresupuestoDataAdapter` | MatrixNext.Data.Adapters.CU | Acceso a datos | `GetPresupuestosAsync()`, `CreateAsync()`, `UpdateAsync()`, `DeleteAsync()`, `GetByIdAsync()`, `UpdateJobBookAsync()` |


---

#### 7.2.3 ViewModels

| ViewModel | Namespace | Propósito | Propiedades Clave |
|-----------|-----------|-----------|-------------------|
| `PresupuestoIndexViewModel` | MatrixNext.Web.Areas.CU.Models | Vista principal | `List<AlternativaViewModel>`, `InfoJobBookViewModel`, `long PropuestaId` |
| `AlternativaViewModel` | MatrixNext.Web.Areas.CU.Models | Card de alternativa | `int Id`, `string Descripcion`, `int DiasCampo`, `int DiasTotal`, `int NumMediciones`, `int CantidadPresupuestos`, `decimal ValorTotal` |
| `EditarAlternativaViewModel` | MatrixNext.Web.Areas.CU.Models | Modal crear/editar alternativa | `string Descripcion`, `int DiasCampo/Diseno/Proceso/Informes`, `int NumMediciones`, `int Periodicidad`, `byte TipoPresupuesto` |
| `EditarPresupuestoViewModel` | MatrixNext.Web.Areas.CU.Models | Form completo presupuesto | `IQParametrosViewModel`, `PreguntasViewModel`, `List<MuestraViewModel>`, `List<ActividadViewModel>`, etc. |
| `IQParametrosViewModel` | MatrixNext.Web.Areas.CU.Models | Parámetros IQuote (110 props) | Mapeo 1:1 con `IQ_Parametros` |
| `PreguntasViewModel` | MatrixNext.Web.Areas.CU.Models | Clasificación preguntas | `int PregCerradas`, `int PregCerradasMultiples`, `int PregAbiertas`, `int PregAbiertasMultiples`, `int PregOtras`, `int PregDemograficos` |
| `ProcesoViewModel` | MatrixNext.Web.Areas.CU.Models | Proceso DP | `int ProcCodigo`, `string Nombre`, `bool Seleccionado`, `double? Porcentaje` |
| `MuestraF2FViewModel` | MatrixNext.Web.Areas.CU.Models | Línea de muestra F2F | `int CiudadCodigo`, `string NombreCiudad`, `int CODANE`, `int NSE5y6`, `int NSE4`, `int NSE123`, `int Total` |
| `MuestraCATIViewModel` | MatrixNext.Web.Areas.CU.Models | Línea de muestra CATI | `int TipoMuestra`, `string DescripcionTipo`, `int Cantidad` |
| `MuestraOnlineViewModel` | MatrixNext.Web.Areas.CU.Models | Línea de muestra Online | `int MetodologiaId`, `string NombreMetodologia`, `int Cantidad` |
| `ActividadViewModel` | MatrixNext.Web.Areas.CU.Models | Actividad subcontratada | `int ActividadCodigo`, `string Nombre`, `decimal Costo`, `int Unidades`, `string DescripcionUnidades` |
| `AnalisisViewModel` | MatrixNext.Web.Areas.CU.Models | Análisis estadístico | `int TipoAnalisis`, `string Descripcion`, `int Cantidad` |
| `HoraProfesionalViewModel` | MatrixNext.Web.Areas.CU.Models | Hora profesional | `int CargoCodigo`, `string NombreCargo`, `int Horas`, `decimal TarifaHora`, `decimal Total` |
| `JobBookExternoViewModel` | MatrixNext.Web.Areas.CU.Models | JobBook Externo | `string NoJobBook`, `List<ConceptoValorDTO>` |
| `JobBookInternoViewModel` | MatrixNext.Web.Areas.CU.Models | JobBook Interno | `string NoJobBook`, `List<ConceptoCostoDTO>` |
| `ConfiguracionDPViewModel` | MatrixNext.Web.Areas.CU.Models | Configuraciones DP | `bool DPTransformacion`, `bool DPUnificacion`, `byte DPComplejidad`, `byte DPPonderacion`, 10+ props de fuentes in/out |
| `ConfiguracionPTViewModel` | MatrixNext.Web.Areas.CU.Models | Configuraciones PT | `byte PTApoyosPunto`, `bool PTCompra`, `bool PTNeutralizador`, `byte PTTipoProducto`, `byte PTLotes`, etc. |
| `ConfiguracionCLTViewModel` | MatrixNext.Web.Areas.CU.Models | Configuraciones CLT | `int ParTipoCLT`, `decimal ParAlquilerEquipos`, `bool ParApoyoLogistico`, `bool ParAccesoInternet` |

---

### 7.3 Patrones de Arquitectura Aplicados

#### 7.3.1 Repository Pattern

```csharp
// IPresupuestoDataAdapter.cs
public interface IPresupuestoDataAdapter
{
    Task<List<PresupuestoDto>> GetPresupuestosAsync(long propuestaId, int? alternativaId = null);
    Task<PresupuestoDetalleDto> GetByIdAsync(long id);
    Task<long> CreateAsync(CreatePresupuestoCommand command);
    Task UpdateAsync(UpdatePresupuestoCommand command);
    Task DeleteAsync(long id);
    Task UpdateJobBookAsync(long id, string jobBook);
    Task<List<PresupuestoDto>> GetPresupuestosParaRevisionAsync(long usuarioId, bool revisados);
}

// PresupuestoDataAdapter.cs
public class PresupuestoDataAdapter : IPresupuestoDataAdapter
{
    private readonly ApplicationDbContext _context;
    private readonly IDbConnection _connection;
    
    // Implementación con EF Core + Dapper
}
```

---

#### 7.3.2 Service Layer Pattern

```csharp
// IPresupuestoService.cs
public interface IPresupuestoService
{
    Task<List<AlternativaViewModel>> ObtenerAlternativasAsync(long propuestaId);
    Task<long> CrearAlternativaAsync(CrearAlternativaRequest request);
    Task<long> GuardarPresupuestoAsync(GuardarPresupuestoRequest request);
    Task CopiarPresupuestoAsync(CopiarPresupuestoRequest request);
    Task DuplicarAlternativaAsync(long propuestaId, int alternativaId);
    Task ImportarAlternativaAsync(ImportarAlternativaRequest request);
    Task<JobBookExternoViewModel> GenerarJBEAsync(long presupuestoId);
    Task<JobBookInternoViewModel> GenerarJBIAsync(long presupuestoId);
}

// PresupuestoService.cs
public class PresupuestoService : IPresupuestoService
{
    private readonly IPresupuestoDataAdapter _dataAdapter;
    private readonly IIQuoteCalculator _calculator;
    private readonly IMuestraService _muestraService;
    
    // Implementación con inyección de dependencias
}
```

---

#### 7.3.3 CQRS-lite Pattern (Command/Query Separation)

**Commands (escritura):**
```csharp
public class CreatePresupuestoCommand
{
    public long PropuestaId { get; set; }
    public int Alternativa { get; set; }
    public IQParametrosViewModel Parametros { get; set; }
    public PreguntasViewModel Preguntas { get; set; }
    public List<MuestraViewModel> Muestra { get; set; }
    // ... más propiedades
}

public class UpdatePresupuestoCommand { /* ... */ }
```

**Queries (lectura):**
```csharp
public class GetPresupuestosQuery
{
    public long PropuestaId { get; set; }
    public int? AlternativaId { get; set; }
    public int? TecnicaId { get; set; }
}

public class GetPresupuestoByIdQuery
{
    public long Id { get; set; }
}
```

---

#### 7.3.4 Dependency Injection Pattern

```csharp
// ServiceCollectionExtensions.cs - Patrón ya existente en MatrixNext
public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddCUPresupuestoModule(this IServiceCollection services)
    {
        // Data Adapters
        services.AddScoped<IPresupuestoDataAdapter, PresupuestoDataAdapter>();
        services.AddScoped<IMuestraDataAdapter, MuestraDataAdapter>();
        services.AddScoped<IActividadDataAdapter, ActividadDataAdapter>();
        
        // Services
        services.AddScoped<IPresupuestoService, PresupuestoService>();
        services.AddScoped<IMuestraService, MuestraService>();
        services.AddScoped<IActividadService, ActividadService>();
        services.AddScoped<IIQuoteCalculator, IQuoteCalculator>();
        
        return services;
    }
}
```

---

#### 7.3.5 Validation Pattern (FluentValidation)

```csharp
public class EditarPresupuestoViewModelValidator : AbstractValidator<EditarPresupuestoViewModel>
{
    public EditarPresupuestoViewModelValidator()
    {
        RuleFor(x => x.Parametros.ParGrupoObjetivo)
            .NotEmpty().WithMessage("Grupo objetivo es requerido")
            .MaximumLength(300).WithMessage("Máximo 300 caracteres");
        
        RuleFor(x => x.Parametros.ParIncidencia)
            .InclusiveBetween(1, 100).WithMessage("Incidencia debe estar entre 1 y 100")
            .When(x => x.Parametros.ParIncidencia.HasValue);
        
        RuleFor(x => x.Parametros.ParProductividad)
            .GreaterThan(0).WithMessage("Productividad debe ser mayor a 0")
            .When(x => x.Parametros.ParProductividad.HasValue);
        
        // ... más reglas
    }
}
```

---

### 7.4 Librerías JavaScript a Incorporar

| Librería | Versión | Propósito | Uso en Presupuesto |
|----------|---------|-----------|-------------------|
| **DataTables.js** | 1.13+ | Grids con sorting, filtering, paging | Grids de muestra, actividades, análisis |
| **ag-Grid Community** | 31+ | Grid avanzado (opcional) | Grid principal de presupuestos (11 acciones) |
| **SweetAlert2** | 11+ | Alertas/confirmaciones modernas | Confirmación de eliminación, mensajes de éxito/error |
| **Chosen.js** o **Select2** | Latest | Dropdowns con búsqueda | Ciudades, metodologías, técnicas, procesos |
| **Flatpickr** | 4+ | Date picker ligero | Fechas de revisión, aprobación (ya existe _DatePicker) |
| **AutoNumeric.js** | 4+ | Input numérico formateado | Valores de moneda, porcentajes |
| **Tabulator** (alternativa ag-Grid) | 5+ | Grid moderno y ligero | Alternativa gratuita a ag-Grid |

---

### 7.5 Reutilización de Código del Legado

#### ✅ **REUTILIZAR SIN CAMBIOS:**
- **ClosedXML.Excel** (import de Excel) - Compatible .NET Core
- **Queries SQL de SPs** - Mantener como están
- **Validaciones de negocio** - Migrar lógica exacta

#### ⚠️ **ADAPTAR/REFACTORIZAR:**
- **Cotizador.General** → `IQuoteCalculator` (migrar a C#, refactorizar VB.NET)

#### ❌ **NO REUTILIZAR (Reemplazar):**
- **GridViews** → DataTables/ag-Grid
- **UpdatePanels** → fetch API + partial views
- **AjaxControlToolkit** → Bootstrap + jQuery
- **ViewState** → Session/AJAX state management

---

## 8️⃣ BACKLOG INICIAL PRIORIZADO

### 8.1 Épica 1: Infraestructura y Setup (P0 - Sprint 1)

| ID | Historia de Usuario | Criterios de Aceptación | Estimación | Dependencias |
|----|---------------------|-------------------------|------------|--------------|
| **US-001** | Como desarrollador, necesito configurar el DbContext con las entidades IQ_* para acceder a las tablas de presupuestos | • DbContext con 8 DbSets configurados<br>• Fluent API con claves compuestas<br>• Connection string configurada<br>• Migraciones no automáticas (reverse-engineer) | 5 SP | - |
| **US-002** | Como desarrollador, necesito implementar los Data Adapters para presupuestos | • `IPresupuestoDataAdapter` con 8 métodos<br>• Implementación con EF Core + Dapper<br>• Unit tests con mock | 8 SP | US-001 |
| **US-003** | Como desarrollador, necesito migrar el motor IQuote (`Cotizador.General`) a C# | • Clase `IQuoteCalculator` con 30+ métodos migrados<br>• Unit tests con casos de prueba reales<br>• Validación de paridad con legacy (100% match) | 21 SP | US-001 |
| **US-004** | Como desarrollador, necesito configurar el registro de servicios en DI container | • `ServiceCollectionExtensions.AddCUPresupuestoModule()`<br>• Todos los servicios registrados<br>• Resolución exitosa en controller | 3 SP | US-002, US-003 |

**Total Sprint 1:** 37 Story Points (~1.5 semanas con equipo de 2 devs)

---

### 8.2 Épica 2: Vista Principal y Alternativas (P0 - Sprint 2)

| ID | Historia de Usuario | Criterios de Aceptación | Estimación | Dependencias |
|----|---------------------|-------------------------|------------|--------------|
| **US-005** | Como Gerente de Cuentas, necesito ver la lista de alternativas de una propuesta para navegar entre ellas | • Ruta `/CU/Presupuesto/{propuestaId}`<br>• Panel lateral con cards de alternativas<br>• Datos: Descripción, Días, Mediciones, ValorTotal<br>• Acciones: Nueva, Duplicar, Importar | 8 SP | US-004 |
| **US-006** | Como Gerente de Cuentas, necesito crear una nueva alternativa para proponer opciones al cliente | • Modal "Nueva Alternativa"<br>• Campos: Descripción, DiasCampo/Diseno/Proceso/Informes, NumMediciones<br>• Validaciones con FluentValidation<br>• Auto-incremento de número de alternativa | 5 SP | US-005 |
| **US-007** | Como Gerente de Cuentas, necesito editar datos generales de una alternativa para corregir información | • Modal "Editar Alternativa"<br>• Mismos campos que US-006<br>• Carga de datos existentes<br>• Update en DB | 3 SP | US-006 |
| **US-008** | Como Gerente de Cuentas, necesito duplicar una alternativa existente para crear variaciones rápidamente | • Botón "Duplicar" en card<br>• Copia completa: IQ_Parametros + IQ_Muestra + IQ_Preguntas + IQ_Procesos + IQ_CostoActividades<br>• Nuevo número de alternativa asignado | 8 SP | US-005 |
| **US-009** | Como Gerente de Cuentas, necesito importar una alternativa desde otra propuesta para reutilizar configuraciones | • Modal "Importar Alternativa"<br>• Búsqueda de propuestas<br>• Selección de alternativa a importar<br>• Importación completa de datos | 8 SP | US-008 |

**Total Sprint 2:** 32 Story Points (~1.5 semanas)

---

### 8.3 Épica 3: Formulario de Presupuesto (P0 - Sprint 3-4)

| ID | Historia de Usuario | Criterios de Aceptación | Estimación | Dependencias |
|----|---------------------|-------------------------|------------|--------------|
| **US-010** | Como Gerente de Cuentas, necesito abrir el formulario de presupuesto con todos los parámetros IQuote para configurar el costeo | • Modal grande con tabs:<br>&nbsp;&nbsp;1. General<br>&nbsp;&nbsp;2. Preguntas & Procesos<br>&nbsp;&nbsp;3. Muestra<br>&nbsp;&nbsp;4. Actividades<br>&nbsp;&nbsp;5. Configuraciones Avanzadas<br>• 110+ campos cargados<br>• Validaciones client-side | 21 SP | US-004 |
| **US-011** | Como Gerente de Cuentas, necesito el panel de Preguntas para clasificar el cuestionario | • Partial `_PreguntasPanel.cshtml`<br>• 6 inputs numéricos<br>• Total automático de preguntas<br>• Cálculo de tiempo de diseño | 5 SP | US-010 |
| **US-012** | Como Gerente de Cuentas, necesito el panel de Procesos para seleccionar actividades de Data Processing | • Partial `_ProcesosPanel.cshtml`<br>• 10+ checkboxes de procesos<br>• Input de porcentaje por proceso<br>• Validación: % entre 0-100 | 5 SP | US-010 |
| **US-013** | Como Gerente de Cuentas, necesito guardar el presupuesto con todos sus parámetros para generar el costeo | • Botón "Guardar" en modal<br>• Transacción que guarda:<br>&nbsp;&nbsp;• IQ_Parametros (110 props)<br>&nbsp;&nbsp;• IQ_DatosGeneralesPresupuesto<br>&nbsp;&nbsp;• IQ_Preguntas<br>&nbsp;&nbsp;• IQ_ProcesosPresupuesto<br>&nbsp;&nbsp;• CU_Presupuestos<br>• Cálculo automático con IQuoteCalculator<br>• Mensaje de éxito | 13 SP | US-003, US-010 |

**Total Sprint 3-4:** 44 Story Points (~2 semanas)

---

### 8.4 Épica 4: Gestión de Muestra (P0 - Sprint 5)

| ID | Historia de Usuario | Criterios de Aceptación | Estimación | Dependencias |
|----|---------------------|-------------------------|------------|--------------|
| **US-014** | Como Gerente de Cuentas, necesito agregar muestra Face-to-Face distribuida por ciudades para costear trabajo de campo | • Form inline: Ciudad (Select2), Dificultad, Cantidad<br>• Grid con columnas: CODANE, Ciudad, NSE5y6, NSE4, NSE123, Total<br>• Botón "Agregar"<br>• Insert en IQ_Muestra_1 | 8 SP | US-013 |
| **US-015** | Como Gerente de Cuentas, necesito agregar muestra CATI para costear telefónicas | • Form: TipoMuestra (dropdown), Cantidad<br>• Grid: TipoMuestra, Cantidad<br>• Insert en IQ_Muestra_1 con MetCodigo=2 | 5 SP | US-014 |
| **US-016** | Como Gerente de Cuentas, necesito agregar muestra Online para costear encuestas web | • Form: MetodologiaOnline (dropdown), Cantidad<br>• Grid: Metodologia, Cantidad<br>• Insert en IQ_Muestra_1 con MetCodigo=3 | 5 SP | US-014 |
| **US-017** | Como Gerente de Cuentas, necesito eliminar líneas de muestra para corregir errores | • Botón "Eliminar" en cada fila de grid<br>• Confirmación con SweetAlert2<br>• Delete en IQ_Muestra_1<br>• Recalcular totales | 3 SP | US-014 |
| **US-018** | Como Gerente de Cuentas, necesito importar muestra desde Excel para agilizar carga de ciudades | • Dropzone para subir .xlsx<br>• Validación de formato (columnas esperadas)<br>• Inserción masiva en IQ_Muestra_1<br>• Mensaje de éxito con # registros insertados | 8 SP | US-014 |

**Total Sprint 5:** 29 Story Points (~1 semana)

---

### 8.5 Épica 5: Grid de Presupuestos y Acciones (P1 - Sprint 6)

| ID | Historia de Usuario | Criterios de Aceptación | Estimación | Dependencias |
|----|---------------------|-------------------------|------------|--------------|
| **US-019** | Como Gerente de Cuentas, necesito ver la lista de presupuestos de una alternativa filtrados por técnica para revisar costos | • Grid con columnas: Metodología, Fase, Muestra, Valor, GrossMargin, Revisado<br>• Filtro por Técnica (dropdown)<br>• 11 botones de acción (iconos)<br>• DataTables.js o ag-Grid | 13 SP | US-013 |
| **US-020** | Como Gerente de Cuentas, necesito editar un presupuesto existente para ajustar parámetros | • Botón "Editar" (lápiz)<br>• Abre modal de presupuesto (US-010)<br>• Carga datos existentes<br>• Update en DB | 5 SP | US-019 |
| **US-021** | Como Gerente de Cuentas, necesito copiar un presupuesto para crear variación de metodología | • Botón "Copiar" (copy icon)<br>• Modal: Seleccionar nueva Metodología/Fase<br>• Duplica IQ_Parametros + relacionados<br>• Inserta nuevo registro | 8 SP | US-019 |
| **US-022** | Como Gerente de Cuentas, necesito eliminar un presupuesto para limpiar alternativas | • Botón "Eliminar" (trash icon)<br>• Confirmación con SweetAlert2<br>• Delete cascade en IQ_Parametros + relacionados<br>• Refrescar grid | 3 SP | US-019 |

**Total Sprint 6:** 29 Story Points (~1 semana)

---

### 8.6 Épica 6: JobBook y Reportes (P1 - Sprint 7)

| ID | Historia de Usuario | Criterios de Aceptación | Estimación | Dependencias |
|----|---------------------|-------------------------|------------|--------------|
| **US-023** | Como Gerente de Cuentas, necesito generar JobBook Externo (JBE) para enviar al cliente | • Botón "JBE" en grid<br>• Modal con grid de conceptos/valores (con markup y GM)<br>• Formato: Concepto, Valor, % (si aplica)<br>• Totales calculados | 13 SP | US-003, US-019 |
| **US-024** | Como Gerente de Cuentas, necesito generar JobBook Interno (JBI) para análisis de costos | • Botón "JBI" en grid<br>• Modal con grid de costos reales (sin markup)<br>• Formato: Concepto, Costo<br>• Totales calculados | 13 SP | US-023 |
| **US-025** | Como Gerente de Cuentas, necesito asignar número de JobBook a un presupuesto para seguimiento | • Input "JobBook" en modal de presupuesto<br>• Update en CU_Presupuestos.JobBook<br>• Sincronización con IQ_Parametros.ParNumJobBook | 3 SP | US-020 |

**Total Sprint 7:** 29 Story Points (~1 semana)

---

### 8.7 Épica 7: Flujos de Autorización (P1 - Sprint 8)

| ID | Historia de Usuario | Criterios de Aceptación | Estimación | Dependencias |
|----|---------------------|-------------------------|------------|--------------|
| **US-026** | Como Gerente de Operaciones, necesito revisar presupuestos marcados para revisión para aprobar/rechazar GM | • Vista `/CU/Presupuesto/Revision`<br>• Grid con presupuestos donde ParaRevisar=1<br>• Filtrados por usuario autorizado<br>• Botones: Aprobar, Rechazar | 8 SP | US-019 |
| **US-027** | Como Gerente de Operaciones, necesito ajustar Gross Margin de un presupuesto para cumplir políticas | • Modal "Ajustar GM"<br>• Input: Nuevo GM (%), Observación<br>• Validación de autorización (US_Usuarios_Autorizaciones)<br>• Update en IQ_Parametros.ParGrossMargin<br>• Log de cambios | 8 SP | US-026 |
| **US-028** | Como Gerente de Cuentas, necesito marcar presupuesto como "Para Revisar" cuando GM < umbral para solicitar autorización | • Checkbox "Marcar para revisión"<br>• Update en CU_Presupuestos.ParaRevisar<br>• Notificación a gerente de operaciones | 5 SP | US-020 |

**Total Sprint 8:** 21 Story Points (~1 semana)

---

### 8.8 Resumen de Backlog

| Épica | Sprints | Story Points | Prioridad | Estado |
|-------|---------|--------------|-----------|--------|
| 1. Infraestructura y Setup | Sprint 1 | 37 | P0 🔴 | 📋 Planificado |
| 2. Vista Principal y Alternativas | Sprint 2 | 32 | P0 🔴 | 📋 Planificado |
| 3. Formulario de Presupuesto | Sprint 3-4 | 44 | P0 🔴 | 📋 Planificado |
| 4. Gestión de Muestra | Sprint 5 | 29 | P0 🔴 | 📋 Planificado |
| 5. Grid de Presupuestos y Acciones | Sprint 6 | 29 | P1 🟡 | 📋 Planificado |
| 6. JobBook y Reportes | Sprint 7 | 29 | P1 🟡 | 📋 Planificado |
| 7. Flujos de Autorización | Sprint 8 | 21 | P1 🟡 | 📋 Planificado |

**TOTAL:** 221 Story Points ≈ **8-9 semanas** con equipo de 2 developers

---

## 9️⃣ CHECKLIST DE VERIFICACIÓN PRE-MIGRACIÓN

### 9.1 Base de Datos ✅

- [ ] **Verificar estructura de tablas en ambiente de desarrollo**
  - [ ] CU_Presupuestos (15 columnas)
  - [ ] IQ_Parametros (110+ columnas)
  - [ ] IQ_DatosGeneralesPresupuesto (14 columnas)
  - [ ] IQ_Muestra_1 (8 columnas, clave compuesta)
  - [ ] IQ_Preguntas (6 tipos)
  - [ ] IQ_ProcesosPresupuesto (4 columnas)
  - [ ] IQ_CostoActividades (9 columnas)
  - [ ] IQ_ControlCostos (12 columnas)

- [ ] **Validar Stored Procedures**
  - [ ] CU_Presupuesto_Get (SELECT principal)
  - [ ] CU_Estudios_Presupuestos_Asignados_Get
  - [ ] CU_Presupuestos_JobBook_Edit
  - [ ] CU_PresupuestosRevisionPorGerenteOperaciones
  - [ ] IQ_UpdateParNumJobBook

- [ ] **Verificar relaciones FK**
  - [ ] CU_Presupuestos.PropuestaId → CU_Propuestas.Id
  - [ ] IQ_Parametros ← IQ_Muestra_1 (1:N)
  - [ ] IQ_Parametros ← IQ_Preguntas (1:1)
  - [ ] IQ_Parametros ← IQ_ProcesosPresupuesto (1:N)
  - [ ] IQ_Parametros ← IQ_CostoActividades (1:N)

- [ ] **Crear backup de tablas IQ_* en ambiente de desarrollo**
  - [ ] Script de backup ejecutado
  - [ ] Verificar restore de backup

---

### 9.2 Código Legacy ✅

- [ ] **Analizar dependencias de Cotizador.General**
  - [ ] Listar todos los métodos públicos (30+ métodos)
  - [ ] Identificar métodos privados/helpers
  - [ ] Documentar algoritmos sin comentarios
  - [ ] Crear casos de prueba con datos reales



- [ ] **Inventariar UserControl UC_Header_Presupuesto.ascx**
  - [ ] Listar todos los controles (50+) con nombres y tipos
  - [ ] Identificar validaciones client-side (JavaScript)
  - [ ] Mapear eventos a lógica de negocio

- [ ] **Documentar flujos de UpdatePanels**
  - [ ] Identificar qué secciones se actualizan parcialmente
  - [ ] Mapear a llamadas AJAX equivalentes

---

### 9.3 Ambiente de Desarrollo ✅

- [ ] **Configurar proyecto MatrixNext.Web**
  - [ ] Verificar .NET 8 instalado
  - [ ] Restaurar paquetes NuGet
  - [ ] Compilación exitosa

- [ ] **Configurar proyecto MatrixNext.Data**
  - [ ] Verificar Entity Framework Core 8
  - [ ] Verificar Dapper
  - [ ] Connection string configurada

- [ ] **Extensiones de VS Code / Visual Studio**
  - [ ] C# Dev Kit
  - [ ] EF Core Power Tools (para reverse engineering)
  - [ ] SQL Server extension

- [ ] **Librerías JavaScript**
  - [ ] Bootstrap 5+ verificado
  - [ ] jQuery 3+ verificado
  - [ ] Decidir: DataTables.js vs ag-Grid (DT-02)
  - [ ] Instalar Select2 o Chosen.js
  - [ ] Instalar SweetAlert2
  - [ ] Instalar AutoNumeric.js

---

### 9.4 Conocimiento del Negocio ✅

- [ ] **Sesiones con usuarios clave**
  - [ ] Gerente de Cuentas (uso diario de Presupuesto.aspx)
  - [ ] Gerente de Operaciones (flujo de autorizaciones)
  - [ ] CFO (ajustes de Gross Margin)


- [ ] **Documentar reglas de negocio**
  - [ ] Fórmula de Gross Margin: `GM = (ValorVenta - CostoDirecto) / ValorVenta`
  - [ ] Umbrales de autorización para GM
  - [ ] Reglas de productividad F2F/CATI/Online
  - [ ] Cálculo de días de campo
  - [ ] Distribución de horas profesionales

- [ ] **Obtener datos de prueba**
  - [ ] Exportar 10-20 presupuestos reales (anonimizados)
  - [ ] Casos de uso completos (desde creación hasta JBE/JBI)
  - [ ] Casos edge: presupuestos multimetodología, tracking, internacionales

---

### 9.5 Testing ✅

- [ ] **Preparar estrategia de testing**
  - [ ] Unit tests para IQuoteCalculator (validación de paridad)
  - [ ] Integration tests para Data Adapters
  - [ ] End-to-End tests con Playwright (flujos completos)
  - [ ] Performance tests (1000+ presupuestos en grid)

- [ ] **Definir criterios de aceptación**
  - [ ] Paridad 100% en cálculos con legacy
  - [ ] Performance: Grid carga en < 2 segundos
  - [ ] Performance: Guardar presupuesto en < 3 segundos
  - [ ] UX: Validaciones en tiempo real (< 500ms)

---

### 9.6 Seguridad y Permisos ✅

- [ ] **Revisar roles y permisos**
  - [ ] Gerente de Cuentas: CRUD completo de presupuestos
  - [ ] Gerente de Operaciones: Autorización de GM
  - [ ] CFO: Ajuste de GM sin límites
  - [ ] Usuarios regulares: Solo lectura

- [ ] **Implementar autorización en endpoints**
  - [ ] `[Authorize(Roles = "GerenteCuentas")]` en acciones CRUD
  - [ ] `[Authorize(Policy = "PuedeAutorizarGM")]` en ajustes
  - [ ] Validación server-side de permisos (no confiar solo en UI)

---

### 9.7 Decisiones Técnicas Pendientes (Repetido de 6.6) ✅

- [ ] **DT-01**: ¿ag-Grid Enterprise o DataTables.js?
  - [ ] Evaluar costos de licencia ag-Grid
  - [ ] Evaluar features necesarios (edición inline, export, etc.)
  - [ ] Deadline: **Antes de Sprint 1**

- [ ] **DT-02**: ¿Migración 1:1 o refactorización de Cotizador.General?
  - [ ] Sesión técnica con equipo
  - [ ] Evaluar riesgos de refactorización
  - [ ] Deadline: **Antes de Sprint 2**

- [ ] **DT-04**: ¿Concurrencia optimista o pesimista?
  - [ ] Evaluar frecuencia de ediciones concurrentes
  - [ ] Decidir estrategia (RowVersion, Locks, Last-write-wins)
  - [ ] Deadline: **Sprint 2**

- [ ] **DT-05**: ¿Hangfire o Azure Functions para background jobs?
  - [ ] Evaluar infraestructura disponible
  - [ ] Considerar costos de Azure Functions
  - [ ] Deadline: **Sprint 1**

---

## 🔟 DECISIONES TÉCNICAS CLAVE TOMADAS

### 10.1 Arquitectura

| Decisión | Opción Elegida | Justificación | Fecha |
|----------|----------------|---------------|-------|
| **Patrón de acceso a datos** | Repository Pattern con Data Adapters | • Consistente con arquitectura existente de MatrixNext<br>• Separación clara entre lógica de negocio y acceso a datos<br>• Facilita testing con mocks | ✅ Confirmado |
| **ORM Strategy** | EF Core + Dapper híbrido | • EF Core para CRUD simple y relaciones<br>• Dapper para SPs existentes y queries complejos<br>• Mejor performance en queries de lectura | ✅ Confirmado |
| **Service Layer** | Servicios por dominio (Presupuesto, Muestra, Actividad) | • Cohesión alta por bounded context<br>• Facilita mantenimiento<br>• Permite reutilización | ✅ Confirmado |
| **Validation** | FluentValidation + Client-side (jQuery Unobtrusive) | • Validaciones centralizadas en clases Validator<br>• Reutilizable en API y MVC<br>• Client-side mejora UX | ✅ Confirmado |

---

### 10.2 Frontend

| Decisión | Opción Elegida | Justificación | Fecha |
|----------|----------------|---------------|-------|
| **Framework CSS** | Bootstrap 5 (ya existente en MatrixNext) | • Consistencia con resto de la aplicación<br>• No requiere aprendizaje adicional<br>• Modales y forms ya estilizados | ✅ Confirmado |
| **State Management** | Session + AJAX (sin SPA framework) | • No requiere Angular/React/Vue<br>• Mantiene paradigma MVC<br>• Suficiente para complejidad del módulo | ✅ Confirmado |
| **Modales** | Bootstrap Modals + Partial Views | • Nativo de Bootstrap<br>• Fácil integración con MVC<br>• No requiere librerías adicionales | ✅ Confirmado |
| **Tabs en formulario** | Bootstrap Tabs | • Organiza 110+ campos en secciones<br>• UX mejorada vs scrolling largo<br>• Validación por tab | ✅ Confirmado |
| **Date Picker** | Flatpickr (componente existente _DatePicker.cshtml) | • Ya implementado en MatrixNext<br>• Ligero y customizable<br>• Consistencia con otras áreas | ✅ Confirmado |
| **Select con búsqueda** | Select2 (componente existente _SearchSelect.cshtml) | • Ya implementado en MatrixNext<br>• Funciona bien con listas largas (ciudades)<br>• AJAX search si es necesario | ✅ Confirmado |

---

### 10.3 Grids de Datos

| Decisión | Opción Elegida | Justificación | Fecha |
|----------|----------------|---------------|-------|
| **Grid principal de presupuestos** | ❓ **PENDIENTE** (DT-02)<br>Opciones:<br>A) ag-Grid Community (gratis)<br>B) ag-Grid Enterprise (licencia)<br>C) DataTables.js (gratis) | • **ag-Grid Enterprise**: Edición inline, export Excel/PDF, columnas complejas<br>• **ag-Grid Community**: Features limitados pero suficientes<br>• **DataTables.js**: Gratuito, amplia adopción, suficiente para mayoría de casos | ⏳ Sprint 1 |
| **Grids secundarios** (Muestra, Actividades, Análisis) | DataTables.js | • Funcionalidad suficiente<br>• Gratuito<br>• Sorting, filtering, paging out-of-the-box | ✅ Confirmado |
| **Export a Excel** | ClosedXML (ya en uso) | • Compatible .NET Core<br>• Ya usado en legacy para import de muestra<br>• Migración 1:1 | ✅ Confirmado |

---

### 10.4 Calculadora IQuote

| Decisión | Opción Elegida | Justificación | Fecha |
|----------|----------------|---------------|-------|
| **Estrategia de migración** | ❓ **PENDIENTE** (DT-03)<br>Opciones:<br>A) Migración 1:1 de VB.NET a C#<br>B) Refactorización completa | • **Migración 1:1**: Más rápido, menor riesgo, validación más fácil<br>• **Refactorización**: Código más limpio, pero mayor riesgo de bugs | ⏳ Sprint 2 |
| **Testing de paridad** | Ejecutar ambas versiones en paralelo (shadow mode) | • Comparar resultados de cálculos<br>• Validar 100% de paridad antes de deprecar legacy<br>• Logging de discrepancias | ✅ Confirmado |
| **Ubicación** | MatrixNext.Data/Services/CU/IQuoteCalculator.cs | • Service inyectable via DI<br>• Reutilizable desde Web y APIs<br>• Unit testeable | ✅ Confirmado |

---



### 10.6 Concurrencia y Transacciones

| Decisión | Opción Elegida | Justificación | Fecha |
|----------|----------------|---------------|-------|
| **Concurrencia** | ❓ **PENDIENTE** (DT-04)<br>Opciones:<br>A) Optimistic Concurrency (RowVersion)<br>B) Pessimistic Locking<br>C) Last-write-wins | • **Optimistic**: Mejor para bajo conflicto, UX no bloqueante<br>• **Pessimistic**: Bloquea ediciones concurrentes<br>• **Last-write-wins**: Simple pero datos pueden perderse | ⏳ Sprint 2 |
| **Transacciones** | TransactionScope para operaciones multi-tabla | • Garantiza atomicidad en SavePresupuesto<br>• Rollback automático en caso de error<br>• Compatible con EF Core y Dapper | ✅ Confirmado |
| **Isolation Level** | Read Committed (default SQL Server) | • Balance entre consistencia y performance<br>• Suficiente para casos de uso | ✅ Confirmado |

---

### 10.7 Logging y Monitoreo

| Decisión | Opción Elegida | Justificación | Fecha |
|----------|----------------|---------------|-------|
| **Logging Framework** | Serilog (si ya está en MatrixNext) o ILogger (built-in) | • Structured logging<br>• Sinks a Application Insights, archivos, etc.<br>• Compatible con DI de .NET | ✅ Confirmado |
| **Log de cambios** | IQ_LogCambios (tabla ya existente) | • Ya hay tabla IQ_LogCambios en DB<br>• Registrar cambios de GM, autorizaciones<br>• Auditoría completa | ✅ Confirmado |
| **Performance Monitoring** | Application Insights (si disponible) | • Tracking de tiempos de respuesta<br>• Alertas en caso de slowdowns<br>• Custom metrics para cálculos IQuote | ⏳ Definir con Ops |

---

### 10.8 Deployment y CI/CD

| Decisión | Opción Elegida | Justificación | Fecha |
|----------|----------------|---------------|-------|
| **Estrategia de deployment** | Feature flag para rollout gradual | • Deploy código nuevo sin activarlo<br>• Activar por usuario o rol<br>• Rollback inmediato si hay problemas | ✅ Confirmado |
| **Migración de datos** | NO requiere (tablas ya existen) | • CU_Presupuestos e IQ_* ya en producción<br>• Solo agregar código nuevo | ✅ Confirmado |
| **Testing en producción** | Shadow mode con logging | • Usuarios continúan usando legacy<br>• Ejecutar nuevo código en background<br>• Comparar resultados y loggear discrepancias | ✅ Confirmado |

---

## 1️⃣1️⃣ ESTIMACIÓN PRELIMINAR

### 11.1 Desglose por Componente

| Componente | Complejidad | Story Points | Horas (1 SP = 4h) | Devs | Duración |
|------------|-------------|--------------|-------------------|------|----------|
| **Infraestructura (DbContext, Entities, Adapters)** | Media | 37 | 148h | 2 | 1.5 semanas |
| **Migración IQuoteCalculador** | Alta | 21 | 84h | 1 | 2 semanas |
| **Vista Principal + Alternativas** | Media | 32 | 128h | 2 | 1.5 semanas |
| **Formulario de Presupuesto (110 campos)** | Alta | 44 | 176h | 2 | 2 semanas |
| **Gestión de Muestra (F2F/CATI/Online)** | Media | 29 | 116h | 2 | 1 semana |
| **Grid de Presupuestos + Acciones** | Media | 29 | 116h | 2 | 1 semana |
| **JobBook (JBI/JBE)** | Media | 29 | 116h | 2 | 1 semana |
| **Flujos de Autorización** | Media | 21 | 84h | 1-2 | 1 semana |
| **Testing (Unit + Integration + E2E)** | Media | 30 | 120h | 2 | 1.5 semanas |
| **Documentación y UAT** | Baja | 15 | 60h | 1-2 | 1 semana |

**TOTAL:** 287 Story Points = **1,148 horas** = **143 días-persona**

---

### 11.2 Estimación con Equipo de 2 Developers

**Asunciones:**
- Velocidad del equipo: 25-30 SP por sprint (2 semanas)
- Sprints de 2 semanas
- 2 developers full-time
- 40% buffer para riesgos e imprevistos

| Escenario | Sprints | Semanas | Meses |
|-----------|---------|---------|-------|
| **Optimista** (30 SP/sprint, sin bloqueos) | 9 sprints | 18 semanas | 4.5 meses |
| **Realista** (25 SP/sprint, con buffer) | 11 sprints | 22 semanas | 5.5 meses |
| **Pesimista** (20 SP/sprint, múltiples bloqueos) | 14 sprints | 28 semanas | 7 meses |

**Recomendación:** Planificar para **5.5 meses** (escenario realista)

---

### 11.3 Hitos Clave

| Hito | Fecha Estimada | Entregable | Criterio de Éxito |
|------|----------------|------------|-------------------|
| **M1: Infraestructura completa** | Fin Sprint 1 (Semana 2) | DbContext + Adapters + IQuoteCalculator (80%) | • Unit tests pasando<br>• Cálculos con paridad > 95% |
| **M2: MVP - Crear Presupuesto** | Fin Sprint 4 (Semana 8) | Crear alternativa + Formulario presupuesto + Muestra | • Usuario puede crear presupuesto completo<br>• Cálculos funcionan<br>• Datos se guardan correctamente |
| **M3: Feature Complete** | Fin Sprint 8 (Semana 16) | Todas las funcionalidades | • Grid con todas las acciones<br>• JBI/JBE generados<br>• Autorizaciones funcionando |
| **M4: Production Ready** | Fin Sprint 11 (Semana 22) | Sistema completo + testing + documentación | • 100% paridad con legacy<br>• UAT aprobado<br>• Performance < 3s |

---

### 11.4 Riesgos que Afectan Estimación

| Riesgo | Probabilidad | Impacto | Mitigación | Tiempo Adicional |
|--------|--------------|---------|------------|------------------|
| **Algoritmos IQuote mal documentados** | Alta | Alto | Sesiones con expertos de negocio, testing exhaustivo | +2 semanas |
| **Datos de prueba insuficientes** | Media | Medio | Exportar 100+ presupuestos reales temprano | +1 semana |
| **Cambios de scope** | Alta | Alto | Strict change control, product backlog priorizado | +2-4 semanas |
| **Bugs críticos en producción legacy** | Media | Alto | Soporte paralelo, fixes urgentes | Variable |
| **Aprendizaje de equipo** | Media | Medio | Pair programming, code reviews | +1 semana |

**Total Buffer Recomendado:** +3 semanas = **25 semanas (6.25 meses)** en escenario pesimista

---

## 1️⃣2️⃣ PRÓXIMOS PASOS

### 12.1 Inmediatos (Esta semana)

- [ ] **Aprobar este análisis con stakeholders**
  - [ ] Sesión de revisión con Product Owner
  - [ ] Presentación a Tech Lead / Arquitecto
  - [ ] Aprobación de gerencia (inversión de 6 meses)

- [ ] **Resolver Decisiones Técnicas Pendientes (DT-01, DT-02)**
  - [ ] **DT-01**: Evaluar costos de ag-Grid Enterprise vs DataTables
  - [ ] **DT-02**: Decidir estrategia de migración de Cotizador.General

- [ ] **Configurar ambiente de desarrollo**
  - [ ] Clonar repositorio MatrixNext
  - [ ] Restaurar base de datos de desarrollo con datos de prueba
  - [ ] Verificar compilación y ejecución local

- [ ] **Crear backlog en herramienta de gestión** (Azure DevOps, Jira, etc.)
  - [ ] Crear épicas 1-7
  - [ ] Crear historias de usuario (US-001 a US-028)
  - [ ] Asignar estimaciones (Story Points)
  - [ ] Priorizar backlog

---

### 12.2 Sprint 0 (Semana 1)

- [ ] **Setup de proyecto**
  - [ ] Crear branch `feature/presupuesto-migration`
  - [ ] Configurar CI/CD pipeline (si no existe)
  - [ ] Configurar SonarQube o code quality tools

- [ ] **Sesiones de conocimiento**
  - [ ] 2h con Gerente de Cuentas (flujo completo de presupuestos)
  - [ ] 1h con Gerente de Operaciones (autorizaciones)

- [ ] **Preparar datos de prueba**
  - [ ] Exportar 50 presupuestos reales (anonimizados)
  - [ ] Crear dataset con casos edge (multimetodología, tracking)
  - [ ] Restaurar en base de datos de desarrollo

- [ ] **Definir Definition of Done**
  - [ ] Code reviewed por peer
  - [ ] Unit tests con coverage > 80%
  - [ ] Integration tests para endpoints
  - [ ] Documentación actualizada (XML comments)
  - [ ] Validado en ambiente QA

---

### 12.3 Sprint 1 (Semanas 2-3)

- [ ] **US-001**: Configurar DbContext con entidades IQ_*
- [ ] **US-002**: Implementar PresupuestoDataAdapter
- [ ] **US-003**: Migrar IQuoteCalculator (80%)
- [ ] **US-004**: Configurar DI container
- [ ] **Revisión de Sprint**: Demo de infraestructura funcionando

---

### 12.4 Sprint 2-4 (Semanas 4-9)

- [ ] **Épica 2**: Vista Principal y Alternativas (US-005 a US-009)
- [ ] **Épica 3**: Formulario de Presupuesto (US-010 a US-013)
- [ ] **Checkpoint**: Primera demo a usuarios (crear presupuesto end-to-end)

---

### 12.5 Sprint 5-8 (Semanas 10-17)

- [ ] **Épica 4**: Gestión de Muestra (US-014 a US-018)
- [ ] **Épica 5**: Grid de Presupuestos y Acciones (US-019 a US-022)
- [ ] **Épica 6**: JobBook y Reportes (US-023 a US-025)
- [ ] **Épica 7**: Flujos de Autorización (US-026 a US-028)
- [ ] **Checkpoint**: Segunda demo a usuarios (funcionalidad completa menos iQuote)

---

### 12.6 Sprint 9-10 (Semanas 18-21)

- [ ] **Testing exhaustivo**
  - [ ] UAT con usuarios reales
  - [ ] Performance testing (1000+ presupuestos)
  - [ ] Security testing (autorizaciones, roles)

---

### 12.7 Sprint 11 (Semanas 22-23)

- [ ] **Corrección de bugs de UAT**
- [ ] **Documentación final**
  - [ ] User manual (para usuarios finales)
  - [ ] Technical documentation (para developers)
  - [ ] Runbooks (para operaciones)

- [ ] **Deployment a producción**
  - [ ] Deploy con feature flag deshabilitado
  - [ ] Shadow mode (ejecutar en paralelo sin mostrar UI)
  - [ ] Habilitar para usuarios beta (5-10 usuarios)
  - [ ] Monitorear métricas y errores
  - [ ] Rollout gradual (25%, 50%, 100%)

---

### 12.8 Post-Launch (Semana 24+)

- [ ] **Monitoreo continuo**
  - [ ] Dashboard con métricas clave (tiempos de respuesta, errores)
  - [ ] Alertas configuradas en Application Insights
  - [ ] Revisión diaria de logs

- [ ] **Soporte y estabilización**
  - [ ] Hotfix de bugs críticos (< 24h)
  - [ ] Mejoras de performance si necesario
  - [ ] Capacitación adicional a usuarios

- [ ] **Deprecación de legacy**
  - [ ] Semana 28: Desactivar Presupuesto.aspx en producción
  - [ ] Semana 30: Archivar código legacy
  - [ ] Semana 32: Eliminar dependencias obsoletas (AjaxControlToolkit)

---

## ✅ CONCLUSIÓN DEL ANÁLISIS

### Resumen Ejecutivo

Este documento ha realizado un análisis exhaustivo de **Presupuesto.aspx** (3,309 líneas de código VB.NET + 1,568 líneas de markup HTML) para su migración a **ASP.NET Core MVC** en el proyecto **MatrixNext**.

**Hallazgos clave:**

1. **Complejidad ALTA** 🔴
   - Motor de cálculo IQuote con 30+ métodos (Cotizador.General)
   - 110+ parámetros de configuración (IQ_Parametros)
   - UserControl monolítico de 744 líneas

2. **Riesgos principales:**
   - Pérdida de fidelidad en cálculos si migración de IQuoteCalculator no es exacta
   - Conocimiento de negocio concentrado en pocos usuarios

3. **Estimación realista:**
   - **5.5 meses** (11 sprints) con equipo de 2 developers
   - **287 Story Points** = 1,148 horas
   - **8 épicas** con 28 historias de usuario

4. **Entregables:**
   - Sistema completo con paridad 100% funcional con legacy
   - Performance mejorada (< 3s en operaciones críticas)
   - UX modernizada con tabs, modales Bootstrap, grids interactivos

**Próximo paso crítico:**  
✅ **Aprobación de stakeholders y resolución de decisiones técnicas pendientes (DT-01, DT-02) antes de Sprint 1**

---

**Fin del Análisis** 📋

Documento generado: Enero 3, 2026  
Analista: GitHub Copilot  
Versión: 1.0  
Estado: ✅ COMPLETO - Listo para aprobación

---
