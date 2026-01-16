# ANÁLISIS SPRINT 20: MÓDULO INVENTARIO

**Fecha**: 2026-01-16  
**Módulo**: Inventario (INV)  
**Prioridad**: 🟡 BAJA  
**Estado**: 🔵 EN ANÁLISIS

---

## 📋 RESUMEN EJECUTIVO

El módulo de **Inventario** es el sistema de gestión de activos de la compañía. Administra:
- **Registro de Artículos**: Equipos, consumibles, papelería, periféricos
- **Asignación de Activos Fijos**: Tablets, computadores, periféricos a empleados
- **Mantenimiento de Equipos**: Histórico de mantenimientos preventivos/correctivos
- **Stock de Consumibles**: Control de entrega/devolución de consumibles
- **Legalizaciones**: Legalización de consumibles entregados
- **Reportes**: Remanente de stock, legalizaciones

Este es el **ÚLTIMO módulo** del proyecto de migración.

---

## 📂 ESTRUCTURA EN WEBMATRIX

### Páginas identificadas (7):

| Página | Función | Complejidad |
|--------|---------|-------------|
| `RegistroArticulos.aspx` | CRUD de artículos (equipos, consumibles) | 🔴 ALTA (2759 líneas) |
| `AsignacionActivosFijos.aspx` | Asignar tablets/equipos a empleados | 🟠 MEDIA |
| `MantenimientoEquipos.aspx` | Registro de mantenimientos | 🟢 BAJA |
| `EntregaConsumibles.aspx` | Control de stock consumibles | 🟠 MEDIA |
| `Legalizaciones.aspx` | Legalizar consumibles entregados | 🟠 MEDIA |
| `ReporteLegalizaciones.aspx` | Reporte de legalizaciones | 🟢 BAJA |
| `ReporteRemanente.aspx` | Reporte de stock remanente | 🟢 BAJA |

---

## 🗄️ STORED PROCEDURES IDENTIFICADOS

### Registro de Artículos (CRUD principal):

| SP | Propósito | Parámetros clave |
|----|-----------|------------------|
| `INV_RegistroArticulos_Get` | Obtener artículos con filtros complejos | `Id`, `TipoArticulo`, `Articulo`, `TipoComputador`, `Estado`, `Sede`, `UsuarioAsignado`, `TodosCampos` |
| `INV_RegistroArticulos_Add` | Crear artículo (50 parámetros) | `TipoArticulo`, `Articulo`, `FechaCompra`, `UsuarioRegistra`, `CentroCosto`, `BU`, `JobBook`, etc. |
| `INV_RegistroArticulos_Edit` | Actualizar artículo | Mismos parámetros que Add + `Id` |
| `INV_RegistroArticulos_Asignado_Edit` | Marcar artículo como asignado/disponible | `Id`, `Asignado` |

### Asignaciones de Activos Fijos:

| SP | Propósito | Parámetros clave |
|----|-----------|------------------|
| `INV_Asignaciones_Get` | Obtener asignaciones con filtros | `IdActivoFijo`, `Articulo`, `BU`, `JobBook`, `UsuarioAsignado`, `Asignado` |
| `INV_Asignaciones_Add` | Crear asignación | `IdActivoFijo`, `UsuarioAsignado`, `FechaAsignacion`, `BU`, `JobBook`, `Ciudad`, `EstadoTablet` |
| `INV_Asignaciones_Edit` | Actualizar asignación | Mismos parámetros + `Id` |
| `INV_Asignaciones_Del` | Eliminar asignación | `IdActivoFijo` |
| `INV_LogAsignaciones_Add` | Log de asignaciones (auditoría) | `IdActivoFijo`, `IdArticulo`, `IdUsuario`, `Asignado` |

### Stock de Consumibles:

| SP | Propósito | Parámetros clave |
|----|-----------|------------------|
| `INV_StockConsumibles_Get` | Obtener stock con filtros | `Id`, `IdConsumible`, `TipoMovimiento`, `BU`, `JobBook`, `UsuarioAsignado`, `Legalizado` |
| `INV_StockConsumibles_Add` | Registrar movimiento de stock | `IdConsumible`, `Fecha`, `TipoMovimiento`, `Estado`, `BU`, `JobBook`, `Valor`, `Total`, `Disponible` |
| `INV_StockxLegalizar_Get` | Obtener stock pendiente de legalizar | `BU`, `JobBook`, `Articulo`, `UsuarioAsignado`, `IdConsumible` |

### Legalizaciones:

| SP | Propósito | Parámetros clave |
|----|-----------|------------------|
| `INV_Legalizaciones_Get` | Obtener legalizaciones | `Id`, `IdConsumible`, `BU`, `JobBook`, `Articulo`, `UsuarioAsignado` |
| `INV_Legalizaciones_Add` | Crear legalización | `IdConsumible`, `TipoLegalizacion`, `Radicado`, `Unidades`, `Firmas`, `ValorLegalizado`, `Legalizado` |
| `INV_Legalizaciones_Edit` | Actualizar legalización | Mismos parámetros + `Id` |
| `INV_Legalizaciones_Del` | Eliminar legalización | `Id` |

### Mantenimiento de Equipos:

| SP | Propósito | Parámetros clave |
|----|-----------|------------------|
| `INV_MantenimientoEquipos_Get` | Obtener mantenimientos | `Id`, `IdActivoFijo`, `Articulo`, `TipoMantenimiento`, `UsuarioResponsable` |
| `INV_MantenimientoEquipos_Add` | Registrar mantenimiento | `IdActivoFijo`, `Fecha`, `TipoMantenimiento`, `UsuarioResponsable`, `Observaciones` |
| `INV_MantenimientoEquipos_Edit` | Actualizar mantenimiento | Mismos parámetros + `Id` |

### Catálogos y Reportes:

| SP | Propósito |
|----|-----------|
| `INV_Articulos_Get` | Catálogo de artículos disponibles |
| `INV_BU_Get` | Business Units |
| `INV_Sede_Get` | Sedes |
| `INV_EstadoArticulo_Get` | Estados de artículos |
| `INV_EstadoConsumible_Get` | Estados de consumibles |
| `INV_EstadoTablet_Get` | Estados de tablets |
| `INV_TipoMovimiento_Get` | Tipos de movimiento de stock |
| `INV_TipoLegalizacion_Get` | Tipos de legalización |
| `INV_Papeleria_Get` | Catálogo de papelería |
| `INV_Perifericos_Get` | Catálogo de periféricos |
| `INV_Camaras_Get` | Catálogo de cámaras |
| `INV_ValorBono_Get` | Valores de bonos |
| `INV_ReporteLegalizaciones` | Reporte de legalizaciones |
| `INV_ReporteRemanente` | Reporte de stock remanente |

**Total SP identificados**: **31 stored procedures**

---

## 🎯 FLUJOS DE NEGOCIO PRINCIPALES

### 1. Registro de Artículos

**Flujo**: 
1. Usuario selecciona tipo de artículo (Computador, Tablet, Celular, Consumible, etc.)
2. Según tipo, se despliegan campos específicos:
   - **Computador**: Marca, Modelo, Procesador, Memoria, Almacenamiento, Sistema Operativo, Serial, Office, Programas
   - **Tablet**: IdTablet, IdSTG, Tamaño Pantalla, Estado Tablet
   - **Celular**: Chip, IMEI, Operador, Número, Cantidad Minutos
   - **Consumible**: Tipo Producto, Producto, Cantidad
   - **Periférico**: Tipo Periférico
   - **Papelería**: Producto Papelería, Cantidad
3. Campos comunes: Fecha Compra, Centro Costo, BU, JobBook, Cuenta Contable, Valor, Estado, Sede
4. Se ejecuta `INV_RegistroArticulos_Add` que retorna el `Id` del artículo creado
5. El artículo queda disponible para asignación

**Validaciones**:
- Tipo artículo requerido
- Artículo requerido
- Fecha compra no futura
- Valor > 0
- Campos específicos según tipo artículo

### 2. Asignación de Activos Fijos

**Flujo**:
1. Se listan artículos disponibles (`Asignado = false`)
2. Usuario selecciona artículo y empleado destino
3. Se completan datos: Fecha Asignación, BU, JobBook, Ciudad, Estado Tablet (si aplica), Cargo
4. Se ejecuta `INV_Asignaciones_Add`
5. Se ejecuta `INV_LogAsignaciones_Add` para auditoría
6. Se actualiza artículo con `INV_RegistroArticulos_Asignado_Edit` (`Asignado = true`)

**Validaciones**:
- Artículo debe estar disponible (`Asignado = false`)
- Usuario destino requerido
- No se puede eliminar si empleado tiene consumibles pendientes de legalizar

### 3. Entrega de Consumibles (Stock)

**Flujo**:
1. Usuario selecciona consumible del catálogo
2. Selecciona tipo de movimiento (Entrada, Salida)
3. Completa: Fecha, BU, JobBook, Ciudad, Valor, Cantidad
4. Para **Salida**: selecciona usuario destino
5. Se calcula: `Total = Valor * Cantidad`, `Disponible = Stock anterior ± Cantidad`
6. Se ejecuta `INV_StockConsumibles_Add`
7. Movimiento queda pendiente de legalizar

**Validaciones**:
- Tipo movimiento requerido
- Para Salida: usuario destino requerido
- Stock disponible >= Cantidad (si es Salida)
- Valor > 0

### 4. Legalización de Consumibles

**Flujo**:
1. Sistema obtiene stock pendiente con `INV_StockxLegalizar_Get`
2. Usuario selecciona registros a legalizar
3. Ingresa: Tipo Legalización, Radicado, Fecha, Unidades, Firmas, Devoluciones, Notas Crédito, Descuento Nómina
4. Se calcula: `ValorLegalizado`, `Pendiente`
5. Se ejecuta `INV_Legalizaciones_Add`
6. Se actualiza stock con `Legalizado = true`

**Validaciones**:
- Stock debe estar en estado "Por Legalizar"
- Radicado requerido
- Suma de Firmas + Devoluciones + NotasCredito + DescuentoNomina <= Total entregado

### 5. Mantenimiento de Equipos

**Flujo**:
1. Usuario selecciona activo fijo asignado
2. Ingresa: Fecha, Tipo Mantenimiento (Preventivo/Correctivo), Responsable, Observaciones
3. Se ejecuta `INV_MantenimientoEquipos_Add`
4. Se registra en histórico del equipo

**Validaciones**:
- Activo debe estar asignado
- Fecha no futura
- Tipo mantenimiento requerido

---

## 🧩 COMPONENTES A CREAR

### DTOs (8 clases):

1. **RegistroArticuloDto** 
   - 50 propiedades (dinámicas según tipo artículo)
   - Validaciones condicionales según `IdTipoArticulo`

2. **RegistroArticuloListDto**
   - Versión simplificada para grids
   - Propiedades calculadas: `TipoArticuloNombre`, `ArticuloNombre`, `EstadoNombre`, `AsignadoTexto`

3. **AsignacionActivoDto**
   - `IdActivoFijo`, `UsuarioAsignado`, `FechaAsignacion`, `BU`, `JobBook`, `Ciudad`, `EstadoTablet`, `Observacion`

4. **AsignacionListDto**
   - Versión para grid con datos joined

5. **StockConsumibleDto**
   - `IdConsumible`, `TipoMovimiento`, `Fecha`, `BU`, `JobBook`, `Valor`, `Cantidad`, `UsuarioAsignado`

6. **StockConsumibleListDto**
   - Para grid con calculados

7. **LegalizacionDto**
   - `IdConsumible`, `TipoLegalizacion`, `Radicado`, `Unidades`, `Firmas`, `Devoluciones`, `NotasCredito`, `DescuentoNomina`

8. **MantenimientoEquipoDto**
   - `IdActivoFijo`, `Fecha`, `TipoMantenimiento`, `UsuarioResponsable`, `Observaciones`

### Adapters (5 clases):

1. **RegistroArticulosAdapter**
   - `IObtenerTodosAsync()`, `ObtenerPorIdAsync(id)`, `CrearAsync(dto)`, `ActualizarAsync(dto)`, `ActualizarAsignadoAsync(id, asignado)`

2. **AsignacionesAdapter**
   - `ObtenerTodosAsync()`, `CrearAsync(dto)`, `ActualizarAsync(dto)`, `EliminarAsync(id)`, `CrearLogAsync(dto)`

3. **StockConsumiblesAdapter**
   - `ObtenerTodosAsync()`, `ObtenerPorLegalizarAsync(filtros)`, `CrearAsync(dto)`

4. **LegalizacionesAdapter**
   - `ObtenerTodosAsync()`, `CrearAsync(dto)`, `ActualizarAsync(dto)`, `EliminarAsync(id)`

5. **MantenimientoEquiposAdapter**
   - `ObtenerTodosAsync()`, `ObtenerPorActivoAsync(idActivoFijo)`, `CrearAsync(dto)`, `ActualizarAsync(dto)`

### Services (5 clases):

1. **RegistroArticulosService**
   - Validaciones según tipo artículo
   - Cálculos de totales
   - Lógica de disponibilidad

2. **AsignacionesService**
   - Validar disponibilidad de artículo
   - Crear log de auditoría
   - Actualizar estado artículo

3. **StockConsumiblesService**
   - Calcular stock disponible
   - Validar movimientos
   - Identificar consumibles por legalizar

4. **LegalizacionesService**
   - Validar montos
   - Calcular valores legalizados
   - Actualizar stock

5. **MantenimientoEquiposService**
   - Validar fechas
   - Histórico de mantenimientos

### Controllers (5):

1. **RegistroArticulosController** (área INV)
   - Index, Create, Edit, Details, Delete
   - GetArticulosPorTipo (AJAX)

2. **AsignacionesController** (área INV)
   - Index, Create, Edit, Details, Delete
   - GetArticulosDisponibles (AJAX)

3. **StockConsumiblesController** (área INV)
   - Index, Create

4. **LegalizacionesController** (área INV)
   - Index, Create, Edit, Delete

5. **MantenimientoEquiposController** (área INV)
   - Index, Create, Edit

### Views (15 archivos):

```
Areas/INV/Views/
├── RegistroArticulos/
│   ├── Index.cshtml
│   ├── _CreateEdit.cshtml (modal complejo con tabs por tipo artículo)
│   ├── _Details.cshtml
│
├── Asignaciones/
│   ├── Index.cshtml
│   ├── _CreateEdit.cshtml
│   ├── _Details.cshtml
│
├── StockConsumibles/
│   ├── Index.cshtml
│   ├── _CreateEdit.cshtml
│
├── Legalizaciones/
│   ├── Index.cshtml
│   ├── _CreateEdit.cshtml
│
├── MantenimientoEquipos/
│   ├── Index.cshtml
│   ├── _CreateEdit.cshtml
│
└── Reportes/
    ├── ReporteLegalizaciones.cshtml
    └── ReporteRemanente.cshtml
```

### JavaScript/CSS (5 archivos):

```
wwwroot/
├── js/inv/
│   ├── registro-articulos.js (lógica compleja de tabs)
│   ├── asignaciones.js
│   ├── stock-consumibles.js
│   ├── legalizaciones.js
│   └── mantenimiento.js
│
└── css/inv/
    ├── inventario.css (estilos compartidos)
    └── tabs-articulos.css (estilos de tabs por tipo)
```

---

## ⚠️ COMPLEJIDAD Y RIESGOS

### 🔴 ALTA COMPLEJIDAD:

1. **RegistroArticulos.aspx (2759 líneas)**
   - Formulario dinámico con 8 tipos de artículos
   - Cada tipo tiene campos específicos
   - Requiere tabs o collapse para organizar UI
   - 50 parámetros en SP Add/Edit

2. **Validaciones condicionales**
   - Campos requeridos cambian según `IdTipoArticulo`
   - Lógica de negocio compleja (stock, legalizaciones)

3. **Relaciones entre entidades**
   - Artículo → Asignación (1:N)
   - Artículo → Mantenimiento (1:N)
   - Consumible → Stock → Legalización

### 🟠 RIESGOS:

1. **Performance**: Grid de artículos puede tener miles de registros
   - Solución: Paginación server-side, filtros eficientes

2. **Permisos**: Diferentes roles tienen acceso a diferentes tipos de artículos
   - Solución: Filtrado por `GrupoUnidad` en SP

3. **Concurrencia**: Múltiples usuarios asignando el mismo artículo
   - Solución: Validar `Asignado = false` antes de crear asignación

---

## 📊 ESTIMACIÓN

| Fase | Tareas | Estimación |
|------|--------|------------|
| **Fase 1: Análisis** | Documentación, identificación SP | ✅ Completado |
| **Fase 2: Data Layer** | DTOs (8), Adapters (5), Services (5) | 2-3 días |
| **Fase 3: Web Layer** | Controllers (5), Views (15), JS/CSS (5) | 3-4 días |
| **Fase 4: Testing** | Build, pruebas funcionales, ajustes | 1-2 días |
| **Fase 5: Documentación** | MIGRACION_INVENTARIO_COMPLETADA.md | 1 día |
| **TOTAL** | | **7-10 días (1-2 semanas)** |

---

## 📝 NOTAS IMPORTANTES

1. ✅ **Todos los SP existen** en `CO_Matrix_SP_Names.csv`
2. ✅ **CoreProject** tiene clase `Inventario.vb` completa con todos los métodos
3. ⚠️ **Módulo más complejo** del proyecto por cantidad de entidades y relaciones
4. 🎯 **Último módulo** de la migración - al completarlo, el proyecto estará 100% migrado

---

## 🚀 PRÓXIMOS PASOS

1. ✅ Crear este documento de análisis
2. ⬜ Crear DTOs (8 clases)
3. ⬜ Crear Adapters (5 clases)
4. ⬜ Crear Services (5 clases)
5. ⬜ Crear Controllers (5 clases)
6. ⬜ Crear Views (15 archivos)
7. ⬜ Crear JavaScript/CSS (5 archivos)
8. ⬜ Registrar DI en Program.cs
9. ⬜ Actualizar menú en _main-sidebar.cshtml
10. ⬜ Testing y build verification
11. ⬜ Documentación final

---

**Documento creado**: 2026-01-16  
**Última actualización**: 2026-01-16
