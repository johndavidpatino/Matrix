# Sample Missing Entity Models - Template & Examples

**Purpose:** Template code for creating the 10 missing entity models  
**Created:** January 12, 2026

---

## QUICK TEMPLATE

All EQ entity models follow this pattern:

```csharp
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MatrixNext.Web.Models.EQ
{
    /// <summary>
    /// [Brief description of what this table stores]
    /// Mapea desde Excel [Source sheet if applicable]
    /// </summary>
    [Table("eq_table_name")]
    public class EqEntityName
    {
        [Key]
        public int Id { get; set; }

        // Key properties (for compound key UPSERT)
        [Required]
        [StringLength(100)]
        public string KeyProperty1 { get; set; }

        // Regular properties
        public decimal ValorProperty { get; set; }

        // Timestamps
        public DateTime FechaCreacion { get; set; } = DateTime.UtcNow;
        public DateTime FechaModificacion { get; set; } = DateTime.UtcNow;
    }
}
```

---

## ENTITY #1: EqParamMisc

**Purpose:** Key-value parameter store for miscellaneous EQ settings  
**Table:** eq_param_misc  
**Legacy UPSERT:** Used in UpsertMisc with Clave (key) as compound key

```csharp
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MatrixNext.Web.Models.EQ
{
    /// <summary>
    /// Parametros miscelaneos clave-valor para configuracion general EQ
    /// Mapea valores como PRECIOS_VERSION, VALORHORA_VERSION, otros config
    /// </summary>
    [Table("eq_param_misc")]
    public class EqParamMisc
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(100)]
        public string Clave { get; set; } // e.g., "PRECIOS_VERSION", "VALORHORA_VERSION"

        public decimal? ValorDecimal { get; set; }

        [StringLength(500)]
        public string ValorTexto { get; set; } = string.Empty;

        public DateTime FechaCreacion { get; set; } = DateTime.UtcNow;

        public DateTime FechaModificacion { get; set; } = DateTime.UtcNow;
    }
}
```

---

## ENTITY #2: EqEnvioParam

**Purpose:** Global envios/shipping parameters (single row table)  
**Table:** eq_envio_param  
**Legacy UPSERT:** Checks if EXISTS, then UPDATE or INSERT (single row)

```csharp
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MatrixNext.Web.Models.EQ
{
    /// <summary>
    /// Parametros globales de envio (divisor volumetrico, tipologias)
    /// Solo existe una fila en esta tabla
    /// </summary>
    [Table("eq_envio_param")]
    public class EqEnvioParam
    {
        [Key]
        public int Id { get; set; }

        public decimal DivisorVolumetrico { get; set; }

        [StringLength(100)]
        public string TipologiaUrbano { get; set; } = string.Empty;

        [StringLength(100)]
        public string TipologiaNacional { get; set; } = string.Empty;

        public DateTime FechaCreacion { get; set; } = DateTime.UtcNow;

        public DateTime FechaModificacion { get; set; } = DateTime.UtcNow;
    }
}
```

---

## ENTITY #3: EqEnvioTarifa

**Purpose:** Shipping rates by tipologia  
**Table:** eq_envio_tarifa  
**Legacy UPSERT:** Compound key on (Tipologia)

```csharp
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MatrixNext.Web.Models.EQ
{
    /// <summary>
    /// Tarifas de envio por tipologia (urbano, nacional, etc.)
    /// Mapea desde Excel Valores Insumos reclutamiento
    /// </summary>
    [Table("eq_envio_tarifa")]
    public class EqEnvioTarifa
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(100)]
        public string Tipologia { get; set; } // e.g., "Urbano", "Nacional", "Internacional"

        public decimal KiloInicial { get; set; }

        public decimal KiloAdicional { get; set; }

        public decimal SeguroPct { get; set; }

        public decimal ValorDeclaradoMin { get; set; }

        public DateTime FechaCreacion { get; set; } = DateTime.UtcNow;

        public DateTime FechaModificacion { get; set; } = DateTime.UtcNow;
    }
}
```

---

## ENTITY #4: EqProductividadCiudad

**Purpose:** Productivity metrics by city  
**Table:** eq_productividad_ciudad  
**Legacy UPSERT:** Compound key on (Ciudad)

```csharp
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MatrixNext.Web.Models.EQ
{
    /// <summary>
    /// Productividad (encuestadores y entrevistas por ciudad)
    /// Mapea desde Excel Parametros
    /// </summary>
    [Table("eq_productividad_ciudad")]
    public class EqProductividadCiudad
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(100)]
        public string Ciudad { get; set; }

        public decimal Encuestadores { get; set; }

        public decimal Productividad { get; set; } // entrevistas por encuestador

        public DateTime FechaCreacion { get; set; } = DateTime.UtcNow;

        public DateTime FechaModificacion { get; set; } = DateTime.UtcNow;
    }
}
```

---

## ENTITY #5: EqCodificacionParam

**Purpose:** Coding parameters by scenario  
**Table:** eq_codificacion_param  
**Legacy UPSERT:** Compound key on (Escenario)

```csharp
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MatrixNext.Web.Models.EQ
{
    /// <summary>
    /// Parametros de codificacion por escenario
    /// Mapea desde Excel Tarifario Codificacion
    /// </summary>
    [Table("eq_codificacion_param")]
    public class EqCodificacionParam
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(100)]
        public string Escenario { get; set; } // e.g., "Simple", "Complejo", etc.

        public int Registros { get; set; }

        public int PregAbiertas { get; set; }

        public int PregAbiertasMult { get; set; }

        public decimal Dias { get; set; }

        public decimal Horas { get; set; }

        public decimal ValorIpsos { get; set; }

        public DateTime FechaCreacion { get; set; } = DateTime.UtcNow;

        public DateTime FechaModificacion { get; set; } = DateTime.UtcNow;
    }
}
```

---

## ENTITY #6: EqCostUnitarioOps

**Purpose:** Unit costs for OPS activities  
**Table:** eq_cost_unitario_ops  
**Legacy UPSERT:** Compound key on (CodMatrix)

```csharp
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MatrixNext.Web.Models.EQ
{
    /// <summary>
    /// Costos unitarios de actividades OPS
    /// Mapea desde Excel Tarifario OPS
    /// </summary>
    [Table("eq_cost_unitario_ops")]
    public class EqCostUnitarioOps
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public int CodMatrix { get; set; } // Compound key - unique activity code

        [StringLength(200)]
        public string Actividad { get; set; } = string.Empty;

        public decimal Tarifa { get; set; }

        [StringLength(50)]
        public string Unidad { get; set; } = string.Empty; // "Hora", "Por entrevista", etc.

        public decimal? Horas { get; set; }

        public DateTime FechaCreacion { get; set; } = DateTime.UtcNow;

        public DateTime FechaModificacion { get; set; } = DateTime.UtcNow;
    }
}
```

---

## ENTITY #7: EqTarifaMystery

**Purpose:** Mystery shopping rates by visit type and complexity  
**Table:** eq_tarifa_mystery  
**Legacy UPSERT:** Compound key on (TipoVisita, Complejidad)

```csharp
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MatrixNext.Web.Models.EQ
{
    /// <summary>
    /// Tarifas de mystery shopping por tipo visita y complejidad
    /// Mapea desde Excel MYSTERY sheet
    /// </summary>
    [Table("eq_tarifa_mystery")]
    public class EqTarifaMystery
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(50)]
        public string TipoVisita { get; set; } // Compound key - "Tipo1", "Tipo2", "Tipo3"

        [Required]
        [StringLength(100)]
        public string Complejidad { get; set; } // Compound key - "Baja", "Media", "Alta"

        public decimal VrUnitario { get; set; }

        public int OlasDefault { get; set; } = 1;

        public DateTime FechaCreacion { get; set; } = DateTime.UtcNow;

        public DateTime FechaModificacion { get; set; } = DateTime.UtcNow;
    }
}
```

---

## ENTITY #8: EqCostBaseDatos

**Purpose:** Cost of databases by type  
**Table:** eq_cost_base_datos  
**Legacy UPSERT:** Compound key on (Tipo)

```csharp
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MatrixNext.Web.Models.EQ
{
    /// <summary>
    /// Costos de compra de bases de datos por tipo
    /// Mapea desde Excel Entradas
    /// </summary>
    [Table("eq_cost_base_datos")]
    public class EqCostBaseDatos
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(100)]
        public string Tipo { get; set; } // Compound key - e.g., "BD_Hogares", "BD_Empresas"

        public decimal Valor { get; set; }

        public DateTime FechaCreacion { get; set; } = DateTime.UtcNow;

        public DateTime FechaModificacion { get; set; } = DateTime.UtcNow;
    }
}
```

---

## ENTITY #9: EqParamFactores

**Purpose:** Scaling factors for various calculations  
**Table:** eq_param_factores  
**Legacy Query:** Used in AllFactores() with WHERE Activo=1

```csharp
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MatrixNext.Web.Models.EQ
{
    /// <summary>
    /// Factores de escala y ajuste para calculos EQ
    /// Filtrados por Activo = 1 en queries
    /// </summary>
    [Table("eq_param_factores")]
    public class EqParamFactores
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(50)]
        public string Tipo { get; set; } // e.g., "EconomiaEscala", "Inflacion"

        [Required]
        [StringLength(50)]
        public string Codigo { get; set; } // Compound lookup key - e.g., "2024-Q1"

        [StringLength(200)]
        public string Descripcion { get; set; } = string.Empty;

        public decimal Factor { get; set; }

        [Range(1, 1000)]
        public int Orden { get; set; } = 1;

        public bool Activo { get; set; } = true;

        public DateTime FechaCreacion { get; set; } = DateTime.UtcNow;

        public DateTime FechaModificacion { get; set; } = DateTime.UtcNow;
    }
}
```

---

## ENTITY #10: EqRateHoras

**Purpose:** Minimum hours lookup table by SL/RecordDetail/Methodology  
**Table:** eq_rate_horas  
**Legacy Query:** Used in GetHorasMinimas() with compound key lookup

```csharp
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MatrixNext.Web.Models.EQ
{
    /// <summary>
    /// Tabla de referencia de horas minimas por SL, RecordDetail y Metodologia
    /// Desglosado por nivel OPS (L3-L7)
    /// Mapea desde Excel Rate Horas
    /// </summary>
    [Table("eq_rate_horas")]
    public class EqRateHoras
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(100)]
        public string Key { get; set; } // Compound key formatted as "SL|RecordDetail|MetodologiaSL"

        [StringLength(50)]
        public string SL { get; set; } = string.Empty;

        [StringLength(100)]
        public string RecordDetail { get; set; } = string.Empty;

        [StringLength(50)]
        public string MetodologiaSL { get; set; } = string.Empty;

        public decimal HorasL3 { get; set; }

        public decimal HorasL4 { get; set; }

        public decimal HorasL5 { get; set; }

        public decimal HorasL6 { get; set; }

        public decimal HorasL7 { get; set; }

        public DateTime FechaCreacion { get; set; } = DateTime.UtcNow;

        public DateTime FechaModificacion { get; set; } = DateTime.UtcNow;
    }
}
```

---

## STEP-BY-STEP CREATION INSTRUCTIONS

### 1. Create Entity Files

```bash
# Navigate to Models/EQ folder
cd MatrixNext.Web\Models\EQ

# Create all 10 files using templates above
# Files to create:
# - EqParamMisc.cs
# - EqEnvioParam.cs
# - EqEnvioTarifa.cs
# - EqProductividadCiudad.cs
# - EqCodificacionParam.cs
# - EqCostUnitarioOps.cs
# - EqTarifaMystery.cs
# - EqCostBaseDatos.cs
# - EqParamFactores.cs
# - EqRateHoras.cs
```

**Estimated Time:** 30 minutes (copy/paste + minor edits)

### 2. Register DbSets in MatrixDbContext

Add to `MatrixDbContext.cs` after line 56:

```csharp
// ===== EQ: MAESTRAS (CONTINUED) =====
public DbSet<EqParamMisc> EqParamMiscs { get; set; }
public DbSet<EqEnvioParam> EqEnvioParams { get; set; }
public DbSet<EqEnvioTarifa> EqEnvioTarifas { get; set; }
public DbSet<EqProductividadCiudad> EqProductividadCiudads { get; set; }
public DbSet<EqCodificacionParam> EqCodificacionParams { get; set; }
public DbSet<EqCostUnitarioOps> EqCostUnitarioOps { get; set; }
public DbSet<EqTarifaMystery> EqTarifaMysteries { get; set; }
public DbSet<EqCostBaseDatos> EqCostBaseDatos { get; set; }
public DbSet<EqParamFactores> EqParamFactores { get; set; }
public DbSet<EqRateHoras> EqRateHoras { get; set; }
```

**Estimated Time:** 5 minutes

### 3. Create Migration

```bash
cd c:\Users\johnd\source\repos\johndavidpatino\Matrix\MatrixNext

dotnet ef migrations add AddEQMasterTablesPhase2 -c MatrixDbContext -p MatrixNext.Web

# This will generate Migration file in MatrixNext.Web\Migrations\
```

**Estimated Time:** 1 minute

### 4. Review Generated Migration

```bash
# List migrations
dotnet ef migrations list

# Review last migration (should create 10 new tables)
# File: MatrixNext.Web\Migrations\[timestamp]_AddEQMasterTablesPhase2.cs
```

### 5. Update Database

```bash
# Dev/Staging database
dotnet ef database update -c MatrixDbContext

# Verify tables created
# SELECT TABLE_NAME FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME LIKE 'eq_%'
```

**Estimated Time:** 2 minutes

### 6. Add to EasyQuoteAdminServiceEF (Optional)

You can extend `EasyQuoteAdminServiceEF.cs` with UPSERT methods for these entities:

```csharp
public async Task<OperationResult> UpsertParamMiscAsync(EqParamMisc misc)
{
    var existing = await _context.EqParamMiscs
        .FirstOrDefaultAsync(p => p.Clave == misc.Clave);

    if (existing != null)
    {
        existing.ValorDecimal = misc.ValorDecimal;
        existing.ValorTexto = misc.ValorTexto;
        existing.FechaModificacion = DateTime.UtcNow;
        _context.Update(existing);
    }
    else
    {
        misc.FechaCreacion = DateTime.UtcNow;
        misc.FechaModificacion = DateTime.UtcNow;
        _context.Add(misc);
    }

    await _context.SaveChangesAsync();
    return new OperationResult { Success = true, Message = "ParamMisc guardado" };
}
```

---

## VALIDATION AFTER CREATION

Run these queries to verify all tables were created:

```sql
-- Check all EQ tables exist
SELECT TABLE_NAME FROM INFORMATION_SCHEMA.TABLES 
WHERE TABLE_SCHEMA = 'dbo' AND TABLE_NAME LIKE 'eq_%'
ORDER BY TABLE_NAME;

-- Expected output (should have 23 tables total):
-- eq_codificacion_param
-- eq_cost_base_datos
-- eq_cost_insumos
-- eq_cost_result
-- eq_cost_unitario_ops
-- eq_envio_param
-- eq_envio_tarifa
-- eq_locaciones
-- eq_methodology
-- eq_mystery
-- eq_param_factores
-- eq_param_misc
-- eq_param_precio
-- eq_param_script_proc
-- eq_productividad_ciudad
-- eq_questionnaire
-- eq_quote_header
-- eq_rate_estadistica
-- eq_rate_horas
-- eq_sample_city
-- eq_staff_sl
-- eq_tarifa_mystery
-- eq_valor_hora_ops

-- Check column names match entities
SELECT COLUMN_NAME, DATA_TYPE 
FROM INFORMATION_SCHEMA.COLUMNS 
WHERE TABLE_NAME = 'eq_param_misc'
ORDER BY ORDINAL_POSITION;

-- Expected columns:
-- Id (int)
-- Clave (nvarchar(100))
-- ValorDecimal (decimal(18,2) or NULL)
-- ValorTexto (nvarchar(500))
-- FechaCreacion (datetime2)
-- FechaModificacion (datetime2)
```

---

## TOTAL EFFORT ESTIMATE

| Task | Time |
|------|------|
| Create 10 entity files | 30 min |
| Add DbSet registrations | 5 min |
| Create migration | 1 min |
| Update database | 2 min |
| Validation & testing | 15 min |
| **TOTAL** | **~1 hour** |

---

## SUMMARY

All 10 missing entities follow the same pattern:
- [Key] Id
- Compound key property/properties
- Other data properties
- FechaCreacion & FechaModificacion timestamps

Once created, EasyQuoteAdminServiceEF can be easily extended with UPSERT methods for each.

**Created:** January 12, 2026
