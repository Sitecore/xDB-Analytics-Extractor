# Register Service Configuration

This action will register the `xDB Analytics Extractor Scheduled Task` to run every day at 3:00PM.

> [!NOTE]
> You can change the scheduled task configuration, by using the Windows Task Scheduler

## Usage

```yaml
register: {
    # The path to the executable.
    # Default: null
    targetExecutable: ''
}
```
