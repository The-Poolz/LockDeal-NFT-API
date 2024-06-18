using MetaDataAPI.Erc20Manager;
using MetaDataAPI.Providers.Image.Models;
using poolz.finance.csharp.contracts.LockDealNFT.ContractDefinition;

namespace MetaDataAPI.Providers.PoolInformation;

public class DealPoolInfo : PoolInfo
{
    public DealPoolInfo(BasePoolInfo poolInfo, Erc20Token erc20)
        : base(poolInfo, erc20)
    { }

    public DealPoolInfo(BasePoolInfo[] poolsInfo, Erc20Token erc20)
        : base(poolsInfo, erc20)
    { }

    public override IEnumerable<PropertyInfo> UrlifyProperties => Enumerable.Empty<PropertyInfo>();

    public override string DescriptionTemplate => "This NFT represents immediate access to {{LeftAmount}} units of the specified asset {{Erc20Token}}.";
}