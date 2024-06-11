USE xDBAnalyticsExtractor;
GO

IF NOT EXISTS (SELECT 1 FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_SCHEMA = 'dbo' AND TABLE_NAME = 'Interactions')
BEGIN
    CREATE TABLE Interactions (
        InteractionId UNIQUEIDENTIFIER PRIMARY KEY,
        StartDateTime DATETIME2,
        EndDateTime DATETIME2,
        LastModified DATETIME2,
        CampaignId UNIQUEIDENTIFIER,
        ChannelId UNIQUEIDENTIFIER NOT NULL,
        EngagementValue INT,
        Duration TIME,
        UserAgent NVARCHAR(MAX),
        Bounces INT,
        Conversions INT,
        Converted INT,
        TimeOnSite INT,
        PageViews INT,
        OutcomeOccurrences INT,
        MonetaryValue MONEY
    );
END

IF NOT EXISTS (SELECT 1 FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_SCHEMA = 'dbo' AND TABLE_NAME = 'EventDefinitions')
BEGIN
    CREATE TABLE EventDefinitions (
        Id UNIQUEIDENTIFIER PRIMARY KEY,
        Alias NVARCHAR(MAX),
        CreatedDate DATETIME2,
        LastModifiedDate DATETIME2,
        CreatedBy NVARCHAR(MAX),
        LastModifiedBy NVARCHAR(MAX),
        Culture NVARCHAR(10),
        Description NVARCHAR(MAX)
    );
END

IF NOT EXISTS (SELECT 1 FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_SCHEMA = 'dbo' AND TABLE_NAME = 'CampaignDefinitions')
BEGIN
    CREATE TABLE CampaignDefinitions (
        Id UNIQUEIDENTIFIER PRIMARY KEY,
        Alias NVARCHAR(MAX),
        CreatedDate DATETIME2,
        LastModifiedDate DATETIME2,
        CreatedBy NVARCHAR(MAX),
        LastModifiedBy NVARCHAR(MAX),
        Culture NVARCHAR(10),
        Description NVARCHAR(MAX)
    );
END

IF NOT EXISTS (SELECT 1 FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_SCHEMA = 'dbo' AND TABLE_NAME = 'GoalDefinitions')
BEGIN
    CREATE TABLE GoalDefinitions (
        Id UNIQUEIDENTIFIER PRIMARY KEY,
        Alias NVARCHAR(MAX),
        CreatedDate DATETIME2,
        LastModifiedDate DATETIME2,
        CreatedBy NVARCHAR(MAX),
        LastModifiedBy NVARCHAR(MAX),
        Culture NVARCHAR(10),
        Description NVARCHAR(MAX)
    );
END

IF NOT EXISTS (SELECT 1 FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_SCHEMA = 'dbo' AND TABLE_NAME = 'OutcomeDefinitions')
BEGIN
    CREATE TABLE OutcomeDefinitions (
        Id UNIQUEIDENTIFIER PRIMARY KEY,
        Alias NVARCHAR(MAX),
        CreatedDate DATETIME2,
        LastModifiedDate DATETIME2,
        CreatedBy NVARCHAR(MAX),
        LastModifiedBy NVARCHAR(MAX),
        Culture NVARCHAR(10),
        Description NVARCHAR(MAX)
    );
END

IF NOT EXISTS (SELECT 1 FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_SCHEMA = 'dbo' AND TABLE_NAME = 'Campaigns')
BEGIN
    CREATE TABLE Campaigns (
        Id UNIQUEIDENTIFIER PRIMARY KEY,
        InteractionId UNIQUEIDENTIFIER FOREIGN KEY REFERENCES Interactions(InteractionId),
        CustomValues NVARCHAR(MAX),
        Data NVARCHAR(MAX),
        DataKey NVARCHAR(MAX),
        DefinitionId UNIQUEIDENTIFIER,
        ItemId UNIQUEIDENTIFIER,
        EngagementValue INT,
        ParentEventId UNIQUEIDENTIFIER,
        Text NVARCHAR(MAX),
        Timestamp DATETIME2,
        Duration TIME,
        CampaignDefinitionId UNIQUEIDENTIFIER FOREIGN KEY REFERENCES CampaignDefinitions(Id)
    );
END

IF NOT EXISTS (SELECT 1 FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_SCHEMA = 'dbo' AND TABLE_NAME = 'Devices')
BEGIN
    CREATE TABLE Devices (
        Id BIGINT IDENTITY(1,1) PRIMARY KEY,
        BrowserVersion NVARCHAR(255),
        BrowserMajorName NVARCHAR(MAX),
        BrowserMinorName NVARCHAR(MAX),
        DeviceCategory NVARCHAR(MAX),
        ScreenSize NVARCHAR(MAX),
        OperatingSystem NVARCHAR(MAX),
        OperatingSystemVersion NVARCHAR(255),
        Language NVARCHAR(10),
        CanSupportTouchScreen BIT,
        DeviceVendor NVARCHAR(MAX),
        DeviceVendorHardwareModel NVARCHAR(MAX),
        InteractionId UNIQUEIDENTIFIER FOREIGN KEY REFERENCES Interactions(InteractionId)
    );
END

IF NOT EXISTS (SELECT 1 FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_SCHEMA = 'dbo' AND TABLE_NAME = 'Downloads')
BEGIN
    CREATE TABLE Downloads (
        Id UNIQUEIDENTIFIER PRIMARY KEY,
        InteractionId UNIQUEIDENTIFIER FOREIGN KEY REFERENCES Interactions(InteractionId),
        CustomValues NVARCHAR(MAX),
        Data NVARCHAR(MAX),
        DataKey NVARCHAR(MAX),
        DefinitionId UNIQUEIDENTIFIER,
        ItemId UNIQUEIDENTIFIER,
        EngagementValue INT,
        ParentEventId UNIQUEIDENTIFIER,
        Text NVARCHAR(MAX),
        Timestamp DATETIME2,
        Duration TIME,
        EventDefinitionId UNIQUEIDENTIFIER
    );
END

IF NOT EXISTS (SELECT 1 FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_SCHEMA = 'dbo' AND TABLE_NAME = 'GeoNetworks')
BEGIN
    CREATE TABLE GeoNetworks (
        Id BIGINT IDENTITY(1,1) PRIMARY KEY,
        Area NVARCHAR(MAX),
        Country NVARCHAR(MAX),
        Region NVARCHAR(MAX),
        Metro NVARCHAR(MAX),
        City NVARCHAR(MAX),
        Latitude FLOAT,
        Longitude FLOAT,
        LocationId UNIQUEIDENTIFIER,
        PostalCode NVARCHAR(20),
        IspName NVARCHAR(MAX),
        BusinessName NVARCHAR(MAX),
        InteractionId UNIQUEIDENTIFIER FOREIGN KEY REFERENCES Interactions(InteractionId)
    );
END

IF NOT EXISTS (SELECT 1 FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_SCHEMA = 'dbo' AND TABLE_NAME = 'Goals')
BEGIN
    CREATE TABLE Goals (
        Id UNIQUEIDENTIFIER PRIMARY KEY,
        InteractionId UNIQUEIDENTIFIER FOREIGN KEY REFERENCES Interactions(InteractionId),
        CustomValues NVARCHAR(MAX),
        Data NVARCHAR(MAX),
        DataKey NVARCHAR(MAX),
        DefinitionId UNIQUEIDENTIFIER,
        ItemId UNIQUEIDENTIFIER,
        EngagementValue INT,
        ParentEventId UNIQUEIDENTIFIER,
        Text NVARCHAR(MAX),
        Timestamp DATETIME2,
        Duration TIME
    );
END

IF NOT EXISTS (SELECT 1 FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_SCHEMA = 'dbo' AND TABLE_NAME = 'Outcomes')
BEGIN
    CREATE TABLE Outcomes (
        Id UNIQUEIDENTIFIER PRIMARY KEY,
        InteractionId UNIQUEIDENTIFIER FOREIGN KEY REFERENCES Interactions(InteractionId),
        CustomValues NVARCHAR(MAX),
        Data NVARCHAR(MAX),
        DataKey NVARCHAR(MAX),
        DefinitionId UNIQUEIDENTIFIER,
        ItemId UNIQUEIDENTIFIER,
        EngagementValue INT,
        ParentEventId UNIQUEIDENTIFIER,
        Text NVARCHAR(MAX),
        Timestamp DATETIME2,
        Duration TIME,
        CurrencyCode NVARCHAR(3),
        MonetaryValue MONEY
    );
END

IF NOT EXISTS (SELECT 1 FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_SCHEMA = 'dbo' AND TABLE_NAME = 'PageViews')
BEGIN
    CREATE TABLE PageViews (
        Id UNIQUEIDENTIFIER PRIMARY KEY,
        InteractionId UNIQUEIDENTIFIER FOREIGN KEY REFERENCES Interactions(InteractionId),
        CustomValues NVARCHAR(MAX),
        Data NVARCHAR(MAX),
        DataKey NVARCHAR(MAX),
        DefinitionId UNIQUEIDENTIFIER,
        ItemId UNIQUEIDENTIFIER,
        EngagementValue INT,
        ParentEventId UNIQUEIDENTIFIER,
        Text NVARCHAR(MAX),
        Timestamp DATETIME2,
        Duration TIME,
        ItemLanguage NVARCHAR(10),
        ItemVersion INT,
        Url NVARCHAR(MAX),
        EventDefinitionId UNIQUEIDENTIFIER
    );
END

IF NOT EXISTS (SELECT 1 FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_SCHEMA = 'dbo' AND TABLE_NAME = 'Searches')
BEGIN
    CREATE TABLE Searches (
        Id UNIQUEIDENTIFIER PRIMARY KEY,
        InteractionId UNIQUEIDENTIFIER FOREIGN KEY REFERENCES Interactions(InteractionId),
        CustomValues NVARCHAR(MAX),
        Data NVARCHAR(MAX),
        DataKey NVARCHAR(MAX),
        DefinitionId UNIQUEIDENTIFIER,
        ItemId UNIQUEIDENTIFIER,
        EngagementValue INT,
        ParentEventId UNIQUEIDENTIFIER,
        Text NVARCHAR(MAX),
        Timestamp DATETIME2,
        Duration TIME,
        Keywords NVARCHAR(MAX),
        EventDefinitionId UNIQUEIDENTIFIER
    );
END

IF NOT EXISTS (SELECT 1 FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_SCHEMA = 'dbo' AND TABLE_NAME = 'Channels')
BEGIN
    CREATE TABLE Channels (
        Id UNIQUEIDENTIFIER PRIMARY KEY,
        Code NVARCHAR(MAX) NULL,
        Description NVARCHAR(MAX) NULL,
        Name NVARCHAR(MAX) NULL,
        UriCulture NVARCHAR(MAX) NULL,
        UriPath NVARCHAR(MAX) NULL,
        Uri NVARCHAR(MAX) NULL
    );
END