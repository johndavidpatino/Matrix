# Diccionario de campos y bloques de cálculo (Excel → MVC)

- Archivo origen: `Ipsos EasyQuote 2025v2 XLS.xlsx`
- Inputs referenciados (celdas no-fórmula): **3,622**
- Outputs (fórmulas finales / sinks): **3,142**
- Fórmulas intermedias: **2,358**
- Bloques de cálculo detectados: **3,033**

## Top 10 inputs más influyentes

| Hoja | Celda | Propiedad sugerida | Tipo | #Refs |
|---|---|---|---|---:|
| Hoja2 | E50 | GM | decimal | 21 |
| MuestraTec1 | C34 | NoDias | int | 15 |
| Parametros | H124 | Reclutamiento | string | 13 |
| MYSTERY | M128 | MYSTERYM128 | string | 13 |
| Entradas | E20 | VLOOKUPE20ParametrosB24C3420 | string | 12 |
| Parametros | B219 | Otros | string | 11 |
| Hoja2 | D7 | ResearchLevel6 | int | 9 |
| Precios bases | L105 | Field6026180100L105 | int | 8 |
| Precios bases | X89 | U88X89 | int | 8 |
| Precios bases | X105 | Field6026180100X105 | int | 8 |

## Bloques (Top 15 por #Outputs)

| Bloque | Hoja | #Outputs | #Deps internas |
|---|---|---:|---:|
| Costos Directos discriminados::B01 | Costos Directos discriminados | 9 | 62 |
| Entradas::B01 | Entradas | 4 | 65 |
| Valores Insumos reclutamiento::B01 | Valores Insumos reclutamiento | 3 | 5 |
| Valores Insumos reclutamiento::B03 | Valores Insumos reclutamiento | 3 | 5 |
| Valores Insumos reclutamiento::B02 | Valores Insumos reclutamiento | 3 | 27 |
| Estructura de costos - 2023::B32 | Estructura de costos - 2023 | 2 | 15 |
| Estructura de costos - 2023::B03 | Estructura de costos - 2023 | 2 | 15 |
| Estructura de costos - 2023::B02 | Estructura de costos - 2023 | 2 | 15 |
| Estructura de costos - 2023::B01 | Estructura de costos - 2023 | 2 | 15 |
| Hoja2::B04 | Hoja2 | 2 | 13 |
| Estructura de costos - 2023::B05 | Estructura de costos - 2023 | 2 | 15 |
| Hoja2::B03 | Hoja2 | 2 | 9 |
| Hoja2::B02 | Hoja2 | 2 | 13 |
| Hoja2 (2)::B01 | Hoja2 (2) | 2 | 4 |
| Hoja2 (2)::B02 | Hoja2 (2) | 2 | 4 |

## Cómo usarlo

- **FieldDictionary_Inputs**: candidatos a inputs del formulario (ViewModel) con tipo inferido, label sugerido y validación (si aplica).
- **CalcBlocks_BySheet** y **BlockMembers_Outputs**: agrupan outputs en bloques por hoja para partir el motor de cálculo en servicios C#.
- **BlockGraph**: dependencias entre bloques (si un bloque alimenta a otro).
