# MAPEO UploadService (MatrixNext)

**Ubicación**
- Interface: MatrixNext.Web/Services/IUploadService.cs
- Implementación: MatrixNext.Web/Services/UploadService.cs
- Controller API: MatrixNext.Web/Controllers/UploadController.cs
- DI: registrado en Program.cs `AddScoped<IUploadService, UploadService>()`

**Raíz de almacenamiento**
- Carpeta base: `wwwroot/uploads/{moduleId}/{entityId}/`
- Nombre de archivo: `GUID_original.ext`

**Interfaz IUploadService**
- `Task<UploadResultVM> SubirArchivoAsync(string moduleId, long entityId, IFormFile file)`
  - Valida extensiones: .pdf, .doc, .docx, .xls, .xlsx, .csv, .txt, .jpg, .png
  - Límite tamaño: 20 MB
  - Retorna ruta relativa y absoluta, nombre y tamaño
- `Task<FileStreamResult> DescargarArchivoAsync(string rutaRelativa, long usuarioId)`
  - Abre ruta física en wwwroot y devuelve stream con mime type
- `Task<bool> EliminarArchivoAsync(string rutaRelativa, long usuarioId, string razon)`
  - Borra archivo físico, loguea auditoría
- `Task<List<ArchivoVM>> ListarArchivosAsync(string moduleId, long entityId)`
  - Enumera archivos en carpeta de entidad, con nombre, ruta, tamaño KB, fecha

**Notas de uso para GD**
- Para repositorio GD usar `moduleId = "GD"` y `entityId = IdContenedor` (trabajo/proyecto según corresponda).
- Guardar la `RutaRelativa` retornada en la tabla GD_RepositorioDocumentos (campo Url).
- Para versionamiento usar `GD_GD_RepositorioDocumentos_Add` (ya calcula MAX+1 en BD); UploadService solo persiste archivo físico.
- Descarga/eliminación requieren la ruta relativa almacenada.
