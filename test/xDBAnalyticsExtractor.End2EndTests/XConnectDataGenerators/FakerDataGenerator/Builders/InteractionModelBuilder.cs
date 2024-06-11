using System.Globalization;
using Bogus;
using Sitecore.XConnect.Collection.Model;

namespace xDBAnalyticsExtractor.End2EndTests.XConnectDataGenerators.FakerDataGenerator.Builders;

/// <summary>
/// A builder class for generating and configuring various parts of interaction for a Sitecore XConnect interaction.
/// When initialized always beggin with always beggin with .WithContact().WithInteraction()
/// </summary>
public class InteractionModelBuilder
{
    private readonly XConnectClient _client;
    private readonly Faker _faker;
    private Contact? _contact;
    private Interaction? _interaction;
    private bool _isContactIdentified;

    public InteractionModelBuilder(XConnectClient client)
    {
        _client = client;
        _isContactIdentified = false;
        _faker = new Faker();
    }
    
    public InteractionModelBuilder WithContact(DateTime? birthdate = null, string? firstName = null, string? lastName = null,
        string? middleName = null,
        string? gender = null, string? jobTitle = null, string? nickname = null, string? suffix = null,
        string? title = null, string? preferredLanguage = null, string? contactEmail = null, string? source = null)
    {
        // When optional parameters are null generate data from faker
        birthdate ??= _faker.Date.Between(DateTime.Today.AddYears(-100), DateTime.Today.AddYears(19));
        firstName ??= _faker.Name.FirstName();
        lastName ??= _faker.Name.LastName();
        middleName ??= _faker.Name.FirstName();
        gender ??= _faker.PickRandom(new[] { "Male", "Female" });
        jobTitle ??= _faker.Name.JobTitle();
        nickname ??= _faker.Name.FirstName();
        suffix ??= _faker.Name.Suffix();
        title ??= _faker.Name.Prefix();
        preferredLanguage ??= _faker.PickRandom(new[] { "en", "da", "el", "de", "zh" });
        contactEmail ??= _faker.Person.Email;
        source ??= _faker.PickRandom(new[] { "Twitter", "Facebook", "Instagram", "LinkedIn", "Other" });
        
        // Identify the user based on source and contactEmail
        IdentifiedContactReference reference = new IdentifiedContactReference(source, contactEmail);
        var contact = _client.Get<Contact>(reference, new ContactExpandOptions(new string[] { PersonalInformation.DefaultFacetKey })
        {
            Interactions = new RelatedInteractionsExpandOptions()
            {
                StartDateTime = DateTime.UtcNow.AddDays(-1),
                EndDateTime = DateTime.UtcNow,
                Limit = 100
            }
        });
        
        if (contact!=null && contact.Id != null)
        {
            // Note: when a contact is Identifyed the methods WithDeviceProfile() .WithProfileScores() .WithChangeProfileScoreEvent() are ignores from code, because xConnect crushes. This is done with boolean _isContactIdentified = true
            _isContactIdentified = true;
            _contact = contact;
            return this;
        }
        
        contact = new Contact(new ContactIdentifier(source, contactEmail, ContactIdentifierType.Known));
        var personalInfoFacet = new PersonalInformation()
        {
            Birthdate = birthdate,
            FirstName = firstName,
            LastName = lastName,
            MiddleName = middleName,
            Gender = gender,
            JobTitle = jobTitle,
            Nickname = nickname,
            Suffix = suffix,
            Title = title,
            PreferredLanguage = preferredLanguage
        };

        if (contact.GetFacet<EmailAddressList>(EmailAddressList.DefaultFacetKey) == null)
        {
            //Update Email
            EmailAddressList emails =
                new EmailAddressList(new EmailAddress(contactEmail, true), "Home");
            _client.SetFacet(contact, EmailAddressList.DefaultFacetKey, emails);
        }

        _client.SetPersonal(contact, personalInfoFacet);
        _client.AddContact(contact);
        _client.Submit(); // This is needed in order to pass the contact id into the DeviceProfile
        _contact = contact;
        return this;
    }

    public InteractionModelBuilder WithInteraction(Guid? campaignId = null, Guid? venueId = null, string? userAgent = null)
    {
        // When optional parameters are null generate data from faker
        var channelId = _faker.PickRandom(new[] { Guid.Parse("D07286FA-67CE-4D66-8783-0140B8B91EF1"), Guid.Parse("3648772F-0CB9-4B90-9E65-A97B2C729008") }); // channel item guids are for: "Online" or "Offline"
        campaignId ??= _faker.Random.Guid();
        venueId ??= _faker.Random.Guid();
        userAgent ??= _faker.Internet.UserAgent();
        
        var interaction= new Interaction(_contact, InteractionInitiator.Brand, channelId, userAgent)
        {
            // CampaignId and VenueId can be nullables
            CampaignId = campaignId.Value,
            VenueId = venueId
        };
        _interaction = interaction;
        return this;
    }

    public InteractionModelBuilder WithWebVisit(string? browserMajorName = null, string? browserMinorName = null,
        string? browserVersion = null, string? language = null, string? oSMajorVersion = null,
        string? oSMinorVersion = null,
        string? oSName = null, string? referrer = null, bool? isSelfReferrer = null, int? screenWidth = null,
        int? screenHeight = null,
        string? searchKeywords = null, string? siteName = null)
    {
        // When optional parameters are null generate data from faker
        browserMajorName ??= _faker.Internet.DomainName();
        browserMinorName ??= _faker.Lorem.Word();
        browserVersion ??= _faker.Random.Number(1, 50).ToString();
        language ??= _faker.PickRandom(new[] { "en", "da", "el", "de", "zh" });
        oSMajorVersion ??= _faker.Random.Number(51, 100).ToString();
        oSMinorVersion ??= _faker.Random.Number(1, 1000).ToString();
        oSName ??= _faker.Lorem.Word();
        referrer ??= _faker.Internet.Url();
        isSelfReferrer ??= _faker.Random.Bool();
        screenWidth ??= _faker.Random.Number(360, 4000);
        screenHeight ??= _faker.Random.Number(360, 3000);
        searchKeywords ??= _faker.Lorem.Word();
        siteName ??= _faker.Company.CompanyName();

        var webVisitFacet = new WebVisit
        {
            Browser = new BrowserData()
            {
                BrowserMajorName = browserMajorName,
                BrowserMinorName = browserMinorName,
                BrowserVersion = browserVersion
            },
            Language = language,
            OperatingSystem = new OperatingSystemData()
            {
                MajorVersion = oSMajorVersion,
                MinorVersion = oSMinorVersion,
                Name = oSName
            },
            Referrer = referrer,
            IsSelfReferrer = isSelfReferrer.Value,
            Screen = new ScreenData()
            {
                ScreenWidth = screenWidth.Value,
                ScreenHeight = screenHeight.Value
            },
            SearchKeywords = searchKeywords,
            SiteName = siteName
        };
        _client.SetWebVisit(_interaction, webVisitFacet);
        return this;
    }

    public InteractionModelBuilder WithIpInfo(double? latitude = null, double? longitude = null, Guid? locationId = null, string? ipAddress = null,
        string? areaCode = null, string? businessName = null,
        string? city = null, string? country = null, string? isp = null, string? dns = null, string? metroCode = null,
        string? postalCode = null, string? region = null,
        string? url = null)
    {
        // When optional parameters are null generate data from faker
        latitude ??= _faker.Address.Latitude();
        longitude ??= _faker.Address.Longitude();
        locationId ??= _faker.Random.Guid();
        ipAddress ??= _faker.Internet.Ip();
        areaCode ??= _faker.Random.Number(100, 999).ToString();
        businessName ??= _faker.Company.CompanyName();
        city ??= _faker.Address.City();
        country ??= _faker.Address.Country();
        isp ??= _faker.Company.CompanyName();
        dns ??= _faker.Internet.DomainName();
        metroCode ??= _faker.Random.Number(100, 999).ToString();
        postalCode ??= _faker.Address.ZipCode();
        region ??= _faker.Address.State();
        url ??= _faker.Internet.Url();

        var ipInfoFacet = new IpInfo(ipAddress)
        {
            // latitude, longitude, locationId can be nullables
            AreaCode = areaCode,
            BusinessName = businessName,
            City = city,
            Country = country,
            Isp = isp,
            Dns = dns,
            Latitude = latitude.Value,
            Longitude = longitude.Value,
            LocationId = locationId.Value,
            MetroCode = metroCode,
            PostalCode = postalCode,
            Region = region,
            Url = url
        };

        _client.SetIpInfo(_interaction, ipInfoFacet);
        return this;
    }

    public InteractionModelBuilder WithUserAgent(string? deviceType = null, string? deviceVendor = null,
        string? deviceVendorHardwareModel = null)
    {
        // When optional parameters are null generate data from faker
        deviceType ??= _faker.PickRandom(new[] { "Desktop", "Mobile", "Tablet" });
        deviceVendor ??= _faker.PickRandom(new[] { "Chrome", "Firefox", "Safari" });
        deviceVendorHardwareModel ??= _faker.Lorem.Word();

        var userAgentFacet = new UserAgentInfo()
        {
            CanSupportTouchScreen = false,
            DeviceType = deviceType,
            DeviceVendor = deviceVendor,
            DeviceVendorHardwareModel = deviceVendorHardwareModel
        };
        _client.SetUserAgentInfo(_interaction, userAgentFacet);
        return this;
    }

    public InteractionModelBuilder WithLocaleInfo(int? timeZoneOffset = null, string? latitude = null,
        string? longitude = null)
    {
        // When optional parameters are null generate data from faker
        if (!timeZoneOffset.HasValue)
            timeZoneOffset ??= _faker.Random.Int(-12, 12);
        latitude ??= _faker.Address.Latitude().ToString(CultureInfo.InvariantCulture);
        longitude ??= _faker.Address.Longitude().ToString(CultureInfo.InvariantCulture);

        var localeInfoFacet = new LocaleInfo()
        {
            TimeZoneOffset = TimeSpan.FromHours(timeZoneOffset.Value),
            GeoCoordinate = new GeoCoordinate(double.Parse(latitude), double.Parse(longitude))
        };
        _client.SetLocaleInfo(_interaction, localeInfoFacet);
        return this;
    }

    public InteractionModelBuilder WithDeviceProfile()
    {
        if (!_isContactIdentified)
        {
            var deviceProfile = new DeviceProfile(_contact!.Id!.Value)
            {
                LastKnownContact = _contact
            };
            if (_interaction != null)
                _interaction.DeviceProfile = new DeviceProfileReference(deviceProfile.Id!.Value);
            _client.AddDeviceProfile(deviceProfile);
            return this;
        }
        // If contact is identified, do nothing
        return this;
    }

    public EventModelBuilder AddEvents()
    {
        return new EventModelBuilder(_interaction!, _client, _contact!, _isContactIdentified);
    }
}