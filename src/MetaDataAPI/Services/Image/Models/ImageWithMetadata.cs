using Pinata.Client;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using MetaDataAPI.Providers.Attributes;

namespace MetaDataAPI.Services.Image.Models;

public class ImageWithMetadata : PinataMetadata
{
    internal string MetadataName { get; }
    internal string Description { get; }
    internal string ImageIpfs { get; }
    internal string Attributes { get; }

    public ImageWithMetadata(Erc721Metadata erc712Metadata)
    {
        MetadataName = erc712Metadata.Name;
        Description = erc712Metadata.Description;
        ImageIpfs = erc712Metadata.Image;
        Attributes = JArray.FromObject(erc712Metadata.Attributes).ToString(Formatting.None);

        KeyValues = new Dictionary<string, string>
        {
            { nameof(MetadataName), MetadataName },
            { nameof(Description), Description },
            { nameof(ImageIpfs), ImageIpfs },
            { nameof(Attributes), Attributes }
        };
    }
}