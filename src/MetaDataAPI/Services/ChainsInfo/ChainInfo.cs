using Net.Web3.EthereumWallet;

namespace MetaDataAPI.Services.ChainsInfo;

public class ChainInfo(long chainId, EthereumAddress lockDealNft)
{
    public long ChainId { get; set; } = chainId;
    public EthereumAddress LockDealNFT { get; set; } = lockDealNft;
}