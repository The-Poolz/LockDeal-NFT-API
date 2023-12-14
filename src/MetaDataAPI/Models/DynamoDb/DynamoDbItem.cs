using System.Numerics;
using MetaDataAPI.Models.Response;

namespace MetaDataAPI.Models.DynamoDb;

public class DynamoDbItem
{
    public BigInteger PoolId { get; set; }
    public string ProviderName { get; set; }
    public List<Erc721Attribute> Attributes { get; set; }

    public DynamoDbItem(string providerName, BigInteger poolId, List<Erc721Attribute> attributes)
    {
        ProviderName = providerName;
        PoolId = poolId;
        Attributes = attributes;
    }
}