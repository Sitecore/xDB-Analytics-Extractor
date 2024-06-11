# Test Configuration
This action executes the unit tests of the solution, similar to the `dotnet test` command.

## Usage
```yaml
test:
    # The filetype of the project for which the action will be performed.
    # Default: sln
    fileType: 'sln'

    # The name of the project to perform the action.
    # Default: null
    projectName: ''

    # Path to a directory to be searched for additional test adapters
    # Default: null
    testAdapterPath: ''

    # Run the tests in blame mode.
    # Default: false
    blame: ''

    # Run the tests in blame mode and collect a crash dump.
    # Default: false
    blameCrash: ''

    # Define the type of the crash dump to be collected.
    # Default: null
    blameCrashDumpType: ''

    # Collect a crash dump on expected as well as unexpected test host exit.
    # Default: false
    blameCrashCollectAlways: ''

    # Run the tests in blame mode and collect a hang dump when a test exceeds the given timeout.
    # Default: false
    blameHang: ''

    # Define the type of the crash dump to be collected.
    # Default: null
    blameHangDumpType: ''

    # Define a per-test timeout, after which a hang dump is triggered.
    # Default: null
    blameHangTimeout: ''

    # Define a data collector for the test run.
    # Default: null
    collector: ''

    # Enable diagnostic mode.
    # Default: null
    diagnostics: ''

    # Set the value of an environmental variable.
    # Default: null
    environment: ''

    # The TFM of the target framework to run tests for.
    # Default: null
    framework: ''

    # Filter tests in the current project using the given expression.
    # Default: null
    filter: ''

    # Allows the command to stop and wait for user input or action.
    # Default: false
    interactive: ''

    # Specify a logger for test results and optionally switches for the logger.
    # Default: null
    logger: ''

    # Allow for an implicit build run.
    # Default: false
    build: ''

    # Allow for an implicit restore run.
    # Default: false
    restore: ''

    # Specify the directory in which to find the binaries to run.
    # Default: null
    output: ''

    # The directory where the test results are going to be placed.
    # Default: null
    results: ''

    # The target runtime to test for.
    # Default: null
    runtime: ''

    # Define the .runsettings file to use for running the tests.
    # Default: null
    settings: ''

    # List the discovered tests instead of running the tests
    # Default: false
    listTests: ''

    # The verbosity level of the action.
    # Default: null
    verbosity: ''
```

# Scenarios
- [Running the tests for this solution](#running-the-tests-for-this-solution)
- [Adding a filter](#adding-a-filter)
- [Running with multiple options](#running-with-multiple-options)

### Running the tests for this solution
```json
"test": {}
```

### Adding a filter
```json
"test": {
    "filter": "FullyQualifiedName~MyNamespace.MyClass.MyTestMethod"
}
```

### Running with multiple options
```json
"test": {
    "framework": "netcoreapp3.1",
    "filter": "FullyQualifiedName~MyNamespace.MyClass.MyTestMethod",
    "logger": "console;verbosity=detailed",
    "results": "./TestResults",
    "settings": "./runtimesettings.runtimesettings",
    "blame": true, 
    "blameCrashDumpType": "full",
    "blameCrashCollectAlways": true, 
    "blameHang": true,
    "blameHangDumpType": "full",
    "blameHangTimeout": "5min"
}
```