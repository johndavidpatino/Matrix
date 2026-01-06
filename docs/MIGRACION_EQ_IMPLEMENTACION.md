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
- Grid de mystery/shoppers y staff SL.
- Resumen con GM/PB/OP y costos por rubro.
- Admin: tablas maestras con formulario de upsert basico (precios, valor hora OPS, insumos, envios) y lectura del resto de seeds.

## 3. Datos y SP
- Script BD: `MatrixNext/EQ_SCHEMA.sql` (maestras, operacion, TVP, seeds completos).
- SP: `EQ_Quote_Save`, `EQ_Quote_Get`.

## 4. Backlog / TODO (estado)
- [x] Area EQ + rutas/menú.
- [x] Modelos y adapter Dapper/TVP.
- [x] Calculadora inicial real (campo, mystery, incentivos/reclutamiento, scripting/proc/DC/estadistica, staff SL/OPS, locaciones/envios/base de datos/viaticos/codificacion, GM/PB/OP).
- [x] UI EasyQuote con tabs y validaciones básicas.
- [x] Admin: vista Parametros + upsert para precios, valor hora OPS, insumos, envios (roles Admin/SuperAdmin).
- [ ] Refinar formulas exactas vs Excel: dias setup+campo, tipologia/envios volumetrico, base de datos real, viaticos con productividad, factor refrigeracion exacto, codificacion por preguntas/regs.
- [ ] Admin: CRUD completo para locaciones, mystery, codificacion, cost unitarios; import CSV.
- [ ] Tests minimos: baseline 400 Bogota F2F 20 min y variantes CATI/ONLINE/Mystery.
- [ ] Documentar mapping campo a campo y supuestos.

## 5. Notas de calculo recientes
- Locaciones: usa tarifa gross * dias_base (seed). Refrigeracion aplica factor 1.1 (ajustar cuando tengamos factor real).
- Envios: si envio_ciudades y peso >0, usa tipologia URBANO (1 ciudad) o NACIONAL (2+), suma seguro minimo; pendiente peso volumetrico.
- Base de datos: aplica costo unitario si se selecciona distinto a "No requiere".
- Viaticos: transportes PST encuestadores/supervisores * max dias_base de locaciones activas.
- Codificacion: si flag y preguntas abiertas >0, aplica valor Ipsos de primer escenario.
