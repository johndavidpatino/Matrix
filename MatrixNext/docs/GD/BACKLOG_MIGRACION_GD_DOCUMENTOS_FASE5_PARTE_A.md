# 📋 BACKLOG DE MIGRACIÓN - GD_Documentos FASE 5 PARTE A

**Fases**: FASE 5 PARTE A (Sprint 8)  
**Tema**: PNC - Proceso Nueva Creación  
**Horas Totales**: 40h  
**Duración Estimada**: 1 semana (1 sprint)  
**Versión**: 1.0  
**Fecha**: 2026-01-09

---

## 📑 CONTENIDO

- [Resumen Ejecutivo](#resumen-ejecutivo)
- [Sprint 8: PNC - Proceso Nueva Creación](#sprint-8-pnc---proceso-nueva-creación)

---

## 🎯 RESUMEN EJECUTIVO

### Objetivos de FASE 5 PARTE A

Implementar **Proceso Nueva Creación (PNC)** - Sistema para crear nuevos documentos controlados con aprobación multirevidor.

**PNC = Proceso diferente al maestro/solicitudes**: Usuario solicita crear un NUEVO documento controlado (no actualizar existente).

### Flujo PNC Conceptual

```
1. Usuario crea solicitud PNC
   ↓
2. Sistema asigna revisores (área, calidad, etc.)
   ↓
3. Revisores aprueban/rechazan en paralelo
   ↓
4. Si TODO aprobado → Se crea documento maestro automáticamente
   ↓
5. Se genera repositorio con v1.0
```

### Diferencias PNC vs Solicitud

| Aspecto | Solicitud | PNC |
|---------|-----------|-----|
| Propósito | Actualizar documento existente | **Crear nuevo documento** |
| Maestro | Debe existir | Se crea automáticamente |
| Documento inicial | Ya existe | Se proporciona en solicitud |
| Revisores | Reutilización | Nueva asignación |
| Éxito = | Estado "Aprobado" | Maestro creado + v1.0 repo |

### Dependencias Críticas

✅ **COMPLETADAS**:
- FASE 1-4: Infraestructura, Catálogos, Maestro, Solicitudes, Aprobaciones, Email

⚠️ **PENDIENTE**:
- Lógica de auto-creación de maestro (debe investigarse con P0-5)
- Decisión: ¿En qué punto se crea? (Inmediato vs Post-aprobación)

### Reglas Aplicables

| Regla | Descripción | Prioridad |
|-------|-------------|-----------|
| REGLA 2 | Mapear SP exactamente | 🔴 CRÍTICA |
| REGLA 3 | Usar EF para CRUD | 🟠 ALTA |
| REGLA 6 | Paridad 1:1 con legado | 🔴 CRÍTICA |
| REGLA 7 | Reutilizar servicios | 🟠 ALTA |
| REGLA 11 | Validar permisos | 🔴 CRÍTICA |
| REGLA 12 | Input validation | 🔴 CRÍTICA |
| REGLA 14 | Async/await | 🟠 ALTA |

---

## 🚀 SPRINT 8: PNC - PROCESO NUEVA CREACIÓN

### Objetivo

Implementar sistema completo de solicitudes PNC para creación de nuevos documentos controlados.

**Horas Estimadas**: 40h  
**Duración**: 5-6 días  
**Criterio de Éxito**:
- ✅ Usuario crea solicitud PNC
- ✅ Sistema asigna revisores automáticamente
- ✅ Revisores aprueban/rechazan
- ✅ Maestro creado automáticamente post-aprobación
- ✅ Repositorio v1.0 generado
- ✅ Email enviados en cada paso
- ✅ 0 inconsistencias

---

### TAREA 8.1: Mapear SPs de PNC (1.5h)

**Descripción**: Documentar SPs para PNC vs WebMatrix

**SPs Identificados** (del análisis):

1. `GD_SolicitudPNC_Insert` - Crear solicitud PNC
2. `GD_SolicitudPNC_Update` - Actualizar solicitud
3. `GD_SolicitudPNC_Select` - Obtener solicitud + detalles
4. `GD_SolicitudPNC_SelectAll` - Listar solicitudes
5. `GD_SolicitudPNC_Delete` - Cancelar solicitud
6. `GD_RevisionPNC_Insert` - Registrar revisión
7. `GD_RevisionPNC_Update` - Actualizar revisión
8. `GD_RevisionPNC_Select` - Obtener revisión
9. `GD_MaestroDocumentos_Insert` - Auto-crear maestro post-aprobación
10. `GD_RepositorioDocumentos_Insert` - Auto-crear v1.0 repo

**Crear MAPEO_SP_PNC.csv**:

```csv
SP_WebMatrix,SP_MatrixNext,Parámetros,Descripción,Criticidad
GD_SolicitudPNC_Insert,GD_SolicitudPNC_Insert,"@idArea, @idResponsable, @nombreDocumento, @descripcion, @archivoPath, @creadoPor, @fechaRegistro","Crear nueva solicitud PNC",🔴CRÍTICA
GD_SolicitudPNC_Update,GD_SolicitudPNC_Update,"@idSolicitud, @nombreDocumento, @descripcion, @estadoId, @modificadoPor, @fechaModificacion","Actualizar solicitud PNC",🟠ALTA
GD_SolicitudPNC_Select,GD_SolicitudPNC_Select,"@idSolicitud","Obtener solicitud con detalles",🔴CRÍTICA
GD_SolicitudPNC_SelectAll,GD_SolicitudPNC_SelectAll,"@filtroEstado (opcional), @idArea (opcional)","Listar solicitudes PNC",🟠ALTA
GD_SolicitudPNC_Delete,GD_SolicitudPNC_Delete,"@idSolicitud","Cancelar solicitud PNC",🟠ALTA
GD_RevisionPNC_Insert,GD_RevisionPNC_Insert,"@idSolicitud, @idRevisor, @estado, @comentarios, @fechaRevision","Registrar revisión PNC",🔴CRÍTICA
GD_RevisionPNC_Update,GD_RevisionPNC_Update,"@idRevision, @estado, @comentarios, @modificadoPor, @fechaModificacion","Actualizar revisión PNC",🟠ALTA
GD_RevisionPNC_Select,GD_RevisionPNC_Select,"@idRevision","Obtener revisión PNC",🟠ALTA
GD_MaestroDocumentos_Insert,GD_MaestroDocumentos_Insert,"(usado por auto-creación)","Auto-crear maestro post-aprobación",🔴CRÍTICA
GD_RepositorioDocumentos_Insert,GD_RepositorioDocumentos_Insert,"(usado por auto-creación)","Auto-crear v1.0 post-aprobación",🔴CRÍTICA
```

**Validación**:
- ✅ MAPEO_SP_PNC.csv creado
- ✅ 10 SPs documentadas
- ✅ Parámetros exactos
- ✅ Criticidad asignada

---

### TAREA 8.2: Crear ViewModels PNC (2h)

**Descripción**: 5+ ViewModels para PNC

**Ubicación**: `Models/ViewModels/GD/PNC/`

**ViewModels**:

#### 1. PncSolicitudCreateVM (POST)
```csharp
public class PncSolicitudCreateVM
{
    [Required(ErrorMessage = "Nombre documento requerido")]
    [StringLength(200)]
    public string NombreDocumento { get; set; }

    [Required]
    public int IdArea { get; set; }

    [Required]
    public int IdResponsable { get; set; }

    [StringLength(1000)]
    public string Descripcion { get; set; }

    [Required]
    public int IdProceso { get; set; }

    // Archivo = se envía via FormFile, no aquí
}
```

#### 2. PncSolicitudVM (GET - Detalle)
```csharp
public class PncSolicitudVM
{
    public int Id { get; set; }
    public string NombreDocumento { get; set; }
    public string Area { get; set; }
    public string Responsable { get; set; }
    public string Descripcion { get; set; }
    public string EstadoActual { get; set; } // Pendiente, EnRevision, Aprobado, Rechazado
    public DateTime FechaRegistro { get; set; }
    public string CreadoPor { get; set; }

    public List<PncRevisionVM> Revisiones { get; set; } = new();
    public int TotalRevisores { get; set; }
    public int RevisoresAprobados { get; set; }
    public int RevisoresRechazados { get; set; }
}
```

#### 3. PncSolicitudListVM (GET - Listado)
```csharp
public class PncSolicitudListVM
{
    public int Id { get; set; }
    public string NombreDocumento { get; set; }
    public string Area { get; set; }
    public string Estado { get; set; }
    public DateTime FechaRegistro { get; set; }
    public string Solicitante { get; set; }
    public int RevisoresAprobados { get; set; }
    public int TotalRevisores { get; set; }
}
```

#### 4. PncRevisionVM (GET - Revisión)
```csharp
public class PncRevisionVM
{
    public int Id { get; set; }
    public int IdSolicitud { get; set; }
    public string NombreRevisor { get; set; }
    public string EmailRevisor { get; set; }
    public string Estado { get; set; } // Pendiente, Aprobado, Rechazado
    public string Comentarios { get; set; }
    public DateTime? FechaRevision { get; set; }
}
```

#### 5. PncAsignReviewersVM (POST)
```csharp
public class PncAsignReviewersVM
{
    [Required]
    public int IdSolicitud { get; set; }

    [Required]
    public List<int> IdRevisores { get; set; } = new(); // Array de IDs usuarios
}
```

#### 6. PncAprobarVM (POST)
```csharp
public class PncAprobarVM
{
    [Required]
    public int IdRevision { get; set; }

    [StringLength(500)]
    public string Comentarios { get; set; }
}
```

#### 7. PncRechazarVM (POST)
```csharp
public class PncRechazarVM
{
    [Required]
    public int IdRevision { get; set; }

    [Required(ErrorMessage = "Motivo rechazo requerido")]
    [StringLength(500)]
    public string MotivosRechazo { get; set; }
}
```

**Validación**:
- ✅ 7 ViewModels creados
- ✅ Validaciones DataAnnotations
- ✅ Propiedades completas

---

### TAREA 8.3: Crear Adapter PNC (3h)

**Descripción**: Adapter Dapper para PNC

**Ubicación**: `Data/Adapters/GD/GdPncAdapter.cs`

**Interfaz**:

```csharp
public interface IGdPncAdapter
{
    Task<int> CrearSolicitud(PncSolicitudCreateVM vm, int idUsuarioActual);
    Task<PncSolicitudVM> ObtenerSolicitudById(int id);
    Task<List<PncSolicitudListVM>> ListarSolicitudes(string filtroEstado = null);
    Task<bool> ActualizarSolicitud(int id, PncSolicitudVM vm);
    Task<bool> CancelarSolicitud(int id);
    
    Task<int> CrearRevision(int idSolicitud, List<int> idRevisores);
    Task<PncRevisionVM> ObtenerRevisionById(int id);
    Task<List<PncRevisionVM>> ObtenerRevisionesBySolicitud(int idSolicitud);
    Task<bool> AprobarRevision(int idRevision, string comentarios, int idUsuarioActual);
    Task<bool> RechazarRevision(int idRevision, string motivos, int idUsuarioActual);
    
    Task<(int idMaestro, int idRepositorio)> AutoCrearMaestroYRepositorio(int idSolicitud);
}
```

**Implementación** (parcial - mostrar estructura):

```csharp
public class GdPncAdapter : IGdPncAdapter
{
    private readonly string _connectionString;
    private readonly ILogger<GdPncAdapter> _logger;

    public GdPncAdapter(IConfiguration config, ILogger<GdPncAdapter> logger)
    {
        _connectionString = config.GetConnectionString("DefaultConnection");
        _logger = logger;
    }

    // ============ SOLICITUD ============

    public async Task<int> CrearSolicitud(PncSolicitudCreateVM vm, int idUsuarioActual)
    {
        using (var connection = new SqlConnection(_connectionString))
        {
            var parameters = new DynamicParameters();
            parameters.Add("@nombreDocumento", vm.NombreDocumento);
            parameters.Add("@idArea", vm.IdArea);
            parameters.Add("@idResponsable", vm.IdResponsable);
            parameters.Add("@descripcion", vm.Descripcion);
            parameters.Add("@idProceso", vm.IdProceso);
            parameters.Add("@creadoPor", idUsuarioActual);
            parameters.Add("@fechaRegistro", DateTime.UtcNow.AddHours(-5));

            // ⚠️ Retorna ID de solicitud creada
            var result = await connection.QueryFirstOrDefaultAsync<int>(
                "GD_SolicitudPNC_Insert",
                parameters,
                commandType: CommandType.StoredProcedure);

            _logger.LogInformation($"Solicitud PNC creada: {result}");
            return result;
        }
    }

    public async Task<PncSolicitudVM> ObtenerSolicitudById(int id)
    {
        using (var connection = new SqlConnection(_connectionString))
        {
            var parameters = new DynamicParameters();
            parameters.Add("@idSolicitud", id);

            var result = await connection.QueryFirstOrDefaultAsync<PncSolicitudVM>(
                "GD_SolicitudPNC_Select",
                parameters,
                commandType: CommandType.StoredProcedure);

            if (result != null)
            {
                // Obtener revisiones asociadas
                result.Revisiones = await ObtenerRevisionesBySolicitud(id);
                result.TotalRevisores = result.Revisiones.Count;
                result.RevisoresAprobados = result.Revisiones.Count(r => r.Estado == "Aprobado");
                result.RevisoresRechazados = result.Revisiones.Count(r => r.Estado == "Rechazado");
            }

            return result;
        }
    }

    public async Task<List<PncSolicitudListVM>> ListarSolicitudes(string filtroEstado = null)
    {
        using (var connection = new SqlConnection(_connectionString))
        {
            var parameters = new DynamicParameters();
            if (!string.IsNullOrEmpty(filtroEstado))
                parameters.Add("@filtroEstado", filtroEstado);

            var result = await connection.QueryAsync<PncSolicitudListVM>(
                "GD_SolicitudPNC_SelectAll",
                parameters,
                commandType: CommandType.StoredProcedure);

            return result.ToList();
        }
    }

    public async Task<bool> CancelarSolicitud(int id)
    {
        using (var connection = new SqlConnection(_connectionString))
        {
            var parameters = new DynamicParameters();
            parameters.Add("@idSolicitud", id);

            var result = await connection.ExecuteAsync(
                "GD_SolicitudPNC_Delete",
                parameters,
                commandType: CommandType.StoredProcedure);

            return result > 0;
        }
    }

    // ============ REVISIÓN ============

    public async Task<int> CrearRevision(int idSolicitud, List<int> idRevisores)
    {
        // ⚠️ Crear una revisión POR cada revisor
        using (var connection = new SqlConnection(_connectionString))
        {
            int ultimoId = 0;

            foreach (var idRevisor in idRevisores)
            {
                var parameters = new DynamicParameters();
                parameters.Add("@idSolicitud", idSolicitud);
                parameters.Add("@idRevisor", idRevisor);
                parameters.Add("@estado", "Pendiente"); // Estado inicial
                parameters.Add("@comentarios", "");
                parameters.Add("@fechaRevision", DBNull.Value);

                ultimoId = await connection.QueryFirstOrDefaultAsync<int>(
                    "GD_RevisionPNC_Insert",
                    parameters,
                    commandType: CommandType.StoredProcedure);
            }

            _logger.LogInformation($"Revisiones PNC creadas para solicitud {idSolicitud}");
            return ultimoId;
        }
    }

    public async Task<PncRevisionVM> ObtenerRevisionById(int id)
    {
        using (var connection = new SqlConnection(_connectionString))
        {
            var parameters = new DynamicParameters();
            parameters.Add("@idRevision", id);

            var result = await connection.QueryFirstOrDefaultAsync<PncRevisionVM>(
                "GD_RevisionPNC_Select",
                parameters,
                commandType: CommandType.StoredProcedure);

            return result;
        }
    }

    public async Task<List<PncRevisionVM>> ObtenerRevisionesBySolicitud(int idSolicitud)
    {
        using (var connection = new SqlConnection(_connectionString))
        {
            var sql = @"
                SELECT 
                    id, idSolicitud, idRevisor, nombreRevisor, emailRevisor,
                    estado, comentarios, fechaRevision
                FROM GD_RevisionPNC
                WHERE idSolicitud = @idSolicitud
                ORDER BY idRevisor
            ";

            var result = await connection.QueryAsync<PncRevisionVM>(sql, new { idSolicitud });
            return result.ToList();
        }
    }

    public async Task<bool> AprobarRevision(int idRevision, string comentarios, int idUsuarioActual)
    {
        using (var connection = new SqlConnection(_connectionString))
        {
            var parameters = new DynamicParameters();
            parameters.Add("@idRevision", idRevision);
            parameters.Add("@estado", "Aprobado");
            parameters.Add("@comentarios", comentarios ?? "");
            parameters.Add("@modificadoPor", idUsuarioActual);
            parameters.Add("@fechaModificacion", DateTime.UtcNow.AddHours(-5));

            var result = await connection.ExecuteAsync(
                "GD_RevisionPNC_Update",
                parameters,
                commandType: CommandType.StoredProcedure);

            return result > 0;
        }
    }

    public async Task<bool> RechazarRevision(int idRevision, string motivos, int idUsuarioActual)
    {
        using (var connection = new SqlConnection(_connectionString))
        {
            var parameters = new DynamicParameters();
            parameters.Add("@idRevision", idRevision);
            parameters.Add("@estado", "Rechazado");
            parameters.Add("@comentarios", motivos);
            parameters.Add("@modificadoPor", idUsuarioActual);
            parameters.Add("@fechaModificacion", DateTime.UtcNow.AddHours(-5));

            var result = await connection.ExecuteAsync(
                "GD_RevisionPNC_Update",
                parameters,
                commandType: CommandType.StoredProcedure);

            return result > 0;
        }
    }

    // ============ AUTO-CREACIÓN ============

    public async Task<(int idMaestro, int idRepositorio)> AutoCrearMaestroYRepositorio(int idSolicitud)
    {
        // ⚠️ CRÍTICO: Obtener datos solicitud PNC
        var solicitud = await ObtenerSolicitudById(idSolicitud);
        if (solicitud == null)
            return (-1, -1);

        using (var connection = new SqlConnection(_connectionString))
        using (var transaction = connection.BeginTransaction())
        {
            try
            {
                connection.Open();

                // 1. Insertar maestro
                var paramsMaestro = new DynamicParameters();
                paramsMaestro.Add("@nombre", solicitud.NombreDocumento);
                paramsMaestro.Add("@codigo", GenerarCodigoDocumento()); // ⚠️ TODO: Generar código único
                paramsMaestro.Add("@idProceso", solicitud.IdProceso); // ⚠️ Requiere agregar a solicitud
                paramsMaestro.Add("@idResponsable", solicitud.IdResponsable);
                paramsMaestro.Add("@creadoPor", solicitud.CreadoPor); // ⚠️ Requiere agregar a solicitud
                paramsMaestro.Add("@fechaRegistro", DateTime.UtcNow.AddHours(-5));

                var idMaestro = await connection.QueryFirstOrDefaultAsync<int>(
                    "GD_MaestroDocumentos_Insert",
                    paramsMaestro,
                    commandType: CommandType.StoredProcedure,
                    transaction: transaction);

                if (idMaestro <= 0)
                    throw new Exception("Error creando maestro");

                // 2. Insertar repositorio v1.0
                var paramsRepo = new DynamicParameters();
                paramsRepo.Add("@idDocumento", idMaestro);
                paramsRepo.Add("@version", "1.0");
                paramsRepo.Add("@rutaArchivo", solicitud.RutaArchivoOriginal); // ⚠️ Requiere agregar a solicitud
                paramsRepo.Add("@creadoPor", solicitud.CreadoPor);
                paramsRepo.Add("@fechaRegistro", DateTime.UtcNow.AddHours(-5));

                var idRepositorio = await connection.QueryFirstOrDefaultAsync<int>(
                    "GD_RepositorioDocumentos_Insert",
                    paramsRepo,
                    commandType: CommandType.StoredProcedure,
                    transaction: transaction);

                if (idRepositorio <= 0)
                    throw new Exception("Error creando repositorio");

                transaction.Commit();
                _logger.LogInformation($"Maestro y Repositorio creados: {idMaestro}, {idRepositorio}");
                return (idMaestro, idRepositorio);
            }
            catch (Exception ex)
            {
                transaction.Rollback();
                _logger.LogError($"Error auto-creando maestro: {ex.Message}");
                return (-1, -1);
            }
        }
    }

    private string GenerarCodigoDocumento()
    {
        // ⚠️ TODO: Generar código único según regla de negocio
        // Ej: GD-{YYYY}-{NNNN} donde NNNN es secuencial
        return $"GD-{DateTime.Now.Year}-{Guid.NewGuid().ToString("N").Substring(0, 4).ToUpper()}";
    }
}
```

**Validación**:
- ✅ Adapter implementado
- ✅ 10+ métodos
- ✅ Transacciones en auto-creación
- ✅ Logging completo

---

### TAREA 8.4: Crear Service PNC (4h)

**Descripción**: Lógica de negocio PNC

**Ubicación**: `Data/Services/GD/GdPncService.cs`

**Interfaz**:

```csharp
public interface IGdPncService
{
    Task<(bool success, int idSolicitud, string message)> CrearSolicitud(PncSolicitudCreateVM vm, IFormFile archivo);
    Task<PncSolicitudVM> ObtenerSolicitudById(int id);
    Task<List<PncSolicitudListVM>> ListarSolicitudes(string filtroEstado = null);
    Task<(bool success, string message)> CancelarSolicitud(int id);
    
    Task<(bool success, string message)> AsignarRevisores(int idSolicitud, List<int> idRevisores);
    Task<(bool success, string message)> AprobarRevision(int idRevision, string comentarios);
    Task<(bool success, string message)> RechazarRevision(int idRevision, string motivos);
}
```

**Implementación** (estructura principal):

```csharp
public class GdPncService : IGdPncService
{
    private readonly IGdPncAdapter _adapter;
    private readonly IGdEmailService _emailService;
    private readonly IUploadService _uploadService; // REGLA 7: Reutilizar
    private readonly ILogger<GdPncService> _logger;
    private readonly ICurrentUserService _currentUser;

    public async Task<(bool success, int idSolicitud, string message)> CrearSolicitud(
        PncSolicitudCreateVM vm, IFormFile archivo)
    {
        try
        {
            // REGLA 12: Validar entrada
            if (vm == null || archivo == null)
                return (false, 0, "Datos incompletos");

            if (archivo.Length > 10 * 1024 * 1024) // 10 MB máximo
                return (false, 0, "Archivo muy grande (máximo 10 MB)");

            // REGLA 7: Usar UploadService para guardar archivo
            var (uploadSuccess, uploadPath) = await _uploadService.GuardarArchivoAsync(
                archivo,
                "PNC",
                _currentUser.Id);

            if (!uploadSuccess)
                return (false, 0, "Error guardando archivo");

            // Crear solicitud en BD
            var idSolicitud = await _adapter.CrearSolicitud(vm, _currentUser.Id);
            if (idSolicitud <= 0)
                return (false, 0, "Error creando solicitud");

            _logger.LogInformation($"Solicitud PNC creada: {idSolicitud}");
            return (true, idSolicitud, "Solicitud PNC creada exitosamente");
        }
        catch (Exception ex)
        {
            _logger.LogError($"Error creando solicitud PNC: {ex.Message}");
            return (false, 0, $"Error: {ex.Message}");
        }
    }

    public async Task<PncSolicitudVM> ObtenerSolicitudById(int id)
    {
        return await _adapter.ObtenerSolicitudById(id);
    }

    public async Task<List<PncSolicitudListVM>> ListarSolicitudes(string filtroEstado = null)
    {
        return await _adapter.ListarSolicitudes(filtroEstado);
    }

    public async Task<(bool success, string message)> CancelarSolicitud(int id)
    {
        try
        {
            var solicitud = await ObtenerSolicitudById(id);
            if (solicitud == null)
                return (false, "Solicitud no encontrada");

            if (solicitud.EstadoActual != "Pendiente")
                return (false, "Solo se pueden cancelar solicitudes en estado Pendiente");

            var result = await _adapter.CancelarSolicitud(id);
            return (result, "Solicitud cancelada");
        }
        catch (Exception ex)
        {
            _logger.LogError($"Error cancelando solicitud: {ex.Message}");
            return (false, $"Error: {ex.Message}");
        }
    }

    public async Task<(bool success, string message)> AsignarRevisores(int idSolicitud, List<int> idRevisores)
    {
        try
        {
            // REGLA 12: Validar
            if (idRevisores == null || idRevisores.Count == 0)
                return (false, "Debe asignar al menos un revisor");

            if (idRevisores.Count > 10) // Límite arbitrario
                return (false, "Máximo 10 revisores permitidos");

            // Crear revisiones
            var resultId = await _adapter.CrearRevision(idSolicitud, idRevisores);
            if (resultId <= 0)
                return (false, "Error asignando revisores");

            // ✅ ENVIAR EMAILS a revisores
            var solicitud = await ObtenerSolicitudById(idSolicitud);
            var emailsRevisores = await ObtenerEmailsRevisores(idRevisores);
            _ = _emailService.NotificarRevisoresPNC(idSolicitud, emailsRevisores, solicitud.NombreDocumento);

            _logger.LogInformation($"Revisores asignados a PNC {idSolicitud}");
            return (true, "Revisores asignados y notificados");
        }
        catch (Exception ex)
        {
            _logger.LogError($"Error asignando revisores PNC: {ex.Message}");
            return (false, $"Error: {ex.Message}");
        }
    }

    public async Task<(bool success, string message)> AprobarRevision(int idRevision, string comentarios = "")
    {
        try
        {
            // Actualizar revisión
            var result = await _adapter.AprobarRevision(idRevision, comentarios, _currentUser.Id);
            if (!result)
                return (false, "Error aprobando");

            // Obtener revisión + solicitud
            var revision = await _adapter.ObtenerRevisionById(idRevision);
            var solicitud = await ObtenerSolicitudById(revision.IdSolicitud);

            // ✅ VERIFICAR si TODAS las revisiones están aprobadas
            var todasAprobadas = solicitud.RevisoresAprobados == solicitud.TotalRevisores;

            if (todasAprobadas)
            {
                // 🔴 CRÍTICO: Auto-crear maestro + repositorio
                var (idMaestro, idRepositorio) = 
                    await _adapter.AutoCrearMaestroYRepositorio(revision.IdSolicitud);

                if (idMaestro <= 0)
                    return (false, "Error creando documento");

                // Actualizar estado solicitud a "Aprobado" + link a nuevo maestro
                // ⚠️ TODO: Implementar método ActualizarEstadoYMaestro

                // ✅ Enviar email aprobación a solicitante
                var emailSolicitante = await ObtenerEmailSolicitante(revision.IdSolicitud);
                _ = _emailService.NotificarAprobacionPNC(
                    revision.IdSolicitud, 
                    emailSolicitante, 
                    idMaestro);

                _logger.LogInformation($"PNC completada - Maestro creado: {idMaestro}");
                return (true, $"PNC aprobada - Documento creado (ID: {idMaestro})");
            }

            return (true, "Revisión aprobada (pendientes otros revisores)");
        }
        catch (Exception ex)
        {
            _logger.LogError($"Error aprobando PNC: {ex.Message}");
            return (false, $"Error: {ex.Message}");
        }
    }

    public async Task<(bool success, string message)> RechazarRevision(int idRevision, string motivos)
    {
        try
        {
            // REGLA 12: Validar
            if (string.IsNullOrWhiteSpace(motivos))
                return (false, "Motivos rechazo requeridos");

            // Actualizar revisión
            var result = await _adapter.RechazarRevision(idRevision, motivos, _currentUser.Id);
            if (!result)
                return (false, "Error rechazando");

            // Obtener revisión + solicitud
            var revision = await _adapter.ObtenerRevisionById(idRevision);
            var solicitud = await ObtenerSolicitudById(revision.IdSolicitud);

            // ⚠️ Si UNA revisión rechazada → marcar solicitud como rechazada inmediatamente
            // ⚠️ TODO: Implementar método ActualizarEstadoRechazado

            // ✅ Enviar email rechazo a solicitante
            var emailSolicitante = await ObtenerEmailSolicitante(revision.IdSolicitud);
            _ = _emailService.NotificarRechazoPNC(
                revision.IdSolicitud,
                emailSolicitante,
                revision.NombreRevisor,
                motivos);

            _logger.LogInformation($"PNC rechazada por {revision.NombreRevisor}");
            return (true, "Revisión rechazada - Solicitante notificado");
        }
        catch (Exception ex)
        {
            _logger.LogError($"Error rechazando PNC: {ex.Message}");
            return (false, $"Error: {ex.Message}");
        }
    }

    // Métodos auxiliares
    private async Task<List<string>> ObtenerEmailsRevisores(List<int> idRevisores)
    {
        // TODO: Query BD
        return new List<string>();
    }

    private async Task<string> ObtenerEmailSolicitante(int idSolicitud)
    {
        // TODO: Query BD
        return "";
    }
}
```

**Validación**:
- ✅ Service implementado
- ✅ Validaciones completas
- ✅ Auto-creación de maestro
- ✅ Email integrado
- ✅ Async/await

---

### TAREA 8.5: Crear PncController (4h)

**Descripción**: Controller CRUD para PNC

**Ubicación**: `Areas/GD/Controllers/PncController.cs`

**Métodos Principales**:

```csharp
[Area("GD")]
[Authorize]
[Route("GD/PNC")]
public class PncController : Controller
{
    private readonly IGdPncService _service;
    private readonly ILogger<PncController> _logger;

    // GET: /GD/PNC
    public async Task<IActionResult> Index(string estado = "")
    {
        var solicitudes = await _service.ListarSolicitudes(estado);
        return View(solicitudes);
    }

    // GET: /GD/PNC/Create
    public IActionResult Create()
    {
        return View();
    }

    // POST: /GD/PNC/Create
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(PncSolicitudCreateVM vm, IFormFile archivo)
    {
        if (!ModelState.IsValid || archivo == null)
            return Json(new { success = false, message = "Datos inválidos" });

        var (success, idSolicitud, message) = await _service.CrearSolicitud(vm, archivo);

        if (success)
            return Json(new { success = true, message, redirectUrl = Url.Action("Detail", new { id = idSolicitud }) });

        return Json(new { success = false, message });
    }

    // GET: /GD/PNC/{id}
    public async Task<IActionResult> Detail(int id)
    {
        var solicitud = await _service.ObtenerSolicitudById(id);
        if (solicitud == null)
            return NotFound();

        if (Request.Headers["X-Requested-With"] == "XMLHttpRequest")
            return PartialView("_DetailModal", solicitud);

        return View(solicitud);
    }

    // POST: /GD/PNC/{id}/AssignReviewers
    [HttpPost]
    public async Task<IActionResult> AssignReviewers(int id, PncAsignReviewersVM vm)
    {
        var (success, message) = await _service.AsignarRevisores(id, vm.IdRevisores);

        return Json(new { success, message });
    }

    // POST: /GD/PNC/{idRevision}/Approve
    [HttpPost]
    public async Task<IActionResult> Approve(int idRevision, PncAprobarVM vm)
    {
        var (success, message) = await _service.AprobarRevision(idRevision, vm.Comentarios);

        return Json(new { success, message });
    }

    // POST: /GD/PNC/{idRevision}/Reject
    [HttpPost]
    public async Task<IActionResult> Reject(int idRevision, PncRechazarVM vm)
    {
        var (success, message) = await _service.RechazarRevision(idRevision, vm.MotivosRechazo);

        return Json(new { success, message });
    }

    // GET: /GD/PNC/{id}/Cancel
    public async Task<IActionResult> Cancel(int id)
    {
        var (success, message) = await _service.CancelarSolicitud(id);

        if (success)
            return Json(new { success = true, message, redirectUrl = Url.Action("Index") });

        return Json(new { success = false, message });
    }
}
```

**Validación**:
- ✅ Controller compilable
- ✅ 6+ métodos CRUD
- ✅ AJAX support
- ✅ Autorización

---

### TAREA 8.6: Crear Vistas PNC (4h)

**Descripción**: 5+ vistas para PNC

**Vistas a Crear**:

1. **Index.cshtml** - Listado solicitudes PNC
2. **Create.cshtml** - Crear nueva solicitud
3. **_CreateModal.cshtml** - Modal para crear
4. **_DetailModal.cshtml** - Detalle solicitud
5. **_AssignReviewersModal.cshtml** - Asignar revisores
6. **_ReviewDetailModal.cshtml** - Detalles revisión

**Vista Principal** (Index.cshtml):

```html
@model List<PncSolicitudListVM>

@{ ViewData["Title"] = "Solicitudes PNC"; }

<div class="container-fluid mt-4">
    <h2>Proceso Nueva Creación (PNC)</h2>

    <div class="mb-3">
        <a href="#" class="btn btn-primary" data-bs-toggle="modal" data-bs-target="#createModal">
            ➕ Nueva Solicitud
        </a>
    </div>

    <!-- Filtros -->
    <div class="card mb-3">
        <div class="card-body">
            <form id="filterForm" method="get" class="row g-3">
                <div class="col-md-3">
                    <select name="estado" class="form-select" onchange="document.getElementById('filterForm').submit();">
                        <option value="">Todos los estados</option>
                        <option value="Pendiente">Pendiente</option>
                        <option value="EnRevision">En Revisión</option>
                        <option value="Aprobado">Aprobado</option>
                        <option value="Rechazado">Rechazado</option>
                    </select>
                </div>
            </form>
        </div>
    </div>

    <!-- Tabla -->
    <div class="table-responsive">
        <table class="table table-hover">
            <thead class="table-light">
                <tr>
                    <th>Documento</th>
                    <th>Área</th>
                    <th>Solicitante</th>
                    <th>Estado</th>
                    <th>Revisores</th>
                    <th>Fecha</th>
                    <th>Acciones</th>
                </tr>
            </thead>
            <tbody>
                @foreach (var item in Model)
                {
                    <tr>
                        <td>@item.NombreDocumento</td>
                        <td>@item.Area</td>
                        <td>@item.Solicitante</td>
                        <td>
                            <span class="badge" style="background-color: @GetColorEstado(item.Estado)">
                                @item.Estado
                            </span>
                        </td>
                        <td>@item.RevisoresAprobados / @item.TotalRevisores</td>
                        <td><small>@item.FechaRegistro.ToString("dd/MM/yyyy")</small></td>
                        <td>
                            <a href="#" class="btn btn-sm btn-info" 
                               onclick="loadDetail(@item.Id)">Ver</a>
                        </td>
                    </tr>
                }
            </tbody>
        </table>
    </div>
</div>

<!-- Modal Crear -->
<div id="createModal" class="modal fade" tabindex="-1">
    <div class="modal-dialog modal-lg">
        <div class="modal-content">
            <div class="modal-header">
                <h5>Nueva Solicitud PNC</h5>
                <button type="button" class="btn-close" data-bs-dismiss="modal"></button>
            </div>
            <form id="formCreate" enctype="multipart/form-data">
                <div class="modal-body">
                    <div class="mb-3">
                        <label class="form-label">Nombre Documento *</label>
                        <input type="text" name="nombreDocumento" class="form-control" required>
                    </div>
                    <div class="mb-3">
                        <label class="form-label">Área *</label>
                        <select name="idArea" class="form-select" required>
                            <option value="">Seleccionar...</option>
                            <!-- Opciones dinámicas -->
                        </select>
                    </div>
                    <div class="mb-3">
                        <label class="form-label">Responsable *</label>
                        <select name="idResponsable" class="form-select" required>
                            <option value="">Seleccionar...</option>
                        </select>
                    </div>
                    <div class="mb-3">
                        <label class="form-label">Proceso *</label>
                        <select name="idProceso" class="form-select" required>
                            <option value="">Seleccionar...</option>
                        </select>
                    </div>
                    <div class="mb-3">
                        <label class="form-label">Descripción</label>
                        <textarea name="descripcion" class="form-control" rows="4"></textarea>
                    </div>
                    <div class="mb-3">
                        <label class="form-label">Archivo Documento *</label>
                        <input type="file" name="archivo" class="form-control" required accept=".pdf,.doc,.docx">
                    </div>
                </div>
                <div class="modal-footer">
                    <button type="button" class="btn btn-secondary" data-bs-dismiss="modal">Cancelar</button>
                    <button type="submit" class="btn btn-primary">Crear Solicitud</button>
                </div>
            </form>
        </div>
    </div>
</div>

@section Scripts {
    <script>
        // JavaScript para manejar modal crear
        document.getElementById('formCreate').addEventListener('submit', async (e) => {
            e.preventDefault();
            const formData = new FormData(e.target);
            
            const response = await fetch('/GD/PNC/Create', {
                method: 'POST',
                body: formData
            });
            
            const result = await response.json();
            if (result.success) {
                window.location.href = result.redirectUrl;
            }
        });

        function loadDetail(id) {
            // Cargar modal detail
            fetch(`/GD/PNC/${id}?X-Requested-With=XMLHttpRequest`)
                .then(r => r.text())
                .then(html => {
                    document.body.insertAdjacentHTML('beforeend', html);
                    new bootstrap.Modal(document.querySelector('#detailModal')).show();
                });
        }

        function GetColorEstado(estado) {
            return estado switch
            {
                'Aprobado' => '#28a745',
                'Rechazado' => '#dc3545',
                'En Revisión' => '#ffc107',
                _ => '#6c757d'
            };
        }
    </script>
}
```

**Validación**:
- ✅ Vistas compilables
- ✅ Formularios funcionales
- ✅ Modales Bootstrap
- ✅ JavaScript AJAX

---

### TAREA 8.7: Expandir Email Service - PNC (1h)

**Descripción**: Agregar notificaciones PNC a GdEmailService

**Métodos a Agregar**:

```csharp
public async Task<bool> NotificarRevisoresPNC(int idSolicitud, List<string> emailsRevisores, string nombreDocumento)
{
    // Similar a NotificarRevisoresSolicitud pero para PNC
    // Template: `EmailTemplates/GD/PncSolicitudCreada.html`
}

public async Task<bool> NotificarAprobacionPNC(int idSolicitud, string emailSolicitante, int idMaestroCreado)
{
    // Notificar que documento fue creado exitosamente
    // Template: `EmailTemplates/GD/PncAprobado.html`
}

public async Task<bool> NotificarRechazoPNC(int idSolicitud, string emailSolicitante, string revisor, string motivos)
{
    // Notificar rechazo
    // Template: `EmailTemplates/GD/PncRechazado.html`
}
```

**Validación**:
- ✅ Métodos agregados
- ✅ Async/await

---

### TAREA 8.8: Crear Templates Email PNC (1h)

**Descripción**: 3 templates HTML para PNC

**Ubicación**: `wwwroot/EmailTemplates/GD/PNC/`

**Archivos**:
- `PncSolicitudCreada.html` - Notificación a revisores
- `PncAprobado.html` - Notificación documento creado
- `PncRechazado.html` - Notificación rechazo

*(Estructura similar a templates de Solicitudes de TAREA 6.2)*

**Validación**:
- ✅ 3 templates creados
- ✅ HTML válido
- ✅ Variables Razor correctas

---

### TAREA 8.9: Expandir Program.cs - Registrar PNC (0.5h)

**Descripción**: Registrar servicios PNC en DI

```csharp
// PNC
builder.Services.AddScoped<IGdPncAdapter, GdPncAdapter>();
builder.Services.AddScoped<IGdPncService, GdPncService>();
```

**Validación**:
- ✅ Servicios registrados
- ✅ Compilación exitosa

---

### TAREA 8.10: Testing PNC Completo (2h)

**Descripción**: Validar flujo PNC end-to-end

**Escenarios**:

1. **Crear Solicitud PNC**:
   - [ ] Acceder a /GD/PNC
   - [ ] Click "Nueva Solicitud"
   - [ ] Cargar archivo
   - [ ] Enviar formulario
   - [ ] Solicitud creada en BD
   - [ ] Estado = "Pendiente"

2. **Asignar Revisores**:
   - [ ] Click "Asignar Revisores" en solicitud
   - [ ] Seleccionar 2-3 revisores
   - [ ] Enviar
   - [ ] Revisiones creadas en BD (1 por revisor)
   - [ ] Emails enviados a revisores

3. **Aprobar Revisión**:
   - [ ] Revisor accede a `/GD/Aprobaciones`
   - [ ] Click "Aprobar"
   - [ ] Confirmación guardada
   - [ ] Revisor muestra "Aprobado"

4. **Todos Aprueban → Auto-Crear Maestro**:
   - [ ] Último revisor aprueba
   - [ ] Maestro creado automáticamente
   - [ ] Repositorio v1.0 creado
   - [ ] Email aprobación enviado
   - [ ] Link a nuevo documento incluido en email

5. **Rechazo Inmediato**:
   - [ ] Cualquier revisor rechaza
   - [ ] Estado solicitud → "Rechazado"
   - [ ] Email rechazo enviado
   - [ ] Maestro NO se crea

**Validación**:
- ✅ Flujo completo funcional
- ✅ BD consistente
- ✅ Emails correctos
- ✅ Estados correctos

---

### Registro de Completitud - Sprint 8

| Tarea | Horas | Estado |
|-------|-------|--------|
| 8.1 Mapear SPs | 1.5h | ⏳ |
| 8.2 ViewModels | 2h | ⏳ |
| 8.3 Adapter | 3h | ⏳ |
| 8.4 Service | 4h | ⏳ |
| 8.5 Controller | 4h | ⏳ |
| 8.6 Vistas | 4h | ⏳ |
| 8.7 Email Service | 1h | ⏳ |
| 8.8 Templates Email | 1h | ⏳ |
| 8.9 Program.cs | 0.5h | ⏳ |
| 8.10 Testing | 2h | ⏳ |
| **TOTAL SPRINT 8** | **40h** | **⏳** |

---

## ✅ CRITERIOS DE ÉXITO - FASE 5 PARTE A

**DEBE CUMPLIRSE ANTES DE PASAR A PARTE B**:

1. ✅ Crear solicitud PNC funcional
2. ✅ Asignar revisores funcional
3. ✅ Aprobación/rechazo funcional
4. ✅ Auto-creación maestro al 100% aprobar
5. ✅ Repositorio v1.0 auto-creado
6. ✅ 3 tipos email PNC enviados
7. ✅ 0 errores compilación
8. ✅ Commit cambios

---

**Fin de FASE 5 PARTE A**

→ Próxima: [FASE 5 PARTE B - Escáner + Config]

