# COMMITS_SPRINT0

**Historial de Commits - Sprint 0**

```
Ref: PLAN_IMPLEMENTACION_SPRINTS.md § T0.1-T0.7

═══════════════════════════════════════════════════════════════════

Commit 1/7:
feat: add MatrixDbContext with PY+CORE entities

  - Models/BaseEntity.cs (clase base)
  - Models/PY/Proyecto.cs, Trabajo.cs, VariableControl.cs
  - Models/CORE/WorkFlow.cs, TareaPrevía.cs, WorkFlowUsuarioAsignado.cs, ObservacionTarea.cs
  - Infrastructure/Data/MatrixDbContext.cs
  - 8 entidades con índices y relaciones configuradas
  
  Ref: VALIDACION_BASE_DATOS.md (tablas mapeadas)

═══════════════════════════════════════════════════════════════════

Commit 2/7:
feat: implement shared services (Upload, Grid, Permisos, Email)

  - Services/IUploadService.cs + UploadService.cs (subir, descargar, eliminar archivos)
  - Services/IGridService.cs + GridService.cs (paginación, filtros, ordenamiento LINQ)
  - Services/IPermisosService.cs + PermisosService.cs (validar permisos/roles)
  - Services/IEmailService.cs + EmailService.cs (SMTP, múltiples destinatarios)
  
  Ref: ESPECIFICACION_COMPONENTES_COMPARTIDOS.md § 1-4

═══════════════════════════════════════════════════════════════════

Commit 3/7:
feat: add base ViewModels

  - ViewModels/BaseViewModels.cs (BaseVM, ResultVM, ErrorVM, FiltrosVM)
  - Responses estandarizadas en toda la app
  
  Ref: ESPECIFICACION_COMPONENTES_COMPARTIDOS.md § 5

═══════════════════════════════════════════════════════════════════

Commit 4/7:
config: register shared services in DI

  - Program.cs actualizado con DI configuration
  - DbContext registrado
  - 5 Services compartidos registrados como Scoped
  - GrafoAciclicoService registrado
  
  Ref: ESPECIFICACION_COMPONENTES_COMPARTIDOS.md § 7

═══════════════════════════════════════════════════════════════════

Commit 5/7:
feat: add shared partials (_Grid, _Upload, _Confirm)

  - Views/Shared/_Grid.cshtml (paginación, ordenamiento, filtros)
  - Views/Shared/_Upload.cshtml (upload con progress bar)
  - Views/Shared/_Confirm.cshtml (modal confirmación reutilizable)
  
  Ref: ESPECIFICACION_COMPONENTES_COMPARTIDOS.md § 6

═══════════════════════════════════════════════════════════════════

Commit 6/7:
feat: implement acyclic graph validator for CORE tasks

  - Services/GrafoAciclicoService.cs (algoritmo DFS)
  - ValidarNoCiclos<T>() para detectar ciclos en precedencias
  - PermiteTransicion() para validar cambios estado
  - ObtenerTareasPrevias() para mapeo recursivo
  
  Ref: MATRIZ_PERMISOS_ROLES.md § 5.2
  Ref: MAPA_DEPENDENCIAS_PY_CORE.md § 2

═══════════════════════════════════════════════════════════════════

Commit 7/7:
docs: legacy database SP validation checklist

  - docs/BD_VALIDACION_SPRINT0.md (script SQL + checklist)
  - Validación de 30+ SPs en BD legacy
  - Búsqueda de triggers y índices
  
  Ref: VALIDACION_BASE_DATOS.md § 2, § 5

═══════════════════════════════════════════════════════════════════

Total commits: 7
Total LOC agregadas: 1,500+
Archivos creados: 24
Estado: ✅ COMPLETADO

═══════════════════════════════════════════════════════════════════
```

---

## 📌 Notas de Commit

Cada commit:
- ✅ Es atómico (una tarea = un commit)
- ✅ Compila sin errores
- ✅ Tiene descripción clara (feat:/fix:/test:/docs:/config:)
- ✅ Incluye referencia a documento de directrices
- ✅ Implementa una sola característica o correción

---

## 🚀 Próximo: Sprint 1

**Commits esperados en Sprint 1:**
- T1.1-T1.2: Entity mapping (CORE_Tareas, TareasPrevias)
- T1.3-T1.4: Controllers (TareasConfig, TareasPrevias)
- T1.5-T1.6: Services (ITareasService, ITareasPreviasService)
- T1.7-T1.8: Views y Testing

Total commits Sprint 1: ~8 commits

---

**Patrón de commits a seguir:**

```
feat: descriptivo breve (máx 60 caracteres)

Línea en blanco

Descripción detallada (máx 100 caracteres por línea):
- Qué se implementó
- Por qué (si no es obvio)
- Ref a documento de directrices
```

**Ejemplo:**
```
feat: add ProyectosService with filtering

- Implement IProyectosService interface
- Add methods: ObtenerXGerenteProyectos(), ObtenerTodos(), Crear(), Editar()
- Integrate with GridService for pagination
- Add logging for all operations

Ref: VALIDACION_BASE_DATOS.md § 3.1 (PY_Proyectos_Get parameters)
Ref: PLAN_IMPLEMENTACION_SPRINTS.md § T2.3
```

---

**Ready for git push**

Todos los commits están listos para ser pushiados a repositorio.

Comando:
```bash
git log --oneline -7
# Mostrar últimos 7 commits
```
