# MIGRACIÓN TH CAPACITACIONES COMPLETADA

**Fecha**: 2026-01-28  
**Commit**: 84a9895a  
**Sprint**: Fase 1 TH (Post-Auditoría)

---

## 📋 RESUMEN

Módulo de **Capacitaciones** del área TH migrado completamente desde WebMatrix.

### Página WebMatrix Original
- `WebMatrix/TH_TalentoHumano/Capacitacion.aspx`

---

## ✅ COMPONENTES MIGRADOS

### 1. DTOs (MatrixNext.Data/Modules/TH/Capacitaciones/Models/)

| DTO | Descripción |
|-----|-------------|
| `CapacitacionDto` | Modelo para listar capacitaciones |
| `CapacitacionCreateEditDto` | Modelo para crear/editar capacitación |
| `CapacitacionParticipanteDto` | Modelo para participantes de capacitación |
| `CapacitacionParticipanteCreateDto` | Modelo para agregar participante |
| `CapacitacionParticipanteUpdateDto` | Modelo para actualizar participante |
| `PersonaCapacitacionDto` | Modelo para búsqueda de personas |
| `BuscarPersonasCapacitacionParams` | Parámetros de búsqueda |
| `ResponsableComboDto` | Combo de responsables |

### 2. Adapter (MatrixNext.Data/Modules/TH/Capacitaciones/Adapters/)

**ICapacitacionAdapter** - Interfaz con métodos:
- `ObtenerCapacitacionesAsync` - Listar capacitaciones
- `ObtenerCapacitacionPorIdAsync` - Obtener por ID
- `GuardarCapacitacionAsync` - Crear/Editar
- `EliminarCapacitacionAsync` - Eliminar
- `CrearRefuerzoAsync` - Crear capacitación de refuerzo
- `ObtenerParticipantesAsync` - Listar participantes
- `AgregarParticipanteAsync` - Agregar participante
- `ActualizarParticipanteAsync` - Actualizar participante
- `EliminarParticipanteAsync` - Eliminar participante
- `BuscarPersonasAsync` - Buscar personas disponibles
- `ObtenerResponsablesAsync` - Combo de responsables

### 3. Service (MatrixNext.Data/Modules/TH/Capacitaciones/Services/)

**ICapacitacionService** - Servicio con lógica de negocio:
- Validaciones de negocio
- Logging de operaciones
- Manejo de errores amigables
- Patrón de retorno `(bool, string, int?)`

### 4. Controller (MatrixNext.Web/Areas/TH/Controllers/)

**CapacitacionesController**:
- `[Area("TH")]`
- `[Authorize]`
- `[Route("TH/[controller]")]`
- Permiso equivalente: 86 (WebMatrix)

**Endpoints**:
| Acción | Método | Descripción |
|--------|--------|-------------|
| `Index` | GET | Vista principal |
| `Lista` | GET | Partial con listado (AJAX) |
| `Create` | GET | Modal crear |
| `Edit` | GET | Modal editar |
| `Save` | POST | Guardar capacitación |
| `DeleteConfirm` | GET | Modal confirmar eliminación |
| `Delete` | POST | Eliminar capacitación |
| `CrearRefuerzo` | POST | Crear capacitación de refuerzo |
| `Participantes` | GET | Modal participantes |
| `AgregarParticipante` | GET | Partial agregar participante |
| `BuscarPersonas` | GET | Búsqueda AJAX |
| `GuardarParticipante` | POST | Agregar participante |
| `ActualizarParticipante` | POST | Actualizar participante |
| `EliminarParticipante` | POST | Eliminar participante |

### 5. Vistas (MatrixNext.Web/Areas/TH/Views/Capacitaciones/)

| Vista | Tipo | Descripción |
|-------|------|-------------|
| `Index.cshtml` | Vista | Página principal con filtros |
| `_Lista.cshtml` | Partial | Tabla de capacitaciones |
| `_CreateEditModal.cshtml` | Partial | Modal crear/editar |
| `_DeleteModal.cshtml` | Partial | Modal confirmar eliminación |
| `_Participantes.cshtml` | Partial | Modal gestión participantes |
| `_AgregarParticipanteModal.cshtml` | Partial | Formulario agregar participante |

---

## 🗄️ STORED PROCEDURES MAPEADOS

| SP | Acción | Verificado |
|----|--------|------------|
| `TH_Capacitaciones_Get` | Listar capacitaciones | ✅ |
| `TH_Capacitaciones_Add` | Crear capacitación | ✅ |
| `TH_Capacitaciones_Edit` | Editar capacitación | ✅ |
| `TH_Capacitaciones_Del` | Eliminar capacitación | ✅ |
| `TH_Capacitaciones_AddRefuerzo` | Crear refuerzo | ✅ |
| `TH_CapacitacionParticipantes_Get` | Listar participantes | ✅ |
| `TH_CapacitacionesParticipantes_Add` | Agregar participante | ✅ |
| `TH_CapacitacionesParticipantes_Edit` | Actualizar participante | ✅ |
| `TH_CapacitacionesParticipantes_Del` | Eliminar participante | ✅ |
| `TH_CapacitacionPersonas_Get` | Buscar personas | ✅ |
| `TH_Responsables_Combo` | Combo responsables | ✅ |

---

## 🎨 FUNCIONALIDADES UI

### Vista Principal (Index)
- ✅ Filtros: Responsable, Fecha Desde, Fecha Hasta
- ✅ Botón "Nueva Capacitación"
- ✅ Lista paginada AJAX
- ✅ Botones de acción por registro

### CRUD Capacitaciones
- ✅ Modal crear/editar
- ✅ Validación client-side y server-side
- ✅ Campos: Actividad, Fecha, Ubicación, Duración, Responsable, Capacitador, Modo Evaluación, Objetivo
- ✅ Toast de notificaciones
- ✅ Confirmación de eliminación

### Gestión de Participantes
- ✅ Modal de participantes por capacitación
- ✅ Tabla editable: Asistió, Aprobó, Observaciones
- ✅ Guardar cambios inline
- ✅ Búsqueda de personas para agregar
- ✅ Eliminar participante con confirmación
- ✅ Resumen (Total, Asistieron, Aprobaron)

### Funcionalidad Especial
- ✅ Crear Capacitación de Refuerzo (copia de una existente)

---

## 🔧 CONFIGURACIÓN

### DI Registration
```csharp
// ServiceCollectionExtensions.cs
services.AddScoped<ICapacitacionAdapter, CapacitacionAdapter>();
services.AddScoped<ICapacitacionService, CapacitacionService>();
```

### Menú
- ✅ Agregado en `_main-sidebar.cshtml` bajo categoría "Talento Humano"

---

## 🧪 TESTING PENDIENTE

### Funcional (verificar en ambiente)
- [ ] Crear capacitación
- [ ] Editar capacitación
- [ ] Eliminar capacitación
- [ ] Crear refuerzo
- [ ] Agregar participante
- [ ] Actualizar participante (Asistió, Aprobó, Observaciones)
- [ ] Eliminar participante
- [ ] Filtros funcionan
- [ ] Paginación (si aplica)

---

## 📊 MÉTRICAS

| Métrica | Valor |
|---------|-------|
| Archivos creados | 10 |
| Líneas de código | ~1,687 |
| SPs mapeados | 11 |
| Endpoints | 14 |
| Vistas | 6 |

---

## 🔄 SIGUIENTE MÓDULO

Según priorización de Fase 1 TH:
1. ~~Capacitaciones~~ ✅
2. **Contratistas** → SIGUIENTE
3. Personas
4. HojasVida/HojaVida
5. HWH/HWH-Admin/HWH-RH
