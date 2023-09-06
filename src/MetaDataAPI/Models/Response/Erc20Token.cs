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
            Name = "Test";//RpcCaller.GetName(address); //Todo: Fix this
            Symbol = "TST"; //"RpcCaller.GetSymbol(address); //Todo: Fix this
            Tokens.Add(address, this);
        }
    }
    public string Name { get; }
    public string Symbol { get; }
    public string Address { get; }
    public byte Decimals { get; }
}
