# Plan de migracion WebForms -> MVC (.NET 8)

## Objetivos
- Reemplazar WebMatrix (WebForms + EF 4.8 VB) por MVCMatrix (ASP.NET Core MVC .NET 8).
- Crear una capa de datos moderna (EF Core + Dapper) reutilizable entre aplicaciones.
- Mantener servicio estable durante la migracion (coexistencia controlada y despliegues por slices).

## Arquitectura destino
- Solucion `MatrixNext` con proyectos:
  - `MatrixNext.Web` (MVC .NET 8, estilo MVCMatrix, Razor Runtime Compilation para desarrollo).
  - `MatrixNext.Data` (.NET 8 class library, EF Core + Dapper, DbContext + query services + repos/units si se requieren).
  - (Opcional) `MatrixNext.Utilidades` si se necesitan helpers compartidos.
- BD compartida inicialmente; se migran esquemas via EF Core migrations una vez estabilizado el modelo.

## Fases y entregables
1) **Arranque** ✅ COMPLETADO
   - ✅ Crear solucion y proyectos base (.NET 8): MatrixNext.Web y MatrixNext.Data
   - ✅ Configurar DI: Session, Cookies, servicios de datos (UsuarioAuthService, LogService, EncryptionService)
   - ✅ Health checks implementados (/health endpoint con self-check)
   - ✅ Configuracion: cadenas de conexion en appsettings.json
   - ✅ Middleware global de manejo de excepciones (GlobalExceptionHandlerMiddleware)
   - ✅ Autenticación basada en cookies + claims ("MatrixCookies", sliding expiration 8h)
   
2) **Capa de datos** 🔄 EN PROGRESO
   - ✅ Servicios con Dapper para autenticación (UsuarioAuthService) y logs (LogService)
   - ✅ Modelos básicos: Usuario, LogEjecucion, LogEntrada
   - ✅ Utilidad de encriptación migrada de VB.NET (TripleDES legacy compatible)
   - ⏳ PENDIENTE: Crear adaptadores para contextos CoreProject (US_Entities, CU_Entities, etc.)
   - ⏳ PENDIENTE: Servicios Dapper/EF6 por módulo según avance de migraciones
   - ⏳ PENDIENTE: Configurar migraciones EF Core para nuevas features
   
3) **Utilidades** ✅ COMPLETADO (básico)
   - ✅ Migrado EncryptionService (Cifrado con TripleDES de VB.NET Encripcion.vb)
   - ⏳ PENDIENTE: Evaluar otros helpers de carpeta Utilidades según necesidad
   
4) **Migracion funcional (vertical slices por módulo)** 🔄 FASE ACTIVA
   - ✅ **Slice: Login** (Default.aspx -> LoginController)
     - ✅ LoginController con autenticación por cookies + claims
     - ✅ Vista Login/Index.cshtml con estilos MVCMatrix (sign-in-basic)
     - ✅ Layout completo de MVCMatrix copiado (_Layout.cshtml + partials)
     - ✅ Assets completos copiados (Bootstrap 5, iconos, JS libs)
     - ✅ Validación de usuario/contraseña con TripleDES
     - ✅ Master password bypass para desarrollo ("Matrix#$%&")
     - ✅ Logs de auditoría (LOG_Ejecucion, LOG_Entrada)
   - ✅ HomeController con autorización ([Authorize])
   - ✅ Vista AccessDenied para errores 401/403
   - ⏳ PENDIENTE: Módulos funcionales por orden de prioridad (ver sección "Orden de Migracion de Modulos")
   
5) **Seguridad y estado** ✅ COMPLETADO (básico)
   - ✅ Autenticación con cookies + claims (ClaimTypes.NameIdentifier, Name, Email, NombreCompleto)
   - ✅ Session configurado (30 min timeout) + cookie persistente
   - ✅ Rutas de autorización configuradas (LoginPath, LogoutPath, AccessDeniedPath)
   - ⏳ PENDIENTE: Autorización basada en roles/policies según permisos de WebMatrix
   - ⏳ PENDIENTE: Cache distribuido si se requiere escalar a múltiples instancias
   
6) **Despliegue** ⏳ FINAL (sin coexistencia)
   - Retirar WebMatrix y redireccionar tráfico a MatrixNext una vez cobertura completa ✅ COMPLETADO (básico)
   - Revisar autenticacion/autorizacion; migrar de FormsAuth/Session a cookies + claims y, si aplica, cache distribuido.
   - Quitar dependencias de ViewState; usar tempdata o modelos persistidos.
6) **Despliegue y coexistencia**
   - Ejecutar WebForms y MVC en paralelo (subdominio o path) hasta cobertura completa.
   - Feature flags para activar nuevas rutas; monitoreo y rollback plan.
7) **Retiro de legado**
   - Congelar cambios en WebForms; redirigir trafico a MVC.
   - Archivar proyecto VB y limpiar pipelines.

## Estrategia de Migración por Módulos

### Estructura de Carpetas WebMatrix (Módulos)
WebMatrix está organizado en **26 módulos temáticos** dentro de carpetas:

| Módulo | Carpeta | Archivos .aspx | Dominio CoreProject | Prioridad |
|--------|---------|---|---|---|
| **Seguridad** | `US_Usuarios/` | 14 páginas | `US_Model` | 🔴 CRÍTICA |
| **Dashboard** | `Home/` | 3 páginas | `CORE_Model` + múltiples | 🔴 CRÍTICA |
| **Proyectos** | `PY_Proyectos/` | 18 páginas | `PY_Model` | 🔴 ALTA |
| **Operaciones - Cuantitativo** | `OP_Cuantitativo/` | múltiples | `OP_Cuanti_Model` | 🟠 ALTA |
| **Operaciones - Cualitativo** | `OP_Cualitativo/` | múltiples | `OP_Entities` | 🟠 ALTA |
| **Finanzas/Compras** | `FI_AdministrativoFinanciero/` | 21 páginas | `FI_Model` | 🟠 ALTA |
| **Documentos** | `GD_Documentos/` | múltiples | `GD_Model` | 🟡 MEDIA |
| **Reportes** | `RP_Reportes/` | múltiples | `REP_Model` | 🟡 MEDIA |
| **Talento Humano** | `TH_TalentoHumano/` | múltiples | `TH_Model` | 🟡 MEDIA |
| **Clientes** | `CU_Cuentas/` | múltiples | `CU_Model` | 🟡 MEDIA |
| Otros módulos | 16 carpetas más | - | Varios | 🟢 BAJA |

### Estrategia de Progresión por Módulo

**Nivel 1: Módulo Completo (carpeta WebMatrix)**
- Migrar TODOS los .aspx dentro de una carpeta (ej: 14 páginas de `US_Usuarios/`)
- Crear contexto/servicios en MatrixNext.Data para ese módulo
- Agrupar Controllers/Views en MatrixNext.Web por módulo

**Nivel 2: Página Individual dentro del Módulo**
- Cada .aspx.vb tiene correspondencia 1:1 con una acción de Controller
- Migrar Views en carpeta con nombre de módulo (ej: `Views/Usuarios/`, `Views/Usuarios/Roles.cshtml`)

**Implementación por página:**
```
WebMatrix/
  US_Usuarios/
    Usuarios.aspx ────────> MatrixNext.Web/Controllers/UsuariosController.cs
                         └──> MatrixNext.Web/Views/Usuarios/Usuarios.cshtml
    Roles.aspx ──────────> MatrixNext.Web/Controllers/RolesController.cs
                         └──> MatrixNext.Web/Views/Roles/Index.cshtml
    Permisos.aspx ───────> MatrixNext.Web/Controllers/PermisosController.cs
                         └──> MatrixNext.Web/Views/Permisos/Index.cshtml
```

### Orden Recomendado de Migración
1. **[COMPLETADO]** Login (`Default.aspx`) - Base para autenticación
2. **[SIGUIENTE]** `US_Usuarios/` - Gestión de usuarios y permisos (14 páginas)
3. `Home/` - Dashboard post-login (3 páginas)
4. `PY_Proyectos/` - Gestión de proyectos (18 páginas)
5. `OP_Cuantitativo/` - Operaciones cuantitativas
6. `OP_Cualitativo/` - Operaciones cualitativas
7. `FI_AdministrativoFinanciero/` - Finanzas (21 páginas)
8. Módulos restantes por prioridad operativa

### Por qué Módulo Completo?
✅ **Ventajas:**
- Minimiza impacto en CoreProject (actualiza un contexto a la vez)
- Agrupa lógica relacionada en MatrixNext
- Facilita testing del módulo en su totalidad
- Reduces cambios disruptivos en la BD
- Permite validar patrón MVC antes de escalar

❌ **Evita:**
- Migrar aspx aisladas sin contexto del módulo
- Dependencias rotas entre páginas del mismo módulo
- Duplicación de servicios/repositorios

## Checklist operativo (iterativo por módulo)
- [ ] Seleccionar módulo WebForms (carpeta completa: ej `US_Usuarios/`, `PY_Proyectos/`)
- [ ] Analizar dependencias entre páginas .aspx del módulo y contextos CoreProject requeridos
- [ ] Crear adaptador/wrapper de contextos CoreProject en MatrixNext.Data (ej: `UsuarioContextAdapter`)
- [ ] Diseñar Controllers + ViewModels para cada .aspx del módulo
- [ ] Crear Views en carpeta temática (ej: `Views/Usuarios/`, `Views/Proyectos/`)
- [ ] Implementar servicios de dominio en MatrixNext.Data reutilizando lógica CoreProject
- [ ] Migrar validaciones (data annotations/FluentValidation)
- [ ] Reemplazar SQL Server stored procedures por servicios Dapper/EF
- [ ] Aplicar estilos MVCMatrix al layout temático
- [ ] Configurar rutas en Program.cs según patrón (área opcional si módulos muy grandes)
- [ ] Testing: unidad (servicios), integracion (DbContext/Dapper), controller/action
- [ ] Revisión UX y accesibilidad
- [ ] Deploy y validación en ambiente destino
- [ ] Eliminar módulo de WebMatrix y verificar que no haya referencias residuales

## Notas tecnicas iniciales
- EF Core paquetes: `Microsoft.EntityFrameworkCore.SqlServer` 8.0.10; Dapper 2.1.35.
- Preferir configuracion via `IOptions` y `appsettings.*`; secretos fuera del repo.
- Transacciones: `DbContext` + `IDbContextTransaction`; para Dapper usar la misma conexion/transaction cuando mezcle EF/Dapper.
- Evitar acoplar Controllers a EF; usar servicios/handlers inyectados.
- Migraciones: mantener en `MatrixNext.Data` y versionar junto al codigo.

## Proximo paso sugerido
**Fase 2: Migración del Módulo US_Usuarios (Seguridad)**

### Por qué US_Usuarios primero?
1. **Infraestructura de autenticación ya existe** (LoginController implementado)
2. **Bajo riesgo de dependencias externas** - módulo self-contained
3. **Prepara terreno para otros módulos** - gestión de roles/permisos usada por todos
4. **Proporciona patrón replicable** para siguientes módulos

### Plan de acción US_Usuarios
1. **Crear adaptador CoreProject** en MatrixNext.Data
   ```csharp
   // MatrixNext.Data/Adapters/UsuarioContextAdapter.cs
   public class UsuarioContextAdapter
   {
       private readonly US_Entities _context = new US_Entities();
       
       // Métodos que envuelven funcionalidad de CoreProject
       public List<US_Usuarios> ObtenerTodos() => _context.US_Usuarios.ToList();
       public List<US_Usuarios> ObtenerPorRol(int rolId) => ...
       // etc
   }
   ```

2. **Migrar 14 páginas de US_Usuarios/**
   - Usuarios.aspx → UsuariosController.Index/Create/Edit/Delete
   - Roles.aspx → RolesController.Index/Create/Edit
   - Permisos.aspx → PermisosController.Index/Assign
   - GrupoUnidad.aspx → GrupoUnidadController
   - (+ 10 páginas más)

3. **Estructurar Views**
   ```
   Views/
     Usuarios/
       Index.cshtml
       Create.cshtml
       Edit.cshtml
     Roles/
       Index.cshtml
       Create.cshtml
       Edit.cshtml
     GrupoUnidad/
       Index.cshtml
   ```

4. **Implementar servicios de dominio**
   - UsuarioService (CRUD + búsquedas)
   - RolService (asignaciones, permisos)
   - GrupoUnidadService
   - PermisosService

5. **Configurar autorización**
   - Validar que usuarios migrados mantienen roles/permisos
   - Aplicar [Authorize] con políticas apropiadas

### Estrategia sin coexistencia
- ❌ NO mantener WebMatrix en paralelo
- ✅ Retirar módulo completo de WebMatrix cuando esté 100% migrado en MatrixNext
- ✅ Redirecciones en lugar de duplicación
- ✅ Testing exhaustivo antes de retirar módulo legacy

---

**Siguiente acción concreta**: Crear estructura US_Usuarios en MatrixNext.Web/Data y comenzar migración con página "Usuarios.aspx"

## Estado actual (Última actualización - 2 Enero 2026)
- ✅ **Login funcional** con autenticación robusta (cookies + claims + session)
- ✅ **Infraestructura completa**: Health checks, manejo global de errores, logging básico
- ✅ **Layout MVCMatrix** integrado con todos los assets
- ✅ **Estrategia de datos definida**: Migración incremental por módulo reutilizando CoreProject
- ✅ **Orden de módulos definido**: 26 módulos clasificados por prioridad sin coexistencia
- ✅ **Patrón de migración establecido**: Por módulo completo → páginas individuales
- 🔄 **PRÓXIMO PASO**: Migración del módulo US_Usuarios (14 páginas de seguridad y permisos)
