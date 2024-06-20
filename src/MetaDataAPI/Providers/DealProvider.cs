using MetaDataAPI.BlockchainManager.Models;
using poolz.finance.csharp.contracts.LockDealNFT.ContractDefinition;

namespace MetaDataAPI.Providers;

public class DealProvider : AbstractProvider
{
    public DealProvider(BasePoolInfo poolInfo, ChainInfo chainInfo, IServiceProvider serviceProvider)
        : this(new []{ poolInfo }, chainInfo, serviceProvider)
    { }

    public DealProvider(BasePoolInfo[] poolsInfo, ChainInfo chainInfo, IServiceProvider serviceProvider)
        : base(poolsInfo, chainInfo, serviceProvider)
    { }

    protected override string DescriptionTemplate =>
        "This NFT represents immediate access to {{LeftAmount}} units of the specified asset {{Erc20Token}}.";
}