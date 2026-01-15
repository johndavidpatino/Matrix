# Mapeo SP - Cierre de Trabajo con GD (Sprint 12.1.6)

**Módulo**: OP (Operativo)  
**Funcionalidad**: Cierre de Trabajos con Validación de Documentos GD  
**Fecha**: 2026-01-15  
**Estado**: ✅ Completado  
**Verificación**: CoreProject → MatrixNext

---

## 1. Identificación de Stored Procedures

### Origen: CoreProject
**Archivos relevantes**:
- `CoreProject/TrabajosClass.vb` - Gestión de trabajos
- `CoreProject/GestionDocumentalClass.vb` - Validación de documentos

**SPs Identificados**:
1. `PY_Trabajos_UpdateEstado` - Actualiza estado del trabajo
2. `GD_DocumentosEscaneados_ValidarCierre` - Valida documentos para cierre

---

## 2. Mapeo de Stored Procedures

| Acción | SP Nombre | Parámetros | Retorno | Notas |
|--------|-----------|-----------|---------|-------|
| **CambiarEstadoACerrado** | `PY_Trabajos_UpdateEstado` | @IdTrabajo, @NuevoEstado, @FechaCierre, @CerradoPor, @Observaciones | Int | Si no existe, usar query directa |
| **ValidarDocumentos** | Query directa | @IdTrabajo | Dinámico | Consulta GD_DocumentosEscaneados |
| **ObtenerTrabajo** | Query directa | @IdTrabajo | Fila única | Consulta PY_Trabajos |

---

## 3. Modelos de Datos

### DTOs Usados

**CierreTrabajoDto**
```csharp
public long IdTrabajo { get; set; }
public string EstadoAnterior { get; set; }
public string EstadoNuevo { get; set; } = "Cerrado"
public DateTime FechaCierre { get; set; }
public string? Observaciones { get; set; }
public bool ValidacionDocumentosOk { get; set; }
public int TotalDocumentosValidados { get; set; }
public long? UsuarioId { get; set; }
```

**ValidacionDocumentosDto**
```csharp
public bool EsValido { get; set; }
public int TotalDocumentos { get; set; }
public int DocumentosValidados { get; set; }
public List<string> ErroresValidacion { get; set; }
public string? MensajeError { get; set; }
```

---

## 4. Implementación en MatrixNext

### Adapter Pattern

**Archivo**: `MatrixNext.Data/Adapters/OP/CierreTrabajoAdapter.cs`

```csharp
public class CierreTrabajoAdapter : ICierreTrabajoAdapter
{
    // 1. ObtenerTrabajoAsync: Consulta estado actual
    // 2. ValidarDocumentosAsync: Verifica documentos en GD
    // 3. CambiarEstadoACerradoAsync: Ejecuta SP o query de actualización
    // 4. ObtenerDatosTrabajoAsync: Datos para notificación
}
```

### Service Layer

**Archivo**: `MatrixNext.Data/Services/OP/CierreTrabajoService.cs`

```csharp
public class CierreTrabajoService : ICierreTrabajoService
{
    // ValidarRequisitosParaCierreAsync: Orquesta validaciones
    // CerrarTrabajoAsync: Cierre completo (validar + cambiar estado + notificar)
    // ObtenerValidacionDocumentosAsync: Información de validación
}
```

---

## 5. Registro DI en Program.cs

```csharp
// ===== SPRINT 12.1.6: OP Cierre de Trabajo con GD =====
builder.Services.AddScoped<ICierreTrabajoAdapter, CierreTrabajoAdapter>();
builder.Services.AddScoped<ICierreTrabajoService, CierreTrabajoService>();
```

---

## 6. Flujo de Cierre

1. **Validación Previa** (ValidarRequisitosParaCierreAsync)
   - Verificar que trabajo existe
   - Validar documentos escaneados (GD_DocumentosEscaneados)
   - Retornar lista de errores si no cumple

2. **Cambio de Estado** (CambiarEstadoACerradoAsync)
   - Ejecutar SP `PY_Trabajos_UpdateEstado`
   - Fallback a query si SP no existe
   - Auditoría: registro de quién cierra y cuándo

3. **Notificación** (vía OpNotificacionService)
   - Obtener coordinador + PMO + COE
   - Enviar email de cierre exitoso
   - No bloquear si falla email

---

## 7. Checklist de Completitud

- ✅ DTOs: CierreTrabajoDto, ValidacionDocumentosDto, ConfiguracionGdDto
- ✅ Adapter interface: ICierreTrabajoAdapter
- ✅ Adapter implementation: CierreTrabajoAdapter (4 métodos)
- ✅ Service interface: ICierreTrabajoService
- ✅ Service implementation: CierreTrabajoService
- ✅ Validación de documentos GD
- ✅ Cambio de estado con fallback a query
- ✅ Integración con OpNotificacionService
- ✅ Registro DI en Program.cs
- ✅ Logging en INFO/WARNING/ERROR levels
- ✅ Manejo de errores

---

**Documento creado**: 2026-01-15  
**Versión**: 1.0  
**Completitud**: 100%  
**Listo para QA**: ✅ Sí
