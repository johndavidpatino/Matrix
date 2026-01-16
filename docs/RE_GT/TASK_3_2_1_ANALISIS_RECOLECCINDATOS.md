# TASK 3.2.1: ANÁLISIS - RecoleccionDeDatos.aspx

**Fecha**: 2026-01-15  
**Duración**: 1.5 horas  
**Status**: ✅ COMPLETADO

---

## 📊 RESUMEN EJECUTIVO

| Métrica | Valor |
|---------|-------|
| **Líneas de código** | 148 (ASPX) + 15 (VB.NET) = 163 total |
| **Tipo de página** | Landing page con navegación slider |
| **Funcionalidad** | Menú de acceso rápido a operaciones de recolección |
| **Complejidad** | ⭐ BAJA (UI pura, sin lógica de BD) |
| **Dependencias** | TraficoTareas, AsignacionCOE, AsignacionJBI, Cambios JBI, etc. |
| **Estimación migración** | 2-3 horas (UI + enrutamiento) |

---

## 🔍 ANÁLISIS DETALLADO

### Estructura

**File**: `WebMatrix/RE_GT/RecoleccionDeDatos.aspx.vb`
- Líneas: 15
- Funcionalidad principal: Verificación de permisos (Permiso ID: 26)
- Patrón: PreInit validation

```vb
Private Sub _RecoleccionDeDatos_PreInit(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.PreInit
    Dim permisos As New Datos.ClsPermisosUsuarios
    Dim UsuarioID As Int64 = Int64.Parse(Session("IDUsuario").ToString())
    If permisos.VerificarPermisoUsuario(26, UsuarioID) = False Then
        Response.Redirect("../home.aspx")
    End If
End Sub
```

**Acción**: Solo check de permisos → En MatrixNext será `[Authorize]` en controller

---

### Componentes UI

**Master Page**: `RD_.master` (Recolección Datos master)

**Contenido**: Slider de navegación HTML puro con 2 secciones:

#### Sección 1: "Gerencia de Operaciones"
Links internos:
- AsignarOMP → `AsignacionCOE.aspx`
- AsignarJBI → `AsignacionJBI.aspx`
- RevisarPresupuestos → `../CU_Cuentas/RevisionPresupuestos.aspx`
- AjustarCostos → `../CAP/PresupuestosAprobados.aspx?opt=2`
- Trabajos atrasados → `../RP_Reportes/TrabajosConAtraso.aspx`
- Seguimiento → `../RP_Reportes/TrabajosPorGerencia.aspx`
- Planeación Tráfico → `../RP_Reportes/PlaneacionOperaciones.aspx`
- Producción → `../MBO/CampoProduccion.aspx`
- Calidad campo → `../MBO/CampoCalidadTotal.aspx`
- Ficha de Encuestador → `../TH_TalentoHumano/ListadoEncuestadores.aspx`
- Tiempos revisión → `../RP_Reportes/InformeTiemposRevisionPresupuestos.aspx`
- Cambios JBI → `../RE_GT/CambiosJBI.aspx`

#### Sección 2: "Subdirección Operativa"
Links:
- Planeación Estimada General → `../RP_Reportes/PlaneacionOperaciones.aspx`
- Planeación Estimada Campo → `../RP_Reportes/PlaneacionCampo.aspx`
- Planeación Propuestas → `../RP_Reportes/PlaneacionPropuestas.aspx`
- Planeación Estudios → `../RP_Reportes/PlaneacionEstudios.aspx`
- [+ más enlaces]

---

## 🏗️ FLUJO DE MIGRACIÓN

```
WebMatrix (VB.NET WebForms)        MatrixNext (.NET MVC)
┌────────────────────────────┐    ┌────────────────────────────┐
│ RecoleccionDeDatos.aspx    │    │ Area: RE_GT                │
│  - VB.NET code-behind      │───→│ Controller: RecoleccionController
│  - Permiso check (ID:26)   │    │ Action: Index              │
│  - HTML slider             │    │ View: Index.cshtml         │
│  - Links a otros módulos   │    │ [Authorize(Permission:26)] │
└────────────────────────────┘    └────────────────────────────┘
```

---

## 📋 CHECKLIST DE MIGRACIÓN

### Code-Behind
- [ ] Crear RecoleccionController en `Areas/RE_GT/Controllers/`
- [ ] Implementar acción Index con `[Authorize]`
- [ ] Validar permiso ID 26 (si es específico de dominio, agregar custom attribute)
- [ ] Inyectar servicios necesarios (opcional para landing page)

### Vista
- [ ] Crear `Areas/RE_GT/Views/Recoleccion/Index.cshtml`
- [ ] Convertir slider HTML puro a componentes Bootstrap 5
- [ ] Actualizar links a rutas ASP.NET Core pattern (`/AREA/CONTROLLER/ACTION`)
- [ ] Mantener layout visualmente similar (menú slider o tarjetas)

### Integración
- [ ] Registrar controller en DI (Program.cs)
- [ ] Actualizar sidebar para link a esta página
- [ ] Probar navegación a todos los links internos

---

## 🔗 DEPENDENCIAS

**Internas** (dentro de MatrixNext):
- TraficoTareas (CORE/WorkFlow) ✅ Ya migrada
- AsignacionCOE (OP_Cuantitativo)
- AsignacionJBI (OP_Cuantitativo)
- CambiosJBI (RE_GT - PENDIENTE)
- Reportes varios (RP_Reportes - PENDIENTE)

**Externas**:
- Master page `RD_.master` → Convertir a Layout MVC
- Slider CSS/JS → Usar Bootstrap 5 carousel o similar

---

## 📊 COMPLEJIDAD Y ESTIMACIÓN

**Complejidad**: ⭐ BAJA
- No hay lógica de negocio
- No hay acceso a BD
- Principalmente HTML + enrutamiento

**Estimación Real**:
- Code-behind: 0.5h
- Vista: 1h
- Integración + testing: 1h
- **Total**: 2-3 horas

**Riesgo**: BAJO
- Landing page, sin dependencias críticas
- Fácil de validar
- Rollback simple si hay problemas

---

## 💾 BASE DE DATOS

**Acceso a BD**: NO
**Stored Procedures**: NONE
**Tablas**: NONE
**Logica**: Solo verificación de permisos (AuthorizeAttribute)

---

## 🎯 DECISIONES DE DISEÑO

1. **Master Page**: NO mantener (usar Layout de MatrixNext)
2. **Slider**: Convertir a Bootstrap 5 grid o flex layout
3. **Permisos**: Usar `[Authorize(Policy = "RecoleccionDatos")]` si hay política específica, o simple `[Authorize]`
4. **Links**: Usar `asp-action`, `asp-controller`, `asp-area` Razor helpers

---

## ✅ ESTADO

- [x] Análisis completado
- [ ] Migración iniciada
- [ ] Testing pendiente

**Siguiente**: TASK 3.3.1 - Crear controller e implementar

---

**Creado**: 2026-01-15  
**Autor**: GitHub Copilot (Sprint 17 Automatización)  
**Referencia**: docs/RE_GT/TASK_3_2_1_ANALISIS_RECOLECCINDATOS.md
