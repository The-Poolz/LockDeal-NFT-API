using Nethereum.Web3;
using Nethereum.Contracts.Services;
using EnvironmentManager.Extensions;
using Nethereum.RPC.TransactionManagers;

namespace MetaDataAPI.Models.Response;

public class Erc20Token
{
    public string Name { get; internal set; }
    public string Symbol { get; internal set; }
    public string Address { get; internal set; }
    public byte Decimals { get; internal set; }

    public Erc20Token(string address, Nethereum.Contracts.Standards.ERC20.ERC20ContractService? rpcCaller = null)
    {
        var web3 = new Web3(Environments.RPC_URL.Get());
        rpcCaller ??= new(new EthApiContractService(web3.Client, new TransactionManager(web3.Client)), address);
        Address = address;
        Decimals = rpcCaller.DecimalsQueryAsync().GetAwaiter().GetResult();
        Name = rpcCaller.NameQueryAsync().GetAwaiter().GetResult();
        Symbol = rpcCaller.SymbolQueryAsync().GetAwaiter().GetResult();
    }

    public override string ToString() => $"{Name} ({Symbol}@{Address[..5]}...{Address[^5..]})";
}
