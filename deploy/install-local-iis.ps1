# Déploie Budget.API sur l'IIS local. À lancer dans PowerShell **en administrateur**, depuis la racine du dépôt :
#   powershell -ExecutionPolicy Bypass -File deploy\install-local-iis.ps1
# Prérequis : IIS actif + .NET 10 Hosting Bundle installé (https://dotnet.microsoft.com/download/dotnet/10.0).
param(
    [string]$SiteName   = 'BudgetApi',
    [int]   $Port       = 8080,
    [string]$SitePath   = 'C:\inetpub\budget-api',
    [string]$SqlServer  = 'INEXA-DEV-GBANE\MSSQLSERVER01',
    [string]$Database   = 'BudgetDb',
    [string]$Frontend   = 'http://localhost:5174'
)
$ErrorActionPreference = 'Stop'

# Test par SID : fonctionne même si le contrôleur de domaine est injoignable.
$adminSid = New-Object Security.Principal.SecurityIdentifier ([Security.Principal.WellKnownSidType]::BuiltinAdministratorsSid, $null)
if (-not ([Security.Principal.WindowsPrincipal][Security.Principal.WindowsIdentity]::GetCurrent()).IsInRole($adminSid)) {
    throw "Lance ce script dans un PowerShell ouvert en administrateur."
}
if (-not ((Test-Path "$env:ProgramFiles\IIS\Asp.Net Core Module\V2\aspnetcorev2.dll") -or
          (Test-Path "$env:windir\System32\inetsrv\aspnetcorev2.dll"))) {
    throw "Module ASP.NET Core absent : installe le .NET 10 Hosting Bundle, puis relance (iisreset)."
}

$publish = Join-Path $PSScriptRoot 'publish'
if (-not (Test-Path "$publish\Budget.API.dll")) { throw "Dossier publish introuvable : $publish" }

Import-Module WebAdministration

# 1. Fichiers (arrêt du site pour libérer les DLL lors d'une mise à jour)
if (Test-Path "IIS:\Sites\$SiteName") { Stop-Website $SiteName }
New-Item -ItemType Directory -Force $SitePath | Out-Null
$webConfigPath = Join-Path $SitePath 'web.config'
$keep = $null
if (Test-Path $webConfigPath) { $keep = Get-Content $webConfigPath -Raw }   # conserve la config (et la clé JWT) existante
robocopy $publish $SitePath /MIR /XF web.config /NFL /NDL /NJH /NJS | Out-Null
if ($LASTEXITCODE -ge 8) { throw "robocopy a échoué ($LASTEXITCODE)" }
if ($keep) { Set-Content $webConfigPath $keep -Encoding UTF8 } else { Copy-Item "$publish\web.config" $webConfigPath }

# 2. Pool d'application (sans code managé)
if (-not (Test-Path "IIS:\AppPools\$SiteName")) { New-WebAppPool $SiteName | Out-Null }
Set-ItemProperty "IIS:\AppPools\$SiteName" managedRuntimeVersion ''

# 3. Site
if (-not (Test-Path "IIS:\Sites\$SiteName")) {
    New-Website -Name $SiteName -PhysicalPath $SitePath -Port $Port -ApplicationPool $SiteName | Out-Null
}

# 4. Droits sur le dossier
icacls $SitePath /grant "IIS AppPool\${SiteName}:(OI)(CI)RX" | Out-Null

# 5. Accès SQL Server pour l'identité du pool (auth Windows)
$login = "IIS APPPOOL\$SiteName"
$sql = @"
IF SUSER_ID(N'$login') IS NULL CREATE LOGIN [$login] FROM WINDOWS;
USE [$Database];
IF USER_ID(N'$login') IS NULL CREATE USER [$login] FOR LOGIN [$login];
ALTER ROLE db_datareader ADD MEMBER [$login];
ALTER ROLE db_datawriter ADD MEMBER [$login];
ALTER ROLE db_ddladmin  ADD MEMBER [$login];
"@
$cn = New-Object System.Data.SqlClient.SqlConnection "Server=$SqlServer;Database=master;Trusted_Connection=True;TrustServerCertificate=True;"
$cn.Open(); $cmd = $cn.CreateCommand(); $cmd.CommandText = $sql; $cmd.ExecuteNonQuery() | Out-Null; $cn.Close()

# 6. Variables d'environnement dans web.config
[xml]$xml = Get-Content $webConfigPath
$asp = $xml.SelectSingleNode('//aspNetCore')
$envNode = $asp.SelectSingleNode('environmentVariables')
if (-not $envNode) { $envNode = $xml.CreateElement('environmentVariables'); $asp.AppendChild($envNode) | Out-Null }
$existing = @{}; foreach ($e in $envNode.environmentVariable) { $existing[$e.name] = $e.value }

$conn = "Server=$SqlServer;Database=$Database;Trusted_Connection=True;TrustServerCertificate=True;"
$jwt = if ($existing['Jwt__SecretKey']) { $existing['Jwt__SecretKey'] } else {
    $b = New-Object byte[] 48; [Security.Cryptography.RandomNumberGenerator]::Create().GetBytes($b); [Convert]::ToBase64String($b) }
$vars = [ordered]@{
    'ASPNETCORE_ENVIRONMENT'            = 'Production'
    'ConnectionStrings__BudgetDatabase' = $conn
    'ApiKeysDatabase__ConnectionString' = $conn
    'ApiKeysDatabase__Provider'         = 'sqlserver'
    'Jwt__SecretKey'                    = $jwt
    'Cors__AllowedOrigins__0'           = $Frontend
}
$envNode.RemoveAll()
foreach ($k in $vars.Keys) {
    $n = $xml.CreateElement('environmentVariable'); $n.SetAttribute('name', $k); $n.SetAttribute('value', $vars[$k])
    $envNode.AppendChild($n) | Out-Null
}
$xml.Save($webConfigPath)

Start-WebAppPool $SiteName -ErrorAction SilentlyContinue
Start-Website $SiteName
Write-Host "OK : http://localhost:$Port  (site '$SiteName', dossier $SitePath)"
