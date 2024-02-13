using Nethereum.Web3;
using Net.Web3.EthereumWallet;
using Nethereum.Contracts.Standards.ERC20;

namespace MetaDataAPI.RPC;

public class ERC20Service
{
    private readonly ERC20ContractService contractService;

    public ERC20Service(string rpcUrl, EthereumAddress contractAddress)
        : this(new Web3(rpcUrl), contractAddress)
    { }

    public ERC20Service(IWeb3 web3, EthereumAddress contractAddress)
    {
        contractService = web3.Eth.ERC20.GetContractService(contractAddress);
    }

    public ERC20Service(ERC20ContractService contractService)
    {
        this.contractService = contractService;
    }

    public virtual byte GetDecimals()
    {
        return contractService.DecimalsQueryAsync()
            .GetAwaiter()
            .GetResult();
    }

    public virtual string GetName()
    {
        return contractService.NameQueryAsync()
            .GetAwaiter()
            .GetResult();
    }

    public virtual string GetSymbol()
    {
        return contractService.SymbolQueryAsync()
            .GetAwaiter()
            .GetResult();
    }
}