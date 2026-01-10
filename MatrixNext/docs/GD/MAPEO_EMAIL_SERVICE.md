# Integración Email Service - MatrixNext

**Fase**: FASE 4, Sprint 6, Tarea 6.1  
**Fecha**: 2026-01-10  
**Estado**: ✅ COMPLETADO

---

## 📧 BackgroundService Email MatrixNext

### Ubicación de Componentes

```
MatrixNext.Web/Services/
├── IEmailService.cs                    (interfaz básica de envío)
├── EmailService.cs                     (implementación SMTP)
├── IEmailQueueService.cs               (interfaz cola asíncrona)
├── EmailQueueService.cs                (implementación cola in-memory)
└── EmailQueueBackgroundService.cs      (BackgroundService procesador)
```

---

## 📋 API Documentada

### IEmailService (Envío Directo - Síncrono)

**Propósito**: Interfaz base para envío de emails SMTP

```csharp
public interface IEmailService
{
    Task<bool> EnviarAsync(string destinatario, string asunto, string cuerpo, bool esHtml = true);
    Task<bool> EnviarMultipleAsync(List<string> destinatarios, string asunto, string cuerpo);
    Task<bool> EnviarConArchivosAsync(string destinatario, string asunto, string cuerpo, List<string> rutasArchivos);
}
```

**Uso** (NO recomendado para requests HTTP - bloquea el thread):
```csharp
await _emailService.EnviarAsync("user@example.com", "Test", "<h1>Body</h1>", esHtml: true);
```

---

### IEmailQueueService (Envío Asíncrono - Recomendado)

**Propósito**: Cola de emails para procesamiento en background SIN bloquear request HTTP

```csharp
public interface IEmailQueueService
{
    Task QueueEmailAsync(string destinatario, string asunto, string cuerpo, bool esHtml = true);
    Task QueueEmailMultipleAsync(List<string> destinatarios, string asunto, string cuerpo);
    Task QueueEmailConArchivosAsync(string destinatario, string asunto, string cuerpo, List<string> rutasArchivos);
    
    int GetQueueDepth();                    // Obtener tamaño de cola
    EmailQueueStats GetStats();             // Obtener estadísticas
}
```

**Uso Recomendado** (asíncrono, sin bloqueo):
```csharp
// En un controller o service
await _emailQueueService.QueueEmailAsync(
    destinatario: "user@example.com",
    asunto: "Nueva Solicitud",
    cuerpo: htmlBody,
    esHtml: true
);

// Request se completa inmediatamente, email se envía en background
```

**Ventajas**:
- ✅ No bloquea request HTTP
- ✅ Reintentos automáticos (hasta 3 intentos)
- ✅ Logging completo
- ✅ Stats para monitoreo
- ✅ In-memory (sin dependencias externas como Hangfire)

---

### EmailQueueBackgroundService

**Propósito**: BackgroundService que procesa la cola cada 5 segundos

**Características**:
- **Intervalo**: 5 segundos entre procesamiento
- **Reintentos**: 3 intentos máximo por email
- **Scope**: Crea scope para cada ciclo (DI correcto)
- **Logging**: Registra éxitos y fallos

**⚠️ IMPORTANTE**: Ya está registrado en `Program.cs`, **NO requiere modificación**

---

## ⚙️ Configuración

### appsettings.json

**Ubicación**: `MatrixNext.Web/appsettings.json`

```json
{
  "EmailSettings": {
    "SmtpServer": "smtp.gmail.com",
    "SmtpPort": 587,
    "SenderEmail": "noreply@matrix.local",
    "SenderName": "Matrix - Gestión Documental",
    "Username": "your-email@gmail.com",
    "Password": "your-app-password",
    "EnableSsl": true,
    "Timeout": 30000
  }
}
```

**Verificación**: 
```bash
# Buscar en appsettings.json
grep -A 8 "EmailSettings" appsettings.json
```

---

### Program.cs (Registro en DI)

**Estado**: ✅ Ya registrados (verificado en análisis)

```csharp
// Email services
builder.Services.AddScoped<IEmailService, EmailService>();
builder.Services.AddSingleton<IEmailQueueService, EmailQueueService>();
builder.Services.AddHostedService<EmailQueueBackgroundService>();
```

**⚠️ NO MODIFICAR**: Ya están registrados correctamente

---

## 📄 Templates de Email

### Ubicación Propuesta

```
MatrixNext.Web/wwwroot/EmailTemplates/GD/
└── SolicitudCreada.html
```

### Soporte de Templates

**Métodos disponibles**:

1. **String.Replace Simple** (NO Razor completo):
   ```csharp
   var template = await File.ReadAllTextAsync(templatePath);
   var email = template
       .Replace("{{NombreDocumento}}", solicitud.NombreDocumento)
       .Replace("{{Solicitante}}", solicitud.Solicitante);
   ```

2. **RazorEngine** (Requiere NuGet):
   ```bash
   dotnet add package RazorEngine
   ```
   ```csharp
   using RazorEngine;
   var result = Engine.Razor.RunCompile(template, "key", null, model);
   ```

**Recomendación para FASE 4**: Usar String.Replace (sin dependencias adicionales)

---

## 🔍 Ejemplos de Uso en MatrixNext

### Ejemplo 1: OpMuestraService

**Ubicación**: `MatrixNext.Web/Services/OP/OpMuestraService.cs` (línea 266)

```csharp
await _emailService.EnviarAsync(
    destinatario: coordinador.Email,
    asunto: "Nueva Muestra Asignada",
    cuerpo: cuerpoHtml,
    esHtml: true
);
```

**⚠️ Problema**: Usa `IEmailService` directo (bloquea request), debería usar `IEmailQueueService`

---

### Ejemplo 2: OpProgramacionService

**Ubicación**: `MatrixNext.Web/Services/OP/OpProgramacionService.cs` (línea 22)

```csharp
private readonly IEmailQueueService _emailQueueService;

// En método:
await _emailQueueService.QueueEmailAsync(
    destinatario: usuario.Email,
    asunto: "Notificación Programación",
    cuerpo: htmlBody
);
```

**✅ Correcto**: Usa `IEmailQueueService` (sin bloqueo)

---

## 📦 Estructura para GD_Documentos

### Propuesta de Implementación

```csharp
// MatrixNext.Data/Services/GD/GdEmailService.cs
public class GdEmailService : IGdEmailService
{
    private readonly IEmailQueueService _emailQueueService;
    private readonly ILogger<GdEmailService> _logger;

    public async Task<bool> NotificarRevisoresSolicitud(int idSolicitud, List<string> emailsRevisores)
    {
        // 1. Obtener datos solicitud
        // 2. Cargar template HTML
        // 3. Replace variables
        // 4. QueueEmailAsync para cada revisor
        
        foreach (var email in emailsRevisores)
        {
            await _emailQueueService.QueueEmailAsync(email, asunto, cuerpoHtml);
        }
        
        return true;
    }
}
```

**Registro en DI** (agregar en `Program.cs`):
```csharp
builder.Services.AddScoped<IGdEmailService, GdEmailService>();
```

---

## ✅ Validación de Integración

### Checklist de Verificación

- [x] IEmailService existe y compilable
- [x] IEmailQueueService existe y compilable
- [x] EmailQueueBackgroundService existe y funcional
- [x] Servicios registrados en Program.cs
- [x] EmailSettings en appsettings.json (verificar configuración)
- [x] Ejemplos de uso en OP_* módulos funcionando
- [ ] **Tarea 6.2**: Crear template SolicitudCreada.html
- [ ] **Tarea 6.3**: Implementar GdEmailService

---

## 📊 Estadísticas de Email Queue

### Métodos de Monitoreo

```csharp
// Obtener tamaño de cola
var queueDepth = _emailQueueService.GetQueueDepth();

// Obtener stats
var stats = _emailQueueService.GetStats();
Console.WriteLine($"Procesados: {stats.ProcessedCount}");
Console.WriteLine($"Fallidos: {stats.FailedCount}");
Console.WriteLine($"En Cola: {stats.QueuedCount}");
Console.WriteLine($"Último proceso: {stats.LastProcessedTime}");
```

---

## 🚨 Errores Comunes

### Error 1: Email no se envía

**Causa**: `IEmailService` inyectado en lugar de `IEmailQueueService`

**Solución**:
```csharp
// ❌ INCORRECTO (bloquea request)
private readonly IEmailService _emailService;

// ✅ CORRECTO (asíncrono)
private readonly IEmailQueueService _emailQueueService;
```

---

### Error 2: EmailSettings no configurado

**Síntoma**: `SmtpException: Authentication failed`

**Solución**: Verificar `appsettings.json` tiene EmailSettings completo

---

### Error 3: Template no encontrado

**Síntoma**: `FileNotFoundException: ...EmailTemplates/GD/...`

**Solución**: Verificar ruta absoluta
```csharp
var templatePath = Path.Combine(
    Directory.GetCurrentDirectory(), 
    "wwwroot", 
    "EmailTemplates", 
    "GD", 
    "SolicitudCreada.html"
);

if (!File.Exists(templatePath))
    throw new FileNotFoundException($"Template no encontrado: {templatePath}");
```

---

## 📝 Conclusiones de Tarea 6.1

✅ **COMPLETADO**: BackgroundService Email localizado e integración documentada

**Hallazgos**:
1. Sistema de email asíncrono **YA EXISTE** y está completo
2. No requiere instalación de dependencias externas
3. API simple y bien documentada (`IEmailQueueService`)
4. BackgroundService procesa cada 5 segundos
5. Reintentos automáticos (3 intentos)
6. Stats disponibles para monitoreo

**Próxima Tarea**: 6.2 - Crear template HTML para notificación a revisores

---

**Actualizado**: 2026-01-10  
**Autor**: GitHub Copilot  
**Referencia**: MatrixNext.Web/Services/IEmailService.cs, EmailQueueService.cs, EmailQueueBackgroundService.cs
