using SixLabors.Fonts;
using SixLabors.ImageSharp;
using MetaDataAPI.Providers;
using MetaDataAPI.Models.DynamoDb;
using ImageAPI.ProvidersImages.Simple;
using SixLabors.ImageSharp.Processing;
using ImageAPI.ProvidersImages.Advanced;

namespace ImageAPI.ProvidersImages;

public static class ProviderImageFactory
{
    public static ProviderImage Create(Image backgroundImage, Font font, DynamoDbItem[] attributes)
    {
        var providerName = attributes[0].ProviderName;

        return Providers(backgroundImage, font, attributes)[providerName]();
    }

    private static Dictionary<string, Func<ProviderImage>> Providers(Image backgroundImage, Font font, DynamoDbItem[] dynamoDbItems) => new()
    {
        { nameof(DealProvider), () => new DealProviderImage(backgroundImage.Clone(_ => {}), font, dynamoDbItems) },
        { nameof(LockDealProvider), () => new LockDealProviderImage(backgroundImage.Clone(_ => {}), font, dynamoDbItems) },
        { nameof(TimedDealProvider), () => new TimedDealProviderImage(backgroundImage.Clone(_ => {}), font, dynamoDbItems) },
        { nameof(CollateralProvider), () => new CollateralProviderImage(backgroundImage.Clone(_ => {}), font, dynamoDbItems.ToList()) },
        { nameof(RefundProvider), () => new RefundProviderImage(backgroundImage.Clone(_ => {}), font, dynamoDbItems.ToList()) }
    };
}