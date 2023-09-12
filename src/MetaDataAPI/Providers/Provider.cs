using MetaDataAPI.Models.Response;

namespace MetaDataAPI.Providers;

public abstract class Provider : IProvider
{
    protected Provider(BasePoolInfo basePoolInfo)
    {
        PoolInfo = basePoolInfo;
        Attributes = new();
    }
    public BasePoolInfo PoolInfo { get;}

    public List<Erc721Attribute> Attributes { get; }

    public abstract string GetDescription();
    internal void AddAttributes(string name)
    {
        Attributes.Add(new Erc721Attribute("ProviderName", name, Models.Types.DisplayType.String));
        Attributes.AddRange(GetParams());
    }
    public abstract List<Erc721Attribute> GetParams();
}
