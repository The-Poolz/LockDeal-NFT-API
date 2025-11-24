using MetaDataAPI.Providers;

namespace MetaDataAPI.Services.Image;

public interface IImageService
{
    public Task<string> GetImageAsync(AbstractProvider provider);
}