namespace MetaDataAPI.Services.Image.Handlebar;

public class HandlebarsToken(string name, string label, object value)
{
    public string Name { get; } = name;
    public string Label { get; } = label;
    public object Value { get; } = value;
}