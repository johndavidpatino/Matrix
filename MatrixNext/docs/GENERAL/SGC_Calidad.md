# SGC_Calidad - Estado de migración

**Contexto**: módulo ubicado en `WebMatrix/SGC_Calidad/` responsable por control de calidad, registros de auditoría y reportes internos. A la fecha no se ha iniciado una migración formal en MatrixNext pero existe evidencia en métricas, gaps y backlog.

## Estado actual
- **Código legacy**: presente en WebForms (SGC_Calidad.aspx + code-behind).
- **Documentación**: se han registrado hallazgos en `MODULOS_MIGRACION.md` (prioridad baja) y en los backlog generales de Gaps/Sprints.
- **Plan**: priorizado en baja prioridad, se ejecutará en fases posteriores cuando los módulos críticos estén estabilizados.

## Próximos pasos sugeridos
1. Recopilar listados de pantallas actuales/recursos (carpetas JS, assets, plantillas).
2. Mapear dependencias hacia CORE, CU y OP (documentar en este archivo).
3. Planificar un sprint (estimado ~2-3 semanas) con pruebas unitarias + validaciones de seguridad.

## Referencias
- `MatrixNext/MODULOS_MIGRACION.md` – roadmap general de módulos migrados.
- `WebMatrix/SGC_Calidad/` – código original en WebMatrix.
