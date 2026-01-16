# TASK 1: ANALYSIS - TraficoTareas.aspx (COMPLETADA)

**Duración**: 1 hora  
**Fecha**: 2026-01-15  
**Archivo Analizado**: `WebMatrix/RE_GT/TraficoTareas.aspx.vb` (257 líneas)

---

## 📋 ANÁLISIS COMPLETO

### 1️⃣ FLUJO PRINCIPAL

**Page_Load**:
```
1. Verificar URLRetorno (parámetro QueryString)
2. Validar UnidadId (requerido, QueryString)
   - Mapeo permiso: UnidadId → Permiso específico (107-129)
   - Si permiso falso → Redirect a Home
3. Validar RolId (requerido, QueryString)
4. Validar TrabajoId (opcional, QueryString)
   - Si existe → Cargar información del trabajo
   - Si no existe → Mostrar listado de trabajos por unidad
5. Asignar URL devolucion según URLRetorno
```

---

### 2️⃣ UNIDADES SOPORTADAS (8 páginas TraficoTareas)

| UnidadId | Nombre | PermId | Rol | Origen |
|----------|--------|--------|-----|--------|
| **5** | Crítica | 107 | CoE | GestionTratamiento |
| **6** | Verificación | 109 | CoE | GestionTratamiento |
| **7** | Captura | 111 | CoE | GestionTratamiento |
| **8** | Codificación | 108 | CoE | GestionTratamiento |
| **9** | DataCleaning | 110 | CoE | GestionTratamiento |
| **10** | Procesamiento | 112 | CoE | GestionTratamiento |
| **11** | Scripting | 115 | Jefe | Recolección |
| **12** | Pilotos | 116 | Jefe | Recolección |
| **13** | Estadística | 121 | - | ES_Estadistica |
| **14** | Call Center | 129 | Jefe | Recolección |

**Nota**: Unidades 5-10 = GestionTratamiento, 11-14 = Recolección

---

### 3️⃣ COMPONENTES UI

#### **Accordion 0 - Listado de Trabajos**

```html
<h3>Trabajos</h3>
├─ Búsqueda (txtBuscar + btnBuscar)
└─ GridView gvTrabajos
   ├─ Columnas:
   │  ├─ id (ID Trabajo)
   │  ├─ JobBook
   │  ├─ NombreTrabajo
   │  ├─ Muestra
   │  ├─ NombreMetodologia
   │  ├─ NombreCOE (OMP)
   │  ├─ NombreUnidad
   │  ├─ [Avance] - Redirige a RP_Reportes/AvanceDeCampo.aspx
   │  └─ [Gestionar] - Carga Accordion 1
   ├─ PageSize: 25 registros
   ├─ AllowPaging: Yes
   └─ DataSource: WorkFlow.obtenerTrabajosWorkFlow(UnidadId, BuscarTexto)
```

#### **Accordion 1 - Gestión de Tareas**

```html
<h3>Gestión de Tareas</h3>
├─ [Módulo Tareas] → Gestion-Tareas.aspx (con params)
├─ [Listado de Documentos] → ListaDocumentosXHilos.aspx
├─ [Ver Información General] → RP_Reportes/InformacionGeneral.aspx
├─ [Personal Asignado] → Export a Excel (visible solo unidades 11,14)
└─ [Ver Trabajos] → Vuelve a Accordion 0
```

---

### 4️⃣ MÉTODOS PRINCIPALES

#### **CargarTrabajos()**
```vb
Sub CargarTrabajos()
    Dim oWorkFlow As New WorkFlow
    gvTrabajos.DataSource = oWorkFlow.obtenerTrabajosWorkFlow(hfIdUnidad.Value, Nothing)
    gvTrabajos.DataBind()
End Sub
```
**Propósito**: Llenar GridView con trabajos de la unidad  
**SP Legacy**: `obtenerTrabajosWorkFlow(idUnidad, texto_busqueda)`  
**DataAdapter**: CoreProject.WorkFlow

#### **asignarURLDevolucion()**
```vb
Sub asignarURLDevolucion()
    Select Case URLRetorno
        Case UrlOriginal.RE_GT_TraficoTareas_Critica
            lbtnVolver.PostBackUrl = "~/RE_GT/HomeGestionTratamiento.aspx"
        Case UrlOriginal.RE_GT_TraficoTareas_Scripting
            lbtnVolver.PostBackUrl = "~/RE_GT/HomeRecoleccion.aspx"
        ' ... 13 casos más
    End Select
End Sub
```
**Propósito**: Mapear URLRetorno (enum int) a URL de retorno específica  
**Casos**: 14 casos (RE_GT_TraficoTareas_*, CORE_ListaTrabajosTareas, etc.)

#### **btnBuscar_Click()**
```vb
Protected Sub btnBuscar_Click(...)
    Dim oWorkFlow As New WorkFlow
    gvTrabajos.DataSource = oWorkFlow.obtenerTrabajosWorkFlow(
        hfIdUnidad.Value, txtBuscar.Text)
    gvTrabajos.DataBind()
    ActivateAccordion(0, EffectActivateAccordion.SlideEffect)
End Sub
```
**Propósito**: Buscar trabajos por nombre/descripción

#### **gvTrabajos_RowCommand()**
```vb
Private Sub gvTrabajos_RowCommand(sender, e)
    If e.CommandName = "Gestionar" Then
        ' Cargar información del trabajo
        ' Validar tipo de proyecto (Cuali/Cuanti)
        ' Mostrar Accordion 1
    ElseIf e.CommandName = "Avance" Then
        ' Redirect a AvanceDeCampo.aspx
    End If
End Sub
```
**Propósito**: Manejar clicks en botones del GridView

#### **btnPersonalAsignado_Click()**
```vb
Private Sub btnPersonalAsignado_Click(...)
    ' Crear Excel con:
    ' IdAsignacion, TrabajoId, Nombres, Apellidos, 
    ' Identificacion, Cargo, CodDane, Ciudad
    Dim o As New CoordinacionCampoPersonal
    lstCambios = o.ListadoPersonasAsignadas(hfIdTrabajo.Value)
    ' Export a Excel
End Sub
```
**Propósito**: Descargar listado de personal asignado (solo unidades 11, 14)

---

### 5️⃣ PARÁMETROS QueryString

| Parámetro | Requerido | Tipo | Ejemplo | Propósito |
|-----------|-----------|------|---------|-----------|
| **UnidadId** | ✅ Sí | int | 5 | Filtrar por unidad OP |
| **RolId** | ✅ Sí | int | 1 | Validar rol usuario |
| **TrabajoId** | ❌ No | long | 12345 | Cargar trabajo específico |
| **URLRetorno** | ❌ No | int | 1 | Enum para navegación retorno |

---

### 6️⃣ STORED PROCEDURES IDENTIFICADOS

#### **SP Legacy**: `WorkFlow.obtenerTrabajosWorkFlow`

```sql
-- Parámetros:
@IdUnidad INT
@TextoBusqueda NVARCHAR(MAX) -- NULL para todos

-- Retorna:
-- id, JobBook, NombreTrabajo, Muestra, NombreMetodologia, 
-- NombreCOE, NombreUnidad, Estado, Prioridad, ...
```

**Ubicación en MatrixNext**: 
- ✅ Service: `CORE/Services/WorkFlowService`
- ✅ Adapter: `Data/Adapters/CORE/WorkFlowAdapter`
- ✅ Controller: `CORE/Controllers/WorkFlowController`

#### **SP Legacy**: `CoordinacionCampoPersonal.ListadoPersonasAsignadas`

```sql
-- Parámetro:
@IdTrabajo BIGINT

-- Retorna:
-- IdAsignacion, TrabajoId, Nombres, Apellidos, 
-- Identificacion, Cargo, CodDane, Ciudad
```

**Ubicación en MatrixNext**: 
- ✅ Service: `OP/Services/TraficoService`
- ✅ Adapter: `OP_Trafico/TraficoAdapter`

---

### 7️⃣ NAVEGACIÓN - URLRetorno ENUM (14 Casos)

```csharp
// MatrixNext equivalente necesario:
public enum URLRetorno : int
{
    RE_GT_TraficoTareas_Scripting = 1,      // → HomeRecoleccion
    RE_GT_TraficoTareas_Pilotos = 2,        // → HomeRecoleccion
    RE_GT_TraficoTareas_Critica = 3,        // → HomeGestionTratamiento
    RE_GT_TraficoTareas_Verificacion = 4,   // → HomeGestionTratamiento
    RE_GT_TraficoTareas_Captura = 5,        // → HomeGestionTratamiento
    RE_GT_TraficoTareas_Codificacion = 6,   // → HomeGestionTratamiento
    RE_GT_TraficoTareas_Datacleaning = 7,   // → HomeGestionTratamiento
    RE_GT_TraficoTareas_Procesamiento = 8,  // → HomeGestionTratamiento
    RE_GT_TraficoTareas_Estadistica = 9,    // → Es_Estadistica/Default
    CORE_ListaTrabajosTareas = 10,          // → Gestion-Tareas-Trabajos
    RE_GT_TrabajosPorGerencia = 11,         // → RP_Reportes/TrabajosPorGerencia
    RE_GT_TraficoEncuestasRMC = 12,         // → TraficoEncuestas (UnidadId=38)
    RE_GT_CallCenter = 13,                  // → HomeRecoleccion
    Default = 0                              // → HomeRecoleccion
}
```

---

### 8️⃣ PERMISOS - Mapeo UnidadId → PermId

```csharp
// Validación per unit
public static class UnidadPermisosMap
{
    public static Dictionary<int, int> Permisos = new()
    {
        { 5, 107 },   // Crítica
        { 6, 109 },   // Verificación
        { 7, 111 },   // Captura
        { 8, 108 },   // Codificación
        { 9, 110 },   // DataCleaning
        { 10, 112 },  // Procesamiento
        { 11, 115 },  // Scripting
        { 12, 116 },  // Pilotos
        { 13, 121 },  // Estadística
        { 14, 129 }   // Call Center
    };
}
```

---

### 9️⃣ DATOS DE SESIÓN

```vb
' Guardados en Session:
Session("TrabajoId")         ' Long
Session("NombreTrabajo")     ' String: "id | JobBook | Nombre"

' HiddenFields (ViewState):
hfIdUnidad      ' Int64 - Unidad actual
hfIdRol         ' Int64 - Rol del usuario
hfIdTrabajo     ' Long - Trabajo seleccionado
```

---

## 🎯 RESUMEN PARA MIGRACIÓN

### ✅ Requisitos Funcionales

```
1. Vista Consolidada de TraficoTareas
   ├─ Listar trabajos por unidad (5-14)
   ├─ Búsqueda/filtros
   ├─ Paginación (25 registros/página)
   ├─ Estados de trabajo (Creada, EnProgreso, Completada, Anulada)
   └─ Validación de permisos por unidad

2. Accordion 0 - Listado
   ├─ GridView con 9 columnas
   ├─ Botones de acción (Avance, Gestionar)
   ├─ Búsqueda por nombre/descripción
   └─ Paginación completa

3. Accordion 1 - Gestión
   ├─ 5 botones de navegación (Tareas, Documentos, Ficha, Personal, Volver)
   ├─ Export Excel (solo unidades 11, 14)
   └─ Navegación condicional según tipo proyecto

4. Navegación Retorno (URLRetorno)
   ├─ 14 casos de mapeo
   ├─ Redirect automático según origen
   └─ Parámetros QS en algunas rutas
```

### ✅ Tecnología a Usar

```
Controller: WorkFlowController (CORE)
Service: IWorkFlowService
Adapter: IWorkFlowAdapter (ya existe)
DTOs: TareasPorUnidadDto + TraficoTareasViewModel
Views: TraficoTareas.cshtml (consolidada)
SignalR: Opcional (actualización real-time)
```

### ✅ Puntos Críticos

```
⚠️ Permisos: Validar por unidad (107-129)
⚠️ URLRetorno: 14 casos de mapeo distintos
⚠️ Tipo Proyecto: Mostrar/ocultar btnFichaCuanti según tipo
⚠️ Session: Mantener TrabajoId y NombreTrabajo en sesión
⚠️ Excel: Export solo para unidades 11, 14
⚠️ GridView: Paginación de 25 registros
```

---

## 🚀 RECOMENDACIONES IMPLEMENTACIÓN

### **Enfoque (Recomendado)**:

1. ✅ Crear DTO `TareasPorUnidadDto` (con propiedades mapeadas)
2. ✅ Crear ViewModel `TraficoTareasViewModel` (con filtros + lista)
3. ✅ Extender `WorkFlowService` con método `ObtenerTareasPorUnidadAsync`
4. ✅ Extender `WorkFlowAdapter` para SP call
5. ✅ Crear action `TraficoTareas` en `WorkFlowController`
6. ✅ Crear view `TraficoTareas.cshtml` con Accordion bootstrap
7. ✅ Implementar enum `URLRetorno` en `UrlOriginalHelper`
8. ✅ Agregar export Excel opcional

---

## ✅ CHECKLIST TASK 1

- [x] Código legacy analizado (257 líneas)
- [x] Flujo principal documentado
- [x] Unidades soportadas mapeadas (10 casos)
- [x] Componentes UI identificados
- [x] Métodos principales documentados (6 métodos)
- [x] Parámetros QueryString catalogados
- [x] SPs identificados y ubicación en MatrixNext confirmada
- [x] Navegación URLRetorno mapeada (14 casos)
- [x] Permisos por unidad documentados (10 mapeos)
- [x] Datos de sesión identificados
- [x] Requisitos funcionales listos para TASK 2

**TASK 1 COMPLETADA ✅** - Listo para TASK 2 (DTO + ViewModel)

---

**Documentación**: 2026-01-15 15:10 UTC  
**Próxima**: TASK 2 - Crear DTOs y ViewModels (1-2 horas)
