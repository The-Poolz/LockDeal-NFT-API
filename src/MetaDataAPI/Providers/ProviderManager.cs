using MetaDataAPI.Providers.Image;
using MetaDataAPI.Providers.Attributes;
using MetaDataAPI.Providers.Description;
using MetaDataAPI.Providers.PoolInformation;
using MetaDataAPI.Providers.Attributes.Models;

namespace MetaDataAPI.Providers;

public class ProviderManager : IProviderManager
{
    private readonly IImageProvider imageProvider;
    private readonly IAttributesProvider attributesProvider;
    private readonly IDescriptionProvider descriptionProvider;

    public ProviderManager(
        IImageProvider imageProvider,
        IAttributesProvider attributesProvider,
        IDescriptionProvider descriptionProvider
    )
    {
        this.imageProvider = imageProvider;
        this.attributesProvider = attributesProvider;
        this.descriptionProvider = descriptionProvider;
    }

    public ProviderManager()
    {
        imageProvider = new ImageProvider();
        attributesProvider = new AttributesProvider();
        descriptionProvider = new DescriptionProvider();
    }

    public Erc721Metadata Metadata(PoolInfo poolInfo)
    {
        return new Erc721Metadata(
            name: $"Lock Deal NFT Pool: {poolInfo.PoolId}",
            description: descriptionProvider.Description(poolInfo),
            image: imageProvider.ImageUrl(poolInfo),
            attributes: attributesProvider.Attributes(poolInfo)
        );
    }
}