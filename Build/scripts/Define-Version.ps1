param (
    [string] $branch = $null,
    [string] $commitHash = $null, 
    [string] $buildCounter = $null, 
    [string] $envFile = $null,
    [string] $verFile = $null)

if ([string]::IsNullOrEmpty($branch)) {
    $branch = git rev-parse --abbrev-ref HEAD 2>&1;
}

if ([string]::IsNullOrEmpty($commitHash)) {
    $commitHash = git rev-parse --short HEAD;
}

if ([string]::IsNullOrEmpty($buildCounter)) {
    $buildCounter = "0";
}

switch -regex ($branch) {
    "^master$" { 
        $semName = "-master"
        $semMeta = "+$commitHash"
    }
    "^release/(.*)$" { 
        $semName = ""
        $semMeta = "+$commitHash"
    }
    "^(?<BranchType>(defect|feature))(/([a-z]+))?/(?<TaskRef>[a-z]{3,}[0-9]+-[0-9]+)(-((.*))?)?$" { 
        $semName = "-$($matches.BranchType)-$($matches.TaskRef)"
        $semMeta = ""
    }
    default { 
        Write-Error "Unexpected branch name: $branch" -ErrorAction Stop
    }
}

if ([string]::IsNullOrEmpty($verFile)) {
    git fetch --tags 2>&1
    $tagVersion = git describe --abbrev=0 --match "v*" 2>&1
    $versionText = $tagVersion.ToString().Trim(" ", "`r", "`n");
} else {
    $fileVersion = if (Test-Path $verFile -PathType leaf) { Get-Content $verFile } else { "" }
    $versionText = $fileVersion.ToString().Trim(" ", "`r", "`n");
}

$versionRegex = [regex]::match($versionText, "^v(?<major>[0-9]+)\.(?<minor>[0-9]+)\.(?<revision>[0-9]+)$")
$versionData = if (-not $versionRegex.Success) { @{ major = 0; minor = 0; revision = 0; } } 
              else { @{ 
                      major = $versionRegex.Groups['major'].Value -as [int]; 
                      minor = $versionRegex.Groups['minor'].Value -as [int]; 
                      revision = $versionRegex.Groups['revision'].Value -as [int]; 
                    } };

$shortVersion = $(-join ($versionData.major, ".", $versionData.minor, ".", $versionData.revision)).Trim(" ", "`r", "`n")
$standardVersion = -join ($shortVersion, ".", $buildCounter)
$semanticVersion = -join ($shortVersion, $semName, $semMeta) 

if ([string]::IsNullOrEmpty($envFile)) {
    return @{ 
        short = $shortVersion;
        standard = $standardVersion;
        semantic = $semanticVersion;
    }
} else {
    Add-Content $envFile "CI_SHORT_VER=$shortVersion"
    Add-Content $envFile "CI_ASSEMBLY_VER=$standardVersion"
    Add-Content $envFile "CI_SEMAMTIC_VER=$semanticVersion"
}