using Nethereum.Web3;
using System.Numerics;
using MetaDataAPI.Erc20Manager;
using MetaDataAPI.Providers.Image;
using MetaDataAPI.Providers.Attributes;
using MetaDataAPI.Providers.Description;
using MetaDataAPI.BlockchainManager.Models;
using MetaDataAPI.Providers.PoolInformation;
using MetaDataAPI.Providers.Attributes.Models;
using poolz.finance.csharp.contracts.LockDealNFT;
using poolz.finance.csharp.contracts.LockDealNFT.ContractDefinition;

namespace MetaDataAPI.Providers;

public class ProviderManager : IProviderManager
{
    private readonly IErc20Provider erc20Provider;
    private readonly IImageProvider imageProvider;
    private readonly IAttributesProvider attributesProvider;
    private readonly IDescriptionProvider descriptionProvider;

    public ProviderManager(
        IErc20Provider erc20Provider,
        IImageProvider imageProvider,
        IAttributesProvider attributesProvider,
        IDescriptionProvider descriptionProvider
    )
    {
        this.erc20Provider = erc20Provider;
        this.imageProvider = imageProvider;
        this.attributesProvider = attributesProvider;
        this.descriptionProvider = descriptionProvider;
    }

    public ProviderManager()
    {
        erc20Provider = new Erc20Provider();
        imageProvider = new ImageProvider();
        attributesProvider = new AttributesProvider();
        descriptionProvider = new DescriptionProvider();
    }

    public Erc721Metadata Metadata(BigInteger poolId, ChainInfo chainInfo)
    {
        var fullPoolInfo = FetchPoolInfo(poolId, chainInfo);
        var poolInfo = fullPoolInfo.FirstOrDefault()!;

        var erc20 = erc20Provider.GetErc20Token(chainInfo.RpcUrl, chainInfo.ChainId, poolInfo.Token);

        var typeName = poolInfo.Name.Replace("Provider", "PoolInfo");
        var type = Type.GetType($"MetaDataAPI.Providers.{typeName}, MetaDataAPI")
            ?? throw new InvalidOperationException($"Cannot found '{typeName}' type. Please check if this PoolInfo implemented.");
        var providerPoolInfo = (PoolInfo)Activator.CreateInstance(type, poolInfo, erc20)!;

        return Metadata(providerPoolInfo);
    }
    
    public Erc721Metadata Metadata(PoolInfo poolInfo)
    {
        return new Erc721Metadata(
            name: $"Lock Deal NFT Pool: {poolInfo.PoolId}",
            description: descriptionProvider.Description(poolInfo),
            image: imageProvider.ImageUrl(poolInfo),
            attributes: attributesProvider.Attributes(poolInfo)
        );
    }

    public IEnumerable<BasePoolInfo> FetchPoolInfo(BigInteger poolId, ChainInfo chainInfo)
    {
        return FetchPoolInfo(poolId, new LockDealNFTService(new Web3(chainInfo.RpcUrl), chainInfo.LockDealNFT));
    }

    public IEnumerable<BasePoolInfo> FetchPoolInfo(BigInteger poolId, LockDealNFTService lockDealNFTService)
    {
        return lockDealNFTService.GetFullDataQueryAsync(poolId)
            .GetAwaiter()
            .GetResult()
            .PoolInfo
            .ToArray();
    }
}