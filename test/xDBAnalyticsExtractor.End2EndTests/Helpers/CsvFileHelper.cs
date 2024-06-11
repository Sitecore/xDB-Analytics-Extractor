using System.Globalization;
using System.Text.Json;
using CsvHelper;
using CsvHelper.Configuration;
using System.Dynamic;

namespace xDBAnalyticsExtractor.End2EndTests.Helpers;

public class CsvFileHelper
{
    private readonly string _searches = "searches";
    private readonly string _pageViews = "pageViews";
    private readonly string _outcomes = "outcomes";
    private readonly string _outcomeDefinitions = "outcomeDefinitions";
    private readonly string _interaction = "interactions";
    private readonly string _goals = "goals";
    private readonly string _goalDefinitions = "goalDefinitions";
    private readonly string _geoNetworks = "geoNetworks";
    private readonly string _eventDefinitions = "eventDefinitions";
    private readonly string _downloads = "downloads";
    private readonly string _devices = "devices";
    private readonly string _campaigns = "campaigns";
    
    public enum CsvExport
    {
        Searches,
        PageViews,
        Outcomes,
        OutcomeDefinitions,
        Interactions,
        Goals,
        GoalDefinitions,
        GeoNetworks,
        EventDefinitions,
        Downloads,
        Devices,
        Campaigns
    }
    
    public string SelectCsv(CsvExport csvdefinition)
    {
        switch (csvdefinition)
        {
            case CsvExport.Searches:
                return _searches;
            case CsvExport.PageViews:
                return _pageViews;
            case CsvExport.Outcomes:
                return _outcomes;
            case CsvExport.OutcomeDefinitions:
                return _outcomeDefinitions;
            case CsvExport.Interactions:
                return _interaction;
            case CsvExport.Goals:
                return _goals;
            case CsvExport.GoalDefinitions:
                return _goalDefinitions;
            case CsvExport.GeoNetworks:
                return _geoNetworks;
            case CsvExport.EventDefinitions:
                return _eventDefinitions;
            case CsvExport.Downloads:
                return _downloads;
            case CsvExport.Devices:
                return _devices;
            case CsvExport.Campaigns:
                return _campaigns;
            default:
                throw new ArgumentException("Invalid csv selection");
        }
    }
    
    public string[] GetExportedCsvFiles(string folderPath)
    {
        return Directory.GetFiles(folderPath, "*.csv");
    }

    public string[] GetEntityNamesFromCsvFiles(string[] csvFiles)
    {
        // Use LINQ to extract the entity names
        var entityNames = csvFiles.Select(fileName =>
        {
            string fileNameWithoutExtension = Path.GetFileNameWithoutExtension(fileName);
            string[] parts = fileNameWithoutExtension.Split('-');
            return parts[0];
        }).ToArray();

        return entityNames;
    }

    public string ReadFromCsvFile(string[] csvFiles, string entityName, string[]? dynamicData = null)
    {
        var result = new List<Dictionary<string, string>>();

        foreach (var filePath in csvFiles)
        {
            var fileName = Path.GetFileName(filePath);

            // Check if the fileName starts with entityName
            if (fileName.StartsWith(entityName))
            {
                using (var reader = new StreamReader(filePath))
                using (var csv = new CsvReader(reader, new CsvConfiguration(CultureInfo.InvariantCulture)))
                {
                    var records = csv.GetRecords<dynamic>().ToList();

                    foreach (var record in records)
                    {
                        var dictionary = new Dictionary<string, string>();
                        
                        var expandoObject = (ExpandoObject)record;

                        foreach (var keyValuePair in expandoObject)
                        {
                            var columnName = keyValuePair.Key;
                            var columnValue = keyValuePair.Value?.ToString();

                            if (dynamicData != null && dynamicData.Any(item => item == columnName))
                            {
                                // If the column is in dynamicData array, replace the value with "*"
                                dictionary[columnName] = "*";
                                Console.WriteLine($"{columnName} in {entityName} was marked as dynamic data, and its value is replaced with asterisks");
                            }
                            else
                            {
                                dictionary[columnName] = columnValue;
                            }
                        }

                        result.Add(dictionary);
                    }
                }
            }
        }

        // Serialize the list of dictionaries to JSON
        return JsonSerializer.Serialize(result);
    }
}