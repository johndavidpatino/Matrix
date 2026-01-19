# Script para eliminar warnings CS0168 (variable 'ex' declarada pero no usada)
param(
    [string]$RootPath = "."
)

$files = Get-ChildItem -Path $RootPath -Recurse -Filter "*.cs" -File | 
    Where-Object { $_.FullName -notlike "*\obj\*" -and $_.FullName -notlike "*\bin\*" }

$modifiedCount = 0
$totalFixed = 0

foreach ($file in $files) {
    $lines = Get-Content $file.FullName
    $modified = $false
    
    for ($i = 0; $i -lt $lines.Count; $i++) {
        # Detectar catch (Exception ex) o catch (SqlException ex)
        if ($lines[$i] -match '^\s*catch \((Exception|SqlException) ex\)\s*$') {
            $exceptionType = $matches[1]
            
            # Buscar si 'ex' se usa en el bloque catch
            $usesEx = $false
            $bracketCount = 0
            $inCatchBlock = $false
            
            for ($j = $i+1; $j -lt $lines.Count; $j++) {
                $currentLine = $lines[$j]
                
                # Contar llaves
                if ($currentLine -match '\{') { 
                    $bracketCount++ 
                    $inCatchBlock = $true
                }
                if ($currentLine -match '\}') { 
                    $bracketCount--
                    if ($bracketCount -eq -1) { break }  # Fin del bloque catch
                }
                
                # Verificar si usa 'ex' (logger, throw, etc)
                if ($inCatchBlock -and ($currentLine -match '\bex\b' -and $currentLine -notmatch '^\s*catch')) {
                    $usesEx = $true
                    break
                }
            }
            
            # Si no usa 'ex', removerla
            if (-not $usesEx) {
                $lines[$i] = $lines[$i] -replace ' ex\)', ')'
                $modified = $true
                $totalFixed++
                Write-Host "  Fixed: $($file.Name):$($i+1)" -ForegroundColor Yellow
            }
        }
    }
    
    if ($modified) {
        $lines | Set-Content $file.FullName -Encoding UTF8
        $modifiedCount++
        Write-Host "Modified: $($file.Name)" -ForegroundColor Green
    }
}

Write-Host "`nSummary:" -ForegroundColor Cyan
Write-Host "  Files modified: $modifiedCount" -ForegroundColor Green
Write-Host "  Total CS0168 fixed: $totalFixed" -ForegroundColor Green
