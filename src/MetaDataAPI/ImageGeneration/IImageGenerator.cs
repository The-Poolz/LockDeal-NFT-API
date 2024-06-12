using MetaDataAPI.ImageGeneration.UrlifyModels;

namespace MetaDataAPI.ImageGeneration;

public interface IImageGenerator
{
    public string Generate(BaseUrlifyModel urlify, string description);
}