# Mapeo SP - Consolidar Productividad Multiroles (Sprint 12.1.8)

**Módulo**: OP (Operativo)  
**Funcionalidad**: Gestión unificada de productividad para PMO, Coordinador, Campo y MyS  
**Fecha**: 2026-01-15  
**Estado**: ✅ Completado  
**Verificación**: CoreProject → MatrixNext

---

## 1. Identificación de Stored Procedures

### Origen: CoreProject
**Archivos relevantes**:
- `CoreProject/ProductividadClass.vb` - Gestión de planillas PMO/Coordinador
- `CoreProject/ProductividadCampoClass.vb` - Gestión de planillas Campo
- `CoreProject/ProductividadCallClass.vb` - Gestión de planillas MyS/Call Center

**SPs Identificados**:
1. `OP_CuantiDapper.CuantiProdProductividad_Get` - Obtiene planillas según filtros
2. `CuantiPlanillasTrabajosUpdate` - Aprueba planilla
3. `CuantiPlanillasTrabajosRemove` - Rechaza planilla
4. `OP_CuantiPlanillas_GetResumen` - Resumen por corte/mes

---

## 2. Mapeo de Stored Procedures

| Acción | SP Nombre | Parámetros | Retorno | Notas |
|--------|-----------|-----------|---------|-------|
| **ObtenerPlanillasPorRol** | Query directa con JOINs | @Rol, @UsuarioId, @Filtros, @Offset, @PageSize | List<ProductividadPlanillaDto> | Filtrado por rol en WHERE |
| **ObtenerResumen** | Query directa | @Año, @Mes, @Corte, @IdTrabajo | ResumenProductividadDto | Agregaciones SUM/COUNT |
| **AprobarPlanilla** | `UPDATE CuantiPlanillas` | @IdPlanilla, @MontoAutorizado, @AprobadoPor | Bool | Fallback a query |
| **RechazarPlanilla** | `UPDATE CuantiPlanillas` | @IdPlanilla, @Observaciones, @UsuarioId | Bool | Fallback a query |
| **TienePermiso** | Query directa | @UsuarioId, @IdTrabajo, @Rol | Bool | Valida según rol y asignación |
| **CalcularCorte16_15** | Lógica interna | @Fecha | (Int, Int, Int) | Calcula corte/mes/año |
| **ObtenerPermisosUsuario** | Query directa | @UsuarioId | PermisosProductividadDto | Consulta US_PermisosUsuario |
| **ObtenerTrabajosAsignados** | Query directa | @UsuarioId, @Rol | List<dynamic> | Filtrado por rol |

---

## 3. Modelos de Datos

### DTOs Usados

**ProductividadPlanillaDto**
```csharp
public long IdPlanilla { get; set; }
public long IdTrabajo { get; set; }
public string NumeroTrabajo { get; set; }
public long IdEmpleado { get; set; }
public string NombreEmpleado { get; set; }
public DateTime Fecha { get; set; }
public int Cantidad { get; set; }
public decimal MontoReportado { get; set; }
public decimal? MontoAutorizado { get; set; }
public string TipoProductividad { get; set; }
public string Estado { get; set; } // Pendiente, Aprobado, Rechazado
public int Corte16_15 { get; set; }
public bool PuedeAprobar { get; set; }
public bool PuedeRechazar { get; set; }
```

**FiltrosProductividadDto**
```csharp
public long? IdTrabajo { get; set; }
public DateTime? FechaInicio { get; set; }
public DateTime? FechaFin { get; set; }
public int? Corte { get; set; }
public string? Estado { get; set; }
public int PageNumber { get; set; } = 1;
public int PageSize { get; set; } = 50;
```

**PermisosProductividadDto**
```csharp
public bool PuedeVerPMO { get; set; }          // Permiso 100
public bool PuedeVerCoordinador { get; set; }  // Permiso 135
public bool PuedeVerCampo { get; set; }        // Permiso 156
public bool PuedeVerMyS { get; set; }          // Permiso 157
public bool PuedeAprobar { get; set; }
public bool PuedeRechazar { get; set; }
public string RolActual { get; set; }
```

---

## 4. Implementación en MatrixNext

### Adapter Pattern

**Archivo**: `MatrixNext.Data/Adapters/OP/ProductividadAdapter.cs`

```csharp
public class ProductividadAdapter : IProductividadAdapter
{
    // 1. ObtenerPlanillasPorRolAsync: Query con filtrado según rol
    //    - PMO: Todos los trabajos activos
    //    - Coordinador: Trabajos donde es coordinador (PY_TrabajosPersonal)
    //    - Campo: Solo sus propias planillas (p.IdEmpleado = @UsuarioId)
    //    - MyS: Planillas de Supervisión y Llamadas
    
    // 2. ObtenerResumenAsync: Agregaciones SUM/COUNT por periodo
    
    // 3. AprobarPlanillaAsync: UPDATE con validación Estado='Pendiente'
    
    // 4. RechazarPlanillaAsync: UPDATE con ObservacionesRechazo
    
    // 5. TienePermisoAsync: Valida según rol y tabla PY_TrabajosPersonal
    
    // 6. CalcularCorte16_15Async: Lógica días 1-15 vs 16-fin mes
    
    // 7. ObtenerPermisosUsuarioAsync: Query a US_PermisosUsuario con MAX(CASE)
    
    // 8. ObtenerTrabajosAsignadosAsync: Query filtrado por rol
}
```

### Service Layer

**Archivo**: `MatrixNext.Data/Services/OP/ProductividadConsolidadoService.cs`

```csharp
public class ProductividadConsolidadoService : IProductividadConsolidadoService
{
    // ObtenerPlanillasAsync: Obtiene permisos → llama adapter con rol
    // ObtenerResumenProductividadAsync: Valida permisos → resumen
    // AprobarPlanillasAsync: Valida permisos → bucle aprobación → resumen resultado
    // RechazarPlanillaAsync: Valida permisos + observaciones → rechazar
    // PuedeRealizarAccionAsync: Valida permisos según acción
    // ObtenerPermisosYRolAsync: Wrapper de adapter
    // ObtenerTrabajosDisponiblesAsync: Obtiene permisos → trabajos según rol
}
```

---

## 5. Registro DI en Program.cs

```csharp
// ===== SPRINT 12.1.8: OP Consolidar Productividad Multiroles =====
builder.Services.AddScoped<IProductividadAdapter, ProductividadAdapter>();
builder.Services.AddScoped<IProductividadConsolidadoService, ProductividadConsolidadoService>();
```

---

## 6. Lógica de Roles y Permisos

### Matriz de Permisos

| Rol | Permiso ID | Puede Ver | Puede Aprobar | Puede Rechazar | Filtrado |
|-----|-----------|-----------|---------------|----------------|----------|
| **PMO** | 100 | Todos los trabajos | ✅ Sí | ✅ Sí | Ninguno (ve todo) |
| **Coordinador** | 135 | Sus trabajos asignados | ✅ Sí | ✅ Sí | PY_TrabajosPersonal.Cargo='Coordinador' |
| **Campo** | 156 | Sus propias planillas | ❌ No | ❌ No | p.IdEmpleado = @UsuarioId |
| **MyS/Call** | 157 | Supervisión y Llamadas | ❌ No | ❌ No | TipoProductividad IN ('Supervisión', 'Llamadas') |

### Prioridad de Roles

Cuando un usuario tiene múltiples permisos, el rol se determina por prioridad:

1. **PMO** (100) - Mayor prioridad
2. **Coordinador** (135)
3. **Campo** (156)
4. **MyS** (157) - Menor prioridad

---

## 7. Cálculo de Corte 16-15

### Lógica Implementada

```csharp
public async Task<(int Corte, int Mes, int Año)> CalcularCorte16_15Async(DateTime fecha)
{
    int corte, mes, año;
    
    if (fecha.Day >= 1 && fecha.Day <= 15)
    {
        // Primera quincena
        corte = 1;
        mes = fecha.Month;
        año = fecha.Year;
    }
    else
    {
        // Segunda quincena
        corte = 2;
        mes = fecha.Month;
        año = fecha.Year;
    }
    
    return (corte, mes, año);
}
```

### Ejemplos

| Fecha | Corte | Mes | Año |
|-------|-------|-----|-----|
| 2026-01-10 | 1 | 1 | 2026 |
| 2026-01-15 | 1 | 1 | 2026 |
| 2026-01-16 | 2 | 1 | 2026 |
| 2026-01-31 | 2 | 1 | 2026 |
| 2026-02-01 | 1 | 2 | 2026 |

---

## 8. Flujo de Aprobación

1. **Validación de permisos**
   - Usuario debe tener permiso 100 (PMO) o 135 (Coordinador)
   - Si es Coordinador, validar que es del trabajo correspondiente

2. **Validación de planilla**
   - Planilla debe estar en estado "Pendiente"
   - Monto autorizado no puede ser mayor al reportado (validación adicional recomendada)

3. **Actualización**
   - UPDATE CuantiPlanillas SET Estado='Aprobado', MontoAutorizado=@Monto, FechaAprobacion=NOW(), AprobadoPor=@UserId
   - WHERE IdPlanilla=@Id AND Estado='Pendiente'

4. **Auditoría**
   - Log de operación con usuario, fecha y monto
   - Registro de aprobación en tabla de auditoría (opcional)

---

## 9. Flujo de Rechazo

1. **Validación de permisos** (igual que aprobación)

2. **Validación de observaciones**
   - Observaciones son obligatorias para rechazar

3. **Actualización**
   - UPDATE CuantiPlanillas SET Estado='Rechazado', ObservacionesRechazo=@Obs, FechaAprobacion=NOW(), AprobadoPor=@UserId
   - WHERE IdPlanilla=@Id AND Estado='Pendiente'

4. **Notificación** (opcional)
   - Enviar email al empleado con las observaciones de rechazo

---

## 10. Diferencias con WebMatrix

### ❌ Eliminado (Deprecated)

1. **4 Controllers separados**: PMO, Coordinador, Campo, MyS
   - WebMatrix: ProductividadPMOController, ProductividadCoordinadorController, etc.
   - MatrixNext: Un solo servicio `ProductividadConsolidadoService` con filtrado por rol

2. **Lógica duplicada**: Validaciones repetidas en cada controller
   - WebMatrix: Cada controller tiene su propia validación de corte 16-15
   - MatrixNext: Helper centralizado `CalcularCorte16_15Async()`

3. **Permisos hardcoded**: Checks de permisos en cada acción
   - WebMatrix: `if (permiso == 100 || permiso == 135) { ... }`
   - MatrixNext: `ObtenerPermisosUsuarioAsync()` con matriz de permisos

### ✅ Agregado (Mejoras)

1. **Servicio unificado**: Un solo punto de entrada para todas las operaciones
2. **Validación centralizada de permisos**: Método `TienePermisoAsync()`
3. **Resumen de productividad**: Agregaciones en una sola consulta
4. **Paginación**: Soporte para OFFSET/FETCH con filtros
5. **Filtrado avanzado**: Múltiples criterios (fecha, corte, estado, tipo)
6. **Logging detallado**: Trazabilidad completa de operaciones

---

## 11. Checklist de Completitud

- ✅ DTOs: ProductividadPlanillaDto, FiltrosProductividadDto, AprobacionPlanillaDto, ResumenProductividadDto, PermisosProductividadDto
- ✅ Adapter interface: IProductividadAdapter (9 métodos)
- ✅ Adapter implementation: ProductividadAdapter
- ✅ Service interface: IProductividadConsolidadoService (7 métodos)
- ✅ Service implementation: ProductividadConsolidadoService
- ✅ Filtrado por rol (PMO, Coordinador, Campo, MyS)
- ✅ Validación de permisos (100, 135, 156, 157)
- ✅ Aprobación/Rechazo con validaciones
- ✅ Cálculo corte 16-15
- ✅ Resumen de productividad
- ✅ Paginación
- ✅ Logging INFO/WARNING/ERROR
- ✅ Manejo de errores con try-catch
- ✅ Registro DI en Program.cs
- ✅ Endpoint genérico para aprobación masiva

---

**Documento creado**: 2026-01-15  
**Versión**: 1.0  
**Completitud**: 100%  
**Listo para QA**: ✅ Sí
