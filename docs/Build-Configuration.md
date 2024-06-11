# Build Configuration
This action builds the project and its dependencies into a set of binaries, similar to the `dotnet build` command.

> [!CAUTION]
> It is recommended to ALWAYS have this step in your `install.json` configuration.

## Usage

```yaml
build:
    # The filetype of the project for which the action will be performed.
    # Default: sln
    fileType: 'sln'

    # The name of the project to perform the action.
    # Default: null
    projectName: ''

    # The build configuration of the project.
    # Default: null
    configuration: ''

    # Forces all dependencies to be resolved.
    # Default: false
    force: ''

    # Allows the command to stop and wait for user input or action.
    # Default: false
    interactive: ''
    
    # Ignore P2P dependencies and only build the specified project.
    # Default: false
    noDependencies: ''

    # Marks the build as unsafe for incremental building.
    # Default: false
    noIncremental: ''

    # Execute an implicit restore during build.
    # Default: false
    restore: ''

    # The verbosity level of the action.
    # Default: null
    verbosity: ''
```

# Scenarios
- [Build the current solution](#build-the-current-solution)
- [Build a specific project](#build-a-specific-project)
- [Force rebuilding without dependencies](#force-rebuilding-without-dependencies)
- [Build without incremental compilation](#build-without-incremental-compilation)
- [Build with diagnostic output](#build-with-diagnostic-output)

### Build the current solution
```json
"build": {}
```

### Build a specific project
```json
"build": {
    "fileType": "csproj",
    "projectName": "xDB Analytics Extractor"
}
```

### Force rebuilding without dependencies
```json
"build": {
    "force": true,
    "noDependencies": true
}
```
### Build without incremental compilation
```json
"build": {
    "noIncremental": true
}
```

### Build with diagnostic output
```json
"build": {
    "verbosity":"diag"
}
```