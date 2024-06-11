# Publish Configuration

This action publishes the application and its dependencies to a folder for deployment, similar to the `dotnet publish` command.

## Usage 
```yaml
publish: 
    # The filetype of the project for which the action will be performed.
    # Default: sln
    fileType: 'sln'

    # The name of the project to perform the action.
    # Default: null
    projectName: ''

    # The build configuration of the project.
    # Default: null
    configuration: ''

    # Publishes the application for a specified target framework.
    # Default: null
    framework: ''

    # Forces all dependencies to be resolved even if the last restore was successful.
    # Default: false
    force: ''

    # Allows the command to stop and wait for user input or action.
    # Default: false
    interactive: ''

    # Specifies one or several target manifests to use to trim the set of packages published with the app.
    # Default: null
    manifest: ''

    # Explicitly build the project before restore.
    # Default: false
    build: ''

    # Explicitly ignores project-to-project references and only restores the root project.
    # Default: false
    dependencies: ''

    # Explicilty runs a restore before publishing the project.
    # Default: false
    restore: ''

    # Specifies the path for the output directory.
    # Default: null
    output: ''

    # Specifies the target operating system (OS).
    # Default: null
    os: ''

    # The URI of the NuGet package source to use during the restore operation
    # Default: null
    source: ''

    # The verbosity level of the action.
    # Default: null
    verbosity: ''
```