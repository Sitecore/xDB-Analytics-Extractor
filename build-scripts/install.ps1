function Get-JsonFirstLevelKeys {
    param (
        [string]$jsonFilePath
    )

    # Read the JSON file
    $jsonContent = Get-Content -Raw -Path $jsonFilePath | ConvertFrom-Json

    # Get the first level keys as strings
    $keys = $jsonContent | Get-Member -MemberType Properties | Select-Object -ExpandProperty Name | ForEach-Object { $_.ToString() }

    return $keys
}

$jsonFilePath = ".\yourconfig.json"
$firstLevelKeys = Get-JsonFirstLevelKeys -jsonFilePath $jsonFilePath

if ($($firstLevelKeys -contains "checkout")) {
    Invoke-Expression -Command ".\invoke.ps1"
    Set-Location ..                 
}

if ($($firstLevelKyes -contains "clean")) {
    Invoke-Expression -Command ".\invoke.ps1 -scriptPath .\utils\clean-action.ps1 -step clean"
    Set-Location ..
}

if ($($firstLevelKeys -contains "restore")) {
    Invoke-Expression -Command ".\invoke.ps1 -scriptPath .\utils\restore-action.ps1 -step restore"
    Set-Location ..
}

if ($($firstLevelKeys -contains "build")) {
    Invoke-Expression -Command ".\invoke.ps1 -scriptPath .\utils\build-action.ps1 -step build"
    Set-Location ..
}


if ($($firstLevelKeys -contains "test")) {
    Invoke-Expression -Command ".\invoke.ps1 -scriptPath .\utils\test-action.ps1 -step test"
    Set-Location ..
}

if ($($firstLevelKeys -contains "csv")) {
    Invoke-Expression -Command ".\invoke.ps1 -scriptPath .\utils\create-directories-for-csv-action.ps1 -step csv"
}

if ($($firstLevelKeys -contains "register")) {
    Invoke-Expression -Command ".\invoke.ps1 -scriptPath .\utils\register-action.ps1 -step register"
}