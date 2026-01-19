# INVENTARIO MÓDULO US_USUARIOS
## Comparación WebMatrix → MatrixNext

**Fecha de generación**: 2026-01-18  
**Objetivo**: Identificar páginas .aspx faltantes y funcionalidades excedentes en MatrixNext

---

## 📊 RESUMEN EJECUTIVO

| Métrica | Cantidad |
|---------|----------|
| **Páginas WebMatrix** | 14 |
| **Controllers MatrixNext** | 4 |
| **Páginas migradas** | 5 |
| **Páginas faltantes** | 9 |
| **Cobertura** | **36%** |

---

## 📋 TABLA DE MAPEO COMPLETA

### Páginas WebMatrix vs MatrixNext

| # | Página WebMatrix | Estado | Controller MatrixNext | Funcionalidades Implementadas |
|---|------------------|--------|----------------------|------------------------------|
| 1 | **Usuarios.aspx** | ✅ Migrada | `UsuariosController.cs` | Index, Create, Edit, Delete, Details, Search |
| 2 | **CambioContrasena.aspx** | ✅ Migrada | `UsuariosController.cs` | ChangePassword, MyChangePassword |
| 3 | **Roles.aspx** | ⚠️ Parcial | `RolesController.cs` | Index, CreateModal, EditModal, DeleteModal (solo vistas, falta POST) |
| 4 | **Permisos.aspx** | ⚠️ Parcial | `PermisosController.cs` | Solo Index (falta CRUD completo) |
| 5 | **GrupoUnidad.aspx** | ⚠️ Parcial | `GrupoUnidadController.cs` | Index, CreateModal, EditModal, DeleteModal (solo vistas, falta POST) |
| 6 | **GruposPermisos.aspx** | ❌ Faltante | N/A | — |
| 7 | **RolesPermisos.aspx** | ❌ Faltante | N/A | — |
| 8 | **RolesUsuarios.aspx** | ⚠️ Integrada | `UsuariosController.cs` | AssignRole/RemoveRole desde Details de Usuario |
| 9 | **TipoGrupoUnidad.aspx** | ❌ Faltante | N/A | — |
| 10 | **UsuariosUnidades.aspx** | ⚠️ Integrada | `UsuariosController.cs` | AssignUnidad/RemoveUnidad desde Details de Usuario |
| 11 | **Feedback.aspx** | ❌ Faltante | N/A | — |
| 12 | **SeguimientoFeedback.aspx** | ❌ Faltante | N/A | — |
| 13 | **Unidades.aspx** | ❌ Faltante | N/A | — |
| 14 | **PermisosUsuarios.aspx** | ⚠️ Integrada | `UsuariosController.cs` | AssignPermiso/RemovePermiso desde Details de Usuario |

---

## 🔍 ANÁLISIS DETALLADO POR PÁGINA

### ✅ MIGRADAS COMPLETAMENTE (2)

#### 1. Usuarios.aspx → UsuariosController.cs
**WebMatrix funcionalidades:**
- Consultar usuarios por nombre
- Consultar usuarios por unidad
- Consultar usuarios por grupo unidad
- Consultar usuarios por rol
- Crear usuario (con cifrado de contraseña)
- Editar usuario
- Verificación de permisos (permiso 88)

**MatrixNext implementación:**
- Index (listado)
- Search (búsqueda AJAX)
- Create (formulario + POST)
- Edit (formulario + POST)
- Delete (confirmación + POST)
- Details (vista detalle)
- Modales: CreateModal, EditModal, DeleteModal

**Verificación**: ✅ Paridad funcional completa

#### 2. CambioContrasena.aspx → UsuariosController.cs
**WebMatrix funcionalidades:**
- Validar contraseña antigua
- Cambiar contraseña con cifrado
- Log de ejecución

**MatrixNext implementación:**
- ChangePassword/{id} (GET + POST)
- MyChangePassword (redirecciona al usuario actual)
- Vista: ChangePassword.cshtml

**Verificación**: ✅ Paridad funcional completa

---

### ⚠️ MIGRADAS PARCIALMENTE (5)

#### 3. Roles.aspx → RolesController.cs
**WebMatrix funcionalidades:**
- Listar roles
- Crear rol (GuardarRol)
- Editar rol (EditarRol)
- Eliminar rol (EliminarRol)
- Navegar a RolesUsuarios
- Verificación de permisos (permiso 91)

**MatrixNext implementación:**
- Index (listado) ✅
- CreateModal (solo GET) ⚠️
- EditModal/{id} (solo GET) ⚠️
- DeleteModal/{id} (solo GET) ⚠️

**Faltantes:**
- [ ] POST para Create
- [ ] POST para Edit
- [ ] POST para Delete
- [ ] Navegación a RolesUsuarios (manejo de usuarios por rol)

#### 4. Permisos.aspx → PermisosController.cs
**WebMatrix funcionalidades:**
- Listar permisos por grupo (IdGrupoPermiso)
- Crear permiso (US_Permisos_Add)
- Editar permiso (US_Permisos_Edit)
- Eliminar permiso (US_Permisos_Del)
- Navegar a RolesPermisos

**MatrixNext implementación:**
- Index (listado básico) ✅

**Faltantes:**
- [ ] Filtro por IdGrupoPermiso
- [ ] CreateModal + POST
- [ ] EditModal + POST
- [ ] DeleteModal + POST
- [ ] Navegación a RolesPermisos

#### 5. GrupoUnidad.aspx → GrupoUnidadController.cs
**WebMatrix funcionalidades:**
- Listar grupos de unidad por tipo (IdTipoGrupoUnidad)
- Crear grupo (US_GrupoUnidad_Add)
- Editar grupo (US_GrupoUnidad_Edit)
- Eliminar grupo (US_GrupoUnidad_Del)
- Navegar a Unidades

**MatrixNext implementación:**
- Index ✅
- CreateModal (solo GET) ⚠️
- EditModal/{id} (solo GET) ⚠️
- DeleteModal/{id} (solo GET) ⚠️

**Faltantes:**
- [ ] Filtro por IdTipoGrupoUnidad
- [ ] POST para Create
- [ ] POST para Edit
- [ ] POST para Delete
- [ ] Navegación a Unidades

#### 6. RolesUsuarios.aspx → Integrada en UsuariosController
**WebMatrix funcionalidades:**
- Listar usuarios por rol (QueryString IdRol)
- Agregar usuario a rol (GuardarRolesUsuarios)

**MatrixNext implementación:**
- AssignRole (desde Details de Usuario) ✅
- RemoveRole (desde Details de Usuario) ✅
- GetRolesAsignados/{usuarioId} ✅
- GetRolesDisponibles/{usuarioId} ✅

**Nota:** En MatrixNext la lógica está integrada en la vista Details del Usuario, no como página separada.

**Faltante funcional:**
- [ ] Vista de "Usuarios por Rol" (listar usuarios de un rol específico)

#### 7. UsuariosUnidades.aspx → Integrada en UsuariosController
**WebMatrix funcionalidades:**
- Listar unidades del usuario (QueryString IdUsuario)
- Agregar unidad a usuario (GuardarUsuariosUnidades)
- Cascada: TipoGrupoUnidad → GrupoUnidad → Unidad

**MatrixNext implementación:**
- AssignUnidad ✅
- RemoveUnidad ✅
- GetUnidadesAsignadas/{usuarioId} ✅
- GetUnidadesDisponibles/{usuarioId} ✅

**Faltante funcional:**
- [ ] Cascada completa (TipoGrupo → Grupo → Unidad)

#### 8. PermisosUsuarios.aspx → Integrada en UsuariosController
**WebMatrix funcionalidades:**
- Listar permisos del usuario (QueryString IdUsuario)
- Agregar permiso a usuario (GuardarPermisosUsuario)
- Cascada: GrupoPermiso → Permiso

**MatrixNext implementación:**
- AssignPermiso ✅
- RemovePermiso ✅
- GetPermisosAsignados/{usuarioId} ✅
- GetPermisosDisponibles/{usuarioId} ✅

**Faltante funcional:**
- [ ] Cascada (GrupoPermiso → Permiso)

---

### ❌ NO MIGRADAS (6)

#### 9. GruposPermisos.aspx
**WebMatrix funcionalidades:**
- Listar grupos de permisos
- Crear grupo (GuardarGrupoPermiso)
- Editar grupo (EditarGrupoPermiso)
- Eliminar grupo (EliminarGrupoPermiso)
- Navegar a Permisos
- Verificación de permisos (permiso 90)

**Acción requerida:** Crear `GruposPermisosController.cs` con CRUD completo

#### 10. RolesPermisos.aspx
**WebMatrix funcionalidades:**
- Listar permisos por rol (QueryString IdPermiso)
- Agregar rol a permiso (GuardarRolesPermisos)

**Acción requerida:** Crear vistas/actions para asignar roles a permisos, o integrar en PermisosController

#### 11. TipoGrupoUnidad.aspx
**WebMatrix funcionalidades:**
- Listar tipos de grupo unidad
- Crear tipo (US_TipoGrupoUnidad_Add)
- Editar tipo (US_TipoGrupoUnidad_Edit)
- Eliminar tipo (US_TipoGrupoUnidad_Del)
- Navegar a GrupoUnidad
- Verificación de permisos (permiso 89)

**Acción requerida:** Crear `TipoGrupoUnidadController.cs` con CRUD completo

#### 12. Unidades.aspx
**WebMatrix funcionalidades:**
- Listar unidades por grupo (QueryString IdGrupoUnidad)
- Crear unidad (US_Unidades_Add)
- Editar unidad (US_Unidades_Edit)
- Eliminar unidad (US_Unidades_Del)

**Acción requerida:** Crear `UnidadesController.cs` con CRUD completo

#### 13. Feedback.aspx
**WebMatrix funcionalidades:**
- Formulario de retroalimentación
- Seleccionar tipo de asunto
- Enviar correo con feedback
- Log de ejecución

**Acción requerida:** Crear `FeedbackController.cs` con formulario de envío

#### 14. SeguimientoFeedback.aspx
**WebMatrix funcionalidades:**
- Listar feedback pendientes
- Listar feedback resueltos
- Responder feedback
- Marcar como solucionado
- Enviar respuesta por correo

**Acción requerida:** Crear `SeguimientoFeedbackController.cs` con gestión completa

---

## ⚠️ FUNCIONALIDADES EN MATRIXNEXT QUE NO EXISTEN EN WEBMATRIX

Ninguna identificada. MatrixNext mantiene paridad funcional donde está implementado.

---

## 📈 MATRIZ DE PRIORIZACIÓN

### Alta prioridad (Funcionalidad core incompleta)
| Página | Razón |
|--------|-------|
| GruposPermisos.aspx | Prerequisito para gestión de Permisos |
| TipoGrupoUnidad.aspx | Prerequisito para gestión de GrupoUnidad |
| Unidades.aspx | Prerequisito para asignación de usuarios a unidades |

### Media prioridad (Funcionalidad administrativa)
| Página | Razón |
|--------|-------|
| RolesPermisos.aspx | Gestión de seguridad |
| Completar CRUD Roles | Modales sin POST |
| Completar CRUD Permisos | Solo Index |
| Completar CRUD GrupoUnidad | Modales sin POST |

### Baja prioridad (Funcionalidad secundaria)
| Página | Razón |
|--------|-------|
| Feedback.aspx | Funcionalidad de soporte |
| SeguimientoFeedback.aspx | Funcionalidad de soporte |

---

## 🗃️ STORED PROCEDURES IDENTIFICADOS

| SP/Método WebMatrix | Usado en página |
|--------------------|-----------------|
| `US_Usuarios_Add` | Usuarios.aspx |
| `US_Usuarios_Edit` | Usuarios.aspx |
| `US_Permisos_Add` | Permisos.aspx |
| `US_Permisos_Edit` | Permisos.aspx |
| `US_Permisos_Del` | Permisos.aspx |
| `US_GrupoUnidad_Add` | GrupoUnidad.aspx |
| `US_GrupoUnidad_Edit` | GrupoUnidad.aspx |
| `US_GrupoUnidad_Del` | GrupoUnidad.aspx |
| `US_TipoGrupoUnidad_Add` | TipoGrupoUnidad.aspx |
| `US_TipoGrupoUnidad_Edit` | TipoGrupoUnidad.aspx |
| `US_TipoGrupoUnidad_Del` | TipoGrupoUnidad.aspx |
| `US_Unidades_Add` | Unidades.aspx |
| `US_Unidades_Edit` | Unidades.aspx |
| `US_Unidades_Del` | Unidades.aspx |
| `GuardarRol` | Roles.aspx |
| `EditarRol` | Roles.aspx |
| `EliminarRol` | Roles.aspx |
| `GuardarGrupoPermiso` | GruposPermisos.aspx |
| `EditarGrupoPermiso` | GruposPermisos.aspx |
| `EliminarGrupoPermiso` | GruposPermisos.aspx |
| `GuardarRolesPermisos` | RolesPermisos.aspx |
| `GuardarRolesUsuarios` | RolesUsuarios.aspx |
| `GuardarUsuariosUnidades` | UsuariosUnidades.aspx |
| `GuardarPermisosUsuario` | PermisosUsuarios.aspx |
| `EnviarFeedBack` | Feedback.aspx |
| `ObtenerFeedbackPendientes` | SeguimientoFeedback.aspx |
| `ObtenerFeedbackResueltos` | SeguimientoFeedback.aspx |
| `ActualizarFeedback` | SeguimientoFeedback.aspx |

---

## 📝 CHECKLIST DE MIGRACIÓN PENDIENTE

### Controllers a crear:
- [ ] `TipoGrupoUnidadController.cs`
- [ ] `UnidadesController.cs`
- [ ] `GruposPermisosController.cs`
- [ ] `FeedbackController.cs`
- [ ] `SeguimientoFeedbackController.cs`

### Controllers a completar:
- [ ] `RolesController.cs` - Agregar POST Create/Edit/Delete
- [ ] `PermisosController.cs` - Agregar CRUD completo + filtro por grupo
- [ ] `GrupoUnidadController.cs` - Agregar POST Create/Edit/Delete + filtro por tipo

### Vistas a crear:
- [ ] TipoGrupoUnidad: Index, _CreateModal, _EditModal, _DeleteModal
- [ ] Unidades: Index, _CreateModal, _EditModal, _DeleteModal
- [ ] GruposPermisos: Index, _CreateModal, _EditModal, _DeleteModal
- [ ] Feedback: Create
- [ ] SeguimientoFeedback: Index, _ResponderModal

---

## 🔄 ESTIMACIÓN DE ESFUERZO

| Componente | Esfuerzo | Horas estimadas |
|------------|----------|-----------------|
| TipoGrupoUnidadController (CRUD) | Medio | 4h |
| UnidadesController (CRUD) | Medio | 4h |
| GruposPermisosController (CRUD) | Medio | 4h |
| Completar RolesController | Bajo | 2h |
| Completar PermisosController | Medio | 3h |
| Completar GrupoUnidadController | Bajo | 2h |
| FeedbackController | Medio | 3h |
| SeguimientoFeedbackController | Alto | 5h |
| **TOTAL** | | **27h** |

---

**Documento generado automáticamente**  
**Última actualización**: 2026-01-18
