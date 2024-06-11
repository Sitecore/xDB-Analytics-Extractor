param (
    [string]$jsonFilePath = '.\yourconfig.json',
    [string]$scriptPath = '.\utils\checkout-action.ps1',
    [string]$step = 'checkout'
)

# Force Update

# Read JSON content from file
$jsonContent = Get-Content $jsonFilePath -Raw | ConvertFrom-Json

# Check if the specified section exists in the JSON
if ($null -eq $jsonContent.$step) {
    Write-Host "Error: JSON section '$step' not found." -ForegroundColor Red
    exit 1
}

# Initialize an empty string for script arguments
$scriptArguments = ''

# Iterate through JSON properties and add them to script arguments
$jsonContent.$step.PSObject.Properties | ForEach-Object {
    $propertyName = $_.Name
    $propertyValue = $_.Value

    # Only add to script arguments if the property has a value
    if ($propertyValue -ne $null -and $propertyValue -ne '' -and -not ($propertyValue -eq $true -or $propertyValue -eq $false)) {
        $scriptArguments += "-$propertyName '$propertyValue' "
    } elseif ($propertyValue -ne $null -and ($propertyValue -ne $false)) {
        $scriptArguments += "-$propertyName "
    }
}

# Construct the final command to invoke the other script
$fullCommand = "$scriptPath $scriptArguments"

Write-Host $fullCommand

# Execute the constructed command
Invoke-Expression $fullCommand