using System.Numerics;
using MetaDataAPI.Models.Response;
using poolz.finance.csharp.LockDealNFT.ContractDefinition;

namespace MetaDataAPI.Models.DynamoDb;

public class DynamoDbItem(string providerName, string tokenSymbol, BigInteger poolId, List<Erc721Attribute> attributes)
{
    public BigInteger PoolId { get; set; } = poolId;
    public string ProviderName { get; set; } = providerName;
    public string TokenSymbol { get; set; } = tokenSymbol;
    public List<Erc721Attribute> Attributes { get; set; } = attributes;
    public DynamoDbItem(string providerName, BasePoolInfo poolInfo, List<Erc721Attribute> attributes) :
        this(providerName, new Erc20Token(poolInfo.Token).Symbol, poolInfo.PoolId, attributes) { }
    public DynamoDbItem() : this(string.Empty, string.Empty, new(), []) { } // This for for unit test
}