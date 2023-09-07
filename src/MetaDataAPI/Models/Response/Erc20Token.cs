using MetaDataAPI.Utils;

namespace MetaDataAPI.Models.Response;

public class Erc20Token
{
    private readonly static Dictionary<string, Erc20Token> Tokens = new();
    private readonly IRpcCaller _rpcCaller;
    public Erc20Token(string address, IRpcCaller? rpcCaller = null)
    {
        _rpcCaller = rpcCaller ?? new RpcCaller();
        Address = address;
        if (Tokens.ContainsKey(address))
        {
            var token = Tokens[address];
            Name = token.Name;
            Symbol = token.Symbol;
            Decimals = token.Decimals;
        }
        else
        {
            Decimals = _rpcCaller.GetDecimals(address);
            Name = _rpcCaller.GetName(address); 
            Symbol = _rpcCaller.GetSymbol(address); 
            Tokens.Add(address, this);
        }
    }
    public string Name { get; internal set; }
    public string Symbol { get; internal set; }
    public string Address { get; internal set; }
    public byte Decimals { get; internal set; }
    public override string ToString() => $"{Name} ({Symbol}@{Address[..5]}...{Address[^5..]})";
}
