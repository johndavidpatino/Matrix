# SPRINT 11 - STORED PROCEDURES IDENTIFICADOS ✅

Búsqueda exhaustiva completada en `CO_Matrix_SP_Names.csv`

---

## 📋 **OP_RO (Operational Review) - 20 SPs**

### Ejecución Cuestionario (CRUD)
- `OP_RO_EjecucionCuestionario_Add` → Crear ejecución
- `OP_RO_EjecucionCuestionario_Del` → Eliminar ejecución
- `OP_RO_EjecucionCuestionario_Edit` → Editar ejecución
- `OP_RO_EjecucionCuestionario_Get` → Obtener ejecución

### Ejecución Instructivo (CRUD)
- `OP_RO_EjecucionInstructivo_Add` → Crear ejecución
- `OP_RO_EjecucionInstructivo_Del` → Eliminar ejecución
- `OP_RO_EjecucionInstructivo_Edit` → Editar ejecución
- `OP_RO_EjecucionInstructivo_Get` → Obtener ejecución

### Ejecución Material Ayuda (CRUD)
- `OP_RO_EjecucionMaterialAyuda_Add` → Crear ejecución
- `OP_RO_EjecucionMaterialAyuda_Del` → Eliminar ejecución
- `OP_RO_EjecucionMaterialAyuda_Edit` → Editar ejecución
- `OP_RO_EjecucionMaterialAyuda_Get` → Obtener ejecución

### Gestión General OP_RO
- `OP_RO_CuestionarioGet` → Listar cuestionarios
- `OP_RO_CuestionarioGetId` → Obtener cuestionario por ID
- `OP_RO_InstructivoGet` → Listar instructivos
- `OP_RO_InstructivoGetId` → Obtener instructivo por ID
- `OP_RO_MaterialAyudaGet` → Listar materiales de ayuda
- `OP_RO_MaterialAyudaGetId` → Obtener material por ID
- `OP_RO_MetodologiaGet` → Listar metodologías
- `OP_RO_MetodologiaGetId` → Obtener metodología por ID

---

## 🚦 **OP_Trafico (Operational Traffic) - 19 SPs**

### Gestión Archivos Tráfico
- `OP_TraficoArhivos_GetDisponibleDevolucion` → Archivos disponibles para devolución
- `OP_TraficoArhivos_GetDisponibleEnvio` → Archivos disponibles para envío
- `OP_TraficoArhivos_MuestraEnviadaRMC` → Mostrar archivos enviados a RMC

### Gestión Encuestas Tráfico (Transiciones de Estado)
- `OP_TraficoEncuesta_GetCritica` → Obtener encuesta en estado Crítica
- `OP_TraficoEncuesta_GetRMC` → Obtener encuesta en estado RMC
- `OP_TraficoEncuestas_Add_RMC` → Agregar encuesta a RMC
- `OP_TraficoEncuestas_Edit_Critica` → Editar crítica
- `OP_TraficoEncuestas_Edit_Verificacion` → Editar verificación
- `OP_TraficoEncuestas_Get` → Obtener encuesta individual
- `OP_TraficoEncuestas_ListadoGet` → Listar encuestas
- `OP_TraficoEncuestasBorrarEnvio` → Eliminar envío
- `OP_TraficoEncuestasCiudad` → Filtrar por ciudad
- `OP_TraficoEncuestasMuestraCiudadesRMC` → Mostrar ciudades en RMC

### Reportes Tráfico (No incluir en Sprint 11)
- `REP_CantidadEnviadaTrafico` → Reporte cantidad enviada
- `REP_PlaneacionTraficoOperacionesBDD` → Reporte planificación
- `REP_TRAFICO_GENERAL_OPERACIONES` → Reporte general
- `REP_TRAFICO_PROCESAMIENTO` → Reporte procesamiento
- `REP_TraficoAreas` → Reporte por áreas
- `REP_TraficoAreasGeneral` → Reporte general áreas
- `REP_TraficoAreasGeneralTrabajos` → Reporte áreas-trabajos

---

## 🎯 **PATRONES IDENTIFICADOS**

### OP_RO Pattern:
```
OP_RO_Ejecución[Tipo]_[Operación]
├─ Tipo: Cuestionario | Instructivo | MaterialAyuda
└─ Operación: Add | Del | Edit | Get
```

### OP_Trafico Pattern:
```
OP_TraficoEncuestas_[Operación]_[Estado]
├─ Operación: Add | Edit | Get
└─ Estado: RMC | Critica | Verificacion
```

---

## 📊 **RESUMEN DE SCOPE**

| Módulo | Total SPs | CRUD | Listado | Transiciones | Estado |
|--------|-----------|------|---------|--------------|--------|
| **OP_RO** | 20 | ✅ (12) | ✅ (8) | N/A | 🟢 COMPLETO |
| **OP_Trafico** | 19* | ✅ (8) | ✅ (2) | ✅ (10-Reportes excluidos) | 🟢 COMPLETO |

*Sin contar 7 SPs de reportes (REP_*) que manejan RP_Reportes

---

## ✅ **DECISIONES**

1. **Scope Sprint 11**: Incluir SOLO operaciones (no reportes)
   - ✅ OP_RO_EjecucionCuestionario_*, OP_RO_Instructivo_*, OP_RO_MaterialAyuda_*
   - ✅ OP_TraficoEncuestas_* (gestión, captura, crítica, verificación)
   - ❌ REP_* (reportes → delegados a Sprint 12 RP_Reportes)

2. **Mapeo a DTOs**:
   - OP_RODTO.cs: 4 tipos ✅ alineados con SPs CRUD
   - OP_TraficoDTOS.cs: 5 tipos ✅ alineados con SPs de transición

3. **Orden de implementación recomendado**:
   - ✅ **Opción A (RECOMENDADA)**: OP_RO primero (menos complejo, más SPs definidos)
   - Luego OP_Trafico (requiere gestión de transiciones de estado)

---

## 🚀 **PRÓXIMAS ACCIONES**

1. [ ] Crear IOP_ROService + OP_ROService
2. [ ] Crear IOP_ROAdapter → Mapear 20 SPs
3. [ ] Expandir OP_ROController (endpoints para CRUD de 4 tipos)
4. [ ] Crear vistas Razor OP_RO
5. [ ] (Repetir 1-4 para OP_Trafico)
6. [ ] Testing integral

---

**Documento generado**: 2026-01-15 | **Estado**: Listo para implementación inmediata
