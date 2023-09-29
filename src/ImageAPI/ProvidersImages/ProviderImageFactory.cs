using SixLabors.Fonts;
using SixLabors.ImageSharp;
using MetaDataAPI.Providers;
using MetaDataAPI.Models.Response;
using ImageAPI.ProvidersImages.Simple;
using ImageAPI.ProvidersImages.Advanced;

namespace ImageAPI.ProvidersImages;

public static class ProviderImageFactory
{
    public static ProviderImage Create(Image backgroundImage, Font font, Erc721Attribute[] attributes)
    {
        var providerName = Array.Find(attributes, x => x.TraitType == "ProviderName") ??
            throw new InvalidOperationException("Attributes don't contains attribute with name 'ProviderName'.");

        return Providers(backgroundImage, font, attributes)[providerName.Value.ToString()!]();
    }

    private static Dictionary<string, Func<ProviderImage>> Providers(Image backgroundImage, Font font, IEnumerable<Erc721Attribute> attributes) => new()
    {
        { nameof(DealProvider), () => new DealProviderImage(backgroundImage, font, attributes) },
        { nameof(LockDealProvider), () => new LockDealProviderImage(backgroundImage, font, attributes) },
        { nameof(TimedDealProvider), () => new TimedDealProviderImage(backgroundImage, font, attributes) },
        { nameof(BundleProvider), () => new BundleProviderImage(backgroundImage, font, attributes) },
    };
}