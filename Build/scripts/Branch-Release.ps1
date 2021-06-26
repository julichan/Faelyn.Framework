param ([string] $tid = $null)

if ([string]::IsNullOrEmpty($tid)) { 
    Write-Error "Release branches can only be created from a tag" -ErrorAction Stop
} 

$versionRegex = [regex]::match($tid, "^v(?<major>[0-9]+)\.(?<minor>[0-9]+)\.(?<revision>[0-9]+)$")
$versionData = if (-not $versionRegex.Success) { @{ major = 0; minor = 0; revision = 0; } } 
              else { @{ 
                      major = $versionRegex.Groups['major'].Value -as [int]; 
                      minor = $versionRegex.Groups['minor'].Value -as [int]; 
                      revision = $versionRegex.Groups['revision'].Value -as [int]; 
                    } };
$shortVersion = $(-join ($versionData.major, ".", $versionData.minor, ".", $versionData.revision)).Trim(" ", "`r", "`n")

$newBranch = "release/$shortVersion"
$checkBranch = git ls-remote --heads origin $newBranch 2>&1
if (-not [string]::IsNullOrWhiteSpace($checkBranch)) {
    Write-Host "Creating release branch..."
    git checkout -qb $newBranch
    git push -q origin $newBranch
    git checkout -q $branch
    git branch -qD $newBranch
}