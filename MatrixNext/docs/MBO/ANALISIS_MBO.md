# ANÁLISIS DEL MÓDULO MBO (Management By Objectives)

> **Sprint**: 16  
> **Prioridad**: BAJA  
> **Complejidad Estimada**: ALTA  
> **Fecha de Análisis**: 2026-01-15  
> **Tipo**: Dashboards y Visualizaciones (Gerenciales + Operacionales)

---

## 📋 RESUMEN EJECUTIVO

### Descripción del Módulo

MBO (Management By Objectives) es un módulo de **dashboards gerenciales y operacionales** que muestra indicadores clave de rendimiento (KPIs) para la dirección, gerencia y operaciones. Incluye visualizaciones mediante **FusionCharts** (gráficos interactivos) y reportes de gestión en tiempo real.

### Alcance de la Migración

- **Páginas a Migrar**: 18 + Default.aspx (dashboard principal)
- **Stored Procedures Identificados**: 30+ SP
- **Subsistemas**: 
  1. **AOT (Achievement of Tasks)** - Gestión por Objetivos (4 páginas)
  2. **Campo** - Indicadores de Operaciones de Campo (6 páginas)
  3. **Propuestas/Gestión** - Gestión de Propuestas y Transacciones Matrix (8 páginas)
- **Características Especiales**:
  - Visualizaciones dinámicas (gráficos, gauges, cylindros)
  - Carga masiva de errores de campo vía Excel
  - Cálculo de métricas en tiempo real

---

## 📁 ESTRUCTURA DE PÁGINAS

### Sub-Módulo 1: AOT (Achievement of Tasks) - 4 páginas

| Página | Descripción | Permiso ID | Funcionalidad Principal |
|--------|-------------|------------|------------------------|
| `AOTDireccion.aspx` | Dashboard AOT para Dirección | 23 | Visualiza Budget vs Ejecución AOT por año/mes para todas las unidades |
| `AOTGerencia.aspx` | Dashboard AOT por Gerencia | - | Visualiza Budget vs Ejecución AOT filtrado por unidad/gerente |
| `AOTPorGerentes.aspx` | Dashboard AOT desagregado por Gerentes | - | Muestra desempeño AOT individual de gerentes de cuenta |
| `GerenciaAOT.aspx` | Vista AOT para unidad específica (GET param) | - | Visualiza ejecución AOT para una gerencia/unidad específica |

#### Características del Subsistema AOT:
- **Métrica Principal**: AOT = Achievement of Tasks (Logro de Objetivos)
- **Categorías**: Action, Watch, Booster
- **Visualizaciones**: Gauges angulares, gráficos de barras 3D, cilindros
- **Filtros**: Año, Mes, Unidad, Gerente

### Sub-Módulo 2: Campo (Operaciones de Campo) - 6 páginas

| Página | Descripción | Permiso ID | Funcionalidad Principal |
|--------|-------------|------------|------------------------|
| `CampoCalidadCiudad.aspx` | Calidad de Campo por Ciudad | 50 | Muestra índice de calidad por ciudad (% errores) |
| `CampoCalidadTotal.aspx` | Calidad Total de Campo | 49 | Gauge de calidad total anual + histórico mensual |
| `CampoEncuestadores.aspx` | Rendimiento de Encuestadores | 51 | Ranking de encuestadores por muestra/errores/índice calidad |
| `CampoErroresUnEstudio.aspx` | Errores por Estudio | - | Detalle de errores de un trabajo específico |
| `CampoProduccion.aspx` | Producción de Campo | 48 | Dashboard de encuestas aprobadas vs realizadas (total + por metodología) |
| `CargarErrores.aspx` | Carga Masiva de Errores | - | Upload Excel con errores de campo → inserción a BD |

#### Características del Subsistema Campo:
- **Métrica Principal**: Índice de Calidad = (Errores / Muestra) * 100
- **Visualizaciones**: Gauges (semáforo verde/amarillo/rojo), cilindros, gráficos de barras
- **Funcionalidad Especial**: Upload de Excel con validación y carga masiva

### Sub-Módulo 3: Propuestas y Gestión - 8 páginas

| Página | Descripción | Permiso ID | Funcionalidad Principal |
|--------|-------------|------------|------------------------|
| `PropuestasEstadoTotal.aspx` | Propuestas por Estado (Total) | - | Gráficos de propuestas creadas/enviadas/aprobadas por unidad |
| `PropuestasEstadoUnidad.aspx` | Propuestas por Estado (Unidad) | - | Similar a Total pero filtrado por unidad específica |
| `PropuestasSinTrabajo.aspx` | Propuestas Aprobadas sin Trabajo | - | Lista propuestas aprobadas que no tienen trabajo asignado |
| `MatrixGestion.aspx` | Transacciones Matrix | - | Dashboard de Briefs → Propuestas → Presupuestos → Estudios → Proyectos → Trabajos |
| `EncuestasAlaFecha.aspx` | Encuestas a la Fecha (Ajax para refresh) | - | Retorna valor actual de encuestas realizadas (usado por CampoProduccion) |
| `IndicesManualesCuentas.aspx` | Índices y Manuales | - | Dashboard de índices de manuales de cuentas (no detallado en código) |
| `ProductoNoConformeRegistrar.aspx` | Registro de Producto No Conforme | - | Formulario para registrar productos no conformes (SGC - Sistema de Gestión de Calidad) |
| `Default.aspx` | Dashboard Principal MBO | 23 | Página de entrada al módulo (redirige o muestra menú) |

---

## 🗄️ STORED PROCEDURES IDENTIFICADOS

### Grupo 1: AOT (Achievement of Tasks) - 6 SP

| Stored Procedure | Parámetros | Descripción | Usado en |
|------------------|------------|-------------|----------|
| `MBO_PGAOTBudgetEjecucionAñoMes` | `@Año`, `@Mes`, `@Sigla` | Retorna Budget vs Ejecución AOT por año/mes para una unidad | `AOTDireccion.aspx`, `AOTGerencia.aspx` |
| `MBO_PGAOTBudgetMetaTotal` | `@Sigla` | Retorna Budget y Meta total anual para una unidad | `AOTDireccion.aspx`, `GerenciaAOT.aspx` |
| `MBO_PGAOTEjecucionTotal` | `@Año`, `@Mes`, `@Sigla` | Retorna ejecución AOT total a la fecha | `AOTDireccion.aspx` |
| `MBO_PGAOTBudgetEjecucionUnidad` | `@Año`, `@Mes`, `@Sigla` | Retorna Budget vs Ejecución por unidad desagregada | `AOTDireccion.aspx` |
| `MBO_PGAOTPorUnidadGerente` | `@Año`, `@Mes`, `@Sigla` | Retorna AOT desagregado por unidad y gerente | `AOTPorGerentes.aspx` |
| `MBO_AOTAcquisition` | `@Sigla` | Retorna datos de adquisición AOT | `AOTDireccion.aspx` |

### Grupo 2: Campo (Operaciones) - 14 SP

| Stored Procedure | Parámetros | Descripción | Usado en |
|------------------|------------|-------------|----------|
| `MBO_CUEncuestasAprobadas` | `@Año`, `@Mes` | Retorna total encuestas aprobadas (target) | `CampoProduccion.aspx` |
| `MBO_CUEncuestasAprobadasAñoMes` | `@Año`, `@Mes` | Retorna encuestas aprobadas por año/mes | - |
| `MBO_OPCampoEncuestasAlaFecha` | `@Año`, `@Mes` | Retorna encuestas realizadas a la fecha | `CampoProduccion.aspx`, `EncuestasAlaFecha.aspx` |
| `MBO_CUEncuestasAprobadasPorMetodologia` | `@Año`, `@Mes` | Retorna encuestas aprobadas desagregadas por metodología | `CampoProduccion.aspx` |
| `MBO_OPCampoEncuestasAlaFechaPorMetodologia` | `@Año`, `@Mes` | Retorna encuestas realizadas por metodología | `CampoProduccion.aspx` |
| `MBO_OPCampoCalidadTotal` | `@Año` | Retorna índice de calidad total del año (actual + anterior) | `CampoCalidadTotal.aspx` |
| `MBO_OPCampoCalidadMesTotal` | `@Año`, `@Mes` | Retorna índice de calidad por mes del año | `CampoCalidadTotal.aspx` |
| `MBO_OPCampoCiudadTotal` | `@Año` | Retorna índice de calidad por ciudad (año completo) | `CampoCalidadCiudad.aspx` |
| `MBO_OPCampoCiudadMes` | `@Año`, `@Mes` | Retorna índice de calidad por ciudad y mes | `CampoCalidadCiudad.aspx` |
| `MBO_TrabajosConErrores` | - | Vista que retorna trabajos con errores cargados | `CampoErroresUnEstudio.aspx` |
| `MBO_OPCampoErroresUnEstudio` | `@Estudio` | Retorna errores de un estudio específico | `CampoErroresUnEstudio.aspx` |
| `MBO_OPCampoEncuestadores` | `@Año`, `@Mes`, `@Encuestador` | Retorna ranking de encuestadores por rendimiento | `CampoEncuestadores.aspx` |
| `MBO_OPCampoMuestraErroresMesEncuestador` | `@Año`, `@Mes`, `@Encuestador` | Retorna muestra y errores por encuestador en un mes | `CampoEncuestadores.aspx` |
| `MBO_OPCampoExisteEstudioEncuesta` | `@Estudio`, `@Encuesta` | Valida si existe combinación estudio/encuesta (para evitar duplicados en carga) | `CargarErrores.aspx` (clase Errores.vb) |

### Grupo 3: Propuestas y Gestión - 10 SP

| Stored Procedure | Parámetros | Descripción | Usado en |
|------------------|------------|-------------|----------|
| `MBO_PropuestasCreadasEnviadasSinAnuncioActualizar` | `@IdUnidad` | Retorna propuestas creadas/enviadas y las que faltan actualizar por unidad | `PropuestasEstadoTotal.aspx`, `PropuestasEstadoUnidad.aspx` |
| `MBO_PropuestasCreadasEnviadasSinAnuncioGC` | `@IdUnidad` | Retorna propuestas creadas/enviadas por Gerente de Cuentas | `PropuestasEstadoTotal.aspx` |
| `MBO_PropuestasAltaProbabilidadPorActualizar` | `@IdUnidad` | Retorna propuestas de alta probabilidad sin actualizar | `PropuestasEstadoTotal.aspx` |
| `MBO_PropuestasAprobadasSinAnuncioActualizar` | `@IdUnidad` | Retorna propuestas aprobadas sin anuncio y sin actualizar | `PropuestasEstadoTotal.aspx` |
| `MBO_PropuestasAprobadasSinTrabajoPorUnidad` | `@IdUnidad` | Retorna propuestas aprobadas sin trabajo asignado (por unidad) | `PropuestasSinTrabajo.aspx` |
| `MBO_PropuestasAprobadasSinTrabajoUnidadMetodo` | `@IdUnidad`, `@Metodologia` | Retorna propuestas aprobadas sin trabajo (filtro unidad + metodología) | `PropuestasSinTrabajo.aspx` |
| `MBO_PropuestasAprobadasSinTrabajo` | - | Retorna todas las propuestas aprobadas sin trabajo | `PropuestasSinTrabajo.aspx` |
| `MBO_PGGestionMatrix` | - | Retorna conteo de transacciones Matrix (Briefs, Propuestas, Presupuestos, Estudios, Proyectos, Trabajos) | `MatrixGestion.aspx` |
| `MBO_ObtenerUnidadesUsuario` | `@IdUsuario` | Retorna unidades asociadas a un usuario (para filtrar dashboards) | `AOTDireccion.aspx`, otros |
| `MBO_PGIndicesManuales` | - | Retorna índices de manuales de cuentas | `IndicesManualesCuentas.aspx` |

---

## 📊 FLUJOS DE NEGOCIO PRINCIPALES

### Flujo 1: Dashboard AOT para Dirección

```
1. Usuario accede a /MBO/AOTDireccion
2. Sistema valida permiso 23
3. Sistema obtiene unidades del usuario (MBO_ObtenerUnidadesUsuario)
4. Usuario selecciona mes/año
5. Sistema carga datos:
   - Budget vs Ejecución Total (MBO_PGAOTBudgetEjecucionAñoMes)
   - Meta Total (MBO_PGAOTBudgetMetaTotal)
   - Ejecución por Unidad (MBO_PGAOTBudgetEjecucionUnidad)
   - AOT Acquisition (MBO_AOTAcquisition)
6. Sistema genera visualizaciones FusionCharts:
   - Gauge angular para % logro total
   - Gráfico de barras 3D por unidad
   - Cilindros para Action/Watch/Booster
7. Retorna vista con gráficos interactivos
```

### Flujo 2: Dashboard Calidad Total de Campo

```
1. Usuario accede a /MBO/CampoCalidadTotal
2. Sistema valida permiso 49
3. Sistema obtiene año actual y anterior
4. Sistema carga datos:
   - Calidad Total Año Actual + Anterior (MBO_OPCampoCalidadTotal)
   - Calidad por Mes del Año (MBO_OPCampoCalidadMesTotal)
5. Sistema genera visualizaciones:
   - 2 Gauges angulares (año actual vs anterior) con semáforo:
     * Verde: 0-6% errores
     * Amarillo: 6-10% errores
     * Rojo: 10-30% errores
   - Gráfico de línea mensual
6. Retorna vista con gráficos
```

### Flujo 3: Carga Masiva de Errores de Campo

```
1. Usuario accede a /MBO/CargarErrores
2. Usuario carga archivo Excel con errores
3. Sistema lee Excel con clase Errores.vb:
   - Valida estructura del archivo
   - Lee fila por fila (desde fila 5)
   - Por cada fila:
     a. Valida si existe Estudio/Encuesta (MBO_OPCampoExisteEstudioEncuesta)
     b. Si NO existe:
        - Inserta registro en tabla de errores
        - Incrementa contador de encuestas cargadas
     c. Si existe:
        - Salta registro (evita duplicados)
        - Incrementa contador de encuestas NO cargadas
4. Sistema retorna mensaje con resumen:
   - Encuestas cargadas: X
   - Encuestas NO cargadas: Y (duplicadas)
5. Muestra notificación (clase Notificacion.vb)
```

### Flujo 4: Dashboard de Propuestas por Estado

```
1. Usuario accede a /MBO/PropuestasEstadoTotal
2. Sistema carga datos de propuestas:
   - Creadas/Enviadas por Unidad (MBO_PropuestasCreadasEnviadasSinAnuncioActualizar)
   - Creadas/Enviadas por Gerente (MBO_PropuestasCreadasEnviadasSinAnuncioGC)
   - Alta Probabilidad sin Actualizar (MBO_PropuestasAltaProbabilidadPorActualizar)
   - Aprobadas sin Anuncio (MBO_PropuestasAprobadasSinAnuncioActualizar)
3. Sistema genera gráficos FusionCharts:
   - Gráfico de barras 3D: Creadas/Enviadas vs Por Actualizar (por unidad)
   - Gráfico de barras 3D: Alta Probabilidad vs Por Actualizar
   - Gráfico de barras 3D: Creadas/Enviadas por Gerente de Cuentas
4. Retorna vista con 3+ gráficos interactivos
```

---

## 🛠️ COMPONENTES COMPARTIDOS

### Clase: Errores.vb

**Ubicación**: `WebMatrix/MBO/Errores.vb`

**Responsabilidad**: Carga masiva de errores desde Excel

**Métodos Principales**:
```vb
Public Sub InsertarErrores(ByVal dt As DataSet)
```

**Proceso**:
1. Abre transacción de BD
2. Lee DataSet fila por fila
3. Valida existencia de Estudio/Encuesta (`MBO_OPCampoExisteEstudioEncuesta`)
4. Inserta registro si no existe
5. Commit o rollback de transacción

**Stored Procedure Usado**:
- `MBO_OPCampoExisteEstudioEncuesta(Estudio, Encuesta)` → Retorna filas si existe

### Clase: VOErrores.vb

**Ubicación**: `WebMatrix/MBO/VOErrores.vb`

**Responsabilidad**: Value Object para errores de campo

**Propiedades**:
```vb
Public Año As Integer
Public Mes As Integer
Public Unidad As String
Public Estudio As Decimal
Public Encuesta As Decimal
Public Ciudad As Decimal
Public Encuestador As Decimal
Public Supervisor As Decimal
Public Error1 As Integer
Public Pregunta1 As String
Public Error2 As Integer
Public Pregunta2 As String
Public Error3 As Integer
Public Pregunta3 As String
... (hasta Error6/Pregunta6)
```

### Clase: Notificacion.vb

**Ubicación**: `WebMatrix/MBO/Notificacion.vb`

**Responsabilidad**: Mostrar notificaciones al usuario (Toast o Alert)

**Método**:
```vb
Public Sub ShowNotification(mensaje As String, tipo As ShowNotifications)
```

**Tipos de Notificación**:
- `InfoNotification`
- `SuccessNotification`
- `ErrorNotification`

---

## 🎨 TECNOLOGÍAS Y LIBRERÍAS

### FusionCharts (Gráficos Interactivos)

**Versión**: FusionCharts v3 (Flash-based)

**Tipos de Gráficos Usados**:
- `AngularGauge.swf` - Gauge con aguja (usado en Calidad)
- `Cylinder.swf` - Cilindro vertical (usado en Producción)
- `MSColumn3D.swf` - Gráfico de barras multi-serie 3D
- `Column3D.swf` - Gráfico de barras simple 3D

**Ejemplo de Uso en WebMatrix**:
```vb
Dim xml As New StringBuilder()
xml.Append("<chart caption='Calidad' lowerLimit='0' upperLimit='30'>")
xml.Append("<colorRange>")
xml.Append("<color minValue='0' maxValue='6' code='8BBA00'/>") ' Verde
xml.Append("<color minValue='6' maxValue='10' code='F6BD0F'/>") ' Amarillo
xml.Append("<color minValue='10' maxValue='30' code='FF654F'/>") ' Rojo
xml.Append("</colorRange>")
xml.Append("<dials><dial value='" & indice & "'/></dials>")
xml.Append("</chart>")

ChartLiteral.Text = FusionCharts.RenderChart(
    "../FusionWidgets/AngularGauge.swf", 
    "", 
    xml.ToString(), 
    "chartid1", 
    "300", 
    "200", 
    False, 
    True
)
```

**⚠️ DECISIÓN TÉCNICA PARA MATRIXNEXT**:

FusionCharts v3 usa Flash (deprecated). Para MatrixNext usar:
1. **Chart.js** (open-source, modern, HTML5)
2. **ApexCharts** (open-source, interactivo, responsive)
3. **Highcharts** (licencia comercial, muy potente)

**Recomendación**: **Chart.js** (gratuito, amplia comunidad, fácil integración con ASP.NET Core)

---

## 🔄 MAPEO DE VISTAS (WebMatrix → MatrixNext)

### Dashboard Structure Proposal

```
MatrixNext.Web/Areas/MBO/
├── Controllers/
│   ├── HomeController.cs                    # Dashboard principal MBO (Index)
│   ├── AOTController.cs                     # 4 acciones (Direccion, Gerencia, PorGerentes, GerenciaAOT)
│   ├── CampoController.cs                   # 6 acciones (CalidadTotal, CalidadCiudad, Encuestadores, etc.)
│   └── PropuestasController.cs              # 8 acciones (EstadoTotal, EstadoUnidad, SinTrabajo, MatrixGestion, etc.)
│
├── Views/
│   ├── Home/
│   │   └── Index.cshtml                     # Dashboard principal con cards a subsistemas
│   │
│   ├── AOT/
│   │   ├── Direccion.cshtml                 # Dashboard AOT Dirección (gauges + gráficos)
│   │   ├── Gerencia.cshtml                  # Dashboard AOT Gerencia
│   │   ├── PorGerentes.cshtml               # Dashboard AOT por Gerentes
│   │   └── GerenciaAOT.cshtml               # Dashboard AOT unidad específica
│   │
│   ├── Campo/
│   │   ├── CalidadTotal.cshtml              # Gauges calidad total
│   │   ├── CalidadCiudad.cshtml             # Gráficos calidad por ciudad
│   │   ├── Encuestadores.cshtml             # Ranking encuestadores
│   │   ├── ErroresUnEstudio.cshtml          # Tabla errores de estudio
│   │   ├── Produccion.cshtml                # Dashbo

ard producción (encuestas aprobadas vs realizadas)
│   │   └── CargarErrores.cshtml             # Form upload Excel + resultado carga
│   │
│   └── Propuestas/
│       ├── EstadoTotal.cshtml               # Gráficos propuestas (total)
│       ├── EstadoUnidad.cshtml              # Gráficos propuestas (filtro unidad)
│       ├── SinTrabajo.cshtml                # Tabla propuestas sin trabajo
│       ├── MatrixGestion.cshtml             # Dashboard transacciones Matrix
│       ├── IndicesManuales.cshtml           # Dashboard índices manuales
│       └── ProductoNoConforme.cshtml        # Form registro producto no conforme
```

### Routing Proposal

```csharp
// MatrixNext.Web/Program.cs
app.MapAreaControllerRoute(
    name: "mbo_area",
    areaName: "MBO",
    pattern: "MBO/{controller=Home}/{action=Index}/{id?}");
```

---

## ⚠️ RIESGOS Y DESAFÍOS TÉCNICOS

### 1. Deprecación de FusionCharts Flash

**Riesgo**: Alto  
**Impacto**: Alto (todas las visualizaciones usan Flash)

**Solución**:
- Migrar a Chart.js o ApexCharts
- Crear componentes reutilizables de gráficos:
  - `_GaugeChart.cshtml` (Partial View con Chart.js gauge)
  - `_BarChart.cshtml` (Partial View con Chart.js bar chart)
  - `_CylinderChart.cshtml` (Partial View con custom CSS + Chart.js)

**Ejemplo de Componente Gauge en Chart.js**:
```cshtml
@* Views/Shared/_GaugeChart.cshtml *@
@model GaugeChartViewModel

<div>
    <canvas id="@Model.CanvasId"></canvas>
</div>

<script>
const ctx = document.getElementById('@Model.CanvasId');
new Chart(ctx, {
    type: 'doughnut',
    data: {
        datasets: [{
            data: [@Model.Value, @(Model.MaxValue - Model.Value)],
            backgroundColor: ['@Model.Color', '#e0e0e0'],
            circumference: 180,
            rotation: 270
        }]
    },
    options: {
        responsive: true,
        plugins: {
            legend: { display: false },
            tooltip: { enabled: false }
        }
    }
});
</script>
```

### 2. Carga Masiva de Excel

**Riesgo**: Medio  
**Impacto**: Medio (funcionalidad crítica para Campo)

**Solución**:
- Usar `EPPlus` o `ClosedXML` para leer Excel en .NET Core
- Validar estructura del archivo antes de procesar
- Procesar en background con `IHostedService` si archivo es grande
- Retornar progreso vía SignalR (opcional)

**Ejemplo con EPPlus**:
```csharp
// MatrixNext.Core/Services/MBO/ErroresCargaService.cs
public async Task<(int cargadas, int duplicadas)> CargarErroresAsync(Stream excelStream)
{
    using var package = new ExcelPackage(excelStream);
    var worksheet = package.Workbook.Worksheets[0];
    
    int cargadas = 0, duplicadas = 0;
    
    // Leer desde fila 6 (fila 5 en 1-based index)
    for (int row = 6; row <= worksheet.Dimension.End.Row; row++)
    {
        var estudio = worksheet.Cells[row, 4].Value?.ToString();
        var encuesta = worksheet.Cells[row, 7].Value?.ToString();
        
        // Validar si existe
        bool existe = await _adapter.ExisteEstudioEncuestaAsync(estudio, encuesta);
        
        if (!existe)
        {
            var error = new ErrorCampoDto
            {
                Año = Convert.ToInt32(worksheet.Cells[row, 1].Value),
                Mes = Convert.ToInt32(worksheet.Cells[row, 2].Value),
                Estudio = Convert.ToDecimal(estudio),
                Encuesta = Convert.ToDecimal(encuesta),
                // ... mapear otras columnas
            };
            
            await _adapter.InsertarErrorAsync(error);
            cargadas++;
        }
        else
        {
            duplicadas++;
        }
    }
    
    return (cargadas, duplicadas);
}
```

### 3. Datos en Tiempo Real (Refresh Automático)

**Riesgo**: Bajo  
**Impacto**: Bajo (solo `EncuestasAlaFecha.aspx` usa refresh cada 3 segundos)

**Solución**:
- Usar JavaScript `setInterval()` para polling HTTP
- **Mejor**: Implementar SignalR para push real-time
- Endpoint: `GET /MBO/Campo/EncuestasAlaFecha` (retorna JSON)

**Ejemplo con JavaScript Fetch API**:
```javascript
// Views/Campo/Produccion.cshtml
setInterval(async () => {
    const response = await fetch('/MBO/Campo/EncuestasAlaFecha?año=2026&mes=1');
    const data = await response.json();
    
    // Actualizar gráfico Chart.js
    chartInstance.data.datasets[0].data[0] = data.encuestasRealizadas;
    chartInstance.update();
}, 3000); // Cada 3 segundos
```

### 4. Parámetros de Unidad/Gerencia

**Riesgo**: Bajo  
**Impacto**: Medio (filtros dinámicos por usuario)

**Solución**:
- Obtener unidades del usuario logueado con `MBO_ObtenerUnidadesUsuario(userId)`
- Dropdown en vista para seleccionar unidad
- Filtrar datos en backend según selección

---

## 📦 ENTIDADES Y DTOs NECESARIOS

### DTOs para AOT

```csharp
// MatrixNext.Data/Models/MBO/AOTBudgetEjecucionDto.cs
public class AOTBudgetEjecucionDto
{
    public int Año { get; set; }
    public int Mes { get; set; }
    public string Sigla { get; set; }
    public long BudgetTotal { get; set; }
    public long MetaTotal { get; set; }
    public long AOTTotal { get; set; }
    public long BudgetAction { get; set; }
    public long MetaAction { get; set; }
    public long BudgetWatch { get; set; }
    public long MetaWatch { get; set; }
    public long BudgetBooster { get; set; }
    public long MetaBooster { get; set; }
}

public class AOTUnidadDto
{
    public string Unidad { get; set; }
    public long BudgetUnidad { get; set; }
    public long MetaUnidad { get; set; }
    public long ActualUnidad { get; set; }
    public decimal PorcentajeLogro { get; set; }
}
```

### DTOs para Campo

```csharp
// MatrixNext.Data/Models/MBO/CampoEncuestasDto.cs
public class CampoEncuestasDto
{
    public int Año { get; set; }
    public int Mes { get; set; }
    public int EncuestasAprobadas { get; set; }
    public int EncuestasRealizadas { get; set; }
    public decimal PorcentajeAvance { get; set; }
}

public class CampoEncuestasPorMetodologiaDto
{
    public int GrupoInforme { get; set; }
    public string Metodologia { get; set; }
    public int Encuestas { get; set; }
}

public class CampoCalidadDto
{
    public int Año { get; set; }
    public decimal Muestra { get; set; }
    public decimal Errores { get; set; }
    public decimal IndiceCalidad { get; set; } // (Errores / Muestra) * 100
}

public class CampoCiudadDto
{
    public string Ciudad { get; set; }
    public decimal Muestra { get; set; }
    public decimal Errores { get; set; }
    public decimal IndiceCalidad { get; set; }
}

public class CampoEncuestadorDto
{
    public int Encuestador { get; set; }
    public string NombreEncuestador { get; set; }
    public decimal Muestra { get; set; }
    public decimal Errores { get; set; }
    public decimal IndiceCalidad { get; set; }
}

public class ErrorCampoDto
{
    public int Año { get; set; }
    public int Mes { get; set; }
    public string Unidad { get; set; }
    public decimal Estudio { get; set; }
    public decimal Encuesta { get; set; }
    public decimal Ciudad { get; set; }
    public decimal Encuestador { get; set; }
    public decimal Supervisor { get; set; }
    public int? Error1 { get; set; }
    public string Pregunta1 { get; set; }
    public int? Error2 { get; set; }
    public string Pregunta2 { get; set; }
    public int? Error3 { get; set; }
    public string Pregunta3 { get; set; }
    public int? Error4 { get; set; }
    public string Pregunta4 { get; set; }
    public int? Error5 { get; set; }
    public string Pregunta5 { get; set; }
    public int? Error6 { get; set; }
    public string Pregunta6 { get; set; }
}
```

### DTOs para Propuestas

```csharp
// MatrixNext.Data/Models/MBO/PropuestasDto.cs
public class PropuestasEstadoDto
{
    public string GrupoUnidad { get; set; }
    public int PropuestasEnGestion { get; set; }
    public int PropuestasPorActualizar { get; set; }
    public decimal PorcentajeSinActualizar { get; set; }
}

public class PropuestasPorGerenteDto
{
    public string Nombres { get; set; }
    public string Apellidos { get; set; }
    public int PropuestasEnGestion { get; set; }
    public int PropuestasPorActualizar { get; set; }
}

public class PropuestaSinTrabajoDto
{
    public int IdPropuesta { get; set; }
    public string NombrePropuesta { get; set; }
    public string Cliente { get; set; }
    public string Unidad { get; set; }
    public string Metodologia { get; set; }
    public DateTime FechaAprobacion { get; set; }
    public int DiasSinTrabajo { get; set; }
}

public class GestionMatrixDto
{
    public int Brief { get; set; }
    public int Propuestas { get; set; }
    public int Presupuestos { get; set; }
    public int Estudios { get; set; }
    public int Proyectos { get; set; }
    public int Trabajos { get; set; }
}
```

---

## 🎯 PRIORIDAD DE IMPLEMENTACIÓN

### Fase 1: AOT (Achievement of Tasks) - ALTA PRIORIDAD

**Justificación**: Dashboards ejecutivos, alta visibilidad

**Páginas**:
1. `AOTDireccion.aspx` → `AOTController.Direccion()`
2. `AOTGerencia.aspx` → `AOTController.Gerencia()`
3. `AOTPorGerentes.aspx` → `AOTController.PorGerentes()`
4. `GerenciaAOT.aspx` → `AOTController.GerenciaAOT(id)`

**SP a implementar**: 6 SP del grupo AOT

**Estimación**: 2-3 días (con gráficos Chart.js)

### Fase 2: Campo (Operaciones) - MEDIA PRIORIDAD

**Justificación**: Operaciones críticas, carga de datos masiva

**Páginas**:
1. `CampoProduccion.aspx` → `CampoController.Produccion()`
2. `CampoCalidadTotal.aspx` → `CampoController.CalidadTotal()`
3. `CampoCalidadCiudad.aspx` → `CampoController.CalidadCiudad()`
4. `CampoEncuestadores.aspx` → `CampoController.Encuestadores()`
5. `CampoErroresUnEstudio.aspx` → `CampoController.ErroresUnEstudio(id)`
6. `CargarErrores.aspx` → `CampoController.CargarErrores()` (POST con Excel)

**SP a implementar**: 14 SP del grupo Campo

**Estimación**: 3-4 días (con upload Excel + validaciones)

### Fase 3: Propuestas y Gestión - BAJA PRIORIDAD

**Justificación**: Dashboards gerenciales, menos críticos

**Páginas**:
1. `MatrixGestion.aspx` → `PropuestasController.MatrixGestion()`
2. `PropuestasEstadoTotal.aspx` → `PropuestasController.EstadoTotal()`
3. `PropuestasEstadoUnidad.aspx` → `PropuestasController.EstadoUnidad()`
4. `PropuestasSinTrabajo.aspx` → `PropuestasController.SinTrabajo()`
5. `IndicesManualesCuentas.aspx` → `PropuestasController.IndicesManuales()`
6. `ProductoNoConformeRegistrar.aspx` → `PropuestasController.ProductoNoConforme()` (CRUD)
7. `EncuestasAlaFecha.aspx` → `CampoController.EncuestasAlaFecha()` (AJAX endpoint)
8. `Default.aspx` → `HomeController.Index()`

**SP a implementar**: 10 SP del grupo Propuestas

**Estimación**: 2-3 días

---

## ✅ CHECKLIST DE IMPLEMENTACIÓN

### Pre-Implementación

- [x] Análisis de páginas WebMatrix completado
- [x] Identificación de stored procedures completada
- [x] Mapeo de flujos de negocio completado
- [ ] Validar que todos los SP existen en BD staging
- [ ] Documentar parámetros de cada SP
- [ ] Crear maquetas UI para gráficos (Chart.js)

### Implementación Fase 1 (AOT)

- [ ] Crear DTOs para AOT
- [ ] Crear `IAOTAdapter` + `AOTAdapter` con Dapper (6 métodos async)
- [ ] Crear `IAOTService` + `AOTService` (lógica de cálculo de % logro)
- [ ] Crear `AOTController` (4 acciones)
- [ ] Crear vistas con Chart.js:
  - [ ] `AOT/Direccion.cshtml` (Gauges + gráficos de barras)
  - [ ] `AOT/Gerencia.cshtml`
  - [ ] `AOT/PorGerentes.cshtml`
  - [ ] `AOT/GerenciaAOT.cshtml`
- [ ] Registrar DI en `Program.cs`
- [ ] Testing funcional (validar métricas vs WebMatrix)

### Implementación Fase 2 (Campo)

- [ ] Crear DTOs para Campo
- [ ] Crear `ICampoAdapter` + `CampoAdapter` (14 métodos async)
- [ ] Crear `ICampoService` + `CampoService` (cálculo de índices, validaciones)
- [ ] Crear `CampoController` (6 acciones + 1 AJAX endpoint)
- [ ] Crear vistas con Chart.js:
  - [ ] `Campo/Produccion.cshtml` (Cilindros encuestas)
  - [ ] `Campo/CalidadTotal.cshtml` (Gauges semáforo)
  - [ ] `Campo/CalidadCiudad.cshtml` (Gráficos de barras)
  - [ ] `Campo/Encuestadores.cshtml` (Tabla ranking)
  - [ ] `Campo/ErroresUnEstudio.cshtml` (Tabla errores)
  - [ ] `Campo/CargarErrores.cshtml` (Form upload + progreso)
- [ ] Implementar carga masiva de Excel con EPPlus
- [ ] Registrar DI en `Program.cs`
- [ ] Testing funcional (validar datos vs WebMatrix)

### Implementación Fase 3 (Propuestas)

- [ ] Crear DTOs para Propuestas
- [ ] Crear `IPropuestasAdapter` + `PropuestasAdapter` (10 métodos async)
- [ ] Crear `IPropuestasService` + `PropuestasService`
- [ ] Crear `PropuestasController` (8 acciones)
- [ ] Crear vistas:
  - [ ] `Propuestas/MatrixGestion.cshtml` (Gráfico de barras)
  - [ ] `Propuestas/EstadoTotal.cshtml` (Múltiples gráficos)
  - [ ] `Propuestas/EstadoUnidad.cshtml`
  - [ ] `Propuestas/SinTrabajo.cshtml` (Tabla filtrable)
  - [ ] `Propuestas/IndicesManuales.cshtml`
  - [ ] `Propuestas/ProductoNoConforme.cshtml` (Form CRUD)
- [ ] Registrar DI en `Program.cs`
- [ ] Testing funcional

### Post-Implementación

- [ ] Actualizar menú en `_Sidebar.cshtml`
- [ ] Crear `MIGRACION_MBO_COMPLETADA.md`
- [ ] Actualizar `DASHBOARD_MIGRACION.md`
- [ ] Commit y push a Git
- [ ] Code review
- [ ] Deploy a staging
- [ ] Testing de aceptación con usuarios

---

## 📝 NOTAS ADICIONALES

### Consideraciones de Rendimiento

- Los dashboards cargan **múltiples SP simultáneamente** (hasta 5-6 en una sola página)
- **Solución**: Ejecutar queries en paralelo con `Task.WhenAll()` en el Service

```csharp
// Ejemplo: AOTService.ObtenerDatosDireccionAsync()
public async Task<AOTDireccionViewModel> ObtenerDatosDireccionAsync(int año, int mes, string sigla)
{
    var tasks = new[]
    {
        _adapter.ObtenerBudgetEjecucionAsync(año, mes, sigla),
        _adapter.ObtenerMetaTotalAsync(sigla),
        _adapter.ObtenerEjecucionTotalAsync(año, mes, sigla),
        _adapter.ObtenerBudgetPorUnidadAsync(año, mes, sigla),
        _adapter.ObtenerAOTAcquisitionAsync(sigla)
    };
    
    await Task.WhenAll(tasks);
    
    return new AOTDireccionViewModel
    {
        BudgetEjecucion = await tasks[0],
        MetaTotal = await tasks[1],
        EjecucionTotal = await tasks[2],
        UnidadesDetalle = await tasks[3],
        Acquisition = await tasks[4]
    };
}
```

### Permisos Identificados

| Permiso ID | Descripción | Aplica a |
|------------|-------------|----------|
| 23 | Acceso a MBO (Default, AOTDireccion) | Default.aspx, AOTDireccion.aspx |
| 48 | Acceso a Campo Producción | CampoProduccion.aspx |
| 49 | Acceso a Campo Calidad Total | CampoCalidadTotal.aspx |
| 50 | Acceso a Campo Calidad por Ciudad | CampoCalidadCiudad.aspx |
| 51 | Acceso a Campo Encuestadores | CampoEncuestadores.aspx |

---

**Documento generado**: 2026-01-15  
**Analista**: GitHub Copilot (Claude Sonnet 4.5)  
**Próximo paso**: Implementación Fase 1 (AOT)
