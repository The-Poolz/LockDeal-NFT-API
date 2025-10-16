using Net.Web3.EthereumWallet.Extensions;
using Net.Cache.DynamoDb.ERC20.DynamoDb.Models;

namespace MetaDataAPI.Services.Erc20;

public class Erc20Token(string name, string symbol, string address, byte decimals)
{
    public string Name { get; } = name;
    public string Symbol { get; } = symbol;
    public string Address { get; } = address;
    public byte Decimals { get; } = decimals;

    public Erc20Token(Erc20TokenDynamoDbEntry cacheErc20) : this(
        cacheErc20.Name,
        cacheErc20.Symbol,
        cacheErc20.Address,
        cacheErc20.Decimals
    )
    { }

    public override string ToString() => $"{Name} ({Symbol}@{Address.ToShortAddress(5)})";
}