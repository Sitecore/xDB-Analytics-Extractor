# CSV Output Configuration

This action generates the appropriate file paths for storing the csv output.

> [!NOTE]  
> This action will create the appropriate directory structure and provide read/write
> access to the folders.

## Usage
```yaml
csv: {
    # The directory structure that you want the CSV output to be stored.
    # Default: null
    targetDirectory: ''
}
```