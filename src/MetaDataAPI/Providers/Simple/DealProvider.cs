using MetaDataAPI.ImageGeneration.UrlifyModels;
using MetaDataAPI.ImageGeneration.UrlifyModels.Simple;
using poolz.finance.csharp.contracts.LockDealNFT.ContractDefinition;

namespace MetaDataAPI.Providers;

public class DealProvider : Provider
{
    public override string ToString() => $"immediate access to {LeftAmount}"; 
    public override string ProviderName => nameof(DealProvider);
    public override string Description =>
        $"This NFT represents immediate access to {LeftAmount} units of the specified asset {PoolInfo.Token}.";

    public override BaseUrlifyModel Urlify => new DealUrlifyModel(PoolInfo);

    public DealProvider(BasePoolInfo[] basePoolInfos) 
        : base(basePoolInfos)
    { }

    public DealProvider(BasePoolInfo basePoolInfo)
        : base(new [] { basePoolInfo })
    { }
}