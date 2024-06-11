namespace xDBAnalyticsExtractor.ExportSchema;

public class Export
{
    public int Id { get; set; }
    public DateTime LastExported { get; set; }
    public Export(DateTime lastExported)
    {
        LastExported = lastExported;
    }
    public Export()
    {
        
    }
}
