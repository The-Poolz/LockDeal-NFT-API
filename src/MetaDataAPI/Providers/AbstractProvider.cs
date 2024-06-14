using System.Numerics;
using MetaDataAPI.Providers.Attributes.Models;
using MetaDataAPI.Providers.PoolInformation;
using MetaDataAPI.Providers.AttributesProviders;
using MetaDataAPI.Providers.Description;
using MetaDataAPI.Providers.Image;

namespace MetaDataAPI.Providers;

public abstract class AbstractProvider
{
    private readonly IDescriptionProvider descriptionProvider;
    private readonly IImageProvider imageProvider;
    private readonly IAttributesProvider attributesProvider;

    protected abstract PoolInfo PoolInfo { get; }

    protected AbstractProvider(
        IImageProvider imageProvider,
        IAttributesProvider attributesProvider,
        IDescriptionProvider descriptionProvider
    )
    {
        this.imageProvider = imageProvider;
        this.attributesProvider = attributesProvider;
        this.descriptionProvider = descriptionProvider;
    }

    protected AbstractProvider()
    {
        descriptionProvider = new DescriptionProvider();
        imageProvider = new ImageProvider();
    }

    public Erc721Metadata Metadata(BigInteger poolId)
    {
        return new Erc721Metadata(
            name: $"Lock Deal NFT Pool: {poolId}",
            description: descriptionProvider.Description(PoolInfo),
            image: imageProvider.ImageUrl(PoolInfo)
            //attributes: attributesProvider.Attributes()
        );
    }
}