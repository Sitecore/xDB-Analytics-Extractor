namespace xDBAnalyticsExtractor.End2EndTests.XConnectDataGenerators.CompositeDataGenerator.Builders;

public class DeviceProfileBuilder : XConnectEntityNode
{
    private readonly DeviceProfile _deviceProfile;
    private readonly Interaction _interaction;

    public DeviceProfileBuilder(Interaction interaction)
    {
        _deviceProfile = new DeviceProfile();
        _interaction = interaction;
    }

    // Overloaded constructor for Device
    public DeviceProfileBuilder(Interaction interaction, Guid guid)
    {
        _deviceProfile = new DeviceProfile(guid);
        _interaction = interaction;
    }

    public DeviceProfileBuilder AddLastKnownContact(Contact contact)
    {
        _deviceProfile.LastKnownContact = contact;
        _interaction.DeviceProfile = new DeviceProfileReference(_deviceProfile.Id!.Value);
        return this;
    }

    public DeviceProfile Build()
    {
        return _deviceProfile;
    }

    public override void BuildInteraction()
    {
        xConnectClient.Result.AddDeviceProfile(_deviceProfile);
    }
}