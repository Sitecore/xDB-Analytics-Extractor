# Interactions Evaluator

With the **Interactions Evaluator** tool we want to help the user decide how often the trigger that starts the export should run. The essential data that we need for this decision are firstly an average number of interactions per day that are entered in Sitecore, and secondly how many interactions per minute can xDB Analytics Extractor export from Sitecore with XConnect client. Furthermore, by using those two metrics we can identify if xDB Analytics Extractor can export all data that are produced per day.

## Setup
There are two parts in the configuration of xDB Analytics Extractor.InteractionsEvaluator tool, the first is the [appsettings.json](../src/xDB Analytics Extractor.InteractionsEvaluator/appsettings.json) and the second is the [Constants.cs](../src/xDB Analytics Extractor.InteractionsEvaluator/Constants.cs). Information about the required configuration can be found in the [Installation](../README.md#installation) section of the Readme and in [Metrics](#metrics) section below respectively.


## Metrics

### GetNumberOfBatchesProcessedAsync

This method finds the number of batches of 200 interactions that we process in the duration of Constants.BATCH_PROCESSING_SECONDS for each run and we calculate the minimum, maximum and average number of batches processed and we show them to the user.




### GetNumberOfInteractionsForDaysSpecified

- Constants.NUMBER_OF_DAYS :  This is used to determine until how many days ago we will retrieve interactions.
We ask from the xconnect client to retrieve interactions that their EndDateTime is greater or equals to the current date minus the Constants.NUMBER_OF_DAYS.
Then we divide the interactions with Constants.NUMBER_OF_DAYS to get average number of interactions per day.
We divide average interactions per day with 86400 which are the seconds in a day to find interactions per seconds




<!--
------------------------------------------------------------


- Constants.EVALUATOR_DURATION_IN_MINUTES : How many minutes the evaluator will run for.
- Constants.BATCH_PROCESSING_SECONDS : How many seconds the batch processing will be running for.
 
GetNumberOfBatchesProcessedAsync
This method finds the number of batches of 200 interactions that we process in the duration of Constants.BATCH_PROCESSING_SECONDS for each run and we calculate the 
minimum, maximum and average number of batches processed and we show them to the user.
 
 
GetNumberOfInteractionsForDaysSpecified
- Constants.NUMBER_OF_DAYS :  This is used to determine until how many days ago we will retrieve interactions.
We ask from the xconnect client to retrieve interactions that their EndDateTime is greater or equals to the current date minus the Constants.NUMBER_OF_DAYS.
Then we divide the interactions with Constants.NUMBER_OF_DAYS to get average number of interactions per day.
We divide average interactions per day with 86400 which are the seconds in a day to find interactions per seconds
 
We calculate secondsPerInteraction = secondsPerBatch(Constants.BATCH_PROCESSING_SECONDS / averageBatches) / 200.
 
If interactions per second > 1 / secondsPerInteraction then the scheduled task wont be able to handle the interactions generated per day.
 
We added a random number of seconds to wait for each run.
- Constants.MIN_VALUE_MILLISECONDS : The minimum seconds to wait
- Constants.MAX_VALUE_MILLISECONDS : The maximum seconds to wait 

------------------------------------------------------------
-->