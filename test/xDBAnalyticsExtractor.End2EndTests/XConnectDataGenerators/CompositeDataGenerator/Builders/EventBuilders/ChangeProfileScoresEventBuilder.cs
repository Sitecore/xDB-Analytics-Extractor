using Sitecore.XConnect.Collection.Model;

namespace xDBAnalyticsExtractor.End2EndTests.XConnectDataGenerators.CompositeDataGenerator.Builders.EventBuilders;

public class ChangeProfileScoresEventBuilder : XConnectEntityNode
{
    //Profiles
    public enum ProfileDefinitionSelection
    {
        Focus,
        Function,
        Persona,
        Score
    }

    //Profile Keys
    private readonly Guid _profileFocus = Guid.Parse("{24DFF2CF-B30A-4B75-8967-2FE3DED82271}");
    private readonly Guid _profileFunction = Guid.Parse("{BA06B827-C6F2-4748-BD75-AA178B770E83}");
    private readonly Guid _profilePersona = Guid.Parse("{B5BDEE45-C945-476F-9EE6-3B8A9255C17E}");
    private readonly Guid _profileScore = Guid.Parse("{9311296F-1A3D-4E58-AED1-0A6B4B1E654B}");

    //Focus Profiles Keys
    private readonly Guid _focusKeyBackground = Guid.Parse("{03379AF5-F1AE-4610-B15B-4C7F1032B464}");
    private readonly Guid _focusKeyPractical = Guid.Parse("{5FDD9829-E689-454D-9ABC-8F95AE68744C}");
    private readonly Guid _focusKeyProcess = Guid.Parse("{F5652C06-676B-4E12-A9D0-06D000E5F1C8}");
    private readonly Guid _focusKeyScope = Guid.Parse("{B32BFACC-3494-4127-B050-CF50078E2B4C}");

    //Function Profile Keys
    private readonly Guid _functionKeyBuildingTrust = Guid.Parse("{233A2D01-6CCD-4D4E-BBEE-9196D492736C}");
    private readonly Guid _functionKeyCallToAction = Guid.Parse("{9581F4A4-B83D-4F37-9814-59226398566F}");
    private readonly Guid _functionKeyCreateDesire = Guid.Parse("{30AAF69F-C10A-4057-B918-6F8A8D0CEFDD}");
    private readonly Guid _functionKeyDefineConcept = Guid.Parse("{3C82B054-218F-40B7-9201-C2A126AD2C07}");

    //Persona Profiles

    private readonly Guid _personaKeyCecile = Guid.Parse("{B8810A60-3837-4AD1-B0DE-CF14AC5F4792}");
    private readonly Guid _personaKeyChris = Guid.Parse("{50278469-25F6-456B-BF9A-92C22FD38086}");
    private readonly Guid _personaKeyIan = Guid.Parse("{F2D7FF63-50A2-4595-96E6-2912864FBC90}");
    private readonly Guid _personaKeySandra = Guid.Parse("{9790D1F9-1F78-4B39-933D-46B08230F05F}");

    //Score Profiles

    private readonly Guid _scoreKeyLead = Guid.Parse("{178107BF-4294-4FA4-A0DA-30BDE00A1346}");

    // Rest of the fields
    private readonly ChangeProfileScoresEvent _changeProfileScoresEvent;
    private ProfileScoreDelta _delta = null!;
    private readonly DateTime _timestamp;
    private readonly Interaction _interaction;
    private ContactBehaviorProfile _cbp = null!;
    private readonly Contact _contact;
    private readonly ProfileScore _profileScoreObject;
    private readonly Dictionary<Guid, double> _values;

    public ChangeProfileScoresEventBuilder(Interaction interaction, Contact contact,
        DateTime timestamp)
    {
        _timestamp = timestamp;
        _contact = contact;
        _interaction = interaction;
        _changeProfileScoresEvent = new ChangeProfileScoresEvent(timestamp);
        _profileScoreObject = new ProfileScore();
        _values = new Dictionary<Guid, double>();
    }

    public ChangeProfileScoresEventBuilder AddProfileScoreDelta(ProfileDefinitionSelection profileDefinition, int score,
        bool isFirstInteractionOfContact)
    {
        _delta = new ProfileScoreDelta
        {
            ScoreCount = score
        };
        CalculateProfileScores(profileDefinition, score);
        CalculateChangeProfileScoresEventDelta(profileDefinition, score, _delta);
        _changeProfileScoresEvent.Delta.Add(SelectProfile(profileDefinition), _delta);

        //Only set this to true for the first time of interaction of the contact
        if (isFirstInteractionOfContact)
        {
            _cbp = new ContactBehaviorProfile
            {
                SourceInteractionStartDateTime = _timestamp
            };
            // Set the Values property
            _profileScoreObject.Values = _values;
            _cbp.Scores.Add(SelectProfile(profileDefinition), _profileScoreObject);
        }

        return this;
    }

    public ChangeProfileScoresEventBuilder AddMatchedPatternId(Guid? fakeOrRealMatchedPatternId)
    {
        _profileScoreObject.MatchedPatternId =
            fakeOrRealMatchedPatternId; // Needs a Guid of matched profile, it can be fake or assign with real card id
        return this;
    }

    public ChangeProfileScoresEventBuilder AddProfileDefinitionId(ProfileDefinitionSelection profileDefinition)
    {
        _profileScoreObject.ProfileDefinitionId = SelectProfile(profileDefinition);
        return this;
    }

    public ChangeProfileScoresEventBuilder AddProfileScore(double score)
    {
        _profileScoreObject.Score = score;
        return this;
    }

    public ChangeProfileScoresEventBuilder AddProfileScoreCount(int scoreCount)
    {
        _profileScoreObject.ScoreCount = scoreCount;
        return this;
    }

    private void CalculateProfileScores(ProfileDefinitionSelection profileDefinitionDefinition, int scoreCount)
    {
        //Calculate profile score and insert it to the ProfileScore element, Profile Score will be used later
        //for inserting into CBP for first time and Calculate CBP values
        switch (SelectProfile(profileDefinitionDefinition))
        {
            case var r when (r == _profileFocus):
                _values.Add(_focusKeyBackground, 5 * scoreCount); //Background
                _values.Add(_focusKeyPractical, 2 * scoreCount); //Practical
                _values.Add(_focusKeyProcess, 4 * scoreCount); //Process
                _values.Add(_focusKeyScope, 1 * scoreCount); //Scope
                break;
            case var r when (r == _profileFunction):
                _values.Add(_functionKeyBuildingTrust, 2 * scoreCount); //Building Trust
                _values.Add(_functionKeyCallToAction, 0); //Call to Action
                _values.Add(_functionKeyCreateDesire, 2 * scoreCount); //CreateDesire
                _values.Add(_functionKeyDefineConcept, 0); //Define Concept
                break;
            case var r when (r == _profilePersona):
                _values.Add(_personaKeyCecile, 5 * scoreCount); //Cecile
                _values.Add(_personaKeyChris, 0); //Chriss
                _values.Add(_personaKeyIan, 5 * scoreCount); //Ian
                _values.Add(_personaKeySandra, 0); //Sandra
                break;
            case var r when (r == _profileScore):
                _values.Add(_scoreKeyLead, 10 * scoreCount); //Lead
                break;
        }
    }

    private void CalculateChangeProfileScoresEventDelta(ProfileDefinitionSelection profileDefinitionDefinition, int scoreCount,
        ProfileScoreDelta delta)
    {
        //Calculate profile score and insert it to the ProfileScore element, Profile Score will be used later
        //for inserting into CBP for first time and Calculate CBP values

        switch (SelectProfile(profileDefinitionDefinition))
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

    // Profile selector
    private Guid SelectProfile(ProfileDefinitionSelection definitionSelection)
    {
        switch (definitionSelection)
        {
            case ProfileDefinitionSelection.Focus:
                return _profileFocus;
            case ProfileDefinitionSelection.Function:
                return _profileFunction;
            case ProfileDefinitionSelection.Persona:
                return _profilePersona;
            case ProfileDefinitionSelection.Score:
                return _profileScore;
            default:
                throw new ArgumentException("Invalid profile selection");
        }
    }

    public ProfileScore Build()
    {
        return _profileScoreObject;
    }

    public override void BuildInteraction()
    {
        _interaction.Events.Add(_changeProfileScoresEvent);
        xConnectClient.Result.SetFacet(_contact, ContactBehaviorProfile.DefaultFacetKey, _cbp);
    }
}