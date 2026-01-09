# 🧪 Guía de Testing - Sprint 2: Planillas Moderación/Informes

## 📋 Información General

**Módulo**: Administración de Planillas Cualitativas  
**Sprint**: 2  
**Fecha**: Enero 9, 2026  
**Componentes Implementados**:
- Backend: OpPlanillasModeracionService (11 métodos, ~650 LOC)
- Controller: CualitativoPlanillasController (11 actions)
- Views: Index, EditModeracion, EditInforme
- ViewModels: 7 modelos (PlanillaListItemVm, PlanillaModeracionVm, etc.)

---

## 🎯 Objetivos de Testing

### Fase 4: Testing E2E Flujos Planillas
- ✅ Verificar flujo completo CRUD planillas moderación
- ✅ Verificar flujo completo CRUD planillas informes
- ✅ Verificar workflow aprobación/rechazo
- ✅ Verificar búsqueda JobBooks (autocomplete)
- ✅ Verificar filtros y paginación
- ✅ Verificar exportación Excel

---

## 🚀 Preparación Entorno de Testing

### 1. Verificar Build
```powershell
cd C:\Users\johnd\source\repos\johndavidpatino\Matrix
dotnet build MatrixNext/MatrixNext.Web/MatrixNext.Web.csproj
```
**Resultado esperado**: Compilación exitosa con 22 warnings (nullability - pre-existentes, no bloqueantes)

### 2. Ejecutar Aplicación
```powershell
cd MatrixNext/MatrixNext.Web
dotnet run
```
**URL esperada**: https://localhost:5001 o http://localhost:5000

### 3. Login con Usuario de Prueba
- Navegar a `/Account/Login` o `/Identity/Account/Login`
- Usar credenciales de usuario con permisos OP (Cualitativo)
- Verificar Claims authentication (ClaimTypes.NameIdentifier debe estar presente)

### 4. Navegar al Módulo
- URL: `/OP/Cualitativo/Planillas` o `/OP/Cualitativo/Planillas/Index`
- Verificar acceso autorizado (no redirige a login)
- Verificar renderizado correcto de la página Index

---

## 📝 Casos de Prueba Detallados

### TC-PL-01: Listado de Planillas con Filtros

#### Precondiciones
- Usuario autenticado con permisos OP
- Base de datos con al menos 5 planillas de cada tipo (Moderación/Informes)

#### Pasos
1. Navegar a `/OP/Cualitativo/Planillas`
2. Verificar que se muestre el grid con planillas
3. Aplicar filtro "Tipo Planilla: Moderación"
   - Click en dropdown "Tipo Planilla"
   - Seleccionar "Moderación"
   - Click en botón "Buscar"
4. Verificar que solo se muestren planillas de tipo "Moderación" (badge azul info)
5. Aplicar filtro "Estado Aprobación: En Espera"
   - Seleccionar "En Espera" en dropdown "Estado Aprobación"
   - Click en "Buscar"
6. Verificar que solo se muestren planillas con badge amarillo "En Espera"
7. Click en "Limpiar" y verificar que se limpien todos los filtros

#### Resultado Esperado
- Grid actualiza correctamente con cada filtro
- Badges de tipo planilla: Moderación (bg-info azul), Informes (bg-primary azul oscuro)
- Badges de estado: En Espera (bg-warning amarillo), Aprobado (bg-success verde), No Aprobado (bg-danger rojo)
- Paginación aparece si hay >25 registros
- Contador "Mostrando X-Y de Z registros" correcto

#### Criterios de Aceptación
- [x] Filtros funcionan independientemente
- [x] Filtros se combinan correctamente (AND lógico)
- [x] Paginación respeta filtros aplicados
- [x] Botón "Limpiar" resetea a estado inicial

---

### TC-PL-02: Crear Planilla de Moderación

#### Precondiciones
- Usuario autenticado
- Al menos 1 JobBook disponible en base de datos
- Al menos 1 técnica de moderación configurada
- Al menos 1 moderador disponible

#### Pasos
1. Desde Index, click en dropdown "Nueva Planilla"
2. Seleccionar "Planilla de Moderación"
3. Verificar redirección a `/OP/Cualitativo/Planillas/EditModeracion`
4. **Campo JobBook**:
   - Click en input "JobBook"
   - Escribir las primeras 2 letras de un JobBook existente (ej: "AB")
   - Esperar 300ms para que aparezca el dropdown autocomplete
   - Verificar que aparezcan resultados con formato "JobBook - Nombre Trabajo"
   - Click en un resultado para seleccionarlo
   - Verificar que el input se llene con el JobBook seleccionado
5. **Campo Fecha Planilla**:
   - Seleccionar fecha actual o futura
6. **Campo Técnica**:
   - Abrir dropdown "Técnica"
   - Seleccionar una técnica (ej: "Focus Group")
7. **Campo Muestra**:
   - Ingresar número de muestra (ej: 8)
8. **Campo Moderador**:
   - Abrir dropdown "Moderador"
   - Seleccionar un moderador disponible
9. **Campo Observaciones** (opcional):
   - Ingresar observaciones si se desea
10. **Verificar Estado Aprobación**:
    - Confirmar que el dropdown "Estado Aprobación" está disabled
    - Confirmar que muestra "En Espera" por defecto
11. Click en botón "Guardar Planilla"
12. Verificar redirección a Index con mensaje de éxito en TempData

#### Resultado Esperado
- Autocomplete JobBooks funciona con mínimo 2 caracteres
- Resultados autocomplete se ocultan al hacer click fuera
- Todos los campos obligatorios tienen validación
- Estado aprobación inicia en "En Espera" y está disabled
- Mensaje de éxito: "Planilla de moderación creada exitosamente (ID: X)"
- Nueva planilla aparece en el grid con badge "En Espera"

#### Criterios de Aceptación
- [x] Validación client-side funciona (HTML5 required)
- [x] Validación server-side funciona (ModelState)
- [x] CSRF token presente y validado
- [x] Autocomplete JobBooks con debounce 300ms
- [x] INSERT en tabla PY_PlanillaModeracion exitoso
- [x] UsuarioId tomado de ClaimTypes.NameIdentifier

#### Casos de Error a Probar
- Intentar guardar sin JobBook → Error de validación
- Intentar guardar sin técnica → Error de validación
- Intentar guardar muestra = 0 → Error de validación (min=1)
- JobBook que no existe → Error del servicio
- Usuario no autenticado → Redirect a login

---

### TC-PL-03: Crear Planilla de Informes

#### Precondiciones
- Usuario autenticado
- Al menos 1 JobBook disponible

#### Pasos
1. Desde Index, click en "Nueva Planilla" → "Planilla de Informes"
2. Verificar redirección a `/OP/Cualitativo/Planillas/EditInforme`
3. **Campo JobBook**:
   - Probar autocomplete igual que en TC-PL-02
4. **Campo Fecha**:
   - Seleccionar fecha actual
5. **Campo Técnica**:
   - Ingresar texto libre (ej: "Entrevista en Profundidad")
   - Nota: Este campo es input text, no dropdown
6. **Campo Muestra**:
   - Ingresar número (ej: 12)
7. **Campo ID Cuentas UU** (opcional):
   - Dejar vacío o ingresar número
8. **Campo Analista**:
   - Ingresar nombre (ej: "Juan Pérez")
9. **Campo Service Line** (opcional):
   - Ingresar texto (ej: "Salud")
10. **Campo Observaciones** (opcional):
    - Ingresar observaciones
11. Click en "Guardar Planilla"
12. Verificar redirección a Index con filtro "tipoPlantilla=Informes"

#### Resultado Esperado
- Técnica es input text libre (diferencia con Moderación)
- Campos específicos de informes visibles: Analista, Service Line, ID Cuentas UU
- Mensaje de éxito: "Planilla de informes creada exitosamente (ID: X)"
- Redirección al Index con filtro aplicado

#### Criterios de Aceptación
- [x] Validación campos obligatorios funciona
- [x] INSERT en tabla PY_PlanillaInformes exitoso
- [x] Campos opcionales permiten valores NULL
- [x] Redirección con querystring correcto

---

### TC-PL-04: Editar Planilla de Moderación

#### Precondiciones
- Existe al menos 1 planilla de moderación con IdEstadoAprobacion = 1 (En Espera)

#### Pasos
1. Desde Index, ubicar una planilla de tipo "Moderación" con estado "En Espera"
2. Click en botón "Editar" (icono lápiz)
3. Verificar carga del formulario con datos existentes:
   - JobBook pre-llenado
   - Fecha Planilla pre-llenada
   - Técnica seleccionada en dropdown
   - Muestra con valor numérico
   - Moderador seleccionado en dropdown
   - Observaciones con texto existente (si las había)
   - Estado Aprobación disabled mostrando estado actual
4. Modificar campo "Muestra" (ej: cambiar de 8 a 10)
5. Modificar "Observaciones" (agregar texto adicional)
6. Click en "Guardar Planilla"
7. Verificar mensaje de éxito: "Planilla de moderación actualizada exitosamente"
8. Verificar en Index que los cambios se reflejan

#### Resultado Esperado
- Formulario carga datos existentes correctamente
- UPDATE en base de datos funciona
- FechaModificacion se actualiza
- UsuarioModificacion se registra

#### Criterios de Aceptación
- [x] GET a servicio ObtenerPlanillaModeracionAsync funciona
- [x] Dropdowns se pre-seleccionan con valores correctos
- [x] UPDATE preserva campos no modificados
- [x] CSRF token validado en POST

---

### TC-PL-05: Aprobar Planilla (Workflow)

#### Precondiciones
- Existe planilla con IdEstadoAprobacion = 1 (En Espera)
- Usuario tiene permisos de aprobación

#### Pasos
1. Desde Index, ubicar planilla con estado "En Espera"
2. Verificar que aparecen botones "Aprobar" (check verde) y "Rechazar" (X roja)
3. Click en botón "Aprobar"
4. Verificar aparición de confirm dialog JavaScript: "¿Está seguro que desea aprobar esta planilla?"
5. Click en "Aceptar" en el confirm
6. Esperar respuesta AJAX
7. Verificar alert de éxito: "Planilla aprobada exitosamente"
8. Verificar reload de página automático
9. Verificar que planilla ahora muestra badge "Aprobado" (verde)
10. Verificar que botones "Aprobar" y "Rechazar" ya no aparecen

#### Resultado Esperado
- AJAX POST a `/OP/Cualitativo/Planillas/AprobarPlanilla` exitoso
- UPDATE en base de datos: IdEstadoAprobacion = 2, FechaAprobacion = NOW, UsuarioAprobacion = CurrentUser
- Badge cambia de amarillo a verde
- Botones de acción desaparecen (solo permitido en estado En Espera)

#### Criterios de Aceptación
- [x] AJAX con CSRF token correcto
- [x] Response JSON con {success: true, message: "..."}
- [x] Tabla actualizada correctamente
- [x] FechaAprobacion registrada
- [x] UsuarioAprobacion registrado

#### Verificación en Base de Datos
```sql
SELECT IdPlanilla, IdEstadoAprobacion, FechaAprobacion, UsuarioAprobacion, Observaciones
FROM PY_PlanillaModeracion -- o PY_PlanillaInformes
WHERE IdPlanilla = [ID_PLANILLA_APROBADA]
```
**Esperado**: IdEstadoAprobacion = 2, FechaAprobacion NOT NULL, UsuarioAprobacion NOT NULL

---

### TC-PL-06: Rechazar Planilla con Observaciones (Workflow)

#### Precondiciones
- Existe planilla con IdEstadoAprobacion = 1 (En Espera)

#### Pasos
1. Desde Index, ubicar planilla con estado "En Espera"
2. Click en botón "Rechazar" (X roja)
3. Verificar apertura de modal "Rechazar Planilla"
4. **Intentar sin observaciones**:
   - Dejar textarea "Observaciones" vacío
   - Click en botón "Rechazar" del modal
   - Verificar alert: "Las observaciones son requeridas para rechazar la planilla"
   - Modal NO se cierra
5. **Rechazar con observaciones**:
   - Ingresar texto en "Observaciones" (ej: "No cumple con los requisitos de calidad")
   - Click en botón "Rechazar"
   - Esperar respuesta AJAX
   - Verificar alert: "Planilla rechazada exitosamente"
   - Verificar reload automático
6. Verificar badge cambió a "No Aprobado" (rojo)
7. Click en "Editar" de la planilla rechazada
8. Verificar que campo "Observaciones" muestra el texto de rechazo

#### Resultado Esperado
- Modal Bootstrap funciona correctamente
- Validación client-side de observaciones requeridas
- AJAX POST exitoso con observaciones
- UPDATE en base de datos: IdEstadoAprobacion = 3, Observaciones actualizado
- Badge cambia de amarillo a rojo

#### Criterios de Aceptación
- [x] Validación client-side previene envío sin observaciones
- [x] Modal se cierra solo después de éxito
- [x] Observaciones se guardan correctamente
- [x] Estado cambia a 3 (No Aprobado)

---

### TC-PL-07: Exportar Planillas a Excel

#### Precondiciones
- Existen al menos 5 planillas en base de datos (mixtas: Moderación e Informes)

#### Pasos
1. Desde Index, aplicar filtro "Tipo Planilla: Moderación"
2. Click en botón "Exportar" (verde con icono Excel)
3. Verificar descarga de archivo Excel
4. **Verificar nombre archivo**: `Planillas_Moderacion_YYYYMMDD_HHmmss.xlsx`
5. **Abrir archivo Excel**:
   - Verificar hoja se llama "Planillas"
   - Verificar título reporte en primera fila
   - Verificar headers en segunda fila (IdPlanilla, Tipo, JobBook, Fecha, etc.)
   - Verificar datos de planillas filtradas (solo Moderación)
   - Verificar formato de fechas DD/MM/YYYY
6. Limpiar filtros y exportar "Todas" las planillas
7. Verificar nombre archivo: `Planillas_Todas_YYYYMMDD_HHmmss.xlsx`
8. Verificar que incluye tanto Moderación como Informes

#### Resultado Esperado
- GET a `/OP/Cualitativo/Planillas/ExportExcel?tipoPlantilla=X&statusRegistro=Y`
- IExportService.ExportarExcelAsync genera archivo válido
- ClosedXML formatea correctamente
- Content-Type: `application/vnd.openxmlformats-officedocument.spreadsheetml.sheet`
- Nombre archivo dinámico con timestamp

#### Criterios de Aceptación
- [x] Exportación respeta filtros aplicados
- [x] Archivo Excel válido (abre sin errores)
- [x] Formato de fechas correcto
- [x] Columnas correctas según PlanillaListItemVm
- [x] Si no hay datos, muestra TempData["Warning"] y redirige

---

### TC-PL-08: Búsqueda JobBooks (Autocomplete)

#### Precondiciones
- Base de datos con JobBooks que contienen texto variado (ej: "AB-2025-001 Estudio Mercado", "CD-2025-002 Investigación Producto")

#### Pasos
1. Navegar a "Nueva Planilla de Moderación"
2. **Búsqueda exitosa**:
   - Click en input "JobBook"
   - Escribir "AB" (mínimo 2 caracteres)
   - Esperar 300ms (debounce)
   - Verificar que aparece dropdown con resultados
   - Verificar formato: "AB-2025-001 - Estudio Mercado"
   - Click en un resultado
   - Verificar que input se llena con el JobBook
   - Verificar que dropdown se oculta
3. **Búsqueda sin resultados**:
   - Limpiar input
   - Escribir "ZZZZZ" (texto que no existe)
   - Esperar 300ms
   - Verificar que dropdown NO aparece (o aparece vacío)
4. **Menos de 2 caracteres**:
   - Limpiar input
   - Escribir "A" (solo 1 carácter)
   - Verificar que dropdown NO aparece
5. **Click fuera del input**:
   - Escribir "AB" para mostrar dropdown
   - Click en cualquier parte fuera del input y dropdown
   - Verificar que dropdown se oculta

#### Resultado Esperado
- AJAX GET a `/OP/Cualitativo/Planillas/BuscarJobBooks?termino=AB`
- Servicio ejecuta query LIKE '%AB%' contra tabla JobBook o Jobs
- Response JSON: `[{jobBook: "AB-2025-001", nombreTrabajo: "Estudio Mercado", ...}, ...]`
- Debounce previene requests excesivos
- Mínimo 2 caracteres requerido

#### Criterios de Aceptación
- [x] Búsqueda case-insensitive
- [x] LIKE query funciona con % en ambos lados
- [x] JSON serialization correcta
- [x] Dropdown se posiciona correctamente (z-index: 1000)

---

### TC-PL-09: Paginación Manual

#### Precondiciones
- Más de 25 planillas en base de datos

#### Pasos
1. Navegar a Index sin filtros
2. Verificar que se muestran las primeras 25 planillas
3. Verificar contador: "Mostrando 1 - 25 de X registros"
4. Verificar paginación aparece con botones:
   - "Anterior" (disabled)
   - Números de página (1, 2, 3, ...)
   - "Siguiente"
5. Click en botón "2"
6. Verificar URL actualiza: `?pageIndex=1&pageSize=25`
7. Verificar que muestra registros 26-50
8. Verificar botón "1" ahora no está activo
9. Verificar botón "Anterior" ahora está enabled
10. Click en "Anterior"
11. Verificar regreso a página 1
12. Aplicar filtro "Tipo Planilla: Moderación"
13. Verificar que paginación se resetea a página 1
14. Verificar contador refleja total filtrado

#### Resultado Esperado
- Paginación controlada por querystring (pageIndex, pageSize)
- Backend calcula OFFSET y FETCH NEXT correctamente
- Contador preciso: `(pageIndex * pageSize) + 1` hasta `MIN((pageIndex + 1) * pageSize, totalRecords)`
- Botones "Anterior" y "Siguiente" se deshabilitan en extremos

#### Criterios de Aceptación
- [x] Paginación respeta filtros
- [x] URL refleja estado actual
- [x] Navegación con botones funciona
- [x] Contador matemático correcto

---

### TC-PL-10: Validación de Permisos y Seguridad

#### Pasos de Seguridad
1. **Usuario no autenticado**:
   - Logout de la aplicación
   - Intentar navegar a `/OP/Cualitativo/Planillas`
   - Verificar redirect a `/Account/Login` o `/Identity/Account/Login`
2. **CSRF Protection**:
   - Inspeccionar formulario de crear planilla
   - Verificar presencia de `<input name="__RequestVerificationToken" type="hidden" value="..." />`
   - Intentar POST sin token CSRF (usar Postman o curl)
   - Verificar error 400 Bad Request o similar
3. **SQL Injection**:
   - En búsqueda JobBooks, intentar ingresar: `'; DROP TABLE PY_PlanillaModeracion; --`
   - Verificar que query es parametrizada (Dapper previene injection)
   - Verificar que no ocurre error ni ejecución maliciosa
4. **XSS Prevention**:
   - Crear planilla con observaciones: `<script>alert('XSS')</script>`
   - Guardar y ver en Index
   - Verificar que script se muestra como texto, no se ejecuta (Razor @Model.Property auto-escapa HTML)

#### Criterios de Aceptación
- [x] [Authorize] attribute en controller previene acceso no autenticado
- [x] [ValidateAntiForgeryToken] en POST actions
- [x] Dapper queries parametrizadas
- [x] Razor auto-escape HTML

---

## ✅ Checklist de Verificación Final

### Funcionalidad
- [ ] CRUD completo planillas moderación funciona
- [ ] CRUD completo planillas informes funciona
- [ ] Workflow aprobación funciona (estado cambia a Aprobado)
- [ ] Workflow rechazo funciona (estado cambia a No Aprobado, observaciones requeridas)
- [ ] Autocomplete JobBooks funciona con mínimo 2 caracteres
- [ ] Filtros por tipo plantilla y estado funcionan
- [ ] Paginación manual con querystring funciona
- [ ] Exportación Excel con filtros funciona
- [ ] Badges de estado muestran colores correctos

### Validación
- [ ] Campos obligatorios validados client-side (HTML5)
- [ ] Campos obligatorios validados server-side (ModelState)
- [ ] Observaciones requeridas en rechazo
- [ ] Muestra >= 1 validada

### Seguridad
- [ ] Autenticación requerida ([Authorize])
- [ ] CSRF tokens presentes y validados
- [ ] Queries parametrizadas (Dapper)
- [ ] HTML auto-escapado (Razor)

### UX
- [ ] Mensajes de éxito claros (TempData)
- [ ] Mensajes de error informativos
- [ ] Loading states en AJAX (opcional)
- [ ] Responsive design funciona en móvil (Bootstrap)

### Performance
- [ ] Paginación reduce carga (solo 25 registros por página)
- [ ] Autocomplete con debounce 300ms
- [ ] Queries optimizadas (índices en IdEstadoAprobacion, TipoPlantilla)

---

## 🐛 Issues Encontrados Durante Testing

### Issue #1: [Título del Issue]
**Descripción**: [Descripción detallada]  
**Pasos para reproducir**:
1. [Paso 1]
2. [Paso 2]

**Resultado esperado**: [...]  
**Resultado actual**: [...]  
**Severidad**: Alta / Media / Baja  
**Estado**: Abierto / En progreso / Resuelto  

---

## 📊 Resumen de Testing

### Cobertura de Casos de Prueba
- Total casos planificados: 10
- Total casos ejecutados: [ ] / 10
- Total casos pasados: [ ] / 10
- Total casos fallados: [ ] / 10

### Defectos Encontrados
- Críticos: 0
- Altos: 0
- Medios: 0
- Bajos: 0

### Recomendaciones
1. [Recomendación 1 basada en testing]
2. [Recomendación 2]

---

## 🎬 Próximos Pasos

Después de completar testing E2E:
1. Resolver defectos críticos y altos
2. Documentar issues menores para backlog
3. Actualizar SPRINT_2_PROGRESS_TRACKING.md con resultados
4. Commit Sprint 2 completo con mensaje descriptivo
5. Iniciar Sprint 3 o siguiente fase según roadmap

---

**Última actualización**: Enero 9, 2026  
**Responsable Testing**: [Nombre del tester]  
**Estado**: ⏳ En progreso
