using MetaDataAPI.Providers.Image.Models;
using MetaDataAPI.Providers.Image.Services;
using MetaDataAPI.Providers.PoolInformation;

namespace MetaDataAPI.Providers.Image;

public class ImageProvider : IImageProvider
{
    private readonly DynamicTypeBuilder typeBuilder;

    public ImageProvider()
    {
        typeBuilder = new DynamicTypeBuilder();
    }

    public ImageProvider(DynamicTypeBuilder typeBuilder)
    {
        this.typeBuilder = typeBuilder;
    }

    public string ImageUrl(PoolInfo poolInfo)
    {
        var urlify = typeBuilder.CreateUrlifyModel(poolInfo);
        return new NftUrlifyModel(urlify).BuildUrl();
    }
}