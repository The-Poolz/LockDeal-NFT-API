using System.Numerics;
using System.Reflection;
using MetaDataAPI.Utils;
using MetaDataAPI.Models;
using Newtonsoft.Json.Linq;
using MetaDataAPI.Models.Types;
using MetaDataAPI.Models.Response;
using EnvironmentManager.Extensions;
using MetaDataAPI.ImageGeneration;
using MetaDataAPI.ImageGeneration.UrlifyModels;
using poolz.finance.csharp.contracts.LockDealNFT.ContractDefinition;

namespace MetaDataAPI.Providers;

public abstract class Provider
{
    private readonly IImageGenerator imageGenerator;

    [Display(DisplayType.String)]
    public abstract string ProviderName { get; }

    [Display(DisplayType.Number)]
    public virtual decimal LeftAmount { get; }

    [Display(DisplayType.Number)]
    public virtual BigInteger Collection => PoolInfo.VaultId;

    public virtual Erc20Token Token => new (PoolInfo.Token);

    public BasePoolInfo PoolInfo { get; }

    public abstract string Description { get; }

    public abstract BaseUrlifyModel Urlify { get; }

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

    protected Provider(BasePoolInfo[] basePoolInfo)
    {
        PoolInfo = basePoolInfo.FirstOrDefault()!;
        var converter = new ConvertWei(Token.Decimals);
        LeftAmount = converter.WeiToEth(PoolInfo.Params[0]);
        imageGenerator = new ImageGenerator();
    }

    public string GetJsonErc721Metadata() => JToken.FromObject(GetErc721Metadata()).ToString();
    public virtual Erc721Metadata GetErc721Metadata()
    {
        var name = "Lock Deal NFT Pool: " + PoolInfo.PoolId;
        var image = imageGenerator.Generate(Urlify, Description);
        return new Erc721Metadata(name, GetDescription(), image, Attributes.ToList());
    }

    private string GetDescription()
    {
        string lockDealNftAddress = Environments.LOCK_DEAL_NFT_ADDRESS.Get();

        if (IsFullyWithdrawn(lockDealNftAddress)) return "This NFT has been fully withdrawn and is no longer governing any assets.";
        if (IsFullyRefunded(lockDealNftAddress)) return "This NFT has been fully refunded and no longer holds any governance over assets.";

        return Description;
    }

    private bool IsFullyWithdrawn(string nftAddress) => nftAddress == PoolInfo.Owner;
    private bool IsFullyRefunded(string nftAddress) => nftAddress != PoolInfo.Owner && LeftAmount == 0;
}
