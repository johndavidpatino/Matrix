# Mapeo SP - Carga Masiva CATI vs Planillas (Sprint 12.1.7)

**Módulo**: OP (Operativo)  
**Funcionalidad**: Importación masiva de datos desde Excel (CATI RMC y Planillas)  
**Fecha**: 2026-01-15  
**Estado**: ✅ Completado  
**Verificación**: CoreProject → MatrixNext

---

## 1. Identificación de Stored Procedures

### Origen: CoreProject
**Archivos relevantes**:
- `CoreProject/CatiRmcClass.vb` - Operaciones CATI
- `CoreProject/ProductividadClass.vb` - Gestión de planillas

**SPs Identificados**:
1. `CatiRMC_BorrarDatosRespuestasCatiRMCtmp` - Limpia tabla temporal
2. `CatiRMC_ValidarDatos` - Validaciones de estructura
3. `CatiRMC_InsertarDatosEnRespuestas` - Inserta datos validados
4. `OP_CuantiPlanillas_Insert` - Inserta planillas
5. `OP_CuantiPlanillas_Validate` - Valida planillas (corte 16-15, festivos)

---

## 2. Mapeo de Stored Procedures

| Acción | SP Nombre | Parámetros | Retorno | Notas |
|--------|-----------|-----------|---------|-------|
| **ValidarColumnasCati** | Query directa | N/A | Bool + Errores | Validación de headers Excel |
| **ValidarColumnasPlanillas** | Query directa | N/A | Bool + Errores | Validación de headers Excel |
| **ValidarFilaCati** | Query directa | @TrabajoId, @TipoActividad | ResultadoValidacionFilaDto | Verifica trabajo existe, TipoActividad válido |
| **ValidarFilaPlanilla** | Query directa | @IdTrabajo, @IdEmpleado, @Fecha | ResultadoValidacionFilaDto | Valida corte 16-15, festivos |
| **ObtenerFestivos** | Query directa | @Ano | List<DateTime> | Consulta tabla Configuracion_Festivos |
| **InsertarCatiRmc** | `INSERT INTO CatiRMC_RespuestasTmp` | Batch | Int | Transacción con commit |
| **InsertarPlanillas** | `INSERT INTO CuantiPlanillas_Tmp` | Batch | Int | Transacción con commit |
| **CalcularCorte16_15** | Lógica interna | @Fecha | Int (1 o 2) | Cálculo quincena |

---

## 3. Modelos de Datos

### DTOs Usados

**CargaCatiRmcDto**
```csharp
public long TrabajoId { get; set; }
public int Res_Numero { get; set; }
public string Per_NumIdentificacionEncu { get; set; }
public string? Per_NumIdentificacionSup { get; set; }
public string? Res_IDM { get; set; }
public string? Res_Ciudad { get; set; }
public DateTime? Res_Fecha { get; set; }
public string? TipoSupervision { get; set; }
public string TipoActividad { get; set; } // Enum: Implementación, InstruccionarioRespondido, InstruccionarioCorregido, Supervisión
```

**CargaPlanillaDto**
```csharp
public long IdTrabajo { get; set; }
public long IdEmpleado { get; set; }
public DateTime Fecha { get; set; }
public int Cantidad { get; set; }
public string? TipoProductividad { get; set; }
public string? Observaciones { get; set; }
```

**ResumenCargaMasivaDto**
```csharp
public string TipoCarga { get; set; } // "CATI" o "Planillas"
public int TotalFilas { get; set; }
public int FilasValidas { get; set; }
public int FilasRechazadas { get; set; }
public List<ResultadoValidacionFilaDto> Validaciones { get; set; }
public DateTime FechaCarga { get; set; }
public long UsuarioId { get; set; }
public string NombreArchivo { get; set; }
```

---

## 4. Implementación en MatrixNext

### Adapter Pattern

**Archivo**: `MatrixNext.Data/Adapters/OP/CargaMasivaAdapter.cs`

```csharp
public class CargaMasivaAdapter : ICargaMasivaAdapter
{
    // 1. ValidarColumnasExcelCatiAsync: Verifica 9 columnas requeridas
    // 2. ValidarColumnasExcelPlanillasAsync: Verifica 5 columnas requeridas
    // 3. ValidarFilaCatiAsync: Valida trabajo existe, TipoActividad enum, campos requeridos
    // 4. ValidarFilaPlanillaAsync: Valida trabajo, empleado, cantidad > 0, corte 16-15, festivos
    // 5. ObtenerFestivosAsync: Consulta tabla Configuracion_Festivos
    // 6. InsertarCatiRmcAsync: INSERT batch con transacción
    // 7. InsertarPlanillasAsync: INSERT batch con transacción
    // 8. CalcularCorte16_15Async: Lógica día 1-15 vs 16-fin mes
}
```

### Service Layer

**Archivo**: `MatrixNext.Data/Services/OP/CargaMasivaService.cs`

```csharp
public class CargaMasivaService : ICargaMasivaService
{
    // ProcesarCatiRmcAsync: ClosedXML → Validar columnas → Extraer → Validar filas → Insertar (si ejecutar=true)
    // ProcesarPlanillasAsync: ClosedXML → Validar columnas → Extraer → Validar filas → Insertar (si ejecutar=true)
    // ExtraerDatosExcelAsync: Genérico para lectura Excel (lógica ya en métodos específicos)
    // ValidarFilasAsync: Genérico para validación (lógica ya en métodos específicos)
}
```

---

## 5. Registro DI en Program.cs

```csharp
// ===== SPRINT 12.1.7: OP Carga Masiva CATI vs Planillas =====
builder.Services.AddScoped<ICargaMasivaAdapter, CargaMasivaAdapter>();
builder.Services.AddScoped<ICargaMasivaService, CargaMasivaService>();
```

---

## 6. Validaciones Implementadas

### CATI RMC (9 columnas requeridas)

1. **Columnas**: TrabajoId, Res_Numero, Per_NumIdentificacionEncu, Per_NumIdentificacionSup, Res_IDM, Res_Ciudad, Res_Fecha, TipoSupervision, TipoActividad
2. **TrabajoId**: Debe existir en PY_Trabajos
3. **TipoActividad**: Enum válido (Implementación, InstruccionarioRespondido, InstruccionarioCorregido, Supervisión)
4. **Per_NumIdentificacionEncu**: No puede estar vacío
5. **Res_Fecha**: Si existe, no puede ser posterior a mañana (advertencia)

### Planillas (5 columnas requeridas)

1. **Columnas**: IdTrabajo, IdEmpleado, Fecha, Cantidad, TipoProductividad
2. **IdTrabajo**: Debe existir en PY_Trabajos
3. **IdEmpleado**: Debe existir en TH_Empleado
4. **Cantidad**: Debe ser mayor a 0
5. **Fecha**: Validar corte 16-15 (1-15 vs 16-fin mes)
6. **Festivos**: Advertencia si fecha es festivo o domingo

---

## 7. Diferencias con WebMatrix

### ❌ Eliminado (Deprecated)

1. **OleDb**: Reemplazado por ClosedXML
   - WebMatrix: `OleDbConnection` + `OleDbDataAdapter`
   - MatrixNext: `ClosedXML.Excel.XLWorkbook` + lectura directa

2. **Validaciones en JS/VB**: Movidas a backend C#
   - WebMatrix: Validaciones client-side con JavaScript
   - MatrixNext: Validaciones server-side async con logging

3. **Tablas temporales**: Simplificadas
   - WebMatrix: Múltiples tablas intermedias
   - MatrixNext: Una tabla `_Tmp` por tipo

### ✅ Agregado (Mejoras)

1. **Validaciones robustas**: Try-catch con logging en cada paso
2. **Resumen detallado**: Objeto `ResumenCargaMasivaDto` con errores por fila
3. **Modo vista previa**: `ejecutar=false` solo valida sin insertar
4. **Async/await**: Procesamiento no bloqueante
5. **Auditoría**: Campos `InsertadoFecha` y `InsertadoPor` en todas las inserciones

---

## 8. Flujo de Procesamiento

1. **Recepción** (Controller)
   - Usuario sube archivo .xls/.xlsx
   - Controller llama a `ICargaMasivaService.ProcesarCatiRmcAsync()` o `ProcesarPlanillasAsync()`

2. **Lectura Excel** (Service)
   - Abre archivo con ClosedXML
   - Extrae columnas de primera fila
   - Valida estructura contra lista requerida

3. **Extracción** (Service)
   - Itera desde fila 2 hasta vacío
   - Convierte celdas a DTOs con `TryGetValue()`
   - Agrega a lista en memoria

4. **Validación** (Adapter)
   - Para cada fila:
     - Valida FK (Trabajo, Empleado)
     - Valida enums (TipoActividad)
     - Valida fechas (corte, festivos)
     - Retorna `ResultadoValidacionFilaDto`

5. **Inserción** (Adapter, solo si ejecutar=true)
   - Filtra solo filas válidas
   - Abre transacción
   - INSERT batch
   - Commit

6. **Resumen** (Service)
   - Retorna `ResumenCargaMasivaDto` con:
     - TotalFilas, FilasValidas, FilasRechazadas
     - Lista completa de validaciones
     - Metadata de carga

---

## 9. Checklist de Completitud

- ✅ DTOs: CargaCatiRmcDto, CargaPlanillaDto, ResumenCargaMasivaDto, ResultadoValidacionFilaDto, ConfiguracionCargaMasivaDto
- ✅ Adapter interface: ICargaMasivaAdapter (10 métodos)
- ✅ Adapter implementation: CargaMasivaAdapter
- ✅ Service interface: ICargaMasivaService (4 métodos)
- ✅ Service implementation: CargaMasivaService
- ✅ Uso de ClosedXML (reemplazo OleDb)
- ✅ Validaciones CATI: columnas, trabajo, TipoActividad, campos requeridos
- ✅ Validaciones Planillas: columnas, trabajo, empleado, cantidad, corte 16-15, festivos
- ✅ Cálculo corte 16-15
- ✅ Consulta festivos desde BD
- ✅ Inserción batch con transacción
- ✅ Auditoría (InsertadoFecha, InsertadoPor)
- ✅ Logging INFO/WARNING/ERROR
- ✅ Manejo de errores con try-catch
- ✅ Registro DI en Program.cs
- ✅ Modo vista previa (ejecutar=false)

---

**Documento creado**: 2026-01-15  
**Versión**: 1.0  
**Completitud**: 100%  
**Listo para QA**: ✅ Sí
