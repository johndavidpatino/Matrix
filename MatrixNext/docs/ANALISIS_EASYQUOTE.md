# ANALISIS EASYQUOTE - MIGRACION A MATRIXNET

**Documento tecnico**  
**Version**: 1.0  
**Fecha**: 2026-01-05  
**Fuente**: `Ipsos EasyQuote 2025v2.xlsm`  
**Objetivo**: inventario completo de entradas, parametros y calculos para migrar el cotizador a un formulario MVC en MatrixNet asegurando paridad 1:1.

---

## Indice
1. Resumen ejecutivo
2. Inventario de hojas del libro
3. Entradas de usuario (hoja `Entradas`)
4. Tablas y parametros de soporte
5. Motor de calculo (`Costos Directos discriminados`)
6. Sugerencia de modelo de datos MVC
7. Reglas/validaciones implicitas
8. Checklist de desarrollo y QA
9. Flujo MatrixNext (UI/API) y archivos sembrados
10. Casos de prueba y reconciliacion
11. Pendientes y riesgos

---

## 1. Resumen ejecutivo
- EasyQuote calcula costos operativos, staff y margenes para estudios de investigacion (F2F, CATI, online, autoaplicado, mystery/shopper).
- La hoja `Entradas` concentra toda la captura; la hoja `Costos Directos discriminados` es el motor de formulas y consolida costos sin/ con gross.
- Parametros y tarifas estan distribuidos en tablas auxiliares (`Parametros`, `Precios bases`, `Tarifario Estadistica*`, `Horas`, `Valores Insumos reclutamiento`, `Valor Hora - Alternativas`, `Valor por peso`, `Estructura de costos - 2023`, fichas pais).
- Resultado clave: costos por rubro (campo+calidad+viaticos, TDD, incentivos, estadistica), costos directos OPS, PB+RMF, GM, OP y AOT por unidad y totales.

## 2. Inventario de hojas del libro
- `Entradas`: formulario principal con datos generales, parametros de recoleccion, definicion de muestra por ciudades/NSE, flags de procesos y outputs resumen.
- `Costos Directos discriminados`: calculadora central. Toma entradas, aplica tarifas y parametrizaciones, calcula rubros, margenes y totales.
- `Parametros`: matrices de precio por duracion/incidencia y tipo de metodologia (F2F/CATI/ONLINE/auto), switches Si/No, factores y codigos de metodologia. Incluye horas por proceso (Script, Procesamiento, Harmoni, Graficacion) segun minutos.
- `Precios bases`: version base de la matriz de precios por minutos vs penetracion.
- `Tarifario Estadistica` y `Tarifario Estadistica2`: ratecard de servicios estadisticos (descripcion, horas estimadas, precio referencia 2024, factor de escala, lead time) usado para costeo de analitica adicional.
- `MuestraTec1` / `MuestraTec2`: distribucion poblacional por NSE y ciudad, fracciones por NSE y planilla para capturar muestra tecnica 1 y 2 (400 casos de ejemplo en #1).
- `Valores Insumos reclutamiento`: costos por NSE (reclutamiento/obsequios), productividad, dias, locaciones, insumos de prueba, transporte, telefonia, scripting/procesamiento/toplines, tarifas unitarias y seccion de logistica de envios (kilos).
- `MYSTERY`: configuracion de tipos de visita (1-3), numero de olas, costos unitarios y plantilla de costeo (coord campo, asistencias, critica, desplazamientos, bonos).
- `Horas`: tabla de horas estimadas por nivel (L3-L7) segun SL/record detail/metodologia; incluye loaded/billing rates por nivel.
- `Valor Hora - Alternativas`: valor hora OPS para 2022/2023 (alternativas) y ratecard de niveles 1-8 con fechas de vigencia.
- `Estructura de costos - 2023`: costo hora OPS por combinacion de L7/L4 y factor de cobertura para productos estadisticos especificos.
- `Valor por peso`: tabla de tarifas de envio (urbano/nacional/reexpediciones), porcentaje de seguro y simulador por peso/volumen.
- `Codificacion`: parametros para costo de codificacion por numero de preguntas abiertas, registros, dias, horas y totales.
- `Bitacora de ajustes`: backlog historico de cambios (realizado/pendiente) para considerar en migracion.
- Hojas pais (`Costa Rica`, `Guatemala`, `Rep. Dominicana`, `Puerto Rico`, `Panama`, `El Salvador`, `Nicaragua`, `Honduras`): perfiles socioeconomicos (participacion NSE, ingresos, ejemplos de colonias) para posibles ajustes de muestra.

## 3. Entradas de usuario (hoja `Entradas`)
### 3.1 Datos generales
- Nombre Propuesta/# (`C4`), Grupo Objetivo (`C5`), Cliente (`C6`).
- Fechas: aprobacion estimada (`C7`), campo (`C8`).
- Probabilidad de aprobacion (`C9`: Alta/Media/Baja; mapeo en `Parametros!207:210`).
- SL (`C10`), Metodologia SL (`C11`), Record Detail (`C12`).
- Categoria/Producto (`C13`), valor proveedor externo (`D14`), internacional (`D15`).
- Valor GMU (`C34`) para markups internacionales.

### 3.2 Parametros de recoleccion y cuestionario
- Metodologia de recoleccion (`C16`, ej. Hogares) y tecnica(s) de recoleccion 1-3 (`C17:C18` con tipos F2F/CATI/ONLINE/AUTOAP/MYSTERY/SHOPPER codificados en `Parametros!230:239`).
- Base de datos requerida (`C19` con opciones No requiere/Comprar/Cliente).
- Penetracion/incidencia (`C20`, con label y valor).
- Duracion cuestionario en minutos (`C21`), numero de preguntas abiertas (`C22`), abiertas multiples (`C23`), otros (`C24`).
- Top line (`C25` Si/No), Datacleaning (`C26` total/parcial), conversiÃ³n/generacion ASCII (`C27`), script reclutamiento (`C28`), scripting (`C29` con tipo Nuevo/Duplicado/Reutilizacion), codificacion (`C30`), procesamiento (`C31`) y # procesamientos (`C32`), proceso estadistico (`C33` Si/No).
- Dias de set up (`C35`) y dias de campo (`C36`), numero de olas/rondas (`C43`), sobre-muestra (%) (`C46`).
- Flags adicionales: Harmoni (`C37`), Graficacion (`C38`), Otros costos (`C39`), Viaticos campo (`C40`), Otros incentivos (`C41`), Reprografia (`C42`).

### 3.3 Logistica y muestra
- Envio a ciudades (`C44` Si/No) y peso del producto para traslado (`C45` en gramos, usado para calculo de flete).
- Ciudades y distribucion de muestra (`C47:C55`): lista Bogota, B/quilla, M/llin, Cali, B/manga, C/gena, Otras; con bandera de inclusion (col B), total muestra (col D) y desagregacion por NSE 1-6 (cols E-J). Ejemplo cargado: 400 casos en Bogota distribuidos por NSE via `MuestraTec1`.
- Parametros de reclutamiento/incentivos: valor reclutamiento (`C56` con default por NSE en `Valores Insumos reclutamiento`), valor obsequio (`C57`), estudio con ninos (`C58`), taxi participantes (`C59`).
- Clase de prueba (`C60` Monodica/Monodica secuencial/etc. define multiplicadores), refrigeracion anticipada (`C61`), compra de producto (`C62`), etiquetado/blind (`C63`), embalaje (`C64`), # productos a testear (`C65`), productos por respondiente (`C66`), # patinadores por ciudad (`C67`), siembra y recoleccion (`C68`), apoyo reclutamiento en sitio (`C69` con niveles en `Parametros!214:217`).
- Mystery/Shopper: tabla en filas 71-75 para hasta 3 tipos de visita con # de olas y costos de desplazamiento/edicion/otros.

### 3.4 Staff OPS / SL
- Tabla "Metodologias vs horas por cargo SL" (`C89:F95`):
  - L4-L7: horas minimas via `VLOOKUP` a `Horas` (key = SL + record detail + metodologia SL).
  - Horas presupuestadas (col E) editable, valor total = horas * tarifa nivel (`Horas!J6:J9`).
  - Total ajustado dividendo (1-26.84%) para gross interno.
- Tabla resumen de costos (`C98:G106`): trae valores desde `Costos Directos discriminados` (campo+calidad+viaticos, TDD, incentivos, estadistica, staff OPS) y calcula AOT, GM, PB+RMF, OP y %OP.

### 3.5 Diccionario de datos (campos de captura †’ API/UI sugerido)
Formato: campo (tipo sugerido) [default] €” validacion / dependencia / origen.
- propuesta_nombre (string) €” obligatorio.
- grupo_objetivo (string) €” obligatorio.
- cliente (string) €” obligatorio.
- fecha_aprobacion_estimada (date) €” opcional.
- fecha_campo (date) €” opcional.
- prob_aprobacion (enum: Alta/Media/Baja, default Alta) €” valida en `Parametros!207:210`.
- sl (string corto) €” obligatorio; map a `Horas.KEY` prefijo.
- metodologia_sl (string) €” obligatorio; parte de `Horas.KEY`.
- record_detail (string) €” obligatorio; parte de `Horas.KEY`.
- categoria_producto (enum: Otro, Bebidas, etc.) €” usado solo para notas de bitacora (vasos Murano en Bebidas).
- valor_proveedor_externo (decimal) €” para campo externo F2F; si >0 activa rubro `D70`.
- valor_proveedor_internacional (decimal) €” para campo internacional; si >0 activa `D69`.
- metodologia_recoleccion (enum) €” debe mapear a `Parametros!230:239`.
- tecnica1_tipo / tecnica2_tipo / tecnica3_tipo (enum) €” mismos codigos; tecnica1_flag bool indica seleccion.
- base_datos (enum: No requiere/Comprar/Cliente) €” valida contra `Parametros!204:205`.
- penetracion_label (string) + penetracion_valor (decimal) €” usar label para UI, valor para matrices.
- duracion_minutos (int) €” 5..60; mapea a matriz precios y horas de script/procesamiento.
- preguntas_abiertas (int) €” default 0; impacta codificacion (tabla `Codificacion`).
- preguntas_abiertas_mult (int) €” default 0; idem.
- otros_procesos (string) €” solo nota.
- top_line (bool, default No).
- datacleaning (enum: Total/Parcial/No) €” default Total cuando flag Si.
- ascii (bool) €” conversiÃ³n/generacion ASCII.
- script_reclutamiento (bool) €” activa `D123`.
- scripting (bool) + tipo_script (enum: Nuevo/Duplicado/Reutilizacion) €” activa `D122`, multiplicador `Parametros!180:182`.
- codificacion (bool).
- procesamiento (bool).
- num_procesamientos (int, default 1).
- proceso_estadistico (bool) €” Si activa `D121` usando `Entradas!D99`.
- valor_gmu (decimal) €” GMU adicional para internacional.
- dias_setup (int, default 2) €” `C35`.
- dias_campo (int, default calculado `ROUNDUP(C58)`).
- num_olas (int, default 1).
- harmoni (bool), graficacion (bool), otros_costos (decimal opc), viaticos_campo (decimal opc), otros_incentivos (decimal opc), reprografia_paginas (int opc).
- envio_ciudades (bool), peso_producto_gramos (decimal), sobre_muestra_pct (decimal).
- ciudades[]: {nombre, activa bool, muestra_total, nse1..nse6} €” validar suma NSE = muestra_total; suma ciudades = muestra_total global.
- reclutamiento_tipo (enum/valor) e incentivos/obsequios por NSE (sobrescriben `Valores Insumos reclutamiento`).
- estudio_ninos (bool), taxi_participantes (bool), clase_prueba (enum), refrigeracion (bool), compra_producto (decimal), etiquetado_tipo (enum Parametros!226:229), embalaje (bool), productos_testear (int), productos_por_resp (int), patinadores_por_ciudad (int), siembra (bool), apoyo_reclutamiento_sitio (enum Parametros!214:217).
- mystery_visitas[]: {tipo_visita 1-3, complejidad, num_olas, desplazamientos, tanques, alertas, edicion, alquiler, compra_dispositivos}.
- staff_sl[]: {nivel L4-L7, horas_presup} €” horas minimas lookup en `Horas`.

## 4. Tablas y parametros de soporte
- `Parametros`:
  - Matriz de precio base por duracion (5-60 min) y penetracion (Mas82, 75-82, 67-74, 55-66, 46-54, 37-45) para F2F (`B4:AI12`).
  - Factores/labels de penetracion (`B24:B29`) y mapping Si/No (`B219:C220`), reclutamiento (`B214:C217`), etiquetado/empacado/blind (`B226:D229`), metodologias/codigos (`B230:F239`).
  - Horas estandar para Script/Procesamiento/Harmoni/Graficacion por duracion (`B171:E179`) y multiplicadores por tipo de script (Nuevo=1, Duplicado=4, Reutilizacion=2 en `B180:B182`).
  - Probabilidad aprobacion (Alta/Media/Baja) y clase de prueba (Monodica/Monodica secuencial/No aplica).
- `Precios bases`: misma estructura de matriz que `Parametros` pero con valores base (sin overhead).
- `Horas`: tabla con KEY = "<SL> | <RecordDetail> | <Metodologia SL>" y horas minimas sugeridas L3-L7, mas loaded/billing rate por nivel (col I-L).
- `Valor Hora - Alternativas`: valores hora OPS alternativos por nivel (2022, 2023 alt1/alt2) y ratecard niveles 1-8 con vigencia.
- `Tarifario Estadistica*` y `Estructura de costos - 2023`: catalogo de servicios de analitica (anexo de horas, factor de escala, precio de referencia, lead time).
- `Valores Insumos reclutamiento`: costos por NSE (reclutamiento/obsequios), productividad diaria, dias, supervisores/logistica, insumos (agua, vasos, galletas, etc.), locaciones por ciudad, costos unitarios de campo, scripting, datacleaning, procesamiento, envios (kilos) y tarifas de telefonia/bonos.
- `Valor por peso`: tarifas de transporte (urbano/nacional/reexpedicion) y seguro (1% valor declarado, minimo 25.000), simulador de cobro por peso volumetrico.
- `Codificacion`: parametros para calcular costo segun registros, numero de preguntas abiertas/abiertas multiples, dias y horas; incluye total dias/horas y valores (Val. Ipsos).
- `MYSTERY`: costos unitarios y totales por visita y segmento, staff de campo y critica, desplazamientos, fotocopias, alquiler equipos, compra de dispositivos y seguimiento.
- `Bitacora de ajustes`: referencias de cambios a mantener (ej. cotizar vasos Murano en bebidas, ajuste transporte ninos 15.000, transporte bebidas alcoholicas 28.000, sumar script de reclutamiento, refrigeracion, pilotos pendientes).
- Nuevas tablas semilla en MatrixNext: `eq_productividad_ciudad` (encuestadores/productividad por ciudad), `eq_param_misc` (factor refrigeracion, costo nevera, divisor volumetrico), `eq_envio_param` (tipologias urbano/nacional) y `eq_cost_base_datos` (No requiere/Cliente/Comprar). Valores actuales son placeholders hasta confirmar en Excel.

## 5. Motor de calculo (`Costos Directos discriminados`)
Referencias clave (columna D = costo total, E = valor con GM donde aplica):
- Muestra y multiplicadores: muestra base `C52` + sobremuestra `D53`; dias de campo `C57=ROUNDUP(C58)`; productividad `C55`.
- Campo F2F (`D67`): (valor encuesta + parafiscales) * muestra * factor si siembra/recoleccion (E50=1 o 2). Valor encuesta via `Parametros` matriz de minutos vs penetracion.
- Campo CATI (`D68`) y Online/Auto (`D71`): usa matrices dedicadas de `Parametros` (rangos 80:104 y 94:104).
- Campo internacional (`D69`) y proveedor externo (`D70`): toman valores directos de entradas D15/D14.
- Mystery/Shopper (`D73`, `D74`, `D75`): usa totales de `MYSTERY!M39` o multiplicadores por dias/productividad.
- Reclutamiento e incentivos: `D76` (reclutamiento) e `D114` (incentivos) desde `Valores Insumos reclutamiento` + obsequio de entradas.
- Insumos y blindaje: `D77` productos de prueba, `D78` rotulacion/empacado/blind multiplicado por #productos y clase de prueba, `D84` locaciones central por ciudades/metodologia.
- Transporte: `D79` encuestadores, `D80` supervisores, `D82` ninos, `D83` bebidas alcoholicas, `D81` envios (usa tabla de envios segun peso y ciudades).
- Staff de campo/calidad: `D90` supervisores, `D91` coordinacion campo, `D92` entrenamiento, `D93` verificacion (incluye GM 21.45%), `D97` critica, `D95` costo telefonico verificacion, `D96` siembra telefonica.
- Locaciones por ciudad (`D101:D104`) multiplican tarifa por #dias y factor de refrigeracion (`D42`).
- Base de datos (`D110`) = costo base * tipo (No requiere/Cliente/Comprar).
- Totales: `D112` total costo directo campo; `D113` compra producto; `D115` tablets; `D116` total con incentivos; `D129` staff OPS (sumatoria de estadistica/scripting/datacleaning/etc.).
- Staff OPS/analytics: `D121` estadistica (desde `Entradas!D99`), `D122` scripting (horas por minutos * tipo script), `D124` datacleaning, `D125` toplines, `D126` procesamiento, `D127` Harmoni, `D128` graficacion.
- Margenes y OP: `D132` direct cost OPS = ((D112+D70)/(1-GM OPS 21.45%))+compra+tablets+campo internacional+incentivos/(1-7%)+staff con GM; `D135` AOT; `D137` GM=D135+D136; `D138` PB+RMF = -AOT * 4.3%; `D139` Prof Time = -staff OPS SL (`F95`); `D140` OP = GM+PB+RMF+ProfTime; `%OP`=D140/D135.
- Resumen (`D150:E154`): separa costos con y sin gross por rubro.
- Version en USD usa TRM (`H130`).

### 5.1 Formulas replicables (pseudocodigo)
- `valor_encuesta_f2f = lookup_preco(duracion, penetracion, Parametros.base) + parafiscales_pct * valor_encuesta_base`
- `dias_campo = ceil(total_encuestas / (productividad_por_dia * encuestadores_por_ciudad))`
- `costo_campo_f2f = valor_encuesta_f2f * muestra_total * factor_siembra (1|2)`
- `costo_campo_cati = lookup_cati(duracion, penetracion) * muestra_cati`
- `costo_campo_online = lookup_online(duracion, penetracion) * muestra_online`
- `reclutamiento_total = sum_nse(valor_reclutamiento_nse * cant_por_nse) + comision_bonos`
- `incentivos_total = sum_nse(valor_obsequio_nse * cant_por_nse) / (1-7%)`
- `insumos_prueba = precio_insumo * muestra_total * num_productos`
- `blind/rotulacion = precio_rotulacion * muestra_total * productos_por_resp * factor_etiquetado`
- `transporte_encuestador = tarifa_transporte_enc * dias_campo * (#encuestadores * #ciudades)`
- `transporte_supervisor = tarifa_transporte_sup * dias_campo * (#supervisores * #ciudades)`
- `envios = tarifa_envio_kilo(peso_kg) * #ciudades_si_envio`
- `locaciones = tarifa_locacion_ciudad * (dias_setup + dias_campo) * factor_refrigeracion`
- `staff_campo = sum(supervision + coordinacion + entrenamiento + verificacion + critica + telefonico)`
- `scripting = tarifa_script_hora * horas_script_por_duracion * multiplicador_tipo_script`
- `procesamiento = tarifa_proc_hora * horas_proc_por_duracion * num_procesamientos / productividad`
- `datacleaning = tarifa_dc_hora * horas_dc_por_duracion * factor_total/parcial`
- `estadistica = precio_tarifario_estadistica(servicio)`
- `staff_ops_total = sum(scripting, procesamiento, datacleaning, toplines, harmoni, graficacion, estadistica)`
- `direct_cost_ops = ((costo_directo_campo + campo_externo)/(1-gm_ops)) + compra_prod + tablets + campo_internacional + incentivos/(1-7%) + staff_ops_con_gm`
- `aot = direct_cost_ops` (equivale a AOT en hoja)
- `gm = aot - total_con_incentivos` (signo segun hoja)
- `pb_rmf = -aot * 0.043`
- `prof_time = -staff_sl_total`
- `op = gm + pb_rmf + prof_time`
- `%op = op / aot`

## 6. Sugerencia de modelo de datos MVC
- `eq_quote_header` (id, propuesta, cliente, grupo_objetivo, fechas_aprobacion/campo, prob_aprobacion enum, SL, metodologia_sl, record_detail, categoria, proveedor_ext, proveedor_int, valor_gmu, notas).
- `eq_questionnaire` (quote_id FK, duracion_min, penetracion_id, preguntas_abiertas, abiertas_multiples, otros, top_line bool, datacleaning enum, ascii bool, script_reclutamiento bool, scripting_tipo enum, codificacion bool, procesamiento bool, num_procesamientos, proceso_estadistico bool, clase_prueba enum, refrigeracion bool, compra_producto, etiquetas_tipo enum, embalaje bool, productos_testear, productos_por_resp, patinadores_por_ciudad, siembra bool).
- `eq_methodology` (quote_id, metodologia_recoleccion, tecnica1 tipo, tecnica1_flag, tecnica2, tecnica3, base_datos enum, incidencia_label, incidencia_valor, metodologias_mix flag).
- `eq_sample_city` (id, quote_id, ciudad, activa bool, muestra_total, nse1..nse6, metodologia_tec# referenciada, sobre_muestra_pct, peso_producto_gr, envio_ciudades bool).
- `eq_logistica` (quote_id, dias_setup, dias_campo, num_olas, viaticos_campo, otros_incentivos, reprografia_paginas, apoyo_reclutamiento_tipo, apoyo_en_sitio_factor, envios_tipo, transporte_participantes, taxi_participantes bool).
- `eq_mystery` (quote_id, tipo_visita, complejidad, num_olas, desplazamientos, tanques, alertas, edicion_video, alquiler_equipos, compra_dispositivos, seguimiento).
- `eq_staff_sl` (quote_id, nivel L3-L7, horas_minimas, horas_presup, tarifa_nivel, valor_total, fuente Horas).
- `eq_ratecards` (tablas maestras): `eq_param_precio` (duracion, penetracion, perfil, valor_base, coordinacion, total), `eq_param_script_proc` (duracion, horas_script, horas_proc, horas_harmoni, horas_graficacion), `eq_param_prob_aprob`, `eq_param_metodologia_cod`, `eq_valor_hora_ops` (nivel, alternativa, vigencia, base_cost, overhead, loaded, billing), `eq_rate_estadistica` (catalogo de servicios con horas, precio_ref, factor_escala, lead_time), `eq_cost_insumos` (NSE, reclutamiento, obsequio, productividad, dias, locaciones, insumos, transportes, telefonia, envios_kilo).
- `eq_cost_result` (quote_id, moneda, costo_campo, costo_calidad, viaticos, incentivos, insumos, logistica, staff_ops, estadistica, scripting, datacleaning, toplines, procesamiento, harmoni, graficacion, compra_producto, tablets, costo_directo_total, costo_con_incentivos, direct_cost_ops, gm, pb_rmf, prof_time, op, pct_op, aot_unitario, aot_total, resumen_rubros campo, tdd, incentivos, estadistica).
- `eq_country_profile` (pais, nse, share_poblacion, ingresos, ejemplos_colonia) para futuras validaciones de muestra.

### 6.1 Detalle de tablas maestras (semillas)
- `eq_param_precio` (id, tipo_metodologia enum {F2F,CATI,ONLINE,AUTO}, penetracion_rango, duracion_min, valor_perfil, valor_coord, valor_total, version, vigente_desde, vigente_hasta).
- `eq_param_script_proc` (duracion_min, horas_script, horas_proc, horas_harmoni, horas_graficacion).
- `eq_param_metodologia` (codigo, descripcion, multipliers_siembra, notas_bitacora).
- `eq_param_prob` (codigo, orden).
- `eq_valor_hora_ops` (nivel, alternativa, base_cost_rate, overhead_rate, loaded_cost_rate, billing_rate, vigente_desde, vigente_hasta).
- `eq_rate_estadistica` (id, categoria, servicio, horas_est, precio_ref_2024, factor_escala, lead_time, ejemplos, factor_economia_escala).
- `eq_cost_insumos` (nse, reclutamiento, obsequio, productividad, dias, sup, log, transporte_enc, transporte_sup, valor_envio_1er_kilo, valor_kilo_adicional, seguro_pct, valor_min_declarar).
- `eq_locaciones` (ciudad, tarifa_base, tarifa_con_gross, dias_base).
- `eq_cost_unitario_ops` (actividad, cod_matrix, tarifa_unitaria, unidad, fuente `Valores Insumos reclutamiento`).
- `eq_codificacion_param` (escenario, registros, preguntas_abiertas, preguntas_mult, dias, horas, valor_ipsos).
- `eq_tarifa_mystery` (tipo_visita, complejidad, cod_matrix, vr_unitario, olas_default, multiplicador_ola2plus, incluye_coord, incluye_asist, incluye_critica, observaciones).

## 7. Reglas y validaciones implicitas
- Probabilidad aprobacion, clase de prueba, top line, datacleaning, ascii, script reclutamiento, codificacion, proceso estadistico, harmoni, graficacion, siembra/recoleccion, envio a ciudades, refrigeracion anticipada: todas son opciones Si/No mapeadas a 1/0 en `Parametros!219:220`.
- Metodologia y tecnicas usan codigos de `Parametros!230:239`; condicionan formulas de costo (F2F=1, CATI=2, ONLINE=10, AUTO INNO=4, MYSTERY=5, SHOPPER=8/9).
- Valor por encuesta: lookup a matrices de duracion vs penetracion; validar minutos dentro de rango 5-60 y penetracion dentro de catalogo.
- Distribucion de muestra por NSE debe sumar a la muestra total por ciudad y al total general (`C54`). Si se usa `MuestraTec1`, valores base de NSE se cargan automaticamente; MVC debe recalcular totales y proporciones.
- Reclutamiento/obsequios predefinidos por NSE (`Valores Insumos reclutamiento`); permitir override pero recalcular comision de bonos (10%) y total con comision.
- Transporte ninos y bebidas alcoholicas son condicionados por flags (`Entradas!A58` y `E59`), con valores fijos 15.000 y 28.000 (bitacora).
- Etiquetado/empacado/blind aplica multiplicador segun seleccion (`Parametros!226:229`).
- Refrigeracion anticipada (`Entradas!D61`) multiplica locaciones por 1 si aplica (E105:E109).
- Mystery: numero de olas por tipo de visita afecta totales; olas adicionales al 50% para critica (nota en `MYSTERY!20`).
- Staff SL: horas minimas via `Horas` y tarifas `Horas!J5:J9`; total dividido por (1-26.84%) para gross.
- GM OPS fijo 21.45% (`D117`), PB+RMF 4.3% (`E116`), OP SL/Ops 15% (`H115`), OP Ops 7% (`D145`).
- TRM en `H130`; cualquier cambio de moneda debe recalcular unidades y totales.
- Validar division por cero en unitarios (IFERROR en Excel). En backend usar manejo seguro (si muestra_total=0, regresar 0 y mensaje).
- Campos numericos no negativos; porcentajes entre 0 y 1; minutos enteros.
- Si tecnica=CATI/ONLINE/AUTO, rubros F2F (transporte, locaciones, etc.) deben apagarse.
- Refrigeracion solo aplica a locaciones (E105:E109) cuando flag Si.
- Envios: usar peso volumetrico max(peso_real, peso_volumetrico) y tarifa segun tipologia (urbano/nacional/reexpedicion).

## 8. Checklist de desarrollo y QA
- Captura MVC
  - Implementar formulario con secciones de datos generales, cuestionario/procesos, logistica/muestra, mystery/shopper, staff SL.
  - Dropdowns sincronizados con tablas maestras (`Parametros`, `Horas`, `Valor Hora - Alternativas`, `Tarifario Estadistica*`).
  - Validar que los valores por NSE/ciudad se sumen correctamente y permiten cargar distribuciones sugeridas (MuestraTec1/MuestraTec2).
- Motor de costos
  - Replicar matrices de precio por duracion/incidencia (F2F/CATI/Online/Auto) y multiplicadores de siembra, clase de prueba, etiquetado, refrigeracion, apoyo reclutamiento, GM OPS, PB+RMF, OP.
  - Implementar formulas de campo/incentivos/logistica/locaciones/telefoniÌa/siembra exactamente como `Costos Directos discriminados`.
  - Calcular staff OPS y estadistica (horas*tarifa) y staff SL (L4-L7) con gross 26.84%.
  - Generar resumen por rubro y totales (con y sin gross) y version en USD usando TRM.
- Datos maestros
  - Cargar tablas de `Valores Insumos reclutamiento` (reclutamiento, obsequios, locaciones, insumos, costos unitarios, envios kilo), `Valor por peso`, `Estructura de costos - 2023`, `Valor Hora - Alternativas`, `Codificacion`.
  - Exponer mantenimiento para tasas (TRM, GM, PB+RMF, OP) y tarifas si cambian.
- QA y reconciliacion
  - Crear casos de prueba con el set de datos de ejemplo (muestra 400 Bogota, Hogares F2F, duracion 20, penetracion >82%, scripting nuevo) y comparar celda a celda contra Excel (D67, D76, D116, D132, D135, D140, F150-F154).
  - Validar escenarios alternos: CATI, online/auto, mystery con 2-3 tipos de visita, multiple olas, refrigeracion Si, envio ciudades Si con peso >1kg, compra de producto >0.
  - Verificar redondeos (ROUNDUP dias de campo), divisiones por muestra (valor unitario) y manejo de division por cero (IFERROR en formulas).
  - Confirmar pendientes de bitacora (pilotos, mix mode, autoaplicado) y documentar comportamientos decididos.
- Entrega
  - Documentar mapping de cada campo Excel -> modelo -> UI -> API.
  - Incluir seed scripts para tablas maestras y tests de regresion de costos.
  - Validar permisos/roles si se requieren (ej. solo ciertos roles editan tarifas).

## 9. Flujo MatrixNext (UI/API) y archivos sembrados
- UI (MVC):
  - Paso 1 Datos generales: propuesta/cliente/SL/metodologia, fechas, prob_aprobacion.
  - Paso 2 Cuestionario y procesos: duracion, penetracion, flags (topline/datacleaning/ascii/script/codificacion/proceso_estadistico).
  - Paso 3 Muestra y logistica: ciudades con desglose NSE, sobre-muestra, siembra, apoyo, envios/peso, productos a testear, etiquetado/blind, refrigeracion, clase de prueba.
  - Paso 4 Mystery/Shopper (opcional): tipos de visita, olas, desplazamientos/equipos/bonos.
  - Paso 5 Staff SL: horas minimas sugeridas + override.
  - Paso 6 Resumen de costos: rubros, unitarios, totales, GM/OP, version USD.
- API (sugerido):
  - `POST /api/quotes/easyquote` crea header + detalle.
  - `PUT /api/quotes/easyquote/{id}` actualiza y recalcula.
  - `POST /api/quotes/easyquote/{id}/calculate` ejecuta motor (stateless) y devuelve breakdown.
  - `GET /api/quotes/easyquote/{id}/result` obtiene ultimo calculo.
  - `GET /api/masters/easyquote/*` para tablas maestras (precios base, horas, insumos, tarifario estadistica, valor hora OPS, envios, paises).
- Seeds/archivos:
  - CSV/JSON de `Parametros` (matrices duracion vs penetracion para F2F/CATI/ONLINE/AUTO).
  - CSV/JSON de `Valores Insumos reclutamiento` (reclutamiento/obsequios por NSE, locaciones, insumos, unitarios).
  - CSV/JSON de `Horas` (KEY, horas min L3-L7, tarifas).
  - CSV/JSON de `Tarifario Estadistica2` (usar version 2 como principal).
  - CSV/JSON de `Valor Hora - Alternativas` y `Estructura de costos - 2023`.
  - Tabla de envios (`Valor por peso`) con tarifas 1er kilo/kilo adicional/seguro.
  - Bitacora de ajustes: registrar como issues/resolved en backlog de migracion.

### 9.1 Alineacion con DIRECTRICES_MIGRACION.md
- Areas: crear area `EQ` para el cotizador con controller `EasyQuoteController` y vistas en `Areas/EQ/EasyQuote`.
- UI: usar modales para capturas secundarias (ej. editar fila de ciudad, agregar visita mystery) y grids reutilizando `_Grid`, `_Modal`, `_DatePicker`, `_Toast`.
- MenÃº/sidebar: agregar entrada €œEasyQuote€ en `_Sidebar.cshtml` bajo Proyectos/Cotizador.
- Async/await y [Authorize] en controller; validar permisos para admin de parametros en un controller separado (`EasyQuoteAdminController`).
- EF para inserts/updates simples (semillas de masters y persistencia de cotizacion); no hay SP legacy conocidos, pero validar en CoreProject antes de crear logica nueva. Si existen SP de IQuote, mapearlos via adapter.
- Documentar cambios en `MODULOS_MIGRACION.md` y generar `MIGRACION_EASYQUOTE_COMPLETADA.md` al cierre.

## 10. Casos de prueba y reconciliacion
- Caso base (plantilla Excel actual): F2F Hogares, duracion 20, penetracion 82%, muestra 400 Bogota, scripting nuevo, procesamiento Si, datacleaning total, sin envios, sin refrigeracion. Validar celdas: D67, D76, D116, D132, D135, D140, E150:E154 contra resultado API.
- Variantes:
  1) CATI, penetracion 55%, duracion 30, base datos Cliente, sin transporte, con envios peso 1.5kg; validar D68, envios D81, TRM.
  2) ONLINE/Auto Inno, duracion 15, penetracion 67%, con sobre-muestra 10% y productos_por_resp=2; validar rotulacion D78, blind factor.
  3) Mystery con 2 tipos de visita y 3 olas; validar MYSTERY totales M39, M49 y que D73/D75 reflejen.
  4) Bebidas alcoholicas flag en taxi participantes: activar transporte bebidas (D83=28.000) y ajuste de valor.
  5) Estudios con ninos: transporte ninos D82=15.000 * muestra; refrigeracion Si activa locaciones E105:E109.
  6) Script duplicado: tipo_script=Duplicado (x4 horas) y num_procesamientos>1.
  7) Proveedor externo e internacional: D70/D69 >0 y GMU aplicado; validar conversiÃ³n USD.
  8) Codificacion Si con preguntas_abiertas>0: validar lookup `Codificacion!` y D94.
- Reconciliacion: generar CSV de breakdown por rubro y comparar contra export de Excel (usar mismo set de inputs) con tolerancia 0 en moneda local y 2 decimales en USD.

## 11. Pendientes y riesgos
- Bitacora pendientes: incorporar mix mode, autoaplicado (revisar codigos Parametros), pilotos (campo adicional no modelado).
- Riesgo de tarifas desactualizadas: confirmar vigencias de `Valor Hora - Alternativas` y `Tarifario Estadistica` (2024 vs 2025).
- Dependencia en TRM manual (`H130`): definir fuente automatica o control de version.
- Validar reglas de redondeo (dias campo y unitarios) frente a requerimientos financieros.
- Asegurar control de versiones de tablas maestras (version/fecha) para reproducibilidad.
- UX: replicar facilidad de Excel con tabla editable por celdas para ciudades/NSE y visitas mystery; considerar ediciÃ³n inline en grid con totales recalculados on-change y modales solo para detalles. Proveer pÃ¡gina separada de administraciÃ³n de parÃ¡metros (tarifas, matrices, insumos) con filtros, ediciÃ³n inline y carga masiva (CSV) respetando directrices de area/autorization.
---

Ruta propuesta del documento: `docs/ANALISIS_EASYQUOTE.md`.

