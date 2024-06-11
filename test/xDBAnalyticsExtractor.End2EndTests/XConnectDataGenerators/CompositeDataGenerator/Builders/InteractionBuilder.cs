namespace xDBAnalyticsExtractor.End2EndTests.XConnectDataGenerators.CompositeDataGenerator.Builders;

public class InteractionBuilder : XConnectEntityNode
{
    public enum ChannelSelection
    {
        Direct,
        Telemarketing
    }

    private readonly Guid _direct = Guid.Parse("B418E4F2-1013-4B42-A053-B6D4DCA988BF");
    private readonly Guid _telemarketing = Guid.Parse("8D52BB4E-22CF-483D-BE62-760A372160CD");
    private readonly Interaction _interaction;

    public InteractionBuilder(Contact contact,
        InteractionInitiator interactionInitiator, ChannelSelection channel, string userAgent)
    {
        _interaction = new Interaction(contact, interactionInitiator, SelectChannel(channel), userAgent);
    }

    private Guid SelectChannel(ChannelSelection selection)
    {
        switch (selection)
        {
            case ChannelSelection.Direct:
                return _direct;
            case ChannelSelection.Telemarketing:
                return _telemarketing;
            default:
                throw new ArgumentException("Invalid channel selection");
        }
    }

    public InteractionBuilder AddCampaignId(Guid campaignId)
    {
        _interaction.CampaignId = campaignId;

        return this;
    }

    public InteractionBuilder AddVenueId(Guid venueId)
    {
        _interaction.VenueId = venueId;

        return this;
    }

    public Interaction Build()
    {
        return _interaction;
    }

    public override void BuildInteraction()
    {
        xConnectClient.Result.AddInteraction(_interaction);
        xConnectClient.Result.Submit();
    }
}