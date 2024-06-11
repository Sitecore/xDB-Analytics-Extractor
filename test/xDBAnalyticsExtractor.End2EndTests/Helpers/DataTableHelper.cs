using System.Data;
using Microsoft.Extensions.Configuration;
using Microsoft.VisualBasic.FileIO;
using xDBAnalyticsExtractor.End2EndTests.Tests.Configuration;
using ConfigurationManager = xDBAnalyticsExtractor.End2EndTests.Configuration.ConfigurationManager;

namespace xDBAnalyticsExtractor.End2EndTests.Helpers;

public class DataTableHelper
{
    private CsvFileHelper _csvFileHelper = new();
    private readonly IConfiguration _config = ConfigurationManager.InitConfiguration();

    public ExportedData InitializeExpectedExportedData(string section)
    {
        var configReader = new ConfigurationManager();
        return configReader.LoadExportedData(section);
    }

    public DataTable InitializeExpectedDataTable()
    {
        var dataTable = new DataTable();
        var exportedDataProperties = typeof(ExportedData).GetProperties();

        foreach (var property in exportedDataProperties)
        {
            var dataColumn = new DataColumn(property.Name, property.PropertyType);
            dataColumn.MaxLength = 2147483647;    
            dataColumn.ReadOnly = true;
            dataTable.Columns.Add(dataColumn);
        }
        
        return dataTable;
    }
    
    public DataTable LoadDataTableFromCsv(CsvFileHelper.CsvExport csvExport)
    {
        DataTable dataTable = new DataTable();
        DirectoryInfo directoryInfo = new DirectoryInfo(_config.GetValue<string>("CSVExportPath")!);

        // Find CSV files in the directory whose name contains "interaction"
        FileInfo[] csvFiles = directoryInfo.GetFiles("*.csv")
            .Where(file => file.Name.Contains(_csvFileHelper.SelectCsv(csvExport)))
            .ToArray();

        if (csvFiles.Length == 0)
        {
            Console.WriteLine("No CSV file with 'interaction' in the name found.");
            return dataTable;
        }

        // Assume we are working with the first matching CSV file
        FileInfo csvFile = csvFiles[0];

        using (TextFieldParser parser = new TextFieldParser(csvFile.FullName))
        {
            parser.TextFieldType = FieldType.Delimited;
            parser.SetDelimiters(",");

            // Read the CSV header and use it to create DataTable columns
            string[] headers = parser.ReadFields();
            foreach (string header in headers)
            {
                dataTable.Columns.Add(header);
            }

            // Read the remaining rows and add them to the DataTable
            while (!parser.EndOfData)
            {
                string[] fields = parser.ReadFields();
                dataTable.Rows.Add(fields);
            }
        }

        return dataTable;
    }

    public string[] RowFromExpectedExportedData(ExportedData expectedExportedData)
    {
        var row = new List<string>();
        var interactionProperties = typeof(ExportedData).GetProperties();
        
        foreach (var property in interactionProperties)
        {
            row.Add(property.GetValue(expectedExportedData)?.ToString());
        }
        
        return row.ToArray();
    }
    
    public DataRow FindRowByColumnValue(DataTable dataTable, string columnName, string columnValue)
    {
        foreach (DataRow row in dataTable.Rows)
        {
            if (row[columnName].ToString() == columnValue)
            {
                return row;
            }
        }

        return null; // Return null if a matching row is not found
    }
}