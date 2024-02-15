using MetaDataAPI.Models.DynamoDb;
using MetaDataAPI.Models.Response;
using poolz.finance.csharp.LockDealNFT.ContractDefinition;

namespace MetaDataAPI.Providers;

public class DealProvider : Provider
{
    public override string ToString() => $"immediate access to {LeftAmount}"; 
    public override string ProviderName => nameof(DealProvider);
    public override string Description =>
        $"This NFT represents immediate access to {LeftAmount} units of the specified asset {PoolInfo.Token}.";
    public override List<DynamoDbItem> DynamoDbAttributes => new()
    {
        new DynamoDbItem(ProviderName, PoolInfo, new List<Erc721Attribute>
        {
            new("Collection", Collection),
            new("LeftAmount", LeftAmount)
        })
    };
    public DealProvider(BasePoolInfo[] basePoolInfos) 
        : base(basePoolInfos) { }
    public DealProvider(BasePoolInfo basePoolInfo)
        : base([basePoolInfo]) { }
}