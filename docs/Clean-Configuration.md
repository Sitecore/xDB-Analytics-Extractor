# Clean Configuration
This action removes the outputs created during a build, including both intermediate (obj) and final output (bin) folders.

The action by default will check for a `.sln` file to perform the clean on.

> [!CAUTION]
> It is recommended to ALWAYS have this step in your `install.json` configuration.


## Usage
```yaml
clean:
    # The filetype of the project for which the action will be performed.
    # Default: sln
    fileType: 'sln'

    # The name of the project to perform the action.
    # Default: null
    projectName: ''

    # The build configuration of the project.
    # Default: null
    configuration: ''

    # The verbosity level of the action.
    # Default: null
    verbosity: ''
```

## Scenarios
- [Clean the service solution](#clean-the-service-solution)
- [Clean a specific service project](#clean-a-specific-service-project)
- [Clean a solution with Release configuration](#clean-a-solution-with-release-configuration)


### Clean the service solution
```json
"clean": {}
```

### Clean a specific service project
```json
"clean": {
    "fileType": "csproj",
    "projectName": "xDB Analytics Extractor"
}
```

### Clean a solution with Release configuration
```json
"clean": {
    "configuration": "Release"
}
```