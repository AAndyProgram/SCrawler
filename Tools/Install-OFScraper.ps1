# Self-contained SCrawler OF-Scraper setup (this file only; no extra .py/.bat).
#
# Installs ofscraper 3.12.9 via pip, pins aiolimiter 1.1.0, then applies DRM
# false-success patch 20260827_drm_minimal_v2 (embedded alt_download.py).
#
# Needs Python 3.11.x or 3.12.x. Older than 3.11.x and 3.13+ both fail with
# ofscraper 3.12.9. If a usable Python/pip is missing, this script points you
# to Python 3.11.6. After install, empty DRM segments / 0-byte audio+video are
# not marked downloaded.
#
# Usage:
#   .\Install-OFScraper.ps1
#   .\Install-OFScraper.ps1 -Force
#   .\Install-OFScraper.ps1 -Quiet
#   .\Install-OFScraper.ps1 -SkipDrmPatch
#   .\Install-OFScraper.ps1 -PythonPath "C:\Path\to\python.exe"
#
# The script was written by @cjb900. @cjb900, you are the best!

param(
    [string]$PythonPath = "",
    [switch]$Force,
    [switch]$Quiet,
    [switch]$SkipDrmPatch
)

$ErrorActionPreference = "Stop"

$OfScraperVersion = "3.12.9"
$AioLimiterVersion = "1.1.0"
$DrmPatchId = "20260827_drm_minimal_v2"
$DrmPatchRelPath = "actions\actions\download\managers\alt_download.py"
$DrmPatchSentinel = "DRM download produced 0 bytes"
$PythonMin = [version]"3.11.0"
$PythonMaxExclusive = [version]"3.13.0"
$PythonRecommended = "3.11.6"
$PythonReleaseUrl = "https://www.python.org/downloads/release/python-3116/"
$PythonInstaller64Url = "https://www.python.org/ftp/python/3.11.6/python-3.11.6-amd64.exe"

if ($ExecutionContext.SessionState.LanguageMode -eq "ConstrainedLanguage") {
    Write-Host "Error: PowerShell execution policy is restricting script execution." -ForegroundColor Red
    Write-Host "Run: powershell -ExecutionPolicy Bypass -File `"$($MyInvocation.MyCommand.Path)`"" -ForegroundColor Cyan
    if ($Host.Name -eq "ConsoleHost" -and -not $Quiet) {
        Write-Host "Press any key to exit..." -ForegroundColor Gray
        $null = $Host.UI.RawUI.ReadKey("NoEcho,IncludeKeyDown")
    }
    exit 1
}

function Write-ColorOutput {
    param(
        [string]$Message,
        [string]$Color = "White"
    )
    if (-not $Quiet) {
        Write-Host $Message -ForegroundColor $Color
    }
}

function Test-WindowsAppsStub {
    param([string]$ExePath)
    return ($ExePath -match '\\WindowsApps\\')
}

function Get-PythonVersion {
    param([string]$ExePath)
    try {
        $output = & $ExePath -c "import sys; print('%d.%d.%d' % sys.version_info[:3])" 2>$null
        if ($LASTEXITCODE -ne 0 -or [string]::IsNullOrWhiteSpace($output)) {
            return $null
        }
        return [version]($output.Trim())
    }
    catch {
        return $null
    }
}

function Test-SupportedPython {
    param([version]$Version)
    return ($Version -ge $PythonMin -and $Version -lt $PythonMaxExclusive)
}

function Get-PythonAgeLabel {
    param([version]$Version)
    if ($Version -lt $PythonMin) { return "too old" }
    if ($Version -ge $PythonMaxExclusive) { return "too new" }
    return "supported"
}

function Add-PythonCandidate {
    param(
        [System.Collections.Generic.List[object]]$List,
        [string]$ExePath
    )
    if ([string]::IsNullOrWhiteSpace($ExePath)) { return }
    try { $ExePath = [System.IO.Path]::GetFullPath($ExePath) } catch { return }
    if (-not (Test-Path -LiteralPath $ExePath)) { return }
    if (Test-WindowsAppsStub -ExePath $ExePath) { return }
    foreach ($existing in $List) {
        if ($existing.Path -eq $ExePath) { return }
    }
    $version = Get-PythonVersion -ExePath $ExePath
    if (-not $version) { return }
    $List.Add([pscustomobject]@{
        Path      = $ExePath
        Version   = $version
        Supported = (Test-SupportedPython -Version $version)
        Age       = (Get-PythonAgeLabel -Version $version)
    })
}

function Find-PythonInstalls {
    $found = New-Object "System.Collections.Generic.List[object]"

    if ($PythonPath) {
        Add-PythonCandidate -List $found -ExePath $PythonPath
        return $found
    }

    $pyLauncher = Get-Command py -ErrorAction SilentlyContinue
    if ($pyLauncher) {
        try {
            $pyList = & py -0p 2>$null
            foreach ($line in @($pyList)) {
                if ($line -match '([A-Za-z]:\\.+\\python\.exe)') {
                    Add-PythonCandidate -List $found -ExePath $Matches[1].Trim()
                }
            }
        }
        catch { }

        foreach ($tag in @("3.14", "3.13", "3.12", "3.11", "3.10", "3.9", "3.8")) {
            try {
                $resolved = & py "-$tag" -c "import sys; print(sys.executable)" 2>$null
                if ($LASTEXITCODE -eq 0 -and -not [string]::IsNullOrWhiteSpace($resolved)) {
                    Add-PythonCandidate -List $found -ExePath $resolved.Trim()
                }
            }
            catch { }
        }
    }

    foreach ($cmdName in @("python", "python3")) {
        $cmd = Get-Command $cmdName -ErrorAction SilentlyContinue
        if ($cmd -and $cmd.Source) {
            Add-PythonCandidate -List $found -ExePath $cmd.Source
        }
    }

    $searchRoots = @(
        "$env:LOCALAPPDATA\Programs\Python",
        "$env:ProgramFiles",
        "${env:ProgramFiles(x86)}"
    )
    foreach ($root in $searchRoots) {
        if (-not (Test-Path -LiteralPath $root)) { continue }
        Get-ChildItem -Path $root -Directory -ErrorAction SilentlyContinue |
            Where-Object { $_.Name -match '^Python3\d+$' } |
            ForEach-Object {
                $exe = Join-Path $_.FullName "python.exe"
                Add-PythonCandidate -List $found -ExePath $exe
            }
        if ($root -eq "$env:LOCALAPPDATA\Programs\Python") {
            Get-ChildItem -Path $root -Filter python.exe -Recurse -ErrorAction SilentlyContinue |
                Where-Object { $_.Directory.Name -notmatch '^(Scripts|Lib|include)$' } |
                ForEach-Object { Add-PythonCandidate -List $found -ExePath $_.FullName }
        }
    }

    return $found
}

function Test-Pip {
    param([string]$ExePath)
    try {
        $null = & $ExePath -m pip --version 2>$null
        return ($LASTEXITCODE -eq 0)
    }
    catch {
        return $false
    }
}

function Install-Pip {
    param([string]$ExePath)
    Write-ColorOutput "pip is missing. Trying python -m ensurepip..." "Yellow"
    try {
        & $ExePath -m ensurepip --upgrade
        if (Test-Pip -ExePath $ExePath) { return $true }
    }
    catch { }
    return $false
}

function Get-PipPackageVersion {
    param(
        [string]$ExePath,
        [string]$PackageName
    )
    try {
        $output = & $ExePath -m pip show $PackageName 2>$null
        foreach ($line in @($output)) {
            if ($line -match '^Version:\s*(.+)$') {
                return $Matches[1].Trim()
            }
        }
    }
    catch { }
    return $null
}

function Invoke-PipInstall {
    param(
        [string]$ExePath,
        [string[]]$PipArgs
    )
    $common = @(
        "-m", "pip", "install",
        "--disable-pip-version-check",
        "--no-warn-script-location"
    ) + $PipArgs

    & $ExePath @common
    if ($LASTEXITCODE -eq 0) { return $true }

    Write-ColorOutput "Retrying with --user (install into the current user site-packages)..." "Yellow"
    $userArgs = @(
        "-m", "pip", "install",
        "--user",
        "--disable-pip-version-check",
        "--no-warn-script-location"
    ) + $PipArgs
    & $ExePath @userArgs
    return ($LASTEXITCODE -eq 0)
}

function Get-OfScraperExe {
    param([string]$ExePath)
    try {
        $resolved = & $ExePath -c @"
import os, sys
from pathlib import Path
candidates = []
scripts = Path(sys.executable).resolve().parent / 'Scripts'
candidates.append(scripts / 'ofscraper.exe')
try:
    import sysconfig
    scripts2 = Path(sysconfig.get_path('scripts'))
    candidates.append(scripts2 / 'ofscraper.exe')
except Exception:
    pass
base = Path(os.environ.get('APPDATA', '')) / 'Python'
if base.exists():
    candidates.extend(base.glob('Python3*/Scripts/ofscraper.exe'))
for c in candidates:
    if c.is_file():
        print(c)
        break
"@ 2>$null
        if ($LASTEXITCODE -eq 0 -and -not [string]::IsNullOrWhiteSpace($resolved)) {
            $path = $resolved.Trim()
            if (Test-Path -LiteralPath $path) { return $path }
        }
    }
    catch { }
    return $null
}

function Show-PythonMissing {
    param(
        [string]$Reason,
        [object[]]$Unsupported = @()
    )
    Write-ColorOutput $Reason "Red"
    Write-ColorOutput "" "White"
    Write-ColorOutput "OF-Scraper $OfScraperVersion only works with Python 3.11.x or 3.12.x." "Yellow"
    Write-ColorOutput "  Too old:  3.10.x and earlier (missing language/package features)" "Gray"
    Write-ColorOutput "  Too new:  3.13.x and later (incompatible with this ofscraper release)" "Gray"
    Write-ColorOutput "" "White"

    $tooOld = @($Unsupported | Where-Object { $_.Age -eq "too old" } | Sort-Object Version)
    $tooNew = @($Unsupported | Where-Object { $_.Age -eq "too new" } | Sort-Object Version)

    if ($tooOld.Count -gt 0) {
        Write-ColorOutput "Found Python older than 3.11.x (cannot be used):" "Yellow"
        foreach ($item in $tooOld) {
            Write-ColorOutput ("  {0}  too old  ({1})" -f $item.Version, $item.Path) "Gray"
        }
        Write-ColorOutput "" "White"
    }
    if ($tooNew.Count -gt 0) {
        Write-ColorOutput "Found Python 3.13 or newer (cannot be used):" "Yellow"
        foreach ($item in $tooNew) {
            Write-ColorOutput ("  {0}  too new  ({1})" -f $item.Version, $item.Path) "Gray"
        }
        Write-ColorOutput "" "White"
    }
    if ($Unsupported.Count -eq 0) {
        Write-ColorOutput "No Python install was detected on this machine." "Yellow"
        Write-ColorOutput "" "White"
    }

    Write-ColorOutput "Install CPython $PythonRecommended (64-bit), then re-run this script." "Yellow"
    Write-ColorOutput "In the installer, enable 'Add python.exe to PATH'." "Yellow"
    Write-ColorOutput "Leave any 3.10-or-older and 3.13+ installs in place; this script will pick 3.11/3.12." "Yellow"
    Write-ColorOutput "" "White"
    Write-ColorOutput "Release page: $PythonReleaseUrl" "Cyan"
    Write-ColorOutput "64-bit installer: $PythonInstaller64Url" "Cyan"
    Write-ColorOutput "" "White"

    if ($Quiet) { return }

    $answer = Read-Host "Open the Python $PythonRecommended download page now? (y/n)"
    if ($answer -match '^[Yy]') {
        Start-Process $PythonReleaseUrl
    }
}

function Get-OfScraperPackageDir {
    param([string]$ExePath)
    try {
        $output = & $ExePath -c "import ofscraper, pathlib; print(pathlib.Path(next(iter(ofscraper.__path__))))" 2>$null
        if ($LASTEXITCODE -eq 0 -and -not [string]::IsNullOrWhiteSpace($output)) {
            $dir = $output.Trim()
            if (Test-Path -LiteralPath $dir) { return $dir }
        }
    }
    catch { }
    return $null
}

function Test-DrmPatchApplied {
    param([string]$AltDownloadPath)
    if (-not (Test-Path -LiteralPath $AltDownloadPath)) { return $false }
    try {
        $text = [System.IO.File]::ReadAllText($AltDownloadPath)
        return $text.Contains($DrmPatchSentinel)
    }
    catch {
        return $false
    }
}

function Get-DrmPatchBase64 {
    # Patched alt_download.py for ofscraper 3.12.9 (patch 20260827_drm_minimal_v2)
    return @'
ciIiIgogICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgCiBfX19fX19fICBfX19fX19fICAgICAg
ICAgX19fX19fXyAgX19fX19fXyAgX19fX19fXyAgX19fX19fXyAgX19fX19fXyAgX19fX19fXyAgX19fX19fXyAKKCAgX19fICApKCAgX19fXyBcICAgICAg
ICggIF9fX18gXCggIF9fX18gXCggIF9fX18gKSggIF9fXyAgKSggIF9fX18gKSggIF9fX18gXCggIF9fX18gKQp8ICggICApIHx8ICggICAgXC8gICAgICAg
fCAoICAgIFwvfCAoICAgIFwvfCAoICAgICl8fCAoICAgKSB8fCAoICAgICl8fCAoICAgIFwvfCAoICAgICl8CnwgfCAgIHwgfHwgKF9fICAgICBfX19fXyB8
IChfX19fXyB8IHwgICAgICB8IChfX19fKXx8IChfX18pIHx8IChfX19fKXx8IChfXyAgICB8IChfX19fKXwKfCB8ICAgfCB8fCAgX18pICAgKF9fX19fKShf
X19fXyAgKXwgfCAgICAgIHwgICAgIF9fKXwgIF9fXyAgfHwgIF9fX19fKXwgIF9fKSAgIHwgICAgIF9fKQp8IHwgICB8IHx8ICggICAgICAgICAgICAgICAg
ICAgKSB8fCB8ICAgICAgfCAoXCAoICAgfCAoICAgKSB8fCAoICAgICAgfCAoICAgICAgfCAoXCAoICAgCnwgKF9fXykgfHwgKSAgICAgICAgICAgICAvXF9f
X18pIHx8IChfX19fL1x8ICkgXCBcX198ICkgICAoIHx8ICkgICAgICB8IChfX19fL1x8ICkgXCBcX18KKF9fX19fX18pfC8gICAgICAgICAgICAgIFxfX19f
X19fKShfX19fX19fL3wvICAgXF9fL3wvICAgICBcfHwvICAgICAgIChfX19fX19fL3wvICAgXF9fLwogICAgICAgICAgICAgICAgICAgICAgICAgICAgICAg
ICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgIAoiIiIKCmltcG9ydCBhc3luY2lvCmltcG9ydCBwYXRobGli
CmltcG9ydCByZQppbXBvcnQgdHJhY2ViYWNrCmZyb20gZnVuY3Rvb2xzIGltcG9ydCBwYXJ0aWFsCmZyb20gdXJsbGliLnBhcnNlIGltcG9ydCB1cmxzcGxp
dCwgdXJsdW5zcGxpdAoKCmltcG9ydCBhaW9maWxlcwppbXBvcnQgYXJyb3cKaW1wb3J0IHBzdXRpbApmcm9tIGh1bWFuZnJpZW5kbHkgaW1wb3J0IGZvcm1h
dF9zaXplCgppbXBvcnQgb2ZzY3JhcGVyLmNsYXNzZXMucGxhY2Vob2xkZXIgYXMgcGxhY2Vob2xkZXIKaW1wb3J0IG9mc2NyYXBlci5hY3Rpb25zLnV0aWxz
Lmdsb2JhbHMgYXMgY29tbW9uX2dsb2JhbHMKaW1wb3J0IG9mc2NyYXBlci51dGlscy5jb25zdGFudHMgYXMgY29uc3RhbnRzCmZyb20gb2ZzY3JhcGVyLmNs
YXNzZXMuZG93bmxvYWRfcmV0cmllcyBpbXBvcnQgZG93bmxvYWRfcmV0cnkKCmZyb20gb2ZzY3JhcGVyLmFjdGlvbnMudXRpbHMucGFyYW1zIGltcG9ydCBn
ZXRfYWx0X3BhcmFtcwpmcm9tIG9mc2NyYXBlci5hY3Rpb25zLnV0aWxzLmxvZyBpbXBvcnQgZ2V0X21lZGlhbG9nCmZyb20gb2ZzY3JhcGVyLmFjdGlvbnMu
dXRpbHMubG9nIGltcG9ydCAoCiAgICBnZXRfdXJsX2xvZywKICAgIHBhdGhfdG9fZmlsZV9sb2dnZXIsCiAgICB0ZW1wX2ZpbGVfbG9nZ2VyLAopCmZyb20g
b2ZzY3JhcGVyLmFjdGlvbnMuYWN0aW9ucy5kb3dubG9hZC51dGlscy5jaHVuayBpbXBvcnQgKAogICAgZ2V0X2lkZWFsX2NodW5rX3NpemUsCikKZnJvbSBv
ZnNjcmFwZXIuYWN0aW9ucy51dGlscy5yZXRyaWVzIGltcG9ydCBnZXRfZG93bmxvYWRfcmV0cmllcwpmcm9tIG9mc2NyYXBlci5hY3Rpb25zLnV0aWxzLnNl
bmQuY2h1bmsgaW1wb3J0IHNlbmRfY2h1bmtfbXNnCmZyb20gb2ZzY3JhcGVyLmNsYXNzZXMuc2Vzc2lvbm1hbmFnZXIuc2Vzc2lvbm1hbmFnZXIgaW1wb3J0
ICgKICAgIEZPUkNFRF9ORVcsCiAgICBTSUdOLAopCmltcG9ydCBvZnNjcmFwZXIudXRpbHMuYXV0aC5yZXF1ZXN0IGFzIGF1dGhfcmVxdWVzdHMKZnJvbSBv
ZnNjcmFwZXIuYWN0aW9ucy5hY3Rpb25zLmRvd25sb2FkLm1hbmFnZXJzLmRvd25sb2FkbWFuYWdlciBpbXBvcnQgRG93bmxvYWRNYW5hZ2VyCmltcG9ydCBv
ZnNjcmFwZXIuYWN0aW9ucy51dGlscy5wYXRocy5wYXRocyBhcyBjb21tb25fcGF0aHMKaW1wb3J0IG9mc2NyYXBlci5hY3Rpb25zLnV0aWxzLmxvZyBhcyBj
b21tb25fbG9ncwpmcm9tIG9mc2NyYXBlci5kYi5vcGVyYXRpb25zXy5tZWRpYSBpbXBvcnQgZG93bmxvYWRfbWVkaWFfdXBkYXRlCmltcG9ydCBvZnNjcmFw
ZXIuYWN0aW9ucy51dGlscy5nZW5lcmFsIGFzIGNvbW1vbgppbXBvcnQgb2ZzY3JhcGVyLnV0aWxzLmRhdGVzIGFzIGRhdGVzCmZyb20gb2ZzY3JhcGVyLnV0
aWxzLnN5c3RlbS5zdWJwcm9jZXNzIGltcG9ydCBydW4KaW1wb3J0IG9mc2NyYXBlci51dGlscy5zZXR0aW5ncyBhcyBzZXR0aW5ncwppbXBvcnQgb2ZzY3Jh
cGVyLnV0aWxzLnN5c3RlbS5zeXN0ZW0gYXMgc3lzdGVtCmltcG9ydCBvZnNjcmFwZXIuYWN0aW9ucy5hY3Rpb25zLmRvd25sb2FkLnV0aWxzLmtleWhlbHBl
cnMgYXMga2V5aGVscGVycwppbXBvcnQgb2ZzY3JhcGVyLnV0aWxzLmNhY2hlIGFzIGNhY2hlCmltcG9ydCBvZnNjcmFwZXIudXRpbHMubGl2ZS51cGRhdGVy
IGFzIHByb2dyZXNzX3VwZGF0ZXIKZnJvbSBvZnNjcmFwZXIuYWN0aW9ucy51dGlscy5zZW5kLm1lc3NhZ2UgaW1wb3J0IHNlbmRfbXNnCgoKY2xhc3MgQWx0
RG93bmxvYWRNYW5hZ2VyKERvd25sb2FkTWFuYWdlcik6CiAgICBkZWYgX19pbml0X18oc2VsZiwgbXVsdGk9RmFsc2UpOgogICAgICAgIHN1cGVyKCkuX19p
bml0X18obXVsdGk9bXVsdGkpCgogICAgYXN5bmMgZGVmIGFsdF9kb3dubG9hZChzZWxmLCBjLCBlbGUsIHVzZXJuYW1lLCBtb2RlbF9pZCk6CiAgICAgICAg
Y29tbW9uX2dsb2JhbHMubG9nLmRlYnVnKAogICAgICAgICAgICBmIntnZXRfbWVkaWFsb2coZWxlKX0gRG93bmxvYWRpbmcgd2l0aCBwcm90ZWN0ZWQgbWVk
aWEgZG93bmxvYWRlciIKICAgICAgICApCiAgICAgICAgYXN5bmMgZm9yIF8gaW4gZG93bmxvYWRfcmV0cnkoKToKICAgICAgICAgICAgd2l0aCBfOgogICAg
ICAgICAgICAgICAgdHJ5OgogICAgICAgICAgICAgICAgICAgIHNoYXJlZFBsYWNlaG9sZGVyT2JqID0gYXdhaXQgcGxhY2Vob2xkZXIuUGxhY2Vob2xkZXJz
KAogICAgICAgICAgICAgICAgICAgICAgICBlbGUsICJtcDQiCiAgICAgICAgICAgICAgICAgICAgKS5pbml0KCkKICAgICAgICAgICAgICAgICAgICBjb21t
b25fZ2xvYmFscy5sb2cuZGVidWcoCiAgICAgICAgICAgICAgICAgICAgICAgIGYie2dldF9tZWRpYWxvZyhlbGUpfSBkb3dubG9hZCB1cmw6ICB7Z2V0X3Vy
bF9sb2coZWxlKX0iCiAgICAgICAgICAgICAgICAgICAgKQogICAgICAgICAgICAgICAgZXhjZXB0IEV4Y2VwdGlvbiBhcyBlOgogICAgICAgICAgICAgICAg
ICAgIHJhaXNlIGUKCiAgICAgICAgYXVkaW8gPSBhd2FpdCBlbGUubXBkX2F1ZGlvCiAgICAgICAgdmlkZW8gPSBhd2FpdCBlbGUubXBkX3ZpZGVvCiAgICAg
ICAgcGF0aF90b19maWxlX2xvZ2dlcihzaGFyZWRQbGFjZWhvbGRlck9iaiwgZWxlKQoKICAgICAgICBhdWRpbyA9IGF3YWl0IHNlbGYuX2FsdF9kb3dubG9h
ZF9kb3dubG9hZGVyKGF1ZGlvLCBjLCBlbGUpCiAgICAgICAgdmlkZW8gPSBhd2FpdCBzZWxmLl9hbHRfZG93bmxvYWRfZG93bmxvYWRlcih2aWRlbywgYywg
ZWxlKQoKICAgICAgICBwb3N0X3Jlc3VsdCA9IGF3YWl0IHNlbGYuX21lZGlhX2l0ZW1fcG9zdF9wcm9jZXNzX2FsdCgKICAgICAgICAgICAgYXVkaW8sIHZp
ZGVvLCBlbGUsIHVzZXJuYW1lLCBtb2RlbF9pZAogICAgICAgICkKICAgICAgICBpZiBwb3N0X3Jlc3VsdDoKICAgICAgICAgICAgcmV0dXJuIHBvc3RfcmVz
dWx0CiAgICAgICAgYXdhaXQgc2VsZi5fbWVkaWFfaXRlbV9rZXlzX2FsdChjLCBhdWRpbywgdmlkZW8sIGVsZSkKCiAgICAgICAgcmV0dXJuIGF3YWl0IHNl
bGYuX2hhbmRsZV9yZXN1bHRfYWx0KAogICAgICAgICAgICBzaGFyZWRQbGFjZWhvbGRlck9iaiwgZWxlLCBhdWRpbywgdmlkZW8sIHVzZXJuYW1lLCBtb2Rl
bF9pZAogICAgICAgICkKCiAgICBhc3luYyBkZWYgX2FsdF9kb3dubG9hZF9kb3dubG9hZGVyKHNlbGYsIGl0ZW0sIGMsIGVsZSk6CiAgICAgICAgc2VsZi5f
ZG93bmxvYWRzcGFjZShtZWRpYXR5cGU9ZWxlLm1lZGlhdHlwZSkKICAgICAgICBwbGFjZWhvbGRlck9iaiA9IGF3YWl0IHBsYWNlaG9sZGVyLnRlbXBGaWxl
UGxhY2Vob2xkZXIoCiAgICAgICAgICAgIGVsZSwgZiJ7aXRlbVsnbmFtZSddfS5wYXJ0IgogICAgICAgICkuaW5pdCgpCiAgICAgICAgaXRlbVsicGF0aCJd
ID0gcGxhY2Vob2xkZXJPYmoudGVtcGZpbGVwYXRoCiAgICAgICAgaXRlbVsidG90YWwiXSA9IE5vbmUKICAgICAgICAKICAgICAgICBhc3luYyBmb3IgXyBp
biBkb3dubG9hZF9yZXRyeSgpOgogICAgICAgICAgICB3aXRoIF86CiAgICAgICAgICAgICAgICB0cnk6CiAgICAgICAgICAgICAgICAgICAgX2F0dGVtcHQg
PSBzZWxmLl9hbHRfYXR0ZW1wdF9nZXQoaXRlbSkKICAgICAgICAgICAgICAgICAgICBfYXR0ZW1wdC5zZXQoX2F0dGVtcHQuZ2V0KDApICsgMSkKICAgICAg
ICAgICAgICAgICAgICBpZiBfYXR0ZW1wdC5nZXQoKSA+IDE6CiAgICAgICAgICAgICAgICAgICAgICAgIHBhdGhsaWIuUGF0aChwbGFjZWhvbGRlck9iai50
ZW1wZmlsZXBhdGgpLnVubGluaygKICAgICAgICAgICAgICAgICAgICAgICAgICAgIG1pc3Npbmdfb2s9VHJ1ZQogICAgICAgICAgICAgICAgICAgICAgICAp
CiAgICAgICAgICAgICAgICAgICAgZGF0YSA9IGF3YWl0IHNlbGYuX2dldF9kYXRhKGVsZSwgaXRlbSkKICAgICAgICAgICAgICAgICAgICBzdGF0dXMgPSBG
YWxzZQogICAgICAgICAgICAgICAgICAgIGlmIGRhdGE6CiAgICAgICAgICAgICAgICAgICAgICAgIGl0ZW0sIHN0YXR1cyA9IGF3YWl0IHNlbGYuX3Jlc3Vt
ZV9kYXRhX2hhbmRsZXJfYWx0KAogICAgICAgICAgICAgICAgICAgICAgICAgICAgZGF0YSwgaXRlbSwgZWxlLCBwbGFjZWhvbGRlck9iagogICAgICAgICAg
ICAgICAgICAgICAgICApCgogICAgICAgICAgICAgICAgICAgIGVsc2U6CiAgICAgICAgICAgICAgICAgICAgICAgIGl0ZW0sIHN0YXR1cyA9IGF3YWl0IHNl
bGYuX2ZyZXNoX2RhdGFfaGFuZGxlcl9hbHQoCiAgICAgICAgICAgICAgICAgICAgICAgICAgICBpdGVtLCBlbGUsIHBsYWNlaG9sZGVyT2JqCiAgICAgICAg
ICAgICAgICAgICAgICAgICkKICAgICAgICAgICAgICAgICAgICBpZiBub3Qgc3RhdHVzOgogICAgICAgICAgICAgICAgICAgICAgICB0cnk6CiAgICAgICAg
ICAgICAgICAgICAgICAgICAgICBpdGVtID0gYXdhaXQgc2VsZi5fYWx0X2Rvd25sb2FkX3NlbmRyZXEoCiAgICAgICAgICAgICAgICAgICAgICAgICAgICAg
ICAgaXRlbSwgYywgZWxlLCBwbGFjZWhvbGRlck9iagogICAgICAgICAgICAgICAgICAgICAgICAgICAgKQogICAgICAgICAgICAgICAgICAgICAgICBleGNl
cHQgRXhjZXB0aW9uIGFzIEU6CiAgICAgICAgICAgICAgICAgICAgICAgICAgICByYWlzZSBFCiAgICAgICAgICAgICAgICAgICAgcmV0dXJuIGl0ZW0KICAg
ICAgICAgICAgICAgIGV4Y2VwdCBPU0Vycm9yIGFzIEU6CiAgICAgICAgICAgICAgICAgICAgY29tbW9uX2dsb2JhbHMubG9nLmRlYnVnKAogICAgICAgICAg
ICAgICAgICAgICAgICBmIntnZXRfbWVkaWFsb2coZWxlKX0gW2F0dGVtcHQge19hdHRlbXB0LmdldCgpfS97Z2V0X2Rvd25sb2FkX3JldHJpZXMoKX1dIE51
bWJlciBvZiBPcGVuIEZpbGVzIC0+IHsgbGVuKHBzdXRpbC5Qcm9jZXNzKCkub3Blbl9maWxlcygpKX0iCiAgICAgICAgICAgICAgICAgICAgKQogICAgICAg
ICAgICAgICAgICAgIGNvbW1vbl9nbG9iYWxzLmxvZy5kZWJ1ZygKICAgICAgICAgICAgICAgICAgICAgICAgZiIge2dldF9tZWRpYWxvZyhlbGUpfSBbYXR0
ZW1wdCB7X2F0dGVtcHQuZ2V0KCl9L3tnZXRfZG93bmxvYWRfcmV0cmllcygpfV0gT3BlbiBGaWxlcyAtPiB7bGlzdChtYXAobGFtYmRhIHg6KHgucGF0aCx4
LmZkKSxwc3V0aWwuUHJvY2VzcygpLm9wZW5fZmlsZXMoKSkpfSIKICAgICAgICAgICAgICAgICAgICApCiAgICAgICAgICAgICAgICAgICAgcmFpc2UgRQog
ICAgICAgICAgICAgICAgZXhjZXB0IEV4Y2VwdGlvbiBhcyBFOgogICAgICAgICAgICAgICAgICAgIGNvbW1vbl9nbG9iYWxzLmxvZy50cmFjZWJhY2tfKAog
ICAgICAgICAgICAgICAgICAgICAgICBmIntnZXRfbWVkaWFsb2coZWxlKX0gW2F0dGVtcHQge19hdHRlbXB0LmdldCgpfS97Z2V0X2Rvd25sb2FkX3JldHJp
ZXMoKX1dIHt0cmFjZWJhY2suZm9ybWF0X2V4YygpfSIKICAgICAgICAgICAgICAgICAgICApCiAgICAgICAgICAgICAgICAgICAgY29tbW9uX2dsb2JhbHMu
bG9nLnRyYWNlYmFja18oCiAgICAgICAgICAgICAgICAgICAgICAgIGYie2dldF9tZWRpYWxvZyhlbGUpfSBbYXR0ZW1wdCB7X2F0dGVtcHQuZ2V0KCl9L3tn
ZXRfZG93bmxvYWRfcmV0cmllcygpfV0ge0V9IgogICAgICAgICAgICAgICAgICAgICkKICAgICAgICAgICAgICAgICAgICByYWlzZSBFCgogICAgYXN5bmMg
ZGVmIF9hbHRfZG93bmxvYWRfc2VuZHJlcShzZWxmLCBpdGVtLCBjLCBlbGUsIHBsYWNlaG9sZGVyT2JqKToKICAgICAgICB0cnk6CiAgICAgICAgICAgIF9h
dHRlbXB0ID0gc2VsZi5fYWx0X2F0dGVtcHRfZ2V0KGl0ZW0pCiAgICAgICAgICAgIHVybCA9IF9tcGRfc2VnbWVudF91cmwoZWxlLm1wZCwgaXRlbVsib3Jp
Z25hbWUiXSkKICAgICAgICAgICAgY29tbW9uX2dsb2JhbHMubG9nLmRlYnVnKAogICAgICAgICAgICAgICAgZiJ7Z2V0X21lZGlhbG9nKGVsZSl9IEF0dGVt
cHRpbmcgdG8gZG93bmxvYWQgbWVkaWEge2l0ZW1bJ29yaWduYW1lJ119IHdpdGgge3VybH0iCiAgICAgICAgICAgICkKICAgICAgICAgICAgY29tbW9uX2ds
b2JhbHMubG9nLmRlYnVnKAogICAgICAgICAgICAgICAgZiJ7Z2V0X21lZGlhbG9nKGVsZSl9IFthdHRlbXB0IHtfYXR0ZW1wdC5nZXQoKX0ve2dldF9kb3du
bG9hZF9yZXRyaWVzKCl9XSBkb3dubG9hZCB0ZW1wIHBhdGgge3BsYWNlaG9sZGVyT2JqLnRlbXBmaWxlcGF0aH0iCiAgICAgICAgICAgICkKICAgICAgICAg
ICAgcmV0dXJuIGF3YWl0IHNlbGYuX3NlbmRfcmVxX2lubmVyKGMsIGVsZSwgaXRlbSwgcGxhY2Vob2xkZXJPYmopCiAgICAgICAgZXhjZXB0IE9TRXJyb3Ig
YXMgRToKICAgICAgICAgICAgcmFpc2UgRQogICAgICAgIGV4Y2VwdCBFeGNlcHRpb24gYXMgRToKICAgICAgICAgICAgcmFpc2UgRQoKICAgIGFzeW5jIGRl
ZiBfc2VuZF9yZXFfaW5uZXIoc2VsZiwgYywgZWxlLCBpdGVtLCBwbGFjZWhvbGRlck9iaik6CiAgICAgICAgdG90YWwgPSBOb25lCiAgICAgICAgdHJ5OgoK
ICAgICAgICAgICAgcmVzdW1lX3NpemUgPSBzZWxmLl9nZXRfcmVzdW1lX3NpemUocGxhY2Vob2xkZXJPYmosIG1lZGlhdHlwZT1lbGUubWVkaWF0eXBlKQog
ICAgICAgICAgICBoZWFkZXJzID0gc2VsZi5fZ2V0X3Jlc3VtZV9oZWFkZXIocmVzdW1lX3NpemUsIGl0ZW1bInRvdGFsIl0pCiAgICAgICAgICAgICMgcmVz
ZXQgdG90YWwKICAgICAgICAgICAgdG90YWwgPSBOb25lCiAgICAgICAgICAgIGNvbW1vbl9nbG9iYWxzLmxvZy5kZWJ1ZyhmIntnZXRfbWVkaWFsb2coZWxl
KX0gcmVzdW1lIGhlYWRlciB7aGVhZGVyc30iKQogICAgICAgICAgICBwYXJhbXMgPSBnZXRfYWx0X3BhcmFtcyhlbGUpCiAgICAgICAgICAgIHVybCA9IF9t
cGRfc2VnbWVudF91cmwoZWxlLm1wZCwgaXRlbVsib3JpZ25hbWUiXSkKICAgICAgICAgICAgaGVhZGVycyA9IHsiQ29va2llIjogZiJ7ZWxlLmhsc19oZWFk
ZXJ9e2F1dGhfcmVxdWVzdHMuZ2V0X2Nvb2tpZXNfc3RyKCl9In0KICAgICAgICAgICAgY29tbW9uX2dsb2JhbHMubG9nLmRlYnVnKAogICAgICAgICAgICAg
ICAgZiJ7Z2V0X21lZGlhbG9nKGVsZSl9IFthdHRlbXB0IHtzZWxmLl9hbHRfYXR0ZW1wdF9nZXQoaXRlbSkuZ2V0KCl9L3tnZXRfZG93bmxvYWRfcmV0cmll
cygpfV0gRG93bmxvYWRpbmcgbWVkaWEgd2l0aCB1cmwgIHt1cmx9IgogICAgICAgICAgICApCiAgICAgICAgICAgIGFzeW5jIHdpdGggYy5yZXF1ZXN0c19h
c3luYygKICAgICAgICAgICAgICAgIHVybD11cmwsCiAgICAgICAgICAgICAgICBoZWFkZXJzPWhlYWRlcnMsCiAgICAgICAgICAgICAgICBwYXJhbXM9cGFy
YW1zLAogICAgICAgICAgICAgICAgIyBhY3Rpb249W0ZPUkNFRF9ORVcsU0lHTl0gaWYgY29uc3RhbnRzLmdldGF0dHIoIkFMVF9GT1JDRV9LRVkiKSBlbHNl
IE5vbmUKICAgICAgICAgICAgKSBhcyBsOgogICAgICAgICAgICAgICAgY29udGVudF90eXBlID0gKGwuaGVhZGVycy5nZXQoImNvbnRlbnQtdHlwZSIpIG9y
ICIiKS5sb3dlcigpCiAgICAgICAgICAgICAgICBjbF9yYXcgPSBsLmhlYWRlcnMuZ2V0KCJjb250ZW50LWxlbmd0aCIpCiAgICAgICAgICAgICAgICB0cnk6
CiAgICAgICAgICAgICAgICAgICAgdG90YWwgPSBpbnQoY2xfcmF3KSBpZiBjbF9yYXcgaXMgbm90IE5vbmUgZWxzZSBOb25lCiAgICAgICAgICAgICAgICBl
eGNlcHQgKFR5cGVFcnJvciwgVmFsdWVFcnJvcik6CiAgICAgICAgICAgICAgICAgICAgdG90YWwgPSBOb25lCiAgICAgICAgICAgICAgICBpdGVtWyJ0b3Rh
bCJdID0gdG90YWwKCiAgICAgICAgICAgICAgICBpZiBhbnkoCiAgICAgICAgICAgICAgICAgICAgeCBpbiBjb250ZW50X3R5cGUKICAgICAgICAgICAgICAg
ICAgICBmb3IgeCBpbiAoImRhc2greG1sIiwgIm1wZWd1cmwiLCAiYXBwbGljYXRpb24veG1sIiwgInRleHQveG1sIikKICAgICAgICAgICAgICAgICk6CiAg
ICAgICAgICAgICAgICAgICAgcmFpc2UgRXhjZXB0aW9uKAogICAgICAgICAgICAgICAgICAgICAgICBmIntnZXRfbWVkaWFsb2coZWxlKX0gRFJNIHNlZ21l
bnQgcmV0dXJuZWQgcGxheWxpc3QgIgogICAgICAgICAgICAgICAgICAgICAgICBmImNvbnRlbnQtdHlwZT17Y29udGVudF90eXBlIXJ9IHVybD17dXJsfSIK
ICAgICAgICAgICAgICAgICAgICApCiAgICAgICAgICAgICAgICBpZiBub3QgdG90YWwgb3IgdG90YWwgPD0gMDoKICAgICAgICAgICAgICAgICAgICByYWlz
ZSBFeGNlcHRpb24oCiAgICAgICAgICAgICAgICAgICAgICAgIGYie2dldF9tZWRpYWxvZyhlbGUpfSBEUk0gc2VnbWVudCBlbXB0eSBzaXplICIKICAgICAg
ICAgICAgICAgICAgICAgICAgZiIoY29udGVudC10eXBlPXtjb250ZW50X3R5cGUgb3IgJ3Vua25vd24nfSwgdXJsPXt1cmx9KSIKICAgICAgICAgICAgICAg
ICAgICApCgogICAgICAgICAgICAgICAgZGF0YSA9IHsKICAgICAgICAgICAgICAgICAgICAiY29udGVudC10b3RhbCI6IHRvdGFsLAogICAgICAgICAgICAg
ICAgICAgICJjb250ZW50LXR5cGUiOiBsLmhlYWRlcnMuZ2V0KCJjb250ZW50LXR5cGUiKSwKICAgICAgICAgICAgICAgIH0KCiAgICAgICAgICAgICAgICBj
b21tb25fZ2xvYmFscy5sb2cuZGVidWcoCiAgICAgICAgICAgICAgICAgICAgZiJ7Z2V0X21lZGlhbG9nKGVsZSl9IGRhdGEgZnJvbSByZXF1ZXN0IHtkYXRh
fSIKICAgICAgICAgICAgICAgICkKICAgICAgICAgICAgICAgIGNvbW1vbl9nbG9iYWxzLmxvZy5kZWJ1ZygKICAgICAgICAgICAgICAgICAgICBmIntnZXRf
bWVkaWFsb2coZWxlKX0gdG90YWwgZnJvbSByZXF1ZXN0IHtmb3JtYXRfc2l6ZShkYXRhLmdldCgnY29udGVudC10b3RhbCcpKSBpZiBkYXRhLmdldCgnY29u
dGVudC10b3RhbCcpIGVsc2UgJ3Vua25vd24nfSIKICAgICAgICAgICAgICAgICkKICAgICAgICAgICAgICAgIGF3YWl0IHNlbGYuX3RvdGFsX2NoYW5nZV9o
ZWxwZXIoTm9uZSwgdG90YWwpCiAgICAgICAgICAgICAgICBhd2FpdCBzZWxmLl9zZXRfZGF0YShlbGUsIGl0ZW0sIGRhdGEpCgogICAgICAgICAgICAgICAg
dGVtcF9maWxlX2xvZ2dlcihwbGFjZWhvbGRlck9iaiwgZWxlKQogICAgICAgICAgICAgICAgaWYgYXdhaXQgc2VsZi5fY2hlY2tfZm9yY2VkX3NraXAoZWxl
LCB0b3RhbCkgPT0gMDoKICAgICAgICAgICAgICAgICAgICBpdGVtWyJ0b3RhbCJdID0gMAogICAgICAgICAgICAgICAgICAgIHRvdGFsID0gaXRlbVsidG90
YWwiXQogICAgICAgICAgICAgICAgICAgIGF3YWl0IHNlbGYuX3RvdGFsX2NoYW5nZV9oZWxwZXIodG90YWwsIDApCiAgICAgICAgICAgICAgICAgICAgcmV0
dXJuIGl0ZW0KICAgICAgICAgICAgICAgIGVsaWYgdG90YWwgIT0gcmVzdW1lX3NpemU6CiAgICAgICAgICAgICAgICAgICAgYXdhaXQgc2VsZi5fZG93bmxv
YWRfZmlsZW9iamVjdF93cml0ZXIoCiAgICAgICAgICAgICAgICAgICAgICAgIHRvdGFsLCBsLCBlbGUsIHBsYWNlaG9sZGVyT2JqLCBpdGVtCiAgICAgICAg
ICAgICAgICAgICAgKQoKICAgICAgICAgICAgYXdhaXQgc2VsZi5fc2l6ZV9jaGVja2VyKHBsYWNlaG9sZGVyT2JqLnRlbXBmaWxlcGF0aCwgZWxlLCB0b3Rh
bCkKICAgICAgICAgICAgcmV0dXJuIGl0ZW0KICAgICAgICBleGNlcHQgRXhjZXB0aW9uIGFzIEU6CiAgICAgICAgICAgIGF3YWl0IHNlbGYuX3RvdGFsX2No
YW5nZV9oZWxwZXIodG90YWwsIDApIGlmIHRvdGFsIGVsc2UgTm9uZQogICAgICAgICAgICByYWlzZSBFCgogICAgYXN5bmMgZGVmIF9kb3dubG9hZF9maWxl
b2JqZWN0X3dyaXRlcihzZWxmLCB0b3RhbCwgbCwgZWxlLCBwbGFjZWhvbGRlck9iaiwgaXRlbSk6CiAgICAgICAgY29tbW9uX2dsb2JhbHMubG9nLmRlYnVn
KAogICAgICAgICAgICBmIntnZXRfbWVkaWFsb2coZWxlKX0gW2F0dGVtcHQge3NlbGYuX2FsdF9hdHRlbXB0X2dldChpdGVtKS5nZXQoKX0ve2dldF9kb3du
bG9hZF9yZXRyaWVzKCl9XSB3cml0aW5nIG1lZGlhIHRvIGRpc2siCiAgICAgICAgKQogICAgICAgIGlmIHRvdGFsID4gY29uc3RhbnRzLmdldGF0dHIoIk1B
WF9SRUFEX1NJWkUiKToKICAgICAgICAgICAgYXdhaXQgc2VsZi5fZG93bmxvYWRfZmlsZW9iamVjdF93cml0ZXJfc3RyZWFtZXIoZWxlLHRvdGFsLCBsLCBw
bGFjZWhvbGRlck9iaikKICAgICAgICBlbHNlOgogICAgICAgICAgICBhd2FpdCBzZWxmLl9kb3dubG9hZF9maWxlb2JqZWN0X3dyaXRlcl9yZWFkZXIoZWxl
LHRvdGFsLCBsLCBwbGFjZWhvbGRlck9iaikKICAgICAgICBjb21tb25fZ2xvYmFscy5sb2cuZGVidWcoCiAgICAgICAgICAgIGYie2dldF9tZWRpYWxvZyhl
bGUpfSBbYXR0ZW1wdCB7c2VsZi5fYWx0X2F0dGVtcHRfZ2V0KGl0ZW0pLmdldCgpfS97Z2V0X2Rvd25sb2FkX3JldHJpZXMoKX1dIGZpbmlzaGVkIHdyaXRp
bmcgbWVkaWEgdG8gZGlzayIKICAgICAgICApCgogICAgYXN5bmMgZGVmIF9kb3dubG9hZF9maWxlb2JqZWN0X3dyaXRlcl9yZWFkZXIoc2VsZiwgZWxlLCB0
b3RhbCwgcmVzLCBwbGFjZWhvbGRlck9iaik6CgogICAgICAgIHRhc2sxID0gYXdhaXQgc2VsZi5fYWRkX2Rvd25sb2FkX2pvYl90YXNrKAogICAgICAgICAg
ICBlbGUsIHRvdGFsPXRvdGFsLCBwbGFjZWhvbGRlck9iaj1wbGFjZWhvbGRlck9iagogICAgICAgICkKICAgICAgICBmaWxlb2JqZWN0ID0gYXdhaXQgYWlv
ZmlsZXMub3BlbihwbGFjZWhvbGRlck9iai50ZW1wZmlsZXBhdGgsICJhYiIpLl9fYWVudGVyX18oKQogICAgICAgIHRyeToKICAgICAgICAgICAgYXdhaXQg
ZmlsZW9iamVjdC53cml0ZShhd2FpdCByZXMucmVhZF8oKSkKICAgICAgICBleGNlcHQgRXhjZXB0aW9uIGFzIEU6CiAgICAgICAgICAgIHJhaXNlIEUKICAg
ICAgICBmaW5hbGx5OgogICAgICAgICAgICAjIENsb3NlIGZpbGUgaWYgbmVlZGVkCiAgICAgICAgICAgIHRyeToKICAgICAgICAgICAgICAgIGF3YWl0IGZp
bGVvYmplY3QuY2xvc2UoKQogICAgICAgICAgICBleGNlcHQgRXhjZXB0aW9uIGFzIEU6CiAgICAgICAgICAgICAgICByYWlzZSBFCiAgICAgICAgICAgIHRy
eToKICAgICAgICAgICAgICAgIGF3YWl0IHNlbGYuX3JlbW92ZV9kb3dubG9hZF9qb2JfdGFzayh0YXNrMSwgZWxlKQogICAgICAgICAgICBleGNlcHQgRXhj
ZXB0aW9uIGFzIEU6CiAgICAgICAgICAgICAgICByYWlzZSBFCgogICAgYXN5bmMgZGVmIF9kb3dubG9hZF9maWxlb2JqZWN0X3dyaXRlcl9zdHJlYW1lcigK
ICAgICAgICBzZWxmLCBlbGUsIHRvdGFsLCByZXMsIHBsYWNlaG9sZGVyT2JqCiAgICApOgoKICAgICAgICB0YXNrMSA9IGF3YWl0IHNlbGYuX2FkZF9kb3du
bG9hZF9qb2JfdGFzayhlbGUsIHRvdGFsLCBwbGFjZWhvbGRlck9iaikKICAgICAgICBmaWxlb2JqZWN0ID0gYXdhaXQgYWlvZmlsZXMub3BlbihwbGFjZWhv
bGRlck9iai50ZW1wZmlsZXBhdGgsICJhYiIpLl9fYWVudGVyX18oKQogICAgICAgIGNodW5rX3NpemUgPSBnZXRfaWRlYWxfY2h1bmtfc2l6ZSh0b3RhbCwg
cGxhY2Vob2xkZXJPYmoudGVtcGZpbGVwYXRoKQogICAgICAgIHRyeToKICAgICAgICAgICAgYXN5bmMgZm9yIGNodW5rIGluIHJlcy5pdGVyX2NodW5rZWQo
Y2h1bmtfc2l6ZSk6CiAgICAgICAgICAgICAgICBhd2FpdCBmaWxlb2JqZWN0LndyaXRlKGNodW5rKQogICAgICAgICAgICAgICAgc2VuZF9jaHVua19tc2co
ZWxlLCB0b3RhbCwgcGxhY2Vob2xkZXJPYmopCiAgICAgICAgZXhjZXB0IEV4Y2VwdGlvbiBhcyBFOgogICAgICAgICAgICByYWlzZSBFCiAgICAgICAgZmlu
YWxseToKICAgICAgICAgICAgIyBDbG9zZSBmaWxlIGlmIG5lZWRlZAogICAgICAgICAgICB0cnk6CiAgICAgICAgICAgICAgICBhd2FpdCBmaWxlb2JqZWN0
LmNsb3NlKCkKICAgICAgICAgICAgZXhjZXB0IEV4Y2VwdGlvbiBhcyBFOgogICAgICAgICAgICAgICAgcmFpc2UgRQoKICAgICAgICAgICAgdHJ5OgogICAg
ICAgICAgICAgICAgYXdhaXQgc2VsZi5fcmVtb3ZlX2Rvd25sb2FkX2pvYl90YXNrKHRhc2sxLCBlbGUpCiAgICAgICAgICAgIGV4Y2VwdCBFeGNlcHRpb24g
YXMgRToKICAgICAgICAgICAgICAgIHJhaXNlIEUKCiAgICBhc3luYyBkZWYgX2hhbmRsZV9yZXN1bHRfYWx0KAogICAgICAgIHNlbGYsIHNoYXJlZFBsYWNl
aG9sZGVyT2JqLCBlbGUsIGF1ZGlvLCB2aWRlbywgdXNlcm5hbWUsIG1vZGVsX2lkCiAgICApOgogICAgICAgIHRlbXBQbGFjZWhvbGRlciA9IGF3YWl0IHBs
YWNlaG9sZGVyLnRlbXBGaWxlUGxhY2Vob2xkZXIoCiAgICAgICAgICAgIGVsZSwgZiJ0ZW1wX3tlbGUuaWQgb3IgYXdhaXQgZWxlLmZpbmFsX2ZpbGVuYW1l
fS5tcDQiCiAgICAgICAgKS5pbml0KCkKICAgICAgICB0ZW1wX3BhdGggPSB0ZW1wUGxhY2Vob2xkZXIudGVtcGZpbGVwYXRoCiAgICAgICAgdGVtcF9wYXRo
LnVubGluayhtaXNzaW5nX29rPVRydWUpCiAgICAgICAgdCA9IHJ1bigKICAgICAgICAgICAgWwogICAgICAgICAgICAgICAgc2V0dGluZ3MuZ2V0X2ZmbXBl
ZygpLAogICAgICAgICAgICAgICAgIi1pIiwKICAgICAgICAgICAgICAgIHN0cih2aWRlb1sicGF0aCJdKSwKICAgICAgICAgICAgICAgICItaSIsCiAgICAg
ICAgICAgICAgICBzdHIoYXVkaW9bInBhdGgiXSksCiAgICAgICAgICAgICAgICAiLWMiLAogICAgICAgICAgICAgICAgImNvcHkiLAogICAgICAgICAgICAg
ICAgIi1tb3ZmbGFncyIsCiAgICAgICAgICAgICAgICAidXNlX21ldGFkYXRhX3RhZ3MiLAogICAgICAgICAgICAgICAgc3RyKHRlbXBfcGF0aCksCiAgICAg
ICAgICAgIF0sCiAgICAgICAgKQogICAgICAgIGlmIHQuc3RkZXJyLmRlY29kZSgpLmZpbmQoIk91dHB1dCIpID09IC0xOgogICAgICAgICAgICBjb21tb25f
Z2xvYmFscy5sb2cuZGVidWcoZiJ7Y29tbW9uX2xvZ3MuZ2V0X21lZGlhbG9nKGVsZSl9IGZmbXBlZyBmYWlsZWQiKQogICAgICAgICAgICBjb21tb25fZ2xv
YmFscy5sb2cuZGVidWcoCiAgICAgICAgICAgICAgICBmIntjb21tb25fbG9ncy5nZXRfbWVkaWFsb2coZWxlKX0gZmZtcGVnIHt0LnN0ZGVyci5kZWNvZGUo
KX0iCiAgICAgICAgICAgICkKICAgICAgICAgICAgY29tbW9uX2dsb2JhbHMubG9nLmRlYnVnKAogICAgICAgICAgICAgICAgZiJ7Y29tbW9uX2xvZ3MuZ2V0
X21lZGlhbG9nKGVsZSl9IGZmbXBlZyB7dC5zdGRvdXQuZGVjb2RlKCl9IgogICAgICAgICAgICApCgogICAgICAgIHZpZGVvWyJwYXRoIl0udW5saW5rKG1p
c3Npbmdfb2s9VHJ1ZSkKICAgICAgICBhdWRpb1sicGF0aCJdLnVubGluayhtaXNzaW5nX29rPVRydWUpCiAgICAgICAKCiAgICAgICAgY29tbW9uX2dsb2Jh
bHMubG9nLmRlYnVnKAogICAgICAgICAgICBmIk1vdmluZyBpbnRlcm1lZGlhdGUgcGF0aCB7dGVtcF9wYXRofSB0byB7c2hhcmVkUGxhY2Vob2xkZXJPYmou
dHJ1bmljYXRlZF9maWxlcGF0aH0iCiAgICAgICAgKQogICAgICAgIGNvbW1vbl9wYXRocy5tb3ZlSGVscGVyKAogICAgICAgICAgICB0ZW1wX3BhdGgsIHNo
YXJlZFBsYWNlaG9sZGVyT2JqLnRydW5pY2F0ZWRfZmlsZXBhdGgsIGVsZQogICAgICAgICkKICAgICAgICAoCiAgICAgICAgICAgIGNvbW1vbl9wYXRocy5h
ZGRHbG9iYWxEaXIoc2hhcmVkUGxhY2Vob2xkZXJPYmouZmlsZWRpcikKICAgICAgICAgICAgaWYgc3lzdGVtLmdldF9wYXJlbnRfcHJvY2VzcygpCiAgICAg
ICAgICAgIGVsc2UgY29tbW9uX3BhdGhzLmFkZExvY2FsRGlyKHNoYXJlZFBsYWNlaG9sZGVyT2JqLmZpbGVkaXIpCiAgICAgICAgKQogICAgICAgIGlmIGVs
ZS5wb3N0ZGF0ZToKICAgICAgICAgICAgbmV3RGF0ZSA9IGRhdGVzLmNvbnZlcnRfbG9jYWxfdGltZShlbGUucG9zdGRhdGUpCiAgICAgICAgICAgIGNvbW1v
bl9nbG9iYWxzLmxvZy5kZWJ1ZygKICAgICAgICAgICAgICAgIGYie2NvbW1vbl9sb2dzLmdldF9tZWRpYWxvZyhlbGUpfSBBdHRlbXB0IHRvIHNldCBEYXRl
IHRvIHthcnJvdy5nZXQobmV3RGF0ZSkuZm9ybWF0KCdZWVlZLU1NLUREIEhIOm1tJyl9IgogICAgICAgICAgICApCiAgICAgICAgICAgIGNvbW1vbl9wYXRo
cy5zZXRfdGltZShzaGFyZWRQbGFjZWhvbGRlck9iai50cnVuaWNhdGVkX2ZpbGVwYXRoLCBuZXdEYXRlKQogICAgICAgICAgICBjb21tb25fZ2xvYmFscy5s
b2cuZGVidWcoCiAgICAgICAgICAgICAgICBmIntjb21tb25fbG9ncy5nZXRfbWVkaWFsb2coZWxlKX0gRGF0ZSBzZXQgdG8ge2Fycm93LmdldChzaGFyZWRQ
bGFjZWhvbGRlck9iai50cnVuaWNhdGVkX2ZpbGVwYXRoLnN0YXQoKS5zdF9tdGltZSkuZm9ybWF0KCdZWVlZLU1NLUREIEhIOm1tJyl9IgogICAgICAgICAg
ICApCiAgICAgICAgaWYgZWxlLmlkOgogICAgICAgICAgICBhd2FpdCBkb3dubG9hZF9tZWRpYV91cGRhdGUoCiAgICAgICAgICAgICAgICBlbGUsCiAgICAg
ICAgICAgICAgICBmaWxlcGF0aD1zaGFyZWRQbGFjZWhvbGRlck9iai50cnVuaWNhdGVkX2ZpbGVwYXRoLAogICAgICAgICAgICAgICAgbW9kZWxfaWQ9bW9k
ZWxfaWQsCiAgICAgICAgICAgICAgICB1c2VybmFtZT11c2VybmFtZSwKICAgICAgICAgICAgICAgIGRvd25sb2FkZWQ9VHJ1ZSwKICAgICAgICAgICAgICAg
IGhhc2hkYXRhPWF3YWl0IGNvbW1vbi5nZXRfaGFzaCgKICAgICAgICAgICAgICAgICAgICBzaGFyZWRQbGFjZWhvbGRlck9iaiwgbWVkaWF0eXBlPWVsZS5t
ZWRpYXR5cGUKICAgICAgICAgICAgICAgICksCiAgICAgICAgICAgICAgICBzaXplPXNoYXJlZFBsYWNlaG9sZGVyT2JqLnNpemUsCiAgICAgICAgICAgICkK
ICAgICAgICBjb21tb24uYWRkX2FkZGl0aW9uYWxfZGF0YShzaGFyZWRQbGFjZWhvbGRlck9iaiwgZWxlKQogICAgICAgIHJldHVybiBlbGUubWVkaWF0eXBl
LCB2aWRlb1sidG90YWwiXSArIGF1ZGlvWyJ0b3RhbCJdCgogICAgYXN5bmMgZGVmIF9yZXN1bWVfZGF0YV9oYW5kbGVyX2FsdChzZWxmLCBkYXRhLCBpdGVt
LCBlbGUsIHBsYWNlaG9sZGVyT2JqKToKICAgICAgICBjb21tb25fZ2xvYmFscy5sb2cuZGVidWcoCiAgICAgICAgICAgIGYie2dldF9tZWRpYWxvZyhlbGUp
fSBbYXR0ZW1wdCB7Y29tbW9uX2dsb2JhbHMuYXR0ZW1wdC5nZXQoKX0ve2dldF9kb3dubG9hZF9yZXRyaWVzKCl9XSB1c2luZyBkYXRhIGZvciBwb3NzaWJs
ZSBkb3dubG9hZCByZXN1bXB0aW9uIgogICAgICAgICkKICAgICAgICBjb21tb25fZ2xvYmFscy5sb2cuZGVidWcoZiJ7Z2V0X21lZGlhbG9nKGVsZSl9IERh
dGEgZnJvbSBjYWNoZXtkYXRhfSIpCiAgICAgICAgY29tbW9uX2dsb2JhbHMubG9nLmRlYnVnKAogICAgICAgICAgICBmIntnZXRfbWVkaWFsb2coZWxlKX0g
VG90YWwgc2l6ZSBmcm9tIGNhY2hlIHtmb3JtYXRfc2l6ZShkYXRhLmdldCgnY29udGVudC10b3RhbCcpKSBpZiBkYXRhLmdldCgnY29udGVudC10b3RhbCcp
IGVsc2UgJ3Vua25vd24nfSIKICAgICAgICApCiAgICAgICAgdG90YWwgPSBpbnQoZGF0YS5nZXQoImNvbnRlbnQtdG90YWwiKSkgaWYgZGF0YS5nZXQoImNv
bnRlbnQtdG90YWwiKSBlbHNlIE5vbmUKICAgICAgICBpdGVtWyJ0b3RhbCJdID0gdG90YWwKICAgICAgICByZXN1bWVfc2l6ZSA9IHNlbGYuX2dldF9yZXN1
bWVfc2l6ZShwbGFjZWhvbGRlck9iaiwgbWVkaWF0eXBlPWVsZS5tZWRpYXR5cGUpCiAgICAgICAgcmVzdW1lX3NpemUgPSBzZWxmLl9yZXN1bWVfY2xlYW5l
cigKICAgICAgICAgICAgcmVzdW1lX3NpemUsIHRvdGFsLCBwbGFjZWhvbGRlck9iai50ZW1wZmlsZXBhdGgKICAgICAgICApCgogICAgICAgIGNvbW1vbl9n
bG9iYWxzLmxvZy5kZWJ1ZygKICAgICAgICAgICAgZiJ7Z2V0X21lZGlhbG9nKGVsZSl9IHJlc3VtZV9zaXplOiB7cmVzdW1lX3NpemV9ICBhbmQgdG90YWw6
IHt0b3RhbCB9IgogICAgICAgICkKCiAgICAgICAgaWYgdG90YWwgaXMgTm9uZSBvciBpbnQodG90YWwpIDw9IDA6CiAgICAgICAgICAgIHRyeToKICAgICAg
ICAgICAgICAgIGF3YWl0IGFzeW5jaW8uZ2V0X2V2ZW50X2xvb3AoKS5ydW5faW5fZXhlY3V0b3IoCiAgICAgICAgICAgICAgICAgICAgY29tbW9uX2dsb2Jh
bHMudGhyZWFkLAogICAgICAgICAgICAgICAgICAgIHBhcnRpYWwoCiAgICAgICAgICAgICAgICAgICAgICAgIGNhY2hlLnNldCwKICAgICAgICAgICAgICAg
ICAgICAgICAgZiJ7aXRlbVsnbmFtZSddfV97ZWxlLmlkfV97ZWxlLnVzZXJuYW1lfV9oZWFkZXJzIiwKICAgICAgICAgICAgICAgICAgICAgICAgTm9uZSwK
ICAgICAgICAgICAgICAgICAgICApLAogICAgICAgICAgICAgICAgKQogICAgICAgICAgICBleGNlcHQgRXhjZXB0aW9uOgogICAgICAgICAgICAgICAgcGFz
cwogICAgICAgICAgICByZXR1cm4gaXRlbSwgRmFsc2UKCiAgICAgICAgaWYgYXdhaXQgc2VsZi5fY2hlY2tfZm9yY2VkX3NraXAoZWxlLCB0b3RhbCkgPT0g
MDoKICAgICAgICAgICAgaXRlbVsidG90YWwiXSA9IDAKICAgICAgICAgICAgcmV0dXJuIGl0ZW0sIFRydWUKICAgICAgICBlbGlmIHRvdGFsID09IHJlc3Vt
ZV9zaXplOgogICAgICAgICAgICBjb21tb25fZ2xvYmFscy5sb2cuZGVidWcoCiAgICAgICAgICAgICAgICBmIntnZXRfbWVkaWFsb2coZWxlKX0gdG90YWw9
PXJlc3VtZV9zaXplIHNraXBwaW5nIGRvd25sb2FkIgogICAgICAgICAgICApCiAgICAgICAgICAgIHRlbXBfZmlsZV9sb2dnZXIocGxhY2Vob2xkZXJPYmos
IGVsZSkKICAgICAgICAgICAgaWYgc2VsZi5fYWx0X2F0dGVtcHRfZ2V0KGl0ZW0pLmdldCgpID09IDA6CiAgICAgICAgICAgICAgICBwYXNzCiAgICAgICAg
ICAgIGF3YWl0IHNlbGYuX3RvdGFsX2NoYW5nZV9oZWxwZXIoTm9uZSwgdG90YWwpCiAgICAgICAgICAgIHJldHVybiBpdGVtLCBUcnVlCiAgICAgICAgZWxp
ZiB0b3RhbCAhPSByZXN1bWVfc2l6ZToKICAgICAgICAgICAgcmV0dXJuIGl0ZW0sIEZhbHNlCgogICAgYXN5bmMgZGVmIF9mcmVzaF9kYXRhX2hhbmRsZXJf
YWx0KHNlbGYsIGl0ZW0sIGVsZSwgcGxhY2Vob2xkZXJPYmopOgogICAgICAgIGNvbW1vbl9nbG9iYWxzLmxvZy5kZWJ1ZygKICAgICAgICAgICAgZiJ7Z2V0
X21lZGlhbG9nKGVsZSl9IFthdHRlbXB0IHtjb21tb25fZ2xvYmFscy5hdHRlbXB0LmdldCgpfS97Z2V0X2Rvd25sb2FkX3JldHJpZXMoKX1dIGZyZXNoIGRv
d25sb2FkIGZvciBtZWRpYSIKICAgICAgICApCiAgICAgICAgcmVzdW1lX3NpemUgPSBzZWxmLl9nZXRfcmVzdW1lX3NpemUocGxhY2Vob2xkZXJPYmosIG1l
ZGlhdHlwZT1lbGUubWVkaWF0eXBlKQogICAgICAgIGNvbW1vbl9nbG9iYWxzLmxvZy5kZWJ1ZyhmIntnZXRfbWVkaWFsb2coZWxlKX0gcmVzdW1lX3NpemU6
IHtyZXN1bWVfc2l6ZX0iKQogICAgICAgIHJldHVybiBpdGVtLCBGYWxzZQoKICAgIGRlZiBfYWx0X2F0dGVtcHRfZ2V0KHNlbGYsIGl0ZW0pOgogICAgICAg
IGlmIGl0ZW1bInR5cGUiXSA9PSAidmlkZW8iOgogICAgICAgICAgICByZXR1cm4gY29tbW9uX2dsb2JhbHMuYXR0ZW1wdAogICAgICAgIGlmIGl0ZW1bInR5
cGUiXSA9PSAiYXVkaW8iOgogICAgICAgICAgICByZXR1cm4gY29tbW9uX2dsb2JhbHMuYXR0ZW1wdDIKCiAgICBhc3luYyBkZWYgX2dldF9kYXRhKHNlbGYs
IGVsZSwgaXRlbSk6CiAgICAgICAgZGF0YSA9IGF3YWl0IGFzeW5jaW8uZ2V0X2V2ZW50X2xvb3AoKS5ydW5faW5fZXhlY3V0b3IoCiAgICAgICAgICAgIGNv
bW1vbl9nbG9iYWxzLnRocmVhZCwKICAgICAgICAgICAgcGFydGlhbChjYWNoZS5nZXQsIGYie2l0ZW1bJ25hbWUnXX1fe2VsZS5pZH1fe2VsZS51c2VybmFt
ZX1faGVhZGVycyIpLAogICAgICAgICkKICAgICAgICByZXR1cm4gZGF0YQoKICAgIGFzeW5jIGRlZiBfc2V0X2RhdGEoc2VsZiwgZWxlLCBpdGVtLCBkYXRh
KToKICAgICAgICBkYXRhID0gYXdhaXQgYXN5bmNpby5nZXRfZXZlbnRfbG9vcCgpLnJ1bl9pbl9leGVjdXRvcigKICAgICAgICAgICAgY29tbW9uX2dsb2Jh
bHMudGhyZWFkLAogICAgICAgICAgICBwYXJ0aWFsKGNhY2hlLnNldCwgZiJ7aXRlbVsnbmFtZSddfV97ZWxlLmlkfV97ZWxlLnVzZXJuYW1lfV9oZWFkZXJz
IiwgZGF0YSksCiAgICAgICAgKQogICAgICAgIHJldHVybiBkYXRhCgogICAgZGVmIF9nZXRfaXRlbV90b3RhbChzZWxmLCBpdGVtKToKICAgICAgICByZXR1
cm4gaXRlbVsicGF0aCJdLmFic29sdXRlKCkuc3RhdCgpLnN0X3NpemUKCiAgICBhc3luYyBkZWYgX21lZGlhX2l0ZW1fcG9zdF9wcm9jZXNzX2FsdChzZWxm
LCBhdWRpbywgdmlkZW8sIGVsZSwgdXNlcm5hbWUsIG1vZGVsX2lkKToKICAgICAgICBhdWRpb190b3RhbCA9IGF1ZGlvWyJ0b3RhbCJdIGlmIGF1ZGlvIGVs
c2UgMAogICAgICAgIHZpZGVvX3RvdGFsID0gdmlkZW9bInRvdGFsIl0gaWYgdmlkZW8gZWxzZSAwCgogICAgICAgIGlmIChhdWRpb190b3RhbCArIHZpZGVv
X3RvdGFsKSA9PSAwOgogICAgICAgICAgICBpZiBlbGUubWVkaWF0eXBlLmNhcGl0YWxpemUoKSA9PSAiRm9yY2VkX3NraXBwZWQiOgogICAgICAgICAgICAg
ICAgcmV0dXJuIGVsZS5tZWRpYXR5cGUsIDAKICAgICAgICAgICAgcmFpc2UgRXhjZXB0aW9uKAogICAgICAgICAgICAgICAgZiJ7Z2V0X21lZGlhbG9nKGVs
ZSl9IERSTSBkb3dubG9hZCBwcm9kdWNlZCAwIGJ5dGVzICIKICAgICAgICAgICAgICAgICIoYXVkaW8rdmlkZW8pLiBOb3QgbWFya2luZyBhcyBkb3dubG9h
ZGVkLiIKICAgICAgICAgICAgKQogICAgICAgIGZvciBtIGluIFthdWRpbywgdmlkZW9dOgogICAgICAgICAgICBtWyJ0b3RhbCJdID0gc2VsZi5fZ2V0X2l0
ZW1fdG90YWwobSkKCiAgICAgICAgZm9yIG0gaW4gW2F1ZGlvLCB2aWRlb106CiAgICAgICAgICAgIGlmIG5vdCBpc2luc3RhbmNlKG0sIGRpY3QpOgogICAg
ICAgICAgICAgICAgcmV0dXJuIG0KICAgICAgICAgICAgYXdhaXQgc2VsZi5fc2l6ZV9jaGVja2VyKG1bInBhdGgiXSwgZWxlLCBtWyJ0b3RhbCJdKQoKICAg
IGFzeW5jIGRlZiBfbWVkaWFfaXRlbV9rZXlzX2FsdChzZWxmLCBjLCBhdWRpbywgdmlkZW8sIGVsZSk6CiAgICAgICAgYXN5bmMgZm9yIF8gaW4gZG93bmxv
YWRfcmV0cnkoKToKICAgICAgICAgICAgd2l0aCBfOgogICAgICAgICAgICAgICAgdHJ5OgogICAgICAgICAgICAgICAgICAgIGZvciBpdGVtIGluIFthdWRp
bywgdmlkZW9dOgogICAgICAgICAgICAgICAgICAgICAgICBpdGVtID0gYXdhaXQga2V5aGVscGVycy51bl9lbmNyeXB0KGl0ZW0sIGMsIGVsZSkKICAgICAg
ICAgICAgICAgIGV4Y2VwdCBFeGNlcHRpb24gYXMgRToKICAgICAgICAgICAgICAgICAgICBjb21tb25fZ2xvYmFscy5sb2cudHJhY2ViYWNrXyhFKQogICAg
ICAgICAgICAgICAgICAgIGNvbW1vbl9nbG9iYWxzLmxvZy50cmFjZWJhY2tfKHRyYWNlYmFjay5mb3JtYXRfZXhjKCkpCiAgICAgICAgICAgICAgICAgICAg
cmFpc2UgRQoKCgogICAgYXN5bmMgZGVmIF9hZGRfZG93bmxvYWRfam9iX3Rhc2soc2VsZiwgZWxlLCB0b3RhbD1Ob25lLCBwbGFjZWhvbGRlck9iaj1Ob25l
KToKICAgICAgICBwYXRoc3RyID0gc3RyKHBsYWNlaG9sZGVyT2JqLnRlbXBmaWxlcGF0aCkKICAgICAgICB0YXNrMSA9IE5vbmUKICAgICAgICBpZiBub3Qg
c2VsZi5fbXVsdGk6CiAgICAgICAgICAgIHRhc2sxID0gcHJvZ3Jlc3NfdXBkYXRlci5hZGRfZG93bmxvYWRfam9iX3Rhc2soCiAgICAgICAgICAgICAgICBm
InsocGF0aHN0cls6Y29uc3RhbnRzLmdldGF0dHIoJ1BBVEhfU1RSX01BWCcpXSArICcuLi4uJykgaWYgbGVuKHBhdGhzdHIpID4gY29uc3RhbnRzLmdldGF0
dHIoJ1BBVEhfU1RSX01BWCcpIGVsc2UgcGF0aHN0cn1cbiIsCiAgICAgICAgICAgICAgICB0b3RhbD10b3RhbCwKICAgICAgICAgICAgKQogICAgICAgIGVs
c2U6CiAgICAgICAgICAgIGF3YWl0IHNlbmRfbXNnKAogICAgICAgICAgICAgICAgcGFydGlhbCgKICAgICAgICAgICAgICAgICAgICBwcm9ncmVzc191cGRh
dGVyLmFkZF9kb3dubG9hZF9qb2JfbXVsdGlfdGFzaywKICAgICAgICAgICAgICAgICAgICBmInsocGF0aHN0cls6Y29uc3RhbnRzLmdldGF0dHIoJ1BBVEhf
U1RSX01BWCcpXSArICcuLi4uJykgaWYgbGVuKHBhdGhzdHIpID4gY29uc3RhbnRzLmdldGF0dHIoJ1BBVEhfU1RSX01BWCcpIGVsc2UgcGF0aHN0cn1cbiIs
CiAgICAgICAgICAgICAgICAgICAgZWxlLmlkLAogICAgICAgICAgICAgICAgICAgIHRvdGFsPXRvdGFsLAogICAgICAgICAgICAgICAgICAgIGZpbGU9cGxh
Y2Vob2xkZXJPYmoudGVtcGZpbGVwYXRoLAogICAgICAgICAgICAgICAgKQogICAgICAgICAgICApCiAgICAgICAgcmV0dXJuIHRhc2sxCgpkZWYgX21wZF9z
ZWdtZW50X3VybChtcGRfdXJsOiBzdHIsIHNlZ21lbnRfbmFtZTogc3RyKSAtPiBzdHI6CiAgICBwYXJ0cyA9IHVybHNwbGl0KChtcGRfdXJsIG9yICIiKS5z
dHJpcCgpKQogICAgbmFtZSA9IChzZWdtZW50X25hbWUgb3IgIiIpLnN0cmlwKCkubHN0cmlwKCIvIikKICAgIHBhdGggPSBwYXJ0cy5wYXRoIG9yICIiCiAg
ICBiYXNlX3BhdGggPSByZS5zdWIociJbXi9dK1wubXBkJCIsICIiLCBwYXRoLCBmbGFncz1yZS5JR05PUkVDQVNFKQogICAgaWYgYmFzZV9wYXRoID09IHBh
dGg6CiAgICAgICAgYmFzZV9wYXRoID0gKHBhdGgucnNwbGl0KCIvIiwgMSlbMF0gKyAiLyIpIGlmICIvIiBpbiBwYXRoIGVsc2UgIi8iCiAgICBlbGlmIG5v
dCBiYXNlX3BhdGguZW5kc3dpdGgoIi8iKToKICAgICAgICBiYXNlX3BhdGggKz0gIi8iCiAgICByZXR1cm4gdXJsdW5zcGxpdCgocGFydHMuc2NoZW1lLCBw
YXJ0cy5uZXRsb2MsIGJhc2VfcGF0aCArIG5hbWUsIHBhcnRzLnF1ZXJ5LCAiIikpCgo=
'@
}

function Get-DrmPatchBytes {
    $b64 = Get-DrmPatchBase64
    $b64 = ($b64 -replace '\s', '')
    return [Convert]::FromBase64String($b64)
}

function Install-DrmPatch {
    param(
        [string]$ExePath,
        [bool]$Reinstalled
    )
    $script:DrmPatchStatus = "not applied"
    if ($SkipDrmPatch) {
        Write-ColorOutput "Skipping DRM patch (-SkipDrmPatch)." "Yellow"
        $script:DrmPatchStatus = "skipped"
        return $true
    }

    $pkg = Get-OfScraperPackageDir -ExePath $ExePath
    if (-not $pkg) {
        Write-ColorOutput "Could not locate the ofscraper package directory; DRM patch was not applied." "Red"
        return $false
    }

    $target = Join-Path $pkg $DrmPatchRelPath
    if (-not (Test-Path -LiteralPath $target)) {
        Write-ColorOutput "Expected DRM file is missing: $target" "Red"
        return $false
    }

    if (-not $Reinstalled -and (Test-DrmPatchApplied -AltDownloadPath $target)) {
        Write-ColorOutput "DRM false-success patch is already present." "Green"
        $script:DrmPatchStatus = $DrmPatchId
        return $true
    }

    Write-ColorOutput "" "White"
    Write-ColorOutput "Applying DRM false-success patch ($DrmPatchId)..." "Yellow"
    Write-ColorOutput "Empty DRM segments and 0-byte audio+video will no longer be marked downloaded." "Gray"

    $stamp = Get-Date -Format "yyyyMMdd_HHmmss"
    $backupRoot = Join-Path $env:TEMP "ofscraper_patch_backup_${DrmPatchId}_$stamp"
    $backupFile = Join-Path $backupRoot $DrmPatchRelPath
    New-Item -ItemType Directory -Force -Path (Split-Path -Parent $backupFile) | Out-Null
    Copy-Item -LiteralPath $target -Destination $backupFile -Force

    try {
        $bytes = Get-DrmPatchBytes
        [System.IO.File]::WriteAllBytes($target, $bytes)
    }
    catch {
        Write-ColorOutput "DRM patch failed: $($_.Exception.Message)" "Red"
        Write-ColorOutput "Restore from backup: $backupFile" "Yellow"
        return $false
    }

    if (-not (Test-DrmPatchApplied -AltDownloadPath $target)) {
        Write-ColorOutput "DRM patch wrote a file, but the expected fix marker is missing." "Red"
        Write-ColorOutput "Restore from backup: $backupFile" "Yellow"
        return $false
    }

    Write-ColorOutput "DRM patch applied." "Green"
    Write-ColorOutput "Backup: $backupFile" "Gray"
    Write-ColorOutput "If an earlier run marked protected media as downloaded with 0 bytes, rescrape that media." "Gray"
    $script:DrmPatchStatus = $DrmPatchId
    return $true
}

function Pause-IfInteractive {
    if ($Host.Name -eq "ConsoleHost" -and -not $Quiet) {
        Write-Host ""
        Write-Host "Press any key to exit..." -ForegroundColor Gray
        $null = $Host.UI.RawUI.ReadKey("NoEcho,IncludeKeyDown")
    }
}

Write-ColorOutput "" "White"
Write-ColorOutput "=== SCrawler OF-Scraper installer ===" "Cyan"
Write-ColorOutput "Target: ofscraper==$OfScraperVersion + aiolimiter==$AioLimiterVersion" "Gray"
Write-ColorOutput "Python: 3.11.x or 3.12.x only (not 3.10 or older, not 3.13+)" "Gray"
Write-ColorOutput "DRM patch: $DrmPatchId (alt_download.py false-success fix)" "Gray"
Write-ColorOutput "" "White"

$installs = @(Find-PythonInstalls)
$supported = @($installs | Where-Object { $_.Supported } | Sort-Object @{ Expression = { $_.Version.Minor } }, @{ Expression = { $_.Version.Build } } -Descending)
# Prefer 3.11 over 3.12: 3.11 is the version SCrawler/OF-Scraper docs recommend (3.11.6).
$preferred = @($supported | Where-Object { $_.Version.Minor -eq 11 } | Sort-Object Version -Descending)
if ($preferred.Count -gt 0) {
    $python = $preferred[0]
} elseif ($supported.Count -gt 0) {
    $python = ($supported | Sort-Object Version -Descending)[0]
} else {
    $python = $null
}

if (-not $python) {
    $reason = "A supported Python was not found. Need 3.11.x or 3.12.x (not older than 3.11, not 3.13+)."
    if ($PythonPath) {
        $reason = "The Python executable at '$PythonPath' is missing, or is older than 3.11.x, or is 3.13+."
    }
    Show-PythonMissing -Reason $reason -Unsupported @($installs | Where-Object { -not $_.Supported })
    Pause-IfInteractive
    exit 1
}

Write-ColorOutput ("Using Python {0}" -f $python.Version) "Green"
Write-ColorOutput $python.Path "Gray"

if (-not (Test-Pip -ExePath $python.Path)) {
    if (-not (Install-Pip -ExePath $python.Path)) {
        Show-PythonMissing -Reason "pip is not available for this Python install and ensurepip failed."
        Pause-IfInteractive
        exit 1
    }
    Write-ColorOutput "pip is now available." "Green"
}

$currentOfs = Get-PipPackageVersion -ExePath $python.Path -PackageName "ofscraper"
$currentAio = Get-PipPackageVersion -ExePath $python.Path -PackageName "aiolimiter"

if ($currentOfs) {
    Write-ColorOutput "Installed ofscraper: $currentOfs" "White"
} else {
    Write-ColorOutput "ofscraper is not installed." "Yellow"
}
if ($currentAio) {
    Write-ColorOutput "Installed aiolimiter: $currentAio" "White"
}

$needOfs = $Force -or ($currentOfs -ne $OfScraperVersion)

if ($needOfs) {
    Write-ColorOutput "" "White"
    Write-ColorOutput "Installing ofscraper==$OfScraperVersion ..." "Yellow"
    if (-not (Invoke-PipInstall -ExePath $python.Path -PipArgs @("ofscraper==$OfScraperVersion"))) {
        Write-ColorOutput "Failed to install ofscraper==$OfScraperVersion" "Red"
        Pause-IfInteractive
        exit 1
    }
} else {
    Write-ColorOutput "ofscraper $OfScraperVersion is already installed." "Green"
}

Write-ColorOutput "" "White"
Write-ColorOutput "Pinning aiolimiter==$AioLimiterVersion (newer versions break OF-Scraper $OfScraperVersion)..." "Yellow"
if (-not (Invoke-PipInstall -ExePath $python.Path -PipArgs @("--force-reinstall", "--no-deps", "aiolimiter==$AioLimiterVersion"))) {
    Write-ColorOutput "Failed to install aiolimiter==$AioLimiterVersion" "Red"
    Pause-IfInteractive
    exit 1
}

$finalOfs = Get-PipPackageVersion -ExePath $python.Path -PackageName "ofscraper"
$finalAio = Get-PipPackageVersion -ExePath $python.Path -PackageName "aiolimiter"

if ($finalOfs -ne $OfScraperVersion) {
    Write-ColorOutput "ofscraper version after install is '$finalOfs', expected $OfScraperVersion." "Red"
    Pause-IfInteractive
    exit 1
}
if ($finalAio -ne $AioLimiterVersion) {
    Write-ColorOutput "aiolimiter version after install is '$finalAio', expected $AioLimiterVersion." "Red"
    Pause-IfInteractive
    exit 1
}

$drmOk = Install-DrmPatch -ExePath $python.Path -Reinstalled $needOfs
if (-not $drmOk -and -not $SkipDrmPatch) {
    Write-ColorOutput "Install finished, but the DRM patch did not apply." "Yellow"
}

$ofsExe = Get-OfScraperExe -ExePath $python.Path

Write-ColorOutput "" "White"
Write-ColorOutput "Install complete." "Green"
Write-ColorOutput ("  ofscraper    {0}" -f $finalOfs) "White"
Write-ColorOutput ("  aiolimiter   {0}" -f $finalAio) "White"
Write-ColorOutput ("  DRM patch    {0}" -f $DrmPatchStatus) "White"
Write-ColorOutput ("  Python       {0}" -f $python.Version) "White"

if ($ofsExe) {
    Write-ColorOutput "" "White"
    Write-ColorOutput "Paste this into SCrawler: Settings -> OnlyFans -> OF-Scraper path" "Cyan"
    Write-ColorOutput $ofsExe "Green"
} else {
    Write-ColorOutput "" "White"
    Write-ColorOutput "ofscraper installed, but ofscraper.exe was not found on disk." "Yellow"
    Write-ColorOutput "Look under the Python Scripts folder next to:" "Yellow"
    Write-ColorOutput $python.Path "Gray"
}

Pause-IfInteractive
exit 0
