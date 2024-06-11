namespace xDBAnalyticsExtractor.End2EndTests.XConnectDataGenerators.CompositeDataGenerator;

public class XConnectEntityRoot : XConnectEntity
{
    private readonly List<XConnectEntity> _children = new List<XConnectEntity>();

    protected static Task<XConnectClient> xConnectClient;

    public XConnectEntityRoot(Task<XConnectClient> xConnectClient)
    {
        XConnectEntityRoot.xConnectClient = xConnectClient;
    }

    public virtual void Add(XConnectEntityRoot component)
    {
        _children.Add(component);
    }

    public virtual void Remove(XConnectEntityRoot component)
    {
        _children.Remove(component);
    }

    public XConnectEntityRoot MakeShallowCopy()
    {
        return (XConnectEntityRoot)MemberwiseClone();
    }

    public List<XConnectEntityRoot> MakeShallowCopies(int count)
    {
        var copies = new List<XConnectEntityRoot>();
        for (int i = 0; i < count; i++)
        {
            copies.Add((XConnectEntityRoot)MemberwiseClone());
        }

        return copies;
    }

    public override void BuildInteraction()
    {
        foreach (var child in _children)
        {
            child.BuildInteraction();
        }
    }
}