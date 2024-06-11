param (
    [string] $targetDirectory
)

# Force update

# Check if the target directory is provided
if (-not $targetDirectory) {
    Write-Host "Error: Target directory is not provided." -ForegroundColor Red
    exit 1
}

# Create the target directory and all subdirectories
New-Item -ItemType Directory -Force -Path $targetDirectory

# Get all directories and subdirectories in the target directory
$allDirectories = Get-ChildItem $targetDirectory -Directory -Recurse

# Ensure that each directory allows writing
foreach ($directory in $allDirectories) {
    $acl = Get-Acl $directory.FullName
    $rule = New-Object System.Security.AccessControl.FileSystemAccessRule("Everyone", "Modify", "ContainerInherit, ObjectInherit", "None", "Allow")
    $acl.SetAccessRule($rule)
    Set-Acl -Path $directory.FullName -AclObject $acl
    Write-Host "Directory permissions updated for: $($directory.FullName)" -ForegroundColor Green
}

Write-Host "Directories created and permissions updated successfully." -ForegroundColor Green
