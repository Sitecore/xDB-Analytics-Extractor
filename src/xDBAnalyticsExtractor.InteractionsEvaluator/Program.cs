using xDBAnalyticsExtractor.InteractionsEvaluator;
using xDBAnalyticsExtractor.InteractionsEvaluator.XConnectConfiguration;
using Microsoft.Extensions.Configuration;
using Sitecore.XConnect.Client;
using System.Diagnostics;

var builder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: false);

IConfiguration config = builder.Build();
var cert = config.GetSection("XConnectClient").GetValue<string>("Certificate") ?? string.Empty;
var clientUrl = config.GetSection("XConnectClient").GetValue<string>("ClientUrl") ?? string.Empty;
var collectionClientUrl = config.GetSection("XConnectClient").GetValue<string>("CollectionClientUrl") ?? string.Empty;
var searchClientUrl = config.GetSection("XConnectClient").GetValue<string>("SearchClientUrl") ?? string.Empty;
var configurationClientUrl = config.GetSection("XConnectClient").GetValue<string>("ConfigurationClientUrl") ?? string.Empty;

var xConnectConfig = Configurator.Set(cert, collectionClientUrl, searchClientUrl, configurationClientUrl);
var configIsInitialized = await Initializer.InitializeAsync(xConnectConfig);
var count = 0;
List<int> batches = new();
if (configIsInitialized)
{
    Stopwatch sw = Stopwatch.StartNew();
    Console.WriteLine("Please wait for all the runs to finish.");
    Console.WriteLine("-----------------------------------------------");
    while (sw.Elapsed <= TimeSpan.FromMinutes(Constants.EVALUATOR_DURATION_IN_MINUTES)) // Modify this to adjust the duration, in minutes, for which this evaluator will run.
    {
        using var cancellationTokenSource = new CancellationTokenSource(TimeSpan.FromSeconds(Constants.BATCH_PROCESSING_SECONDS)); // Modify this to adjust the duration of the batch processing.
        await PrintBatchesPerSeconds(xConnectConfig, cancellationTokenSource);
        WaitForRandomNumberOfMilliseconds();
    }
    var maxBatches = batches.Max();
    var minBatches = batches.Min();
    var averageBatches = batches.Average();
    // Y (seconds for 1 batch) =  BATCH_PROCESSING_SECONDS / averageBatches 
    var secondsPerBatch = Constants.BATCH_PROCESSING_SECONDS / averageBatches;
    // T = Y / 200 ( seconds per interaction)
    var secondsPerInteraction = secondsPerBatch / 200;

    Console.ForegroundColor = ConsoleColor.Cyan;
    Console.WriteLine($"Minimum batches: {minBatches}, Maximum batches: {maxBatches}, Average batches after {count} runs: {averageBatches}");
    Console.ForegroundColor = ConsoleColor.Magenta;
    var averageInteractionsPerDay = await Retriever.GetNumberOfInteractionsForDaysSpecified(xConnectConfig) / Constants.NUMBER_OF_DAYS;
    // G = averageInteractionsPerDay / 86400 seconds
    var G = averageInteractionsPerDay / 86400;
    // G <= 1 / T
    if(G > (1/secondsPerInteraction))
    {
        // not viable scenario
        Console.ForegroundColor = ConsoleColor.Red;
        Console.WriteLine("The scheduled task won't be able to handle the interactions generated per day.");
        Console.ForegroundColor = ConsoleColor.White;
    }

    Console.WriteLine($"Average interactions per day: {averageInteractionsPerDay}");
    Console.ForegroundColor = ConsoleColor.White;
    Console.WriteLine("-----------------------------------------------");

}

async Task PrintBatchesPerSeconds(XConnectClientConfiguration xConnectConfig, CancellationTokenSource cancellationTokenSource)
{
    var numberOfBatches = await Retriever.GetNumberOfBatchesProcessedAsync(xConnectConfig, cancellationTokenSource.Token);
    batches.Add(numberOfBatches);
    count++;
    Console.ForegroundColor = ConsoleColor.Green;
    Console.WriteLine($"{count}{GetSuffix(count)} run - Batches processed in {Constants.BATCH_PROCESSING_SECONDS} seconds: {numberOfBatches}");
    Console.ForegroundColor = ConsoleColor.White;
}

static void WaitForRandomNumberOfMilliseconds()
{
    Random random = new Random();
    int milliseconds = random.Next(Constants.MIN_VALUE_MILLISECONDS, Constants.MAX_VALUE_MILLISECONDS); // Modify this to adjust the time intervals during which the app waits before initiating the next run.
    Thread.Sleep(milliseconds);
}

static string GetSuffix(int number)
{
    if (number >= 11 && number <= 13)
    {
        return "th";
}

    int lastDigit = number % 10;

    switch (lastDigit)
    {
        case 1:
            return "st";
        case 2:
            return "nd";
        case 3:
            return "rd";
        default:
            return "th";
    }
}