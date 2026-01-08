# BACKLOG Y REGISTRO DE AVANCES - MÓDULO OP_CUALITATIVO

**Fecha de inicio**: 09 de enero de 2026  
**Responsable**: Equipo OP  
**Versión**: 1.0  
**Basado en**: [ANALISIS_OP_CUALITATIVO.md](ANALISIS_OP_CUALITATIVO.md)

---

## 1️⃣ Resumen ejecutivo

**Objetivo**: trasladar los 19 WebForms del alcance (sección 2 del análisis) a ASP.NET Core dentro de `Areas/OP`, manteniendo paridad 1:1 en flujos de COE, filtros, fichas, programación e IPS.  
**Prioridades**:
- **P0**: implementar los controladores/servicios para Trabajos, Campo, Filtros, Fichas y Aprobaciones con SP legados.
- **P1**: programaciones (Programacion Campo, IPS y planillas) con exportaciones/validaciones y componentes compartidos.
- **P2**: refuerzo de testing, documentación, optimizaciones y reporting (emails, queue, dashboards).

---

## 2️⃣ Tareas e hitos

| ID | Descripción | Prioridad | Estado | Referencia | Estimación |
|----|-------------|-----------|--------|------------|------------|
| OP-C01 | `TrabajosController` + `TrabajoCualitativoService` + vistas Index/partials (grid, filtros, botones hacia fichas y muestra). | P0 | Pendiente | `ANALISIS_OP_CUALITATIVO.md` secciones 2‑4 | 14h |
| OP-C02 | `CampoController` y `CampoCualitativoService` con exportación ICS/Excel y navegación a `GD_Documentos`. | P0 | Pendiente | `CampoCualitativo.aspx.vb` (export, btnDocumentos) | 10h |
| OP-F01 | `FiltrosController.Configurar` + `FiltroConfigVm` + `CampoCualitativoAdapter` para nuevos filtros dinámicos (preguntas, tipofiltro). | P0 | Pendiente | `DisenarFiltros.aspx.vb` (guardar filtros). | 8h |
| OP-F02 | `FiltrosController.Aprobar` / `FiltrosController.AprobarAsistencia` + SP `REP_OP_Respuestas_Filtro` + lógica de logs `OP_LogRespuestas_Filtro`. | P0 | Pendiente | `AprobacionesFiltros.aspx.vb`, `AprobacionesFiltrosAsitencia.aspx.vb`. | 10h |
| OP-F03 | `FichasController` para entrevista, sesión y observación + `FichaParametrosVm` y `IEmailService` (envíos). | P0 | Pendiente | `FichaEntrevista.aspx.vb`, `FichaSesion.aspx.vb`, `FichaObservacion.aspx.vb`. | 16h |
| OP-P01 | `ProgramacionController` con cronograma, estados, `ProgramacionCampoVm` y exportaciones (ClosedXML). | P1 | Pendiente | `ProgramacionCampo.aspx.vb` (ClosedXML, estados enumerados). | 12h |
| OP-I01 | `IpsController` con grid editable, notificaciones y `SqlDataSource` a `OP_IPS_Procesos`. | P1 | Pendiente | `IPSCuali.aspx.vb`. | 10h |
| OP-L01 | `PlanillasController` (API + JS) reutilizando `/Scripts/js/Pages/OP_Cualitativo/AdministracionRegistroPlanillas`. | P1 | Pendiente | `AdministracionRegistroPlanillas.aspx` y su JS. | 12h |
| OP-T01 | Testing/documentación fina (backlog/checklist) y actualización del dashboard. | P2 | Pendiente | Secciones 8‑12 del análisis. | 6h |

---

## 3️⃣ Sprint planning provisional

**Sprint 1 (Semana 1)**  
- Configurar `Areas/OP` y registrar servicios en `Program.cs` (`TrabajoCualitativoService`, `CampoCualitativoService`, `FiltrosService`).  
- Implementar `TrabajosController` y `CampoController` con datos mock y rutas; validar SP `REP_OP_Respuestas_Filtro`.  
- Generar vistas básicas (grid de COE, accordion de workflows, botón a `GD_Documentos`).  

**Sprint 2 (Semana 2)**  
- Finalizar `FiltrosController` + `Aprobaciones` con grid y export Excel.  
- Implementar `FichasController` (Entrevista/Observación/Transcripción) y enviar correos de entrega.  
- Desplegar `ProgramacionController` y `IpsController` con sus reglas de negocio.  

**Sprint 3 (Semana 3)**  
- Desarrollar API/JS para planillas (módulo ya existente).  
- Ejecutar pruebas manuales end-to-end del flujo COE → filtros → fichas/filtros.  
- Documentar backlog y actualizar `DASHBOARD_MIGRACION` con estado “en ejecución”.  

**Sprint 4 (Semana 4)**  
- Optimizar performance (caching, índices, export tracking).  
- Integrar pruebas E2E final, queue/email si aplica y checklist de salida.  
- Revisar backlog y marcar módulo como “ready” después de QA.  

---

## 4️⃣ Referencias clave y dependencias

- `ANALISIS_OP_CUALITATIVO.md`: evidencia de cada WebForm, riesgos y decisiones.  
- `DIRECTRICES_MIGRACION.md`: reglas 1‑15 (nombres BD, áreas, modales, SP).  
- `Program.cs`: registrar servicios/adapters y aplicar filters/authorization.  
- `GD_Documentos`, `PY_Proyectos`, `WorkFlow`: botones en WebForms proporcionan las redirecciones necesarias.  
- `CoreProject` (OP Entities, CoordinacionCampo, SegmentosCuali, WorkFlow, etc.) ya contiene SP consultados por los nuevos adapters.  

---

## 5️⃣ Próximos pasos inmediatos

1. Validar SP y tablas listadas en la sección 5 del análisis para `CampoCualitativo` y filtros.  
2. Diseñar `ViewModels` base (`TrabajoCualitativoIndexVm`, `FiltroConfigVm`, `FichaEntrevistaVm`, `ProgramacionCampoVm`, `IpsRevisionVm`).  
3. Implementar skeleton controllers para `Trabajos`, `Campo` y `Filtros` y verificar navegación (sin `Session`).  
4. Reunión con stakeholders para priorizar si hay decisiones adicionales (modales, exportaciones, queue).  
5. Actualizar `DASHBOARD_MIGRACION.md` con este backlog y su estado (en análisis → en ejecución).  
