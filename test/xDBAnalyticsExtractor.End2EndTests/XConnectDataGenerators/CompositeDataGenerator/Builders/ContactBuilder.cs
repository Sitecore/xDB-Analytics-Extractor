using Bogus;
using Sitecore.XConnect.Collection.Model;

namespace xDBAnalyticsExtractor.End2EndTests.XConnectDataGenerators.CompositeDataGenerator.Builders;

public class ContactBuilder : XConnectEntityNode
{
    private PersonalInformation _personalInfoFacet;
    private EmailAddressList _emails;
    private Contact _contact = new();
    private bool _isIdentified = false;
    private readonly Faker _faker = new();

    public ContactBuilder AddPersonalInformation()
    {
        var birthdate = _faker.Person.DateOfBirth;
        var middleName = _faker.Name.Prefix();
        var firstName = _faker.Person.FirstName;
        var lastName = _faker.Person.LastName;
        var gender = _faker.PickRandom("He", "She", "It", "Him", "Her");
        var jobTitle = _faker.Name.JobTitle();
        var nickname = _faker.Person.UserName;
        var suffix = _faker.Name.Suffix();
        var title = _faker.Name.JobType();
        var preferredLanguage = _faker.PickRandom("en", "da-DK", "de-DE", "ja-JP", "zh-CN");

        AddPersonalInformation(birthdate, middleName, firstName, lastName, gender, jobTitle, nickname,
            suffix, title, preferredLanguage);
        return this;
    }

    public ContactBuilder AddPersonalInformation(DateTime? birthdate,
        string middleName, string firstName, string lastName, string gender, string jobTitle, string nickname,
        string suffix, string title, string preferredLanguage)
    {
        _personalInfoFacet = new PersonalInformation()
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
        return this;
    }

    public ContactBuilder AddContactIdentifiers()
    {
        var source = _faker.PickRandom(new[] { "Twitter", "Facebook", "Instagram", "LinkedIn", "Other" });
        var contactEmail = _faker.Person.Email;
        var preferredkey = _faker.PickRandom(new[] { "Email", "Phone", "Tablet", "Other" });

        AddContactIdentifiers(source, contactEmail, preferredkey);
        return this;
    }

    public ContactBuilder AddContactIdentifiers(string source, string contactEmail, string preferredkey)
    {
        // Identify the user based on source and contactEmail
        IdentifiedContactReference reference = new IdentifiedContactReference(source, contactEmail);
        var contact = xConnectClient.Result.Get<Contact>(reference,
            new ContactExpandOptions(PersonalInformation.DefaultFacetKey)
            {
                Interactions = new RelatedInteractionsExpandOptions()
            });

        // Note: when a contact is Identifyed the DeviceProfile ProfileScores ChangeProfileScoreEvent entities should not be created within the interaction, because xConnect crushes. Second interaction will still count though.
        if (contact != null && contact.Id != null)
        {
            _isIdentified = true;
            _contact = contact;
            return this;
        }

        contact = new Contact(new ContactIdentifier(source, contactEmail, ContactIdentifierType.Known));
        if (contact!.GetFacet<EmailAddressList>(EmailAddressList.DefaultFacetKey) == null)
        {
            //Update Email
            _emails =
                new EmailAddressList(new EmailAddress(contactEmail, true), preferredkey);
            xConnectClient.Result.SetFacet(contact, EmailAddressList.DefaultFacetKey, _emails);
        }

        _contact = contact;
        return this;
    }

    public Contact Build()
    {
        return _contact;
    }

    public override void BuildInteraction()
    {
        if (!_isIdentified)
        {
            xConnectClient.Result.SetPersonal(_contact, _personalInfoFacet);
            xConnectClient.Result.AddContact(_contact);
            xConnectClient.Result.Submit();
        }
    }
}