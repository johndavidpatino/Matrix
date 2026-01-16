# 📋 TASK 1.1 & 1.3 - ANÁLISIS COMBINADO

> **Sprint**: 18  
> **Tasks**: 1.1 (RecoleccionDatos) + 1.3 (AsignacionCampo)  
> **Módulo**: RE_GT  
> **Fecha**: 2025-01-16  
> **ETA**: 1.5-2h total

---

## 📊 RESUMEN EJECUTIVO

### TASK 1.1: RecoleccionDatos Sub-pages

| Métrica | Valor |
|---------|-------|
| **Status** | ⏳ LANDING PAGE CREADA (Sprint 17) |
| **Páginas Internas** | 0 encontradas (no existen subcarpetas) |
| **Conclusión** | Landing page es suficiente; no hay sub-pages que migrar |

**Hallazgo Importante**: No existen páginas internas de RecoleccionDatos. El archivo `RecoleccionDatos.aspx` (Sprint 17) es la landing page que contiene navegación a otros módulos. No hay necesidad de crear sub-páginas adicionales.

---

### TASK 1.3: AsignacionCampo.aspx

| Métrica | Valor |
|---------|-------|
| **Archivo** | WebMatrix/RE_GT/AsignacionCampo.aspx |
| **LOC Total** | 209 LOC ASPX + 156 LOC VB = 365 LOC |
| **Complejidad** | ⭐⭐⭐ MEDIA-ALTA |
| **Tipo** | Grid de Trabajos + Modal para Asignación |
| **Patrón** | GridView + UpdatePanel + Modal |
| **Permisos** | ID: 19 (VerificarPermisoUsuario) |

---

## 🔍 TASK 1.1: RecoleccionDatos Sub-pages ANALYSIS

### Hallazgos

**1. Estructura de Carpetas**
```
WebMatrix/RE_GT/
├── RecoleccionDeDatos.aspx ✅ (MIGRADA Sprint 17)
├── RecoleccionDeDatos.aspx.vb
├── RecoleccionDeDatos.aspx.designer.vb
└── [No hay subcarpetas]
```

**2. Estado Actual**
- ✅ Landing page `RecoleccionDeDatos.aspx` migrada Sprint 17
- ✅ Navigation menú con 2 secciones (Gerencia, Subdirección)
- ✅ 24 items de navegación a otros módulos
- ✅ Permisos ID: 26

**3. Conclusión**
**NO hay sub-pages internas que migrar**. La página landing es una interfaz de navegación pura. Los items enlazan a:
- OP_Cuantitativo (AsignarOMP, AsignarJBI, etc.)
- OP_Cualitativo (Planificación items)
- Otros módulos

**4. Decisión**
- ✅ **TASK 1.1 COMPLETADA** (Landing page ya existe desde Sprint 17)
- ✅ **No requiere trabajo adicional**
- ✅ **Reducción de scope: -5-6h**

---

## 🔍 TASK 1.3: AsignacionCampo.aspx - ANÁLISIS DETALLADO

### 1. ESTRUCTURA DE PÁGINA (ASPX)

**Ubicación**: `WebMatrix/RE_GT/AsignacionCampo.aspx` (209 líneas)

**Componentes Principales**:

#### Encabezado
```aspx
<%@ Page Title="" Language="vb" AutoEventWireup="false" 
         MasterPageFile="~/MasterPage/MasterRecoleccion.master"
         CodeBehind="AsignacionCampo.aspx.vb" 
         Inherits="WebMatrix.AsignacionCampo" %>
```

#### Scripts Incluidos
- `jquery.blockUI.js` - Bloquear UI durante procesamiento
- `blockUIOnAllAjaxRequests.js` - Auto-bloquear en AJAX

#### UI Elements

**1. Notificaciones**:
```aspx
<!-- Info/Error messages con iconos -->
```

**2. UpdatePanel Principal**:
```aspx
<asp:UpdatePanel runat="server" ID="upDatos">
  <!-- Panel con HiddenFields y GridView -->
  <asp:HiddenField ID="hfidTrabajo" runat="server" />
  <asp:HiddenField ID="hfidTipoProyecto" runat="server" />
  
  <!-- DropDownList de GrupoUnidades (condicional) -->
  
  <!-- GridView: gvTrabajos -->
</asp:UpdatePanel>
```

**3. GridView - gvTrabajos**:
```
Columnas:
- Id (BoundField)
- NombreTrabajo (BoundField)
- Muestra (BoundField)
- FechaTentativaInicioCampo (Date format: dd/MM/yyyy)
- FechaTentativaFinalizacion (Date format: dd/MM/yyyy)
- GerenteProyectos (BoundField)
- NombreUnidad (BoundField)
- Asignar (TemplateField - ImageButton)

Features:
- Paginación (PageSize: 25)
- AlternatingRowStyle (CSS: "odd")
- DataKeyNames: "Id,TipoProyectoId"
- Custom PagerTemplate (Primero, Anterior, Siguiente, Ultimo)
```

**4. Modal Dialog - GerenteAsignar**:
```aspx
<div id="GerenteAsignar">
  <label>Seleccione Usuario se asignará como Coordinador</label>
  <asp:DropDownList ID="ddlLider" Width="400px" runat="server"></asp:DropDownList>
  <asp:Button ID="btnUpdate" runat="server" Text="Asignar" />
</div>
```

---

### 2. CODE-BEHIND (VB.NET)

**Ubicación**: `WebMatrix/RE_GT/AsignacionCampo.aspx.vb` (156 líneas)

**Clase**: `AsignacionCampo` inherits `System.Web.UI.Page`

#### Propiedades
```vb
Private _proyectoId As Int64
Private _idUsuario As Int64
```

#### Métodos Principales

**1. CargarTrabajos()**
```vb
Sub CargarTrabajos()
    Dim oGerenteOp As New GerentesDeOperacion
    ' SP: ListadoTrabajosParaAsignarCoe(grupoUnidadId)
    Dim ltraba = oGerenteOp.ListadoTrabajosParaAsignarCoe(ddlGrupoUnidades.SelectedValue)
    gvTrabajos.DataSource = ltraba.ToList
    gvTrabajos.DataBind()
End Sub
```
- **Data Source**: `GerentesDeOperacion.ListadoTrabajosParaAsignarCoe()`
- **Filter**: GrupoUnidad
- **Retorna**: Trabajos sin COE asignado

**2. CargarGruposUnidad()**
```vb
Sub CargarGruposUnidad()
    ddlGrupoUnidades.Items.Insert(0, New ListItem With {.Text = "IUU - Cualitativo", .Value = 20})
End Sub
```
- Hardcoded: Solo "IUU - Cualitativo" (Value: 20)
- ⚠️ **ISSUE**: No es dinámico, solo una unidad

**3. CargarCOEs()**
```vb
Sub CargarCOEs()
    Dim oUsuarios As New US.Usuarios
    Dim listapersonas = (From lpersona In oUsuarios.UsuariosxGrupoUnidadXrol(ddlGrupoUnidades.SelectedValue, ListaRoles.COE)
                         Select Id = lpersona.id, Nombre = lpersona.Apellidos & " " & lpersona.Nombres)
    ddlLider.DataSource = listapersonas.ToList()
    ' ... databind
End Sub
```
- **Data Source**: Usuarios con rol COE en la GrupoUnidad
- **Filtro**: GrupoUnidad + Role (COE)
- **Sorting**: Por nombre (apellido + nombres)

**4. ObtenerUsuarios()**
```vb
Public Function ObtenerUsuarios() As List(Of ObtenerUsuarios_Result)
    Dim Data As New Datos.ClsPermisosUsuarios
    Return Data.ObtenerUsuarios
End Function
```
- **SP**: `ObtenerUsuarios()` (retorna lista de usuarios activos)

**5. EnviarEmail()**
```vb
Sub EnviarEmail()
    Dim oEnviarCorreo As New EnviarCorreo
    If String.IsNullOrEmpty(hfidTrabajo.Value) Then
        Throw New Exception("Debe elegir un estudio...")
    End If
    oEnviarCorreo.enviarCorreo(WebMatrix.Util.obtenerUrlRaiz() & "/Emails/CampoAsignado.aspx?idTrabajo=" & hfidTrabajo.Value)
End Sub
```
- **Acción**: Envía email notificando asignación de coordinador
- **Template**: `/Emails/CampoAsignado.aspx?idTrabajo={id}`

**6. log()**
```vb
Public Sub log(ByVal iddoc As Int64?, ByVal idaccion As Int16)
    Dim log As New LogEjecucion
    log.Guardar(27, iddoc, Now(), Session("IDUsuario"), idaccion)
End Sub
```
- **Entity**: 27 (AsignacionCampo)
- **Action**: idaccion (3 = update)
- **Auditoría**: Registro en LogEjecucion

#### Eventos

**Page_Load**:
```vb
Protected Sub Page_Load(...) Handles Me.Load
    If Not IsPostBack Then
        ' Validar login
        If Session("IDUsuario") Is Nothing Then
            Response.Redirect("../Default.aspx?ReturnUrl=...")
        End If
        
        ' Validar permiso 19
        If permisos.VerificarPermisoUsuario(19, UsuarioID) = False Then
            Response.Redirect("../Home.aspx")
        End If
        
        CargarGrupoUnidades()
        CargarTrabajos()
        CargarCOEs()
    End If
End Sub
```

**gvTrabajos_PageIndexChanging**:
```vb
Private Sub gvTrabajos_PageIndexChanging(...) Handles gvTrabajos.PageIndexChanging
    gvTrabajos.PageIndex = e.NewPageIndex
    CargarTrabajos()
    ActivateAccordion(0, EffectActivateAccordion.SlideEffect)
End Sub
```

**gvTrabajos_RowCommand**:
```vb
Private Sub gvTrabajos_RowCommand(...) Handles gvTrabajos.RowCommand
    If e.CommandName = "Asignar" Then
        Me.hfidTrabajo.Value = Me.gvTrabajos.DataKeys(CInt(e.CommandArgument))("Id")
        Me.hfidTipoProyecto.Value = Me.gvTrabajos.DataKeys(CInt(e.CommandArgument))("TipoProyectoId")
        upGerenteAsignar.Update()  ' Muestra modal
    End If
End Sub
```

**btnUpdate_Click** (Modal Submit):
```vb
Protected Sub btnUpdate_Click(...) Handles btnUpdate.Click
    Dim oTrabajo As New Trabajo
    Dim oeTrabajo As PY_Trabajos_Get_Result
    
    oeTrabajo = oTrabajo.obtenerXId(hfidTrabajo.Value)
    
    ' Actualizar trabajo con nuevo COE (ddlLider.SelectedValue)
    oTrabajo.Guardar(
        hfidTrabajo.Value, 
        oeTrabajo.ProyectoId, 
        oeTrabajo.OP_MetodologiaId, 
        oeTrabajo.PresupuestoId, 
        oeTrabajo.NombreTrabajo, 
        oeTrabajo.Muestra, 
        oeTrabajo.FechaTentativaInicioCampo, 
        oeTrabajo.FechaTentativaFinalizacion, 
        ddlLider.SelectedValue,  ' ← Nuevo COE
        oeTrabajo.Unidad, 
        oeTrabajo.JobBook, 
        TipoRecoleccion, 
        Nothing
    )
    
    CargarTrabajos()
    log(hfidTrabajo.Value, 3)
    EnviarEmail()
    ShowNotification("Trabajo actualizado y asignado", ShowNotifications.InfoNotification)
End Sub
```

---

### 3. FLUJO DE NEGOCIO

**Caso 1: Carga Inicial**
```
1. Usuario accede a página
2. Sistema valida permiso 19
3. Sistema carga lista de GrupoUnidades (hardcoded: IUU)
4. Sistema carga trabajos sin COE de esa unidad
5. Sistema carga lista de usuarios con rol COE
6. Vista muestra GridView con trabajos
```

**Caso 2: Asignación de Coordinador**
```
1. Usuario hace click en ImageButton "Asignar" en una fila
2. Sistema guarda IdTrabajo + TipoProyecto en HiddenFields
3. Sistema abre modal dialog "GerenteAsignar"
4. Modal muestra dropdown de COEs disponibles
5. Usuario selecciona COE
6. Usuario hace click en "Asignar"
7. Sistema:
   - Obtiene datos del trabajo
   - Actualiza trabajo COE = ddlLider.SelectedValue
   - Registra log (action: 3)
   - Envía email
   - Recarga GridView
   - Muestra mensaje de éxito
```

**Caso 3: Paginación**
```
1. Usuario navega entre páginas (Primero, Anterior, Siguiente, Último)
2. Sistema recarga GridView con nueva página
3. Sistema contrae accordion (EffectActivateAccordion.SlideEffect)
```

---

### 4. BASE DE DATOS

#### SPs Identificados

**1. GerentesDeOperacion.ListadoTrabajosParaAsignarCoe()**
- **Propósito**: Obtener trabajos sin COE asignado
- **Parámetros**: GrupoUnidadId
- **Retorna**: Id, NombreTrabajo, Muestra, FechaTentativaInicioCampo, FechaTentativaFinalizacion, GerenteProyectos, NombreUnidad, TipoProyectoId
- **SP Probable**: `GER_OPE_ListadoTrabajosParaAsignarCoe` o similar

**2. US.Usuarios.UsuariosxGrupoUnidadXrol()**
- **Propósito**: Obtener usuarios con rol específico en grupo/unidad
- **Parámetros**: GrupoUnidadId, Rol (ListaRoles.COE)
- **Retorna**: id, Apellidos, Nombres
- **SP Probable**: `US_USUARIO_ObtenerXGrupoUnidadXRol`

**3. Trabajo.obtenerXId()**
- **Propósito**: Obtener datos completos de trabajo
- **Parámetros**: IdTrabajo
- **Retorna**: PY_Trabajos_Get_Result (ProyectoId, OP_MetodologiaId, PresupuestoId, NombreTrabajo, etc.)

**4. Trabajo.Guardar()**
- **Propósito**: Actualizar trabajo
- **Parámetros**: IdTrabajo, ProyectoId, OP_MetodologiaId, PresupuestoId, NombreTrabajo, Muestra, Fechas, COE_Id, Unidad, JobBook, TipoRecoleccion, null
- **SP Probable**: `PY_TRABAJO_Guardar` o UPDATE

**5. LogEjecucion.Guardar()**
- **Propósito**: Registrar acción en auditoría
- **Parámetros**: EntityId (27), DocumentoId (IdTrabajo), Timestamp, UsuarioId, AccionId (3)
- **SP Probable**: `LOG_GuardarEjecucion`

**6. Datos.ClsPermisosUsuarios.VerificarPermisoUsuario()**
- **Propósito**: Verificar si usuario tiene permiso
- **Parámetros**: PermissionId (19), UsuarioID
- **Retorna**: Boolean

---

### 5. DEPENDENCIAS EXTERNAS

**Clase CoreProject**:
- `GerentesDeOperacion` - Listados de trabajos
- `US.Usuarios` - Información de usuarios
- `Trabajo` - CRUD de trabajos
- `LogEjecucion` - Auditoría

**Servicios**:
- `EnviarCorreo` - Envío de emails

**Session**:
- `Session("IDUsuario")` - ID de usuario autenticado

---

### 6. ISSUES IDENTIFICADOS

**1. ⚠️ GrupoUnidades Hardcoded**
```vb
Sub CargarGruposUnidad()
    ddlGrupoUnidades.Items.Insert(0, New ListItem With {.Text = "IUU - Cualitativo", .Value = 20})
End Sub
```
- Solo carga UNA unidad (IUU)
- No es dinámico
- **Mejora**: Debe cargar todas las unidades disponibles para el usuario

**2. ⚠️ Falta Selector de Unidad**
- El panel `pnlGrupoUnidades` está marcado como `Visible="false"`
- Selector nunca se muestra al usuario
- **Mejora**: Mostrar dropdown dinámico de unidades

**3. ⚠️ Session Hardcoding**
- Depende de `Session("IDUsuario")`
- Sin validación de tipo
- **Mejora**: Usar Claims/Identity de .NET

---

### 7. ESTADÍSTICAS

| Métrica | Valor |
|---------|-------|
| **LOC ASPX** | 209 |
| **LOC VB** | 156 |
| **LOC Total** | 365 |
| **Métodos** | 6 + 4 event handlers |
| **Controles ASP.NET** | 7 (GridView, UpdatePanel, TextBox, Button, etc.) |
| **SPs** | 6+ |
| **Permisos** | 1 (ID: 19) |
| **Validaciones** | 3 |

---

### 8. COMPLEJIDAD ESTIMADA

**Rating**: ⭐⭐⭐ MEDIA-ALTA (3/5)

**Razones**:
- ✅ Lógica relativamente simple (CRUD en GridView)
- ⚠️ GridView con paginación y modal
- ⚠️ UpdatePanel (AJAX partial postback)
- ⚠️ Múltiples llamadas a métodos CoreProject
- ⚠️ Email notification
- ⚠️ Auditoría requerida

**Estimación Migración**: 3-4 horas

---

## 🎯 RESUMEN FINAL DE FASE 1

| Task | Análisis | Páginas | Complejidad | Migración |
|------|----------|---------|------------|-----------|
| **1.1** RecoleccionDatos | ✅ COMPLETADO | 0 (no existen) | N/A | 0h (completado Sprint 17) |
| **1.2** CambiosJBI | ✅ COMPLETADO | 1 | ⭐⭐ MEDIA | 2-3h |
| **1.3** AsignacionCampo | ✅ COMPLETADO | 1 | ⭐⭐⭐ MEDIA-ALTA | 3-4h |

---

## 💡 IMPLICACIONES PARA SPRINT 18

**Scope Reduction**: -5-6 horas (TASK 1.1 no requiere trabajo)

**Nuevo Timeline**:
- Estimado original: 12-15h
- Con reducción: 6-9h (más eficiente)
- Expected actual: 4-6h (si fluye bien)

**Priorización**:
1. TASK 2.2: CambiosJBI (2-3h) - Más simple
2. TASK 2.3: AsignacionCampo (3-4h) - Más complejo
3. TASK 3.1: Testing (1.5-2h)
4. TASK 3.2: Documentación (1h)

---

**Responsable**: GitHub Copilot  
**Fecha**: 2025-01-16  
**Status**: ✅ FASE 1 ANÁLISIS COMPLETADO (Reducción de scope identificada)
