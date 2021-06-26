param (
    [string] $bid = $null, #branch identifier
    [string] $tid = $null, #tag identifier
    [string] $mid = $null, #merge request identifier
    [string] $cid = $null, #commit identifier
    [string] $buildCounter = $null, 
    [string] $envFile = $null,
    [switch] $customBuild)

if ($bid -eq $null -and $tid -eq $null -and $mid -eq $null -and $customBuild.IsPresent) {
    $bid = git rev-parse --abbrev-ref HEAD 2>&1;
    $tid = git describe --exact-match --abbrev=0 --match "v*" 2>&1
}

if ([string]::IsNullOrEmpty($cid)) {
    $cid = git rev-parse --short HEAD;
}

if ([string]::IsNullOrEmpty($buildCounter)) {
    $buildCounter = "0";
}

if (-not [string]::IsNullOrEmpty($bid) -and $bid -ne "HEAD") {
    switch -regex ($bid) {
        "^master$" { 
            $semName = "-master"
            $semMeta = "+$cid"
        }
        "^release/(.*)$" { 
            $semName = ""
            $semMeta = "+$cid"
        }
        "^(?<BranchType>(defect|feature))(/([a-z]+))?/(?<TaskRef>[a-z]{3,}[0-9]+-[0-9]+)(-((.*))?)?$" { 
            $semName = "-$($matches.BranchType)-$($matches.TaskRef)"
            $semMeta = ""
        }
        default { 
            Write-Error "Unexpected branch name: $bid" -ErrorAction Stop
        }
    }
} elseif (-not [string]::IsNullOrEmpty($tid) -and $tid -match "^v([0-9]+)\.([0-9]+)\.([0-9]+)$") {
    $semName = ""
    $semMeta = "+$cid"
} elseif (-not [string]::IsNullOrEmpty($mid)) {
    $semName = "-merge-request-$mid"
    $semMeta = "+$cid"
} else {
    Write-Error "Unsupported type of build" -ErrorAction Stop
}

if (-not [string]::IsNullOrEmpty($tid)) {
    $versionText = $tid.Trim(" ", "`r", "`n");
} else {
    git fetch --tags 2>&1
    $tagVersion = git describe --abbrev=0 --match "v*" 2>&1
    $versionText = $tagVersion.ToString().Trim(" ", "`r", "`n");
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