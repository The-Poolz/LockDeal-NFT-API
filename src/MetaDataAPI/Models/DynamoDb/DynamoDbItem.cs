using MetaDataAPI.Models.Response;

namespace MetaDataAPI.Models.DynamoDb;

public class DynamoDbItem
{
    public string ProviderName { get; set; }
    public List<Erc721Attribute> Attributes { get; set; }

    public DynamoDbItem(string providerName, List<Erc721Attribute> attributes)
    {
        ProviderName = providerName;
        Attributes = attributes;
    }
}