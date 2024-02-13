using Net.Web3.EthereumWallet;

namespace MetaDataAPI.RPC.Models;

public class ERC20Token
{
    public EthereumAddress Address { get; }
    public string Name { get; }
    public string Symbol { get; }
    public byte Decimals { get; }

    public ERC20Token(EthereumAddress address, ERC20Service erc20Service)
    {
        Address = address;
        Name = erc20Service.GetName();
        Symbol = erc20Service.GetSymbol();
        Decimals = erc20Service.GetDecimals();
    }

    public ERC20Token(EthereumAddress address, string name, string symbol, byte decimals)
    {
        Address = address;
        Name = name;
        Symbol = symbol;
        Decimals = decimals;
    }

    public override string ToString() => $"{Name} ({Symbol}@{Address.ToShortAddress(5)})";
}