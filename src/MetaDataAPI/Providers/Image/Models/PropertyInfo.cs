namespace MetaDataAPI.Providers.Image.Models;

public class PropertyInfo
{
    public string Name { get; set; }
    public Type Type { get; set; }
    public string Value { get; set; }
    public int Order { get; set; }

    public PropertyInfo(string name, Type type, string value, int order = 0)
    {
        Name = name;
        Type = type;
        Value = value;
        Order = order;
    }
}