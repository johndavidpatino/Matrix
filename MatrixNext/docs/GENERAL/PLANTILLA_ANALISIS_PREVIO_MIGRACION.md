# 🧩 PLANTILLA BASE – ANÁLISIS PREVIO A MIGRACIÓN (GENÉRICA)

Actúa como **arquitecto/a de software especializado en migraciones ASP.NET WebForms → ASP.NET Core MVC (.NET 8)**.

---

## 🚨 IMPORTANTE
- **No escribas código** en esta fase.
- Tu tarea es **analizar, inventariar y documentar** el módulo **{{NOMBRE_MODULO}}** antes de iniciar su migración.

---

## 🎯 ALCANCE

### Archivos a Analizar (Fase 1)

Analiza **únicamente** los WebForms ubicados en:

```
WebMatrix/{{RUTA_MODULO}}/
```

> La lista concreta de archivos a analizar será proporcionada en cada solicitud.

Para cada archivo `.aspx`:
- Analiza su code-behind asociado (`.vb` / `.cs`)
- Identifica **flujos**, **eventos/postbacks**, **dependencias**, **responsabilidades** y **acoplamientos**

### Fuera de Alcance (Fase 2 – Solo Mención)
- Archivos explícitamente marcados como *“Fase posterior”*
- Archivos `.ascx`, `.master`, helpers secundarios  
  → **Solo inventariarlos si están referenciados**, no analizarlos en detalle

---

## 📖 DIRECTRICES OBLIGATORIAS

Antes de iniciar, **lee y aplica estrictamente**:

1. **DIRECTRICES_MIGRACION.md**  
   Reglas obligatorias de migración (especial atención a reglas 1–10)

2. **ANALISIS_MODULO_REFERENCIA.md**  
   Documento de referencia de **calidad, profundidad y estructura**  
   (ejemplo: `ANALISIS_CU_CUENTAS.md`)

3. **MODULOS_MIGRACION.md**  
   Contexto general del ecosistema y dependencias entre módulos

---

## 🔴 REGLAS CRÍTICAS (OBLIGATORIAS)

### Regla 1: Evidencia Concreta Obligatoria
- **NO inventes** funcionalidades, lógica ni flujos.
- Toda funcionalidad debe tener **evidencia concreta**:
  - Archivo (`.aspx`, `.vb`, `.cs`)
  - Método / evento (`Page_Load`, `btnX_Click`, `RowCommand`, etc.)
  - Stored Procedure / query concreta
- Si algo **no se puede confirmar**:
  - Marcar como: `⚠️ NO ENCONTRADO / POR CONFIRMAR`
  - **NO asumir comportamiento**

### Regla 2: Respeto Absoluto a Nombres de Base de Datos
- Identificar y listar **exactamente**:
  - Tablas
  - Stored Procedures
  - Columnas (respetando casing)
- **NO renombrar, normalizar ni “mejorar”** nombres en esta fase

### Regla 3: Mapeo 1:1 WebForms → MVC
Cada `.aspx` debe mapearse a:
- Controller
- Action(s)
- View
- ViewModel
- Service / DAL

El objetivo es **paridad funcional total**, no rediseño.

### Regla 4: Reutilización de Componentes Existentes
Identificar componentes reutilizables existentes:
- Modales
- Grids
- DatePickers
- Selectores
- CSS / JS compartidos

**NO** proponer componentes nuevos si ya existe uno equivalente.

### Regla 5: Modularización por Área
El módulo debe migrarse dentro de su **Área correspondiente**:

```
Areas/{{AREA}}/Controllers
Areas/{{AREA}}/Views
Data/Services/{{AREA}}
Data/Adapters
```

Registrar el módulo en `Program.cs` si aplica.

---

## 📦 ENTREGABLE ESPERADO

Documento:

```
ANALISIS_{{NOMBRE_MODULO}}.md
```

---

## 🧱 ESTRUCTURA OBLIGATORIA DEL DOCUMENTO

### 1️⃣ Resumen Ejecutivo
- Propósito del módulo
- Usuarios / roles (si se evidencia)
- Dependencias con otros módulos
- Complejidad estimada (🟢🟠🔴 con justificación)

### 2️⃣ Inventario del Legado (Tabla)
Columnas mínimas:
- Archivo
- Funcionalidad
- Eventos clave
- Dependencias
- Estado de evidencia (✅ / ⚠️)

> Diferenciar claramente **confirmado vs inferido**  
> Todo lo inferido debe marcarse ⚠️

### 3️⃣ Flujos Funcionales Detallados
Para cada flujo:
- Pasos secuenciales
- Evidencia por paso
- Validaciones
- Lógica de negocio
- Resultado éxito / error
- Riesgos técnicos detectados

### 4️⃣ Mapa de Migración 1:1
Mapeo explícito entre:
- WebForm original
- Ruta MVC
- Controller / Action
- View
- ViewModel
- Service / DAL
- Nota de paridad funcional

### 5️⃣ Base de Datos y Stored Procedures
- Tablas involucradas
- Stored Procedures utilizados
- Decisión preliminar: **EF Core** vs **Dapper / SP**

### 6️⃣ Riesgos y Consideraciones Técnicas
Evaluar y documentar:
- ViewState
- UpdatePanel
- Session
- Frames
- SP legacy
- Configuración hardcodeada

### 7️⃣ Componentes Reutilizables Existentes
- Qué se puede reutilizar
- Dónde está
- Para qué flujo aplica

### 8️⃣ Backlog Inicial
- P0 / P1 / P2
- Estimación preliminar
- Dependencias técnicas

### 9️⃣ Checklist de Verificación Pre-Migración
Checklist completo para validar que el análisis es suficiente para empezar a codear.

### 🔟 Decisiones Técnicas Clave
Tabla explícita de decisiones (qué se hará y por qué).

### 1️⃣1️⃣ Estimación Preliminar
- Páginas
- Controllers
- Views
- Services
- SP
- Horas / semanas

### 1️⃣2️⃣ Próximos Pasos
Qué hacer inmediatamente después del análisis.

---

## ⚠️ IMPORTANTE SOBRE LA ENTREGA

- **NO generes el documento completo en una sola respuesta**
- Trabaja de forma **incremental**
- Comienza SOLO con las secciones **1️⃣ a 3️⃣**
- Al final pregunta explícitamente:

> **¿Continúo con la siguiente sección?**

En cada respuesta:
- Mantén el mismo archivo lógico `ANALISIS_{{NOMBRE_MODULO}}.md`
- **NO repitas** contenido ya entregado
- Asume que lo previo ya existe

---

## 🎯 OBJETIVO FINAL

El documento debe permitir que:
- Cualquier desarrollador pueda iniciar la migración **sin dudas**
- No existan **sorpresas técnicas**
- El mapeo sea **100% trazable**
- Las estimaciones sean **confiables**
- Los riesgos estén **identificados antes de codear**
