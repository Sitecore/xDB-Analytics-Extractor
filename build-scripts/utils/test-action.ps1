param(
    [string]$fileType = "sln",
    [string]$projectName = $null,
    [string]$testAdapterPath = $null,
    [switch]$blame,
    [switch]$blameCrash,
    [string]$blameCrashDumpType = $null,
    [switch]$blameCrashCollectAlways,
    [switch]$blameHang,
    [string]$blameHangDumpType = $null,
    [string]$blameHangTimeout = $null,
    [string]$collector = $null,
    [string]$diagnostics = $null,
    [string]$framework = $null,
    [string]$environment = $null,
    [string]$filter = $null,
    [switch]$interactive,
    [string]$logger = $null,
    [switch]$build,
    [switch]$restore,
    [string]$output = $null,
    [string]$results = $null,
    [string]$runtime = $null,
    [string]$settings = $null,
    [switch]$listTests,
    [string]$verbosity = $null
)

# Force update

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

$testCommand = "dotnet test "

if($null -ne $testAdapterPath -and $testAdapterPath -ne '') {
    $testCommand += "--test-adapter-path $testAdapterPath"
}

if ($null -ne $blame -and $blame -ne $false) {
    $testCommand += "--blame "
}

if ($null -ne $blameCrash -and $blameCrash -ne $false) {
    $testCommand += "--blame-crash "
}

if($null -ne $blameCrashDumpType -and $blameCrashDumpType -ne '') {
    $testCommand += "--blame-crash-dump-type $blameCrashDumpType"
}

if ($null -ne $blameCrashCollectAlways -and $blameCrashCollectAlways -ne $false) {
    $testCommand += "--blame-crash-collect-always "
}

if ($null -ne $blameHang -and $blameHang -ne $false) {
    $testCommand += "--blame-hang "
}

if($null -ne $blameHangDumpType -and $blameHangDumpType -ne '') {
    $testCommand += "--blame-hang-dump-type $blameHangDumpType"
}

if($null -ne $blameHangTimeout -and $blameHangTimeout -ne '') {
    $testCommand += "--blame-hang-timeout $blameHangTimeout"
}

if ($null -ne $configuration -and $configuration -ne '') {
    $testCommand += "--configuration $configuration "
}

if ($null -ne $collector -and $collector -ne '') {
    $buildCommand += "--collect $collector "
}

if ($null -ne $diagnostics -and $diagnostics -ne '') {
    $buildCommand += "--diag $diagnostics "
}

if ($null -ne $framework -and $framework -ne '') {
    $buildCommand += "--framework $framework "
}

if ($null -ne $environment -and $environment -ne '') {
    $buildCommand += "--environment $environment "
}

if ($null -ne $filter -and $filter -ne '') {
    $buildCommand += "--filter $filter "
}

if ($null -ne $interactive -and $interactive -ne $false) {
    $testCommand += "--interactive "
}

if ($null -ne $logger -and $logger -ne '') {
    $buildCommand += "--logger $logger "
}

if ($null -eq $build -or $build -eq $false) {
    $buildCommand += "--no-build "
}

if ($null -eq $restore -or $restore -eq $false) {
    $buildCommand += "--no-restore "
}

if ($null -ne $logger -and $logger -ne '') {
    $testCommand += "--logger "
}

if ($null -ne $output -and $output -ne '') {
    $testCommand += "--output $output "
}

if ($null -ne $results -and $results -ne '') {
    $testCommand += "--results-directory $results "
}

if ($null -ne $runtime -and $runtime -ne '') {
    $testCommand += "--runtime $runtime "
}

if ($null -ne $settings -and $settings -ne '') {
    $testCommand += "--settings $settings "
}

if ($null -eq $listTests -or $listTests -eq $false) {
    $testCommand += "--list-tests "
}

if ($null -ne $verbosity -and $verbosity -ne '') {
    $testCommand += "--verbosity $verbosity"
}

Invoke-Expression $testCommand