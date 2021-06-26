param (
    [string] $branch = $null,
    [string] $releaseType = $null,
    [string] $verFile = $null)

if ([string]::IsNullOrEmpty($branch)) {
    $branch = git rev-parse --abbrev-ref HEAD 2>&1;
}

$currentCommit = git rev-parse --verify HEAD
$latestCommit = git rev-parse "origin/$branch^{commit}"

if (($branch -ne "master" -and $branch -notmatch "release/(?<major>[0-9]+)\.(?<minor>[0-9]+)\.(?<revision>[0-9]+)") -or $currentCommit -ne $latestCommit) { 
    Write-Error "Release branches can only be created from the latest commit of the master branch or a release branch." -ErrorAction Stop
} 

if ([string]::IsNullOrEmpty($verFile)) {
    git fetch --tags 2>&1
    $tagVersion = git describe --exact-match --abbrev=0 --match "v*" 2>&1
    $versionText = $tagVersion.ToString().Trim(" ", "`r", "`n");
} else {
    $fileVersion = if (Test-Path $verFile -PathType leaf) { Get-Content $verFile } else { "" }
    $versionText = $fileVersion.ToString().Trim(" ", "`r", "`n");
}

$versionRegex = [regex]::match($versionText, "^v(?<major>[0-9]+)\.(?<minor>[0-9]+)(\.(?<revision>[0-9]+))?$")
$oldVersionData = if (-not $versionRegex.Success) { @{ major = 0; minor = 0; revision = 0; } } 
               else { @{ 
                      major = $versionRegex.Groups['major'].Value -as [int]; 
                      minor = $versionRegex.Groups['minor'].Value -as [int]; 
                      revision = $versionRegex.Groups['revision'].Value -as [int]; 
                    } };

$newVersionData = switch ($releaseType) {
    "major" {
        @{
            major = $oldVersionData.major + 1;
            minor = 0;
            revision = 0;
        }
    }
    "minor" {
        @{
            major = $oldVersionData.major;
            minor = $oldVersionData.minor + 1;
            revision = 0;
        }
    }
    "revision" {
        @{
            major = $oldVersionData.major;
            minor = $oldVersionData.minor;
            revision = $oldVersionData.revision + 1;
        }
    }
    default {
        Write-Error "Unexpected release type: $releaseType" -ErrorAction Stop
    }
}                    

$newVersion = $(-join ($newVersionData.major, ".", $newVersionData.minor, ".", $newVersionData.revision)).Trim(" ", "`r", "`n")
$tagVersion = $(-join ("v", $newVersion)).Trim(" ", "`r", "`n")

if ([string]::IsNullOrEmpty($verFile)) {
    Write-Host "Creating or overwriting version file..."
    Set-Content $verFile "$tagVersion"
    Write-Host "Adding version file to git..."
    git add $verFile
    Write-Host "Creating git commit..."
    git commit -qm "Version $newVersion"
    Write-Host "Pushing git commit..."
    git push -q
}

Write-Host "Creating git tag..."
git tag -a $tagVersion -m $tagVersion
Write-Host "Pushing tag to git..."
git push -q origin $tagVersion