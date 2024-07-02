namespace MetaDataAPI.Providers.Image;

public class QueryStringToken
{
    public string TokenName { get; }
    public string Header { get; }
    public object Value { get; }

    public QueryStringToken(string tokenName, string header, object value)
    {
        TokenName = tokenName;
        Header = header;
        Value = value;
    }

    public override string ToString() => $"${TokenName}|{Header}|{Value}";
}