# Restore Configuarion
This action restores all the dependencies and tools of the service.

The action by default will check for a `.sln` file to perform the restore on.

> [!CAUTION]
> It is recommended to ALWAYS have this step in your `install.json` configuration, since most 
> actions do not perform an implicit restore.


## Usage
```yaml
restore:
    # The filetype of the project for which the action will be performed.
    # Default: sln
    fileType: 'sln'

    # The name of the project to perform the action.
    # Default: null
    projectName: ''

    # The build configuration of the project.
    # Default: null
    configuration: ''

    # Disable restoring multiple projects in parallel.
    # Default: false
    disableParallel: ''

    # Forces all dependencies to be resolved.
    # Default: false
    force: ''

    # Forces a reevaluation of all dependencies.
    # Default: false
    forceEvaluate: ''

    # Do not cache HTTP requests.
    # Default: false
    noCache: ''

    # Restore the root project and not the references.
    # Default: false
    noDependencies: ''

    # The verbosity level of the command.
    # Default: null
    verbosity: ''
```

## Scenarios
- [Restore the service solution](#restore-the-service-solution)
- [Restore a specific service project](#restore-a-specific-service-project)
- [Clean a solution with Release configuration](#clean-a-solution-with-release-configuration)


### Restore the service solution
```json
"restore": {}
```

### Restore a specific service project
```json
"restore": {
    "fileType": "csproj",
    "projectName": "xDB Analytics Extractor"
}
```

### Force reevaluation and restoration of all dependencies
```json
"restore": {
    "force": true,
    "forceEvaluation": true
}
```