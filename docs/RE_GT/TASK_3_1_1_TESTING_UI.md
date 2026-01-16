# TASK 3.1.1: TESTING UI BÁSICO - TRAFICO TAREAS

**Objetivo**: Validar que TraficoTareas carga correctamente en navegador y toda funcionalidad básica funciona.

**Fecha**: 2026-01-15  
**Duración Estimada**: 1h  
**Status**: ⏳ En Progreso

---

## ✅ CHECKLIST DE TESTING

### 1. Acceso a la URL ✅
- [ ] Navegar a `http://localhost:5000/CORE/WorkFlow/TraficoTareas` (o puerto configurado)
- [ ] Página carga sin errores (HTTP 200)
- [ ] Sin redirects inesperados
- [ ] Página es responsiva (desktop + mobile)

**Notas**:
- URL pattern: `/AREA/CONTROLLER/ACTION`
- AREA = CORE
- CONTROLLER = WorkFlow
- ACTION = TraficoTareas

### 2. Carga de Datos ✅
- [ ] Tabla se renderiza con datos
- [ ] Encabezados: Tipo, Fecha Inicio, Fecha Fin, Días, Estado, Acciones
- [ ] Mínimo 1 fila (usuario tiene tareas)
- [ ] Sin errores en consola (F12 → Console)
- [ ] Indicadores visuales funcionan:
  - [ ] Badge de estado con color correcto (Radicada, Aprobada, Rechazada, etc.)
  - [ ] Ícono de urgencia (⚠️) cuando días ≤ 5
  - [ ] Contador de días vencidos en rojo si aplica

### 3. Filtros ✅

#### 3a. Filtro por Unidad
- [ ] Dropdown carga con lista de unidades
- [ ] Seleccionar unidad filtra tabla
- [ ] Tabla se actualiza sin refresh página
- [ ] Contador de registros actualiza

#### 3b. Filtro por Estado
- [ ] Dropdown carga: Radicada, Aprobada, Rechazada, Pendiente, etc.
- [ ] Seleccionar estado filtra tabla
- [ ] Filtro se combina con otros

#### 3c. Filtro por Prioridad
- [ ] Dropdown carga: Baja, Media, Alta, Crítica
- [ ] Seleccionar filtra tabla
- [ ] Ordenamiento visual en UI

#### 3d. Búsqueda de Texto
- [ ] Input de búsqueda esconde/muestra registros
- [ ] Busca en: Descripción, Responsable, ID
- [ ] Búsqueda no diferencia mayúsculas
- [ ] Búsqueda en tiempo real (no requiere botón)

#### 3e. Filtros Combinados
- [ ] Aplicar Unidad + Estado: solo muestra datos con ambos criterios
- [ ] Aplicar Unidad + Prioridad + Búsqueda: funciona correctamente
- [ ] Limpiar filtros: vuelve a mostrar todos

### 4. Paginación ✅
- [ ] Tabla muestra 25 registros por página (configurable)
- [ ] Total de registros mostrado (ej: "1-25 de 156 registros")
- [ ] Botón "Anterior" deshabilitado en página 1
- [ ] Botón "Siguiente" deshabilitado en última página
- [ ] Clickear página 2+ muestra registros diferentes
- [ ] Paginación funciona después de aplicar filtros
- [ ] Performance aceptable (< 1s para cambio de página)

### 5. Modal de Detalles ✅
- [ ] Clickear ícono 👁️ en columna "Acciones" abre modal
- [ ] Modal se posiciona en centro de pantalla
- [ ] Modal contiene detalles completos:
  - [ ] ID de tarea
  - [ ] Descripción
  - [ ] Unidad responsable
  - [ ] Asignado a
  - [ ] Fecha inicio / Fin
  - [ ] Prioridad
  - [ ] Estado
  - [ ] Observaciones
- [ ] Botón cerrar (X) cierra modal
- [ ] Clicking fuera modal cierra (backdrop)
- [ ] Sin errores JS en consola

### 6. Indicadores Visuales ✅
- [ ] Urgencia (amarillo si ≤ 5 días, rojo si vencido)
- [ ] Badges de estado con colores:
  - Verde: Aprobada
  - Azul: Radicada
  - Naranja: Pendiente
  - Rojo: Rechazada
- [ ] Iconografía Font Awesome carga correctamente (no caras de gato 😹)
- [ ] Responsive en mobile (badges no se superponen)

### 7. Carga de Página ✅
- [ ] Tiempo de carga inicial: **< 2 segundos** (target de performance)
- [ ] Network tab (F12) muestra:
  - [ ] HTML principal: ~50KB
  - [ ] JavaScript bundle: ~100KB
  - [ ] CSS: ~30KB
  - [ ] Imágenes/Font Awesome: ~200KB
- [ ] Total <500KB (objetivo de bundle optimization)
- [ ] Sin recursos 404 (todas las request HTTP 200/304)

### 8. Errores en Consola (F12 → Console) ✅
- [ ] **CRÍTICO**: 0 errores (Error en rojo)
- [ ] **ALTO**: 0 warnings críticos (console.warn)
- [ ] **BAJO**: Warnings pre-existentes aceptables (CSP, deprecation)
- [ ] Aplicación se ejecuta sin trazas de error

---

## 📝 HALLAZGOS Y OBSERVACIONES

### Éxitos ✅
- [Completar durante testing]

### Problemas Encontrados ⚠️
- [Completar durante testing]

### Recomendaciones 💡
- [Completar durante testing]

---

## 🔍 EVIDENCIA

### Screenshots
- [ ] UI cargada correctamente
- [ ] Tabla con datos
- [ ] Modal abierto
- [ ] Filtros aplicados

### Logs
- [Copiar aquí output de consola si hay errores]

### Métricas
- Tiempo carga inicial: _____ ms
- Tamaño bundle: _____ KB
- Número registros en tabla: _____
- Performance score (Lighthouse): _____ / 100

---

## ✅ SIGNOFF

**Testeado por**: GitHub Copilot  
**Fecha**: 2026-01-15  
**Ambiente**: Local (localhost:5000)  
**Browser**: [Completar: Chrome, Edge, Firefox]  
**Resultado**: ⏳ [PASS / FAIL / INCONCLUSO]

---

**Siguiente TASK**: 3.1.2 Testing de Permisos
