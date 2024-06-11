namespace xDBAnalyticsExtractor;

public class CommandLineArgs
{
    public string[] Args { get; set; } = new string[10];
    public const string SQL_ARGUMENT = "-sql";
    public const string FILE_ARGUMENT = "-file";
    public const string CURRENT_DATA_ARGUMENT = "-current";
    public const string HISTORICAL_DATA_ARGUMENT = "-historical";

    public CommandLineArgs(string[] args)
    {
        Args = args;
    }
}
