using MetaDataAPI.Utils;
using Newtonsoft.Json.Linq;
using MetaDataAPI.Models.Response;
using MetaDataAPI.Models.Types;
using MetaDataAPI.Models;
using System.Numerics;
using System.Reflection;
using MetaDataAPI.Models.DynamoDb;
using MetaDataAPI.Storage;

namespace MetaDataAPI.Providers;

public abstract class Provider
{
    public const string FullyWithdrawnDescription = "This NFT has been fully withdrawn and is no longer governing any assets.";
    public const string FullyRefundedDescription = "This NFT has been fully refunded and no longer holds any governance over assets.";
    public bool IsFullyWithdrawn => Environments.LockDealNftAddress == PoolInfo.Owner;
    public bool IsFullyRefunded => Environments.LockDealNftAddress != PoolInfo.Owner && LeftAmount == 0;

    [Display(DisplayType.String)]
    public abstract string ProviderName { get; }
    public virtual Erc20Token Token => PoolInfo.Token;
    public abstract string Description { get; }
    public abstract List<DynamoDbItem> DynamoDbAttributes { get; }
    public virtual IEnumerable<Erc721Attribute> Attributes
    => GetType()
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

    public BasePoolInfo PoolInfo { get; }
    [Display(DisplayType.Number)]
    public virtual BigInteger Collection => PoolInfo.VaultId;
    protected Provider(BasePoolInfo basePoolInfo)
    {
        PoolInfo = basePoolInfo;
        var converter = new ConvertWei(PoolInfo.Token.Decimals);
        LeftAmount = converter.WeiToEth(basePoolInfo.Params[0]);
    }
    [Display(DisplayType.Number)]
    public virtual decimal LeftAmount { get; }
    public string GetJsonErc721Metadata(DynamoDb dynamoDb) =>
        JToken.FromObject(GetErc721Metadata(dynamoDb)).ToString();

    private string GetDescription()
    {
        if (IsFullyWithdrawn) return FullyWithdrawnDescription;
        if (IsFullyRefunded) return FullyRefundedDescription;
        return Description;
    }

    private Erc721Metadata GetErc721Metadata(DynamoDb dynamoDb)
    {
        var hash = dynamoDb.PutItem(DynamoDbAttributes);
        var name = "Lock Deal NFT Pool: " + PoolInfo.PoolId;
        var image = @$"https://nft.poolz.finance/{Environments.NameOfStage}/image?hash={hash}";
        return new Erc721Metadata(name, GetDescription(), image, Attributes.ToList());
    }
}
