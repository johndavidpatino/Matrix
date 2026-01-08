# MANUAL DE USUARIO - Módulo OP_Cuantitativo

**Versión**: 1.0  
**Fecha**: 2026-01-08  
**Módulo**: OP_Cuantitativo (Operaciones Cuantitativas)  
**Plataforma**: MatrixNext (ASP.NET Core 8.0)

---

## 📋 TABLA DE CONTENIDOS

1. [Introducción](#introducción)
2. [Acceso y Permisos](#acceso-y-permisos)
3. [Navegación Principal](#navegación-principal)
4. [Flujos por Rol](#flujos-por-rol)
5. [Guía de Pantallas](#guía-de-pantallas)
6. [Tareas Comunes](#tareas-comunes)
7. [Notificaciones y Alertas](#notificaciones-y-alertas)
8. [Resolución de Problemas](#resolución-de-problemas)
9. [Preguntas Frecuentes](#preguntas-frecuentes)
10. [Glosario](#glosario)

---

## 📖 INTRODUCCIÓN

### ¿Qué es OP_Cuantitativo?

El módulo **OP_Cuantitativo** gestiona las operaciones de campo para estudios cuantitativos (encuestas, sondeos). Permite:

- ✅ Configurar trabajos y marcos muestrales (COE)
- ✅ Asignar personal de campo a coordinadores
- ✅ Registrar producción diaria
- ✅ Revisar productividad (flujo multirrol: PMO → Coordinador → Campo → MyS/Call)
- ✅ Controlar exportaciones IPS
- ✅ Gestionar documentos asociados
- ✅ Programar festivos nacionales/locales

### Usuarios del Módulo

| Rol | Responsabilidades |
|-----|-------------------|
| **PMO** | Revisión inicial de productividad, aprobación primer nivel |
| **Coordinador** | Asignación de personal, revisión de productividad, coordinación de campo |
| **Campo (Encuestador)** | Registro de producción diaria, consulta de asignaciones |
| **MyS/Call Center** | Revisión final de productividad, control de calidad |
| **Administrador** | Configuración general, gestión de catálogos |

---

## 🔐 ACCESO Y PERMISOS

### Inicio de Sesión

1. Acceder a **MatrixNext**: `https://[servidor]/`
2. Ingresar credenciales (usuario y contraseña)
3. El sistema valida contra Active Directory/Base de datos
4. Redirige automáticamente al módulo según permisos

### Permisos por Pantalla

| Pantalla | PMO | Coordinador | Campo | MyS/Call | Admin |
|----------|-----|-------------|-------|----------|-------|
| Trabajos | ✅ | ✅ | 👁️ | ✅ | ✅ |
| Ficha Cuantitativa | ✅ | ✅ | ❌ | ❌ | ✅ |
| Muestra | ✅ | ✅ | ❌ | ❌ | ✅ |
| Asignación | ❌ | ✅ | 👁️ | ❌ | ✅ |
| Registro Producción | ❌ | ✅ | ✅ | ❌ | ✅ |
| Revisión Productividad | ✅ | ✅ | 👁️ | ✅ | ✅ |
| IPS Exportes | ❌ | ✅ | ❌ | ✅ | ✅ |
| Gestión Documental | ✅ | ✅ | 👁️ | ✅ | ✅ |

**Leyenda**: ✅ Crear/Editar | 👁️ Solo Lectura | ❌ Sin Acceso

---

## 🧭 NAVEGACIÓN PRINCIPAL

### Menú Principal OP_Cuantitativo

```
📊 OP_Cuantitativo
   ├─ 🏢 Trabajos                    → Lista de trabajos cuantitativos
   ├─ 👤 Mis Trabajos (Coordinador)  → Trabajos asignados a mí
   ├─ 📞 Call Center                 → Vista para revisores MyS/Call
   ├─ 🔍 Consulta Trabajos           → Búsqueda avanzada
   │
   ├─ 📋 Gestión COE
   │  ├─ Ficha Cuantitativa          → Configurar marco muestral
   │  ├─ Muestra                     → Asignar cuotas a coordinadores
   │  ├─ Estimación                  → Calcular recursos necesarios
   │  ├─ Asignación                  → Asignar encuestadores
   │  └─ Coordinación                → Panel de control coordinador
   │
   ├─ 📊 Campo
   │  ├─ Registro de Producción      → Entrada diaria de datos
   │  └─ Dashboard Tráfico           → Métricas en tiempo real
   │
   ├─ ✅ Revisión
   │  └─ Revisión de Productividad   → Flujo multirrol aprobación
   │
   └─ ⚙️ Administración
      ├─ IPS Exportes                → Control de exportaciones
      ├─ Gestión Documental          → Documentos asociados
      └─ Festivos                    → Calendario festivos
```

### Breadcrumbs (Migas de Pan)

Todas las pantallas muestran ruta de navegación:

```
Inicio > OP_Cuantitativo > Trabajos > Detalle (JobBook: 12345)
```

---

## 👥 FLUJOS POR ROL

### 🔵 ROL: PMO

#### Flujo Principal

1. **Revisar Trabajos** (`/OP/Trabajos`)
   - Ver lista completa de trabajos cuantitativos
   - Filtrar por estado, coordinador, metodología

2. **Revisión de Productividad - Nivel 1** (`/OP/RevisionProductividad`)
   - Revisar registros pendientes de aprobación
   - Aprobar o rechazar producción diaria
   - Agregar observaciones si es necesario
   - Sistema notifica automáticamente a Coordinador

3. **Consultar Documentos** (`/OP/GestionDocumental`)
   - Ver documentos cargados por coordinadores
   - Descargar anexos, planillas, evidencias

#### Pantalla Típica: Revisión de Productividad

```
┌─────────────────────────────────────────────────────────┐
│ Revisión de Productividad - PMO                         │
├─────────────────────────────────────────────────────────┤
│ Filtros: [Trabajo ▼] [Coordinador ▼] [Fecha Desde/Hasta]│
│                                                          │
│ ┌──────────────────────────────────────────────────┐   │
│ │ Fecha      Coordinador  Encuestador  Actividad   │   │
│ │ 2026-01-08 Juan Pérez   Ana García   Encuestas   │   │
│ │ Cantidad: 25 | Observaciones: Ninguna            │   │
│ │ [✓ Aprobar] [✗ Rechazar] [💬 Comentar]          │   │
│ └──────────────────────────────────────────────────┘   │
│                                                          │
│ Paginación: [← Anterior] [1] [2] [3] [Siguiente →]     │
└─────────────────────────────────────────────────────────┘
```

---

### 🟢 ROL: Coordinador

#### Flujo Principal

1. **Acceder a Mis Trabajos** (`/OP/TrabajosCoordinador`)
   - Ver trabajos asignados a mí como coordinador
   - Estado de cada trabajo (Activo, En Campo, Cerrado)

2. **Configurar COE** (para trabajos nuevos)
   - **Ficha Cuantitativa** (`/OP/FichaCuantitativa`): Definir marco muestral (universo, metodología, cuotas)
   - **Muestra** (`/OP/Muestra`): Asignar cuotas específicas a mi coordinación
   - **Estimación** (`/OP/Estimacion`): Calcular recursos (encuestadores, días, costos)
   - **Asignación** (`/OP/Asignacion`): Asignar encuestadores a metas específicas

3. **Monitorear Producción** (`/OP/Coordinacion`)
   - Panel de control con métricas en tiempo real
   - Ver avance diario, pendientes, cumplimiento de metas

4. **Registrar Producción** (opcional, puede hacerlo el campo)
   - `/OP/RegistroProduccion`: Ingresar datos en nombre del equipo

5. **Revisar Productividad - Nivel 2** (`/OP/RevisionProductividad`)
   - Aprobar/Rechazar producción después de PMO
   - Agregar comentarios para MyS/Call
   - Sistema notifica a siguiente nivel

6. **Exportar IPS** (`/OP/IPSExportes`)
   - Programar exportaciones automáticas
   - Descargar archivos Excel/CSV
   - Ver historial de exportaciones

7. **Gestionar Documentos** (`/OP/GestionDocumental`)
   - Subir planillas, evidencias, reportes
   - Descargar documentos de auditoría

#### Pantalla Típica: Coordinación (Panel de Control)

```
┌─────────────────────────────────────────────────────────┐
│ Panel de Coordinación - Trabajo: 12345                  │
├─────────────────────────────────────────────────────────┤
│ Avance Global: ████████░░ 78% (1,250/1,600 encuestas)  │
│                                                          │
│ Métricas Hoy (2026-01-08):                             │
│ ┌──────────────┬──────────────┬──────────────┐         │
│ │ Encuestadores│ Producción   │ Pendientes   │         │
│ │     12       │    156       │     45       │         │
│ └──────────────┴──────────────┴──────────────┘         │
│                                                          │
│ Encuestadores Activos:                                  │
│ ┌────────────────────────────────────────┐             │
│ │ Nombre        Meta  Logrado  Pendiente │             │
│ │ Ana García     120    95       25      │ [📊 Ver]  │
│ │ Carlos Ruiz    100    78       22      │ [📊 Ver]  │
│ │ María López     80    83        0      │ [✅ OK]   │
│ └────────────────────────────────────────┘             │
│                                                          │
│ [📥 Exportar Excel] [📧 Notificar Equipo]              │
└─────────────────────────────────────────────────────────┘
```

---

### 🟡 ROL: Campo (Encuestador)

#### Flujo Principal

1. **Ver Mis Asignaciones** (`/OP/TrabajosCoordinador` con filtro)
   - Ver trabajos donde estoy asignado como encuestador
   - Revisar metas y actividades pendientes

2. **Registrar Producción Diaria** (`/OP/RegistroProduccion`)
   - Seleccionar trabajo y fecha
   - Seleccionar unidad, actividad, subactividad (desde catálogos cacheados)
   - Ingresar cantidad realizada
   - Agregar observaciones opcionales
   - Guardar (sistema valida y notifica a PMO)

3. **Consultar Estado** (`/OP/RevisionProductividad` - solo lectura)
   - Ver historial de registros propios
   - Ver estado de aprobación (Pendiente/Aprobado/Rechazado)
   - Leer observaciones de revisores

#### Pantalla Típica: Registro de Producción

```
┌─────────────────────────────────────────────────────────┐
│ Registro de Producción - Ana García                     │
├─────────────────────────────────────────────────────────┤
│ Trabajo: [12345 - Encuesta Satisfacción Cliente ▼]     │
│ Fecha: [2026-01-08 📅]                                  │
│                                                          │
│ ┌────────────────────────────────────────┐             │
│ │ Unidad:        [Bogotá ▼]              │             │
│ │ Actividad:     [Encuestas Telefónicas ▼]│            │
│ │ Subactividad:  [Llamadas Completadas ▼] │            │
│ │ Cantidad:      [__25__]                │             │
│ │ Observaciones: [Zona norte, 2-4pm]     │             │
│ └────────────────────────────────────────┘             │
│                                                          │
│ [💾 Guardar Registro] [🔄 Limpiar]                     │
│                                                          │
│ Registros Hoy:                                          │
│ ┌────────────────────────────────────────┐             │
│ │ Hora  Actividad           Cantidad     │             │
│ │ 09:15 Llamadas Completadas  25         │ ✅ Guardado│
│ │ 14:30 Llamadas Completadas  18         │ ⏳ Pendiente│
│ └────────────────────────────────────────┘             │
└─────────────────────────────────────────────────────────┘
```

---

### 🟣 ROL: MyS/Call Center

#### Flujo Principal

1. **Acceder Vista Call Center** (`/OP/CallCenter`)
   - Dashboard específico para control de calidad
   - Ver trabajos en monitoreo

2. **Revisión de Productividad - Nivel Final** (`/OP/RevisionProductividad`)
   - Revisar registros aprobados por PMO y Coordinador
   - Validar calidad de datos
   - Aprobación final o rechazo con observaciones
   - Sistema registra en auditoría

3. **Controlar IPS Exportes** (`/OP/IPSExportes`)
   - Ver exportaciones programadas
   - Descargar archivos para análisis
   - Validar integridad de datos

#### Pantalla Típica: Vista Call Center

```
┌─────────────────────────────────────────────────────────┐
│ Control de Calidad - Call Center                        │
├─────────────────────────────────────────────────────────┤
│ Trabajos en Monitoreo: 8 activos                        │
│                                                          │
│ ┌────────────────────────────────────────────────────┐ │
│ │ JobBook  Coordinador  Avance  Pendiente Revisión   │ │
│ │ 12345    Juan Pérez   78%     45 registros         │ │
│ │ [🔍 Revisar] [📊 Métricas] [📥 Exportar]          │ │
│ └────────────────────────────────────────────────────┘ │
│                                                          │
│ Alertas:                                                │
│ ⚠️ Trabajo 12346: 3 registros rechazados hoy           │
│ ✅ Trabajo 12347: Producción al 100%                   │
└─────────────────────────────────────────────────────────┘
```

---

## 📺 GUÍA DE PANTALLAS

### 1. Trabajos (`/OP/Trabajos`)

**Propósito**: Lista maestra de todos los trabajos cuantitativos

**Funcionalidad**:
- Tabla paginada con búsqueda y filtros
- Columnas: JobBook, Nombre, Coordinador, Estado, Metodología, Fecha Inicio/Fin
- Acciones: Ver Detalle, Editar (si admin), Eliminar (si sin producción)

**Acciones Rápidas**:
- 🔍 Búsqueda por JobBook o Nombre
- 📅 Filtro por rango de fechas
- 👤 Filtro por Coordinador
- 📊 Filtro por Estado (Activo, En Campo, Cerrado, Cancelado)

**Navegación**:
- Clic en fila → Detalle del trabajo
- Botón "Crear Trabajo" (solo admin/PMO)

---

### 2. Ficha Cuantitativa (`/OP/FichaCuantitativa`)

**Propósito**: Configurar marco muestral y características del estudio

**Campos Principales**:
- **Universo**: Descripción de la población objetivo
- **Metodología**: Telefónica, Presencial, Online, Mixta
- **Tamaño Muestral**: N total requerido
- **Margen de Error**: % (calculado automáticamente)
- **Nivel de Confianza**: 90%, 95%, 99%
- **Tipo de Muestreo**: Aleatorio Simple, Estratificado, Conglomerados
- **Cuotas**: Tabla con segmentaciones (edad, género, NSE, ubicación)

**Validaciones**:
- Tamaño muestral > 0
- Margen de error entre 1% y 10%
- Suma de cuotas = 100%

**Flujo**:
1. Seleccionar trabajo
2. Llenar campos descriptivos
3. Definir tabla de cuotas (agregar/editar/eliminar filas)
4. Guardar → Sistema valida y notifica a coordinador

---

### 3. Muestra (`/OP/Muestra`)

**Propósito**: Asignar cuotas específicas a coordinadores

**Funcionalidad**:
- Seleccionar trabajo
- Ver cuotas definidas en Ficha Cuantitativa
- Asignar cantidades a cada coordinador
- Sistema valida que suma = total requerido

**Ejemplo Visual**:

```
Trabajo: 12345
Total Requerido: 1,600 encuestas

┌─────────────────────────────────────────────┐
│ Coordinador     Zona      Cuota Asignada   │
│ Juan Pérez      Bogotá    600              │
│ María González  Medellín  500              │
│ Carlos Ruiz     Cali      500              │
├─────────────────────────────────────────────┤
│ TOTAL:                    1,600  ✅        │
└─────────────────────────────────────────────┘
```

---

### 4. Estimación (`/OP/Estimacion`)

**Propósito**: Calcular recursos necesarios (tiempo, personal, costos)

**Inputs**:
- Cuota asignada
- Rendimiento promedio (encuestas/día/persona)
- Días disponibles
- Costo por encuesta

**Outputs**:
- Encuestadores necesarios (calculado)
- Días de campo (calculado)
- Costo total estimado (calculado)

**Fórmulas**:
```
Encuestadores = TECHO(Cuota / (Rendimiento * Días))
Costo Total = Cuota * Costo Unitario
```

---

### 5. Asignación (`/OP/Asignacion`)

**Propósito**: Asignar encuestadores específicos a metas

**Flujo**:
1. Seleccionar trabajo y coordinador (si admin)
2. Ver lista de encuestadores disponibles
3. Asignar meta a cada uno
4. Especificar actividad principal
5. Guardar → Sistema notifica por email a encuestadores

**Validaciones**:
- Meta > 0
- Suma de metas ≤ cuota asignada al coordinador
- Encuestador no duplicado

---

### 6. Registro de Producción (`/OP/RegistroProduccion`)

**Propósito**: Entrada diaria de datos de campo

**Performance Optimization**:
- ✅ **Catálogos cacheados**: Unidades, Actividades, Subactividades se cargan desde IMemoryCache (15 min TTL)
- ⚡ **Mejora**: De ~50ms (DB query) a <5ms (cache hit)
- 📊 **Impacto**: 80%+ reducción en queries repetitivos

**Campos**:
- Trabajo (dropdown)
- Fecha (datepicker, default: hoy)
- Unidad (dropdown cacheado)
- Actividad (dropdown cacheado, filtrado por unidad)
- Subactividad (dropdown cacheado, filtrado por actividad)
- Cantidad (número)
- Observaciones (textarea, opcional)

**Validaciones**:
- Cantidad > 0
- Fecha ≤ hoy
- Encuestador asignado al trabajo
- No exceder meta asignada (warning, no bloquea)

**Flujo Posterior**:
- Sistema envía notificación a PMO para revisión
- Email automático con resumen diario al coordinador

---

### 7. Revisión de Productividad (`/OP/RevisionProductividad`)

**Propósito**: Flujo multirrol de aprobación

**Niveles de Revisión**:

#### Nivel 1: PMO
- Ve: Registros con estado "Pendiente Revisión PMO"
- Acciones: Aprobar → Estado "Pendiente Revisión Coordinador" / Rechazar → Estado "Rechazado PMO"

#### Nivel 2: Coordinador
- Ve: Registros con estado "Pendiente Revisión Coordinador"
- Acciones: Aprobar → Estado "Pendiente Revisión MyS" / Rechazar → Estado "Rechazado Coordinador"

#### Nivel 3: MyS/Call Center
- Ve: Registros con estado "Pendiente Revisión MyS"
- Acciones: Aprobar → Estado "Aprobado Final" / Rechazar → Estado "Rechazado MyS"

**Notificaciones Automáticas**:
- Cada aprobación/rechazo envía email al siguiente nivel
- Rechazos notifican a encuestador y coordinador

**Columnas Tabla**:
- Fecha, Encuestador, Trabajo, Actividad, Cantidad, Estado, Observaciones
- Acciones: [✓ Aprobar] [✗ Rechazar] [💬 Comentar]

---

### 8. IPS Exportes (`/OP/IPSExportes`)

**Propósito**: Controlar exportaciones y limpieza automática

**Funcionalidades**:

#### Programar Exportación
- Seleccionar trabajo
- Tipo de export: Excel (.xlsx), CSV
- Frecuencia: Manual, Diaria, Semanal
- Fecha programada (si automática)
- Retención: Días antes de eliminar archivo (default: 30)

#### Listar Exportaciones
- Tabla con: Trabajo, Tipo, Fecha Creación, Tamaño, Estado (Pendiente/Completo/Eliminado)
- Acciones: [📥 Descargar] [🗑️ Eliminar Manual]

#### Background Service Automático
- ✅ **OpExportCleanupBackgroundService** ejecuta cada hora
- Elimina archivos físicos con `FechaProgramadaLimpieza < DateTime.Now`
- Actualiza estado en tabla `OP_ExportesAuditoria`
- Log de cada eliminación

**Performance**:
- Archivos almacenados en: `/wwwroot/exports/OP/{JobBook}/`
- Cleanup evita acumulación infinita de archivos

---

### 9. Gestión Documental (`/OP/GestionDocumental`)

**Propósito**: Repositorio de documentos asociados a trabajos

**Funcionalidades**:
- **Upload**: Seleccionar trabajo → Subir archivo (PDF, Excel, Word, imágenes)
- **Lista**: Tabla con Nombre, Tipo, Tamaño, Fecha Subida, Usuario
- **Descargar**: Clic en nombre → descarga directa
- **Eliminar**: Solo usuario que subió o admin

**Validaciones Upload**:
- Tamaño máx: 10 MB (configurable en `appsettings.json`)
- Extensiones permitidas: .pdf, .xlsx, .docx, .jpg, .png, .zip

**Integración**:
- Usa componente compartido `UploadService`
- Path: `/uploads/OP/{JobBook}/`

---

### 10. Festivos (`/OP/Festivos`)

**Propósito**: Gestionar calendario de festivos para cálculo de jornadas

**Funcionalidad**:
- CRUD de festivos: Fecha, Nombre, Tipo (Nacional/Departamental/Municipal)
- Alcance: Aplica a todos los trabajos o específico por ubicación
- Importar festivos predefinidos (botón "Cargar Festivos Colombia 2026")

**Uso**:
- Sistema excluye festivos al calcular días hábiles en Estimación
- Integración con módulo `TH_Ausencia` para cálculo de jornadas laborales

---

## 🎯 TAREAS COMUNES

### Tarea 1: Crear un Nuevo Trabajo Cuantitativo

**Rol**: PMO o Administrador

**Pasos**:
1. Ir a `/OP/Trabajos`
2. Clic en **[+ Crear Trabajo]**
3. Llenar formulario:
   - JobBook (número único)
   - Nombre descriptivo
   - Cliente
   - Metodología
   - Fecha Inicio / Fecha Fin
   - Coordinador Principal (dropdown)
   - Estado: Activo
4. Guardar
5. Sistema crea trabajo y notifica a Coordinador por email

**Siguiente Paso**: Coordinador debe configurar Ficha Cuantitativa

---

### Tarea 2: Asignar Encuestadores a un Trabajo

**Rol**: Coordinador

**Pasos**:
1. Ir a `/OP/Asignacion`
2. Seleccionar trabajo
3. Clic en **[+ Agregar Encuestador]**
4. Buscar encuestador por nombre o ID
5. Asignar meta (ejemplo: 100 encuestas)
6. Seleccionar actividad principal
7. Repetir para todos los encuestadores
8. Guardar → Sistema valida suma ≤ cuota
9. Sistema envía email a cada encuestador con detalles

---

### Tarea 3: Registrar Producción Diaria (Encuestador)

**Rol**: Campo (Encuestador)

**Pasos**:
1. Ir a `/OP/RegistroProduccion`
2. Seleccionar trabajo asignado
3. Verificar fecha (default: hoy)
4. Seleccionar Unidad (ejemplo: Bogotá)
5. Seleccionar Actividad (ejemplo: Encuestas Telefónicas) **← Cargado desde cache**
6. Seleccionar Subactividad (ejemplo: Llamadas Completadas) **← Cargado desde cache**
7. Ingresar cantidad (ejemplo: 25)
8. Agregar observaciones si es necesario
9. Clic en **[💾 Guardar Registro]**
10. Sistema confirma: "Registro guardado. Notificación enviada a PMO."

**Resultado**:
- Registro queda en estado "Pendiente Revisión PMO"
- PMO recibe email automático

---

### Tarea 4: Aprobar Productividad (PMO)

**Rol**: PMO

**Pasos**:
1. Ir a `/OP/RevisionProductividad`
2. Filtrar por fecha (ejemplo: hoy)
3. Ver lista de registros "Pendiente Revisión PMO"
4. Revisar cada registro:
   - Validar cantidad vs meta
   - Leer observaciones
5. Para aprobar: Clic en **[✓ Aprobar]**
   - Sistema cambia estado a "Pendiente Revisión Coordinador"
   - Notifica a Coordinador por email
6. Para rechazar: Clic en **[✗ Rechazar]**
   - Agregar motivo en popup
   - Sistema cambia estado a "Rechazado PMO"
   - Notifica a Encuestador y Coordinador

---

### Tarea 5: Exportar Datos IPS

**Rol**: Coordinador o MyS/Call

**Pasos**:
1. Ir a `/OP/IPSExportes`
2. Clic en **[+ Nueva Exportación]**
3. Seleccionar:
   - Trabajo
   - Tipo: Excel
   - Frecuencia: Manual
4. Clic en **[Generar]**
5. Sistema procesa (puede tardar 10-30 seg)
6. Al completar, aparece en lista con botón **[📥 Descargar]**
7. Descargar archivo

**Archivo Generado**:
- Nombre: `IPS_Export_{JobBook}_{Fecha}.xlsx`
- Contenido: Todos los registros aprobados con columnas: Fecha, Encuestador, Unidad, Actividad, Cantidad, Estado

**Limpieza Automática**:
- Archivo se eliminará automáticamente después de 30 días (configurable)

---

## 🔔 NOTIFICACIONES Y ALERTAS

### Emails Automáticos

El módulo envía notificaciones por email en los siguientes eventos:

| Evento | Destinatario | Contenido |
|--------|--------------|-----------|
| Nuevo trabajo creado | Coordinador asignado | Detalles del trabajo, link a Ficha Cuantitativa |
| Encuestador asignado | Encuestador | Meta, actividad, fecha inicio, contacto coordinador |
| Producción registrada | PMO | Resumen diario: encuestador, trabajo, cantidad |
| Aprobación PMO | Coordinador | Registros aprobados, link a revisión |
| Aprobación Coordinador | MyS/Call | Registros aprobados, link a revisión |
| Rechazo en cualquier nivel | Encuestador + Coordinador | Motivo de rechazo, observaciones |
| Export IPS generado | Coordinador + MyS | Link de descarga, tamaño archivo |

### Cola de Emails Asíncronos

- ✅ **OpEmailQueueService** procesa emails en background
- ⚡ Operaciones de guardado no bloquean esperando envío
- 📊 Retry automático hasta 3 intentos si falla SMTP
- 🔍 Auditoría completa en tabla `OP_EmailQueue`

### Alertas en Pantalla

- 🔴 **Error**: Validaciones fallidas, operaciones no exitosas
- 🟡 **Warning**: Meta excedida (no bloquea), campos opcionales vacíos
- 🟢 **Success**: Operación completada correctamente

---

## 🛠️ RESOLUCIÓN DE PROBLEMAS

### Problema 1: No Puedo Ver Mis Trabajos

**Síntomas**: Al acceder a `/OP/TrabajosCoordinador`, lista vacía

**Causas Posibles**:
1. No estás asignado como Coordinador en ningún trabajo
2. Filtros activos ocultan trabajos
3. Sesión expirada

**Soluciones**:
1. Verificar con Admin que estás asignado como Coordinador
2. Limpiar filtros (botón "Limpiar Filtros")
3. Cerrar sesión y volver a ingresar

---

### Problema 2: Error al Registrar Producción

**Mensaje**: "Error al guardar registro de producción"

**Causas Posibles**:
1. No estás asignado al trabajo seleccionado
2. Fecha mayor a hoy
3. Cantidad = 0 o negativa
4. Catálogo no cargado (rare, cache miss)

**Soluciones**:
1. Verificar que el trabajo aparece en tus asignaciones
2. Revisar fecha (debe ser ≤ hoy)
3. Ingresar cantidad válida (> 0)
4. Refrescar página (F5) para recargar catálogos

---

### Problema 3: Email No Recibido

**Síntomas**: Esperaba notificación de aprobación/rechazo, no llegó

**Causas Posibles**:
1. Email en carpeta Spam/Correo no deseado
2. Cola de emails con alta carga (delay de hasta 5 min)
3. Dirección de email incorrecta en perfil de usuario

**Soluciones**:
1. Revisar carpeta Spam
2. Esperar 10 minutos y verificar nuevamente
3. Actualizar email en perfil de usuario (`/US/Usuarios/MiPerfil`)
4. Contactar a Admin si persiste

---

### Problema 4: Export IPS No Se Genera

**Síntomas**: Al solicitar export, queda en estado "Pendiente" indefinidamente

**Causas Posibles**:
1. No hay datos aprobados para exportar
2. Error de permisos en carpeta `/wwwroot/exports/`
3. Timeout en query (trabajo con >100K registros)

**Soluciones**:
1. Verificar que existen registros en estado "Aprobado Final"
2. Contactar a Admin/IT para verificar permisos de escritura
3. Para trabajos grandes, contactar a IT para export manual via SQL

---

### Problema 5: Pantalla Lenta (Catálogos)

**Síntomas**: Dropdowns de Actividades/Subactividades tardan >5 segundos en cargar

**Causa**: Cache expirado, reconstruyendo desde base de datos

**Solución**:
- ✅ **Automática**: Sistema recarga cache automáticamente cada 15 minutos
- ⚡ Primera carga después de expiración puede tardar ~1-2 seg
- 🔄 Cargas subsecuentes son instantáneas (<5ms) mientras cache esté vigente
- Si persiste, contactar a IT (puede indicar problema de red/SQL)

---

## ❓ PREGUNTAS FRECUENTES

### ¿Puedo registrar producción de días anteriores?

**Sí**, pero solo si la fecha es ≤ hoy. No se permite registro a futuro. Si necesitas corregir un registro antiguo, edita el registro existente o contacta a tu Coordinador.

---

### ¿Qué pasa si rechazan mi producción?

Recibirás email con:
- Motivo de rechazo
- Observaciones del revisor
- Link para ver detalles

Debes:
1. Revisar observaciones
2. Corregir error (si aplica)
3. Registrar nuevamente con datos correctos

El registro rechazado queda en historial para auditoría.

---

### ¿Puedo exceder mi meta asignada?

**Sí**, el sistema permite registrar producción que excede la meta (muestra warning, no bloquea). Esto puede ocurrir por:
- Sobrecumplimiento planificado
- Reemplazo de encuestas rechazadas
- Extensión de muestra

Coordinador debe validar en Revisión de Productividad.

---

### ¿Cómo sé cuánto me falta para cumplir mi meta?

1. Ir a `/OP/TrabajosCoordinador` (vista encuestador)
2. Ver columna "Avance": muestra "Logrado / Meta"
3. Ejemplo: 85 / 100 → Faltan 15

También puedes ver desglose en `/OP/RegistroProduccion` en sección "Mis Registros".

---

### ¿Los festivos afectan mis plazos?

**Sí**. El sistema considera festivos al calcular:
- Estimación de días de campo
- Cálculo de jornadas laborales (integración con TH_Ausencia)

Admin debe configurar festivos en `/OP/Festivos` para que se apliquen correctamente.

---

### ¿Puedo descargar mi historial de producción?

**Sí**:
1. Ir a `/OP/RevisionProductividad`
2. Filtrar por tu nombre
3. Seleccionar rango de fechas
4. Clic en **[📥 Exportar Excel]**

Archivo incluye todos tus registros con estado de aprobación.

---

## 📚 GLOSARIO

| Término | Definición |
|---------|------------|
| **COE** | Centro de Operaciones Estadísticas. Proceso de configuración del marco muestral y asignación de recursos. |
| **Cuota** | Cantidad de encuestas asignadas a un segmento específico (edad, género, ubicación, etc.) |
| **Ficha Cuantitativa** | Documento que define características del estudio: universo, metodología, tamaño muestral, cuotas. |
| **IPS** | Sistema de Información de Producción en Sitio. Exportaciones de datos de campo. |
| **JobBook** | Identificador único del proyecto/trabajo en sistema Ipsos. |
| **Marco Muestral** | Descripción de la población objetivo y método de selección de unidades. |
| **Metodología** | Técnica de recolección: Telefónica (CATI), Presencial (CAPI), Online (CAWI), Mixta. |
| **MyS** | Metodología y Sistemas. Equipo de control de calidad. |
| **PMO** | Project Management Office. Oficina de gestión de proyectos. |
| **Productividad** | Cantidad de unidades completadas (encuestas, llamadas, visitas) por encuestador/día. |
| **Rendimiento** | Tasa promedio de producción (ejemplo: 10 encuestas/día/persona). |
| **Revisión Multirrol** | Flujo de aprobación secuencial: PMO → Coordinador → MyS/Call. |
| **Subactividad** | Nivel de detalle dentro de una Actividad (ejemplo: Actividad: Encuestas; Subactividad: Completadas). |
| **Tráfico** | Dashboard de métricas en tiempo real (movimientos, avances, pendientes). |
| **Unidad** | Nivel geográfico o administrativo (ejemplo: Bogotá, Medellín, Nacional). |

---

## 📞 SOPORTE

### Contacto

- **Mesa de Ayuda IT**: [email protected] | Ext. 1234
- **PMO Operaciones**: [email protected] | Ext. 5678
- **Admin Sistema**: [email protected]

### Horario de Atención

- Lunes a Viernes: 8:00 AM - 6:00 PM
- Sábados: 9:00 AM - 1:00 PM (solo urgencias)

### Recursos Adicionales

- 📖 **Wiki Técnica**: `https://wiki.ipsos.com/matrixnext/op`
- 🎥 **Video Tutoriales**: `https://training.ipsos.com/op-cuantitativo`
- 💬 **Canal Slack**: `#matrixnext-op-support`

---

**Fin del Manual de Usuario**  
**Versión**: 1.0  
**Última Actualización**: 2026-01-08
