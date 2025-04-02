using Pinata.Client;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using MetaDataAPI.Providers;

namespace MetaDataAPI.Services.Image.Models;

public class ImageWithMetadata : PinataMetadata
{
    internal string MetadataName { get; }
    internal string Description { get; }
    internal string Attributes { get; }

    public ImageWithMetadata(AbstractProvider provider)
    {
        MetadataName = provider.MetadataName;
        Description = provider.GetDescription();
        Attributes = JArray.FromObject(provider.GetAttributes()).ToString(Formatting.None);

        KeyValues = new Dictionary<string, string>
        {
            { nameof(MetadataName), MetadataName },
            { nameof(Description), Description },
            { nameof(Attributes), Attributes }
        };
    }
}