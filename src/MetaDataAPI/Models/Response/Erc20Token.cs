using MetaDataAPI.Utils;

namespace MetaDataAPI.Models.Response;

public class Erc20Token
{
    private readonly static Dictionary<string, Erc20Token> Tokens = new();
    public Erc20Token(string address)
    {
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
            Decimals = RpcCaller.GetDecimals(address);
            Name = RpcCaller.GetName(address); 
            Symbol = RpcCaller.GetSymbol(address); 
            Tokens.Add(address, this);
        }
    }
    public string Name { get; internal set; }
    public string Symbol { get; internal set; }
    public string Address { get; internal set; }
    public byte Decimals { get; internal set; }
    public override string ToString() => $"{Name} ({Symbol}@{Address[..5]}...{Address[^5..]})";
}
