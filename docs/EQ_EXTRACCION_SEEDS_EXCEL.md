# GUÍA DE EXTRACCIÓN DE SEEDS - EXCEL A MATRIXNEXT

**Archivo fuente**: `Ipsos EasyQuote 2025v2.xlsm`  
**Objetivo**: Extraer seeds maestros reales para reemplazar placeholders en MatrixNext.  
**Fecha**: 2026-01-05  
**Prioridad**: 🔥 **CRÍTICA** - Bloqueador para paridad 1:1

---

## 📋 ÍNDICE

1. [Pre-requisitos](#pre-requisitos)
2. [Matriz Precios F2F](#1-matriz-precios-f2f)
3. [Matriz CATI](#2-matriz-cati)
4. [Matriz Online/Auto](#3-matriz-onlineauto)
5. [Factores y Parámetros](#4-factores-y-parámetros)
6. [Tabla Horas SL](#5-tabla-horas-sl)
7. [Otros Seeds Críticos](#6-otros-seeds-críticos)
8. [Validación](#7-validación)

---

## Pre-requisitos

**Antes de comenzar**:
- ✅ Tener acceso al archivo `Ipsos EasyQuote 2025v2.xlsm`.
- ✅ Microsoft Excel instalado.
- ✅ Editor de texto (VS Code, Notepad++) para generar CSVs.
- ✅ SQL Server Management Studio para importar CSVs.

**Herramientas opcionales**:
- Excel2CSV converter (online o local).
- PowerQuery Excel para limpieza de datos.

---

## 1. Matriz Precios F2F

### Ubicación en Excel
**Hoja**: `Parametros`  
**Rango**: `B4:AI12` (según ANALISIS_EASYQUOTE §4)

### Estructura Esperada
```
Columnas (penetración): MAS82 | 75-82 | 67-74 | 55-66 | 46-54 | 37-45
Filas (duración min): 5 | 10 | 15 | 20 | 25 | 30 | 35 | 40 | 45 | 50 | 55 | 60
Valores: ValorPerfil | ValorCoordinacion | ValorTotal (por celda o separados)
```

### Pasos de Extracción

1. **Abrir Excel** → Hoja `Parametros`.
2. **Seleccionar rango** `B4:AI12`.
3. **Inspeccionar estructura**:
   - ¿Columnas son penetraciones?
   - ¿Filas son duraciones?
   - ¿Valores son totales o separados (perfil + coordinación)?
4. **Screenshot** del rango completo (documentar).
5. **Exportar a CSV**:
   - Copiar rango → Pegar en nuevo Excel.
   - Agregar headers: `duracion_min,penetracion_codigo,valor_perfil,valor_coord,valor_total`.
   - Guardar como `eq_param_precio_f2f_real.csv`.

### Ejemplo CSV Esperado
```csv
duracion_min,penetracion_codigo,valor_perfil,valor_coord,valor_total
5,MAS82,12000,3000,15000
5,75-82,11500,2900,14400
5,67-74,11000,2800,13800
...
60,37-45,45000,8000,53000
```

### Notas Importantes
- **Validar**: Total filas debe ser ~6 penetraciones × ~12 duraciones = **~72 registros**.
- **Placeholders actuales**: Reemplazar todos los registros en `eq_param_precio` con estos datos reales.
- **Versionado**: Marcar como versión 1 con fecha 2026-01-05.

---

## 2. Matriz CATI

### Ubicación en Excel
**Hoja**: `Parametros`  
**Rango**: `!80:104` (según ANALISIS_EASYQUOTE §6.2.3)  
**Nota**: Rango aproximado, validar visualmente en Excel.

### Estructura Esperada
```
Igual a matriz F2F pero para metodología CATI:
Columnas (penetración): MAS82 | 75-82 | 67-74 | 55-66 | 46-54 | 37-45
Filas (duración min): 5 | 10 | 15 | ... | 60
Valores: ValorTotal por celda
```

### Pasos de Extracción

1. **Abrir Excel** → Hoja `Parametros`.
2. **Scroll a filas 80-104**.
3. **Buscar sección** titulada "CATI" o similar.
4. **Seleccionar rango** completo de matriz CATI.
5. **Screenshot** (documentar).
6. **Exportar a CSV**:
   - Headers: `duracion_min,penetracion_codigo,valor_perfil,valor_coord,valor_total`.
   - Guardar como `eq_param_cati_real.csv`.

### Ejemplo CSV
```csv
duracion_min,penetracion_codigo,valor_perfil,valor_coord,valor_total
5,MAS82,8000,2000,10000
5,75-82,7800,1950,9750
...
```

### Notas Importantes
- **Tabla nueva**: Crear `eq_param_cati` con misma estructura que `eq_param_precio`.
- **Validar**: ~72 registros esperados.

---

## 3. Matriz Online/Auto

### Ubicación en Excel
**Hoja**: `Parametros`  
**Rango**: `!94:104` (según ANALISIS_EASYQUOTE §6.2.3)  
**Nota**: Puede estar en misma sección que CATI o separada.

### Estructura Esperada
```
Igual a matrices anteriores pero para Online/Auto:
Columnas (penetración): MAS82 | 75-82 | 67-74 | 55-66 | 46-54 | 37-45
Filas (duración min): 5 | 10 | 15 | ... | 60
Valores: ValorTotal por celda
```

### Pasos de Extracción

1. **Abrir Excel** → Hoja `Parametros`.
2. **Scroll a filas 94-104**.
3. **Buscar sección** titulada "ONLINE" o "AUTO" o similar.
4. **Seleccionar rango** completo.
5. **Screenshot** (documentar).
6. **Exportar a CSV**: `eq_param_online_real.csv`.

### Notas Importantes
- **Tabla nueva**: Crear `eq_param_online`.
- **Validar**: ~72 registros esperados.

---

## 4. Factores y Parámetros

### 4.1 Tipo Script (Nuevo/Duplicado/Reutilización)

**Ubicación**: `Parametros` filas `180:182` (según ANALISIS §6.2.3)

**Estructura Esperada**:
```
Tipo           | Factor
---------------+--------
Nuevo          | 1.0
Duplicado      | 0.75
Reutilización  | 0.5
```

**CSV**: `eq_param_factores_script.csv`
```csv
tipo,codigo,descripcion,factor,orden
SCRIPT_TIPO,Nuevo,Script completamente nuevo,1.0,1
SCRIPT_TIPO,Duplicado,Script duplicado con ajustes,0.75,2
SCRIPT_TIPO,Reutilizacion,Script reutilizado sin cambios,0.5,3
```

### 4.2 Probabilidad Aprobación

**Ubicación**: `Parametros` filas `207:210`

**Estructura Esperada**:
```
Probabilidad | Factor/Orden
-------------+-------------
Alta         | 1
Media        | 2
Baja         | 3
```

**CSV**: `eq_param_factores_prob.csv`
```csv
tipo,codigo,descripcion,factor,orden
PROB_APROBACION,Alta,Alta probabilidad de aprobación,1.0,1
PROB_APROBACION,Media,Media probabilidad,1.0,2
PROB_APROBACION,Baja,Baja probabilidad,1.0,3
```

### 4.3 Apoyo Reclutamiento

**Ubicación**: `Parametros` filas `214:217`

**Estructura Esperada**:
```
Apoyo                | Factor
---------------------+--------
Sin apoyo            | 1.0
Logística            | 1.2
Apoyo completo       | 1.5
```

**CSV**: `eq_param_factores_apoyo.csv`
```csv
tipo,codigo,descripcion,factor,orden
APOYO_RECLUTAMIENTO,SinApoyo,Sin apoyo adicional,1.0,1
APOYO_RECLUTAMIENTO,Logistica,Apoyo logístico en sitio,1.2,2
APOYO_RECLUTAMIENTO,Completo,Apoyo completo de reclutamiento,1.5,3
```

### 4.4 Etiquetado/Blind

**Ubicación**: `Parametros` filas `226:229`

**Estructura Esperada**:
```
Tipo                 | Factor
---------------------+--------
Sin etiquetado       | 0
Rotulado             | 1.0
Blind                | 1.5
Blind + Rotulado     | 2.0
```

**CSV**: `eq_param_factores_etiquetado.csv`
```csv
tipo,codigo,descripcion,factor,orden
ETIQUETADO,SinEtiquetado,Sin etiquetado,0,1
ETIQUETADO,Rotulado,Rotulado estándar,1.0,2
ETIQUETADO,Blind,Blind (sin marca visible),1.5,3
ETIQUETADO,BlindRotulado,Blind + Rotulado,2.0,4
```

### 4.5 Clase de Prueba

**Ubicación**: `Parametros` filas aproximadas (buscar "Clase de prueba")

**Estructura Esperada**:
```
Clase                  | Factor
-----------------------+--------
Monódica               | 1.0
Monódica secuencial    | 1.2
Comparativa            | 1.5
No aplica              | 0
```

**CSV**: `eq_param_factores_clase_prueba.csv`
```csv
tipo,codigo,descripcion,factor,orden
CLASE_PRUEBA,Monodica,Prueba monódica,1.0,1
CLASE_PRUEBA,MonodicaSecuencial,Prueba monódica secuencial,1.2,2
CLASE_PRUEBA,Comparativa,Prueba comparativa,1.5,3
CLASE_PRUEBA,NoAplica,No aplica,0,4
```

### Consolidar Factores

**Acción**: Unir todos los CSVs de factores en uno solo:
```csv
tipo,codigo,descripcion,factor,orden
SCRIPT_TIPO,Nuevo,Script completamente nuevo,1.0,1
SCRIPT_TIPO,Duplicado,Script duplicado con ajustes,0.75,2
SCRIPT_TIPO,Reutilizacion,Script reutilizado sin cambios,0.5,3
PROB_APROBACION,Alta,Alta probabilidad de aprobación,1.0,1
PROB_APROBACION,Media,Media probabilidad,1.0,2
PROB_APROBACION,Baja,Baja probabilidad,1.0,3
APOYO_RECLUTAMIENTO,SinApoyo,Sin apoyo adicional,1.0,1
APOYO_RECLUTAMIENTO,Logistica,Apoyo logístico en sitio,1.2,2
APOYO_RECLUTAMIENTO,Completo,Apoyo completo de reclutamiento,1.5,3
ETIQUETADO,SinEtiquetado,Sin etiquetado,0,1
ETIQUETADO,Rotulado,Rotulado estándar,1.0,2
ETIQUETADO,Blind,Blind (sin marca visible),1.5,3
ETIQUETADO,BlindRotulado,Blind + Rotulado,2.0,4
CLASE_PRUEBA,Monodica,Prueba monódica,1.0,1
CLASE_PRUEBA,MonodicaSecuencial,Prueba monódica secuencial,1.2,2
CLASE_PRUEBA,Comparativa,Prueba comparativa,1.5,3
CLASE_PRUEBA,NoAplica,No aplica,0,4
```

**Guardar como**: `eq_param_factores_consolidado.csv`

---

## 5. Tabla Horas SL

### Ubicación en Excel
**Hoja**: `Horas`

### Estructura Esperada
```
KEY (formato: "SL|RecordDetail|MetodologiaSL") | Horas L3 | Horas L4 | Horas L5 | Horas L6 | Horas L7 | Loaded Rate L3 | ... | Billing Rate L7
```

### Pasos de Extracción

1. **Abrir Excel** → Hoja `Horas`.
2. **Inspeccionar estructura**:
   - ¿Cómo se forma el KEY? (probablemente concatenación de 3 columnas).
   - ¿Columnas de horas L3-L7?
   - ¿Columnas de rates (loaded, billing)?
3. **Screenshot** completo.
4. **Exportar a CSV**:
   - Si KEY no existe, crear concatenando: `=A2&"|"&B2&"|"&C2`.
   - Headers: `sl,record_detail,metodologia_sl,horas_l3,horas_l4,horas_l5,horas_l6,horas_l7,loaded_rate_l3,...,billing_rate_l7,key`.
   - Guardar como `eq_rate_horas_real.csv`.

### Ejemplo CSV
```csv
sl,record_detail,metodologia_sl,horas_l3,horas_l4,horas_l5,horas_l6,horas_l7,key
TS,Detailed,CATI,0,10,15,20,5,TS|Detailed|CATI
TS,Summary,F2F,0,8,12,15,3,TS|Summary|F2F
BUS,Detailed,ONLINE,0,6,10,12,2,BUS|Detailed|ONLINE
...
```

### Notas Importantes
- **KEY compuesta**: Debe ser única por combinación (SL + RecordDetail + MetodologiaSL).
- **Validar**: Sin duplicados de KEY.
- **Tabla nueva**: Crear `eq_rate_horas`.

---

## 6. Otros Seeds Críticos

### 6.1 Parafiscales F2F

**Ubicación**: Buscar en `Parametros` o `Valores Insumos reclutamiento`.  
**Valor esperado**: Porcentaje (ej: 0.05 = 5%).

**Acción**:
- Si existe, documentar.
- Si no existe, asumir 0% y validar con usuario Excel.

**Guardar en**: `eq_param_misc` como clave `PARAFISCALES_PCT`.

### 6.2 Refrigeración

**Ubicación**: Excel celda `D42` (según ANALISIS §5).

**Valores a confirmar**:
- Factor refrigeración (placeholder actual: 1.15).
- Costo nevera (placeholder actual: 970000).

**Acción**:
- Abrir Excel → celda D42 → inspeccionar fórmula.
- Extraer factor y costo.
- Actualizar `eq_param_misc`:
  - `FACTOR_REFRIGERACION` = valor real.
  - `COSTO_NEVERA` = valor real.

### 6.3 Base de Datos Costos

**Ubicación**: Buscar en `Valores Insumos reclutamiento` o similar.

**Valores esperados**:
```
No requiere: 0
Cliente:     [valor_real]
Comprar:     [valor_real]
```

**Acción**:
- Extraer costos reales.
- Actualizar `eq_cost_base_datos` (reemplazar placeholders 100/200/300).

### 6.4 Codificación Tabla Completa

**Ubicación**: Hoja `Codificacion`.

**Estructura esperada**:
```
Escenario | Registros | PregAbiertas | PregAbiertasMult | Dias | Horas | ValorIpsos
```

**Acción**:
- Exportar tabla completa → `eq_codificacion_param_real.csv`.
- Reemplazar seed actual (solo 1 escenario) con tabla completa.

---

## 7. Validación

### Checklist de Validación

**Después de extraer cada seed**:
- [ ] ✅ Screenshot documentado.
- [ ] ✅ CSV generado con headers correctos.
- [ ] ✅ Validar número de registros esperados.
- [ ] ✅ Sin valores NULL o vacíos (excepto opcionales).
- [ ] ✅ Formatear decimales correctamente (punto, no coma).
- [ ] ✅ Codificación UTF-8 sin BOM.

### Validación Final Pre-Importación

**Antes de importar a SQL**:
1. Abrir cada CSV en Excel → validar visualmente.
2. Contar registros → comparar con esperado.
3. Buscar duplicados (KEY o combinaciones únicas).
4. Validar rangos de valores (ej: factores 0-2, costos >0).
5. Validar consistencia (ej: ValorTotal = ValorPerfil + ValorCoordinacion).

### Post-Importación SQL

**Queries de verificación**:
```sql
-- Matriz F2F
SELECT COUNT(*) FROM eq_param_precio WHERE MetodologiaCodigo = 'F2F';
-- Esperado: ~72 registros

-- Matriz CATI
SELECT COUNT(*) FROM eq_param_cati;
-- Esperado: ~72 registros

-- Matriz Online
SELECT COUNT(*) FROM eq_param_online;
-- Esperado: ~72 registros

-- Factores
SELECT tipo, COUNT(*) FROM eq_param_factores GROUP BY tipo;
-- Esperado: SCRIPT_TIPO=3, PROB_APROBACION=3, APOYO_RECLUTAMIENTO=3, ETIQUETADO=4, CLASE_PRUEBA=4

-- Tabla Horas
SELECT COUNT(*) FROM eq_rate_horas;
-- Esperado: variable, sin duplicados de KEY

-- Misc
SELECT Clave, ValorDecimal FROM eq_param_misc WHERE Clave IN ('FACTOR_REFRIGERACION', 'COSTO_NEVERA', 'PARAFISCALES_PCT');
-- Validar valores reales

-- Base Datos
SELECT * FROM eq_cost_base_datos;
-- Validar costos reales (no 100/200/300)
```

---

## 📝 Checklist General de Extracción

### Pre-Extracción
- [ ] ✅ Acceso al archivo Excel Ipsos EasyQuote 2025v2.xlsm.
- [ ] ✅ Leer esta guía completa.
- [ ] ✅ Preparar carpeta de trabajo (ej: `C:\Temp\EQ_Seeds`).
- [ ] ✅ Tener Excel, editor CSV y SSMS listos.

### Durante Extracción
- [ ] Matriz F2F (Parametros!B4:AI12) → CSV.
- [ ] Matriz CATI (Parametros!80:104) → CSV.
- [ ] Matriz Online (Parametros!94:104) → CSV.
- [ ] Factores (Parametros varias filas) → CSV consolidado.
- [ ] Tabla Horas (hoja Horas) → CSV.
- [ ] Parafiscales (buscar %) → eq_param_misc.
- [ ] Refrigeración (D42) → eq_param_misc.
- [ ] Base Datos costos → eq_cost_base_datos.
- [ ] Codificación tabla completa → CSV.
- [ ] Screenshots de TODAS las fuentes.

### Post-Extracción
- [ ] Validar CSVs (formato, registros, sin duplicados).
- [ ] Crear tablas SQL faltantes (eq_param_cati, eq_param_online, eq_param_factores, eq_rate_horas).
- [ ] Importar CSVs a SQL (via BULK INSERT o Admin UI).
- [ ] Ejecutar queries de validación SQL.
- [ ] Documentar cualquier supuesto o ambigüedad encontrada.
- [ ] Actualizar MIGRACION_EQ_IMPLEMENTACION con estado "Seeds reales cargados".

---

## 🚨 Problemas Comunes y Soluciones

### Problema 1: No encuentro la matriz CATI en Parametros!80:104
**Solución**: Buscar visualmente en la hoja `Parametros` secciones tituladas "CATI", "Telefónico" o similar. Puede estar en otra fila.

### Problema 2: Valores con coma decimal (ej: 1,5 en vez de 1.5)
**Solución**: Excel con configuración regional española. Exportar CSV y buscar/reemplazar "," por "." en editor de texto.

### Problema 3: KEY de tabla Horas no existe como columna
**Solución**: Crear columna calculada en Excel: `=CONCATENAR(A2;"|";B2;"|";C2)` donde A2=SL, B2=RecordDetail, C2=MetodologiaSL.

### Problema 4: Factores no tienen valores numéricos
**Solución**: Inferir factor basado en contexto (ej: "Nuevo"=1.0, "Duplicado"=0.75) y validar con usuario Excel.

### Problema 5: No encuentro porcentaje parafiscales
**Solución**: Buscar en fórmulas de celdas de cálculo de campo F2F. Si no existe, asumir 0% y documentar supuesto.

---

## 📞 Contacto y Soporte

**Si encuentras problemas durante extracción**:
1. Documentar problema con screenshot.
2. Anotar ubicación exacta en Excel (hoja, celda/rango).
3. Hacer supuesto razonable y documentarlo.
4. Continuar con siguiente seed.
5. Lista de problemas/supuestos al final para revisión.

**Entregable final**:
- Carpeta con CSVs: `eq_param_precio_f2f_real.csv`, `eq_param_cati_real.csv`, `eq_param_online_real.csv`, `eq_param_factores_consolidado.csv`, `eq_rate_horas_real.csv`, etc.
- Documento `SUPUESTOS_Y_PROBLEMAS.md` con hallazgos.
- Screenshots carpeta `Screenshots/`.

---

**Última actualización**: 2026-01-05  
**Próxima revisión**: Post-extracción (Sprint 1.1 completado)
