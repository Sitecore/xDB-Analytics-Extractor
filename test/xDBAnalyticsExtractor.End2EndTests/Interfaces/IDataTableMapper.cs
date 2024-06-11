using System.Data;

namespace xDBAnalyticsExtractor.End2EndTests.Interfaces;

public interface IDataTableMapper
{
    DataTable XConnectEntityToDataTable(IEnumerable<Interaction> interactions);
}