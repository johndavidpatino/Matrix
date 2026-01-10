# DIRECTRICES DE MIGRACIÓN - WebMatrix → MatrixNext



**Documento de Referencia Técnica**  

**Versión**: 1.0  

**Última Actualización**: 2026-01-02  

**Aplicable a**: Todos los módulos en migración



---



## 📋 ÍNDICE



1. [Reglas Core](#reglas-core)

2. [Arquitectura y Patrones](#arquitectura-y-patrones)

3. [Base de Datos](#base-de-datos)

4. [Controladores y Servicios](#controladores-y-servicios)

5. [Vistas y UI](#vistas-y-ui)

6. [Menú y Navegación](#menú-y-navegación)

7. [Testing y Validación](#testing-y-validación)

8. [Documentación](#documentación)



---



## 🎯 REGLAS CORE



### REGLA 1: Respetar Convenciones de Base de Datos



**Descripción**: Todos los nombres de procedimientos, tablas, columnas y tipos deben respetar exactamente la nomenclatura de la base de datos.



**Aplicación**:

- No cambiar nombres de SP ni de tablas

- Mantener prefijos existentes (TH_, US_, PY_, etc.)

- Respetar casing original (ej: `IdEmpleado`, `FechaInicio`)

- Validar en SQL Server antes de usar







---



### REGLA 2: Consultar CoreProject y Mapear Metadata de Base de Datos (Tablas, SP, Campos)



**Descripción**: Antes de crear o rehacer cualquier funcionalidad, siempre consultar el código y los adaptadores en `CoreProject` (WebMatrix legacy) para mapear de forma exhaustiva los nombres de tablas, procedimientos almacenados, vistas, nombres de campos, tipos de datos y cualquier otra metadata de la base de datos. `CoreProject` es la fuente primaria para nomenclatura y uso; `CO_Matrix_Structure.sql` debe usarse como referencia autoritativa del esquema (tipos y estructura).



**Aplicación**:

1. Revisar `CoreProject` para identificar: nombres exactos de tablas, SP, parámetros y campos usados por cada acción.

2. Documentar el mapeo (acción → SP(s) → parámetros → columnas afectadas).

3. No reimplementar lógica SQL existente sin justificarlo; preferir invocar SP ya existentes o migrarlos de forma controlada.

4. Crear adaptadores que usen exactamente los nombres y parámetros identificados en `CoreProject`.

5. Cross-check: corroborar nombres y tipos contra `CO_Matrix_Structure.sql` (ver sección "Verificación con CO_Matrix_Structure.sql").



**Proceso**:

```

Paso 1: Analizar CoreProject

└─ ¿Qué SP/tabla/columnas usa el DataLayer? → ej: TH_Ausencia.RegistrosAusencia, tabla TH_SolicitudAusencia, columna IdEmpleado



Paso 2: Validar en SQL Server o contra CO_Matrix_Structure.sql

└─ Confirmar existencia y tipo: SELECT TOP 0 * FROM TH_SolicitudAusencia;  -- o revisar script CO_Matrix_Structure.sql



Paso 3: Mapear en Adapter (usar nombres exactos)

└─ public List<...> ObtenerSolicitudes(...) { return connection.Query("TH_Ausencia.RegistrosAusencia", ...) }



Paso 4: Exponer en Service y Controller

└─ public (bool, List<...>) ObtenerSolicitudes(...) { ... }



Paso 5: Documentar y agregar pruebas que ejecuten el SP/consulta sobre ambiente de staging

```



**Beneficios**:

- ✅ Evita ruptura por nombres/tipos inconsistentes

- ✅ Mantiene compatibilidad con procesos y reportes ya existentes

- ✅ Facilita auditoría y rollback

- ✅ Permite validar cambios contra el script oficial `CO_Matrix_Structure.sql`



---



### REGLA 2.1: Prohibido inventar objetos de BD (validaci?n obligatoria)

**Descripci?n**: No se crean ni se asumen tablas, SP, vistas o columnas. Solo se usan objetos existentes en `MatrixNext/docs/OP/SQL/` y/o `CO_Matrix_Structure.sql`.

**Aplicaci?n**:
- Verificar el objeto en los archivos de `MatrixNext/docs/OP/SQL/` y registrar la fuente (archivo + nombre exacto).
- Si no existe en los archivos, detener implementaci?n y escalar al DBA/arquitecto.
- Documentar discrepancias en `MIGRACION_[MODULO]_COMPLETADA.md` antes de tocar c?digo.

### REGLA 3: Utilizar EF para Inserciones y Actualizaciones



**Descripción**: Usar Entity Framework Core para operaciones INSERT y UPDATE simples. Reservar SP para lógica compleja.



**Aplicación**:



**✅ USAR EF CORE para**:

- INSERT nuevos registros

- UPDATE de campos simples

- DELETE de registros (si no hay triggers complejos)

- Operaciones que NO requieren lógica de negocio en SQL



**✅ USAR STORED PROCEDURES para**:

- Lógica compleja (validaciones, cálculos)

- Múltiples tablas (transacciones)

- Reportes con JOIN pesados

- Cálculos de auditoría







---



### REGLA 4: Ejecutar Procedimientos Almacenados de Cada Acción



**Descripción**: Cada acción de WebMatrix ejecuta procedimientos específicos. Identificarlos y ejecutarlos en MatrixNext de la misma forma.



**Aplicación**:



**Mapeo Necesario**:

```

WebMatrix Action          →  SP Ejecutado              →  MatrixNext

═════════════════════════════════════════════════════════════════════

Crear Solicitud          →  TH_Ausencia.RegistrosAusencia (INSERT)

                            TH_Ausencia.CalculoDias

                            TH_Ausencia.ValidarSolicitud



Aprobar Solicitud        →  TH_Ausencia.RegistrosAusencia (UPDATE Estado=20)

                            TH_Ausencia.CausarVacaciones (si aplica)



Rechazar Solicitud       →  TH_Ausencia.RegistrosAusencia (UPDATE Estado=10)



Crear Incapacidad        →  TH_Ausencia_Incapacidades (INSERT)



Obtener Historial        →  TH_AUSENCIA_GET (SP legado)

                            o TH_Ausencia.RegistrosAusencia



Generar Reportes         →  TH_REP_Vacaciones, TH_REP_Beneficios, etc.

```



**Cómo Identificar SP**:

1. Abrir WebMatrix proyecto

2. Buscar clase DataLayer/DataAdapter del módulo

3. Notar qué SP se llama en cada método

4. Documentar exactamente el nombre del SP

5. Copiar la lógica de parámetros





---



### REGLA 5: Preferir Modales para Edición y Detalles



**Descripción**: Usar modales (Bootstrap Modal) en lugar de páginas separadas para editar, ver detalles, o eliminar registros.



**Aplicación**:



**Acciones que DEBEN ser Modal**:

- ✅ Editar registro (Create/Edit combined)

- ✅ Ver detalles ampliados

- ✅ Confirmar eliminación

- ✅ Cambiar estado (aprobar/rechazar)

- ✅ Agregar comentarios

- ✅ Seleccionar opciones secundarias



**Acciones que NO necesitan Modal**:

- ❌ Index/Listado (página principal)

- ❌ Dashboard/Summary

- ❌ Reportes complejos

- ❌ Navegación entre secciones



### REGLA 5.1: UX AJAX-First (Modales + JSON + Toast + Refresh Parcial)



**Descripción**: Todas las interacciones de creación/edición/confirmación deben priorizar una experiencia sin navegación completa usando modales, formularios parciales, respuestas JSON y notificaciones tipo toast.



**Aplicación**:

- UX First: priorizar claridad, menos clics y estados visibles; si hay conflicto con paridad técnica, elevar la decisión.
- GET modal: devolver PartialView cuando `X-Requested-With = XMLHttpRequest`.

- POST modal: en éxito responder JSON `{ success, message }`; en error de validación devolver el parcial con los mensajes.

- Toast: usar contenedor compartido para confirmar acciones y errores no bloqueantes.

- Refresh parcial: recargar únicamente el contenedor con `data-grid-url` del listado afectado.

- Fallback: sin AJAX, renderizar vistas completas (progresive enhancement) manteniendo la funcionalidad.

- Estándar cliente: reutilizar `Views/Shared/_AjaxModal.cshtml`, `Views/Shared/_ToastContainer.cshtml` y `wwwroot/js/ajax-modal.js`.





---



### REGLA 6: Agregar Acciones Existentes, No Crear Nuevas



**Descripción**: Solo migrar acciones (botones, funcionalidades) que existan en WebMatrix. No agregar nuevas features durante la migración.



**Aplicación**:



**✅ HACER**:

- Crear/Editar/Leer/Eliminar (si existen en WebMatrix)

- Aprobar/Rechazar (si existen)

- Cambiar estado (si existen)

- Exportar/Reportes (si existen)

- Búsqueda/Filtros (si existen)



**❌ NO HACER**:

- Agregar nuevos campos que no estén en WebMatrix

- Crear nuevas acciones (ej: "DuplicarSolicitud")

- Cambiar flujo de negocio

- Agregar validaciones adicionales

- Implementar nuevos reportes





---



### REGLA 7: Aprovechar Elementos Visuales Disponibles



**Descripción**: Usar componentes (controles, dropdowns, selectores) que ya existen en MatrixNext de otros módulos migrados.



**Aplicación**:



**Componentes Reutilizables**:

```

Componente               Ubicación                    Uso

═════════════════════════════════════════════════════════════════════

Modal CRUD              Views/Shared/_Modal*         Editar/Crear

DatePicker              Views/Shared/_DatePicker     Seleccionar fechas

Dropdown Usuarios       Views/Shared/_SelectUser     Seleccionar persona

Grid Paginado           Views/Shared/_Grid           Mostrar listados

Buscador                Views/Shared/_Search         Buscar registros

Confirmación Modal      Views/Shared/_Confirm        Confirmar acciones

Toast Notificaciones    Views/Shared/_Toast          Mostrar mensajes

Loading Spinner         Views/Shared/_Loading        Indicador de carga

Badge Estados           Views/Shared/_Badge          Mostrar estados

Sidebar Menú            Views/Shared/_Sidebar        Navegación

```



**Cómo Usar**:



---



### REGLA 8: Priorizar Detalle sobre Velocidad



**Descripción**: Es mejor migrar pocos webforms completamente que muchos webforms incompletos. Avanzar lentamente asegura calidad.



**Aplicación**:



```

Semana 1: Módulo COMPLETO (100% de 1-2 webforms)

├── Análisis exhaustivo

├── Mapeo de SP

├── Implementar CRUD perfecto

├── Documentar cada detalle

├── Testing funcional

└── ✅ LISTO PARA PRODUCCIÓN



Semana 2: Siguiente Módulo COMPLETO

├── Repetir proceso

├── Aplicar lecciones aprendidas

├── Menos problemas esta vez

└── ✅ LISTO PARA PRODUCCIÓN

```



**Vs. Patrón INCORRECTO**:

```

Intento Rápido (EVITAR):

├── Semana 1: Migrar 8 webforms (sin testear bien)

├── Semana 2: Semana 3: Bugs aparecen

├── Semana 3: Devolverse a arreglar problemas

├── Semana 4: Aún hay bugs

└── ❌ TIEMPO PERDIDO

```



**Checklist de Completitud**:
- Para cada webform migrado:
- Todos los campos presentes
- Todos los botones implementados
- Todas las validaciones funcionan
- Todos los SP se ejecutan
- Modales funcionan correctamente
- B?squeda/Filtros funcionan
- Paginaci?n funciona
- Exportaci?n funciona (si existe)
- Error handling implementado
- Logging implementado
- Documentaci?n completa
- Testing funcional exitoso
- Code review pasado




---



### REGLA 9: Mantener Estructura de Áreas



**Descripción**: Usar Areas para todos los módulos excepto funcionalidades globales (Login, Home, etc.).



**Aplicación**:



**Estructura Obligatoria**:

```

MatrixNext.Web/

├── Areas/

│   ├── TH/                              # Talento Humano

│   │   ├── Controllers/

│   │   │   ├── AusenciasController.cs

│   │   │   ├── EmpleadosController.cs

│   │   │   └── NominaController.cs

│   │   └── Views/

│   │       ├── Ausencias/

│   │       │   ├── Index.cshtml

│   │       │   ├── Create.cshtml

│   │       │   └── _Modal.cshtml

│   │       ├── Empleados/

│   │       └── Nomina/

│   │

│   ├── PY/                              # Proyectos

│   │   ├── Controllers/

│   │   └── Views/

│   │

│   └── [Otros módulos...]

│

├── Controllers/                         # Controllers GLOBALES SOLO

│   ├── HomeController.cs               # Dashboard principal

│   └── AccountController.cs            # Login (opcional, si existe)

│

├── Views/

│   ├── Home/

│   │   └── Index.cshtml                # Dashboard

│   └── Shared/                         # Componentes compartidos

│       ├── _Layout.cshtml

│       ├── _Sidebar.cshtml

│       ├── _Modal.cshtml

│       ├── _DatePicker.cshtml

│       └── [Otros componentes]

│

└── Program.cs                          # Configuración global

```



**Beneficios**:

- ✅ Escalabilidad (agregar módulos fácilmente)

- ✅ Equipos independientes (cada área por equipo)

- ✅ Evitar conflictos de nombres

- ✅ URLs claras (`/TH/Ausencias`, `/PY/Proyectos`)

- ✅ Mantenibilidad



**Registro en Program.cs**:



---



### REGLA 10: Crear Menú y Sidebar para Acceso



**Descripción**: Agregar entradas en el menú/sidebar para cada módulo y submodulo migrado.



**Aplicación**:



**Estructura de Menú**:

```

Home                          → /Home

├── Talento Humano            → #

│   ├── Ausencias             → /TH/Ausencias

│   │   ├── Nueva Solicitud   → /TH/Ausencias/Create

│   │   ├── Mis Solicitudes   → /TH/Ausencias

│   │   ├── Por Aprobar       → /TH/GestionAusencia

│   │   └── Equipo            → /TH/AusenciasEquipo

│   ├── Empleados             → /TH/Empleados

│   ├── Nómina                → /TH/Nomina

│   └── ...

│

├── Proyectos                 → #

│   ├── Gestión Proyectos     → /PY/Proyectos

│   ├── Actividades           → /PY/Actividades

│   ├── Hitos                 → /PY/Hitos

│   └── Reportes              → /PY/Reportes

│

├── Administración            → #

│   ├── Usuarios              → /US/Usuarios

│   ├── Roles                 → /US/Roles

│   ├── Permisos              → /US/Permisos

│   └── Grupos                → /US/Grupos

│

└── [Otros módulos...]

```



**Implementación en _Sidebar.cshtml**:



**Actualizaciones Necesarias al Agregar Módulos**:

```

1. Crear área con controllers/views

2. Agregar entrada en _Sidebar.cshtml

3. Registrar módulo en Program.cs (AddTHModule, etc.)

4. Documentar en este archivo

5. Commit a git

```



---



## 🏗️ ARQUITECTURA Y PATRONES



### PATRÓN: Adapter + Service + Controller



**Estructura Obligatoria**:

```

Request (HTTP)

    ↓

Controller (recibe request, valida, coordina)

    ↓

Service (lógica de negocio, transformación)

    ↓

DataAdapter (interactúa con BD)

    ↓

SQL (SP o EF)

    ↓

Response (JSON o View)

```



**Responsabilidades Claras**:



| Capa | Responsabilidad | Ejemplos |

|------|-----------------|----------|

| **Controller** | Recibir request, coordinar, retornar respuesta | Validar headers, autenticación, llamar service |

| **Service** | Lógica de negocio, validaciones | Calcular días, validar disponibilidad, logging |

| **Adapter** | Acceso a datos, mapeo | Ejecutar SP, EF CRUD, mapear resultados |

| **Database** | Almacenamiento, triggers, índices | Tablas, SP, vistas |





---



## 💾 BASE DE DATOS



### Convenciones de Nombres



| Elemento | Formato | Ejemplo | Regla |

|----------|---------|---------|-------|

| **Tabla** | `[MODULO]_[Entidad]` | `TH_SolicitudAusencia` | PascalCase, con prefijo |

| **Columna** | `[NombreEnCamelCase]` | `IdEmpleado`, `FechaInicio` | Respetar casing original |

| **SP** | `[MODULO]_[Accion]` o `[MODULO].[Accion]` | `TH_AUSENCIA_GET` o `TH_Ausencia.RegistrosAusencia` | MAYÚSCULAS o [schema]. |

| **PK** | Siempre `Id` | `Id` | int o long |

| **FK** | `Id[Tabla]` | `IdEmpleado`, `IdSolicitud` | Referencia a tabla |

| **Auditoría** | `RegistradoPor`, `FechaRegistro`, `ModificadoPor`, `FechaModificacion` | - | En cada tabla |



### Verificación con los scripts divididos y `CoreProject`



**Objetivo**: Corroborar que la estructura (tablas), nombres de objetos (tablas, SP, vistas), columnas y tipos de datos en la implementación coinciden con los scripts oficiales y con el uso observado en `CoreProject`.



**Archivos disponibles (ruta: `MatrixNext/docs/SQL/`)**:

- `CO_Matrix_SP_Names.csv` — CSV con la lista de nombres de Stored Procedures (ideal para búsquedas rápidas).

- `CO_Matrix_Structure_SP.sql` — definiciones completas de Stored Procedures (cuerpo y parámetros).

- `CO_Matrix_Structure_Tables.sql` — definiciones completas de tablas (columnas, tipos, constraints).

- `CO_Matrix_Structure_Views.sql` — definiciones de vistas.



**Uso recomendado**:

- Necesito sólo los nombres de SP → abrir `CO_Matrix_SP_Names.csv`.

- Inspeccionar parámetros y lógica del SP → `CO_Matrix_Structure_SP.sql`.

- Ver definición de columnas, tipos y constraints → `CO_Matrix_Structure_Tables.sql`.

- Revisar vistas utilizadas en reportes/joins → `CO_Matrix_Structure_Views.sql`.



**Pasos replicables**:

1. Identificar objetivo (nombre SP / tabla / vista) y elegir el archivo correcto según la tabla anterior.

2. Buscar en archivo correspondiente con el editor o desde PowerShell/CLI:

    - Buscar en CSV (SP names): `Select-String -Path .\MatrixNext\docs\OP\SQL\CO_Matrix_SP_Names.csv -Pattern "TH_AUSENCIA"`

    - Buscar en scripts: `Select-String -Path .\MatrixNext\docs\OP\SQL\CO_Matrix_Structure_*.sql -Pattern "TH_AUSENCIA"`

3. Verificar definición y types:

    - Para tablas: revisar columnas, `NULL/NOT NULL`, `PK`, `FK` en `CO_Matrix_Structure_Tables.sql`.

    - Para SP: revisar parámetros y valor de retorno en `CO_Matrix_Structure_SP.sql`.

4. Comparar con el uso en `CoreProject` (DataLayer): confirmar que los parámetros y campos usados coinciden con lo descrito en los archivos.

5. En ambiente de staging, ejecutar verificaciones en BD real (no productiva):

    - `SELECT TOP 0 * FROM [Schema].[Table];` — valida columnas y nombres localmente.

    - `EXEC sp_describe_first_result_set N'SELECT * FROM Schema.Table';` — muestra tipos y metadata.

6. Si hay diferencias de nombres o tipos:

    - Documentar la discrepancia en `MIGRACION_[MODULO]_COMPLETADA.md` y notificar al DBA/CoreProject owner.

    - No cambiar el código hasta acordar plan de reconciliación.



**Automatización mínima (opcional)**:

- Extraer lista de objetos de los `.sql` y generar CSV para comparaciones automáticas (PowerShell, Python).

- Usar `sqlpackage` o herramientas de schema-compare para detectar diferencias entre el script y la BD de staging.



**Nota**: `CoreProject` dicta cómo se usan los objetos; los archivos bajo `MatrixNext/docs/OP/SQL/` documentan cómo están definidos. Ambos deben coincidir o tener un plan de reconciliación.









---



## 🎮 CONTROLADORES Y SERVICIOS



- Controllers: delgados, validan entrada, orquestan y delegan al service.

- Services: concentran lógica de negocio; adapters manejan acceso a BD.

- Evitar lógica de UI en services y lógica de negocio en views.













---



## 🎨 VISTAS Y UI



- Mantener paridad funcional con WebMatrix; no inventar flujos ni campos.

- Reutilizar parciales compartidos y estilos existentes.

- Priorizar UX First y flujos sin recarga completa.









---



## 📋 MENÚ Y NAVEGACIÓN



### Actualización de Sidebar



**Archivos a Modificar**:

- `Views/Shared/_Sidebar.cshtml` - Agregar entradas de menú

- `wwwroot/css/sidebar.css` - Estilos si es necesario



**Proceso**:

1. Identificar dónde va el módulo en la jerarquía

2. Agregar `<li>` con enlace correcto

3. Incluir iconos FontAwesome consistentes

4. Probar que los enlaces funcionen

5. Validar que sea accesible con permisos del usuario



---



## ✔️ TESTING Y VALIDACIÓN



### Checklist Pre-Commit

Antes de commitear c?digo, verificar:

- Compilaci?n sin errores
- 0 warnings cr?ticos (nullability aceptable)
- Todos los m?todos implementados
- Todos los SP ejecutados correctamente
- Modales funcionan
- B?squeda/filtros funcionan
- Paginaci?n funciona
- Permisos [Authorize] aplicados
- Logging en operaciones cr?ticas
- Manejo de excepciones completo
- DI registrado en Program.cs
- Men? actualizado en _Sidebar.cshtml
- Documentaci?n actualizada
- Sin archivos sin usar
- Sin TODO comentarios





### Requisitos de commit y auditoría de Sprint



Al finalizar cada sprint (o cuando existan cambios relevantes), es obligatorio realizar un commit y push con la siguiente información mínima:



- **Branch**: usar `feature/<modulo>-<descripcion>` o `hotfix/<descripcion>` según corresponda.

- **Commit**: mensaje estructurado siguiendo la plantilla de este repo (módulo: acción corta). Incluir referencia a ticket/issue si aplica.

- **Contenido mínimo**:

    - Código fuente modificado (Controllers, Services, Adapters, Views).

    - Scripts SQL modificados o evidencia de verificación (si aplica) y/o archivos `CO_Matrix_*` usados para validación.

    - Resultados de verificación frente a `MatrixNext/docs/OP/SQL/` (captura o CSV con resultados de comparación si se utilizó herramienta).

    - Actualización de `MIGRACION_[MODULO]_COMPLETADA.md` o `BACKLOG_MODULO_*` con observaciones y decisiones tomadas.

- **Push y PR**: subir branch remoto y crear Pull Request con descripción, checklist de aceptación y referencias a las reglas de migración.

- **Etiqueta en PR**: añadir etiqueta `sprint-complete` o `relevant-change` según corresponda.



No se acepta merge a main/master sin PR revisado y aprobado por el arquitecto o responsable del módulo.



Si durante el sprint se detectan discrepancias en nombres/tipos (BD vs CoreProject), documentarlas en la PR y en `MIGRACION_[MODULO]_COMPLETADA.md` y marcar la PR con `requires-dba` para atención del equipo DBA.



### Testing Funcional Mínimo



Para cada vista, probar:

1. Acceso: ¿Puedo acceder con [Authorize]?

2. Crear: ¿Puedo crear nuevo registro?

3. Editar: ¿Puedo editar existente?

4. Eliminar: ¿Puedo eliminar con confirmación?

5. Búsqueda: ¿Funcionan filtros?

6. Paginación: ¿Se pagina correctamente?

7. Modal: ¿Se abre y cierra?

8. Error: ¿Qué pasa si hay error en BD?



---



## 📖 DOCUMENTACIÓN



### Documentación Mínima Requerida



Por cada módulo migrado:



1. **ANALISIS_[MODULO].md** (conciso, enfocado en decisiones)

   - Descripción de módulo

   - Páginas a migrar

   - Procedimientos SQL

   - Flujos de negocio

   - Diagramas



2. **MIGRACION_[MODULO]_COMPLETADA.md**

   - Checklist de implementación

   - Componentes migrados

   - SP mapeados

   - Testing realizado



3. **Comentarios en Código**

   - Métodos complejos documentados

   - SP ejecutados documentados

   - Excepciones documentadas




**Principio: Documentar sin exceso**
- Documentar decisiones, mapeos y riesgos; evitar repetir código o descripciones obvias.
- Preferir listas cortas y referencias a archivos fuente en lugar de narrativas largas.
- Si un documento crece, resumir y enlazar anexos o evidencia.

---



## ⚠️ REGLAS ADICIONALES (AGREGADAS)



### REGLA 11: Validar Permisos de Usuario



**Descripción**: Siempre validar que el usuario autenticado tiene permisos para la acción.



**Aplicación**:

- Validar rol o pertenencia antes de permitir la acción.
- Si hay duda, bloquear y registrar.


---



### REGLA 12: Validar Datos de Entrada



**Descripción**: Siempre validar que los datos recibidos sean válidos antes de procesarlos.



**Aplicación**:

- Validar ModelState y reglas básicas de negocio antes de persistir.
- Rechazar valores vacíos o inconsistentes con mensaje claro.


---



### REGLA 13: Manejar Errores Gracefully



**Descripción**: Nunca retornar stack trace al cliente. Retornar mensajes amigables.



**Aplicación**:

- Log interno con contexto y responder mensaje genérico al usuario.
- No exponer stack trace ni detalles sensibles.


---



### REGLA 14: Usar Async/Await en Controllers



**Descripción**: Usar async/await para operaciones de I/O (BD, APIs externas).



**Aplicación**:

- Usar async/await en I/O; evitar bloqueos con .Result o .Wait().
- Mantener cancelación y timeouts cuando aplique.


---



### REGLA 15: Documentar Modificaciones en MODULOS_MIGRACION.md



**Descripción**: Mantener actualizado el documento maestro de migración con cada módulo completado.



**Aplicación**:

- Agregar estado ✅ COMPLETADO cuando termina módulo

- Especificar qué páginas se migraron

- Actualizar "Próximo a migrar"

- Incluir enlace a ANALISIS_[MODULO].md



---



## 🎯 RESUMEN DE REGLAS



| # | Regla | Prioridad | Aplicable |

|---|-------|-----------|-----------|

| 1 | Respetar nombres BD | 🔴 CRÍTICA | Siempre |

| 2 | Analizar SP y tablas en CoreProject | 🔴 CRÍTICA | Siempre |
| 2.1 | Prohibido inventar objetos de BD | ?? CR?TICA | Siempre |

| 3 | Usar EF para CRUD simple | 🟠 ALTA | Siempre |

| 4 | Ejecutar SP de WebMatrix | 🔴 CRÍTICA | Siempre |

| 5 | Preferir modales | 🟠 ALTA | UI |

| 6 | Agregar acciones existentes | 🔴 CRÍTICA | Features |

| 7 | Aprovechar componentes | 🟠 ALTA | UI |

| 8 | Priorizar detalle | 🔴 CRÍTICA | Proceso |

| 9 | Mantener áreas | 🟠 ALTA | Estructura |

| 10 | Crear menú de acceso | 🟠 ALTA | Navegación |

| 11 | Validar permisos | 🔴 CRÍTICA | Security |

| 12 | Validar entrada | 🔴 CRÍTICA | Data |

| 13 | Manejar errores | 🟠 ALTA | UX |

| 14 | Usar async/await | 🟠 ALTA | Performance |

| 15 | Documentar cambios | 🟠 ALTA | Tracking |



---



## 📝 CÓMO USAR ESTE DOCUMENTO



1. **Antes de migrar un módulo**: Leer todas las reglas

2. **Durante la migración**: Consultarlo como referencia

3. **Al completar**: Verificar contra checklist

4. **Para nuevos devs**: Es la guía de estándares



**Ubicación**: `MatrixNext/DIRECTRICES_MIGRACION.md`



**Actualizar cuando**:

- Se descubra nuevo patrón útil

- Se encuentre una regla inconsistente

- Se agregue nuevo estándar



---



**Documento Oficial**  

**Versión**: 1.0  

**Aprobado**: 2026-01-02  

**Revisión Siguiente**: Mensual o cuando se descubra inconsistencia



