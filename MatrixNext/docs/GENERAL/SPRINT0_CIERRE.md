# Sprint 0 – Cierre de setup y mapeo (2026-01-11)

Objetivo: aplicar checklist de validación a los módulos prioritarios (GD, PY, TH) antes de iniciar desarrollo de Fases 1-4 / siguientes sprints. No se introdujo código nuevo; solo inventarios y mapeos.

## Cobertura por módulo
- **GD_Documentos (Fases 1-4)**
  - Inventario legacy y MatrixNext: ver docs [GD/INVENTARIO_GD_DOCUMENTOS_FASES1_4.md](../GD/INVENTARIO_GD_DOCUMENTOS_FASES1_4.md).
  - Mapeo acción→SP→parámetros con validación en `CO_Matrix_Structure_SP.sql`: [GD/MAPEO_ACCION_SP_GD_FASES1_4.md](../GD/MAPEO_ACCION_SP_GD_FASES1_4.md). SP confirmados: GD_TipoSolicitud_*, GD_EstadoSolicitud_*, GD_Procesos_*, GD_MaestroDocumentos_Add2, GD_DocumentosControlados_Add/Activo, GD_DocumentosMaestros_Update, GD_SolDocumentos_Add, GD_Revisiones_Add/Edit/Get/GetRev, GD_RepositorioDocumentos_GetXTrabajo, GD_EscanerDocumentos_Del. `SolicitudDocumentos_Update` no existe en scripts.
- **PY_Proyectos (pendientes InHomeVisit, VariablesControl, etc.)**
  - Inventario y mapeos previos reutilizables: [PY/ANALISIS_PY_PROYECTOS.md](../PY/ANALISIS_PY_PROYECTOS.md), [PY/MIGRACION_PY_PROYECTOS.md](../PY/MIGRACION_PY_PROYECTOS.md), validaciones BD: [PY/BD_VALIDACION_SPRINT0.md](../PY/BD_VALIDACION_SPRINT0.md), dependencias Core: [PY/MAPA_DEPENDENCIAS_PY_CORE.md](../PY/MAPA_DEPENDENCIAS_PY_CORE.md), evidencias SP/tablas: [PY/VALIDACION_EVIDENCIAS_PY_CORE.md](../PY/VALIDACION_EVIDENCIAS_PY_CORE.md).
- **TH_TalentoHumano (Empleados/Nómina pendientes; Ausencias ya ok)**
  - Análisis existentes: [TH/ANALISIS_TH_EMPLEADOS.md](../TH/ANALISIS_TH_EMPLEADOS.md), [TH/ANALISIS_TH_AUSENCIAS.md](../TH/ANALISIS_TH_AUSENCIAS.md); cierre Ausencias: [TH/RESUMEN_MIGRACION_AUSENCIAS.md](../TH/RESUMEN_MIGRACION_AUSENCIAS.md). Cambios recientes en empleados: [TH/CAMBIOS_TH_EMPLEADOS_20260103.md](../TH/CAMBIOS_TH_EMPLEADOS_20260103.md).

## Checklist Sprint 0 cumplido
- Inventario legacy vs MatrixNext para GD/PY/TH (sin duplicar trabajo ya hecho).
- Mapeo acción→SP→parámetros validado contra CoreProject y `CO_Matrix_Structure_SP.sql` (GD), y reutilización de mapeos existentes para PY/TH.
- Fuentes de verdad identificadas y enlazadas; no se detectaron SP faltantes excepto `SolicitudDocumentos_Update` (no existe en scripts → no usar).

## Próximos pasos (Sprint 1 en adelante)
1) GD Fases 1-2: implementar/paridad de catalogos/infraestructura usando mapeo confirmado; revisar UI vs WebForms y cerrar gaps de flujo/AJAX.
2) GD Fases 3-4: maestro/workflow y repositorio; validar roles y estados.
3) PY pendientes: InHomeVisit, VariablesControl, Instructivos/Planillas, DuplicarTrabajos, DistribucionEntrevistas (usar mapeos PY existentes).
4) TH Empleados/Nómina: completar flujos y reportes respetando SP existentes.

## Notas
- Aún no se crean commits: esperar ajustes de paridad UI/flujo en Sprint 1.
- Mantener Regla 2/2.1 (no inventar objetos); Regla 5.1 (AJAX-first) en todas las implementaciones.
