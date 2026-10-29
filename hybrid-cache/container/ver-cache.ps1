# Mostra o conteúdo de uma chave do HybridCache no Redis como JSON formatado.
# O HybridCache grava um cabeçalho binário antes do JSON; este script descarta esse cabeçalho.
#
# Uso:
#   .\container\ver-cache.ps1            -> lista as chaves da API
#   .\container\ver-cache.ps1 articles   -> mostra a lista de artigos
#   .\container\ver-cache.ps1 article:1  -> mostra o artigo 1

param([string]$Key)

$container = "hybrid-cache-redis"
[Console]::OutputEncoding = [System.Text.Encoding]::UTF8

if (-not $Key) {
    Write-Host "Chaves no Redis:" -ForegroundColor Cyan
    docker exec $container redis-cli --scan | Where-Object { $_ -notlike "__MSFT_HCT__*" } | Sort-Object
    return
}

$ttl = docker exec $container redis-cli TTL $Key
if ($ttl -eq "-2") {
    Write-Host "A chave '$Key' não existe (ou já expirou)." -ForegroundColor Yellow
    return
}

$raw = (docker exec $container redis-cli --raw HGET $Key data) -join "`n"

$start = $raw.IndexOfAny([char[]]"[{")
$end = $raw.LastIndexOfAny([char[]]"]}")

Write-Host "Chave: $Key | expira em ${ttl}s" -ForegroundColor Cyan

if ($start -lt 0 -or $end -lt $start) {
    Write-Host "Valor sem JSON (provavelmente null = 'não encontrado' em cache)." -ForegroundColor Yellow
    return
}

# No PowerShell 5.1, sem isso as listas saem embrulhadas em { "value": [...], "Count": n }
Remove-TypeData System.Array -ErrorAction SilentlyContinue

$json = $raw.Substring($start, $end - $start + 1) | ConvertFrom-Json
ConvertTo-Json -InputObject $json -Depth 10
