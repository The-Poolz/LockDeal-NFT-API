using MetaDataAPI.ImageGeneration.Services;
using MetaDataAPI.ImageGeneration.UrlifyModels;

namespace MetaDataAPI.ImageGeneration;

public class ImageGenerator : IImageGenerator
{
    private readonly IImageRenderer renderer;
    private readonly IUrlShortener shortener;

    public ImageGenerator()
    {
        renderer = new ImageRenderer();
        shortener = new UrlShortener();
    }

    public ImageGenerator(IImageRenderer renderer, IUrlShortener shortener)
    {
        this.renderer = renderer;
        this.shortener = shortener;
    }

    public string Generate(BaseUrlifyModel urlify, string description)
    {
        var image = renderer.RenderImage(urlify.BuildUrl());
        return shortener.Shorten(image, description);
    }
}