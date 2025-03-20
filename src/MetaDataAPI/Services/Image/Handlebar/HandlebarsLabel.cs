namespace MetaDataAPI.Services.Image.Handlebar;

public class HandlebarsLabel(string name, object value)
{
    public string Name { get; } = name;
    public object Value { get; } = value;
}