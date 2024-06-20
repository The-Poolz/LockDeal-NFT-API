using Net.Urlify;
using TLY.ShortUrl;
using Nethereum.Web3;
using System.Numerics;
using HandlebarsDotNet;
using System.Reflection;
using Net.Urlify.Attributes;
using MetaDataAPI.Erc20Manager;
using EnvironmentManager.Extensions;
using MetaDataAPI.Providers.Attributes;
using MetaDataAPI.Providers.Image.Models;
using MetaDataAPI.BlockchainManager.Models;
using MetaDataAPI.Providers.Attributes.Models;
using Microsoft.Extensions.DependencyInjection;
using poolz.finance.csharp.contracts.LockDealNFT.ContractDefinition;
using poolz.finance.csharp.contracts.LockDealNFT;

namespace MetaDataAPI.Providers;

public abstract class AbstractProvider : Urlify
{
    protected readonly IErc20Provider erc20Provider;
    protected readonly ITlyContext tlyContext;

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
        : this(poolsInfo, chainInfo, DefaultServiceProvider.Instance)
    { }

    protected AbstractProvider(BasePoolInfo[] poolsInfo, ChainInfo chainInfo, IServiceProvider serviceProvider)
        : base((string)Environments.NFT_HTML_ENDPOINT.Get())
    {
        erc20Provider = serviceProvider.GetService<IErc20Provider>() ?? throw new ArgumentException($"Service '{nameof(IErc20Provider)}' is required.");
        tlyContext = serviceProvider.GetService<ITlyContext>() ?? throw new ArgumentException($"Service '{nameof(ITlyContext)}' is required.");

        FullData = poolsInfo;
        PoolInfo = poolsInfo[0];
        ChainInfo = chainInfo;

        Erc20Token = erc20Provider.GetErc20Token(chainInfo, PoolInfo.Token);

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
        var url = new UrlifyProvider(this).BuildUrl();
        var description = $"ChainId: {ChainInfo.ChainId}, PoolId: {PoolId}, ProviderName: {Name}, VaultId: {VaultId}";
        return tlyContext.GetShortUrlAsync(url, description)
            .GetAwaiter()
            .GetResult()
            .ShortUrl;
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

    public static BasePoolInfo[] FetchPoolInfo(BigInteger poolId, ChainInfo chainInfo)
    {
        return FetchPoolInfo(poolId, new LockDealNFTService(new Web3(chainInfo.RpcUrl), chainInfo.LockDealNFT));
    }

    public static BasePoolInfo[] FetchPoolInfo(BigInteger poolId, LockDealNFTService lockDealNFTService)
    {
        return lockDealNFTService.GetFullDataQueryAsync(poolId)
            .GetAwaiter()
            .GetResult()
            .PoolInfo
            .ToArray();
    }
}