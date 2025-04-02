using MetaDataAPI.Services.ChainsInfo;
using poolz.finance.csharp.contracts.LockDealNFT.ContractDefinition;

namespace MetaDataAPI.Providers;

public class DispenserProvider(BasePoolInfo[] poolsInfo, ChainInfo chainInfo, IServiceProvider serviceProvider)
    : AbstractProvider(poolsInfo, chainInfo, serviceProvider)
{
    protected override string DescriptionTemplate => "DESCRIPTION_HERE, {{LeftAmount}}";
}