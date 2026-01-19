# PLAN DE TESTING INTEGRAL - MatrixNext

> **Fecha**: 18 de enero de 2026  
> **Fase**: 5 - Testing Integral (25h)  
> **Objetivo**: Validar funcionalidad, integración, seguridad y performance de 28 módulos migrados

---

## 📋 ÍNDICE

1. [Testing Funcional (12h)](#testing-funcional)
2. [Testing de Integración (8h)](#testing-de-integración)
3. [Testing de Seguridad y Performance (5h)](#testing-de-seguridad-y-performance)
4. [Criterios de Aceptación](#criterios-de-aceptación)
5. [Registro de Ejecución](#registro-de-ejecución)

---

## 🧪 TESTING FUNCIONAL (12h)

### Objetivo
Validar que cada módulo migrado funciona correctamente según especificaciones de WebMatrix.

### Módulos a Testear (28 total)

#### 1. **Módulos PY (Proyectos)** - 7 módulos

| # | Módulo | Funcionalidad | Sprint | Prioridad |
|---|--------|--------------|--------|-----------|
| 1 | PY_VariablesControl | CRUD Variables Control + Reportes + Excel | 22 | 🔴 Alta |
| 2 | PY_DuplicarTrabajos | Duplicar trabajo con opciones | 22 | 🔴 Alta |
| 3 | PY_Distribucion | CRUD Distribución campo | Previo | 🟡 Media |
| 4 | PY_ControlCalidad | CRUD Control calidad + Preguntas | Previo | 🟡 Media |
| 5 | PY_AsignacionCampo | Asignar personal a trabajo | Previo | 🟡 Media |
| 6 | PY_Cualitativo | Gestión completa cuali | Previo | 🟢 Baja |
| 7 | PY_Trabajos | CRUD Trabajos básico | Previo | 🟡 Media |

#### 2. **Módulos OP (Operaciones)** - 8 módulos

| # | Módulo | Funcionalidad | Sprint | Prioridad |
|---|--------|--------------|--------|-----------|
| 8 | OP_SolicitudPresupuestosInternos | Solicitar presupuesto COE | 22 | 🔴 Alta |
| 9 | OP_RegistroProduccion | CRUD Producción + Reportes | Previo | 🔴 Alta |
| 10 | OP_Trafico | Dashboard tráfico | 11 | 🟡 Media |
| 11 | OP_RO (Research Operations) | Operaciones investigación | 11 | 🟡 Media |
| 12 | OP_Cuantitativo | Gestión trabajos cuanti | 12 | 🟡 Media |
| 13 | OP_Cualitativo | Gestión trabajos cuali | 6 | 🟢 Baja |
| 14 | OP_Planillas | CRUD Planillas aprobación | Previo | 🟡 Media |
| 15 | OP_Notificaciones | Sistema notificaciones | Previo | 🟢 Baja |

#### 3. **Módulos CC (Contabilidad/Costos)** - 4 módulos

| # | Módulo | Funcionalidad | Sprint | Prioridad |
|---|--------|--------------|--------|-----------|
| 16 | CC_Presupuestos | CRUD Presupuestos + Cálculos | Previo | 🔴 Alta |
| 17 | CC_PresupuestosInternos | Presupuestos internos | Previo | 🟡 Media |
| 18 | CC_Liquidacion | Liquidación trabajos | Previo | 🟡 Media |
| 19 | CC_CuentasCobro | CRUD Cuentas cobro | Previo | 🟢 Baja |

#### 4. **Módulos TH (Talento Humano)** - 3 módulos

| # | Módulo | Funcionalidad | Sprint | Prioridad |
|---|--------|--------------|--------|-----------|
| 20 | TH_Empleados | CRUD Empleados + Hoja vida | Previo | 🔴 Alta |
| 21 | TH_HWH | Horas trabajadas | Previo | 🟡 Media |
| 22 | TH_Capacitacion | Gestión capacitaciones | Previo | 🟢 Baja |

#### 5. **Módulos ES (Estudios/Metodología)** - 3 módulos

| # | Módulo | Funcionalidad | Sprint | Prioridad |
|---|--------|--------------|--------|-----------|
| 23 | ES_MetodologiaCampo | CRUD Metodología + Aprobación | Previo | 🟡 Media |
| 24 | ES_DisenoMuestral | CRUD Diseño muestral + Aprobación | Previo | 🟡 Media |
| 25 | ES_BriefDisenoMuestral | Brief diseño | Previo | 🟢 Baja |

#### 6. **Módulos GD/SGC (Gestión Documental/SGC)** - 3 módulos

| # | Módulo | Funcionalidad | Sprint | Prioridad |
|---|--------|--------------|--------|-----------|
| 26 | GD_SolicitudDocumentos | Solicitudes revisión docs | Previo | 🟡 Media |
| 27 | GD_PNC | Gestión PNC (Producto No Conforme) | Previo | 🔴 Alta |
| 28 | SGC_Auditoria | Gestión auditorías SGC | Previo | 🟢 Baja |

---

### Checklist de Testing por Módulo

Para cada módulo, ejecutar estos tests:

#### ✅ **Test 1: Acceso y Autorización**
- [ ] Usuario sin permisos NO puede acceder
- [ ] Usuario con permisos SÍ puede acceder
- [ ] Redirect a Login si no está autenticado
- [ ] Atributo `[Authorize]` presente en controller

#### ✅ **Test 2: CRUD Básico**
- [ ] **Create**: Modal abre, formulario valida, guarda correctamente
- [ ] **Read**: Lista carga datos, paginación funciona
- [ ] **Update**: Modal edición carga datos, actualiza correctamente
- [ ] **Delete**: Confirmación funciona, elimina correctamente

#### ✅ **Test 3: Validaciones**
- [ ] Campos requeridos muestran error si vacíos
- [ ] Validaciones de formato (email, fecha, número) funcionan
- [ ] `ModelState.IsValid` detecta errores
- [ ] Mensajes de error son claros y en español

#### ✅ **Test 4: AJAX y UX**
- [ ] Modal abre sin recargar página
- [ ] Spinner se muestra durante operaciones
- [ ] Toast/Alert muestra éxito/error
- [ ] Grid se refresca sin reload completo
- [ ] Botón submit se deshabilita durante request

#### ✅ **Test 5: Búsqueda y Filtros**
- [ ] Búsqueda por texto funciona
- [ ] Filtros por dropdown aplican correctamente
- [ ] Paginación mantiene filtros activos
- [ ] Reset limpia todos los filtros

#### ✅ **Test 6: Manejo de Errores**
- [ ] Error de BD retorna mensaje amigable
- [ ] NO se expone stack trace al usuario
- [ ] Logger registra error completo (con ex)
- [ ] Error HTTP 500 muestra página custom

#### ✅ **Test 7: Integración con BD**
- [ ] SP se ejecutan correctamente
- [ ] Parámetros se pasan con tipos correctos
- [ ] Transacciones se confirman (commit) o revierten (rollback)
- [ ] Auditoría se registra (RegistradoPor, FechaRegistro)

---

## 🔗 TESTING DE INTEGRACIÓN (8h)

### Objetivo
Validar flujos que cruzan múltiples módulos.

### Escenarios de Integración Críticos

#### **Escenario 1: Flujo Completo Trabajo PY → OP → CC**

**Descripción**: Crear trabajo en PY, asignar producción en OP, generar presupuesto en CC

**Pasos**:
1. ✅ Crear trabajo en `PY_Trabajos` (JobBook, Cliente, Metodología)
2. ✅ Asignar personal en `PY_AsignacionCampo` (Coordinador, Supervisores)
3. ✅ Registrar producción en `OP_RegistroProduccion` (Encuestas, horas)
4. ✅ Generar presupuesto en `CC_Presupuestos` basado en producción
5. ✅ Verificar que datos fluyen correctamente entre módulos

**Criterio de éxito**: JobBook visible en todos los módulos, cálculos coherentes

---

#### **Escenario 2: Workflow Aprobación Metodología ES → PY**

**Descripción**: Aprobar metodología en ES_MetodologiaCampo y reflejar en trabajo PY

**Pasos**:
1. ✅ Crear metodología en `ES_MetodologiaCampo` para IdTrabajo
2. ✅ Enviar a aprobación (cambio estado)
3. ✅ Aprobar metodología (Director Técnico)
4. ✅ Verificar que `PY_Trabajos` muestra estado "Metodología Aprobada"
5. ✅ Validar que email de notificación se envía

**Criterio de éxito**: Estado sincronizado, email enviado correctamente

---

#### **Escenario 3: Workflow PNC GD → OP → SGC**

**Descripción**: Registrar PNC en campo, gestionar correcciones, auditar

**Pasos**:
1. ✅ Registrar PNC en `GD_PNC` (descripción, categoría, responsable)
2. ✅ Asignar causas y acciones correctivas
3. ✅ Cambiar estado a "En corrección"
4. ✅ Verificar que aparece en dashboard `OP_Trafico` (indicadores calidad)
5. ✅ Cerrar PNC y verificar en `SGC_Auditoria` (registro histórico)

**Criterio de éxito**: PNC trazable en 3 módulos, estados coherentes

---

#### **Escenario 4: Duplicar Trabajo PY con Transferencia Presupuesto CC**

**Descripción**: Usar `PY_DuplicarTrabajos` y verificar que presupuesto se copia/referencia correctamente

**Pasos**:
1. ✅ Crear trabajo original con presupuesto en `CC_Presupuestos`
2. ✅ Duplicar trabajo con opción "Copiar presupuesto"
3. ✅ Verificar que nuevo trabajo tiene presupuesto independiente
4. ✅ Duplicar trabajo con opción "Referenciar presupuesto"
5. ✅ Verificar que ambos trabajos apuntan al mismo presupuesto

**Criterio de éxito**: Lógica de copia/referencia funciona según opción seleccionada

---

#### **Escenario 5: Variables Control PY + Reportes Consolidados**

**Descripción**: Registrar variables control y validar cálculos en reportes

**Pasos**:
1. ✅ Registrar evaluación en `PY_VariablesControl` (6 variables, 1-5 puntos)
2. ✅ Generar "Reporte Detallado" (por evaluado)
3. ✅ Verificar % cumplimiento (promedio de 6 variables)
4. ✅ Generar "Reporte Por Mes" (consolidado mensual)
5. ✅ Exportar a Excel y verificar formato

**Criterio de éxito**: Cálculos matemáticos correctos, Excel descarga con formato

---

## 🔒 TESTING DE SEGURIDAD Y PERFORMANCE (5h)

### 1. **Seguridad - Autorización (2h)**

#### Test 1.1: Validar `[Authorize]` en todos los endpoints

**Script PowerShell**:
```powershell
.\scripts\Validate-Authorize.ps1 -Verbose
```

**Resultado esperado**: 100% cobertura de `[Authorize]`

#### Test 1.2: Inyección SQL

**Test**: Intentar inyección en campos de búsqueda
```
Búsqueda: ' OR 1=1 --
Búsqueda: '; DROP TABLE PY_Trabajos; --
```

**Resultado esperado**: Dapper parametriza correctamente, NO ejecuta SQL inyectado

#### Test 1.3: CSRF (Cross-Site Request Forgery)

**Test**: Intentar POST sin `@Html.AntiForgeryToken()`

**Resultado esperado**: Error 400 Bad Request (ValidateAntiForgeryToken funciona)

#### Test 1.4: XSS (Cross-Site Scripting)

**Test**: Ingresar HTML/JavaScript en campos de texto
```
Observaciones: <script>alert('XSS')</script>
```

**Resultado esperado**: Razor escapa HTML automáticamente (`@Model.Observaciones`)

---

### 2. **Performance - Tiempos de Respuesta (2h)**

#### Test 2.1: Listados con paginación

**Requisito**: Listado con 1,000 registros carga en < 2 segundos

**Módulos críticos**:
- `OP_RegistroProduccion` (alto volumen)
- `PY_Trabajos` (listado principal)
- `TH_Empleados` (listado completo)

**Herramienta**: Chrome DevTools (Network tab)

#### Test 2.2: Query N+1

**Test**: Verificar que NO hay múltiples queries en bucle

**Ejemplo problemático**:
```csharp
// ❌ N+1: 1 query por trabajo
foreach (var trabajo in trabajos) {
    var cliente = _db.Clientes.Find(trabajo.IdCliente); // Query dentro de loop
}

// ✅ Correcto: 1 query total
var trabajos = _db.Trabajos.Include(t => t.Cliente).ToList();
```

**Resultado esperado**: Usar `.Include()` o joins en SP, NO queries individuales

#### Test 2.3: Carga de modales AJAX

**Requisito**: Modal abre en < 500ms

**Test**: Medir tiempo desde click hasta modal visible

**Módulos a validar**:
- `PY_VariablesControl` (modal edición)
- `OP_SolicitudPresupuestosInternos` (modal solicitud)
- `GD_PNC` (modal causas)

---

### 3. **Performance - Uso de Async/Await (1h)**

#### Test 3.1: Validar async en controllers

**Script PowerShell**:
```powershell
# Verificar que NO hay .Result o .Wait()
Select-String -Path "MatrixNext.Web/Areas/**/*.cs" -Pattern "\.Result|\.Wait\(\)" -Recursive
```

**Resultado esperado**: 0 matches (100% async/await)

#### Test 3.2: Validar async en services

**Script**:
```powershell
Select-String -Path "MatrixNext.Data/Services/**/*.cs" -Pattern "public [^a].*Task" -Recursive
```

**Resultado esperado**: Todos los métodos que retornan `Task` son `async`

---

## ✅ CRITERIOS DE ACEPTACIÓN

### Funcionalidad
- [ ] **100%** módulos pasan checklist CRUD básico (28/28)
- [ ] **100%** validaciones funcionan correctamente
- [ ] **100%** modales AJAX funcionan (apertura, guardado, cierre, refresh)
- [ ] **0** errores de validación sin mensaje al usuario

### Integración
- [ ] **5/5** escenarios de integración pasan exitosamente
- [ ] Datos fluyen correctamente entre módulos relacionados
- [ ] Estados sincronizados en workflows multi-módulo

### Seguridad
- [ ] **100%** endpoints con `[Authorize]` (validado con script)
- [ ] **0** vulnerabilidades SQL injection
- [ ] **0** vulnerabilidades XSS
- [ ] CSRF protection activo en todos los POST

### Performance
- [ ] Listados con 1,000 registros cargan en **< 2s**
- [ ] Modales AJAX abren en **< 500ms**
- [ ] **0** queries N+1 en listados
- [ ] **100%** operaciones I/O usan async/await

---

## 📝 REGISTRO DE EJECUCIÓN

### Sprint 22 - Fase 5 Testing

**Fecha inicio**: 18 de enero de 2026  
**Responsable**: Equipo MatrixNext  
**Estado**: 🟡 En progreso

---

#### **Sesión 1: Testing Funcional Módulos Fase 4** (4h)

**Fecha**: 18 de enero de 2026

| Módulo | Checklist | Resultado | Observaciones |
|--------|-----------|-----------|---------------|
| PY_VariablesControl | ⏳ Pendiente | - | Prioridad Alta |
| PY_DuplicarTrabajos | ⏳ Pendiente | - | Prioridad Alta |
| OP_SolicitudPresupuestosInternos | ⏳ Pendiente | - | Prioridad Alta |

---

#### **Sesión 2: Testing Funcional Módulos Críticos** (4h)

**Fecha**: TBD

| Módulo | Checklist | Resultado | Observaciones |
|--------|-----------|-----------|---------------|
| TBD | - | - | - |

---

#### **Sesión 3: Testing Integración** (8h)

**Fecha**: TBD

| Escenario | Pasos Completados | Resultado | Issues |
|-----------|-------------------|-----------|--------|
| Flujo PY → OP → CC | 0/5 | ⏳ Pendiente | - |
| Workflow ES → PY | 0/5 | ⏳ Pendiente | - |
| Workflow PNC | 0/5 | ⏳ Pendiente | - |
| Duplicar Trabajo | 0/5 | ⏳ Pendiente | - |
| Variables Control | 0/5 | ⏳ Pendiente | - |

---

#### **Sesión 4: Testing Seguridad y Performance** (5h)

**Fecha**: TBD

| Test | Tool | Resultado | Issues |
|------|------|-----------|--------|
| Validate-Authorize.ps1 | PowerShell | ⏳ Pendiente | - |
| SQL Injection | Manual | ⏳ Pendiente | - |
| CSRF | Manual | ⏳ Pendiente | - |
| XSS | Manual | ⏳ Pendiente | - |
| Performance Listados | Chrome DevTools | ⏳ Pendiente | - |
| Query N+1 | SQL Profiler | ⏳ Pendiente | - |
| Async/Await | PowerShell | ⏳ Pendiente | - |

---

## 📊 MÉTRICAS DE CALIDAD

### Cobertura Actual

- **Módulos migrados**: 28/28 (100%)
- **Módulos testeados**: 0/28 (0%)
- **Tests funcionales pasados**: 0/196 (0%) - 7 tests × 28 módulos
- **Tests integración pasados**: 0/5 (0%)
- **Tests seguridad pasados**: 0/4 (0%)
- **Tests performance pasados**: 0/3 (0%)

### Objetivo Fase 5

- **Módulos testeados**: 28/28 (100%)
- **Tests funcionales pasados**: 196/196 (100%)
- **Tests integración pasados**: 5/5 (100%)
- **Tests seguridad pasados**: 4/4 (100%)
- **Tests performance pasados**: 3/3 (100%)

**Total tests**: 208 tests a ejecutar

---

## 🚀 PRÓXIMOS PASOS

1. ✅ **Crear TESTING_PLAN.md** (Completado)
2. ⏳ **Ejecutar Testing Funcional Fase 4** (4h) - En progreso
3. ⏳ Ejecutar Testing Funcional Módulos Críticos (4h)
4. ⏳ Ejecutar Testing Funcional Módulos Restantes (4h)
5. ⏳ Ejecutar Testing Integración (8h)
6. ⏳ Ejecutar Testing Seguridad y Performance (5h)
7. ⏳ Documentar resultados y crear reporte final

**Tiempo total estimado**: 25h
