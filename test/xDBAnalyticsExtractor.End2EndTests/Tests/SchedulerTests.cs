namespace xDBAnalyticsExtractor.End2EndTests.Tests;

public class SchedulerTests : BaseFixture
{
    private Interaction _interactionResult;
    
    [SetUp]
    public void SetUp()
    {
        ClearETLDatabase();
        ClearXdbCollectionShards();
        CleanUpCsvFilesFolder();
        ClearEtlWorkerDb();
    }

    [TearDown]
    public void TearDown()
    {
    }    
    
    [Test]
    public async Task ETLWorkerDbWithNoEntriesAndMultipleInteractions_ExportCurrentData_NoInteractionsExtractedAndETLWorkerDbUpdatedWithMaxEndDate()
    {
        // Arrange
        //Submit multiple interactions
        var interactionList = new List<Interaction>();
        interactionList.Add(SubmitInteraction(-3));
        interactionList.Add(SubmitInteraction(-2));
        interactionList.Add(SubmitInteraction(-1));
        
        // Act
        await RunExtractorWorkerForCurrentData();
        var dataTableResult = _etlDatabaseHelper.LoadDataTableFromETLDatabase(_eTLDatabaseConnectionString, "Interactions");
        var maxInteractionEndDateTime = interactionList.OrderByDescending(i => i.EndDateTime).First().EndDateTime;
        var lastExported = _exportRepository.GetLatestExport().LastExported;

        // Assert
        dataTableResult.Rows.Count.Should().Be(0);
        lastExported.Should().BeCloseTo(maxInteractionEndDateTime, TimeSpan.FromMilliseconds(10));
    }
    
    [Test]
    public async Task ETLWorkerDbWithNoEntriesAndNoInteractions_ExportCurrentData_NoInteractionsExtractedAndETLWorkerDbUpdatedWithUtcNow()
    {
        // Arrange
        
        // Act
        await RunExtractorWorkerForCurrentData();
        var utcNow = DateTime.UtcNow;
        var lastExported = _exportRepository.GetLatestExport().LastExported;
        var dataTableResult = _etlDatabaseHelper.LoadDataTableFromETLDatabase(_eTLDatabaseConnectionString, "Interactions");
        
        // Assert
        dataTableResult.Rows.Count.Should().Be(0);
        lastExported.Should().BeCloseTo(utcNow, TimeSpan.FromSeconds(5));
    }
    
    [Test]
    public async Task MultipleInteractions_ExportCurrentData_OnlyNewInteractionsExtractedAndETLWorkerDbUpdated()
    {
        // Arrange
        //Insert record with LastExported set to -12h
        SetLastExportedDateTimeUtcNowMinusHours(12);
        //Interactions before lastExported date
        SubmitInteraction(-15);
        SubmitInteraction(-20);
        SubmitInteraction(-18);
        SubmitInteraction(-14);
        //Interactions after lastExported date
        var interactionList = new List<Interaction>();
        interactionList.Add(SubmitInteraction(-3));
        interactionList.Add(SubmitInteraction(-10));
        interactionList.Add(SubmitInteraction(-7));
        
        //Act
        await RunExtractorWorkerForCurrentData();
        var dataTableResult = _etlDatabaseHelper.LoadDataTableFromETLDatabase(_eTLDatabaseConnectionString, "Interactions");
        var maxInteractionEndDateTime = interactionList.OrderByDescending(i => i.EndDateTime).First().EndDateTime;
        var lastExported = _exportRepository.GetLatestExport().LastExported;

        // Assert
        dataTableResult.Rows.Count.Should().Be(interactionList.Count);
        lastExported.Should().BeCloseTo(maxInteractionEndDateTime, TimeSpan.FromMilliseconds(10));
    }
    
    [Test]
    public async Task NoNewInteractions_ExportCurrentData_NoInteractionsExtractedAndETLWorkerNotChanged()
    {
        // Arrange
        //Insert record with LastExported set to -12h
        SetLastExportedDateTimeUtcNowMinusHours(12);
        //Interaction before lastExported
        SubmitInteraction(-15);
        var lastExported = _exportRepository.GetLatestExport().LastExported;
        
        // Act
        await RunExtractorWorkerForCurrentData();
        var dataTableResult = _etlDatabaseHelper.LoadDataTableFromETLDatabase(_eTLDatabaseConnectionString, "Interactions");
        var lastExportedAfterNextRun = _exportRepository.GetLatestExport().LastExported;

        // Assert
        dataTableResult.Rows.Count.Should().Be(0);
        lastExported.Should().Be(lastExportedAfterNextRun);
    }

    [Test]
    public async Task ETLWorkerDbWithNoEntries_ExportHistoricalData_NoInteractionsExtractedAndETLWorkerDbUpdatedWithMaxEndDate()
    {
        // Arrange
        //Submit multiple interactions
        var interactionList = new List<Interaction>();
        interactionList.Add(SubmitInteraction(-3));
        interactionList.Add(SubmitInteraction(-2));
        interactionList.Add(SubmitInteraction(-1));
        
        // Act
        await RunExtractorWorkerForHistoricalData();
        var dataTableResult = _etlDatabaseHelper.LoadDataTableFromETLDatabase(_eTLDatabaseConnectionString, "Interactions");
        var maxInteractionEndDateTime = interactionList.OrderByDescending(i => i.EndDateTime).First().EndDateTime;
        var lastExported = _exportRepository.GetLatestExport().LastExported;

        // Assert
        dataTableResult.Rows.Count.Should().Be(0);
        lastExported.Should().BeCloseTo(maxInteractionEndDateTime, TimeSpan.FromMilliseconds(10));
    }
    
    [Test]
    public async Task ETLWorkerDbWithNoEntriesAndNoInteractions_ExportHistoricalData_NoInteractionsExtractedAndETLWorkerDbUpdatedWithUtcNow()
    {
        // Arrange
        
        // Act
        await RunExtractorWorkerForHistoricalData();
        var utcNow = DateTime.UtcNow;
        var lastExported = _exportRepository.GetLatestExport().LastExported;
        var dataTableResult = _etlDatabaseHelper.LoadDataTableFromETLDatabase(_eTLDatabaseConnectionString, "Interactions");
        
        // Assert
        dataTableResult.Rows.Count.Should().Be(0);
        lastExported.Should().BeCloseTo(utcNow, TimeSpan.FromSeconds(5));
    }
    
    [Test]
    public async Task MultipleInteractions_ExportHistoricalData_OnlyHistoricalInteractionsWithinRangeExtractedAndETLWorkerDbUpdatedWithMinEndDate()
    {
        // Arrange
        //set lastExported to UtcNow-12h
        SetLastExportedDateTimeUtcNowMinusHours(12);
        
        //Submit multiple interactions before historical range (before UtcNow-1d) <- Will Not Be Exported
        SubmitInteraction(-35);
        SubmitInteraction(-27);
        SubmitInteraction(-40);
        SubmitInteraction(-60);
        SubmitInteraction(-72);
        
        //Submit multiple interactions before lastExported (-12h) but after historical range (before UtcNow-1d) <- Will Be Exported
        var interactionList = new List<Interaction>();
        interactionList.Add(SubmitInteraction(-20));
        interactionList.Add(SubmitInteraction(-15));
        interactionList.Add(SubmitInteraction(-18));
        interactionList.Add(SubmitInteraction(-17));
        
        //Submit multiple interactions after lastExported (-12h) <- Will Not Be Exported
        SubmitInteraction(-9);
        SubmitInteraction(-10);
        SubmitInteraction(-5);
        
        // Act
        await RunExtractorWorkerForHistoricalData();
        var dataTableResult = _etlDatabaseHelper.LoadDataTableFromETLDatabase(_eTLDatabaseConnectionString, "Interactions");
        var minInteractionEndDateTime = interactionList.OrderByDescending(i => i.EndDateTime).Last().EndDateTime;
        var lastExported = _exportRepository.GetEarliestExport().LastExported;

        // Assert
        dataTableResult.Rows.Count.Should().Be(interactionList.Count);
        lastExported.Should().BeCloseTo(minInteractionEndDateTime, TimeSpan.FromMilliseconds(10));
    }
    
    [Test]
    public async Task InteractionsAfterEarliestExportDate_ExportHistoricalData_NoInteractionsExtractedAndETLWorkerNotChanged()
    {
        // Arrange
        //set lastExported to UtcNow-12h
        SetLastExportedDateTimeUtcNowMinusHours(12);
        //Interaction after Earliest Export date
        SubmitInteraction(-1);
        var lastExported = _exportRepository.GetEarliestExport().LastExported;

        // Act
        await RunExtractorWorkerForHistoricalData();
        var dataTableResult = _etlDatabaseHelper.LoadDataTableFromETLDatabase(_eTLDatabaseConnectionString, "Interactions");
        var lastExportedAfterNextRun = _exportRepository.GetLatestExport().LastExported;

        // Assert
        dataTableResult.Rows.Count.Should().Be(0);
        lastExported.Should().Be(lastExportedAfterNextRun);
    }
}