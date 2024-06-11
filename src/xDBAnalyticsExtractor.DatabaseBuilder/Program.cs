using System.Diagnostics;
using DbUp;
using System.Reflection;
using Microsoft.Extensions.Configuration;
using Spectre.Console;

namespace xDBAnalyticsExtractor.DatabaseBuilder;

public class Program
{
    public static void Main()
    {
        var config = GetConfiguration();
        var connectionString = config.GetConnectionString("SQLServer");

        EnsureDatabase.For.SqlDatabase(connectionString);

        var upgrader =
        DeployChanges.To
            .SqlDatabase(connectionString)
            .WithScriptsEmbeddedInAssembly(Assembly.GetExecutingAssembly())
            .LogToConsole()
            .Build();

        var result = upgrader.PerformUpgrade();

        if (!result.Successful)
        {
            AnsiConsole.WriteException(result.Error);
            WaitIfDebug();
        }

        AnsiConsole.Write(new Markup("\n[bold green]Success![/]\n"));
        WaitIfDebug();
    }

    private static IConfiguration GetConfiguration()
    {
        IConfiguration configuration = new ConfigurationBuilder()
            .AddJsonFile("appsettings.json", true, true)
            .Build();

        return configuration;
    }

    [Conditional("DEBUG")]
    public static void WaitIfDebug()
    {
        Console.ReadLine();
    }
}