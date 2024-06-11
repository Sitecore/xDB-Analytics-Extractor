using xDBAnalyticsExtractor.Models;
using Sitecore.XConnect.Collection.Model;

namespace xDBAnalyticsExtractor.Processors;

public static class GeoNetworkProcessor
{
    public static GeoNetworkModel? Process(IpInfo? geoNetwork, Guid? interactionId)
    {
        if (geoNetwork is null || interactionId is null) 
            return null;

        return new GeoNetworkModel
        {
            Area = geoNetwork?.AreaCode ?? "null",
            Country = geoNetwork?.Country ?? "null",
            Region = geoNetwork?.Region ?? "null",
            Metro = geoNetwork?.MetroCode ?? "null",
            City = geoNetwork?.City ?? "null",
            Latitude = geoNetwork?.Latitude ?? null,
            Longitude = geoNetwork?.Longitude ?? null,
            LocationId = geoNetwork?.LocationId ?? null,
            PostalCode = geoNetwork?.PostalCode ?? "null",
            InteractionId = interactionId,
            BusinessName =  geoNetwork?.BusinessName ?? "null",
            IspName = geoNetwork?.Isp ?? "null"
        };
    }
}