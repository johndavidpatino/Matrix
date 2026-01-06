# Documentación de Módulos FI y CC

Carpeta centralizada para toda la documentación relacionada con los módulos de **Finanzas/Compras (FI)** y **FinzOpe/Liquidación (CC)**.

## 📋 Documentos Incluidos

### CU - Cuentas y Presupuestos (Cotización)

- **[ANALISIS_CU_CUENTAS.md](ANALISIS_CU_CUENTAS.md)** - Análisis detallado del módulo de Cuentas
  - Estructura de datos de cuentas comerciales
  - Funcionalidades principales
  - Procedimientos almacenados relacionados
  - Checklist de migración

- **[ANALISIS_CU_PRESUPUESTO.md](ANALISIS_CU_PRESUPUESTO.md)** - Análisis detallado del módulo de Presupuestos
  - Gestión de alternativas presupuestales
  - GridViews y controles dinámicos (50+ campos)
  - Relaciones maestro-detalle complejas
  - Matriz de cambios técnicos y riesgos
  - Épicas y estimaciones de esfuerzo

- **[REPORT_CU_CUENTAS_IMPLEMENTACION.md](REPORT_CU_CUENTAS_IMPLEMENTACION.md)** - Reporte de implementación del módulo Cuentas
  - Estado actual de migración
  - Archivos creados/modificados
  - Cambios aplicados

- **[REPORT_CU_PRESUPUESTO.md](REPORT_CU_PRESUPUESTO.md)** - Reporte de implementación del módulo Presupuestos
  - Detalles de migración del módulo complejo
  - Pruebas realizadas
  - Validaciones de funcionamiento

## 🎯 Estructura de Módulos en MatrixNext

```
MatrixNext.Data/Modules/CU/
├── Adapters/           # Dapper adapters para SP de Cuentas y Presupuestos
├── Services/           # Lógica de negocio de CU
├── DTOs/              # Data transfer objects
└── ServiceCollectionExtensions.cs

MatrixNext.Web/Areas/CU/
└── Controllers/        # API endpoints para Cuentas y Presupuestos
```

## 📌 Notas Importantes

- El módulo **CU** maneja tanto **Cuentas comerciales** como **Presupuestos/Cotizaciones**
- La documentación de **FI (Finanzas/Compras)** será agregada cuando se inicie su migración
- El módulo **CC (FinzOpe)** tendrá su propia documentación cuando sea relevante
- Todos los archivos de análisis contienen listas de verificación pre-migración

## 🔗 Documentación Relacionada

Para documentación general de arquitectura y directrices, ver:
- [MigrationPlan.md](../../MigrationPlan.md) - Plan de migración general
- [DIRECTRICES_MIGRACION.md](../../MatrixNext/DIRECTRICES_MIGRACION.md) - Directrices y patrones
