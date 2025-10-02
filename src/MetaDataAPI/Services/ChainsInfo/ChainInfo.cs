using Net.Web3.EthereumWallet;

namespace MetaDataAPI.Services.ChainsInfo;

public class ChainInfo(long chainId, string rpcUrl, EthereumAddress lockDealNft)
{
    public long ChainId { get; set; } = chainId;
    public string RpcUrl { get; set; } = rpcUrl;
    public EthereumAddress LockDealNFT { get; set; } = lockDealNft;
}