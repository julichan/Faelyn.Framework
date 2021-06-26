param (
    [string] $branch = $null,
    [string] $version = $null)

if ([string]::IsNullOrEmpty($branch)) {
    $branch = git rev-parse --abbrev-ref HEAD 2>&1;
}

if (($branch -ne "master" -and $branch -notmatch "release/(?<major>[0-9]+)\.(?<minor>[0-9]+)\.(?<revision>[0-9]+)")) { 
    Write-Error "Release branches can only be created from tagged the master branch or a release branch." -ErrorAction Stop
} 

$newBranch = "release/$version"
$checkBranch = git ls-remote --heads origin $newBranch 2>&1
if (-not [string]::IsNullOrWhiteSpace($checkBranch)) {
    Write-Error "Branch already exists" -ErrorAction Stop
}

Write-Host "Creating release branch..."
git checkout -qb $newBranch
git push -q origin $newBranch
git checkout -q $branch
git branch -qD $newBranch