using MetaDataAPI.Utils;
using MetaDataAPI.Models.Types;
using MetaDataAPI.Models.Response;
using MetaDataAPI.Models;
using System.Numerics;
using MetaDataAPI.Models.DynamoDb;
using MetaDataAPI.RPC.Models.PoolInfo;

namespace MetaDataAPI.Providers;

public class RefundProvider : Provider
{
    [Display(DisplayType.Number)]
    public decimal MainCoinAmount => new ConvertWei(CollateralProvider.MainCoin.Decimals).WeiToEth(PoolInfo.Params[1]);

    [Display(DisplayType.Number)]
    public BigInteger MainCoinCollection => CollateralProvider.MainCoinCollection;

    [Display(DisplayType.String)]
    public string SubProviderName => SubProvider.ProviderName;

    public override string ProviderName => nameof(RefundProvider);
    public override string Description =>
        $"This NFT encompasses {LeftAmount} units of the asset {PoolInfo.Token} " +
        $"with an associated refund rate of {CollateralProvider.Rate}. Post rate calculation, the refundable " +
        $"amount in the primary asset {CollateralProvider.MainCoin} will be {MainCoinAmount}.";

    public Provider SubProvider { get; }
    public CollateralProvider CollateralProvider { get; }

    public override List<DynamoDbItem> DynamoDbAttributes
    {
        get
        {
            var dynamoDbAttributes = new List<DynamoDbItem>
            {
                new(ProviderName, PoolInfo, new List<Erc721Attribute>
                {
                    new("MainCoinAmount", MainCoinAmount),
                    new("MainCoinCollection", MainCoinCollection)
                }),
                new(SubProvider.ProviderName, SubProvider.PrimaryProviderInfo, SubProvider.Attributes.Where(attr => attr.TraitType != "ProviderName").ToList())
            };

            return dynamoDbAttributes;
        }
    }

    public RefundPoolInfo PoolInfo { get; }

    public RefundProvider(List<BasePoolInfo> basePoolInfo, string rpcUrl)
        : base(basePoolInfo)
    {
        PoolInfo = new RefundPoolInfo(basePoolInfo[0], rpcUrl);
        var providerFactory = new ProviderFactory();
        SubProvider = providerFactory.Create(PoolInfo.PoolId + 1);
        CollateralProvider = providerFactory.Create<CollateralProvider>(PoolInfo.Params[2]);
    }
}