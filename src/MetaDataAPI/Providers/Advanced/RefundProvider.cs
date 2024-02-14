using MetaDataAPI.Utils;
using MetaDataAPI.Models.Types;
using MetaDataAPI.Models.Response;
using MetaDataAPI.Models;
using System.Numerics;
using MetaDataAPI.Models.DynamoDb;

namespace MetaDataAPI.Providers;

public class RefundProvider : Provider
{
    public override string ProviderName => nameof(RefundProvider);
    public override string Description =>
        $"This NFT encompasses {LeftAmount} units of the asset {PoolInfo.Token} " +
        $"with an associated refund rate of {Rate}. Post rate calculation, the refundable " +
        $"amount in the primary asset {MainCoin} will be {MainCoinAmount}.";

    public Provider SubProvider { get; }
    public Erc20Token MainCoin { get; }

    [Display(DisplayType.Number)]
    public decimal Rate { get; }

    [Display(DisplayType.Number)]
    public decimal MainCoinAmount { get; }

    [Display(DisplayType.Number)]
    public BigInteger MainCoinCollection { get; }

    [Display(DisplayType.String)]
    public string SubProviderName { get; }

    public override List<DynamoDbItem> DynamoDbAttributes
    {
        get
        {
            var dynamoDbAttributes = new List<DynamoDbItem>
            {
                new(ProviderName, PoolInfo, new List<Erc721Attribute>
                {
                    new("Rate", Rate),
                    new("MainCoinAmount", MainCoinAmount),
                    new("MainCoinCollection", MainCoinCollection)
                }),
                new(SubProvider.ProviderName, SubProvider.PoolInfo, SubProvider.Attributes.Where(attr => attr.TraitType != "ProviderName").ToList())
            };

            return dynamoDbAttributes;
        }
    }

    public RefundProvider(List<BasePoolInfo> basePoolInfo)
        : base(basePoolInfo[0])
    {
        var collateral = basePoolInfo[2];
        MainCoin = collateral.Token;
        MainCoinCollection = collateral.VaultId;
        SubProvider = ProviderFactory.Create(new List<BasePoolInfo> { basePoolInfo[1] });
        SubProviderName = SubProvider.ProviderName;
        Rate = new ConvertWei(21).WeiToEth(collateral.Params[2]);
        MainCoinAmount = SubProvider.LeftAmount * Rate;
    }
}