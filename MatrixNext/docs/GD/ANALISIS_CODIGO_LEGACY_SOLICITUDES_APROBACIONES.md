# Análisis del Código Legacy - Solicitudes y Aprobaciones GD

**Fecha**: 10 de Enero de 2026  
**Sprint**: 5 - Tarea 5.1 (Investigation)  
**Archivos Revisados**:
- `WebMatrix/GD_Documentos/GD_SolicitudDocumentos.aspx.vb` (224 líneas)
- `WebMatrix/GD_Documentos/Aprobacion.aspx.vb` (133 líneas)
- `CoreProject/Clases/GD/GD_Procedimientos.vb` (631 líneas)
- SPs: `GD_Revisiones_Edit`, `GD_Revisiones_Get`, `GD_Revisiones_GetRev`

---

## 1. FLUJO DE SOLICITUDES (Actual en WebMatrix)

### 1.1 Creación de Solicitud
**Archivo**: `GD_SolicitudDocumentos.aspx.vb`

```vb
' Líneas 160-176: CONSTRUCCIÓN (TipoSolicitud = 1)
Public Sub Construccion()
    ' Validaciones (nombre, código, área, cargo)
    
    ' 1. Insertar documento en maestro
    Dim lastid As Integer = GDProc.IngresarDocumentoMaestro2(...)
    
    ' 2. Insertar solicitud
    GDProc.IngresarSolicitudDocumento(Date.UtcNow.AddHours(-5), ddlSolicitante.SelectedValue, 
        txtArea.Text, txtCargo.Text, ddlTipoSolicitud.SelectedValue, lastid, ...)
    
    ' 3. Crear registro en DocumentosControlados
    Dim DCLastId As Integer = GDProc.IngresarDocumentoControlado(lastid, False, "", "", "", "")
    
    ' 4. ASIGNAR REVISORES Y ENVIAR CORREO
    EnviarYRevisar(DCLastId)
End Sub
```

### 1.2 Asignación de Revisores
**Archivo**: `GD_SolicitudDocumentos.aspx.vb`, líneas 138-152

```vb
Sub EnviarYRevisar(ByVal UltimoId As Integer)
    Dim List As List(Of Usuarios_Result)
    Dim mailUsu As New List(Of String)
    List = Session("Usuarios")  ' <- USUARIOS GUARDADOS EN SESSION
    
    For Each a As Usuarios_Result In List
        Try
            mailUsu.Add(a.Email)
            ' IMPORTANTE: TipoRevision = 1 (PENDIENTE)
            GDProc.guardarRevision(UltimoId, a.id, DateTime.UtcNow.AddHours(-5), 1)
        Catch ex As Exception
            Throw ex
        End Try
    Next
    
    ' Enviar correo a todos los revisores
    Dim sm As New EnviarCorreo
    sm.sendMail(mailUsu, "PRUEBA", txtContenido.Content.ToString)
End Sub
```

**HALLAZGOS CRÍTICOS**:
1. ✅ Los revisores se asignan **TODOS AL MISMO TIEMPO** después de crear la solicitud
2. ✅ Estado inicial de revisión: **TipoRevision = 1** (Pendiente)
3. ⚠️ La lista de usuarios viene de **Session("Usuarios")** (no parametrizado en POST)
4. ✅ El correo se envía a **TODOS los revisores simultáneamente**
5. ✅ `DocumentoId` en `GD_Revisiones` es el ID del **DocumentoControlado** (no MaestroDocumento)

---

## 2. FLUJO DE APROBACIONES (Actual en WebMatrix)

### 2.1 Consulta de Documentos Pendientes
**Archivo**: `Aprobacion.aspx.vb`, líneas 37-48

```vb
Public Function ConsultarRevision(ByVal IdUsuario As Integer) As List(Of GD_Revisiones_Get_Result)
    Dim Data As New GD.GD_Procedimientos
    Dim Info As List(Of GD_Revisiones_Get_Result)
    Try
        Info = Data.ObtenerRevisionAprobarUsuario(IdUsuario)  ' <- SP GD_Revisiones_GetRev
        Return Info
    Catch ex As Exception
        Throw ex
    End Try
End Function
```

**SP `GD_Revisiones_GetRev`** (líneas 21563-21583 en CO_Matrix_Structure_SP.sql):
```sql
SELECT IdRevision, R.DocumentoId, UsuarioId, FechaAprobacion,
       R.TipoRevision AS TipoRevisionId, 
       TR.Revision as TipoRevision,
       DC.DocumentoId as DocumentoControladoId,
       MD.Documento as NombreDocumento
FROM GD_Revisiones R
INNER JOIN GD_TipoRevision TR ON TR.IdTipoRevision = R.TipoRevision 
INNER JOIN GD_DocumentosControlados DC ON DC.Id = R.DocumentoId 
INNER JOIN GD_MaestroDocumentos MD ON MD.IdDocumento = DC.DocumentoId 
WHERE UsuarioId = @UsuarioId AND TipoRevision = 2  -- <- SOLO APROBADAS (Estado 2)
```

**IMPORTANTE**: Este SP filtra por `TipoRevision = 2` (YA APROBADAS), **NO por pendientes**.

### 2.2 Aprobación Individual
**Archivo**: `Aprobacion.aspx.vb`, líneas 116-126

```vb
Protected Sub btnGuardar_Click(sender As Object, e As EventArgs) Handles btnGuardar.Click
    If Validar() = True Then
        Dim US As New GD.GD_Procedimientos
        ' CAMBIAR ESTADO A 3 (???)
        US.editarRevision(RID, DID, Request.QueryString("IdUsuario").ToString, 
                         Date.UtcNow.AddHours(-5), 3)
        lblResult.Text = "Documento Aprobado correctamente"
        Limpiar()
        CargarGrid(1)
        lista.Visible = True
        datos.Visible = False
    End If
End Sub
```

**HALLAZGOS CRÍTICOS**:
1. ⚠️ El estado cambia a **TipoRevision = 3** al aprobar
2. ⚠️ **NO HAY LÓGICA DE CONTEO** de aprobaciones vs revisores totales
3. ⚠️ El SP `GD_Revisiones_GetRev` filtra por estado 2, pero al aprobar se guarda como 3
4. ❌ **INCONSISTENCIA**: El flujo de aprobación parece incompleto o mal implementado

---

## 3. ESTADOS DE REVISIÓN (GD_TipoRevision)

Basado en el análisis del código:

| IdTipoRevision | Revision | Uso en Legacy |
|----------------|----------|---------------|
| 1 | Pendiente | Estado inicial al asignar revisores (`guardarRevision`) |
| 2 | Aprobado/Revisado | Filtro en `GD_Revisiones_GetRev` (documentos YA aprobados) |
| 3 | ??? | Estado al hacer clic en "Aprobar" (`editarRevision`) |

**NOTA**: No se encontró referencia al estado 0 (rechazado) en el código legacy.

---

## 4. LÓGICA DE APROBACIÓN (AND vs OR)

### 4.1 Análisis de Stored Procedures

**NO SE ENCONTRÓ** lógica de agregación en los SPs:
- ✅ `GD_Revisiones_Add`: Inserta individualmente (estado 1 = Pendiente)
- ✅ `GD_Revisiones_Edit`: Actualiza individualmente (estado 3 = Aprobado?)
- ❌ **NO EXISTE** SP para calcular si todos los revisores aprobaron
- ❌ **NO EXISTE** SP para actualizar el estado global de la solicitud

### 4.2 Análisis de Código VB.NET

**NO SE ENCONTRÓ** lógica de validación de aprobaciones totales:
- ❌ No hay conteo de revisores aprobados vs totales
- ❌ No hay cambio automático del estado de la solicitud
- ❌ No hay notificación al solicitante al completar todas las aprobaciones

### 4.3 Conclusión Preliminar

**EL SISTEMA LEGACY PARECE INCOMPLETO**:
1. Los revisores se asignan correctamente (estado 1 = Pendiente)
2. Cada revisor puede aprobar individualmente (estado 3)
3. **NO HAY LÓGICA** para determinar cuándo una solicitud está completamente aprobada
4. **NO HAY WORKFLOW** de finalización de solicitud

**POSIBLE EXPLICACIÓN**:
- El módulo de aprobaciones fue parcialmente implementado
- La lógica de negocio podría estar en otra parte del sistema (no encontrada)
- Podría ser un proceso manual/externo al sistema

---

## 5. DIVERGENCIAS CON LA IMPLEMENTACIÓN FASE 3

### 5.1 Nuestra Implementación (Sprint 4)

**Archivo**: `MatrixNext.Data/Services/GD/GdSolicitudesService.cs`, líneas 135-176

```csharp
public async Task<(bool success, string message)> AsignarRevisores(int idSolicitud, List<int> idRevisores)
{
    // Validaciones
    if (idSolicitud <= 0) return (false, "ID de solicitud inválido");
    if (idRevisores == null || idRevisores.Count == 0) 
        return (false, "Debe seleccionar al menos un revisor");
    
    // Verificar que la solicitud existe
    var solicitud = await _adapter.ObtenerSolicitudById(idSolicitud);
    if (solicitud == null) return (false, "Solicitud no encontrada");
    
    // ASIGNAR TODOS LOS REVISORES (igual que legacy)
    int exitosas = 0;
    var errores = new List<string>();
    
    foreach (var idRevisor in idRevisores)
    {
        try
        {
            // Estado inicial = 0 (Pendiente) <- DIFERENCIA: Legacy usa 1
            var resultado = await _adapter.CrearRevision(idSolicitud, 
                solicitud.IdDocumento, idRevisor);
            if (resultado) exitosas++;
            else errores.Add($"Fallo al asignar revisor {idRevisor}");
        }
        catch (Exception ex)
        {
            errores.Add($"Error con revisor {idRevisor}: {ex.Message}");
        }
    }
    
    // Retornar resultado agregado
    if (exitosas == idRevisores.Count)
        return (true, $"{exitosas} revisor(es) asignado(s) correctamente");
    else
        return (exitosas > 0, 
            $"{exitosas} revisor(es) asignado(s) correctamente; {errores.Count} error(es)");
}
```

**DIFERENCIAS CLAVE**:
1. ✅ **MISMA LÓGICA**: Asignar todos los revisores al mismo tiempo
2. ⚠️ **ESTADO INICIAL**: Usamos `0` (Pendiente), legacy usa `1`
3. ✅ **VALIDACIONES**: Agregamos más validaciones que el legacy
4. ✅ **MANEJO DE ERRORES**: Reportamos errores individuales (legacy lanza excepción)

### 5.2 Mapeo de Estados

**Propuesta de normalización**:

| Valor | Legacy (TipoRevision) | MatrixNext (Estado) | Comentario |
|-------|----------------------|---------------------|------------|
| 0 | (No usado) | Pendiente | Nuestra implementación |
| 1 | Pendiente | Aprobado | **CONFLICTO**: Legacy = Pendiente |
| 2 | Aprobado/Revisado | Rechazado | **CONFLICTO**: Legacy = Aprobado |
| 3 | ??? Aprobado | - | Legacy al hacer clic "Aprobar" |

**RECOMENDACIÓN**: 
- Mantener nuestra numeración (0=Pendiente, 1=Aprobado, 2=Rechazado)
- Agregar migración de datos legacy: TipoRevision 1 → Estado 0, TipoRevision 3 → Estado 1

---

## 6. RECOMENDACIONES PARA SPRINT 5

### 6.1 Implementar Lógica de Aprobación Completa

**QUE NO EXISTE EN LEGACY** pero es necesario:

1. **Servicio de Aprobación Individual**:
   ```csharp
   Task<(bool success, string message)> AprobarRevision(int idRevision, int idUsuario, 
       string comentarios);
   Task<(bool success, string message)> RechazarRevision(int idRevision, int idUsuario, 
       string comentarios, string motivo);
   ```

2. **Servicio de Validación de Aprobaciones**:
   ```csharp
   // Lógica AND: TODOS deben aprobar
   Task<bool> VerificarAprobacionCompleta(int idSolicitud);
   
   // Obtener conteos
   Task<(int total, int aprobados, int rechazados, int pendientes)> 
       ObtenerEstadisticasRevision(int idSolicitud);
   ```

3. **Servicio de Finalización de Solicitud**:
   ```csharp
   // Cambiar estado de solicitud cuando todos aprueban
   Task<(bool success, string message)> FinalizarSolicitud(int idSolicitud);
   
   // Notificar al solicitante
   Task NotificarSolicitante(int idSolicitud, string resultado);
   ```

### 6.2 Stored Procedures Necesarios (NO EXISTEN)

```sql
-- Verificar si todos los revisores aprobaron
CREATE PROCEDURE GD_Revisiones_VerificarAprobacionCompleta
    @IdSolicitud INT
AS
BEGIN
    DECLARE @Total INT, @Aprobados INT
    
    SELECT @Total = COUNT(*)
    FROM GD_Revisiones
    WHERE DocumentoId IN (
        SELECT Id FROM GD_DocumentosControlados 
        WHERE DocumentoId = @IdSolicitud
    )
    
    SELECT @Aprobados = COUNT(*)
    FROM GD_Revisiones
    WHERE DocumentoId IN (
        SELECT Id FROM GD_DocumentosControlados 
        WHERE DocumentoId = @IdSolicitud
    )
    AND TipoRevision = 1  -- Estado Aprobado
    
    IF @Total = @Aprobados AND @Total > 0
        RETURN 1  -- Todos aprobaron
    ELSE
        RETURN 0  -- Faltan aprobaciones
END
```

### 6.3 Decisión de Negocio REQUERIDA

**PREGUNTA CRÍTICA**: ¿Qué pasa cuando todos los revisores aprueban?

**Opciones**:
1. **AUTO-APROBACIÓN**: Cambiar estado de solicitud a "Aprobada" automáticamente
2. **NOTIFICACIÓN**: Enviar correo al solicitante + requiere aprobación manual final
3. **WORKFLOW**: Activar siguiente paso en proceso (publicación, etc.)

**ACCIÓN REQUERIDA**: 
- [ ] Entrevista con stakeholder(s) (Jorge/Diana/Responsable GD)
- [ ] Documentar reglas de negocio en `/docs/GD/REGLAS_APROBACION.md`
- [ ] Actualizar backlog con hallazgos

---

## 7. COMPATIBILIDAD CON LEGACY

### 7.1 Migración de Datos

**Mapeo de Estados (GD_TipoRevision)**:
```sql
-- Script de migración de datos legacy
UPDATE GD_Revisiones
SET TipoRevision = CASE
    WHEN TipoRevision = 1 THEN 0  -- Pendiente legacy → Pendiente nuevo
    WHEN TipoRevision = 3 THEN 1  -- Aprobado legacy → Aprobado nuevo
    WHEN TipoRevision = 2 THEN 1  -- Revisado legacy → Aprobado nuevo
    ELSE 0  -- Por defecto: Pendiente
END
WHERE TipoRevision IN (1, 2, 3)
```

### 7.2 Compatibilidad de API

**MANTENER**:
- ✅ Estructura de `GD_Revisiones` table
- ✅ Relación `DocumentoId` → `GD_DocumentosControlados.Id`
- ✅ Asignación múltiple de revisores en una sola operación

**CAMBIAR**:
- ⚠️ Estados de revisión (normalizar a 0/1/2)
- ⚠️ Agregar lógica de aprobación completa (no existe en legacy)
- ⚠️ Agregar notificaciones (legacy solo envía al inicio)

---

## 8. CONCLUSIONES

### 8.1 Hallazgos Principales

1. ✅ **FLUJO DE SOLICITUDES**: Bien implementado en legacy, compatible con nuestra Fase 3
2. ❌ **FLUJO DE APROBACIONES**: Incompleto en legacy, requiere implementación completa
3. ⚠️ **ESTADOS**: Conflicto de numeración, requiere migración de datos
4. ❌ **LÓGICA AND/OR**: No existe en legacy, debe ser definida por negocio

### 8.2 Próximos Pasos (Sprint 5)

**Tarea 5.2-5.8**: Implementación de Aprobaciones

1. **Crear SPs faltantes**:
   - `GD_Revisiones_VerificarAprobacionCompleta`
   - `GD_Solicitudes_CambiarEstado`
   - `GD_Revisiones_GetEstadisticas`

2. **Implementar servicios**:
   - `GdAprobacionesService.AprobarRevision()`
   - `GdAprobacionesService.RechazarRevision()`
   - `GdAprobacionesService.ObtenerEstadisticasRevision()`
   - `GdAprobacionesService.VerificarAprobacionCompleta()`

3. **Crear controlador**:
   - `AprobacionesController.Index()` (lista de pendientes)
   - `AprobacionesController.Aprobar()` (modal + POST)
   - `AprobacionesController.Rechazar()` (modal + POST)

4. **Crear vistas**:
   - `Index.cshtml` (tabla de documentos pendientes)
   - `_AprobarModal.cshtml` (formulario de aprobación)
   - `_RechazarModal.cshtml` (formulario de rechazo + motivo)

### 8.3 Bloqueadores

- 🔴 **CRÍTICO**: Definir lógica de aprobación (AND vs OR) con stakeholder
- 🔴 **CRÍTICO**: Definir qué ocurre cuando todos aprueban (workflow siguiente)
- 🟡 **MEDIO**: Migrar datos legacy de estados de revisión
- 🟡 **MEDIO**: Implementar notificaciones por email

---

**Revisado por**: GitHub Copilot  
**Próxima acción**: Reunión con stakeholder para definir REGLAS_APROBACION.md
