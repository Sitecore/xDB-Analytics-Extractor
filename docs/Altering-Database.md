# Altering the Database

#### Adding Scripts
> [!NOTE]
> Scripts that have previously been executed will not be re-executed.

1. Create a new `.sql` script under the `scripts` folder.
2. Mark your script as an *Embedded Resource*.
3. Rebuild the project and run the command

> [!IMPORTANT]
> The scripts are running in **alphabetical order** so make sure that you follow the `ScriptXXXX_<script name>`. If you want to run a new script before an old script, you will need to rename the scripts accordingly.

#### Updating
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