using MetaDataAPI.Utils;

namespace MetaDataAPI.Models.Response;

public class Erc20Token
{
    const string AddressZero = "0x0000000000000000000000000000";
    private readonly static Dictionary<string, Erc20Token> Tokens = new()
    { { AddressZero, new() } };
    internal Erc20Token() {
        Address = AddressZero;
        Name = string.Empty;
        Decimals = 0;
        Symbol = string.Empty;
    }
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
            Symbol = "TST"; //"RpcCaller.GetSymbol(address); //Todo: Fix this
            Tokens.Add(address, this);
        }
    }
    public string Name { get; internal set; }
    public string Symbol { get; internal set; }
    public string Address { get; internal set; }
    public byte Decimals { get; internal set; }
}
