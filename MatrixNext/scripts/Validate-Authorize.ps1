# ==================================================================================
# Script: Validate-Authorize.ps1
# Propósito: Validar que todos los Controllers tengan [Authorize] attribute
# Autor: GitHub Copilot - MatrixNext Migration Team
# Fecha: 2026-01-16
# Sprint: 21 Semana 1 - Fase 1 (Tarea A3)
# ==================================================================================

param(
    [string]$ControllersPath = ".\MatrixNext.Web\Areas",
    [string]$GlobalControllersPath = ".\MatrixNext.Web\Controllers",
    [switch]$ExportReport,
    [string]$ReportPath = ".\reports\AUTHORIZE_VALIDATION_REPORT.md",
    [string[]]$ExcludeControllers = @("ErrorController", "HomeController")  # Controllers que no requieren auth
)

Write-Host "=====================================================================" -ForegroundColor Cyan
Write-Host "    VALIDACIÓN DE [Authorize] - MatrixNext Controllers" -ForegroundColor Cyan
Write-Host "=====================================================================" -ForegroundColor Cyan
Write-Host ""

# Función para analizar un controller
function Test-ControllerAuthorization {
    param(
        [System.IO.FileInfo]$File,
        [string[]]$Excluded
    )
    
    $content = Get-Content $File.FullName -Raw
    $className = $File.BaseName
    
    # Verificar si está en lista de exclusión
    if ($Excluded -contains $className) {
        return @{
            File = $File.Name
            Path = $File.FullName
            ClassName = $className
            HasAuthorize = $null  # Excluido
            Status = "⚪ Excluido"
            Area = (Split-Path (Split-Path $File.DirectoryName -Parent) -Leaf)
        }
    }
    
    # Buscar [Authorize] en el archivo
    $hasAuthorize = $content -match '\[Authorize[^\]]*\]'
    
    # Determinar área
    $area = "Global"
    if ($File.FullName -match '\\Areas\\([^\\]+)\\') {
        $area = $matches[1]
    }
    
    return @{
        File = $File.Name
        Path = $File.FullName
        ClassName = $className
        HasAuthorize = $hasAuthorize
        Status = if ($hasAuthorize) { "✅ Con [Authorize]" } else { "❌ SIN [Authorize]" }
        Area = $area
    }
}

# Función principal de escaneo
function Get-ControllersAuthorizationStatus {
    param(
        [string]$AreaPath,
        [string]$GlobalPath,
        [string[]]$Excluded
    )
    
    Write-Host "[1/3] Escaneando controllers en áreas..." -ForegroundColor Yellow
    
    $results = @()
    
    # Escanear controllers de áreas
    if (Test-Path $AreaPath) {
        $areaControllers = Get-ChildItem -Path $AreaPath -Recurse -Filter "*Controller.cs"
        
        $total = $areaControllers.Count
        $current = 0
        
        foreach ($file in $areaControllers) {
            $current++
            Write-Progress -Activity "Analizando controllers de áreas" -Status "$current de $total" -PercentComplete (($current / $total) * 100)
            
            $result = Test-ControllerAuthorization -File $file -Excluded $Excluded
            $results += $result
        }
        
        Write-Progress -Activity "Analizando controllers de áreas" -Completed
        Write-Host "   ✅ Analizados $total controllers de áreas" -ForegroundColor Green
    }
    
    # Escanear controllers globales
    Write-Host "[2/3] Escaneando controllers globales..." -ForegroundColor Yellow
    
    if (Test-Path $GlobalPath) {
        $globalControllers = Get-ChildItem -Path $GlobalPath -Filter "*Controller.cs"
        
        foreach ($file in $globalControllers) {
            $result = Test-ControllerAuthorization -File $file -Excluded $Excluded
            $results += $result
        }
        
        Write-Host "   ✅ Analizados $($globalControllers.Count) controllers globales" -ForegroundColor Green
    }
    
    return $results
}

# Función para generar estadísticas
function Get-AuthorizationStatistics {
    param([array]$Results)
    
    Write-Host "[3/3] Generando estadísticas..." -ForegroundColor Yellow
    
    $withAuth = ($Results | Where-Object { $_.HasAuthorize -eq $true }).Count
    $withoutAuth = ($Results | Where-Object { $_.HasAuthorize -eq $false }).Count
    $excluded = ($Results | Where-Object { $_.HasAuthorize -eq $null }).Count
    $total = $Results.Count
    
    $stats = @{
        Total = $total
        WithAuthorize = $withAuth
        WithoutAuthorize = $withoutAuth
        Excluded = $excluded
        Percentage = if (($total - $excluded) -gt 0) { 
            [math]::Round(($withAuth / ($total - $excluded)) * 100, 2) 
        } else { 
            100 
        }
    }
    
    # Agrupar por área
    $byArea = $Results | Group-Object -Property Area | ForEach-Object {
        $areaName = $_.Name
        $areaItems = $_.Group
        
        @{
            Area = $areaName
            Total = $areaItems.Count
            WithAuth = ($areaItems | Where-Object { $_.HasAuthorize -eq $true }).Count
            WithoutAuth = ($areaItems | Where-Object { $_.HasAuthorize -eq $false }).Count
            Excluded = ($areaItems | Where-Object { $_.HasAuthorize -eq $null }).Count
        }
    }
    
    $stats['ByArea'] = $byArea
    
    Write-Host "   ✅ Estadísticas generadas" -ForegroundColor Green
    
    return $stats
}

# Función para exportar reporte
function Export-AuthorizationReport {
    param(
        [array]$Results,
        [hashtable]$Stats,
        [string]$OutputPath
    )
    
    $reportDir = Split-Path $OutputPath -Parent
    if (-not (Test-Path $reportDir)) {
        New-Item -Path $reportDir -ItemType Directory -Force | Out-Null
    }
    
    $report = @"
# 🔒 REPORTE DE VALIDACIÓN - [Authorize] ATTRIBUTE

**Fecha Generación**: $(Get-Date -Format "yyyy-MM-dd HH:mm:ss")  
**Sprint**: 21 Semana 1 - Fase 1 (Tarea A3)  
**Generado por**: Validate-Authorize.ps1

---

## 🎯 RESUMEN EJECUTIVO

| Métrica | Valor | Estado |
|---------|-------|--------|
| **Total Controllers** | $($Stats.Total) | - |
| **Con [Authorize]** | $($Stats.WithAuthorize) | $(if ($Stats.WithoutAuthorize -eq 0) { '✅' } else { '⚠️' }) |
| **SIN [Authorize]** | $($Stats.WithoutAuthorize) | $(if ($Stats.WithoutAuthorize -eq 0) { '✅' } else { '❌' }) |
| **Excluidos (permitido)** | $($Stats.Excluded) | ⚪ |
| **% Cobertura** | $($Stats.Percentage)% | $(if ($Stats.Percentage -eq 100) { '✅' } else { '❌' }) |

---

## 📊 ESTADÍSTICAS POR ÁREA

| Área | Total | Con [Authorize] | SIN [Authorize] | Excluidos | % Cobertura |
|------|-------|-----------------|-----------------|-----------|-------------|
"@

    foreach ($area in $Stats.ByArea | Sort-Object -Property Area) {
        $coverage = if (($area.Total - $area.Excluded) -gt 0) {
            [math]::Round(($area.WithAuth / ($area.Total - $area.Excluded)) * 100, 2)
        } else {
            100
        }
        
        $statusIcon = if ($area.WithoutAuth -eq 0) { '✅' } else { '❌' }
        
        $report += "`n| $($area.Area) | $($area.Total) | $($area.WithAuth) | $($area.WithoutAuth) | $($area.Excluded) | $coverage% $statusIcon |"
    }

    # Controllers SIN [Authorize]
    $withoutAuth = $Results | Where-Object { $_.HasAuthorize -eq $false }
    
    if ($withoutAuth.Count -gt 0) {
        $report += @"


---

## ❌ CONTROLLERS SIN [Authorize] - ACCIÓN REQUERIDA ($($withoutAuth.Count))

| # | Controller | Área | Ruta Archivo |
|---|------------|------|--------------|
"@

        $counter = 1
        foreach ($item in $withoutAuth | Sort-Object -Property Area, ClassName) {
            $relativePath = $item.Path -replace [regex]::Escape((Get-Location).Path), "."
            $report += "`n| $counter | ``$($item.ClassName)`` | $($item.Area) | $relativePath |"
            $counter++
        }
        
        $report += @"


---

## 🚨 ACCIONES CRÍTICAS REQUERIDAS

### Para Controllers SIN [Authorize]:

1. **Agregar [Authorize] a nivel de clase**:
   ``````csharp
   [Area("NombreArea")]
   [Authorize]  // ← AGREGAR ESTA LÍNEA
   public class NombreController : Controller
   {
       // ...
   }
   ``````

2. **Si requiere permisos específicos**:
   ``````csharp
   [Authorize(Roles = "Admin,User")]
   // O
   [Authorize(Policy = "RequireAdminRole")]
   ``````

3. **Si algún método debe ser público** (raro):
   ``````csharp
   [Area("NombreArea")]
   [Authorize]
   public class NombreController : Controller
   {
       [AllowAnonymous]  // ← Solo para métodos específicos
       public IActionResult PublicAction()
       {
           // ...
       }
   }
   ``````

---

## ⚠️ BLOQUEANTE PARA PRODUCCIÓN

Este reporte identifica **$($withoutAuth.Count) controllers sin [Authorize]** que deben corregirse antes del Go-Live.

**Asignado a**: DEV Team  
**Tiempo estimado**: $([math]::Ceiling($withoutAuth.Count * 0.1)) horas (6 min/controller)  
**Prioridad**: 🔴 CRÍTICA

"@
    } else {
        $report += @"


---

## ✅ VALIDACIÓN EXITOSA

Todos los controllers tienen [Authorize] attribute (excepto los explícitamente excluidos).

**Estado**: 🟢 APROBADO - Cumple directriz de seguridad

"@
    }

    # Controllers Excluidos
    $excludedItems = $Results | Where-Object { $_.HasAuthorize -eq $null }
    
    if ($excludedItems.Count -gt 0) {
        $report += @"


---

## ⚪ CONTROLLERS EXCLUIDOS (Permitido sin [Authorize])

| # | Controller | Área | Motivo |
|---|------------|------|--------|
"@

        $counter = 1
        foreach ($item in $excludedItems | Sort-Object -Property ClassName) {
            $motivo = switch ($item.ClassName) {
                "HomeController" { "Página de inicio pública" }
                "ErrorController" { "Manejo de errores globales" }
                "LoginController" { "Autenticación pública" }
                default { "Explícitamente excluido" }
            }
            
            $report += "`n| $counter | ``$($item.ClassName)`` | $($item.Area) | $motivo |"
            $counter++
        }
    }

    $report += @"


---

**Generado por**: ``scripts/Validate-Authorize.ps1``  
**Comando ejecutado**: ``.\Validate-Authorize.ps1 -ExportReport``  
**Directriz**: Regla #11 - Validar permisos con [Authorize] en TODOS los controllers
"@

    $report | Out-File -FilePath $OutputPath -Encoding UTF8
    
    Write-Host "   ✅ Reporte exportado: $OutputPath" -ForegroundColor Green
}

# ==================================================================================
# EJECUCIÓN PRINCIPAL
# ==================================================================================

try {
    # Escanear controllers
    $results = Get-ControllersAuthorizationStatus -AreaPath $ControllersPath -GlobalPath $GlobalControllersPath -Excluded $ExcludeControllers
    
    if ($results.Count -eq 0) {
        Write-Host "⚠️  ADVERTENCIA: No se encontraron controllers para analizar" -ForegroundColor Yellow
        exit 0
    }
    
    # Generar estadísticas
    $stats = Get-AuthorizationStatistics -Results $results
    
    Write-Host ""
    Write-Host "=====================================================================" -ForegroundColor Cyan
    Write-Host "                         RESULTADOS FINALES" -ForegroundColor Cyan
    Write-Host "=====================================================================" -ForegroundColor Cyan
    Write-Host ""
    Write-Host "  Total Controllers:       $($stats.Total)" -ForegroundColor White
    Write-Host "  Con [Authorize]:         $($stats.WithAuthorize)" -ForegroundColor Green
    Write-Host "  SIN [Authorize]:         $($stats.WithoutAuthorize)" -ForegroundColor $(if ($stats.WithoutAuthorize -eq 0) { "Green" } else { "Red" })
    Write-Host "  Excluidos:               $($stats.Excluded)" -ForegroundColor Gray
    Write-Host "  Cobertura:               $($stats.Percentage)%" -ForegroundColor $(if ($stats.Percentage -eq 100) { "Green" } else { "Red" })
    Write-Host ""
    
    if ($stats.WithoutAuthorize -eq 0) {
        Write-Host "  ✅ VALIDACIÓN EXITOSA" -ForegroundColor Green
        Write-Host "  Todos los controllers tienen [Authorize]" -ForegroundColor Green
    } else {
        Write-Host "  ❌ VALIDACIÓN FALLIDA" -ForegroundColor Red
        Write-Host "  $($stats.WithoutAuthorize) controllers SIN [Authorize] requieren corrección" -ForegroundColor Red
        
        Write-Host ""
        Write-Host "  Controllers afectados:" -ForegroundColor Yellow
        $withoutAuth = $results | Where-Object { $_.HasAuthorize -eq $false } | Sort-Object -Property Area, ClassName
        foreach ($item in $withoutAuth) {
            Write-Host "     - [$($item.Area)] $($item.ClassName)" -ForegroundColor Yellow
        }
    }
    
    Write-Host ""
    Write-Host "=====================================================================" -ForegroundColor Cyan
    Write-Host ""
    
    # Exportar reporte si se solicita
    if ($ExportReport) {
        Export-AuthorizationReport -Results $results -Stats $stats -OutputPath $ReportPath
    }
    
    # Exit code basado en resultados
    if ($stats.WithoutAuthorize -gt 0) {
        exit 1  # Validación fallida
    } else {
        exit 0  # Validación exitosa
    }
}
catch {
    Write-Host ""
    Write-Host "❌ ERROR CRÍTICO: $($_.Exception.Message)" -ForegroundColor Red
    Write-Host "   Stack Trace: $($_.ScriptStackTrace)" -ForegroundColor Gray
    exit 1
}
