# Mapa de Módulos para Migración WebMatrix → MatrixNext

## Módulos Identificados y Clasificados por Prioridad

### 🔴 CRÍTICA (Implementar primero)

#### 1. **US_Usuarios** (14 páginas)
- **Carpeta**: `WebMatrix/US_Usuarios/`
- **Contexto**: `US_Model` (CoreProject)
- **Páginas**:
  - Usuarios.aspx
  - CambioContrasena.aspx
  - Roles.aspx (x2)
  - Permisos.aspx
  - GrupoUnidad.aspx
  - GruposPermisos.aspx
  - RolesPermisos.aspx
  - RolesUsuarios.aspx
  - TipoGrupoUnidad.aspx
  - UsuariosUnidades.aspx
  - Feedback.aspx
  - SeguimientoFeedback.aspx
- **Dependencias**: Bajo (infraestructura solo)
- **Status**: ⏳ SIGUIENTE

#### 2. **Home** (3 páginas)
- **Carpeta**: `WebMatrix/Home/`
- **Contexto**: `CORE_Model` + múltiples
- **Páginas**:
  - Home.aspx (dashboard principal)
  - Default.aspx (ALT: puede estar en raíz)
  - DefaultOLD.aspx
- **Dependencias**: ALTA (consume datos de múltiples módulos)
- **Status**: 🔄 DESPUÉS de US_Usuarios

---

### 🟠 ALTA (Prioritario)

#### 3. **PY_Proyectos** (18 páginas)
- **Carpeta**: `WebMatrix/PY_Proyectos/`
- **Contexto**: `PY_Model` (CoreProject)
- **Dependencias**: Medias (referencia Usuarios, Metodologías)
- **Volumen**: Grande pero bien estructurado

#### 4. **OP_Cuantitativo** (múltiples)
- **Carpeta**: `WebMatrix/OP_Cuantitativo/`
- **Contexto**: `OP_Cuanti_Model` (CoreProject)
- **Dependencias**: Altas (métodos, cálculos, variables)

#### 5. **OP_Cualitativo** (múltiples)
- **Carpeta**: `WebMatrix/OP_Cualitativo/`
- **Contexto**: `OP_Entities` (CoreProject)
- **Dependencias**: Altas (entrevistas, moderadores, sesiones)

#### 6. **FI_AdministrativoFinanciero** (21 páginas)
- **Carpeta**: `WebMatrix/FI_AdministrativoFinanciero/`
- **Contexto**: `FI_Model` (CoreProject)
- **Dependencias**: Altas (compras, órdenes, facturas)
- **Volumen**: Muy grande

---

### 🟡 MEDIA (Estratégica)

#### 7. **GD_Documentos**
- **Contexto**: `GD_Model`
- **Dependencias**: Medias

#### 8. **RP_Reportes**
- **Contexto**: `REP_Model`
- **Notas**: Consultas complejas → ideal para Dapper

#### 9. **TH_TalentoHumano**
- **Contexto**: `TH_Model`
- **Dependencias**: Medias (empleados, contratistas)

#### 10. **CU_Cuentas** (Clientes)
- **Contexto**: `CU_Model`
- **Dependencias**: Medias

#### 11. **CC_FinzOpe** (Financiera - Operacional)
- **Contexto**: `CC_FinzOpe`
- **Volumen**: Grande

---

### 🟢 BAJA (Complementaria)

12. **OP_RO** (Revisión Operacional)
13. **OP_Trafico**
14. **PY_ControlCalidad**
15. **PY_Adquisiciones**
16. **PNC** (Producto No Conforme)
17. **SG_Actas** (Seguimiento - Actas)
18. **ES_Estadistica**
19. **Centro_Informacion**
20. **Inventario**
21. **IT**
22. **MBO** (Objetivos)
23. **ResumenProduccion**
24. **RE_GT**
25. **PC_PropiedadCliente**
26. Otros (Account, Controls, etc.)

---

## Patrón de Migración por Módulo

```
Módulo WebMatrix (ej: US_Usuarios/)
│
├── 14 páginas .aspx.vb
│
└── MatrixNext → Controllers + Views + Services
    │
    ├── Controllers/
    │   ├── UsuariosController.cs
    │   ├── RolesController.cs
    │   ├── PermisosController.cs
    │   └── GrupoUnidadController.cs
    │
    ├── Views/
    │   ├── Usuarios/
    │   │   ├── Index.cshtml
    │   │   ├── Create.cshtml
    │   │   ├── Edit.cshtml
    │   │   └── Delete.cshtml
    │   ├── Roles/
    │   │   └── [idem estructura]
    │   ├── Permisos/
    │   │   └── [idem estructura]
    │   └── GrupoUnidad/
    │       └── [idem estructura]
    │
    └── Data/Services/
        ├── UsuarioService.cs
        ├── RolService.cs
        ├── PermisosService.cs
        └── GrupoUnidadService.cs
```

---

## Cronología Recomendada

| Fase | Semana | Módulo | Entregables |
|------|--------|--------|-------------|
| 0 | 1 | Login (✅ HECHO) | LoginController, autenticación |
| 1 | 2-3 | US_Usuarios | 14 páginas migradas, adaptador CoreProject |
| 2 | 4 | Home | Dashboard funcional |
| 3 | 5-7 | PY_Proyectos | 18 páginas, gestión completa |
| 4 | 8-9 | OP_Cuantitativo | Operaciones cuantitativos |
| 5 | 10-11 | OP_Cualitativo | Operaciones cualitativos |
| 6 | 12-15 | FI_Administrativo | 21 páginas finanzas |
| 7+ | 16+ | Módulos restantes | Por prioridad operativa |

---

## Decisiones Clave

✅ **SIN coexistencia**: Eliminar WebMatrix completamente al terminar cada módulo  
✅ **Adaptar CoreProject**: Crear wrappers que encapsulen contextos EF6  
✅ **Testing exhaustivo**: Validar funcionalidad antes de eliminar legacy  
✅ **Migraciones EF Core**: Solo para nuevas features, no for legacy  
✅ **Dapper para consultas**: Mantener para SP complejas  

---

## Próximo Paso Concreto

**Crear estructura base para US_Usuarios:**

```bash
# En MatrixNext.Web
mkdir Controllers/US
mkdir Views/US

# En MatrixNext.Data
mkdir Models/US
mkdir Services/US
mkdir Adapters
```

**Luego**: Migrar primera página (Usuarios.aspx) como demo del patrón
