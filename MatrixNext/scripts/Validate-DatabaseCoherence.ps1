# ============================================================
# SCRIPT DE VALIDACIÓN COMPLETA DE COHERENCIA CON BD
# MatrixNext - Validación 100%
# ============================================================
# Ejecutar desde: MatrixNext/
# Requisitos: Acceso a BD CO_Matrix_Intranet
# ============================================================

param(
    [string]$ConnectionString = "Data Source=.\SQLEXPRESS;Initial Catalog=CO_Matrix_Intranet;Integrated Security=True;TrustServerCertificate=True;",
    [string]$OutputPath = "docs\SQL\VALIDACION_COMPLETA",
    [switch]$IncludeLegacyCheck
)

$ErrorActionPreference = "Stop"
$startTime = Get-Date

Write-Host "============================================================" -ForegroundColor Cyan
Write-Host " VALIDACIÓN COMPLETA DE COHERENCIA CON BD" -ForegroundColor Cyan
Write-Host " MatrixNext - $(Get-Date -Format 'yyyy-MM-dd HH:mm')" -ForegroundColor Cyan
Write-Host "============================================================" -ForegroundColor Cyan

# Crear directorio de salida
if (-not (Test-Path $OutputPath)) {
    New-Item -ItemType Directory -Path $OutputPath -Force | Out-Null
}

# ============================================================
# PASO 1: CONECTAR A BD Y EXTRAER OBJETOS EXISTENTES
# ============================================================
Write-Host "`n[PASO 1] Conectando a BD y extrayendo objetos..." -ForegroundColor Yellow

$conn = New-Object System.Data.SqlClient.SqlConnection($ConnectionString)
$conn.Open()
Write-Host "  ✓ Conectado a: $($conn.Database)" -ForegroundColor Green

# Extraer SP de BD
$cmd = $conn.CreateCommand()
$cmd.CommandText = "SELECT SCHEMA_NAME(schema_id) + '.' + name as FullName, name FROM sys.procedures ORDER BY name"
$adapter = New-Object System.Data.SqlClient.SqlDataAdapter($cmd)
$spTable = New-Object System.Data.DataTable
$adapter.Fill($spTable) | Out-Null
$spEnBD = $spTable | ForEach-Object { $_.name }
Write-Host "  ✓ SP en BD: $($spEnBD.Count)" -ForegroundColor Green

# Extraer Tablas de BD
$cmd.CommandText = "SELECT SCHEMA_NAME(schema_id) + '.' + name as FullName, name FROM sys.tables WHERE type = 'U' ORDER BY name"
$adapter = New-Object System.Data.SqlClient.SqlDataAdapter($cmd)
$tableTable = New-Object System.Data.DataTable
$adapter.Fill($tableTable) | Out-Null
$tablasEnBD = $tableTable | ForEach-Object { $_.name }
Write-Host "  ✓ Tablas en BD: $($tablasEnBD.Count)" -ForegroundColor Green

# Extraer Vistas de BD
$cmd.CommandText = "SELECT SCHEMA_NAME(schema_id) + '.' + name as FullName, name FROM sys.views ORDER BY name"
$adapter = New-Object System.Data.SqlClient.SqlDataAdapter($cmd)
$viewTable = New-Object System.Data.DataTable
$adapter.Fill($viewTable) | Out-Null
$vistasEnBD = $viewTable | ForEach-Object { $_.name }
Write-Host "  ✓ Vistas en BD: $($vistasEnBD.Count)" -ForegroundColor Green

# Extraer Columnas de todas las tablas
$cmd.CommandText = @"
SELECT 
    t.name AS TableName,
    c.name AS ColumnName,
    ty.name AS DataType,
    c.max_length,
    c.is_nullable,
    c.is_identity
FROM sys.tables t
INNER JOIN sys.columns c ON t.object_id = c.object_id
INNER JOIN sys.types ty ON c.system_type_id = ty.system_type_id AND c.user_type_id = ty.user_type_id
WHERE t.type = 'U'
ORDER BY t.name, c.column_id
"@
$adapter = New-Object System.Data.SqlClient.SqlDataAdapter($cmd)
$columnTable = New-Object System.Data.DataTable
$adapter.Fill($columnTable) | Out-Null
Write-Host "  ✓ Columnas extraídas: $($columnTable.Rows.Count)" -ForegroundColor Green

# Crear hashtable de columnas por tabla
$columnasPorTabla = @{}
foreach ($row in $columnTable.Rows) {
    $tableName = $row.TableName
    if (-not $columnasPorTabla.ContainsKey($tableName)) {
        $columnasPorTabla[$tableName] = @()
    }
    $columnasPorTabla[$tableName] += $row.ColumnName
}

# Extraer parámetros de SP
$cmd.CommandText = @"
SELECT 
    p.name AS SPName,
    par.name AS ParamName,
    t.name AS DataType,
    par.max_length,
    par.is_output
FROM sys.procedures p
INNER JOIN sys.parameters par ON p.object_id = par.object_id
INNER JOIN sys.types t ON par.system_type_id = t.system_type_id AND par.user_type_id = t.user_type_id
WHERE par.parameter_id > 0
ORDER BY p.name, par.parameter_id
"@
$adapter = New-Object System.Data.SqlClient.SqlDataAdapter($cmd)
$paramTable = New-Object System.Data.DataTable
$adapter.Fill($paramTable) | Out-Null
Write-Host "  ✓ Parámetros de SP extraídos: $($paramTable.Rows.Count)" -ForegroundColor Green

# Crear hashtable de parámetros por SP
$paramsPorSP = @{}
foreach ($row in $paramTable.Rows) {
    $spName = $row.SPName
    if (-not $paramsPorSP.ContainsKey($spName)) {
        $paramsPorSP[$spName] = @()
    }
    $paramsPorSP[$spName] += @{
        Name = $row.ParamName
        Type = $row.DataType
        MaxLength = $row.max_length
        IsOutput = $row.is_output
    }
}

$conn.Close()

# ============================================================
# PASO 2: EXTRAER REFERENCIAS DEL CÓDIGO
# ============================================================
Write-Host "`n[PASO 2] Extrayendo referencias del código MatrixNext..." -ForegroundColor Yellow

# 2.1 Extraer SP referenciados (patrón CommandType.StoredProcedure o ExecuteAsync("SP_NAME"))
$spReferenciados = @{}
$archivosCS = Get-ChildItem -Path "MatrixNext.Data","MatrixNext.Web" -Recurse -Filter "*.cs" -ErrorAction SilentlyContinue

foreach ($archivo in $archivosCS) {
    $contenido = Get-Content $archivo.FullName -Raw
    
    # Buscar llamadas a SP con Dapper
    $matches = [regex]::Matches($contenido, 'ExecuteAsync\s*\(\s*"([^"]+)"')
    foreach ($match in $matches) {
        $spName = $match.Groups[1].Value
        if ($spName -match '^[A-Za-z_][A-Za-z0-9_]+$' -and $spName.Length -gt 3) {
            if (-not $spReferenciados.ContainsKey($spName)) {
                $spReferenciados[$spName] = @()
            }
            $spReferenciados[$spName] += $archivo.FullName
        }
    }
    
    # Buscar QueryAsync
    $matches = [regex]::Matches($contenido, 'QueryAsync[^(]*\(\s*"([^"]+)"')
    foreach ($match in $matches) {
        $spName = $match.Groups[1].Value
        if ($spName -match '^[A-Za-z_][A-Za-z0-9_]+$' -and $spName.Length -gt 3 -and $spName -notmatch '^SELECT|INSERT|UPDATE|DELETE') {
            if (-not $spReferenciados.ContainsKey($spName)) {
                $spReferenciados[$spName] = @()
            }
            $spReferenciados[$spName] += $archivo.FullName
        }
    }
    
    # Buscar CommandType.StoredProcedure con nombre cercano
    if ($contenido -match 'CommandType\.StoredProcedure') {
        $lines = $contenido -split "`n"
        for ($i = 0; $i -lt $lines.Count; $i++) {
            if ($lines[$i] -match 'CommandType\.StoredProcedure' -or $lines[$i] -match 'commandType:\s*CommandType\.StoredProcedure') {
                # Buscar nombre de SP en líneas cercanas
                for ($j = [Math]::Max(0, $i-5); $j -lt [Math]::Min($lines.Count, $i+5); $j++) {
                    if ($lines[$j] -match '"([A-Z][A-Za-z0-9_]+)"' -and $lines[$j] -notmatch '@') {
                        $spName = $matches[1]
                        if ($spName.Length -gt 3 -and $spName -match '_') {
                            if (-not $spReferenciados.ContainsKey($spName)) {
                                $spReferenciados[$spName] = @()
                            }
                            $spReferenciados[$spName] += $archivo.FullName
                        }
                    }
                }
            }
        }
    }
}

Write-Host "  ✓ SP referenciados en código: $($spReferenciados.Count)" -ForegroundColor Green

# 2.2 Extraer Tablas referenciadas (FROM, INTO, UPDATE, JOIN, DbSet)
$tablasReferenciadas = @{}

foreach ($archivo in $archivosCS) {
    $contenido = Get-Content $archivo.FullName -Raw
    
    # Buscar referencias SQL directas
    $patterns = @(
        'FROM\s+\[?dbo\]?\.\[?([A-Za-z_][A-Za-z0-9_]+)\]?',
        'FROM\s+([A-Z][A-Za-z0-9_]+)\s',
        'JOIN\s+\[?dbo\]?\.\[?([A-Za-z_][A-Za-z0-9_]+)\]?',
        'JOIN\s+([A-Z][A-Za-z0-9_]+)\s',
        'INTO\s+\[?dbo\]?\.\[?([A-Za-z_][A-Za-z0-9_]+)\]?',
        'UPDATE\s+\[?dbo\]?\.\[?([A-Za-z_][A-Za-z0-9_]+)\]?',
        'DbSet<[^>]+>\s+(\w+)\s*\{'
    )
    
    foreach ($pattern in $patterns) {
        $matches = [regex]::Matches($contenido, $pattern, [System.Text.RegularExpressions.RegexOptions]::IgnoreCase)
        foreach ($match in $matches) {
            $tableName = $match.Groups[1].Value
            # Filtrar falsos positivos
            if ($tableName -match '^[A-Z]' -and $tableName.Length -gt 2 -and 
                $tableName -notmatch '^(SELECT|WHERE|AND|OR|SET|VALUES|NULL|TRUE|FALSE|INT|VARCHAR|DATETIME)$') {
                if (-not $tablasReferenciadas.ContainsKey($tableName)) {
                    $tablasReferenciadas[$tableName] = @()
                }
                $tablasReferenciadas[$tableName] += $archivo.FullName
            }
        }
    }
}

Write-Host "  ✓ Tablas referenciadas en código: $($tablasReferenciadas.Count)" -ForegroundColor Green

# 2.3 Extraer columnas/propiedades referenciadas
$columnasReferenciadas = @{}

foreach ($archivo in $archivosCS) {
    $contenido = Get-Content $archivo.FullName -Raw
    
    # Buscar propiedades en DTOs/Models que mapean a columnas
    $matches = [regex]::Matches($contenido, '\[Column\("([^"]+)"\)\]')
    foreach ($match in $matches) {
        $colName = $match.Groups[1].Value
        if (-not $columnasReferenciadas.ContainsKey($colName)) {
            $columnasReferenciadas[$colName] = @()
        }
        $columnasReferenciadas[$colName] += $archivo.FullName
    }
    
    # Buscar referencias en queries SQL
    $matches = [regex]::Matches($contenido, '\.\s*([A-Z][a-z]+[A-Za-z0-9]*)\s*[,\s\)]')
    foreach ($match in $matches) {
        $colName = $match.Groups[1].Value
        if ($colName.Length -gt 2 -and $colName -notmatch '^(ToString|GetType|Equals|GetHashCode|Select|Where|OrderBy|First|Any|Count)$') {
            if (-not $columnasReferenciadas.ContainsKey($colName)) {
                $columnasReferenciadas[$colName] = @()
            }
            $columnasReferenciadas[$colName] += $archivo.FullName
        }
    }
}

Write-Host "  ✓ Columnas/Propiedades extraídas: $($columnasReferenciadas.Count)" -ForegroundColor Green

# ============================================================
# PASO 3: VALIDAR SP
# ============================================================
Write-Host "`n[PASO 3] Validando Stored Procedures..." -ForegroundColor Yellow

$spNoExisten = @()
$spExisten = @()
$spConTipoIncorrecto = @()

foreach ($sp in $spReferenciados.Keys) {
    if ($sp -in $spEnBD) {
        $spExisten += @{
            Name = $sp
            Files = $spReferenciados[$sp]
        }
    }
    elseif ($sp -in $tablasEnBD -or $sp -in $vistasEnBD) {
        # Es una tabla o vista, no un SP
        $spConTipoIncorrecto += @{
            Name = $sp
            Type = if ($sp -in $tablasEnBD) { "TABLA" } else { "VISTA" }
            Files = $spReferenciados[$sp]
        }
    }
    else {
        $spNoExisten += @{
            Name = $sp
            Files = $spReferenciados[$sp]
        }
    }
}

Write-Host "  ✓ SP válidos: $($spExisten.Count)" -ForegroundColor Green
Write-Host "  ⚠ SP que son Tablas/Vistas: $($spConTipoIncorrecto.Count)" -ForegroundColor Yellow
Write-Host "  ✗ SP que NO existen: $($spNoExisten.Count)" -ForegroundColor Red

# ============================================================
# PASO 4: VALIDAR TABLAS
# ============================================================
Write-Host "`n[PASO 4] Validando Tablas..." -ForegroundColor Yellow

$tablasNoExisten = @()
$tablasExisten = @()
$tablasSonVistas = @()

foreach ($tabla in $tablasReferenciadas.Keys) {
    if ($tabla -in $tablasEnBD) {
        $tablasExisten += @{
            Name = $tabla
            Files = $tablasReferenciadas[$tabla]
        }
    }
    elseif ($tabla -in $vistasEnBD) {
        $tablasSonVistas += @{
            Name = $tabla
            Files = $tablasReferenciadas[$tabla]
        }
    }
    elseif ($tabla -in $spEnBD) {
        # Es un SP, ignorar
    }
    else {
        # Verificar si es un falso positivo común
        $falsoPositivo = $tabla -match '^(Model|Service|Controller|Adapter|ViewModel|Dto|Result|Entity|Context|Response|Request)$'
        if (-not $falsoPositivo) {
            $tablasNoExisten += @{
                Name = $tabla
                Files = $tablasReferenciadas[$tabla]
            }
        }
    }
}

Write-Host "  ✓ Tablas válidas: $($tablasExisten.Count)" -ForegroundColor Green
Write-Host "  ℹ Referencias a Vistas: $($tablasSonVistas.Count)" -ForegroundColor Cyan
Write-Host "  ✗ Tablas que NO existen: $($tablasNoExisten.Count)" -ForegroundColor Red

# ============================================================
# PASO 5: GENERAR REPORTES
# ============================================================
Write-Host "`n[PASO 5] Generando reportes..." -ForegroundColor Yellow

# Reporte de SP no existentes
$reporteSP = @"
# STORED PROCEDURES NO EXISTENTES EN BD
# Generado: $(Get-Date -Format 'yyyy-MM-dd HH:mm')
# Total: $($spNoExisten.Count)

"@

foreach ($sp in ($spNoExisten | Sort-Object { $_.Name })) {
    $reporteSP += "`n## $($sp.Name)`n"
    $reporteSP += "Archivos:`n"
    foreach ($file in ($sp.Files | Sort-Object -Unique)) {
        $reporteSP += "- $($file.Replace((Get-Location).Path + '\', ''))`n"
    }
}

$reporteSP | Out-File -FilePath "$OutputPath\SP_NO_EXISTEN.md" -Encoding UTF8
Write-Host "  ✓ $OutputPath\SP_NO_EXISTEN.md" -ForegroundColor Green

# Reporte de Tablas no existentes
$reporteTablas = @"
# TABLAS NO EXISTENTES EN BD
# Generado: $(Get-Date -Format 'yyyy-MM-dd HH:mm')
# Total: $($tablasNoExisten.Count)

"@

foreach ($tabla in ($tablasNoExisten | Sort-Object { $_.Name })) {
    $reporteTablas += "`n## $($tabla.Name)`n"
    $reporteTablas += "Archivos:`n"
    foreach ($file in ($tabla.Files | Sort-Object -Unique)) {
        $reporteTablas += "- $($file.Replace((Get-Location).Path + '\', ''))`n"
    }
}

$reporteTablas | Out-File -FilePath "$OutputPath\TABLAS_NO_EXISTEN.md" -Encoding UTF8
Write-Host "  ✓ $OutputPath\TABLAS_NO_EXISTEN.md" -ForegroundColor Green

# Reporte de SP que son Tablas/Vistas (error de tipo)
$reporteTipo = @"
# SP REFERENCIADOS QUE SON TABLAS O VISTAS
# Generado: $(Get-Date -Format 'yyyy-MM-dd HH:mm')
# Total: $($spConTipoIncorrecto.Count)
# NOTA: Estos se usan como SP pero en realidad son Tablas/Vistas

"@

foreach ($item in ($spConTipoIncorrecto | Sort-Object { $_.Name })) {
    $reporteTipo += "`n## $($item.Name) [$($item.Type)]`n"
    $reporteTipo += "Archivos:`n"
    foreach ($file in ($item.Files | Sort-Object -Unique)) {
        $reporteTipo += "- $($file.Replace((Get-Location).Path + '\', ''))`n"
    }
}

$reporteTipo | Out-File -FilePath "$OutputPath\SP_SON_TABLAS_VISTAS.md" -Encoding UTF8
Write-Host "  ✓ $OutputPath\SP_SON_TABLAS_VISTAS.md" -ForegroundColor Green

# Reporte resumen
$resumen = @"
# RESUMEN DE VALIDACIÓN DE COHERENCIA CON BD
# Generado: $(Get-Date -Format 'yyyy-MM-dd HH:mm')
# Base de datos: CO_Matrix_Intranet
# Duración: $([Math]::Round(((Get-Date) - $startTime).TotalSeconds, 2)) segundos

## OBJETOS EN BD

| Tipo | Cantidad |
|------|----------|
| Stored Procedures | $($spEnBD.Count) |
| Tablas | $($tablasEnBD.Count) |
| Vistas | $($vistasEnBD.Count) |
| Columnas totales | $($columnTable.Rows.Count) |

## REFERENCIAS EN CÓDIGO

| Tipo | Referenciados | Válidos | Errores |
|------|---------------|---------|---------|
| SP | $($spReferenciados.Count) | $($spExisten.Count) | $($spNoExisten.Count) |
| Tablas | $($tablasReferenciadas.Count) | $($tablasExisten.Count) | $($tablasNoExisten.Count) |
| SP→Tabla/Vista | - | - | $($spConTipoIncorrecto.Count) |

## ESTADO DE VALIDACIÓN

$(if ($spNoExisten.Count -eq 0 -and $tablasNoExisten.Count -eq 0) {
"✅ **VALIDACIÓN EXITOSA** - Todos los objetos referenciados existen en BD"
} else {
"❌ **VALIDACIÓN FALLIDA** - Hay objetos referenciados que NO existen en BD"
})

## ERRORES A CORREGIR

### SP que NO existen ($($spNoExisten.Count))
$(if ($spNoExisten.Count -gt 0) {
    ($spNoExisten | ForEach-Object { "- ``$($_.Name)``" }) -join "`n"
} else { "Ninguno" })

### Tablas que NO existen ($($tablasNoExisten.Count))
$(if ($tablasNoExisten.Count -gt 0) {
    ($tablasNoExisten | ForEach-Object { "- ``$($_.Name)``" }) -join "`n"
} else { "Ninguno" })

### SP usados como Tabla/Vista ($($spConTipoIncorrecto.Count))
$(if ($spConTipoIncorrecto.Count -gt 0) {
    ($spConTipoIncorrecto | ForEach-Object { "- ``$($_.Name)`` → $($_.Type)" }) -join "`n"
} else { "Ninguno" })

## ARCHIVOS GENERADOS

- ``SP_NO_EXISTEN.md`` - Detalle de SP no existentes
- ``TABLAS_NO_EXISTEN.md`` - Detalle de tablas no existentes
- ``SP_SON_TABLAS_VISTAS.md`` - SP que son tablas/vistas
- ``VALIDACION_RESUMEN.md`` - Este archivo

"@

$resumen | Out-File -FilePath "$OutputPath\VALIDACION_RESUMEN.md" -Encoding UTF8
Write-Host "  ✓ $OutputPath\VALIDACION_RESUMEN.md" -ForegroundColor Green

# ============================================================
# RESUMEN FINAL
# ============================================================
Write-Host "`n============================================================" -ForegroundColor Cyan
Write-Host " RESUMEN DE VALIDACIÓN" -ForegroundColor Cyan
Write-Host "============================================================" -ForegroundColor Cyan

Write-Host "`nObjetos en BD:" -ForegroundColor White
Write-Host "  SP: $($spEnBD.Count) | Tablas: $($tablasEnBD.Count) | Vistas: $($vistasEnBD.Count)"

Write-Host "`nValidación de código:" -ForegroundColor White
Write-Host "  SP referenciados: $($spReferenciados.Count) (válidos: $($spExisten.Count), errores: $($spNoExisten.Count))"
Write-Host "  Tablas referenciadas: $($tablasReferenciadas.Count) (válidas: $($tablasExisten.Count), errores: $($tablasNoExisten.Count))"

if ($spNoExisten.Count -gt 0 -or $tablasNoExisten.Count -gt 0) {
    Write-Host "`n❌ VALIDACIÓN FALLIDA" -ForegroundColor Red
    Write-Host "   Revisar reportes en: $OutputPath" -ForegroundColor Yellow
} else {
    Write-Host "`n✅ VALIDACIÓN EXITOSA" -ForegroundColor Green
}

Write-Host "`nDuración: $([Math]::Round(((Get-Date) - $startTime).TotalSeconds, 2)) segundos" -ForegroundColor Gray
Write-Host "Reportes: $OutputPath\" -ForegroundColor Gray
