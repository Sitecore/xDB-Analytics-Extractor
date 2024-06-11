using System.Data;
using xDBAnalyticsExtractor.End2EndTests.Interfaces;
using Sitecore.XConnect.Collection.Model;

namespace xDBAnalyticsExtractor.End2EndTests.DataTableMappers.SqlDataTableMappers;

public class SqlGeoNetworkToDataTableMapper : IDataTableMapper
{
    public DataTable XConnectEntityToDataTable(IEnumerable<Interaction> interactions)
    {
        DataTable geoNetworkDataTable = new DataTable();

        DataColumn area = new DataColumn("Area", typeof(string))
        {
            MaxLength = Int32.MaxValue
        };
        geoNetworkDataTable.Columns.Add(area);

        DataColumn country = new DataColumn("Country", typeof(string))
        {
            MaxLength = Int32.MaxValue
        };
        geoNetworkDataTable.Columns.Add(country);

        DataColumn region = new DataColumn("Region", typeof(string))
        {
            MaxLength = Int32.MaxValue
        };
        geoNetworkDataTable.Columns.Add(region);

        DataColumn metro = new DataColumn("Metro", typeof(string))
        {
            MaxLength = Int32.MaxValue
        };
        geoNetworkDataTable.Columns.Add(metro);

        DataColumn city = new DataColumn("City", typeof(string))
        {
            MaxLength = Int32.MaxValue
        };
        geoNetworkDataTable.Columns.Add(city);

        DataColumn latitude = new DataColumn("Latitude", typeof(double));
        geoNetworkDataTable.Columns.Add(latitude);

        DataColumn longitude = new DataColumn("Longitude", typeof(double));
        geoNetworkDataTable.Columns.Add(longitude);

        DataColumn locationId = new DataColumn("LocationId", typeof(Guid));
        geoNetworkDataTable.Columns.Add(locationId);

        DataColumn postalCode = new DataColumn("PostalCode", typeof(string))
        {
            MaxLength = 20
        };
        geoNetworkDataTable.Columns.Add(postalCode);
        
        DataColumn ispName = new DataColumn("IspName", typeof(string))
        {
            MaxLength = Int32.MaxValue
        };
        geoNetworkDataTable.Columns.Add(ispName);
        
        DataColumn businessName = new DataColumn("BusinessName", typeof(string))
        {
            MaxLength = Int32.MaxValue
        };
        geoNetworkDataTable.Columns.Add(businessName);
        
        DataColumn interactionId = new DataColumn("InteractionId", typeof(Guid));
        geoNetworkDataTable.Columns.Add(interactionId);

        foreach (var interaction in interactions)
        {
            geoNetworkDataTable.Rows.Add(
                interaction.GetFacet<IpInfo>().AreaCode ?? "null",
                interaction.GetFacet<IpInfo>().Country ?? "null",
                interaction.GetFacet<IpInfo>().Region ?? "null",
                interaction.GetFacet<IpInfo>().MetroCode ?? "null",
                interaction.GetFacet<IpInfo>().City ?? "null",
                interaction.GetFacet<IpInfo>().Latitude ?? null,
                interaction.GetFacet<IpInfo>().Longitude ?? null,
                interaction.GetFacet<IpInfo>().LocationId ?? null,
                interaction.GetFacet<IpInfo>().PostalCode ?? "null",
                interaction.GetFacet<IpInfo>().Isp,
                interaction.GetFacet<IpInfo>().BusinessName ?? "null",
                interaction.Id

            ).AcceptChanges();
        }

        return geoNetworkDataTable;
    }
}