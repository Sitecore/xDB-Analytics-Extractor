DbUp is a .NET library that helps you to deploy changes to SQL Server databases. It tracks which SQL scripts have been run already, and runs the change scripts that are needed to get your database up to date.

This tool uses DbUp to manage and monitor this procedure.

## Getting Started
1. Open the `appsettings.json` file, and locate the `ConnectionStrings` key.
2. Update the connection string with your server's properties

> [!NOTE]
> The tool will automatically create a new database with the name specified by the connection string, if it doesn't already exist. 

## Adding Scripts
1. Create a new `.sql` script under the `scripts` folder.
2. Mark your script as an *Embedded Resource*.
3. Rebuild the project and run the command

> [!IMPORTANT]
> The scripts are running in **alphabetical order** so make sure that you follow the `ScriptXXXX_<script name>`. If you want to run a new script before an old script, you will need to rename the scripts accordingly.

> [!NOTE]
> Scripts that have previously been executed will not be re-executed.

## Updating
When you want to **alter** your database schema, you have **two options**:

##### 1. Create a script that will make the required changes
> [!IMPORTANT]
> This is the method that we recommend you perform your changes.

Create a new `.sql` script using the instructions above. 

##### 2. Alter the script that you want to make a change to
> [!CAUTION]
> This method is not recommended and you should use it only if there is no
> other option available. It might lead to errors and potential data loss.

DbUp uses a special table called `SchemaVersions` to keep track of the scripts that have been executed. This way, it avoids running the same scripts again if they have not been modified since the last run. The journal table stores the history of all script runs in the database.

If you want to alter a preexisting script, you need to modify the `SchemaVersions` table and remove the run for the modified script, as well as **all dependent scripts**. 

This is an error prone procedure, and we recommend against using it.