# 📋 PLAN DE REVISIÓN EXHAUSTIVA - MIGRACIÓN WEBMATRIX → MATRIXNEXT

**Fecha**: 2026-01-18  
**Objetivo**: Verificar paridad funcional y cumplimiento de estructura BD  
**Criterio Principal**: NO agregar funcionalidades que no existían en WebMatrix  

---

## 🎯 CRITERIOS DE REVISIÓN POR MÓDULO

### Para CADA módulo se debe verificar:

1. **Paridad de Páginas**: ¿Cada página WebMatrix (.aspx) tiene su equivalente en MatrixNext?
2. **Paridad de SP**: ¿Los SP usados son EXACTAMENTE los del CoreProject?
3. **Sin Funciones Nuevas**: ¿Se agregó algo que NO existía? (excepto modales para CRUD que es mejora válida)
4. **Estructura BD**: ¿Los nombres de tablas/columnas son idénticos?
5. **Flujos de Negocio**: ¿Se mantiene la lógica original?

---

## 📊 MÓDULOS A REVISAR

| # | Módulo | Área MatrixNext | Carpeta WebMatrix | Prioridad |
|---|--------|-----------------|-------------------|-----------|
| 1 | US_Usuarios | US | US_Usuarios | CRÍTICA |
| 2 | TH_TalentoHumano | TH | TH_TalentoHumano | CRÍTICA |
| 3 | CU_Cuentas | CU | CU_Cuentas | CRÍTICA |
| 4 | CC_FinzOpe + FI | CC | CC_FinzOpe, FI_AdministrativoFinanciero | CRÍTICA |
| 5 | OP_Cuantitativo | OP | OP_Cuantitativo | ALTA |
| 6 | OP_Cualitativo | OP | OP_Cualitativo | ALTA |
| 7 | OP_RO | OP | OP_RO | ALTA |
| 8 | OP_Trafico | OP | OP_Trafico | ALTA |
| 9 | CORE (Workflow) | CORE | CORE | ALTA |
| 10 | EQ (EasyQuote) | EQ | N/A (nuevo sistema) | MEDIA |
| 11 | ES_Estadistica | ES | ES_Estadistica | MEDIA |
| 12 | GD_Documentos | GD | GD_Documentos | MEDIA |
| 13 | INV (Inventario) | INV | Inventario | MEDIA |
| 14 | MBO | MBO | MBO, MBO_Gerencial, MBO_Operaciones | MEDIA |
| 15 | PY_Proyectos | PY | PY_Proyectos | ALTA |
| 16 | PY_ControlCalidad | PY | PY_ControlCalidad | ALTA |
| 17 | RP_Reportes | RP | RP_Reportes | MEDIA |
| 18 | SGC_Calidad | SGC | SGC_Calidad | MEDIA |
| 19 | PC_PropiedadCliente | PC | PC_PropiedadCliente | BAJA |
| 20 | IT | IT | IT | BAJA |
| 21 | RE_GT | RE_GT | RE_GT | MEDIA |

---

## 🔍 METODOLOGÍA DE REVISIÓN

### FASE 1: Inventario de Páginas Legacy

Para cada módulo:
```
1. Listar TODAS las páginas .aspx en WebMatrix/[Módulo]/
2. Documentar funcionalidad principal de cada página
3. Identificar SP usados en CodeBehind (.vb)
```

### FASE 2: Comparación con MatrixNext

```
1. Mapear cada página WebMatrix → Controller/Action en MatrixNext
2. Verificar que SP usados son idénticos
3. Identificar funcionalidades EXTRA que no existían
```

### FASE 3: Validación de Corrección

```
1. Marcar funciones que deben REMOVERSE
2. Marcar funciones que son MEJORAS VÁLIDAS (ej: modales)
3. Ejecutar correcciones
```

---

## 📝 PLANTILLA DE AUDITORÍA POR MÓDULO

```markdown
## [NOMBRE_MÓDULO]

### Páginas Legacy vs MatrixNext

| Página WebMatrix | Controller/Action MatrixNext | Estado | Notas |
|------------------|------------------------------|--------|-------|
| [Ejemplo.aspx]   | [ExampleController/Index]    | ✅/⚠️/❌ | |

### SP Usados

| SP en CoreProject | Usado en MatrixNext | Verificado |
|-------------------|---------------------|------------|
| [SP_Name]         | ✅/❌               | ✅/❌      |

### Funcionalidades Nuevas (NO en WebMatrix)

| Funcionalidad | Decisión | Razón |
|---------------|----------|-------|
| [Descripción] | MANTENER/REMOVER | [Justificación] |

### Acciones Requeridas

- [ ] Acción 1
- [ ] Acción 2
```

---

## ⏱️ ESTIMACIÓN DE TIEMPO

| Fase | Módulos | Horas Estimadas |
|------|---------|-----------------|
| Inventario WebMatrix | 21 módulos | 8h |
| Comparación MatrixNext | 21 módulos | 12h |
| Correcciones | Variable | 16h |
| **TOTAL** | | **36h** |

---

## 📅 CRONOGRAMA

- **Día 1**: Módulos críticos (US, TH, CU, CC/FI)
- **Día 2**: Módulos operativos (OP_*, CORE)
- **Día 3**: Módulos complementarios (EQ, ES, GD, INV, MBO)
- **Día 4**: Módulos restantes (PY, RP, SGC, PC, IT, RE_GT)
- **Día 5**: Correcciones y documentación final

---

## ✅ CHECKLIST FINAL PRE-PRODUCCIÓN

- [ ] Todos los módulos auditados
- [ ] Funciones nuevas injustificadas removidas
- [ ] SP verificados contra CoreProject
- [ ] Nombres BD verificados
- [ ] 0 errores de compilación
- [ ] Documentación actualizada
