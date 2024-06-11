using HandlebarsDotNet;
using EnvironmentManager.Extensions;

namespace MetaDataAPI.ImageGeneration.Services;

public class ImageRenderer : IImageRenderer
{
    public string RenderImage(string url)
    {
        return Handlebars.Compile(Environments.HTML_TO_IMAGE_ENDPOINT_TEMPLATE.Get())(new
        {
            Url = url
        });
    }
}