$baseDir = Resolve-Path(".")
$outputFolder = Join-Path $baseDir "build-output\"
$solution = Join-Path $baseDir "source\Fuzzing.sln"
$windir = $env:windir

if((ls "$windir\Microsoft.NET\Framework\v4.0*") -eq $null ) {
	throw "Building requires .NET 4.0, which doesn't appear to be installed on this machine."
}

$v4_net_version = (ls "$windir\Microsoft.NET\Framework\v4.0*").Name

$msbuild = "$windir\Microsoft.NET\Framework\$v4_net_version\MSBuild.exe"

$options = "/noconsolelogger /p:Configuration=Release /p:OutDir=""$outputFolder"""

if (Test-Path $outputFolder) {
	Remove-Item $outputFolder -Recurse -Force
}

$build = $msbuild + " ""$solution"" " + $options + " /t:Build"

Invoke-Expression $build
