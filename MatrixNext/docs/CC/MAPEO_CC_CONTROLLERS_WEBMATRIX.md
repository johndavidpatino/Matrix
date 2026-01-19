# MAPEO: Controllers CC (MatrixNext) → Páginas WebMatrix

> **Fecha**: 2026-01-18  
> **Propósito**: Identificar equivalencias entre controllers del área CC y páginas legacy de WebMatrix  
> **Carpetas analizadas**:
> - MatrixNext: `MatrixNext.Web/Areas/CC/Controllers/` (22 controllers)
> - WebMatrix: `WebMatrix/CC_FinzOpe/` (31 páginas .aspx)
> - WebMatrix: `WebMatrix/FI_AdministrativoFinanciero/` (21 páginas .aspx)
> - WebMatrix: `WebMatrix/CAP/` (11 páginas .aspx)

---

## 📊 RESUMEN EJECUTIVO

| Categoría | Cantidad | Porcentaje |
|-----------|----------|------------|
| ✅ Controllers CON equivalencia WebMatrix | **15** | 68% |
| ⚠️ Controllers POSIBLE equivalencia (verificar) | **3** | 14% |
| ❌ Controllers SIN equivalencia clara | **4** | 18% |
| ❌ Páginas WebMatrix SIN controller migrado | **28** | - |

---

## ✅ CONTROLLERS CON EQUIVALENCIA CONFIRMADA (15)

| # | Controller MatrixNext | Página WebMatrix | Carpeta | Confianza |
|---|----------------------|------------------|---------|-----------|
| 1 | `CargueDescuentosSsController.cs` | `CargueDescuentosSS.aspx` | CC_FinzOpe | 🟢 Alta |
| 2 | `ConteoTrabajosController.cs` | `ConteoTrabajos.aspx` | CC_FinzOpe | 🟢 Alta |
| 3 | `ControlPresupuestosController.cs` | `ControlPresupuestos.aspx` | CAP | 🟢 Alta |
| 4 | `EstadoJobBooksController.cs` | `EstadoJobBooks.aspx` | CC_FinzOpe | 🟢 Alta |
| 5 | `GenerarBonificacionController.cs` | `GenerarBonificacion.aspx` | CC_FinzOpe | 🟢 Alta |
| 6 | `LiquidarPlanillasActividadesController.cs` | `LiquidarPlanillasActividades.aspx` | CC_FinzOpe | 🟢 Alta |
| 7 | `LiquidarProductividadPstController.cs` | `LiquidarProductividadPST.aspx` | CC_FinzOpe | 🟢 Alta |
| 8 | `PresupuestosInternosController.cs` | `PresupuestosInternos.aspx` + `PresupuestosInternosIndex.aspx` + `PresupuestoInterno.aspx` | CC_FinzOpe | 🟢 Alta |
| 9 | `RegistroProduccionController.cs` | `Produccion.aspx` | CC_FinzOpe | 🟢 Alta |
| 10 | `ReporteActividadesProduccionController.cs` | `ReporteActividadesProduccion.aspx` | CC_FinzOpe | 🟢 Alta |
| 11 | `ReporteContabilizacionPstController.cs` | `ReporteContabilizacionPST.aspx` | CC_FinzOpe | 🟢 Alta |
| 12 | `ReporteConteosController.cs` | `ReporteConteoTrabajos.aspx` | CC_FinzOpe | 🟢 Alta |
| 13 | `ReportePagosController.cs` | `ReportePagos.aspx` | CC_FinzOpe | 🟢 Alta |
| 14 | `RequerimientosEquipoController.cs` | `GenerarRequerimientos.aspx` + `ListadodeRequerimientos.aspx` | CC_FinzOpe | 🟢 Alta |
| 15 | `ResumenProductividadController.cs` | `ResumenesdeProduccion.aspx` | CC_FinzOpe | 🟢 Alta |

---

## ⚠️ CONTROLLERS CON EQUIVALENCIA POSIBLE (Verificar) (3)

| # | Controller MatrixNext | Posible Página WebMatrix | Notas |
|---|----------------------|--------------------------|-------|
| 1 | `CcFinzOpeController.cs` | **Múltiples páginas CC_FinzOpe** | Controller base/genérico que agrupa funcionalidades. Mapea parcialmente a: `LiquidacionTarifas.aspx`, `OrdenesdeServicio.aspx` |
| 2 | `RevisarGeneracionBonificacionController.cs` | `GenerarBonificacion.aspx` (parte 2) | Parece ser una vista de revisión de la misma funcionalidad de bonificación. Verificar si es tab/sección de `GenerarBonificacion.aspx` |
| 3 | `AnulacionLiquidacionesController.cs` | `OrdenesdeServicio.aspx` (función anular) | La funcionalidad de anulación está **embebida** en `OrdenesdeServicio.aspx` (ver línea 330: `ToolTip="Anular"`). No es página separada |

---

## ❌ CONTROLLERS SIN EQUIVALENCIA CLARA EN WEBMATRIX (4)

> **⚠️ ACCIÓN REQUERIDA**: Verificar si estas funcionalidades son NUEVAS (no deberían existir según política de migración) o si están en otra ubicación

| # | Controller MatrixNext | Análisis | Recomendación |
|---|----------------------|----------|---------------|
| 1 | `AsignacionCostosPstController.cs` | **NO encontrado** en WebMatrix. Buscado: "AsignacionCostos", "CostosPst", "Asignar Costos" | 🔴 **VERIFICAR**: ¿Es funcionalidad nueva? Si no existe en WebMatrix → REMOVER |
| 2 | `CalculoJornadaLaboralController.cs` | **NO encontrado** en WebMatrix. Buscado: "JornadaLaboral", "CalculoJornada", "Jornada" | 🔴 **VERIFICAR**: ¿Es funcionalidad nueva? Si no existe en WebMatrix → REMOVER |
| 3 | `ConsolidacionProduccionController.cs` | **NO encontrado** como página separada. Posible parte de `Produccion.aspx` o `ResumenesdeProduccion.aspx` | 🟡 **VERIFICAR**: ¿Es proceso interno dentro de otra página? |
| 4 | `ReporteVarianzasPresupuestariasController.cs` | **NO encontrado** en WebMatrix. Buscado: "Varianza", "VarianzasPresupuestarias" | 🔴 **VERIFICAR**: ¿Es funcionalidad nueva? Si no existe en WebMatrix → REMOVER |

---

## ❌ PÁGINAS WEBMATRIX SIN CONTROLLER MIGRADO (28)

### CC_FinzOpe (16 páginas pendientes)

| # | Página WebMatrix | Funcionalidad | Prioridad |
|---|------------------|---------------|-----------|
| 1 | `ActividadesTrabajo.aspx` | Gestión de actividades por trabajo | 🟡 Media |
| 2 | `CargarInformacion.aspx` | Carga de información general | 🟡 Media |
| 3 | `ConfiguracionPresupuesto.aspx` | Configuración de presupuestos | 🟠 Alta |
| 4 | `CuentasdeCobro.aspx` | Gestión cuentas de cobro | 🔴 Crítica |
| 5 | `EliminarCargueProduccion.aspx` | Eliminar cargue producción | 🟡 Media |
| 6 | `Evaluacion-Facturas-Operaciones.aspx` | Evaluación facturas operaciones | 🟡 Media |
| 7 | `ExportarProduccionIDs.aspx` | Exportar producción por IDs | 🟢 Baja |
| 8 | `LiquidacionTarifas.aspx` | Liquidación de tarifas | 🟠 Alta |
| 9 | `ListadoCuentasRecibidas.aspx` | Listado cuentas recibidas | 🟠 Alta |
| 10 | `ListadoTrabajos.aspx` | Listado de trabajos | 🟠 Alta |
| 11 | `OrdenesdeServicio.aspx` | Órdenes de servicio | 🔴 Crítica |
| 12 | `PresupuestosPorSegmento.aspx` | Presupuestos por segmento | 🟡 Media |
| 13 | `RecepcionCuentasdeCobro.aspx` | Recepción cuentas de cobro | 🔴 Crítica |
| 14 | `ReporteOrdenesdeServicio.aspx` | Reporte órdenes de servicio | 🟡 Media |
| 15 | `Trabajos.aspx` | Gestión de trabajos | 🟠 Alta |
| 16 | `TrabajosSinPresupuesto.aspx` | Trabajos sin presupuesto | 🟡 Media |

### FI_AdministrativoFinanciero (12 páginas pendientes)

| # | Página WebMatrix | Funcionalidad | Prioridad |
|---|------------------|---------------|-----------|
| 1 | `Aprobacion-Evaluacion-Facturas.aspx` | Aprobación/Evaluación facturas | 🔴 Crítica |
| 2 | `Contratacion.aspx` | Módulo contratación | 🟠 Alta |
| 3 | `Default.aspx` | Home del módulo (menú) | 🟢 Baja |
| 4 | `Default-Compras.aspx` | Home compras | 🟢 Baja |
| 5 | `DetalleRequerimientos.aspx` | Detalle de requerimientos | 🟡 Media |
| 6 | `Evaluacion-Proveedor-Facturas.aspx` | Evaluación proveedor facturas | 🟡 Media |
| 7 | `EvaluacionProveedores.aspx` | Evaluación de proveedores | 🟡 Media |
| 8 | `Gestion-Ordenes.aspx` + `Gestion-Ordenes1.aspx` + `Gestion-Ordenes-Aprobacion.aspx` | Gestión órdenes | 🔴 Crítica |
| 9 | `Gestion-Traza-Facturas.aspx` | Trazabilidad facturas | 🟡 Media |
| 10 | `HomeCompras.aspx` | Home compras | 🟢 Baja |
| 11 | `ListadoEstudios.aspx` | Listado de estudios | 🟡 Media |
| 12 | `ListadoPropuestas.aspx` | Listado de propuestas | 🟡 Media |
| 13 | `NominaDistribucionCostos.aspx` | Nómina distribución costos | 🟠 Alta |
| 14 | `PrestacionServicios-CT.aspx` | PST Contratistas | 🟠 Alta |
| 15 | `QuejasReclamosProveedores.aspx` | Quejas/Reclamos proveedores | 🟢 Baja |
| 16 | `Recepcion-Facturas.aspx` + `Recepcion-Facturas-Antiguo.aspx` | Recepción facturas | 🔴 Crítica |
| 17 | `ReporteFacturasRadicadas.aspx` | Reporte facturas radicadas | 🟡 Media |
| 18 | `ReporteOrdenesFacturas.aspx` | Reporte órdenes facturas | 🟡 Media |

---

## 🎯 RECOMENDACIONES

### 1. Controllers a REMOVER (si confirman que son nuevos):
```
- AsignacionCostosPstController.cs
- CalculoJornadaLaboralController.cs
- ReporteVarianzasPresupuestariasController.cs
```

### 2. Controllers a CONSOLIDAR:
```
- AnulacionLiquidacionesController.cs → Integrar en OrdenesServicioController (cuando se migre)
- RevisarGeneracionBonificacionController.cs → Integrar en GenerarBonificacionController
- ConsolidacionProduccionController.cs → Integrar en RegistroProduccionController
```

### 3. Páginas CRÍTICAS pendientes de migrar:
```
1. CuentasdeCobro.aspx → CC/CuentasCobroController
2. OrdenesdeServicio.aspx → CC/OrdenesServicioController  
3. RecepcionCuentasdeCobro.aspx → CC/RecepcionCuentasController
4. Recepcion-Facturas.aspx → FI/RecepcionFacturasController
5. Gestion-Ordenes.aspx → FI/GestionOrdenesController
```

---

## 📁 REFERENCIAS DE UBICACIONES

### WebMatrix
```
WebMatrix/
├── CC_FinzOpe/          ← Finanzas y Operaciones (31 páginas)
├── FI_AdministrativoFinanciero/  ← Administrativo Financiero (21 páginas)
├── CAP/                 ← Control de Presupuestos (11 páginas)
└── ...
```

### MatrixNext
```
MatrixNext.Web/
└── Areas/
    └── CC/
        └── Controllers/  ← 22 controllers
```

---

## 📋 CHECKLIST DE VERIFICACIÓN

- [ ] Confirmar con stakeholders si `AsignacionCostosPstController` es funcionalidad nueva
- [ ] Confirmar con stakeholders si `CalculoJornadaLaboralController` es funcionalidad nueva
- [ ] Confirmar con stakeholders si `ReporteVarianzasPresupuestariasController` es funcionalidad nueva
- [ ] Confirmar si `ConsolidacionProduccionController` debe ser parte de `RegistroProduccionController`
- [ ] Revisar `CcFinzOpeController` - parece ser un controller base demasiado genérico
- [ ] Priorizar migración de páginas críticas (Cuentas de Cobro, Órdenes de Servicio)

---

**Documento generado automáticamente para auditoría de migración MatrixNext**
