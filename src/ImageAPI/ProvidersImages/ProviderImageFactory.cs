using MetaDataAPI.Providers;
using MetaDataAPI.Models.DynamoDb;
using ImageAPI.ProvidersImages.Simple;
using ImageAPI.ProvidersImages.Advanced;
using ImageAPI.ProvidersImages.DelayVault;

namespace ImageAPI.ProvidersImages;

public static class ProviderImageFactory
{
    public static ProviderImage Create(DynamoDbItem[] attributes)
    {
        var providerName = attributes[0].ProviderName;

        return Providers(attributes)[providerName]();
    }

    private static Dictionary<string, Func<ProviderImage>> Providers(IReadOnlyList<DynamoDbItem> dynamoDbItems) => new()
    {
        { nameof(DealProvider), () => new DealProviderImage(dynamoDbItems) },
        { nameof(LockDealProvider), () => new LockDealProviderImage(dynamoDbItems) },
        { nameof(TimedDealProvider), () => new TimedDealProviderImage(dynamoDbItems) },
        { nameof(CollateralProvider), () => new CollateralProviderImage(dynamoDbItems) },
        { nameof(RefundProvider), () => new RefundProviderImage(dynamoDbItems) },
        { nameof(DelayVaultProvider), () => new DelayVaultProviderImage(dynamoDbItems) },
    };
}