using System.Numerics;
using MetaDataAPI.Models.Response;
using MetaDataAPI.RPC.Models.PoolInfo;

namespace MetaDataAPI.Models.DynamoDb;

public class DynamoDbItem
{
    public BigInteger PoolId { get; set; }
    public string ProviderName { get; set; }
    public string TokenSymbol { get; set; }
    public List<Erc721Attribute> Attributes { get; set; }

    public DynamoDbItem()
    {
        ProviderName = string.Empty;
        TokenSymbol = string.Empty;
        Attributes = new List<Erc721Attribute>();
    }

    public DynamoDbItem(string providerName, BasePoolInfo poolInfo, List<Erc721Attribute> attributes)
    {
        ProviderName = providerName;
        TokenSymbol = poolInfo.Token.Symbol;
        PoolId = poolInfo.PoolId;
        Attributes = attributes;
    }

    public DynamoDbItem(string providerName, string tokenSymbol, BigInteger poolId, List<Erc721Attribute> attributes)
    {
        ProviderName = providerName;
        TokenSymbol = tokenSymbol;
        PoolId = poolId;
        Attributes = attributes;
    }
}