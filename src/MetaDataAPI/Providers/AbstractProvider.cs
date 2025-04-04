﻿using Nethereum.Web3;
using System.Numerics;
using HandlebarsDotNet;
using System.Reflection;
using MetaDataAPI.Services.Erc20;
using MetaDataAPI.Services.Image;
using MetaDataAPI.Services.ChainsInfo;
using MetaDataAPI.Providers.Attributes;
using Microsoft.Extensions.DependencyInjection;
using poolz.finance.csharp.contracts.LockDealNFT;
using poolz.finance.csharp.contracts.LockDealNFT.ContractDefinition;

namespace MetaDataAPI.Providers;

public abstract class AbstractProvider
{
    protected readonly ILockDealNFTService LockDealNft;
    protected readonly IErc20Provider Erc20Provider;

    private string? _cachedDescription;
    private IEnumerable<Erc721MetadataItem>? _cachedAttributes;

    public IEnumerable<BasePoolInfo> FullData { get; }
    public BasePoolInfo PoolInfo { get; }
    public ChainInfo ChainInfo { get; }
    public Erc20Token Erc20Token { get; }

    public BigInteger PoolId { get; }

    [Erc721MetadataItem("provider name", DisplayType.String)]
    public string Name { get; }

    [Erc721MetadataItem("collection", DisplayType.Number)]
    public virtual BigInteger VaultId { get; }

    [Erc721MetadataItem("left amount", DisplayType.Number)]
    public decimal LeftAmount { get; }

    protected AbstractProvider(BasePoolInfo[] poolsInfo, ChainInfo chainInfo)
        : this(poolsInfo, chainInfo, DefaultServiceProvider.Instance)
    { }

    protected AbstractProvider(BasePoolInfo[] poolsInfo, ChainInfo chainInfo, IServiceProvider serviceProvider)
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
    }

    protected abstract string DescriptionTemplate { get; }

    public string MetadataName => $"Lock Deal NFT Pool: {PoolId}";

    public string GetDescription()
    {
        var isFullyWithdrawn = ChainInfo.LockDealNFT == PoolInfo.Owner;
        if (isFullyWithdrawn) return "This NFT has been fully withdrawn and is no longer governing any assets.";

        var isFullyRefunded = ChainInfo.LockDealNFT != PoolInfo.Owner && LeftAmount == 0;
        if (isFullyRefunded) return "This NFT has been fully refunded and no longer holds any governance over assets.";

        return _cachedDescription ??= Handlebars.Compile(DescriptionTemplate)(this);
    }

    private string GetImage()
    {
        return new ImageService().GetImageAsync(this).GetAwaiter().GetResult();
    }

    public IEnumerable<Erc721MetadataItem> GetAttributes()
    {
        return _cachedAttributes ??= GetType().GetProperties()
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
            name: MetadataName,
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