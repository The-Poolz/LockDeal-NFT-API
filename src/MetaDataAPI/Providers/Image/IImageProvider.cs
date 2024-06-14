using MetaDataAPI.Providers.PoolInformation;

namespace MetaDataAPI.Providers.Image;

public interface IImageProvider
{
    public string ImageUrl(PoolInfo poolInfo);
}