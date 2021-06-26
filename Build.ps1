$version = ./Build/scripts/Define-Version.ps1
dotnet build Faelyn.sln -c Debug "/p:Version=$($version.standard)" "/p:InformationalVersion=$($version.semantic)" "/p:PackageVersion=$($version.semantic)"