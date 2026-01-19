# Script para corregir warnings CS8618 agregando 'required' modifier
param(
    [string]$RootPath = "."
)

$files = Get-ChildItem -Path $RootPath -Recurse -Filter "*Dto.cs" -File | 
    Where-Object { $_.FullName -notlike "*\obj\*" -and $_.FullName -notlike "*\bin\*" }

$modifiedCount = 0
$totalFixed = 0

foreach ($file in $files) {
    $lines = Get-Content $file.FullName
    $modified = $false
    
    for ($i = 0; $i -lt $lines.Count; $i++) {
        $line = $lines[$i]
        
        # Detectar propiedades public string/int/bool/etc sin init y non-nullable
        # Patrón: public tipo Nombre { get; set; }
        if ($line -match '^\s*public\s+(string|int|long|bool|decimal|DateTime|Guid)(\??)?\s+(\w+)\s*\{\s*get;\s*set;\s*\}\s*$') {
            $tipo = $matches[1]
            $esNullable = $matches[2]
            $propiedad = $matches[3]
            
            # Si NO es nullable (no tiene ?) y NO tiene 'required'
            if ([string]::IsNullOrEmpty($esNullable) -and $line -notmatch '\brequired\b') {
                # Agregar 'required' antes de 'public'
                $lines[$i] = $line -replace '^\s*public\s+', '    public required '
                $modified = $true
                $totalFixed++
            }
        }
    }
    
    if ($modified) {
        $lines | Set-Content $file.FullName -Encoding UTF8
        $modifiedCount++
        Write-Host "Modified: $($file.Name) ($totalFixed properties)" -ForegroundColor Green
    }
}

Write-Host "`nSummary:" -ForegroundColor Cyan
Write-Host "  Files modified: $modifiedCount" -ForegroundColor Green
Write-Host "  Total CS8618 fixed: $totalFixed" -ForegroundColor Green
