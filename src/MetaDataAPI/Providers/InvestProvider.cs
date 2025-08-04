using MetaDataAPI.Services.ChainsInfo;
using poolz.finance.csharp.contracts.LockDealNFT.ContractDefinition;

namespace MetaDataAPI.Providers;

public class InvestProvider(BasePoolInfo[] poolsInfo, ChainInfo chainInfo, IServiceProvider serviceProvider)
    : AbstractProvider(poolsInfo, chainInfo, serviceProvider)
{
    public string MaxInvestAmount { get; } = poolsInfo[0].Params[0].ToString();

    protected override string DescriptionTemplate => "DESCRIPTION HERE";
}