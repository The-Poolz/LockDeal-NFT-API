using System.Numerics;
using System.Reflection;
using MetaDataAPI.Utils;
using MetaDataAPI.Models;
using Newtonsoft.Json.Linq;
using MetaDataAPI.Models.Types;
using MetaDataAPI.Models.Response;
using EnvironmentManager.Extensions;
using poolz.finance.csharp.contracts.LockDealNFT.ContractDefinition;

namespace MetaDataAPI.Providers;

public abstract class Provider
{
    public const string FullyWithdrawnDescription = "This NFT has been fully withdrawn and is no longer governing any assets.";
    public const string FullyRefundedDescription = "This NFT has been fully refunded and no longer holds any governance over assets.";
    public bool IsFullyWithdrawn => Environments.LOCK_DEAL_NFT_ADDRESS.Get() == PoolInfo.Owner;
    public bool IsFullyRefunded => Environments.LOCK_DEAL_NFT_ADDRESS.Get() != PoolInfo.Owner && LeftAmount == 0;

    [Display(DisplayType.String)]
    public abstract string ProviderName { get; }
    public virtual Erc20Token Token => new (PoolInfo.Token);
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
    protected Provider(BasePoolInfo[] basePoolInfo)
    {
        PoolInfo = basePoolInfo.FirstOrDefault()!;
        var converter = new ConvertWei(Token.Decimals);
        LeftAmount = converter.WeiToEth(PoolInfo.Params[0]);
    }
    [Display(DisplayType.Number)]
    public virtual decimal LeftAmount { get; }

    private string GetDescription()
    {
        if (IsFullyWithdrawn) return FullyWithdrawnDescription;
        if (IsFullyRefunded) return FullyRefundedDescription;
        return Description;
    }

    public string GetJsonErc721Metadata() => JToken.FromObject(GetErc721Metadata()).ToString();
    private Erc721Metadata GetErc721Metadata()
    {
        var name = "Lock Deal NFT Pool: " + PoolInfo.PoolId;
        var image = "SHORT_URL_WILL_BE_HERE";
        return new Erc721Metadata(name, GetDescription(), image, Attributes.ToList());
    }
}
