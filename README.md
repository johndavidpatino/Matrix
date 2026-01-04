# CO_Matrix

[![Security Scan BackEnd](https://github.com/Ipsos-Dev-LATAM/CO_Matrix/actions/workflows/securityScan.yml/badge.svg?event=push)](https://github.com/Ipsos-Dev-LATAM/CO_Matrix/actions/workflows/securityScan.yml)
[![Build BackEnd](https://github.com/Ipsos-Dev-LATAM/CO_Matrix/actions/workflows/build.yml/badge.svg?event=push)](https://github.com/Ipsos-Dev-LATAM/CO_Matrix/actions/workflows/build.yml)

## 🚀 Migración WebMatrix → MatrixNext

**Estado actual**: 3/25 módulos completados (12%)

### Módulos Migrados ✅

- **US_Usuarios**: Gestión de usuarios, roles, permisos
- **TH_Ausencias**: Solicitudes de ausencias, aprobaciones, incapacidades
- **CU_Cuentas**: JobBooks, Brief, Propuestas, Estudios, Presupuestos completos

### Documentación de Migración

Toda la documentación de planificación y seguimiento se encuentra en [`MatrixNext/`](MatrixNext/):

- **[MODULOS_MIGRACION.md](MatrixNext/MODULOS_MIGRACION.md)**: Inventario completo de módulos y estado
- **[DASHBOARD_MIGRACION.md](MatrixNext/DASHBOARD_MIGRACION.md)**: Métricas, progreso y timeline
- **[DIRECTRICES_MIGRACION.md](MatrixNext/DIRECTRICES_MIGRACION.md)**: Estándares y reglas de migración

### Compilación

```bash
cd MatrixNext
dotnet build MatrixNext.sln
```

**Estado**: ✅ Compila sin errores (solo warnings de nullable)
