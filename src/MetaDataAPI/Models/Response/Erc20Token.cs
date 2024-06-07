using EnvironmentManager.Extensions;
using MetaDataAPI.Storage;
using Nethereum.Contracts.Services;
using Nethereum.RPC.TransactionManagers;
using Nethereum.Web3;

namespace MetaDataAPI.Models.Response;

public class Erc20Token
{
    private static readonly Dictionary<string, Erc20Token> Tokens = new();

    public string Name { get; internal set; }
    public string Symbol { get; internal set; }
    public string Address { get; internal set; }
    public byte Decimals { get; internal set; }

    public Erc20Token(string address, Nethereum.Contracts.Standards.ERC20.ERC20ContractService? rpcCaller = null)
    {
        var web3 = new Web3(Environments.RPC_URL.GetEnvironmentValue());
        rpcCaller ??= new(new EthApiContractService(web3.Client, new TransactionManager(web3.Client)), address);
        Address = address;
        if (Tokens.TryGetValue(address, out var token))
        {
            Name = token.Name;
            Symbol = token.Symbol;
            Decimals = token.Decimals;
        }
        else
        {
            Decimals = rpcCaller.DecimalsQueryAsync().GetAwaiter().GetResult();
            Name = rpcCaller.NameQueryAsync().GetAwaiter().GetResult(); 
            Symbol = rpcCaller.SymbolQueryAsync().GetAwaiter().GetResult(); 
            Tokens.Add(address, this);
        }
    }

    public override string ToString() => $"{Name} ({Symbol}@{Address[..5]}...{Address[^5..]})";
}
