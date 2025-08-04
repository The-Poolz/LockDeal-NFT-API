using Nethereum.Util;
using System.Globalization;
using MetaDataAPI.Services.ChainsInfo;
using poolz.finance.csharp.contracts.LockDealNFT.ContractDefinition;

namespace MetaDataAPI.Providers;

public class InvestProvider : AbstractProvider
{
    public string MaxInvestAmount { get; }

    public InvestProvider(BasePoolInfo[] poolsInfo, ChainInfo chainInfo, IServiceProvider serviceProvider)
        : base(poolsInfo, chainInfo, serviceProvider)
    {
        MaxInvestAmount = UnitConversion.Convert.FromWei(poolsInfo[0].Params[0], Erc20Token.Decimals).ToString(CultureInfo.InvariantCulture);
    }

    protected override string DescriptionTemplate => "This NFT enables the holder to invest up to {{MaxInvestAmount}} tokens of {{Erc20Token.Symbol}}.";
}