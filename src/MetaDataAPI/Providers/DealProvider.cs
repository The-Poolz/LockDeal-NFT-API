using MetaDataAPI.Erc20Manager;
using MetaDataAPI.BlockchainManager.Models;
using poolz.finance.csharp.contracts.LockDealNFT.ContractDefinition;

namespace MetaDataAPI.Providers;

public class DealProvider : AbstractProvider
{
    public DealProvider(BasePoolInfo[] poolsInfo, ChainInfo chainInfo)
        : base(poolsInfo, chainInfo)
    { }

    public DealProvider(BasePoolInfo poolInfo, ChainInfo chainInfo, IErc20Provider erc20Provider)
        : this(new []{ poolInfo }, chainInfo, erc20Provider)
    { }

    public DealProvider(BasePoolInfo[] poolsInfo, ChainInfo chainInfo, IErc20Provider erc20Provider)
        : base(poolsInfo, chainInfo, erc20Provider)
    { }

    protected override string DescriptionTemplate =>
        "This NFT represents immediate access to {{LeftAmount}} units of the specified asset {{Erc20Token}}.";
}