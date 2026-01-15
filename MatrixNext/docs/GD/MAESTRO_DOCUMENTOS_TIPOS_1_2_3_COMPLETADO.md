# Sprint 12.3.5: Maestro - Tipos 2 y 3 (+ Tipo 1)

**Ref**: BACKLOG_QA_MODULOS_PENDIENTES.md § Sprint 12.3.5  
**Duración**: 12h (completado)  
**Estado**: ✅ COMPLETADO  

---

## 📋 Descripción

Implementación de maestro de documentos con soporte para 3 tipos de solicitudes: Tipo 1 (Construcción), Tipo 2 (Actualización/Versionamiento), Tipo 3 (Anulación). Incluye lógica condicional, creación de documentos controlados y auditoría de cambios.

---

## 🎯 Objetivos Alcanzados

✅ **DTOs** (MaestroDocumentoDto.cs - 150 líneas):
- MaestroDocumentoDto (13 propiedades base)
- MaestroTipo1ConstruccionDto (3 propiedades adicionales)
- MaestroTipo2ActualizacionDto (5 propiedades adicionales)
- MaestroTipo3AnulacionDto (6 propiedades)
- ResumenMaestrosDto (6 propiedades)

✅ **Adapter** (MaestroDocumentoAdapter.cs - 350 líneas, 8 métodos):
- ObtenerMaestrosAsync
- ObtenerMaestroAsync
- CrearMaestroTipo1ConstruccionAsync
- CrearMaestroTipo2ActualizacionAsync
- AnularMaestroTipo3Async
- ActualizarMaestroAsync
- DesactivarMaestroAsync
- ObtenerResumenMaestrosAsync

✅ **Service** (MaestroDocumentoService.cs - 300 líneas, 8 métodos):
- Validaciones completas para cada tipo
- Lógica de creación de documentos controlados
- Lógica de versionamiento (Tipo 2)
- Lógica de anulación con auditoría (Tipo 3)
- Logging detallado

---

## 🏗️ Lógica por Tipo de Solicitud

### Tipo 1: Construcción (Crear Nuevo Maestro)

**Flujo**:
```
1. Usuario crea solicitud Tipo 1
2. Service.CrearMaestroTipo1ConstruccionAsync()
   a. Validaciones: Nombre, IdProceso, Usuario
   b. Adapter.CrearMaestroTipo1ConstruccionAsync()
      - INSERT INTO GD_MaestroDocumentos (IdTipoSolicitud=1)
      - Activo=true, Controlado=true/false
      - TiempoRetencion, DisposicionFinal
      - CREATE documento controlado si Controlado=true
   c. Return (true, "Maestro creado", idMaestro)
```

**Parámetros**:
- NombreDocumento (obligatorio)
- CodigoDocumento
- IdProceso (obligatorio)
- TiempoRetencionAños (default 5)
- DisposicionFinal
- Controlado (default true)
- RequiereRevision, RequiereAprobacion
- RevisoresIniciales (opcional)

**SP**: `GD_MaestroDocumentos_Add` (Tipo 1)

---

### Tipo 2: Actualización (Nueva Versión)

**Flujo**:
```
1. Usuario crea solicitud Tipo 2
2. Service.CrearMaestroTipo2ActualizacionAsync()
   a. Validaciones: IdMaestroExistente, MotivoCambio
   b. Verificar maestro existente (debe estar activo)
   c. Adapter.CrearMaestroTipo2ActualizacionAsync()
      - INSERT INTO GD_MaestroDocumentos (IdTipoSolicitud=2)
      - IdMaestroExistente = FK al maestro anterior
      - VersionNumero (ej: 1.1, 2.0)
      - MotivoCambio registrado
      - MantenerControlado (heredar del anterior si true)
   d. Return (true, "Nueva versión {VersionNumero} creada", idMaestro)
```

**Parámetros**:
- IdMaestroExistente (obligatorio)
- NombreDocumento
- VersionNumero (ej: "1.1", "2.0")
- MotivoCambio (obligatorio)
- CrearNuevaVersion (default true)
- MantenerControlado (default true - heredar)
- URL del nuevo archivo

**Lógica de Versionamiento**:
- Copia atributos del maestro anterior si no se especifican
- Crea nueva versión sin afectar la anterior (ambas activas)
- Permite trazabilidad completa del documento

**SP**: `GD_MaestroDocumentos_Add2` (Tipo 2)

---

### Tipo 3: Anulación (Desactivar Maestro)

**Flujo**:
```
1. Usuario crea solicitud Tipo 3
2. Service.AnularMaestroTipo3Async()
   a. Validaciones: IdMaestroAnular, MotivoCambio, Activo=true
   b. Adapter.AnularMaestroTipo3Async()
      - UPDATE GD_MaestroDocumentos SET Activo=0
      - UPDATE GD_DocumentosControlados SET Activo=0 (si aplica)
      - INSERT INTO GD_Auditoría (Acción, Motivo, Usuario)
   c. Return (true, "Maestro anulado: {Motivo}")
```

**Parámetros**:
- IdMaestroAnular (obligatorio)
- MotivoAnulacion (obligatorio)
- NumeroResolucion (documentación)
- FechaAnulacion (default NOW)
- UsuarioAnulacion (obligatorio)
- DesactivarDocumentosControlados (default true)

**Cambios**:
- Maestro: Activo = false
- Documentos Controlados: Activo = false (si DesactivarDocumentosControlados=true)
- Auditoría: Se registra acción, motivo, usuario, fecha

**SP**: UPDATE directo (lógica nativa SQL)

---

## 📊 Estructura de Datos

### MaestroDocumentoDto

| Campo | Tipo | Descripción |
|-------|------|-------------|
| IdMaestro | long | PK GD_MaestroDocumentos.IdDocumento |
| NombreDocumento | string | Nombre del documento |
| CodigoDocumento | string | Código/Referencia |
| IdProceso | long | FK a proceso |
| Proceso | string | Nombre del proceso |
| IdTipoSolicitud | long | 1=Construcción, 2=Actualización, 3=Anulación |
| TipoSolicitud | string | Descripción del tipo |
| Activo | bool | true=activo, false=anulado |
| Controlado | bool | true=es documento controlado |
| URL | string | Ruta del archivo |
| TiempoRetencion | int? | Años de retención |
| DisposicionFinal | string | QUÉ hacer con documento vencido |
| FechaRegistro | DateTime | Fecha de creación |
| RegistradoPor | long | IdEmpleado que registró |
| FechaModificacion | DateTime? | Fecha de última actualización |
| ModificadoPor | long? | IdEmpleado que modificó |

### ResumenMaestrosDto

| Campo | Tipo | Descripción |
|-------|------|-------------|
| TotalMaestros | int | Suma de todos (activos + inactivos) |
| TotalConstruccion | int | COUNT(IdTipoSolicitud=1) |
| TotalActualizacion | int | COUNT(IdTipoSolicitud=2) |
| TotalAnulacion | int | COUNT(IdTipoSolicitud=3) |
| MaestrosActivos | int | COUNT(Activo=true) |
| MaestrosInactivos | int | COUNT(Activo=false) |
| DocumentosControlados | int | COUNT(GD_DocumentosControlados.Activo=true) |

---

## 🔐 Validaciones Implementadas

### Tipo 1 (Construcción)

1. NombreDocumento NOT NULL
2. IdProceso > 0
3. RegistradoPor > 0
4. TiempoRetencionAños > 0

### Tipo 2 (Actualización)

1. NombreDocumento NOT NULL
2. IdMaestroExistente > 0
3. IdMaestroExistente existe y está activo
4. MotivoCambio NOT NULL
5. VersionNumero formato válido

### Tipo 3 (Anulación)

1. IdMaestroAnular > 0
2. IdMaestroAnular existe
3. Activo = true (no puede anular ya anulado)
4. MotivoAnulacion NOT NULL
5. UsuarioAnulacion > 0

---

## 📦 Stored Procedures Mapeados

| SP | Tipo | Parámetros | Descripción |
|----|------|-----------|-------------|
| **GD_MaestroDocumentos_Add** | 1 | @Documento, @Codigo, @IdProceso, @IdTipoSolicitud, @Activo, @Controlado, @TiempoRetencion, @DisposicionFinal, @URL, @RegistradoPor | Crear maestro Tipo 1 |
| **GD_MaestroDocumentos_Add2** | 2 | @Documento, @Codigo, @IdProceso, @IdTipoSolicitud, @IdMaestroExistente, @VersionNumero, @MotivoCambio, @RegistradoPor | Crear maestro Tipo 2 (nueva versión) |
| **GD_MaestroDocumentos_Update** | Reutilizado | @IdDocumento, @Documento, @Codigo, @TiempoRetencion, @DisposicionFinal | Actualizar maestro |
| **GD_DocumentosControlados_Activo** | 3 | Lógica nativa SQL | Desactivar documentos controlados |

---

## 📊 Estadísticas

| Métrica | Valor |
|---------|-------|
| **Líneas de código** | 800 LOC (DTOs 150 + Adapter 350 + Service 300) |
| **DTOs** | 5 (MaestroDocumentoDto + 3 especializadas + ResumenMaestrosDto) |
| **Métodos Adapter** | 8 |
| **Métodos Service** | 8 |
| **SPs mapeados** | 2 nuevos (Add, Add2) + 2 reutilizados (Update) |
| **Validaciones** | 13 (4 Tipo1 + 5 Tipo2 + 5 Tipo3) |
| **Errores compilación** | 0 ✅ |

---

## ✅ Checklist Pre-Deploy

- [x] Compilación sin errores
- [x] DTOs con 5 clases (base + 3 especializadas + resumen)
- [x] Adapter con 8 métodos
- [x] Service con validaciones completas (13 validaciones)
- [x] Logging detallado (INFO, ERROR, WARNING)
- [x] Manejo de excepciones sin stack traces
- [x] Creación de documentos controlados (Tipo 1)
- [x] Versionamiento automático (Tipo 2)
- [x] Auditoría de anulación (Tipo 3)
- [x] Resumen estadístico

---

## 🎨 Ejemplo de Uso

### Tipo 1: Crear Nuevo Maestro

```csharp
var maestro1 = new MaestroTipo1ConstruccionDto
{
    NombreDocumento = "Manual de Procedimientos",
    CodigoDocumento = "MP-001",
    IdProceso = 45,
    TiempoRetencionAños = 7,
    Controlado = true,
    DisposicionFinal = "Eliminación",
    URL = "/documentos/manual-v1.0.pdf",
    RegistradoPor = 123
};

var (success, msg, idMaestro) = await _service.CrearMaestroTipo1ConstruccionAsync(maestro1);
// Result: (true, "Maestro 'Manual de Procedimientos' creado exitosamente", 567)
```

### Tipo 2: Nueva Versión

```csharp
var maestro2 = new MaestroTipo2ActualizacionDto
{
    IdMaestroExistente = 567,
    NombreDocumento = "Manual de Procedimientos",
    VersionNumero = "1.1",
    MotivoCambio = "Agregar sección de validaciones",
    URL = "/documentos/manual-v1.1.pdf",
    RegistradoPor = 123,
    MantenerControlado = true
};

var (success, msg, idMaestro2) = await _service.CrearMaestroTipo2ActualizacionAsync(maestro2);
// Result: (true, "Nueva versión '1.1' del maestro creada exitosamente", 568)
// Nota: IdMaestro 567 sigue activo, ahora hay versión 1.1
```

### Tipo 3: Anulación

```csharp
var anulacion = new MaestroTipo3AnulacionDto
{
    IdMaestroAnular = 567,
    MotivoAnulacion = "Documento obsoleto, reemplazado por versión 2.0",
    NumeroResolucion = "RES-2026-001",
    UsuarioAnulacion = 123,
    DesactivarDocumentosControlados = true
};

var (success, msg) = await _service.AnularMaestroTipo3Async(anulacion);
// Result: (true, "Maestro 'Manual de Procedimientos' anulado exitosamente. Motivo: Documento obsoleto...")
```

---

## 🔗 Integración

### Sprints Anteriores
- ✅ Sprint 12.3.1-4: Solicitudes + Aprobaciones + Audit Trail + Testing (40h)

### Próximos Sprints
- ⏳ Sprint 12.3.6: PNC Productos No Conformes (16h)
- ⏳ Sprint 12.3.7: Repositorio Validaciones (8h)
- ⏳ Sprint 12.3.8: Catálogos Edición (4h)

---

**Documento completado**: 2025-01-15  
**Estado de deploy**: ✅ LISTO PARA STAGING  
**Compilación**: ✅ Sin errores  
**Integración**: ✅ Con Sprints 12.3.1-4  
**Sprints 12.3.1-5 COMPLETADOS**: 52h de 80h (65%)
