namespace MetaDataAPI.Providers.Image;

public class HandlebarsToken(string tokenName, object value, string label)
{
    public string TokenName { get; } = tokenName;
    public object Value { get; } = value;
    public string Label { get; } = label;
}