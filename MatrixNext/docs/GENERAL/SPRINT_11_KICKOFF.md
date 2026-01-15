# SPRINT 11 - OP_RO + OP_TRAFICO - KICKOFF

**Fecha**: 2026-01-15  
**Status**: 🟡 PRÓXIMO SPRINT  
**Duración estimada**: 2 semanas  
**Prioridad**: Alta

---

## 🎯 OBJETIVO

Completar la integración de los módulos **OP_RO (Revisión y Observaciones)** y **OP_Trafico (Tráfico de Trabajos)** con paridad WebMatrix legacy.

---

## 📊 ESTADO ACTUAL

### Descubrimiento: Infrastructure Ready
| Componente | Status | Ubicación |
|-----------|--------|-----------|
| OP_ROController | ✅ Existe | `MatrixNext.Web/Areas/OP/Controllers/OP_ROController.cs` |
| OP_TraficoController | ✅ Existe | `MatrixNext.Web/Areas/OP/Controllers/OP_TraficoController.cs` |
| Vistas OP_RO | ✅ Existen | `MatrixNext.Web/Areas/OP/Views/OP_RO/` |
| Vistas OP_Trafico | ✅ Existen | `MatrixNext.Web/Areas/OP/Views/OP_Trafico/` |
| Build Status | ✅ 0 Errores | Compilación limpia |

---

## 🔍 PRE-SPRINT: Analysis Required

### Tarea 1: Revisar Controllers existentes
- [ ] Leer `OP_ROController.cs` (endpoints, métodos)
- [ ] Leer `OP_TraficoController.cs` (endpoints, métodos)
- [ ] Verificar si usan IService/IAdapter pattern
- [ ] Documentar en `SPRINT_11_ESTADO_ACTUAL.md`

### Tarea 2: Buscar equivalentes en WebMatrix
- [ ] Buscar `OP_RO` en WebMatrix folder
- [ ] Buscar `Trafico` en WebMatrix folder
- [ ] Identificar aspx pages correspondientes
- [ ] Mapear acciones (listar, crear, editar, eliminar)

### Tarea 3: Verificar Services y Adapters
- [ ] ¿Existen IOP_ROService e IOP_ROAdapter?
- [ ] ¿Existen IOP_TraficoService e IOP_TraficoAdapter?
- [ ] Si no existen → crear
- [ ] Documentar dependencias

### Tarea 4: Identificar Stored Procedures
- [ ] Buscar SPs en `matrixNext/docs/SQL/` para OP_RO
- [ ] Buscar SPs en `matrixNext/docs/SQL/` para OP_Trafico
- [ ] Crear mapeo: SP → Adapter method
- [ ] Crear mapeo: Acción → SP

### Tarea 5: Scope Definition (Regla 6)
- [ ] ¿Todas las acciones existen en WebMatrix?
- [ ] ¿Exportar a Excel/PDF?
- [ ] ¿Auditoría?
- [ ] ¿Notificaciones?
- [ ] Documentar decisiones en `SPRINT_11_SCOPE_VERIFICATION.md`

---

## 📋 TAREAS PRINCIPALES (Estimadas)

### Sprint 11 Timeline
1. **Analysis & Planning** (1-2 días / 8h)
   - Revisar controllers, vistas, WebMatrix
   - Crear state documents
   - Definir scope

2. **OP_RO Implementation** (3-5 días / 24h)
   - Crear/completar IOP_ROService
   - Crear/completar OP_ROAdapter
   - Endpoints REST
   - Vistas Razor (CRUD)
   - Testing básico

3. **OP_Trafico Implementation** (3-5 días / 24h)
   - Crear/completar IOP_TraficoService
   - Crear/completar OP_TraficoAdapter
   - Endpoints REST
   - Vistas Razor (CRUD)
   - Testing básico

4. **Integration & QA** (1-2 días / 16h)
   - Build verification (0 errors target)
   - Manual testing
   - Documentation

**Total**: 72-80 horas

---

## 🔗 DEPENDENCIAS

- IAuthorizationService (ya existe - usar para validar permisos)
- ILogger<T> (inyección estándar)
- MatrixDbContext (EF Core context)
- Dapper (para SP execution)
- ApiResponse<T> pattern (estándar del proyecto)

---

## 📚 DOCUMENTACIÓN REQUERIDA

**Pre-Sprint**:
- [ ] `SPRINT_11_ESTADO_ACTUAL.md` - Análisis inicial

**Durante Sprint**:
- [ ] `SPRINT_11_KICKOFF.md` - Este documento (guía de trabajo)
- [ ] `SPRINT_11_OP_RO_ANALISIS.md` - Detalles OP_RO
- [ ] `SPRINT_11_OP_TRAFICO_ANALISIS.md` - Detalles OP_Trafico

**Post-Sprint**:
- [ ] `SPRINT_11_COMPLETADO.md` - Resumen técnico

---

## ✅ DONE CRITERIA

**Por cada módulo (OP_RO, OP_Trafico)**:
- [ ] Todos los métodos públicos implementados (sin NotImplementedExceptions)
- [ ] Todos los endpoints funcionan (manual testing)
- [ ] Build: 0 Errors, 0 Warnings
- [ ] Logging detallado en todas las operaciones
- [ ] Manejo de errores sin exponer stack traces
- [ ] Documentación de SP mapeados
- [ ] Vistas CRUD funcionando (modales AJAX)
- [ ] Paginación/filtros si existen en legacy
- [ ] Validación de permisos con [Authorize]

---

## 🚨 REGLAS APLICABLES

| Regla | Descripción |
|-------|-------------|
| REGLA 2 | Mapeo exacto de BD (nombres de SP, parámetros exactos) |
| REGLA 3 | Validación de respuestas (try-catch-log) |
| REGLA 4 | Ejecutar SP correspondientes de WebMatrix |
| REGLA 6 | Solo migrar acciones existentes en WebMatrix |
| REGLA 7 | Patrón Controller → Service → Adapter → BD |
| REGLA 8 | Async/await obligatorio en I/O |
| REGLA 9 | [Authorize] en controllers |
| REGLA 10 | ApiResponse<T> en todos endpoints |

---

## 📞 PUNTOS DE CONTACTO

- **WebMatrix legacy code**: `WebMatrix/OP_RO/`, `WebMatrix/OP_Trafico/`
- **Stored Procedures**: `MatrixNext/docs/SQL/CO_Matrix_SP_Names.csv`
- **Database models**: `MatrixNext/MatrixNext.Data/Models/OP/`
- **Ejemplos**: Sprint 10 (RP_Reportes) - usar como referencia

---

## 🎬 PRÓXIMAS ACCIONES (Ahora)

1. **INMEDIATO**: Crear branch `feature/sprint-11-setup`
   ```bash
   git checkout -b feature/sprint-11-setup
   ```

2. **HOY**: Leer ambos controllers
   ```bash
   code MatrixNext/MatrixNext.Web/Areas/OP/Controllers/OP_ROController.cs
   code MatrixNext/MatrixNext.Web/Areas/OP/Controllers/OP_TraficoController.cs
   ```

3. **HOY**: Buscar equivalentes en WebMatrix
   ```bash
   Get-ChildItem -Path ".\WebMatrix" -Filter "*RO*" -Recurse
   Get-ChildItem -Path ".\WebMatrix" -Filter "*Trafico*" -Recurse
   ```

4. **MAÑANA**: Crear `SPRINT_11_ESTADO_ACTUAL.md` con hallazgos

---

**Status**: 🟡 Listo para iniciar análisis  
**Estimación**: 2 semanas (72-80h)  
**Next step**: Análisis detallado de controllers existentes

---

*Generado: 2026-01-15 14:45 UTC*  
*Sprint 11 Planning*
