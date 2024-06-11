using xDBAnalyticsExtractor.Dto;
using xDBAnalyticsExtractor.Models;
using xDBAnalyticsExtractor.Processors;
using Serilog;
using Sitecore.Marketing.Definitions;
using Sitecore.Marketing.Taxonomy.Mapping;
using Sitecore.Marketing.Taxonomy.Model.Channel;
using System.Globalization;

namespace xDBAnalyticsExtractor.Utils;

public static class InteractionDtoUtils
{
    public static (IEnumerable<InteractionModel?> interactionModels,
        IEnumerable<CampaignModel> campaignModels,
        IEnumerable<DeviceModel?> deviceModels,
        IEnumerable<DownloadModel> downloadModels,
        IEnumerable<GeoNetworkModel?> geoNetworkModels,
        IEnumerable<GoalModel> goalModels,
        IEnumerable<OutcomeModel> outcomeModels,
        IEnumerable<PageViewModel> pageViewModels,
        IEnumerable<SearchModel> searchModels
        ) SplitEntities(IEnumerable<InteractionDto> interactionDtos)
    {
        var interactionModels = interactionDtos.Select(dto => dto.InteractionModel).ToList();
        var campaignModels = interactionDtos.SelectMany(dto => dto.CampaignModels).ToList();
        var deviceModels = interactionDtos.Where(dto => dto.DeviceModel is not null).Select(dto => dto.DeviceModel).ToList();
        var downloadModels = interactionDtos.SelectMany(dto => dto.DownloadModels).ToList();
        var geoNetworkModels = interactionDtos.Where(dto => dto.GeoNetworkModel is not null).Select(dto => dto.GeoNetworkModel).ToList();
        var goalModels = interactionDtos.SelectMany(dto => dto.GoalModels).ToList();
        var outcomeModels = interactionDtos.SelectMany(dto => dto.OutcomeModels).ToList();
        var pageViewModels = interactionDtos.SelectMany(dto => dto.PageViewModels).ToList();
        var searchModels = interactionDtos.SelectMany(dto => dto.SearchModels).ToList();

        return (interactionModels, campaignModels, deviceModels, downloadModels,
            geoNetworkModels, goalModels, outcomeModels, pageViewModels, searchModels);
    }

    public static (IEnumerable<GoalDefinitionModel> goalDefinitionModels,
        IEnumerable<CampaignDefinitionModel> campaignDefinitionModels,
        IEnumerable<OutcomeDefinitionModel> outcomeDefinitionModels,
        IEnumerable<EventDefinitionModel> eventDefinitionModels)
        RetrieveDefinitions()
    {
        var _goalDefinitionManager = DefinitionManagerExternal.CreateGoalDefinitionManager();
        var _campaignDefinitionManager = DefinitionManagerExternal.CreateCampaignDefinitionManager();
        var _outcomeDefinitionManager = DefinitionManagerExternal.CreateOutcomeDefinitionManager();
        var _eventDefinitionManager = DefinitionManagerExternal.CreateEventDefinitionManager();
        var goalDefinitions = _goalDefinitionManager.GetAll(CultureInfo.InvariantCulture);
        var campaignDefinitions = _campaignDefinitionManager.GetAll(CultureInfo.InvariantCulture);
        var outcomeDefinitions = _outcomeDefinitionManager.GetAll(CultureInfo.InvariantCulture);
        var eventDefinitions = _eventDefinitionManager.GetAll(CultureInfo.InvariantCulture);
        var goalDefinitionModels = GoalDefinitionProcessor.Process(goalDefinitions);
        var campaignDefinitionModels = CampaignDefinitionProcessor.Process(campaignDefinitions);
        var outcomeDefinitionModels = OutcomeDefinitionProcessor.Process(outcomeDefinitions);
        var eventDefinitionModels = EventDefinitionProcessor.Process(eventDefinitions);
        return (goalDefinitionModels, campaignDefinitionModels, outcomeDefinitionModels, eventDefinitionModels);
    }

    public static IEnumerable<ChannelModel> GetTaxonModels(IEnumerable<InteractionDto> records)
    {
        var channelIds = records.Select(x => x.InteractionModel?.ChannelId).Distinct().ToArray();
        var _channelDefinitionManager = DefinitionManagerExternal.CreateChannelTaxonomyManager();
        List<Channel> channels = new(channelIds.Length);
        if (channelIds is null)
        {
            return Enumerable.Empty<ChannelModel>();
        }
        for (int i = 0; i < channelIds?.Length; i++)
        {
            try
            {
                if (channelIds is not null && channelIds[0] is not null)
                {
                    channels.Add(_channelDefinitionManager.GetChannel((Guid)channelIds[0]!, CultureInfo.InvariantCulture));
                }
            }
            catch (InvalidTypeMappingException ex)
            {
                Log.Error(ex, $"Error on channel id {channelIds[i]}. Incorrect type.");
            }
        }
        var channelModels = ChannelProcessor.Process(channels);
        return channelModels;
    }
}
