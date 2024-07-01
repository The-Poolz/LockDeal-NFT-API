using Net.Cache.DynamoDb.ERC20.Models;
using Net.Web3.EthereumWallet.Extensions;

namespace MetaDataAPI.Services.Erc20;

public class Erc20Token
{
    public string Name { get; }
    public string Symbol { get; }
    public string Address { get; }
    public byte Decimals { get; }

    public Erc20Token(string name, string symbol, string address, byte decimals)
    {
        Name = name;
        Symbol = symbol;
        Address = address;
        Decimals = decimals;
    }

    public Erc20Token(ERC20DynamoDbTable cacheErc20)
    {
        Name = cacheErc20.Name;
        Symbol = cacheErc20.Symbol;
        Address = cacheErc20.Address;
        Decimals = cacheErc20.Decimals;
    }

    public override string ToString() => $"{Name} ({Symbol}@{Address.ToShortAddress(5)})";
}