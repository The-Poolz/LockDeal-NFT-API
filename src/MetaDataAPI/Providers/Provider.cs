using MetaDataAPI.Providers.Attributes;
using MetaDataAPI.Providers.Image;
using MetaDataAPI.Providers.Description;
using MetaDataAPI.Providers.PoolInformation;
using MetaDataAPI.Providers.Attributes.Models;

namespace MetaDataAPI.Providers;

public class Provider
{
    private readonly IDescriptionProvider descriptionProvider;
    private readonly IImageProvider imageProvider;
    private readonly IAttributesProvider attributesProvider;

    protected Provider(
        IImageProvider imageProvider,
        IAttributesProvider attributesProvider,
        IDescriptionProvider descriptionProvider
    )
    {
        this.imageProvider = imageProvider;
        this.attributesProvider = attributesProvider;
        this.descriptionProvider = descriptionProvider;
    }

    protected Provider()
    {
        descriptionProvider = new DescriptionProvider();
        imageProvider = new ImageProvider();
    }

    public Erc721Metadata Metadata(PoolInfo poolInfo)
    {
        return new Erc721Metadata(
            name: $"Lock Deal NFT Pool: {poolInfo.PoolId}",
            description: descriptionProvider.Description(poolInfo),
            image: imageProvider.ImageUrl(poolInfo)
            //attributes: attributesProvider.Attributes()
        );
    }
}