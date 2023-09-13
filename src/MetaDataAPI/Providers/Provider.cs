using MetaDataAPI.Models.Response;

namespace MetaDataAPI.Providers;

public abstract class Provider : IProvider
{
    public BasePoolInfo PoolInfo { get; }
    public List<Erc721Attribute> Attributes { get; }

    protected Provider(BasePoolInfo basePoolInfo)
    {
        PoolInfo = basePoolInfo;
        Attributes = new List<Erc721Attribute>();
    }

    public abstract string GetDescription();
    public abstract List<Erc721Attribute> GetParams();
    internal void AddAttributes(string name)
    {
        Attributes.Add(new Erc721Attribute("ProviderName", name));
        Attributes.AddRange(GetParams());
    }
}
