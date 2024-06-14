using MetaDataAPI.Erc20Manager;
using MetaDataAPI.Providers.PoolInformation;
using poolz.finance.csharp.contracts.LockDealNFT.ContractDefinition;

namespace MetaDataAPI.Providers;

public class DealProvider : AbstractProvider
{
    public DealProvider(BasePoolInfo poolInfo, Erc20Token erc20Token)
    {
        PoolInfo = new DealPoolInfo(poolInfo, erc20Token);
    }

    protected override DealPoolInfo PoolInfo { get; }
}