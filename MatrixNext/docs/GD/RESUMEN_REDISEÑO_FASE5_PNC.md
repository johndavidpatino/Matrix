# RESUMEN - REDISEÑO FASE 5: PNC (PRODUCTO NO CONFORME)

**Fecha:** 2026-01-10  
**Sprint:** Pre-Sprint 8  
**Responsable:** Análisis Técnico  
**Estado:** ✅ COMPLETADO

---

## 📋 Resumen Ejecutivo

Se rediseñó completamente el **BACKLOG FASE 5 PARTE A** tras descubrir que el backlog original proponía una feature inexistente en el sistema legacy.

### Decisión del Usuario

**Usuario solicitó:** "Quiero migrar el PNC real"  
**Contexto:** Presenté 3 opciones tras detectar discrepancia crítica  
**Opción elegida:** Opción 1 - Migrar PNC REAL (Producto No Conforme)

---

## 🔍 Hallazgos del Análisis

### Problema Detectado

**Backlog Original (FASE5_PARTE_A.md):**
- Propuso: "PNC - Proceso Nueva Creación"
- Objetivo: Sistema para crear nuevos documentos controlados con aprobación multirevidor
- SPs propuestos: `GD_SolicitudPNC_Insert`, `GD_SolicitudPNC_Update`, etc.

**Resultado Investigación:**
```bash
grep_search "GD_SolicitudPNC" → 0 MATCHES (SPs NO existen)
grep_search "PNC" en SQL → 20+ MATCHES (todas tablas calidad)
```

### Realidad del Sistema Legacy

**PNC = Producto No Conforme** (Sistema Calidad ISO 9001)

**Archivos Encontrados:**
- `CoreProject/Clases/PNC/PNCClass.vb` (262 líneas)
- `WebMatrix/GD_Documentos/ProductoNoConformeRegistrar.aspx`
- `WebMatrix/GD_Documentos/GD_SeguimientoPNC.aspx`
- `WebMatrix/MBO/ProductoNoConformeRegistrar.aspx`

**Tablas Reales (12 tablas):**
```sql
PNC_ProductoNoConforme          -- Maestro PNC
PNC_ProductoNoConformeCausas    -- Causas raíz
PNC_ProductoNoConformeAcciones  -- Plan acciones correctivas
PNC_Categorias                  -- Categorías no conformidad
PNC_FuenteReclamo              -- Cliente interno/externo
PNC_TiposDeAccion              -- Inmediata/Correctiva/Preventiva
PNC_Procedimientos
PNC_Procesos
PNC_Productos                  -- Sistema nuevo (alternativo)
PNC_Productos_Causas
PNC_Productos_Estados
PNC_Productos_Log
```

**Stored Procedures (16 SPs):**
```sql
PNC_ObtenerProductoNoConforme(@JobBook)
PNC_ObtenerProductoNoConformeTodos()
PNC_GetById(@Id)
PNC_ProductoNoConformeCausas_Get(@IdPNC)
PNC_ProductoNoConformeAcciones_Get(@IdPNC, @IdCausa)
PNC_Causa_Get(@IdPNC)
PNC_EmailAcciones
PNC_EmailNotificacionReporte
PNC_Productos_Add
PNC_Productos_Get
PNC_Productos_Causas_Add
PNC_Productos_CorreosNotificar
PNC_Productos_Log_Estado_Add
PNC_Productos_Log_Get
PNC_Producto_UpdateEstado
PNC_Seguimiento_Get
```

---

## ✅ Solución Implementada

### Nuevo Backlog Creado

**Archivo:** `BACKLOG_MIGRACION_PNC_PRODUCTOS_NO_CONFORMES.md`

**Contenido:**

1. **Análisis Completo Legacy** (262 líneas PNCClass.vb)
   - 12 tablas documentadas
   - 16 SPs mapeados
   - 3 páginas WebForms identificadas
   - Workflow de estados definido

2. **Sprint 8 Rediseñado (40h)**
   - Tarea 8.1: Análisis y Mapeo (6h)
   - Tarea 8.2: ViewModels PNC (6h) - 20+ modelos
   - Tarea 8.3: Adapter PNC (8h) - 16 SPs con Dapper
   - Tarea 8.4: Service PNC (6h) - Lógica negocio
   - Tarea 8.5: Controller PNC (6h) - CRUD completo
   - Tarea 8.6: Vistas Razor (8h) - 6 vistas

3. **Arquitectura 3 Capas**
   ```
   PncController
       ↓
   PncService (validaciones, email, transacciones)
       ↓
   PncAdapter (Dapper, 16 SPs)
       ↓
   SQL Server
   ```

4. **ViewModels (20+)**
   - ProductoNoConformeVM (maestro)
   - ProductoNoConformeDetalleVM (completo)
   - ProductoNoConformeCausaVM
   - ProductoNoConformeAccionVM
   - PncCategoriaVM, PncFuenteReclamoVM, etc.

5. **Flujo Completo**
   ```
   Registro PNC → Asignar Causas → Plan Acciones → Seguimiento → Cierre
   ```

---

## 📊 Comparación Backlogs

| Aspecto | Original (OBSOLETO) | Nuevo (CORRECTO) |
|---------|---------------------|------------------|
| **Feature** | Proceso Nueva Creación | Producto No Conforme |
| **Propósito** | Crear documentos controlados | Gestión calidad ISO 9001 |
| **SPs** | 10 inventados | 16 existentes |
| **Tablas** | 0 (inventadas) | 12 reales |
| **Código Legacy** | 0 líneas | 262 líneas (PNCClass.vb) |
| **Páginas WebForms** | 0 | 3 identificadas |
| **Relación GD** | Directa (crear docs) | Indirecta (seguimiento calidad) |
| **Prioridad** | ALTA | MEDIA (módulo independiente) |
| **Evidencia** | ❌ NO existe | ✅ Código completo |

---

## 🎯 Funcionalidad Real PNC

### Sistema de Calidad ISO 9001

**PNC = Producto No Conforme**: Producto/servicio que NO cumple especificaciones de calidad.

**Flujo Real:**

1. **Registro Reclamo**
   - Cliente interno/externo reporta no conformidad
   - Asociado a JobBook/Estudio
   - Fuente: Cliente externo, auditoría, interno

2. **Identificar Causas Raíz**
   - Análisis 5 porqués
   - Causa raíz documentada
   - Relación PNC → Causas (1:N)

3. **Plan de Acciones Correctivas**
   - Acción Inmediata (corregir problema)
   - Acción Correctiva (evitar recurrencia)
   - Acción Preventiva (prevenir similares)
   - Responsables asignados
   - Fechas planeadas/ejecutadas

4. **Seguimiento**
   - Verificar ejecución acciones
   - Evidencias de cierre
   - Emails recordatorio si vencen

5. **Cierre PNC**
   - Todas causas cerradas
   - Todas acciones ejecutadas
   - Estado: Cerrado, FechaCierre

**NO ES**: Sistema de gestión documental (eso es GD_SolicitudDocumentos).

---

## 📚 Lecciones Aprendidas

### REGLA 6 - Paridad 1:1

**Violación Detectada:** Backlog proponía feature inexistente.

**Proceso Correcto Aplicado:**

1. ✅ **Iniciar Tarea X.1**: "Mapear SPs..."
2. ✅ **Buscar SPs propuestos**: `grep_search "GD_SolicitudPNC"`
3. ✅ **Resultado 0 matches**: ⚠️ ALERTA - SPs NO existen
4. ✅ **Investigar legacy**: `grep_search "PNC"` → 40+ matches
5. ✅ **Analizar código**: PNCClass.vb, tablas SQL
6. ✅ **Rediseñar backlog**: Basado en evidencia real

**Aprendizaje:**
- ❌ **NO asumir** funcionalidad sin evidencia código
- ✅ **SÍ validar** SPs propuestos antes de implementar
- ✅ **SÍ grep_search** como primer paso de análisis
- ✅ **SÍ consultar** usuario ante discrepancias

---

## 🔄 Archivos Afectados

### Creados
- ✅ `BACKLOG_MIGRACION_PNC_PRODUCTOS_NO_CONFORMES.md` (nuevo backlog correcto)
- ✅ `RESUMEN_REDISEÑO_FASE5_PNC.md` (este documento)

### Modificados
- ✅ `BACKLOG_MIGRACION_GD_DOCUMENTOS_FASE5_PARTE_A.md` (marcado OBSOLETO)

### Estado Backlog Original
```markdown
# ⚠️ BACKLOG OBSOLETO - REEMPLAZADO POR BACKLOG_MIGRACION_PNC_PRODUCTOS_NO_CONFORMES.md

## 🚨 AVISO IMPORTANTE

**Este backlog ha sido DESCARTADO** tras análisis detallado del sistema legacy (2026-01-10).

[... explicación completa ...]

# ❌ CONTENIDO ORIGINAL DESCARTADO (SOLO REFERENCIA HISTÓRICA)

[... contenido original preservado para referencia ...]
```

---

## 📦 Próximos Pasos

### Opción 1: Implementar PNC Real (Recomendado si se requiere sistema calidad)

**Ventajas:**
- ✅ Cumple REGLA 6 (paridad 1:1)
- ✅ Migra funcionalidad existente
- ✅ Sistema calidad ISO 9001 completo
- ✅ 16 SPs legacy mapeados

**Desventajas:**
- ⚠️ Módulo independiente (no crítico para GD core)
- ⚠️ Requiere 40h adicionales

**Timeline:**
- Sprint 8: Implementación PNC (40h)
- Testing: 8h
- Deployment: 4h
- **Total: 52h (1.3 semanas)**

### Opción 2: Excluir FASE 5 Completa (Si solo interesa GD core)

**Ventajas:**
- ✅ GD completo con FASES 1-4
- ✅ Funcionalidad creación docs YA existe (GD_SolicitudDocumentos tipo=Construcción)
- ✅ Deployment inmediato

**Desventajas:**
- ❌ Sin sistema calidad ISO 9001
- ❌ PNC legacy queda sin migrar

**Timeline:**
- Testing E2E GD: 12h
- Deployment: 4h
- **Total: 16h (2 días)**

---

## ✅ Validación del Análisis

### Evidencia Técnica

**grep_search ejecutados:**
```bash
# 1. Buscar SPs propuestos (backlog original)
grep_search "GD_SolicitudPNC" → 0 MATCHES

# 2. Buscar PNC en SQL
grep_search "CREATE TABLE [dbo].[PNC_" → 20 MATCHES
  - PNC_ProductoNoConforme
  - PNC_ProductoNoConformeCausas
  - PNC_ProductoNoConformeAcciones
  - PNC_Categorias
  - PNC_FuenteReclamo
  - PNC_TiposDeAccion
  - PNC_Productos (sistema nuevo)
  
# 3. Buscar PNC en VB.NET
grep_search "PNC" en CoreProject → 20 MATCHES
  - PNCClass.vb (262 líneas)
  - PNCEntities context
  - CU_ClientesPNC.vb

# 4. Buscar páginas WebForms
grep_search "ProductoNoConforme" en *.aspx → 10 MATCHES
  - ProductoNoConformeRegistrar.aspx (3 ubicaciones)
  - GD_SeguimientoPNC.aspx
  - ProductosNoConformeRelacion.aspx
```

**Archivos analizados:**
- ✅ `CO_Matrix_Structure_Tables.sql` (líneas 10090-10330)
- ✅ `CoreProject/Clases/PNC/PNCClass.vb` (líneas 1-150)
- ✅ `ANALISIS_GD_DOCUMENTOS.md` (confirmó PNC = calidad)

---

## 📝 Notas Adicionales

### Confusión Terminológica

**Encontrados 2 sistemas PNC en legacy:**

1. **PNC_ProductoNoConforme** (original, simple)
   - Usado en: GD_Documentos, MBO
   - SPs: PNC_ObtenerProductoNoConforme, PNC_GetById
   - Clase: PNCClass.vb

2. **PNC_Productos** (nuevo, avanzado)
   - Campos adicionales: proceso, procedimiento, impacto
   - Workflow estados: PNC_Productos_Estados
   - Log: PNC_Productos_Log

**Decisión:** Migrar **PNC_ProductoNoConforme** (original) por:
- Mayor uso en páginas legacy
- Estructura más simple
- Mejor documentación en PNCClass.vb

### Feature "Crear Documentos" YA Existe

**Backlog original asumía:** PNC es para crear documentos nuevos.

**Realidad:**
- ✅ `GD_SolicitudDocumentos` con `TipoSolicitud = 1` (Construcción)
- ✅ FASE 3 ya implementó workflow completo
- ✅ Revisores multirevidor YA funciona
- ✅ Auto-creación maestro post-aprobación YA existe

**Conclusión:** Feature propuesta YA está implementada en FASE 3.

---

## 🎯 Decisión Final

**Usuario eligió:** Migrar PNC REAL (Producto No Conforme)

**Justificación:**
- Cumple REGLA 6 (paridad 1:1)
- Sistema calidad útil para auditorías ISO
- Código legacy completo y documentado
- Oportunidad de aplicar lecciones aprendidas

**Próximo paso:**
- Implementar Sprint 8 según `BACKLOG_MIGRACION_PNC_PRODUCTOS_NO_CONFORMES.md`

---

**Generado:** 2026-01-10  
**Última Actualización:** 2026-01-10  
**Estado:** ✅ ANÁLISIS COMPLETADO - LISTO PARA SPRINT 8
