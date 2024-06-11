param (
    [string]$fileType = "sln",
    [string]$projectName = $null,
    [string]$configuration = $null,
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

$cleanCommand = "dotnet clean "

if ($null -ne $configuration -and $configuration -ne '') {
    $cleanCommand += "--configuration $configuration "
}

if ($null -ne $verbosity -and $verbosity -ne '') {
    $cleanCommand += "--verbosity $verbosity"
}

Invoke-Expression $cleanCommand