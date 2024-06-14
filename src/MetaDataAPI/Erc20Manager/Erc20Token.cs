using Net.Cache.DynamoDb.ERC20.Models;
using Net.Web3.EthereumWallet.Extensions;

namespace MetaDataAPI.Erc20Manager;

public class Erc20Token
{
    public string Name { get; set; }
    public string Symbol { get; set; }
    public string Address { get; set; }
    public byte Decimals { get; set; }

    public Erc20Token(ERC20DynamoDbTable cacheErc20)
    {
        Name = cacheErc20.Name;
        Symbol = cacheErc20.Symbol;
        Address = cacheErc20.Address;
        Decimals = cacheErc20.Decimals;
    }

    public override string ToString() => $"{Name} ({Symbol}@{Address.ToShortAddress(5)})";
}