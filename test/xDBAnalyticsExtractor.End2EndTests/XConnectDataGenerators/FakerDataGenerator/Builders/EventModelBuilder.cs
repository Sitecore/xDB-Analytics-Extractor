using Bogus;
using Sitecore.XConnect.Collection.Model;

namespace xDBAnalyticsExtractor.End2EndTests.XConnectDataGenerators.FakerDataGenerator.Builders;

/// <summary>
/// A builder class for generating and configuring various types of events for a Sitecore XConnect interaction.
/// When initialized always beggin with .AddEvents().WithPageViewEvent()
/// </summary>
public class EventModelBuilder
{
    private readonly Interaction _interaction;
    private readonly XConnectClient _client;
    private readonly Contact _contact;
    private readonly Faker _faker;
    private PageViewEvent _pageViewEvent = null!;

    //Profiles
    private readonly Guid _profileFocus = Guid.Parse("{24DFF2CF-B30A-4B75-8967-2FE3DED82271}");
    private readonly Guid _profileFunction = Guid.Parse("{BA06B827-C6F2-4748-BD75-AA178B770E83}");
    private readonly Guid _profilePersona = Guid.Parse("{B5BDEE45-C945-476F-9EE6-3B8A9255C17E}");
    private readonly Guid _profileScore = Guid.Parse("{9311296F-1A3D-4E58-AED1-0A6B4B1E654B}");

    //Focus Profile Keys
    private readonly Guid _focusKeyBackground = Guid.Parse("{03379AF5-F1AE-4610-B15B-4C7F1032B464}");
    private readonly Guid _focusKeyPractical = Guid.Parse("{5FDD9829-E689-454D-9ABC-8F95AE68744C}");
    private readonly Guid _focusKeyProcess = Guid.Parse("{F5652C06-676B-4E12-A9D0-06D000E5F1C8}");
    private readonly Guid _focusKeyScope = Guid.Parse("{B32BFACC-3494-4127-B050-CF50078E2B4C}");

    //Function Profile Keys
    private readonly Guid _functionKeyBuildingTrust = Guid.Parse("{233A2D01-6CCD-4D4E-BBEE-9196D492736C}");
    private readonly Guid _functionKeyCallToAction = Guid.Parse("{9581F4A4-B83D-4F37-9814-59226398566F}");
    private readonly Guid _functionKeyCreateDesire = Guid.Parse("{30AAF69F-C10A-4057-B918-6F8A8D0CEFDD}");
    private readonly Guid _functionKeyDefineConcept = Guid.Parse("{3C82B054-218F-40B7-9201-C2A126AD2C07}");

    //Persona Profile Keys
    private readonly Guid _personaKeyCecile = Guid.Parse("{B8810A60-3837-4AD1-B0DE-CF14AC5F4792}");
    private readonly Guid _personaKeyChris = Guid.Parse("{50278469-25F6-456B-BF9A-92C22FD38086}");
    private readonly Guid _personaKeyIan = Guid.Parse("{F2D7FF63-50A2-4595-96E6-2912864FBC90}");
    private readonly Guid _personaKeySandra = Guid.Parse("{9790D1F9-1F78-4B39-933D-46B08230F05F}");

    //Score Profile Keys
    private readonly Guid _scoreKeyLead = Guid.Parse("{178107BF-4294-4FA4-A0DA-30BDE00A1346}");
    private readonly bool _isContactIdentified;

    public EventModelBuilder(Interaction interaction, XConnectClient client, Contact contact, bool isIsContactIdentified)
    {
        _isContactIdentified = isIsContactIdentified;
        _interaction = interaction;
        _client = client;
        _contact = contact;
        _faker = new Faker();
    }

    [Obsolete("Obsolete, used for older versions of sitecore")]
    public EventModelBuilder WithProfileScores(Guid? matchedPatternId = null, Guid? profileDefinitionId = null,
        double? score = null, int scoreCount = 3)
    {
        if (!_isContactIdentified)
        {
            // When optional parameters are null generate data from faker
            score ??= _faker.Random.Double(0, 100);
            matchedPatternId ??= _faker.Random.Guid();
            profileDefinitionId ??=
                _faker.PickRandom(new[] { _profileFocus, _profileFunction, _profilePersona, _profileScore });
            var values = new Dictionary<Guid, double>();

            CalculateProfileScores(profileDefinitionId.Value, scoreCount, values);
            var profileScore = new ProfileScore()
            {
                // MatchedPatternId can be nullable
                MatchedPatternId = matchedPatternId.Value,
                ProfileDefinitionId = profileDefinitionId.Value,
                Score = score.Value,
                ScoreCount = scoreCount,
                Values = values
            };

            var profileScoresFacet = new ProfileScores();
            profileScoresFacet.Scores.Add(profileDefinitionId.Value, profileScore);

            _client.SetFacet(_interaction, ProfileScores.DefaultFacetKey, profileScoresFacet);
            return this;
        }
        // If contact is identified, do nothing
        return this;
    }

    public EventModelBuilder WithChangeProfileScoreEvent(Guid? matchedPatternId = null, DateTime? timestamp = null,
        Guid? profileDefinitionId = null,
        double? score = null, int? scoreCount = null, bool changeProfileScoreEvent = true)
    {
        if (!_isContactIdentified)
        {
            // When optional parameters are null generate data from faker
            matchedPatternId ??= _faker.Random.Guid();
            timestamp ??= _faker.Date.Recent(1, DateTime.UtcNow).AddHours(-1);
            profileDefinitionId ??=
                _faker.PickRandom(new[] { _profileFocus, _profileFunction, _profilePersona, _profileScore });
            score ??= _faker.Random.Double(0, 100);
            scoreCount ??= _faker.Random.Int(1, 100);

            //initialize Change Profile Score Event and Profile Score Delta
            var changeProfileScoresEvent = new ChangeProfileScoresEvent(timestamp.Value);
            var delta = new ProfileScoreDelta
            {
                ScoreCount = scoreCount.Value
            };

            //initialize values
            var values = new Dictionary<Guid, double>();

            //Calculate profile score to be inserted into Profile Score
            CalculateProfileScores(profileDefinitionId.Value, scoreCount.Value, values);

            //Initialize Profile Score
            var profileScore = new ProfileScore()
            {
                // MatchedPatternId can be nullable
                MatchedPatternId = matchedPatternId.Value,
                ProfileDefinitionId = profileDefinitionId.Value,
                Score = score.Value,
                ScoreCount = scoreCount.Value,
                Values = values
            };

            //Calculate delta to be added to Change Profile Score Event
            CalculateChangeProfileScoresEventDelta(profileDefinitionId.Value, scoreCount.Value, delta);

            //Add delta to Change Profile Scores Event
            changeProfileScoresEvent.Delta.Add(profileDefinitionId.Value, delta);

            //Add Change Profile Score Event to the interaction
            _interaction.Events.Add(changeProfileScoresEvent);

            //Only set this to true for the first time of interaction of the contact
            if (changeProfileScoreEvent)
            {
                var cbp = new ContactBehaviorProfile
                {
                    SourceInteractionStartDateTime = timestamp.Value
                };
                cbp.Scores.Add(profileScore.ProfileDefinitionId, profileScore);
                _client.SetFacet(_contact, ContactBehaviorProfile.DefaultFacetKey, cbp);
            }

            return this;
        }
        // If contact is identified, do nothing
        return this;
    }

    private void CalculateProfileScores(Guid profileDefinitionId, int scoreCount, IDictionary<Guid, double> values)
    {
        //Calculate profile score and insert it to the ProfileScore element, Profile Score will be used later
        //for inserting into CBP for first time and Calculate CBP values
        switch (profileDefinitionId)
        {
            case var r when (r == _profileFocus):
                values.Add(_focusKeyBackground, 5 * scoreCount); //Background
                values.Add(_focusKeyPractical, 2 * scoreCount); //Practical
                values.Add(_focusKeyProcess, 4 * scoreCount); //Process
                values.Add(_focusKeyScope, 1 * scoreCount); //Scope
                break;
            case var r when (r == _profileFunction):
                values.Add(_functionKeyBuildingTrust, 2 * scoreCount); //Building Trust
                values.Add(_functionKeyCallToAction, 0); //Call to Action
                values.Add(_functionKeyCreateDesire, 2 * scoreCount); //CreateDesire
                values.Add(_functionKeyDefineConcept, 0); //Define Concept
                break;
            case var r when (r == _profilePersona):
                values.Add(_personaKeyCecile, 5 * scoreCount); //Cecile
                values.Add(_personaKeyChris, 0); //Chriss
                values.Add(_personaKeyIan, 5 * scoreCount); //Ian
                values.Add(_personaKeySandra, 0); //Sandra
                break;
            case var r when (r == _profileScore):
                values.Add(_scoreKeyLead, 10 * scoreCount); //Lead
                break;
        }
    }

    private void CalculateChangeProfileScoresEventDelta(Guid profileDefinitionId, int scoreCount,
        ProfileScoreDelta delta)
    {
        //Calculate profile score and insert it to the ProfileScore element, Profile Score will be used later
        //for inserting into CBP for first time and Calculate CBP values

        switch (profileDefinitionId)
        {
            case var r when (r == _profileFocus):
                delta.Values.Add(_focusKeyBackground, 5 * scoreCount); //Background
                delta.Values.Add(_focusKeyPractical, 2 * scoreCount); //Practical
                delta.Values.Add(_focusKeyProcess, 4 * scoreCount); //Process
                delta.Values.Add(_focusKeyScope, 1 * scoreCount); //Scope
                break;
            case var r when (r == _profileFunction):
                delta.Values.Add(_functionKeyBuildingTrust, 2 * scoreCount); //Building Trust
                delta.Values.Add(_functionKeyCallToAction, 0); //Call to Action
                delta.Values.Add(_functionKeyCreateDesire, 2 * scoreCount); //CreateDesire
                delta.Values.Add(_functionKeyDefineConcept, 0); //Define Concept
                break;
            case var r when (r == _profilePersona):
                delta.Values.Add(_personaKeyCecile, 5 * scoreCount); //Cecile
                delta.Values.Add(_personaKeyChris, 0); //Chriss
                delta.Values.Add(_personaKeyIan, 5 * scoreCount); //Ian
                delta.Values.Add(_personaKeySandra, 0); //Sandra
                break;
            case var r when (r == _profileScore):
                delta.Values.Add(_scoreKeyLead, 10 * scoreCount); //Lead
                break;
        }
    }

    public EventModelBuilder WithPageViewEvent(Guid? sitecoreDeviceDataId = null, string? itemId = null,
        string? sitecoreDeviceName = null,
        int? itemVersion = null,
        string? itemLanguage = null, string? data = null, string? dataKey = null, int? engagementValue = null,
        string? text = null, int? duration = null, string? url = null)
    {
        // When optional parameters are null generate data from faker
        itemId ??= Guid.NewGuid().ToString();
        sitecoreDeviceName ??= _faker.Lorem.Word();
        itemVersion ??= _faker.Random.Number(1, 10000);
        itemLanguage ??= _faker.PickRandom(new[] { "en", "da", "el", "de", "zh" });
        data ??= _faker.Lorem.Word();
        dataKey ??= _faker.Lorem.Word();
        engagementValue ??= _faker.Random.Int(0, 10);
        text ??= _faker.Lorem.Sentence();
        duration ??= _faker.Random.Int(0, 1000);
        url ??= _faker.Internet.Url();
        if (!sitecoreDeviceDataId.HasValue)
        {
            sitecoreDeviceDataId = Guid.NewGuid();
        }

        var sitecoreDeviceData =
            new SitecoreDeviceData(sitecoreDeviceDataId.Value,
                sitecoreDeviceName); // Guid of the item that describes the device and the device name

        var pageViewEvent = new PageViewEvent(DateTime.UtcNow.AddHours(-1), Guid.Parse(itemId), itemVersion.Value, itemLanguage)
        {
            Data = data,
            DataKey = dataKey,
            EngagementValue = engagementValue.Value,
            Text = text,
            Duration = TimeSpan.FromSeconds(duration.Value),
            Url = url,
            SitecoreRenderingDevice = sitecoreDeviceData,
            CustomValues =
            {
                { "Id", Guid.NewGuid().ToString() },
                { "Data", _faker.Lorem.Word() },
                { "Date", _faker.Date.Recent(1, DateTime.UtcNow).AddHours(-1).ToString("f") }
            }
        };
        _interaction.Events.Add(pageViewEvent);
        _pageViewEvent = pageViewEvent;
        return this;
    }

    public EventModelBuilder WithSearchEvent(Guid? searchItemDefinition = null, DateTime? timestamp = null,
        string? data = null,
        string? dataKey = null, int? engagementValue = null, string? text = null, int? duration = null,
        string? keywords = null)
    {
        // When optional parameters are null generate data from faker
        data ??= _faker.Lorem.Word();
        dataKey ??= _faker.Lorem.Word();
        text ??= _faker.Lorem.Sentence();
        keywords ??= _faker.Lorem.Word();
        engagementValue ??= _faker.Random.Int(0, 10);
        duration ??= _faker.Random.Int(0, 1000);
        if (!searchItemDefinition.HasValue)
        {
            searchItemDefinition = Guid.NewGuid();
        }

        if (!timestamp.HasValue)
        {
            timestamp = _faker.Date.Recent(1, DateTime.UtcNow).AddHours(-1);  //Between(DateTime.Now.Add(TimeSpan.FromHours(-3)), DateTime.Now);
        }

        var searchEvent = new SearchEvent(timestamp.Value)
        {
            Data = data,
            DataKey = dataKey,
            ItemId = searchItemDefinition.Value,
            EngagementValue = engagementValue.Value,
            ParentEventId = _pageViewEvent.Id,
            Text = text,
            Duration = TimeSpan.FromSeconds(duration.Value),
            Keywords = keywords,
            CustomValues =
            {
                { "Id", Guid.NewGuid().ToString() },
                { "Data", _faker.Lorem.Word() },
                { "Date", _faker.Date.Recent(1, DateTime.UtcNow).AddHours(-1).ToString("f") }
            }
        };
        _interaction.Events.Add(searchEvent);
        return this;
    }

    public EventModelBuilder WithDownloadEvent(DateTime? timestamp = null, string? data = null, string? dataKey = null,
        int? engagementValue = null, string? text = null, int? duration = null)
    {
        // When optional parameters are null generate data from faker
        timestamp ??= _faker.Date.Recent(1, DateTime.UtcNow).AddHours(-1);
        data ??= _faker.Lorem.Word();
        dataKey ??= _faker.Lorem.Word();
        text ??= _faker.Lorem.Sentence();
        engagementValue ??= _faker.Random.Int(0, 10);
        duration ??= _faker.Random.Int(0, 1000);

        var downloadItemDefinition = Guid.NewGuid();
        var downloadEvent = new DownloadEvent(timestamp.Value, downloadItemDefinition)
        {
            Data = data,
            DataKey = dataKey,
            ItemId = downloadItemDefinition,
            EngagementValue = engagementValue.Value,
            ParentEventId = _pageViewEvent.Id,
            Text = text,
            Duration = TimeSpan.FromSeconds(duration.Value),
            CustomValues =
            {
                { "Id", Guid.NewGuid().ToString() },
                { "Data", _faker.Lorem.Word() },
                { "Date", _faker.Date.Recent(1, DateTime.UtcNow).AddHours(-1).ToString("f") }
            }
        };
        _interaction.Events.Add(downloadEvent);
        return this;
    }

    public EventModelBuilder WithCampaignEvent(DateTime? timestamp = null, string? data = null, string? dataKey = null,
        int? engagementValue = null, string? text = null, int? duration = null)
    {
        // When optional parameters are null generate data from faker
        timestamp ??= _faker.Date.Recent(1, DateTime.UtcNow).AddHours(-1);
        data ??= _faker.Lorem.Word();
        dataKey ??= _faker.Lorem.Word();
        text ??= _faker.Lorem.Sentence();
        engagementValue ??= _faker.Random.Int(0, 10);
        duration ??= _faker.Random.Int(0, 1000);
        
        //Campaigns available after Installing "Extractor Package.zip"
        //Install the package in Sitecore instance under test
        //and Deploying Marketing Definitions
        var campaignItemDefinition = Guid.Parse(_faker.PickRandom("{F3EC0049-1D8A-4582-8DEE-BB1BF05DE843}",
            "{34509012-D6CC-4668-9E06-50BD36A844F6}", "{EA0D8753-9B64-4A50-98E2-00E8143E0B8F}"));
        
        var campaignEvent = new CampaignEvent(campaignItemDefinition, timestamp.Value)
        {
            Data = data,
            DataKey = dataKey,
            ItemId = campaignItemDefinition,
            EngagementValue = engagementValue.Value,
            ParentEventId = _pageViewEvent.Id,
            Text = text,
            Duration = TimeSpan.FromSeconds(duration.Value),
            CustomValues =
            {
                { "Id", Guid.NewGuid().ToString() },
                { "Data", _faker.Lorem.Word() },
                { "Date", _faker.Date.Recent(1, DateTime.UtcNow).AddHours(-1).ToString("f") }
            }
        };
        _interaction.Events.Add(campaignEvent);
        return this;
    }

    public EventModelBuilder WithOutcomeEvent(string? data = null, string? dataKey = null,
        int? engagementValue = null, string? text = null, int? duration = null)
    {
        // When optional parameters are null generate data from faker
        data ??= _faker.Lorem.Word();
        dataKey ??= _faker.Lorem.Word();
        text ??= _faker.Lorem.Sentence();
        engagementValue ??= _faker.Random.Int(0, 10);
        duration ??= _faker.Random.Int(0, 1000);
        var outcomeItemDefinition = Guid.Parse(_faker.PickRandom("{52054874-4767-47DC-8099-8C08BFA307AA}",
            "{C2D9DFBC-E465-45FD-BA21-0A06EBE942D6}", "{BF6B8EE3-9FFB-4C58-9CB4-301C1C710F89}",
            "{5646D20E-B10A-42BA-876B-2A3BB3CBC641}", "{B4D9C749-65E7-457D-B61D-4150B9E51424}",
            "{F4830B80-1BB1-4746-89C7-96EFE40DA572}", "{75D53206-47B3-4391-BD48-75C42E5FC2CE}",
            "{9016E456-95CB-42E9-AD58-997D6D77AE83}"));

        var outcome = new Outcome(outcomeItemDefinition, DateTime.UtcNow.AddHours(-1), "EUR", 100.00m)
        {
            Data = data,
            DataKey = dataKey,
            ItemId = outcomeItemDefinition,
            EngagementValue = engagementValue.Value,
            ParentEventId = _pageViewEvent.Id,
            Text = text,
            Duration = TimeSpan.FromSeconds(duration.Value),
            CustomValues =
            {
                { "Id", Guid.NewGuid().ToString() },
                { "Data", _faker.Lorem.Word() },
                { "Date", _faker.Date.Recent(1, DateTime.UtcNow).AddHours(-1).ToString("f") }
            }
        };
        _interaction.Events.Add(outcome);
        return this;
    }

    public EventModelBuilder WithGoalEvent(DateTime? timestamp = null, string? data = null, string? dataKey = null,
        int? engagementValue = null, string? text = null, int? duration = null)
    {
        timestamp ??= _faker.Date.Recent(1, DateTime.UtcNow).AddHours(-1);
        data ??= _faker.Lorem.Word();
        dataKey ??= _faker.Lorem.Word();
        text ??= _faker.Lorem.Sentence();
        engagementValue ??= _faker.Random.Int(0, 10);
        duration ??= _faker.Random.Int(0, 1000);

        var goalItemDefinition = Guid.Parse(_faker.PickRandom("{968897F1-328A-489D-88E8-BE78F4370958}",
            "{87431B9B-FA39-4780-BEB3-1047B9E61876}", "{28A7C944-B8B6-45AD-A635-6F72E8F81F69}",
            "{66722F52-2D13-4DCC-90FC-EA7117CF2298}", "{1779CC42-EF7A-4C58-BF19-FA85D30755C9}",
            "{8FFB183B-DA1A-4C74-8F3A-9729E9FCFF6A}"));
        var goal = new Goal(goalItemDefinition, timestamp.Value)
        {
            // ParentEventId can be null
            Data = data,
            DataKey = dataKey,
            ItemId = goalItemDefinition,
            EngagementValue = engagementValue.Value,
            Text = text,
            Duration = TimeSpan.FromSeconds(duration.Value),
            CustomValues =
            {
                { "Id", Guid.NewGuid().ToString() },
                { "Data", _faker.Lorem.Word() },
                { "Date", _faker.Date.Recent(1, DateTime.UtcNow).AddHours(-1).ToString("f") }
            }
        };
        _interaction.Events.Add(goal);
        return this;
    }

    public EventModelBuilder WithCustomEvent(string? data = null, string? dataKey = null,
        int? engagementValue = null, string? text = null, int? duration = null)
    {
        data ??= _faker.Lorem.Word();
        dataKey ??= _faker.Lorem.Word();
        text ??= _faker.Lorem.Sentence();
        engagementValue ??= _faker.Random.Int(0, 10);
        duration ??= _faker.Random.Int(0, 1000);

        var customEvent = new Event(Guid.NewGuid(), DateTime.UtcNow.AddHours(-1))
        {
            Data = data,
            DataKey = dataKey,
            ItemId = Guid.NewGuid(),
            EngagementValue = engagementValue.Value,
            ParentEventId = _pageViewEvent.Id,
            Text = text,
            Duration = TimeSpan.FromSeconds(duration.Value),
            CustomValues =
            {
                { "Id", Guid.NewGuid().ToString() },
                { "Data", _faker.Lorem.Word() },
                { "Date", _faker.Date.Recent(1, DateTime.UtcNow).AddHours(-1).ToString("f") }
            }
        };
        _interaction.Events.Add(customEvent);
        return this;
    }

    public EventModelBuilder WithANumberOfRandomEvents(int numberOfEvents)
    {
        var eventMethods = new Action[]
        {
            //() => WithPageViewEvent(),
            () => WithSearchEvent(),
            () => WithDownloadEvent(),
            () => WithCampaignEvent(),
            () => WithOutcomeEvent(),
            () => WithGoalEvent(),
            () => WithCustomEvent()
            //() => WithChangeProfileScoreEvent() commented out that because it is to heavy to produce multiple times and Submit() crushes
        };

        for (var i = 0; i < numberOfEvents; i++)
        {
            int randomIndex =
                _faker.Random.Number(eventMethods.Length - 1); // -1 because are 7 elements in list to pick from 0 to 6 
            eventMethods[randomIndex].Invoke();
        }

        return this;
    }

    public DataGenerationResult Build()
    {
        _client.AddInteraction(_interaction);
        _client.Submit();
        return new DataGenerationResult(_interaction);
    }
}