using xDBAnalyticsExtractor.Dto;
namespace xDBAnalyticsExtractor.Interfaces;

public interface IFileExporter
{
    void ExportToFile(IEnumerable<InteractionDto> records);
}
