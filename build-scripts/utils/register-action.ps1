param (
    [string] $targetExecutable
)

# Define the action the task will perform
$action = New-ScheduledTaskAction -Execute $targetExecutable

# Define the trigger for the task
$trigger = New-ScheduledTaskTrigger -Daily -At 3:00PM

# Create the scheduled task
Register-ScheduledTask -Action $action -Trigger $trigger -TaskName "xDB Analytics Extractor Scheduled Task"