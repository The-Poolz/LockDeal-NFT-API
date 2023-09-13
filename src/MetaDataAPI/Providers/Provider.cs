using Newtonsoft.Json;
using MetaDataAPI.Utils;
using MetaDataAPI.Models.Response;

namespace MetaDataAPI.Providers;

public abstract class Provider : IProvider
{
    public BasePoolInfo PoolInfo { get; }
    public List<Erc721Attribute> Attributes { get; }

    protected Provider(BasePoolInfo basePoolInfo)
    {
        PoolInfo = basePoolInfo;
        Attributes = new List<Erc721Attribute>();
    }

    public abstract string GetDescription();
    public abstract List<Erc721Attribute> GetParams();
    internal void AddAttributes(string name)
    {
        Attributes.Add(new Erc721Attribute("ProviderName", name));
        Attributes.AddRange(GetParams());
    }

    public Erc721Metadata SaveToCache(DynamoDb dynamoDb)
    {
        var jsonProvider = JsonConvert.SerializeObject(Attributes);
        var hash = DynamoDb.StringToSha256(jsonProvider);

        dynamoDb.PutItemAsync(hash, jsonProvider)
            .GetAwaiter()
            .GetResult();

        return GetErc721Metadata(hash);
    }

    public Erc721Metadata GetErc721Metadata(string hash)
    {
        var name = "Lock Deal NFT Pool: " + PoolInfo.PoolId;
        var image = @$"https://nft.poolz.finance/test/image?id={hash}";
        return new Erc721Metadata(name, GetDescription(), image, Attributes);
    }
}
