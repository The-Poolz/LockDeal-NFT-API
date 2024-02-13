using System.Numerics;
using MetaDataAPI.Utils;
using System.Reflection;
using MetaDataAPI.Models;
using MetaDataAPI.Storage;
using Newtonsoft.Json.Linq;
using MetaDataAPI.Models.Types;
using MetaDataAPI.Models.DynamoDb;
using MetaDataAPI.Models.Response;
using MetaDataAPI.Providers.PoolInfo;

namespace MetaDataAPI.Providers;

public abstract class Provider
{
    [Display(DisplayType.String)]
    public abstract string ProviderName { get; }

    [Display(DisplayType.Number)]
    public virtual BigInteger Collection => PrimaryProviderInfo.VaultId;

    [Display(DisplayType.Number)]
    public virtual decimal LeftAmount => new ConvertWei(PrimaryProviderInfo.Token.Decimals).WeiToEth(PrimaryProviderInfo.Params[0]);

    public abstract string Description { get; }
    public abstract List<DynamoDbItem> DynamoDbAttributes { get; }
    public virtual IEnumerable<Erc721Attribute> Attributes => GetType()
        .GetProperties(BindingFlags.Public | BindingFlags.Instance)
       .SelectMany(prop =>
       {
           if (!prop.GetCustomAttributes<DisplayAttribute>(true).Any())
               return Array.Empty<Erc721Attribute>();
           var displayAttribute = prop.GetCustomAttribute<DisplayAttribute>();
           var propertyValue = prop.GetValue(this);
           return displayAttribute!.TryGetErc721Attribute(prop.Name, propertyValue, out var erc721Attribute)
               ? new[] { erc721Attribute }
               : Array.Empty<Erc721Attribute>();
       });

    public bool IsFullyWithdrawn => Environments.LockDealNftAddress == PrimaryProviderInfo.Owner;
    public bool IsFullyRefunded => Environments.LockDealNftAddress != PrimaryProviderInfo.Owner && LeftAmount == 0;
    public BasePoolInfo PrimaryProviderInfo { get; }

    protected Provider(BasePoolInfo basePoolInfo)
    {
        PrimaryProviderInfo = basePoolInfo;
    }

    public string GetJsonErc721Metadata(DynamoDb dynamoDb) =>
        JToken.FromObject(GetErc721Metadata(dynamoDb)).ToString();

    private string GetDescription()
    {
        if (IsFullyWithdrawn) return "This NFT has been fully withdrawn and is no longer governing any assets.";
        if (IsFullyRefunded) return "This NFT has been fully refunded and no longer holds any governance over assets.";
        return Description;
    }

    private Erc721Metadata GetErc721Metadata(DynamoDb dynamoDb)
    {
        var hash = dynamoDb.PutItem(DynamoDbAttributes);
        var name = "Lock Deal NFT Pool: " + PrimaryProviderInfo.PoolId;
        var image = @$"https://nft.poolz.finance/{Environments.NameOfStage}/image?hash={hash}";
        return new Erc721Metadata(name, GetDescription(), image, Attributes.ToList());
    }
}
