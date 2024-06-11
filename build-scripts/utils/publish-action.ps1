param(
    [string]$fileType = "sln",
    [string]$projectName = $null,
    [string]$configuration = $null,
    [string]$framework = $null,
    [switch]$force = $false,
    [switch]$interactive = $false,
    [string]$manifest = $null,
    [switch]$build = $false,
    [switch]$dependencies = $false,
    [switch]$restore = $false,
    [string]$output = $null,
    [string]$os = $null,
    [string]$source = $null,
    [string]$verbosity = $null
)

# Check if a project name was provided and the file type is .csproj
if ($FileType -eq 'csproj' -and !$ProjectName) {
    Write-Host "No project name provided." -ForegroundColor Red
    exit 1
}

# Get all .sln or .csproj files in the current directory and its subdirectories
if ($FileType -eq 'csproj') {
    $files = Get-ChildItem -Path . -Recurse -Include "$ProjectName.$FileType"
} else {
    $files = Get-ChildItem -Path . -Recurse -Include *.$FileType
}

# Check if any .sln or .csproj files exist
if ($files.Count -eq 0) {
    Write-Host "No .$FileType file found for the project '$ProjectName' in the current directory or its subdirectories." -ForegroundColor Red
    exit 1
} else {
    # If the project name is empty and the file type is .sln, display the name of the solution file
    if (!$ProjectName -and $FileType -eq 'sln') {
        $ProjectName = $files[0].BaseName
    }

    Write-Host ".$FileType file found for the project '$ProjectName' in the current directory or its subdirectories." -ForegroundColor Green

    # Navigate to the directory of the found file
    Set-Location -Path $files[0].Directory.FullName
    Write-Host "Navigated to directory: $($files[0].Directory.FullName)" -ForegroundColor DarkYellow
}

$publishCommand = "dotnet publish"

if ($null -ne $configuration -and $configuration -ne '') {
    $publishCommand += "--configuration $configuration "
}

if ($null -ne $framework -and $framework -ne '') {
    $publishCommand += "--framework $framework "
}

if ($null -ne $force -and $force -ne $false) {
    $publishCommand += "--force "
}

if ($null -ne $interactive -and $interactive -ne $false) {
    $publishCommand += "--interactive "
}

if ($null -ne $manifest -and $manifest -ne '') {
    $publishCommand += "--manifest $manifest "
}

if ($null -eq $build -or $build -eq $false) {
    $publishCommand += "--no-build "
}

if ($null -eq $dependencies -or $dependencies -eq $false) {
    $publishCommand += "--no-dependencies "
}

if ($null -eq $restore -or $restore -eq $false) {
    $publishCommand += "--no-restore "
}

if ($null -ne $output -and $output -ne '') {
    $publishCommand += "--output $output "
}

if ($null -ne $os -and $os -ne '') {
    $publishCommand += "--os $os "
}

if ($null -ne $source -and $source -ne '') {
    $publishCommand += "--source $source "
}

if ($null -ne $verbosity -and $verbosity -ne '') {
    $publishCommand += "--verbosity $verbosity"
}

Invoke-Expression $publishCommand