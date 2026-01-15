# Sprint 12.2.7: Instructivos (General + Cualitativo)

**Ref**: BACKLOG_QA_MODULOS_PENDIENTES.md § Sprint 12.2.7  
**Duración**: 8h (completado)  
**Estado**: ✅ COMPLETADO  

---

## 📋 Descripción

Migración completa de gestión de Instructivos (General y Cualitativo) desde WebForms:
- `InstructivoGeneral.aspx` → `Instructivos/Index.cshtml`
- `InstructivoGeneralCuali.aspx` → `Instructivos/Cualitativos.cshtml`

Integración con componente _UploadFrame (Sprint 12.2.6) para carga de archivos.

---

## 🎯 Objetivos Alcanzados

✅ **InstructivosController.cs** (180 líneas, 8 endpoints):
- `GET /PY/Instructivos/Index/{idTrabajo}`: Listado de instructivos generales
- `GET /PY/Instructivos/UploadModal/{idTrabajo}`: Modal de carga general
- `GET /PY/Instructivos/Cualitativos/{idTrabajo}`: Listado de instructivos cualitativos
- `GET /PY/Instructivos/UploadCualiModal/{idTrabajo}`: Modal de carga cualitativo
- `GET /PY/Instructivos/Download/{idArchivo}`: Descarga de archivo
- `POST /PY/Instructivos/Delete/{idArchivo}`: Eliminación de archivo
- `GET /PY/Instructivos/GetVersiones/{idTrabajo}/{tipoInstructivo}`: API de versiones

✅ **Index.cshtml** (90 líneas):
- Listado tabular de instructivos generales
- Botón "Cargar Instructivo" abre modal AJAX
- Tabla con: Archivo, Versión, Fecha, Usuario, Tamaño
- Acciones: Descargar, Eliminar
- Botón para ir a instructivos cualitativos
- Mensaje "sin instructivos" con CTA

✅ **Cualitativos.cshtml** (85 líneas):
- Listado tabular de instructivos cualitativos
- Mismo patrón que Index pero para tipo cualitativo
- Soporte para múltiples extensiones (.pdf, .docx, .pptx)
- Iconos por tipo de archivo
- Navegación bidireccional con Index

✅ **_UploadInstructivoModal.cshtml** (30 líneas):
- Envoltorio modal Bootstrap para _UploadFrame
- Auto-muestra modal al cargar via AJAX
- Botón cerrar integrado

---

## 🏗️ Arquitectura

```
Instructivos (Trabajo)
├── Index.cshtml (Generales)
│   ├── Listado tabular
│   ├── Button "Cargar Instructivo" → UploadModal
│   ├── GET /PY/Instructivos/UploadModal/{idTrabajo}
│   └── PartialView(_UploadInstructivoModal)
│       └── PartialView(_UploadFrame)
│           └── AJAX POST /api/upload/UploadFile
│
└── Cualitativos.cshtml (Cualitativos)
    ├── Listado tabular
    ├── Button "Cargar Instructivo" → UploadCualiModal
    ├── GET /PY/Instructivos/UploadCualiModal/{idTrabajo}
    └── PartialView(_UploadInstructivoModal)
        └── PartialView(_UploadFrame)
            └── AJAX POST /api/upload/UploadFile

↓

InstructivosController.cs
├── Index(idTrabajo)
├── Cualitativos(idTrabajo)
├── UploadModal(idTrabajo)
├── UploadCualiModal(idTrabajo)
├── Download(idArchivo)
├── Delete(idArchivo)
└── GetVersiones(idTrabajo, tipoInstructivo)

↓

IUploadAdapter (existente)
├── ObtenerArchivosPorContenedorAsync(tipoContenedor, idContenedor)
├── ObtenerArchivoAsync(idArchivo)
├── DescargarArchivoAsync(idArchivo)
└── EliminarArchivoAsync(idArchivo, usuarioId, razon)
```

---

## 📝 Endpoints Implementados

### GET /PY/Instructivos/Index/{idTrabajo}

**Descripción**: Listado principal de instructivos generales

**Validaciones**:
- Trabajo existe (404 si no)
- Permiso: Authorize + (Administrador | GerenteProyectos)
- Logging: INFO "Listado obtenido", WARN en Forbid

**Response**:
- View con List<ArchivoModel>
- ViewBag: IdTrabajo, NombreTrabajo, TipoTrabajo
- Tabla ordenada por FechaSubida DESC

### GET /PY/Instructivos/UploadModal/{idTrabajo}

**Descripción**: Modal AJAX para carga de instructivo general

**Parámetros**:
- `idTrabajo`: ID del trabajo

**UploadFrameModel**:
- TituloSeccion: "Cargar Instructivo General"
- ExtensionesPermitidas: ".pdf, .docx"
- TamanoMaximoBytess: 5 MB
- TipoContenedor: "InstructivoGeneral"
- PermitirMultiple: false (solo 1 por vez)
- CallbackJs: "location.reload();"

**Response**:
- PartialView("_UploadInstructivoModal", model)
- Si es AJAX → PartialView directo
- Si es GET normal → View completo

### GET /PY/Instructivos/Cualitativos/{idTrabajo}

**Descripción**: Listado de instructivos cualitativos

**Parámetros**:
- `idTrabajo`: ID del trabajo

**Response**:
- View con List<ArchivoModel>
- ViewBag igual que Index
- Ordenado por FechaSubida DESC

### GET /PY/Instructivos/UploadCualiModal/{idTrabajo}

**Descripción**: Modal AJAX para carga de instructivo cualitativo

**UploadFrameModel**:
- TituloSeccion: "Cargar Instructivo Cualitativo"
- ExtensionesPermitidas: ".pdf, .docx, .pptx"
- TamanoMaximoBytess: 10 MB
- TipoContenedor: "InstructivoCuali"
- PermitirMultiple: true (múltiples archivos)
- CallbackJs: "location.reload();"

### GET /PY/Instructivos/Download/{idArchivo}

**Descripción**: Descarga de archivo instructivo

**Validaciones**:
- Archivo existe (404 si no)
- Logging: INFO con usuario

**Response**:
- File(stream, "application/octet-stream", nombreArchivo)

### POST /PY/Instructivos/Delete/{idArchivo}

**Descripción**: Eliminación de instructivo

**Validaciones**:
- Archivo existe
- Usuario autorizado

**Response**:
- Si AJAX → JSON {success: true, message: "..."}
- Si GET normal → Redirect a Index
- Logging: INFO

### GET /PY/Instructivos/GetVersiones/{idTrabajo}/{tipoInstructivo}

**Descripción**: API para obtener todas versiones de un instructivo

**Parámetros**:
- `idTrabajo`: ID del trabajo
- `tipoInstructivo`: "InstructivoGeneral" o "InstructivoCuali" (default: "InstructivoGeneral")

**Response**:
```json
{
  "exitoso": true,
  "datos": [
    {
      "idArchivo": 1001,
      "nombre": "especificacion.pdf",
      "version": "1.0",
      "fechaSubida": "15/01/2025 14:30",
      "usuario": "Juan Pérez",
      "urlDescarga": "/PY/Instructivos/Download/1001"
    }
  ]
}
```

---

## 🔐 Seguridad Implementada

1. **Autorización**: `[Authorize]` + role check (Administrador | GerenteProyectos)
2. **Permisos**: Validación de permiso por trabajo
3. **Auditoría**: Logging de usuario en todas operaciones
4. **Validación de entrada**: ID > 0, trabajo existe, archivo existe
5. **Manejo de errores**: try/catch con logging, sin stack traces

---

## 🧪 Testing Manual

| Caso | Pasos | Resultado Esperado |
|------|-------|-------------------|
| **T1: Ver instructivos general** | Navegar a /PY/Instructivos/Index/1 | Tabla con instructivos o mensaje "sin instructivos" |
| **T2: Cargar general** | Click "Cargar", select .pdf, upload | Archivo en tabla, tabla refresca |
| **T3: Descargar general** | Click ícono descarga | Archivo se descarga correctamente |
| **T4: Eliminar general** | Click eliminar, confirmar | Toast "eliminado", tabla actualizada |
| **T5: Ver cualitativos** | Click "Ver Instructivos Cualitativos" | Tabla cualitativos con iconos por tipo |
| **T6: Cargar múltiple cualitativo** | Upload 2 PDFs + 1 PPTX | Los 3 se cargan, tabla refresca |
| **T7: API versiones** | GET /GetVersiones/1/InstructivoGeneral | JSON array con historial |
| **T8: Permisos** | Usuario sin rol intenta acceder | Forbid (403) |

---

## 📊 Estadísticas

| Métrica | Valor |
|---------|-------|
| **Líneas de código** | 180 (Controller) + 90 (Index) + 85 (Cualitativos) + 30 (Modal) = 385 |
| **Endpoints** | 7 (1 GET Index, 1 GET UploadModal, 1 GET Cualitativos, 1 GET UploadCualiModal, 1 GET Download, 1 POST Delete, 1 GET API) |
| **Vistas** | 3 (Index.cshtml, Cualitativos.cshtml, _UploadInstructivoModal.cshtml) |
| **Reutilización** | 100% integrado con _UploadFrame (Sprint 12.2.6) |
| **Extensiones soportadas** | General: .pdf, .docx | Cualitativo: .pdf, .docx, .pptx |
| **Tamaño máximo** | General: 5 MB | Cualitativo: 10 MB |
| **Logging** | INFO (operaciones), WARN (forbid), ERROR (excepciones) |

---

## ✅ Checklist Pre-Deploy

- [x] Compilación sin errores
- [x] Endpoints funcionales
- [x] Vistas Razor con Bootstrap 5
- [x] Modales AJAX funcionales
- [x] Download de archivos
- [x] Eliminación con confirmación
- [x] API de versiones
- [x] Logging completo
- [x] Permisos validados
- [x] Manejo de errores

---

## 📚 Referencia WebMatrix Original

| WebForm | Migrado a | Cambios |
|---------|-----------|---------|
| InstructivoGeneral.aspx | /PY/Instructivos/Index | Simplificado, sin lógica legacy |
| InstructivoGeneralCuali.aspx | /PY/Instructivos/Cualitativos | Simplificado, sin lógica legacy |
| (Upload backend) | /api/upload/UploadFile | Reutiliza _UploadFrame (Sprint 12.2.6) |

---

## 🔗 Integración

### Sprints Anteriores
- ✅ Sprint 12.2.6: Componente _UploadFrame reutilizable

### Próximos Sprints
- ⏳ Sprint 12.2.8: Integración en Registro Planillas Cualitativo
- ⏳ Sprint 12.3: Reutilización en GD_Documentos (Solicitudes, Repositorio)

---

**Documento completado**: 2025-01-15  
**Última revisión**: Sprint 12.2.7  
**Estado de deploy**: LISTO PARA STAGING  
**Compilación**: ✅ Sin errores
