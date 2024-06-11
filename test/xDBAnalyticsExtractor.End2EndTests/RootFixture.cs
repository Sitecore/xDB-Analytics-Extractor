using System.Xml;
using xDBAnalyticsExtractor.End2EndTests.Configuration;

namespace xDBAnalyticsExtractor.End2EndTests;

[SetUpFixture]
public class RootFixture
{
    private TestInitializer _initialize;
    private InstanceInfoSettings _instanceInfoSettings;

    [OneTimeSetUp]
    public void OneTimeSetup()
    {
        _initialize = new TestInitializer();
        _instanceInfoSettings = _initialize.InstanceInfoSettings;
        if (!IsInstanceConfigured())
        {
            throw new AssertionException(
                "IndexAnonymousContactData and/or IndexPIISensitiveData parameters are not set to TRUE on Instance under test");
        }
    }

    [OneTimeTearDown]
    public void OneTimeTearDown()
    {
    }

    private bool IsInstanceConfigured()
    {
        List<string> filePaths = new List<string>
        {
            $@"{_instanceInfoSettings.XConnectPath}\App_Data\Config\Sitecore\SearchIndexer\sc.Xdb.Collection.IndexerSettings.xml",
            $@"{_instanceInfoSettings.XConnectPath}\App_Data\jobs\continuous\IndexWorker\App_Data\Config\Sitecore\SearchIndexer\sc.Xdb.Collection.IndexerSettings.xml"
        };

        List<string> parameterNames = new List<string>
        {
            "IndexPIISensitiveData",
            "IndexAnonymousContactData"
        };

        foreach (string filePath in filePaths)
        {
            foreach (string parameterName in parameterNames)
            {
                string? parameterValue = GetParameterValue(filePath, parameterName);
                if (parameterValue is not "true")
                {
                    Console.WriteLine(
                        $"Parameter's {parameterName} value in {filePath} is {parameterValue}, but expected TRUE.");
                    return false;
                }
            }
        }

        Console.WriteLine("Instance configured successfully.");
        return true;
    }

    private string? GetParameterValue(string filePath, string parameterName)
    {
        try
        {
            var xmlDoc = new XmlDocument();
            xmlDoc.Load(filePath);

            XmlNode parameterTag = xmlDoc.SelectSingleNode($"//{parameterName}");

            if (parameterTag != null)
            {
                return parameterTag.InnerText;
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"An error occurred: {ex.Message}");
        }

        return null;
    }
}