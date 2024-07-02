using System.Numerics;
using Net.Web3.EthereumWallet;

namespace MetaDataAPI.Services.ChainsInfo;

public class ChainInfo
{
    public BigInteger ChainId { get; set; }
    public string RpcUrl { get; set; }
    public EthereumAddress LockDealNFT { get; set; }

    public ChainInfo(BigInteger chainId, string rpcUrl, EthereumAddress lockDealNft)
    {
        ChainId = chainId;
        RpcUrl = rpcUrl;
        LockDealNFT = lockDealNft;
    }
}