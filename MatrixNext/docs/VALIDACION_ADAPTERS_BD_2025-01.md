# Validación y Corrección de Adapters - Coherencia con BD

**Fecha**: 2025-01 (Sprint Validación)  
**Objetivo**: Verificar que todos los adapters referencien objetos existentes en CO_Matrix_Intranet  
**Resultado**: 0 errores de compilación tras correcciones

---

## 📊 Resumen Ejecutivo

### Validación de SP
- **SP referenciados en adapters**: 226 únicos
- **SP encontrados en BD**: 1,497 totales
- **SP faltantes**: ✅ **0** (todos los SP referenciados existen)

### Validación de Tablas
- **Tablas con problemas identificadas**: 25+
- **Adapters corregidos**: 15+
- **Métodos convertidos a NotImplementedException**: ~20 (tablas inexistentes)

---

## 🔧 Correcciones Realizadas

### 1. TraficoAdapter.cs (OP)
**Archivo**: `MatrixNext.Data/Adapters/OP/TraficoAdapter.cs`

| Problema | Corrección |
|----------|------------|
| Tabla `PY_Trabajos` | → `PY_Trabajo` |
| Tabla `OP_Unidades` | → `US_Unidades` |
| Tabla `TH_Empleado` | → `TH_Personas` |
| Columna `IdMovimiento` | → `id` |
| Columna `CantidadEnviada/Recibida` | → `Cantidad` |
| Columna `IdUnidadOrigen/Destino` | → `UnidadEnvia/UnidadRecibe` |
| Tabla `OP_TraficoPersonal` | 🚫 NO EXISTE - `throw NotImplementedException` |
| Tabla `OP_UnidadesPermisos` | 🚫 NO EXISTE - simplificado |

### 2. ProductividadAdapter.cs (OP)
**Archivo**: `MatrixNext.Data/Adapters/OP/ProductividadAdapter.cs`

| Problema | Corrección |
|----------|------------|
| Tabla `CuantiPlanillas` | → `OP_CuantiPlanillas` |
| Tabla `PY_Trabajos` | → `PY_Trabajo` |
| Tabla `TH_Empleado` | → `TH_Personas` |
| Tabla `PY_TrabajosPersonal` | → `OP_PersonasAsignadasTrabajo` |
| Tabla `US_PermisosUsuario` | → `US_PermisosUsuarios` |
| Método `RechazarPlanilla` | `throw NotImplementedException` (BD no soporta) |

### 3. SupervisionAdapter.cs (OP) [Sesión anterior]
**Archivo**: `MatrixNext.Data/Adapters/OP/SupervisionAdapter.cs`

| Problema | Corrección |
|----------|------------|
| Tabla `OP_SupervisionTelefonica` | → `OP_SupervisionCampoTelefonico` |
| Todas las columnas | → Ajustadas a estructura real |

### 4. DistribucionAdapter.cs (PY) [Sesión anterior]
**Archivo**: `MatrixNext.Data/Adapters/PY/DistribucionAdapter.cs`

| Problema | Corrección |
|----------|------------|
| Tabla `PY_DistribucionEntrevistas` | 🚫 NO EXISTE |
| Tabla `PY_VariablesControl` | 🚫 NO EXISTE |
| Tabla `PY_InHomeVisit` | 🚫 NO EXISTE |
| Mayoría de métodos | `throw NotImplementedException` |

### 5. CierreTrabajoAdapter.cs (OP)
**Archivo**: `MatrixNext.Data/Adapters/OP/CierreTrabajoAdapter.cs`

| Problema | Corrección |
|----------|------------|
| Tabla `PY_Trabajos` | → `PY_Trabajo` |
| Columna `IdTrabajo` | → `id` |
| Columna `Estado` | → `JobBk_Estado` |

### 6. ESMetodologiaCampoAdapter.cs (ES)
**Archivo**: `MatrixNext.Data/Adapters/ES/ESMetodologiaCampoAdapter.cs`

| Problema | Corrección |
|----------|------------|
| Tabla `PY_Trabajos` | → `PY_Trabajo` |
| Columna `t.Nombre` | → `t.IdCli_Nombre` |
| Columna `t.Id` | → `t.id` |

### 7. AsignacionCampoAdapter.cs (RE_GT)
**Archivo**: `MatrixNext.Data/Adapters/RE_GT/AsignacionCampoAdapter.cs`

| Problema | Corrección |
|----------|------------|
| Tabla `PY_Trabajos` | → `PY_Trabajo` |
| Tabla `GD_COE` | 🚫 NO EXISTE - JOIN eliminado |
| Columna `IdTrabajo` | → `id` |
| Columna `IdCOE` | → `COE` |

### 8. CambioJBIAdapter.cs (RE_GT)
**Archivo**: `MatrixNext.Data/Adapters/RE_GT/CambioJBIAdapter.cs`

| Problema | Corrección |
|----------|------------|
| Tabla `PY_Trabajos` | → `PY_Trabajo` |
| Tabla `IQ_Fase` | → `IQ_Fases` |
| Columna `IdTrabajo` | → `id` |

### 9. SolicitudesAdapter.cs (GD)
**Archivo**: `MatrixNext.Data/Adapters/GD/SolicitudesAdapter.cs`

| Problema | Corrección |
|----------|------------|
| Tabla `TH_Empleado` | → `TH_Personas` |
| Columna `IdEmpleado` | → `id` |

### 10. CargaMasivaAdapter.cs (OP)
**Archivo**: `MatrixNext.Data/Adapters/OP/CargaMasivaAdapter.cs`

| Problema | Corrección |
|----------|------------|
| Tabla `PY_Trabajos` | → `PY_Trabajo` |
| Tabla `TH_Empleado` | → `TH_Personas` |
| Columna `IdTrabajo` | → `id` |
| Columna `IdEmpleado` | → `id` |

### 11. EncuestasAdapter.cs (OP)
**Archivo**: `MatrixNext.Data/Adapters/OP/EncuestasAdapter.cs`

| Problema | Corrección |
|----------|------------|
| Tabla `PY_Trabajos` | → `PY_Trabajo` |
| Tabla `GN_Unidades` | → `US_Unidades` |
| Columna `Descripcion` | → `Nombre` |

### 12. HomeRecoleccionDashboardAdapter.cs (OP)
**Archivo**: `MatrixNext.Data/Adapters/OP/HomeRecoleccionDashboardAdapter.cs`

| Problema | Corrección |
|----------|------------|
| Tabla `PY_Trabajos` | → `PY_Trabajo` |
| Tabla `PY_Proyectos` | → `PY_Proyecto` |
| Tabla `TH_Usuario` | → `US_Usuarios` |
| Tabla `CS_Unidad` | → `US_Unidades` |
| Múltiples columnas | → Ajustadas a estructura real |

### 13. NotificacionesOpAdapter.cs (OP)
**Archivo**: `MatrixNext.Data/Adapters/OP/NotificacionesOpAdapter.cs`

| Problema | Corrección |
|----------|------------|
| Tabla `PY_Trabajos` | → `PY_Trabajo` |
| Tabla `TH_Usuario` | → `US_Usuarios` |
| Tabla `TH_UsuarioRol` | → `US_RolesUsuarios` |
| Tabla `CS_Unidad` | → `US_Unidades` |
| Tabla `PY_Proyectos` | → `PY_Proyecto` |

### 14. PncAdapter.cs (GD)
**Archivo**: `MatrixNext.Data/Adapters/GD/PncAdapter.cs`

| Problema | Corrección |
|----------|------------|
| Tabla `PNC_ProductoNoConformes_Causas` | → `PNC_ProductoNoConformeCausas` |

### 15. CatalogosAdapter.cs (GD)
**Archivo**: `MatrixNext.Data/Adapters/GD/CatalogosAdapter.cs`

| Problema | Corrección |
|----------|------------|
| Tabla `GD_Estados` | 🚫 NO EXISTE - `throw NotImplementedException` |
| Tabla `GD_Procesos` | 🚫 NO EXISTE - `throw NotImplementedException` |
| Método `ObtenerResumenAsync` | → Solo cuenta GD_TipoSolicitud |

---

## 📋 Tablas NO EXISTENTES (con NotImplementedException)

| Tabla Referenciada | Existe Alternativa | Adapters Afectados |
|--------------------|--------------------|--------------------|
| `PY_DistribucionEntrevistas` | ❌ No | DistribucionAdapter |
| `PY_VariablesControl` | ❌ No | DistribucionAdapter |
| `PY_InHomeVisit` | ❌ No | DistribucionAdapter |
| `OP_TraficoPersonal` | ❌ No | TraficoAdapter |
| `OP_UnidadesPermisos` | ❌ No | TraficoAdapter |
| `GD_Estados` | ⚠️ GD_EstadoSolicitud | CatalogosAdapter |
| `GD_Procesos` | ❌ No | CatalogosAdapter |
| `GD_COE` | ❌ No | AsignacionCampoAdapter |

---

## 📌 Mapeo de Tablas Corregidas

| Nombre Incorrecto | Nombre Correcto | PK |
|-------------------|-----------------|-----|
| `PY_Trabajos` | `PY_Trabajo` | `id` (bigint) |
| `PY_Proyectos` | `PY_Proyecto` | `id` |
| `TH_Empleado` | `TH_Personas` | `id` |
| `TH_Usuario` | `US_Usuarios` | `Id` |
| `TH_UsuarioRol` | `US_RolesUsuarios` | - |
| `CS_Unidad` | `US_Unidades` | `id` |
| `GN_Unidades` | `US_Unidades` | `id` |
| `OP_Unidades` | `US_Unidades` | `id` |
| `CuantiPlanillas` | `OP_CuantiPlanillas` | `Id` |
| `IQ_Fase` | `IQ_Fases` | `IdFase` |
| `US_PermisosUsuario` | `US_PermisosUsuarios` | - |
| `PY_TrabajosPersonal` | `OP_PersonasAsignadasTrabajo` | `id` |
| `PNC_ProductoNoConformes_Causas` | `PNC_ProductoNoConformeCausas` | - |
| `OP_SupervisionTelefonica` | `OP_SupervisionCampoTelefonico` | `Id` |

---

## ✅ Estado de Compilación

```
Build succeeded.
    0 Errores
    ~450 Warnings (nullability warnings - no críticos)
```

---

## 📝 Recomendaciones

1. **Crear tablas faltantes** si la funcionalidad es requerida:
   - `GD_Estados` (catálogo de estados general)
   - `GD_Procesos` (catálogo de procesos)
   - `PY_DistribucionEntrevistas` (distribución de campo)

2. **Documentar** en cada adapter qué funcionalidades están deshabilitadas

3. **Testing** antes de desplegar:
   - Probar flujos principales que usan adapters corregidos
   - Verificar que `NotImplementedException` no afecte funcionalidades críticas

---

**Documento generado**: 2025-01
**Próxima revisión**: Cuando se agreguen nuevas tablas a BD
