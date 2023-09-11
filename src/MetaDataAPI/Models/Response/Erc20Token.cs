using MetaDataAPI.Utils;

namespace MetaDataAPI.Models.Response;

public class Erc20Token
{
    private static readonly Dictionary<string, Erc20Token> Tokens = new();

    public string Name { get; internal set; }
    public string Symbol { get; internal set; }
    public string Address { get; internal set; }
    public byte Decimals { get; internal set; }

    public Erc20Token(string address, IRpcCaller? rpcCaller = null)
    {
        rpcCaller ??= new RpcCaller();
        Address = address;
        if (Tokens.TryGetValue(address, out var token))
        {
            Name = token.Name;
            Symbol = token.Symbol;
            Decimals = token.Decimals;
        }
        else
        {
            Decimals = rpcCaller.GetDecimals(address);
            Name = rpcCaller.GetName(address); 
            Symbol = rpcCaller.GetSymbol(address); 
            Tokens.Add(address, this);
        }
    }

    public override string ToString() => $"{Name} ({Symbol}@{Address[..5]}...{Address[^5..]})";
}
