# ANÁLISIS DEL MÓDULO IT (Infraestructura Tecnológica / Sincronización)

**Sprint**: 15  
**Prioridad**: 🟡 BAJA (módulo administrativo especializado)  
**Fecha Análisis**: 2025-01-14  
**Analista**: GitHub Copilot  
**Estado**: ✅ ANÁLISIS COMPLETADO

---

## 📊 RESUMEN EJECUTIVO

### Hallazgo Crítico ⚠️

**El módulo IT NO tiene stored procedures explícitos en la base de datos documentada**. 

La funcionalidad de sincronización está implementada mediante:
- **Entity Framework** (`GCEntities` - GestionCampo.edmx)
- **Function Imports** mapeados directamente al modelo EF
- **Operaciones en memoria** sobre entidades

**Decisión de migración**:
1. ✅ **Migrar funcionalidad** usando **EF Core** en lugar de Dapper
2. ✅ **Requiere análisis de esquema de BD** de gestión de campo
3. ⚠️ **NO crear stored procedures nuevos** (seguir patrón EF)

---

## 🎯 DESCRIPCIÓN DEL MÓDULO

### Propósito
Módulo administrativo especializado para **resolución de problemas de sincronización** de datos entre:
- Sistema Matrix (web)
- iField / Softsyn (aplicación de campo)
- Base de datos central

### Usuarios Objetivo
- **Administradores de TI** (permiso 133, 134)
- **Soporte técnico** especializado
- **Coordinadores de campo** con problemas de sync

### Alcance
**Módulo de bajo uso, alta criticidad cuando se necesita**. Funciones de:
- Ajustes de trabajos problemáticos
- Corrección de datos de encuestas
- Habilitación/deshabilitación de sincronización
- Gestión de encuestas piloto

---

## 📑 PÁGINAS IDENTIFICADAS

### 1. Default.aspx - Dashboard de Navegación
**Ruta WebMatrix**: `WebMatrix/IT/Default.aspx`  
**Tipo**: Página de navegación (slider/carousel)  
**Permiso**: 133

**Secciones del menú**:
1. **Synchronization**
   - Arreglar Problemas (`SyncIssues.aspx`)

2. **Centro Información** (links externos a otros módulos)
   - Almacenamiento en Disco
   - Solicitud de Medios
   - Consulta de Solicitudes
   - Recuperación Documentos

3. **Inventario** (link externo)
   - Registro de Artículos

4. **Usuarios** (link externo)
   - Gestión de Usuarios
   - Unidades
   - Permisos
   - Roles

**Decisión de migración**:
- ✅ Crear `ITController.Index()` con menú de navegación
- ✅ Integrar con módulos ya migrados (US_Usuarios cuando exista)
- ✅ Enlaces a Centro Información e Inventario pendientes (fuera de scope actual)

---

### 2. SyncIssues.aspx - Resolución de Problemas de Sincronización
**Ruta WebMatrix**: `WebMatrix/IT/SyncIssues.aspx`  
**Tipo**: Formulario de operaciones administrativas  
**Permiso**: 133  
**MasterPage**: `GD_F.master` (formularios genéricos)

**Acordeones (secciones)**:

#### Acordeón 0: Ajustar trabajos
**Campos**:
- `txtNumeroTrabajo` (int64) - ID del trabajo

**Acciones**:
1. **Quitar Preguntas Entrenamiento**
   - Botón: `btnQuitarEntrenamiento`
   - Función: `Sync.QuitarPreguntasEntrenamiento(trabajoId)`
   - SP/Función EF: `Sync_EncuestasEntrenamiento`
   - Propósito: Eliminar encuestas de entrenamiento asociadas a un trabajo

2. **Quitar Supervisión Estudio Especializado**
   - Botón: `btnSupervision`
   - Función: `Sync.ErrorTrabajoEspecializado(trabajoId)`
   - SP/Función EF: `Sync_ErrorTrabajoEspecializado`
   - Propósito: Desactivar supervisión en trabajos especializados con problemas

3. **Habilitar Sincronización**
   - Botón: `btnSincronizacion`
   - Función: `Sync.HabilitarSincronizacion(trabajoId)`
   - SP/Función EF: `Sync_HabilitarSincronizacionEstudio`
   - Propósito: Reactivar sincronización en trabajos bloqueados

#### Acordeón 1: Actualizar preguntas
**Campos**:
- `txtTrabajoId` (int64) - ID del trabajo
- `ddlPreguntas` (dropdown) - Pregunta a actualizar
- `txtSbjNum` (decimal) - Número de sujeto/encuesta
- `txtNewValor` (string) - Nuevo valor para la respuesta

**Acciones**:
1. **Mostrar preguntas**
   - Botón: `btnSearch`
   - Función: `Sync.PreguntasGet(trabajoId, null)`
   - SP/Función EF: `Sync_Preguntas_Get`
   - Retorna: Lista de preguntas (DCP_Descripcion, Pr_Nombre)

2. **Actualizar respuesta**
   - Botón: `btnActualizarValor`
   - Funciones:
     * `Sync.ActualizarPregunta(sbjNum, valor, dcp, trabajoId)` → `Sync_Preguntas_UpdateInfo`
     * `Sync.obtenerIdRegistroRespuestas(trabajoId, sbjNum)` → `obtenerRespuestaIdRegistroXIdTrabajoNumeroEncuesta`
     * `Sync.grabarAuditoria(...)` → `GrabarAuditoria`
   - Propósito: Corregir manualmente respuestas con problemas de sync
   - **Validación especial**: Si pregunta es `Res_Fecha`, convierte formato DD/MM/YYYY → MM/DD/YYYY

#### Acordeón 2: Habilitar encuesta piloto
**Campos**:
- `txtSbjNumPiloto` (decimal) - SbjNum de encuesta

**Acciones**:
1. **Habilitar**
   - Botón: `btnHabilitarPiloto`
   - Función: `Sync.HabilitarEncuestaPiloto(sbjNum)`
   - SP/Función EF: `Sync_HabilitarEncuestasPiloto`
   - Propósito: Reactivar encuesta piloto previamente deshabilitada

#### Acordeón 3: Encuesta piloto
**Campos**:
- `txtSbjNumPiloto2` (decimal) - SbjNum de encuesta

**Acciones**:
1. **Encuesta Piloto**
   - Botón: `btnEncuestaPiloto`
   - Función: `Sync.EncuestaPiloto(sbjNum)`
   - SP/Función EF: `Sync_EncuestaPiloto`
   - Propósito: Marcar encuesta como piloto

---

## 💾 DATOS Y STORED PROCEDURES

### ⚠️ HALLAZGO CRÍTICO: Sin SP Documentados

**Búsqueda en archivos SQL**:
```
MatrixNext/docs/SQL/CO_Matrix_SP_Names.csv
MatrixNext/docs/SQL/CO_Matrix_Structure_SP.sql
MatrixNext/docs/SQL/CO_Matrix_Structure_Tables.sql
```

**Resultado**: ❌ **NINGUNO de los procedimientos `Sync_*` está documentado**

**Único SP relacionado encontrado**:
- `OP_ProcessSyncFromIField_SP_Process` (módulo OP, no IT)

### Implementación Real en CoreProject

**Archivo**: `CoreProject/Clases/GestionCampo/Sync.vb`  
**Patrón**: **Entity Framework con Function Imports**

```vb
Private oMatrixContext As GCEntities  ' Entity Framework Context

' Función EF mapeada
Public Function PreguntasGet(ByVal TrabajoId As Int64?, SubjNum As Decimal?) As List(Of Sync_Preguntas_Get_Result)
    Return oMatrixContext.Sync_Preguntas_Get(TrabajoId, SubjNum).ToList
End Function

' Función EF mapeada
Public Sub QuitarPreguntasEntrenamiento(ByVal TrabajoId As Int64)
    oMatrixContext.Sync_EncuestasEntrenamiento(TrabajoId)
End Sub

' ... etc
```

**Interpretación**:
- Las funciones `Sync_*` son **Function Imports** de EF mapeados a:
  - Stored Procedures no documentados, O
  - Funciones de tabla (TVF), O
  - Operaciones EF complejas

### EDMX Identificado
**Archivo**: `CoreProject/GestionCampo.edmx`  
**Contexto**: `GCEntities`

**Próximos pasos** (para implementación):
1. Abrir `GestionCampo.edmx` y revisar Function Imports
2. Identificar entidades relacionadas (Respuestas, Trabajos, Encuestas)
3. Mapear a EF Core con Fluent API

---

## 🎯 MAPEO DE FUNCIONES (CoreProject → MatrixNext)

| Acción WebMatrix | Función CoreProject | Función EF | Tipo | Estrategia MatrixNext |
|------------------|---------------------|------------|------|------------------------|
| **Quitar Entrenamiento** | `Sync.QuitarPreguntasEntrenamiento()` | `Sync_EncuestasEntrenamiento` | Function Import | EF Core Query |
| **Quitar Supervisión** | `Sync.ErrorTrabajoEspecializado()` | `Sync_ErrorTrabajoEspecializado` | Function Import | EF Core Query |
| **Habilitar Sync** | `Sync.HabilitarSincronizacion()` | `Sync_HabilitarSincronizacionEstudio` | Function Import | EF Core Query |
| **Listar Preguntas** | `Sync.PreguntasGet()` | `Sync_Preguntas_Get` | Function Import | EF Core Query |
| **Actualizar Pregunta** | `Sync.ActualizarPregunta()` | `Sync_Preguntas_UpdateInfo` | Function Import | EF Core Query |
| **Habilitar Piloto** | `Sync.HabilitarEncuestaPiloto()` | `Sync_HabilitarEncuestasPiloto` | Function Import | EF Core Query |
| **Marcar Piloto** | `Sync.EncuestaPiloto()` | `Sync_EncuestaPiloto` | Function Import | EF Core Query |
| **Obtener ID Registro** | `Sync.obtenerIdRegistroRespuestas()` | `obtenerRespuestaIdRegistroXIdTrabajoNumeroEncuesta` | Function Import | EF Core Query |
| **Grabar Auditoría** | `Sync.grabarAuditoria()` | `GrabarAuditoria` | Function Import | EF Core Query |

---

## 🏗️ ARQUITECTURA PROPUESTA (MatrixNext)

### ⚠️ DECISIÓN DE DISEÑO: Usar EF Core, NO Dapper

**Justificación**:
1. ✅ No hay stored procedures documentados
2. ✅ Lógica actual usa EF (no hay SQL raw)
3. ✅ Operaciones complejas sobre entidades (no simples CRUD)
4. ✅ Auditoría integrada
5. ✅ Bajo volumen de uso (no hay concerns de performance)

### Estructura de archivos

```
MatrixNext.Data/
├── Context/
│   └── ITDbContext.cs                    ← EF Core Context para IT (separado o integrado en MatrixDbContext)
├── Entities/IT/
│   ├── Trabajo.cs                        ← Entidad Trabajo (campos de sync)
│   ├── Respuesta.cs                      ← Entidad Respuestas
│   ├── Pregunta.cs                       ← Entidad Preguntas
│   ├── TipoResultadoVerificacion.cs      ← Entidad tipos
│   └── Auditoria.cs                      ← Entidad auditoría
├── DTOs/IT/
│   ├── SyncTrabajoDto.cs                 ← DTO para operaciones de trabajo
│   ├── SyncPreguntaDto.cs                ← DTO para preguntas
│   └── SyncRespuestaUpdateDto.cs         ← DTO para actualizar respuestas

MatrixNext.Infrastructure/
├── Repositories/IT/
│   └── ITSyncRepository.cs               ← Repository EF Core para operaciones Sync

MatrixNext.Core/
├── Services/IT/
│   └── ITSyncService.cs                  ← Lógica de negocio Sync

MatrixNext.Web/
├── Areas/IT/
│   ├── Controllers/
│   │   ├── ITController.cs               ← Dashboard (Default.aspx)
│   │   └── SyncIssuesController.cs       ← Operaciones Sync (SyncIssues.aspx)
│   └── Views/
│       ├── IT/
│       │   └── Index.cshtml              ← Dashboard navegación
│       └── SyncIssues/
│           └── Index.cshtml              ← Formulario acordeón
```

### Patrón de implementación

```
HTTP Request
    ↓
[Controller]  ← Recibe, valida, coordina, retorna View/JSON
    ↓
[Service]     ← Validaciones, lógica de negocio, transformaciones
    ↓
[Repository]  ← EF Core queries, SaveChanges
    ↓
[DbContext]   ← EF Core tracked entities
    ↓
[Database]    ← SQL Server (tablas de GestionCampo)
```

---

## 📋 ESTIMACIÓN DE COMPONENTES

### Archivos a crear

| Componente | Cantidad | Estimación LOC | Complejidad |
|------------|----------|----------------|-------------|
| **Entities** | 5 | ~400 | 🟡 MEDIA (mapeo EF Core) |
| **DTOs** | 3 | ~150 | 🟢 BAJA |
| **Repository** | 1 | ~400 | 🟠 ALTA (queries EF complejas) |
| **Service** | 1 | ~500 | 🟠 ALTA (validaciones + auditoría) |
| **Controllers** | 2 | ~300 | 🟢 BAJA |
| **Views** | 2 | ~400 | 🟡 MEDIA (acordeón UI) |
| **DI/Config** | - | ~50 | 🟢 BAJA |
| **TOTAL** | **14 archivos** | **~2,200 LOC** | 🟠 **ALTA** (EF mapping) |

### Comparación con otros sprints

| Sprint | Archivos | LOC | Patrón | Complejidad |
|--------|----------|-----|--------|-------------|
| ES_Estadistica | 22 | ~5,000 | Dapper + SP | 🟡 MEDIA |
| **IT Sync** | **14** | **~2,200** | **EF Core** | **🟠 ALTA** |

**Menos archivos pero mayor complejidad técnica** debido a:
- Mapeo de entidades sin SP documentados
- Lógica de auditoría integrada
- Conversión de Function Imports a EF Core
- Validaciones complejas (fechas, formato)

---

## ⚠️ RIESGOS Y CONSIDERACIONES

### 1. Stored Procedures No Documentados
**Riesgo**: 🔴 ALTO  
**Descripción**: Los SP `Sync_*` no están en `CO_Matrix_Structure_SP.sql`  
**Mitigación**:
1. ✅ Ejecutar en staging: `SELECT * FROM sys.procedures WHERE name LIKE 'Sync_%'`
2. ✅ Revisar `GestionCampo.edmx` para ver definiciones de Function Imports
3. ✅ Si no existen SP, implementar lógica directa en EF Core
4. ✅ Testing exhaustivo en staging antes de producción

### 2. Lógica de Auditoría
**Riesgo**: 🟡 MEDIO  
**Descripción**: `GrabarAuditoria` con enums específicos (`ETipoAccion`, `EModulo`, `ETabla`)  
**Mitigación**:
1. ✅ Mapear enums a constantes o enums C#
2. ✅ Crear tabla de auditoría si no existe
3. ✅ Implementar logging adicional con Serilog

### 3. Formato de Fechas
**Riesgo**: 🟡 MEDIO  
**Descripción**: Conversión manual DD/MM/YYYY → MM/DD/YYYY para `Res_Fecha`  
**Mitigación**:
1. ✅ Usar `DateTime.Parse` con `CultureInfo.InvariantCulture`
2. ✅ Validar formato con regex antes de parsear
3. ✅ Logging de errores de formato

### 4. Permisos Especiales
**Riesgo**: 🟢 BAJO  
**Descripción**: Requiere permisos 133 y 134 (admin IT)  
**Mitigación**:
1. ✅ Validar permisos con `[Authorize]` + custom policy
2. ✅ Integrar con sistema de permisos existente

---

## 🧪 TESTING REQUERIDO

### Pre-implementación (análisis de BD)
- [ ] Conectar a staging y ejecutar:
  ```sql
  SELECT name, type_desc 
  FROM sys.procedures 
  WHERE name LIKE 'Sync_%' OR name LIKE '%Sync%'
  ORDER BY name;
  ```
- [ ] Verificar existencia de tablas: `Respuestas`, `Preguntas`, `Trabajos`, `Auditoria`
- [ ] Revisar `GestionCampo.edmx` para entender esquema
- [ ] Documentar hallazgos en este archivo

### Post-implementación
- [ ] Crear trabajo de prueba y ejecutar operaciones Sync
- [ ] Validar que auditoría se graba correctamente
- [ ] Probar actualización de respuesta con formato fecha
- [ ] Validar permisos 133/134
- [ ] Verificar que acordeones funcionan (UI)
- [ ] Testing end-to-end con usuario administrador real

---

## 📖 DOCUMENTACIÓN RELACIONADA

### WebMatrix (origen)
- `WebMatrix/IT/Default.aspx` - Dashboard navegación
- `WebMatrix/IT/SyncIssues.aspx` - Formulario operaciones Sync

### CoreProject (lógica actual)
- `CoreProject/Clases/GestionCampo/Sync.vb` - Clase principal
- `CoreProject/Sync_Preguntas_Get_Result.vb` - DTO resultado
- `CoreProject/GestionCampo.edmx` - Entity Framework model

### Archivos SQL (consultados)
- ❌ `MatrixNext/docs/SQL/CO_Matrix_SP_Names.csv` - No contiene `Sync_*`
- ❌ `MatrixNext/docs/SQL/CO_Matrix_Structure_SP.sql` - No contiene `Sync_*`
- ⚠️ `MatrixNext/docs/SQL/CO_Matrix_Structure_Tables.sql` - Pendiente verificar tablas

---

## 🎯 DECISIÓN FINAL

### ✅ PROCEDER CON MIGRACIÓN

**Condiciones**:
1. ✅ **Prioridad BAJA**: No bloquea otros módulos críticos
2. ✅ **Patrón claro**: EF Core en lugar de Dapper
3. ✅ **Alcance definido**: 2 páginas, funcionalidad administrativa
4. ⚠️ **Requiere análisis adicional**: Revisar BD staging antes de codificar

### 🔄 PRÓXIMOS PASOS (en orden)

1. **Análisis de Base de Datos** (CRÍTICO antes de codificar)
   - Conectar a staging
   - Verificar SP `Sync_*` (existen o no)
   - Identificar tablas relacionadas
   - Documentar esquema real

2. **Mapeo de Entidades**
   - Crear `Entities/IT/` basado en esquema real
   - Configurar `ITDbContext` con Fluent API
   - Mapear relaciones entre entidades

3. **Implementación (siguiendo orden estándar)**
   - DTOs → Repository → Service → Controllers → Views → DI

4. **Testing exhaustivo** (crítico por ser operaciones administrativas)

5. **Documentación de cierre** (`MIGRACION_IT_COMPLETADA.md`)

---

## 📝 NOTAS ADICIONALES

### Por qué prioridad BAJA
- Módulo de uso administrativo/soporte (no operacional diario)
- Usuarios limitados (solo admins IT)
- No bloquea funcionalidad de otros módulos
- Requiere análisis técnico adicional (complejidad alta)

### Recomendación
**Postponer IT para Sprint posterior** y priorizar módulos con:
- SP documentados ✅
- Mayor volumen de usuarios ✅
- Funcionalidad operacional crítica ✅

Sugerencias de módulos alternativos:
- **FI** (Finanzas) - CRUD tradicional, SP documentados
- **CI** (Centro Información) - Gestión documental
- **PY** (Proyectos) - Core business

---

**Documento creado**: 2025-01-14  
**Próxima actualización**: Post análisis de BD staging  
**Estado**: ⏸️ PAUSA RECOMENDADA - Priorizar otros módulos primero
