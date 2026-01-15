# MAPEO SP - DISTRIBUCIÓN ENTREVISTAS, VARIABLES CONTROL E INHOME VISITS

**Módulo**: PY_Proyectos  
**Sprint**: 12.2.1 - 12.2.3  
**Responsable**: GitHub Copilot (MatrixNext Migration)  
**Fecha**: 2025-01-XX  

---

## 1. DISTRIBUCIÓN DE ENTREVISTAS (Sprint 12.2.1)

### 1.1 Stored Procedures

| SP | Propósito | Parámetros | OUTPUT | Observaciones |
|----|-----------|------------|--------|---------------|
| `PY_DistribucionEntrevistas_Get` | Obtener distribuciones por trabajo | `@IdTrabajo BIGINT` | Recordset | JOIN con PY_Trabajos, PY_Metodologias, OP_Unidades |
| `PY_DistribucionEntrevistas_Save` | Registrar distribución | `@IdTrabajo`, `@IdMetodologia`, `@IdUnidad`, `@Ciudad`, `@CantidadAsignada`, `@AsignadoPor` | `@IdDistribucion BIGINT` | INSERT en PY_DistribucionEntrevistas |
| `PY_CuotasDistribucion_Get` | Obtener cuotas por distribución | `@IdDistribucion BIGINT` | Recordset | Cuotas con indicador `CumpleCuota` |

### 1.2 Tablas

**PY_DistribucionEntrevistas**
```sql
CREATE TABLE PY_DistribucionEntrevistas (
    IdDistribucion BIGINT PRIMARY KEY IDENTITY,
    IdTrabajo BIGINT NOT NULL,
    IdMetodologia INT NOT NULL,
    IdUnidad INT NOT NULL,
    Ciudad NVARCHAR(100),
    CantidadAsignada INT NOT NULL,
    CantidadCompletada INT,
    FechaAsignacion DATETIME NOT NULL DEFAULT GETDATE(),
    AsignadoPor BIGINT NOT NULL,
    CONSTRAINT FK_Distribucion_Trabajo FOREIGN KEY (IdTrabajo) REFERENCES PY_Trabajos(IdTrabajo),
    CONSTRAINT FK_Distribucion_Metodologia FOREIGN KEY (IdMetodologia) REFERENCES PY_Metodologias(IdMetodologia),
    CONSTRAINT FK_Distribucion_Unidad FOREIGN KEY (IdUnidad) REFERENCES OP_Unidades(IdUnidad)
);
```

**PY_CuotasDistribucion**
```sql
CREATE TABLE PY_CuotasDistribucion (
    IdCuota BIGINT PRIMARY KEY IDENTITY,
    IdDistribucion BIGINT NOT NULL,
    VariableCuota NVARCHAR(100) NOT NULL, -- Edad, Género, NSE, etc.
    ValorCuota NVARCHAR(100) NOT NULL,
    CantidadRequerida INT NOT NULL,
    CantidadObtenida INT DEFAULT 0,
    CONSTRAINT FK_Cuota_Distribucion FOREIGN KEY (IdDistribucion) REFERENCES PY_DistribucionEntrevistas(IdDistribucion)
);
```

### 1.3 Validaciones

| Validación | Regla | Mensaje Error |
|------------|-------|---------------|
| Suma Total | `SUM(CantidadAsignada) == PY_Trabajos.TotalMuestra` | "La suma de la distribución ({suma}) no coincide con la muestra total ({total})" |
| Cantidades | `CantidadAsignada > 0` | "Todas las cantidades deben ser mayores a cero" |
| Metodología | `IdMetodologia IN (Presencial, Telefónica, Online)` | "Metodología no válida" |
| Ciudad RMC | `Ciudad IS NOT NULL` cuando `IdUnidad IN (119, 120)` | "Ciudad es obligatoria para RMC" |

### 1.4 Flujo de Negocio

```
1. Obtener muestra total del trabajo (PY_Trabajos.TotalMuestra)
2. Usuario distribuye por unidades (puede ser múltiples)
3. Validar que SUM(cantidades) == TotalMuestra
4. Registrar cada distribución en PY_DistribucionEntrevistas
5. Calcular cuotas automáticamente (basado en variables de control)
6. Actualizar CantidadCompletada cuando se registren encuestas
7. Calcular % Avance: (CantidadCompletada / CantidadAsignada) * 100
```

---

## 2. VARIABLES DE CONTROL (Sprint 12.2.2)

### 2.1 Stored Procedures

| SP | Propósito | Parámetros | OUTPUT | Observaciones |
|----|-----------|------------|--------|---------------|
| `PY_VariablesControl_Get` | Obtener variables por trabajo | `@IdTrabajo BIGINT` | Recordset | Variables con validaciones |
| `PY_VariablesControl_Add` | Crear variable de control | `@IdTrabajo`, `@NombreVariable`, `@TipoDato`, `@ValorMinimo`, `@ValorMaximo`, `@ValoresPermitidos`, `@Obligatorio`, `@Descripcion`, `@RegistradoPor` | `@IdVariable BIGINT` | INSERT en PY_VariablesControl |
| `PY_VariablesControl_Update` | Actualizar variable | `@IdVariable`, `@NombreVariable`, ..., `@ModificadoPor` | N/A | UPDATE PY_VariablesControl |
| `PY_VariablesControl_Delete` | Eliminar variable | `@IdVariable BIGINT` | N/A | DELETE PY_VariablesControl |

### 2.2 Tablas

**PY_VariablesControl**
```sql
CREATE TABLE PY_VariablesControl (
    IdVariable BIGINT PRIMARY KEY IDENTITY,
    IdTrabajo BIGINT NOT NULL,
    NombreVariable NVARCHAR(100) NOT NULL,
    TipoDato NVARCHAR(20) NOT NULL, -- Numérico, Texto, Rango, Lista
    ValorMinimo DECIMAL(18,2),
    ValorMaximo DECIMAL(18,2),
    ValoresPermitidos NVARCHAR(MAX), -- JSON array para listas
    Obligatorio BIT NOT NULL DEFAULT 0,
    Descripcion NVARCHAR(500),
    FechaRegistro DATETIME NOT NULL DEFAULT GETDATE(),
    RegistradoPor BIGINT NOT NULL,
    CONSTRAINT FK_VariableControl_Trabajo FOREIGN KEY (IdTrabajo) REFERENCES PY_Trabajos(IdTrabajo),
    CONSTRAINT CHK_TipoDato CHECK (TipoDato IN ('Numérico', 'Texto', 'Rango', 'Lista'))
);
```

### 2.3 Validaciones

| Validación | Regla | Mensaje Error |
|------------|-------|---------------|
| Nombre | `NombreVariable IS NOT NULL AND LEN(NombreVariable) > 0` | "El nombre de la variable es obligatorio" |
| Tipo Dato | `TipoDato IN ('Numérico', 'Texto', 'Rango', 'Lista')` | "Tipo de dato no válido" |
| Rango | `ValorMinimo <= ValorMaximo` | "El valor mínimo no puede ser mayor al valor máximo" |
| Lista | `TipoDato = 'Lista' THEN ValoresPermitidos IS NOT NULL` | "Debe especificar valores permitidos para tipo Lista" |

---

## 3. INHOME VISIT (Sprint 12.2.3)

### 3.1 Stored Procedures

| SP | Propósito | Parámetros | OUTPUT | Observaciones |
|----|-----------|------------|--------|---------------|
| `PY_InHomeVisit_Get` | Obtener visitas por trabajo | `@IdTrabajo BIGINT` | Recordset | JOIN con TH_Empleado para responsable |
| `PY_InHomeVisit_Save` | Crear/actualizar visita | `@IdTrabajo`, `@LugarVisita`, `@FechaProgramada`, `@CantidadParticipantes`, `@Recursos`, `@Observaciones`, `@ResponsableId`, `@RegistradoPor` | `@IdVisita BIGINT` | INSERT en PY_InHomeVisit, Estado default = 'Programada' |
| `PY_InHomeVisit_UpdateEstado` | Cambiar estado visita | `@IdVisita`, `@NuevoEstado` | N/A | UPDATE Estado, si 'Realizada' → FechaRealizada = GETDATE() |

### 3.2 Tablas

**PY_InHomeVisit**
```sql
CREATE TABLE PY_InHomeVisit (
    IdVisita BIGINT PRIMARY KEY IDENTITY,
    IdTrabajo BIGINT NOT NULL,
    LugarVisita NVARCHAR(200) NOT NULL,
    FechaProgramada DATETIME NOT NULL,
    FechaRealizada DATETIME,
    Estado NVARCHAR(20) NOT NULL DEFAULT 'Programada', -- Programada, Realizada, Cancelada, Reprogramada
    CantidadParticipantes INT NOT NULL,
    Recursos NVARCHAR(500), -- Equipo necesario (grabadora, video, etc.)
    Observaciones NVARCHAR(MAX),
    ResponsableId BIGINT,
    FechaRegistro DATETIME NOT NULL DEFAULT GETDATE(),
    RegistradoPor BIGINT NOT NULL,
    CONSTRAINT FK_InHomeVisit_Trabajo FOREIGN KEY (IdTrabajo) REFERENCES PY_Trabajos(IdTrabajo),
    CONSTRAINT FK_InHomeVisit_Responsable FOREIGN KEY (ResponsableId) REFERENCES TH_Empleado(IdEmpleado),
    CONSTRAINT CHK_Estado CHECK (Estado IN ('Programada', 'Realizada', 'Cancelada', 'Reprogramada'))
);
```

### 3.3 Validaciones

| Validación | Regla | Mensaje Error |
|------------|-------|---------------|
| Lugar | `LugarVisita IS NOT NULL AND LEN(LugarVisita) > 0` | "El lugar de la visita es obligatorio" |
| Fecha Futura | `FechaProgramada >= CAST(GETDATE() AS DATE)` | "La fecha programada debe ser futura" |
| Participantes | `CantidadParticipantes > 0` | "La cantidad de participantes debe ser mayor a cero" |
| Estado | `Estado IN ('Programada', 'Realizada', 'Cancelada', 'Reprogramada')` | "Estado no válido" |
| Realizada | `Estado = 'Realizada' THEN FechaRealizada IS NOT NULL` | "Debe registrar fecha de realización" |

### 3.4 Flujo de Negocio

```
CREAR VISITA:
1. Usuario registra lugar, fecha programada, cantidad participantes, recursos necesarios
2. Asignar responsable (opcional)
3. Estado inicial: 'Programada'
4. Enviar notificación al responsable (email/alerta)

EJECUTAR VISITA:
1. Cambiar estado a 'Realizada'
2. Registrar FechaRealizada = GETDATE()
3. Capturar observaciones de la visita
4. Actualizar dashboard de visitas

CANCELAR/REPROGRAMAR:
1. Cambiar estado a 'Cancelada' o 'Reprogramada'
2. Si reprogramada: registrar nueva visita con nueva fecha
3. Vincular visitas reprogramadas (IdVisitaOriginal)
```

---

## 4. DEPENDENCIAS

### Tablas Referenciadas
- `PY_Trabajos` (IdTrabajo, NumeroTrabajo, TotalMuestra)
- `PY_Metodologias` (IdMetodologia, Nombre)
- `OP_Unidades` (IdUnidad, Nombre)
- `TH_Empleado` (IdEmpleado, Nombres, Apellidos)

### Permisos Requeridos
- **Distribución**: Permiso 100 (PMO) o 135 (Coordinador)
- **Variables Control**: Permiso 100 (PMO)
- **InHome Visit**: Permiso 100 (PMO) o 135 (Coordinador)

---

## 5. AUDITORÍA

Todas las operaciones registran:
- `FechaRegistro`: Timestamp de creación
- `RegistradoPor`: Usuario que registra
- `FechaModificacion`: Timestamp de última actualización (si aplica)
- `ModificadoPor`: Usuario que modifica (si aplica)

---

## 6. REGLAS DE NEGOCIO CLAVE

1. **Distribución**:
   - Suma total DEBE coincidir con muestra del trabajo
   - Una distribución puede tener múltiples cuotas
   - Cuotas se calculan automáticamente según variables de control

2. **Variables Control**:
   - Variables numéricas/rango deben tener ValorMinimo <= ValorMaximo
   - Variables tipo Lista requieren ValoresPermitidos (JSON array)
   - Variables obligatorias bloquean guardado si no cumplen validación

3. **InHome Visit**:
   - Solo visitas 'Programada' pueden cambiar a 'Realizada'
   - Al cambiar a 'Realizada', FechaRealizada = GETDATE()
   - Visitas 'Cancelada' no pueden cambiar a otro estado
   - Visitas 'Reprogramada' generan nueva visita vinculada

---

**Estado**: ✅ Implementación completa Sprints 12.2.1-12.2.3  
**Próximos pasos**: Sprint 12.2.4 - Mapeo y Documentación SP PY (10h)
