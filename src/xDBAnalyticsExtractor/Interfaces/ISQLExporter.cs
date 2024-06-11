using xDBAnalyticsExtractor.Dto;

namespace xDBAnalyticsExtractor.Interfaces;

public interface ISQLExporter
{
    void ExportToDatabase(IEnumerable<InteractionDto> records);
}
