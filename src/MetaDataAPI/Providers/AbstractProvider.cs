using Net.Urlify;
using Nethereum.Web3;
using System.Numerics;
using HandlebarsDotNet;
using System.Reflection;
using Net.Urlify.Attributes;
using MetaDataAPI.Erc20Manager;
using EnvironmentManager.Extensions;
using MetaDataAPI.Providers.Attributes;
using MetaDataAPI.Providers.Image.Models;
using MetaDataAPI.Providers.Attributes.Models;
using poolz.finance.csharp.contracts.LockDealNFT.ContractDefinition;
using MetaDataAPI.BlockchainManager.Models;

namespace MetaDataAPI.Providers;

public abstract class AbstractProvider : Urlify
{
    protected readonly IErc20Provider erc20Provider;

    public IEnumerable<BasePoolInfo> FullData { get; }

    public BasePoolInfo PoolInfo { get; }

    public ChainInfo ChainInfo { get; }

    public Erc20Token Erc20Token { get; }

    [QueryStringProperty("id")]
    public BigInteger PoolId { get; }

    [QueryStringProperty("name")]
    public string Name { get; }

    [Erc721Attribute("collection", DisplayType.Number)]
    public virtual BigInteger VaultId { get; }

    [Erc721Attribute("left amount", DisplayType.Number)]
    public decimal LeftAmount { get; }

    [QueryStringProperty("tA")]
    public QueryStringToken Token { get; set; }

    protected AbstractProvider(BasePoolInfo[] poolsInfo, ChainInfo chainInfo)
        : this(poolsInfo, chainInfo, new Erc20Provider())
    { }

    protected AbstractProvider(BasePoolInfo[] poolsInfo, ChainInfo chainInfo, IErc20Provider erc20Provider)
        : base((string)Environments.NFT_HTML_ENDPOINT.Get())
    {
        this.erc20Provider = erc20Provider;

        FullData = poolsInfo;
        PoolInfo = poolsInfo[0];
        ChainInfo = chainInfo;

        Erc20Token = erc20Provider.GetErc20Token(chainInfo, PoolInfo.Token);

        PoolId = PoolInfo.PoolId;
        Name = PoolInfo.Name;
        VaultId = PoolInfo.VaultId;
        LeftAmount = LeftAmount = Web3.Convert.FromWei(PoolInfo.Params[0], Erc20Token.Decimals);

        Token = new QueryStringToken(Erc20Token.Name, "Left Amount", LeftAmount);
    }

    protected abstract string DescriptionTemplate { get; }
    protected virtual void OnDescriptionCreating() { }

    private string GetDescription()
    {
        OnDescriptionCreating();

        return Handlebars.Compile(DescriptionTemplate)(this);
    }

    private string GetImage()
    {
        // TODO: Create ITlyContext interface, and use it here to return short url
        return new UrlifyProvider(this).BuildUrl();
    }

    private IEnumerable<Erc721Attribute> GetAttributes()
    {
        return this.GetType().GetProperties()
            .Select(property => new { Property = property, Attribute = property.GetCustomAttribute<Erc721AttributeAttribute>() })
            .Where(x => x.Attribute != null)
            .Select(x => new
            {
                x.Property,
                Attribute = x.Attribute!,
                Value = x.Property.GetValue(this) ?? throw new InvalidOperationException($"Cannot process Erc721Attribute property '{x.Property.Name}' with nullable value.")
            })
            .Select(x => new Erc721Attribute(x.Attribute.TraitType, x.Value, x.Attribute.DisplayType));
    }

    public Erc721Metadata GetErc721Metadata()
    {
        return new Erc721Metadata(
            name: $"Lock Deal NFT Pool: {PoolId}",
            description: GetDescription(),
            image: GetImage(),
            attributes: GetAttributes()
        );
    }
}