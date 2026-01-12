# BD_VALIDACION_SPRINT0

**Checklist de Validación - BD Legacy**

Ejecutar ANTES de continuar con Sprint 1.

---

## ✅ Script SQL de Validación

Ref: `VALIDACION_BASE_DATOS.md` § 2

Ejecuta en SQL Server Management Studio (BD legacy):

```sql
-- =================================
-- 1. Confirmar SPs PY_Proyectos
-- =================================

SELECT 
  ROUTINE_NAME,
  ROUTINE_TYPE
FROM INFORMATION_SCHEMA.ROUTINES
WHERE ROUTINE_SCHEMA = 'dbo' 
  AND ROUTINE_TYPE = 'PROCEDURE'
  AND (ROUTINE_NAME LIKE 'PY_Proyectos%' OR ROUTINE_NAME LIKE 'PY_Proyecto_%')
ORDER BY ROUTINE_NAME;

-- Resultado esperado:
-- □ PY_Proyecto_Add
-- □ PY_Proyectos_Edit
-- □ PY_Proyectos_EditGerentePY
-- □ PY_Proyectos_Get
-- □ PY_Proyectos_Get_XAsignar
-- □ PY_Proyectos_Get_XREAsignar
-- □ PY_EspCuentasCuanti
-- □ PY_EspCuentasCuali


-- =================================
-- 2. Confirmar SPs PY_Trabajo
-- =================================

SELECT 
  ROUTINE_NAME,
  ROUTINE_TYPE
FROM INFORMATION_SCHEMA.ROUTINES
WHERE ROUTINE_SCHEMA = 'dbo' 
  AND ROUTINE_TYPE = 'PROCEDURE'
  AND (ROUTINE_NAME LIKE 'PY_Trabajo%' OR ROUTINE_NAME LIKE 'Py_Trabajo%')
ORDER BY ROUTINE_NAME;

-- Resultado esperado:
-- □ PY_Trabajo_Add
-- □ PY_Trabajo_Del
-- □ PY_Trabajo_Edit
-- □ PY_Trabajo_Get
-- □ PY_Trabajo_NombreTrabajoYaExiste
-- □ PY_Trabajos_GET_All
-- □ PY_Trabajos_Get
-- □ Py_TrabajoDuplicar


-- =================================
-- 3. Confirmar PARÁMETROS SPs
-- =================================

-- Ejemplo: PY_Proyectos_Get
EXEC sp_help 'PY_Proyectos_Get';

-- Debe retornar parámetros como:
-- □ @IdProyecto
-- □ @IdGerenteProyectos
-- ... etc


-- =================================
-- 4. Confirmar TRIGGERS
-- =================================

SELECT 
  OBJECT_NAME(parent_id) AS TableName,
  name AS TriggerName
FROM sys.triggers
WHERE is_instead_of_trigger = 0
  AND (OBJECT_NAME(parent_id) LIKE 'PY_Trabajo%'
       OR OBJECT_NAME(parent_id) LIKE 'CORE_WorkFlow%');

-- Resultado esperado:
-- □ ¿Existen triggers que sincronicen PY ↔ CORE?
-- □ Si sí, documentar nombres y lógica


-- =================================
-- 5. Confirmar ÍNDICES
-- =================================

SELECT 
  OBJECT_NAME(i.object_id) AS TableName,
  i.name AS IndexName,
  c.name AS ColumnName
FROM sys.indexes i
INNER JOIN sys.index_columns ic ON i.object_id = ic.object_id AND i.index_id = ic.index_id
INNER JOIN sys.columns c ON ic.object_id = c.object_id AND ic.column_id = c.column_id
WHERE OBJECT_NAME(i.object_id) IN ('PY_Proyectos', 'PY_Trabajo', 'CORE_WorkFlow', 'US_Usuarios')
  AND i.name NOT LIKE 'PK_%'
ORDER BY OBJECT_NAME(i.object_id), i.name;

-- Resultado esperado:
-- □ IX_PY_Trabajo_IdProyecto
-- □ IX_PY_Trabajo_Estado
-- □ IX_CORE_WorkFlow_IdTrabajo
-- □ IX_CORE_WorkFlow_Estado
```

---

## 📋 Checklist de Validación

- [ ] Ejecutar script SQL en BD legacy
- [ ] Documentar resultados (número de SPs encontrados)
- [ ] Confirmar todos los 30+ SPs esperados existen
- [ ] Confirmar parámetros de 5 SPs críticos
- [ ] Confirmar tipos retorno (Result tables vs @@IDENTITY)
- [ ] Buscar triggers que sincronicen PY ↔ CORE
- [ ] Validar índices existen (si no, crear en Sprint 1)
- [ ] Documentar versión SQL Server (2016+, 2019, 2022)

---

## 📝 Resultados (Completar después de ejecutar validación)

```
Fecha validación: ___________
Técnico: ___________
BD Server: ___________
Versión SQL Server: ___________

SPs encontrados: _____ de 30+
Parámetros validados: ✓
Triggers encontrados: _____ 
Índices existentes: ✓
```

---

**T0.7 Status:** Pendiente validación en BD legacy

Una vez completada, proceder a Sprint 1.
