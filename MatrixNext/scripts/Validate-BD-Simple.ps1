# ============================================================
# VALIDACION COMPLETA DE COHERENCIA CON BD - MatrixNext
# ============================================================

$connStr = "Data Source=.\SQLEXPRESS;Initial Catalog=CO_Matrix_Intranet;Integrated Security=True;TrustServerCertificate=True;"
$outputPath = "docs\SQL\VALIDACION_COMPLETA"

Write-Host "============================================================" -ForegroundColor Cyan
Write-Host " VALIDACION COMPLETA DE COHERENCIA CON BD" -ForegroundColor Cyan
Write-Host "============================================================" -ForegroundColor Cyan

# Crear directorio
if (-not (Test-Path $outputPath)) {
    New-Item -ItemType Directory -Path $outputPath -Force | Out-Null
}

# Conectar a BD
Write-Host "`n[PASO 1] Conectando a BD..." -ForegroundColor Yellow
$conn = New-Object System.Data.SqlClient.SqlConnection($connStr)
$conn.Open()
Write-Host "  Conectado a: $($conn.Database)" -ForegroundColor Green

# Extraer SP de BD
$cmd = $conn.CreateCommand()
$cmd.CommandText = "SELECT name FROM sys.procedures ORDER BY name"
$reader = $cmd.ExecuteReader()
$spEnBD = @()
while ($reader.Read()) {
    $spEnBD += $reader["name"]
}
$reader.Close()
Write-Host "  SP en BD: $($spEnBD.Count)" -ForegroundColor Green

# Extraer Tablas
$cmd.CommandText = "SELECT name FROM sys.tables WHERE type = 'U' ORDER BY name"
$reader = $cmd.ExecuteReader()
$tablasEnBD = @()
while ($reader.Read()) {
    $tablasEnBD += $reader["name"]
}
$reader.Close()
Write-Host "  Tablas en BD: $($tablasEnBD.Count)" -ForegroundColor Green

# Extraer Vistas
$cmd.CommandText = "SELECT name FROM sys.views ORDER BY name"
$reader = $cmd.ExecuteReader()
$vistasEnBD = @()
while ($reader.Read()) {
    $vistasEnBD += $reader["name"]
}
$reader.Close()
Write-Host "  Vistas en BD: $($vistasEnBD.Count)" -ForegroundColor Green

$conn.Close()

# Extraer SP del codigo
Write-Host "`n[PASO 2] Extrayendo referencias del codigo..." -ForegroundColor Yellow

$spReferenciados = @{}
$archivos = Get-ChildItem -Path "MatrixNext.Data","MatrixNext.Web" -Recurse -Filter "*.cs" -ErrorAction SilentlyContinue

foreach ($archivo in $archivos) {
    $contenido = Get-Content $archivo.FullName -Raw -ErrorAction SilentlyContinue
    if (-not $contenido) { continue }
    
    # Buscar ExecuteAsync, QueryAsync con SP
    $patrones = @(
        'ExecuteAsync\s*\(\s*"([A-Za-z_][A-Za-z0-9_\.]+)"',
        'QueryAsync[^(]*\(\s*"([A-Za-z_][A-Za-z0-9_\.]+)"',
        'ExecuteScalarAsync[^(]*\(\s*"([A-Za-z_][A-Za-z0-9_\.]+)"',
        'Query[^A][^(]*\(\s*"([A-Za-z_][A-Za-z0-9_\.]+)"'
    )
    
    foreach ($patron in $patrones) {
        $matchList = [regex]::Matches($contenido, $patron)
        foreach ($m in $matchList) {
            $spName = $m.Groups[1].Value
            # Filtrar SQL inline
            if ($spName -notmatch '^(SELECT|INSERT|UPDATE|DELETE|FROM|WHERE|SET)' -and $spName.Length -gt 3) {
                # Limpiar schema si existe
                $cleanName = $spName -replace '^[a-z]+\.', ''
                if (-not $spReferenciados.ContainsKey($cleanName)) {
                    $spReferenciados[$cleanName] = @()
                }
                $spReferenciados[$cleanName] += $archivo.Name
            }
        }
    }
}

Write-Host "  SP referenciados: $($spReferenciados.Count)" -ForegroundColor Green

# Validar SP
Write-Host "`n[PASO 3] Validando SP contra BD..." -ForegroundColor Yellow

$spValidos = @()
$spNoExisten = @()
$spSonTablas = @()

foreach ($sp in $spReferenciados.Keys) {
    if ($spEnBD -contains $sp) {
        $spValidos += $sp
    }
    elseif ($tablasEnBD -contains $sp) {
        $spSonTablas += @{ Name = $sp; Tipo = "TABLA"; Archivos = ($spReferenciados[$sp] | Select-Object -Unique) -join ", " }
    }
    elseif ($vistasEnBD -contains $sp) {
        $spSonTablas += @{ Name = $sp; Tipo = "VISTA"; Archivos = ($spReferenciados[$sp] | Select-Object -Unique) -join ", " }
    }
    else {
        $spNoExisten += @{ Name = $sp; Archivos = ($spReferenciados[$sp] | Select-Object -Unique) -join ", " }
    }
}

Write-Host "  SP validos: $($spValidos.Count)" -ForegroundColor Green
Write-Host "  SP son Tablas/Vistas: $($spSonTablas.Count)" -ForegroundColor Yellow
Write-Host "  SP NO existen: $($spNoExisten.Count)" -ForegroundColor Red

# Generar reportes
Write-Host "`n[PASO 4] Generando reportes..." -ForegroundColor Yellow

# Reporte SP no existen
$report = "# SP NO EXISTENTES EN BD`n"
$report += "# Fecha: $(Get-Date -Format 'yyyy-MM-dd HH:mm')`n"
$report += "# Total: $($spNoExisten.Count)`n`n"

foreach ($item in ($spNoExisten | Sort-Object { $_.Name })) {
    $report += "## $($item.Name)`n"
    $report += "Archivos: $($item.Archivos)`n`n"
}

$report | Out-File "$outputPath\SP_NO_EXISTEN.md" -Encoding UTF8
Write-Host "  Creado: $outputPath\SP_NO_EXISTEN.md" -ForegroundColor Green

# Reporte SP que son tablas
$report = "# SP USADOS QUE SON TABLAS O VISTAS`n"
$report += "# Fecha: $(Get-Date -Format 'yyyy-MM-dd HH:mm')`n"
$report += "# Total: $($spSonTablas.Count)`n`n"

foreach ($item in ($spSonTablas | Sort-Object { $_.Name })) {
    $report += "## $($item.Name) [$($item.Tipo)]`n"
    $report += "Archivos: $($item.Archivos)`n`n"
}

$report | Out-File "$outputPath\SP_SON_TABLAS.md" -Encoding UTF8
Write-Host "  Creado: $outputPath\SP_SON_TABLAS.md" -ForegroundColor Green

# Resumen
$report = "# RESUMEN VALIDACION BD`n"
$report += "# Fecha: $(Get-Date -Format 'yyyy-MM-dd HH:mm')`n`n"
$report += "## OBJETOS EN BD`n"
$report += "- SP: $($spEnBD.Count)`n"
$report += "- Tablas: $($tablasEnBD.Count)`n"
$report += "- Vistas: $($vistasEnBD.Count)`n`n"
$report += "## VALIDACION DE CODIGO`n"
$report += "- SP referenciados: $($spReferenciados.Count)`n"
$report += "- SP validos: $($spValidos.Count)`n"
$report += "- SP NO existen: $($spNoExisten.Count)`n"
$report += "- SP son Tablas/Vistas: $($spSonTablas.Count)`n`n"

if ($spNoExisten.Count -eq 0) {
    $report += "## ESTADO: VALIDACION EXITOSA`n"
} else {
    $report += "## ESTADO: ERRORES ENCONTRADOS`n"
    $report += "`nSP que NO existen:`n"
    foreach ($item in ($spNoExisten | Sort-Object { $_.Name })) {
        $report += "- $($item.Name)`n"
    }
}

$report | Out-File "$outputPath\RESUMEN.md" -Encoding UTF8
Write-Host "  Creado: $outputPath\RESUMEN.md" -ForegroundColor Green

# Final
Write-Host "`n============================================================" -ForegroundColor Cyan
Write-Host " RESULTADO" -ForegroundColor Cyan
Write-Host "============================================================" -ForegroundColor Cyan
Write-Host "SP validos: $($spValidos.Count) / $($spReferenciados.Count)"
Write-Host "SP NO existen: $($spNoExisten.Count)" -ForegroundColor $(if ($spNoExisten.Count -gt 0) { "Red" } else { "Green" })
Write-Host "Reportes en: $outputPath\"
