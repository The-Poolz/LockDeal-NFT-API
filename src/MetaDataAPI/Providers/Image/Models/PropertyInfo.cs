namespace MetaDataAPI.Providers.Image.Models;

public class PropertyInfo
{
    public string Name { get; set; }
    public string Value { get; set; }
    public int Order { get; set; }

    public PropertyInfo(string name, string value, int order = 0)
    {
        Name = name;
        Value = value;
        Order = order;
    }
}