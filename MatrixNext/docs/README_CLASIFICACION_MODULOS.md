# ⚠️ CLASIFICACIÓN DE MÓDULOS - LEER PRIMERO

> **OBJETIVO**: Evitar que el equipo de desarrollo trabaje en módulos ya completados.  
> **Fecha**: 2026-01-15  
> **Sprint Actual**: Sprint 12 Parte 1 (Revisión/QA)

---

## 🚨 REGLA DE ORO: NO TOCAR MÓDULOS COMPLETADOS

Los **Sprints 1-11 están 100% completados**. **NO reiniciar migración** en ninguno de los siguientes módulos:

### ✅ MÓDULOS COMPLETADOS (18 total - NO TOCAR)

1. US_Usuarios
2. TH_TalentoHumano (Sprint 4 API)
3. TH_Ausencias
4. CU_Cuentas
5. CC_FinzOpe / FI_Administrativo
6. OP_Cualitativo
7. CORE (Workflow)
8. EQ (EasyQuote)
9. Home Dashboard
10. RP_Reportes
11. OP_RO
12. OP_Trafico

**Evidencia**: Ver [DASHBOARD_MIGRACION.md](GENERAL/DASHBOARD_MIGRACION.md) sección "✅ MÓDULOS COMPLETADOS"

---

## 🔍 MÓDULOS EN REVISIÓN/QA (3 total - Solo Auditar)

> **Acción**: Verificar completitud al 100%, QA funcional, ajustes menores **SOLAMENTE**.  
> **NO** migrar desde cero - el código ya existe en MatrixNext.

| Módulo | Acción Requerida | Prioridad |
| --- | --- | --- |
| **OP_Cuantitativo** | Auditar 31 páginas WebMatrix vs MatrixNext, completar missing | 🔴 ALTA |
| **PY_Proyectos** | Auditar 18 páginas WebMatrix vs MatrixNext, completar asignaciones/reportes | 🟠 MEDIA |
| **GD_Documentos** | Verificar workflows aprobación, filesystem integration | 🟠 MEDIA |

**Evidencia**: Ver [DASHBOARD_MIGRACION.md](GENERAL/DASHBOARD_MIGRACION.md) sección "🔍 MÓDULOS EN REVISIÓN/QA"

**Archivos clave**:
- OP_Cuantitativo: `MatrixNext.Web/Areas/OP/Controllers/FichaCuantitativaController.cs`
- PY_Proyectos: `MatrixNext.Web/Areas/PY/Controllers`
- GD_Documentos: `MatrixNext.Web/Areas/GD/Controllers`

---

## 🚧 MÓDULOS PENDIENTES MIGRACIÓN (8 total - Trabajo Completo)

> **Acción**: Iniciar migración completa desde cero siguiendo [DIRECTRICES_MIGRACION.md](../DIRECTRICES_MIGRACION.md)

### Orden Sugerido (Sprints 12-19)

| Sprint | Módulo | Prioridad | Estimación | WebMatrix Path |
| --- | --- | --- | --- | --- |
| 12 | PY_ControlCalidad | 🟡 MEDIA-BAJA | 3-4 sem | `WebMatrix/PY_ControlCalidad` |
| 13 | SGC_Calidad | 🟡 MEDIA-BAJA | 2-3 sem | (ver `GENERAL/SGC_Calidad.md`) |
| 14 | ES_Estadistica | 🟡 BAJA | 2-3 sem | `WebMatrix/ES_Estadistica` |
| 15 | IT | 🟡 BAJA | 1-2 sem | `WebMatrix/IT` |
| 16-17 | MBO / MBO_Gerencial / MBO_Operaciones | 🟡 BAJA | 4-6 sem | `WebMatrix/MBO*` |
| 18 | ResumenProduccion | 🟡 BAJA | 2-3 sem | `WebMatrix/ResumenProduccion` |
| 19 | RE_GT | 🟡 BAJA | 1-2 sem | `WebMatrix/RE_GT` |
| 19 | PC_PropiedadCliente | 🟡 BAJA | 1-2 sem | `WebMatrix/PC_PropiedadCliente` |

**Estado**: ❌ **NINGUNO DE ESTOS TIENE CÓDIGO EN MATRIXNEXT** - Migración desde cero requerida.

---

## ⛔ MÓDULOS EXCLUIDOS (2 total - NO MIGRAR)

> **Decisión de negocio**: NO migrar por razones estratégicas.

1. **Centro_Informacion** - Excluido por decisión del usuario
2. **Inventario** - Fuera de alcance FI

---

## 📊 RESUMEN RÁPIDO

| Categoría | Cantidad | % | Acción |
| --- | --- | --- | --- |
| ✅ Completados | 18 | 58% | **NO TOCAR** |
| 🔍 En Revisión/QA | 3 | 10% | Solo auditar |
| 🚧 Pendientes | 8 | 26% | Migración completa |
| ⛔ Excluidos | 2 | 6% | No migrar |
| **TOTAL** | **31** | **100%** | - |

---

## 🎯 FLUJO DE TRABAJO RECOMENDADO

### Para módulos EN REVISIÓN/QA (OP_Cuantitativo, PY_Proyectos, GD_Documentos)

```
1. Leer DIRECTRICES_MIGRACION.md
2. Abrir WebMatrix legacy (carpeta correspondiente)
3. Listar TODAS las páginas .aspx en WebMatrix
4. Abrir MatrixNext (Areas/[MODULO]/Controllers)
5. Crear matriz: Página WebMatrix → Controller MatrixNext → Status
6. Identificar missing pages
7. Para cada missing:
   - Analizar SP ejecutados en CoreProject
   - Crear Adapter (Dapper + SP)
   - Crear Service (lógica de negocio)
   - Crear Controller (endpoints REST)
   - Crear Views (Razor + Ajax)
8. QA funcional completo
9. Documentar en VERIFICACION_[MODULO].md
10. Actualizar DASHBOARD_MIGRACION.md
```

### Para módulos PENDIENTES (PY_ControlCalidad, SGC_Calidad, etc.)

```
1. Leer DIRECTRICES_MIGRACION.md
2. Usar PLAN_MIGRACION_PY_PROYECTOS.md como plantilla
3. Crear ANALISIS_[MODULO].md
4. Seguir patrón: Adapter → Service → Controller → Views
5. Testing progresivo
6. Documentar en MIGRACION_[MODULO]_COMPLETADA.md
7. Actualizar DASHBOARD_MIGRACION.md
```

---

## 📖 DOCUMENTOS CLAVE

| Documento | Para Qué Usar |
| --- | --- |
| **[DASHBOARD_MIGRACION.md](GENERAL/DASHBOARD_MIGRACION.md)** | Estado actual ejecutivo, métricas, clasificación |
| **[MODULOS_MIGRACION.md](../MODULOS_MIGRACION.md)** | Catálogo completo con LOC, evidencia, estados |
| **[DIRECTRICES_MIGRACION.md](../DIRECTRICES_MIGRACION.md)** | 15 reglas obligatorias (leer SIEMPRE antes de migrar) |
| **[PLAN_MIGRACION_PY_PROYECTOS.md](../PLAN_MIGRACION_PY_PROYECTOS.md)** | Plantilla para planificar nuevos módulos |
| **[ANALISIS_OP_CUANTITATIVO.md](OP/ANALISIS_OP_CUANTITATIVO.md)** | Ejemplo de análisis completo (módulo grande) |

---

## ❓ PREGUNTAS FRECUENTES

### ¿Puedo empezar a trabajar en OP_Cuantitativo?

**SÍ**, pero **NO migrar desde cero**. El código base ya existe en `Areas/OP/Controllers/FichaCuantitativaController.cs`. Tu trabajo es:
- Auditar qué falta
- Completar missing features
- QA funcional

### ¿Puedo empezar a trabajar en PY_ControlCalidad?

**SÍ**, este módulo **NO tiene código en MatrixNext**. Debes migrar desde cero siguiendo las directrices.

### ¿Cómo sé si un módulo está completado?

1. Buscar en [DASHBOARD_MIGRACION.md](GENERAL/DASHBOARD_MIGRACION.md) sección "✅ MÓDULOS COMPLETADOS"
2. Si está en esa lista con ✅ → **NO TOCAR**
3. Si está en "🔍 EN REVISIÓN/QA" → Solo auditar y completar
4. Si está en "🚧 PENDIENTES" → Migración completa

### ¿Qué hago si encuentro un bug en un módulo completado?

1. **NO reiniciar migración**
2. Crear issue específico documentando el bug
3. Corregir SOLAMENTE el bug
4. QA de regresión
5. Documentar en CHANGELOG

---

**Última actualización**: 2026-01-15  
**Responsable**: Equipo MatrixNext  
**Contacto**: Ver DASHBOARD_MIGRACION.md para métricas actualizadas
