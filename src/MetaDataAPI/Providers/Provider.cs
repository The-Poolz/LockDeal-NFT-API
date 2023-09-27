using MetaDataAPI.Utils;
using Newtonsoft.Json.Linq;
using MetaDataAPI.Models.Response;
using MetaDataAPI.Models.Types;
using MetaDataAPI.Models;
using System.Numerics;
using System.Reflection;

namespace MetaDataAPI.Providers;

public abstract class Provider
{
    [Display(DisplayType.String)]
    public abstract string ProviderName { get; }
    [Display(DisplayType.String)]
    public virtual Erc20Token Token => PoolInfo.Token;
    public abstract string Description { get; }
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

    private Erc721Metadata GetErc721Metadata(DynamoDb dynamoDb)
    {
        var hash = dynamoDb.PutItem(Attributes);

        var name = "Lock Deal NFT Pool: " + PoolInfo.PoolId;
        var image = @$"https://nft.poolz.finance/test/image?id={hash}";
        return new Erc721Metadata(name, Description, image, Attributes.ToList());
    }
}
