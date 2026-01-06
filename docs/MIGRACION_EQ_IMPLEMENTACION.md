# MIGRACION EASYQUOTE A MATRIXNEXT (AREA EQ)

**Objetivo**: Implementar el modulo EasyQuote con UX de grillas editables, calculo 1:1 con el Excel y administracion de parametros. Este documento es el plan operativo y backlog hasta completar 100%.

## 1. Estructura de area (EQ)
- `Areas/EQ/Controllers/EasyQuoteController.cs` (captura/calculo)
- `Areas/EQ/Controllers/EasyQuoteAdminController.cs` (parametros/seeds UI)
- `Areas/EQ/Views/EasyQuote/` (`Index` con tabs/grids)
- `Areas/EQ/Views/EasyQuoteAdmin/` (`Parametros.cshtml` para tablas maestras)
- `Areas/EQ/Models/` (ViewModels/DTOs segun analisis)
- `Areas/EQ/Services/` (`EasyQuoteService`, `EasyQuoteAdminService`)
- `Areas/EQ/Services/Internal/` (`EasyQuoteAdapter`, `QuoteCalculator`)
- `Areas/EQ/Services/Masters/` (`EasyQuoteMasterService` cache maestros)

## 2. UX clave (grids editables)
- Tabs: Datos, Cuestionario, Muestra, Mystery, Staff, Resumen.
- Grid de muestra por ciudad/NSE con validacion de sumas.
- Grid de mystery/shoppers y staff SL con add-row.
- Resumen con GM/PB/OP y costos por rubro.
- Admin: tablas maestras con formulario de upsert basico (precios, valor hora OPS, insumos, envios) y lectura del resto de seeds.

## 3. Datos y SP
- Script BD: `MatrixNext/EQ_SCHEMA.sql` (maestras, operacion, TVP, seeds completos).
- SP: `EQ_Quote_Save`, `EQ_Quote_Get`.

## 4. Backlog / TODO (estado)
- [x] Area EQ + rutas/menu.
- [x] Modelos y adapter Dapper/TVP.
- [x] Calculadora inicial real (campo, mystery, incentivos/reclutamiento, scripting/proc/DC/estadistica, staff SL/OPS, locaciones/envios/base de datos/viaticos/codificacion, GM/PB/OP).
- [x] UI EasyQuote con tabs/grids JS (datos, cuestionario, muestra, mystery, staff, resumen) con calcular/guardar y add-row inline.
- [x] Admin: vista Parametros + upsert para precios, valor hora OPS, insumos, envios (roles Admin/SuperAdmin). Parametrizado misc/envio (divisor volumetrico, tipologias), productividad por ciudad y base de datos.
- [ ] Refinar formulas exactas vs Excel: dias setup+campo, tipologia/envios volumetrico con dimensiones, base de datos real, viaticos con productividad diferenciada, factor refrigeracion exacto, codificacion por preguntas/regs.
- [ ] Admin: CRUD completo para locaciones, mystery, codificacion, cost unitarios; import CSV.
- [ ] Tests minimos: baseline 400 Bogota F2F 20 min y variantes CATI/ONLINE/Mystery.
- [ ] Documentar mapping campo a campo y supuestos.

## 5. Notas de calculo recientes
- Locaciones: usa tarifa gross * dias_base (seed). Refrigeracion aplica `eq_param_misc.FACTOR_REFRIGERACION` y costo nevera (placeholder 1.15 y 970000).
- Envios: si envio_ciudades y peso >0, usa tipologia URBANO (1 ciudad) o NACIONAL (2+) desde `eq_envio_param`, divisor volumetrico seed (5000). Falta integrar dimensiones y tabla de tipologias especificas.
- Base de datos: costo lee `eq_cost_base_datos` (No requiere/Cliente/Comprar) seeds placeholder.
- Productividad: dias de campo calculados con `eq_productividad_ciudad` (encuestadores/productividad por ciudad).
- Viaticos: transportes PST encuestadores/supervisores * dias de campo calculados.
- Codificacion: si flag y preguntas abiertas >0, aplica `eq_codificacion_param` primer escenario; pendiente calibrar por #regs/#preguntas reales.

