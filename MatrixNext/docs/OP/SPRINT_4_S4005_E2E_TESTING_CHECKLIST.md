# Sprint 4 - S4-005: E2E Testing Checklist (Without Manual Execution)

**Status**: In Progress  
**Time Estimate**: 16h  
**Approach**: Document complete workflows and verification steps (manual testing deferred to migration end)  
**Created**: 2025-01-XX  

---

## Overview

This document defines the complete end-to-end workflows that must be tested during the migration cutover. Rather than executing manual tests now, we document the verification procedures and prepare automated test helpers for the actual migration phase.

**Important Note**: Actual manual execution is deferred to the end of the migration when data is ready. This document serves as:
1. **Test Plan**: All workflows and verification steps
2. **Execution Guide**: Steps to follow when manual testing begins
3. **Validation Checklist**: Sign-off criteria for each workflow
4. **Regression Prevention**: Baseline for comparison with existing system

---

## E2E Workflows

### Workflow 1: Trabajos → Ficha Cuantitativa → Estimación (CFT Creation)

**Objective**: Complete quantitative work setup workflow  
**Duration**: ~45 minutes  
**Actors**: Coordinador de Campo

#### Setup Requirements
- [ ] Work created in Portal with status "Activo"
- [ ] Budget defined (monto, actividades)
- [ ] Cities selected for execution
- [ ] Team assigned (coordinador, field staff)

#### Workflow Steps

1. **Go to Portal** → Search trabajo by ID
   - [ ] Trabajo appears in search results
   - [ ] Status displays correctly
   - [ ] Budget amount visible

2. **Create Ficha Cuantitativa** → Save with:
   - [ ] Work ID pre-populated
   - [ ] Budget auto-calculated from presupuesto
   - [ ] Percentages default to 25/50/75 split
   - Verify: System calculates total sample = sum(ciudades)

3. **Add Cities to Ficha**:
   - [ ] Select 3-5 cities from Divipola
   - [ ] Assign quantities (min 10, max 500 per city)
   - [ ] Dates default to presupuesto start/end
   - Verify: Total sample updates dynamically

4. **Create Estimación from Ficha**:
   - [ ] "Generar Estimación" button visible
   - [ ] New record appears in list
   - [ ] Status = "Abierta"
   - [ ] Dias Incluidos = presupuesto dias
   - Verify: All ciudades transferred with quantities

5. **Activate Estimación**:
   - [ ] "Activar" button available
   - [ ] Status changes to "Activa"
   - [ ] Cannot modify quantities after activation
   - [ ] Date range locked

#### Verification Checkpoints

| Step | Check | Expected | Pass |
|------|-------|----------|------|
| 1 | Portal search | Trabajo found | ☐ |
| 2 | Ficha save | ID generated | ☐ |
| 3 | Cities added | Count = selected | ☐ |
| 4 | Estimación created | Status = Abierta | ☐ |
| 5 | Activation | Status = Activa | ☐ |

#### Sign-Off Criteria
- [ ] All 5 verification checkpoints passed
- [ ] No validation errors during save
- [ ] No JavaScript console errors
- [ ] Load time < 3 seconds per page

---

### Workflow 2: Coordinador - Asignación de Personal y Seguimiento

**Objective**: Assign field teams and track productivity  
**Duration**: ~30 minutes  
**Actors**: Coordinador de Campo

#### Setup Requirements
- [ ] Active trabajo with estimación
- [ ] Personnel registered in system (minimum 2 field workers)
- [ ] Asignación template configured

#### Workflow Steps

1. **Go to Asignación Module**:
   - [ ] Dropdown filters work (estatus, ciudad, coordinador)
   - [ ] Search by trabajo ID
   - [ ] Results load in < 2 seconds

2. **Create Asignación**:
   - [ ] Select trabajo from list
   - [ ] Assign coordinador (auto-complete)
   - [ ] Select multiple field staff
   - [ ] Set dates and horarios
   - Verify: Total assigned ≤ muestra total

3. **Verify Coverage**:
   - [ ] "Ver Cobertura" button shows map
   - [ ] All assigned ciudades highlighted
   - [ ] Coverage % calculated correctly
   - Formula: (assigned / muestra_total) * 100

4. **Print Planilla**:
   - [ ] "Imprimir" generates PDF
   - [ ] Contains all assigned personnel
   - [ ] Dates and hours visible
   - [ ] QR code generates (if applicable)

5. **Track Asignación**:
   - [ ] Status updates available (En Proceso, Completada, Cancelada)
   - [ ] Edit allowed only in En Proceso
   - [ ] Audit trail logs all changes
   - [ ] Deleted records marked as Cancelada

#### Verification Checkpoints

| Step | Check | Expected | Pass |
|------|-------|----------|------|
| 1 | Module access | Filters work | ☐ |
| 2 | Create | Record saved | ☐ |
| 3 | Coverage | % calculated | ☐ |
| 4 | Print | PDF generated | ☐ |
| 5 | Track | Status updates | ☐ |

---

### Workflow 3: Call Center - Distribución y Recepción de Trabajos

**Objective**: Receive and distribute work assignments  
**Duration**: ~25 minutes  
**Actors**: Call Center Agent

#### Setup Requirements
- [ ] Active trabajos waiting for distribution
- [ ] Call center agent with proper roles
- [ ] Distribution rules configured

#### Workflow Steps

1. **View Pending Trabajos**:
   - [ ] List shows unassigned trabajos
   - [ ] Sort by: prioridad, fecha, ciudad
   - [ ] Quick view shows key metrics

2. **Assign to Coordinador**:
   - [ ] Select trabajo
   - [ ] Choose coordinador from active list
   - [ ] Add notes/comentarios
   - [ ] Status changes to "Asignado"
   - Verify: Notification sent to coordinador

3. **Track Distribution**:
   - [ ] "Enviados" list shows assigned trabajos
   - [ ] "No Enviados" shows pending
   - [ ] Filter by date range
   - [ ] Export to Excel with assignment date

4. **Receive Confirmations**:
   - [ ] Coordinador response appears in "Confirmados"
   - [ ] Response includes accepted/rejected status
   - [ ] Date/time of response recorded
   - [ ] Unconfirmed items flagged after 24h

#### Verification Checkpoints

| Step | Check | Expected | Pass |
|------|-------|----------|------|
| 1 | View pending | List displayed | ☐ |
| 2 | Assign | Status = Asignado | ☐ |
| 3 | Track | Filters work | ☐ |
| 4 | Receive | Confirmations appear | ☐ |

---

### Workflow 4: Tráfico - Movimiento de Información

**Objective**: Monitor and manage data traffic/movement  
**Duration**: ~20 minutes  
**Actors**: Tráfico Coordinator

#### Setup Requirements
- [ ] Active trabajos with assigned personal
- [ ] Data being uploaded from field
- [ ] Tráfico module configured

#### Workflow Steps

1. **View Traffic Dashboard**:
   - [ ] Total movimientos displayed
   - [ ] Recibidos count accurate
   - [ ] En Proceso count accurate
   - [ ] Rechazados count accurate
   - Formula: Total = Recibidos + En Proceso + Rechazados

2. **Filter by Work**:
   - [ ] Select trabajo from dropdown
   - [ ] Dashboard updates with trabajo-specific data
   - [ ] Percentages recalculate (rejection rate, completion %)
   - Verify: Rate = (Rechazados / Total) * 100

3. **Investigate Rejections**:
   - [ ] Click "Rechazados" to see details
   - [ ] Error messages displayed
   - [ ] Filter by error type
   - [ ] Bulk reprocess option available

4. **Monitor In-Process**:
   - [ ] "En Proceso" shows stuck items
   - [ ] Age of records displayed
   - [ ] Alert if > 24 hours old
   - [ ] Manual retry available

5. **Generate Report**:
   - [ ] Export traffic summary to Excel
   - [ ] Include rejection details
   - [ ] Date range selection
   - [ ] Charts for visualization

#### Verification Checkpoints

| Step | Check | Expected | Pass |
|------|-------|----------|------|
| 1 | Dashboard | Counts correct | ☐ |
| 2 | Filter | Data updates | ☐ |
| 3 | Rejections | Details visible | ☐ |
| 4 | In-Process | Age tracked | ☐ |
| 5 | Report | Export works | ☐ |

---

### Workflow 5: Revisión de Productividad y Registros

**Objective**: Review productivity and validate field records  
**Duration**: ~35 minutes  
**Actors**: Revisor de Calidad

#### Setup Requirements
- [ ] Work with completed asignaciones
- [ ] Field data uploaded and processed
- [ ] Revisión rules configured

#### Workflow Steps

1. **Access Revisión Module**:
   - [ ] List shows trabajos ready for review
   - [ ] Filter by: estatus, coordinador, fecha
   - [ ] Search by trabajo ID
   - [ ] Sort by: prioridad, fecha, estado

2. **Start Productividad Revisión**:
   - [ ] Open trabajo record
   - [ ] View all registros (records) for review
   - [ ] Details: ciudad, fecha, cantidad, calidad
   - [ ] Pre-filled checklist items

3. **Validate Individual Records**:
   - [ ] Mark records as: Válido, Cuestionable, Rechazado
   - [ ] Add comments for rejected records
   - [ ] Photo/document review (if applicable)
   - [ ] Cross-check with planilla data

4. **Review Summary Statistics**:
   - [ ] Total registros count
   - [ ] % Válidos = (válidos / total) * 100
   - [ ] % Cuestionables displayed
   - [ ] % Rechazados displayed
   - Verify: Sum = 100%

5. **Save and Escalate**:
   - [ ] Mark revisión as "Completada"
   - [ ] Generate report
   - [ ] Flag for correction if % Rechazados > 5%
   - [ ] Notify coordinador of issues
   - Verify: Audit log records completion

6. **Revisit Corrected Records**:
   - [ ] Coordinador updates rejected records
   - [ ] Revisor re-validates (optional workflow)
   - [ ] Final status = "Aceptado" or "Rechazado"

#### Verification Checkpoints

| Step | Check | Expected | Pass |
|------|-------|----------|------|
| 1 | Access | List loads | ☐ |
| 2 | Open | Details visible | ☐ |
| 3 | Validate | Marks save | ☐ |
| 4 | Summary | % correct | ☐ |
| 5 | Save | Log recorded | ☐ |
| 6 | Re-review | Updates reflected | ☐ |

---

### Workflow 6: IPS - Generación de Exportaciones y Auditoría

**Objective**: Generate IPS exports with audit compliance  
**Duration**: ~25 minutes  
**Actors**: IPS Coordinator

#### Setup Requirements
- [ ] Approved trabajos with completed revisiones
- [ ] Exportables data validated
- [ ] IPS format requirements defined

#### Workflow Steps

1. **Access IPS Module**:
   - [ ] List shows trabajo ready for export
   - [ ] Filter by: estatus, periodo, tipo
   - [ ] Search by trabajo ID

2. **Select Datos for Export**:
   - [ ] Choose trabajos (single or batch)
   - [ ] Verify all required fields present
   - [ ] Data validation runs automatically
   - Verify: No errors before export allowed

3. **Generate Excel Export**:
   - [ ] Click "Exportar Revisiones"
   - [ ] File generates with naming: `OP_Export_[YYYYMMDD_HHMMSS].xlsx`
   - [ ] Contains all selected records
   - [ ] Formatting matches IPS requirements

4. **Verify Export Contents**:
   - [ ] Open exported file
   - [ ] Row count matches selection
   - [ ] All required columns present
   - [ ] No truncated data
   - [ ] Formulas not included (values only)

5. **Audit Trail Verification**:
   - [ ] Export registered in OP_ExportesAuditoria table
   - [ ] User, timestamp, file size recorded
   - [ ] File marked as "Exitoso"
   - Query verification:
     ```sql
     SELECT * FROM OP_ExportesAuditoria 
     WHERE TrabajoId = @trabajoId 
     ORDER BY FechaExportacion DESC
     ```

6. **Cleanup Verification**:
   - [ ] Old exports (> 30 days) scheduled for deletion
   - [ ] Cleanup service runs hourly (verify logs)
   - [ ] Files deleted but audit records retained
   - [ ] No sensitive data in file system

#### Verification Checkpoints

| Step | Check | Expected | Pass |
|------|-------|----------|------|
| 1 | Access | List loads | ☐ |
| 2 | Validate | No errors | ☐ |
| 3 | Export | File created | ☐ |
| 4 | Verify | Row count OK | ☐ |
| 5 | Audit | Entry recorded | ☐ |
| 6 | Cleanup | Old files deleted | ☐ |

---

## System-Wide Verification

### Performance Baselines
- [ ] Page loads: < 3 seconds
- [ ] Searches: < 2 seconds for 10,000+ records
- [ ] Exports: < 30 seconds for 1000+ rows
- [ ] Report generation: < 10 seconds
- [ ] Concurrent users: 50+ without degradation

### Data Integrity
- [ ] Referential integrity maintained (FK checks)
- [ ] No orphaned records after deletions
- [ ] Audit logs complete and immutable
- [ ] Transaction rollbacks work correctly
- [ ] Duplicate key prevention

### Error Handling
- [ ] Invalid input rejected gracefully
- [ ] Error messages user-friendly (Spanish)
- [ ] No stack traces in UI
- [ ] Validation errors clear and actionable
- [ ] Network timeouts handled

### Browser Compatibility
- [ ] Chrome (latest 2 versions)
- [ ] Firefox (latest 2 versions)
- [ ] Edge (latest 2 versions)
- [ ] Mobile browsers (iOS Safari, Chrome Mobile)

---

## Testing Execution Plan

### Phase 1: Pre-Migration Setup (T-2 Weeks)
- [ ] Test environment loaded with production-like data
- [ ] User accounts created (5 per role type)
- [ ] Test data sets prepared (see below)
- [ ] Success criteria documented
- [ ] Sign-off from business users

### Phase 2: User Acceptance Testing (T-1 Week)
- [ ] Run all 6 workflows with 2-3 iterations each
- [ ] Document any issues (defects vs. design decisions)
- [ ] Verify all checkpoints
- [ ] Performance testing on peak load
- [ ] Regression testing vs. legacy system

### Phase 3: Cutover Execution (T-0)
- [ ] Run workflows in parallel with legacy system
- [ ] Validate data consistency
- [ ] Confirm audit trails
- [ ] Final sign-off from stakeholders

### Phase 4: Post-Cutover Validation (T+1 Week)
- [ ] Monitor live data for 5 business days
- [ ] Spot-check 20 random trabajos
- [ ] Verify cleanup processes (exports, old data)
- [ ] Performance monitoring
- [ ] User feedback collection

---

## Test Data Sets

### Minimum Required Test Data

1. **Trabajos**
   - Type A: Small work (1 city, 50 sample)
   - Type B: Medium work (3 cities, 150 sample)
   - Type C: Large work (8 cities, 500 sample)
   - Count: Minimum 5 of each type

2. **Personnel**
   - 1 Call Center Agent
   - 2 Coordinadores de Campo
   - 5 Field Workers
   - 2 Traffic Coordinators
   - 2 Quality Reviewers
   - 1 IPS Coordinator

3. **Supporting Data**
   - Cities: Bogotá, Medellín, Cali (main hubs)
   - Activities: 5-10 standard activities
   - Unidades: 3-5 unit types
   - Status codes: All standard statuses

---

## Known Limitations & Deferrals

| Item | Status | Plan |
|------|--------|------|
| Offline mode | Not for Sprint 4 | Post-migration enhancement |
| Mobile app | Not for Sprint 4 | Q2 2025 feature |
| Advanced analytics | Not for Sprint 4 | Dashboard enhancements |
| Automatic backups | Manual for now | Implement T+1 week |
| Multi-language | Spanish only | English post-migration |

---

## Sign-Off & Approval

### Prepared By
- [ ] Development Team (Date: _________)

### Reviewed By  
- [ ] QA Lead (Date: _________)
- [ ] Product Owner (Date: _________)

### Approved For Execution By
- [ ] Business Sponsor (Date: _________)

### Test Execution Results
- Date: _________
- Executed By: _________________
- Issues Found: _____ (Critical: ___ | Major: ___ | Minor: ___)
- All Checkpoints Passed: ☐ Yes / ☐ No (If No, list exceptions below)

**Exceptions**:
```
1. 
2. 
3. 
```

**Sign-Off**: _________________________ (Business Sponsor)

---

## Appendix: Helper Commands

### Database Verification Queries
```sql
-- Verify audit trail for exports
SELECT TOP 10 * FROM OP_ExportesAuditoria 
WHERE Exitoso = 1 
ORDER BY FechaExportacion DESC;

-- Check for orphaned records
SELECT * FROM OpEstimaciones e
WHERE NOT EXISTS (SELECT 1 FROM PYTrabajos t WHERE t.Id = e.TrabajoId);

-- Verify movement counts
SELECT TrabajoId, COUNT(*) as Total 
FROM OpTrafico 
GROUP BY TrabajoId;
```

### Log File Locations
- Application Logs: `/logs/MatrixNext-{date}.log`
- SQL Audit Logs: SQL Server Event Log
- Background Service Logs: Application Insights

### Rollback Procedures
- [ ] Database backup location: ________
- [ ] Backup frequency: Every 4 hours
- [ ] Recovery time objective: < 15 minutes
- [ ] Recovery point objective: < 30 minutes

---

**Document Version**: 1.0  
**Last Updated**: 2025-01-XX  
**Next Review**: Post-migration week 1
