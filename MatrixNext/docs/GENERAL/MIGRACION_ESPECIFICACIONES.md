# ESPECIFICACIONES DE MIGRACIÓN - WebMatrix → MatrixNext

Documento guía para copilotizar los módulos que aún no se han movido al nuevo stack. Resume las reglas del repositorio y define las entradas de trabajo pendientes.

## 1. Referencias obligatorias

- **Reglas principales**: `MatrixNext/DIRECTRICES_MIGRACION.md` (Reglas 1-15). Copilot debe respetar:
  - Consultar `CoreProject` (`WebMatrix/App_Code`, `DataAdapters`) antes de reimplementar cualquier lógica SQL (Regla 2).
  - Ejecutar exactamente los Stored Procedures identificados por cada acción y no inventar objetos de base de datos (Reglas 2.1 y 4).
  - Usar EF Core para CRUD simples y SP para lógica compleja (Regla 3).
  - Reutilizar modales/partial views, AJAX y componentes compartidos; no crear flujos nuevos (Reglas 5, 5.1, 6 y 7).
  - Mantener Areas, registrar en Program.cs y actualizar el menú/sidebar por módulo (Reglas 9 y 10).
  - Validar permisos, datos, errores y usar `async/await` (Reglas 11-14).
  - Documentar cada avance en `MODULOS_MIGRACION.md` y los documentos `ANALISIS_*`, `MIGRACION_*` correspondientes (Regla 15).

## 2. Estructura de trabajo para cada módulo faltante

1. **Análisis**: revisar `WebMatrix/<Módulo>` (carpetas y `.aspx`) y mapear SP en `MatrixNext/docs/SQL/CO_Matrix_*`.
2. **Adapter**: crear/ajustar clases en `MatrixNext/MatrixNext.Web/DataAdapters` o `MatrixNext.Data/Adapters`.
3. **Service**: implementar la lógica en el servicio correspondiente siguiendo el patrón Adapter → Service → Controller.
4. **Controller / Area**: ubicar en `MatrixNext/MatrixNext.Web/Areas/<Módulo>/Controllers`.
5. **Views**: usar Razor + modales (`Views/Shared`) replicando la UI de WebMatrix.
6. **Menú y menú lateral**: agregar entry en `_Sidebar.cshtml`.
7. **Testing y documentación**: actualizar checklists, escribir `ANALISIS_<MODULO>.md`, `MIGRACION_<MODULO>_COMPLETADA.md` y registrar en `MODULOS_MIGRACION.md` y en el dashboard.

## 3. Módulos sin migrar (principalmente WebMatrix → depende de este documento)

| Módulo | Prioridad actual | WebMatrix path | Notas clave |
| --- | --- | --- | --- |
| OP_RO | Media-baja | `WebMatrix/OP_RO/` | Reportes de revisión operativa, filtros y exportes; se debe integrar con `Areas/OP/OP_ROController` ya esbozado. |
| OP_Trafico | Media-baja | `WebMatrix/OP_Trafico/` | Tráfico de datos + dashboards; replicar vistas `Trafico.aspx` y SP asociados. |
| PY_ControlCalidad | Baja | `WebMatrix/PY_ControlCalidad/` | Control de calidad de proyectos; SP en `CO_Matrix_Structure_SP.sql` (`PY_ControlCalidad_*`). |
| PY_Adquisiciones | Baja | `WebMatrix/PY_Adquisiciones/` | Adquisición de servicios/productos; replicar flujos de resumen y generación de requisiciones. |
| PNC | Baja | `WebMatrix/PNC/` | Producto No Conforme, actualmente referenciado desde GD fase 5; requiere su propio area si se decide separar. |
| SG_Actas | Baja | `WebMatrix/SG_Actas/` | Seguimiento de actas, doc en `MatrixNext/docs/GENERAL/SGC_Calidad.md`. |
| SGC_Calidad | Baja | `WebMatrix/SGC_Calidad/` (legacy doc inexistente) | Solo existe guía en `MatrixNext/docs/GENERAL/SGC_Calidad.md`; preparar base de area futura. |
| ES_Estadistica | Baja | `WebMatrix/ES_Estadistica/` | Estadísticas avanzadas; replicar tablas y vistas reporting. |
| Centro_Informacion | Baja | `WebMatrix/Centro_Informacion/` | Paneles de información general. |
| Inventario | Baja | `WebMatrix/Inventario/` | Control de inventario (note: FI decidió no migrar parte de inventario). |
| IT | Baja | `WebMatrix/IT/` | Administración de recursos tecnológicos. |
| MBO* (MBO/MBO_Gerencial/MBO_Operaciones) | Baja | `WebMatrix/MBO*/` | Objetivos por áreas (gerencial, operaciones); replicar vistas y backend. |
| ResumenProduccion | Baja | `WebMatrix/ResumenProduccion/` | Panel de resumen de producción. |
| RE_GT | Baja | `WebMatrix/RE_GT/` | Reportes especiales/seguimiento. |
| PC_PropiedadCliente | Baja | `WebMatrix/PC_PropiedadCliente/` | Gestión de propiedad de clientes y contratos. |
| Otros (Account, Controls, etc.) | Baja | `WebMatrix/Account/`, `WebMatrix/AppUsersControls/`, `WebMatrix/Controls/` | Herramientas globales; evaluar una carpeta shared o global area según sea necesario. |

> Nota: este grupo debe ser la referencia para agregar nuevas tareas a GitHub Copilot; cada PR debe enlazar esta tabla y la regla 15 de documentación.

## 4. Checklist para Copilot (seguir en cada ticket)

1. Validar objetos en `MatrixNext/docs/SQL/CO_Matrix_*` y referenciar los nombres exactos en `ANALISIS_*` antes de escribir código.
2. Reutilizar servicios compartidos (`Services/Shared`, `Infrastructure/Data`) y no crear DAL duplicados.
3. Generar pruebas mínimas (unitarias o manuales) que ejecuten los SP migrados y registrar los resultados en `MIGRACION_*`.
4. Añadir notas en `_Sidebar.cshtml` y en `Program.cs` para registrar el módulo.
5. Actualizar `MODULOS_MIGRACION.md` y `MatrixNext/docs/GENERAL/DASHBOARD_MIGRACION.md` enlazando la especificación de este documento y dejando claro qué quedó pendiente.

## 5. Orquestación Copilot → humanos

- Copilot puede esbozar controladores, servicios, vistas y adapters si se le dan directrices de la tabla anterior.
- Antes de mergear, un desarrollador debe revisar los SP referenciados en este documento y aprobar que cumplen las reglas de naming y privilegios (Reglas 1-4).
- Documentar en PR: `sprint`, `modulo`, `SP usados`, `regla cumplida`.

**Fin del documento de especificaciones. Copilot, úsalo como base para crear tickets y PRs claros.**

## 6. Guía de ejecución para desarrolladores

- **1. Priorizar módulos**: use este mismo documento (tabla de módulo/faltante) como lista de tareas y confirme con `MatrixNext/MODULOS_MIGRACION.md` y `docs/GENERAL/DASHBOARD_MIGRACION.md` que el módulo figura como pendiente.
- **2. Documentación de soporte**: antes de tocar código, abra `MatrixNext/DIRECTRICES_MIGRACION.md` y el `ANALISIS_<MODULO>.md` existente; si no hay análisis, genere una plantilla usando `MatrixNext/PLANTILLA_ANALISIS_PREVIO_MIGRACION.md` y actualice con SP/tablas claves.
- **3. Mapear SQL**: buscar los SP indicados en `MatrixNext/docs/SQL/CO_Matrix_SP_Names.csv` y `CO_Matrix_Structure_*.sql`; anotar nombre/params en el análisis y en el adapter.
- **4. Tareas de migración**:
  1. Crear/ajustar Adapter/Service/Controller (utilizar Areas, respetar `Program.cs` y `_Sidebar.cshtml`).
  2. Reproducir vistas WebMatrix como Razor, preferir modales, partials y AJAX (`Views/Shared/_AjaxModal.cshtml`).
  3. Ejecutar migraciones EF (si aplica) y SP en entorno local o staging; registrar resultados en `MIGRACION_<MODULO>_COMPLETADA.md`.
  4. Validar permisos ([Authorize]), ModelState y manejo de errores según reglas 11-14.
- **5. Verificación**: correr pruebas unitarias/test manual (crear si no existen) para cada acción clave, documentar resultados (capturas consola/logs) y agregar evidencia en `MIGRACION_*`.
- **6. Registro de avance**: actualizar `MODULOS_MIGRACION.md` y `docs/GENERAL/DASHBOARD_MIGRACION.md` con el estado y link al análisis; añadir un ticket o PR a Copilot con referencia a este documento y la regla 15.
