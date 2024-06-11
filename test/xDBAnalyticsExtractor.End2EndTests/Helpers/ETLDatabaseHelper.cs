using System.Data;
using System.Data.SqlClient;
using System.Text.Json;

namespace xDBAnalyticsExtractor.End2EndTests.Helpers;

public class ETLDatabaseHelper
{
    public string[] GetTableNames(string connectionString)
    {
        try
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                DataTable tablesSchema = connection.GetSchema("Tables");
                string[] tableNames = new string[tablesSchema.Rows.Count];
                for (int i = 0; i < tablesSchema.Rows.Count; i++)
                {
                    tableNames[i] = tablesSchema.Rows[i]["TABLE_NAME"].ToString();
                }

                return tableNames;
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine("Error: " + ex.Message);
            return null;
        }
    }

    public string LoadJsonFromETLDatabase(string connectionString, string tableName,
        string[]? dynamicData = null)
    {
        var dataTable = LoadDataTableFromETLDatabase(connectionString, tableName);

        // Prepare Dictionary with column headers and data
        List<Dictionary<string, string>> data = new List<Dictionary<string, string>>();

        foreach (DataRow row in dataTable.Rows)
        {
            Dictionary<string, string> dictionary = new Dictionary<string, string>();

            foreach (DataColumn column in dataTable.Columns) 
            { 
                string columnName = column.ColumnName; 
                string columnValue = row[column].ToString();

                // Replace data with "*" if the header is in dynamicData array
                if (dynamicData?.Contains(columnName) == true)
                { 
                    columnValue = "*"; 
                }
                
                dictionary.Add(columnName, columnValue); 
            }
            
            data.Add(dictionary); 
        }

        return JsonSerializer.Serialize(data);
    }
    
    public DataTable LoadDataTableFromETLDatabase(string connectionString, string tableName)
    {
        try
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                // Fetch the first row of data for the specified table
                using (SqlCommand command = new SqlCommand($"SELECT * FROM {tableName}", connection))
                {
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        var dataTable = new DataTable();
                        dataTable.Load(reader);
                        return dataTable;
                    }
                }
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine("Error: " + ex.Message);
            return null;
        }
    }
}



    
