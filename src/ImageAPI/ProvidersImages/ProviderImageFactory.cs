using ImageAPI.Utils;
using MetaDataAPI.Providers;
using MetaDataAPI.Models.Response;
using ImageAPI.ProvidersImages.Simple;

namespace ImageAPI.ProvidersImages;

public class ProviderImageFactory
{
    public ProviderImage Create(ImageProcessor imageProcessor, Erc721Attribute[] attributes)
    {
        var providerName = Array.Find(attributes, x => x.TraitType == "ProviderName") ??
            throw new InvalidOperationException("Attributes don't contains attribute with name 'ProviderName'.");

        return Providers(imageProcessor, attributes)[providerName.Value.ToString()!]();
    }

    private static Dictionary<string, Func<ProviderImage>> Providers(ImageProcessor imageProcessor, IEnumerable<Erc721Attribute> attributes) => new()
    {
        { nameof(DealProvider), () => new DealProviderImage(imageProcessor, attributes) }
    };
}