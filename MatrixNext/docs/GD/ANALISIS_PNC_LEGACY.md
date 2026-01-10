# ANÁLISIS SISTEMA LEGACY - PNC (PRODUCTO NO CONFORME)

**Fecha Análisis:** 2026-01-10  
**Sistema:** Gestión de Calidad ISO 9001  
**Framework:** WebForms VB.NET  
**Estado:** ✅ ANÁLISIS COMPLETADO

---

## 📋 Resumen Ejecutivo

**PNC = Producto No Conforme** es el sistema de gestión de calidad ISO 9001 para registrar y hacer seguimiento a productos/servicios que NO cumplen especificaciones de calidad.

### Propósito del Sistema

- ✅ Registrar reclamos de clientes (internos/externos) sobre no conformidades
- ✅ Identificar causas raíz mediante análisis (5 porqués, Ishikawa, etc.)
- ✅ Implementar plan de acciones correctivas (inmediatas, correctivas, preventivas)
- ✅ Hacer seguimiento hasta cierre del PNC
- ✅ Generar reportes para auditorías ISO 9001

---

## 🗂️ Estructura de Base de Datos

### Tablas Principales (Sistema Original)

#### 1. PNC_ProductoNoConforme (Maestro)

```sql
CREATE TABLE [dbo].[PNC_ProductoNoConforme](
    [Id] [int] IDENTITY(1,1) NOT NULL PRIMARY KEY,
    [IdEstudio] [int] NULL,
    [IdTrabajo] [int] NULL,
    [JobBook] [nchar](15) NULL,
    [FechaReclamo] [date] NULL,
    [IdReporta] [bigint] NULL,
    [IdUnidad] [int] NULL,
    [IdClienteExterno] [bigint] NULL,
    [FuenteReclamo] [int] NULL,           -- FK a PNC_FuenteReclamo
    [Categoria] [int] NULL,                -- FK a PNC_Categorias
    [Tarea] [int] NULL,
    [Descripcion] [varchar](max) NULL,
    [Cerrado] [bit] NULL,
    [FechaCierre] [date] NULL,
    [Usuario] [bigint] NULL,
    [FechaGrabacion] [date] NULL,
    [FechaActualizacion] [date] NULL
)
```

**Campos Clave:**
- `JobBook`: Asociación al estudio/proyecto
- `FuenteReclamo`: Cliente externo, cliente interno, auditoría, etc.
- `Categoria`: Tipo de no conformidad por unidad/rol
- `Cerrado`: Estado del PNC (abierto/cerrado)

#### 2. PNC_ProductoNoConformeCausas

```sql
CREATE TABLE [dbo].[PNC_ProductoNoConformeCausas](
    [Id] [int] IDENTITY(1,1) NOT NULL PRIMARY KEY,
    [IdPNC] [int] NULL,                   -- FK a PNC_ProductoNoConforme
    [CausaRaiz] [varchar](max) NULL
)
```

**Relación:** 1 PNC → N Causas

#### 3. PNC_ProductoNoConformeAcciones

```sql
CREATE TABLE [dbo].[PNC_ProductoNoConformeAcciones](
    [Id] [int] IDENTITY(1,1) NOT NULL PRIMARY KEY,
    [IdPNC] [int] NULL,                   -- FK a PNC_ProductoNoConforme
    [IdCausa] [int] NULL,                 -- FK a PNC_ProductoNoConformeCausas
    [TipoAccion] [int] NULL,              -- 1=Inmediata, 2=Correctiva, 3=Preventiva
    [Accion] [varchar](max) NULL,
    [FechaPlaneada] [date] NULL,
    [FechaEjecucion] [date] NULL,
    [IdResponsableAccion] [int] NULL,
    [IdResponsableSeguimiento] [int] NULL,
    [EvidenciaCierre] [varchar](max) NULL,
    [PermiteActualizar] [bit] NULL
)
```

**Relación:** 1 Causa → N Acciones

**Tipos de Acción:**
- **1 = Inmediata:** Corregir el problema de inmediato
- **2 = Correctiva:** Evitar recurrencia del problema
- **3 = Preventiva:** Prevenir problemas similares

### Tablas Catálogos

#### 4. PNC_Categorias

```sql
CREATE TABLE [dbo].[PNC_Categorias](
    [Id] [int] NOT NULL PRIMARY KEY,
    [Descripcion] [varchar](72) NULL,
    [IdUnidad] [int] NULL,
    [IdRol] [int] NULL
)
```

**Ejemplos:**
- Categoría por Unidad (Operaciones, Data Entry, etc.)
- Categoría por Rol (Supervisor, Coordinador, etc.)

#### 5. PNC_FuenteReclamo

```sql
CREATE TABLE [dbo].[PNC_FuenteReclamo](
    [Id] [int] NOT NULL PRIMARY KEY,
    [Descripcion] [nchar](30) NULL
)
```

**Valores Típicos:**
- Cliente Externo
- Cliente Interno
- Auditoría Interna
- Auditoría Externa
- Queja/Reclamo

#### 6. PNC_TiposDeAccion

```sql
CREATE TABLE [dbo].[PNC_TiposDeAccion](
    [Id] [int] NOT NULL PRIMARY KEY,
    [Accion] [nchar](20) NULL
)
```

**Valores:**
- 1: Inmediata
- 2: Correctiva
- 3: Preventiva

#### 7. PNC_Procedimientos

```sql
CREATE TABLE [dbo].[PNC_Procedimientos](
    [id] [tinyint] IDENTITY(1,1) NOT NULL PRIMARY KEY,
    [Descripcion] [varchar](50) NOT NULL
)
```

#### 8. PNC_Procesos

```sql
CREATE TABLE [dbo].[PNC_Procesos](
    [id] [tinyint] IDENTITY(1,1) NOT NULL PRIMARY KEY,
    [Descripcion] [varchar](50) NOT NULL
)
```

### Sistema Alternativo (PNC_Productos)

**Nota:** Existe un sistema más avanzado con tablas `PNC_Productos_*` pero decidimos migrar el sistema original por ser más usado.

```sql
PNC_Productos                  -- Maestro avanzado (con campos adicionales)
PNC_Productos_Causas          -- Causas con corrección
PNC_Productos_Estados         -- Estados del workflow
PNC_Productos_Log             -- Log de cambios de estado
```

---

## 📝 Stored Procedures (16 SPs)

### Sistema Original (PNC_ProductoNoConforme)

#### Listados

```sql
-- Obtener todos los PNC
PNC_ObtenerProductoNoConformeTodos()

-- Obtener PNC por JobBook
PNC_ObtenerProductoNoConforme(@JobBook varchar(20))

-- Obtener PNC por ID
PNC_GetById(@Id int)

-- Obtener causas de un PNC
PNC_ProductoNoConformeCausas_Get(@IdPNC int)

-- Obtener acciones de una causa
PNC_ProductoNoConformeAcciones_Get(@IdPNC int, @IdCausa int)

-- Obtener causas con detalle
PNC_Causa_Get(@IdPNC int)
```

#### Notificaciones

```sql
-- Email recordatorio de acciones pendientes
PNC_EmailAcciones

-- Email notificación de nuevo PNC
PNC_EmailNotificacionReporte
```

### Sistema Avanzado (PNC_Productos)

```sql
-- Insert nuevo producto
PNC_Productos_Add(...)

-- Listado productos
PNC_Productos_Get(...)

-- Insert causa
PNC_Productos_Causas_Add(...)

-- Correos a notificar
PNC_Productos_CorreosNotificar(@ProductoId)

-- Actualizar estado
PNC_Producto_UpdateEstado(@Id, @Estado, @Usuario)

-- Insert log
PNC_Productos_Log_Estado_Add(...)

-- Obtener log
PNC_Productos_Log_Get(@ProductoId)

-- Seguimiento general
PNC_Seguimiento_Get(...)
```

---

## 💻 Código VB.NET Legacy

### PNCClass.vb (262 líneas)

**Ubicación:** `CoreProject/Clases/PNC/PNCClass.vb`

#### Estructura

```vb
Public Class PNCClass
    Enum EEstados
        enviado = 1
        actualizado = 2
        anulado = 3
        eliminado = 4
        aceptado = 5
        rechazado = 6
        causaRegistrada = 7
    End Enum

    Private oMatrixContext As PNCEntities
    Private oMatrixContextU As US_Entities
    Private oMatrixContextC As CU_Entities
    Private oMatrixContextP As PY_Entities
End Class
```

#### Métodos Principales

**Listados:**
```vb
Public Function LstPNCTodos() As List(Of PNC_ObtenerProductoNoConformeTodos_Result)
    Return oMatrixContext.PNC_ObtenerProductoNoConformeTodos().ToList
End Function

Public Function LstPNC(VJobBook As String) As List(Of PNC_ObtenerProductoNoConforme_Result)
    Return oMatrixContext.PNC_ObtenerProductoNoConforme(VJobBook).ToList
End Function

Public Function LstPNCCausas(VIdPNC As Integer?) As List(Of PNC_VerProductoNoConformeCausas)
    Return (From t In oMatrixContext.PNC_VerProductoNoConformeCausas 
            Where t.IdPNC = VIdPNC Select t).ToList
End Function

Public Function LstPNCAcciones(VIdPNC As Integer, VIdCausa As Integer) 
    As List(Of PNC_VerProductoNoConformeDetalle)
    Return (From t In oMatrixContext.PNC_VerProductoNoConformeDetalle 
            Where t.IdPNC = VIdPNC And t.IdCausa = VIdCausa Select t).ToList
End Function
```

**Catálogos:**
```vb
Public Function LstFuente() As List(Of PNC_FuenteReclamo)
    Return oMatrixContext.PNC_FuenteReclamo.ToList
End Function

Public Function LstCategoria() As List(Of PNC_Categorias)
    Return oMatrixContext.PNC_Categorias.ToList
End Function

Public Function LstTipoAccion() As List(Of PNC_TiposDeAccion)
    Return oMatrixContext.PNC_TiposDeAccion.ToList
End Function

Public Function LstUsuarios() As List(Of PNC_VObtenerUsuarios)
    Return (From u In oMatrixContext.PNC_VObtenerUsuarios 
            Order By u.Nombre Select u).ToList
End Function
```

**CRUD PNC:**
```vb
Public Function GrabarRegistroPNC(
    WIdEstudio, WIdTrabajo, WJobBook, WFechaReclamo, 
    WReporta, WUnidad, WCliente, WFuente, WCategoria, 
    WTarea, WDescripcion, WCerrado, WFechaCierre, 
    WUsuario, WFechaGrabacion, WFechaActualizacion
) As Int64
    Dim PNCRegistro = New PNC_ProductoNoConforme()
    
    PNCRegistro.IdEstudio = CInt(WIdEstudio)
    PNCRegistro.IdTrabajo = WIdTrabajo
    PNCRegistro.JobBook = WJobBook
    ' ... asignación de campos
    
    oMatrixContext.PNC_ProductoNoConforme.Add(PNCRegistro)
    oMatrixContext.SaveChanges()
    Return PNCRegistro.Id
End Function
```

**Validaciones:**
```vb
Public Function ExisteAccion(WIdpNC As Integer, WIdCausa As Integer, WTipoAccion As Integer) As Boolean
    Dim VAccion = (From a In oMatrixContext.PNC_ProductoNoConformeAcciones 
                   Where a.IdPNC = WIdpNC And a.IdCausa = WIdCausa And a.TipoAccion = WTipoAccion 
                   Select a.TipoAccion).ToList
    Return VAccion.Count > 0
End Function

Public Function ExisteAccionInmediata(WIdpNC As Integer, WIdCausa As Integer) As Boolean
    ' Validar que existe al menos una acción inmediata (TipoAccion=1)
    Return ExisteAccion(WIdpNC, WIdCausa, 1)
End Function
```

**Obtener Información Relacionada:**
```vb
Public Function ObtenerNombreEstudio(VJobBook As String) As List(Of CU_Estudios)
    Return (From c In oMatrixContextC.CU_Estudios 
            Where c.JobBook.Contains(VJobBook.Substring(1, 9)) 
            Select c).ToList
End Function

Public Function ObtenerNombreUnidad(IdEstudio As Int32) As String
    Return (From u In oMatrixContext.PNC_VObtenerUnidad 
            Where u.id = IdEstudio 
            Select u.NombreUnidad).FirstOrDefault()
End Function

Public Function ObtenerNombreCliente(IdEstudio As Integer) As String
    Return (From t In oMatrixContext.PNC_VObtenerCliente 
            Where t.IdEstudio = IdEstudio 
            Select t.RazonSocial).FirstOrDefault()
End Function
```

---

## 🌐 Páginas WebForms

### 1. ProductoNoConformeRegistrar.aspx

**Ubicaciones:**
- `/GD_Documentos/ProductoNoConformeRegistrar.aspx`
- `/MBO/ProductoNoConformeRegistrar.aspx`
- `/TH_TalentoHumano/` (referenciado en menú)

**Funcionalidad:**
- Registro de nuevo PNC
- Asociación a JobBook/Estudio
- Selección Fuente Reclamo, Categoría
- Agregar Causas
- Agregar Acciones por causa
- Notificación email a responsables

### 2. ProductosNoConformeRelacion.aspx

**Ubicación:** `/GD_Documentos/ProductosNoConformeRelacion.aspx`

**Funcionalidad:**
- Listado de todos los PNC
- Filtros: JobBook, Fecha, Estado (Abierto/Cerrado)
- Grid con PNC Id, JobBook, Estudio, Fecha, Fuente, Categoría
- Link a detalle PNC

### 3. GD_SeguimientoPNC.aspx

**Ubicación:** `/GD_Documentos/GD_SeguimientoPNC.aspx`

**Funcionalidad:**
- Ver detalle completo PNC
- Ver todas las causas
- Ver todas las acciones por causa
- Actualizar estado acciones
- Registrar evidencias de cierre
- Cerrar PNC cuando todas las acciones están completadas

---

## 🔄 Workflow del Sistema

### Flujo Completo

```
1. REGISTRO PNC
   Usuario reporta no conformidad
   ↓
   Asocia a JobBook/Estudio
   ↓
   Selecciona Fuente (cliente externo/interno/auditoría)
   ↓
   Selecciona Categoría (por unidad/rol)
   ↓
   Describe el problema
   ↓
   Sistema asigna ID PNC
   ↓
   Email notificación a responsables

2. ANÁLISIS CAUSAS
   Responsable identifica causas raíz
   ↓
   Registra causas (pueden ser múltiples)
   ↓
   Por cada causa define:
     - Causa raíz (descripción)

3. PLAN DE ACCIONES
   Por cada causa registra acciones:
   ↓
   Acción Inmediata (OBLIGATORIA)
     - Qué se hará de inmediato
     - Fecha planeada
     - Responsable acción
     - Responsable seguimiento
   ↓
   Acción Correctiva
     - Para evitar recurrencia
   ↓
   Acción Preventiva
     - Para prevenir similares

4. SEGUIMIENTO
   Sistema envía recordatorios email
   ↓
   Responsables ejecutan acciones
   ↓
   Registran fecha ejecución
   ↓
   Cargan evidencias de cierre

5. CIERRE PNC
   Validación: Todas las causas tienen acciones
   ↓
   Validación: Todas las acciones ejecutadas
   ↓
   Sistema marca PNC como Cerrado
   ↓
   Fecha de cierre = hoy
```

### Estados del PNC

**Enum en PNCClass.vb:**
```vb
Enum EEstados
    enviado = 1              ' PNC enviado/creado
    actualizado = 2          ' PNC actualizado
    anulado = 3             ' PNC anulado
    eliminado = 4           ' PNC eliminado
    aceptado = 5            ' PNC aceptado
    rechazado = 6           ' PNC rechazado
    causaRegistrada = 7     ' Causa registrada
End Enum
```

---

## 📊 Relaciones entre Entidades

```
CU_Estudios (1) ←────────── (N) PNC_ProductoNoConforme
                                       ↓
                                       | (1)
                                       ↓
                                (N) PNC_ProductoNoConformeCausas
                                       ↓
                                       | (1)
                                       ↓
                                (N) PNC_ProductoNoConformeAcciones

PY_Trabajo (1) ←────────── (N) PNC_ProductoNoConforme

US_Usuarios (1) ←────────── (N) PNC_ProductoNoConforme (IdReporta)
US_Usuarios (1) ←────────── (N) PNC_ProductoNoConformeAcciones (IdResponsableAccion)
US_Usuarios (1) ←────────── (N) PNC_ProductoNoConformeAcciones (IdResponsableSeguimiento)

PNC_FuenteReclamo (1) ←──── (N) PNC_ProductoNoConforme
PNC_Categorias (1) ←──────── (N) PNC_ProductoNoConforme
PNC_TiposDeAccion (1) ←───── (N) PNC_ProductoNoConformeAcciones
```

---

## 🔍 Reglas de Negocio

### Validaciones Críticas

1. **Acción Inmediata Obligatoria**
   - Cada causa DEBE tener al menos 1 acción de tipo "Inmediata" (TipoAccion=1)
   - Validado en `ExisteAccionInmediata()`

2. **JobBook Requerido**
   - Todo PNC debe estar asociado a un JobBook/Estudio
   - Lookup a `CU_Estudios`

3. **Fecha Reclamo**
   - No puede ser futura
   - FechaReclamo <= DateTime.Now

4. **Cierre PNC**
   - Solo se puede cerrar si:
     - Tiene al menos 1 causa
     - Cada causa tiene al menos 1 acción
     - Todas las acciones tienen FechaEjecucion
   - Al cerrar: Cerrado=true, FechaCierre=hoy

5. **Permisos**
   - Reporta: Puede crear PNC
   - Responsable Acción: Puede ejecutar acción
   - Responsable Seguimiento: Puede cerrar acción

---

## 📧 Notificaciones Email

### Eventos que Generan Email

1. **Nuevo PNC Registrado**
   - SP: `PNC_EmailNotificacionReporte`
   - A: IdReporta (quien reporta)
   - CC: Responsables de la categoría

2. **Acción Próxima a Vencer**
   - SP: `PNC_EmailAcciones`
   - Cuando: FechaPlaneada - 3 días < hoy
   - A: IdResponsableAccion, IdResponsableSeguimiento

3. **Acción Vencida**
   - SP: `PNC_EmailAcciones`
   - Cuando: FechaPlaneada < hoy AND FechaEjecucion IS NULL
   - A: IdResponsableAccion, IdResponsableSeguimiento

---

## 🎯 ViewModels a Crear

### Maestro

1. **ProductoNoConformeVM** - Registro/Edición PNC
2. **ProductoNoConformeDetalleVM** - Vista completa (maestro + causas + acciones)
3. **ProductoNoConformeListadoVM** - Grid de resultados

### Causas/Acciones

4. **ProductoNoConformeCausaVM** - Causa individual
5. **ProductoNoConformeAccionVM** - Acción individual
6. **AccionDetalleVM** - Acción con info relacionada

### Catálogos

7. **PncCategoriaVM**
8. **PncFuenteReclamoVM**
9. **PncTipoAccionVM**
10. **PncProcesoVM**
11. **PncProcedimientoVM**

### Workflow

12. **PncSeguimientoVM** - Vista seguimiento
13. **PncLogEstadoVM** - Historial cambios
14. **PncNotificacionVM** - Datos para emails

### Búsqueda

15. **PncFiltrosVM** - Filtros de búsqueda

### DTOs para SPs

16. **PncObtenerProductoNoConformeDTO** - Resultado SP listado
17. **PncVerCausasDTO** - Resultado SP causas
18. **PncVerAccionesDTO** - Resultado SP acciones
19. **PncCorreosNotificarDTO** - Emails a notificar

---

## 📝 Mapeo ViewModels vs Entity

| ViewModel | Tabla/SP | Uso |
|-----------|----------|-----|
| ProductoNoConformeVM | PNC_ProductoNoConforme | CRUD maestro |
| ProductoNoConformeCausaVM | PNC_ProductoNoConformeCausas | CRUD causas |
| ProductoNoConformeAccionVM | PNC_ProductoNoConformeAcciones | CRUD acciones |
| PncObtenerProductoNoConformeDTO | PNC_ObtenerProductoNoConforme SP | Listado con joins |
| ProductoNoConformeDetalleVM | PNC_GetById SP | Vista completa |
| PncCategoriaVM | PNC_Categorias | Catálogo |
| PncFuenteReclamoVM | PNC_FuenteReclamo | Catálogo |
| PncTipoAccionVM | PNC_TiposDeAccion | Catálogo |

---

## 🛠️ Tareas de Implementación

### Tarea 8.1 ✅ COMPLETADA
- Análisis completo legacy
- Documentación 12 tablas
- Documentación 16 SPs
- Análisis PNCClass.vb
- Identificación 3 páginas WebForms

### Tarea 8.2 - ViewModels (6h)
- Crear 20+ ViewModels
- DataAnnotations validaciones
- Navegación (NombreEstudio, NombreReporta, etc.)
- Enums (TipoAccionEnum)

### Tarea 8.3 - Adapter (8h)
- Interface IPncAdapter
- Implementación con Dapper
- Mapeo 16 SPs
- CRUD transaccional

### Tarea 8.4 - Service (6h)
- Interface IPncService
- Validaciones negocio
- Orquestación transacciones
- Integración IEmailQueueService

### Tarea 8.5 - Controller (6h)
- PncController completo
- 10+ endpoints
- Validaciones ModelState
- Autorización

### Tarea 8.6 - Vistas (8h)
- Index.cshtml (listado)
- Detalle.cshtml (completo)
- Crear.cshtml (registro)
- AgregarCausa.cshtml
- AgregarAccion.cshtml
- Seguimiento.cshtml

---

## ✅ Criterios de Aceptación

### Funcional
- [x] Sistema documentado completamente
- [ ] Usuario puede registrar PNC
- [ ] Usuario puede agregar causas
- [ ] Usuario puede agregar acciones
- [ ] Sistema valida acción inmediata obligatoria
- [ ] Usuario puede cerrar PNC
- [ ] Emails enviados en eventos clave

### Técnico
- [x] 12 tablas mapeadas
- [x] 16 SPs documentados
- [x] PNCClass.vb analizado
- [x] 3 páginas WebForms identificadas
- [ ] 20+ ViewModels creados
- [ ] Adapter con Dapper
- [ ] Service con validaciones
- [ ] Controller con endpoints
- [ ] 6 vistas Razor

---

**Generado:** 2026-01-10  
**Última Actualización:** 2026-01-10  
**Estado:** ✅ ANÁLISIS COMPLETADO - LISTO PARA IMPLEMENTACIÓN
