# E2E Testing Guide - Sprint 3: Revisión de Planillas y Registro de Producción

**Date**: 2026-01-08  
**Sprint**: 3  
**Module**: OP (Operaciones)  
**Tester**: QA Team  

## Test Environment Requirements

- ✅ Database: SQL Server with populated Catalogo_* tables
- ✅ SPs: OP_CuantiDapper_Get, OP_PlanillaProductividad_Aprobar, OP_PlanillaProductividad_Rechazar
- ✅ Web Server: ASP.NET Core 8.0 running locally or staging
- ✅ Browser: Chrome/Firefox (Bootstrap 5 compatible)
- ✅ Permissions: Users with roles 100, 135, 156, 157 for testing

---

## TEST SUITE 1: Revisión de Planillas Multirrol

### TS1.1: PMO Revision Flow (Permiso 100)

#### TC1.1.1 - Load PMO Revision Dashboard
- **Objective**: Verify PMO dashboard loads with trabajo selector
- **Steps**:
  1. Login as PMO user (Permiso 100)
  2. Navigate to: `/OP/RevisionProductividadPMO`
  3. Verify page loads with "Revisión de Planillas - PMO" title
  4. Verify "Seleccione un Trabajo" dropdown populated with trabajos
  5. Verify "Cargar Planillas" button is enabled
- **Expected Result**: Dashboard displays correctly with trabajos loaded
- **Status**: ⏳ Pending

#### TC1.1.2 - Load Planillas for Selected Trabajo
- **Objective**: Load and display planillas for selected trabajo
- **Steps**:
  1. Select a trabajo with pending planillas
  2. Click "Cargar Planillas" button
  3. Verify spinner shows briefly
  4. Verify grid displays with columns: ID, Concepto, Cantidad, Valor, Monto Previo, Monto Actual, Diferencia, Estado, Acciones
  5. Verify planilla count badge updates
- **Expected Result**: Grid populated with planillas, count shows correct number
- **Status**: ⏳ Pending

#### TC1.1.3 - Approve Planilla with Valid Monto
- **Objective**: Successfully approve a planilla with valid authorization amount
- **Steps**:
  1. Load planillas (from TC1.1.2)
  2. Click "Aprobar" button on first planilla
  3. Verify modal opens with planilla details
  4. Verify "Monto Previo" and "Monto Actual" display correctly
  5. Modify "Monto a Autorizar" to valid amount (e.g., reduce from actual)
  6. Enter observation: "Aprobado con reducción presupuestaria"
  7. Click "Aprobar" button in modal
- **Expected Result**: Success toast appears, modal closes, planilla status updates to "Aprobada"
- **Status**: ⏳ Pending

#### TC1.1.4 - Approve Planilla - Invalid Monto (Negative)
- **Objective**: Verify validation prevents negative montos
- **Steps**:
  1. Open approve modal (from TC1.1.3)
  2. Enter negative amount in "Monto a Autorizar"
  3. Click "Aprobar"
- **Expected Result**: Error message displays: "Debe ingresar un monto válido"
- **Status**: ⏳ Pending

#### TC1.1.5 - Reject Planilla with Observation
- **Objective**: Reject a planilla with detailed observation
- **Steps**:
  1. Click "Rechazar" button on a planilla
  2. Verify reject modal opens
  3. Enter observation: "Valores no corresponden a cotización aprobada"
  4. Click "Rechazar" in modal
- **Expected Result**: Success toast, modal closes, planilla status updates to "Rechazada"
- **Status**: ⏳ Pending

#### TC1.1.6 - Reject Planilla - Empty Observation
- **Objective**: Verify observation is required for rejection
- **Steps**:
  1. Open reject modal
  2. Leave observation empty
  3. Click "Rechazar"
- **Expected Result**: Error message: "Debe indicar el motivo del rechazo"
- **Status**: ⏳ Pending

#### TC1.1.7 - Filter Approved Planillas
- **Objective**: Verify approved planillas show correct badge
- **Steps**:
  1. Approve 1-2 planillas
  2. Reload planillas for same trabajo
  3. Verify approved planillas show "Aprobada" badge (green)
  4. Verify pending planillas show "Pendiente" badge (yellow)
- **Expected Result**: Status badges display correctly
- **Status**: ⏳ Pending

---

### TS1.2: Coordinador Revision Flow (Permiso 135)

#### TC1.2.1 - Coordinator Loads Own Zone Trabajos
- **Objective**: Coordinator sees only trabajos in assigned zone
- **Steps**:
  1. Login as Coordinador user (Permiso 135)
  2. Navigate to `/OP/RevisionProductividadCoordinador`
  3. Verify dropdown shows only trabajos in coordinator's zone
- **Expected Result**: Only zone-specific trabajos displayed
- **Status**: ⏳ Pending

#### TC1.2.2 - Coordinator Approve Flow
- **Objective**: Coordinator can approve planillas with zone supervisory role
- **Steps**:
  1. Load planillas for zone trabajo
  2. Approve a planilla with monto authorization
  3. Add observation: "Aprobado supervisión zona"
- **Expected Result**: Planilla approved successfully
- **Status**: ⏳ Pending

---

### TS1.3: Campo Revision Flow (Permiso 156)

#### TC1.3.1 - Field Supervisor Loads Own City Trabajos
- **Objective**: Field supervisor sees only trabajos in assigned city
- **Steps**:
  1. Login as Supervisor de Campo (Permiso 156)
  2. Navigate to `/OP/RevisionProductividadCampo`
  3. Verify dropdown shows only trabajos in assigned city
- **Expected Result**: Only city-specific trabajos displayed
- **Status**: ⏳ Pending

#### TC1.3.2 - Field Supervisor Approve Flow
- **Objective**: Field supervisor can approve planillas
- **Steps**:
  1. Load planillas for city trabajo
  2. Approve planilla
- **Expected Result**: Successful approval
- **Status**: ⏳ Pending

---

### TS1.4: MyS/Call Revision Flow (Permiso 157)

#### TC1.4.1 - MyS/Call User Sees Only CATI/CAWI Activities
- **Objective**: MyS/Call users see only CATI/CAWI related planillas
- **Steps**:
  1. Login as MyS/Call user (Permiso 157)
  2. Navigate to `/OP/RevisionProductividadMYSCall`
  3. Load planillas
  4. Verify planillas show type badges: CATI (17%), CAWI (purple)
- **Expected Result**: Only CATI/CAWI activities shown, correct badges
- **Status**: ⏳ Pending

#### TC1.4.2 - MyS/Call Approve CATI Planilla
- **Objective**: Approve CATI-type planilla
- **Steps**:
  1. Select CATI-type planilla
  2. Approve with observation: "CATI aprobado"
- **Expected Result**: Successfully approved
- **Status**: ⏳ Pending

---

## TEST SUITE 2: Registro de Producción

### TS2.1: Form Initialization

#### TC2.1.1 - Form Loads with Today's Date
- **Objective**: Verify date field initializes to current date
- **Steps**:
  1. Navigate to `/OP/RegistroProduccionOP`
  2. Verify "Fecha" field contains today's date
- **Expected Result**: Date = today
- **Status**: ⏳ Pending

#### TC2.1.2 - Unidades Load in First Dropdown
- **Objective**: First cascading dropdown populated on page load
- **Steps**:
  1. Page loads
  2. Verify "Unidad / Área" dropdown contains multiple units
- **Expected Result**: Multiple units visible (e.g., "CATI", "CAWI", "Encuestas")
- **Status**: ⏳ Pending

---

### TS2.2: Cascading Dropdowns

#### TC2.2.1 - Select Unidad → Actividades Cascade
- **Objective**: Selecting unidad loads corresponding actividades
- **Steps**:
  1. Select "CATI" from Unidad dropdown
  2. Verify "Actividad" dropdown is enabled
  3. Verify actividades load for CATI
- **Expected Result**: Actividad dropdown populated with CATI actividades
- **Status**: ⏳ Pending

#### TC2.2.2 - Select Actividad → Subactividades Cascade
- **Objective**: Selecting actividad loads subactividades
- **Steps**:
  1. Select "Encuestas Completadas" from Actividad
  2. Verify "Subactividad" dropdown is enabled
  3. Verify subactividades load
- **Expected Result**: Subactividad dropdown populated
- **Status**: ⏳ Pending

#### TC2.2.3 - Cascading Reset on Change
- **Objective**: Changing parent resets children
- **Steps**:
  1. Select Unidad → Actividad → Subactividad
  2. Change Unidad selection
  3. Verify Actividad and Subactividad reset to defaults
- **Expected Result**: Dependent dropdowns reset
- **Status**: ⏳ Pending

---

### TS2.3: JobBook Search Modal

#### TC2.3.1 - Open JobBook Search Modal
- **Objective**: Search modal opens correctly
- **Steps**:
  1. Click "Buscar" button next to Job Book field
  2. Verify modal opens with "Buscar Job Book" title
- **Expected Result**: Modal displays with search input
- **Status**: ⏳ Pending

#### TC2.3.2 - Search JobBooks by Codigo
- **Objective**: Search JobBooks by code
- **Steps**:
  1. Open search modal
  2. Enter codigo: "JB001"
  3. Click "Buscar"
  4. Verify results table shows matching JobBooks
- **Expected Result**: Results table populated with matching records
- **Status**: ⏳ Pending

#### TC2.3.3 - Select JobBook from Results
- **Objective**: Select JobBook populates form field
- **Steps**:
  1. Search and get results
  2. Click "Seleccionar" on first result
  3. Verify modal closes
  4. Verify Job Book field shows selected codigo + nombre
- **Expected Result**: JobBook selected and displayed
- **Status**: ⏳ Pending

#### TC2.3.4 - Empty Search Input
- **Objective**: Validate empty search warning
- **Steps**:
  1. Open modal
  2. Leave search empty
  3. Click "Buscar"
- **Expected Result**: Warning toast: "Ingrese un criterio de búsqueda"
- **Status**: ⏳ Pending

---

### TS2.4: Form Validation

#### TC2.4.1 - Cantidad Validation (Must be > 0)
- **Objective**: Reject zero or negative cantidad
- **Steps**:
  1. Fill all required fields
  2. Enter "0" in Cantidad
  3. Click "Guardar Registro"
- **Expected Result**: Error: "La cantidad debe ser mayor a 0"
- **Status**: ⏳ Pending

#### TC2.4.2 - Date Cannot Be Future
- **Objective**: Reject future dates
- **Steps**:
  1. Select tomorrow's date
  2. Click "Guardar Registro"
- **Expected Result**: Error: "No se puede registrar actividades en fechas futuras"
- **Status**: ⏳ Pending

#### TC2.4.3 - All Required Fields Mandatory
- **Objective**: Form requires all mandatory fields
- **Steps**:
  1. Leave Unidad empty
  2. Try to submit
- **Expected Result**: Browser validation prevents submission OR server error
- **Status**: ⏳ Pending

---

### TS2.5: Form Submission

#### TC2.5.1 - Register Activity Successfully
- **Objective**: Save activity with all required fields
- **Steps**:
  1. Complete form:
     - Unidad: "CATI"
     - Actividad: "Encuestas Completadas"
     - Subactividad: "Encuestas Telefónicas"
     - Job Book: "JB001"
     - Cantidad: "10"
     - Fecha: Today
  2. Click "Guardar Registro"
- **Expected Result**: Success toast with ID, form clears, date resets to today
- **Status**: ⏳ Pending

#### TC2.5.2 - Form Clear Button
- **Objective**: Clear button resets form
- **Steps**:
  1. Fill form with data
  2. Click "Limpiar"
  3. Verify all fields reset
- **Expected Result**: Form cleared, date reset to today
- **Status**: ⏳ Pending

#### TC2.5.3 - Optional Fields (Hora, Observaciones)
- **Objective**: Register without optional fields
- **Steps**:
  1. Submit form without HoraInicio, HoraFin, Observaciones
  2. Verify saves successfully
- **Expected Result**: Successfully saved
- **Status**: ⏳ Pending

#### TC2.5.4 - Register with All Optional Fields
- **Objective**: Submit with all optional fields
- **Steps**:
  1. Fill form including:
     - Hora Inicio: "08:00"
     - Hora Fin: "12:00"
     - Observaciones: "Actividad completada exitosamente"
  2. Submit
- **Expected Result**: Successfully saved with all details
- **Status**: ⏳ Pending

---

### TS2.6: Mis Registros Tab

#### TC2.6.1 - Load My Registrations
- **Objective**: Tab displays user's saved registrations
- **Steps**:
  1. Click "Mis Registros" tab
  2. Wait for data load
  3. Verify table shows recent registrations
- **Expected Result**: Table populated with user's registrations
- **Status**: ⏳ Pending

#### TC2.6.2 - Mis Registros Empty State
- **Objective**: Show message when no registrations
- **Steps**:
  1. New user without registrations clicks "Mis Registros"
- **Expected Result**: Message: "No tienes registros guardados"
- **Status**: ⏳ Pending

#### TC2.6.3 - Registrations Display Correct Data
- **Objective**: Verify displayed data matches saved data
- **Steps**:
  1. Register activity with specific details
  2. Go to "Mis Registros"
  3. Verify row shows: Fecha, Unidad, Actividad, Cantidad, JobBook
- **Expected Result**: Data matches
- **Status**: ⏳ Pending

---

## TEST SUITE 3: Integration & Edge Cases

### TS3.1: Permission Controls

#### TC3.1.1 - Unauthorized User Cannot Access PMO Review
- **Objective**: Non-PMO user denied access to PMO reviewer
- **Steps**:
  1. Login as non-PMO user
  2. Try to navigate to `/OP/RevisionProductividadPMO`
- **Expected Result**: 403 Forbidden or redirected
- **Status**: ⏳ Pending

#### TC3.1.2 - Coordinator Can't Access PMO Review
- **Objective**: Verify role-based access control
- **Steps**:
  1. Login as Coordinador
  2. Try to access PMO endpoint directly
- **Expected Result**: Denied access
- **Status**: ⏳ Pending

---

### TS3.2: Performance

#### TC3.2.1 - Grid Handles 100+ Planillas
- **Objective**: UI remains responsive with large datasets
- **Steps**:
  1. Ensure trabajo has 100+ planillas
  2. Load planillas
  3. Verify grid scrolls smoothly
- **Expected Result**: Grid responsive, scroll smooth
- **Status**: ⏳ Pending

---

### TS3.3: Error Scenarios

#### TC3.3.1 - Network Error on Load
- **Objective**: Gracefully handle network errors
- **Steps**:
  1. Simulate network failure (DevTools)
  2. Try to load planillas
- **Expected Result**: Error toast displayed
- **Status**: ⏳ Pending

#### TC3.3.2 - Server Error Response
- **Objective**: Handle server errors gracefully
- **Steps**:
  1. Trigger scenario causing server error
  2. Verify error toast + logging
- **Expected Result**: User-friendly error message
- **Status**: ⏳ Pending

---

## Test Execution Summary Template

| Test Case | Status | Notes | Duration |
|-----------|--------|-------|----------|
| TC1.1.1   | ⏳     |       |          |
| TC1.1.2   | ⏳     |       |          |
| ...       | ...    | ...   | ...      |

---

## Sign-Off

**QA Lead**: _________________  
**Date**: _________________  
**Overall Status**: 🔴 Not Started / 🟡 In Progress / 🟢 Complete  

**Issues Found**: 
- [ ] No blockers
- [ ] Minor issues only
- [ ] Critical issues found

**Recommended Actions**: 
_____________________
