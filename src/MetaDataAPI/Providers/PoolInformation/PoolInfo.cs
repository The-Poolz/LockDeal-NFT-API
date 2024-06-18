using Nethereum.Web3;
using System.Numerics;
using MetaDataAPI.Erc20Manager;
using MetaDataAPI.Providers.Attributes;
using MetaDataAPI.Providers.Image.Models;
using MetaDataAPI.Providers.Attributes.Models;
using poolz.finance.csharp.contracts.LockDealNFT.ContractDefinition;

// ReSharper disable VirtualMemberCallInConstructor

namespace MetaDataAPI.Providers.PoolInformation;

public abstract class PoolInfo : BasePoolInfo
{
    public Erc20Token Erc20Token { get; }

    [Erc721Attribute("provider name", DisplayType.String)]
    public override string Name { get; set; }

    [Erc721Attribute("collection", DisplayType.Number)]
    public override BigInteger VaultId { get; set; }

    // TODO: Need to create converter for a BigDecimal type, cause in json it print like an object
    //[JsonConverter(typeof(BigDecimalConverter))]
    [Erc721Attribute("left amount", DisplayType.Number)]
    public decimal LeftAmount { get; set; }

    public UrlifyModelCreation UrlifyModelCreation => new(Name, UrlifyProperties);
    public abstract IEnumerable<PropertyInfo> UrlifyProperties { get; }

    public abstract string DescriptionTemplate { get; }
    public virtual void OnDescriptionCreating() {  }

    protected PoolInfo(BasePoolInfo[] poolsInfo, Erc20Token erc20)
        : this(poolsInfo[0], erc20)
    { }

    protected PoolInfo(BasePoolInfo poolInfo, Erc20Token erc20)
    {
        Provider = poolInfo.Provider;
        Name = poolInfo.Name;
        PoolId = poolInfo.PoolId;
        VaultId = poolInfo.VaultId;
        Owner = poolInfo.Owner;
        Token = poolInfo.Token;
        Params = poolInfo.Params;

        Erc20Token = erc20;
        LeftAmount = Web3.Convert.FromWei(Params[0], erc20.Decimals);
    }
}