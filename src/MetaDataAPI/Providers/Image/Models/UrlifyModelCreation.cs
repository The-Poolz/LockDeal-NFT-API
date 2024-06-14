namespace MetaDataAPI.Providers.Image.Models;

public class UrlifyModelCreation
{
    public string ClassName { get; set; }
    public IEnumerable<PropertyInfo> Properties { get; set; }

    public UrlifyModelCreation(string providerName, IEnumerable<PropertyInfo> properties)
    {
        ClassName = providerName + "UrlifyModel";
        Properties = properties;
    }
}