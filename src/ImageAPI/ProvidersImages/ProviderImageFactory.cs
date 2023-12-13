using SixLabors.ImageSharp;
using MetaDataAPI.Providers;
using MetaDataAPI.Models.DynamoDb;
using SixLabors.ImageSharp.Processing;
using ImageAPI.ProvidersImages.Simple;
using ImageAPI.ProvidersImages.Advanced;

namespace ImageAPI.ProvidersImages;

public static class ProviderImageFactory
{
    public static ProviderImage Create(Image backgroundImage, DynamoDbItem[] attributes)
    {
        var providerName = attributes[0].ProviderName;

        return Providers(backgroundImage, attributes)[providerName]();
    }

    private static Dictionary<string, Func<ProviderImage>> Providers(Image backgroundImage, DynamoDbItem[] dynamoDbItems) => new()
    {
        { nameof(DealProvider), () => new DealProviderImage(backgroundImage.Clone(_ => {}), dynamoDbItems) },
        { nameof(LockDealProvider), () => new LockDealProviderImage(backgroundImage.Clone(_ => {}), dynamoDbItems) },
        { nameof(TimedDealProvider), () => new TimedDealProviderImage(backgroundImage.Clone(_ => {}), dynamoDbItems) },
        { nameof(CollateralProvider), () => new CollateralProviderImage(backgroundImage.Clone(_ => {}), dynamoDbItems.ToList()) },
        { nameof(RefundProvider), () => new RefundProviderImage(backgroundImage.Clone(_ => {}), dynamoDbItems.ToList()) }
    };
}