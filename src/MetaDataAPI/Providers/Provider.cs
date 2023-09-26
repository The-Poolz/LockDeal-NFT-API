using MetaDataAPI.Utils;
using Newtonsoft.Json.Linq;
using MetaDataAPI.Models.Response;

namespace MetaDataAPI.Providers;

public abstract class Provider
{
    public abstract string ProviderName { get; }
    public abstract string Description { get; }
    public abstract IEnumerable<Erc721Attribute> ProviderAttributes { get; }
    public BasePoolInfo PoolInfo { get; }

    protected Provider(BasePoolInfo basePoolInfo)
    {
        PoolInfo = basePoolInfo;
    }

    public IEnumerable<Erc721Attribute> GetErc721Attributes() =>
        new List<Erc721Attribute>(ProviderAttributes)
        {
            ProviderNameAttribute
        };

    public string GetJsonErc721Metadata(DynamoDb dynamoDb) =>
        JToken.FromObject(GetErc721Metadata(dynamoDb)).ToString();

    private Erc721Metadata GetErc721Metadata(DynamoDb dynamoDb)
    {
        var attributes = GetErc721Attributes().ToArray();
        var hash = dynamoDb.PutItem(attributes);

        var name = "Lock Deal NFT Pool: " + PoolInfo.PoolId;
        var image = @$"https://nft.poolz.finance/test/image?id={hash}";
        return new Erc721Metadata(name, Description, image, attributes);
    }
    private Erc721Attribute ProviderNameAttribute => new("ProviderName", ProviderName);
    protected Erc721Attribute TokenNameAttribute => new("TokenName", PoolInfo.Token.Name);
}
