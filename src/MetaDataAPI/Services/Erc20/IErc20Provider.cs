using Net.Web3.EthereumWallet;

namespace MetaDataAPI.Services.Erc20;

public interface IErc20Provider
{
    public Erc20Token GetErc20Token(ChainsInfo.ChainInfo chainInfo, EthereumAddress address);
}