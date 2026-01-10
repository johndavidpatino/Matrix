# Testing Plan - Email Asíncrono (FASE 4 Sprint 6)

**Fecha**: 2026-01-10  
**Sprint**: FASE 4 Sprint 6 - Tarea 6.7  
**Horas Estimadas**: 1h

---

## 📋 Objetivo

Validar que el sistema de notificaciones por email funciona correctamente:
- ✅ Emails se encolan sin bloquear request HTTP
- ✅ BackgroundService procesa emails correctamente
- ✅ Template HTML renderiza correctamente
- ✅ Links funcionales
- ✅ Logs completos

---

## 🧪 Tests Requeridos

### Test 1: Crear Solicitud + Asignar Revisores → Email Encolado

**Pasos**:
1. Navegar a `/GD/Solicitudes/Crear`
2. Llenar formulario:
   - Tipo Solicitud: 1 (Construcción)
   - Documento: Seleccionar cualquiera
   - Solicitante: Usuario de prueba
   - Área: "TI"
   - Cargo: "Desarrollador"
   - Razón: "Prueba email"
   - Descripción: "Testing notificaciones email Sprint 6"
3. Guardar solicitud
4. Ir a `/GD/Solicitudes/Detalle/{id}` → Asignar Revisores
5. Seleccionar 2-3 revisores
6. Guardar

**Resultado Esperado**:
- ✅ Request se completa inmediatamente (< 500ms)
- ✅ Logs muestran: "Notificaciones para solicitud {id}: X/Y notificaciones encoladas correctamente"
- ✅ Logs muestran: "Email encolado para revisor {nombre} ({email})"
- ✅ `GetQueueDepth()` > 0

---

### Test 2: BackgroundService Procesa Cola

**Pasos**:
1. Esperar 5-10 segundos después del Test 1
2. Verificar logs del BackgroundService

**Resultado Esperado**:
- ✅ Logs muestran: "Processing email queue" cada 5 segundos
- ✅ Logs muestran: "Email sent successfully to {email}"
- ✅ `GetQueueDepth()` = 0 (cola vacía)
- ✅ `GetStats().ProcessedCount` incrementó en N (cantidad de revisores)

---

### Test 3: Contenido de Email Correcto

**Pasos**:
1. Revisar inbox de email de revisor de prueba
2. Abrir email "📋 Nueva Solicitud de Documento"

**Resultado Esperado**:
- ✅ Subject: "📋 Nueva Solicitud de Documento - {IdDocumento}"
- ✅ Nombre revisor correcto
- ✅ Nombre documento presente
- ✅ Solicitante presente
- ✅ Área, Cargo, Razón, Descripción presentes
- ✅ Fecha solicitud formateada (dd/MM/yyyy HH:mm)
- ✅ HTML renderiza correctamente (no se ven {{variables}})
- ✅ Colores y estilos correctos

---

### Test 4: Links Funcionales

**Pasos**:
1. En email recibido, click en "🔍 Revisar Solicitud"
2. Verificar redirección

**Resultado Esperado**:
- ✅ Redirige a `/GD/Solicitudes/Detalle/{id}`
- ✅ Muestra detalles correctos de solicitud
- ✅ Botones Aprobar/Rechazar visibles (si está implementado)

---

### Test 5: Múltiples Revisores

**Pasos**:
1. Crear solicitud
2. Asignar 5 revisores
3. Verificar logs

**Resultado Esperado**:
- ✅ Logs: "5/5 notificaciones encoladas correctamente"
- ✅ 5 emails encolados (verificar `GetQueueDepth()`)
- ✅ Todos los revisores reciben email

---

### Test 6: Manejo de Errores - Email Inválido

**Pasos**:
1. Modificar temporalmente `ObtenerEmailUsuario()` para retornar email inválido o vacío
2. Crear solicitud + asignar revisor
3. Verificar logs

**Resultado Esperado**:
- ✅ Logs: "Revisor {id} no tiene email configurado"
- ✅ Mensaje: "X/Y notificaciones encoladas correctamente. Errores: Revisor {nombre} sin email"
- ✅ Request no falla (success = true parcial)

---

### Test 7: Template No Encontrado

**Pasos**:
1. Renombrar temporalmente `SolicitudCreada.html` → `SolicitudCreada_backup.html`
2. Crear solicitud + asignar revisores
3. Verificar logs

**Resultado Esperado**:
- ✅ Logs: "Template de email no encontrado: {path}"
- ✅ Mensaje: "Template no encontrado: ..."
- ✅ Success = false
- ✅ Request no genera exception (error manejado)

---

### Test 8: Reintentos Automáticos

**Pasos**:
1. Configurar EmailSettings en appsettings.json con SMTP inválido (puerto incorrecto)
2. Crear solicitud + asignar revisor
3. Esperar 15-20 segundos
4. Verificar logs

**Resultado Esperado**:
- ✅ Email encolado correctamente
- ✅ BackgroundService intenta enviar
- ✅ Falla con `SmtpException`
- ✅ Se reintenta automáticamente (MaxRetries = 3)
- ✅ Logs: "Email failed after 3 retries"
- ✅ `GetStats().FailedCount` incrementa

---

## 📊 Métricas de Calidad

### Cobertura
- ✅ Happy path (solicitud → revisores → emails OK)
- ✅ Errores manejados (email inválido, template no encontrado)
- ✅ Edge cases (múltiples revisores, reintentos)

### Performance
- ✅ Request completa en < 500ms (sin esperar envío email)
- ✅ Queue processing cada 5 segundos
- ✅ No memory leaks (monitorear `GetQueueDepth()`)

### Logging
- ✅ Logs informativos (success cases)
- ✅ Logs de warning (email inválido)
- ✅ Logs de error (exceptions)
- ✅ Structured logging con {variables}

---

## 🔍 Comandos de Verificación

### Verificar cola de emails
```csharp
// En controller o service
var queueDepth = _emailQueueService.GetQueueDepth();
var stats = _emailQueueService.GetStats();
_logger.LogInformation("Queue Depth: {Depth}, Processed: {Processed}, Failed: {Failed}", 
    queueDepth, stats.ProcessedCount, stats.FailedCount);
```

### Logs esperados (success)
```
[INF] Iniciando envío de notificación para solicitud 123
[INF] Email encolado para revisor Juan Pérez (juan.perez@example.com)
[INF] Email encolado para revisor María García (maria.garcia@example.com)
[INF] Notificaciones para solicitud 123: 2/2 notificaciones encoladas correctamente
[INF] Processing email queue
[INF] Email sent successfully to juan.perez@example.com
[INF] Email sent successfully to maria.garcia@example.com
```

### Logs esperados (error parcial)
```
[WRN] Revisor 456 no tiene email configurado
[INF] Notificaciones para solicitud 123: 1/2 notificaciones encoladas correctamente. Errores: Revisor Sin Email sin email
```

---

## ✅ Checklist Final

**Compilación**:
- [x] Proyecto compila sin errores
- [x] Todas las dependencias inyectadas correctamente

**Funcionalidad**:
- [ ] Test 1: Email encolado (verificar logs)
- [ ] Test 2: BackgroundService procesa (verificar logs)
- [ ] Test 3: Contenido email correcto (verificar inbox)
- [ ] Test 4: Links funcionales (click en email)
- [ ] Test 5: Múltiples revisores (verificar N emails)
- [ ] Test 6: Error manejo email inválido (verificar logs)
- [ ] Test 7: Error template no encontrado (verificar logs)
- [ ] Test 8: Reintentos automáticos (verificar logs)

**Logs**:
- [ ] Sin errores no manejados
- [ ] Logs informativos presentes
- [ ] Warnings para casos edge
- [ ] Errors con stack trace completo

---

## 🚨 Problemas Conocidos

### ~~Problema 1: ObtenerEmailUsuario() es TEMPORAL~~ ✅ RESUELTO

**Descripción**: Método `ObtenerEmailUsuario()` en `GdEmailService.cs` retornaba email de ejemplo

**Impacto**: ~~Emails no se envían a destinatarios reales~~ → **RESUELTO**

**Solución Implementada**: 
Consulta directa a tabla `US_Usuarios` usando Dapper

**Código implementado**:
```csharp
// En GdEmailService.cs (IMPLEMENTADO)
private async Task<string> ObtenerEmailUsuario(int idUsuario)
{
    using var conn = new SqlConnection(_connectionString);
    await conn.OpenAsync();
    
    var email = await conn.QueryFirstOrDefaultAsync<string>(
        "SELECT Email FROM US_Usuarios WHERE Id = @Id",
        new { Id = idUsuario }
    );

    return string.IsNullOrWhiteSpace(email) ? string.Empty : email;
}
```

**Estado**: ✅ COMPLETADO

---

### Problema 2: NombreDocumento y Solicitante son IDs

**Descripción**: Template muestra IDs en lugar de nombres:
```csharp
NombreDocumento = solicitud.IdDocumento.ToString(), // TODO
Solicitante = solicitud.IdSolicitante.ToString(), // TODO
```

**Impacto**: Email muestra "Documento: 123" en lugar de "Documento: Manual de Calidad"

**Solución**:
1. Modificar `ObtenerSolicitudById()` para incluir joins con tablas relacionadas
2. O crear nuevo método `ObtenerSolicitudConDetalles()` que retorne DTO completo
3. Actualizar template rendering con nombres reales

---

### Problema 3: appsettings.json EmailSettings

**Descripción**: Configuración SMTP puede no estar presente o ser inválida

**Validación Requerida**:
```bash
# Verificar EmailSettings en appsettings.json
grep -A 8 "EmailSettings" MatrixNext.Web/appsettings.json
```

**Configuración Ejemplo**:
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

---

## 📝 Conclusión Testing

**Estado**: 🟡 PENDIENTE (requiere ejecución manual)

**Próximos Pasos**:
1. Ejecutar Tests 1-8 manualmente
2. Verificar logs en cada caso
3. Revisar inbox de email de prueba
4. Documentar resultados
5. Resolver Problemas Conocidos 1-3 (si es necesario)
6. Commit final de Sprint 6

**Criterios de Aceptación para Completar Tarea 6.7**:
- ✅ Al menos Tests 1-4 ejecutados exitosamente
- ✅ Logs muestran funcionamiento correcto
- ✅ Email recibido con contenido correcto (aunque sea @example.com)
- ✅ Sin errores no manejados

---

**Actualizado**: 2026-01-10  
**Responsable**: GitHub Copilot  
**Referencia**: BACKLOG_MIGRACION_GD_DOCUMENTOS_FASE4.md § Sprint 6
