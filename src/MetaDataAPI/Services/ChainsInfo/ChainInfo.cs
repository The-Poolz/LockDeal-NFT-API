using System.Numerics;
using Net.Web3.EthereumWallet;

namespace MetaDataAPI.Services.ChainsInfo;

public class ChainInfo(BigInteger chainId, EthereumAddress lockDealNft)
{
    public BigInteger ChainId { get; set; } = chainId;
    public EthereumAddress LockDealNFT { get; set; } = lockDealNft;
}