using Net.Web3.EthereumWallet;
using MetaDataAPI.Services.ChainsInfo;

namespace MetaDataAPI.Services.Erc20;

public interface IErc20Provider
{
    public Erc20Token GetErc20Token(ChainInfo chainInfo, EthereumAddress address);
}