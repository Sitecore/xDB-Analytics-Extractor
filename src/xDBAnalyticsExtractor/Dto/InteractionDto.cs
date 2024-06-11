using xDBAnalyticsExtractor.Models;

namespace xDBAnalyticsExtractor.Dto;

public class InteractionDto
{
    public InteractionModel? InteractionModel { get; init; } = new();
    public List<GoalModel> GoalModels { get; init; } = new List<GoalModel>();
    public List<CampaignModel> CampaignModels { get; init; } = new List<CampaignModel>();
    public List<DownloadModel> DownloadModels { get; init; } = new List<DownloadModel>();
    public List<OutcomeModel> OutcomeModels { get; init; } = new List<OutcomeModel>();
    public List<PageViewModel> PageViewModels { get; init; } = new List<PageViewModel>();
    public List<SearchModel> SearchModels { get; init; } = new List<SearchModel>();
    public DeviceModel? DeviceModel { get; init; } = new();
    public GeoNetworkModel? GeoNetworkModel { get; init; } = new();
}