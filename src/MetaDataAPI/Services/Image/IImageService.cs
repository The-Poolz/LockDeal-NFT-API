using MetaDataAPI.Providers;

namespace MetaDataAPI.Services.Image;

public interface IImageService
{
    public string UploadImage(AbstractProvider provider);
}