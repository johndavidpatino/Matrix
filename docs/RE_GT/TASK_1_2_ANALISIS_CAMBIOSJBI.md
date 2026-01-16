# 📋 TASK 1.2 - ANÁLISIS: CambiosJBI.aspx

> **Sprint**: 18  
> **Task**: 1.2 - Análisis CambiosJBI  
> **Módulo**: RE_GT  
> **Fecha**: 2025-01-16  
> **ETA**: 0.5-1h

---

## 📊 RESUMEN EJECUTIVO

| Métrica | Valor |
|---------|-------|
| **Archivo** | WebMatrix/RE_GT/CambiosJBI.aspx |
| **LOC Total** | 149 LOC ASPX + 115 LOC VB = 264 LOC |
| **Complejidad** | ⭐ MEDIA |
| **Tipo** | Formulario CRUD (Change JobBook Interno) |
| **Patrón** | UpdatePanel + Modal dialogs |
| **Permisos** | ID: 19 (VerificarPermisoUsuario) |

---

## 🔍 ANÁLISIS DETALLADO

### 1. ESTRUCTURA DE PÁGINA (ASPX)

**Ubicación**: `WebMatrix/RE_GT/CambiosJBI.aspx` (149 líneas)

**Componentes Principales**:

#### Encabezado
```aspx
<%@ Page Title="" Language="vb" AutoEventWireup="false" 
         MasterPageFile="~/MasterPage/RD_F.master"
         CodeBehind="CambiosJBI.aspx.vb" 
         Inherits="WebMatrix.CambiosJBI" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
```

#### Content Sections
- **Content1 (CPH_Head)**: JavaScript para validación y controles
- **Content2 (CPH_OpcionesMenu)**: Vacío
- **Content3 (CPH_Titulo)**: "Cambios JBI"
- **Content4 (CPH_ComentFormulario)**: Vacío
- **Content5 (CPH_ContentForm)**: Formulario principal

#### UI Elements

**1. Notificaciones**:
```aspx
<div id="info" class="information ui-corner-all ui-state-highlight" style="display: none;">
  <!-- Info message -->
</div>
<div id="error" class="error_message ui-state-error ui-corner-all" style="display: none;">
  <!-- Error message -->
</div>
```

**2. UpdatePanel con Accordion**:
```aspx
<asp:UpdatePanel runat="server" ID="upDatos" ChildrenAsTriggers="false" UpdateMode="Conditional">
  <div id="accordion">
    <h3>Parámetros para realizar el cambio de JBI</h3>
    <div class="block">
      <!-- Formulario -->
    </div>
  </div>
</asp:UpdatePanel>
```

**3. Formulario de Entrada**:
```
- txtIdTrabajo: TextBox para ID Trabajo
- ddlFases: DropDownList de fases (cargado dinámicamente)
- txtNuevoJBI: TextBox para Nuevo JobBook (validación de formato 99-999999-99-99)
- btnCambiarJBI: Button para ejecutar cambio
```

**4. JavaScript**:
- jQuery Accordion
- jQuery Dialog modal
- jQuery Validation
- Input mask para formato JBI
- ActualizarPresupuestosAsignados(): manejo de checkboxes en presupuestos

---

### 2. CODE-BEHIND (VB.NET)

**Ubicación**: `WebMatrix/RE_GT/CambiosJBI.aspx.vb` (115 líneas)

**Clase**: `CambiosJBI` inherits `System.Web.UI.Page`

#### Métodos Principales

**1. CambiarJobBookInterno()**
```vb
Sub CambiarJobBookInterno()
    Dim iqent As New IQ.Consultas
    iqent.CambiarJBI(txtIdTrabajo.Text, ddlFases.SelectedValue, txtNuevoJBI.Text)
End Sub
```
- Crea instancia de IQ.Consultas (CoreProject)
- Ejecuta SP: `iqent.CambiarJBI()`
- **SP**: Probablemente `IQ_JBI.CambiarJBI` o similar

**2. CargarFases()**
```vb
Sub CargarFases()
    Dim iqent As New IQ.Consultas
    Me.ddlFases.DataSource = iqent.FasesList
    Me.ddlFases.DataValueField = "IdFase"
    Me.ddlFases.DataTextField = "DescFase"
    Me.ddlFases.DataBind()
    Me.ddlFases.Items.Insert(0, New ListItem With {.Value = "-1", .Text = "--Seleccione--"})
End Sub
```
- **Data Source**: `IQ.Consultas.FasesList` (desde CoreProject)
- **Fields**: IdFase, DescFase
- Agrega item vacío por defecto (-1)
- **SP**: Probablemente `IQ_Fase.ObtenerActivas`

**3. ValidadFaseCreada()** (sic - typo en nombre)
```vb
Sub ValidadFaseCreada()
    ' Validaciones:
    ' 1. txtIdTrabajo no vacío
    ' 2. ddlFases ≠ -1 (seleccionado)
    ' 3. txtNuevoJBI no vacío y formato correcto
    ' 4. Trabajo existe (oTrabajo.obtenerXId)
    ' 5. Fase está creada en presupuestos
    
    ' Si todo OK:
    CambiarJobBookInterno()
    logCambiosJBI(infoT.JobBook)
    ShowNotification("El JobBook Interno ha sido cambiado Correctamente!")
    limpiarControles()
End Sub
```

**4. logCambiosJBI()**
```vb
Sub logCambiosJBI(ByVal jBIAnterior As String)
    Dim iqent As New IQ.Consultas
    Dim UsuarioID As Int64 = Int64.Parse(Session("IDUsuario").ToString())
    iqent.GuardarLogCambiosJBI(txtIdTrabajo.Text, jBIAnterior, txtNuevoJBI.Text, UsuarioID)
End Sub
```
- **SP**: `IQ_JBI.GuardarLogCambiosJBI()` o similar
- **Parámetros**: IdTrabajo, JBIAnterior, NuevoJBI, UsuarioID
- Auditoría de cambios

**5. limpiarControles()**
```vb
Sub limpiarControles()
    txtIdTrabajo.Text = ""
    ddlFases.SelectedValue = "-1"
    txtNuevoJBI.Text = ""
End Sub
```

#### Eventos

**Page_Load**:
```vb
Protected Sub Page_Load(...) Handles Me.Load
    If Not IsPostBack Then
        ' Verificar permiso 19
        If permisos.VerificarPermisoUsuario(19, UsuarioID) = False Then
            Response.Redirect("../Home.aspx")
        End If
        CargarFases()
    End If
End Sub
```
- **Permiso Requerido**: ID = 19
- **Validación**: Al cargar la página (no postback)
- **Acción si sin permiso**: Redirige a Home

**btnCambiarJBI_Click**:
```vb
Protected Sub btnCambiarJBI_Click(sender As Object, e As EventArgs) Handles btnCambiarJBI.Click
    ValidadFaseCreada()
End Sub
```

---

## 🗄️ BASE DE DATOS

### SPs Identificados

1. **IQ.Consultas.FasesList**
   - **Propósito**: Obtener lista de fases activas
   - **Retorna**: IdFase, DescFase
   - **SP Probable**: `IQ_Fase.ObtenerActivas` o `SP_IQ_ObtenerFases`

2. **IQ.Consultas.CambiarJBI()**
   - **Propósito**: Cambiar JobBook Interno de un trabajo
   - **Parámetros**: IdTrabajo, IdFase, NuevoJBI
   - **SP Probable**: `IQ_JBI.CambiarJBI` o `SP_IQ_CambiarJobBook`
   - **Acciones**: UPDATE trabajo SET JBI = @NuevoJBI WHERE IdTrabajo = @IdTrabajo

3. **IQ.Consultas.GuardarLogCambiosJBI()**
   - **Propósito**: Guardar log de cambios de JBI
   - **Parámetros**: IdTrabajo, JBIAnterior, NuevoJBI, UsuarioID
   - **SP Probable**: `IQ_JBI.GuardarLogCambios` o `SP_IQ_LogCambiosJBI`
   - **Acciones**: INSERT INTO LogCambiosJBI (IdTrabajo, JBIAnterior, JBINuevo, IdUsuario, FechaCambio)

4. **Trabajo.obtenerXId()**
   - **Propósito**: Obtener información de trabajo por ID
   - **Parámetros**: IdTrabajo
   - **Retorna**: Trabajo object con properties: IdTrabajo, IdPropuesta, Alternativa, JobBook, MetCodigo

5. **ControlCostos.ObtenerParametros()**
   - **Propósito**: Validar que la fase existe en presupuestos
   - **Parámetros**: IdPropuesta, Alternativa, IdFase, MetCodigo
   - **Retorna**: Lista de parámetros (>0 = fase creada)

6. **Datos.ClsPermisosUsuarios.VerificarPermisoUsuario()**
   - **Propósito**: Verificar si usuario tiene permiso
   - **Parámetros**: PermissionId (19), UsuarioID
   - **Retorna**: Boolean

---

## 🎯 CASOS DE USO

### Caso 1: Cambio Exitoso
```
1. Usuario ingresa ID Trabajo
2. Sistema valida que existe
3. Usuario selecciona Fase
4. Sistema valida que fase existe en presupuestos
5. Usuario ingresa nuevo JBI (formato: 99-999999-99-99)
6. Usuario hace click en "Cambiar JobBook Interno"
7. Sistema:
   - Guarda cambio en BD
   - Registra log con JBI anterior
   - Muestra mensaje de éxito
   - Limpia formulario
```

### Caso 2: Error - Validación
```
Si falta información o formato incorrecto:
- Muestra mensaje de error específico
- Cambia a acordeón 0
- Focus en control problemático
```

### Caso 3: Error - Permisos
```
Si usuario sin permiso 19:
- Redirige a Home.aspx
```

---

## 📋 ENTIDADES RELACIONADAS

**Tablas**:
- `Trabajos` (IdTrabajo, JBI, IdPropuesta, Alternativa, JobBook)
- `Fases` (IdFase, DescFase)
- `Presupuestos` (IdPropuesta, Alternativa, IdFase, MetCodigo) - validación
- `LogCambiosJBI` (IdTrabajo, JBIAnterior, JBINuevo, IdUsuario, FechaCambio)

**Objetos CoreProject**:
- `IQ.Consultas` (métodos: FasesList, CambiarJBI, GuardarLogCambiosJBI)
- `Trabajo` (obtenerXId)
- `ControlCostos` (ObtenerParametros)
- `Datos.ClsPermisosUsuarios` (VerificarPermisoUsuario)

---

## 🔒 SEGURIDAD

**Validaciones Implementadas**:
1. ✅ Permiso requerido (ID: 19) al cargar página
2. ✅ Validación de datos de entrada (no vacíos)
3. ✅ Validación de formato JBI (máscara: 99-999999-99-99)
4. ✅ Validación de existencia de trabajo
5. ✅ Validación de existencia de fase en presupuestos
6. ✅ Auditoría de cambios (log)

**Patrones Utilizados**:
- UpdatePanel para postback parcial
- Session para usuario ID
- Mensaje de error/éxito UI
- Datos privados en Session

---

## 📊 ESTADÍSTICAS

| Métrica | Valor |
|---------|-------|
| **LOC ASPX** | 149 |
| **LOC VB** | 115 |
| **LOC Total** | 264 |
| **Métodos** | 6 (+ 1 event handler) |
| **Controles ASP.NET** | 4 (TextBox, DropDownList, Button, UpdatePanel) |
| **SPs** | 6 |
| **Permisos** | 1 (ID: 19) |
| **Validaciones** | 5 |

---

## 🎯 COMPLEJIDAD ESTIMADA

**Rating**: ⭐⭐ MEDIA (2/5)

**Razones**:
- ✅ Lógica simple (cambiar un campo)
- ✅ Controles estándar ASP.NET
- ✅ Sin loops complejos
- ⚠️ Múltiples validaciones
- ⚠️ Interacción con múltiples SPs
- ⚠️ Auditoría requerida

**Estimación Migración**: 2-3 horas

---

## 🔄 MIGRACIÓN A MATRIXNEXT

### Patrón Propuesto

```csharp
// 1. DTO
public class CambioJBIDto
{
    public int IdTrabajo { get; set; }
    public int IdFase { get; set; }
    public string NuevoJBI { get; set; }
    public string JBIAnterior { get; set; }
}

// 2. Service
public interface ICambioJBIService
{
    Task<IEnumerable<FaseDto>> ObtenerFasesAsync();
    Task<TrabajoDto> ObtenerTrabajoAsync(int idTrabajo);
    Task<(bool success, string message)> CambiarJBIAsync(CambioJBIDto dto, int usuarioId);
}

// 3. Controller
[Area("RE_GT")]
[Authorize(Roles = "Permiso_19")]
public class CambioJBIController : Controller
{
    [HttpGet]
    public async Task<IActionResult> Index()
    {
        var fases = await _service.ObtenerFasesAsync();
        return View(fases);
    }
    
    [HttpPost]
    public async Task<IActionResult> Cambiar(CambioJBIDto dto)
    {
        var (success, message) = await _service.CambiarJBIAsync(dto, User.GetUserId());
        return Json(new { success, message });
    }
}

// 4. View
<!-- Bootstrap 5 form -->
```

---

## ✅ CHECKLIST MIGRACIÓN

- [ ] DTO creado (CambioJBIDto)
- [ ] Service creado (ICambioJBIService)
- [ ] Adapter creado (CambioJBIAdapter)
- [ ] Controller creado (CambioJBIController)
- [ ] View creada (Index.cshtml)
- [ ] SPs verificados en MatrixNext/docs/SQL/
- [ ] Permisos mapeados (permiso 19 → [Authorize])
- [ ] DI registrado en Program.cs
- [ ] Build verificado (0 errores)
- [ ] Documentación completada

---

## 📝 NOTAS ADICIONALES

**Dependencias**:
- IQ_Fase (SP para obtener fases)
- IQ_Trabajos (SP para obtener trabajo)
- IQ_Presupuestos (SP para validar fase)
- IQ_JBI (SPs para cambiar y loguear)

**Consideraciones Especiales**:
- Validación de formato JBI es crítica (máscara: 99-999999-99-99)
- Auditoría obligatoria (guardar JBI anterior)
- Permiso 19 debe estar mapeado correctamente
- Fase debe existir en presupuestos (validación de negocio)

---

**Responsable**: GitHub Copilot  
**Fecha**: 2025-01-16  
**Status**: ✅ COMPLETADO
