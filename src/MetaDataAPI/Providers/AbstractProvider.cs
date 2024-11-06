using Net.Urlify;
using Nethereum.Web3;
using System.Numerics;
using HandlebarsDotNet;
using System.Reflection;
using Net.Urlify.Attributes;
using MetaDataAPI.Services.Erc20;
using MetaDataAPI.Providers.Image;
using EnvironmentManager.Extensions;
using MetaDataAPI.Services.ChainsInfo;
using MetaDataAPI.Providers.Attributes;
using Microsoft.Extensions.DependencyInjection;
using poolz.finance.csharp.contracts.LockDealNFT;
using poolz.finance.csharp.contracts.LockDealNFT.ContractDefinition;

namespace MetaDataAPI.Providers;

public abstract class AbstractProvider : Urlify
{
    protected readonly ILockDealNFTService LockDealNft;
    protected readonly IErc20Provider Erc20Provider;

    public IEnumerable<BasePoolInfo> FullData { get; }
    public BasePoolInfo PoolInfo { get; }
    public ChainInfo ChainInfo { get; }
    public Erc20Token Erc20Token { get; }

    [QueryStringProperty("id")]
    public BigInteger PoolId { get; }

    [QueryStringProperty("name")]
    [Erc721MetadataItem("provider name", DisplayType.String)]
    public string Name { get; }

    [Erc721MetadataItem("collection", DisplayType.Number)]
    public virtual BigInteger VaultId { get; }

    [Erc721MetadataItem("left amount", DisplayType.Number)]
    public decimal LeftAmount { get; }

    [QueryStringProperty("tA")]
    public QueryStringToken Token { get; }

    protected AbstractProvider(BasePoolInfo[] poolsInfo, ChainInfo chainInfo)
        : this(poolsInfo, chainInfo, DefaultServiceProvider.Instance)
    { }

    protected AbstractProvider(BasePoolInfo[] poolsInfo, ChainInfo chainInfo, IServiceProvider serviceProvider)
        : base((string)Environments.NFT_HTML_ENDPOINT.Get())
    {
        Erc20Provider = serviceProvider.GetRequiredService<IErc20Provider>();
        LockDealNft = serviceProvider.GetRequiredService<ILockDealNFTService>();

        FullData = poolsInfo;
        PoolInfo = poolsInfo[0];
        ChainInfo = chainInfo;

        Erc20Token = Erc20Provider.GetErc20Token(chainInfo, PoolInfo.Token);

        PoolId = PoolInfo.PoolId;
        Name = PoolInfo.Name;
        VaultId = PoolInfo.VaultId;
        LeftAmount = Web3.Convert.FromWei(PoolInfo.Params[0], Erc20Token.Decimals);

        Token = new QueryStringToken(Erc20Token.Name, "Left Amount", LeftAmount);
    }

    protected abstract string DescriptionTemplate { get; }

    private string GetDescription()
    {
        var isFullyWithdrawn = ChainInfo.LockDealNFT == PoolInfo.Owner;
        if (isFullyWithdrawn) return "This NFT has been fully withdrawn and is no longer governing any assets.";

        var isFullyRefunded = ChainInfo.LockDealNFT != PoolInfo.Owner && LeftAmount == 0;
        if (isFullyRefunded) return "This NFT has been fully refunded and no longer holds any governance over assets.";

        return Handlebars.Compile(DescriptionTemplate)(this);
    }

    private string GetImage()
    {
        return new UrlifyProvider(this).BuildUrl();
    }

    private IEnumerable<Erc721MetadataItem> GetAttributes()
    {
        return GetType().GetProperties()
            .Select(property => new { Property = property, Attribute = property.GetCustomAttribute<Erc721MetadataItemAttribute>() })
            .Where(x => x.Attribute != null)
            .Select(x => new
            {
                x.Property,
                Attribute = x.Attribute!,
                Value = x.Property.GetValue(this) ?? throw new InvalidOperationException($"Cannot process {nameof(Erc721MetadataItem)} property '{x.Property.Name}' with nullable value.")
            })
            .Select(x => new Erc721MetadataItem(x.Attribute.TraitType, x.Value, x.Attribute.DisplayType));
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

    public static AbstractProvider CreateFromPoolInfo(BasePoolInfo[] poolsInfo, ChainInfo chainInfo, IServiceProvider serviceProvider) =>
        CreateFromPoolInfo<AbstractProvider>(poolsInfo, chainInfo, serviceProvider);

    public static TProvider CreateFromPoolInfo<TProvider>(BasePoolInfo[] poolsInfo, ChainInfo chainInfo, IServiceProvider serviceProvider)
        where TProvider : AbstractProvider
    {
        var poolInfo = poolsInfo[0];
        var type = Type.GetType($"MetaDataAPI.Providers.{poolInfo.Name}, MetaDataAPI")
            ?? throw new InvalidOperationException($"Cannot found '{poolInfo.Name}' type. Please check if this Provider implemented.");
        return (TProvider)Activator.CreateInstance(type, poolsInfo, chainInfo, serviceProvider)!;
    }
}