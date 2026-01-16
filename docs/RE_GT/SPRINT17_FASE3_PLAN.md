# SPRINT 17 - FASE 3: CONSOLIDACIÓN Y TESTING

> **Estado Entrada**: Fase 2 completada (1,819 LOC, 0 errores, TraficoTareas 100%)
> **Objetivo**: Validar funcionalidad, iniciar migraciones pendientes, preparar próximos sprints
> **Duración Estimada**: 8-12 horas distribuidas en 3 subfases
> **Fecha Inicio**: 2026-01-15

---

## 📊 FASE 3 - DESGLOSE DE ACTIVIDADES

### SUBFASE 3.1: TESTING TRAFICO TAREAS (3-4h)
**Objetivo**: Validar que TraficoTareas funciona correctamente en navegador y bajo carga

#### TASK 3.1.1: Testing UI Básico (1h)
- [ ] Navegar a `/CORE/WorkFlow/TraficoTareas`
- [ ] Verificar carga de página sin errores
- [ ] Validar tabla se renderiza con datos
- [ ] Probar filtros (Unidad, Estado, Prioridad, Búsqueda)
- [ ] Probar paginación (anterior/siguiente)
- [ ] Clickear modal de detalles
- [ ] Validar indicadores de urgencia/vencimiento

**Evidencia esperada**:
- Screenshot de UI funcionando
- Log de no errores en consola
- Datos correspondientes a usuario logueado
- Modal se abre sin errores JS

#### TASK 3.1.2: Testing de Permisos (1h)
- [ ] Verificar que usuario solo ve tareas de su unidad
- [ ] Intentar acceso directo a unidades restringidas (debe fallar)
- [ ] Probar con diferentes roles (Supervisor, Operario, Admin)
- [ ] Validar acceso a Export solo en unidades permitidas

**Evidencia esperada**:
- Log de acceso denegado cuando corresponde
- Datos filtrados correctamente por unit
- Export disponible/deshabilitado según permisos

#### TASK 3.1.3: Testing de Rendimiento (1h)
- [ ] Medir tiempos de carga (target: <2s primera pantalla)
- [ ] Probar con 100+ registros en tabla
- [ ] Validar paginación con volumen
- [ ] Monitorear queries a BD (target: <5 queries por acción)

**Evidencia esperada**:
- Tiempos de respuesta < 2s
- Paginación fluida
- Sin N+1 queries

#### TASK 3.1.4: Testing de Errores (1h)
- [ ] BD offline: Error graceful sin stack trace
- [ ] Datos inválidos: Validación en cliente y servidor
- [ ] Session expirada: Redirect a login
- [ ] Export con archivo corrupto: Recuperación elegante

**Evidencia esperada**:
- Mensajes amigables (no stack traces)
- Logging de errores (pero no expuesto al usuario)
- Rollback limpio en caso de fallos

---

### SUBFASE 3.2: ANÁLISIS PRÓXIMAS PÁGINAS (2-3h)
**Objetivo**: Identificar siguientes páginas a migrar con estimaciones exactas

#### TASK 3.2.1: Analizar RecoleccionDatos.aspx (1.5h)
- [ ] Abrir `WebMatrix/RE_GT_Recoleccion/RecoleccionDatos.aspx.vb` (verificar ruta exacta)
- [ ] Mapear lógica de negocio (líneas clave, eventos)
- [ ] Identificar tablas/SP usadas
- [ ] Contar unidades de funcionalidad
- [ ] Estimar LOC a migrar
- [ ] Documentar en ANALISIS_RECOLECCINDATOS.md

**Entregable**: ANALISIS_RECOLECCINDATOS.md (similar a ANALISIS_TRAFICO_TAREAS.md)

#### TASK 3.2.2: Analizar GestionTratamiento.aspx (1.5h)
- [ ] Abrir `WebMatrix/RE_GT_GestionTratamiento/GestionTratamiento.aspx.vb`
- [ ] Mapear lógica similar a RecoleccionDatos
- [ ] Identificar dependencias internas (¿llama a RecoleccionDatos?)
- [ ] Estimar LOC
- [ ] Documentar en ANALISIS_GESTIONTRATAMIENTO.md

**Entregable**: ANALISIS_GESTIONTRATAMIENTO.md

---

### SUBFASE 3.3: INICIAR RECOLECCINDATOS (3-5h)
**Objetivo**: Comenzar migración de RecoleccionDatos siguiendo patrón TraficoTareas

#### TASK 3.3.1: DTO + ViewModel (1.5h)
- [ ] Crear clases DTOs necesarias
- [ ] Crear ViewModel(s)
- [ ] Ubicar en MatrixNext.Core/DTOs/RE_GT/
- [ ] Validar contra CoreProject

#### TASK 3.3.2: Service + Adapter (1.5h)
- [ ] Crear IRecoleccionDataosService en MatrixNext.Core/Services/RE_GT/
- [ ] Crear RecoleccionDatosAdapter en MatrixNext.Web/Services/RE_GT/
- [ ] Implementar métodos principales (GET, CREATE, UPDATE, DELETE)
- [ ] Registrar en DI (Program.cs)

#### TASK 3.3.3: Controller (1h)
- [ ] Crear RecoleccionDatosController en Areas/RE_GT/Controllers/
- [ ] Implementar 3-5 acciones principales
- [ ] Aplicar [Authorize] y validaciones

#### TASK 3.3.4: Vista (1h)
- [ ] Crear RecoleccionDatos.cshtml en Areas/RE_GT/Views/
- [ ] Bootstrap 5 + AJAX modal pattern
- [ ] Integrar con TraficoTareas si existe dependencia

---

## 📈 MÉTRICA DE ÉXITO - FASE 3

| Métrica | Target | Status |
|---------|--------|--------|
| TraficoTareas Testing | 100% funcional ✓ | ⏳ Pendiente |
| Errores descubiertos | 0 (o documentados) | ⏳ Pendiente |
| Próximas análisis | 2 documentos | ⏳ Pendiente |
| RecoleccionDatos avance | 40%+ (DTOs+Service) | ⏳ Pendiente |
| Build status | 0 errores | ⏳ Pendiente |
| Documentación | Actualizada | ⏳ Pendiente |

---

## 📋 CHECKLIST PRE-TASK

- [ ] `dotnet build` sin errores
- [ ] Git en rama `feature/sprint17-fase3` (o `develop`)
- [ ] BD de testing accesible
- [ ] VS Code abierto en carpeta MatrixNext
- [ ] Copilot instructions actualizadas

---

## 🔗 REFERENCIAS

- [MIGRACION_RE_GT_COMPLETADA.md](./MIGRACION_RE_GT_COMPLETADA.md) - Estado Fase 2
- [ANALISIS_TRAFICO_TAREAS.md](./ANALISIS_TRAFICO_TAREAS.md) - Patrón a seguir
- [DIRECTRICES_MIGRACION.md](../DIRECTRICES_MIGRACION.md) - Reglas obligatorias
- WebMatrix paths:
  - `CoreProject/RE_GT/` - DataLayer legacy
  - `WebMatrix/RE_GT_*/` - Páginas a migrar

---

**Creado**: 2026-01-15
**Autor**: GitHub Copilot (Sprint 17 Automatización)
**Estado**: 📋 Planificación completada → 🚀 Listos para iniciar TASK 3.1.1
