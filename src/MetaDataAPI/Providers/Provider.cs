using MetaDataAPI.Models.Response;

namespace MetaDataAPI.Providers;

public abstract class Provider : IProvider
{
    public abstract string ProviderName { get; }
    public abstract IEnumerable<Erc721Attribute> ProviderAttributes { get; }
    public BasePoolInfo PoolInfo { get; }
    public List<Erc721Attribute> Attributes { get; }

    protected Provider(BasePoolInfo basePoolInfo)
    {
        PoolInfo = basePoolInfo;
        Attributes = new List<Erc721Attribute>();
        AddAttributes();
    }

    public abstract string GetDescription();
    internal void AddAttributes()
    {
        Attributes.Add(ProviderNameAttribute);
        Attributes.Add(TokenNameAttribute);
        Attributes.AddRange(ProviderAttributes);
    }

    private Erc721Attribute ProviderNameAttribute => new("ProviderName", ProviderName);
    private Erc721Attribute TokenNameAttribute => new("TokenName", PoolInfo.Token.Name);
}
