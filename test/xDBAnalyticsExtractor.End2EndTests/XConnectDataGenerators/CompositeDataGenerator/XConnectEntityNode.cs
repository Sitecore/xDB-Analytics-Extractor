namespace xDBAnalyticsExtractor.End2EndTests.XConnectDataGenerators.CompositeDataGenerator;

public class XConnectEntityNode : XConnectEntityRoot
{
    public XConnectEntityNode() : base(xConnectClient)
    {
    }

    public override void Add(XConnectEntityRoot component)
    {
        throw new InvalidOperationException("Add method not supported for XConnectEntityNode.");
    }

    public override void Remove(XConnectEntityRoot component)
    {
        throw new InvalidOperationException("Remove method not supported for XConnectEntityNode.");
    }

    public override void BuildInteraction()
    {
    }
}