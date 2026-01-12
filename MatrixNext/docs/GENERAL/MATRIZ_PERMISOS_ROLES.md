# MATRIZ_PERMISOS_ROLES

**Fase 3: Matriz de Permisos y Roles** - [Authorize] & Seguridad

Documento generado: 6 enero 2026
Estatus: 🔄 EN CONSTRUCCIÓN

---

## 📊 Resumen Ejecutivo

Mapeo de:
- **Permisos legacy** (IDs numéricos 24, 38, 97, etc.) → nuevos roles ASP.NET Core
- **Flujos protegidos** (qué [Authorize(Roles="...")] requerida en cada controller)
- **Validaciones en tiempo ejecución** (VerificarPermisoUsuario)
- **Auditoría** (qué usuarios modificaron qué, cuándo)

---

## 1️⃣ PERMISOS LEGACY IDENTIFICADOS

### De análisis VALIDACION_EVIDENCIAS_PY_CORE.md:

| PermissionID | Módulo | Descripción | Controllers/WebForms | Línea |
| --- | --- | --- | --- | --- |
| **24** | PY_Proyectos | Home (dashboard) | Home.aspx | Pre-Init |
| **38** | PY_Proyectos | Listar proyectos | PY_Proyectos.aspx, Page_Load | 18 |
| **97** | PY_Proyectos | Crear/editar trabajos | Trabajos.aspx, Page_Load | 245 |
| **?** | CORE | Gestionar tareas | Gestion-Tareas.aspx | ⚠️ POR CONFIRMAR |
| **?** | OP | Configurar muestras | [OP controllers] | ⚠️ POR CONFIRMAR |
| **?** | CU | [Acceso Brief/Estudio] | [CU controllers] | ⚠️ POR CONFIRMAR |

---

## 2️⃣ ROLES INFERIDOS DE CÓDIGO

### De Trabajos.aspx.vb (línea ~289 Guardar()):

```vb
' Enum ERolResponsable
Enum ERolResponsable
  GerenteProyectos = 6      ' [Authorize(Roles="GerenteProyectos")]
  ' ... otros roles sin documentar
End Enum

' En Guardar(), se valida:
If usuario.Rol = ERolResponsable.GerenteProyectos Then
  ' Puede crear trabajo nuevo
  Trabajo.GuardarTrabajo()
Else
  ' No autorizado
End If
```

### De TrabajosCualitativos.aspx.vb (línea ~338 Page_Load()):

```vb
' CargarModeradoreslist() llama:
CampoCualitativo.ObtenerModeradores()

' ⚠️ PREGUNTA: ¿Qué rol es "Moderador"?
' ¿ID? ¿Pertenece a US_Usuarios.RolesPermisos?
```

---

## 3️⃣ TABLA: ROLES VS MÓDULOS VS ACCIONES

### 3.1 PY_Proyectos Module

| Rol | Acción | Permiso | Validación | ¿[Authorize]? | Notas |
| --- | --- | --- | --- | --- | --- |
| **Gerente Proyectos** (6) | Ver lista | 38 | VerificarPermisoUsuario(38) | [Authorize(Roles="GerenteProyectos")] | Listar propios proyectos |
| | Crear proyecto | 38 | Rol=6 + acción=INSERT | [Authorize(Roles="GerenteProyectos")] | Asigna a sí mismo |
| | Editar proyecto | 38 | Rol=6 + esOwner | [Authorize(Roles="GerenteProyectos")] | Solo propios |
| | Ver trabajos | 97 | Combo 38+97 | [Authorize(Roles="GerenteProyectos,Coordinador")] | Filter por usuario |
| | Crear trabajo | 97 | Rol=6 | [Authorize(Roles="GerenteProyectos")] | Lanza tareas CORE |
| | Duplicar trabajo | ? | Rol=6 | [Authorize(Roles="GerenteProyectos")] | ⚠️ Clona CORE tasks |
| **Coordinador** (?) | Ver trabajos asignados | 97 | Rol=? + IdTrabajo IN (...) | [Authorize(Roles="Coordinador")] | Solo asignados a usuario |
| | Cambiar estado trabajo | 97 | Rol=? | [Authorize(Roles="Coordinador")] | Marca como enviado, completado |
| | Registrar muestra | ? | Rol=? | [Authorize(Roles="Coordinador")] | En campo |
| **Moderador Cuali** (?) | Ver trabajo cuali | ? | Rol=? | [Authorize(Roles="Moderador")] | Moderación de datos |
| | Registrar sesión | ? | Rol=? | [Authorize(Roles="Moderador")] | InHomeVisit.aspx |

### 3.2 CORE Module

| Rol | Acción | Permiso | Validación | ¿[Authorize]? | Notas |
| --- | --- | --- | --- | --- | --- |
| **Coordinador** | Ver tareas asignadas | ? | IdUsuario IN CORE_WorkFlow_UsuariosAsignados | [Authorize(Roles="Coordinador")] | Del workflow |
| | Cambiar estado tarea | ? | Validar precedencias + permisos | [Authorize(Roles="Coordinador")] | GrafoAciclico.PermiteTransicion |
| | Registrar observación | ? | Rol=? | [Authorize(Roles="Coordinador")] | Auditoría |
| **Gerente** | Ver tráfico | ? | Agregación por proyecto | [Authorize(Roles="GerenteProyectos")] | Dashboard |
| | Forzar transición tarea | ? | Rol admin | [Authorize(Roles="Administrador")] | ⚠️ Requiere admin |

### 3.3 OP Module

| Rol | Acción | Permiso | Validación | ¿[Authorize]? | Notas |
| --- | --- | --- | --- | --- | --- |
| **OP Manager** | Configurar metodologías | ? | Rol=? | [Authorize(Roles="OPManager")] | Catálogo |
| | Crear muestra | ? | Rol=? | [Authorize(Roles="OPManager,Coordinador")] | Desde Trabajos.aspx |
| | Estimar tráfico | ? | Rol=? | [Authorize(Roles="OPManager")] | Automático en Guardar |

---

## 4️⃣ ACCIONES POR CONTROLLER (MatrixNext)

### 4.1 ProyectosController

```csharp
[Authorize(Roles = "GerenteProyectos")]
public class ProyectosController : Controller
{
  [HttpGet]
  [Authorize(Roles = "GerenteProyectos")]
  public async Task<IActionResult> Index()
  {
    // GET /Proyectos
    // Listar propios proyectos (WHERE GerenteProyectos = User.Id)
    // Validar permiso 38
  }

  [HttpGet]
  [Authorize(Roles = "GerenteProyectos")]
  public async Task<IActionResult> Create()
  {
    // GET /Proyectos/Create
    // Formulario nuevo proyecto
  }

  [HttpPost]
  [Authorize(Roles = "GerenteProyectos")]
  public async Task<IActionResult> Create(ProyectoVM vm)
  {
    // POST /Proyectos
    // Guardar nuevo proyecto
    // Llamar: SP PY_Proyectos_Edit o PY_Proyecto_Add
  }

  [HttpPost]
  [Authorize(Roles = "GerenteProyectos")]
  public async Task<IActionResult> Reasignar(long proyectoId, long nuevoGerente)
  {
    // POST /Proyectos/Reasignar/{id}
    // SP: PY_Proyectos_EditGerentePY
    // Validar: usuario es admin o es gerente actual
  }
}
```

### 4.2 TrabajosController

```csharp
[Authorize(Roles = "GerenteProyectos,Coordinador")]
public class TrabajosController : Controller
{
  [HttpGet]
  [Authorize(Roles = "GerenteProyectos,Coordinador")]
  public async Task<IActionResult> Index(long proyectoId)
  {
    // GET /Proyectos/{proyectoId}/Trabajos
    // Listar trabajos del proyecto
    // Validar permiso 97
    // Filter: Si Coordinador → solo asignados al usuario
  }

  [HttpPost]
  [Authorize(Roles = "GerenteProyectos")]
  public async Task<IActionResult> Create(TrabajoVM vm)
  {
    // POST /Proyectos/{proyectoId}/Trabajos
    // Guardar trabajo (transactional con CORE)
    // Llamar: Trabajo.GuardarTrabajo() + WorkFlow.CrearHiloCrearTareas()
    // Validar: Usuario es gerente proyecto
  }

  [HttpPost]
  [Authorize(Roles = "GerenteProyectos")]
  public async Task<IActionResult> Duplicar(long trabajoId)
  {
    // POST /Proyectos/{proyectoId}/Trabajos/{trabajoId}/Duplicar
    // SP: Py_TrabajoDuplicar
    // Validar: Transactionality (clona CORE tasks?)
  }

  [HttpPost]
  [Authorize(Roles = "GerenteProyectos,Coordinador")]
  public async Task<IActionResult> CambiarEstado(long trabajoId, string nuevoEstado)
  {
    // POST /Proyectos/{proyectoId}/Trabajos/{trabajoId}/CambiarEstado
    // Validar precedencias (GrafoAciclico.PermiteTransicion)
    // Audit log
  }
}
```

### 4.3 TareasPreviasController (CORE)

```csharp
[Authorize(Roles = "Administrador")]
public class TareasPreviasController : Controller
{
  [HttpGet]
  [Authorize(Roles = "Administrador")]
  public async Task<IActionResult> Index()
  {
    // GET /Configuracion/TareasPrevias
    // Listar precedencias del workflow
  }

  [HttpPost]
  [Authorize(Roles = "Administrador")]
  public async Task<IActionResult> Create(TareaPreviaVM vm)
  {
    // POST /Configuracion/TareasPrevias
    // Insertar precedencia
    // VALIDAR: GrafoAciclico.ValidarNoCiclos() ANTES de guardar
    // Si retorna ciclo → HttpBadRequest("Precedencia crearía ciclo")
  }

  [HttpDelete]
  [Authorize(Roles = "Administrador")]
  public async Task<IActionResult> Delete(long id)
  {
    // DELETE /Configuracion/TareasPrevias/{id}
    // Eliminar precedencia (si no hay tareas activas usando)
  }
}
```

### 4.4 GestionTareasController (CORE)

```csharp
[Authorize(Roles = "Coordinador,Administrador")]
public class GestionTareasController : Controller
{
  [HttpGet]
  [Authorize(Roles = "Coordinador,Administrador")]
  public async Task<IActionResult> MisTrabajos()
  {
    // GET /Coordinacion/MisTrabajos
    // Listar tareas asignadas al usuario actual
    // WHERE CORE_WorkFlow_UsuariosAsignados.IdUsuario = User.Id AND estado != Completado
    // Filter por estado
  }

  [HttpPost]
  [Authorize(Roles = "Coordinador")]
  public async Task<IActionResult> CambiarEstado(long tareaId, string nuevoEstado)
  {
    // POST /Coordinacion/Tareas/{tareaId}/CambiarEstado
    // 1. Validar precedencias:
    //    tareasPrevias = await _sp.CORE_WorkFlow_TareasPrevias_Get(tareaId)
    //    si alguna previa está pendiente → Conflict("Tiene tareas previas pendientes")
    // 2. Validar permisos:
    //    si usuario no está en CORE_WorkFlow_UsuariosAsignados → Unauthorized
    // 3. Cambiar estado:
    //    UPDATE CORE_WorkFlow SET Estado = @nuevoEstado WHERE Id = @tareaId
    // 4. Audit log:
    //    INSERT CORE_ObservacionesTareas (IdTarea, IdUsuario, Observación, FechaHora)
    // 5. ¿TRIGGER EN BD ACTUALIZA ESTADO TRABAJO PY?
    //    ⚠️ Si sí → validar transactionality
  }

  [HttpPost]
  [Authorize(Roles = "Coordinador,Administrador")]
  public async Task<IActionResult> AgregarObservacion(long tareaId, string observacion)
  {
    // POST /Coordinacion/Tareas/{tareaId}/Observaciones
    // INSERT CORE_ObservacionesTareas (para auditoría)
  }

  [HttpDelete]
  [Authorize(Roles = "Administrador")]
  public async Task<IActionResult> Anular(long tareaId, string razon)
  {
    // DELETE /Coordinacion/Tareas/{tareaId}
    // Anular tarea (solo admin, requerido motivo)
    // Validar: Tarea no tiene subtareas completadas
  }
}
```

---

## 5️⃣ VALIDACIONES EN TIEMPO EJECUCIÓN

### 5.1 VerificarPermisoUsuario (Migrar de legacy)

**Ubicación Legacy:** `Datos.ClsPermisosUsuarios.VerificarPermisoUsuario(idPermiso, idUsuario)`

**Implementación MatrixNext:**

```csharp
public class PermisosService
{
  private readonly IDataAdapter _dataAdapter;

  public async Task<bool> VerificarPermisoAsync(int permisoId, long usuarioId)
  {
    // SELECT COUNT(*) FROM US_Usuarios_Permisos
    // WHERE IdUsuario = @usuarioId AND IdPermiso = @permisoId

    var result = await _dataAdapter.ExecuteScalarAsync(
      "SELECT COUNT(*) FROM US_Usuarios_Permisos WHERE IdUsuario = @usuarioId AND IdPermiso = @permisoId",
      new { usuarioId, permisoId }
    );

    return Convert.ToInt32(result) > 0;
  }

  public async Task<bool> VerificarRolAsync(long usuarioId, string rolNombre)
  {
    // SELECT COUNT(*) FROM US_Usuarios_Roles WHERE IdUsuario = @usuarioId AND NombreRol = @rolNombre
    var result = await _dataAdapter.ExecuteScalarAsync(
      "SELECT COUNT(*) FROM US_Usuarios_Roles WHERE IdUsuario = @usuarioId AND NombreRol = @rolNombre",
      new { usuarioId, rolNombre }
    );

    return Convert.ToInt32(result) > 0;
  }
}
```

**Middleware personalizado (en Program.cs):**

```csharp
app.Use(async (context, next) =>
{
  // Si usuario no tiene [Authorize], proceder
  var endpoint = context.GetEndpoint();
  var hasAuthorize = endpoint?.Metadata.GetMetadata<AuthorizeAttribute>() != null;
  
  if (!hasAuthorize)
  {
    await next();
    return;
  }

  // Obtener permiso requerido de [AuthorizePermiso(38)]
  var permAttribute = endpoint?.Metadata.GetMetadata<AuthorizePermisoAttribute>();
  if (permAttribute != null)
  {
    var permisosService = context.RequestServices.GetRequiredService<PermisosService>();
    var usuario = context.User;
    var usuarioId = long.Parse(usuario.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? "0");

    var tienePermiso = await permisosService.VerificarPermisoAsync(permAttribute.PermisoId, usuarioId);
    if (!tienePermiso)
    {
      context.Response.StatusCode = StatusCodes.Status403Forbidden;
      return;
    }
  }

  await next();
});
```

### 5.2 GrafoAciclico.ValidarNoCiclos (CORE)

**Ubicación Legacy:** No existe; se valida en base de datos.

**Implementación MatrixNext:**

```csharp
public class GrafoAciclicoService
{
  public bool ValidarNoCiclos(List<TareaPreviaCVM> tareasPrevias)
  {
    // Algoritmo DFS (Depth-First Search) para detectar ciclos

    var grafo = new Dictionary<long, List<long>>();

    // Construir grafo
    foreach (var tarea in tareasPrevias)
    {
      if (!grafo.ContainsKey(tarea.IdTarea))
        grafo[tarea.IdTarea] = new List<long>();

      if (tarea.IdTareaPreviaRequerida.HasValue)
        grafo[tarea.IdTarea].Add(tarea.IdTareaPreviaRequerida.Value);
    }

    // Detectar ciclos
    var visitados = new HashSet<long>();
    var recursionStack = new HashSet<long>();

    foreach (var nodo in grafo.Keys)
    {
      if (DetectarCiclo(nodo, grafo, visitados, recursionStack))
        return false; // Ciclo encontrado
    }

    return true; // Sin ciclos
  }

  private bool DetectarCiclo(long nodo, Dictionary<long, List<long>> grafo, HashSet<long> visitados, HashSet<long> recursionStack)
  {
    visitados.Add(nodo);
    recursionStack.Add(nodo);

    if (grafo.ContainsKey(nodo))
    {
      foreach (var vecino in grafo[nodo])
      {
        if (!visitados.Contains(vecino))
        {
          if (DetectarCiclo(vecino, grafo, visitados, recursionStack))
            return true;
        }
        else if (recursionStack.Contains(vecino))
        {
          return true; // Ciclo detectado
        }
      }
    }

    recursionStack.Remove(nodo);
    return false;
  }

  public bool PermiteTransicion(long tareaId, List<TareaPreviaCVM> tareasPrevias)
  {
    // Verificar que todas las tareas previas están completadas
    foreach (var tarea in tareasPrevias.Where(t => t.IdTarea == tareaId))
    {
      if (tarea.IdTareaPreviaRequerida.HasValue)
      {
        // SELECT Estado FROM CORE_WorkFlow WHERE Id = @IdTareaPreviaRequerida
        // Si Estado != "Completado" → return false
      }
    }

    return true;
  }
}
```

### 5.3 EsOwner (validación en runtime)

```csharp
// En ProyectosController.Edit()
public async Task<IActionResult> Edit(long id, ProyectoVM vm)
{
  var proyecto = await _dataAdapter.GetAsync<Proyecto>(
    "SELECT * FROM PY_Proyectos WHERE Id = @id",
    new { id }
  );

  // Validar que usuario es gerente asignado
  if (proyecto.IdGerenteProyectos != User.GetUserId())
  {
    return Forbid("Solo el gerente asignado puede editar este proyecto");
  }

  // Proceder con UPDATE
  return Ok();
}
```

---

## 6️⃣ AUDITORÍA Y LOGS

### 6.1 Tabla de Auditoría CORE_ObservacionesTareas

```sql
CREATE TABLE CORE_ObservacionesTareas
(
  Id BIGINT PRIMARY KEY IDENTITY,
  IdTarea BIGINT NOT NULL FOREIGN KEY REFERENCES CORE_WorkFlow(Id),
  IdUsuario BIGINT NOT NULL FOREIGN KEY REFERENCES US_Usuarios(Id),
  Observación NVARCHAR(MAX),
  FechaHora DATETIME DEFAULT GETDATE(),
  TipoOperacion NVARCHAR(50) -- 'CambioEstado', 'Anulacion', 'ComentarioGeneral'
);
```

### 6.2 Quién cambió qué, cuándo

```csharp
public class AuditoriaService
{
  public async Task LogearCambioAsync(long tareaId, long usuarioId, string tipoOperacion, string detalles)
  {
    await _dataAdapter.ExecuteAsync(
      @"INSERT INTO CORE_ObservacionesTareas (IdTarea, IdUsuario, Observación, TipoOperacion, FechaHora)
        VALUES (@IdTarea, @IdUsuario, @Observación, @TipoOperacion, GETDATE())",
      new { IdTarea = tareaId, IdUsuario = usuarioId, Observación = detalles, TipoOperacion = tipoOperacion }
    );
  }
}
```

---

## 7️⃣ MATRIZ: PERMISO LEGACY ↔ NUEVO ROL

| PermissionID | Descripción Legacy | Rol ASP.NET Core | [Authorize(Roles="...")] | Controllers |
| --- | --- | --- | --- | --- |
| 24 | Home Dashboard | GerenteProyectos | [Authorize(Roles="GerenteProyectos")] | HomeController |
| 38 | Listar Proyectos | GerenteProyectos | [Authorize(Roles="GerenteProyectos")] | ProyectosController.Index |
| 97 | Crear/Editar Trabajos | GerenteProyectos,Coordinador | [Authorize(Roles="GerenteProyectos,Coordinador")] | TrabajosController |
| ? | Gestionar Tareas CORE | Coordinador | [Authorize(Roles="Coordinador")] | GestionTareasController |
| ? | Config Metodologías | OPManager | [Authorize(Roles="OPManager")] | MetodologiasController |
| ? | Editar Brief/Estudio | CuentaManager | [Authorize(Roles="CuentaManager")] | [CU Controllers] |
| ? | Moderar Cuali | Moderador | [Authorize(Roles="Moderador")] | SesionesController |

---

## 8️⃣ RIESGOS Y MITIGACIONES

### Riesgo 1: Desincronia Permisos entre Legacy y MatrixNext 🟠

**Escenario:** Usuario tiene permiso 38 en BD legacy pero no existe mapping en rol MatrixNext

**Mitigación:**
- [ ] Crear tabla de mapping: `PermisosLegacy_vs_RolesMN`
- [ ] En cada login, sincronizar permisos legacy → claims ASP.NET Core
- [ ] Validar en middleware (PermisosService)

### Riesgo 2: Cambio estado CORE modifica PY sin validación 🟠

**Escenario:** Trigger en BD cambia PY_Trabajo.Estado cuando CORE_WorkFlow.Estado cambia; no hay auditoría

**Mitigación:**
- [ ] Confirmar si trigger existe en legacy
- [ ] Si existe, replicar lógica en GestionTareasController
- [ ] Validar transactionality (usar TransactionScope)

### Riesgo 3: Administrador puede anular tareas en cadena 🔴

**Escenario:** Anular tarea_X que es prereq de tarea_Y; tareas posteriores quedan huérfanas

**Mitigación:**
- [ ] En AnularTarea(), validar que no hay tareas posteriores bloqueadas
- [ ] Si existen, impedir o anular en cascada (con confirmación)

---

## 9️⃣ PRÓXIMAS ACCIONES

- [ ] CONFIRMAR permiso IDs en tabla US_Permisos (24, 38, 97, ?)
- [ ] VALIDAR roles en tabla US_Usuarios_Roles (GerenteProyectos, Coordinador, Moderador, etc.)
- [ ] DISEÑAR middleware de sincronización permisos legacy → ASP.NET Core
- [ ] IMPLEMENTAR [AuthorizePermiso(38)] custom attribute
- [ ] MAPEAR exactamente todas las acciones → valores permiso

---

**Fase 3 completada.** Listo para Fase 4: Especificación Técnica de Componentes Compartidos.
