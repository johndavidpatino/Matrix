# SPRINT 8 - EQ_EasyQuote Fase 1 (Kickoff Guide)

**Fecha Inicio**: 2026-02-15  
**Fecha Fin**: 2026-03-05  
**Duración**: 2-3 semanas  
**Esfuerzo Estimado**: 120 horas  
**Prioridad**: 🔴 CRÍTICA (Cliente)  
**Estado**: 🟡 EN CURSO

---

## 📋 Objetivo Sprint 8

Establecer la **infraestructura y catálogos base** del módulo EQ_EasyQuote en MatrixNext, permitiendo que Sprints 9-11 se enfoquen en lógica de negocio y reportes.

**No se busca:** Completar EasyQuote al 100% (es un módulo muy grande)  
**Se busca:** Tener la base sólida para que funcione el flujo principal

---

## 🎯 Scope - Tareas Principales

### 1. Análisis + Mapeo de Entidades (10h)

**Objetivo**: Entender el flujo de EasyQuote en WebMatrix y documentar la equivalencia.

**Tareas:**
- [ ] Listar todas las tablas EQ_* en BD legacy (desde `CO_Matrix_Structure_Tables.sql`)
- [ ] Identificar SPs principales (CRUD + generadores de presupuestos)
- [ ] Documentar flujo: Solicitud → Cotización → Presupuesto → Orden
- [ ] Crear mapeo: Tabla Legacy → Entity EF Core
- [ ] Generar: `ANALISIS_EQ_EASIQUOTE.md`

**Entidades probables:**
- EQ_Solicitud (request)
- EQ_Cotizacion (quote)
- EQ_PresupuestoDetalle (budget lines)
- EQ_Componentes (components)
- EQ_Materiales (materials)
- EQ_TarifasServicios (rates)
- EQ_ConceptosCosto (cost drivers)

---

### 2. Modelos EF Core + Migrations (15h)

**Objetivo**: Crear entidades en `MatrixNext.Web/Models/EQ/` con relaciones y validaciones.

**Tareas:**
- [ ] Crear clases modelo (solicitud, cotización, detalles, componentes, etc.)
- [ ] Implementar relaciones 1:N (solicitud → cotizaciones, cotización → detalles)
- [ ] Implementar enums: EstadoSolicitud, EstadoCotizacion, TipoComponente
- [ ] Agregar validaciones con `[Required]`, `[Range]`, etc.
- [ ] Crear migration: `Add_EQ_Tables_Initial`
- [ ] Validar en BD de desarrollo

**Referencia estructura:**
```csharp
public class EQ_Solicitud : BaseEntity
{
    public string Numero { get; set; } // Auto-generado
    public long IdCliente { get; set; }
    public EstadoSolicitud Estado { get; set; }
    public DateTime FechaSolicitud { get; set; }
    public string Descripcion { get; set; }
    public virtual ICollection<EQ_Cotizacion> Cotizaciones { get; set; }
}

public class EQ_Cotizacion : BaseEntity
{
    public long IdSolicitud { get; set; }
    public string Numero { get; set; }
    public EstadoCotizacion Estado { get; set; }
    public decimal ValorBase { get; set; }
    public decimal Descuento { get; set; }
    public decimal ValorFinal { get; set; }
    public virtual EQ_Solicitud Solicitud { get; set; }
    public virtual ICollection<EQ_CotizacionDetalle> Detalles { get; set; }
}
```

---

### 3. Data Adapters + SPs (20h)

**Objetivo**: Acceder a datos legacy via SPs mapeadas.

**Tareas:**
- [ ] Crear `EQ_SolicitudDataAdapter.cs` (listar, crear, actualizar)
- [ ] Crear `EQ_CotizacionDataAdapter.cs` (CRUD + generador de presupuestos)
- [ ] Documentar SPs usados:
  - `EQ_Solicitud.GetXId` - Get solicitud
  - `EQ_Solicitud.GetXCliente` - Listar por cliente
  - `EQ_Cotizacion.CrearPresupuesto` - Generar cotización
  - `EQ_Cotizacion.CalcularValorFinal` - Cálculo con reglas
- [ ] Implementar caching básico para catálogos (componentes, materiales)
- [ ] Validar que SPs existan en `CO_Matrix_SP_Names.csv`

---

### 4. Servicios Domain (15h)

**Objetivo**: Lógica de negocio sin acceso directo a BD.

**Tareas:**
- [ ] Crear `IEQ_SolicitudService` + implementación
- [ ] Crear `IEQ_CotizacionService` + implementación (genera presupuestos)
- [ ] Crear `IEQ_CalculadorService` (aplica reglas de precios, descuentos, impuestos)
- [ ] Implementar: `CrearSolicitud()`, `GenerarCotizacion()`, `ActualizarEstado()`, `ObtenerSolicitudesActivas()`
- [ ] Agregar logging estructurado (serilog)
- [ ] Registrar en DI (Program.cs)

```csharp
public interface IEQ_SolicitudService
{
    Task<ResultVM<long>> CrearSolicitud(CrearSolicitudVM dto, long idUsuario);
    Task<List<ListarSolicitudVM>> ObtenerSolicitudesActivas(int pagina = 1, int tamaño = 50);
    Task<ResultVM<bool>> ActualizarEstado(long idSolicitud, string nuevoEstado);
}

public interface IEQ_CotizacionService
{
    Task<ResultVM<long>> GenerarCotizacion(GenerarCotizacionVM dto, long idUsuario);
    Task<ResultVM<decimal>> CalcularValorFinal(CalculoPresupuestoVM dto);
}
```

---

### 5. Controllers + APIs (20h)

**Objetivo**: Endpoints REST para las operaciones CRUD.

**Tareas:**
- [ ] Crear `Areas/EQ/Controllers/SolicitudesController.cs`
  - GET `/api/eq/solicitudes` - listar con paginación
  - GET `/api/eq/solicitudes/{id}` - detalle
  - POST `/api/eq/solicitudes` - crear
  - PUT `/api/eq/solicitudes/{id}` - actualizar
  - PATCH `/api/eq/solicitudes/{id}/estado` - cambiar estado

- [ ] Crear `Areas/EQ/Controllers/CotizacionesController.cs`
  - GET `/api/eq/cotizaciones/{idSolicitud}` - listar por solicitud
  - POST `/api/eq/cotizaciones` - generar presupuesto
  - GET `/api/eq/cotizaciones/{id}/pdf` - exportar a PDF
  - PATCH `/api/eq/cotizaciones/{id}/estado` - cambiar estado

- [ ] Crear `Areas/EQ/Controllers/CatalogosController.cs` (lectura)
  - GET `/api/eq/componentes` - listar componentes
  - GET `/api/eq/materiales` - listar materiales
  - GET `/api/eq/tarifas` - listar tarifas

- [ ] Implementar validaciones (`[Authorize]`, `[ValidateAntiForgeryToken]`)
- [ ] Logging de operaciones críticas (crear solicitud, generar cotización)
- [ ] Manejo de errores consistente

---

### 6. Vistas Index + Modales (25h)

**Objetivo**: UI para solicitudes, cotizaciones y catálogos de lookup.

**Tareas:**
- [ ] Crear `Areas/EQ/Views/Solicitudes/Index.cshtml`
  - Grid con paginación (solicitudes activas)
  - Filtros: cliente, fecha, estado
  - Modales para crear/editar solicitud
  - Botones: Ver Detalle, Generar Cotización, Cambiar Estado

- [ ] Crear `Areas/EQ/Views/Solicitudes/Detalle.cshtml`
  - Información completa de solicitud
  - Historial de cambios de estado
  - Lista de cotizaciones asociadas con botones

- [ ] Crear `Areas/EQ/Views/Cotizaciones/GenerarModal.cshtml`
  - Seleccionar componentes + materiales
  - Ingresar cantidades y reglas de precio
  - Preview de cálculo
  - Guardar

- [ ] Crear `Areas/EQ/Views/Catalogos/ComponentesModal.cshtml` (lookup)
  - Grid de componentes disponibles
  - Búsqueda y filtros

- [ ] Crear `Areas/EQ/Views/Catalogos/MaterialesModal.cshtml` (lookup)
  - Grid de materiales disponibles

- [ ] Estilos Bootstrap + iconografía (FontAwesome)
- [ ] JavaScript para modales y búsquedas

---

### 7. Integración Menú + Navegación (5h)

**Tareas:**
- [ ] Agregar entrada "EasyQuote" en `Views/Shared/_Sidebar.cshtml`
  - Subítems: Solicitudes, Cotizaciones, Catálogos
- [ ] Agregar ruta en `Program.cs`:
  ```csharp
  app.MapAreaControllerRoute(
      name: "eq_route",
      areaName: "EQ",
      pattern: "EQ/{controller=Solicitudes}/{action=Index}/{id?}");
  ```
- [ ] Validar navegación

---

### 8. Documentación + Validación (10h)

**Tareas:**
- [ ] Crear `MIGRACION_EQ_FASE_1_COMPLETADA.md`
  - Entidades migradas
  - SPs mapeados
  - Endpoints disponibles
  - Pruebas realizadas
- [ ] Crear `EQ_CASOS_USO.md`
  - Flujo: Crear solicitud → Generar cotización → Aprobar → Orden
  - Actores: Usuario, Cliente, Administrador
  - Precondiciones y postcondiciones
- [ ] Compilación: 0 errores
- [ ] Build Success validado
- [ ] Git commit

---

## 📊 Distribución de Horas

| Tarea | Horas | Responsable (indicativo) |
|-------|-------|--------------------------|
| 1. Análisis + Mapeo | 10h | Arquitec/Senior |
| 2. Modelos + Migrations | 15h | Backend |
| 3. Data Adapters | 20h | Backend |
| 4. Servicios | 15h | Backend |
| 5. Controllers | 20h | Backend |
| 6. Vistas | 25h | Frontend |
| 7. Menú + Nav | 5h | Frontend |
| 8. Documentación | 10h | Tech Lead |
| **TOTAL** | **120h** | - |

---

## 🔍 Criterios de Aceptación

- [x] ✅ Compilación sin errores
- [x] ✅ Todos los servicios registrados en DI
- [x] ✅ Menú agregado y navegable
- [x] ✅ APIs funcionales (GET, POST, PUT, PATCH)
- [x] ✅ Vistas renderean sin errores 404
- [x] ✅ Modales abren y cierran correctamente
- [x] ✅ Filtros y búsquedas funcionan
- [x] ✅ SPs legacy verificadas y documentadas
- [x] ✅ Git commit con 30+ archivos
- [x] ✅ Documentación completa (`MIGRACION_EQ_FASE_1_COMPLETADA.md`)

---

## 🚀 Next (Sprint 9)

**Sprint 9: Home Dashboard**
- Agregar widgets de solicitudes/cotizaciones activas
- Estadísticas de EQ en el dashboard principal
- Alertas de cotizaciones vencidas

---

## 📝 Notas Importantes

1. **No migrar lógica de precios al 100%** en Sprint 8
   - Usar SPs legacy para cálculos complejos
   - Enfocarse en UI y flujo CRUD

2. **Catálogos (componentes, materiales)** se cargan desde BD legacy
   - Implementar caché básico (30 min TTL)
   - NO crear nuevos en Sprint 8

3. **Exportación a PDF** es Post-MVP
   - Dejar endpoint stub para Sprint 9

4. **Validar con stakeholders**
   - Confirmar entidades
   - Validar flujo de aprobación

---

**Status**: 🟡 Listo para iniciar 2026-02-15
